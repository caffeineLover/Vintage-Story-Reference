using System;

namespace Xabe.FFmpeg
{
	// Token: 0x02000036 RID: 54
	public enum Flag
	{
		// Token: 0x04000398 RID: 920
		mv4,
		// Token: 0x04000399 RID: 921
		qpel,
		// Token: 0x0400039A RID: 922
		loop,
		// Token: 0x0400039B RID: 923
		qscale,
		// Token: 0x0400039C RID: 924
		pass1,
		// Token: 0x0400039D RID: 925
		pass2,
		// Token: 0x0400039E RID: 926
		gray,
		// Token: 0x0400039F RID: 927
		emu_edge,
		// Token: 0x040003A0 RID: 928
		psnr,
		// Token: 0x040003A1 RID: 929
		truncated,
		// Token: 0x040003A2 RID: 930
		drop_changed,
		// Token: 0x040003A3 RID: 931
		ildct,
		// Token: 0x040003A4 RID: 932
		low_delay,
		// Token: 0x040003A5 RID: 933
		global_header,
		// Token: 0x040003A6 RID: 934
		bitexact,
		// Token: 0x040003A7 RID: 935
		aic,
		// Token: 0x040003A8 RID: 936
		[Obsolete]
		cbp,
		// Token: 0x040003A9 RID: 937
		[Obsolete]
		qprd,
		// Token: 0x040003AA RID: 938
		ilme,
		// Token: 0x040003AB RID: 939
		cgop,
		// Token: 0x040003AC RID: 940
		output_corrupt
	}
}
