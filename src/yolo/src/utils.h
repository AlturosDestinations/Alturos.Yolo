#ifndef UTILS_H
#define UTILS_H
#include <stdio.h>

#if defined(_MSC_VER) && _MSC_VER < 1900
	#define snprintf(buf,len, format,...) _snprintf_s(buf, len,len, format, __VA_ARGS__)
#endif

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

int *read_map(char *filename);
YOLODLL_API void free_ptrs(void **ptrs, int n);
void error(const char *s);
void malloc_error();
void file_error(char *s);
void strip(char *s);
char *fgetl(FILE *fp);
float rand_uniform(float min, float max);

#endif
