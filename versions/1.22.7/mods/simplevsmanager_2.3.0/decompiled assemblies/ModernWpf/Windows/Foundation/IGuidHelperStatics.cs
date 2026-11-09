using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000043 RID: 67
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("59C7966B-AE52-5283-AD7F-A1B9E9678ADD")]
	[ContractVersion(typeof(UniversalApiContract), 458752U)]
	internal interface IGuidHelperStatics
	{
		// Token: 0x0600033D RID: 829
		Guid CreateNewGuid();

		// Token: 0x0600033E RID: 830
		bool Equals(in Guid target, in Guid value);

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600033F RID: 831
		Guid Empty { get; }
	}
}
