using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public static class PaddingExtensions
	{
		// Token: 0x0600042A RID: 1066 RVA: 0x0000B028 File Offset: 0x00009228
		private static IContainer Padding(this IContainer element, float top = 0f, float bottom = 0f, float left = 0f, float right = 0f)
		{
			Padding padding = (element as Padding) ?? new Padding();
			padding.Top += top;
			padding.Bottom += bottom;
			padding.Left += left;
			padding.Right += right;
			return element.Element(padding);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000B085 File Offset: 0x00009285
		public static IContainer Padding(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(value, value, value, value);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000B09A File Offset: 0x0000929A
		public static IContainer PaddingHorizontal(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(0f, 0f, value, value);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000B0B7 File Offset: 0x000092B7
		public static IContainer PaddingVertical(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(value, value, 0f, 0f);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000B0D4 File Offset: 0x000092D4
		public static IContainer PaddingTop(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(value, 0f, 0f, 0f);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000B0F5 File Offset: 0x000092F5
		public static IContainer PaddingBottom(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(0f, value, 0f, 0f);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B116 File Offset: 0x00009316
		public static IContainer PaddingLeft(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(0f, 0f, value, 0f);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000B137 File Offset: 0x00009337
		public static IContainer PaddingRight(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Padding(0f, 0f, 0f, value);
		}
	}
}
