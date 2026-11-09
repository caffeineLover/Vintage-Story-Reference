using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200008A RID: 138
	internal class IndexRange
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x00019A22 File Offset: 0x00017C22
		public IndexRange()
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00019A38 File Offset: 0x00017C38
		public IndexRange(int begin, int end)
		{
			if (begin > end)
			{
				int num = begin;
				begin = end;
				end = num;
			}
			this.m_begin = begin;
			this.m_end = end;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00019A66 File Offset: 0x00017C66
		public int Begin
		{
			get
			{
				return this.m_begin;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00019A6E File Offset: 0x00017C6E
		public int End
		{
			get
			{
				return this.m_end;
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00019A76 File Offset: 0x00017C76
		public bool Contains(int index)
		{
			return index >= this.m_begin && index <= this.m_end;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00019A90 File Offset: 0x00017C90
		public bool Split(int splitIndex, ref IndexRange before, ref IndexRange after)
		{
			before = new IndexRange(this.m_begin, splitIndex);
			bool result;
			if (splitIndex < this.m_end)
			{
				after = new IndexRange(splitIndex + 1, this.m_end);
				result = true;
			}
			else
			{
				after = new IndexRange();
				result = false;
			}
			return result;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00019AD2 File Offset: 0x00017CD2
		public bool Intersects(IndexRange other)
		{
			return this.m_begin <= other.End && this.m_end >= other.Begin;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00019AF8 File Offset: 0x00017CF8
		public override bool Equals(object obj)
		{
			IndexRange indexRange = obj as IndexRange;
			return indexRange != null && this == indexRange;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00019B18 File Offset: 0x00017D18
		public override int GetHashCode()
		{
			return (17 * 31 + this.m_begin.GetHashCode()) * 31 + this.m_end.GetHashCode();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00019B3A File Offset: 0x00017D3A
		public static bool operator ==(IndexRange lhs, IndexRange rhs)
		{
			return lhs.m_begin == rhs.m_begin && lhs.m_end == rhs.m_end;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00019B5A File Offset: 0x00017D5A
		public static bool operator !=(IndexRange lhs, IndexRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000333 RID: 819
		private readonly int m_begin = -1;

		// Token: 0x04000334 RID: 820
		private readonly int m_end = -1;
	}
}
