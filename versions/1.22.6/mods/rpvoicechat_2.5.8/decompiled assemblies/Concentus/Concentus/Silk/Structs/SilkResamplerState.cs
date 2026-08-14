using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000050 RID: 80
	internal class SilkResamplerState
	{
		// Token: 0x0600016A RID: 362 RVA: 0x000188B4 File Offset: 0x00016AB4
		internal void Reset()
		{
			Arrays.MemSetInt(this.sIIR, 0, 6);
			Arrays.MemSetInt(this.sFIR_i32, 0, 36);
			Arrays.MemSetShort(this.sFIR_i16, 0, 36);
			Arrays.MemSetShort(this.delayBuf, 0, 48);
			this.resampler_function = 0;
			this.batchSize = 0;
			this.invRatio_Q16 = 0;
			this.FIR_Order = 0;
			this.FIR_Fracs = 0;
			this.Fs_in_kHz = 0;
			this.Fs_out_kHz = 0;
			this.inputDelay = 0;
			this.Coefs = null;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00018938 File Offset: 0x00016B38
		internal void Assign(SilkResamplerState other)
		{
			this.resampler_function = other.resampler_function;
			this.batchSize = other.batchSize;
			this.invRatio_Q16 = other.invRatio_Q16;
			this.FIR_Order = other.FIR_Order;
			this.FIR_Fracs = other.FIR_Fracs;
			this.Fs_in_kHz = other.Fs_in_kHz;
			this.Fs_out_kHz = other.Fs_out_kHz;
			this.inputDelay = other.inputDelay;
			this.Coefs = other.Coefs;
			Arrays.MemCopy(other.sIIR, 0, this.sIIR, 0, 6);
			Arrays.MemCopy(other.sFIR_i32, 0, this.sFIR_i32, 0, 36);
			Arrays.MemCopy(other.sFIR_i16, 0, this.sFIR_i16, 0, 36);
			Arrays.MemCopy(other.delayBuf, 0, this.delayBuf, 0, 48);
		}

		// Token: 0x04000276 RID: 630
		internal readonly int[] sIIR = new int[6];

		// Token: 0x04000277 RID: 631
		internal readonly int[] sFIR_i32 = new int[36];

		// Token: 0x04000278 RID: 632
		internal readonly short[] sFIR_i16 = new short[36];

		// Token: 0x04000279 RID: 633
		internal readonly short[] delayBuf = new short[48];

		// Token: 0x0400027A RID: 634
		internal int resampler_function;

		// Token: 0x0400027B RID: 635
		internal int batchSize;

		// Token: 0x0400027C RID: 636
		internal int invRatio_Q16;

		// Token: 0x0400027D RID: 637
		internal int FIR_Order;

		// Token: 0x0400027E RID: 638
		internal int FIR_Fracs;

		// Token: 0x0400027F RID: 639
		internal int Fs_in_kHz;

		// Token: 0x04000280 RID: 640
		internal int Fs_out_kHz;

		// Token: 0x04000281 RID: 641
		internal int inputDelay;

		// Token: 0x04000282 RID: 642
		internal short[] Coefs;
	}
}
