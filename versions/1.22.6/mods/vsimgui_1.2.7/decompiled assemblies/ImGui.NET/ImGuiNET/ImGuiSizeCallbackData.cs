using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000056 RID: 86
	public struct ImGuiSizeCallbackData
	{
		// Token: 0x040005E8 RID: 1512
		public unsafe void* UserData;

		// Token: 0x040005E9 RID: 1513
		public Vector2 Pos;

		// Token: 0x040005EA RID: 1514
		public Vector2 CurrentSize;

		// Token: 0x040005EB RID: 1515
		public Vector2 DesiredSize;
	}
}
