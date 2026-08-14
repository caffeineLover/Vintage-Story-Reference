using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000088 RID: 136
	[NullableContext(1)]
	[Nullable(0)]
	public static class ExtendExtensions
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x0000A144 File Offset: 0x00008344
		private static IContainer Extend(this IContainer element, bool vertical = false, bool horizontal = false)
		{
			Extend extend = (element as Extend) ?? new Extend();
			extend.ExtendVertical = vertical;
			extend.ExtendHorizontal = horizontal;
			return element.Element(extend);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000A184 File Offset: 0x00008384
		public static IContainer Extend(this IContainer element)
		{
			return element.Extend(true, true);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000A18E File Offset: 0x0000838E
		public static IContainer ExtendVertical(this IContainer element)
		{
			return element.Extend(true, false);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000A198 File Offset: 0x00008398
		public static IContainer ExtendHorizontal(this IContainer element)
		{
			return element.Extend(false, true);
		}
	}
}
