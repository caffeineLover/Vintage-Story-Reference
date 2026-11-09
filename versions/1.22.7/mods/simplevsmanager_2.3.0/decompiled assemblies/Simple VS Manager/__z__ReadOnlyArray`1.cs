using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x0200000B RID: 11
[CompilerGenerated]
internal sealed class <>z__ReadOnlyArray<T> : IEnumerable, ICollection, IList, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x06000041 RID: 65 RVA: 0x00002E8A File Offset: 0x0000108A
	public <>z__ReadOnlyArray(T[] items)
	{
		this._items = items;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00002E99 File Offset: 0x00001099
	[return: Nullable(1)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000043 RID: 67 RVA: 0x00002EA6 File Offset: 0x000010A6
	int ICollection.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000044 RID: 68 RVA: 0x00002EB0 File Offset: 0x000010B0
	bool ICollection.IsSynchronized
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000045 RID: 69 RVA: 0x00002EB3 File Offset: 0x000010B3
	object ICollection.SyncRoot
	{
		[return: Nullable(1)]
		get
		{
			return this;
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002EB6 File Offset: 0x000010B6
	void ICollection.CopyTo([Nullable(1)] Array array, int index)
	{
		this._items.CopyTo(array, index);
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000047 RID: 71 RVA: 0x00002EC5 File Offset: 0x000010C5
	// (set) Token: 0x06000048 RID: 72 RVA: 0x00002ED8 File Offset: 0x000010D8
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

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000049 RID: 73 RVA: 0x00002EDF File Offset: 0x000010DF
	bool IList.IsFixedSize
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600004A RID: 74 RVA: 0x00002EE2 File Offset: 0x000010E2
	bool IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00002EE5 File Offset: 0x000010E5
	int IList.Add([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00002EEC File Offset: 0x000010EC
	void IList.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00002EF3 File Offset: 0x000010F3
	bool IList.Contains([Nullable(2)] object value)
	{
		return this._items.Contains(value);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00002F01 File Offset: 0x00001101
	int IList.IndexOf([Nullable(2)] object value)
	{
		return this._items.IndexOf(value);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00002F0F File Offset: 0x0000110F
	void IList.Insert(int index, [Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002F16 File Offset: 0x00001116
	void IList.Remove([Nullable(2)] object value)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002F1D File Offset: 0x0000111D
	void IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00002F24 File Offset: 0x00001124
	[return: Nullable(new byte[]
	{
		1,
		0
	})]
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000053 RID: 83 RVA: 0x00002F31 File Offset: 0x00001131
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000054 RID: 84 RVA: 0x00002F3B File Offset: 0x0000113B
	T IReadOnlyList<!0>.this[]
	{
		get
		{
			return this._items[index];
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000055 RID: 85 RVA: 0x00002F49 File Offset: 0x00001149
	int ICollection<!0>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000056 RID: 86 RVA: 0x00002F53 File Offset: 0x00001153
	bool ICollection<!0>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00002F56 File Offset: 0x00001156
	void ICollection<!0>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00002F5D File Offset: 0x0000115D
	void ICollection<!0>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00002F64 File Offset: 0x00001164
	bool ICollection<!0>.Contains(T item)
	{
		return this._items.Contains(item);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00002F72 File Offset: 0x00001172
	void ICollection<!0>.CopyTo([Nullable(new byte[]
	{
		1,
		0
	})] T[] array, int arrayIndex)
	{
		this._items.CopyTo(array, arrayIndex);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00002F81 File Offset: 0x00001181
	bool ICollection<!0>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600005C RID: 92 RVA: 0x00002F88 File Offset: 0x00001188
	// (set) Token: 0x0600005D RID: 93 RVA: 0x00002F96 File Offset: 0x00001196
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

	// Token: 0x0600005E RID: 94 RVA: 0x00002F9D File Offset: 0x0000119D
	int IList<!0>.IndexOf(T item)
	{
		return this._items.IndexOf(item);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00002FAB File Offset: 0x000011AB
	void IList<!0>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00002FB2 File Offset: 0x000011B2
	void IList<!0>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x0400001D RID: 29
	[CompilerGenerated]
	private readonly T[] _items;
}
