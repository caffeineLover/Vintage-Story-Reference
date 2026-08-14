using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	[Nullable(0)]
	public static class AlignmentExtensions
	{
		// Token: 0x06000350 RID: 848 RVA: 0x00008ED4 File Offset: 0x000070D4
		private static IContainer AlignHorizontal(this IContainer element, HorizontalAlignment horizontalAlignment)
		{
			Alignment alignment = (element as Alignment) ?? new Alignment();
			alignment.Horizontal = new HorizontalAlignment?(horizontalAlignment);
			return element.Element(alignment);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00008F04 File Offset: 0x00007104
		public static IContainer AlignLeft(this IContainer element)
		{
			return element.AlignHorizontal(HorizontalAlignment.Left);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00008F0D File Offset: 0x0000710D
		public static IContainer AlignCenter(this IContainer element)
		{
			return element.AlignHorizontal(HorizontalAlignment.Center);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00008F16 File Offset: 0x00007116
		public static IContainer AlignRight(this IContainer element)
		{
			return element.AlignHorizontal(HorizontalAlignment.Right);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00008F20 File Offset: 0x00007120
		private static IContainer AlignVertical(this IContainer element, VerticalAlignment verticalAlignment)
		{
			Alignment alignment = (element as Alignment) ?? new Alignment();
			alignment.Vertical = new VerticalAlignment?(verticalAlignment);
			return element.Element(alignment);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00008F50 File Offset: 0x00007150
		public static IContainer AlignTop(this IContainer element)
		{
			return element.AlignVertical(VerticalAlignment.Top);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00008F59 File Offset: 0x00007159
		public static IContainer AlignMiddle(this IContainer element)
		{
			return element.AlignVertical(VerticalAlignment.Middle);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00008F62 File Offset: 0x00007162
		public static IContainer AlignBottom(this IContainer element)
		{
			return element.AlignVertical(VerticalAlignment.Bottom);
		}
	}
}
