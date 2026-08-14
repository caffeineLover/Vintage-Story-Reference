using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001C8 RID: 456
	[WindowsRuntimeType(null)]
	[Guid("C03F6A43-65A4-9818-987E-E0B810D2A6F2")]
	internal interface IAgileReference
	{
		// Token: 0x06000E56 RID: 3670
		IObjectReference Resolve(Guid riid);
	}
}
