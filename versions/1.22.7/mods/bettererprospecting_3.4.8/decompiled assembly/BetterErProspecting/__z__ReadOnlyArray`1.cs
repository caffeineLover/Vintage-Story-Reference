using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000017 RID: 23
[CompilerGenerated]
internal sealed class <>z__ReadOnlyArray<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x06000080 RID: 128 RVA: 0x0000601F File Offset: 0x0000421F
	public <>z__ReadOnlyArray(T[] items)
	{
		this._items = items;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0000602E File Offset: 0x0000422E
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000082 RID: 130 RVA: 0x0000603B File Offset: 0x0000423B
	int ICollection.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000083 RID: 131 RVA: 0x00006045 File Offset: 0x00004245
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000084 RID: 132 RVA: 0x00006048 File Offset: 0x00004248
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0000604B File Offset: 0x0000424B
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		this._items.CopyTo(array, index);
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000086 RID: 134 RVA: 0x0000605A File Offset: 0x0000425A
	// (set) Token: 0x06000087 RID: 135 RVA: 0x0000606D File Offset: 0x0000426D
	object IList.this[]
	{
		[return: Nullable(2)]
		get
		{
			return this._items[index];
		}
		[param: Nullable(2)]
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000088 RID: 136 RVA: 0x00006074 File Offset: 0x00004274
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000089 RID: 137 RVA: 0x00006077 File Offset: 0x00004277
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0000607A File Offset: 0x0000427A
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00006081 File Offset: 0x00004281
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00006088 File Offset: 0x00004288
	bool IList.Contains([Nullable(2)] object value)
	{
		return this._items.Contains(value);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00006096 File Offset: 0x00004296
	int IList.IndexOf([Nullable(2)] object value)
	{
		return this._items.IndexOf(value);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000060A4 File Offset: 0x000042A4
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600008F RID: 143 RVA: 0x000060AB File Offset: 0x000042AB
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000060B2 File Offset: 0x000042B2
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000060B9 File Offset: 0x000042B9
	[return: Nullable(new byte[]
	{
		1,
		0
	})]
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000092 RID: 146 RVA: 0x000060C6 File Offset: 0x000042C6
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000093 RID: 147 RVA: 0x000060D0 File Offset: 0x000042D0
	T IReadOnlyList<!0>.this[]
	{
		get
		{
			return this._items[index];
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000094 RID: 148 RVA: 0x000060DE File Offset: 0x000042DE
	int ICollection<!0>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000095 RID: 149 RVA: 0x000060E8 File Offset: 0x000042E8
	bool ICollection<!0>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x000060EB File Offset: 0x000042EB
	void ICollection<!0>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000097 RID: 151 RVA: 0x000060F2 File Offset: 0x000042F2
	void ICollection<!0>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000098 RID: 152 RVA: 0x000060F9 File Offset: 0x000042F9
	bool ICollection<!0>.Contains(T item)
	{
		return this._items.Contains(item);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00006107 File Offset: 0x00004307
	void ICollection<!0>.CopyTo([Nullable(new byte[]
	{
		1,
		0
	})] T[] array, int arrayIndex)
	{
		this._items.CopyTo(array, arrayIndex);
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00006116 File Offset: 0x00004316
	bool ICollection<!0>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600009B RID: 155 RVA: 0x0000611D File Offset: 0x0000431D
	// (set) Token: 0x0600009C RID: 156 RVA: 0x0000612B File Offset: 0x0000432B
	T IList<!0>.this[]
	{
		get
		{
			return this._items[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00006132 File Offset: 0x00004332
	int IList<!0>.IndexOf(T item)
	{
		return this._items.IndexOf(item);
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00006140 File Offset: 0x00004340
	void IList<!0>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00006147 File Offset: 0x00004347
	void IList<!0>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x04000048 RID: 72
	[CompilerGenerated]
	private readonly T[] _items;
}
