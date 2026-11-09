using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x0200004C RID: 76
	internal class SilkEncoderControl
	{
		// Token: 0x0600015A RID: 346 RVA: 0x000166B4 File Offset: 0x000148B4
		internal void Reset()
		{
			Arrays.MemSetInt(this.Gains_Q16, 0, 4);
			Arrays.MemSetShort(this.PredCoef_Q12[0], 0, 16);
			Arrays.MemSetShort(this.PredCoef_Q12[1], 0, 16);
			Arrays.MemSetShort(this.LTPCoef_Q14, 0, 20);
			this.LTP_scale_Q14 = 0;
			Arrays.MemSetInt(this.pitchL, 0, 4);
			Arrays.MemSetShort(this.AR1_Q13, 0, 64);
			Arrays.MemSetShort(this.AR2_Q13, 0, 64);
			Arrays.MemSetInt(this.LF_shp_Q14, 0, 4);
			Arrays.MemSetInt(this.GainsPre_Q14, 0, 4);
			Arrays.MemSetInt(this.HarmBoost_Q14, 0, 4);
			Arrays.MemSetInt(this.Tilt_Q14, 0, 4);
			Arrays.MemSetInt(this.HarmShapeGain_Q14, 0, 4);
			this.Lambda_Q10 = 0;
			this.input_quality_Q14 = 0;
			this.coding_quality_Q14 = 0;
			this.sparseness_Q8 = 0;
			this.predGain_Q16 = 0;
			this.LTPredCodGain_Q7 = 0;
			Arrays.MemSetInt(this.ResNrg, 0, 4);
			Arrays.MemSetInt(this.ResNrgQ, 0, 4);
			Arrays.MemSetInt(this.GainsUnq_Q16, 0, 4);
			this.lastGainIndexPrev = 0;
		}

		// Token: 0x0400024A RID: 586
		internal readonly int[] Gains_Q16 = new int[4];

		// Token: 0x0400024B RID: 587
		internal readonly short[][] PredCoef_Q12 = Arrays.InitTwoDimensionalArray<short>(2, 16);

		// Token: 0x0400024C RID: 588
		internal readonly short[] LTPCoef_Q14 = new short[20];

		// Token: 0x0400024D RID: 589
		internal int LTP_scale_Q14;

		// Token: 0x0400024E RID: 590
		internal readonly int[] pitchL = new int[4];

		// Token: 0x0400024F RID: 591
		internal readonly short[] AR1_Q13 = new short[64];

		// Token: 0x04000250 RID: 592
		internal readonly short[] AR2_Q13 = new short[64];

		// Token: 0x04000251 RID: 593
		internal readonly int[] LF_shp_Q14 = new int[4];

		// Token: 0x04000252 RID: 594
		internal readonly int[] GainsPre_Q14 = new int[4];

		// Token: 0x04000253 RID: 595
		internal readonly int[] HarmBoost_Q14 = new int[4];

		// Token: 0x04000254 RID: 596
		internal readonly int[] Tilt_Q14 = new int[4];

		// Token: 0x04000255 RID: 597
		internal readonly int[] HarmShapeGain_Q14 = new int[4];

		// Token: 0x04000256 RID: 598
		internal int Lambda_Q10;

		// Token: 0x04000257 RID: 599
		internal int input_quality_Q14;

		// Token: 0x04000258 RID: 600
		internal int coding_quality_Q14;

		// Token: 0x04000259 RID: 601
		internal int sparseness_Q8;

		// Token: 0x0400025A RID: 602
		internal int predGain_Q16;

		// Token: 0x0400025B RID: 603
		internal int LTPredCodGain_Q7;

		// Token: 0x0400025C RID: 604
		internal readonly int[] ResNrg = new int[4];

		// Token: 0x0400025D RID: 605
		internal readonly int[] ResNrgQ = new int[4];

		// Token: 0x0400025E RID: 606
		internal readonly int[] GainsUnq_Q16 = new int[4];

		// Token: 0x0400025F RID: 607
		internal sbyte lastGainIndexPrev;
	}
}
