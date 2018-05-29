#include "network.h"

RectangleCandidateContainer test_detector(IplImage *src, float thresh, network net_glb, char** names_glb, int classes)
{
	fuse_conv_batchnorm(net_glb);
//    srand(2222222);
    
    float nms=.45;	// 0.4F
	image im = load_image_color_ipl_in(src, 0, 0);

	network_predict_image(&net_glb, im); 
	int nboxes = 0;
	detection *dets = get_network_boxes(&net_glb, im.w, im.h, thresh, 0, 1, &nboxes, 1);
		
	if (nms) do_nms_sort(dets, nboxes, classes, nms);
		
	RectangleCandidateContainer container = draw_detections_v3(im, dets, nboxes, thresh, names_glb, classes);
		
	free_detections(dets, nboxes);
    free_image(im);

	return container;
}
