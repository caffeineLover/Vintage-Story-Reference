using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001C2 RID: 450
	[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
	internal interface IErrorInfo
	{
		// Token: 0x06000E45 RID: 3653
		Guid GetGuid();

		// Token: 0x06000E46 RID: 3654
		string GetSource();

		// Token: 0x06000E47 RID: 3655
		string GetDescription();

		// Token: 0x06000E48 RID: 3656
		string GetHelpFile();

		// Token: 0x06000E49 RID: 3657
		string GetHelpFileContent();
	}
}
