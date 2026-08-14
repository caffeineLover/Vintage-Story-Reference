using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000085 RID: 133
	[Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
	internal interface IIterator<T>
	{
		// Token: 0x0600045A RID: 1114
		bool _MoveNext();

		// Token: 0x0600045B RID: 1115
		uint GetMany(ref T[] items);

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600045C RID: 1116
		T _Current { get; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600045D RID: 1117
		bool HasCurrent { get; }
	}
}
