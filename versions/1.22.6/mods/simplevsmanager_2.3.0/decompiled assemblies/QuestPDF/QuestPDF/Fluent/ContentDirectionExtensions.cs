using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000081 RID: 129
	[NullableContext(1)]
	[Nullable(0)]
	public static class ContentDirectionExtensions
	{
		// Token: 0x06000369 RID: 873 RVA: 0x000092F9 File Offset: 0x000074F9
		internal static IContainer ContentDirection(this IContainer element, ContentDirection direction)
		{
			return element.Element(new ContentDirectionSetter
			{
				ContentDirection = direction
			});
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000930D File Offset: 0x0000750D
		public static IContainer ContentFromLeftToRight(this IContainer element)
		{
			return element.ContentDirection(QuestPDF.Infrastructure.ContentDirection.LeftToRight);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00009316 File Offset: 0x00007516
		public static IContainer ContentFromRightToLeft(this IContainer element)
		{
			return element.ContentDirection(QuestPDF.Infrastructure.ContentDirection.RightToLeft);
		}
	}
}
