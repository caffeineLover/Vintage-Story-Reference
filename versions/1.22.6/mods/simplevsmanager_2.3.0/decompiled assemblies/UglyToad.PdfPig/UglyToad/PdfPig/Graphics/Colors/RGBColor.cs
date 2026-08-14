using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200017E RID: 382
	public sealed class RGBColor : IColor, IEquatable<RGBColor>
	{
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00028654 File Offset: 0x00026854
		public ColorSpace ColorSpace { get; } = 1;

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0002865C File Offset: 0x0002685C
		public double R { get; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00028664 File Offset: 0x00026864
		public double G { get; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0002866C File Offset: 0x0002686C
		public double B { get; }

		// Token: 0x060008DA RID: 2266 RVA: 0x00028674 File Offset: 0x00026874
		public RGBColor(double r, double g, double b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00028698 File Offset: 0x00026898
		[return: TupleElementNames(new string[]
		{
			"r",
			"g",
			"b"
		})]
		public ValueTuple<double, double, double> ToRGBValues()
		{
			return new ValueTuple<double, double, double>(this.R, this.G, this.B);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000286B4 File Offset: 0x000268B4
		public override bool Equals(object obj)
		{
			RGBColor rgbcolor = obj as RGBColor;
			return rgbcolor != null && this.Equals(rgbcolor);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x000286D4 File Offset: 0x000268D4
		public bool Equals(RGBColor other)
		{
			return other != null && this.R == other.R && this.G == other.G && this.B == other.B;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002870C File Offset: 0x0002690C
		public override int GetHashCode()
		{
			return new ValueTuple<double, double, double>(this.R, this.G, this.B).GetHashCode();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002873E File Offset: 0x0002693E
		public static bool operator ==(RGBColor color1, RGBColor color2)
		{
			return EqualityComparer<RGBColor>.Default.Equals(color1, color2);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002874C File Offset: 0x0002694C
		public static bool operator !=(RGBColor color1, RGBColor color2)
		{
			return !(color1 == color2);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00028758 File Offset: 0x00026958
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(11, 3);
			defaultInterpolatedStringHandler.AppendLiteral("RGB: (");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.R);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.G);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.B);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000434 RID: 1076
		public static RGBColor Black = new RGBColor(0.0, 0.0, 0.0);

		// Token: 0x04000435 RID: 1077
		public static RGBColor White = new RGBColor(1.0, 1.0, 1.0);
	}
}
