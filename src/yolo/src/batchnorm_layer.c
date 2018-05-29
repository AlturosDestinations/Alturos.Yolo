#include "batchnorm_layer.h"

void forward_batchnorm_layer(layer l, network_state state)
{
    normalize_cpu(l.output, l.rolling_mean, l.rolling_variance, l.batch, l.out_c, l.out_h*l.out_w);
   
    scale_bias(l.output, l.scales, l.batch, l.out_c, l.out_h*l.out_w);
}
