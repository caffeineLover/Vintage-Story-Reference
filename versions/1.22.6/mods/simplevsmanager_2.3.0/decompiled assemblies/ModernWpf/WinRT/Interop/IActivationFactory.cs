using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001C7 RID: 455
	[WindowsRuntimeType(null)]
	[Guid("00000035-0000-0000-C000-000000000046")]
	internal interface IActivationFactory
	{
		// Token: 0x06000E55 RID: 3669
		IntPtr ActivateInstance();
	}
}
