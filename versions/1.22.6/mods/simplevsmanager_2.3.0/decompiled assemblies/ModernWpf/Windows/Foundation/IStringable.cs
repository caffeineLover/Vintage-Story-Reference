using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000048 RID: 72
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("96369F54-8EB6-48F0-ABCE-C1B211E627C3")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IStringable
	{
		// Token: 0x0600036C RID: 876
		string ToString();
	}
}
