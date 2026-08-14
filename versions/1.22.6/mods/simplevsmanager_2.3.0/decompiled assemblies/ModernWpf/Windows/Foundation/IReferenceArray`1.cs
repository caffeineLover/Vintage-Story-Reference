using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation
{
	// Token: 0x0200002F RID: 47
	[Guid("61C17707-2D65-11E0-9AE8-D48564015472")]
	internal interface IReferenceArray<T>
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060002EA RID: 746
		T[] Value { get; }
	}
}
