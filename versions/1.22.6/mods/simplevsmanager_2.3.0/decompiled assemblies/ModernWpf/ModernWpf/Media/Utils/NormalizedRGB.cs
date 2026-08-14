using System;
using System.Windows.Media;

namespace ModernWpf.Media.Utils
{
	// Token: 0x02000237 RID: 567
	internal readonly struct NormalizedRGB : IEquatable<NormalizedRGB>
	{
		// Token: 0x0600109D RID: 4253 RVA: 0x0002FF0C File Offset: 0x0002E10C
		public NormalizedRGB(double r, double g, double b, bool round = true, int roundingPrecision = 5)
		{
			if (round)
			{
				this.R = Math.Round(r, roundingPrecision);
				this.G = Math.Round(g, roundingPrecision);
				this.B = Math.Round(b, roundingPrecision);
				return;
			}
			this.R = r;
			this.G = g;
			this.B = b;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0002FF60 File Offset: 0x0002E160
		public NormalizedRGB(in Color rgb, bool round = true, int roundingPrecision = 5)
		{
			Color color;
			if (round)
			{
				color = rgb;
				this.R = Math.Round((double)color.R / 255.0, roundingPrecision);
				color = rgb;
				this.G = Math.Round((double)color.G / 255.0, roundingPrecision);
				color = rgb;
				this.B = Math.Round((double)color.B / 255.0, roundingPrecision);
				return;
			}
			color = rgb;
			this.R = (double)color.R / 255.0;
			color = rgb;
			this.G = (double)color.G / 255.0;
			color = rgb;
			this.B = (double)color.B / 255.0;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00030040 File Offset: 0x0002E240
		public Color Denormalize(byte a = 255)
		{
			return Color.FromArgb(a, MathUtils.ClampToByte(this.R * 255.0), MathUtils.ClampToByte(this.G * 255.0), MathUtils.ClampToByte(this.B * 255.0));
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00030092 File Offset: 0x0002E292
		public bool Equals(NormalizedRGB other)
		{
			return this.R == other.R && this.G == other.G && this.B == other.B;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x000300C0 File Offset: 0x0002E2C0
		public override bool Equals(object obj)
		{
			if (obj is NormalizedRGB)
			{
				NormalizedRGB normalizedRGB = (NormalizedRGB)obj;
				return this.R == normalizedRGB.R && this.G == normalizedRGB.G && this.B == normalizedRGB.B;
			}
			return base.Equals(obj);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0003011A File Offset: 0x0002E31A
		public override int GetHashCode()
		{
			return this.R.GetHashCode() ^ this.G.GetHashCode() ^ this.B.GetHashCode();
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0003013F File Offset: 0x0002E33F
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.R, this.G, this.B);
		}

		// Token: 0x04000460 RID: 1120
		public const int DefaultRoundingPrecision = 5;

		// Token: 0x04000461 RID: 1121
		public readonly double R;

		// Token: 0x04000462 RID: 1122
		public readonly double G;

		// Token: 0x04000463 RID: 1123
		public readonly double B;
	}
}
