using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000DC RID: 220
	internal static class IUriEscapeStaticsMethods
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x000168AC File Offset: 0x00014AAC
		public unsafe static string UnescapeComponent(IObjectReference _obj, string toUnescape)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(toUnescape);
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

		// Token: 0x06000829 RID: 2089 RVA: 0x00016938 File Offset: 0x00014B38
		public unsafe static string EscapeComponent(IObjectReference _obj, string toEscape)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(toEscape);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
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
