using System;
using System.Numerics;

namespace ImGuiNET
{
	// Token: 0x02000021 RID: 33
	public struct ImFontAtlas
	{
		// Token: 0x0400005E RID: 94
		public ImFontAtlasFlags Flags;

		// Token: 0x0400005F RID: 95
		public IntPtr TexID;

		// Token: 0x04000060 RID: 96
		public int TexDesiredWidth;

		// Token: 0x04000061 RID: 97
		public int TexGlyphPadding;

		// Token: 0x04000062 RID: 98
		public byte Locked;

		// Token: 0x04000063 RID: 99
		public unsafe void* UserData;

		// Token: 0x04000064 RID: 100
		public byte TexReady;

		// Token: 0x04000065 RID: 101
		public byte TexPixelsUseColors;

		// Token: 0x04000066 RID: 102
		public unsafe byte* TexPixelsAlpha8;

		// Token: 0x04000067 RID: 103
		public unsafe uint* TexPixelsRGBA32;

		// Token: 0x04000068 RID: 104
		public int TexWidth;

		// Token: 0x04000069 RID: 105
		public int TexHeight;

		// Token: 0x0400006A RID: 106
		public Vector2 TexUvScale;

		// Token: 0x0400006B RID: 107
		public Vector2 TexUvWhitePixel;

		// Token: 0x0400006C RID: 108
		public ImVector Fonts;

		// Token: 0x0400006D RID: 109
		public ImVector CustomRects;

		// Token: 0x0400006E RID: 110
		public ImVector ConfigData;

		// Token: 0x0400006F RID: 111
		public Vector4 TexUvLines_0;

		// Token: 0x04000070 RID: 112
		public Vector4 TexUvLines_1;

		// Token: 0x04000071 RID: 113
		public Vector4 TexUvLines_2;

		// Token: 0x04000072 RID: 114
		public Vector4 TexUvLines_3;

		// Token: 0x04000073 RID: 115
		public Vector4 TexUvLines_4;

		// Token: 0x04000074 RID: 116
		public Vector4 TexUvLines_5;

		// Token: 0x04000075 RID: 117
		public Vector4 TexUvLines_6;

		// Token: 0x04000076 RID: 118
		public Vector4 TexUvLines_7;

		// Token: 0x04000077 RID: 119
		public Vector4 TexUvLines_8;

		// Token: 0x04000078 RID: 120
		public Vector4 TexUvLines_9;

		// Token: 0x04000079 RID: 121
		public Vector4 TexUvLines_10;

		// Token: 0x0400007A RID: 122
		public Vector4 TexUvLines_11;

		// Token: 0x0400007B RID: 123
		public Vector4 TexUvLines_12;

		// Token: 0x0400007C RID: 124
		public Vector4 TexUvLines_13;

		// Token: 0x0400007D RID: 125
		public Vector4 TexUvLines_14;

		// Token: 0x0400007E RID: 126
		public Vector4 TexUvLines_15;

		// Token: 0x0400007F RID: 127
		public Vector4 TexUvLines_16;

		// Token: 0x04000080 RID: 128
		public Vector4 TexUvLines_17;

		// Token: 0x04000081 RID: 129
		public Vector4 TexUvLines_18;

		// Token: 0x04000082 RID: 130
		public Vector4 TexUvLines_19;

		// Token: 0x04000083 RID: 131
		public Vector4 TexUvLines_20;

		// Token: 0x04000084 RID: 132
		public Vector4 TexUvLines_21;

		// Token: 0x04000085 RID: 133
		public Vector4 TexUvLines_22;

		// Token: 0x04000086 RID: 134
		public Vector4 TexUvLines_23;

		// Token: 0x04000087 RID: 135
		public Vector4 TexUvLines_24;

		// Token: 0x04000088 RID: 136
		public Vector4 TexUvLines_25;

		// Token: 0x04000089 RID: 137
		public Vector4 TexUvLines_26;

		// Token: 0x0400008A RID: 138
		public Vector4 TexUvLines_27;

		// Token: 0x0400008B RID: 139
		public Vector4 TexUvLines_28;

		// Token: 0x0400008C RID: 140
		public Vector4 TexUvLines_29;

		// Token: 0x0400008D RID: 141
		public Vector4 TexUvLines_30;

		// Token: 0x0400008E RID: 142
		public Vector4 TexUvLines_31;

		// Token: 0x0400008F RID: 143
		public Vector4 TexUvLines_32;

		// Token: 0x04000090 RID: 144
		public Vector4 TexUvLines_33;

		// Token: 0x04000091 RID: 145
		public Vector4 TexUvLines_34;

		// Token: 0x04000092 RID: 146
		public Vector4 TexUvLines_35;

		// Token: 0x04000093 RID: 147
		public Vector4 TexUvLines_36;

		// Token: 0x04000094 RID: 148
		public Vector4 TexUvLines_37;

		// Token: 0x04000095 RID: 149
		public Vector4 TexUvLines_38;

		// Token: 0x04000096 RID: 150
		public Vector4 TexUvLines_39;

		// Token: 0x04000097 RID: 151
		public Vector4 TexUvLines_40;

		// Token: 0x04000098 RID: 152
		public Vector4 TexUvLines_41;

		// Token: 0x04000099 RID: 153
		public Vector4 TexUvLines_42;

		// Token: 0x0400009A RID: 154
		public Vector4 TexUvLines_43;

		// Token: 0x0400009B RID: 155
		public Vector4 TexUvLines_44;

		// Token: 0x0400009C RID: 156
		public Vector4 TexUvLines_45;

		// Token: 0x0400009D RID: 157
		public Vector4 TexUvLines_46;

		// Token: 0x0400009E RID: 158
		public Vector4 TexUvLines_47;

		// Token: 0x0400009F RID: 159
		public Vector4 TexUvLines_48;

		// Token: 0x040000A0 RID: 160
		public Vector4 TexUvLines_49;

		// Token: 0x040000A1 RID: 161
		public Vector4 TexUvLines_50;

		// Token: 0x040000A2 RID: 162
		public Vector4 TexUvLines_51;

		// Token: 0x040000A3 RID: 163
		public Vector4 TexUvLines_52;

		// Token: 0x040000A4 RID: 164
		public Vector4 TexUvLines_53;

		// Token: 0x040000A5 RID: 165
		public Vector4 TexUvLines_54;

		// Token: 0x040000A6 RID: 166
		public Vector4 TexUvLines_55;

		// Token: 0x040000A7 RID: 167
		public Vector4 TexUvLines_56;

		// Token: 0x040000A8 RID: 168
		public Vector4 TexUvLines_57;

		// Token: 0x040000A9 RID: 169
		public Vector4 TexUvLines_58;

		// Token: 0x040000AA RID: 170
		public Vector4 TexUvLines_59;

		// Token: 0x040000AB RID: 171
		public Vector4 TexUvLines_60;

		// Token: 0x040000AC RID: 172
		public Vector4 TexUvLines_61;

		// Token: 0x040000AD RID: 173
		public Vector4 TexUvLines_62;

		// Token: 0x040000AE RID: 174
		public Vector4 TexUvLines_63;

		// Token: 0x040000AF RID: 175
		public unsafe IntPtr* FontBuilderIO;

		// Token: 0x040000B0 RID: 176
		public uint FontBuilderFlags;

		// Token: 0x040000B1 RID: 177
		public int PackIdMouseCursors;

		// Token: 0x040000B2 RID: 178
		public int PackIdLines;
	}
}
