using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	[Nullable(0)]
	public static class LineExtensions
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x0000ADBC File Offset: 0x00008FBC
		private static LineDescriptor Line(this IContainer element, LineType type, float thickness)
		{
			if (thickness < 0f)
			{
				throw new ArgumentOutOfRangeException("thickness", "The Line thickness cannot be negative.");
			}
			LineDescriptor lineDescriptor = new LineDescriptor();
			lineDescriptor.Line.Thickness = thickness;
			lineDescriptor.Line.Type = type;
			element.Element(lineDescriptor.Line);
			return lineDescriptor;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000AE0D File Offset: 0x0000900D
		public static LineDescriptor LineVertical(this IContainer element, float thickness, Unit unit = Unit.Point)
		{
			return element.Line(LineType.Vertical, thickness.ToPoints(unit));
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000AE1D File Offset: 0x0000901D
		public static LineDescriptor LineHorizontal(this IContainer element, float thickness, Unit unit = Unit.Point)
		{
			return element.Line(LineType.Horizontal, thickness.ToPoints(unit));
		}
	}
}
