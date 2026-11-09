using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000164 RID: 356
	public sealed class CMYKColor : IColor, IEquatable<CMYKColor>
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00025F1F File Offset: 0x0002411F
		public static IColor Black { get; } = new CMYKColor(0.0, 0.0, 0.0, 1.0);

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00025F26 File Offset: 0x00024126
		public static IColor White { get; } = new CMYKColor(0.0, 0.0, 0.0, 0.0);

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00025F2D File Offset: 0x0002412D
		public ColorSpace ColorSpace { get; } = 2;

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00025F35 File Offset: 0x00024135
		public double C { get; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00025F3D File Offset: 0x0002413D
		public double M { get; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00025F45 File Offset: 0x00024145
		public double Y { get; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x00025F4D File Offset: 0x0002414D
		public double K { get; }

		// Token: 0x0600080E RID: 2062 RVA: 0x00025F55 File Offset: 0x00024155
		public CMYKColor(double c, double m, double y, double k)
		{
			this.C = c;
			this.M = m;
			this.Y = y;
			this.K = k;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00025F84 File Offset: 0x00024184
		[return: TupleElementNames(new string[]
		{
			"r",
			"g",
			"b"
		})]
		public ValueTuple<double, double, double> ToRGBValues()
		{
			return new ValueTuple<double, double, double>((1.0 - this.C) * (1.0 - this.K), (1.0 - this.M) * (1.0 - this.K), (1.0 - this.Y) * (1.0 - this.K));
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00025FF9 File Offset: 0x000241F9
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CMYKColor);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00026008 File Offset: 0x00024208
		public bool Equals(CMYKColor other)
		{
			return other != null && this.C == other.C && this.M == other.M && this.Y == other.Y && this.K == other.K;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00026058 File Offset: 0x00024258
		public override int GetHashCode()
		{
			return (((-492570696 * -1521134295 + this.C.GetHashCode()) * -1521134295 + this.M.GetHashCode()) * -1521134295 + this.Y.GetHashCode()) * -1521134295 + this.K.GetHashCode();
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000260BE File Offset: 0x000242BE
		public static bool operator ==(CMYKColor color1, CMYKColor color2)
		{
			return EqualityComparer<CMYKColor>.Default.Equals(color1, color2);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000260CC File Offset: 0x000242CC
		public static bool operator !=(CMYKColor color1, CMYKColor color2)
		{
			return !(color1 == color2);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000260D8 File Offset: 0x000242D8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(14, 4);
			defaultInterpolatedStringHandler.AppendLiteral("CMYK: (");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.C);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.M);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Y);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.K);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
