using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000053 RID: 83
	internal class StereoDecodeState
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00018B5F File Offset: 0x00016D5F
		internal void Reset()
		{
			Arrays.MemSetShort(this.pred_prev_Q13, 0, 2);
			Arrays.MemSetShort(this.sMid, 0, 2);
			Arrays.MemSetShort(this.sSide, 0, 2);
		}

		// Token: 0x04000291 RID: 657
		internal readonly short[] pred_prev_Q13 = new short[2];

		// Token: 0x04000292 RID: 658
		internal readonly short[] sMid = new short[2];

		// Token: 0x04000293 RID: 659
		internal readonly short[] sSide = new short[2];
	}
}
