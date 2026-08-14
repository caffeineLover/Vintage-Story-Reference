using System;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x0200008D RID: 141
	internal static class Pitch
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x0002FE04 File Offset: 0x0002E004
		internal unsafe static void find_best_pitch(Span<int> xcorr, Span<int> y, int len, int max_pitch, int[] best_pitch, int yshift, int maxcorr)
		{
			int num = 1;
			int shift = Inlines.celt_ilog2(maxcorr) - 14;
			int num2 = -1;
			int a = -1;
			int num3 = 0;
			int b = 0;
			best_pitch[0] = 0;
			best_pitch[1] = 1;
			for (int i = 0; i < len; i++)
			{
				num = Inlines.ADD32(num, Inlines.SHR32(Inlines.MULT16_16(*y[i], *y[i]), yshift));
			}
			for (int j = 0; j < max_pitch; j++)
			{
				if (*xcorr[j] > 0)
				{
					short num4 = Inlines.EXTRACT16(Inlines.VSHR32(*xcorr[j], shift));
					int num5 = Inlines.MULT16_16_Q15((int)num4, (int)num4);
					if (Inlines.MULT16_32_Q15(num5, b) > Inlines.MULT16_32_Q15(a, num))
					{
						if (Inlines.MULT16_32_Q15(num5, num3) > Inlines.MULT16_32_Q15(num2, num))
						{
							a = num2;
							b = num3;
							best_pitch[1] = best_pitch[0];
							num2 = num5;
							num3 = num;
							best_pitch[0] = j;
						}
						else
						{
							a = num5;
							b = num;
							best_pitch[1] = j;
						}
					}
				}
				num += Inlines.SHR32(Inlines.MULT16_16(*y[j + len], *y[j + len]), yshift) - Inlines.SHR32(Inlines.MULT16_16(*y[j], *y[j]), yshift);
				num = Inlines.MAX32(1, num);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0002FF3C File Offset: 0x0002E13C
		internal static void celt_fir5(int[] x, int[] num, int[] y, int N, int[] mem)
		{
			int a = num[0];
			int a2 = num[1];
			int a3 = num[2];
			int a4 = num[3];
			int a5 = num[4];
			int num2 = mem[0];
			int num3 = mem[1];
			int num4 = mem[2];
			int num5 = mem[3];
			int num6 = mem[4];
			for (int i = 0; i < N; i++)
			{
				int num7 = Inlines.SHL32(Inlines.EXTEND32(x[i]), 12);
				num7 = Inlines.MAC16_16(num7, a, num2);
				num7 = Inlines.MAC16_16(num7, a2, num3);
				num7 = Inlines.MAC16_16(num7, a3, num4);
				num7 = Inlines.MAC16_16(num7, a4, num5);
				num7 = Inlines.MAC16_16(num7, a5, num6);
				num6 = num5;
				num5 = num4;
				num4 = num3;
				num3 = num2;
				num2 = x[i];
				y[i] = Inlines.ROUND16(num7, 12);
			}
			mem[0] = num2;
			mem[1] = num3;
			mem[2] = num4;
			mem[3] = num5;
			mem[4] = num6;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00030018 File Offset: 0x0002E218
		internal static void pitch_downsample(int[][] x, int[] x_lp, int len, int C)
		{
			int[] array = new int[5];
			int b = 32767;
			int[] array2 = new int[4];
			int[] mem = new int[5];
			int[] array3 = new int[5];
			int a = 26214;
			int num = Inlines.celt_maxabs32(x[0], 0, len);
			if (C == 2)
			{
				int b2 = Inlines.celt_maxabs32(x[1], 0, len);
				num = Inlines.MAX32(num, b2);
			}
			if (num < 1)
			{
				num = 1;
			}
			int num2 = Inlines.celt_ilog2(num) - 10;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (C == 2)
			{
				num2++;
			}
			int num3 = len >> 1;
			for (int i = 1; i < num3; i++)
			{
				x_lp[i] = Inlines.SHR32(Inlines.HALF32(Inlines.HALF32(x[0][2 * i - 1] + x[0][2 * i + 1]) + x[0][2 * i]), num2);
			}
			x_lp[0] = Inlines.SHR32(Inlines.HALF32(Inlines.HALF32(x[0][1]) + x[0][0]), num2);
			if (C == 2)
			{
				for (int i = 1; i < num3; i++)
				{
					x_lp[i] += Inlines.SHR32(Inlines.HALF32(Inlines.HALF32(x[1][2 * i - 1] + x[1][2 * i + 1]) + x[1][2 * i]), num2);
				}
				x_lp[0] += Inlines.SHR32(Inlines.HALF32(Inlines.HALF32(x[1][1]) + x[1][0]), num2);
			}
			Autocorrelation._celt_autocorr(x_lp, array, null, 0, 4, num3);
			array[0] += Inlines.SHR32(array[0], 13);
			for (int i = 1; i <= 4; i++)
			{
				array[i] -= Inlines.MULT16_32_Q15(2 * i * i, array[i]);
			}
			CeltLPC.celt_lpc(array2, array, 4);
			for (int i = 0; i < 4; i++)
			{
				b = Inlines.MULT16_16_Q15(29491, b);
				array2[i] = Inlines.MULT16_16_Q15(array2[i], b);
			}
			array3[0] = array2[0] + 3277;
			array3[1] = array2[1] + Inlines.MULT16_16_Q15(a, array2[0]);
			array3[2] = array2[2] + Inlines.MULT16_16_Q15(a, array2[1]);
			array3[3] = array2[3] + Inlines.MULT16_16_Q15(a, array2[2]);
			array3[4] = Inlines.MULT16_16_Q15(a, array2[3]);
			Pitch.celt_fir5(x_lp, array3, x_lp, num3, mem);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00030248 File Offset: 0x0002E448
		internal unsafe static void pitch_search(Span<int> x_lp, int x_lp_ptr, int[] y, int len, int max_pitch, out int pitch)
		{
			int[] array = new int[2];
			int num = len + max_pitch;
			int[] array2 = new int[len >> 2];
			int[] array3 = new int[num >> 2];
			int[] array4 = new int[max_pitch >> 1];
			for (int i = 0; i < len >> 2; i++)
			{
				array2[i] = *x_lp[x_lp_ptr + 2 * i];
			}
			for (int i = 0; i < num >> 2; i++)
			{
				array3[i] = y[2 * i];
			}
			int a = Inlines.celt_maxabs32(array2, len >> 2);
			int b = Inlines.celt_maxabs32(array3, num >> 2);
			int num2 = Inlines.celt_ilog2(Inlines.MAX32(1, Inlines.MAX32(a, b))) - 11;
			if (num2 > 0)
			{
				for (int i = 0; i < len >> 2; i++)
				{
					array2[i] = Inlines.SHR16(array2[i], num2);
				}
				for (int i = 0; i < num >> 2; i++)
				{
					array3[i] = Inlines.SHR16(array3[i], num2);
				}
				num2 *= 2;
			}
			else
			{
				num2 = 0;
			}
			int num3 = CeltPitchXCorr.pitch_xcorr(array2, 0, array3, 0, array4, len >> 2, max_pitch >> 2);
			Pitch.find_best_pitch(array4, array3, len >> 2, max_pitch >> 2, array, 0, num3);
			num3 = 1;
			for (int j = 0; j < max_pitch >> 1; j++)
			{
				array4[j] = 0;
				if (Inlines.abs(j - 2 * array[0]) <= 2 || Inlines.abs(j - 2 * array[1]) <= 2)
				{
					int num4 = 0;
					for (int i = 0; i < len >> 1; i++)
					{
						num4 += Inlines.SHR32(Inlines.MULT16_16(*x_lp[x_lp_ptr + i], y[j + i]), num2);
					}
					array4[j] = Inlines.MAX32(-1, num4);
					num3 = Inlines.MAX32(num3, num4);
				}
			}
			Pitch.find_best_pitch(array4, y, len >> 1, max_pitch >> 1, array, num2 + 1, num3);
			int num8;
			if (array[0] > 0 && array[0] < (max_pitch >> 1) - 1)
			{
				int num5 = array4[array[0] - 1];
				int num6 = array4[array[0]];
				int num7 = array4[array[0] + 1];
				if (num7 - num5 > Inlines.MULT16_32_Q15(22938, num6 - num5))
				{
					num8 = 1;
				}
				else if (num5 - num7 > Inlines.MULT16_32_Q15(22938, num6 - num7))
				{
					num8 = -1;
				}
				else
				{
					num8 = 0;
				}
			}
			else
			{
				num8 = 0;
			}
			pitch = 2 * array[0] - num8;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0003048C File Offset: 0x0002E68C
		internal unsafe static int remove_doubling(int[] x, int maxperiod, int minperiod, int N, ref int T0_, int prev_period, int prev_gain)
		{
			Span<int> span = new int[3];
			int num = minperiod;
			maxperiod /= 2;
			minperiod /= 2;
			T0_ /= 2;
			prev_period /= 2;
			N /= 2;
			int num2 = maxperiod;
			if (T0_ >= maxperiod)
			{
				T0_ = maxperiod - 1;
			}
			int num4;
			int num3 = num4 = T0_;
			Span<int> span2 = new int[maxperiod + 1];
			int num5;
			int num6;
			Kernels.dual_inner_prod(MemoryExtensions.AsSpan<int>(x).Slice(num2), MemoryExtensions.AsSpan<int>(x).Slice(num2), MemoryExtensions.AsSpan<int>(x).Slice(num2 - num3), N, out num5, out num6);
			*span2[0] = num5;
			int num7 = num5;
			for (int i = 1; i <= maxperiod; i++)
			{
				int num8 = num2 - i;
				num7 = num7 + Inlines.MULT16_16(x[num8], x[num8]) - Inlines.MULT16_16(x[num8 + N], x[num8 + N]);
				*span2[i] = Inlines.MAX32(0, num7);
			}
			num7 = *span2[num3];
			int num9 = num6;
			int num10 = num7;
			int num11 = 1 + Inlines.HALF32(Inlines.MULT32_32_Q31(num5, num7));
			int num12 = Inlines.celt_ilog2(num11) >> 1;
			int num13 = Inlines.VSHR32(Inlines.MULT16_32_Q15(Inlines.celt_rsqrt_norm(Inlines.VSHR32(num11, 2 * (num12 - 7))), num6), num12 + 1);
			int b = num13;
			for (int j = 2; j <= 15; j++)
			{
				int num14 = Inlines.celt_udiv(2 * num3 + j, 2 * j);
				if (num14 < minperiod)
				{
					break;
				}
				int num15;
				if (j == 2)
				{
					if (num14 + num3 > maxperiod)
					{
						num15 = num3;
					}
					else
					{
						num15 = num3 + num14;
					}
				}
				else
				{
					num15 = Inlines.celt_udiv(2 * Pitch.second_check[j] * num3 + j, 2 * j);
				}
				int num16;
				Kernels.dual_inner_prod(MemoryExtensions.AsSpan<int>(x).Slice(num2), MemoryExtensions.AsSpan<int>(x).Slice(num2 - num14), MemoryExtensions.AsSpan<int>(x).Slice(num2 - num15), N, out num6, out num16);
				num6 += num16;
				num7 = *span2[num14] + *span2[num15];
				int num17 = 1 + Inlines.MULT32_32_Q31(num5, num7);
				int num18 = Inlines.celt_ilog2(num17) >> 1;
				int num19 = Inlines.VSHR32(Inlines.MULT16_32_Q15(Inlines.celt_rsqrt_norm(Inlines.VSHR32(num17, 2 * (num18 - 7))), num6), num18 + 1);
				int num20;
				if (Inlines.abs(num14 - prev_period) <= 1)
				{
					num20 = prev_gain;
				}
				else if (Inlines.abs(num14 - prev_period) <= 2 && 5 * j * j < num3)
				{
					num20 = Inlines.HALF16(prev_gain);
				}
				else
				{
					num20 = 0;
				}
				int num21 = Inlines.MAX16(9830, Inlines.MULT16_16_Q15(22938, b) - num20);
				if (num14 < 3 * minperiod)
				{
					num21 = Inlines.MAX16(13107, Inlines.MULT16_16_Q15(27853, b) - num20);
				}
				else if (num14 < 2 * minperiod)
				{
					num21 = Inlines.MAX16(16384, Inlines.MULT16_16_Q15(29491, b) - num20);
				}
				if (num19 > num21)
				{
					num9 = num6;
					num10 = num7;
					num4 = num14;
					num13 = num19;
				}
			}
			num9 = Inlines.MAX32(0, num9);
			int num22;
			if (num10 <= num9)
			{
				num22 = 32767;
			}
			else
			{
				num22 = Inlines.SHR32(Inlines.frac_div32(num9, num10 + 1), 16);
			}
			for (int j = 0; j < 3; j++)
			{
				*span[j] = Kernels.celt_inner_prod(MemoryExtensions.AsSpan<int>(x).Slice(num2), MemoryExtensions.AsSpan<int>(x).Slice(num2 - (num4 + j - 1)), N);
			}
			int num23;
			if (*span[2] - *span[0] > Inlines.MULT16_32_Q15(22938, *span[1] - *span[0]))
			{
				num23 = 1;
			}
			else if (*span[0] - *span[2] > Inlines.MULT16_32_Q15(22938, *span[1] - *span[2]))
			{
				num23 = -1;
			}
			else
			{
				num23 = 0;
			}
			if (num22 > num13)
			{
				num22 = num13;
			}
			T0_ = 2 * num4 + num23;
			if (T0_ < num)
			{
				T0_ = num;
			}
			return num22;
		}

		// Token: 0x04000455 RID: 1109
		private static readonly int[] second_check = new int[]
		{
			0,
			0,
			3,
			2,
			3,
			2,
			5,
			2,
			3,
			2,
			3,
			2,
			5,
			2,
			3,
			2
		};
	}
}
