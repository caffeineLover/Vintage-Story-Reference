using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001D4 RID: 468
	[WindowsRuntimeType(null)]
	[Guid("00000037-0000-0000-C000-000000000046")]
	internal interface IWeakReference
	{
		// Token: 0x06000E77 RID: 3703
		IObjectReference Resolve(Guid riid);
	}
}
