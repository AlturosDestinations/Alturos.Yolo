#ifndef YOLO_LAYER_H
#define YOLO_LAYER_H

#include "layer.h"
#include "network.h"

int yolo_num_detections(layer l, float thresh);
void correct_yolo_boxes(detection *dets, int n, int w, int h, int netw, int neth, int relative, int letter);

#endif
