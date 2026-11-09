using System;
using System.Collections.Generic;

namespace Xabe.FFmpeg
{
	// Token: 0x02000032 RID: 50
	public interface IFilterConfiguration
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600017A RID: 378
		string FilterType { get; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600017B RID: 379
		int StreamNumber { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600017C RID: 380
		Dictionary<string, string> Filters { get; }
	}
}
