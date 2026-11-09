using System;
using System.Collections.Generic;
using ABI.Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x0200015A RID: 346
	internal static class IEnumerableMethods<T>
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x0001EDF0 File Offset: 0x0001CFF0
		public static IEnumerator<T> GetEnumerator(IObjectReference obj)
		{
			IEnumerator<T> enumerator = IIterableMethods<T>.First(obj) as IEnumerator<T>;
			if (enumerator != null)
			{
				return enumerator;
			}
			throw new InvalidOperationException("Unexpected type for enumerator");
		}
	}
}
