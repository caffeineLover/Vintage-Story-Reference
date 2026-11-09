using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	[Nullable(0)]
	public static class TranslateExtensions
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x0000D6D8 File Offset: 0x0000B8D8
		private static IContainer Translate(this IContainer element, float x = 0f, float y = 0f)
		{
			Translate translate = (element as Translate) ?? new Translate();
			translate.TranslateX += x;
			translate.TranslateY += y;
			return element.Element(translate);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0000D718 File Offset: 0x0000B918
		public static IContainer TranslateX(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Translate(value, 0f);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0000D72F File Offset: 0x0000B92F
		public static IContainer TranslateY(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Translate(0f, value);
		}
	}
}
