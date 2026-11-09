using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001D3 RID: 467
	[WindowsRuntimeType(null)]
	[Guid("00000038-0000-0000-C000-000000000046")]
	internal interface IWeakReferenceSource
	{
		// Token: 0x06000E76 RID: 3702
		IWeakReference GetWeakReference();
	}
}
