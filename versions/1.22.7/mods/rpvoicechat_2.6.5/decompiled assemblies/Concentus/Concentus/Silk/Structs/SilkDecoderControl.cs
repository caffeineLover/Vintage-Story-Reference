using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x0200004A RID: 74
	internal class SilkDecoderControl
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0001651C File Offset: 0x0001471C
		internal void Reset()
		{
			Arrays.MemSetInt(this.pitchL, 0, 4);
			Arrays.MemSetInt(this.Gains_Q16, 0, 4);
			Arrays.MemSetShort(this.PredCoef_Q12[0], 0, 16);
			Arrays.MemSetShort(this.PredCoef_Q12[1], 0, 16);
			Arrays.MemSetShort(this.LTPCoef_Q14, 0, 20);
			this.LTP_scale_Q14 = 0;
		}

		// Token: 0x0400023B RID: 571
		internal readonly int[] pitchL = new int[4];

		// Token: 0x0400023C RID: 572
		internal readonly int[] Gains_Q16 = new int[4];

		// Token: 0x0400023D RID: 573
		internal readonly short[][] PredCoef_Q12 = Arrays.InitTwoDimensionalArray<short>(2, 16);

		// Token: 0x0400023E RID: 574
		internal readonly short[] LTPCoef_Q14 = new short[20];

		// Token: 0x0400023F RID: 575
		internal int LTP_scale_Q14;
	}
}
