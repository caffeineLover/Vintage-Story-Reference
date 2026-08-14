using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	[Nullable(0)]
	public static class ShrinkExtensions
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x0000B8CC File Offset: 0x00009ACC
		private static IContainer Shrink(this IContainer element, bool? vertical = null, bool? horizontal = null)
		{
			Shrink shrink = (element as Shrink) ?? new Shrink();
			if (vertical != null)
			{
				shrink.Vertical = vertical.Value;
			}
			if (horizontal != null)
			{
				shrink.Horizontal = horizontal.Value;
			}
			return element.Element(shrink);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000B91C File Offset: 0x00009B1C
		public static IContainer Shrink(this IContainer element)
		{
			return element.Shrink(new bool?(true), new bool?(true));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000B930 File Offset: 0x00009B30
		public static IContainer ShrinkVertical(this IContainer element)
		{
			return element.Shrink(new bool?(true), default(bool?));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000B954 File Offset: 0x00009B54
		public static IContainer ShrinkHorizontal(this IContainer element)
		{
			bool? horizontal = new bool?(true);
			return element.Shrink(default(bool?), horizontal);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000B979 File Offset: 0x00009B79
		[Obsolete("This element has been renamed since version 2022.1. Please use the Shrink method.")]
		[ExcludeFromCodeCoverage]
		public static IContainer Box(this IContainer element)
		{
			return element.Shrink();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000B981 File Offset: 0x00009B81
		[Obsolete("This element has been renamed since version 2023.11. Please use the Shrink method.")]
		[ExcludeFromCodeCoverage]
		public static IContainer MinimalBox(this IContainer element)
		{
			return element.Shrink();
		}
	}
}
