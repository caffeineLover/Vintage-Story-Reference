using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000082 RID: 130
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StaticArrayNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0000AF90 File Offset: 0x00009190
		public StaticArrayNodeDeserializer(StaticObjectFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000AFB0 File Offset: 0x000091B0
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
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

		// Token: 0x040000CA RID: 202
		private readonly StaticObjectFactory factory;

		// Token: 0x02000184 RID: 388
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ArrayList : IList, ICollection, IEnumerable
		{
			// Token: 0x06000843 RID: 2115 RVA: 0x00019B79 File Offset: 0x00017D79
			public ArrayList()
			{
				this.Clear();
			}

			// Token: 0x06000844 RID: 2116 RVA: 0x00019B88 File Offset: 0x00017D88
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

			// Token: 0x06000845 RID: 2117 RVA: 0x00019BD9 File Offset: 0x00017DD9
			public void Clear()
			{
				this.data = new object[10];
				this.Count = 0;
			}

			// Token: 0x06000846 RID: 2118 RVA: 0x00019BEF File Offset: 0x00017DEF
			bool IList.Contains(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000847 RID: 2119 RVA: 0x00019BF6 File Offset: 0x00017DF6
			int IList.IndexOf(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000848 RID: 2120 RVA: 0x00019BFD File Offset: 0x00017DFD
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000849 RID: 2121 RVA: 0x00019C04 File Offset: 0x00017E04
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600084A RID: 2122 RVA: 0x00019C0B File Offset: 0x00017E0B
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x0600084B RID: 2123 RVA: 0x00019C12 File Offset: 0x00017E12
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x0600084C RID: 2124 RVA: 0x00019C15 File Offset: 0x00017E15
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000115 RID: 277
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

			// Token: 0x0600084F RID: 2127 RVA: 0x00019C2D File Offset: 0x00017E2D
			[NullableContext(1)]
			public void CopyTo(Array array, int index)
			{
				Array.Copy(this.data, 0, array, index, this.Count);
			}

			// Token: 0x17000116 RID: 278
			// (get) Token: 0x06000850 RID: 2128 RVA: 0x00019C43 File Offset: 0x00017E43
			// (set) Token: 0x06000851 RID: 2129 RVA: 0x00019C4B File Offset: 0x00017E4B
			public int Count { get; private set; }

			// Token: 0x17000117 RID: 279
			// (get) Token: 0x06000852 RID: 2130 RVA: 0x00019C54 File Offset: 0x00017E54
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000118 RID: 280
			// (get) Token: 0x06000853 RID: 2131 RVA: 0x00019C57 File Offset: 0x00017E57
			[Nullable(1)]
			public object SyncRoot
			{
				[NullableContext(1)]
				get
				{
					return this.data;
				}
			}

			// Token: 0x06000854 RID: 2132 RVA: 0x00019C5F File Offset: 0x00017E5F
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

			// Token: 0x04000312 RID: 786
			[Nullable(new byte[]
			{
				1,
				2
			})]
			private object[] data;
		}
	}
}
