using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace ModernWpf.Media.Utils
{
	// Token: 0x02000236 RID: 566
	internal class ColorScale
	{
		// Token: 0x06001099 RID: 4249 RVA: 0x0002FA60 File Offset: 0x0002DC60
		public ColorScale(IEnumerable<Color> colors)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("colors");
			}
			int num = Enumerable.Count<Color>(colors);
			this._stops = new ColorScaleStop[num];
			int num2 = 0;
			foreach (Color color in colors)
			{
				if (num2 == 0)
				{
					this._stops[num2] = new ColorScaleStop(color, 0.0);
				}
				else if (num2 == num - 1)
				{
					this._stops[num2] = new ColorScaleStop(color, 1.0);
				}
				else
				{
					this._stops[num2] = new ColorScaleStop(color, (double)num2 * (1.0 / (double)(num - 1)));
				}
				num2++;
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0002FB34 File Offset: 0x0002DD34
		public ColorScale(IEnumerable<ColorScaleStop> stops)
		{
			if (stops == null)
			{
				throw new ArgumentNullException("stops");
			}
			int num = Enumerable.Count<ColorScaleStop>(stops);
			this._stops = new ColorScaleStop[num];
			int num2 = 0;
			foreach (ColorScaleStop source in stops)
			{
				this._stops[num2] = new ColorScaleStop(source);
				num2++;
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
		public Color GetColor(double position, ColorScaleInterpolationMode mode = ColorScaleInterpolationMode.RGB)
		{
			if (this._stops.Length == 1)
			{
				return this._stops[0].Color;
			}
			if (position <= 0.0)
			{
				return this._stops[0].Color;
			}
			if (position >= 1.0)
			{
				return this._stops[this._stops.Length - 1].Color;
			}
			int num = 0;
			for (int i = 0; i < this._stops.Length; i++)
			{
				if (this._stops[i].Position <= position)
				{
					num = i;
				}
			}
			int num2 = num + 1;
			if (num2 >= this._stops.Length)
			{
				num2 = this._stops.Length - 1;
			}
			double position2 = (position - this._stops[num].Position) * (1.0 / (this._stops[num2].Position - this._stops[num].Position));
			if (mode == ColorScaleInterpolationMode.LAB)
			{
				LAB lab = ColorUtils.RGBToLAB(this._stops[num].Color, false, 5);
				LAB lab2 = ColorUtils.RGBToLAB(this._stops[num2].Color, false, 5);
				LAB lab3 = ColorUtils.InterpolateLAB(lab, lab2, position2);
				return ColorUtils.LABToRGB(lab3, false, 5).Denormalize(byte.MaxValue);
			}
			if (mode != ColorScaleInterpolationMode.XYZ)
			{
				return ColorUtils.InterpolateRGB(this._stops[num].Color, this._stops[num2].Color, position2);
			}
			XYZ xyz = ColorUtils.RGBToXYZ(this._stops[num].Color, false, 5);
			XYZ xyz2 = ColorUtils.RGBToXYZ(this._stops[num2].Color, false, 5);
			XYZ xyz3 = ColorUtils.InterpolateXYZ(xyz, xyz2, position2);
			return ColorUtils.XYZToRGB(xyz3, false, 5).Denormalize(byte.MaxValue);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0002FD90 File Offset: 0x0002DF90
		public ColorScale Trim(double lowerBound, double upperBound, ColorScaleInterpolationMode mode = ColorScaleInterpolationMode.RGB)
		{
			if (lowerBound < 0.0 || upperBound > 1.0 || upperBound < lowerBound)
			{
				throw new ArgumentException("Invalid bounds");
			}
			if (lowerBound == upperBound)
			{
				return new ColorScale(new Color[]
				{
					this.GetColor(lowerBound, mode)
				});
			}
			List<ColorScaleStop> list = new List<ColorScaleStop>(this._stops.Length);
			for (int i = 0; i < this._stops.Length; i++)
			{
				if (this._stops[i].Position >= lowerBound && this._stops[i].Position <= upperBound)
				{
					list.Add(this._stops[i]);
				}
			}
			if (list.Count == 0)
			{
				return new ColorScale(new Color[]
				{
					this.GetColor(lowerBound, mode),
					this.GetColor(upperBound, mode)
				});
			}
			if (Enumerable.First<ColorScaleStop>(list).Position != lowerBound)
			{
				list.Insert(0, new ColorScaleStop(this.GetColor(lowerBound, mode), lowerBound));
			}
			if (Enumerable.Last<ColorScaleStop>(list).Position != upperBound)
			{
				list.Add(new ColorScaleStop(this.GetColor(upperBound, mode), upperBound));
			}
			double num = upperBound - lowerBound;
			ColorScaleStop[] array = new ColorScaleStop[list.Count];
			for (int j = 0; j < array.Length; j++)
			{
				double position = (list[j].Position - lowerBound) / num;
				array[j] = new ColorScaleStop(list[j].Color, position);
			}
			return new ColorScale(array);
		}

		// Token: 0x0400045F RID: 1119
		private readonly ColorScaleStop[] _stops;
	}
}
