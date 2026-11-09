using System;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x02000100 RID: 256
	internal static class IVectorChangedEventArgsMethods
	{
		// Token: 0x06000908 RID: 2312 RVA: 0x00019DFC File Offset: 0x00017FFC
		public unsafe static CollectionChange get_CollectionChange(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			CollectionChange result = CollectionChange.Reset;
			method system.Int32_u0020(System.IntPtr,Windows.Foundation.Collections.CollectionChange&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Collections.CollectionChange& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.Foundation.Collections.CollectionChange&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00019E34 File Offset: 0x00018034
		public unsafe static uint get_Index(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}
	}
}
