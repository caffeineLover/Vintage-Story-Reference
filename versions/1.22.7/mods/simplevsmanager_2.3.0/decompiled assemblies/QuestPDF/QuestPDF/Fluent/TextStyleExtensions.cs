using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;
using QuestPDF.Skia.Text;

namespace QuestPDF.Fluent
{
	// Token: 0x020000B1 RID: 177
	[NullableContext(1)]
	[Nullable(0)]
	public static class TextStyleExtensions
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x0000D37C File Offset: 0x0000B57C
		[Obsolete("This element has been renamed since version 2022.3. Please use the FontColor method.")]
		[ExcludeFromCodeCoverage]
		public static TextStyle Color(this TextStyle style, string value)
		{
			return style.FontColor(value);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000D38A File Offset: 0x0000B58A
		public static TextStyle FontColor(this TextStyle style, Color color)
		{
			return style.Mutate(TextStyleProperty.Color, color).Mutate(TextStyleProperty.DecorationColor, color);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000D3A5 File Offset: 0x0000B5A5
		public static TextStyle BackgroundColor(this TextStyle style, Color color)
		{
			return style.Mutate(TextStyleProperty.BackgroundColor, color);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		[Obsolete("This element has been renamed since version 2022.3. Please use the FontFamily method.")]
		[ExcludeFromCodeCoverage]
		public static TextStyle FontType(this TextStyle style, string value)
		{
			return style.FontFamily(new string[]
			{
				value
			});
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000D3C6 File Offset: 0x0000B5C6
		public static TextStyle FontFamily(this TextStyle style, params string[] values)
		{
			if (values == null || values.Length == 0)
			{
				return style;
			}
			return style.Mutate(TextStyleProperty.FontFamilies, values);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000D3D9 File Offset: 0x0000B5D9
		[Obsolete("This element has been renamed since version 2022.3. Please use the FontSize method.")]
		[ExcludeFromCodeCoverage]
		public static TextStyle Size(this TextStyle style, float value)
		{
			return style.FontSize(value);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000D3E2 File Offset: 0x0000B5E2
		public static TextStyle FontSize(this TextStyle style, float value)
		{
			if (value <= 0f)
			{
				throw new ArgumentException("Font size must be greater than 0.");
			}
			return style.Mutate(TextStyleProperty.Size, value);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000D404 File Offset: 0x0000B604
		public static TextStyle LineHeight(this TextStyle style, float? factor)
		{
			float num = factor.GetValueOrDefault();
			if (factor == null)
			{
				num = 0f;
				factor = new float?(num);
			}
			float? num2 = factor;
			num = 0f;
			if (num2.GetValueOrDefault() < num & num2 != null)
			{
				throw new ArgumentException("Line height must be greater than 0.");
			}
			return style.Mutate(TextStyleProperty.LineHeight, factor);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000D464 File Offset: 0x0000B664
		public static TextStyle LetterSpacing(this TextStyle style, float factor = 0f)
		{
			return style.Mutate(TextStyleProperty.LetterSpacing, factor);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000D473 File Offset: 0x0000B673
		public static TextStyle WordSpacing(this TextStyle style, float factor = 0f)
		{
			return style.Mutate(TextStyleProperty.WordSpacing, factor);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000D482 File Offset: 0x0000B682
		public static TextStyle Italic(this TextStyle style, bool value = true)
		{
			return style.Mutate(TextStyleProperty.IsItalic, value);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000D492 File Offset: 0x0000B692
		[Obsolete("This setting is not supported since the 2024.3 version. This flag should be handled automatically by the layout engine.")]
		[ExcludeFromCodeCoverage]
		public static TextStyle WrapAnywhere(this TextStyle style, bool value = true)
		{
			return style;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000D495 File Offset: 0x0000B695
		public static TextStyle Strikethrough(this TextStyle style, bool value = true)
		{
			return style.Mutate(TextStyleProperty.HasStrikethrough, value);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000D4A5 File Offset: 0x0000B6A5
		public static TextStyle Underline(this TextStyle style, bool value = true)
		{
			return style.Mutate(TextStyleProperty.HasUnderline, value);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000D4B5 File Offset: 0x0000B6B5
		public static TextStyle Overline(this TextStyle style, bool value = true)
		{
			return style.Mutate(TextStyleProperty.HasOverline, value);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000D4C5 File Offset: 0x0000B6C5
		public static TextStyle DecorationColor(this TextStyle style, Color color)
		{
			return style.Mutate(TextStyleProperty.DecorationColor, color);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		public static TextStyle DecorationThickness(this TextStyle style, float factor)
		{
			return style.Mutate(TextStyleProperty.DecorationThickness, factor);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
		public static TextStyle DecorationSolid(this TextStyle style)
		{
			return style.Mutate(TextStyleProperty.DecorationStyle, TextStyleConfiguration.TextDecorationStyle.Solid);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public static TextStyle DecorationDouble(this TextStyle style)
		{
			return style.Mutate(TextStyleProperty.DecorationStyle, TextStyleConfiguration.TextDecorationStyle.Double);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000D504 File Offset: 0x0000B704
		public static TextStyle DecorationWavy(this TextStyle style)
		{
			return style.Mutate(TextStyleProperty.DecorationStyle, TextStyleConfiguration.TextDecorationStyle.Wavy);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000D514 File Offset: 0x0000B714
		public static TextStyle DecorationDotted(this TextStyle style)
		{
			return style.Mutate(TextStyleProperty.DecorationStyle, TextStyleConfiguration.TextDecorationStyle.Dotted);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000D524 File Offset: 0x0000B724
		public static TextStyle DecorationDashed(this TextStyle style)
		{
			return style.Mutate(TextStyleProperty.DecorationStyle, TextStyleConfiguration.TextDecorationStyle.Dashed);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000D534 File Offset: 0x0000B734
		public static TextStyle Weight(this TextStyle style, FontWeight weight)
		{
			return style.Mutate(TextStyleProperty.FontWeight, weight);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000D544 File Offset: 0x0000B744
		public static TextStyle Thin(this TextStyle style)
		{
			return style.Weight(FontWeight.Thin);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000D54E File Offset: 0x0000B74E
		public static TextStyle ExtraLight(this TextStyle style)
		{
			return style.Weight(FontWeight.ExtraLight);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000D55B File Offset: 0x0000B75B
		public static TextStyle Light(this TextStyle style)
		{
			return style.Weight(FontWeight.Light);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000D568 File Offset: 0x0000B768
		public static TextStyle NormalWeight(this TextStyle style)
		{
			return style.Weight(FontWeight.Normal);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000D575 File Offset: 0x0000B775
		public static TextStyle Medium(this TextStyle style)
		{
			return style.Weight(FontWeight.Medium);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000D582 File Offset: 0x0000B782
		public static TextStyle SemiBold(this TextStyle style)
		{
			return style.Weight(FontWeight.SemiBold);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000D58F File Offset: 0x0000B78F
		public static TextStyle Bold(this TextStyle style)
		{
			return style.Weight(FontWeight.Bold);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000D59C File Offset: 0x0000B79C
		public static TextStyle ExtraBold(this TextStyle style)
		{
			return style.Weight(FontWeight.ExtraBold);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000D5A9 File Offset: 0x0000B7A9
		public static TextStyle Black(this TextStyle style)
		{
			return style.Weight(FontWeight.Black);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000D5B6 File Offset: 0x0000B7B6
		public static TextStyle ExtraBlack(this TextStyle style)
		{
			return style.Weight(FontWeight.ExtraBlack);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000D5C3 File Offset: 0x0000B7C3
		public static TextStyle NormalPosition(this TextStyle style)
		{
			return style.Position(FontPosition.Normal);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		public static TextStyle Subscript(this TextStyle style)
		{
			return style.Position(FontPosition.Subscript);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000D5D5 File Offset: 0x0000B7D5
		public static TextStyle Superscript(this TextStyle style)
		{
			return style.Position(FontPosition.Superscript);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000D5DE File Offset: 0x0000B7DE
		private static TextStyle Position(this TextStyle style, FontPosition fontPosition)
		{
			return style.Mutate(TextStyleProperty.FontPosition, fontPosition);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0000D5F0 File Offset: 0x0000B7F0
		[Obsolete("This setting is not supported since the 2024.3 version. Please use the FontFamilyFallback method or rely on the new automated fallback mechanism.")]
		[ExcludeFromCodeCoverage]
		public static TextStyle Fallback(this TextStyle style, [Nullable(2)] TextStyle value = null)
		{
			IEnumerable<string> enumerable = style.FontFamilies ?? Array.Empty<string>();
			string[] array = ((value != null) ? value.FontFamilies : null) ?? Array.Empty<string>();
			string[] values = Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.Concat<string>(enumerable, array), (string x) => !string.IsNullOrWhiteSpace(x))));
			return style.FontFamily(values);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0000D65E File Offset: 0x0000B85E
		[Obsolete("This setting is not supported since the 2024.3 version. Please use the FontFamilyFallback method or rely on the new automated fallback mechanism.")]
		[ExcludeFromCodeCoverage]
		public static TextStyle Fallback(this TextStyle style, Func<TextStyle, TextStyle> handler)
		{
			return style.Fallback(handler.Invoke(TextStyle.Default));
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0000D671 File Offset: 0x0000B871
		private static TextStyle TextDirection(this TextStyle style, TextDirection textDirection)
		{
			return style.Mutate(TextStyleProperty.Direction, textDirection);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000D681 File Offset: 0x0000B881
		public static TextStyle DirectionAuto(this TextStyle style)
		{
			return style.TextDirection(QuestPDF.Infrastructure.TextDirection.Auto);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000D68A File Offset: 0x0000B88A
		public static TextStyle DirectionFromLeftToRight(this TextStyle style)
		{
			return style.TextDirection(QuestPDF.Infrastructure.TextDirection.LeftToRight);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000D693 File Offset: 0x0000B893
		public static TextStyle DirectionFromRightToLeft(this TextStyle style)
		{
			return style.TextDirection(QuestPDF.Infrastructure.TextDirection.RightToLeft);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000D69C File Offset: 0x0000B89C
		public static TextStyle EnableFontFeature(this TextStyle style, string featureName)
		{
			return style.Mutate(TextStyleProperty.FontFeatures, new ValueTuple<string, bool>[]
			{
				new ValueTuple<string, bool>(featureName, true)
			});
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000D6B9 File Offset: 0x0000B8B9
		public static TextStyle DisableFontFeature(this TextStyle style, string featureName)
		{
			return style.Mutate(TextStyleProperty.FontFeatures, new ValueTuple<string, bool>[]
			{
				new ValueTuple<string, bool>(featureName, false)
			});
		}
	}
}
