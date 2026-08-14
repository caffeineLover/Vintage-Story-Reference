using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000CA RID: 202
	internal static class IDeferralMethods
	{
		// Token: 0x060007AD RID: 1965 RVA: 0x00013630 File Offset: 0x00011830
		public unsafe static void Complete(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			method system.Int32_u0020(System.IntPtr) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, system.Int32_u0020(System.IntPtr)));
		}
	}
}
