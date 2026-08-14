using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x02000238 RID: 568
	internal readonly struct HSL : IEquatable<HSL>
	{
		// Token: 0x060010A4 RID: 4260 RVA: 0x0003016C File Offset: 0x0002E36C
		public HSL(double h, double s, double l, bool round = true, int roundingPrecision = 5)
		{
			if (round)
			{
				this.H = Math.Round(h, roundingPrecision);
				this.S = Math.Round(s, roundingPrecision);
				this.L = Math.Round(l, roundingPrecision);
				return;
			}
			this.H = h;
			this.S = s;
			this.L = l;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x000301BD File Offset: 0x0002E3BD
		public bool Equals(HSL other)
		{
			return this.H == other.H && this.S == other.S && this.L == other.L;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000301EC File Offset: 0x0002E3EC
		public override bool Equals(object obj)
		{
			if (obj is HSL)
			{
				HSL hsl = (HSL)obj;
				return this.H == hsl.H && this.S == hsl.S && this.L == hsl.L;
			}
			return base.Equals(obj);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00030246 File Offset: 0x0002E446
		public override int GetHashCode()
		{
			return this.H.GetHashCode() ^ this.S.GetHashCode() ^ this.L.GetHashCode();
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0003026B File Offset: 0x0002E46B
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.H, this.S, this.L);
		}

		// Token: 0x04000464 RID: 1124
		public const int DefaultRoundingPrecision = 5;

		// Token: 0x04000465 RID: 1125
		public readonly double H;

		// Token: 0x04000466 RID: 1126
		public readonly double S;

		// Token: 0x04000467 RID: 1127
		public readonly double L;
	}
}
