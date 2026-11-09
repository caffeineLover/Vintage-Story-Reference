using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000011 RID: 17
	public struct ImDrawCmd
	{
		// Token: 0x04000006 RID: 6
		public Vector4 ClipRect;

		// Token: 0x04000007 RID: 7
		public IntPtr TextureId;

		// Token: 0x04000008 RID: 8
		public uint VtxOffset;

		// Token: 0x04000009 RID: 9
		public uint IdxOffset;

		// Token: 0x0400000A RID: 10
		public uint ElemCount;

		// Token: 0x0400000B RID: 11
		public IntPtr UserCallback;

		// Token: 0x0400000C RID: 12
		public unsafe void* UserCallbackData;
	}
}
