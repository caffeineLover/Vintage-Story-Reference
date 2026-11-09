using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x020001A3 RID: 419
	internal class ClipperPolyTree : ClipperPolyNode
	{
		// Token: 0x06000A2F RID: 2607 RVA: 0x000336FC File Offset: 0x000318FC
		public void Clear()
		{
			for (int i = 0; i < this.AllPolys.Count; i++)
			{
				this.AllPolys[i] = null;
			}
			this.AllPolys.Clear();
			this.Children.Clear();
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00033742 File Offset: 0x00031942
		public ClipperPolyNode GetFirst()
		{
			if (this.Children.Count > 0)
			{
				return this.Children[0];
			}
			return null;
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00033760 File Offset: 0x00031960
		public int Total
		{
			get
			{
				int num = this.AllPolys.Count;
				if (num > 0 && this.Children[0] != this.AllPolys[0])
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x040004E6 RID: 1254
		internal List<ClipperPolyNode> AllPolys = new List<ClipperPolyNode>();
	}
}
