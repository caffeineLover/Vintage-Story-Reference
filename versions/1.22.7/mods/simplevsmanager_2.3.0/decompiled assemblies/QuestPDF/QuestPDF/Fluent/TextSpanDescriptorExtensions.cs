using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	public static class TextSpanDescriptorExtensions
	{
		// Token: 0x06000504 RID: 1284 RVA: 0x0000CD34 File Offset: 0x0000AF34
		public static T Style<[Nullable(0)] T>(this T descriptor, TextStyle style) where T : TextSpanDescriptor
		{
			if (style == null)
			{
				return descriptor;
			}
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<0>__OverrideStyle) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<0>__OverrideStyle = new Func<TextStyle, TextStyle, TextStyle>(TextStyleManager.OverrideStyle));
			}
			textSpanDescriptor.MutateTextStyle<TextStyle>(handler, style);
			return descriptor;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000CD69 File Offset: 0x0000AF69
		[Obsolete("This setting is not supported since the 2024.3 version. Please use the FontFamilyFallback method or rely on the new automated fallback mechanism.")]
		[ExcludeFromCodeCoverage]
		public static T Fallback<[Nullable(0)] T>(this T descriptor, [Nullable(2)] TextStyle value = null) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<1>__Fallback) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<1>__Fallback = new Func<TextStyle, TextStyle, TextStyle>(TextStyleExtensions.Fallback));
			}
			textSpanDescriptor.MutateTextStyle<TextStyle>(handler, value);
			return descriptor;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000CD93 File Offset: 0x0000AF93
		[Obsolete("This setting is not supported since the 2024.3 version. Please use the FontFamilyFallback method or rely on the new automated fallback mechanism.")]
		[ExcludeFromCodeCoverage]
		public static T Fallback<[Nullable(0)] T>(this T descriptor, Func<TextStyle, TextStyle> handler) where T : TextSpanDescriptor
		{
			return descriptor.Fallback(handler.Invoke(TextStyle.Default));
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000CDA6 File Offset: 0x0000AFA6
		public static T FontColor<[Nullable(0)] T>(this T descriptor, Color color) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, Color, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<2>__FontColor) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<2>__FontColor = new Func<TextStyle, Color, TextStyle>(TextStyleExtensions.FontColor));
			}
			textSpanDescriptor.MutateTextStyle<Color>(handler, color);
			return descriptor;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000CDD0 File Offset: 0x0000AFD0
		public static T BackgroundColor<[Nullable(0)] T>(this T descriptor, Color color) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, Color, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<3>__BackgroundColor) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<3>__BackgroundColor = new Func<TextStyle, Color, TextStyle>(TextStyleExtensions.BackgroundColor));
			}
			textSpanDescriptor.MutateTextStyle<Color>(handler, color);
			return descriptor;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000CDFA File Offset: 0x0000AFFA
		public static T FontFamily<[Nullable(0)] T>(this T descriptor, params string[] values) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, string[], TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<4>__FontFamily) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<4>__FontFamily = new Func<TextStyle, string[], TextStyle>(TextStyleExtensions.FontFamily));
			}
			textSpanDescriptor.MutateTextStyle<string[]>(handler, values);
			return descriptor;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000CE24 File Offset: 0x0000B024
		public static T FontSize<[Nullable(0)] T>(this T descriptor, float value) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, float, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<5>__FontSize) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<5>__FontSize = new Func<TextStyle, float, TextStyle>(TextStyleExtensions.FontSize));
			}
			textSpanDescriptor.MutateTextStyle<float>(handler, value);
			return descriptor;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000CE4E File Offset: 0x0000B04E
		public static T LineHeight<[Nullable(0)] T>(this T descriptor, float? factor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, float?, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<6>__LineHeight) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<6>__LineHeight = new Func<TextStyle, float?, TextStyle>(TextStyleExtensions.LineHeight));
			}
			textSpanDescriptor.MutateTextStyle<float?>(handler, factor);
			return descriptor;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000CE78 File Offset: 0x0000B078
		public static T LetterSpacing<[Nullable(0)] T>(this T descriptor, float factor = 0f) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, float, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<7>__LetterSpacing) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<7>__LetterSpacing = new Func<TextStyle, float, TextStyle>(TextStyleExtensions.LetterSpacing));
			}
			textSpanDescriptor.MutateTextStyle<float>(handler, factor);
			return descriptor;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000CEA2 File Offset: 0x0000B0A2
		public static T WordSpacing<[Nullable(0)] T>(this T descriptor, float factor = 0f) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, float, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<8>__WordSpacing) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<8>__WordSpacing = new Func<TextStyle, float, TextStyle>(TextStyleExtensions.WordSpacing));
			}
			textSpanDescriptor.MutateTextStyle<float>(handler, factor);
			return descriptor;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000CECC File Offset: 0x0000B0CC
		public static T Italic<[Nullable(0)] T>(this T descriptor, bool value = true) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, bool, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<9>__Italic) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<9>__Italic = new Func<TextStyle, bool, TextStyle>(TextStyleExtensions.Italic));
			}
			textSpanDescriptor.MutateTextStyle<bool>(handler, value);
			return descriptor;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000CEF6 File Offset: 0x0000B0F6
		[Obsolete("This setting is not supported since the 2024.3 version. This flag should be handled automatically by the layout engine.")]
		[ExcludeFromCodeCoverage]
		public static T WrapAnywhere<[Nullable(0)] T>(this T descriptor, bool value = true) where T : TextSpanDescriptor
		{
			return descriptor;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000CEF9 File Offset: 0x0000B0F9
		public static T Strikethrough<[Nullable(0)] T>(this T descriptor, bool value = true) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, bool, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<10>__Strikethrough) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<10>__Strikethrough = new Func<TextStyle, bool, TextStyle>(TextStyleExtensions.Strikethrough));
			}
			textSpanDescriptor.MutateTextStyle<bool>(handler, value);
			return descriptor;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000CF23 File Offset: 0x0000B123
		public static T Underline<[Nullable(0)] T>(this T descriptor, bool value = true) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, bool, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<11>__Underline) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<11>__Underline = new Func<TextStyle, bool, TextStyle>(TextStyleExtensions.Underline));
			}
			textSpanDescriptor.MutateTextStyle<bool>(handler, value);
			return descriptor;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000CF4D File Offset: 0x0000B14D
		public static T Overline<[Nullable(0)] T>(this T descriptor, bool value = true) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, bool, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<12>__Overline) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<12>__Overline = new Func<TextStyle, bool, TextStyle>(TextStyleExtensions.Overline));
			}
			textSpanDescriptor.MutateTextStyle<bool>(handler, value);
			return descriptor;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000CF77 File Offset: 0x0000B177
		public static T DecorationColor<[Nullable(0)] T>(this T descriptor, Color color) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, Color, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<13>__DecorationColor) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<13>__DecorationColor = new Func<TextStyle, Color, TextStyle>(TextStyleExtensions.DecorationColor));
			}
			textSpanDescriptor.MutateTextStyle<Color>(handler, color);
			return descriptor;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000CFA1 File Offset: 0x0000B1A1
		public static T DecorationThickness<[Nullable(0)] T>(this T descriptor, float factor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, float, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<14>__DecorationThickness) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<14>__DecorationThickness = new Func<TextStyle, float, TextStyle>(TextStyleExtensions.DecorationThickness));
			}
			textSpanDescriptor.MutateTextStyle<float>(handler, factor);
			return descriptor;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000CFCB File Offset: 0x0000B1CB
		public static T DecorationSolid<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<15>__DecorationSolid) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<15>__DecorationSolid = new Func<TextStyle, TextStyle>(TextStyleExtensions.DecorationSolid));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000CFF4 File Offset: 0x0000B1F4
		public static T DecorationDouble<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<16>__DecorationDouble) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<16>__DecorationDouble = new Func<TextStyle, TextStyle>(TextStyleExtensions.DecorationDouble));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000D01D File Offset: 0x0000B21D
		public static T DecorationWavy<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<17>__DecorationWavy) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<17>__DecorationWavy = new Func<TextStyle, TextStyle>(TextStyleExtensions.DecorationWavy));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000D046 File Offset: 0x0000B246
		public static T DecorationDotted<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<18>__DecorationDotted) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<18>__DecorationDotted = new Func<TextStyle, TextStyle>(TextStyleExtensions.DecorationDotted));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000D06F File Offset: 0x0000B26F
		public static T DecorationDashed<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<19>__DecorationDashed) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<19>__DecorationDashed = new Func<TextStyle, TextStyle>(TextStyleExtensions.DecorationDashed));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000D098 File Offset: 0x0000B298
		public static T Thin<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<20>__Thin) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<20>__Thin = new Func<TextStyle, TextStyle>(TextStyleExtensions.Thin));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000D0C1 File Offset: 0x0000B2C1
		public static T ExtraLight<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<21>__ExtraLight) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<21>__ExtraLight = new Func<TextStyle, TextStyle>(TextStyleExtensions.ExtraLight));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000D0EA File Offset: 0x0000B2EA
		public static T Light<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<22>__Light) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<22>__Light = new Func<TextStyle, TextStyle>(TextStyleExtensions.Light));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000D113 File Offset: 0x0000B313
		public static T NormalWeight<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<23>__NormalWeight) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<23>__NormalWeight = new Func<TextStyle, TextStyle>(TextStyleExtensions.NormalWeight));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000D13C File Offset: 0x0000B33C
		public static T Medium<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<24>__Medium) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<24>__Medium = new Func<TextStyle, TextStyle>(TextStyleExtensions.Medium));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000D165 File Offset: 0x0000B365
		public static T SemiBold<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<25>__SemiBold) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<25>__SemiBold = new Func<TextStyle, TextStyle>(TextStyleExtensions.SemiBold));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000D18E File Offset: 0x0000B38E
		public static T Bold<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<26>__Bold) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<26>__Bold = new Func<TextStyle, TextStyle>(TextStyleExtensions.Bold));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000D1B7 File Offset: 0x0000B3B7
		public static T ExtraBold<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<27>__ExtraBold) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<27>__ExtraBold = new Func<TextStyle, TextStyle>(TextStyleExtensions.ExtraBold));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		public static T Black<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<28>__Black) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<28>__Black = new Func<TextStyle, TextStyle>(TextStyleExtensions.Black));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000D209 File Offset: 0x0000B409
		public static T ExtraBlack<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<29>__ExtraBlack) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<29>__ExtraBlack = new Func<TextStyle, TextStyle>(TextStyleExtensions.ExtraBlack));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000D232 File Offset: 0x0000B432
		public static T NormalPosition<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<30>__NormalPosition) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<30>__NormalPosition = new Func<TextStyle, TextStyle>(TextStyleExtensions.NormalPosition));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000D25B File Offset: 0x0000B45B
		public static T Subscript<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<31>__Subscript) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<31>__Subscript = new Func<TextStyle, TextStyle>(TextStyleExtensions.Subscript));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000D284 File Offset: 0x0000B484
		public static T Superscript<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<32>__Superscript) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<32>__Superscript = new Func<TextStyle, TextStyle>(TextStyleExtensions.Superscript));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000D2AD File Offset: 0x0000B4AD
		public static T DirectionAuto<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<33>__DirectionAuto) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<33>__DirectionAuto = new Func<TextStyle, TextStyle>(TextStyleExtensions.DirectionAuto));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000D2D6 File Offset: 0x0000B4D6
		public static T DirectionFromLeftToRight<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<34>__DirectionFromLeftToRight) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<34>__DirectionFromLeftToRight = new Func<TextStyle, TextStyle>(TextStyleExtensions.DirectionFromLeftToRight));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000D2FF File Offset: 0x0000B4FF
		public static T DirectionFromRightToLeft<[Nullable(0)] T>(this T descriptor) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<35>__DirectionFromRightToLeft) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<35>__DirectionFromRightToLeft = new Func<TextStyle, TextStyle>(TextStyleExtensions.DirectionFromRightToLeft));
			}
			textSpanDescriptor.MutateTextStyle(handler);
			return descriptor;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000D328 File Offset: 0x0000B528
		public static T EnableFontFeature<[Nullable(0)] T>(this T descriptor, string featureName) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, string, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<36>__EnableFontFeature) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<36>__EnableFontFeature = new Func<TextStyle, string, TextStyle>(TextStyleExtensions.EnableFontFeature));
			}
			textSpanDescriptor.MutateTextStyle<string>(handler, featureName);
			return descriptor;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000D352 File Offset: 0x0000B552
		public static T DisableFontFeature<[Nullable(0)] T>(this T descriptor, string featureName) where T : TextSpanDescriptor
		{
			TextSpanDescriptor textSpanDescriptor = descriptor;
			Func<TextStyle, string, TextStyle> handler;
			if ((handler = TextSpanDescriptorExtensions.<>O.<37>__DisableFontFeature) == null)
			{
				handler = (TextSpanDescriptorExtensions.<>O.<37>__DisableFontFeature = new Func<TextStyle, string, TextStyle>(TextStyleExtensions.DisableFontFeature));
			}
			textSpanDescriptor.MutateTextStyle<string>(handler, featureName);
			return descriptor;
		}

		// Token: 0x020001A0 RID: 416
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000604 RID: 1540
			[Nullable(new byte[]
			{
				0,
				1,
				0,
				1
			})]
			public static Func<TextStyle, TextStyle, TextStyle> <0>__OverrideStyle;

			// Token: 0x04000605 RID: 1541
			[Nullable(new byte[]
			{
				0,
				1,
				0,
				1
			})]
			public static Func<TextStyle, TextStyle, TextStyle> <1>__Fallback;

			// Token: 0x04000606 RID: 1542
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, Color, TextStyle> <2>__FontColor;

			// Token: 0x04000607 RID: 1543
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, Color, TextStyle> <3>__BackgroundColor;

			// Token: 0x04000608 RID: 1544
			[Nullable(new byte[]
			{
				0,
				1,
				0,
				0,
				1
			})]
			public static Func<TextStyle, string[], TextStyle> <4>__FontFamily;

			// Token: 0x04000609 RID: 1545
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, float, TextStyle> <5>__FontSize;

			// Token: 0x0400060A RID: 1546
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, float?, TextStyle> <6>__LineHeight;

			// Token: 0x0400060B RID: 1547
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, float, TextStyle> <7>__LetterSpacing;

			// Token: 0x0400060C RID: 1548
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, float, TextStyle> <8>__WordSpacing;

			// Token: 0x0400060D RID: 1549
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, bool, TextStyle> <9>__Italic;

			// Token: 0x0400060E RID: 1550
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, bool, TextStyle> <10>__Strikethrough;

			// Token: 0x0400060F RID: 1551
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, bool, TextStyle> <11>__Underline;

			// Token: 0x04000610 RID: 1552
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, bool, TextStyle> <12>__Overline;

			// Token: 0x04000611 RID: 1553
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, Color, TextStyle> <13>__DecorationColor;

			// Token: 0x04000612 RID: 1554
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, float, TextStyle> <14>__DecorationThickness;

			// Token: 0x04000613 RID: 1555
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <15>__DecorationSolid;

			// Token: 0x04000614 RID: 1556
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <16>__DecorationDouble;

			// Token: 0x04000615 RID: 1557
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <17>__DecorationWavy;

			// Token: 0x04000616 RID: 1558
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <18>__DecorationDotted;

			// Token: 0x04000617 RID: 1559
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <19>__DecorationDashed;

			// Token: 0x04000618 RID: 1560
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <20>__Thin;

			// Token: 0x04000619 RID: 1561
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <21>__ExtraLight;

			// Token: 0x0400061A RID: 1562
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <22>__Light;

			// Token: 0x0400061B RID: 1563
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <23>__NormalWeight;

			// Token: 0x0400061C RID: 1564
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <24>__Medium;

			// Token: 0x0400061D RID: 1565
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <25>__SemiBold;

			// Token: 0x0400061E RID: 1566
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <26>__Bold;

			// Token: 0x0400061F RID: 1567
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <27>__ExtraBold;

			// Token: 0x04000620 RID: 1568
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <28>__Black;

			// Token: 0x04000621 RID: 1569
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <29>__ExtraBlack;

			// Token: 0x04000622 RID: 1570
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <30>__NormalPosition;

			// Token: 0x04000623 RID: 1571
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <31>__Subscript;

			// Token: 0x04000624 RID: 1572
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <32>__Superscript;

			// Token: 0x04000625 RID: 1573
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <33>__DirectionAuto;

			// Token: 0x04000626 RID: 1574
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <34>__DirectionFromLeftToRight;

			// Token: 0x04000627 RID: 1575
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<TextStyle, TextStyle> <35>__DirectionFromRightToLeft;

			// Token: 0x04000628 RID: 1576
			[Nullable(new byte[]
			{
				0,
				1,
				0,
				1
			})]
			public static Func<TextStyle, string, TextStyle> <36>__EnableFontFeature;

			// Token: 0x04000629 RID: 1577
			[Nullable(new byte[]
			{
				0,
				1,
				0,
				1
			})]
			public static Func<TextStyle, string, TextStyle> <37>__DisableFontFeature;
		}
	}
}
