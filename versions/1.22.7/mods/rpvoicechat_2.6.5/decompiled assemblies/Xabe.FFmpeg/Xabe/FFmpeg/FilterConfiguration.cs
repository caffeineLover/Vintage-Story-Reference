using System;
using System.Collections.Generic;

namespace Xabe.FFmpeg
{
	// Token: 0x02000030 RID: 48
	internal class FilterConfiguration : IFilterConfiguration
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006053 File Offset: 0x00004253
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000605B File Offset: 0x0000425B
		public string FilterType { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006064 File Offset: 0x00004264
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000606C File Offset: 0x0000426C
		public int StreamNumber { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006075 File Offset: 0x00004275
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0000607D File Offset: 0x0000427D
		public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
	}
}
