using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000150 RID: 336
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StaticArrayNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x00018269 File Offset: 0x00016469
		public StaticArrayNodeDeserializer(StaticObjectFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00018288 File Offset: 0x00016488
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			if (!this.factory.IsArray(expectedType))
			{
				value = false;
				return false;
			}
			Type valueType = this.factory.GetValueType(expectedType);
			StaticArrayNodeDeserializer.ArrayList arrayList = new StaticArrayNodeDeserializer.ArrayList();
			StaticCollectionNodeDeserializer.DeserializeHelper(valueType, parser, nestedObjectDeserializer, arrayList, this.factory);
			Array array = this.factory.CreateArray(expectedType, arrayList.Count);
			arrayList.CopyTo(array, 0);
			value = array;
			return true;
		}

		// Token: 0x040002F3 RID: 755
		private readonly StaticObjectFactory factory;

		// Token: 0x02000151 RID: 337
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ArrayList : IList, ICollection, IEnumerable
		{
			// Token: 0x060006E1 RID: 1761 RVA: 0x000182EE File Offset: 0x000164EE
			public ArrayList()
			{
				this.Clear();
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x000182FC File Offset: 0x000164FC
			public int Add(object value)
			{
				if (this.Count == this.data.Length)
				{
					Array.Resize<object>(ref this.data, this.data.Length * 2);
				}
				this.data[this.Count] = value;
				int count = this.Count;
				this.Count = count + 1;
				return count;
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x0001834D File Offset: 0x0001654D
			public void Clear()
			{
				this.data = new object[10];
				this.Count = 0;
			}

			// Token: 0x060006E4 RID: 1764 RVA: 0x0000209F File Offset: 0x0000029F
			bool IList.Contains(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x0000209F File Offset: 0x0000029F
			int IList.IndexOf(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006E6 RID: 1766 RVA: 0x0000209F File Offset: 0x0000029F
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006E7 RID: 1767 RVA: 0x0000209F File Offset: 0x0000029F
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006E8 RID: 1768 RVA: 0x0000209F File Offset: 0x0000029F
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x170000BE RID: 190
			// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0000206F File Offset: 0x0000026F
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x060006EA RID: 1770 RVA: 0x0000206F File Offset: 0x0000026F
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000C0 RID: 192
			public object this[int index]
			{
				get
				{
					return this.data[index];
				}
				set
				{
					this.data[index] = value;
				}
			}

			// Token: 0x060006ED RID: 1773 RVA: 0x00018378 File Offset: 0x00016578
			[NullableContext(1)]
			public void CopyTo(Array array, int index)
			{
				Array.Copy(this.data, 0, array, index, this.Count);
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001838E File Offset: 0x0001658E
			// (set) Token: 0x060006EF RID: 1775 RVA: 0x00018396 File Offset: 0x00016596
			public int Count { get; private set; }

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0000206F File Offset: 0x0000026F
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001839F File Offset: 0x0001659F
			[Nullable(1)]
			public object SyncRoot
			{
				[NullableContext(1)]
				get
				{
					return this.data;
				}
			}

			// Token: 0x060006F2 RID: 1778 RVA: 0x000183A7 File Offset: 0x000165A7
			[NullableContext(1)]
			public IEnumerator GetEnumerator()
			{
				int num;
				for (int i = 0; i < this.Count; i = num)
				{
					yield return this.data[i];
					num = i + 1;
				}
				yield break;
			}

			// Token: 0x040002F4 RID: 756
			[Nullable(new byte[]
			{
				1,
				2
			})]
			private object[] data;
		}
	}
}
