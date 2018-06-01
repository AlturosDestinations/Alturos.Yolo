#include "network.h"

network make_network(int n)
{
    network net = {0};
    net.n = n;
    net.layers = calloc(net.n, sizeof(layer));
    net.seen = calloc(1, sizeof(int));
    return net;
}

void forward_network(network net, network_state state)
{
    state.workspace = net.workspace;
    int i;
    for(i = 0; i < net.n; ++i){
        state.index = i;
        layer l = net.layers[i];
        if(l.delta){
            scal_cpu(l.outputs * l.batch, 0, l.delta, 1);
        }
        l.forward(l, state);
        state.input = l.output;
    }
}

float *get_network_output(network net)
{
    int i;
    for(i = net.n-1; i > 0; --i) if(net.layers[i].type != COST) break;
    return net.layers[i].output;
}

void set_batch_network(network *net, int b)
{
    net->batch = b;
    int i;
    for(i = 0; i < net->n; ++i){
        net->layers[i].batch = b;
    }
}

int get_network_output_size(network net)
{
    int i;
    for(i = net.n-1; i > 0; --i) if(net.layers[i].type != COST) break;
    return net.layers[i].outputs;
}

float *network_predict(network net, float *input)
{
    network_state state;
    state.net = net;
    state.index = 0;
    state.input = input;
    state.truth = 0;
    state.train = 0;
    state.delta = 0;
    forward_network(net, state);
    float *out = get_network_output(net);
    return out;
}

int num_detections(network *net, float thresh)
{
    int i;
    int s = 0;
    for (i = 0; i < net->n; ++i) {
        layer l = net->layers[i];
        if (l.type == YOLO) {
            s += yolo_num_detections(l, thresh);
        }
        if (l.type == DETECTION || l.type == REGION) {
            s += l.w*l.h*l.n;
        }
    }
    return s;
}

detection *make_network_boxes(network *net, float thresh, int *num)
{
    layer l = net->layers[net->n - 1];
    int i;
    int nboxes = num_detections(net, thresh);
    if (num) *num = nboxes;
    detection *dets = calloc(nboxes, sizeof(detection));
    for (i = 0; i < nboxes; ++i) {
        dets[i].prob = calloc(l.classes, sizeof(float));
        if (l.coords > 4) {
            dets[i].mask = calloc(l.coords - 4, sizeof(float));
        }
    }
    return dets;
}

void custom_get_region_detections(layer l, int w, int h, int net_w, int net_h, float thresh, int *map, int relative, detection *dets, int letter)
{
    box *boxes = calloc(l.w*l.h*l.n, sizeof(box));
    float **probs = calloc(l.w*l.h*l.n, sizeof(float *));
    int i, j;
    for (j = 0; j < l.w*l.h*l.n; ++j) probs[j] = calloc(l.classes, sizeof(float *));
    get_region_boxes(l, 1, 1, thresh, probs, boxes, 0, map);
    for (j = 0; j < l.w*l.h*l.n; ++j) {
        dets[j].classes = l.classes;
        dets[j].bbox = boxes[j];
        dets[j].objectness = 1;
        for (i = 0; i < l.classes; ++i) {
            dets[j].prob[i] = probs[j][i];
        }
    }

    free(boxes);
    free_ptrs((void **)probs, l.w*l.h*l.n);

    correct_yolo_boxes(dets, l.w*l.h*l.n, w, h, net_w, net_h, relative, letter);
}

void fill_network_boxes(network *net, int w, int h, float thresh, int *map, int relative, detection *dets, int letter)
{
    int j;
    for (j = 0; j < net->n; ++j) {
        layer l = net->layers[j];
    
        if (l.type == REGION) {
            custom_get_region_detections(l, w, h, net->w, net->h, thresh, map, relative, dets, letter);            
            dets += l.w*l.h*l.n;
        }
    }
}

detection *get_network_boxes(network *net, int w, int h, float thresh, int *map, int relative, int *num, int letter)
{
    detection *dets = make_network_boxes(net, thresh, num);
    fill_network_boxes(net, w, h, thresh, map, relative, dets, letter);
    return dets;
}

void free_detections(detection *dets, int n)
{
    int i;
    for (i = 0; i < n; ++i) {
        free(dets[i].prob);
        if (dets[i].mask) free(dets[i].mask);
    }
    free(dets);
}

float *network_predict_image(network *net, image im)
{
    image imr = letterbox_image(im, net->w, net->h);
    set_batch_network(net, 1);
    float *p = network_predict(*net, imr.data);
    free_image(imr);
    return p;
}

void free_network(network net)
{
    int i;
    for (i = 0; i < net.n; ++i) {
        free_layer(net.layers[i]);
    }
    free(net.layers);

    free(net.workspace);
}

void fuse_conv_batchnorm(network net)
{
    int j;
    for (j = 0; j < net.n; ++j) {
        layer *l = &net.layers[j];

        if (l->type == CONVOLUTIONAL) 
        {
            //printf(" Merges Convolutional-%d and batch_norm \n", j);

            if (l->batch_normalize) {
                int f;
                for (f = 0; f < l->n; ++f)
                {
                    l->biases[f] = l->biases[f] - l->scales[f] * l->rolling_mean[f] / (sqrtf(l->rolling_variance[f]) + .000001f);

                    const size_t filter_size = l->size*l->size*l->c;
                    int i;
                    for (i = 0; i < filter_size; ++i) {
                        int w_index = f*filter_size + i;

                        l->weights[w_index] = l->weights[w_index] * l->scales[f] / (sqrtf(l->rolling_variance[f]) + .000001f);
                    }
                }

                l->batch_normalize = 0;
            }
        }
    }
}
