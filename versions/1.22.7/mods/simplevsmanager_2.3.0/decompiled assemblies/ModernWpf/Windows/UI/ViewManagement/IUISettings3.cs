using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000020 RID: 32
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("03021BE4-5254-4781-8194-5168F7D06D7B")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUISettings3
	{
		// Token: 0x0600022E RID: 558
		Color GetColorValue(UIColorType desiredColor);

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600022F RID: 559
		// (remove) Token: 0x06000230 RID: 560
		event TypedEventHandler<UISettings, object> ColorValuesChanged;
	}
}
