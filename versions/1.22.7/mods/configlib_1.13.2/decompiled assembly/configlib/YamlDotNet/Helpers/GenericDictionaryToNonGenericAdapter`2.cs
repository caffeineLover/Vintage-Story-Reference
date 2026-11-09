using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000194 RID: 404
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class GenericDictionaryToNonGenericAdapter<TKey, [Nullable(2)] TValue> : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600087A RID: 2170 RVA: 0x0001C17E File Offset: 0x0001A37E
		public GenericDictionaryToNonGenericAdapter(IDictionary<TKey, TValue> genericDictionary)
		{
			if (genericDictionary == null)
			{
				throw new ArgumentNullException("genericDictionary");
			}
			this.genericDictionary = genericDictionary;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0000209F File Offset: 0x0000029F
		public void Add(object key, [Nullable(2)] object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0000209F File Offset: 0x0000029F
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0000209F File Offset: 0x0000029F
		public bool Contains(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001C19C File Offset: 0x0001A39C
		public IDictionaryEnumerator GetEnumerator()
		{
			return new GenericDictionaryToNonGenericAdapter<TKey, TValue>.DictionaryEnumerator(this.genericDictionary.GetEnumerator());
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsFixedSize
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0000209F File Offset: 0x0000029F
		public ICollection Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0000209F File Offset: 0x0000029F
		public void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0000209F File Offset: 0x0000029F
		public ICollection Values
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F2 RID: 242
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

		// Token: 0x06000886 RID: 2182 RVA: 0x0000209F File Offset: 0x0000029F
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x0000209F File Offset: 0x0000029F
		public int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsSynchronized
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0000209F File Offset: 0x0000029F
		public object SyncRoot
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001C1C7 File Offset: 0x0001A3C7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000371 RID: 881
		private readonly IDictionary<TKey, TValue> genericDictionary;

		// Token: 0x02000195 RID: 405
		[Nullable(0)]
		private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x0600088B RID: 2187 RVA: 0x0001C1CF File Offset: 0x0001A3CF
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

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001C1DE File Offset: 0x0001A3DE
			public DictionaryEntry Entry
			{
				get
				{
					return new DictionaryEntry(this.Key, this.Value);
				}
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x0600088D RID: 2189 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x0600088E RID: 2190 RVA: 0x0001C21C File Offset: 0x0001A41C
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

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x0600088F RID: 2191 RVA: 0x0001C241 File Offset: 0x0001A441
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x06000890 RID: 2192 RVA: 0x0001C24E File Offset: 0x0001A44E
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			// Token: 0x06000891 RID: 2193 RVA: 0x0001C25B File Offset: 0x0001A45B
			public void Reset()
			{
				this.enumerator.Reset();
			}

			// Token: 0x04000372 RID: 882
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
