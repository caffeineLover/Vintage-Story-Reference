using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x020001A4 RID: 420
	internal class ClipperPolyNode
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x000337AF File Offset: 0x000319AF
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x000337B7 File Offset: 0x000319B7
		public ClipperPolyNode Parent { get; set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x000337C0 File Offset: 0x000319C0
		public bool IsHole
		{
			get
			{
				return this.IsHoleNode();
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x000337C8 File Offset: 0x000319C8
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x000337D0 File Offset: 0x000319D0
		public bool IsOpen { get; set; }

		// Token: 0x06000A38 RID: 2616 RVA: 0x000337DC File Offset: 0x000319DC
		private bool IsHoleNode()
		{
			bool flag = true;
			for (ClipperPolyNode parent = this.Parent; parent != null; parent = parent.Parent)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00033804 File Offset: 0x00031A04
		public int ChildCount
		{
			get
			{
				return this.Children.Count;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00033811 File Offset: 0x00031A11
		public List<ClipperIntPoint> Contour
		{
			get
			{
				return this.Polygon;
			}
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0003381C File Offset: 0x00031A1C
		internal void AddChild(ClipperPolyNode child)
		{
			int count = this.Children.Count;
			this.Children.Add(child);
			child.Parent = this;
			child.Index = count;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0003384F File Offset: 0x00031A4F
		public ClipperPolyNode GetNext()
		{
			if (this.Children.Count > 0)
			{
				return this.Children[0];
			}
			return this.GetNextSiblingUp();
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00033874 File Offset: 0x00031A74
		internal ClipperPolyNode GetNextSiblingUp()
		{
			if (this.Parent == null)
			{
				return null;
			}
			if (this.Index == this.Parent.Children.Count - 1)
			{
				return this.Parent.GetNextSiblingUp();
			}
			return this.Parent.Children[this.Index + 1];
		}

		// Token: 0x040004E8 RID: 1256
		internal List<ClipperIntPoint> Polygon = new List<ClipperIntPoint>();

		// Token: 0x040004E9 RID: 1257
		internal int Index;

		// Token: 0x040004EA RID: 1258
		internal ClipperJoinType JoinType;

		// Token: 0x040004EB RID: 1259
		internal ClipperEndType EndType;

		// Token: 0x040004EC RID: 1260
		internal List<ClipperPolyNode> Children = new List<ClipperPolyNode>();
	}
}
