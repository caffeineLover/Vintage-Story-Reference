using System;

namespace ImGuiNET
{
	// Token: 0x02000054 RID: 84
	[Flags]
	public enum ImGuiPopupFlags
	{
		// Token: 0x040005D6 RID: 1494
		None = 0,
		// Token: 0x040005D7 RID: 1495
		MouseButtonLeft = 0,
		// Token: 0x040005D8 RID: 1496
		MouseButtonRight = 1,
		// Token: 0x040005D9 RID: 1497
		MouseButtonMiddle = 2,
		// Token: 0x040005DA RID: 1498
		MouseButtonMask = 31,
		// Token: 0x040005DB RID: 1499
		MouseButtonDefault = 1,
		// Token: 0x040005DC RID: 1500
		NoOpenOverExistingPopup = 32,
		// Token: 0x040005DD RID: 1501
		NoOpenOverItems = 64,
		// Token: 0x040005DE RID: 1502
		AnyPopupId = 128,
		// Token: 0x040005DF RID: 1503
		AnyPopupLevel = 256,
		// Token: 0x040005E0 RID: 1504
		AnyPopup = 384
	}
}
