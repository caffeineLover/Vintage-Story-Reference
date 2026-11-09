using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	[Nullable(0)]
	public static class StyledBoxExtensions
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x0000B98C File Offset: 0x00009B8C
		public static IContainer Border(this IContainer element, float all, Color color)
		{
			float? left = new float?(all);
			float? top = new float?(all);
			float? right = new float?(all);
			return element.Border(top, new float?(all), left, right).BorderColor(color);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000B9C4 File Offset: 0x00009BC4
		public static IContainer Background(this IContainer element, Color color)
		{
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			styledBox.BackgroundColor = color;
			return element.Element(styledBox);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000B9F0 File Offset: 0x00009BF0
		public static IContainer BackgroundLinearGradient(this IContainer element, float angle, Color[] colors)
		{
			if (colors == null || colors.Length == 0)
			{
				throw new ArgumentException("The background linear-gradient colors cannot be empty.", "colors");
			}
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			styledBox.BackgroundGradientAngle = new float?(angle);
			styledBox.BackgroundGradientColors = colors;
			return element.Element(styledBox);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000BA40 File Offset: 0x00009C40
		private static IContainer Border(this IContainer element, float? top = null, float? bottom = null, float? left = null, float? right = null)
		{
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			float? num = top;
			float num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("top", "The top border cannot be negative.");
			}
			num = bottom;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("bottom", "The bottom border cannot be negative.");
			}
			num = left;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("left", "The left border cannot be negative.");
			}
			num = right;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("right", "The right border cannot be negative.");
			}
			if (styledBox.BorderColor == Colors.Transparent.Hex)
			{
				styledBox.BorderColor = Colors.Black;
			}
			if (top != null)
			{
				styledBox.BorderTop = top.Value;
			}
			if (bottom != null)
			{
				styledBox.BorderBottom = bottom.Value;
			}
			if (left != null)
			{
				styledBox.BorderLeft = left.Value;
			}
			if (right != null)
			{
				styledBox.BorderRight = right.Value;
			}
			return element.Element(styledBox);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000BB8F File Offset: 0x00009D8F
		public static IContainer Border(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Border(new float?(value), new float?(value), new float?(value), new float?(value));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public static IContainer BorderVertical(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? left = new float?(value);
			float? right = new float?(value);
			return element.Border(default(float?), default(float?), left, right);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000BBF8 File Offset: 0x00009DF8
		public static IContainer BorderHorizontal(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Border(new float?(value), new float?(value), default(float?), default(float?));
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000BC34 File Offset: 0x00009E34
		public static IContainer BorderLeft(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? left = new float?(value);
			return element.Border(default(float?), default(float?), left, default(float?));
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000BC74 File Offset: 0x00009E74
		public static IContainer BorderRight(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? right = new float?(value);
			return element.Border(default(float?), default(float?), default(float?), right);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		public static IContainer BorderTop(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.Border(new float?(value), default(float?), default(float?), default(float?));
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		public static IContainer BorderBottom(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? bottom = new float?(value);
			return element.Border(default(float?), bottom, default(float?), default(float?));
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000BD34 File Offset: 0x00009F34
		private static IContainer CornerRadius(this IContainer element, float? topLeft = null, float? topRight = null, float? bottomRight = null, float? bottomLeft = null)
		{
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			float? num = topLeft;
			float num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("topLeft", "The top-left corner radius cannot be negative.");
			}
			num = topRight;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("topRight", "The top-right corner radius cannot be negative.");
			}
			num = bottomRight;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("bottomRight", "The bottom-right corner radius cannot be negative.");
			}
			num = bottomLeft;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("bottomLeft", "The bottom-left corner radius cannot be negative.");
			}
			if (topLeft != null)
			{
				styledBox.BorderRadiusTopLeft = topLeft.Value;
			}
			if (topRight != null)
			{
				styledBox.BorderRadiusTopRight = topRight.Value;
			}
			if (bottomRight != null)
			{
				styledBox.BorderRadiusBottomRight = bottomRight.Value;
			}
			if (bottomLeft != null)
			{
				styledBox.BorderRadiusBottomLeft = bottomLeft.Value;
			}
			return element.Element(styledBox);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000BE61 File Offset: 0x0000A061
		public static IContainer CornerRadius(this IContainer element, float value = 0f, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.CornerRadius(new float?(value), new float?(value), new float?(value), new float?(value));
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000BE8C File Offset: 0x0000A08C
		public static IContainer CornerRadiusTopLeft(this IContainer element, float value = 0f, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.CornerRadius(new float?(value), default(float?), default(float?), default(float?));
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public static IContainer CornerRadiusTopRight(this IContainer element, float value = 0f, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? topRight = new float?(value);
			return element.CornerRadius(default(float?), topRight, default(float?), default(float?));
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000BF0C File Offset: 0x0000A10C
		public static IContainer CornerRadiusBottomLeft(this IContainer element, float value = 0f, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? bottomLeft = new float?(value);
			return element.CornerRadius(default(float?), default(float?), default(float?), bottomLeft);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000BF4C File Offset: 0x0000A14C
		public static IContainer CornerRadiusBottomRight(this IContainer element, float value = 0f, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? bottomRight = new float?(value);
			return element.CornerRadius(default(float?), default(float?), bottomRight, default(float?));
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public static IContainer BorderColor(this IContainer element, Color color)
		{
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			styledBox.BorderColor = color;
			return element.Element(styledBox);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		public static IContainer BorderLinearGradient(this IContainer element, float angle, Color[] colors)
		{
			if (colors == null || colors.Length == 0)
			{
				throw new ArgumentException("The border linear-gradient colors cannot be empty.", "colors");
			}
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			styledBox.BorderGradientAngle = new float?(angle);
			styledBox.BorderGradientColors = colors;
			return element.Element(styledBox);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000C008 File Offset: 0x0000A208
		private static IContainer BorderAlignment(this IContainer element, float value)
		{
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			styledBox.BorderAlignment = new float?(value);
			return element.Element(styledBox);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000C038 File Offset: 0x0000A238
		public static IContainer BorderAlignmentOutside(this IContainer element)
		{
			return element.BorderAlignment(1f);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000C045 File Offset: 0x0000A245
		public static IContainer BorderAlignmentMiddle(this IContainer element)
		{
			return element.BorderAlignment(0.5f);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000C052 File Offset: 0x0000A252
		public static IContainer BorderAlignmentInside(this IContainer element)
		{
			return element.BorderAlignment(0f);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000C060 File Offset: 0x0000A260
		public static IContainer Shadow(this IContainer element, BoxShadowStyle style)
		{
			if (style == null)
			{
				throw new ArgumentNullException("style", "The box shadow style cannot be null.");
			}
			style.Validate();
			StyledBox styledBox = (element as StyledBox) ?? new StyledBox();
			styledBox.Shadow = style;
			return element.Element(styledBox);
		}
	}
}
