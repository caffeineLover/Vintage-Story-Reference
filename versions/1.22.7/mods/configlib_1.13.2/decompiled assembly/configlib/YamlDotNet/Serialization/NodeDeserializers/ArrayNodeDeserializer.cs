using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200013C RID: 316
	public sealed class ArrayNodeDeserializer : INodeDeserializer
	{
		// Token: 0x0600068B RID: 1675 RVA: 0x00016B4C File Offset: 0x00014D4C
		[NullableContext(1)]
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			if (!expectedType.IsArray)
			{
				value = false;
				return false;
			}
			Type elementType = expectedType.GetElementType();
			ArrayNodeDeserializer.ArrayList arrayList = new ArrayNodeDeserializer.ArrayList();
			CollectionNodeDeserializer.DeserializeHelper(elementType, parser, nestedObjectDeserializer, arrayList, true);
			Array array = Array.CreateInstance(elementType, arrayList.Count);
			arrayList.CopyTo(array, 0);
			value = array;
			return true;
		}

		// Token: 0x0200013D RID: 317
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ArrayList : IList, ICollection, IEnumerable
		{
			// Token: 0x0600068D RID: 1677 RVA: 0x00016B9B File Offset: 0x00014D9B
			public ArrayList()
			{
				this.Clear();
			}

			// Token: 0x0600068E RID: 1678 RVA: 0x00016BAC File Offset: 0x00014DAC
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

			// Token: 0x0600068F RID: 1679 RVA: 0x00016BFD File Offset: 0x00014DFD
			public void Clear()
			{
				this.data = new object[10];
				this.Count = 0;
			}

			// Token: 0x06000690 RID: 1680 RVA: 0x0000209F File Offset: 0x0000029F
			bool IList.Contains(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000691 RID: 1681 RVA: 0x0000209F File Offset: 0x0000029F
			int IList.IndexOf(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000692 RID: 1682 RVA: 0x0000209F File Offset: 0x0000029F
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000693 RID: 1683 RVA: 0x0000209F File Offset: 0x0000029F
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000694 RID: 1684 RVA: 0x0000209F File Offset: 0x0000029F
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x06000695 RID: 1685 RVA: 0x0000206F File Offset: 0x0000026F
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000B7 RID: 183
			// (get) Token: 0x06000696 RID: 1686 RVA: 0x0000206F File Offset: 0x0000026F
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000B8 RID: 184
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

			// Token: 0x06000699 RID: 1689 RVA: 0x00016C28 File Offset: 0x00014E28
			[NullableContext(1)]
			public void CopyTo(Array array, int index)
			{
				Array.Copy(this.data, 0, array, index, this.Count);
			}

			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x0600069A RID: 1690 RVA: 0x00016C3E File Offset: 0x00014E3E
			// (set) Token: 0x0600069B RID: 1691 RVA: 0x00016C46 File Offset: 0x00014E46
			public int Count { get; private set; }

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x0600069C RID: 1692 RVA: 0x0000206F File Offset: 0x0000026F
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x0600069D RID: 1693 RVA: 0x00016C4F File Offset: 0x00014E4F
			[Nullable(1)]
			public object SyncRoot
			{
				[NullableContext(1)]
				get
				{
					return this.data;
				}
			}

			// Token: 0x0600069E RID: 1694 RVA: 0x00016C57 File Offset: 0x00014E57
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

			// Token: 0x040002CC RID: 716
			[Nullable(new byte[]
			{
				1,
				2
			})]
			private object[] data;
		}
	}
}
