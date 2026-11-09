using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000BD RID: 189
	internal static class IAsyncActionMethods
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x000126C4 File Offset: 0x000108C4
		public unsafe static void GetResults(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			method system.Int32_u0020(System.IntPtr) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, system.Int32_u0020(System.IntPtr)));
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000126F8 File Offset: 0x000108F8
		public unsafe static Windows.Foundation.AsyncActionCompletedHandler get_Completed(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.AsyncActionCompletedHandler result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = AsyncActionCompletedHandler.FromAbi(intPtr);
			}
			finally
			{
				AsyncActionCompletedHandler.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00012758 File Offset: 0x00010958
		public unsafe static void set_Completed(IObjectReference _obj, Windows.Foundation.AsyncActionCompletedHandler value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = AsyncActionCompletedHandler.CreateMarshaler2(value);
				method system.Int32_u0020(System.IntPtr,System.IntPtr) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), system.Int32_u0020(System.IntPtr,System.IntPtr)));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}
	}
}
