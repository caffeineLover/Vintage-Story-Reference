using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E2 RID: 226
	internal static class IUriRuntimeClassWithAbsoluteCanonicalUriMethods
	{
		// Token: 0x06000855 RID: 2133 RVA: 0x00017B10 File Offset: 0x00015D10
		public unsafe static string get_AbsoluteCanonicalUri(IObjectReference _obj)
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

		// Token: 0x06000856 RID: 2134 RVA: 0x00017B70 File Offset: 0x00015D70
		public unsafe static string get_DisplayIri(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
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
