using System;
using System.Collections.Generic;
using System.Windows.Media;
using ModernWpf.Media.Utils;

namespace ModernWpf.Media.ColorPalette
{
	// Token: 0x0200023F RID: 575
	internal class ColorPalette
	{
		// Token: 0x060010D9 RID: 4313 RVA: 0x0003117A File Offset: 0x0002F37A
		public ColorPalette(int steps, Color baseColor) : this(steps, new ColorPaletteEntry(baseColor))
		{
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0003118C File Offset: 0x0002F38C
		public ColorPalette(int steps, IColorPaletteEntry baseColor)
		{
			if (baseColor == null)
			{
				throw new ArgumentNullException("baseColor");
			}
			if (this._steps <= 0)
			{
				throw new ArgumentException("steps must > 0");
			}
			this._steps = steps;
			this._baseColor = baseColor;
			ColorScale paletteScale = this.GetPaletteScale();
			this._palette = new List<ColorPaletteEntry>(this._steps);
			for (int i = 0; i < this._steps; i++)
			{
				Color color = paletteScale.GetColor((double)i / (double)(this._steps - 1), this._interpolationMode);
				this._palette.Add(new ColorPaletteEntry(color));
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x000312B6 File Offset: 0x0002F4B6
		public IReadOnlyList<ColorPaletteEntry> Palette
		{
			get
			{
				return this._palette;
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x000312C0 File Offset: 0x0002F4C0
		public ColorScale GetPaletteScale()
		{
			Color activeColor = this._baseColor.ActiveColor;
			HSL hsl = ColorUtils.RGBToHSL(activeColor, true, 5);
			NormalizedRGB normalizedRGB = new NormalizedRGB(ref activeColor, true, 5);
			ColorScale colorScale = new ColorScale(new Color[]
			{
				this._scaleColorLight,
				activeColor,
				this._scaleColorDark
			}).Trim(this._clipLight, 1.0 - this._clipDark, ColorScaleInterpolationMode.RGB);
			Color color = colorScale.GetColor(0.0, this._interpolationMode);
			NormalizedRGB normalizedRGB2 = new NormalizedRGB(ref color, true, 5);
			color = colorScale.GetColor(1.0, this._interpolationMode);
			NormalizedRGB normalizedRGB3 = new NormalizedRGB(ref color, true, 5);
			NormalizedRGB normalizedRGB4 = normalizedRGB2;
			NormalizedRGB normalizedRGB5 = normalizedRGB3;
			if (hsl.S >= this._saturationAdjustmentCutoff)
			{
				normalizedRGB4 = ColorBlending.SaturateViaLCH(normalizedRGB4, this._saturationLight, 18.0);
				normalizedRGB5 = ColorBlending.SaturateViaLCH(normalizedRGB5, this._saturationDark, 18.0);
			}
			if (this._multiplyLight != 0.0)
			{
				NormalizedRGB normalizedRGB6 = ColorBlending.Blend(normalizedRGB, normalizedRGB4, ColorBlendMode.Multiply);
				normalizedRGB4 = ColorUtils.InterpolateColor(normalizedRGB4, normalizedRGB6, this._multiplyLight, this._interpolationMode);
			}
			if (this._multiplyDark != 0.0)
			{
				NormalizedRGB normalizedRGB7 = ColorBlending.Blend(normalizedRGB, normalizedRGB5, ColorBlendMode.Multiply);
				normalizedRGB5 = ColorUtils.InterpolateColor(normalizedRGB5, normalizedRGB7, this._multiplyDark, this._interpolationMode);
			}
			if (this._overlayLight != 0.0)
			{
				NormalizedRGB normalizedRGB8 = ColorBlending.Blend(normalizedRGB, normalizedRGB4, ColorBlendMode.Overlay);
				normalizedRGB4 = ColorUtils.InterpolateColor(normalizedRGB4, normalizedRGB8, this._overlayLight, this._interpolationMode);
			}
			if (this._overlayDark != 0.0)
			{
				NormalizedRGB normalizedRGB9 = ColorBlending.Blend(normalizedRGB, normalizedRGB5, ColorBlendMode.Overlay);
				normalizedRGB5 = ColorUtils.InterpolateColor(normalizedRGB5, normalizedRGB9, this._overlayDark, this._interpolationMode);
			}
			return new ColorScale(new Color[]
			{
				normalizedRGB4.Denormalize(byte.MaxValue),
				activeColor,
				normalizedRGB5.Denormalize(byte.MaxValue)
			});
		}

		// Token: 0x04000478 RID: 1144
		protected readonly IColorPaletteEntry _baseColor;

		// Token: 0x04000479 RID: 1145
		protected readonly int _steps = 11;

		// Token: 0x0400047A RID: 1146
		protected readonly List<ColorPaletteEntry> _palette;

		// Token: 0x0400047B RID: 1147
		protected ColorScaleInterpolationMode _interpolationMode;

		// Token: 0x0400047C RID: 1148
		protected Color _scaleColorLight = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400047D RID: 1149
		protected Color _scaleColorDark = Color.FromArgb(byte.MaxValue, 0, 0, 0);

		// Token: 0x0400047E RID: 1150
		protected double _clipLight = 0.185;

		// Token: 0x0400047F RID: 1151
		protected double _clipDark = 0.16;

		// Token: 0x04000480 RID: 1152
		protected double _saturationAdjustmentCutoff = 0.05;

		// Token: 0x04000481 RID: 1153
		protected double _saturationLight = 0.35;

		// Token: 0x04000482 RID: 1154
		protected double _saturationDark = 1.25;

		// Token: 0x04000483 RID: 1155
		protected double _overlayLight;

		// Token: 0x04000484 RID: 1156
		protected double _overlayDark = 0.25;

		// Token: 0x04000485 RID: 1157
		protected double _multiplyLight;

		// Token: 0x04000486 RID: 1158
		protected double _multiplyDark;
	}
}
