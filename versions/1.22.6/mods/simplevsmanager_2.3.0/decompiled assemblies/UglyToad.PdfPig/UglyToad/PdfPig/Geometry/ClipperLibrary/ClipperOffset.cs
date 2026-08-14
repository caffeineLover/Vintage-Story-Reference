using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x0200019E RID: 414
	internal class ClipperOffset
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00033287 File Offset: 0x00031487
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0003328F File Offset: 0x0003148F
		public double ArcTolerance { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00033298 File Offset: 0x00031498
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x000332A0 File Offset: 0x000314A0
		public double MiterLimit { get; set; }

		// Token: 0x06000A1D RID: 2589 RVA: 0x000332A9 File Offset: 0x000314A9
		public ClipperOffset(double miterLimit = 2.0, double arcTolerance = 0.25)
		{
			this.MiterLimit = miterLimit;
			this.ArcTolerance = arcTolerance;
			this.lowest.X = -1L;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x000332D7 File Offset: 0x000314D7
		public void Clear()
		{
			this.polyNodes.Children.Clear();
			this.lowest.X = -1L;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000332F8 File Offset: 0x000314F8
		public void AddPath(List<ClipperIntPoint> path, ClipperJoinType joinType, ClipperEndType endType)
		{
			int num = path.Count - 1;
			if (num < 0)
			{
				return;
			}
			ClipperPolyNode clipperPolyNode = new ClipperPolyNode
			{
				JoinType = joinType,
				EndType = endType
			};
			if (endType != ClipperEndType.ClosedLine)
			{
				if (endType != ClipperEndType.ClosedPolygon)
				{
					goto IL_48;
				}
			}
			while (num > 0 && path[0] == path[num])
			{
				num--;
			}
			IL_48:
			clipperPolyNode.Polygon.Capacity = num + 1;
			clipperPolyNode.Polygon.Add(path[0]);
			int num2 = 0;
			int num3 = 0;
			for (int i = 1; i <= num; i++)
			{
				if (clipperPolyNode.Polygon[num2] != path[i])
				{
					num2++;
					clipperPolyNode.Polygon.Add(path[i]);
					if (path[i].Y > clipperPolyNode.Polygon[num3].Y || (path[i].Y == clipperPolyNode.Polygon[num3].Y && path[i].X < clipperPolyNode.Polygon[num3].X))
					{
						num3 = num2;
					}
				}
			}
			if (endType == ClipperEndType.ClosedPolygon && num2 < 2)
			{
				return;
			}
			this.polyNodes.AddChild(clipperPolyNode);
			if (endType != ClipperEndType.ClosedPolygon)
			{
				return;
			}
			if (this.lowest.X < 0L)
			{
				this.lowest = new ClipperIntPoint((long)(this.polyNodes.ChildCount - 1), (long)num3);
				return;
			}
			ClipperIntPoint clipperIntPoint = this.polyNodes.Children[(int)this.lowest.X].Polygon[(int)this.lowest.Y];
			if (clipperPolyNode.Polygon[num3].Y > clipperIntPoint.Y || (clipperPolyNode.Polygon[num3].Y == clipperIntPoint.Y && clipperPolyNode.Polygon[num3].X < clipperIntPoint.X))
			{
				this.lowest = new ClipperIntPoint((long)(this.polyNodes.ChildCount - 1), (long)num3);
			}
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000334FC File Offset: 0x000316FC
		public void AddPaths(List<List<ClipperIntPoint>> paths, ClipperJoinType joinType, ClipperEndType endType)
		{
			foreach (List<ClipperIntPoint> path in paths)
			{
				this.AddPath(path, joinType, endType);
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0003354C File Offset: 0x0003174C
		public static long Round(double value)
		{
			if (value >= 0.0)
			{
				return (long)(value + 0.5);
			}
			return (long)(value - 0.5);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00033574 File Offset: 0x00031774
		public static ClipperDoublePoint GetUnitNormal(ClipperIntPoint pt1, ClipperIntPoint pt2)
		{
			double num = (double)(pt2.X - pt1.X);
			double num2 = (double)(pt2.Y - pt1.Y);
			if (num == 0.0 && num2 == 0.0)
			{
				return default(ClipperDoublePoint);
			}
			double num3 = 1.0 / Math.Sqrt(num * num + num2 * num2);
			num *= num3;
			num2 *= num3;
			return new ClipperDoublePoint(num2, -num);
		}

		// Token: 0x040004D2 RID: 1234
		private const double DefArcTolerance = 0.25;

		// Token: 0x040004D3 RID: 1235
		private ClipperIntPoint lowest;

		// Token: 0x040004D4 RID: 1236
		private readonly ClipperPolyNode polyNodes = new ClipperPolyNode();
	}
}
