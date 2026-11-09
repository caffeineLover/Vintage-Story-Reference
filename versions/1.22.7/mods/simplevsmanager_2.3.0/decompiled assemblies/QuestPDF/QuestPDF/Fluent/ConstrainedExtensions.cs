using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	[Nullable(0)]
	public static class ConstrainedExtensions
	{
		// Token: 0x06000360 RID: 864 RVA: 0x00009054 File Offset: 0x00007254
		private static IContainer ConstrainedWidth(this IContainer element, float? min = null, float? max = null)
		{
			Constrained constrained = (element as Constrained) ?? new Constrained();
			float? num = min;
			float num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("min", "The minimum width cannot be negative.");
			}
			num = max;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("max", "The maximum width cannot be negative.");
			}
			num = min;
			float? num3 = max;
			if (num.GetValueOrDefault() > num3.GetValueOrDefault() & (num != null & num3 != null))
			{
				throw new ArgumentOutOfRangeException("min", "The minimum width cannot be greater than the maximum width.");
			}
			if (min != null)
			{
				constrained.MinWidth = min;
			}
			if (max != null)
			{
				constrained.MaxWidth = max;
			}
			return element.Element(constrained);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00009126 File Offset: 0x00007326
		public static IContainer Width(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.ConstrainedWidth(new float?(value), new float?(value));
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00009144 File Offset: 0x00007344
		public static IContainer MinWidth(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.ConstrainedWidth(new float?(value), default(float?));
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00009170 File Offset: 0x00007370
		public static IContainer MaxWidth(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? max = new float?(value);
			return element.ConstrainedWidth(default(float?), max);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000091A0 File Offset: 0x000073A0
		private static IContainer ConstrainedHeight(this IContainer element, float? min = null, float? max = null)
		{
			Constrained constrained = (element as Constrained) ?? new Constrained();
			float? num = min;
			float num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("min", "The minimum height cannot be negative.");
			}
			num = max;
			num2 = 0f;
			if (num.GetValueOrDefault() < num2 & num != null)
			{
				throw new ArgumentOutOfRangeException("max", "The maximum height cannot be negative.");
			}
			num = min;
			float? num3 = max;
			if (num.GetValueOrDefault() > num3.GetValueOrDefault() & (num != null & num3 != null))
			{
				throw new ArgumentOutOfRangeException("min", "The minimum height cannot be greater than the maximum height.");
			}
			if (min != null)
			{
				constrained.MinHeight = min;
			}
			if (max != null)
			{
				constrained.MaxHeight = max;
			}
			return element.Element(constrained);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00009272 File Offset: 0x00007472
		public static IContainer Height(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.ConstrainedHeight(new float?(value), new float?(value));
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00009290 File Offset: 0x00007490
		public static IContainer MinHeight(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			return element.ConstrainedHeight(new float?(value), default(float?));
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000092BC File Offset: 0x000074BC
		public static IContainer MaxHeight(this IContainer element, float value, Unit unit = Unit.Point)
		{
			value = value.ToPoints(unit);
			float? max = new float?(value);
			return element.ConstrainedHeight(default(float?), max);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000092EA File Offset: 0x000074EA
		internal static IContainer EnforceSizeWhenEmpty(this IContainer element)
		{
			(element as Constrained).EnforceSizeWhenEmpty = true;
			return element;
		}
	}
}
