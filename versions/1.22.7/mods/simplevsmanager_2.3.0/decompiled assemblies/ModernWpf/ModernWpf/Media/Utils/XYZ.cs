using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x0200023C RID: 572
	internal readonly struct XYZ : IEquatable<XYZ>
	{
		// Token: 0x060010B7 RID: 4279 RVA: 0x000305CC File Offset: 0x0002E7CC
		public XYZ(double x, double y, double z, bool round = true, int roundingPrecision = 5)
		{
			if (round)
			{
				this.X = Math.Round(x, roundingPrecision);
				this.Y = Math.Round(y, roundingPrecision);
				this.Z = Math.Round(z, roundingPrecision);
				return;
			}
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0003061D File Offset: 0x0002E81D
		public bool Equals(XYZ other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0003064C File Offset: 0x0002E84C
		public override bool Equals(object obj)
		{
			if (obj is XYZ)
			{
				XYZ xyz = (XYZ)obj;
				return this.X == xyz.X && this.Y == xyz.Y && this.Z == xyz.Z;
			}
			return base.Equals(obj);
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x000306A6 File Offset: 0x0002E8A6
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x000306CB File Offset: 0x0002E8CB
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.X, this.Y, this.Z);
		}

		// Token: 0x04000473 RID: 1139
		public const int DefaultRoundingPrecision = 5;

		// Token: 0x04000474 RID: 1140
		public readonly double X;

		// Token: 0x04000475 RID: 1141
		public readonly double Y;

		// Token: 0x04000476 RID: 1142
		public readonly double Z;
	}
}
