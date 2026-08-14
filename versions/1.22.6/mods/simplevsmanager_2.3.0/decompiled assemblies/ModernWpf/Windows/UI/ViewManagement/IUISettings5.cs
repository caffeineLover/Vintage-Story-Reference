using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000022 RID: 34
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("5349D588-0CB5-5F05-BD34-706B3231F0BD")]
	[ContractVersion(typeof(UniversalApiContract), 524288U)]
	internal interface IUISettings5
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000234 RID: 564
		bool AutoHideScrollBars { get; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000235 RID: 565
		// (remove) Token: 0x06000236 RID: 566
		event TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs> AutoHideScrollBarsChanged;
	}
}
