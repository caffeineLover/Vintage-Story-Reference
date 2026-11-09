using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000021 RID: 33
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("52BB3002-919B-4D6B-9B78-8DD66FF4B93B")]
	[ContractVersion(typeof(UniversalApiContract), 262144U)]
	internal interface IUISettings4
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000231 RID: 561
		bool AdvancedEffectsEnabled { get; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000232 RID: 562
		// (remove) Token: 0x06000233 RID: 563
		event TypedEventHandler<UISettings, object> AdvancedEffectsEnabledChanged;
	}
}
