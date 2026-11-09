using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000045 RID: 69
	internal class PLCStruct
	{
		// Token: 0x0600013B RID: 315 RVA: 0x000145E0 File Offset: 0x000127E0
		internal void Reset()
		{
			this.pitchL_Q8 = 0;
			Arrays.MemSetShort(this.LTPCoef_Q14, 0, 5);
			Arrays.MemSetShort(this.prevLPC_Q12, 0, 16);
			this.last_frame_lost = 0;
			this.rand_seed = 0;
			this.randScale_Q14 = 0;
			this.conc_energy = 0;
			this.conc_energy_shift = 0;
			this.prevLTP_scale_Q14 = 0;
			Arrays.MemSetInt(this.prevGain_Q16, 0, 2);
			this.fs_kHz = 0;
			this.nb_subfr = 0;
			this.subfr_length = 0;
		}

		// Token: 0x040001AD RID: 429
		internal int pitchL_Q8;

		// Token: 0x040001AE RID: 430
		internal readonly short[] LTPCoef_Q14 = new short[5];

		// Token: 0x040001AF RID: 431
		internal readonly short[] prevLPC_Q12 = new short[16];

		// Token: 0x040001B0 RID: 432
		internal int last_frame_lost;

		// Token: 0x040001B1 RID: 433
		internal int rand_seed;

		// Token: 0x040001B2 RID: 434
		internal short randScale_Q14;

		// Token: 0x040001B3 RID: 435
		internal int conc_energy;

		// Token: 0x040001B4 RID: 436
		internal int conc_energy_shift;

		// Token: 0x040001B5 RID: 437
		internal short prevLTP_scale_Q14;

		// Token: 0x040001B6 RID: 438
		internal readonly int[] prevGain_Q16 = new int[2];

		// Token: 0x040001B7 RID: 439
		internal int fs_kHz;

		// Token: 0x040001B8 RID: 440
		internal int nb_subfr;

		// Token: 0x040001B9 RID: 441
		internal int subfr_length;
	}
}
