using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x02000239 RID: 569
	internal readonly struct HSV : IEquatable<HSV>
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x00030298 File Offset: 0x0002E498
		public bool Equals(HSV other)
		{
			return this.H == other.H && this.S == other.S && this.V == other.V;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x000302C8 File Offset: 0x0002E4C8
		public override bool Equals(object obj)
		{
			if (obj is HSV)
			{
				HSV hsv = (HSV)obj;
				return this.H == hsv.H && this.S == hsv.S && this.V == hsv.V;
			}
			return base.Equals(obj);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00030322 File Offset: 0x0002E522
		public override int GetHashCode()
		{
			return this.H.GetHashCode() ^ this.S.GetHashCode() ^ this.V.GetHashCode();
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00030347 File Offset: 0x0002E547
		public override string ToString()
		{
			return string.Format("{0},{1},{2}", this.H, this.S, this.V);
		}

		// Token: 0x04000468 RID: 1128
		public readonly double H;

		// Token: 0x04000469 RID: 1129
		public readonly double S;

		// Token: 0x0400046A RID: 1130
		public readonly double V;
	}
}
