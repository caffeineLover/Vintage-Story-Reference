using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000089 RID: 137
	[Guid("02B51929-C1C4-4A7E-8940-0312B5C18500")]
	internal interface IKeyValuePair<K, V>
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000472 RID: 1138
		K Key { get; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000473 RID: 1139
		V Value { get; }
	}
}
