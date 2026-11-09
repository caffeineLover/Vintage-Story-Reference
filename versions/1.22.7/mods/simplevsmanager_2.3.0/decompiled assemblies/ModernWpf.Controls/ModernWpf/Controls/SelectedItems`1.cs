using System;
using System.Collections;
using System.Collections.Generic;

namespace ModernWpf.Controls
{
	// Token: 0x020000CD RID: 205
	internal class SelectedItems<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
	{
		// Token: 0x06000A24 RID: 2596 RVA: 0x00022914 File Offset: 0x00020B14
		public SelectedItems(List<SelectedItemInfo> infos, Func<List<SelectedItemInfo>, int, T> getAtImpl)
		{
			this.m_infos = infos;
			this.m_getAtImpl = getAtImpl;
			using (List<SelectedItemInfo>.Enumerator enumerator = infos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SelectionNode selectionNode;
					if (!enumerator.Current.Node.TryGetTarget(ref selectionNode))
					{
						throw new Exception("Selection changed after the SelectedIndices/Items property was read.");
					}
					this.m_totalCount += selectionNode.SelectedCount;
				}
			}
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0002299C File Offset: 0x00020B9C
		~SelectedItems()
		{
			this.m_infos.Clear();
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x000229D0 File Offset: 0x00020BD0
		public int Count
		{
			get
			{
				return this.m_totalCount;
			}
		}

		// Token: 0x170001B6 RID: 438
		public T this[int index]
		{
			get
			{
				return this.m_getAtImpl.Invoke(this.m_infos, index);
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x000229EC File Offset: 0x00020BEC
		public IEnumerator<T> GetEnumerator()
		{
			return new SelectedItems<T>.Enumerator(this);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x000229F4 File Offset: 0x00020BF4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000426 RID: 1062
		private List<SelectedItemInfo> m_infos;

		// Token: 0x04000427 RID: 1063
		private int m_totalCount;

		// Token: 0x04000428 RID: 1064
		private Func<List<SelectedItemInfo>, int, T> m_getAtImpl;

		// Token: 0x02000123 RID: 291
		private class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000CEB RID: 3307 RVA: 0x0002B6ED File Offset: 0x000298ED
			public Enumerator(IReadOnlyList<T> selectedItems)
			{
				this.m_currentIndex = -1;
				base..ctor();
				this.m_selectedItems = selectedItems;
			}

			// Token: 0x06000CEC RID: 3308 RVA: 0x0002B703 File Offset: 0x00029903
			public void Dispose()
			{
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000CED RID: 3309 RVA: 0x0002B708 File Offset: 0x00029908
			public T Current
			{
				get
				{
					IReadOnlyList<T> selectedItems = this.m_selectedItems;
					if (this.m_currentIndex < selectedItems.Count)
					{
						return selectedItems[this.m_currentIndex];
					}
					throw new IndexOutOfRangeException();
				}
			}

			// Token: 0x1700024C RID: 588
			// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0002B73C File Offset: 0x0002993C
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000CEF RID: 3311 RVA: 0x0002B749 File Offset: 0x00029949
			public bool MoveNext()
			{
				if (this.m_currentIndex < this.m_selectedItems.Count)
				{
					this.m_currentIndex++;
					return this.m_currentIndex < this.m_selectedItems.Count;
				}
				throw new IndexOutOfRangeException();
			}

			// Token: 0x06000CF0 RID: 3312 RVA: 0x0002B785 File Offset: 0x00029985
			public void Reset()
			{
				this.m_currentIndex = 1;
			}

			// Token: 0x040005C1 RID: 1473
			private readonly IReadOnlyList<T> m_selectedItems;

			// Token: 0x040005C2 RID: 1474
			private int m_currentIndex;
		}
	}
}
