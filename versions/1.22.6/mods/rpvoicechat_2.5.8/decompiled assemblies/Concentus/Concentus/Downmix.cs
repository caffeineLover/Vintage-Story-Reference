using System;
using Concentus.Common;

namespace Concentus
{
	// Token: 0x0200000A RID: 10
	internal static class Downmix
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00004310 File Offset: 0x00002510
		internal unsafe static void downmix_float(ReadOnlySpan<float> x, Span<int> sub, int sub_ptr, int subframe, int offset, int c1, int c2, int C)
		{
			for (int i = 0; i < subframe; i++)
			{
				*sub[sub_ptr + i] = (int)Inlines.FLOAT2INT16(*x[(i + offset) * C + c1]);
			}
			if (c2 > -1)
			{
				for (int i = 0; i < subframe; i++)
				{
					*sub[sub_ptr + i] += (int)Inlines.FLOAT2INT16(*x[(i + offset) * C + c2]);
				}
			}
			else if (c2 == -2)
			{
				for (int j = 1; j < C; j++)
				{
					int num = j;
					for (int i = 0; i < subframe; i++)
					{
						*sub[sub_ptr + i] += (int)Inlines.FLOAT2INT16(*x[(i + offset) * C + num]);
					}
				}
			}
			int num2 = 4096;
			if (C == -2)
			{
				num2 /= C;
			}
			else
			{
				num2 /= 2;
			}
			for (int i = 0; i < subframe; i++)
			{
				*sub[sub_ptr + i] *= num2;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004408 File Offset: 0x00002608
		internal unsafe static void downmix_int(ReadOnlySpan<short> x, Span<int> sub, int sub_ptr, int subframe, int offset, int c1, int c2, int C)
		{
			for (int i = 0; i < subframe; i++)
			{
				*sub[i + sub_ptr] = (int)(*x[(i + offset) * C + c1]);
			}
			if (c2 > -1)
			{
				for (int i = 0; i < subframe; i++)
				{
					*sub[i + sub_ptr] += (int)(*x[(i + offset) * C + c2]);
				}
			}
			else if (c2 == -2)
			{
				for (int j = 1; j < C; j++)
				{
					for (int i = 0; i < subframe; i++)
					{
						*sub[i + sub_ptr] += (int)(*x[(i + offset) * C + j]);
					}
				}
			}
			int num = 4096;
			if (C == -2)
			{
				num /= C;
			}
			else
			{
				num /= 2;
			}
			for (int i = 0; i < subframe; i++)
			{
				*sub[i + sub_ptr] *= num;
			}
		}

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x0600053D RID: 1341
		internal delegate void downmix_func<T>(ReadOnlySpan<T> _x, Span<int> sub, int sub_ptr, int subframe, int offset, int c1, int c2, int C);
	}
}
