using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000CE RID: 206
	internal static class IGetActivationFactoryMethods
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x00013844 File Offset: 0x00011A44
		public unsafe static object GetActivationFactory(IObjectReference _obj, string activatableClassId)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(activatableClassId);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
						result = MarshalInspectable<object>.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
