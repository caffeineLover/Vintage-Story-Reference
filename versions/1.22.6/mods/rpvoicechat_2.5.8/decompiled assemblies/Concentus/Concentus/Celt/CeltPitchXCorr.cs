using System;
using System.Numerics;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x02000087 RID: 135
	internal static class CeltPitchXCorr
	{
		// Token: 0x060004C6 RID: 1222 RVA: 0x0002D888 File Offset: 0x0002BA88
		internal unsafe static int pitch_xcorr(int[] _x, int x_idx, int[] _y, int y_idx, Span<int> xcorr, int len, int max_pitch)
		{
			int num = 1;
			int i;
			if (Vector.IsHardwareAccelerated)
			{
				for (i = 0; i < max_pitch - 3; i += 4)
				{
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					Kernels.xcorr_kernel_vector(_x, x_idx, _y, y_idx + i, ref num2, ref num3, ref num4, ref num5, len);
					*xcorr[i] = num2;
					*xcorr[i + 1] = num3;
					*xcorr[i + 2] = num4;
					*xcorr[i + 3] = num5;
					num2 = Inlines.MAX32(num2, num3);
					num4 = Inlines.MAX32(num4, num5);
					num2 = Inlines.MAX32(num2, num4);
					num = Inlines.MAX32(num, num2);
				}
			}
			else
			{
				for (i = 0; i < max_pitch - 3; i += 4)
				{
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					int num9 = 0;
					Kernels.xcorr_kernel(_x, x_idx, _y, y_idx + i, ref num6, ref num7, ref num8, ref num9, len);
					*xcorr[i] = num6;
					*xcorr[i + 1] = num7;
					*xcorr[i + 2] = num8;
					*xcorr[i + 3] = num9;
					num6 = Inlines.MAX32(num6, num7);
					num8 = Inlines.MAX32(num8, num9);
					num6 = Inlines.MAX32(num6, num8);
					num = Inlines.MAX32(num, num6);
				}
			}
			while (i < max_pitch)
			{
				int num10 = Kernels.celt_inner_prod(MemoryExtensions.AsSpan<int>(_x), MemoryExtensions.AsSpan<int>(_y, i), len);
				*xcorr[i] = num10;
				num = Inlines.MAX32(num, num10);
				i++;
			}
			return num;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0002D9F0 File Offset: 0x0002BBF0
		internal static int pitch_xcorr(short[] _x, int x_idx, short[] _y, int y_idx, int[] xcorr, int len, int max_pitch)
		{
			int num = 1;
			int i;
			for (i = 0; i < max_pitch - 3; i += 4)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				Kernels.xcorr_kernel(_x, x_idx, _y, y_idx + i, ref num2, ref num3, ref num4, ref num5, len);
				xcorr[i] = num2;
				xcorr[i + 1] = num3;
				xcorr[i + 2] = num4;
				xcorr[i + 3] = num5;
				num2 = Inlines.MAX32(num2, num3);
				num4 = Inlines.MAX32(num4, num5);
				num2 = Inlines.MAX32(num2, num4);
				num = Inlines.MAX32(num, num2);
			}
			while (i < max_pitch)
			{
				int num6 = Kernels.celt_inner_prod(MemoryExtensions.AsSpan<short>(_x, x_idx), MemoryExtensions.AsSpan<short>(_y, y_idx + i), len);
				xcorr[i] = num6;
				num = Inlines.MAX32(num, num6);
				i++;
			}
			return num;
		}
	}
}
