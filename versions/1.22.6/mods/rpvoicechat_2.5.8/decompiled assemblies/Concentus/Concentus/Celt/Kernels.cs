using System;
using System.Numerics;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x02000089 RID: 137
	internal static class Kernels
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x0002DD50 File Offset: 0x0002BF50
		internal unsafe static void celt_fir(Span<short> x, Span<short> num, Span<short> y, int N, int ord, Span<short> mem)
		{
			short[] array = new short[ord];
			short[] array2 = new short[N + ord];
			int i;
			for (i = 0; i < ord; i++)
			{
				array[i] = *num[ord - i - 1];
			}
			for (i = 0; i < ord; i++)
			{
				array2[i] = *mem[ord - i - 1];
			}
			for (i = 0; i < N; i++)
			{
				array2[i + ord] = *x[i];
			}
			for (i = 0; i < ord; i++)
			{
				*mem[i] = *x[N - i - 1];
			}
			for (i = 0; i < N - 3; i += 4)
			{
				int a = 0;
				int a2 = 0;
				int a3 = 0;
				int a4 = 0;
				Kernels.xcorr_kernel(array, 0, array2, i, ref a, ref a2, ref a3, ref a4, ord);
				*y[i] = Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i]), Inlines.PSHR32(a, 12)));
				*y[i + 1] = Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i + 1]), Inlines.PSHR32(a2, 12)));
				*y[i + 2] = Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i + 2]), Inlines.PSHR32(a3, 12)));
				*y[i + 3] = Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i + 3]), Inlines.PSHR32(a4, 12)));
			}
			while (i < N)
			{
				int num2 = 0;
				for (int j = 0; j < ord; j++)
				{
					num2 = Inlines.MAC16_16(num2, array[j], array2[i + j]);
				}
				*y[i] = Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i]), Inlines.PSHR32(num2, 12)));
				i++;
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0002DF28 File Offset: 0x0002C128
		internal unsafe static void celt_fir(Span<int> x, Span<int> num, Span<int> y, int N, int ord, Span<int> mem)
		{
			int[] array = new int[ord];
			int[] array2 = new int[N + ord];
			int i;
			for (i = 0; i < ord; i++)
			{
				array[i] = *num[ord - i - 1];
			}
			for (i = 0; i < ord; i++)
			{
				array2[i] = *mem[ord - i - 1];
			}
			for (i = 0; i < N; i++)
			{
				array2[i + ord] = *x[i];
			}
			for (i = 0; i < ord; i++)
			{
				*mem[i] = *x[N - i - 1];
			}
			for (i = 0; i < N - 3; i += 4)
			{
				int a = 0;
				int a2 = 0;
				int a3 = 0;
				int a4 = 0;
				Kernels.xcorr_kernel(array, 0, array2, i, ref a, ref a2, ref a3, ref a4, ord);
				*y[i] = (int)Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i]), Inlines.PSHR32(a, 12)));
				*y[i + 1] = (int)Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i + 1]), Inlines.PSHR32(a2, 12)));
				*y[i + 2] = (int)Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i + 2]), Inlines.PSHR32(a3, 12)));
				*y[i + 3] = (int)Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i + 3]), Inlines.PSHR32(a4, 12)));
			}
			while (i < N)
			{
				int num2 = 0;
				for (int j = 0; j < ord; j++)
				{
					num2 = Inlines.MAC16_16(num2, array[j], array2[i + j]);
				}
				*y[i] = (int)Inlines.SATURATE16(Inlines.ADD32(Inlines.EXTEND32(*x[i]), Inlines.PSHR32(num2, 12)));
				i++;
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0002E100 File Offset: 0x0002C300
		internal static void xcorr_kernel(short[] x, int x_idx, short[] y, int y_idx, ref int sum0, ref int sum1, ref int sum2, ref int sum3, int len)
		{
			int num = x_idx;
			short b = 0;
			int num2 = y_idx + 1;
			short b2 = y[y_idx];
			short b3 = y[num2++];
			short b4 = y[num2++];
			int i;
			for (i = 0; i < len - 3; i += 4)
			{
				short a = x[num++];
				b = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b2);
				sum1 = Inlines.MAC16_16(sum1, a, b3);
				sum2 = Inlines.MAC16_16(sum2, a, b4);
				sum3 = Inlines.MAC16_16(sum3, a, b);
				a = x[num++];
				b2 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b3);
				sum1 = Inlines.MAC16_16(sum1, a, b4);
				sum2 = Inlines.MAC16_16(sum2, a, b);
				sum3 = Inlines.MAC16_16(sum3, a, b2);
				a = x[num++];
				b3 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b4);
				sum1 = Inlines.MAC16_16(sum1, a, b);
				sum2 = Inlines.MAC16_16(sum2, a, b2);
				sum3 = Inlines.MAC16_16(sum3, a, b3);
				a = x[num++];
				b4 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b);
				sum1 = Inlines.MAC16_16(sum1, a, b2);
				sum2 = Inlines.MAC16_16(sum2, a, b3);
				sum3 = Inlines.MAC16_16(sum3, a, b4);
			}
			if (i++ < len)
			{
				short a2 = x[num++];
				b = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a2, b2);
				sum1 = Inlines.MAC16_16(sum1, a2, b3);
				sum2 = Inlines.MAC16_16(sum2, a2, b4);
				sum3 = Inlines.MAC16_16(sum3, a2, b);
			}
			if (i++ < len)
			{
				short a3 = x[num++];
				b2 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a3, b3);
				sum1 = Inlines.MAC16_16(sum1, a3, b4);
				sum2 = Inlines.MAC16_16(sum2, a3, b);
				sum3 = Inlines.MAC16_16(sum3, a3, b2);
			}
			if (i < len)
			{
				short a4 = x[num++];
				b3 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a4, b4);
				sum1 = Inlines.MAC16_16(sum1, a4, b);
				sum2 = Inlines.MAC16_16(sum2, a4, b2);
				sum3 = Inlines.MAC16_16(sum3, a4, b3);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0002E384 File Offset: 0x0002C584
		internal static void xcorr_kernel(int[] x, int x_idx, int[] y, int y_idx, ref int sum0, ref int sum1, ref int sum2, ref int sum3, int len)
		{
			int num = x_idx;
			int b = 0;
			int num2 = y_idx + 1;
			int b2 = y[y_idx];
			int b3 = y[num2++];
			int b4 = y[num2++];
			int i;
			for (i = 0; i < len - 3; i += 4)
			{
				int a = x[num++];
				b = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b2);
				sum1 = Inlines.MAC16_16(sum1, a, b3);
				sum2 = Inlines.MAC16_16(sum2, a, b4);
				sum3 = Inlines.MAC16_16(sum3, a, b);
				a = x[num++];
				b2 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b3);
				sum1 = Inlines.MAC16_16(sum1, a, b4);
				sum2 = Inlines.MAC16_16(sum2, a, b);
				sum3 = Inlines.MAC16_16(sum3, a, b2);
				a = x[num++];
				b3 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b4);
				sum1 = Inlines.MAC16_16(sum1, a, b);
				sum2 = Inlines.MAC16_16(sum2, a, b2);
				sum3 = Inlines.MAC16_16(sum3, a, b3);
				a = x[num++];
				b4 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a, b);
				sum1 = Inlines.MAC16_16(sum1, a, b2);
				sum2 = Inlines.MAC16_16(sum2, a, b3);
				sum3 = Inlines.MAC16_16(sum3, a, b4);
			}
			if (i++ < len)
			{
				int a2 = x[num++];
				b = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a2, b2);
				sum1 = Inlines.MAC16_16(sum1, a2, b3);
				sum2 = Inlines.MAC16_16(sum2, a2, b4);
				sum3 = Inlines.MAC16_16(sum3, a2, b);
			}
			if (i++ < len)
			{
				int a3 = x[num++];
				b2 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a3, b3);
				sum1 = Inlines.MAC16_16(sum1, a3, b4);
				sum2 = Inlines.MAC16_16(sum2, a3, b);
				sum3 = Inlines.MAC16_16(sum3, a3, b2);
			}
			if (i < len)
			{
				int a4 = x[num++];
				b3 = y[num2++];
				sum0 = Inlines.MAC16_16(sum0, a4, b4);
				sum1 = Inlines.MAC16_16(sum1, a4, b);
				sum2 = Inlines.MAC16_16(sum2, a4, b2);
				sum3 = Inlines.MAC16_16(sum3, a4, b3);
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0002E608 File Offset: 0x0002C808
		internal static void xcorr_kernel_vector(int[] x, int x_idx, int[] y, int y_idx, ref int sum0, ref int sum1, ref int sum2, ref int sum3, int len)
		{
			int i = 0;
			int num = len - 4 - (len - 4) % Vector<int>.Count;
			while (i < num)
			{
				Vector<int> vector = new Vector<int>(x, x_idx + i);
				sum0 += Vector.Dot<int>(Vector<int>.One, Vector.Multiply<int>(vector, new Vector<int>(y, y_idx + i)));
				sum1 += Vector.Dot<int>(Vector<int>.One, Vector.Multiply<int>(vector, new Vector<int>(y, y_idx + i + 1)));
				sum2 += Vector.Dot<int>(Vector<int>.One, Vector.Multiply<int>(vector, new Vector<int>(y, y_idx + i + 2)));
				sum3 += Vector.Dot<int>(Vector<int>.One, Vector.Multiply<int>(vector, new Vector<int>(y, y_idx + i + 3)));
				i += Vector<int>.Count;
			}
			while (i < len)
			{
				int a = x[x_idx + i];
				sum0 = Inlines.MAC16_16(sum0, a, y[y_idx + i]);
				sum1 = Inlines.MAC16_16(sum1, a, y[y_idx + i + 1]);
				sum2 = Inlines.MAC16_16(sum2, a, y[y_idx + i + 2]);
				sum3 = Inlines.MAC16_16(sum3, a, y[y_idx + i + 3]);
				i++;
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0002E728 File Offset: 0x0002C928
		internal unsafe static int celt_inner_prod(Span<short> x, Span<short> y, int N)
		{
			int num = 0;
			for (int i = 0; i < N; i++)
			{
				num = Inlines.MAC16_16(num, *x[i], *y[i]);
			}
			return num;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0002E760 File Offset: 0x0002C960
		internal unsafe static int celt_inner_prod(Span<int> x, Span<int> y, int N)
		{
			int num = 0;
			for (int i = 0; i < N; i++)
			{
				num = Inlines.MAC16_16(num, *x[i], *y[i]);
			}
			return num;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0002E798 File Offset: 0x0002C998
		internal unsafe static void dual_inner_prod(Span<int> x, Span<int> y01, Span<int> y02, int N, out int xy1, out int xy2)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < N; i++)
			{
				num = Inlines.MAC16_16(num, *x[i], *y01[i]);
				num2 = Inlines.MAC16_16(num2, *x[i], *y02[i]);
			}
			xy1 = num;
			xy2 = num2;
		}
	}
}
