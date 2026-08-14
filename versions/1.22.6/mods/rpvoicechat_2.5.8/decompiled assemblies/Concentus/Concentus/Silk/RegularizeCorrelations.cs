using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000033 RID: 51
	internal static class RegularizeCorrelations
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0000F940 File Offset: 0x0000DB40
		internal static void silk_regularize_correlations(int[] XX, int XX_ptr, int[] xx, int xx_ptr, int noise, int D)
		{
			for (int i = 0; i < D; i++)
			{
				Inlines.MatrixSet<int>(XX, XX_ptr, i, i, D, Inlines.silk_ADD32(Inlines.MatrixGet<int>(XX, XX_ptr, i, i, D), noise));
			}
			xx[xx_ptr] += noise;
		}
	}
}
