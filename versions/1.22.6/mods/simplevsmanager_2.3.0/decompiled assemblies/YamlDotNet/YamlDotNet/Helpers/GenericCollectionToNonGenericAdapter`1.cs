using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class GenericCollectionToNonGenericAdapter<T> : IList, ICollection, IEnumerable
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x0000E62F File Offset: 0x0000C82F
		[NullableContext(1)]
		public GenericCollectionToNonGenericAdapter(ICollection<T> genericCollection)
		{
			if (genericCollection == null)
			{
				throw new ArgumentNullException("genericCollection");
			}
			this.genericCollection = genericCollection;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000E650 File Offset: 0x0000C850
		public int Add(object value)
		{
			int count = this.genericCollection.Count;
			this.genericCollection.Add((T)((object)value));
			return count;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000E67B File Offset: 0x0000C87B
		public void Clear()
		{
			this.genericCollection.Clear();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000E688 File Offset: 0x0000C888
		public bool Contains(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000E68F File Offset: 0x0000C88F
		public int IndexOf(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000E696 File Offset: 0x0000C896
		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000E69D File Offset: 0x0000C89D
		public bool IsFixedSize
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000E6AB File Offset: 0x0000C8AB
		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000E6B2 File Offset: 0x0000C8B2
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000061 RID: 97
		public object this[int index]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				((IList<T>)this.genericCollection)[index] = (T)((object)value);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000E6D9 File Offset: 0x0000C8D9
		[NullableContext(1)]
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
		public int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000E6E7 File Offset: 0x0000C8E7
		public bool IsSynchronized
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000E6EE File Offset: 0x0000C8EE
		[Nullable(1)]
		public object SyncRoot
		{
			[NullableContext(1)]
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000E6F5 File Offset: 0x0000C8F5
		[NullableContext(1)]
		public IEnumerator GetEnumerator()
		{
			return this.genericCollection.GetEnumerator();
		}

		// Token: 0x04000118 RID: 280
		[Nullable(1)]
		private readonly ICollection<T> genericCollection;
	}
}
