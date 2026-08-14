using System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D0 RID: 208
	internal static class IGuidHelperStaticsMethods
	{
		// Token: 0x060007B8 RID: 1976 RVA: 0x000139A8 File Offset: 0x00011BA8
		public unsafe static Guid CreateNewGuid(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Guid result = default(Guid);
			method system.Int32_u0020(System.IntPtr,System.Guid&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.Guid&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000139E8 File Offset: 0x00011BE8
		public unsafe static bool Equals(IObjectReference _obj, in Guid target, in Guid value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			method system.Int32_u0020(System.IntPtr,System.Guid&_u0020modreq(System.Runtime.InteropServices.InAttribute),System.Guid&_u0020modreq(System.Runtime.InteropServices.InAttribute),System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid& modreq(System.Runtime.InteropServices.InAttribute),System.Guid& modreq(System.Runtime.InteropServices.InAttribute),System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref target, ref value, ref b, system.Int32_u0020(System.IntPtr,System.Guid&_u0020modreq(System.Runtime.InteropServices.InAttribute),System.Guid&_u0020modreq(System.Runtime.InteropServices.InAttribute),System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return b > 0;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00013A28 File Offset: 0x00011C28
		public unsafe static Guid get_Empty(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Guid result = default(Guid);
			method system.Int32_u0020(System.IntPtr,System.Guid&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.Guid&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}
	}
}
