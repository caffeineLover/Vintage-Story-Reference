using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000DA RID: 218
	internal static class IStringableMethods
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x00016778 File Offset: 0x00014978
		public unsafe static string ToString(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
