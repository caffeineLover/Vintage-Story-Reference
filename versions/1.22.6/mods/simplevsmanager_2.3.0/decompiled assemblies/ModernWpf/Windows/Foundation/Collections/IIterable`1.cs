using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000084 RID: 132
	[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
	internal interface IIterable<T>
	{
		// Token: 0x06000459 RID: 1113
		IEnumerator<T> First();
	}
}
