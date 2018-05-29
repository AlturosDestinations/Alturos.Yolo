#include "image.h"
#include "utils.h"
#include "blas.h"
#include <stdio.h>
#include <math.h>

#ifdef OPENCV
#include "opencv2/highgui/highgui_c.h"
#include "opencv2/imgproc/imgproc_c.h"
#include "opencv2/core/version.hpp"
#endif

RectangleCandidateContainer draw_detections_v3(image im, detection *dets, int num, float thresh, char **names, int classes)
{
	int i, j, a = 0;
	RectangleCandidateContainer container;

	for (int k = 0; k < 10; k++)
	{	
		container.candidates[k].confidence = 0;
		container.candidates[k].rectangle.x = 0;
		container.candidates[k].rectangle.y = 0;
		container.candidates[k].rectangle.w = 0;
		container.candidates[k].rectangle.h = 0;
		//container.candidates[k].objectType = 0;
		for (int z = 0; z < 50; z++)
		{
			container.candidates[k].objectType[z] = 0;
		}
	}

	for (i = 0; i < num; ++i) 
	{
		char labelstr[4096] = { 0 };
		int class_id = -1;
		for (j = 0; j < classes; ++j) 
		{
			if (dets[i].prob[j] > thresh) 
			{
				if (class_id < 0) {
					strcat(labelstr, names[j]);
					class_id = j;
				}
				else {
					strcat(labelstr, ", ");
					strcat(labelstr, names[j]);
				}
				//printf("%s: %.0f%%\n", names[j], dets[i].prob[j] * 100);
				//printf("%s: %.0f%% \t %f\t%f\t%f\t%f \n", names[j], dets[i].prob[j] * 100, dets[i].bbox.x*im.w, dets[i].bbox.y*im.h, dets[i].bbox.w*im.w, dets[i].bbox.h*im.h);
		
				if (a >= 10)
				{
					for (int k = 0; k < 10; k++)
					{
						if (container.candidates[k].confidence < (dets[i].prob[j] * 100))
						{
							container.candidates[k].confidence = dets[i].prob[j] * 100;
							container.candidates[k].rectangle.x = round(dets[i].bbox.x*im.w - round((dets[i].bbox.w*im.w)/2));
							container.candidates[k].rectangle.y = round(dets[i].bbox.y*im.h - round((dets[i].bbox.h*im.h)/2));
							container.candidates[k].rectangle.w = round(dets[i].bbox.w*im.w);
							container.candidates[k].rectangle.h = round(dets[i].bbox.h*im.h);
							//container.candidates[k].objectType = names[j];
							for (int z = 0; z < sizeof(names[j]); z++)
							{
								container.candidates[k].objectType[z] = names[j][z];
							}
						}
					}
				}
				else
				{
					container.candidates[a].rectangle.x = round(dets[i].bbox.x*im.w - round((dets[i].bbox.w*im.w) / 2));
					container.candidates[a].rectangle.y = round(dets[i].bbox.y*im.h - round((dets[i].bbox.h*im.h) / 2));
					container.candidates[a].rectangle.w = round(dets[i].bbox.w*im.w);
					container.candidates[a].rectangle.h = round(dets[i].bbox.h*im.h);
					container.candidates[a].confidence = dets[i].prob[j] * 100;
					//container.candidates[a].objectType = names[j];
					for (int z = 0; z < sizeof(names[j]); z++)
					{
						container.candidates[a].objectType[z] = names[j][z];
					}
					
					//printf("%s: %.0f%% \t %d\t%d\t%d\t%d \n", container.candidates[a].objectType, container.candidates[a].confidence, container.candidates[a].rectangle.x, container.candidates[a].rectangle.y, container.candidates[a].rectangle.w, container.candidates[a].rectangle.h);
				}
				a += 1;				
			}
		}		
	}
	
	//for (i = 0; i < a; i++)
	//{
	//	printf("%s: %.0f%% \t %d\t%d\t%d\t%d \n", container.candidates[i].objectType, container.candidates[i].confidence, container.candidates[i].rectangle.x, container.candidates[i].rectangle.y, container.candidates[i].rectangle.w, container.candidates[i].rectangle.h);
	//}
	return container;
}

void embed_image(image source, image dest, int dx, int dy)
{
    int x,y,k;
    for(k = 0; k < source.c; ++k){
        for(y = 0; y < source.h; ++y){
            for(x = 0; x < source.w; ++x){
                float val = get_pixel(source, x,y,k);
                set_pixel(dest, dx+x, dy+y, k, val);
            }
        }
    }
}

void rgbgr_image(image im)
{
    int i;
    for(i = 0; i < im.w*im.h; ++i){
        float swap = im.data[i];
        im.data[i] = im.data[i+im.w*im.h*2];
        im.data[i+im.w*im.h*2] = swap;
    }
}

