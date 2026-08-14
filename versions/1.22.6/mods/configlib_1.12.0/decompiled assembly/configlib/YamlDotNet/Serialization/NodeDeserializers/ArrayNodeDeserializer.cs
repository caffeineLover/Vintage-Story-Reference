using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000140 RID: 320
	public sealed class ArrayNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x00017494 File Offset: 0x00015694
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

		// Token: 0x02000141 RID: 321
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ArrayList : IList, ICollection, IEnumerable
		{
			// Token: 0x060006C9 RID: 1737 RVA: 0x000174E3 File Offset: 0x000156E3
			public ArrayList()
			{
				this.Clear();
			}

			// Token: 0x060006CA RID: 1738 RVA: 0x000174F4 File Offset: 0x000156F4
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

			// Token: 0x060006CB RID: 1739 RVA: 0x00017545 File Offset: 0x00015745
			public void Clear()
			{
				this.data = new object[10];
				this.Count = 0;
			}

			// Token: 0x060006CC RID: 1740 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			bool IList.Contains(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006CD RID: 1741 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			int IList.IndexOf(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006CE RID: 1742 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006CF RID: 1743 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060006D0 RID: 1744 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0000D44C File Offset: 0x0000B64C
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000B7 RID: 183
			// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0000D44C File Offset: 0x0000B64C
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

			// Token: 0x060006D5 RID: 1749 RVA: 0x00017570 File Offset: 0x00015770
			[NullableContext(1)]
			public void CopyTo(Array array, int index)
			{
				Array.Copy(this.data, 0, array, index, this.Count);
			}

			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x060006D6 RID: 1750 RVA: 0x00017586 File Offset: 0x00015786
			// (set) Token: 0x060006D7 RID: 1751 RVA: 0x0001758E File Offset: 0x0001578E
			public int Count { get; private set; }

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0000D44C File Offset: 0x0000B64C
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00017597 File Offset: 0x00015797
			[Nullable(1)]
			public object SyncRoot
			{
				[NullableContext(1)]
				get
				{
					return this.data;
				}
			}

			// Token: 0x060006DA RID: 1754 RVA: 0x0001759F File Offset: 0x0001579F
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

			// Token: 0x040002FA RID: 762
			[Nullable(new byte[]
			{
				1,
				2
			})]
			private object[] data;
		}
	}
}
