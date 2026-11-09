using System;
using System.Collections.Generic;

namespace Xabe.FFmpeg
{
	// Token: 0x02000031 RID: 49
	internal interface IFilterable
	{
		// Token: 0x06000179 RID: 377
		IEnumerable<IFilterConfiguration> GetFilters();
	}
}
