using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public static class PageSizeExtensions
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0000848B File Offset: 0x0000668B
		public static PageSize Portrait(this PageSize size)
		{
			return new PageSize(Math.Min(size.Width, size.Height), Math.Max(size.Width, size.Height), Unit.Point);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000084B5 File Offset: 0x000066B5
		public static PageSize Landscape(this PageSize size)
		{
			return new PageSize(Math.Max(size.Width, size.Height), Math.Min(size.Width, size.Height), Unit.Point);
		}
	}
}
