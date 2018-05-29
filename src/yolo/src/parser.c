#include <stdio.h>
#include <string.h>
#include <stdlib.h>

#include "activations.h"
#include "assert.h"
#include "batchnorm_layer.h"
#include "blas.h"
#include "convolutional_layer.h"
#include "list.h"
#include "maxpool_layer.h"
#include "option_list.h"
#include "parser.h"
#include "region_layer.h"
#include "utils.h"
#include <stdint.h>

typedef struct{
    char *type;
    list *options;
}section;

list *read_cfg(char *filename);

LAYER_TYPE string_to_layer_type(char * type)
{
    if (strcmp(type, "[region]")==0) return REGION;
    if (strcmp(type, "[conv]")==0
            || strcmp(type, "[convolutional]")==0) return CONVOLUTIONAL;
    if (strcmp(type, "[max]")==0
            || strcmp(type, "[maxpool]")==0) return MAXPOOL;
    return BLANK;
}

void free_section(section *s)
{
    free(s->type);
    node *n = s->options->front;
    while(n){
        kvp *pair = (kvp *)n->val;
        free(pair->key);
        free(pair);
        node *next = n->next;
        free(n);
        n = next;
    }
    free(s->options);
    free(s);
}

typedef struct size_params{
    int batch;
    int inputs;
    int h;
    int w;
    int c;
    int index;
    int time_steps;
    network net;
} size_params;

convolutional_layer parse_convolutional(list *options, size_params params)
{
//	printf("Conv 1\n");
    int n = option_find_int(options, "filters",1);
    int size = option_find_int(options, "size",1);
    int stride = option_find_int(options, "stride",1);
    int pad = option_find_int_quiet(options, "pad",0);
    int padding = option_find_int_quiet(options, "padding",0);
    if(pad) padding = size/2;

//	printf("Conv 2\n");
    char *activation_s = option_find_str(options, "activation", "logistic");
	
//	printf("Conv 3\n");
	//printf("%s\n", activation_s[0]);

    ACTIVATION activation = get_activation(activation_s);
//	printf("Conv 4\n");

    int batch,h,w,c;
    h = params.h;
    w = params.w;
    c = params.c;
    batch=params.batch;
//	printf("Conv 5\n");
    if(!(h && w && c)) error("Layer before convolutional layer must output image.");
    int batch_normalize = option_find_int_quiet(options, "batch_normalize", 0);
    int binary = option_find_int_quiet(options, "binary", 0);
    int xnor = option_find_int_quiet(options, "xnor", 0);
//	printf("Conv 6\n");
    convolutional_layer layer = make_convolutional_layer(batch,h,w,c,n,size,stride,padding,activation, batch_normalize, binary, xnor, params.net.adam);
    layer.flipped = option_find_int_quiet(options, "flipped", 0);
    layer.dot = option_find_float_quiet(options, "dot", 0);
    if(params.net.adam){
        layer.B1 = params.net.B1;
        layer.B2 = params.net.B2;
        layer.eps = params.net.eps;
    }
//	printf("Conv 5\n");
    return layer;
}

layer parse_region(list *options, size_params params)
{
    int coords = option_find_int(options, "coords", 4);
    int classes = option_find_int(options, "classes", 20);
    int num = option_find_int(options, "num", 1);
	int max_boxes = option_find_int_quiet(options, "max", 30);

    layer l = make_region_layer(params.batch, params.w, params.h, num, classes, coords, max_boxes);
    assert(l.outputs == params.inputs);

    l.log = option_find_int_quiet(options, "log", 0);
    l.sqrt = option_find_int_quiet(options, "sqrt", 0);

    l.softmax = option_find_int(options, "softmax", 0);
	l.focal_loss = option_find_int_quiet(options, "focal_loss", 0);
    //l.max_boxes = option_find_int_quiet(options, "max",30);
    l.jitter = option_find_float(options, "jitter", .2);
    l.rescore = option_find_int_quiet(options, "rescore",0);

    l.thresh = option_find_float(options, "thresh", .5);
    l.classfix = option_find_int_quiet(options, "classfix", 0);
    l.absolute = option_find_int_quiet(options, "absolute", 0);
    l.random = option_find_int_quiet(options, "random", 0);

    l.coord_scale = option_find_float(options, "coord_scale", 1);
    l.object_scale = option_find_float(options, "object_scale", 1);
    l.noobject_scale = option_find_float(options, "noobject_scale", 1);
	l.mask_scale = option_find_float(options, "mask_scale", 1);
    l.class_scale = option_find_float(options, "class_scale", 1);
    l.bias_match = option_find_int_quiet(options, "bias_match",0);

    char *tree_file = option_find_str(options, "tree", 0);
    if (tree_file) l.softmax_tree = read_tree(tree_file);
    char *map_file = option_find_str(options, "map", 0);
    if (map_file) l.map = read_map(map_file);

    char *a = option_find_str(options, "anchors", 0);
    if(a){
        int len = strlen(a);
        int n = 1;
        int i;
        for(i = 0; i < len; ++i){
            if (a[i] == ',') ++n;
        }
        for(i = 0; i < n; ++i){
            float bias = atof(a);
            l.biases[i] = bias;
            a = strchr(a, ',')+1;
        }
    }
    return l;
}

