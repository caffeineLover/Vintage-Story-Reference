using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001C5 RID: 453
	[Guid("82BA7092-4C88-427D-A7BC-16DD93FEB67E")]
	internal interface IRestrictedErrorInfo
	{
		// Token: 0x06000E4C RID: 3660
		void GetErrorDetails(out string description, out int error, out string restrictedDescription, out string capabilitySid);

		// Token: 0x06000E4D RID: 3661
		string GetReference();
	}
}
