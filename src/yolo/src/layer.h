#ifndef BASE_LAYER_H
#define BASE_LAYER_H

#include "activations.h"
#include "stddef.h"
#include "tree.h"

struct network_state;

struct layer;
typedef struct layer layer;

typedef enum {
    CONVOLUTIONAL,
    DECONVOLUTIONAL,
    CONNECTED,
    MAXPOOL,
    SOFTMAX,
    DETECTION,
    DROPOUT,
    CROP,
    ROUTE,
    COST,
    NORMALIZATION,
    AVGPOOL,
    LOCAL,
    SHORTCUT,
    ACTIVE,
    RNN,
    GRU,
    CRNN,
    BATCHNORM,
    NETWORK,
    XNOR,
    REGION,
	YOLO,
    REORG,
	UPSAMPLE,
	REORG_OLD,
    BLANK
} LAYER_TYPE;

typedef enum{
    SSE, MASKED, SMOOTH
} COST_TYPE;

typedef struct {
	int batch;
	float learning_rate;
	float momentum;
	float decay;
	int adam;
	float B1;
	float B2;
	float eps;
	int t;
} update_args;

struct layer{
    LAYER_TYPE type;
    ACTIVATION activation;
    COST_TYPE cost_type;
    void (*forward)   (struct layer, struct network_state);
    void (*backward)  (struct layer, struct network_state);
    void (*update)    (struct layer, int, float, float, float);
    void (*forward_gpu)   (struct layer, struct network_state);
    void (*backward_gpu)  (struct layer, struct network_state);
    void (*update_gpu)    (struct layer, int, float, float, float);
    int batch_normalize;
    int shortcut;
    int batch;
    int forced;
    int flipped;
    int inputs;
    int outputs;
    int truths;
    int h,w,c;
    int out_h, out_w, out_c;
    int n;
    int max_boxes;
    int groups;
    int size;
    int side;
    int stride;
    int reverse;
    int pad;
    int sqrt;
    int flip;
    int index;
    int binary;
    int xnor;
    int steps;
    int hidden;
    float dot;
    float angle;
    float jitter;
    float saturation;
    float exposure;
    float shift;
    float ratio;
	int focal_loss;
    int softmax;
    int classes;
    int coords;
    int background;
    int rescore;
    int objectness;
    int does_cost;
    int joint;
    int noadjust;
    int reorg;
    int log;
	int tanh;
	int *mask;
	int total;

    int adam;
    float B1;
    float B2;
    float eps;
    float *m_gpu;
    float *v_gpu;
    int t;
    float *m;
    float *v;

    tree *softmax_tree;
    int  *map;

    float alpha;
    float beta;
    float kappa;

    float coord_scale;
    float object_scale;
    float noobject_scale;
	float mask_scale;
    float class_scale;
    int bias_match;
    int random;
	float ignore_thresh;
	float truth_thresh;
    float thresh;
	float focus;
    int classfix;
    int absolute;

    int onlyforward;
    int stopbackward;
    int dontload;
    int dontloadscales;

    float temperature;
    float probability;
    float scale;

    int *indexes;
    float *rand;
    float *cost;
    char  *cweights;
    float *state;
    float *prev_state;
    float *forgot_state;
    float *forgot_delta;
    float *state_delta;

    float *concat;
    float *concat_delta;

    float *binary_weights;

    float *biases;
    float *bias_updates;

    float *scales;
    float *scale_updates;

    float *weights;
    float *weight_updates;

    float *col_image;
    int   * input_layers;
    int   * input_sizes;
    float * delta;
    float * output;
    float * squared;
    float * norms;

    float * spatial_mean;
    float * mean;
    float * variance;

    float * mean_delta;
    float * variance_delta;

    float * rolling_mean;
    float * rolling_variance;

    float * x;
    float * x_norm;

    struct layer *input_layer;
    struct layer *self_layer;
    struct layer *output_layer;

    struct layer *input_gate_layer;
    struct layer *state_gate_layer;
    struct layer *input_save_layer;
    struct layer *state_save_layer;
    struct layer *input_state_layer;
    struct layer *state_state_layer;

    struct layer *input_z_layer;
    struct layer *state_z_layer;

    struct layer *input_r_layer;
    struct layer *state_r_layer;

    struct layer *input_h_layer;
    struct layer *state_h_layer;

    float *z_cpu;
    float *r_cpu;
    float *h_cpu;

    float *binary_input;

    size_t workspace_size;

};

void free_layer(layer);

#endif