maxpool_layer parse_maxpool(list *options, size_params params)
{
    int stride = option_find_int(options, "stride",1);
    int size = option_find_int(options, "size",stride);
    int padding = option_find_int_quiet(options, "padding", (size-1)/2);

    int batch,h,w,c;
    h = params.h;
    w = params.w;
    c = params.c;
    batch=params.batch;
    if(!(h && w && c)) error("Layer before maxpool layer must output image.");

    maxpool_layer layer = make_maxpool_layer(batch,h,w,c,size,stride,padding);
    return layer;
}

learning_rate_policy get_policy(char *s)
{
    if (strcmp(s, "random")==0) return RANDOM;
    if (strcmp(s, "poly")==0) return POLY;
    if (strcmp(s, "constant")==0) return CONSTANT;
    if (strcmp(s, "step")==0) return STEP;
    if (strcmp(s, "exp")==0) return EXP;
    if (strcmp(s, "sigmoid")==0) return SIG;
    if (strcmp(s, "steps")==0) return STEPS;
    fprintf(stderr, "Couldn't find policy %s, going with constant\n", s);
    return CONSTANT;
}

void parse_net_options(list *options, network *net)
{
//	printf("option 1\n");
    net->batch = option_find_int(options, "batch",1);
    net->learning_rate = option_find_float(options, "learning_rate", .001);
    net->momentum = option_find_float(options, "momentum", .9);
    net->decay = option_find_float(options, "decay", .0001);
    int subdivs = option_find_int(options, "subdivisions",1);
    net->time_steps = option_find_int_quiet(options, "time_steps",1);
    net->batch /= subdivs;
    net->batch *= net->time_steps;
    net->subdivisions = subdivs;

//	printf("option 2\n");
    net->adam = option_find_int_quiet(options, "adam", 0);
    if(net->adam){
        net->B1 = option_find_float(options, "B1", .9);
        net->B2 = option_find_float(options, "B2", .999);
        net->eps = option_find_float(options, "eps", .000001);
    }

    net->h = option_find_int_quiet(options, "height",0);
    net->w = option_find_int_quiet(options, "width",0);
    net->c = option_find_int_quiet(options, "channels",0);
    net->inputs = option_find_int_quiet(options, "inputs", net->h * net->w * net->c);
    net->max_crop = option_find_int_quiet(options, "max_crop",net->w*2);
    net->min_crop = option_find_int_quiet(options, "min_crop",net->w);

//	printf("option 3\n");
	net->small_object = option_find_int_quiet(options, "small_object", 0);
    net->angle = option_find_float_quiet(options, "angle", 0);
    net->aspect = option_find_float_quiet(options, "aspect", 1);
    net->saturation = option_find_float_quiet(options, "saturation", 1);
    net->exposure = option_find_float_quiet(options, "exposure", 1);
    net->hue = option_find_float_quiet(options, "hue", 0);
	net->power = option_find_float_quiet(options, "power", 4);
//	printf("option 4\n");
    if(!net->inputs && !(net->h && net->w && net->c)) error("No input parameters supplied");

    char *policy_s = option_find_str(options, "policy", "constant");
//	printf("option 5\n");
	net->policy = get_policy(policy_s);
//	printf("option 6\n");
	net->burn_in = option_find_int_quiet(options, "burn_in", 0);
//	printf("option 7\n");

    if(net->policy == STEP){
        net->step = option_find_int(options, "step", 1);
        net->scale = option_find_float(options, "scale", 1);
    } else if (net->policy == STEPS){
        char *l = option_find(options, "steps");   
        char *p = option_find(options, "scales");   
        if(!l || !p) error("STEPS policy must have steps and scales in cfg file");
	
        int len = strlen(l);
        int n = 1;
        int i;
        for(i = 0; i < len; ++i){
            if (l[i] == ',') ++n;
        }
        int *steps = calloc(n, sizeof(int));
        float *scales = calloc(n, sizeof(float));
        for(i = 0; i < n; ++i){
            int step    = atoi(l);
            float scale = atof(p);
            l = strchr(l, ',')+1;
            p = strchr(p, ',')+1;
            steps[i] = step;
            scales[i] = scale;
        }
        net->scales = scales;
        net->steps = steps;
        net->num_steps = n;
    } else if (net->policy == EXP){
        net->gamma = option_find_float(options, "gamma", 1);
    } else if (net->policy == SIG){
        net->gamma = option_find_float(options, "gamma", 1);
        net->step = option_find_int(options, "step", 1);
    } else if (net->policy == POLY || net->policy == RANDOM){
        //net->power = option_find_float(options, "power", 1);
    }
    net->max_batches = option_find_int(options, "max_batches", 0);
//	printf("option 8\n");
}