#ifdef OPENCV

image ipl_to_image(IplImage* src)
{
    unsigned char *data = (unsigned char *)src->imageData;
    int h = src->height;
    int w = src->width;
    int c = src->nChannels;
    int step = src->widthStep;
    image out = make_image(w, h, c);
    int i, j, k, count=0;;

    for(k= 0; k < c; ++k){
        for(i = 0; i < h; ++i){
            for(j = 0; j < w; ++j){
                out.data[count++] = data[i*step + j*c + k]/255.;
            }
        }
    }
    return out;
}

image load_image_cv_ipl_in(IplImage* src)
{
	image out = ipl_to_image(src);
	cvReleaseImage(&src);
	rgbgr_image(out);
	return out;
}

#endif

image make_empty_image(int w, int h, int c)
{
    image out;
    out.data = 0;
    out.h = h;
    out.w = w;
    out.c = c;
    return out;
}

image make_image(int w, int h, int c)
{
    image out = make_empty_image(w,h,c);
    out.data = calloc(h*w*c, sizeof(float));
    return out;
}

image float_to_image(int w, int h, int c, float *data)
{
    image out = make_empty_image(w,h,c);
    out.data = data;
    return out;
}

void fill_image(image m, float s)
{
	int i;
	for (i = 0; i < m.h*m.w*m.c; ++i) m.data[i] = s;
}

image letterbox_image(image im, int w, int h)
{
	int new_w = im.w;
	int new_h = im.h;
	if (((float)w / im.w) < ((float)h / im.h)) {
		new_w = w;
		new_h = (im.h * w) / im.w;
	}
	else {
		new_h = h;
		new_w = (im.w * h) / im.h;
	}
	image resized = resize_image(im, new_w, new_h);
	image boxed = make_image(w, h, im.c);
	fill_image(boxed, .5);

	embed_image(resized, boxed, (w - new_w) / 2, (h - new_h) / 2);
	free_image(resized);
	return boxed;
}

image resize_image(image im, int w, int h)
{
    image resized = make_image(w, h, im.c);   
    image part = make_image(w, im.h, im.c);
    int r, c, k;
    float w_scale = (float)(im.w - 1) / (w - 1);
    float h_scale = (float)(im.h - 1) / (h - 1);
    for(k = 0; k < im.c; ++k){
        for(r = 0; r < im.h; ++r){
            for(c = 0; c < w; ++c){
                float val = 0;
                if(c == w-1 || im.w == 1){
                    val = get_pixel(im, im.w-1, r, k);
                } else {
                    float sx = c*w_scale;
                    int ix = (int) sx;
                    float dx = sx - ix;
                    val = (1 - dx) * get_pixel(im, ix, r, k) + dx * get_pixel(im, ix+1, r, k);
                }
                set_pixel(part, c, r, k, val);
            }
        }
    }
    for(k = 0; k < im.c; ++k){
        for(r = 0; r < h; ++r){
            float sy = r*h_scale;
            int iy = (int) sy;
            float dy = sy - iy;
            for(c = 0; c < w; ++c){
                float val = (1-dy) * get_pixel(part, c, iy, k);
                set_pixel(resized, c, r, k, val);
            }
            if(r == h-1 || im.h == 1) continue;
            for(c = 0; c < w; ++c){
                float val = dy * get_pixel(part, c, iy+1, k);
                add_pixel(resized, c, r, k, val);
            }
        }
    }

    free_image(part);
    return resized;
}

image load_image_ipl_in(IplImage* src, int w, int h, int c)
{
	image out = load_image_cv_ipl_in(src);

	if ((h && w) && (h != out.h || w != out.w)) {
		image resized = resize_image(out, w, h);
		free_image(out);
		out = resized;
	}
	return out;
}

image load_image_color_ipl_in(IplImage* src, int w, int h)
{
	return load_image_ipl_in(src, w, h, 3);
}

float get_pixel(image m, int x, int y, int c)
{
    assert(x < m.w && y < m.h && c < m.c);
    return m.data[c*m.h*m.w + y*m.w + x];
}

void set_pixel(image m, int x, int y, int c, float val)
{
    if (x < 0 || y < 0 || c < 0 || x >= m.w || y >= m.h || c >= m.c) return;
    assert(x < m.w && y < m.h && c < m.c);
    m.data[c*m.h*m.w + y*m.w + x] = val;
}

void add_pixel(image m, int x, int y, int c, float val)
{
    assert(x < m.w && y < m.h && c < m.c);
    m.data[c*m.h*m.w + y*m.w + x] += val;
}

void free_image(image m)
{
    if(m.data){
        free(m.data);
    }
}
