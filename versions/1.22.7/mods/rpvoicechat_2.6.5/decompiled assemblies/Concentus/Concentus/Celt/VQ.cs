using System;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x02000091 RID: 145
	internal static class VQ
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x00032950 File Offset: 0x00030B50
		internal unsafe static void exp_rotation1(Span<int> X, int X_ptr, int len, int stride, int c, int s)
		{
			int num = X_ptr;
			int a = Inlines.NEG16(s);
			for (int i = 0; i < len - stride; i++)
			{
				int b = *X[num];
				int b2 = *X[num + stride];
				*X[num + stride] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MAC16_16(Inlines.MULT16_16(c, b2), s, b), 15));
				*X[num] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MAC16_16(Inlines.MULT16_16(c, b), a, b2), 15));
				num++;
			}
			num = X_ptr + (len - 2 * stride - 1);
			for (int i = len - 2 * stride - 1; i >= 0; i--)
			{
				int b3 = *X[num];
				int b4 = *X[num + stride];
				*X[num + stride] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MAC16_16(Inlines.MULT16_16(c, b4), s, b3), 15));
				*X[num] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MAC16_16(Inlines.MULT16_16(c, b3), a, b4), 15));
				num--;
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00032A64 File Offset: 0x00030C64
		internal static void exp_rotation(Span<int> X, int X_ptr, int len, int dir, int stride, int K, int spread)
		{
			int num = 0;
			if (2 * K >= len || spread == 0)
			{
				return;
			}
			int num2 = VQ.SPREAD_FACTOR[spread - 1];
			int num3 = Inlines.celt_div(Inlines.MULT16_16(32767, len), len + num2 * K);
			int num4 = Inlines.HALF16(Inlines.MULT16_16_Q15(num3, num3));
			int num5 = Inlines.celt_cos_norm(Inlines.EXTEND32(num4));
			int num6 = Inlines.celt_cos_norm(Inlines.EXTEND32(Inlines.SUB16(32767, num4)));
			if (len >= 8 * stride)
			{
				num = 1;
				while ((num * num + num) * stride + (stride >> 2) < len)
				{
					num++;
				}
			}
			len = Inlines.celt_udiv(len, stride);
			for (int i = 0; i < stride; i++)
			{
				if (dir < 0)
				{
					if (num != 0)
					{
						VQ.exp_rotation1(X, X_ptr + i * len, len, num, num6, num5);
					}
					VQ.exp_rotation1(X, X_ptr + i * len, len, 1, num5, num6);
				}
				else
				{
					VQ.exp_rotation1(X, X_ptr + i * len, len, 1, num5, (int)((short)(0 - num6)));
					if (num != 0)
					{
						VQ.exp_rotation1(X, X_ptr + i * len, len, num, num6, (int)((short)(0 - num5)));
					}
				}
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00032B60 File Offset: 0x00030D60
		internal unsafe static void normalise_residual(int[] iy, Span<int> X, int X_ptr, int N, int Ryy, int gain)
		{
			int num = Inlines.celt_ilog2(Ryy) >> 1;
			int a = Inlines.MULT16_16_P15(Inlines.celt_rsqrt_norm(Inlines.VSHR32(Ryy, 2 * (num - 7))), gain);
			int num2 = 0;
			do
			{
				*X[X_ptr + num2] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MULT16_16(a, iy[num2]), num + 1));
			}
			while (++num2 < N);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00032BBC File Offset: 0x00030DBC
		internal static uint extract_collapse_mask(int[] iy, int N, int B)
		{
			if (B <= 1)
			{
				return 1U;
			}
			int num = Inlines.celt_udiv(N, B);
			uint num2 = 0U;
			int num3 = 0;
			do
			{
				uint num4 = 0U;
				int num5 = 0;
				do
				{
					num4 |= (uint)iy[num3 * num + num5];
				}
				while (++num5 < num);
				num2 |= ((((num4 > 0U) ? 1U : 0U) << (num3 & 31)) ? 1U : 0U);
			}
			while (++num3 < B);
			return num2;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00032C0C File Offset: 0x00030E0C
		internal unsafe static uint alg_quant(Span<int> X, int X_ptr, int N, int K, int spread, int B, EntropyCoder enc, Span<byte> encodedData)
		{
			int[] array = new int[N];
			int[] array2 = new int[N];
			int[] array3 = new int[N];
			VQ.exp_rotation(X, X_ptr, N, 1, B, K, spread);
			int num = 0;
			int num2 = 0;
			do
			{
				int num3 = X_ptr + num2;
				array3[num2] = ((*X[num3] > 0) ? 1 : -1);
				*X[num3] = Inlines.ABS16(*X[num3]);
				array2[num2] = 0;
				array[num2] = 0;
			}
			while (++num2 < N);
			int num5;
			int num4 = num5 = 0;
			int num6 = K;
			if (K > N >> 1)
			{
				num2 = 0;
				do
				{
					num += *X[X_ptr + num2];
				}
				while (++num2 < N);
				if (num <= K)
				{
					*X[X_ptr] = 16384;
					num2 = X_ptr + 1;
					do
					{
						*X[num2] = 0;
					}
					while (++num2 < N + X_ptr);
					num = 16384;
				}
				int b = (int)Inlines.EXTRACT16(Inlines.MULT16_32_Q16(K - 1, Inlines.celt_rcp(num)));
				num2 = 0;
				do
				{
					array2[num2] = Inlines.MULT16_16_Q15(*X[X_ptr + num2], b);
					array[num2] = array2[num2];
					num4 = Inlines.MAC16_16(num4, array[num2], array[num2]);
					num5 = Inlines.MAC16_16(num5, *X[X_ptr + num2], array[num2]);
					array[num2] *= 2;
					num6 -= array2[num2];
				}
				while (++num2 < N);
			}
			if (num6 > N + 3)
			{
				int num7 = num6;
				num4 = Inlines.MAC16_16(num4, num7, num7);
				num4 = Inlines.MAC16_16(num4, num7, array[0]);
				array2[0] += num6;
				num6 = 0;
			}
			int num8 = 1;
			for (int i = 0; i < num6; i++)
			{
				int b2 = -32767;
				int a = 0;
				int shift = 1 + Inlines.celt_ilog2(K - num6 + i + 1);
				int num9 = 0;
				num4 = Inlines.ADD16(num4, 1);
				num2 = 0;
				do
				{
					int num10 = (int)Inlines.EXTRACT16(Inlines.SHR32(Inlines.ADD32(num5, Inlines.EXTEND32(*X[X_ptr + num2])), shift));
					int num11 = Inlines.ADD16(num4, array[num2]);
					num10 = Inlines.MULT16_16_Q15(num10, num10);
					if (Inlines.MULT16_16(a, num10) > Inlines.MULT16_16(num11, b2))
					{
						a = num11;
						b2 = num10;
						num9 = num2;
					}
				}
				while (++num2 < N);
				num5 = Inlines.ADD32(num5, Inlines.EXTEND32(*X[X_ptr + num9]));
				num4 = Inlines.ADD16(num4, array[num9]);
				array[num9] += 2 * num8;
				array2[num9]++;
			}
			num2 = 0;
			do
			{
				*X[X_ptr + num2] = Inlines.MULT16_16(array3[num2], *X[X_ptr + num2]);
				array2[num2] = ((array3[num2] < 0) ? (-array2[num2]) : array2[num2]);
			}
			while (++num2 < N);
			CWRS.encode_pulses(array2, N, K, enc, encodedData);
			return VQ.extract_collapse_mask(array2, N, B);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00032EEC File Offset: 0x000310EC
		internal static uint alg_unquant(Span<int> X, int X_ptr, int N, int K, int spread, int B, EntropyCoder dec, ReadOnlySpan<byte> encodedData, int gain)
		{
			int[] array = new int[N];
			int ryy = CWRS.decode_pulses(array, N, K, dec, encodedData);
			VQ.normalise_residual(array, X, X_ptr, N, ryy, gain);
			VQ.exp_rotation(X, X_ptr, N, -1, B, K, spread);
			return VQ.extract_collapse_mask(array, N, B);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00032F30 File Offset: 0x00031130
		internal unsafe static void renormalise_vector(Span<int> X, int N, int gain)
		{
			int num = 1 + Kernels.celt_inner_prod(X, X, N);
			int num2 = Inlines.celt_ilog2(num) >> 1;
			int a = Inlines.MULT16_16_P15(Inlines.celt_rsqrt_norm(Inlines.VSHR32(num, 2 * (num2 - 7))), gain);
			int num3 = 0;
			for (int i = 0; i < N; i++)
			{
				*X[num3] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MULT16_16(a, *X[num3]), num2 + 1));
				num3++;
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00032FA0 File Offset: 0x000311A0
		internal unsafe static int stereo_itheta(Span<int> X, Span<int> Y, int stereo, int N)
		{
			int num2;
			int num = num2 = 1;
			if (stereo != 0)
			{
				for (int i = 0; i < N; i++)
				{
					int num3 = Inlines.ADD16(Inlines.SHR16(*X[i], 1), Inlines.SHR16(*Y[i], 1));
					int num4 = Inlines.SUB16(Inlines.SHR16(*X[i], 1), Inlines.SHR16(*Y[i], 1));
					num2 = Inlines.MAC16_16(num2, num3, num3);
					num = Inlines.MAC16_16(num, num4, num4);
				}
			}
			else
			{
				num2 += Kernels.celt_inner_prod(X, X, N);
				num += Kernels.celt_inner_prod(Y, Y, N);
			}
			int x = Inlines.celt_sqrt(num2);
			int y = Inlines.celt_sqrt(num);
			return Inlines.MULT16_16_Q15(20861, Inlines.celt_atan2p(y, x));
		}

		// Token: 0x04000477 RID: 1143
		private static int[] SPREAD_FACTOR = new int[]
		{
			15,
			10,
			5
		};
	}
}
