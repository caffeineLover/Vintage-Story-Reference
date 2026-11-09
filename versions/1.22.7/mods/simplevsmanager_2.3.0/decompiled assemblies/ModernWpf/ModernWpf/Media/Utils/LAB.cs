using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x0200023A RID: 570
	internal readonly struct LAB : IEquatable<LAB>
	{
		// Token: 0x060010AD RID: 4269 RVA: 0x00030374 File Offset: 0x0002E574
		public LAB(double l, double a, double b, bool round = true, int roundingPrecision = 5)
		{
			if (round)
			{
				this.L = Math.Round(l, roundingPrecision);
				this.A = Math.Round(a, roundingPrecision);
				this.B = Math.Round(b, roundingPrecision);
				return;
			}
			this.L = l;
			this.A = a;
			this.B = b;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x000303C5 File Offset: 0x0002E5C5
		public bool Equals(LAB other)
		{
			return this.L == other.L && this.A == other.A && this.B == other.B;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x000303F4 File Offset: 0x0002E5F4
		public override bool Equals(object obj)
		{
			if (obj is LAB)
			{
				LAB lab = (LAB)obj;
				return this.L == lab.L && this.A == lab.A && this.B == lab.B;
			}
			return base.Equals(obj);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0003044E File Offset: 0x0002E64E
		public override int GetHashCode()
		{
			return this.L.GetHashCode() ^ this.A.GetHashCode() ^ this.B.GetHashCode();
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00030473 File Offset: 0x0002E673
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.L, this.A, this.B);
		}

		// Token: 0x0400046B RID: 1131
		public const int DefaultRoundingPrecision = 5;

		// Token: 0x0400046C RID: 1132
		public readonly double L;

		// Token: 0x0400046D RID: 1133
		public readonly double A;

		// Token: 0x0400046E RID: 1134
		public readonly double B;
	}
}
