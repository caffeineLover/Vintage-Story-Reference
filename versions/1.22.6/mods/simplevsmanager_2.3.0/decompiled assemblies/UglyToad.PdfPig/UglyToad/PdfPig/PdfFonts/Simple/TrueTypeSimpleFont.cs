using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Composite;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.Simple
{
	// Token: 0x02000096 RID: 150
	internal class TrueTypeSimpleFont : IFont
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0001028E File Offset: 0x0000E48E
		public NameToken Name { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00010296 File Offset: 0x0000E496
		public bool IsVertical { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0001029E File Offset: 0x0000E49E
		public FontDetails Details { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000270 RID: 624 RVA: 0x000102A6 File Offset: 0x0000E4A6
		// (set) Token: 0x06000271 RID: 625 RVA: 0x000102AE File Offset: 0x0000E4AE
		[NotNull]
		public ToUnicodeCMap ToUnicode { get; set; }

		// Token: 0x06000272 RID: 626 RVA: 0x000102B8 File Offset: 0x0000E4B8
		public TrueTypeSimpleFont(NameToken name, FontDescriptor descriptor, [CanBeNull] CMap toUnicodeCMap, [CanBeNull] Encoding encoding, [CanBeNull] TrueTypeFont font, int firstCharacter, double[] widths)
		{
			this.descriptor = descriptor;
			this.encoding = encoding;
			this.font = font;
			this.firstCharacter = firstCharacter;
			this.widths = widths;
			this.Name = name;
			this.IsVertical = 0;
			this.ToUnicode = new ToUnicodeCMap(toUnicodeCMap);
			FontDetails fontDetails;
			if (descriptor == null)
			{
				fontDetails = null;
			}
			else
			{
				NameToken name2 = this.Name;
				fontDetails = descriptor.ToDetails((name2 != null) ? name2.Data : null);
			}
			FontDetails fontDetails2;
			if ((fontDetails2 = fontDetails) == null)
			{
				NameToken name3 = this.Name;
				fontDetails2 = FontDetails.GetDefault((name3 != null) ? name3.Data : null);
			}
			this.Details = fontDetails2;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00010361 File Offset: 0x0000E561
		public int ReadCharacterCode(IInputBytes bytes, out int codeLength)
		{
			codeLength = 1;
			return (int)bytes.CurrentByte;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001036C File Offset: 0x0000E56C
		public bool TryGetUnicode(int characterCode, out string value)
		{
			value = null;
			if (this.unicodeValuesCache.TryGetValue(characterCode, ref value))
			{
				return true;
			}
			if (this.ToUnicode.CanMapToUnicode && this.ToUnicode.TryGet(characterCode, out value))
			{
				this.unicodeValuesCache[characterCode] = value;
				return true;
			}
			if (this.encoding == null)
			{
				return false;
			}
			string name = this.encoding.GetName(characterCode);
			try
			{
				value = (GlyphList.AdobeGlyphList.NameToUnicode(name) ?? GlyphList.AdditionalGlyphList.NameToUnicode(name));
			}
			catch
			{
				return false;
			}
			if (value != null)
			{
				this.unicodeValuesCache[characterCode] = value;
			}
			return value != null;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0001041C File Offset: 0x0000E61C
		public CharacterBoundingBox GetBoundingBox(int characterCode)
		{
			CharacterBoundingBox result;
			if (this.boundingBoxCache.TryGetValue(characterCode, ref result))
			{
				return result;
			}
			TransformationMatrix fontMatrix = this.GetFontMatrix();
			bool flag;
			PdfRectangle pdfRectangle = this.GetBoundingBoxInGlyphSpace(characterCode, out flag);
			double width = pdfRectangle.Width;
			if (flag)
			{
				pdfRectangle = fontMatrix.Transform(pdfRectangle);
			}
			else
			{
				pdfRectangle = TrueTypeSimpleFont.DefaultTransformation.Transform(pdfRectangle);
			}
			int num = characterCode - this.firstCharacter;
			double num2;
			if (this.widths != null && num >= 0 && num < this.widths.Length)
			{
				flag = false;
				num2 = this.widths[num];
			}
			else if (this.font != null)
			{
				if (!this.font.TryGetAdvanceWidth(characterCode, ref num2))
				{
					num2 = width;
				}
			}
			else
			{
				double[] array = this.widths;
				if (array == null || array.Length == 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(55, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Could not retrieve width for character code: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(characterCode);
					defaultInterpolatedStringHandler.AppendLiteral(" in font ");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				num2 = this.widths[0];
			}
			if (flag)
			{
				num2 = fontMatrix.TransformX(num2);
			}
			else
			{
				num2 = TrueTypeSimpleFont.DefaultTransformation.TransformX(num2);
			}
			CharacterBoundingBox characterBoundingBox = new CharacterBoundingBox(pdfRectangle, num2);
			this.boundingBoxCache[characterCode] = characterBoundingBox;
			return characterBoundingBox;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00010574 File Offset: 0x0000E774
		public TransformationMatrix GetFontMatrix()
		{
			double num = 1000.0;
			TrueTypeFont trueTypeFont = this.font;
			if (((trueTypeFont != null) ? trueTypeFont.TableRegister.HeaderTable : null) != null)
			{
				num = (double)this.font.GetUnitsPerEm();
			}
			return TransformationMatrix.FromValues(1.0 / num, 0.0, 0.0, 1.0 / num, 0.0, 0.0);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000105F0 File Offset: 0x0000E7F0
		private PdfRectangle GetBoundingBoxInGlyphSpace(int characterCode, out bool fromFont)
		{
			fromFont = true;
			if (this.font == null)
			{
				return this.descriptor.BoundingBox;
			}
			PdfRectangle result;
			if (this.font.TryGetBoundingBox(characterCode, new Func<int, int?>(this.CharacterCodeToGlyphId), ref result))
			{
				return result;
			}
			double num;
			if (this.font.TryGetAdvanceWidth(characterCode, ref num))
			{
				return new PdfRectangle(0.0, 0.0, num, 0.0);
			}
			fromFont = false;
			return new PdfRectangle(0.0, 0.0, this.GetWidth(characterCode), 0.0);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00010690 File Offset: 0x0000E890
		private int? CharacterCodeToGlyphId(int characterCode)
		{
			string text;
			string text2;
			if (this.descriptor == null || !this.unicodeValuesCache.TryGetValue(characterCode, ref text) || this.font.TableRegister.CMapTable == null || this.encoding == null || !this.encoding.CodeToNameMap.TryGetValue(characterCode, ref text2) || text2 == null)
			{
				return default(int?);
			}
			if (string.Equals(text2, ".notdef", 5))
			{
				return new int?(0);
			}
			int num = 0;
			if (TrueTypeSimpleFont.<CharacterCodeToGlyphId>g__HasFlag|27_0(this.descriptor.Flags, FontDescriptorFlags.Symbolic) && this.font.WindowsSymbolCMap != null)
			{
				num = this.font.WindowsSymbolCMap.CharacterCodeToGlyphIndex(characterCode);
				if (num == 0 && characterCode >= 0 && characterCode <= 255)
				{
					num = this.font.WindowsSymbolCMap.CharacterCodeToGlyphIndex(characterCode + 61440);
					if (num == 0)
					{
						num = this.font.WindowsSymbolCMap.CharacterCodeToGlyphIndex(characterCode + 61696);
					}
					if (num == 0)
					{
						num = this.font.WindowsSymbolCMap.CharacterCodeToGlyphIndex(characterCode + 61952);
					}
				}
				if (num == 0 && this.font.WindowsUnicodeCMap != null && !string.IsNullOrEmpty(text))
				{
					num = this.font.WindowsUnicodeCMap.CharacterCodeToGlyphIndex((int)text.get_Chars(0));
				}
			}
			else
			{
				if (this.font.WindowsUnicodeCMap != null && !string.IsNullOrEmpty(text))
				{
					num = this.font.WindowsUnicodeCMap.CharacterCodeToGlyphIndex((int)text.get_Chars(0));
				}
				int num2;
				if (num == 0 && this.font.MacRomanCMap != null && MacOsRomanEncoding.Instance.NameToCodeMap.TryGetValue(text2, ref num2))
				{
					num = this.font.MacRomanCMap.CharacterCodeToGlyphIndex(num2);
				}
				if (num == 0 && this.font.TableRegister.PostScriptTable != null)
				{
					for (int i = 0; i < this.font.TableRegister.PostScriptTable.GlyphNames.Count; i++)
					{
						if (string.Equals(this.font.TableRegister.PostScriptTable.GlyphNames[i], text2, 5))
						{
							return new int?(i);
						}
					}
				}
			}
			if (num != 0)
			{
				return new int?(num);
			}
			return default(int?);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000108C0 File Offset: 0x0000EAC0
		private double GetWidth(int characterCode)
		{
			int num = characterCode - this.firstCharacter;
			if (num < 0 || num >= this.widths.Length)
			{
				return (double)this.descriptor.MissingWidth;
			}
			return this.widths[num];
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000108FF File Offset: 0x0000EAFF
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			if (this.font == null)
			{
				path = EmptyArray<PdfSubpath>.Instance;
				return false;
			}
			return this.font.TryGetPath(characterCode, new Func<int, int?>(this.CharacterCodeToGlyphId), ref path);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0001092C File Offset: 0x0000EB2C
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			if (!this.TryGetPath(characterCode, out path))
			{
				return false;
			}
			path = Enumerable.ToList<PdfSubpath>(this.GetFontMatrix().Transform(path));
			return true;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000109AD File Offset: 0x0000EBAD
		[CompilerGenerated]
		internal static bool <CharacterCodeToGlyphId>g__HasFlag|27_0(FontDescriptorFlags value, FontDescriptorFlags target)
		{
			return (value & target) == target;
		}

		// Token: 0x04000149 RID: 329
		private static readonly TransformationMatrix DefaultTransformation = TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);

		// Token: 0x0400014A RID: 330
		private readonly FontDescriptor descriptor;

		// Token: 0x0400014B RID: 331
		private readonly Dictionary<int, CharacterBoundingBox> boundingBoxCache = new Dictionary<int, CharacterBoundingBox>();

		// Token: 0x0400014C RID: 332
		private readonly Dictionary<int, string> unicodeValuesCache = new Dictionary<int, string>();

		// Token: 0x0400014D RID: 333
		[CanBeNull]
		private readonly Encoding encoding;

		// Token: 0x0400014E RID: 334
		[CanBeNull]
		private readonly TrueTypeFont font;

		// Token: 0x0400014F RID: 335
		private readonly int firstCharacter;

		// Token: 0x04000150 RID: 336
		private readonly double[] widths;
	}
}
