using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x0200001F RID: 31
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("BAD82401-2721-44F9-BB91-2BB228BE442F")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUISettings2
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600022B RID: 555
		double TextScaleFactor { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600022C RID: 556
		// (remove) Token: 0x0600022D RID: 557
		event TypedEventHandler<UISettings, object> TextScaleFactorChanged;
	}
}
