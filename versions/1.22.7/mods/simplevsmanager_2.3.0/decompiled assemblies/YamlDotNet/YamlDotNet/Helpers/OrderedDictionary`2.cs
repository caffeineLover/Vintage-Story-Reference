using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	internal sealed class OrderedDictionary<TKey, [Nullable(2)] TValue> : IOrderedDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x1700006E RID: 110
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000E864 File Offset: 0x0000CA64
		public ICollection<TKey> Keys
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000E86C File Offset: 0x0000CA6C
		public ICollection<TValue> Values
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000E874 File Offset: 0x0000CA74
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000E881 File Offset: 0x0000CA81
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000073 RID: 115
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

		// Token: 0x06000469 RID: 1129 RVA: 0x0000E8A1 File Offset: 0x0000CAA1
		public OrderedDictionary() : this(EqualityComparer<TKey>.Default)
		{
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000E8AE File Offset: 0x0000CAAE
		public OrderedDictionary(IEqualityComparer<TKey> comparer)
		{
			this.list = new List<KeyValuePair<TKey, TValue>>();
			this.dictionary = new Dictionary<TKey, TValue>(comparer);
			this.comparer = comparer;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		public void Add([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			if (!this.TryAdd(item))
			{
				OrderedDictionary<TKey, TValue>.ThrowDuplicateKeyException(item.Key);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000E8EB File Offset: 0x0000CAEB
		public void Add(TKey key, TValue value)
		{
			if (!this.TryAdd(key, value))
			{
				OrderedDictionary<TKey, TValue>.ThrowDuplicateKeyException(key);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000E900 File Offset: 0x0000CB00
		private static void ThrowDuplicateKeyException(TKey key)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(50, 1);
			defaultInterpolatedStringHandler.AppendLiteral("An item with the same key ");
			defaultInterpolatedStringHandler.AppendFormatted<TKey>(key);
			defaultInterpolatedStringHandler.AppendLiteral(" has already been added.");
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000E943 File Offset: 0x0000CB43
		[MethodImpl(256)]
		public bool TryAdd(TKey key, TValue value)
		{
			if (this.dictionary.TryAdd(key, value))
			{
				this.list.Add(new KeyValuePair<TKey, TValue>(key, value));
				return true;
			}
			return false;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000E969 File Offset: 0x0000CB69
		[MethodImpl(256)]
		public bool TryAdd([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			if (this.dictionary.TryAdd(item.Key, item.Value))
			{
				this.list.Add(item);
				return true;
			}
			return false;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000E995 File Offset: 0x0000CB95
		public void Clear()
		{
			this.dictionary.Clear();
			this.list.Clear();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000E9AD File Offset: 0x0000CBAD
		public bool Contains([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			return Enumerable.Contains<KeyValuePair<TKey, TValue>>(this.dictionary, item);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000E9BB File Offset: 0x0000CBBB
		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000E9C9 File Offset: 0x0000CBC9
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

		// Token: 0x06000474 RID: 1140 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
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

		// Token: 0x06000475 RID: 1141 RVA: 0x0000E9EA File Offset: 0x0000CBEA
		public void Insert(int index, TKey key, TValue value)
		{
			this.dictionary.Add(key, value);
			this.list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		public bool Remove(TKey key)
		{
			OrderedDictionary<TKey, TValue>.<>c__DisplayClass30_0 CS$<>8__locals1 = new OrderedDictionary<TKey, TValue>.<>c__DisplayClass30_0();
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

		// Token: 0x06000477 RID: 1143 RVA: 0x0000EA80 File Offset: 0x0000CC80
		public bool Remove([Nullable(new byte[]
		{
			0,
			1,
			1
		})] KeyValuePair<TKey, TValue> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000EA90 File Offset: 0x0000CC90
		public void RemoveAt(int index)
		{
			TKey key = this.list[index].Key;
			this.dictionary.Remove(key);
			this.list.RemoveAt(index);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000EACB File Offset: 0x0000CCCB
		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			return this.dictionary.TryGetValue(key, ref value);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000EADA File Offset: 0x0000CCDA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000EAEC File Offset: 0x0000CCEC
		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			this.dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.list)
			{
				this.dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x0400011A RID: 282
		[NonSerialized]
		private Dictionary<TKey, TValue> dictionary;

		// Token: 0x0400011B RID: 283
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private readonly List<KeyValuePair<TKey, TValue>> list;

		// Token: 0x0400011C RID: 284
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000192 RID: 402
		[Nullable(0)]
		private class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
		{
			// Token: 0x17000125 RID: 293
			// (get) Token: 0x06000899 RID: 2201 RVA: 0x0001A670 File Offset: 0x00018870
			public int Count
			{
				get
				{
					return this.orderedDictionary.list.Count;
				}
			}

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001A682 File Offset: 0x00018882
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600089B RID: 2203 RVA: 0x0001A685 File Offset: 0x00018885
			public void Add(TKey item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600089C RID: 2204 RVA: 0x0001A68C File Offset: 0x0001888C
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600089D RID: 2205 RVA: 0x0001A693 File Offset: 0x00018893
			public bool Contains(TKey item)
			{
				return this.orderedDictionary.dictionary.ContainsKey(item);
			}

			// Token: 0x0600089E RID: 2206 RVA: 0x0001A6A6 File Offset: 0x000188A6
			public KeyCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
			{
				this.orderedDictionary = orderedDictionary;
			}

			// Token: 0x0600089F RID: 2207 RVA: 0x0001A6B8 File Offset: 0x000188B8
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				for (int i = 0; i < this.orderedDictionary.list.Count; i++)
				{
					array[i] = this.orderedDictionary.list[i + arrayIndex].Key;
				}
			}

			// Token: 0x060008A0 RID: 2208 RVA: 0x0001A702 File Offset: 0x00018902
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

			// Token: 0x060008A1 RID: 2209 RVA: 0x0001A738 File Offset: 0x00018938
			public bool Remove(TKey item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008A2 RID: 2210 RVA: 0x0001A73F File Offset: 0x0001893F
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0400033E RID: 830
			private readonly OrderedDictionary<TKey, TValue> orderedDictionary;
		}

		// Token: 0x02000193 RID: 403
		[Nullable(0)]
		private class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
		{
			// Token: 0x17000127 RID: 295
			// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0001A747 File Offset: 0x00018947
			public int Count
			{
				get
				{
					return this.orderedDictionary.list.Count;
				}
			}

			// Token: 0x17000128 RID: 296
			// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0001A759 File Offset: 0x00018959
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060008A5 RID: 2213 RVA: 0x0001A75C File Offset: 0x0001895C
			public void Add(TValue item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008A6 RID: 2214 RVA: 0x0001A763 File Offset: 0x00018963
			public void Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008A7 RID: 2215 RVA: 0x0001A76A File Offset: 0x0001896A
			public bool Contains(TValue item)
			{
				return this.orderedDictionary.dictionary.ContainsValue(item);
			}

			// Token: 0x060008A8 RID: 2216 RVA: 0x0001A77D File Offset: 0x0001897D
			public ValueCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
			{
				this.orderedDictionary = orderedDictionary;
			}

			// Token: 0x060008A9 RID: 2217 RVA: 0x0001A78C File Offset: 0x0001898C
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				for (int i = 0; i < this.orderedDictionary.list.Count; i++)
				{
					array[i] = this.orderedDictionary.list[i + arrayIndex].Value;
				}
			}

			// Token: 0x060008AA RID: 2218 RVA: 0x0001A7D6 File Offset: 0x000189D6
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

			// Token: 0x060008AB RID: 2219 RVA: 0x0001A80C File Offset: 0x00018A0C
			public bool Remove(TValue item)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060008AC RID: 2220 RVA: 0x0001A813 File Offset: 0x00018A13
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0400033F RID: 831
			private readonly OrderedDictionary<TKey, TValue> orderedDictionary;
		}
	}
}
