using System;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000093 RID: 147
	[NullableContext(1)]
	[Nullable(0)]
	public class LineDescriptor
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000ACDD File Offset: 0x00008EDD
		internal Line Line { get; } = new Line();

		// Token: 0x0600040C RID: 1036 RVA: 0x0000ACE5 File Offset: 0x00008EE5
		internal LineDescriptor()
		{
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		public LineDescriptor LineColor(Color color)
		{
			this.Line.Color = color;
			return this;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000AD08 File Offset: 0x00008F08
		public LineDescriptor LineDashPattern(float[] dashPattern, Unit unit = Unit.Point)
		{
			if (dashPattern == null)
			{
				throw new ArgumentNullException("dashPattern", "The dash pattern cannot be null.");
			}
			if (dashPattern.Length == 0)
			{
				throw new ArgumentException("The dash pattern cannot be empty.", "dashPattern");
			}
			if (dashPattern.Length % 2 != 0)
			{
				throw new ArgumentException("The dash pattern must contain an even number of elements.", "dashPattern");
			}
			this.Line.DashPattern = Enumerable.ToArray<float>(Enumerable.Select<float, float>(dashPattern, (float x) => x.ToPoints(unit)));
			return this;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000AD83 File Offset: 0x00008F83
		public LineDescriptor LineGradient(Color[] colors)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("colors", "The gradient colors cannot be null.");
			}
			if (colors.Length == 0)
			{
				throw new ArgumentException("The gradient colors cannot be empty.", "colors");
			}
			this.Line.GradientColors = colors;
			return this;
		}
	}
}
