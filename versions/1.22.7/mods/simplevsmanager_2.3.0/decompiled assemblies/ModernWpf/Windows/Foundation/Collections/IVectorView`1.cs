using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000088 RID: 136
	[Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
	internal interface IVectorView<T> : IIterable<T>
	{
		// Token: 0x0600046E RID: 1134
		T GetAt(uint index);

		// Token: 0x0600046F RID: 1135
		bool IndexOf(T value, out uint index);

		// Token: 0x06000470 RID: 1136
		uint GetMany(uint startIndex, ref T[] items);

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000471 RID: 1137
		uint Size { get; }
	}
}
