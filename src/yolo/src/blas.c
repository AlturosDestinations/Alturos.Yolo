#include "blas.h"

#include <math.h>
#include <assert.h>
#include <float.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void reorg_cpu(float *x, int out_w, int out_h, int out_c, int batch, int stride, int forward, float *out)
{
	int b, i, j, k;
	int in_c = out_c / (stride*stride);

	//printf("\n out_c = %d, out_w = %d, out_h = %d, stride = %d, forward = %d \n", out_c, out_w, out_h, stride, forward);
	//printf("  in_c = %d,  in_w = %d,  in_h = %d \n", in_c, out_w*stride, out_h*stride);

	for (b = 0; b < batch; ++b) {
		for (k = 0; k < out_c; ++k) {
			for (j = 0; j < out_h; ++j) {
				for (i = 0; i < out_w; ++i) {
					int in_index = i + out_w*(j + out_h*(k + out_c*b));
					int c2 = k % in_c;
					int offset = k / in_c;
					int w2 = i*stride + offset % stride;
					int h2 = j*stride + offset / stride;
					int out_index = w2 + out_w*stride*(h2 + out_h*stride*(c2 + in_c*b));
					if (forward) out[out_index] = x[in_index];	// used by default for forward (i.e. forward = 0)
					else out[in_index] = x[out_index];
				}
			}
		}
	}
}

void flatten(float *x, int size, int layers, int batch, int forward)
{
    float *swap = calloc(size*layers*batch, sizeof(float));
    int i,c,b;
    for(b = 0; b < batch; ++b){
        for(c = 0; c < layers; ++c){
            for(i = 0; i < size; ++i){
                int i1 = b*layers*size + c*size + i;
                int i2 = b*layers*size + i*layers + c;
                if (forward) swap[i2] = x[i1];
                else swap[i1] = x[i2];
            }
        }
    }
    memcpy(x, swap, size*layers*batch*sizeof(float));
    free(swap);
}

void normalize_cpu(float *x, float *mean, float *variance, int batch, int filters, int spatial)
{
    int b, f, i;
    for(b = 0; b < batch; ++b){
        for(f = 0; f < filters; ++f){
            for(i = 0; i < spatial; ++i){
                int index = b*filters*spatial + f*spatial + i;
                x[index] = (x[index] - mean[f])/(sqrt(variance[f]) + .000001f);
            }
        }
    }
}

void axpy_cpu(int N, float ALPHA, float *X, int INCX, float *Y, int INCY)
{
	int i;
	for (i = 0; i < N; ++i) Y[i*INCY] += ALPHA*X[i*INCX];
}

void scal_cpu(int N, float ALPHA, float *X, int INCX)
{
    int i;
    for(i = 0; i < N; ++i) X[i*INCX] *= ALPHA;
}

void fill_cpu(int N, float ALPHA, float *X, int INCX)
{
    int i;
    for(i = 0; i < N; ++i) X[i*INCX] = ALPHA;
}

void copy_cpu(int N, float *X, int INCX, float *Y, int INCY)
{
	int i;
	for (i = 0; i < N; ++i) Y[i*INCY] = X[i*INCX];
}

void softmax(float *input, int n, float temp, float *output, int stride)
{
    int i;
    float sum = 0;
    float largest = -FLT_MAX;
    for(i = 0; i < n; ++i){
        if(input[i*stride] > largest) largest = input[i*stride];
    }
    for(i = 0; i < n; ++i){
        float e = exp(input[i*stride]/temp - largest/temp);
        sum += e;
        output[i*stride] = e;
    }
    for(i = 0; i < n; ++i){
        output[i*stride] /= sum;
    }
}
