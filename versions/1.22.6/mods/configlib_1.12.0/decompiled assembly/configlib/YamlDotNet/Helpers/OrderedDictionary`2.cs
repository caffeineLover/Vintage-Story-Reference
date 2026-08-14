using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x0200019C RID: 412
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	internal sealed class OrderedDictionary<TKey, [Nullable(2)] TValue> : IOrderedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x170000FB RID: 251
		public TValue this[TKey key]
		{
			get
			{
				return this.dictionary[key];
			}
			set
			{
				OrderedDictionary<TKey, TValue>.<>c__DisplayClass5_0 CS$<>8__locals1 = new OrderedDictionary<TKey, TValue>.<>c__DisplayClass5_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.key = key;
				if (this.dictionary.ContainsKey(CS$<>8__locals1.key))
				{
					int num = this.list.FindIndex(new Predicate<KeyValuePair<TKey, TValue>>(CS$<>8__locals1.<set_Item>b__0));
					this.dictionary[CS$<>8__locals1.key] = value;
					this.list[num] = new KeyValuePair<TKey, TValue>(CS$<>8__locals1.key, value);
					return;
				}
				this.Add(CS$<>8__locals1.key, value);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		public ICollection<TKey> Keys
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x0001CC54 File Offset: 0x0001AE54
		public ICollection<TValue> Values
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001CC5C File Offset: 0x0001AE5C
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0000D44C File Offset: 0x0000B64C
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000100 RID: 256
		[Nullable(new byte[]
		{
			0,
			1,
			1
		})]
		public KeyValuePair<TKey, TValue> this[int index]
		{
			[return: Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			get
			{
				return this.list[index];
			}
			[param: Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			set
			{
				this.list[index] = value;
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001CC86 File Offset: 0x0001AE86
		public OrderedDictionary() : this(EqualityComparer<TKey>.Default)
		{
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001CC93 File Offset: 0x0001AE93
		public OrderedDictionary(IEqualityComparer<TKey> comparer)
		{
			this.list = new List<KeyValuePair<TKey, TValue>>();
			this.dictionary = new Dictionary<TKey, TValue>(comparer);
			this.comparer = comparer;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001CCB9 File Offset: 0x0001AEB9
		public void Add(TKey key, TValue value)
		{
			this.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		public void Add([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			this.dictionary.Add(item.Key, item.Value);
			this.list.Add(item);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001CCEF File Offset: 0x0001AEEF
		public void Clear()
		{
			this.dictionary.Clear();
			this.list.Clear();
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001CD07 File Offset: 0x0001AF07
		public bool Contains([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			return Enumerable.Contains<KeyValuePair<TKey, TValue>>(this.dictionary, item);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001CD15 File Offset: 0x0001AF15
		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001CD23 File Offset: 0x0001AF23
		public void CopyTo([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001CD32 File Offset: 0x0001AF32
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001CD44 File Offset: 0x0001AF44
		public void Insert(int index, TKey key, TValue value)
		{
			this.dictionary.Add(key, value);
			this.list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001CD68 File Offset: 0x0001AF68
		public bool Remove(TKey key)
		{
			OrderedDictionary<TKey, TValue>.<>c__DisplayClass27_0 CS$<>8__locals1 = new OrderedDictionary<TKey, TValue>.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.key = key;
			if (!this.dictionary.ContainsKey(CS$<>8__locals1.key))
			{
				return false;
			}
			int num = this.list.FindIndex(new Predicate<KeyValuePair<TKey, TValue>>(CS$<>8__locals1.<Remove>b__0));
			this.list.RemoveAt(num);
			if (!this.dictionary.Remove(CS$<>8__locals1.key))
			{
				throw new InvalidOperationException();
			}
			return true;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001CDDC File Offset: 0x0001AFDC
		public bool Remove([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001CDEC File Offset: 0x0001AFEC
		public void RemoveAt(int index)
		{
			TKey key = this.list[index].Key;
			this.dictionary.Remove(key);
			this.list.RemoveAt(index);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001CE27 File Offset: 0x0001B027
		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			return this.dictionary.TryGetValue(key, ref value);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001CD32 File Offset: 0x0001AF32
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001CE38 File Offset: 0x0001B038
		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			this.dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				this.dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x040003A1 RID: 929
		[NonSerialized]
		private Dictionary<TKey, TValue> dictionary;

		// Token: 0x040003A2 RID: 930
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private readonly List<KeyValuePair<TKey, TValue>> list;

		// Token: 0x040003A3 RID: 931
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x0200019D RID: 413
		[Nullable(0)]
		private class KeyCollection : ICollection<!0>, IEnumerable<!0>, IEnumerable
		{
			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060008EB RID: 2283 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
			public int Count
			{
				get
				{
					return this.orderedDictionary.list.Count;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060008EC RID: 2284 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060008ED RID: 2285 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			public void Add(TKey item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008EE RID: 2286 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008EF RID: 2287 RVA: 0x0001CEBA File Offset: 0x0001B0BA
			public bool Contains(TKey item)
			{
				return Enumerable.Contains<TKey>(this.orderedDictionary.dictionary.Keys, item);
			}

			// Token: 0x060008F0 RID: 2288 RVA: 0x0001CED2 File Offset: 0x0001B0D2
			public KeyCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
			{
				this.orderedDictionary = orderedDictionary;
			}

			// Token: 0x060008F1 RID: 2289 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				for (int i = 0; i < this.orderedDictionary.list.Count; i++)
				{
					array[i] = this.orderedDictionary.list[i + arrayIndex].Key;
				}
			}

			// Token: 0x060008F2 RID: 2290 RVA: 0x0001CF2E File Offset: 0x0001B12E
			public IEnumerator<TKey> GetEnumerator()
			{
				IEnumerable<KeyValuePair<TKey, TValue>> list = this.orderedDictionary.list;
				Func<KeyValuePair<TKey, TValue>, TKey> func;
				if ((func = OrderedDictionary<TKey, TValue>.KeyCollection.<>c.<>9__10_0) == null)
				{
					func = (OrderedDictionary<TKey, TValue>.KeyCollection.<>c.<>9__10_0 = new Func<KeyValuePair<TKey, TValue>, TKey>(OrderedDictionary<TKey, TValue>.KeyCollection.<>c.<>9.<GetEnumerator>b__10_0));
				}
				return Enumerable.Select<KeyValuePair<TKey, TValue>, TKey>(list, func).GetEnumerator();
			}

			// Token: 0x060008F3 RID: 2291 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			public bool Remove(TKey item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008F4 RID: 2292 RVA: 0x0001CF64 File Offset: 0x0001B164
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040003A4 RID: 932
			private readonly OrderedDictionary<TKey, TValue> orderedDictionary;
		}

		// Token: 0x0200019F RID: 415
		[Nullable(0)]
		private class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
		{
			// Token: 0x17000103 RID: 259
			// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0001CF81 File Offset: 0x0001B181
			public int Count
			{
				get
				{
					return this.orderedDictionary.list.Count;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060008FA RID: 2298 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			public void Add(TValue item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008FB RID: 2299 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008FC RID: 2300 RVA: 0x0001CF93 File Offset: 0x0001B193
			public bool Contains(TValue item)
			{
				return Enumerable.Contains<TValue>(this.orderedDictionary.dictionary.Values, item);
			}

			// Token: 0x060008FD RID: 2301 RVA: 0x0001CFAB File Offset: 0x0001B1AB
			public ValueCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
			{
				this.orderedDictionary = orderedDictionary;
			}

			// Token: 0x060008FE RID: 2302 RVA: 0x0001CFBC File Offset: 0x0001B1BC
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				for (int i = 0; i < this.orderedDictionary.list.Count; i++)
				{
					array[i] = this.orderedDictionary.list[i + arrayIndex].Value;
				}
			}

			// Token: 0x060008FF RID: 2303 RVA: 0x0001D006 File Offset: 0x0001B206
			public IEnumerator<TValue> GetEnumerator()
			{
				IEnumerable<KeyValuePair<TKey, TValue>> list = this.orderedDictionary.list;
				Func<KeyValuePair<TKey, TValue>, TValue> func;
				if ((func = OrderedDictionary<TKey, TValue>.ValueCollection.<>c.<>9__10_0) == null)
				{
					func = (OrderedDictionary<TKey, TValue>.ValueCollection.<>c.<>9__10_0 = new Func<KeyValuePair<TKey, TValue>, TValue>(OrderedDictionary<TKey, TValue>.ValueCollection.<>c.<>9.<GetEnumerator>b__10_0));
				}
				return Enumerable.Select<KeyValuePair<TKey, TValue>, TValue>(list, func).GetEnumerator();
			}

			// Token: 0x06000900 RID: 2304 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			public bool Remove(TValue item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000901 RID: 2305 RVA: 0x0001D03C File Offset: 0x0001B23C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040003A7 RID: 935
			private readonly OrderedDictionary<TKey, TValue> orderedDictionary;
		}
	}
}
