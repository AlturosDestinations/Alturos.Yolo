#ifndef BOX_H
#define BOX_H

#ifdef YOLODLL_EXPORTS
#if defined(_MSC_VER)
#define YOLODLL_API __declspec(dllexport) 
#else
#define YOLODLL_API __attribute__((visibility("default")))
#endif
#else
#if defined(_MSC_VER)
#define YOLODLL_API
#else
#define YOLODLL_API
#endif
#endif

typedef struct{
    float x, y, w, h;
} box;

typedef struct{
    float dx, dy, dw, dh;
} dbox;

typedef struct detection {
	box bbox;
	int classes;
	float *prob;
	float *mask;
	float objectness;
	int sort_class;
} detection;

float box_iou(box a, box b);

YOLODLL_API void do_nms_sort(detection *dets, int total, int classes, float thresh);

typedef struct {
		int x, y, w, h;
	} int_box;

typedef struct rectangelecandidate {
	int_box rectangle;
	double confidence;
	//char * objectType;
	char objectType[50];

} RectangleCandidate;

typedef struct recanglecandidatecontainer {
	RectangleCandidate candidates[10];
} RectangleCandidateContainer;

#endif
