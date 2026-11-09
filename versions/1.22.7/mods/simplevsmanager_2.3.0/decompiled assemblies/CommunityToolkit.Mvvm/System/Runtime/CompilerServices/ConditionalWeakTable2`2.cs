using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ConditionalWeakTable2<TKey, [Nullable(2)] TValue> where TKey : class where TValue : class
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002328 File Offset: 0x00000528
		public ConditionalWeakTable2()
		{
			this.lockObject = new object();
			this.container = new ConditionalWeakTable2<TKey, TValue>.Container(this);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002349 File Offset: 0x00000549
		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			return this.container.TryGetValueWorker(key, ref value);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000235C File Offset: 0x0000055C
		public bool TryAdd(TKey key, TValue value)
		{
			object obj = this.lockObject;
			bool result;
			lock (obj)
			{
				object obj2;
				if (this.container.FindEntry(key, ref obj2) != -1)
				{
					result = false;
				}
				else
				{
					this.CreateEntry(key, value);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023B8 File Offset: 0x000005B8
		public bool Remove(TKey key)
		{
			object obj = this.lockObject;
			bool result;
			lock (obj)
			{
				result = this.container.Remove(key);
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002404 File Offset: 0x00000604
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2091", Justification = "ConditionalWeakTable<TKey, TValue> is only referenced to reuse the callback delegate type, but no value is ever created through reflection.")]
		public TValue GetValue(TKey key, ConditionalWeakTable<TKey, TValue>.CreateValueCallback createValueCallback)
		{
			TValue result;
			if (!this.TryGetValue(key, ref result))
			{
				return this.GetValueLocked(key, createValueCallback);
			}
			return result;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002428 File Offset: 0x00000628
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2091", Justification = "ConditionalWeakTable<TKey, TValue> is only referenced to reuse the callback delegate type, but no value is ever created through reflection.")]
		private TValue GetValueLocked(TKey key, ConditionalWeakTable<TKey, TValue>.CreateValueCallback createValueCallback)
		{
			TValue tvalue = createValueCallback.Invoke(key);
			object obj = this.lockObject;
			TValue result;
			lock (obj)
			{
				TValue tvalue2;
				if (this.container.TryGetValueWorker(key, ref tvalue2))
				{
					result = tvalue2;
				}
				else
				{
					this.CreateEntry(key, tvalue);
					result = tvalue;
				}
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002490 File Offset: 0x00000690
		[NullableContext(0)]
		public ConditionalWeakTable2<TKey, TValue>.Enumerator GetEnumerator()
		{
			Monitor.Enter(this.lockObject);
			return new ConditionalWeakTable2<TKey, TValue>.Enumerator(this);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024A4 File Offset: 0x000006A4
		private void CreateEntry(TKey key, TValue value)
		{
			ConditionalWeakTable2<TKey, TValue>.Container container = this.container;
			if (!container.HasCapacity)
			{
				container = (this.container = container.Resize());
			}
			container.CreateEntryNoResize(key, value);
		}

		// Token: 0x04000014 RID: 20
		private const int InitialCapacity = 8;

		// Token: 0x04000015 RID: 21
		private readonly object lockObject;

		// Token: 0x04000016 RID: 22
		[Nullable(new byte[]
		{
			1,
			0,
			0
		})]
		private volatile ConditionalWeakTable2<TKey, TValue>.Container container;

		// Token: 0x02000044 RID: 68
		[Nullable(0)]
		[CompilerFeatureRequired("RefStructs")]
		public ref struct Enumerator
		{
			// Token: 0x060001B0 RID: 432 RVA: 0x00007A24 File Offset: 0x00005C24
			public Enumerator(ConditionalWeakTable2<TKey, TValue> table)
			{
				this.table = table;
				ConditionalWeakTable2<TKey, TValue>.Container container = table.container;
				if (container == null || container.FirstFreeEntry == 0)
				{
					this.maxIndexInclusive = -1;
				}
				else
				{
					this.maxIndexInclusive = container.FirstFreeEntry - 1;
				}
				this.currentIndex = -1;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x060001B1 RID: 433 RVA: 0x00007A82 File Offset: 0x00005C82
			public void Dispose()
			{
				Monitor.Exit(this.table.lockObject);
				this.table = null;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x060001B2 RID: 434 RVA: 0x00007AB4 File Offset: 0x00005CB4
			public bool MoveNext()
			{
				ConditionalWeakTable2<TKey, TValue>.Container container = this.table.container;
				int i = this.currentIndex;
				int num = this.maxIndexInclusive;
				while (i < num)
				{
					i++;
					if (container.TryGetEntry(i, ref this.key, ref this.value))
					{
						this.currentIndex = i;
						return true;
					}
				}
				this.currentIndex = i;
				return false;
			}

			// Token: 0x060001B3 RID: 435 RVA: 0x00007B0D File Offset: 0x00005D0D
			[MethodImpl(256)]
			public TKey GetKey()
			{
				return this.key;
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x00007B15 File Offset: 0x00005D15
			[MethodImpl(256)]
			public TValue GetValue()
			{
				return this.value;
			}

			// Token: 0x04000073 RID: 115
			private ConditionalWeakTable2<TKey, TValue> table;

			// Token: 0x04000074 RID: 116
			private readonly int maxIndexInclusive;

			// Token: 0x04000075 RID: 117
			private int currentIndex;

			// Token: 0x04000076 RID: 118
			[Nullable(2)]
			private TKey key;

			// Token: 0x04000077 RID: 119
			[Nullable(2)]
			private TValue value;
		}

		// Token: 0x02000045 RID: 69
		[NullableContext(0)]
		private struct Entry
		{
			// Token: 0x04000078 RID: 120
			public DependentHandle depHnd;

			// Token: 0x04000079 RID: 121
			public int HashCode;

			// Token: 0x0400007A RID: 122
			public int Next;
		}

		// Token: 0x02000046 RID: 70
		[Nullable(0)]
		private sealed class Container
		{
			// Token: 0x060001B5 RID: 437 RVA: 0x00007B20 File Offset: 0x00005D20
			internal Container(ConditionalWeakTable2<TKey, TValue> parent)
			{
				this.buckets = new int[8];
				for (int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				this.entries = new ConditionalWeakTable2<TKey, TValue>.Entry[8];
				this.parent = parent;
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x00007B6E File Offset: 0x00005D6E
			private Container(ConditionalWeakTable2<TKey, TValue> parent, int[] buckets, [Nullable(new byte[]
			{
				1,
				0,
				0,
				0
			})] ConditionalWeakTable2<TKey, TValue>.Entry[] entries, int firstFreeEntry)
			{
				this.parent = parent;
				this.buckets = buckets;
				this.entries = entries;
				this.firstFreeEntry = firstFreeEntry;
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x060001B7 RID: 439 RVA: 0x00007B93 File Offset: 0x00005D93
			internal bool HasCapacity
			{
				get
				{
					return this.firstFreeEntry < this.entries.Length;
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x060001B8 RID: 440 RVA: 0x00007BA5 File Offset: 0x00005DA5
			internal int FirstFreeEntry
			{
				get
				{
					return this.firstFreeEntry;
				}
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x00007BB0 File Offset: 0x00005DB0
			internal void CreateEntryNoResize(TKey key, TValue value)
			{
				this.VerifyIntegrity();
				this.invalid = true;
				int num = RuntimeHelpers.GetHashCode(key) & int.MaxValue;
				int num2 = this.firstFreeEntry;
				this.firstFreeEntry = num2 + 1;
				int num3 = num2;
				this.entries[num3].HashCode = num;
				this.entries[num3].depHnd = new DependentHandle(key, value);
				int num4 = num & this.buckets.Length - 1;
				this.entries[num3].Next = this.buckets[num4];
				Volatile.Write(ref this.buckets[num4], num3);
				this.invalid = false;
			}

			// Token: 0x060001BA RID: 442 RVA: 0x00007C64 File Offset: 0x00005E64
			internal bool TryGetValueWorker(TKey key, [MaybeNullWhen(false)] out TValue value)
			{
				object obj;
				int num = this.FindEntry(key, ref obj);
				value = Unsafe.As<TValue>(obj);
				return num != -1;
			}

			// Token: 0x060001BB RID: 443 RVA: 0x00007C8C File Offset: 0x00005E8C
			internal int FindEntry(TKey key, [Nullable(2)] out object value)
			{
				int num = RuntimeHelpers.GetHashCode(key) & int.MaxValue;
				int num2 = num & this.buckets.Length - 1;
				for (int num3 = Volatile.Read(ref this.buckets[num2]); num3 != -1; num3 = this.entries[num3].Next)
				{
					if (this.entries[num3].HashCode == num)
					{
						ValueTuple<object, object> targetAndDependent = this.entries[num3].depHnd.TargetAndDependent;
						TKey item = targetAndDependent.Item1;
						value = targetAndDependent.Item2;
						if (item == key)
						{
							GC.KeepAlive(this);
							return num3;
						}
					}
				}
				GC.KeepAlive(this);
				value = null;
				return -1;
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00007D38 File Offset: 0x00005F38
			internal bool TryGetEntry(int index, [Nullable(2)] [NotNullWhen(true)] out TKey key, [MaybeNullWhen(false)] out TValue value)
			{
				if (index < this.entries.Length)
				{
					ValueTuple<object, object> targetAndDependent = this.entries[index].depHnd.TargetAndDependent;
					object item = targetAndDependent.Item1;
					object item2 = targetAndDependent.Item2;
					GC.KeepAlive(this);
					if (item != null)
					{
						key = Unsafe.As<TKey>(item);
						value = Unsafe.As<TValue>(item2);
						return true;
					}
				}
				key = default(TKey);
				value = default(TValue);
				return false;
			}

			// Token: 0x060001BD RID: 445 RVA: 0x00007DA8 File Offset: 0x00005FA8
			internal bool Remove(TKey key)
			{
				this.VerifyIntegrity();
				object obj;
				int num = this.FindEntry(key, ref obj);
				if (num != -1)
				{
					this.RemoveIndex(num);
					return true;
				}
				return false;
			}

			// Token: 0x060001BE RID: 446 RVA: 0x00007DD3 File Offset: 0x00005FD3
			private void RemoveIndex(int entryIndex)
			{
				ConditionalWeakTable2<TKey, TValue>.Entry[] array = this.entries;
				Volatile.Write(ref array[entryIndex].HashCode, -1);
				array[entryIndex].depHnd.Target = null;
			}

			// Token: 0x060001BF RID: 447 RVA: 0x00007DF8 File Offset: 0x00005FF8
			[return: Nullable(new byte[]
			{
				1,
				0,
				0
			})]
			internal ConditionalWeakTable2<TKey, TValue>.Container Resize()
			{
				bool flag = false;
				int num = this.buckets.Length;
				for (int i = 0; i < this.entries.Length; i++)
				{
					ref ConditionalWeakTable2<TKey, TValue>.Entry ptr = ref this.entries[i];
					if (ptr.HashCode == -1)
					{
						flag = true;
						break;
					}
					if (ptr.depHnd.IsAllocated && ptr.depHnd.Target == null)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num = this.buckets.Length * 2;
				}
				return this.Resize(num);
			}

			// Token: 0x060001C0 RID: 448 RVA: 0x00007E70 File Offset: 0x00006070
			[return: Nullable(new byte[]
			{
				1,
				0,
				0
			})]
			internal ConditionalWeakTable2<TKey, TValue>.Container Resize(int newSize)
			{
				int[] array = new int[newSize];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = -1;
				}
				ConditionalWeakTable2<TKey, TValue>.Entry[] array2 = new ConditionalWeakTable2<TKey, TValue>.Entry[newSize];
				int num = 0;
				for (int j = 0; j < this.entries.Length; j++)
				{
					ref ConditionalWeakTable2<TKey, TValue>.Entry ptr = ref this.entries[j];
					int hashCode = ptr.HashCode;
					DependentHandle depHnd = ptr.depHnd;
					if (hashCode != -1 && depHnd.IsAllocated)
					{
						if (depHnd.Target != null)
						{
							ConditionalWeakTable2<TKey, TValue>.Entry[] array3 = array2;
							int num2 = num;
							array3[num2].HashCode = hashCode;
							array3[num2].depHnd = depHnd;
							int num3 = hashCode & array.Length - 1;
							array3[num2].Next = array[num3];
							array[num3] = num;
							num++;
						}
						else
						{
							Volatile.Write(ref ptr.HashCode, -1);
						}
					}
				}
				ConditionalWeakTable2<TKey, TValue>.Container result = new ConditionalWeakTable2<TKey, TValue>.Container(this.parent, array, array2, num);
				this.oldKeepAlive = result;
				GC.KeepAlive(this);
				return result;
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x00007F5A File Offset: 0x0000615A
			private void VerifyIntegrity()
			{
				if (this.invalid)
				{
					ConditionalWeakTable2<TKey, TValue>.Container.<VerifyIntegrity>g__Throw|21_0();
				}
			}

			// Token: 0x060001C2 RID: 450 RVA: 0x00007F6C File Offset: 0x0000616C
			protected override void Finalize()
			{
				try
				{
					if (!this.invalid && this.parent != null)
					{
						if (!this.finalized)
						{
							this.finalized = true;
							object lockObject = this.parent.lockObject;
							lock (lockObject)
							{
								if (this.parent.container == this)
								{
									this.parent.container = null;
								}
							}
							GC.ReRegisterForFinalize(this);
						}
						else
						{
							ConditionalWeakTable2<TKey, TValue>.Entry[] array = this.entries;
							this.invalid = true;
							this.entries = null;
							this.buckets = null;
							if (array != null)
							{
								for (int i = 0; i < array.Length; i++)
								{
									if (this.oldKeepAlive == null || array[i].HashCode == -1)
									{
										array[i].depHnd.Dispose();
									}
								}
							}
						}
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x00008064 File Offset: 0x00006264
			[CompilerGenerated]
			internal static void <VerifyIntegrity>g__Throw|21_0()
			{
				throw new InvalidOperationException("The current collection is in a corrupted state.");
			}

			// Token: 0x0400007B RID: 123
			private readonly ConditionalWeakTable2<TKey, TValue> parent;

			// Token: 0x0400007C RID: 124
			private int[] buckets;

			// Token: 0x0400007D RID: 125
			[Nullable(new byte[]
			{
				1,
				0,
				0,
				0
			})]
			private ConditionalWeakTable2<TKey, TValue>.Entry[] entries;

			// Token: 0x0400007E RID: 126
			private int firstFreeEntry;

			// Token: 0x0400007F RID: 127
			private bool invalid;

			// Token: 0x04000080 RID: 128
			private bool finalized;

			// Token: 0x04000081 RID: 129
			[Nullable(2)]
			private volatile object oldKeepAlive;
		}
	}
}
