using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000052 RID: 82
	public struct ImGuiPlatformMonitor
	{
		// Token: 0x040005CE RID: 1486
		public Vector2 MainPos;

		// Token: 0x040005CF RID: 1487
		public Vector2 MainSize;

		// Token: 0x040005D0 RID: 1488
		public Vector2 WorkPos;

		// Token: 0x040005D1 RID: 1489
		public Vector2 WorkSize;

		// Token: 0x040005D2 RID: 1490
		public float DpiScale;

		// Token: 0x040005D3 RID: 1491
		public unsafe void* PlatformHandle;
	}
}
