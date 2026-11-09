using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000023 RID: 35
	public struct ImFontAtlasCustomRect
	{
		// Token: 0x040000B4 RID: 180
		public ushort Width;

		// Token: 0x040000B5 RID: 181
		public ushort Height;

		// Token: 0x040000B6 RID: 182
		public ushort X;

		// Token: 0x040000B7 RID: 183
		public ushort Y;

		// Token: 0x040000B8 RID: 184
		public uint GlyphID;

		// Token: 0x040000B9 RID: 185
		public float GlyphAdvanceX;

		// Token: 0x040000BA RID: 186
		public Vector2 GlyphOffset;

		// Token: 0x040000BB RID: 187
		public unsafe ImFont* Font;
	}
}
