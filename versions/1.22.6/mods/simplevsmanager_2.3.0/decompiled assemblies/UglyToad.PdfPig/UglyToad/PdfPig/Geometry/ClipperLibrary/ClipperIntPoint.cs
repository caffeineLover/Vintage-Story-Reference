using System;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x020001A2 RID: 418
	internal struct ClipperIntPoint
	{
		// Token: 0x06000A28 RID: 2600 RVA: 0x00033632 File Offset: 0x00031832
		public ClipperIntPoint(long x, long y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00033642 File Offset: 0x00031842
		public ClipperIntPoint(double x, double y)
		{
			this = new ClipperIntPoint((long)x, (long)y);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0003364E File Offset: 0x0003184E
		public ClipperIntPoint(ClipperIntPoint pt)
		{
			this = new ClipperIntPoint(pt.X, pt.Y);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00033662 File Offset: 0x00031862
		public static bool operator ==(ClipperIntPoint a, ClipperIntPoint b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00033682 File Offset: 0x00031882
		public static bool operator !=(ClipperIntPoint a, ClipperIntPoint b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000336A8 File Offset: 0x000318A8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is ClipperIntPoint)
			{
				ClipperIntPoint clipperIntPoint = (ClipperIntPoint)obj;
				return this.X == clipperIntPoint.X && this.Y == clipperIntPoint.Y;
			}
			return false;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000336E9 File Offset: 0x000318E9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040004E4 RID: 1252
		public long X;

		// Token: 0x040004E5 RID: 1253
		public long Y;
	}
}
