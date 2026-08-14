using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000175 RID: 373
	public sealed class GrayColor : IColor, IEquatable<GrayColor>
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x000280B1 File Offset: 0x000262B1
		public static GrayColor Black { get; } = new GrayColor(0.0);

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x000280B8 File Offset: 0x000262B8
		public static GrayColor White { get; } = new GrayColor(1.0);

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x000280BF File Offset: 0x000262BF
		public ColorSpace ColorSpace { get; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x000280C7 File Offset: 0x000262C7
		public double Gray { get; }

		// Token: 0x060008A9 RID: 2217 RVA: 0x000280CF File Offset: 0x000262CF
		public GrayColor(double gray)
		{
			this.Gray = gray;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000280DE File Offset: 0x000262DE
		[return: TupleElementNames(new string[]
		{
			"r",
			"g",
			"b"
		})]
		public ValueTuple<double, double, double> ToRGBValues()
		{
			return new ValueTuple<double, double, double>(this.Gray, this.Gray, this.Gray);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000280F7 File Offset: 0x000262F7
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GrayColor);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00028105 File Offset: 0x00026305
		public bool Equals(GrayColor other)
		{
			return other != null && this.Gray == other.Gray;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00028120 File Offset: 0x00026320
		public override int GetHashCode()
		{
			return this.Gray.GetHashCode();
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002813B File Offset: 0x0002633B
		public static bool operator ==(GrayColor color1, GrayColor color2)
		{
			return EqualityComparer<GrayColor>.Default.Equals(color1, color2);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00028149 File Offset: 0x00026349
		public static bool operator !=(GrayColor color1, GrayColor color2)
		{
			return !(color1 == color2);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00028158 File Offset: 0x00026358
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Gray: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Gray);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
