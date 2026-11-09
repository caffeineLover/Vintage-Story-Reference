using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000B8 RID: 184
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class GenericDictionaryToNonGenericAdapter<TKey, [Nullable(2)] TValue> : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06000440 RID: 1088 RVA: 0x0000E702 File Offset: 0x0000C902
		public GenericDictionaryToNonGenericAdapter(IDictionary<TKey, TValue> genericDictionary)
		{
			if (genericDictionary == null)
			{
				throw new ArgumentNullException("genericDictionary");
			}
			this.genericDictionary = genericDictionary;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000E720 File Offset: 0x0000C920
		public void Add(object key, [Nullable(2)] object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000E727 File Offset: 0x0000C927
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000E72E File Offset: 0x0000C92E
		public bool Contains(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000E735 File Offset: 0x0000C935
		public IDictionaryEnumerator GetEnumerator()
		{
			return new GenericDictionaryToNonGenericAdapter<TKey, TValue>.DictionaryEnumerator(this.genericDictionary.GetEnumerator());
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000E747 File Offset: 0x0000C947
		public bool IsFixedSize
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000E74E File Offset: 0x0000C94E
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0000E755 File Offset: 0x0000C955
		public ICollection Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000E75C File Offset: 0x0000C95C
		public void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000E763 File Offset: 0x0000C963
		public ICollection Values
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000069 RID: 105
		[Nullable(2)]
		public object this[object key]
		{
			[return: Nullable(2)]
			get
			{
				throw new NotSupportedException();
			}
			[param: Nullable(2)]
			set
			{
				this.genericDictionary[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000E78A File Offset: 0x0000C98A
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0000E791 File Offset: 0x0000C991
		public int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0000E798 File Offset: 0x0000C998
		public bool IsSynchronized
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000E79F File Offset: 0x0000C99F
		public object SyncRoot
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000E7A6 File Offset: 0x0000C9A6
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000119 RID: 281
		private readonly IDictionary<TKey, TValue> genericDictionary;

		// Token: 0x02000191 RID: 401
		[Nullable(0)]
		private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06000892 RID: 2194 RVA: 0x0001A5D7 File Offset: 0x000187D7
			public DictionaryEnumerator([Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})] IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
			{
				this.enumerator = enumerator;
			}

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x06000893 RID: 2195 RVA: 0x0001A5E6 File Offset: 0x000187E6
			public DictionaryEntry Entry
			{
				get
				{
					return new DictionaryEntry(this.Key, this.Value);
				}
			}

			// Token: 0x17000122 RID: 290
			// (get) Token: 0x06000894 RID: 2196 RVA: 0x0001A5FC File Offset: 0x000187FC
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x17000123 RID: 291
			// (get) Token: 0x06000895 RID: 2197 RVA: 0x0001A624 File Offset: 0x00018824
			[Nullable(2)]
			public object Value
			{
				[NullableContext(2)]
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x17000124 RID: 292
			// (get) Token: 0x06000896 RID: 2198 RVA: 0x0001A649 File Offset: 0x00018849
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x06000897 RID: 2199 RVA: 0x0001A656 File Offset: 0x00018856
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			// Token: 0x06000898 RID: 2200 RVA: 0x0001A663 File Offset: 0x00018863
			public void Reset()
			{
				this.enumerator.Reset();
			}

			// Token: 0x0400033D RID: 829
			[Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})]
			private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;
		}
	}
}
