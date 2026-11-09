using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D4 RID: 212
	internal static class IMemoryBufferFactoryMethods
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x00013D3C File Offset: 0x00011F3C
		public unsafe static Windows.Foundation.MemoryBuffer Create(IObjectReference _obj, uint capacity)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.MemoryBuffer result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.UInt32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, capacity, ref intPtr, system.Int32_u0020(System.IntPtr,System.UInt32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MemoryBuffer.FromAbi(intPtr);
			}
			finally
			{
				MemoryBuffer.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
