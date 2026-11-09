using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000D7 RID: 215
	public abstract class BookmarkNode
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x000198ED File Offset: 0x00017AED
		public string Title { get; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x000198F5 File Offset: 0x00017AF5
		public IReadOnlyList<BookmarkNode> Children { get; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x000198FD File Offset: 0x00017AFD
		public bool IsLeaf { get; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00019905 File Offset: 0x00017B05
		public int Level { get; }

		// Token: 0x0600040D RID: 1037 RVA: 0x0001990D File Offset: 0x00017B0D
		protected BookmarkNode(string title, int level, IReadOnlyList<BookmarkNode> children)
		{
			this.Title = title;
			this.Level = level;
			if (children == null)
			{
				throw new ArgumentNullException("children");
			}
			this.Children = children;
			this.IsLeaf = (children.Count == 0);
		}
	}
}
