using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x02000026 RID: 38
	public struct ImFontConfig
	{
		// Token: 0x040000C2 RID: 194
		public unsafe void* FontData;

		// Token: 0x040000C3 RID: 195
		public int FontDataSize;

		// Token: 0x040000C4 RID: 196
		public byte FontDataOwnedByAtlas;

		// Token: 0x040000C5 RID: 197
		public int FontNo;

		// Token: 0x040000C6 RID: 198
		public float SizePixels;

		// Token: 0x040000C7 RID: 199
		public int OversampleH;

		// Token: 0x040000C8 RID: 200
		public int OversampleV;

		// Token: 0x040000C9 RID: 201
		public byte PixelSnapH;

		// Token: 0x040000CA RID: 202
		public Vector2 GlyphExtraSpacing;

		// Token: 0x040000CB RID: 203
		public Vector2 GlyphOffset;

		// Token: 0x040000CC RID: 204
		public unsafe ushort* GlyphRanges;

		// Token: 0x040000CD RID: 205
		public float GlyphMinAdvanceX;

		// Token: 0x040000CE RID: 206
		public float GlyphMaxAdvanceX;

		// Token: 0x040000CF RID: 207
		public byte MergeMode;

		// Token: 0x040000D0 RID: 208
		public uint FontBuilderFlags;

		// Token: 0x040000D1 RID: 209
		public float RasterizerMultiply;

		// Token: 0x040000D2 RID: 210
		public ushort EllipsisChar;

		// Token: 0x040000D3 RID: 211
		[FixedBuffer(typeof(byte), 40)]
		public ImFontConfig.<Name>e__FixedBuffer Name;

		// Token: 0x040000D4 RID: 212
		public unsafe ImFont* DstFont;

		// Token: 0x0200008C RID: 140
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 40)]
		public struct <Name>e__FixedBuffer
		{
			// Token: 0x040007EC RID: 2028
			public byte FixedElementField;
		}
	}
}
