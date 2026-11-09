using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D2 RID: 210
	internal static class IMemoryBufferMethods
	{
		// Token: 0x060007BF RID: 1983 RVA: 0x00013BF8 File Offset: 0x00011DF8
		public unsafe static Windows.Foundation.IMemoryBufferReference CreateReference(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.IMemoryBufferReference result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInterface<Windows.Foundation.IMemoryBufferReference>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInterface<Windows.Foundation.IMemoryBufferReference>.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
