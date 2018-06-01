#ifndef IMAGE_H
#define IMAGE_H

#include "box.h"
#include "opencv2/highgui/highgui_c.h"

typedef struct {
    int w;
    int h;
    int c;
    float *data;
} image;

RectangleCandidateContainer draw_detections_v3(image im, detection *dets, int num, float thresh, char **names, int classes);

image resize_image(image im, int w, int h);
void fill_image(image m, float s);
YOLODLL_API image letterbox_image(image im, int w, int h);
void embed_image(image source, image dest, int dx, int dy);
YOLODLL_API void rgbgr_image(image im);
YOLODLL_API image make_image(int w, int h, int c);
image make_empty_image(int w, int h, int c);
image float_to_image(int w, int h, int c, float *data);
YOLODLL_API image load_image_color_ipl_in(IplImage* src, int w, int h);

float get_pixel(image m, int x, int y, int c);
void set_pixel(image m, int x, int y, int c, float val);
void add_pixel(image m, int x, int y, int c, float val);

YOLODLL_API void free_image(image m);
#endif
