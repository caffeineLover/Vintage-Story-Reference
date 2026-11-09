using System;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000097 RID: 151
	internal class MDCTLookup
	{
		// Token: 0x06000539 RID: 1337 RVA: 0x00036287 File Offset: 0x00034487
		internal MDCTLookup()
		{
		}

		// Token: 0x040004DD RID: 1245
		internal int n;

		// Token: 0x040004DE RID: 1246
		internal int maxshift;

		// Token: 0x040004DF RID: 1247
		internal FFTState[] kfft = new FFTState[4];

		// Token: 0x040004E0 RID: 1248
		internal short[] trig;
	}
}
