using System;
using System.Collections.Generic;
using ABI.System.Collections.Generic;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F1 RID: 241
	internal static class IIterableMethods<T>
	{
		// Token: 0x060008BA RID: 2234 RVA: 0x00019348 File Offset: 0x00017548
		public static IEnumerator<T> First(IObjectReference obj)
		{
			IntPtr intPtr = default(IntPtr);
			IEnumerator<T> result;
			try
			{
				ObjectReference<IEnumerable<T>.Vftbl> objectReference = (ObjectReference<IEnumerable<T>.Vftbl>)obj;
				IntPtr thisPtr = objectReference.ThisPtr;
				method first_ = objectReference.Vftbl.First_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, first_));
				result = IEnumerator<T>.FromAbi(intPtr);
			}
			finally
			{
				IEnumerator<T>.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