int is_network(section *s)
{
    return (strcmp(s->type, "[net]")==0
            || strcmp(s->type, "[network]")==0);
}

extern network parse_network_cfg_custom(char *filename, int batch)
{
//	printf("parser 1\n");
    list *sections = read_cfg(filename);
    node *n = sections->front;
    if(!n) error("Config file has no sections");
    network net = make_network(sections->size - 1);
//    net.gpu_index = gpu_index;
    size_params params;

//	printf("parser 2\n");
    section *s = (section *)n->val;
    list *options = s->options;
    if(!is_network(s)) error("First section must be [net] or [network]");
    parse_net_options(options, &net);

//	printf("parser 3\n");
    params.h = net.h;
    params.w = net.w;
    params.c = net.c;
    params.inputs = net.inputs;
	if (batch > 0) net.batch = batch;
    params.batch = net.batch;
    params.time_steps = net.time_steps;
    params.net = net;

//	printf("parser 4\n");
    size_t workspace_size = 0;
    n = n->next;
    int count = 0;
    free_section(s);
    fprintf(stderr, "layer     filters    size              input                output\n");
    
//	printf("parser 5\n");
	while(n){
        params.index = count;
        fprintf(stderr, "%5d ", count);
        s = (section *)n->val;
        options = s->options;
        layer l = {0};
        LAYER_TYPE lt = string_to_layer_type(s->type);
		
        if(lt == CONVOLUTIONAL)
		{
			//printf("parser 6\n");
            l = parse_convolutional(options, params);
        }
		else if(lt == REGION)
		{
            l = parse_region(options, params);
		}
		else if(lt == MAXPOOL)
		{
            l = parse_maxpool(options, params);
        }		
		else{
            fprintf(stderr, "Type not recognized: %s\n", s->type);
        }
	//	printf("parser 7\n");
        l.onlyforward = option_find_int_quiet(options, "onlyforward", 0);
        l.stopbackward = option_find_int_quiet(options, "stopbackward", 0);
        l.dontload = option_find_int_quiet(options, "dontload", 0);
        l.dontloadscales = option_find_int_quiet(options, "dontloadscales", 0);
        option_unused(options);
        net.layers[count] = l;
        if (l.workspace_size > workspace_size) workspace_size = l.workspace_size;
        free_section(s);
        n = n->next;
        ++count;
        if(n){
            params.h = l.out_h;
            params.w = l.out_w;
            params.c = l.out_c;
            params.inputs = l.outputs;
        }
    }   
    free_list(sections);
    net.outputs = get_network_output_size(net);
    net.output = get_network_output(net);
    if(workspace_size){
        //printf("%ld\n", workspace_size);
        net.workspace = calloc(1, workspace_size);
    }
//	printf("parser 8\n");
    return net;
}

list *read_cfg(char *filename)
{
    FILE *file = fopen(filename, "r");
    if(file == 0) file_error(filename);
    char *line;
    int nu = 0;
    list *sections = make_list();
    section *current = 0;
    while((line=fgetl(file)) != 0){
        ++ nu;
        strip(line);
        switch(line[0]){
            case '[':
                current = malloc(sizeof(section));
                list_insert(sections, current);
                current->options = make_list();
                current->type = line;
                break;
            case '\0':
            case '#':
            case ';':
                free(line);
                break;
            default:
                if(!read_option(line, current->options)){
                    fprintf(stderr, "Config file error line %d, could parse: %s\n", nu, line);
                    free(line);
                }
                break;
        }
    }
    fclose(file);
    return sections;
}

