using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArrayNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x00009814 File Offset: 0x00007A14
		public ArrayNodeDeserializer(INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			this.enumNamingConvention = enumNamingConvention;
			this.typeInspector = typeInspector;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000982C File Offset: 0x00007A2C
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			ArrayNodeDeserializer.<>c__DisplayClass3_0 CS$<>8__locals1 = new ArrayNodeDeserializer.<>c__DisplayClass3_0();
			CS$<>8__locals1.<>4__this = this;
			if (!expectedType.IsArray)
			{
				value = false;
				return false;
			}
			CS$<>8__locals1.itemType = expectedType.GetElementType();
			ArrayNodeDeserializer.ArrayList arrayList = new ArrayNodeDeserializer.ArrayList();
			CS$<>8__locals1.array = null;
			CollectionNodeDeserializer.DeserializeHelper(CS$<>8__locals1.itemType, parser, nestedObjectDeserializer, arrayList, true, this.enumNamingConvention, this.typeInspector, new Action<int, object>(CS$<>8__locals1.<Deserialize>g__PromiseResolvedHandler|0));
			CS$<>8__locals1.array = Array.CreateInstance(CS$<>8__locals1.itemType, arrayList.Count);
			arrayList.CopyTo(CS$<>8__locals1.array, 0);
			value = CS$<>8__locals1.array;
			return true;
		}

		// Token: 0x040000B0 RID: 176
		private readonly INamingConvention enumNamingConvention;

		// Token: 0x040000B1 RID: 177
		private readonly ITypeInspector typeInspector;

		// Token: 0x02000179 RID: 377
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ArrayList : IList, ICollection, IEnumerable
		{
			// Token: 0x06000819 RID: 2073 RVA: 0x0001979B File Offset: 0x0001799B
			public ArrayList()
			{
				this.Clear();
			}

			// Token: 0x0600081A RID: 2074 RVA: 0x000197AC File Offset: 0x000179AC
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

			// Token: 0x0600081B RID: 2075 RVA: 0x000197FD File Offset: 0x000179FD
			public void Clear()
			{
				this.data = new object[10];
				this.Count = 0;
			}

			// Token: 0x0600081C RID: 2076 RVA: 0x00019813 File Offset: 0x00017A13
			bool IList.Contains(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600081D RID: 2077 RVA: 0x0001981A File Offset: 0x00017A1A
			int IList.IndexOf(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600081E RID: 2078 RVA: 0x00019821 File Offset: 0x00017A21
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600081F RID: 2079 RVA: 0x00019828 File Offset: 0x00017A28
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000820 RID: 2080 RVA: 0x0001982F File Offset: 0x00017A2F
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x1700010D RID: 269
			// (get) Token: 0x06000821 RID: 2081 RVA: 0x00019836 File Offset: 0x00017A36
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x06000822 RID: 2082 RVA: 0x00019839 File Offset: 0x00017A39
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700010F RID: 271
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

			// Token: 0x06000825 RID: 2085 RVA: 0x00019851 File Offset: 0x00017A51
			[NullableContext(1)]
			public void CopyTo(Array array, int index)
			{
				Array.Copy(this.data, 0, array, index, this.Count);
			}

			// Token: 0x17000110 RID: 272
			// (get) Token: 0x06000826 RID: 2086 RVA: 0x00019867 File Offset: 0x00017A67
			// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001986F File Offset: 0x00017A6F
			public int Count { get; private set; }

			// Token: 0x17000111 RID: 273
			// (get) Token: 0x06000828 RID: 2088 RVA: 0x00019878 File Offset: 0x00017A78
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x06000829 RID: 2089 RVA: 0x0001987B File Offset: 0x00017A7B
			[Nullable(1)]
			public object SyncRoot
			{
				[NullableContext(1)]
				get
				{
					return this.data;
				}
			}

			// Token: 0x0600082A RID: 2090 RVA: 0x00019883 File Offset: 0x00017A83
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

			// Token: 0x040002F6 RID: 758
			[Nullable(new byte[]
			{
				1,
				2
			})]
			private object[] data;
		}
	}
}
