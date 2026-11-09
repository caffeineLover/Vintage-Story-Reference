using System;
using Concentus.Celt.Structs;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x0200008A RID: 138
	internal static class KissFFT
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x0002E7EF File Offset: 0x0002C9EF
		internal static int S_MUL(int a, int b)
		{
			return Inlines.MULT16_32_Q15(b, a);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0002E7F8 File Offset: 0x0002C9F8
		internal static int S_MUL(int a, short b)
		{
			return Inlines.MULT16_32_Q15(b, a);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0002E801 File Offset: 0x0002CA01
		internal static int HALF_OF(int x)
		{
			return x >> 1;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0002E808 File Offset: 0x0002CA08
		internal unsafe static void kf_bfly2(Span<int> Fout, int fout_ptr, int m, int N)
		{
			short b = 23170;
			for (int i = 0; i < N; i++)
			{
				int num = fout_ptr + 8;
				int num2 = *Fout[num];
				int num3 = *Fout[num + 1];
				*Fout[num] = *Fout[fout_ptr] - num2;
				*Fout[num + 1] = *Fout[fout_ptr + 1] - num3;
				*Fout[fout_ptr] += num2;
				*Fout[fout_ptr + 1] += num3;
				num2 = KissFFT.S_MUL(*Fout[num + 2] + *Fout[num + 3], b);
				num3 = KissFFT.S_MUL(*Fout[num + 3] - *Fout[num + 2], b);
				*Fout[num + 2] = *Fout[fout_ptr + 2] - num2;
				*Fout[num + 3] = *Fout[fout_ptr + 3] - num3;
				*Fout[fout_ptr + 2] += num2;
				*Fout[fout_ptr + 3] += num3;
				num2 = *Fout[num + 5];
				num3 = 0 - *Fout[num + 4];
				*Fout[num + 4] = *Fout[fout_ptr + 4] - num2;
				*Fout[num + 5] = *Fout[fout_ptr + 5] - num3;
				*Fout[fout_ptr + 4] += num2;
				*Fout[fout_ptr + 5] += num3;
				num2 = KissFFT.S_MUL(*Fout[num + 7] - *Fout[num + 6], b);
				num3 = KissFFT.S_MUL(0 - *Fout[num + 7] - *Fout[num + 6], b);
				*Fout[num + 6] = *Fout[fout_ptr + 6] - num2;
				*Fout[num + 7] = *Fout[fout_ptr + 7] - num3;
				*Fout[fout_ptr + 6] += num2;
				*Fout[fout_ptr + 7] += num3;
				fout_ptr += 16;
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0002EA20 File Offset: 0x0002CC20
		internal unsafe static void kf_bfly4(Span<int> Fout, int fout_ptr, int fstride, FFTState st, int m, int N, int mm)
		{
			if (m == 1)
			{
				for (int i = 0; i < N; i++)
				{
					int num = *Fout[fout_ptr] - *Fout[fout_ptr + 4];
					int num2 = *Fout[fout_ptr + 1] - *Fout[fout_ptr + 5];
					*Fout[fout_ptr] += *Fout[fout_ptr + 4];
					*Fout[fout_ptr + 1] += *Fout[fout_ptr + 5];
					int num3 = *Fout[fout_ptr + 2] + *Fout[fout_ptr + 6];
					int num4 = *Fout[fout_ptr + 3] + *Fout[fout_ptr + 7];
					*Fout[fout_ptr + 4] = *Fout[fout_ptr] - num3;
					*Fout[fout_ptr + 5] = *Fout[fout_ptr + 1] - num4;
					*Fout[fout_ptr] += num3;
					*Fout[fout_ptr + 1] += num4;
					num3 = *Fout[fout_ptr + 2] - *Fout[fout_ptr + 6];
					num4 = *Fout[fout_ptr + 3] - *Fout[fout_ptr + 7];
					*Fout[fout_ptr + 2] = num + num4;
					*Fout[fout_ptr + 3] = num2 - num3;
					*Fout[fout_ptr + 6] = num - num4;
					*Fout[fout_ptr + 7] = num2 + num3;
					fout_ptr += 8;
				}
				return;
			}
			int num5 = fout_ptr;
			for (int i = 0; i < N; i++)
			{
				fout_ptr = num5 + 2 * i * mm;
				int num6 = fout_ptr + 2 * m;
				int num7 = fout_ptr + 4 * m;
				int num8 = fout_ptr + 6 * m;
				int num11;
				int num10;
				int num9 = num10 = (num11 = 0);
				for (int j = 0; j < m; j++)
				{
					int num12 = KissFFT.S_MUL(*Fout[num6], st.twiddles[num11]) - KissFFT.S_MUL(*Fout[num6 + 1], st.twiddles[num11 + 1]);
					int num13 = KissFFT.S_MUL(*Fout[num6], st.twiddles[num11 + 1]) + KissFFT.S_MUL(*Fout[num6 + 1], st.twiddles[num11]);
					int num14 = KissFFT.S_MUL(*Fout[num7], st.twiddles[num9]) - KissFFT.S_MUL(*Fout[num7 + 1], st.twiddles[num9 + 1]);
					int num15 = KissFFT.S_MUL(*Fout[num7], st.twiddles[num9 + 1]) + KissFFT.S_MUL(*Fout[num7 + 1], st.twiddles[num9]);
					int num16 = KissFFT.S_MUL(*Fout[num8], st.twiddles[num10]) - KissFFT.S_MUL(*Fout[num8 + 1], st.twiddles[num10 + 1]);
					int num17 = KissFFT.S_MUL(*Fout[num8], st.twiddles[num10 + 1]) + KissFFT.S_MUL(*Fout[num8 + 1], st.twiddles[num10]);
					int num18 = *Fout[fout_ptr] - num14;
					int num19 = *Fout[fout_ptr + 1] - num15;
					*Fout[fout_ptr] += num14;
					*Fout[fout_ptr + 1] += num15;
					int num20 = num12 + num16;
					int num21 = num13 + num17;
					int num22 = num12 - num16;
					int num23 = num13 - num17;
					*Fout[num7] = *Fout[fout_ptr] - num20;
					*Fout[num7 + 1] = *Fout[fout_ptr + 1] - num21;
					num11 += fstride * 2;
					num9 += fstride * 4;
					num10 += fstride * 6;
					*Fout[fout_ptr] += num20;
					*Fout[fout_ptr + 1] += num21;
					*Fout[num6] = num18 + num23;
					*Fout[num6 + 1] = num19 - num22;
					*Fout[num8] = num18 - num23;
					*Fout[num8 + 1] = num19 + num22;
					fout_ptr += 2;
					num6 += 2;
					num7 += 2;
					num8 += 2;
				}
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0002EE48 File Offset: 0x0002D048
		internal unsafe static void kf_bfly3(Span<int> Fout, int fout_ptr, int fstride, FFTState st, int m, int N, int mm)
		{
			int num = 2 * m;
			int num2 = 4 * m;
			int num3 = fout_ptr;
			for (int i = 0; i < N; i++)
			{
				fout_ptr = num3 + 2 * i * mm;
				int num5;
				int num4 = num5 = 0;
				int num6 = m;
				do
				{
					int num7 = KissFFT.S_MUL(*Fout[fout_ptr + num], st.twiddles[num5]) - KissFFT.S_MUL(*Fout[fout_ptr + num + 1], st.twiddles[num5 + 1]);
					int num8 = KissFFT.S_MUL(*Fout[fout_ptr + num], st.twiddles[num5 + 1]) + KissFFT.S_MUL(*Fout[fout_ptr + num + 1], st.twiddles[num5]);
					int num9 = KissFFT.S_MUL(*Fout[fout_ptr + num2], st.twiddles[num4]) - KissFFT.S_MUL(*Fout[fout_ptr + num2 + 1], st.twiddles[num4 + 1]);
					int num10 = KissFFT.S_MUL(*Fout[fout_ptr + num2], st.twiddles[num4 + 1]) + KissFFT.S_MUL(*Fout[fout_ptr + num2 + 1], st.twiddles[num4]);
					int num11 = num7 + num9;
					int num12 = num8 + num10;
					int num13 = num7 - num9;
					int num14 = num8 - num10;
					num5 += fstride * 2;
					num4 += fstride * 4;
					*Fout[fout_ptr + num] = *Fout[fout_ptr] - KissFFT.HALF_OF(num11);
					*Fout[fout_ptr + num + 1] = *Fout[fout_ptr + 1] - KissFFT.HALF_OF(num12);
					num13 = KissFFT.S_MUL(num13, -28378);
					num14 = KissFFT.S_MUL(num14, -28378);
					*Fout[fout_ptr] += num11;
					*Fout[fout_ptr + 1] += num12;
					*Fout[fout_ptr + num2] = *Fout[fout_ptr + num] + num14;
					*Fout[fout_ptr + num2 + 1] = *Fout[fout_ptr + num + 1] - num13;
					*Fout[fout_ptr + num] -= num14;
					*Fout[fout_ptr + num + 1] += num13;
					fout_ptr += 2;
				}
				while (--num6 != 0);
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0002F074 File Offset: 0x0002D274
		internal unsafe static void kf_bfly5(Span<int> Fout, int fout_ptr, int fstride, FFTState st, int m, int N, int mm)
		{
			int num = fout_ptr;
			short b = 10126;
			short b2 = -31164;
			short b3 = -26510;
			short b4 = -19261;
			for (int i = 0; i < N; i++)
			{
				int num5;
				int num4;
				int num3;
				int num2 = num3 = (num4 = (num5 = 0));
				fout_ptr = num + 2 * i * mm;
				int num6 = fout_ptr;
				int num7 = fout_ptr + 2 * m;
				int num8 = fout_ptr + 4 * m;
				int num9 = fout_ptr + 6 * m;
				int num10 = fout_ptr + 8 * m;
				for (int j = 0; j < m; j++)
				{
					int num11 = *Fout[num6];
					int num12 = *Fout[num6 + 1];
					int num13 = KissFFT.S_MUL(*Fout[num7], st.twiddles[num3]) - KissFFT.S_MUL(*Fout[num7 + 1], st.twiddles[num3 + 1]);
					int num14 = KissFFT.S_MUL(*Fout[num7], st.twiddles[num3 + 1]) + KissFFT.S_MUL(*Fout[num7 + 1], st.twiddles[num3]);
					int num15 = KissFFT.S_MUL(*Fout[num8], st.twiddles[num2]) - KissFFT.S_MUL(*Fout[num8 + 1], st.twiddles[num2 + 1]);
					int num16 = KissFFT.S_MUL(*Fout[num8], st.twiddles[num2 + 1]) + KissFFT.S_MUL(*Fout[num8 + 1], st.twiddles[num2]);
					int num17 = KissFFT.S_MUL(*Fout[num9], st.twiddles[num4]) - KissFFT.S_MUL(*Fout[num9 + 1], st.twiddles[num4 + 1]);
					int num18 = KissFFT.S_MUL(*Fout[num9], st.twiddles[num4 + 1]) + KissFFT.S_MUL(*Fout[num9 + 1], st.twiddles[num4]);
					int num19 = KissFFT.S_MUL(*Fout[num10], st.twiddles[num5]) - KissFFT.S_MUL(*Fout[num10 + 1], st.twiddles[num5 + 1]);
					int num20 = KissFFT.S_MUL(*Fout[num10], st.twiddles[num5 + 1]) + KissFFT.S_MUL(*Fout[num10 + 1], st.twiddles[num5]);
					num3 += 2 * fstride;
					num2 += 4 * fstride;
					num4 += 6 * fstride;
					num5 += 8 * fstride;
					int num21 = num13 + num19;
					int num22 = num14 + num20;
					int a = num13 - num19;
					int a2 = num14 - num20;
					int num23 = num15 + num17;
					int num24 = num16 + num18;
					int a3 = num15 - num17;
					int a4 = num16 - num18;
					*Fout[num6] += num21 + num23;
					*Fout[num6 + 1] += num22 + num24;
					int num25 = num11 + KissFFT.S_MUL(num21, b) + KissFFT.S_MUL(num23, b3);
					int num26 = num12 + KissFFT.S_MUL(num22, b) + KissFFT.S_MUL(num24, b3);
					int num27 = KissFFT.S_MUL(a2, b2) + KissFFT.S_MUL(a4, b4);
					int num28 = 0 - KissFFT.S_MUL(a, b2) - KissFFT.S_MUL(a3, b4);
					*Fout[num7] = num25 - num27;
					*Fout[num7 + 1] = num26 - num28;
					*Fout[num10] = num25 + num27;
					*Fout[num10 + 1] = num26 + num28;
					int num29 = num11 + KissFFT.S_MUL(num21, b3) + KissFFT.S_MUL(num23, b);
					int num30 = num12 + KissFFT.S_MUL(num22, b3) + KissFFT.S_MUL(num24, b);
					int num31 = 0 - KissFFT.S_MUL(a2, b4) + KissFFT.S_MUL(a4, b2);
					int num32 = KissFFT.S_MUL(a, b4) - KissFFT.S_MUL(a3, b2);
					*Fout[num8] = num29 + num31;
					*Fout[num8 + 1] = num30 + num32;
					*Fout[num9] = num29 - num31;
					*Fout[num9 + 1] = num30 - num32;
					num6 += 2;
					num7 += 2;
					num8 += 2;
					num9 += 2;
					num10 += 2;
				}
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0002F484 File Offset: 0x0002D684
		internal static void opus_fft_impl(FFTState st, Span<int> fout, int fout_ptr)
		{
			int[] array = new int[8];
			int num = (st.shift > 0) ? st.shift : 0;
			array[0] = 1;
			int num2 = 0;
			int num4;
			do
			{
				int num3 = (int)st.factors[2 * num2];
				num4 = (int)st.factors[2 * num2 + 1];
				array[num2 + 1] = array[num2] * num3;
				num2++;
			}
			while (num4 != 1);
			num4 = (int)st.factors[2 * num2 - 1];
			for (int i = num2 - 1; i >= 0; i--)
			{
				int num5;
				if (i != 0)
				{
					num5 = (int)st.factors[2 * i - 1];
				}
				else
				{
					num5 = 1;
				}
				switch (st.factors[2 * i])
				{
				case 2:
					KissFFT.kf_bfly2(fout, fout_ptr, num4, array[i]);
					break;
				case 3:
					KissFFT.kf_bfly3(fout, fout_ptr, array[i] << num, st, num4, array[i], num5);
					break;
				case 4:
					KissFFT.kf_bfly4(fout, fout_ptr, array[i] << num, st, num4, array[i], num5);
					break;
				case 5:
					KissFFT.kf_bfly5(fout, fout_ptr, array[i] << num, st, num4, array[i], num5);
					break;
				}
				num4 = num5;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0002F5A8 File Offset: 0x0002D7A8
		internal static void opus_fft(FFTState st, int[] fin, int[] fout)
		{
			int shift = st.scale_shift - 1;
			short scale = st.scale;
			for (int i = 0; i < st.nfft; i++)
			{
				fout[(int)(2 * st.bitrev[i])] = Inlines.SHR32(Inlines.MULT16_32_Q16(scale, fin[2 * i]), shift);
				fout[(int)(2 * st.bitrev[i] + 1)] = Inlines.SHR32(Inlines.MULT16_32_Q16(scale, fin[2 * i + 1]), shift);
			}
			KissFFT.opus_fft_impl(st, fout, 0);
		}

		// Token: 0x04000451 RID: 1105
		internal const int MAXFACTORS = 8;
	}
}
