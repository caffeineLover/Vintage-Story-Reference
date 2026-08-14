using System;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C2 RID: 194
	internal static class IAsyncInfoMethods
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x00012CE0 File Offset: 0x00010EE0
		public unsafe static void Cancel(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			method system.Int32_u0020(System.IntPtr) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)9 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, system.Int32_u0020(System.IntPtr)));
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00012D14 File Offset: 0x00010F14
		public unsafe static void Close(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			method system.Int32_u0020(System.IntPtr) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)10 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, system.Int32_u0020(System.IntPtr)));
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00012D48 File Offset: 0x00010F48
		public unsafe static Exception get_ErrorCode(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Exception ex = default(Exception);
			Exception result;
			try
			{
				method system.Int32_u0020(System.IntPtr,ABI.System.Exception&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.Exception& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref ex, system.Int32_u0020(System.IntPtr,ABI.System.Exception&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = Exception.FromAbi(ex);
			}
			finally
			{
				Exception.DisposeAbi(ex);
			}
			return result;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00012DA8 File Offset: 0x00010FA8
		public unsafe static uint get_Id(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00012DE0 File Offset: 0x00010FE0
		public unsafe static AsyncStatus get_Status(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			AsyncStatus result = AsyncStatus.Started;
			method system.Int32_u0020(System.IntPtr,Windows.Foundation.AsyncStatus&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.AsyncStatus& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.Foundation.AsyncStatus&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}
	}
}
