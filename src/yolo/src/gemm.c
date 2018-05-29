#include "gemm.h"
#include "utils.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>

void gemm(int TA, int TB, int M, int N, int K, float ALPHA, 
        float *A, int lda, 
        float *B, int ldb,
        float BETA,
        float *C, int ldc)
{
    gemm_cpu( TA,  TB,  M, N, K, ALPHA,A,lda, B, ldb,BETA,C,ldc);
}

#if (defined(__AVX__) && defined(__x86_64__)) || defined(_WIN64)

#define OSXSAVEFlag (1UL<<27)
#define AVXFlag     ((1UL<<28)|OSXSAVEFlag)
#define FMAFlag     ((1UL<<12)|AVXFlag|OSXSAVEFlag)
#define CLMULFlag   ((1UL<< 1)|AVXFlag|OSXSAVEFlag)
#define VAESFlag    ((1UL<<25)|AVXFlag|OSXSAVEFlag)

#include <stdint.h>
//#include <cstdint>

#ifdef _WIN64
#include <intrin.h>
#include <ammintrin.h>
#include <immintrin.h>
#include <smmintrin.h>

#else	// Linux GCC/Clang
#include <x86intrin.h>
#include <ammintrin.h>
#include <immintrin.h>
#include <smmintrin.h>
#include <cpuid.h>

void asm_cpuid(uint32_t* abcd, uint32_t eax)
{
	uint32_t ebx = 0, edx = 0, ecx = 0;

	// EBX is saved to EDI and later restored
	__asm__("movl %%ebx, %%edi;"
		"cpuid;"
		"xchgl %%ebx, %%edi;"
		: "=D"(ebx),
		"+a"(eax), "+c"(ecx), "=d"(edx));

	abcd[0] = eax;
	abcd[1] = ebx;
	abcd[2] = ecx;
	abcd[3] = edx;
}

#endif

int simd_detect_x86(unsigned int idFeature)
{
	uint32_t regs[4];	// EAX, EBX, ECX, EDX;
#ifdef _WIN32
	__cpuid(regs, 0);
	if (regs[0] > 1U) __cpuid(regs, 1);
#else
	__get_cpuid(0, &regs[0], &regs[1], &regs[2], &regs[3]);
	if(regs[0] > 1U) __get_cpuid(1, &regs[0], &regs[1], &regs[2], &regs[3]);
#endif

	if ((regs[2] & idFeature) != idFeature)
		return 0;
	return 1;
}

int is_fma_avx() {
	static int result = -1;
	if (result == -1) {
		result = simd_detect_x86(AVXFlag);
		if (result == 1) printf(" Used AVX \n");
		else printf(" Not used AVX \n");
	}
	return result;
}

// https://software.intel.com/sites/landingpage/IntrinsicsGuide
void gemm_nn(int M, int N, int K, float ALPHA,
	float *A, int lda,
	float *B, int ldb,
	float *C, int ldc)
{
	int i, j, k;
	if (is_fma_avx() == 1) {	// AVX
		for (i = 0; i < M; ++i) {
			for (k = 0; k < K; ++k) {
				float A_PART = ALPHA*A[i*lda + k];
				__m256 a256, b256, c256, result256;	// AVX
				a256 = _mm256_set1_ps(A_PART);
				for (j = 0; j < N - 8; j += 8) {
					b256 = _mm256_loadu_ps(&B[k*ldb + j]);
					c256 = _mm256_loadu_ps(&C[i*ldc + j]);
					// FMA - Intel Haswell (2013), AMD Piledriver (2012)
					//result256 = _mm256_fmadd_ps(a256, b256, c256);
					result256 = _mm256_mul_ps(a256, b256);
					result256 = _mm256_add_ps(result256, c256);
					_mm256_storeu_ps(&C[i*ldc + j], result256);
				}

				int prev_end = (N % 8 == 0) ? (N - 8) : (N / 8) * 8;
				for (j = prev_end; j < N; ++j)
					C[i*ldc + j] += A_PART*B[k*ldb + j];
			}
		}
	}
	else {
		for (i = 0; i < M; ++i) {
			for (k = 0; k < K; ++k) {
				register float A_PART = ALPHA*A[i*lda + k];
				for (j = 0; j < N; ++j) {
					C[i*ldc + j] += A_PART*B[k*ldb + j];
				}			
			}
		}
	}
}
#else

void gemm_nn(int M, int N, int K, float ALPHA,
	float *A, int lda,
	float *B, int ldb,
	float *C, int ldc)
{
	int i, j, k;
	for (i = 0; i < M; ++i) {
		for (k = 0; k < K; ++k) {
			register float A_PART = ALPHA*A[i*lda + k];
			for (j = 0; j < N; ++j) {
				C[i*ldc + j] += A_PART*B[k*ldb + j];
			}
		}
	}
}
#endif	// __x86_64

void gemm_cpu(int TA, int TB, int M, int N, int K, float ALPHA, 
        float *A, int lda, 
        float *B, int ldb,
        float BETA,
        float *C, int ldc)
{
    //printf("cpu: %d %d %d %d %d %f %d %d %f %d\n",TA, TB, M, N, K, ALPHA, lda, ldb, BETA, ldc);
 /*   int i, j;
    for(i = 0; i < M; ++i){
        for(j = 0; j < N; ++j){
            C[i*ldc + j] *= BETA;
        }
    }*/

	int t;
	#pragma omp parallel for
	for (t = 0; t < M; ++t) 
	{
		gemm_nn(1, N, K, ALPHA, A + t*lda, lda, B, ldb, C + t*ldc, ldc);	
	}
}
