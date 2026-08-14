using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000052 RID: 82
	internal class SilkVADState
	{
		// Token: 0x0600016F RID: 367 RVA: 0x00018A68 File Offset: 0x00016C68
		internal void Reset()
		{
			Arrays.MemSetInt(this.AnaState, 0, 2);
			Arrays.MemSetInt(this.AnaState1, 0, 2);
			Arrays.MemSetInt(this.AnaState2, 0, 2);
			Arrays.MemSetInt(this.XnrgSubfr, 0, 4);
			Arrays.MemSetInt(this.NrgRatioSmth_Q8, 0, 4);
			this.HPstate = 0;
			Arrays.MemSetInt(this.NL, 0, 4);
			Arrays.MemSetInt(this.inv_NL, 0, 4);
			Arrays.MemSetInt(this.NoiseLevelBias, 0, 4);
			this.counter = 0;
		}

		// Token: 0x04000287 RID: 647
		internal readonly int[] AnaState = new int[2];

		// Token: 0x04000288 RID: 648
		internal readonly int[] AnaState1 = new int[2];

		// Token: 0x04000289 RID: 649
		internal readonly int[] AnaState2 = new int[2];

		// Token: 0x0400028A RID: 650
		internal readonly int[] XnrgSubfr = new int[4];

		// Token: 0x0400028B RID: 651
		internal readonly int[] NrgRatioSmth_Q8 = new int[4];

		// Token: 0x0400028C RID: 652
		internal short HPstate;

		// Token: 0x0400028D RID: 653
		internal readonly int[] NL = new int[4];

		// Token: 0x0400028E RID: 654
		internal readonly int[] inv_NL = new int[4];

		// Token: 0x0400028F RID: 655
		internal readonly int[] NoiseLevelBias = new int[4];

		// Token: 0x04000290 RID: 656
		internal int counter;
	}
}
