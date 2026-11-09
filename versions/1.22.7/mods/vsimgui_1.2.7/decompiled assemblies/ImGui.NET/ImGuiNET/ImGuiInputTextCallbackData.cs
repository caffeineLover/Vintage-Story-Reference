using System;

namespace ImGuiNET
{
	// Token: 0x0200003A RID: 58
	public struct ImGuiInputTextCallbackData
	{
		// Token: 0x040001AF RID: 431
		public IntPtr Ctx;

		// Token: 0x040001B0 RID: 432
		public ImGuiInputTextFlags EventFlag;

		// Token: 0x040001B1 RID: 433
		public ImGuiInputTextFlags Flags;

		// Token: 0x040001B2 RID: 434
		public unsafe void* UserData;

		// Token: 0x040001B3 RID: 435
		public ushort EventChar;

		// Token: 0x040001B4 RID: 436
		public ImGuiKey EventKey;

		// Token: 0x040001B5 RID: 437
		public unsafe byte* Buf;

		// Token: 0x040001B6 RID: 438
		public int BufTextLen;

		// Token: 0x040001B7 RID: 439
		public int BufSize;

		// Token: 0x040001B8 RID: 440
		public byte BufDirty;

		// Token: 0x040001B9 RID: 441
		public int CursorPos;

		// Token: 0x040001BA RID: 442
		public int SelectionStart;

		// Token: 0x040001BB RID: 443
		public int SelectionEnd;
	}
}
