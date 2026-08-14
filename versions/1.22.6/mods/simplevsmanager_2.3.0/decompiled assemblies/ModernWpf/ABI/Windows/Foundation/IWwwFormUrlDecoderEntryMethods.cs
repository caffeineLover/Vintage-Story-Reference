using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E4 RID: 228
	internal static class IWwwFormUrlDecoderEntryMethods
	{
		// Token: 0x0600085A RID: 2138 RVA: 0x00017CEC File Offset: 0x00015EEC
		public unsafe static string get_Name(IObjectReference _obj)
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

		// Token: 0x0600085B RID: 2139 RVA: 0x00017D4C File Offset: 0x00015F4C
		public unsafe static string get_Value(IObjectReference _obj)
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
