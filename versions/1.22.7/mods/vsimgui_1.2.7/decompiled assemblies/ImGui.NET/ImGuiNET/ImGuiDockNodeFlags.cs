using System;

namespace ImGuiNET
{
	// Token: 0x02000036 RID: 54
	[Flags]
	public enum ImGuiDockNodeFlags
	{
		// Token: 0x0400017E RID: 382
		None = 0,
		// Token: 0x0400017F RID: 383
		KeepAliveOnly = 1,
		// Token: 0x04000180 RID: 384
		NoDockingInCentralNode = 4,
		// Token: 0x04000181 RID: 385
		PassthruCentralNode = 8,
		// Token: 0x04000182 RID: 386
		NoSplit = 16,
		// Token: 0x04000183 RID: 387
		NoResize = 32,
		// Token: 0x04000184 RID: 388
		AutoHideTabBar = 64
	}
}
