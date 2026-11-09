using System;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x02000080 RID: 128
	internal class BoxedValue<T>
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x00027C89 File Offset: 0x00025E89
		internal BoxedValue(T v = default(T))
		{
			this.Val = v;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00027C98 File Offset: 0x00025E98
		public override string ToString()
		{
			if (this.Val != null)
			{
				return this.Val.ToString();
			}
			return "null";
		}

		// Token: 0x0400042D RID: 1069
		internal T Val;
	}
}
