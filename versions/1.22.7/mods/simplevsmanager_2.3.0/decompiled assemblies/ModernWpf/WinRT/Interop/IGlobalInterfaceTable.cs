using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001CA RID: 458
	[WindowsRuntimeType(null)]
	[Guid("00000146-0000-0000-C000-000000000046")]
	internal interface IGlobalInterfaceTable
	{
		// Token: 0x06000E58 RID: 3672
		IntPtr RegisterInterfaceInGlobal(IntPtr ptr, Guid riid);

		// Token: 0x06000E59 RID: 3673
		void RevokeInterfaceFromGlobal(IntPtr cookie);

		// Token: 0x06000E5A RID: 3674
		IObjectReference GetInterfaceFromGlobal(IntPtr cookie, Guid riid);
	}
}
