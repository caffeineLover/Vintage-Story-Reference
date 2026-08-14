using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200009E RID: 158
	[NullableContext(1)]
	[Nullable(0)]
	public static class ScaleExtensions
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x0000B5DC File Offset: 0x000097DC
		private static IContainer ScaleValue(this IContainer element, float x = 1f, float y = 1f)
		{
			Scale scale = (element as Scale) ?? new Scale();
			scale.ScaleX *= x;
			scale.ScaleY *= y;
			return element.Element(scale);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000B61C File Offset: 0x0000981C
		public static IContainer Scale(this IContainer element, float factor)
		{
			if (factor == 0f)
			{
				throw new ArgumentException("Vertical scale factor cannot be zero.", "factor");
			}
			return element.ScaleValue(factor, factor);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000B63E File Offset: 0x0000983E
		public static IContainer ScaleHorizontal(this IContainer element, float factor)
		{
			if (factor == 0f)
			{
				throw new ArgumentException("Vertical scale factor cannot be zero.", "factor");
			}
			return element.ScaleValue(factor, 1f);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000B664 File Offset: 0x00009864
		public static IContainer ScaleVertical(this IContainer element, float factor)
		{
			if (factor == 0f)
			{
				throw new ArgumentException("Vertical scale factor cannot be zero.", "factor");
			}
			return element.ScaleValue(1f, factor);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000B68A File Offset: 0x0000988A
		public static IContainer FlipHorizontal(this IContainer element)
		{
			return element.ScaleValue(-1f, 1f);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000B69C File Offset: 0x0000989C
		public static IContainer FlipVertical(this IContainer element)
		{
			return element.ScaleValue(1f, -1f);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000B6AE File Offset: 0x000098AE
		public static IContainer FlipOver(this IContainer element)
		{
			return element.ScaleValue(-1f, -1f);
		}
	}
}
