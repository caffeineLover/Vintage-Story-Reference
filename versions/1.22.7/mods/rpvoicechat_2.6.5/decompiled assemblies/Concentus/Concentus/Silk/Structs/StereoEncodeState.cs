using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000054 RID: 84
	internal class StereoEncodeState
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00018BB4 File Offset: 0x00016DB4
		internal void Reset()
		{
			Arrays.MemSetShort(this.pred_prev_Q13, 0, 2);
			Arrays.MemSetShort(this.sMid, 0, 2);
			Arrays.MemSetShort(this.sSide, 0, 2);
			Arrays.MemSetInt(this.mid_side_amp_Q0, 0, 4);
			this.smth_width_Q14 = 0;
			this.width_prev_Q14 = 0;
			this.silent_side_len = 0;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Arrays.MemSetSbyte(this.predIx[i][j], 0, 3);
				}
			}
			Arrays.MemSetSbyte(this.mid_only_flags, 0, 3);
		}

		// Token: 0x04000294 RID: 660
		internal readonly short[] pred_prev_Q13 = new short[2];

		// Token: 0x04000295 RID: 661
		internal readonly short[] sMid = new short[2];

		// Token: 0x04000296 RID: 662
		internal readonly short[] sSide = new short[2];

		// Token: 0x04000297 RID: 663
		internal readonly int[] mid_side_amp_Q0 = new int[4];

		// Token: 0x04000298 RID: 664
		internal short smth_width_Q14;

		// Token: 0x04000299 RID: 665
		internal short width_prev_Q14;

		// Token: 0x0400029A RID: 666
		internal short silent_side_len;

		// Token: 0x0400029B RID: 667
		internal readonly sbyte[][][] predIx = Arrays.InitThreeDimensionalArray<sbyte>(3, 2, 3);

		// Token: 0x0400029C RID: 668
		internal readonly sbyte[] mid_only_flags = new sbyte[3];
	}
}
