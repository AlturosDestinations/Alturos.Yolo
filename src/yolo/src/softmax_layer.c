#include "blas.h"
#include <float.h>
#include <math.h>
#include <stdlib.h>
#include <stdio.h>
#include <assert.h>
#include "tree.h"

void softmax_tree(float *input, int batch, int inputs, float temp, tree *hierarchy, float *output)
{
    int b;
    for(b = 0; b < batch; ++b){
        int i;
        int count = 0;
        for(i = 0; i < hierarchy->groups; ++i){
            int group_size = hierarchy->group_size[i];
            softmax(input+b*inputs + count, group_size, temp, output+b*inputs + count, 1);
            count += group_size;
        }
    }
}
