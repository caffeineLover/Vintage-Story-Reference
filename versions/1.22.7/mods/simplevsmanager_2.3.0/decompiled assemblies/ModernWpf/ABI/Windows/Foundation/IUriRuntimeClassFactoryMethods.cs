using System;
using ABI.System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E0 RID: 224
	internal static class IUriRuntimeClassFactoryMethods
	{
		// Token: 0x06000850 RID: 2128 RVA: 0x000178A8 File Offset: 0x00015AA8
		public unsafe static Uri CreateUri(IObjectReference _obj, string uri)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Uri result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(uri);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
						result = Uri.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				Uri.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00017934 File Offset: 0x00015B34
		public unsafe static Uri CreateWithRelativeUri(IObjectReference _obj, string baseUri, string relativeUri)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Uri result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(baseUri);
				MarshalString.Pinnable pinnable2 = new MarshalString.Pinnable(relativeUri);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						fixed (char* ptr3 = pinnable2.GetPinnableReference())
						{
							char* ptr4 = ptr3;
							method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
							ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), MarshalString.GetAbi(ref pinnable2), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
							result = Uri.FromAbi(intPtr);
						}
					}
				}
				finally
				{
					char* ptr = null;
					char* ptr3 = null;
				}
			}
			finally
			{
				Uri.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
