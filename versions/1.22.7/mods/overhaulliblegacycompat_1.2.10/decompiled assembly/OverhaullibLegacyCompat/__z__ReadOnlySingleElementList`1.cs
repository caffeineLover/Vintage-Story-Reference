using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000003 RID: 3
[CompilerGenerated]
internal sealed class <>z__ReadOnlySingleElementList<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
	public <>z__ReadOnlySingleElementList(T item)
	{
		this._item = item;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000205F File Offset: 0x0000025F
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new <>z__ReadOnlySingleElementList<T>.Enumerator(this._item);
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206C File Offset: 0x0000026C
	int ICollection.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000006 RID: 6 RVA: 0x0000206F File Offset: 0x0000026F
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000007 RID: 7 RVA: 0x00002072 File Offset: 0x00000272
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002075 File Offset: 0x00000275
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		array.SetValue(this._item, index);
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000009 RID: 9 RVA: 0x00002089 File Offset: 0x00000289
	// (set) Token: 0x0600000A RID: 10 RVA: 0x0000209F File Offset: 0x0000029F
	object IList.this[]
	{
		[return: Nullable(2)]
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this._item;
		}
		[param: Nullable(2)]
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600000B RID: 11 RVA: 0x000020A6 File Offset: 0x000002A6
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000C RID: 12 RVA: 0x000020A9 File Offset: 0x000002A9
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000020AC File Offset: 0x000002AC
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000020B3 File Offset: 0x000002B3
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000020BA File Offset: 0x000002BA
	bool IList.Contains([Nullable(2)] object value)
	{
		return EqualityComparer<T>.Default.Equals(this._item, (T)((object)value));
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000020D2 File Offset: 0x000002D2
	int IList.IndexOf([Nullable(2)] object value)
	{
		if (!EqualityComparer<T>.Default.Equals(this._item, (T)((object)value)))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000020EF File Offset: 0x000002EF
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000020F6 File Offset: 0x000002F6
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000020FD File Offset: 0x000002FD
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002104 File Offset: 0x00000304
	[return: Nullable(new byte[]
	{
		1,
		0
	})]
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return new <>z__ReadOnlySingleElementList<T>.Enumerator(this._item);
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000015 RID: 21 RVA: 0x00002111 File Offset: 0x00000311
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000016 RID: 22 RVA: 0x00002114 File Offset: 0x00000314
	T IReadOnlyList<!0>.this[]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this._item;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000017 RID: 23 RVA: 0x00002125 File Offset: 0x00000325
	int ICollection<!0>.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000018 RID: 24 RVA: 0x00002128 File Offset: 0x00000328
	bool ICollection<!0>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000212B File Offset: 0x0000032B
	void ICollection<!0>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002132 File Offset: 0x00000332
	void ICollection<!0>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002139 File Offset: 0x00000339
	bool ICollection<!0>.Contains(T item)
	{
		return EqualityComparer<T>.Default.Equals(this._item, item);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0000214C File Offset: 0x0000034C
	void ICollection<!0>.CopyTo([Nullable(new byte[]
	{
		1,
		0
	})] T[] array, int arrayIndex)
	{
		array[arrayIndex] = this._item;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000215B File Offset: 0x0000035B
	bool ICollection<!0>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600001E RID: 30 RVA: 0x00002162 File Offset: 0x00000362
	// (set) Token: 0x0600001F RID: 31 RVA: 0x00002173 File Offset: 0x00000373
	T IList<!0>.this[]
	{
		get
		{
			if (index != 0)
			{
				throw new IndexOutOfRangeException();
			}
			return this._item;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000217A File Offset: 0x0000037A
	int IList<!0>.IndexOf(T item)
	{
		if (!EqualityComparer<T>.Default.Equals(this._item, item))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002192 File Offset: 0x00000392
	void IList<!0>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002199 File Offset: 0x00000399
	void IList<!0>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x04000001 RID: 1
	[CompilerGenerated]
	private readonly T _item;

	// Token: 0x020001B5 RID: 437
	private sealed class Enumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		// Token: 0x060014DE RID: 5342 RVA: 0x00067DBC File Offset: 0x00065FBC
		public Enumerator(T item)
		{
			this._item = item;
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x00067DCB File Offset: 0x00065FCB
		object IEnumerator.Current
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x00067DD8 File Offset: 0x00065FD8
		T IEnumerator<!0>.Current
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00067DE0 File Offset: 0x00065FE0
		bool IEnumerator.MoveNext()
		{
			return !this._moveNextCalled && (this._moveNextCalled = true);
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00067E01 File Offset: 0x00066001
		void IEnumerator.Reset()
		{
			this._moveNextCalled = false;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00067E0A File Offset: 0x0006600A
		void IDisposable.Dispose()
		{
		}

		// Token: 0x04000A92 RID: 2706
		[CompilerGenerated]
		private readonly T _item;

		// Token: 0x04000A93 RID: 2707
		[CompilerGenerated]
		private bool _moveNextCalled;
	}
}
