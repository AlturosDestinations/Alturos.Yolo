#ifndef MAXPOOL_LAYER_H
#define MAXPOOL_LAYER_H

#include "layer.h"
#include "network.h"

typedef layer maxpool_layer;

maxpool_layer make_maxpool_layer(int batch, int h, int w, int c, int size, int stride, int padding);
void forward_maxpool_layer(const maxpool_layer l, network_state state);

#endif
