using System;

namespace ImGuiNET
{
	// Token: 0x02000018 RID: 24
	public struct ImDrawList
	{
		// Token: 0x0400002B RID: 43
		public ImVector CmdBuffer;

		// Token: 0x0400002C RID: 44
		public ImVector IdxBuffer;

		// Token: 0x0400002D RID: 45
		public ImVector VtxBuffer;

		// Token: 0x0400002E RID: 46
		public ImDrawListFlags Flags;

		// Token: 0x0400002F RID: 47
		public uint _VtxCurrentIdx;

		// Token: 0x04000030 RID: 48
		public IntPtr _Data;

		// Token: 0x04000031 RID: 49
		public unsafe byte* _OwnerName;

		// Token: 0x04000032 RID: 50
		public unsafe ImDrawVert* _VtxWritePtr;

		// Token: 0x04000033 RID: 51
		public unsafe ushort* _IdxWritePtr;

		// Token: 0x04000034 RID: 52
		public ImVector _ClipRectStack;

		// Token: 0x04000035 RID: 53
		public ImVector _TextureIdStack;

		// Token: 0x04000036 RID: 54
		public ImVector _Path;

		// Token: 0x04000037 RID: 55
		public ImDrawCmdHeader _CmdHeader;

		// Token: 0x04000038 RID: 56
		public ImDrawListSplitter _Splitter;

		// Token: 0x04000039 RID: 57
		public float _FringeScale;
	}
}
