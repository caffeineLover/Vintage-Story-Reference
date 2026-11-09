using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E8 RID: 232
	internal static class IWwwFormUrlDecoderRuntimeClassFactoryMethods
	{
		// Token: 0x06000868 RID: 2152 RVA: 0x0001808C File Offset: 0x0001628C
		public unsafe static Windows.Foundation.WwwFormUrlDecoder CreateWwwFormUrlDecoder(IObjectReference _obj, string query)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.WwwFormUrlDecoder result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(query);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
						result = WwwFormUrlDecoder.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				WwwFormUrlDecoder.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
