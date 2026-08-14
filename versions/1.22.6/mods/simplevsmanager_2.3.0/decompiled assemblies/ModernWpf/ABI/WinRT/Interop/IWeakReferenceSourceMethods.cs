using System;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000114 RID: 276
	internal static class IWeakReferenceSourceMethods
	{
		// Token: 0x060009AB RID: 2475 RVA: 0x0001B284 File Offset: 0x00019484
		public unsafe static WinRT.Interop.IWeakReference GetWeakReference(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr zero = IntPtr.Zero;
			WinRT.Interop.IWeakReference result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr*) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)3 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), thisPtr, &zero, system.Int32_u0020(System.IntPtr,System.IntPtr*)));
				result = MarshalInterface<WinRT.Interop.IWeakReference>.FromAbi(zero);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(zero);
			}
			return result;
		}
	}
}
