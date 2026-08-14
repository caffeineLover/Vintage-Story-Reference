using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E6 RID: 230
	internal static class IWwwFormUrlDecoderRuntimeClassMethods
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x00017F0C File Offset: 0x0001610C
		public unsafe static string GetFirstValueByName(IObjectReference _obj, string name)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(name);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
						result = MarshalString.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}
	}
}
