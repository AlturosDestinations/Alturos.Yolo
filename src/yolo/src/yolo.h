#pragma once

#include <iostream>
#include <vector>
#include "box.h"

extern "C" __declspec(dllexport) int tr_context_new(char *cfgfile, char *name_list, char *weightfile);

extern "C" __declspec(dllexport) int tr_process_image(const uint8_t* data, const size_t data_length, RectangleCandidateContainer &container);
