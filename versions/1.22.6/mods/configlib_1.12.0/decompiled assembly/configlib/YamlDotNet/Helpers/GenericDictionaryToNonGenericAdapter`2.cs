using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000198 RID: 408
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class GenericDictionaryToNonGenericAdapter<TKey, [Nullable(2)] TValue> : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x0001CAC6 File Offset: 0x0001ACC6
		public GenericDictionaryToNonGenericAdapter(IDictionary<TKey, TValue> genericDictionary)
		{
			if (genericDictionary == null)
			{
				throw new ArgumentNullException("genericDictionary");
			}
			this.genericDictionary = genericDictionary;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void Add(object key, [Nullable(2)] object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool Contains(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001CAE4 File Offset: 0x0001ACE4
		public IDictionaryEnumerator GetEnumerator()
		{
			return new GenericDictionaryToNonGenericAdapter<TKey, TValue>.DictionaryEnumerator(this.genericDictionary.GetEnumerator());
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool IsFixedSize
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public ICollection Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0000E3DD File Offset: 0x0000C5DD
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

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public int Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public bool IsSynchronized
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0000E3DD File Offset: 0x0000C5DD
		public object SyncRoot
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001CB0F File Offset: 0x0001AD0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400039F RID: 927
		private readonly IDictionary<TKey, TValue> genericDictionary;

		// Token: 0x02000199 RID: 409
		[Nullable(0)]
		private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060008C7 RID: 2247 RVA: 0x0001CB17 File Offset: 0x0001AD17
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
			// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0001CB26 File Offset: 0x0001AD26
			public DictionaryEntry Entry
			{
				get
				{
					return new DictionaryEntry(this.Key, this.Value);
				}
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001CB3C File Offset: 0x0001AD3C
			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x060008CA RID: 2250 RVA: 0x0001CB64 File Offset: 0x0001AD64
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
			// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001CB89 File Offset: 0x0001AD89
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x060008CC RID: 2252 RVA: 0x0001CB96 File Offset: 0x0001AD96
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			// Token: 0x060008CD RID: 2253 RVA: 0x0001CBA3 File Offset: 0x0001ADA3
			public void Reset()
			{
				this.enumerator.Reset();
			}

			// Token: 0x040003A0 RID: 928
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
