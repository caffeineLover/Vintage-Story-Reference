using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000193 RID: 403
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class GenericCollectionToNonGenericAdapter<T> : IList, ICollection, IEnumerable
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x0001C10F File Offset: 0x0001A30F
		[NullableContext(1)]
		public GenericCollectionToNonGenericAdapter(ICollection<T> genericCollection)
		{
			if (genericCollection == null)
			{
				throw new ArgumentNullException("genericCollection");
			}
			this.genericCollection = genericCollection;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001C12D File Offset: 0x0001A32D
		public int Add(object value)
		{
			int count = this.genericCollection.Count;
			this.genericCollection.Add((T)((object)value));
			return count;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001C14B File Offset: 0x0001A34B
		public void Clear()
		{
			this.genericCollection.Clear();
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0000209F File Offset: 0x0000029F
		public bool Contains(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0000209F File Offset: 0x0000029F
		public int IndexOf(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0000209F File Offset: 0x0000029F
		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsFixedSize
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0000209F File Offset: 0x0000029F
		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0000209F File Offset: 0x0000029F
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

		// Token: 0x06000875 RID: 2165 RVA: 0x0000209F File Offset: 0x0000029F
		[NullableContext(1)]
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0000209F File Offset: 0x0000029F
		public int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsSynchronized
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0000209F File Offset: 0x0000029F
		[Nullable(1)]
		public object SyncRoot
		{
			[NullableContext(1)]
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001C171 File Offset: 0x0001A371
		[NullableContext(1)]
		public IEnumerator GetEnumerator()
		{
			return this.genericCollection.GetEnumerator();
		}

		// Token: 0x04000370 RID: 880
		[Nullable(1)]
		private readonly ICollection<T> genericCollection;
	}
}
