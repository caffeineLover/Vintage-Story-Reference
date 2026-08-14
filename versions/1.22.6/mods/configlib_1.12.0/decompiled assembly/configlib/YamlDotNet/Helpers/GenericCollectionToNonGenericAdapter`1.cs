using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000197 RID: 407
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class GenericCollectionToNonGenericAdapter<T> : IList, ICollection, IEnumerable
	{
		// Token: 0x060008A5 RID: 2213 RVA: 0x0001CA57 File Offset: 0x0001AC57
		[NullableContext(1)]
		public GenericCollectionToNonGenericAdapter(ICollection<T> genericCollection)
		{
			if (genericCollection == null)
			{
				throw new ArgumentNullException("genericCollection");
			}
			this.genericCollection = genericCollection;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001CA75 File Offset: 0x0001AC75
		public int Add(object value)
		{
			int count = this.genericCollection.Count;
			this.genericCollection.Add((T)((object)value));
			return count;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001CA93 File Offset: 0x0001AC93
		public void Clear()
		{
			this.genericCollection.Clear();
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool Contains(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public int IndexOf(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool IsFixedSize
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000EA RID: 234
		public object this[int index]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				((IList<!0>)this.genericCollection)[index] = (T)((object)value);
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		[NullableContext(1)]
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool IsSynchronized
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		[Nullable(1)]
		public object SyncRoot
		{
			[NullableContext(1)]
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001CAB9 File Offset: 0x0001ACB9
		[NullableContext(1)]
		public IEnumerator GetEnumerator()
		{
			return this.genericCollection.GetEnumerator();
		}

		// Token: 0x0400039E RID: 926
		[Nullable(1)]
		private readonly ICollection<T> genericCollection;
	}
}
