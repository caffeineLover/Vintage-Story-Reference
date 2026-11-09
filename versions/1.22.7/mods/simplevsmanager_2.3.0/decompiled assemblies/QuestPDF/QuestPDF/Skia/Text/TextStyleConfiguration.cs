using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x0200002C RID: 44
	internal struct TextStyleConfiguration
	{
		// Token: 0x0400008E RID: 142
		public float FontSize;

		// Token: 0x0400008F RID: 143
		public TextStyleConfiguration.FontWeights FontWeight;

		// Token: 0x04000090 RID: 144
		public bool IsItalic;

		// Token: 0x04000091 RID: 145
		public const int FONT_FAMILIES_LENGTH = 16;

		// Token: 0x04000092 RID: 146
		[Nullable(1)]
		[MarshalAs(30, SizeConst = 16)]
		public IntPtr[] FontFamilies;

		// Token: 0x04000093 RID: 147
		public const int FONT_FEATURES_LENGTH = 16;

		// Token: 0x04000094 RID: 148
		[Nullable(1)]
		[MarshalAs(30, SizeConst = 16)]
		public TextStyleConfiguration.FontFeature[] FontFeatures;

		// Token: 0x04000095 RID: 149
		public uint ForegroundColor;

		// Token: 0x04000096 RID: 150
		public uint BackgroundColor;

		// Token: 0x04000097 RID: 151
		public uint DecorationColor;

		// Token: 0x04000098 RID: 152
		public float DecorationThickness;

		// Token: 0x04000099 RID: 153
		public TextStyleConfiguration.TextDecoration DecorationType;

		// Token: 0x0400009A RID: 154
		public TextStyleConfiguration.TextDecorationMode DecorationMode;

		// Token: 0x0400009B RID: 155
		public TextStyleConfiguration.TextDecorationStyle DecorationStyle;

		// Token: 0x0400009C RID: 156
		public float LineHeight;

		// Token: 0x0400009D RID: 157
		public float LetterSpacing;

		// Token: 0x0400009E RID: 158
		public float WordSpacing;

		// Token: 0x0400009F RID: 159
		public float BaselineOffset;

		// Token: 0x0200014E RID: 334
		public struct FontFeature
		{
			// Token: 0x0400044E RID: 1102
			[Nullable(1)]
			[MarshalAs(20)]
			public string Name;

			// Token: 0x0400044F RID: 1103
			public int Value;
		}

		// Token: 0x0200014F RID: 335
		public enum FontWeights
		{
			// Token: 0x04000451 RID: 1105
			Invisible,
			// Token: 0x04000452 RID: 1106
			Thin = 100,
			// Token: 0x04000453 RID: 1107
			ExtraLight = 200,
			// Token: 0x04000454 RID: 1108
			Light = 300,
			// Token: 0x04000455 RID: 1109
			Normal = 400,
			// Token: 0x04000456 RID: 1110
			Medium = 500,
			// Token: 0x04000457 RID: 1111
			SemiBold = 600,
			// Token: 0x04000458 RID: 1112
			Bold = 700,
			// Token: 0x04000459 RID: 1113
			ExtraBold = 800,
			// Token: 0x0400045A RID: 1114
			Black = 900,
			// Token: 0x0400045B RID: 1115
			ExtraBlack = 1000
		}

		// Token: 0x02000150 RID: 336
		[Flags]
		public enum TextDecoration
		{
			// Token: 0x0400045D RID: 1117
			NoDecoration = 0,
			// Token: 0x0400045E RID: 1118
			Underline = 1,
			// Token: 0x0400045F RID: 1119
			Overline = 2,
			// Token: 0x04000460 RID: 1120
			LineThrough = 4
		}

		// Token: 0x02000151 RID: 337
		public enum TextDecorationMode
		{
			// Token: 0x04000462 RID: 1122
			Gaps,
			// Token: 0x04000463 RID: 1123
			Through
		}

		// Token: 0x02000152 RID: 338
		public enum TextDecorationStyle
		{
			// Token: 0x04000465 RID: 1125
			Solid,
			// Token: 0x04000466 RID: 1126
			Double,
			// Token: 0x04000467 RID: 1127
			Dotted,
			// Token: 0x04000468 RID: 1128
			Dashed,
			// Token: 0x04000469 RID: 1129
			Wavy
		}
	}
}
