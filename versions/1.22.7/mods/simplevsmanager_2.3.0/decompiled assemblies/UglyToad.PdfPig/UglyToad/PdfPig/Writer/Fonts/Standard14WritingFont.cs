using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer.Fonts
{
	// Token: 0x02000020 RID: 32
	internal class Standard14WritingFont : IWritingFont
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00009524 File Offset: 0x00007724
		public bool HasWidths { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000952C File Offset: 0x0000772C
		public string Name
		{
			get
			{
				return this.metrics.FontName;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00009539 File Offset: 0x00007739
		public Standard14WritingFont(AdobeFontMetrics metrics)
		{
			this.metrics = metrics;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00009548 File Offset: 0x00007748
		public bool TryGetBoundingBox(char character, out PdfRectangle boundingBox)
		{
			boundingBox = default(PdfRectangle);
			int code = this.CodeMapIfUnicode(character);
			if (code == -1)
			{
				return false;
			}
			AdobeFontMetricsIndividualCharacterMetric adobeFontMetricsIndividualCharacterMetric = Enumerable.FirstOrDefault<AdobeFontMetricsIndividualCharacterMetric>(Enumerable.Select<KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric>, AdobeFontMetricsIndividualCharacterMetric>(Enumerable.Where<KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric>>(this.metrics.CharacterMetrics, (KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric> v) => v.Value.CharacterCode == code), (KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric> v) => v.Value));
			if (adobeFontMetricsIndividualCharacterMetric == null)
			{
				return false;
			}
			boundingBox = new PdfRectangle(adobeFontMetricsIndividualCharacterMetric.BoundingBox.Left, adobeFontMetricsIndividualCharacterMetric.BoundingBox.Bottom, adobeFontMetricsIndividualCharacterMetric.BoundingBox.Left + adobeFontMetricsIndividualCharacterMetric.Width.X, adobeFontMetricsIndividualCharacterMetric.BoundingBox.Top);
			return true;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00009618 File Offset: 0x00007818
		public bool TryGetAdvanceWidth(char character, out double width)
		{
			width = 0.0;
			PdfRectangle pdfRectangle;
			if (!this.TryGetBoundingBox(character, out pdfRectangle))
			{
				return false;
			}
			width = pdfRectangle.Width;
			return true;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00009647 File Offset: 0x00007847
		public TransformationMatrix GetFontMatrix()
		{
			return TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00009684 File Offset: 0x00007884
		public IndirectReferenceToken WriteFont(IPdfStreamWriter writer, IndirectReferenceToken reservedIndirect = null)
		{
			NameToken nameToken = NameToken.StandardEncoding;
			if (string.Equals(this.metrics.FontName, "Symbol", 5) || string.Equals(this.metrics.FontName, "ZapfDingbats", 5))
			{
				nameToken = NameToken.Create("FontSpecific");
			}
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Type, NameToken.Font);
			dictionary.Add(NameToken.Subtype, NameToken.Type1);
			dictionary.Add(NameToken.BaseFont, NameToken.Create(this.metrics.FontName));
			dictionary.Add(NameToken.Encoding, nameToken);
			DictionaryToken token = new DictionaryToken(dictionary);
			if (reservedIndirect != null)
			{
				return writer.WriteToken(token, reservedIndirect);
			}
			return writer.WriteToken(token);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009738 File Offset: 0x00007938
		public byte GetValueForCharacter(char character)
		{
			int characterCode = this.CodeMapIfUnicode(character);
			AdobeFontMetricsIndividualCharacterMetric adobeFontMetricsIndividualCharacterMetric = Enumerable.FirstOrDefault<AdobeFontMetricsIndividualCharacterMetric>(Enumerable.Select<KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric>, AdobeFontMetricsIndividualCharacterMetric>(Enumerable.Where<KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric>>(this.metrics.CharacterMetrics, (KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric> v) => v.Value.CharacterCode == characterCode), (KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric> v) => v.Value));
			if (adobeFontMetricsIndividualCharacterMetric == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(40, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Font '");
				defaultInterpolatedStringHandler.AppendFormatted(this.metrics.FontName);
				defaultInterpolatedStringHandler.AppendLiteral("' does NOT have character '");
				defaultInterpolatedStringHandler.AppendFormatted<char>(character);
				defaultInterpolatedStringHandler.AppendLiteral("' (0x");
				defaultInterpolatedStringHandler.AppendFormatted<int>((int)character, "X");
				defaultInterpolatedStringHandler.AppendLiteral(").");
				throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return (byte)adobeFontMetricsIndividualCharacterMetric.CharacterCode;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009814 File Offset: 0x00007A14
		private int UnicodeToSymbolCode(char character)
		{
			string text = GlyphList.AdobeGlyphList.UnicodeCodePointToName((int)character);
			if (string.Equals(text, ".notdef", 5))
			{
				return -1;
			}
			return SymbolEncoding.Instance.GetCode(text);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000984C File Offset: 0x00007A4C
		private int UnicodeToZapfDingbats(char character)
		{
			string text = GlyphList.ZapfDingbats.UnicodeCodePointToName((int)character);
			if (string.Equals(text, ".notdef", 5))
			{
				return -1;
			}
			return ZapfDingbatsEncoding.Instance.GetCode(text);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009884 File Offset: 0x00007A84
		private int UnicodeToStandardEncoding(char character)
		{
			string text = GlyphList.AdobeGlyphList.UnicodeCodePointToName((int)character);
			if (string.Equals(text, ".notdef", 5))
			{
				return -1;
			}
			StandardEncoding instance = StandardEncoding.Instance;
			int code = instance.GetCode(text);
			if (code == -1)
			{
				string text2 = char.IsUpper(text.get_Chars(0)) ? (char.ToLower(text.get_Chars(0)).ToString() + text.Substring(1)) : (char.ToUpper(text.get_Chars(0)).ToString() + text.Substring(1));
				code = instance.GetCode(text2);
			}
			return code;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00009920 File Offset: 0x00007B20
		private int CodeMapIfUnicode(char character)
		{
			int i = (int)character;
			int result;
			if (string.Equals(this.metrics.FontName, "ZapfDingbats", 5))
			{
				result = ((i < 255) ? i : this.UnicodeToZapfDingbats(character));
			}
			else if (string.Equals(this.metrics.FontName, "Symbol", 5))
			{
				if (i == 172)
				{
					return 216;
				}
				if (i == 247)
				{
					return 184;
				}
				if (i == 181)
				{
					return 109;
				}
				if (i == 215)
				{
					return 180;
				}
				result = ((i < 255) ? i : this.UnicodeToSymbolCode(character));
			}
			else
			{
				if (i == 198)
				{
					return 225;
				}
				if (i == 180)
				{
					return 194;
				}
				if (i == 183)
				{
					return 180;
				}
				if (i == 184)
				{
					return 203;
				}
				if (i == 164)
				{
					return 168;
				}
				if (i == 168)
				{
					return 200;
				}
				if (i == 96)
				{
					return 193;
				}
				if (i == 175)
				{
					return 197;
				}
				if (i == 170)
				{
					return 227;
				}
				if (i == 186)
				{
					return 235;
				}
				if (i == 248)
				{
					return 249;
				}
				if (i == 39)
				{
					return 169;
				}
				result = (Enumerable.Any<KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric>>(this.metrics.CharacterMetrics, (KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric> v) => v.Value.CharacterCode == i) ? i : this.UnicodeToStandardEncoding(character));
			}
			return result;
		}

		// Token: 0x04000097 RID: 151
		private readonly AdobeFontMetrics metrics;
	}
}
