using System;
using WinRT;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200010D RID: 269
	internal static class IAgileReferenceMethods
	{
		// Token: 0x0600097F RID: 2431 RVA: 0x0001AC88 File Offset: 0x00018E88
		public unsafe static IObjectReference Resolve(IObjectReference _obj, Guid riid)
		{
			if (_obj == null)
			{
				return null;
			}
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr zero = IntPtr.Zero;
			method system.Int32_u0020(System.IntPtr,System.Guid*,System.IntPtr*) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)3 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*,System.IntPtr*), thisPtr, &riid, &zero, system.Int32_u0020(System.IntPtr,System.Guid*,System.IntPtr*)));
			IObjectReference objectReferenceForInterface;
			try
			{
				objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface(zero);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(zero);
			}
			return objectReferenceForInterface;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0001ACF0 File Offset: 0x00018EF0
		public unsafe static ObjectReference<T> Resolve<T>(IObjectReference _obj, Guid riid)
		{
			if (_obj == null)
			{
				return null;
			}
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr zero = IntPtr.Zero;
			method system.Int32_u0020(System.IntPtr,System.Guid*,System.IntPtr*) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)3 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*,System.IntPtr*), thisPtr, &riid, &zero, system.Int32_u0020(System.IntPtr,System.Guid*,System.IntPtr*)));
			ObjectReference<T> objectReferenceForInterface;
			try
			{
				objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface<T>(zero);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(zero);
			}
			return objectReferenceForInterface;
		}
	}
}
