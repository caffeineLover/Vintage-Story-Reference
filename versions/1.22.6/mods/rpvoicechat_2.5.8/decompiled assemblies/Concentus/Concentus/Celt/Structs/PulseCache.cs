using System;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000098 RID: 152
	internal class PulseCache
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x0003629B File Offset: 0x0003449B
		internal void Reset()
		{
			this.size = 0;
			this.index = null;
			this.bits = null;
			this.caps = null;
		}

		// Token: 0x040004E1 RID: 1249
		internal int size;

		// Token: 0x040004E2 RID: 1250
		internal short[] index;

		// Token: 0x040004E3 RID: 1251
		internal byte[] bits;

		// Token: 0x040004E4 RID: 1252
		internal byte[] caps;
	}
}
