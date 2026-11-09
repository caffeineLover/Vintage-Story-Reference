using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000015 RID: 21
	public struct ImDrawData
	{
		// Token: 0x04000012 RID: 18
		public byte Valid;

		// Token: 0x04000013 RID: 19
		public int CmdListsCount;

		// Token: 0x04000014 RID: 20
		public int TotalIdxCount;

		// Token: 0x04000015 RID: 21
		public int TotalVtxCount;

		// Token: 0x04000016 RID: 22
		public ImVector CmdLists;

		// Token: 0x04000017 RID: 23
		public Vector2 DisplayPos;

		// Token: 0x04000018 RID: 24
		public Vector2 DisplaySize;

		// Token: 0x04000019 RID: 25
		public Vector2 FramebufferScale;

		// Token: 0x0400001A RID: 26
		public unsafe ImGuiViewport* OwnerViewport;
	}
}
