using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x0200023B RID: 571
	internal readonly struct LCH : IEquatable<LCH>
	{
		// Token: 0x060010B2 RID: 4274 RVA: 0x000304A0 File Offset: 0x0002E6A0
		public LCH(double l, double c, double h, bool round = true, int roundingPrecision = 5)
		{
			if (round)
			{
				this.L = Math.Round(l, roundingPrecision);
				this.C = Math.Round(c, roundingPrecision);
				this.H = Math.Round(h, roundingPrecision);
				return;
			}
			this.L = l;
			this.C = c;
			this.H = h;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x000304F1 File Offset: 0x0002E6F1
		public bool Equals(LCH other)
		{
			return this.L == other.L && this.C == other.C && this.H == other.H;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00030520 File Offset: 0x0002E720
		public override bool Equals(object obj)
		{
			if (obj is LCH)
			{
				LCH lch = (LCH)obj;
				return this.L == lch.L && this.C == lch.C && this.H == lch.H;
			}
			return base.Equals(obj);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0003057A File Offset: 0x0002E77A
		public override int GetHashCode()
		{
			return this.L.GetHashCode() ^ this.C.GetHashCode() ^ this.H.GetHashCode();
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0003059F File Offset: 0x0002E79F
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.L, this.C, this.H);
		}

		// Token: 0x0400046F RID: 1135
		public const int DefaultRoundingPrecision = 5;

		// Token: 0x04000470 RID: 1136
		public readonly double L;

		// Token: 0x04000471 RID: 1137
		public readonly double C;

		// Token: 0x04000472 RID: 1138
		public readonly double H;
	}
}
