using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000029 RID: 41
	[NullableContext(1)]
	[Nullable(0)]
	public class MixedCodeDocumentFragmentList : IEnumerable
	{
		// Token: 0x06000266 RID: 614 RVA: 0x000119E5 File Offset: 0x0000FBE5
		internal MixedCodeDocumentFragmentList(MixedCodeDocument doc)
		{
			this._doc = doc;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000267 RID: 615 RVA: 0x000119FF File Offset: 0x0000FBFF
		public MixedCodeDocument Doc
		{
			get
			{
				return this._doc;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00011A07 File Offset: 0x0000FC07
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x17000089 RID: 137
		public MixedCodeDocumentFragment this[int index]
		{
			get
			{
				return this._items[index];
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00011A22 File Offset: 0x0000FC22
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00011A2A File Offset: 0x0000FC2A
		public void Append(MixedCodeDocumentFragment newFragment)
		{
			if (newFragment == null)
			{
				throw new ArgumentNullException("newFragment");
			}
			this._items.Add(newFragment);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00011A46 File Offset: 0x0000FC46
		public MixedCodeDocumentFragmentList.MixedCodeDocumentFragmentEnumerator GetEnumerator()
		{
			return new MixedCodeDocumentFragmentList.MixedCodeDocumentFragmentEnumerator(this._items);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00011A53 File Offset: 0x0000FC53
		public void Prepend(MixedCodeDocumentFragment newFragment)
		{
			if (newFragment == null)
			{
				throw new ArgumentNullException("newFragment");
			}
			this._items.Insert(0, newFragment);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00011A70 File Offset: 0x0000FC70
		public void Remove(MixedCodeDocumentFragment fragment)
		{
			if (fragment == null)
			{
				throw new ArgumentNullException("fragment");
			}
			int fragmentIndex = this.GetFragmentIndex(fragment);
			if (fragmentIndex == -1)
			{
				throw new IndexOutOfRangeException();
			}
			this.RemoveAt(fragmentIndex);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00011AA4 File Offset: 0x0000FCA4
		public void RemoveAll()
		{
			this._items.Clear();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00011AB1 File Offset: 0x0000FCB1
		public void RemoveAt(int index)
		{
			this._items.RemoveAt(index);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00011ABF File Offset: 0x0000FCBF
		internal void Clear()
		{
			this._items.Clear();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00011ACC File Offset: 0x0000FCCC
		internal int GetFragmentIndex(MixedCodeDocumentFragment fragment)
		{
			if (fragment == null)
			{
				throw new ArgumentNullException("fragment");
			}
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i] == fragment)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040000DB RID: 219
		private MixedCodeDocument _doc;

		// Token: 0x040000DC RID: 220
		private List<MixedCodeDocumentFragment> _items = new List<MixedCodeDocumentFragment>();

		// Token: 0x02000050 RID: 80
		[Nullable(0)]
		public class MixedCodeDocumentFragmentEnumerator : IEnumerator
		{
			// Token: 0x06000347 RID: 839 RVA: 0x000141F2 File Offset: 0x000123F2
			internal MixedCodeDocumentFragmentEnumerator(IList<MixedCodeDocumentFragment> items)
			{
				this._items = items;
				this._index = -1;
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x06000348 RID: 840 RVA: 0x00014208 File Offset: 0x00012408
			public MixedCodeDocumentFragment Current
			{
				get
				{
					return this._items[this._index];
				}
			}

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000349 RID: 841 RVA: 0x0001421B File Offset: 0x0001241B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600034A RID: 842 RVA: 0x00014223 File Offset: 0x00012423
			public bool MoveNext()
			{
				this._index++;
				return this._index < this._items.Count;
			}

			// Token: 0x0600034B RID: 843 RVA: 0x00014246 File Offset: 0x00012446
			public void Reset()
			{
				this._index = -1;
			}

			// Token: 0x0400018B RID: 395
			private int _index;

			// Token: 0x0400018C RID: 396
			private IList<MixedCodeDocumentFragment> _items;
		}
	}
}
