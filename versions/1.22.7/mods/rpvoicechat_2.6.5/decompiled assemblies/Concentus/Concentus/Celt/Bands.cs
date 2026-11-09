using System;
using Concentus.Celt.Structs;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Celt
{
	// Token: 0x02000083 RID: 131
	internal static class Bands
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x000280F8 File Offset: 0x000262F8
		internal static int hysteresis_decision(int val, int[] thresholds, int[] hysteresis, int N, int prev)
		{
			int num = 0;
			while (num < N && val >= thresholds[num])
			{
				num++;
			}
			if (num > prev && val < thresholds[prev] + hysteresis[prev])
			{
				num = prev;
			}
			if (num < prev && val > thresholds[prev - 1] - hysteresis[prev - 1])
			{
				num = prev;
			}
			return num;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00028144 File Offset: 0x00026344
		internal static uint celt_lcg_rand(uint seed)
		{
			return 1664525U * seed + 1013904223U;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00028154 File Offset: 0x00026354
		internal static int bitexact_cos(int x)
		{
			int num = 4096 + x * x >> 13;
			num = 32767 - num + Inlines.FRAC_MUL16(num, -7651 + Inlines.FRAC_MUL16(num, 8277 + Inlines.FRAC_MUL16(-626, num)));
			return 1 + num;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000281A0 File Offset: 0x000263A0
		internal static int bitexact_log2tan(int isin, int icos)
		{
			int num = Inlines.EC_ILOG((uint)icos);
			int num2 = Inlines.EC_ILOG((uint)isin);
			icos <<= 15 - num;
			isin <<= 15 - num2;
			return (num2 - num) * 2048 + Inlines.FRAC_MUL16(isin, Inlines.FRAC_MUL16(isin, -2597) + 7932) - Inlines.FRAC_MUL16(icos, Inlines.FRAC_MUL16(icos, -2597) + 7932);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0002820C File Offset: 0x0002640C
		internal static void compute_band_energies(CeltMode m, int[][] X, int[][] bandE, int end, int C, int LM)
		{
			short[] eBands = m.eBands;
			int shortMdctSize = m.shortMdctSize;
			int num = 0;
			do
			{
				for (int i = 0; i < end; i++)
				{
					int num2 = 0;
					int num3 = Inlines.celt_maxabs32(X[num], (int)eBands[i] << LM, (int)(eBands[i + 1] - eBands[i]) << LM);
					if (num3 > 0)
					{
						int num4 = Inlines.celt_ilog2(num3) - 14 + ((m.logN[i] >> 3) + LM + 1 >> 1);
						int num5 = (int)eBands[i] << LM;
						if (num4 > 0)
						{
							do
							{
								num2 = Inlines.MAC16_16(num2, Inlines.EXTRACT16(Inlines.SHR32(X[num][num5], num4)), Inlines.EXTRACT16(Inlines.SHR32(X[num][num5], num4)));
							}
							while (++num5 < (int)eBands[i + 1] << LM);
						}
						else
						{
							do
							{
								num2 = Inlines.MAC16_16(num2, Inlines.EXTRACT16(Inlines.SHL32(X[num][num5], -num4)), Inlines.EXTRACT16(Inlines.SHL32(X[num][num5], -num4)));
							}
							while (++num5 < (int)eBands[i + 1] << LM);
						}
						bandE[num][i] = 1 + Inlines.VSHR32(Inlines.celt_sqrt(num2), -num4);
					}
					else
					{
						bandE[num][i] = 1;
					}
				}
			}
			while (++num < C);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00028348 File Offset: 0x00026548
		internal static void normalise_bands(CeltMode m, int[][] freq, int[][] X, int[][] bandE, int end, int C, int M)
		{
			short[] eBands = m.eBands;
			int num = 0;
			do
			{
				int num2 = 0;
				do
				{
					int num3 = Inlines.celt_zlog2(bandE[num][num2]) - 13;
					int b = (int)Inlines.EXTRACT16(Inlines.celt_rcp(Inlines.SHL32(Inlines.VSHR32(bandE[num][num2], num3), 3)));
					int num4 = M * (int)eBands[num2];
					do
					{
						X[num][num4] = Inlines.MULT16_16_Q15(Inlines.VSHR32(freq[num][num4], num3 - 1), b);
					}
					while (++num4 < M * (int)eBands[num2 + 1]);
				}
				while (++num2 < end);
			}
			while (++num < C);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000283D4 File Offset: 0x000265D4
		internal unsafe static void denormalise_bands(CeltMode m, int[] X, Span<int> freq, int freq_ptr, Span<int> bandLogE, int bandLogE_ptr, int start, int end, int M, int downsample, int silence)
		{
			short[] eBands = m.eBands;
			int num = M * m.shortMdctSize;
			int num2 = M * (int)eBands[end];
			if (downsample != 1)
			{
				num2 = Inlines.IMIN(num2, num / downsample);
			}
			if (silence != 0)
			{
				num2 = 0;
				end = (start = 0);
			}
			int num3 = freq_ptr;
			int num4 = M * (int)eBands[start];
			for (int i = 0; i < M * (int)eBands[start]; i++)
			{
				*freq[num3++] = 0;
			}
			for (int i = start; i < end; i++)
			{
				int num5 = M * (int)eBands[i];
				int num6 = M * (int)eBands[i + 1];
				int num7 = Inlines.ADD16(*bandLogE[bandLogE_ptr + i], (int)Inlines.SHL16((short)Tables.eMeans[i], 6));
				int num8 = 16 - (num7 >> 10);
				int b;
				if (num8 > 31)
				{
					num8 = 0;
					b = 0;
				}
				else
				{
					b = Inlines.celt_exp2_frac(num7 & 1023);
				}
				if (num8 < 0)
				{
					if (num8 < -2)
					{
						b = 32767;
						num8 = -2;
					}
					do
					{
						*freq[num3] = Inlines.SHR32(Inlines.MULT16_16(X[num4], b), -num8);
					}
					while (++num5 < num6);
				}
				else
				{
					do
					{
						*freq[num3++] = Inlines.SHR32(Inlines.MULT16_16(X[num4++], b), num8);
					}
					while (++num5 < num6);
				}
			}
			Arrays.MemSetWithOffset<int>(freq, 0, freq_ptr + num2, num - num2);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00028530 File Offset: 0x00026730
		internal static void anti_collapse(CeltMode m, int[][] X_, byte[] collapse_masks, int LM, int C, int size, int start, int end, int[] logE, int[] prev1logE, int[] prev2logE, int[] pulses, uint seed)
		{
			for (int i = start; i < end; i++)
			{
				int num = (int)(m.eBands[i + 1] - m.eBands[i]);
				int a = Inlines.celt_udiv(1 + pulses[i], (int)(m.eBands[i + 1] - m.eBands[i])) >> LM;
				int b = Inlines.SHR32(Inlines.celt_exp2(0 - Inlines.SHL16(a, 7)), 1);
				int a2 = Inlines.MULT16_32_Q15(16384, Inlines.MIN32(32767, b));
				int num2 = num << LM;
				int num3 = Inlines.celt_ilog2(num2) >> 1;
				int a3 = Inlines.celt_rsqrt_norm(Inlines.SHL32(num2, 7 - num3 << 1));
				int num4 = 0;
				do
				{
					int num5 = 0;
					int a4 = prev1logE[num4 * m.nbEBands + i];
					int num6 = prev2logE[num4 * m.nbEBands + i];
					if (C == 1)
					{
						a4 = Inlines.MAX16(a4, prev1logE[m.nbEBands + i]);
						num6 = Inlines.MAX16(num6, prev2logE[m.nbEBands + i]);
					}
					int num7 = Inlines.EXTEND32(logE[num4 * m.nbEBands + i]) - Inlines.EXTEND32(Inlines.MIN16(a4, num6));
					num7 = Inlines.MAX32(0, num7);
					int num8;
					if (num7 < 16384)
					{
						int b2 = Inlines.SHR32(Inlines.celt_exp2((int)(0 - Inlines.EXTRACT16(num7))), 1);
						num8 = 2 * Inlines.MIN16(16383, b2);
					}
					else
					{
						num8 = 0;
					}
					if (LM == 3)
					{
						num8 = Inlines.MULT16_16_Q14(23170, Inlines.MIN32(23169, num8));
					}
					num8 = Inlines.SHR16(Inlines.MIN16(a2, num8), 1);
					num8 = Inlines.SHR32(Inlines.MULT16_16_Q15(a3, num8), num3);
					int num9 = (int)m.eBands[i] << LM;
					for (int j = 0; j < 1 << LM; j++)
					{
						if (((int)collapse_masks[i * C + num4] & 1 << j) == 0)
						{
							int num10 = num9 + j;
							for (int k = 0; k < num; k++)
							{
								seed = Bands.celt_lcg_rand(seed);
								X_[num4][num10 + (k << LM)] = (((seed & 32768U) != 0U) ? num8 : (0 - num8));
							}
							num5 = 1;
						}
					}
					if (num5 != 0)
					{
						VQ.renormalise_vector(MemoryExtensions.AsSpan<int>(X_[num4]).Slice(num9), num << LM, 32767);
					}
				}
				while (++num4 < C);
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00028774 File Offset: 0x00026974
		internal unsafe static void intensity_stereo(CeltMode m, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int[][] bandE, int bandID, int N)
		{
			int shift = Inlines.celt_zlog2(Inlines.MAX32(bandE[0][bandID], bandE[1][bandID])) - 13;
			int num = Inlines.VSHR32(bandE[0][bandID], shift);
			int num2 = Inlines.VSHR32(bandE[1][bandID], shift);
			int b = 1 + Inlines.celt_sqrt(1 + Inlines.MULT16_16(num, num) + Inlines.MULT16_16(num2, num2));
			int a = Inlines.DIV32_16(Inlines.SHL32(num, 14), b);
			int a2 = Inlines.DIV32_16(Inlines.SHL32(num2, 14), b);
			for (int i = 0; i < N; i++)
			{
				int b2 = *X[X_ptr + i];
				int b3 = *Y[Y_ptr + i];
				*X[X_ptr + i] = (int)Inlines.EXTRACT16(Inlines.SHR32(Inlines.MAC16_16(Inlines.MULT16_16(a, b2), a2, b3), 14));
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0002884C File Offset: 0x00026A4C
		private unsafe static void stereo_split(Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int N)
		{
			for (int i = 0; i < N; i++)
			{
				int num = Inlines.MULT16_16(23170, *X[X_ptr + i]);
				int num2 = Inlines.MULT16_16(23170, *Y[Y_ptr + i]);
				*X[X_ptr + i] = (int)Inlines.EXTRACT16(Inlines.SHR32(Inlines.ADD32(num, num2), 15));
				*Y[Y_ptr + i] = (int)Inlines.EXTRACT16(Inlines.SHR32(Inlines.SUB32(num2, num), 15));
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000288D0 File Offset: 0x00026AD0
		private unsafe static void stereo_merge(Span<int> X, Span<int> Y, int mid, int N)
		{
			int num;
			int num2;
			Kernels.dual_inner_prod(Y, X, Y, N, out num, out num2);
			num = Inlines.MULT16_32_Q15(mid, num);
			int num3 = Inlines.SHR16(mid, 1);
			int num4 = Inlines.MULT16_16(num3, num3) + num2 - 2 * num;
			int num5 = Inlines.MULT16_16(num3, num3) + num2 + 2 * num;
			if (num5 < 161061 || num4 < 161061)
			{
				X.Slice(0, N).CopyTo(Y);
				return;
			}
			int num6 = Inlines.celt_ilog2(num4) >> 1;
			int num7 = Inlines.celt_ilog2(num5) >> 1;
			int a = Inlines.celt_rsqrt_norm(Inlines.VSHR32(num4, num6 - 7 << 1));
			int a2 = Inlines.celt_rsqrt_norm(Inlines.VSHR32(num5, num7 - 7 << 1));
			if (num6 < 7)
			{
				num6 = 7;
			}
			if (num7 < 7)
			{
				num7 = 7;
			}
			for (int i = 0; i < N; i++)
			{
				int a3 = Inlines.MULT16_16_P15(mid, *X[i]);
				int b = *Y[i];
				*X[i] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MULT16_16(a, Inlines.SUB16(a3, b)), num6 + 1));
				*Y[i] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.MULT16_16(a2, Inlines.ADD16(a3, b)), num7 + 1));
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000289FC File Offset: 0x00026BFC
		internal static int spreading_decision(CeltMode m, int[][] X, ref int average, int last_decision, ref int hf_average, ref int tapset_decision, int update_hf, int end, int C, int M)
		{
			int num = 0;
			int num2 = 0;
			short[] eBands = m.eBands;
			int num3 = 0;
			if (M * (int)(eBands[end] - eBands[end - 1]) <= 8)
			{
				return 0;
			}
			int num4 = 0;
			do
			{
				for (int i = 0; i < end; i++)
				{
					int[] array = new int[3];
					int[] array2 = X[num4];
					int num5 = M * (int)eBands[i];
					int num6 = M * (int)(eBands[i + 1] - eBands[i]);
					if (num6 > 8)
					{
						for (int j = num5; j < num6 + num5; j++)
						{
							int num7 = Inlines.MULT16_16(Inlines.MULT16_16_Q15(array2[j], array2[j]), num6);
							if (num7 < 2048)
							{
								array[0]++;
							}
							if (num7 < 512)
							{
								array[1]++;
							}
							if (num7 < 128)
							{
								array[2]++;
							}
						}
						if (i > m.nbEBands - 4)
						{
							num3 += Inlines.celt_udiv(32 * (array[1] + array[0]), num6);
						}
						int num8 = ((2 * array[2] >= num6) + (2 * array[1] >= num6) + (2 * array[0] >= num6)) ? 1 : 0;
						num += num8 * 256;
						num2++;
					}
				}
			}
			while (++num4 < C);
			if (update_hf != 0)
			{
				if (num3 != 0)
				{
					num3 = Inlines.celt_udiv(num3, C * (4 - m.nbEBands + end));
				}
				hf_average = hf_average + num3 >> 1;
				num3 = hf_average;
				if (tapset_decision == 2)
				{
					num3 += 4;
				}
				else if (tapset_decision == 0)
				{
					num3 -= 4;
				}
				if (num3 > 22)
				{
					tapset_decision = 2;
				}
				else if (num3 > 18)
				{
					tapset_decision = 1;
				}
				else
				{
					tapset_decision = 0;
				}
			}
			num = Inlines.celt_udiv(num, num2);
			num = num + average >> 1;
			average = num;
			num = 3 * num + ((3 - last_decision << 7) + 64) + 2 >> 2;
			int result;
			if (num < 80)
			{
				result = 3;
			}
			else if (num < 256)
			{
				result = 2;
			}
			else if (num < 384)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00028BF8 File Offset: 0x00026DF8
		internal unsafe static void deinterleave_hadamard(Span<int> X, int X_ptr, int N0, int stride, int hadamard)
		{
			int num = N0 * stride;
			int[] array = new int[num];
			if (hadamard != 0)
			{
				int num2 = stride - 2;
				for (int i = 0; i < stride; i++)
				{
					for (int j = 0; j < N0; j++)
					{
						array[Tables.ordery_table[num2 + i] * N0 + j] = *X[j * stride + i + X_ptr];
					}
				}
			}
			else
			{
				for (int i = 0; i < stride; i++)
				{
					for (int j = 0; j < N0; j++)
					{
						array[i * N0 + j] = *X[j * stride + i + X_ptr];
					}
				}
			}
			MemoryExtensions.AsSpan<int>(array, 0, num).CopyTo(X.Slice(X_ptr));
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00028C9C File Offset: 0x00026E9C
		internal unsafe static void interleave_hadamard(Span<int> X, int X_ptr, int N0, int stride, int hadamard)
		{
			int num = N0 * stride;
			int[] array = new int[num];
			if (hadamard != 0)
			{
				int num2 = stride - 2;
				for (int i = 0; i < stride; i++)
				{
					for (int j = 0; j < N0; j++)
					{
						array[j * stride + i] = *X[Tables.ordery_table[num2 + i] * N0 + j + X_ptr];
					}
				}
			}
			else
			{
				for (int i = 0; i < stride; i++)
				{
					for (int j = 0; j < N0; j++)
					{
						array[j * stride + i] = *X[i * N0 + j + X_ptr];
					}
				}
			}
			MemoryExtensions.AsSpan<int>(array, 0, num).CopyTo(X.Slice(X_ptr));
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00028D40 File Offset: 0x00026F40
		internal unsafe static void haar1(Span<int> X, int X_ptr, int N0, int stride)
		{
			N0 >>= 1;
			for (int i = 0; i < stride; i++)
			{
				for (int j = 0; j < N0; j++)
				{
					int num = X_ptr + i + stride * 2 * j;
					int a = Inlines.MULT16_16(23170, *X[num]);
					int b = Inlines.MULT16_16(23170, *X[num + stride]);
					*X[num] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.ADD32(a, b), 15));
					*X[num + stride] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.SUB32(a, b), 15));
				}
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00028DE4 File Offset: 0x00026FE4
		internal static void haar1ZeroOffset(int[] X, int N0, int stride)
		{
			N0 >>= 1;
			for (int i = 0; i < stride; i++)
			{
				for (int j = 0; j < N0; j++)
				{
					int num = i + stride * 2 * j;
					int a = Inlines.MULT16_16(23170, X[num]);
					int b = Inlines.MULT16_16(23170, X[num + stride]);
					X[num] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.ADD32(a, b), 15));
					X[num + stride] = (int)Inlines.EXTRACT16(Inlines.PSHR32(Inlines.SUB32(a, b), 15));
				}
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00028E68 File Offset: 0x00027068
		internal static int compute_qn(int N, int b, int offset, int pulse_cap, int stereo)
		{
			short[] array = new short[]
			{
				16384,
				17866,
				19483,
				21247,
				23170,
				25267,
				27554,
				30048
			};
			int num = 2 * N - 1;
			if (stereo != 0 && N == 2)
			{
				num--;
			}
			int num2 = Inlines.celt_sudiv(b + num * offset, num);
			num2 = Inlines.IMIN(b - pulse_cap - 32, num2);
			num2 = Inlines.IMIN(64, num2);
			int num3;
			if (num2 < 4)
			{
				num3 = 1;
			}
			else
			{
				num3 = array[num2 & 7] >> 14 - (num2 >> 3);
				num3 = num3 + 1 >> 1 << 1;
			}
			return num3;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00028EDC File Offset: 0x000270DC
		internal static void compute_theta_decode(Bands.band_ctx ctx, Bands.split_ctx sctx, ReadOnlySpan<byte> encodedData, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int N, ref int b, int B, int B0, int LM, int stereo, ref int fill)
		{
			int num = 0;
			int inv = 0;
			CeltMode m = ctx.m;
			int i = ctx.i;
			int intensity = ctx.intensity;
			EntropyCoder ec = ctx.ec;
			int[][] bandE = ctx.bandE;
			int num2 = (int)m.logN[i] + LM * 8;
			int offset = (num2 >> 1) - ((stereo != 0 && N == 2) ? 16 : 4);
			int num3 = Bands.compute_qn(N, b, offset, num2, stereo);
			if (stereo != 0 && i >= intensity)
			{
				num3 = 1;
			}
			int num4 = (int)ec.tell_frac();
			if (num3 != 1)
			{
				if (stereo != 0 && N > 2)
				{
					int num5 = 3;
					int num6 = num3 / 2;
					uint ft = (uint)(num5 * (num6 + 1) + num6);
					int num7 = (int)ec.decode(ft);
					int num8;
					if (num7 < (num6 + 1) * num5)
					{
						num8 = num7 / num5;
					}
					else
					{
						num8 = num6 + 1 + (num7 - (num6 + 1) * num5);
					}
					ec.dec_update(encodedData, (uint)((num8 <= num6) ? (num5 * num8) : (num8 - 1 - num6 + (num6 + 1) * num5)), (uint)((num8 <= num6) ? (num5 * (num8 + 1)) : (num8 - num6 + (num6 + 1) * num5)), ft);
					num = num8;
				}
				else if (B0 > 1 || stereo != 0)
				{
					num = (int)ec.dec_uint(encodedData, (uint)(num3 + 1));
				}
				else
				{
					int num9 = ((num3 >> 1) + 1) * ((num3 >> 1) + 1);
					int num10 = (int)ec.decode((uint)num9);
					int num11;
					int num12;
					if (num10 < (num3 >> 1) * ((num3 >> 1) + 1) >> 1)
					{
						num = (int)(Inlines.isqrt32((uint)(8 * num10 + 1)) - 1U) >> 1;
						num11 = num + 1;
						num12 = num * (num + 1) >> 1;
					}
					else
					{
						num = (int)((long)(2 * (num3 + 1)) - (long)((ulong)Inlines.isqrt32((uint)(8 * (num9 - num10 - 1) + 1)))) >> 1;
						num11 = num3 + 1 - num;
						num12 = num9 - ((num3 + 1 - num) * (num3 + 2 - num) >> 1);
					}
					ec.dec_update(encodedData, (uint)num12, (uint)(num12 + num11), (uint)num9);
				}
				num = Inlines.celt_udiv(num * 16384, num3);
			}
			else if (stereo != 0)
			{
				if (b > 16 && ctx.remaining_bits > 16)
				{
					inv = ec.dec_bit_logp(encodedData, 2U);
				}
				else
				{
					inv = 0;
				}
				num = 0;
			}
			int num13 = (int)(ec.tell_frac() - (uint)num4);
			b -= num13;
			int num14;
			int num15;
			int delta;
			if (num == 0)
			{
				num14 = 32767;
				num15 = 0;
				fill &= (1 << B) - 1;
				delta = -16384;
			}
			else if (num == 16384)
			{
				num14 = 0;
				num15 = 32767;
				fill &= (1 << B) - 1 << B;
				delta = 16384;
			}
			else
			{
				num14 = Bands.bitexact_cos((int)((short)num));
				num15 = Bands.bitexact_cos((int)((short)(16384 - num)));
				delta = Inlines.FRAC_MUL16(N - 1 << 7, Bands.bitexact_log2tan(num15, num14));
			}
			sctx.inv = inv;
			sctx.imid = num14;
			sctx.iside = num15;
			sctx.delta = delta;
			sctx.itheta = num;
			sctx.qalloc = num13;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000291A8 File Offset: 0x000273A8
		internal unsafe static void compute_theta_encode(Bands.band_ctx ctx, Bands.split_ctx sctx, Span<byte> encodedData, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int N, ref int b, int B, int B0, int LM, int stereo, ref int fill)
		{
			int num = 0;
			CeltMode m = ctx.m;
			int i = ctx.i;
			int intensity = ctx.intensity;
			EntropyCoder ec = ctx.ec;
			int[][] bandE = ctx.bandE;
			int num2 = (int)m.logN[i] + LM * 8;
			int offset = (num2 >> 1) - ((stereo != 0 && N == 2) ? 16 : 4);
			int num3 = Bands.compute_qn(N, b, offset, num2, stereo);
			if (stereo != 0 && i >= intensity)
			{
				num3 = 1;
			}
			int num4 = VQ.stereo_itheta(X.Slice(X_ptr), Y.Slice(Y_ptr), stereo, N);
			int num5 = (int)ec.tell_frac();
			if (num3 != 1)
			{
				num4 = num4 * num3 + 8192 >> 14;
				if (stereo != 0 && N > 2)
				{
					int num6 = 3;
					int num7 = num4;
					int num8 = num3 / 2;
					uint ft = (uint)(num6 * (num8 + 1) + num8);
					ec.encode(encodedData, (uint)((num7 <= num8) ? (num6 * num7) : (num7 - 1 - num8 + (num8 + 1) * num6)), (uint)((num7 <= num8) ? (num6 * (num7 + 1)) : (num7 - num8 + (num8 + 1) * num6)), ft);
				}
				else if (B0 > 1 || stereo != 0)
				{
					ec.enc_uint(encodedData, (uint)num4, (uint)(num3 + 1));
				}
				else
				{
					int num9 = ((num3 >> 1) + 1) * ((num3 >> 1) + 1);
					int num10 = (num4 <= num3 >> 1) ? (num4 + 1) : (num3 + 1 - num4);
					int num11 = (num4 <= num3 >> 1) ? (num4 * (num4 + 1) >> 1) : (num9 - ((num3 + 1 - num4) * (num3 + 2 - num4) >> 1));
					ec.encode(encodedData, (uint)num11, (uint)(num11 + num10), (uint)num9);
				}
				num4 = Inlines.celt_udiv(num4 * 16384, num3);
				if (stereo != 0)
				{
					if (num4 == 0)
					{
						Bands.intensity_stereo(m, X, X_ptr, Y, Y_ptr, bandE, i, N);
					}
					else
					{
						Bands.stereo_split(X, X_ptr, Y, Y_ptr, N);
					}
				}
			}
			else if (stereo != 0)
			{
				num = ((num4 > 8192) ? 1 : 0);
				if (num != 0)
				{
					for (int j = 0; j < N; j++)
					{
						*Y[Y_ptr + j] = 0 - *Y[Y_ptr + j];
					}
				}
				Bands.intensity_stereo(m, X, X_ptr, Y, Y_ptr, bandE, i, N);
				if (b > 16 && ctx.remaining_bits > 16)
				{
					ec.enc_bit_logp(encodedData, num, 2U);
				}
				else
				{
					num = 0;
				}
				num4 = 0;
			}
			int num12 = (int)(ec.tell_frac() - (uint)num5);
			b -= num12;
			int num13;
			int num14;
			int delta;
			if (num4 == 0)
			{
				num13 = 32767;
				num14 = 0;
				fill &= (1 << B) - 1;
				delta = -16384;
			}
			else if (num4 == 16384)
			{
				num13 = 0;
				num14 = 32767;
				fill &= (1 << B) - 1 << B;
				delta = 16384;
			}
			else
			{
				num13 = Bands.bitexact_cos((int)((short)num4));
				num14 = Bands.bitexact_cos((int)((short)(16384 - num4)));
				delta = Inlines.FRAC_MUL16(N - 1 << 7, Bands.bitexact_log2tan(num14, num13));
			}
			sctx.inv = num;
			sctx.imid = num13;
			sctx.iside = num14;
			sctx.delta = delta;
			sctx.itheta = num4;
			sctx.qalloc = num12;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000294A4 File Offset: 0x000276A4
		internal unsafe static uint quant_band_n1_encode(Bands.band_ctx ctx, Span<byte> encodedData, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int b, Span<int> lowband_out, int lowband_out_ptr)
		{
			int num = 0;
			Span<int> span = X;
			int num2 = X_ptr;
			EntropyCoder ec = ctx.ec;
			int num3 = (!Y.IsEmpty) ? 1 : 0;
			int num4 = 0;
			do
			{
				int num5 = 0;
				if (ctx.remaining_bits >= 8)
				{
					num5 = ((*span[num2] < 0) ? 1 : 0);
					ec.enc_bits(encodedData, (uint)num5, 1U);
					ctx.remaining_bits -= 8;
					b -= 8;
				}
				if (num != 0)
				{
					*span[num2] = ((num5 != 0) ? -16384 : 16384);
				}
				span = Y;
				num2 = Y_ptr;
			}
			while (++num4 < 1 + num3);
			if (!lowband_out.IsEmpty)
			{
				*lowband_out[lowband_out_ptr] = Inlines.SHR16(*X[X_ptr], 4);
			}
			return 1U;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00029558 File Offset: 0x00027758
		internal unsafe static uint quant_band_n1_decode(Bands.band_ctx ctx, ReadOnlySpan<byte> encodedData, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int b, Span<int> lowband_out, int lowband_out_ptr)
		{
			int num = 1;
			Span<int> span = X;
			int num2 = X_ptr;
			EntropyCoder ec = ctx.ec;
			int num3 = (!Y.IsEmpty) ? 1 : 0;
			int num4 = 0;
			do
			{
				int num5 = 0;
				if (ctx.remaining_bits >= 8)
				{
					num5 = (int)ec.dec_bits(encodedData, 1U);
					ctx.remaining_bits -= 8;
					b -= 8;
				}
				if (num != 0)
				{
					*span[num2] = ((num5 != 0) ? -16384 : 16384);
				}
				span = Y;
				num2 = Y_ptr;
			}
			while (++num4 < 1 + num3);
			if (!lowband_out.IsEmpty)
			{
				*lowband_out[lowband_out_ptr] = Inlines.SHR16(*X[X_ptr], 4);
			}
			return 1U;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00029600 File Offset: 0x00027800
		internal unsafe static uint quant_partition_encode(Bands.band_ctx ctx, Span<byte> encodedData, Span<int> X, int X_ptr, int N, int b, int B, Span<int> lowband, int lowband_ptr, int LM, int gain, int fill)
		{
			int num = B;
			uint num2 = 0U;
			int num3 = 0;
			CeltMode m = ctx.m;
			int i = ctx.i;
			int spread = ctx.spread;
			EntropyCoder ec = ctx.ec;
			byte[] bits = m.cache.bits;
			int num4 = (int)m.cache.index[(LM + 1) * m.nbEBands + i];
			if (LM != -1 && b > (int)(bits[num4 + (int)bits[num4]] + 12) && N > 2)
			{
				Bands.split_ctx split_ctx = new Bands.split_ctx();
				int lowband_ptr2 = 0;
				N >>= 1;
				int num5 = X_ptr + N;
				LM--;
				if (B == 1)
				{
					fill = ((fill & 1) | fill << 1);
				}
				B = B + 1 >> 1;
				Bands.compute_theta_encode(ctx, split_ctx, encodedData, X, X_ptr, X, num5, N, ref b, B, num, LM, 0, ref fill);
				int imid = split_ctx.imid;
				int iside = split_ctx.iside;
				int num6 = split_ctx.delta;
				int itheta = split_ctx.itheta;
				int qalloc = split_ctx.qalloc;
				int b2 = imid;
				int b3 = iside;
				if (num > 1 && (itheta & 16383) != 0)
				{
					if (itheta > 8192)
					{
						num6 -= num6 >> 4 - LM;
					}
					else
					{
						num6 = Inlines.IMIN(0, num6 + (N << 3 >> 5 - LM));
					}
				}
				int num7 = Inlines.IMAX(0, Inlines.IMIN(b, (b - num6) / 2));
				int num8 = b - num7;
				ctx.remaining_bits -= qalloc;
				if (!lowband.IsEmpty)
				{
					lowband_ptr2 = lowband_ptr + N;
				}
				int num9 = ctx.remaining_bits;
				if (num7 >= num8)
				{
					num2 = Bands.quant_partition_encode(ctx, encodedData, X, X_ptr, N, num7, B, lowband, lowband_ptr, LM, Inlines.MULT16_16_P15(gain, b2), fill);
					num9 = num7 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 0)
					{
						num8 += num9 - 24;
					}
					num2 |= Bands.quant_partition_encode(ctx, encodedData, X, num5, N, num8, B, lowband, lowband_ptr2, LM, Inlines.MULT16_16_P15(gain, b3), fill >> B) << (num >> 1);
				}
				else
				{
					num2 = Bands.quant_partition_encode(ctx, encodedData, X, num5, N, num8, B, lowband, lowband_ptr2, LM, Inlines.MULT16_16_P15(gain, b3), fill >> B) << (num >> 1);
					num9 = num8 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 16384)
					{
						num7 += num9 - 24;
					}
					num2 |= Bands.quant_partition_encode(ctx, encodedData, X, X_ptr, N, num7, B, lowband, lowband_ptr, LM, Inlines.MULT16_16_P15(gain, b2), fill);
				}
			}
			else
			{
				int num10 = Rate.bits2pulses(m, i, LM, b);
				int num11 = Rate.pulses2bits(m, i, LM, num10);
				ctx.remaining_bits -= num11;
				while (ctx.remaining_bits < 0 && num10 > 0)
				{
					ctx.remaining_bits += num11;
					num10--;
					num11 = Rate.pulses2bits(m, i, LM, num10);
					ctx.remaining_bits -= num11;
				}
				if (num10 != 0)
				{
					int k = Rate.get_pulses(num10);
					num2 = VQ.alg_quant(X, X_ptr, N, k, spread, B, ec, encodedData);
				}
				else if (num3 != 0)
				{
					uint num12 = (uint)((uint)1L << B) - 1U;
					fill &= (int)num12;
					if (fill == 0)
					{
						Arrays.MemSetWithOffset<int>(X, 0, X_ptr, N);
					}
					else
					{
						if (lowband.IsEmpty)
						{
							for (int j = 0; j < N; j++)
							{
								ctx.seed = Bands.celt_lcg_rand(ctx.seed);
								*X[X_ptr + j] = (int)ctx.seed >> 20;
							}
							num2 = num12;
						}
						else
						{
							for (int j = 0; j < N; j++)
							{
								ctx.seed = Bands.celt_lcg_rand(ctx.seed);
								int num13 = 4;
								num13 = (((ctx.seed & 32768U) != 0U) ? num13 : (0 - num13));
								*X[X_ptr + j] = *lowband[lowband_ptr + j] + num13;
							}
							num2 = (uint)fill;
						}
						VQ.renormalise_vector(X.Slice(X_ptr), N, gain);
					}
				}
			}
			return num2;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00029A04 File Offset: 0x00027C04
		internal unsafe static uint quant_partition_decode(Bands.band_ctx ctx, ReadOnlySpan<byte> encodedData, Span<int> X, int X_ptr, int N, int b, int B, Span<int> lowband, int lowband_ptr, int LM, int gain, int fill)
		{
			int num = B;
			uint num2 = 0U;
			int num3 = 1;
			CeltMode m = ctx.m;
			int i = ctx.i;
			int spread = ctx.spread;
			EntropyCoder ec = ctx.ec;
			byte[] bits = m.cache.bits;
			int num4 = (int)m.cache.index[(LM + 1) * m.nbEBands + i];
			if (LM != -1 && b > (int)(bits[num4 + (int)bits[num4]] + 12) && N > 2)
			{
				Bands.split_ctx split_ctx = new Bands.split_ctx();
				int lowband_ptr2 = 0;
				N >>= 1;
				int num5 = X_ptr + N;
				LM--;
				if (B == 1)
				{
					fill = ((fill & 1) | fill << 1);
				}
				B = B + 1 >> 1;
				Bands.compute_theta_decode(ctx, split_ctx, encodedData, X, X_ptr, X, num5, N, ref b, B, num, LM, 0, ref fill);
				int imid = split_ctx.imid;
				int iside = split_ctx.iside;
				int num6 = split_ctx.delta;
				int itheta = split_ctx.itheta;
				int qalloc = split_ctx.qalloc;
				int b2 = imid;
				int b3 = iside;
				if (num > 1 && (itheta & 16383) != 0)
				{
					if (itheta > 8192)
					{
						num6 -= num6 >> 4 - LM;
					}
					else
					{
						num6 = Inlines.IMIN(0, num6 + (N << 3 >> 5 - LM));
					}
				}
				int num7 = Inlines.IMAX(0, Inlines.IMIN(b, (b - num6) / 2));
				int num8 = b - num7;
				ctx.remaining_bits -= qalloc;
				if (!lowband.IsEmpty)
				{
					lowband_ptr2 = lowband_ptr + N;
				}
				int num9 = ctx.remaining_bits;
				if (num7 >= num8)
				{
					num2 = Bands.quant_partition_decode(ctx, encodedData, X, X_ptr, N, num7, B, lowband, lowband_ptr, LM, Inlines.MULT16_16_P15(gain, b2), fill);
					num9 = num7 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 0)
					{
						num8 += num9 - 24;
					}
					num2 |= Bands.quant_partition_decode(ctx, encodedData, X, num5, N, num8, B, lowband, lowband_ptr2, LM, Inlines.MULT16_16_P15(gain, b3), fill >> B) << (num >> 1);
				}
				else
				{
					num2 = Bands.quant_partition_decode(ctx, encodedData, X, num5, N, num8, B, lowband, lowband_ptr2, LM, Inlines.MULT16_16_P15(gain, b3), fill >> B) << (num >> 1);
					num9 = num8 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 16384)
					{
						num7 += num9 - 24;
					}
					num2 |= Bands.quant_partition_decode(ctx, encodedData, X, X_ptr, N, num7, B, lowband, lowband_ptr, LM, Inlines.MULT16_16_P15(gain, b2), fill);
				}
			}
			else
			{
				int num10 = Rate.bits2pulses(m, i, LM, b);
				int num11 = Rate.pulses2bits(m, i, LM, num10);
				ctx.remaining_bits -= num11;
				while (ctx.remaining_bits < 0 && num10 > 0)
				{
					ctx.remaining_bits += num11;
					num10--;
					num11 = Rate.pulses2bits(m, i, LM, num10);
					ctx.remaining_bits -= num11;
				}
				if (num10 != 0)
				{
					int k = Rate.get_pulses(num10);
					num2 = VQ.alg_unquant(X, X_ptr, N, k, spread, B, ec, encodedData, gain);
				}
				else if (num3 != 0)
				{
					uint num12 = (uint)((uint)1L << B) - 1U;
					fill &= (int)num12;
					if (fill == 0)
					{
						Arrays.MemSetWithOffset<int>(X, 0, X_ptr, N);
					}
					else
					{
						if (lowband.IsEmpty)
						{
							for (int j = 0; j < N; j++)
							{
								ctx.seed = Bands.celt_lcg_rand(ctx.seed);
								*X[X_ptr + j] = (int)ctx.seed >> 20;
							}
							num2 = num12;
						}
						else
						{
							for (int j = 0; j < N; j++)
							{
								ctx.seed = Bands.celt_lcg_rand(ctx.seed);
								int num13 = 4;
								num13 = (((ctx.seed & 32768U) != 0U) ? num13 : (0 - num13));
								*X[X_ptr + j] = *lowband[lowband_ptr + j] + num13;
							}
							num2 = (uint)fill;
						}
						VQ.renormalise_vector(X.Slice(X_ptr), N, gain);
					}
				}
			}
			return num2;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00029E0C File Offset: 0x0002800C
		internal unsafe static uint quant_band_encode(Bands.band_ctx ctx, Span<byte> encodedData, Span<int> X, int X_ptr, int N, int b, int B, Span<int> lowband, int lowband_ptr, int LM, Span<int> lowband_out, int lowband_out_ptr, int gain, Span<int> lowband_scratch, int lowband_scratch_ptr, int fill)
		{
			int num = B;
			int num2 = 0;
			int num3 = 0;
			int num4 = 1;
			int num5 = ctx.tf_change;
			int hadamard = (num == 1) ? 1 : 0;
			int num6 = Inlines.celt_udiv(N, B);
			if (N == 1)
			{
				return Bands.quant_band_n1_encode(ctx, encodedData, X, X_ptr, null, 0, b, lowband_out, lowband_out_ptr);
			}
			if (num5 > 0)
			{
				num3 = num5;
			}
			if (!lowband_scratch.IsEmpty && !lowband.IsEmpty && (num3 != 0 || ((num6 & 1) == 0 && num5 < 0) || num > 1))
			{
				lowband.Slice(lowband_ptr, N).CopyTo(lowband_scratch.Slice(lowband_scratch_ptr, N));
				lowband = lowband_scratch;
				lowband_ptr = lowband_scratch_ptr;
			}
			for (int i = 0; i < num3; i++)
			{
				Bands.haar1(X, X_ptr, N >> i, 1 << i);
				if (!lowband.IsEmpty)
				{
					Bands.haar1(lowband, lowband_ptr, N >> i, 1 << i);
				}
				fill = ((int)Bands.bit_interleave_table[fill & 15] | (int)Bands.bit_interleave_table[fill >> 4] << 2);
			}
			B >>= num3;
			num6 <<= num3;
			while ((num6 & 1) == 0 && num5 < 0)
			{
				Bands.haar1(X, X_ptr, num6, B);
				if (!lowband.IsEmpty)
				{
					Bands.haar1(lowband, lowband_ptr, num6, B);
				}
				fill |= fill << B;
				B <<= 1;
				num6 >>= 1;
				num2++;
				num5++;
			}
			num = B;
			int num7 = num6;
			if (num > 1)
			{
				Bands.deinterleave_hadamard(X, X_ptr, num6 >> num3, num << num3, hadamard);
				if (!lowband.IsEmpty)
				{
					Bands.deinterleave_hadamard(lowband, lowband_ptr, num6 >> num3, num << num3, hadamard);
				}
			}
			uint num8 = Bands.quant_partition_encode(ctx, encodedData, X, X_ptr, N, b, B, lowband, lowband_ptr, LM, gain, fill);
			if (num4 != 0)
			{
				if (num > 1)
				{
					Bands.interleave_hadamard(X, X_ptr, num6 >> num3, num << num3, hadamard);
				}
				num6 = num7;
				B = num;
				for (int i = 0; i < num2; i++)
				{
					B >>= 1;
					num6 <<= 1;
					num8 |= num8 >> B;
					Bands.haar1(X, X_ptr, num6, B);
				}
				for (int i = 0; i < num3; i++)
				{
					num8 = (uint)Bands.bit_deinterleave_table[(int)num8];
					Bands.haar1(X, X_ptr, N >> i, 1 << i);
				}
				B <<= num3;
				if (!lowband_out.IsEmpty)
				{
					int a = Inlines.celt_sqrt(Inlines.SHL32(N, 22));
					for (int j = 0; j < N; j++)
					{
						*lowband_out[lowband_out_ptr + j] = Inlines.MULT16_16_Q15(a, *X[X_ptr + j]);
					}
				}
				num8 &= (1U << B) - 1U;
			}
			return num8;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0002A0C0 File Offset: 0x000282C0
		internal unsafe static uint quant_band_decode(Bands.band_ctx ctx, ReadOnlySpan<byte> encodedData, Span<int> X, int X_ptr, int N, int b, int B, Span<int> lowband, int lowband_ptr, int LM, Span<int> lowband_out, int lowband_out_ptr, int gain, Span<int> lowband_scratch, int lowband_scratch_ptr, int fill)
		{
			int num = B;
			int num2 = 0;
			int num3 = 0;
			int num4 = 1;
			int num5 = ctx.tf_change;
			int hadamard = (num == 1) ? 1 : 0;
			int num6 = Inlines.celt_udiv(N, B);
			if (N == 1)
			{
				return Bands.quant_band_n1_decode(ctx, encodedData, X, X_ptr, null, 0, b, lowband_out, lowband_out_ptr);
			}
			if (num5 > 0)
			{
				num3 = num5;
			}
			if (!lowband_scratch.IsEmpty && !lowband.IsEmpty && (num3 != 0 || ((num6 & 1) == 0 && num5 < 0) || num > 1))
			{
				lowband.Slice(lowband_ptr, N).CopyTo(lowband_scratch.Slice(lowband_scratch_ptr, N));
				lowband = lowband_scratch;
				lowband_ptr = lowband_scratch_ptr;
			}
			for (int i = 0; i < num3; i++)
			{
				if (!lowband.IsEmpty)
				{
					Bands.haar1(lowband, lowband_ptr, N >> i, 1 << i);
				}
				fill = ((int)Bands.bit_interleave_table[fill & 15] | (int)Bands.bit_interleave_table[fill >> 4] << 2);
			}
			B >>= num3;
			num6 <<= num3;
			while ((num6 & 1) == 0 && num5 < 0)
			{
				if (!lowband.IsEmpty)
				{
					Bands.haar1(lowband, lowband_ptr, num6, B);
				}
				fill |= fill << B;
				B <<= 1;
				num6 >>= 1;
				num2++;
				num5++;
			}
			num = B;
			int num7 = num6;
			if (num > 1 && !lowband.IsEmpty)
			{
				Bands.deinterleave_hadamard(lowband, lowband_ptr, num6 >> num3, num << num3, hadamard);
			}
			uint num8 = Bands.quant_partition_decode(ctx, encodedData, X, X_ptr, N, b, B, lowband, lowband_ptr, LM, gain, fill);
			if (num4 != 0)
			{
				if (num > 1)
				{
					Bands.interleave_hadamard(X, X_ptr, num6 >> num3, num << num3, hadamard);
				}
				num6 = num7;
				B = num;
				for (int i = 0; i < num2; i++)
				{
					B >>= 1;
					num6 <<= 1;
					num8 |= num8 >> B;
					Bands.haar1(X, X_ptr, num6, B);
				}
				for (int i = 0; i < num3; i++)
				{
					num8 = (uint)Bands.bit_deinterleave_table[(int)num8];
					Bands.haar1(X, X_ptr, N >> i, 1 << i);
				}
				B <<= num3;
				if (!lowband_out.IsEmpty)
				{
					int a = Inlines.celt_sqrt(Inlines.SHL32(N, 22));
					for (int j = 0; j < N; j++)
					{
						*lowband_out[lowband_out_ptr + j] = Inlines.MULT16_16_Q15(a, *X[X_ptr + j]);
					}
				}
				num8 &= (1U << B) - 1U;
			}
			return num8;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0002A33C File Offset: 0x0002853C
		internal unsafe static uint quant_band_stereo_encode(Bands.band_ctx ctx, Span<byte> encodedData, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int N, int b, int B, Span<int> lowband, int lowband_ptr, int LM, Span<int> lowband_out, int lowband_out_ptr, Span<int> lowband_scratch, int lowband_scratch_ptr, int fill)
		{
			int num = 0;
			Bands.split_ctx split_ctx = new Bands.split_ctx();
			EntropyCoder ec = ctx.ec;
			if (N == 1)
			{
				return Bands.quant_band_n1_encode(ctx, encodedData, X, X_ptr, Y, Y_ptr, b, lowband_out, lowband_out_ptr);
			}
			int fill2 = fill;
			Bands.compute_theta_encode(ctx, split_ctx, encodedData, X, X_ptr, Y, Y_ptr, N, ref b, B, B, LM, 1, ref fill);
			int inv = split_ctx.inv;
			int imid = split_ctx.imid;
			int iside = split_ctx.iside;
			int delta = split_ctx.delta;
			int itheta = split_ctx.itheta;
			int qalloc = split_ctx.qalloc;
			int num2 = imid;
			int num3 = iside;
			uint num8;
			if (N == 2)
			{
				int num4 = 0;
				int num5 = b;
				int num6 = 0;
				if (itheta != 0 && itheta != 16384)
				{
					num6 = 8;
				}
				num5 -= num6;
				bool flag = itheta > 8192;
				ctx.remaining_bits -= qalloc + num6;
				Span<int> x;
				int num7;
				Span<int> span;
				if (flag)
				{
					x = Y;
					num7 = Y_ptr;
					span = X;
				}
				else
				{
					x = X;
					num7 = X_ptr;
					span = Y;
				}
				if (num6 != 0)
				{
					num4 = ((*x[num7] * *span[Y_ptr + 1] - *x[num7 + 1] * *span[Y_ptr] < 0) ? 1 : 0);
					ec.enc_bits(encodedData, (uint)num4, 1U);
				}
				num4 = 1 - 2 * num4;
				num8 = Bands.quant_band_encode(ctx, encodedData, x, num7, N, num5, B, lowband, lowband_ptr, LM, lowband_out, lowband_out_ptr, 32767, lowband_scratch, lowband_scratch_ptr, fill2);
				*span[Y_ptr] = (0 - num4) * *x[num7 + 1];
				*span[Y_ptr + 1] = num4 * *x[num7];
				if (num != 0)
				{
					*X[X_ptr] = Inlines.MULT16_16_Q15(num2, *X[X_ptr]);
					*X[X_ptr + 1] = Inlines.MULT16_16_Q15(num2, *X[X_ptr + 1]);
					*Y[Y_ptr] = Inlines.MULT16_16_Q15(num3, *Y[Y_ptr]);
					*Y[Y_ptr + 1] = Inlines.MULT16_16_Q15(num3, *Y[Y_ptr + 1]);
					int a = *X[X_ptr];
					*X[X_ptr] = Inlines.SUB16(a, *Y[Y_ptr]);
					*Y[Y_ptr] = Inlines.ADD16(a, *Y[Y_ptr]);
					a = *X[X_ptr + 1];
					*X[X_ptr + 1] = Inlines.SUB16(a, *Y[Y_ptr + 1]);
					*Y[Y_ptr + 1] = Inlines.ADD16(a, *Y[Y_ptr + 1]);
				}
			}
			else
			{
				int num5 = Inlines.IMAX(0, Inlines.IMIN(b, (b - delta) / 2));
				int num6 = b - num5;
				ctx.remaining_bits -= qalloc;
				int num9 = ctx.remaining_bits;
				if (num5 >= num6)
				{
					num8 = Bands.quant_band_encode(ctx, encodedData, X, X_ptr, N, num5, B, lowband, lowband_ptr, LM, lowband_out, lowband_out_ptr, 32767, lowband_scratch, lowband_scratch_ptr, fill);
					num9 = num5 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 0)
					{
						num6 += num9 - 24;
					}
					num8 |= Bands.quant_band_encode(ctx, encodedData, Y, Y_ptr, N, num6, B, null, 0, LM, null, 0, num3, null, 0, fill >> B);
				}
				else
				{
					num8 = Bands.quant_band_encode(ctx, encodedData, Y, Y_ptr, N, num6, B, null, 0, LM, null, 0, num3, null, 0, fill >> B);
					num9 = num6 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 16384)
					{
						num5 += num9 - 24;
					}
					num8 |= Bands.quant_band_encode(ctx, encodedData, X, X_ptr, N, num5, B, lowband, lowband_ptr, LM, lowband_out, lowband_out_ptr, 32767, lowband_scratch, lowband_scratch_ptr, fill);
				}
			}
			if (num != 0)
			{
				if (N != 2)
				{
					Bands.stereo_merge(X.Slice(X_ptr), Y.Slice(Y_ptr), num2, N);
				}
				if (inv != 0)
				{
					for (int i = Y_ptr; i < N + Y_ptr; i++)
					{
						*Y[i] = (int)((short)(0 - *Y[i]));
					}
				}
			}
			return num8;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0002A770 File Offset: 0x00028970
		internal unsafe static uint quant_band_stereo_decode(Bands.band_ctx ctx, ReadOnlySpan<byte> encodedData, Span<int> X, int X_ptr, Span<int> Y, int Y_ptr, int N, int b, int B, Span<int> lowband, int lowband_ptr, int LM, Span<int> lowband_out, int lowband_out_ptr, Span<int> lowband_scratch, int lowband_scratch_ptr, int fill)
		{
			int num = 1;
			Bands.split_ctx split_ctx = new Bands.split_ctx();
			EntropyCoder ec = ctx.ec;
			if (N == 1)
			{
				return Bands.quant_band_n1_decode(ctx, encodedData, X, X_ptr, Y, Y_ptr, b, lowband_out, lowband_out_ptr);
			}
			int fill2 = fill;
			Bands.compute_theta_decode(ctx, split_ctx, encodedData, X, X_ptr, Y, Y_ptr, N, ref b, B, B, LM, 1, ref fill);
			int inv = split_ctx.inv;
			int imid = split_ctx.imid;
			int iside = split_ctx.iside;
			int delta = split_ctx.delta;
			int itheta = split_ctx.itheta;
			int qalloc = split_ctx.qalloc;
			int num2 = imid;
			int num3 = iside;
			uint num8;
			if (N == 2)
			{
				int num4 = 0;
				int num5 = b;
				int num6 = 0;
				if (itheta != 0 && itheta != 16384)
				{
					num6 = 8;
				}
				num5 -= num6;
				bool flag = itheta > 8192;
				ctx.remaining_bits -= qalloc + num6;
				Span<int> x;
				int num7;
				Span<int> span;
				if (flag)
				{
					x = Y;
					num7 = Y_ptr;
					span = X;
				}
				else
				{
					x = X;
					num7 = X_ptr;
					span = Y;
				}
				if (num6 != 0)
				{
					num4 = (int)ec.dec_bits(encodedData, 1U);
				}
				num4 = 1 - 2 * num4;
				num8 = Bands.quant_band_decode(ctx, encodedData, x, num7, N, num5, B, lowband, lowband_ptr, LM, lowband_out, lowband_out_ptr, 32767, lowband_scratch, lowband_scratch_ptr, fill2);
				*span[Y_ptr] = (0 - num4) * *x[num7 + 1];
				*span[Y_ptr + 1] = num4 * *x[num7];
				if (num != 0)
				{
					*X[X_ptr] = Inlines.MULT16_16_Q15(num2, *X[X_ptr]);
					*X[X_ptr + 1] = Inlines.MULT16_16_Q15(num2, *X[X_ptr + 1]);
					*Y[Y_ptr] = Inlines.MULT16_16_Q15(num3, *Y[Y_ptr]);
					*Y[Y_ptr + 1] = Inlines.MULT16_16_Q15(num3, *Y[Y_ptr + 1]);
					int a = *X[X_ptr];
					*X[X_ptr] = Inlines.SUB16(a, *Y[Y_ptr]);
					*Y[Y_ptr] = Inlines.ADD16(a, *Y[Y_ptr]);
					a = *X[X_ptr + 1];
					*X[X_ptr + 1] = Inlines.SUB16(a, *Y[Y_ptr + 1]);
					*Y[Y_ptr + 1] = Inlines.ADD16(a, *Y[Y_ptr + 1]);
				}
			}
			else
			{
				int num5 = Inlines.IMAX(0, Inlines.IMIN(b, (b - delta) / 2));
				int num6 = b - num5;
				ctx.remaining_bits -= qalloc;
				int num9 = ctx.remaining_bits;
				if (num5 >= num6)
				{
					num8 = Bands.quant_band_decode(ctx, encodedData, X, X_ptr, N, num5, B, lowband, lowband_ptr, LM, lowband_out, lowband_out_ptr, 32767, lowband_scratch, lowband_scratch_ptr, fill);
					num9 = num5 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 0)
					{
						num6 += num9 - 24;
					}
					num8 |= Bands.quant_band_decode(ctx, encodedData, Y, Y_ptr, N, num6, B, null, 0, LM, null, 0, num3, null, 0, fill >> B);
				}
				else
				{
					num8 = Bands.quant_band_decode(ctx, encodedData, Y, Y_ptr, N, num6, B, null, 0, LM, null, 0, num3, null, 0, fill >> B);
					num9 = num6 - (num9 - ctx.remaining_bits);
					if (num9 > 24 && itheta != 16384)
					{
						num5 += num9 - 24;
					}
					num8 |= Bands.quant_band_decode(ctx, encodedData, X, X_ptr, N, num5, B, lowband, lowband_ptr, LM, lowband_out, lowband_out_ptr, 32767, lowband_scratch, lowband_scratch_ptr, fill);
				}
			}
			if (num != 0)
			{
				if (N != 2)
				{
					Bands.stereo_merge(X.Slice(X_ptr), Y.Slice(Y_ptr), num2, N);
				}
				if (inv != 0)
				{
					for (int i = Y_ptr; i < N + Y_ptr; i++)
					{
						*Y[i] = (int)((short)(0 - *Y[i]));
					}
				}
			}
			return num8;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0002AB70 File Offset: 0x00028D70
		internal static void quant_all_bands_encode(int encode, CeltMode m, int start, int end, int[] X_, int[] Y_, byte[] collapse_masks, int[][] bandE, int[] pulses, int shortBlocks, int spread, int dual_stereo, int intensity, int[] tf_res, int total_bits, int balance, EntropyCoder ec, Span<byte> encodedData, int LM, int codedBands, ref uint seed)
		{
			short[] eBands = m.eBands;
			int num = 1;
			int num2 = (Y_ != null) ? 2 : 1;
			int num3 = (encode == 0) ? 1 : 0;
			Bands.band_ctx band_ctx = new Bands.band_ctx();
			int num4 = 1 << LM;
			int num5 = (shortBlocks != 0) ? num4 : 1;
			int num6 = num4 * (int)eBands[start];
			int[] array = new int[num2 * (num4 * (int)eBands[m.nbEBands - 1] - num6)];
			int num7 = num4 * (int)eBands[m.nbEBands - 1] - num6;
			int[] array2 = X_;
			int lowband_scratch_ptr = num4 * (int)eBands[m.nbEBands - 1];
			int num8 = 0;
			band_ctx.bandE = bandE;
			band_ctx.ec = ec;
			band_ctx.encode = encode;
			band_ctx.intensity = intensity;
			band_ctx.m = m;
			band_ctx.seed = seed;
			band_ctx.spread = spread;
			for (int i = start; i < end; i++)
			{
				int num9 = -1;
				int num10 = 0;
				band_ctx.i = i;
				int num11 = (i == end - 1) ? 1 : 0;
				int[] array3 = X_;
				int x_ptr = num4 * (int)eBands[i];
				int[] array4;
				if (Y_ != null)
				{
					array4 = Y_;
					num10 = num4 * (int)eBands[i];
				}
				else
				{
					array4 = null;
				}
				int num12 = num4 * (int)eBands[i + 1] - num4 * (int)eBands[i];
				int num13 = (int)ec.tell_frac();
				if (i != start)
				{
					balance -= num13;
				}
				int num14 = total_bits - num13 - 1;
				band_ctx.remaining_bits = num14;
				int num16;
				if (i <= codedBands - 1)
				{
					int num15 = Inlines.celt_sudiv(balance, Inlines.IMIN(3, codedBands - i));
					num16 = Inlines.IMAX(0, Inlines.IMIN(16383, Inlines.IMIN(num14 + 1, pulses[i] + num15)));
				}
				else
				{
					num16 = 0;
				}
				if (num3 != 0 && num4 * (int)eBands[i] - num12 >= num4 * (int)eBands[start] && (num != 0 || num8 == 0))
				{
					num8 = i;
				}
				int num17 = tf_res[i];
				band_ctx.tf_change = num17;
				if (i >= m.effEBands)
				{
					array3 = array;
					x_ptr = 0;
					if (Y_ != null)
					{
						array4 = array;
						num10 = 0;
					}
					array2 = null;
				}
				if (i == end - 1)
				{
					array2 = null;
				}
				uint num21;
				uint num20;
				if (num8 != 0 && (spread != 3 || num5 > 1 || num17 < 0))
				{
					num9 = Inlines.IMAX(0, num4 * (int)eBands[num8] - num6 - num12);
					int num18 = num8;
					while (num4 * (int)eBands[--num18] > num9 + num6)
					{
					}
					int num19 = num8 - 1;
					while (num4 * (int)eBands[++num19] < num9 + num6 + num12)
					{
					}
					num20 = (num21 = 0U);
					int num22 = num18;
					do
					{
						num21 |= (uint)collapse_masks[num22 * num2];
						num20 |= (uint)collapse_masks[num22 * num2 + num2 - 1];
					}
					while (++num22 < num19);
				}
				else
				{
					num20 = (num21 = (1U << num5) - 1U);
				}
				if (dual_stereo != 0 && i == intensity)
				{
					dual_stereo = 0;
					if (num3 != 0)
					{
						for (int j = 0; j < num4 * (int)eBands[i] - num6; j++)
						{
							array[j] = Inlines.HALF32(array[j] + array[num7 + j]);
						}
					}
				}
				if (dual_stereo != 0)
				{
					num21 = Bands.quant_band_encode(band_ctx, encodedData, array3, x_ptr, num12, num16 / 2, num5, (num9 != -1) ? array : null, num9, LM, (num11 != 0) ? null : array, num4 * (int)eBands[i] - num6, 32767, array2, lowband_scratch_ptr, (int)num21);
					num20 = Bands.quant_band_encode(band_ctx, encodedData, array4, num10, num12, num16 / 2, num5, (num9 != -1) ? array : null, num7 + num9, LM, (num11 != 0) ? null : array, num7 + (num4 * (int)eBands[i] - num6), 32767, array2, lowband_scratch_ptr, (int)num20);
				}
				else
				{
					if (array4 != null)
					{
						num21 = Bands.quant_band_stereo_encode(band_ctx, encodedData, array3, x_ptr, array4, num10, num12, num16, num5, (num9 != -1) ? array : null, num9, LM, (num11 != 0) ? null : array, num4 * (int)eBands[i] - num6, array2, lowband_scratch_ptr, (int)(num21 | num20));
					}
					else
					{
						num21 = Bands.quant_band_encode(band_ctx, encodedData, array3, x_ptr, num12, num16, num5, (num9 != -1) ? array : null, num9, LM, (num11 != 0) ? null : array, num4 * (int)eBands[i] - num6, 32767, array2, lowband_scratch_ptr, (int)(num21 | num20));
					}
					num20 = num21;
				}
				collapse_masks[i * num2] = (byte)(num21 & 255U);
				collapse_masks[i * num2 + num2 - 1] = (byte)(num20 & 255U);
				balance += pulses[i] + num13;
				num = ((num16 > num12 << 3) ? 1 : 0);
			}
			seed = band_ctx.seed;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0002AFE4 File Offset: 0x000291E4
		internal static void quant_all_bands_decode(int encode, CeltMode m, int start, int end, int[] X_, int[] Y_, byte[] collapse_masks, int[][] bandE, int[] pulses, int shortBlocks, int spread, int dual_stereo, int intensity, int[] tf_res, int total_bits, int balance, EntropyCoder ec, ReadOnlySpan<byte> encodedData, int LM, int codedBands, ref uint seed)
		{
			short[] eBands = m.eBands;
			int num = 1;
			int num2 = (Y_ != null) ? 2 : 1;
			int num3 = (encode == 0) ? 1 : 0;
			Bands.band_ctx band_ctx = new Bands.band_ctx();
			int num4 = 1 << LM;
			int num5 = (shortBlocks != 0) ? num4 : 1;
			int num6 = num4 * (int)eBands[start];
			int[] array = new int[num2 * (num4 * (int)eBands[m.nbEBands - 1] - num6)];
			int num7 = num4 * (int)eBands[m.nbEBands - 1] - num6;
			int[] array2 = X_;
			int lowband_scratch_ptr = num4 * (int)eBands[m.nbEBands - 1];
			int num8 = 0;
			band_ctx.bandE = bandE;
			band_ctx.ec = ec;
			band_ctx.encode = encode;
			band_ctx.intensity = intensity;
			band_ctx.m = m;
			band_ctx.seed = seed;
			band_ctx.spread = spread;
			for (int i = start; i < end; i++)
			{
				int num9 = -1;
				int num10 = 0;
				band_ctx.i = i;
				int num11 = (i == end - 1) ? 1 : 0;
				int[] array3 = X_;
				int x_ptr = num4 * (int)eBands[i];
				int[] array4;
				if (Y_ != null)
				{
					array4 = Y_;
					num10 = num4 * (int)eBands[i];
				}
				else
				{
					array4 = null;
				}
				int num12 = num4 * (int)eBands[i + 1] - num4 * (int)eBands[i];
				int num13 = (int)ec.tell_frac();
				if (i != start)
				{
					balance -= num13;
				}
				int num14 = total_bits - num13 - 1;
				band_ctx.remaining_bits = num14;
				int num16;
				if (i <= codedBands - 1)
				{
					int num15 = Inlines.celt_sudiv(balance, Inlines.IMIN(3, codedBands - i));
					num16 = Inlines.IMAX(0, Inlines.IMIN(16383, Inlines.IMIN(num14 + 1, pulses[i] + num15)));
				}
				else
				{
					num16 = 0;
				}
				if (num3 != 0 && num4 * (int)eBands[i] - num12 >= num4 * (int)eBands[start] && (num != 0 || num8 == 0))
				{
					num8 = i;
				}
				int num17 = tf_res[i];
				band_ctx.tf_change = num17;
				if (i >= m.effEBands)
				{
					array3 = array;
					x_ptr = 0;
					if (Y_ != null)
					{
						array4 = array;
						num10 = 0;
					}
					array2 = null;
				}
				if (i == end - 1)
				{
					array2 = null;
				}
				uint num21;
				uint num20;
				if (num8 != 0 && (spread != 3 || num5 > 1 || num17 < 0))
				{
					num9 = Inlines.IMAX(0, num4 * (int)eBands[num8] - num6 - num12);
					int num18 = num8;
					while (num4 * (int)eBands[--num18] > num9 + num6)
					{
					}
					int num19 = num8 - 1;
					while (num4 * (int)eBands[++num19] < num9 + num6 + num12)
					{
					}
					num20 = (num21 = 0U);
					int num22 = num18;
					do
					{
						num21 |= (uint)collapse_masks[num22 * num2];
						num20 |= (uint)collapse_masks[num22 * num2 + num2 - 1];
					}
					while (++num22 < num19);
				}
				else
				{
					num20 = (num21 = (1U << num5) - 1U);
				}
				if (dual_stereo != 0 && i == intensity)
				{
					dual_stereo = 0;
					if (num3 != 0)
					{
						for (int j = 0; j < num4 * (int)eBands[i] - num6; j++)
						{
							array[j] = Inlines.HALF32(array[j] + array[num7 + j]);
						}
					}
				}
				if (dual_stereo != 0)
				{
					num21 = Bands.quant_band_decode(band_ctx, encodedData, array3, x_ptr, num12, num16 / 2, num5, (num9 != -1) ? array : null, num9, LM, (num11 != 0) ? null : array, num4 * (int)eBands[i] - num6, 32767, array2, lowband_scratch_ptr, (int)num21);
					num20 = Bands.quant_band_decode(band_ctx, encodedData, array4, num10, num12, num16 / 2, num5, (num9 != -1) ? array : null, num7 + num9, LM, (num11 != 0) ? null : array, num7 + (num4 * (int)eBands[i] - num6), 32767, array2, lowband_scratch_ptr, (int)num20);
				}
				else
				{
					if (array4 != null)
					{
						num21 = Bands.quant_band_stereo_decode(band_ctx, encodedData, array3, x_ptr, array4, num10, num12, num16, num5, (num9 != -1) ? array : null, num9, LM, (num11 != 0) ? null : array, num4 * (int)eBands[i] - num6, array2, lowband_scratch_ptr, (int)(num21 | num20));
					}
					else
					{
						num21 = Bands.quant_band_decode(band_ctx, encodedData, array3, x_ptr, num12, num16, num5, (num9 != -1) ? array : null, num9, LM, (num11 != 0) ? null : array, num4 * (int)eBands[i] - num6, 32767, array2, lowband_scratch_ptr, (int)(num21 | num20));
					}
					num20 = num21;
				}
				collapse_masks[i * num2] = (byte)(num21 & 255U);
				collapse_masks[i * num2 + num2 - 1] = (byte)(num20 & 255U);
				balance += pulses[i] + num13;
				num = ((num16 > num12 << 3) ? 1 : 0);
			}
			seed = band_ctx.seed;
		}

		// Token: 0x04000431 RID: 1073
		private static readonly byte[] bit_interleave_table = new byte[]
		{
			0,
			1,
			1,
			1,
			2,
			3,
			3,
			3,
			2,
			3,
			3,
			3,
			2,
			3,
			3,
			3
		};

		// Token: 0x04000432 RID: 1074
		private static readonly byte[] bit_deinterleave_table = new byte[]
		{
			0,
			3,
			12,
			15,
			48,
			51,
			60,
			63,
			192,
			195,
			204,
			207,
			240,
			243,
			252,
			byte.MaxValue
		};

		// Token: 0x020000AB RID: 171
		internal class band_ctx
		{
			// Token: 0x0400060A RID: 1546
			internal int encode;

			// Token: 0x0400060B RID: 1547
			internal CeltMode m;

			// Token: 0x0400060C RID: 1548
			internal int i;

			// Token: 0x0400060D RID: 1549
			internal int intensity;

			// Token: 0x0400060E RID: 1550
			internal int spread;

			// Token: 0x0400060F RID: 1551
			internal int tf_change;

			// Token: 0x04000610 RID: 1552
			internal EntropyCoder ec;

			// Token: 0x04000611 RID: 1553
			internal int remaining_bits;

			// Token: 0x04000612 RID: 1554
			internal int[][] bandE;

			// Token: 0x04000613 RID: 1555
			internal uint seed;
		}

		// Token: 0x020000AC RID: 172
		internal class split_ctx
		{
			// Token: 0x04000614 RID: 1556
			internal int inv;

			// Token: 0x04000615 RID: 1557
			internal int imid;

			// Token: 0x04000616 RID: 1558
			internal int iside;

			// Token: 0x04000617 RID: 1559
			internal int delta;

			// Token: 0x04000618 RID: 1560
			internal int itheta;

			// Token: 0x04000619 RID: 1561
			internal int qalloc;
		}
	}
}
