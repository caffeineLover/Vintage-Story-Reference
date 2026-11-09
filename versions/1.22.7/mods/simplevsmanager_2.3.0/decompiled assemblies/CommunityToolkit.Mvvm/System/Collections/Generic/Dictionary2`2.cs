using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("Count = {Count}")]
	internal class Dictionary2<[Nullable(0)] TKey, [Nullable(2)] TValue> : IDictionary2<TKey, TValue>, IDictionary2<TKey>, IDictionary2 where TKey : IEquatable<TKey> where TValue : class
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000024DA File Offset: 0x000006DA
		public Dictionary2()
		{
			this.Initialize(0);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000024E9 File Offset: 0x000006E9
		public int Count
		{
			[MethodImpl(256)]
			get
			{
				return this.count - this.freeCount;
			}
		}

		// Token: 0x17000006 RID: 6
		public TValue this[TKey key]
		{
			get
			{
				ref TValue ptr = this.FindValue(key);
				if (!Unsafe.IsNullRef<TValue>(ref ptr))
				{
					return ptr;
				}
				Dictionary2<TKey, TValue>.ThrowArgumentExceptionForKeyNotFound(key);
				return default(TValue);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000252C File Offset: 0x0000072C
		public void Clear()
		{
			int num = this.count;
			if (num > 0)
			{
				Array.Clear(this.buckets);
				this.count = 0;
				this.freeList = -1;
				this.freeCount = 0;
				Array.Clear(this.entries, 0, num);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002571 File Offset: 0x00000771
		public bool ContainsKey(TKey key)
		{
			return !Unsafe.IsNullRef<TValue>(this.FindValue(key));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002584 File Offset: 0x00000784
		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			ref TValue ptr = this.FindValue(key);
			if (!Unsafe.IsNullRef<TValue>(ref ptr))
			{
				value = ptr;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000025B8 File Offset: 0x000007B8
		public bool TryRemove(TKey key)
		{
			uint hashCode = (uint)key.GetHashCode();
			ref int bucket = this.GetBucket(hashCode);
			Dictionary2<TKey, TValue>.Entry[] array = this.entries;
			int num = -1;
			Dictionary2<TKey, TValue>.Entry ptr;
			for (int i = bucket - 1; i >= 0; i = ptr.Next)
			{
				ptr = ref array[i];
				if (ptr.HashCode == hashCode && ptr.Key.Equals(key))
				{
					if (num < 0)
					{
						bucket = ptr.Next + 1;
					}
					else
					{
						array[num].Next = ptr.Next;
					}
					ptr.Next = -3 - this.freeList;
					if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
					{
						ptr.Key = default(TKey);
					}
					if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
					{
						ptr.Value = default(TValue);
					}
					this.freeList = i;
					this.freeCount++;
					return true;
				}
				num = i;
			}
			return false;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026A4 File Offset: 0x000008A4
		[return: Nullable(2)]
		public ref TValue GetOrAddValueRef(TKey key)
		{
			Dictionary2<TKey, TValue>.Entry[] array = this.entries;
			uint hashCode = (uint)key.GetHashCode();
			ref int bucket = this.GetBucket(hashCode);
			for (int i = bucket - 1; i < array.Length; i = array[i].Next)
			{
				if (array[i].HashCode == hashCode && array[i].Key.Equals(key))
				{
					return ref array[i].Value;
				}
			}
			int num;
			if (this.freeCount > 0)
			{
				num = this.freeList;
				this.freeList = -3 - array[this.freeList].Next;
				this.freeCount--;
			}
			else
			{
				int num2 = this.count;
				if (num2 == array.Length)
				{
					this.Resize();
					bucket = this.GetBucket(hashCode);
				}
				num = num2;
				this.count = num2 + 1;
				array = this.entries;
			}
			Dictionary2<TKey, TValue>.Entry[] array2 = array;
			int num3 = num;
			array2[num3].HashCode = hashCode;
			array2[num3].Next = bucket - 1;
			array2[num3].Key = key;
			array2[num3].Value = default(TValue);
			bucket = num + 1;
			return ref array2[num3].Value;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027C2 File Offset: 0x000009C2
		[NullableContext(0)]
		[MethodImpl(256)]
		public Dictionary2<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new Dictionary2<TKey, TValue>.Enumerator(this);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027CC File Offset: 0x000009CC
		private unsafe ref TValue FindValue(TKey key)
		{
			ref Dictionary2<TKey, TValue>.Entry ptr = (UIntPtr)0;
			uint hashCode = (uint)key.GetHashCode();
			int i = *this.GetBucket(hashCode);
			Dictionary2<TKey, TValue>.Entry[] array = this.entries;
			for (i--; i < array.Length; i = ptr.Next)
			{
				ptr = ref array[i];
				if (ptr.HashCode == hashCode && ptr.Key.Equals(key))
				{
					return ref ptr.Value;
				}
			}
			return (UIntPtr)0;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002844 File Offset: 0x00000A44
		[MemberNotNull(new string[]
		{
			"buckets",
			"entries"
		})]
		private void Initialize(int capacity)
		{
			int prime = HashHelpers.GetPrime(capacity);
			int[] array = new int[prime];
			Dictionary2<TKey, TValue>.Entry[] array2 = new Dictionary2<TKey, TValue>.Entry[prime];
			this.freeList = -1;
			this.fastModMultiplier = HashHelpers.GetFastModMultiplier((uint)prime);
			this.buckets = array;
			this.entries = array2;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002888 File Offset: 0x00000A88
		[MethodImpl(8)]
		private void Resize()
		{
			int num = HashHelpers.ExpandPrime(this.count);
			Dictionary2<TKey, TValue>.Entry[] array = new Dictionary2<TKey, TValue>.Entry[num];
			int num2 = this.count;
			Array.Copy(this.entries, array, num2);
			this.buckets = new int[num];
			this.fastModMultiplier = HashHelpers.GetFastModMultiplier((uint)num);
			for (int i = 0; i < num2; i++)
			{
				if (array[i].Next >= -1)
				{
					ref int bucket = this.GetBucket(array[i].HashCode);
					array[i].Next = bucket - 1;
					bucket = i + 1;
				}
			}
			this.entries = array;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002924 File Offset: 0x00000B24
		[MethodImpl(256)]
		private ref int GetBucket(uint hashCode)
		{
			int[] array = this.buckets;
			return ref array[(int)HashHelpers.FastMod(hashCode, (uint)array.Length, this.fastModMultiplier)];
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002950 File Offset: 0x00000B50
		private static void ThrowArgumentExceptionForKeyNotFound(TKey key)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("The target key ");
			defaultInterpolatedStringHandler.AppendFormatted<TKey>(key);
			defaultInterpolatedStringHandler.AppendLiteral(" was not present in the dictionary");
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x04000017 RID: 23
		private const int StartOfFreeList = -3;

		// Token: 0x04000018 RID: 24
		private int[] buckets;

		// Token: 0x04000019 RID: 25
		[Nullable(new byte[]
		{
			1,
			0,
			0,
			0
		})]
		private Dictionary2<TKey, TValue>.Entry[] entries;

		// Token: 0x0400001A RID: 26
		private ulong fastModMultiplier;

		// Token: 0x0400001B RID: 27
		private int count;

		// Token: 0x0400001C RID: 28
		private int freeList;

		// Token: 0x0400001D RID: 29
		private int freeCount;

		// Token: 0x02000047 RID: 71
		[Nullable(0)]
		[CompilerFeatureRequired("RefStructs")]
		public ref struct Enumerator
		{
			// Token: 0x060001C4 RID: 452 RVA: 0x00008070 File Offset: 0x00006270
			internal Enumerator(Dictionary2<TKey, TValue> dictionary)
			{
				this.entries = dictionary.entries;
				this.index = 0;
				this.count = dictionary.count;
			}

			// Token: 0x060001C5 RID: 453 RVA: 0x00008094 File Offset: 0x00006294
			public bool MoveNext()
			{
				while (this.index < this.count)
				{
					Dictionary2<TKey, TValue>.Entry[] array = this.entries;
					int num = this.index;
					this.index = num + 1;
					if (array[num].Next >= -1)
					{
						return true;
					}
				}
				this.index = this.count + 1;
				return false;
			}

			// Token: 0x060001C6 RID: 454 RVA: 0x000080E6 File Offset: 0x000062E6
			[MethodImpl(256)]
			public TKey GetKey()
			{
				return this.entries[this.index - 1].Key;
			}

			// Token: 0x060001C7 RID: 455 RVA: 0x00008100 File Offset: 0x00006300
			[MethodImpl(256)]
			public TValue GetValue()
			{
				return this.entries[this.index - 1].Value;
			}

			// Token: 0x04000082 RID: 130
			[Nullable(new byte[]
			{
				1,
				0,
				0,
				0
			})]
			private readonly Dictionary2<TKey, TValue>.Entry[] entries;

			// Token: 0x04000083 RID: 131
			private int index;

			// Token: 0x04000084 RID: 132
			private readonly int count;
		}

		// Token: 0x02000048 RID: 72
		[NullableContext(0)]
		private struct Entry
		{
			// Token: 0x04000085 RID: 133
			public uint HashCode;

			// Token: 0x04000086 RID: 134
			public int Next;

			// Token: 0x04000087 RID: 135
			[Nullable(1)]
			public TKey Key;

			// Token: 0x04000088 RID: 136
			[Nullable(2)]
			public TValue Value;
		}
	}
}
