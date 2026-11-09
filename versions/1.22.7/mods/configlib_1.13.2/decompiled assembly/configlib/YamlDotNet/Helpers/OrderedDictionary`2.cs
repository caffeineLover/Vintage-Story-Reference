using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000198 RID: 408
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
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0001C304 File Offset: 0x0001A504
		public ICollection<TKey> Keys
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001C30C File Offset: 0x0001A50C
		public ICollection<TValue> Values
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0001C314 File Offset: 0x0001A514
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0000206F File Offset: 0x0000026F
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

		// Token: 0x0600089F RID: 2207 RVA: 0x0001C33E File Offset: 0x0001A53E
		public OrderedDictionary() : this(EqualityComparer<TKey>.Default)
		{
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001C34B File Offset: 0x0001A54B
		public OrderedDictionary(IEqualityComparer<TKey> comparer)
		{
			this.list = new List<KeyValuePair<TKey, TValue>>();
			this.dictionary = new Dictionary<TKey, TValue>(comparer);
			this.comparer = comparer;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001C371 File Offset: 0x0001A571
		public void Add(TKey key, TValue value)
		{
			this.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001C380 File Offset: 0x0001A580
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

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001C3A7 File Offset: 0x0001A5A7
		public void Clear()
		{
			this.dictionary.Clear();
			this.list.Clear();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001C3BF File Offset: 0x0001A5BF
		public bool Contains([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			return Enumerable.Contains<KeyValuePair<TKey, TValue>>(this.dictionary, item);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001C3CD File Offset: 0x0001A5CD
		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001C3DB File Offset: 0x0001A5DB
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

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001C3EA File Offset: 0x0001A5EA
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

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001C3FC File Offset: 0x0001A5FC
		public void Insert(int index, TKey key, TValue value)
		{
			this.dictionary.Add(key, value);
			this.list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001C420 File Offset: 0x0001A620
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

		// Token: 0x060008AA RID: 2218 RVA: 0x0001C494 File Offset: 0x0001A694
		public bool Remove([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001C4A4 File Offset: 0x0001A6A4
		public void RemoveAt(int index)
		{
			TKey key = this.list[index].Key;
			this.dictionary.Remove(key);
			this.list.RemoveAt(index);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001C4DF File Offset: 0x0001A6DF
		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			return this.dictionary.TryGetValue(key, ref value);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001C3EA File Offset: 0x0001A5EA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			this.dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				this.dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x04000373 RID: 883
		[NonSerialized]
		private Dictionary<TKey, TValue> dictionary;

		// Token: 0x04000374 RID: 884
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private readonly List<KeyValuePair<TKey, TValue>> list;

		// Token: 0x04000375 RID: 885
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000199 RID: 409
		[Nullable(0)]
		private class KeyCollection : ICollection<!0>, IEnumerable<!0>, IEnumerable
		{
			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001C560 File Offset: 0x0001A760
			public int Count
			{
				get
				{
					return this.orderedDictionary.list.Count;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0000206C File Offset: 0x0000026C
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060008B1 RID: 2225 RVA: 0x0000209F File Offset: 0x0000029F
			public void Add(TKey item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008B2 RID: 2226 RVA: 0x0000209F File Offset: 0x0000029F
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008B3 RID: 2227 RVA: 0x0001C572 File Offset: 0x0001A772
			public bool Contains(TKey item)
			{
				return Enumerable.Contains<TKey>(this.orderedDictionary.dictionary.Keys, item);
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x0001C58A File Offset: 0x0001A78A
			public KeyCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
			{
				this.orderedDictionary = orderedDictionary;
			}

			// Token: 0x060008B5 RID: 2229 RVA: 0x0001C59C File Offset: 0x0001A79C
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				for (int i = 0; i < this.orderedDictionary.list.Count; i++)
				{
					array[i] = this.orderedDictionary.list[i + arrayIndex].Key;
				}
			}

			// Token: 0x060008B6 RID: 2230 RVA: 0x0001C5E6 File Offset: 0x0001A7E6
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

			// Token: 0x060008B7 RID: 2231 RVA: 0x0000209F File Offset: 0x0000029F
			public bool Remove(TKey item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x0001C61C File Offset: 0x0001A81C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x04000376 RID: 886
			private readonly OrderedDictionary<TKey, TValue> orderedDictionary;
		}

		// Token: 0x0200019B RID: 411
		[Nullable(0)]
		private class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
		{
			// Token: 0x17000103 RID: 259
			// (get) Token: 0x060008BC RID: 2236 RVA: 0x0001C639 File Offset: 0x0001A839
			public int Count
			{
				get
				{
					return this.orderedDictionary.list.Count;
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x060008BD RID: 2237 RVA: 0x0000206C File Offset: 0x0000026C
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060008BE RID: 2238 RVA: 0x0000209F File Offset: 0x0000029F
			public void Add(TValue item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x0000209F File Offset: 0x0000029F
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x0001C64B File Offset: 0x0001A84B
			public bool Contains(TValue item)
			{
				return Enumerable.Contains<TValue>(this.orderedDictionary.dictionary.Values, item);
			}

			// Token: 0x060008C1 RID: 2241 RVA: 0x0001C663 File Offset: 0x0001A863
			public ValueCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
			{
				this.orderedDictionary = orderedDictionary;
			}

			// Token: 0x060008C2 RID: 2242 RVA: 0x0001C674 File Offset: 0x0001A874
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				for (int i = 0; i < this.orderedDictionary.list.Count; i++)
				{
					array[i] = this.orderedDictionary.list[i + arrayIndex].Value;
				}
			}

			// Token: 0x060008C3 RID: 2243 RVA: 0x0001C6BE File Offset: 0x0001A8BE
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

			// Token: 0x060008C4 RID: 2244 RVA: 0x0000209F File Offset: 0x0000029F
			public bool Remove(TValue item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x04000379 RID: 889
			private readonly OrderedDictionary<TKey, TValue> orderedDictionary;
		}
	}
}
