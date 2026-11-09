using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace ModernWpf.Media.Utils
{
	// Token: 0x0200023D RID: 573
	internal static class ColorUtils
	{
		// Token: 0x060010BC RID: 4284 RVA: 0x000306F8 File Offset: 0x0002E8F8
		public static HSL RGBToHSL(Color rgb, bool round = true, int roundingPrecision = 5)
		{
			NormalizedRGB normalizedRGB = new NormalizedRGB(ref rgb, false, 5);
			return ColorUtils.RGBToHSL(normalizedRGB, round, roundingPrecision);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00030718 File Offset: 0x0002E918
		public static HSL RGBToHSL(in NormalizedRGB rgb, bool round = true, int roundingPrecision = 5)
		{
			double num = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
			double num2 = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
			double num3 = num - num2;
			double num4;
			if (num3 == 0.0)
			{
				num4 = 0.0;
			}
			else if (num == rgb.R)
			{
				num4 = 60.0 * ((rgb.G - rgb.B) / num3 % 6.0);
			}
			else if (num == rgb.G)
			{
				num4 = 60.0 * ((rgb.B - rgb.R) / num3 + 2.0);
			}
			else
			{
				num4 = 60.0 * ((rgb.R - rgb.G) / num3 + 4.0);
			}
			if (num4 < 0.0)
			{
				num4 += 360.0;
			}
			double num5 = (num + num2) / 2.0;
			double s = 0.0;
			if (num3 != 0.0)
			{
				s = num3 / (1.0 - Math.Abs(2.0 * num5 - 1.0));
			}
			return new HSL(num4, s, num5, round, roundingPrecision);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00030880 File Offset: 0x0002EA80
		public static LAB LCHToLAB(in LCH lch, bool round = true, int roundingPrecision = 5)
		{
			double a = 0.0;
			double b = 0.0;
			if (lch.H != 0.0)
			{
				a = Math.Cos(MathUtils.DegreesToRadians(lch.H)) * lch.C;
				b = Math.Sin(MathUtils.DegreesToRadians(lch.H)) * lch.C;
			}
			return new LAB(lch.L, a, b, round, roundingPrecision);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000308F4 File Offset: 0x0002EAF4
		public static LCH LABToLCH(in LAB lab, bool round = true, int roundingPrecision = 5)
		{
			double h = (MathUtils.RadiansToDegrees(Math.Atan2(lab.B, lab.A)) + 360.0) % 360.0;
			double c = Math.Sqrt(lab.A * lab.A + lab.B * lab.B);
			return new LCH(lab.L, c, h, round, roundingPrecision);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0003095C File Offset: 0x0002EB5C
		public static XYZ LABToXYZ(in LAB lab, bool round = true, int roundingPrecision = 5)
		{
			double num = (lab.L + 16.0) / 116.0;
			double num2 = num + lab.A / 500.0;
			double num3 = num - lab.B / 200.0;
			num2 = 0.95047 * ColorUtils.<LABToXYZ>g__LABToXYZHelper|5_0(num2);
			num = ColorUtils.<LABToXYZ>g__LABToXYZHelper|5_0(num);
			num3 = 1.08883 * ColorUtils.<LABToXYZ>g__LABToXYZHelper|5_0(num3);
			return new XYZ(num2, num, num3, round, roundingPrecision);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000309E0 File Offset: 0x0002EBE0
		public static LAB XYZToLAB(in XYZ xyz, bool round = true, int roundingPrecision = 5)
		{
			double num = ColorUtils.<XYZToLAB>g__XYZToLABHelper|6_0(xyz.X / 0.95047);
			double num2 = ColorUtils.<XYZToLAB>g__XYZToLABHelper|6_0(xyz.Y);
			double num3 = ColorUtils.<XYZToLAB>g__XYZToLABHelper|6_0(xyz.Z / 1.08883);
			double l = 116.0 * num2 - 16.0;
			double a = 500.0 * (num - num2);
			double b = -200.0 * (num3 - num2);
			return new LAB(l, a, b, round, roundingPrecision);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00030A64 File Offset: 0x0002EC64
		public static XYZ RGBToXYZ(Color rgb, bool round = true, int roundingPrecision = 5)
		{
			NormalizedRGB normalizedRGB = new NormalizedRGB(ref rgb, false, 5);
			return ColorUtils.RGBToXYZ(normalizedRGB, round, roundingPrecision);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00030A84 File Offset: 0x0002EC84
		public static XYZ RGBToXYZ(in NormalizedRGB rgb, bool round = true, int roundingPrecision = 5)
		{
			double num = ColorUtils.<RGBToXYZ>g__RGBToXYZHelper|8_0(rgb.R);
			double num2 = ColorUtils.<RGBToXYZ>g__RGBToXYZHelper|8_0(rgb.G);
			double num3 = ColorUtils.<RGBToXYZ>g__RGBToXYZHelper|8_0(rgb.B);
			double x = num * 0.4124564 + num2 * 0.3575761 + num3 * 0.1804375;
			double y = num * 0.2126729 + num2 * 0.7151522 + num3 * 0.072175;
			double z = num * 0.0193339 + num2 * 0.119192 + num3 * 0.9503041;
			return new XYZ(x, y, z, round, roundingPrecision);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00030B2C File Offset: 0x0002ED2C
		public static NormalizedRGB XYZToRGB(in XYZ xyz, bool round = true, int roundingPrecision = 5)
		{
			double c = ColorUtils.<XYZToRGB>g__XYZToRGBHelper|9_0(xyz.X * 3.2404542 - xyz.Y * 1.5371385 - xyz.Z * 0.4985314);
			double c2 = ColorUtils.<XYZToRGB>g__XYZToRGBHelper|9_0(xyz.X * -0.969266 + xyz.Y * 1.8760108 + xyz.Z * 0.041556);
			double c3 = ColorUtils.<XYZToRGB>g__XYZToRGBHelper|9_0(xyz.X * 0.0556434 - xyz.Y * 0.2040259 + xyz.Z * 1.0572252);
			return new NormalizedRGB(MathUtils.ClampToUnit(c), MathUtils.ClampToUnit(c2), MathUtils.ClampToUnit(c3), round, roundingPrecision);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00030BF8 File Offset: 0x0002EDF8
		public static LAB RGBToLAB(Color rgb, bool round = true, int roundingPrecision = 5)
		{
			NormalizedRGB normalizedRGB = new NormalizedRGB(ref rgb, false, 5);
			return ColorUtils.RGBToLAB(normalizedRGB, round, roundingPrecision);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00030C18 File Offset: 0x0002EE18
		public static LAB RGBToLAB(in NormalizedRGB rgb, bool round = true, int roundingPrecision = 5)
		{
			XYZ xyz = ColorUtils.RGBToXYZ(rgb, false, 5);
			return ColorUtils.XYZToLAB(xyz, round, roundingPrecision);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00030C38 File Offset: 0x0002EE38
		public static NormalizedRGB LABToRGB(in LAB lab, bool round = true, int roundingPrecision = 5)
		{
			XYZ xyz = ColorUtils.LABToXYZ(lab, false, 5);
			return ColorUtils.XYZToRGB(xyz, round, roundingPrecision);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00030C58 File Offset: 0x0002EE58
		public static LCH RGBToLCH(in NormalizedRGB rgb, bool round = true, int roundingPrecision = 5)
		{
			LAB lab = ColorUtils.RGBToLAB(rgb, true, 4);
			double l = (lab.L == 0.0) ? 0.0 : lab.L;
			double a = (lab.A == 0.0) ? 0.0 : lab.A;
			double b = (lab.B == 0.0) ? 0.0 : lab.B;
			LAB lab2 = new LAB(l, a, b, false, 5);
			return ColorUtils.LABToLCH(lab2, round, roundingPrecision);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00030CEC File Offset: 0x0002EEEC
		public static NormalizedRGB LCHToRGB(in LCH lch, bool round = true, int roundingPrecision = 5)
		{
			LAB lab = ColorUtils.LCHToLAB(lch, false, 5);
			return ColorUtils.LABToRGB(lab, round, roundingPrecision);
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00030D0C File Offset: 0x0002EF0C
		public static Color InterpolateRGB(Color left, Color right, double position)
		{
			if (position <= 0.0)
			{
				return left;
			}
			if (position >= 1.0)
			{
				return right;
			}
			return Color.FromArgb(MathUtils.Lerp(left.A, right.A, position), MathUtils.Lerp(left.R, right.R, position), MathUtils.Lerp(left.G, right.G, position), MathUtils.Lerp(left.B, right.B, position));
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00030D8C File Offset: 0x0002EF8C
		public static NormalizedRGB InterpolateRGB(in NormalizedRGB left, in NormalizedRGB right, double position)
		{
			if (position <= 0.0)
			{
				return left;
			}
			if (position >= 1.0)
			{
				return right;
			}
			return new NormalizedRGB(MathUtils.Lerp(left.R, right.R, position), MathUtils.Lerp(left.G, right.G, position), MathUtils.Lerp(left.B, right.B, position), false, 5);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00030DFC File Offset: 0x0002EFFC
		public static LAB InterpolateLAB(in LAB left, in LAB right, double position)
		{
			if (position <= 0.0)
			{
				return left;
			}
			if (position >= 1.0)
			{
				return right;
			}
			return new LAB(MathUtils.Lerp(left.L, right.L, position), MathUtils.Lerp(left.A, right.A, position), MathUtils.Lerp(left.B, right.B, position), false, 5);
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00030E6C File Offset: 0x0002F06C
		public static XYZ InterpolateXYZ(in XYZ left, in XYZ right, double position)
		{
			if (position <= 0.0)
			{
				return left;
			}
			if (position >= 1.0)
			{
				return right;
			}
			return new XYZ(MathUtils.Lerp(left.X, right.X, position), MathUtils.Lerp(left.Y, right.Y, position), MathUtils.Lerp(left.Z, right.Z, position), false, 5);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00030EDC File Offset: 0x0002F0DC
		public static NormalizedRGB InterpolateColor(in NormalizedRGB left, in NormalizedRGB right, double position, ColorScaleInterpolationMode mode)
		{
			if (mode == ColorScaleInterpolationMode.LAB)
			{
				LAB lab = ColorUtils.RGBToLAB(left, false, 5);
				LAB lab2 = ColorUtils.RGBToLAB(right, false, 5);
				LAB lab3 = ColorUtils.InterpolateLAB(lab, lab2, position);
				return ColorUtils.LABToRGB(lab3, true, 5);
			}
			if (mode != ColorScaleInterpolationMode.XYZ)
			{
				return ColorUtils.InterpolateRGB(left, right, position);
			}
			XYZ xyz = ColorUtils.RGBToXYZ(left, false, 5);
			XYZ xyz2 = ColorUtils.RGBToXYZ(right, false, 5);
			XYZ xyz3 = ColorUtils.InterpolateXYZ(xyz, xyz2, position);
			return ColorUtils.XYZToRGB(xyz3, true, 5);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00030F4B File Offset: 0x0002F14B
		[CompilerGenerated]
		internal static double <LABToXYZ>g__LABToXYZHelper|5_0(double i)
		{
			if (i > 0.206896552)
			{
				return Math.Pow(i, 3.0);
			}
			return 0.12841855 * (i - 0.137931034);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00030F7E File Offset: 0x0002F17E
		[CompilerGenerated]
		internal static double <XYZToLAB>g__XYZToLABHelper|6_0(double i)
		{
			if (i > 0.008856452)
			{
				return Math.Pow(i, 0.3333333333333333);
			}
			return i / 0.12841855 + 0.137931034;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00030FB1 File Offset: 0x0002F1B1
		[CompilerGenerated]
		internal static double <RGBToXYZ>g__RGBToXYZHelper|8_0(double i)
		{
			if (i <= 0.04045)
			{
				return i / 12.92;
			}
			return Math.Pow((i + 0.055) / 1.055, 2.4);
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00030FEE File Offset: 0x0002F1EE
		[CompilerGenerated]
		internal static double <XYZToRGB>g__XYZToRGBHelper|9_0(double i)
		{
			if (i <= 0.0031308)
			{
				return i * 12.92;
			}
			return 1.055 * Math.Pow(i, 0.4166666666666667) - 0.055;
		}

		// Token: 0x04000477 RID: 1143
		public const int DefaultRoundingPrecision = 5;
	}
}
