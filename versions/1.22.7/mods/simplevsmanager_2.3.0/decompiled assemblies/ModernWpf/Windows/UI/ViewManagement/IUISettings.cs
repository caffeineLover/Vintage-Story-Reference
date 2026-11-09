using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x0200001E RID: 30
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("85361600-1C63-4627-BCB1-3A89E0BC9C55")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal interface IUISettings
	{
		// Token: 0x0600021E RID: 542
		Color UIElementColor(UIElementType desiredElement);

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600021F RID: 543
		bool AnimationsEnabled { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000220 RID: 544
		uint CaretBlinkRate { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000221 RID: 545
		bool CaretBrowsingEnabled { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000222 RID: 546
		uint CaretWidth { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000223 RID: 547
		Size CursorSize { get; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000224 RID: 548
		uint DoubleClickTime { get; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000225 RID: 549
		HandPreference HandPreference { get; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000226 RID: 550
		uint MessageDuration { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000227 RID: 551
		uint MouseHoverTime { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000228 RID: 552
		Size ScrollBarArrowSize { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000229 RID: 553
		Size ScrollBarSize { get; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600022A RID: 554
		Size ScrollBarThumbBoxSize { get; }
	}
}
