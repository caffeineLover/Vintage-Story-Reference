using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000041 RID: 65
	internal class CNGState
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00014210 File Offset: 0x00012410
		internal void Reset()
		{
			Arrays.MemSetInt(this.CNG_exc_buf_Q14, 0, 320);
			Arrays.MemSetShort(this.CNG_smth_NLSF_Q15, 0, 16);
			Arrays.MemSetInt(this.CNG_synth_state, 0, 16);
			this.CNG_smth_Gain_Q16 = 0;
			this.rand_seed = 0;
			this.fs_kHz = 0;
		}

		// Token: 0x04000180 RID: 384
		internal readonly int[] CNG_exc_buf_Q14 = new int[320];

		// Token: 0x04000181 RID: 385
		internal readonly short[] CNG_smth_NLSF_Q15 = new short[16];

		// Token: 0x04000182 RID: 386
		internal readonly int[] CNG_synth_state = new int[16];

		// Token: 0x04000183 RID: 387
		internal int CNG_smth_Gain_Q16;

		// Token: 0x04000184 RID: 388
		internal int rand_seed;

		// Token: 0x04000185 RID: 389
		internal int fs_kHz;
	}
}
