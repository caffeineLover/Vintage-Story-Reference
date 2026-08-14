using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	internal struct ArrayContainer<[Nullable(2)] T>
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00006AC3 File Offset: 0x00004CC3
		public readonly T[] Items
		{
			[CompilerGenerated]
			get
			{
				return this.<Items>k__BackingField;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00006ACB File Offset: 0x00004CCB
		public ArrayContainer(object array)
		{
			this.<Items>k__BackingField = ((array as T[]) ?? Array.Empty<T>());
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00006AE2 File Offset: 0x00004CE2
		public bool Equals([Nullable(new byte[]
		{
			0,
			1
		})] ArrayContainer<T> other)
		{
			return Enumerable.SequenceEqual<T>(this.Items, other.Items);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00006AF8 File Offset: 0x00004CF8
		public override bool Equals(object obj)
		{
			if (obj is ArrayContainer<T>)
			{
				ArrayContainer<T> arrayContainer = (ArrayContainer<T>)obj;
				return this.Equals(arrayContainer);
			}
			return false;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00006B20 File Offset: 0x00004D20
		public override int GetHashCode()
		{
			if (this.Items.Length == 1)
			{
				return this.Items[0].GetHashCode();
			}
			int num = 19;
			foreach (T t in this.Items)
			{
				num = num * 31 + t.GetHashCode();
			}
			return num;
		}
	}
}
