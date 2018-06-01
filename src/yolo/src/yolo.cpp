#include "yolo.h"
#include "box.h"
#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

using namespace std;
using namespace cv;

#include "option_list.h"
#include "parser.h"

extern "C" network parse_network_cfg_custom(char *filename, int batch);
//extern "C" list *read_data_cfg(char *filename);
extern "C" char **get_labels(char *filename);
extern "C" void load_weights(network *net, char *filename);
//extern "C" char *option_find_str_dll(list *l, char *key, char *def);
extern "C" RectangleCandidateContainer test_detector(IplImage *src, float thresh, network net_glb, char** names_glb, int classes);

char **names_glb;
network net_glb;
int classes_glb;

int tr_context_new(char *cfgfile, char *name_list, char *weightfile)
{
//    char *cfgfile = "yolov2-tiny-voc.cfg";
//    list *options = read_data_cfg("voc.data");
//    char *name_list = option_find_str_dll(options, "names", "data/names.list");
    
    FILE *file = fopen(cfgfile, "r");
    if (!file)
        return -1;
    else
        fclose(file);

    file = fopen(name_list, "r");
    if (!file)
        return -1;
    else
        fclose(file);

    file = fopen(weightfile, "r");
    if (!file)
        return -1;
    else
        fclose(file);

    names_glb = get_labels(name_list);
    net_glb = parse_network_cfg_custom(cfgfile, 1); //set batch=1
    load_weights(&net_glb, weightfile);
    layer l = net_glb.layers[net_glb.n - 1];
    classes_glb = l.classes;

    return 1;
}

int tr_process_image(const uint8_t* data, const size_t data_length, RectangleCandidateContainer &container)
{
    std::vector<char> vdata(data, data + data_length);
    
    cv::Mat image = imdecode(cv::Mat(vdata), 1);

    IplImage* image2;
    image2 = cvCreateImage(cvSize(image.cols, image.rows), IPL_DEPTH_8U, 3);
    IplImage ipltemp = image;
    cvCopy(&ipltemp, image2);

    RectangleCandidateContainer cal_container = test_detector(image2, 0.25, net_glb, names_glb, classes_glb);

    for (size_t i = 0; i < 10; ++i)
    {
        container.candidates[i].rectangle.x = cal_container.candidates[i].rectangle.x;
        container.candidates[i].rectangle.y = cal_container.candidates[i].rectangle.y;
        container.candidates[i].rectangle.w = cal_container.candidates[i].rectangle.w;
        container.candidates[i].rectangle.h = cal_container.candidates[i].rectangle.h;            
        container.candidates[i].confidence = cal_container.candidates[i].confidence;

        for (int z = 0; z < 50; z++)
        {
            container.candidates[i].objectType[z] = cal_container.candidates[i].objectType[z];
        }

        //printf("%s: %.0f%% \t %d\t%d\t%d\t%d \n", cal_container.candidates[i].objectType, cal_container.candidates[i].confidence, cal_container.candidates[i].rectangle.x, cal_container.candidates[i].rectangle.y, cal_container.candidates[i].rectangle.w, cal_container.candidates[i].rectangle.h);
    }

    return 1;
}
