using System;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020001AA RID: 426
	internal static class DelegateExtensions
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x0002A098 File Offset: 0x00028298
		public static void DynamicInvokeAbi(this Delegate del, object[] invoke_params)
		{
			Marshal.ThrowExceptionForHR((int)del.DynamicInvoke(invoke_params));
		}
	}
}
