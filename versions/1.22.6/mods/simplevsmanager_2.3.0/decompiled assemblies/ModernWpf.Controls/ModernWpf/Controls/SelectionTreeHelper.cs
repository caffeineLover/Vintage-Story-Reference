using System;
using System.Collections.Generic;

namespace ModernWpf.Controls
{
	// Token: 0x020000D4 RID: 212
	internal class SelectionTreeHelper
	{
		// Token: 0x06000A8C RID: 2700 RVA: 0x00024164 File Offset: 0x00022364
		public static void TraverseIndexPath(SelectionNode root, IndexPath path, bool realizeChildren, Action<SelectionNode, IndexPath, int, int> nodeAction)
		{
			SelectionNode selectionNode = root;
			for (int i = 0; i < path.GetSize(); i++)
			{
				int at = path.GetAt(i);
				nodeAction.Invoke(selectionNode, path, i, at);
				if (i < path.GetSize() - 1)
				{
					selectionNode = selectionNode.GetAt(at, realizeChildren);
				}
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000241AC File Offset: 0x000223AC
		public static void Traverse(SelectionNode root, bool realizeChildren, Action<SelectionTreeHelper.TreeWalkNodeInfo> nodeAction)
		{
			List<SelectionTreeHelper.TreeWalkNodeInfo> list = new List<SelectionTreeHelper.TreeWalkNodeInfo>();
			IndexPath indexPath = new IndexPath(null);
			list.Add(new SelectionTreeHelper.TreeWalkNodeInfo(root, indexPath));
			while (list.Count > 0)
			{
				SelectionTreeHelper.TreeWalkNodeInfo treeWalkNodeInfo = list.Last<SelectionTreeHelper.TreeWalkNodeInfo>();
				list.RemoveLast<SelectionTreeHelper.TreeWalkNodeInfo>();
				for (int i = (realizeChildren ? treeWalkNodeInfo.Node.DataCount : treeWalkNodeInfo.Node.ChildrenNodeCount) - 1; i >= 0; i--)
				{
					SelectionNode at = treeWalkNodeInfo.Node.GetAt(i, realizeChildren);
					IndexPath indexPath2 = treeWalkNodeInfo.Path.CloneWithChildIndex(i);
					if (at != null)
					{
						list.Add(new SelectionTreeHelper.TreeWalkNodeInfo(at, indexPath2, treeWalkNodeInfo.Node));
					}
				}
				nodeAction.Invoke(treeWalkNodeInfo);
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00024250 File Offset: 0x00022450
		public static void TraverseRangeRealizeChildren(SelectionNode root, IndexPath start, IndexPath end, Action<SelectionTreeHelper.TreeWalkNodeInfo> nodeAction)
		{
			List<SelectionTreeHelper.TreeWalkNodeInfo> pendingNodes = new List<SelectionTreeHelper.TreeWalkNodeInfo>();
			IndexPath start2 = start;
			SelectionTreeHelper.TraverseIndexPath(root, start, true, delegate(SelectionNode node, IndexPath path, int depth, int childIndex)
			{
				IndexPath indexPath2 = SelectionTreeHelper.StartPath(path, depth);
				bool flag3 = SelectionTreeHelper.IsSubSet(start, indexPath2);
				bool flag4 = SelectionTreeHelper.IsSubSet(end, indexPath2);
				int num2 = (depth < start.GetSize() && flag3) ? Math.Max(0, start.GetAt(depth)) : 0;
				for (int j = (depth < end.GetSize() && flag4) ? Math.Min(node.DataCount - 1, end.GetAt(depth)) : (node.DataCount - 1); j >= num2; j--)
				{
					SelectionNode at2 = node.GetAt(j, true);
					if (at2 != null)
					{
						IndexPath indexPath3 = indexPath2.CloneWithChildIndex(j);
						pendingNodes.Add(new SelectionTreeHelper.TreeWalkNodeInfo(at2, indexPath3, node));
					}
				}
			});
			while (pendingNodes.Count > 0)
			{
				SelectionTreeHelper.TreeWalkNodeInfo treeWalkNodeInfo = pendingNodes.Last<SelectionTreeHelper.TreeWalkNodeInfo>();
				pendingNodes.RemoveLast<SelectionTreeHelper.TreeWalkNodeInfo>();
				int size = treeWalkNodeInfo.Path.GetSize();
				bool flag = SelectionTreeHelper.IsSubSet(start, treeWalkNodeInfo.Path);
				bool flag2 = SelectionTreeHelper.IsSubSet(end, treeWalkNodeInfo.Path);
				int num = (size < start.GetSize() && flag) ? start.GetAt(size) : 0;
				for (int i = (size < end.GetSize() && flag2) ? end.GetAt(size) : (treeWalkNodeInfo.Node.DataCount - 1); i >= num; i--)
				{
					SelectionNode at = treeWalkNodeInfo.Node.GetAt(i, true);
					if (at != null)
					{
						IndexPath indexPath = treeWalkNodeInfo.Path.CloneWithChildIndex(i);
						pendingNodes.Add(new SelectionTreeHelper.TreeWalkNodeInfo(at, indexPath, treeWalkNodeInfo.Node));
					}
				}
				nodeAction.Invoke(treeWalkNodeInfo);
				if (treeWalkNodeInfo.Path.CompareTo(end) == 0)
				{
					break;
				}
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x000243B4 File Offset: 0x000225B4
		private static bool IsSubSet(IndexPath path, IndexPath subset)
		{
			int size = subset.GetSize();
			if (path.GetSize() < size)
			{
				return false;
			}
			for (int i = 0; i < size; i++)
			{
				if (path.GetAt(i) != subset.GetAt(i))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000243F4 File Offset: 0x000225F4
		private static IndexPath StartPath(IndexPath path, int length)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < length; i++)
			{
				list.Add(path.GetAt(i));
			}
			return new IndexPath(list);
		}

		// Token: 0x02000129 RID: 297
		public struct TreeWalkNodeInfo
		{
			// Token: 0x06000D00 RID: 3328 RVA: 0x0002BA09 File Offset: 0x00029C09
			public TreeWalkNodeInfo(SelectionNode node, IndexPath indexPath, SelectionNode parent)
			{
				this.Node = node;
				this.Path = indexPath;
				this.ParentNode = parent;
			}

			// Token: 0x06000D01 RID: 3329 RVA: 0x0002BA20 File Offset: 0x00029C20
			public TreeWalkNodeInfo(SelectionNode node, IndexPath indexPath)
			{
				this = new SelectionTreeHelper.TreeWalkNodeInfo(node, indexPath, null);
			}

			// Token: 0x040005CF RID: 1487
			public SelectionNode Node;

			// Token: 0x040005D0 RID: 1488
			public IndexPath Path;

			// Token: 0x040005D1 RID: 1489
			public SelectionNode ParentNode;
		}
	}
}
