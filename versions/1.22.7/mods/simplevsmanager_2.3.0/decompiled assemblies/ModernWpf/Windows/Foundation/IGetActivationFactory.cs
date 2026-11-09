using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000042 RID: 66
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("4EDB8EE2-96DD-49A7-94F7-4607DDAB8E3C")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IGetActivationFactory
	{
		// Token: 0x0600033C RID: 828
		object GetActivationFactory(string activatableClassId);
	}
}
