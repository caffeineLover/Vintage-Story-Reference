using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000CC RID: 204
	internal static class IDeferralFactoryMethods
	{
		// Token: 0x060007B1 RID: 1969 RVA: 0x00013714 File Offset: 0x00011914
		public unsafe static Windows.Foundation.Deferral Create(IObjectReference _obj, Windows.Foundation.DeferralCompletedHandler handler)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.Deferral result;
			try
			{
				objectReferenceValue = DeferralCompletedHandler.CreateMarshaler2(handler);
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = Deferral.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				Deferral.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
