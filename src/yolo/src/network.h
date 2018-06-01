#ifndef NETWORK_H
#define NETWORK_H

#include <stdint.h>
#include "layer.h"
#include "image.h"

typedef enum {
    CONSTANT, STEP, EXP, POLY, STEPS, SIG, RANDOM
} learning_rate_policy;

typedef struct network{
    float *workspace;
    int n;
    int batch;
    int *seen;
    float epoch;
    int subdivisions;
    float momentum;
    float decay;
    layer *layers;
    int outputs;
    float *output;
    learning_rate_policy policy;

    float learning_rate;
    float gamma;
    float scale;
    float power;
    int time_steps;
    int step;
    int max_batches;
    float *scales;
    int   *steps;
    int num_steps;
    int burn_in;

    int adam;
    float B1;
    float B2;
    float eps;

    int inputs;
    int h, w, c;
    int max_crop;
    int min_crop;
    float angle;
    float aspect;
    float exposure;
    float saturation;
    float hue;
    int small_object;

//    int gpu_index;
    tree *hierarchy;

} network;

typedef struct network_state {
    float *truth;
    float *input;
    float *delta;
    float *workspace;
    int train;
    int index;
    network net;
} network_state;

void free_network(network net);

network make_network(int n);
void forward_network(network net, network_state state);
YOLODLL_API float *network_predict(network net, float *input);
float *get_network_output(network net);
int get_network_output_size(network net);
void set_batch_network(network *net, int b);
YOLODLL_API detection *get_network_boxes(network *net, int w, int h, float thresh, int *map, int relative, int *num, int letter);
YOLODLL_API detection *make_network_boxes(network *net, float thresh, int *num);
YOLODLL_API void free_detections(detection *dets, int n);
YOLODLL_API float *network_predict_image(network *net, image im);
void fuse_conv_batchnorm(network net);

#endif
