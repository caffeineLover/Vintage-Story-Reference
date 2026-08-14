using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x0200004F RID: 79
	internal class SilkPrefilterState
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00018837 File Offset: 0x00016A37
		internal SilkPrefilterState()
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001885C File Offset: 0x00016A5C
		internal void Reset()
		{
			Arrays.MemSetShort(this.sLTP_shp, 0, 512);
			Arrays.MemSetInt(this.sAR_shp, 0, 17);
			this.sLTP_shp_buf_idx = 0;
			this.sLF_AR_shp_Q12 = 0;
			this.sLF_MA_shp_Q12 = 0;
			this.sHarmHP_Q2 = 0;
			this.rand_seed = 0;
			this.lagPrev = 0;
		}

		// Token: 0x0400026E RID: 622
		internal readonly short[] sLTP_shp = new short[512];

		// Token: 0x0400026F RID: 623
		internal readonly int[] sAR_shp = new int[17];

		// Token: 0x04000270 RID: 624
		internal int sLTP_shp_buf_idx;

		// Token: 0x04000271 RID: 625
		internal int sLF_AR_shp_Q12;

		// Token: 0x04000272 RID: 626
		internal int sLF_MA_shp_Q12;

		// Token: 0x04000273 RID: 627
		internal int sHarmHP_Q2;

		// Token: 0x04000274 RID: 628
		internal int rand_seed;

		// Token: 0x04000275 RID: 629
		internal int lagPrev;
	}
}
