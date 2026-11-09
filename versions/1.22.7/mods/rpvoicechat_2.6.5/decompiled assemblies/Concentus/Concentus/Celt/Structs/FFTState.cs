using System;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000096 RID: 150
	internal class FFTState
	{
		// Token: 0x040004D6 RID: 1238
		internal int nfft;

		// Token: 0x040004D7 RID: 1239
		internal short scale;

		// Token: 0x040004D8 RID: 1240
		internal int scale_shift;

		// Token: 0x040004D9 RID: 1241
		internal int shift;

		// Token: 0x040004DA RID: 1242
		internal short[] factors = new short[16];

		// Token: 0x040004DB RID: 1243
		internal short[] bitrev;

		// Token: 0x040004DC RID: 1244
		internal short[] twiddles;
	}
}
