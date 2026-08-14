using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000059 RID: 89
[CompilerGenerated]
internal sealed class <>z__ReadOnlySingleElementList<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x0600028B RID: 651 RVA: 0x0000E394 File Offset: 0x0000C594
	public <>z__ReadOnlySingleElementList(T item)
	{
		this._item = item;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000E3A3 File Offset: 0x0000C5A3
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return new <>z__ReadOnlySingleElementList<T>.Enumerator(this._item);
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600028D RID: 653 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
	int ICollection.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600028E RID: 654 RVA: 0x0000D44C File Offset: 0x0000B64C
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600028F RID: 655 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000E3B3 File Offset: 0x0000C5B3
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		array.SetValue(this._item, index);
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000291 RID: 657 RVA: 0x0000E3C7 File Offset: 0x0000C5C7
	// (set) Token: 0x06000292 RID: 658 RVA: 0x0000E3DD File Offset: 0x0000C5DD
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

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000293 RID: 659 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000294 RID: 660 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
	bool IList.Contains([Nullable(2)] object value)
	{
		return EqualityComparer<T>.Default.Equals(this._item, (T)((object)value));
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000E3FC File Offset: 0x0000C5FC
	int IList.IndexOf([Nullable(2)] object value)
	{
		if (!EqualityComparer<T>.Default.Equals(this._item, (T)((object)value)))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000E3A3 File Offset: 0x0000C5A3
	[return: Nullable(new byte[]
	{
		1,
		0
	})]
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return new <>z__ReadOnlySingleElementList<T>.Enumerator(this._item);
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600029D RID: 669 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600029E RID: 670 RVA: 0x0000E419 File Offset: 0x0000C619
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

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600029F RID: 671 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
	int ICollection<!0>.Count
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
	bool ICollection<!0>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void ICollection<!0>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void ICollection<!0>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0000E42A File Offset: 0x0000C62A
	bool ICollection<!0>.Contains(T item)
	{
		return EqualityComparer<T>.Default.Equals(this._item, item);
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0000E43D File Offset: 0x0000C63D
	void ICollection<!0>.CopyTo([Nullable(new byte[]
	{
		1,
		0
	})] T[] array, int arrayIndex)
	{
		array[arrayIndex] = this._item;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	bool ICollection<!0>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000E419 File Offset: 0x0000C619
	// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000E3DD File Offset: 0x0000C5DD
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

	// Token: 0x060002A8 RID: 680 RVA: 0x0000E44C File Offset: 0x0000C64C
	int IList<!0>.IndexOf(T item)
	{
		if (!EqualityComparer<T>.Default.Equals(this._item, item))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void IList<!0>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	void IList<!0>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x04000154 RID: 340
	[CompilerGenerated]
	private readonly T _item;

	// Token: 0x0200005A RID: 90
	private sealed class Enumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000E464 File Offset: 0x0000C664
		public Enumerator(T item)
		{
			this._item = item;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000E473 File Offset: 0x0000C673
		object IEnumerator.Current
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000E480 File Offset: 0x0000C680
		T IEnumerator<!0>.Current
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E488 File Offset: 0x0000C688
		bool IEnumerator.MoveNext()
		{
			return !this._moveNextCalled && (this._moveNextCalled = true);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E4A9 File Offset: 0x0000C6A9
		void IEnumerator.Reset()
		{
			this._moveNextCalled = false;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00007270 File Offset: 0x00005470
		void IDisposable.Dispose()
		{
		}

		// Token: 0x04000155 RID: 341
		[CompilerGenerated]
		private readonly T _item;

		// Token: 0x04000156 RID: 342
		[CompilerGenerated]
		private bool _moveNextCalled;
	}
}
