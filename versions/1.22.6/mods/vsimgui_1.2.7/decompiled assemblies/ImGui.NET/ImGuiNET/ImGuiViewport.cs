using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000070 RID: 112
	public struct ImGuiViewport
	{
		// Token: 0x04000704 RID: 1796
		public uint ID;

		// Token: 0x04000705 RID: 1797
		public ImGuiViewportFlags Flags;

		// Token: 0x04000706 RID: 1798
		public Vector2 Pos;

		// Token: 0x04000707 RID: 1799
		public Vector2 Size;

		// Token: 0x04000708 RID: 1800
		public Vector2 WorkPos;

		// Token: 0x04000709 RID: 1801
		public Vector2 WorkSize;

		// Token: 0x0400070A RID: 1802
		public float DpiScale;

		// Token: 0x0400070B RID: 1803
		public uint ParentViewportId;

		// Token: 0x0400070C RID: 1804
		public unsafe ImDrawData* DrawData;

		// Token: 0x0400070D RID: 1805
		public unsafe void* RendererUserData;

		// Token: 0x0400070E RID: 1806
		public unsafe void* PlatformUserData;

		// Token: 0x0400070F RID: 1807
		public unsafe void* PlatformHandle;

		// Token: 0x04000710 RID: 1808
		public unsafe void* PlatformHandleRaw;

		// Token: 0x04000711 RID: 1809
		public byte PlatformWindowCreated;

		// Token: 0x04000712 RID: 1810
		public byte PlatformRequestMove;

		// Token: 0x04000713 RID: 1811
		public byte PlatformRequestResize;

		// Token: 0x04000714 RID: 1812
		public byte PlatformRequestClose;
	}
}