void transpose_matrix(float *a, int rows, int cols)
{
    float *transpose = calloc(rows*cols, sizeof(float));
    int x, y;
    for(x = 0; x < rows; ++x){
        for(y = 0; y < cols; ++y){
            transpose[y*rows + x] = a[x*cols + y];
        }
    }
    memcpy(a, transpose, rows*cols*sizeof(float));
    free(transpose);
}

void load_connected_weights(layer l, FILE *fp, int transpose)
{
    fread(l.biases, sizeof(float), l.outputs, fp);
    fread(l.weights, sizeof(float), l.outputs*l.inputs, fp);
    if(transpose){
        transpose_matrix(l.weights, l.inputs, l.outputs);
    }
    if (l.batch_normalize && (!l.dontloadscales)){
        fread(l.scales, sizeof(float), l.outputs, fp);
        fread(l.rolling_mean, sizeof(float), l.outputs, fp);
        fread(l.rolling_variance, sizeof(float), l.outputs, fp);      
    }
}

void load_batchnorm_weights(layer l, FILE *fp)
{
    fread(l.scales, sizeof(float), l.c, fp);
    fread(l.rolling_mean, sizeof(float), l.c, fp);
    fread(l.rolling_variance, sizeof(float), l.c, fp);
}

void load_convolutional_weights_binary(layer l, FILE *fp)
{
    fread(l.biases, sizeof(float), l.n, fp);
    if (l.batch_normalize && (!l.dontloadscales)){
        fread(l.scales, sizeof(float), l.n, fp);
        fread(l.rolling_mean, sizeof(float), l.n, fp);
        fread(l.rolling_variance, sizeof(float), l.n, fp);
    }
    int size = l.c*l.size*l.size;
    int i, j, k;
    for(i = 0; i < l.n; ++i){
        float mean = 0;
        fread(&mean, sizeof(float), 1, fp);
        for(j = 0; j < size/8; ++j){
            int index = i*size + j*8;
            unsigned char c = 0;
            fread(&c, sizeof(char), 1, fp);
            for(k = 0; k < 8; ++k){
                if (j*8 + k >= size) break;
                l.weights[index + k] = (c & 1<<k) ? mean : -mean;
            }
        }
    }
}

void load_convolutional_weights(layer l, FILE *fp)
{
    int num = l.n*l.c*l.size*l.size;
    fread(l.biases, sizeof(float), l.n, fp);
    if (l.batch_normalize && (!l.dontloadscales)){
        fread(l.scales, sizeof(float), l.n, fp);
        fread(l.rolling_mean, sizeof(float), l.n, fp);
        fread(l.rolling_variance, sizeof(float), l.n, fp);
        if(0){
            int i;
            for(i = 0; i < l.n; ++i){
                printf("%g, ", l.rolling_mean[i]);
            }
            printf("\n");
            for(i = 0; i < l.n; ++i){
                printf("%g, ", l.rolling_variance[i]);
            }
            printf("\n");
        }
        if(0){
            fill_cpu(l.n, 0, l.rolling_mean, 1);
            fill_cpu(l.n, 0, l.rolling_variance, 1);
        }
    }
    fread(l.weights, sizeof(float), num, fp);
    if(l.adam){
        fread(l.m, sizeof(float), num, fp);
        fread(l.v, sizeof(float), num, fp);
    }    
    if (l.flipped) {
        transpose_matrix(l.weights, l.c*l.size*l.size, l.n);
    }   
}

void load_weights_upto(network *net, char *filename, int cutoff)
{
    fprintf(stderr, "Loading weights from %s...", filename);
    fflush(stdout);
    FILE *fp = fopen(filename, "rb");
    if(!fp) file_error(filename);

    int major;
    int minor;
    int revision;
    fread(&major, sizeof(int), 1, fp);
    fread(&minor, sizeof(int), 1, fp);
    fread(&revision, sizeof(int), 1, fp);
	if ((major * 10 + minor) >= 2) {
		printf("\n seen 64 \n");
		uint64_t iseen = 0;
		fread(&iseen, sizeof(uint64_t), 1, fp);
		*net->seen = iseen;
	}
	else {
		printf("\n seen 32 \n");
		fread(net->seen, sizeof(int), 1, fp);
	}
    int transpose = (major > 1000) || (minor > 1000);

    int i;
    for(i = 0; i < net->n && i < cutoff; ++i){
        layer l = net->layers[i];
        if (l.dontload) continue;
        if(l.type == CONVOLUTIONAL){
            load_convolutional_weights(l, fp);
        }     
    }
//    fprintf(stderr, "Done!\n");
    fclose(fp);
}

extern void load_weights(network *net, char *filename)
{
    load_weights_upto(net, filename, net->n);
}
