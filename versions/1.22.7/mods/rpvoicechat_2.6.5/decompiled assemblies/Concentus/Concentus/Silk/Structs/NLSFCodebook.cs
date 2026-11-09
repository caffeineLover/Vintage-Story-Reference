using System;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000044 RID: 68
	internal class NLSFCodebook
	{
		// Token: 0x06000139 RID: 313 RVA: 0x0001457C File Offset: 0x0001277C
		internal void Reset()
		{
			this.nVectors = 0;
			this.order = 0;
			this.quantStepSize_Q16 = 0;
			this.invQuantStepSize_Q6 = 0;
			this.CB1_NLSF_Q8 = null;
			this.CB1_iCDF = null;
			this.pred_Q8 = null;
			this.ec_sel = null;
			this.ec_iCDF = null;
			this.ec_Rates_Q5 = null;
			this.deltaMin_Q15 = null;
		}

		// Token: 0x040001A2 RID: 418
		internal short nVectors;

		// Token: 0x040001A3 RID: 419
		internal short order;

		// Token: 0x040001A4 RID: 420
		internal short quantStepSize_Q16;

		// Token: 0x040001A5 RID: 421
		internal short invQuantStepSize_Q6;

		// Token: 0x040001A6 RID: 422
		internal byte[] CB1_NLSF_Q8;

		// Token: 0x040001A7 RID: 423
		internal byte[] CB1_iCDF;

		// Token: 0x040001A8 RID: 424
		internal byte[] pred_Q8;

		// Token: 0x040001A9 RID: 425
		internal byte[] ec_sel;

		// Token: 0x040001AA RID: 426
		internal byte[] ec_iCDF;

		// Token: 0x040001AB RID: 427
		internal byte[] ec_Rates_Q5;

		// Token: 0x040001AC RID: 428
		internal short[] deltaMin_Q15;
	}
}
