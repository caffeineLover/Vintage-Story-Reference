using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ABI.System.Collections.Generic;
using Windows.Foundation.Collections;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F2 RID: 242
	[DynamicInterfaceCastableImplementation]
	[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
	internal interface IIterable<T> : IEnumerable<T>, IEnumerable<T>, IEnumerable, Windows.Foundation.Collections.IIterable<T>
	{
		// Token: 0x060008BB RID: 2235 RVA: 0x000193AC File Offset: 0x000175AC
		static IIterable()
		{
			IIterable<T>.PIID = IEnumerable<T>.PIID;
		}

		// Token: 0x04000194 RID: 404
		public static Guid PIID;
	}
}
