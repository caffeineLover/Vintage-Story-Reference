using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000154 RID: 340
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StaticArrayNodeDeserializer : INodeDeserializer
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x00018BB1 File Offset: 0x00016DB1
		public StaticArrayNodeDeserializer(StaticObjectFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00018BD0 File Offset: 0x00016DD0
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

		// Token: 0x04000321 RID: 801
		private readonly StaticObjectFactory factory;

		// Token: 0x02000155 RID: 341
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ArrayList : IList, ICollection, IEnumerable
		{
			// Token: 0x0600071D RID: 1821 RVA: 0x00018C36 File Offset: 0x00016E36
			public ArrayList()
			{
				this.Clear();
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x00018C44 File Offset: 0x00016E44
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

			// Token: 0x0600071F RID: 1823 RVA: 0x00018C95 File Offset: 0x00016E95
			public void Clear()
			{
				this.data = new object[10];
				this.Count = 0;
			}

			// Token: 0x06000720 RID: 1824 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			bool IList.Contains(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			int IList.IndexOf(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000722 RID: 1826 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000723 RID: 1827 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x0000E3DD File Offset: 0x0000C5DD
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x170000BE RID: 190
			// (get) Token: 0x06000725 RID: 1829 RVA: 0x0000D44C File Offset: 0x0000B64C
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x06000726 RID: 1830 RVA: 0x0000D44C File Offset: 0x0000B64C
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

			// Token: 0x06000729 RID: 1833 RVA: 0x00018CC0 File Offset: 0x00016EC0
			[NullableContext(1)]
			public void CopyTo(Array array, int index)
			{
				Array.Copy(this.data, 0, array, index, this.Count);
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x0600072A RID: 1834 RVA: 0x00018CD6 File Offset: 0x00016ED6
			// (set) Token: 0x0600072B RID: 1835 RVA: 0x00018CDE File Offset: 0x00016EDE
			public int Count { get; private set; }

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x0600072C RID: 1836 RVA: 0x0000D44C File Offset: 0x0000B64C
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x0600072D RID: 1837 RVA: 0x00018CE7 File Offset: 0x00016EE7
			[Nullable(1)]
			public object SyncRoot
			{
				[NullableContext(1)]
				get
				{
					return this.data;
				}
			}

			// Token: 0x0600072E RID: 1838 RVA: 0x00018CEF File Offset: 0x00016EEF
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

			// Token: 0x04000322 RID: 802
			[Nullable(new byte[]
			{
				1,
				2
			})]
			private object[] data;
		}
	}
}
