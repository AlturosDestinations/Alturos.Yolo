#include "layer.h"
#include <stdlib.h>

void free_layer(layer l)
{
	if (l.type == DROPOUT) {
		if (l.rand)           free(l.rand);
		return;
	}
	if (l.cweights)           free(l.cweights);
	if (l.indexes)            free(l.indexes);
	if (l.input_layers)       free(l.input_layers);
	if (l.input_sizes)        free(l.input_sizes);
	if (l.map)                free(l.map);
	if (l.rand)               free(l.rand);
	if (l.cost)               free(l.cost);
	if (l.state)              free(l.state);
	if (l.prev_state)         free(l.prev_state);
	if (l.forgot_state)       free(l.forgot_state);
	if (l.forgot_delta)       free(l.forgot_delta);
	if (l.state_delta)        free(l.state_delta);
	if (l.concat)             free(l.concat);
	if (l.concat_delta)       free(l.concat_delta);
	if (l.binary_weights)     free(l.binary_weights);
	if (l.biases)             free(l.biases);
	if (l.bias_updates)       free(l.bias_updates);
	if (l.scales)             free(l.scales);
	if (l.scale_updates)      free(l.scale_updates);
	if (l.weights)            free(l.weights);
	if (l.weight_updates)     free(l.weight_updates);
	if (l.delta)              free(l.delta);
	if (l.output)             free(l.output);
	if (l.squared)            free(l.squared);
	if (l.norms)              free(l.norms);
	if (l.spatial_mean)       free(l.spatial_mean);
	if (l.mean)               free(l.mean);
	if (l.variance)           free(l.variance);
	if (l.mean_delta)         free(l.mean_delta);
	if (l.variance_delta)     free(l.variance_delta);
	if (l.rolling_mean)       free(l.rolling_mean);
	if (l.rolling_variance)   free(l.rolling_variance);
	if (l.x)                  free(l.x);
	if (l.x_norm)             free(l.x_norm);
	if (l.m)                  free(l.m);
	if (l.v)                  free(l.v);
	if (l.z_cpu)              free(l.z_cpu);
	if (l.r_cpu)              free(l.r_cpu);
	if (l.h_cpu)              free(l.h_cpu);
	if (l.binary_input)       free(l.binary_input);
}
