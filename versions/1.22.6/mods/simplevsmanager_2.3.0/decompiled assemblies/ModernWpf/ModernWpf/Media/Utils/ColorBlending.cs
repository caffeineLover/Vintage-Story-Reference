using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x02000233 RID: 563
	internal static class ColorBlending
	{
		// Token: 0x06001087 RID: 4231 RVA: 0x0002F6B0 File Offset: 0x0002D8B0
		public static NormalizedRGB SaturateViaLCH(in NormalizedRGB input, double saturation, double saturationConstant = 18.0)
		{
			LCH lch = ColorUtils.RGBToLCH(input, false, 5);
			double num = lch.C + saturation * saturationConstant;
			if (num < 0.0)
			{
				num = 0.0;
			}
			LCH lch2 = new LCH(lch.L, num, lch.H, false, 5);
			return ColorUtils.LCHToRGB(lch2, false, 5);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0002F708 File Offset: 0x0002D908
		public static NormalizedRGB Blend(in NormalizedRGB bottom, in NormalizedRGB top, ColorBlendMode mode)
		{
			switch (mode)
			{
			case ColorBlendMode.Burn:
				return ColorBlending.BlendBurn(bottom, top);
			case ColorBlendMode.Darken:
				return ColorBlending.BlendDarken(bottom, top);
			case ColorBlendMode.Dodge:
				return ColorBlending.BlendDodge(bottom, top);
			case ColorBlendMode.Lighten:
				return ColorBlending.BlendLighten(bottom, top);
			case ColorBlendMode.Multiply:
				return ColorBlending.BlendMultiply(bottom, top);
			case ColorBlendMode.Overlay:
				return ColorBlending.BlendOverlay(bottom, top);
			case ColorBlendMode.Screen:
				return ColorBlending.BlendScreen(bottom, top);
			default:
				throw new ArgumentException("Unknown blend mode", "mode");
			}
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0002F780 File Offset: 0x0002D980
		public static NormalizedRGB BlendBurn(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendBurn(bottom.R, top.R), ColorBlending.BlendBurn(bottom.G, top.G), ColorBlending.BlendBurn(bottom.B, top.B), false, 5);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0002F7BC File Offset: 0x0002D9BC
		public static double BlendBurn(double bottom, double top)
		{
			if (top == 0.0)
			{
				return 0.0;
			}
			return 1.0 - (1.0 - bottom) / top;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0002F7EB File Offset: 0x0002D9EB
		public static NormalizedRGB BlendDarken(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendDarken(bottom.R, top.R), ColorBlending.BlendDarken(bottom.G, top.G), ColorBlending.BlendDarken(bottom.B, top.B), false, 5);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0002F827 File Offset: 0x0002DA27
		public static double BlendDarken(double bottom, double top)
		{
			return Math.Min(bottom, top);
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0002F830 File Offset: 0x0002DA30
		public static NormalizedRGB BlendDodge(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendDodge(bottom.R, top.R), ColorBlending.BlendDodge(bottom.G, top.G), ColorBlending.BlendDodge(bottom.B, top.B), false, 5);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0002F86C File Offset: 0x0002DA6C
		public static double BlendDodge(double bottom, double top)
		{
			if (top >= 1.0)
			{
				return 1.0;
			}
			double num = bottom / (1.0 - top);
			if (num >= 1.0)
			{
				return 1.0;
			}
			return num;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0002F8B4 File Offset: 0x0002DAB4
		public static NormalizedRGB BlendLighten(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendLighten(bottom.R, top.R), ColorBlending.BlendLighten(bottom.G, top.G), ColorBlending.BlendLighten(bottom.B, top.B), false, 5);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0002F8F0 File Offset: 0x0002DAF0
		public static double BlendLighten(double bottom, double top)
		{
			return Math.Max(bottom, top);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0002F8F9 File Offset: 0x0002DAF9
		public static NormalizedRGB BlendMultiply(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendMultiply(bottom.R, top.R), ColorBlending.BlendMultiply(bottom.G, top.G), ColorBlending.BlendMultiply(bottom.B, top.B), false, 5);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0002F935 File Offset: 0x0002DB35
		public static double BlendMultiply(double bottom, double top)
		{
			return bottom * top;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0002F93A File Offset: 0x0002DB3A
		public static NormalizedRGB BlendOverlay(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendOverlay(bottom.R, top.R), ColorBlending.BlendOverlay(bottom.G, top.G), ColorBlending.BlendOverlay(bottom.B, top.B), false, 5);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0002F978 File Offset: 0x0002DB78
		public static double BlendOverlay(double bottom, double top)
		{
			if (bottom < 0.5)
			{
				return MathUtils.ClampToUnit(2.0 * top * bottom);
			}
			return MathUtils.ClampToUnit(1.0 - 2.0 * (1.0 - top) * (1.0 - bottom));
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0002F9D4 File Offset: 0x0002DBD4
		public static NormalizedRGB BlendScreen(in NormalizedRGB bottom, in NormalizedRGB top)
		{
			return new NormalizedRGB(ColorBlending.BlendScreen(bottom.R, top.R), ColorBlending.BlendScreen(bottom.G, top.G), ColorBlending.BlendScreen(bottom.B, top.B), false, 5);
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0002FA10 File Offset: 0x0002DC10
		public static double BlendScreen(double bottom, double top)
		{
			return 1.0 - (1.0 - top) * (1.0 - bottom);
		}

		// Token: 0x04000458 RID: 1112
		public const double DefaultSaturationConstant = 18.0;
	}
}
