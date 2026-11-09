using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x0200001F RID: 31
	public struct ImFont
	{
		// Token: 0x04000049 RID: 73
		public ImVector IndexAdvanceX;

		// Token: 0x0400004A RID: 74
		public float FallbackAdvanceX;

		// Token: 0x0400004B RID: 75
		public float FontSize;

		// Token: 0x0400004C RID: 76
		public ImVector IndexLookup;

		// Token: 0x0400004D RID: 77
		public ImVector Glyphs;

		// Token: 0x0400004E RID: 78
		public unsafe ImFontGlyph* FallbackGlyph;

		// Token: 0x0400004F RID: 79
		public unsafe ImFontAtlas* ContainerAtlas;

		// Token: 0x04000050 RID: 80
		public unsafe ImFontConfig* ConfigData;

		// Token: 0x04000051 RID: 81
		public short ConfigDataCount;

		// Token: 0x04000052 RID: 82
		public ushort FallbackChar;

		// Token: 0x04000053 RID: 83
		public ushort EllipsisChar;

		// Token: 0x04000054 RID: 84
		public short EllipsisCharCount;

		// Token: 0x04000055 RID: 85
		public float EllipsisWidth;

		// Token: 0x04000056 RID: 86
		public float EllipsisCharStep;

		// Token: 0x04000057 RID: 87
		public byte DirtyLookupTables;

		// Token: 0x04000058 RID: 88
		public float Scale;

		// Token: 0x04000059 RID: 89
		public float Ascent;

		// Token: 0x0400005A RID: 90
		public float Descent;

		// Token: 0x0400005B RID: 91
		public int MetricsTotalSurface;

		// Token: 0x0400005C RID: 92
		[FixedBuffer(typeof(byte), 2)]
		public ImFont.<Used4kPagesMap>e__FixedBuffer Used4kPagesMap;

		// Token: 0x0200008B RID: 139
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 2)]
		public struct <Used4kPagesMap>e__FixedBuffer
		{
			// Token: 0x040007EB RID: 2027
			public byte FixedElementField;
		}
	}
}
