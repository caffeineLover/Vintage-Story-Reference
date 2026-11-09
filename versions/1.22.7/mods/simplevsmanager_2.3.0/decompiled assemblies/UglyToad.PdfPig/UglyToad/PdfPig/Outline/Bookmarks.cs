using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000D8 RID: 216
	public class Bookmarks
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00019948 File Offset: 0x00017B48
		public IReadOnlyList<BookmarkNode> Roots { get; }

		// Token: 0x0600040F RID: 1039 RVA: 0x00019950 File Offset: 0x00017B50
		public Bookmarks(IReadOnlyList<BookmarkNode> roots)
		{
			this.Roots = roots;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001995F File Offset: 0x00017B5F
		public IEnumerable<BookmarkNode> GetNodes()
		{
			foreach (BookmarkNode node in this.Roots)
			{
				foreach (BookmarkNode bookmarkNode in Bookmarks.GetNodes(node))
				{
					yield return bookmarkNode;
				}
				IEnumerator<BookmarkNode> enumerator2 = null;
			}
			IEnumerator<BookmarkNode> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001996F File Offset: 0x00017B6F
		private static IEnumerable<BookmarkNode> GetNodes(BookmarkNode node)
		{
			yield return node;
			foreach (BookmarkNode node2 in node.Children)
			{
				foreach (BookmarkNode bookmarkNode in Bookmarks.GetNodes(node2))
				{
					yield return bookmarkNode;
				}
				IEnumerator<BookmarkNode> enumerator2 = null;
			}
			IEnumerator<BookmarkNode> enumerator = null;
			yield break;
			yield break;
		}
	}
}
