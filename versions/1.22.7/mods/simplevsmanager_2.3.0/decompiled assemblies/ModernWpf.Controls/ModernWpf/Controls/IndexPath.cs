using System;
using System.Collections.Generic;

namespace ModernWpf.Controls
{
	// Token: 0x02000089 RID: 137
	public sealed class IndexPath
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x000197F9 File Offset: 0x000179F9
		public IndexPath(int index)
		{
			this.m_path.Add(index);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00019818 File Offset: 0x00017A18
		public IndexPath(int groupIndex, int itemIndex)
		{
			this.m_path.Add(groupIndex);
			this.m_path.Add(itemIndex);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00019844 File Offset: 0x00017A44
		public IndexPath(IList<int> indices)
		{
			if (indices != null)
			{
				for (int i = 0; i < indices.Count; i++)
				{
					this.m_path.Add(indices[i]);
				}
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00019888 File Offset: 0x00017A88
		public static IndexPath CreateFrom(int index)
		{
			return new IndexPath(index);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00019890 File Offset: 0x00017A90
		public static IndexPath CreateFrom(int groupIndex, int itemIndex)
		{
			return new IndexPath(groupIndex, itemIndex);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00019899 File Offset: 0x00017A99
		public static IndexPath CreateFromIndices(IList<int> indices)
		{
			return new IndexPath(indices);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000198A1 File Offset: 0x00017AA1
		public int GetSize()
		{
			return this.m_path.Count;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000198AE File Offset: 0x00017AAE
		public int GetAt(int index)
		{
			return this.m_path[index];
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000198BC File Offset: 0x00017ABC
		public int CompareTo(IndexPath other)
		{
			int num = 0;
			int count = this.m_path.Count;
			int count2 = other.m_path.Count;
			if (count == 0 || count2 == 0)
			{
				num = count - count2;
			}
			else
			{
				for (int i = 0; i < Math.Min(count, count2); i++)
				{
					if (this.m_path[i] < other.m_path[i])
					{
						num = -1;
						break;
					}
					if (this.m_path[i] > other.m_path[i])
					{
						num = 1;
						break;
					}
				}
				num = ((num == 0) ? (count - count2) : num);
			}
			if (num != 0)
			{
				num = ((num > 0) ? 1 : -1);
			}
			return num;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00019960 File Offset: 0x00017B60
		public override string ToString()
		{
			string text = "R";
			foreach (int num in this.m_path)
			{
				text += ".";
				text += num.ToString();
			}
			return text;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000199D0 File Offset: 0x00017BD0
		internal bool IsValid()
		{
			bool result = true;
			for (int i = 0; i < this.m_path.Count; i++)
			{
				if (this.m_path[i] < 0)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00019A09 File Offset: 0x00017C09
		internal IndexPath CloneWithChildIndex(int childIndex)
		{
			List<int> list = new List<int>(this.m_path);
			list.Add(childIndex);
			return new IndexPath(list);
		}

		// Token: 0x04000332 RID: 818
		private readonly List<int> m_path = new List<int>();
	}
}
