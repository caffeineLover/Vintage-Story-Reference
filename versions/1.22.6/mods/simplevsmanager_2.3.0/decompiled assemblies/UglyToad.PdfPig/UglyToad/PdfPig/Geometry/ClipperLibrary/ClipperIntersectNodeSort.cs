using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x02000199 RID: 409
	internal class ClipperIntersectNodeSort : IComparer<ClipperIntersectNode>
	{
		// Token: 0x06000A12 RID: 2578 RVA: 0x000331F4 File Offset: 0x000313F4
		public int Compare(ClipperIntersectNode node1, ClipperIntersectNode node2)
		{
			long num = node2.Pt.Y - node1.Pt.Y;
			if (num > 0L)
			{
				return 1;
			}
			if (num >= 0L)
			{
				return 0;
			}
			return -1;
		}
	}
}
