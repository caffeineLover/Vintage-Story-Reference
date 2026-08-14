using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Fonts.TrueType.Subsetting;
using UglyToad.PdfPig.Fonts.TrueType.Tables;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer.Fonts
{
	// Token: 0x02000022 RID: 34
	internal class TrueTypeWritingFont : IWritingFont
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00009E38 File Offset: 0x00008038
		public bool HasWidths { get; } = 1;

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00009E40 File Offset: 0x00008040
		public string Name
		{
			get
			{
				return this.font.Name;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00009E4D File Offset: 0x0000804D
		public TrueTypeWritingFont(TrueTypeFont font, IReadOnlyList<byte> fontFileBytes)
		{
			this.font = font;
			this.fontFileBytes = fontFileBytes;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00009E87 File Offset: 0x00008087
		public bool TryGetBoundingBox(char character, out PdfRectangle boundingBox)
		{
			return this.font.TryGetBoundingBox((int)character, ref boundingBox);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00009E96 File Offset: 0x00008096
		public bool TryGetAdvanceWidth(char character, out double width)
		{
			return this.font.TryGetAdvanceWidth((int)character, ref width);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00009EA8 File Offset: 0x000080A8
		public TransformationMatrix GetFontMatrix()
		{
			int unitsPerEm = this.font.GetUnitsPerEm();
			return TransformationMatrix.FromValues(1.0 / (double)unitsPerEm, 0.0, 0.0, 1.0 / (double)unitsPerEm, 0.0, 0.0);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00009F04 File Offset: 0x00008104
		public IndirectReferenceToken WriteFont(IPdfStreamWriter writer, IndirectReferenceToken reservedIndirect = null)
		{
			TrueTypeSubsetEncoding trueTypeSubsetEncoding = new TrueTypeSubsetEncoding(Enumerable.ToList<char>(this.characterMapping.Keys));
			StreamToken token = DataCompresser.CompressToStream(TrueTypeSubsetter.Subset(Enumerable.ToArray<byte>(this.fontFileBytes), trueTypeSubsetEncoding));
			IndirectReferenceToken indirectReferenceToken = writer.WriteToken(token);
			NameToken nameToken = NameToken.Create(this.font.TableRegister.NameTable.GetPostscriptName());
			PostScriptTable postScriptTable = this.font.TableRegister.PostScriptTable;
			HorizontalHeaderTable horizontalHeaderTable = this.font.TableRegister.HorizontalHeaderTable;
			PdfRectangle bounds = this.font.TableRegister.HeaderTable.Bounds;
			decimal num = 1000m / this.font.TableRegister.HeaderTable.UnitsPerEm;
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Type, NameToken.FontDescriptor);
			dictionary.Add(NameToken.FontName, nameToken);
			dictionary.Add(NameToken.Flags, new NumericToken(4m));
			dictionary.Add(NameToken.FontBbox, TrueTypeWritingFont.GetBoundingBox(bounds, num));
			dictionary.Add(NameToken.ItalicAngle, new NumericToken((decimal)postScriptTable.ItalicAngle));
			dictionary.Add(NameToken.Ascent, new NumericToken(Math.Round(horizontalHeaderTable.Ascent * num, 2)));
			dictionary.Add(NameToken.Descent, new NumericToken(Math.Round(horizontalHeaderTable.Descent * num, 2)));
			dictionary.Add(NameToken.CapHeight, new NumericToken(90m));
			dictionary.Add(NameToken.StemV, new NumericToken(90m));
			dictionary.Add(NameToken.FontFile2, indirectReferenceToken);
			Dictionary<NameToken, IToken> dictionary2 = dictionary;
			Os2Table os2Table = this.font.TableRegister.Os2Table;
			if (os2Table == null)
			{
				throw new InvalidFontFormatException("Embedding TrueType font requires OS/2 table.");
			}
			Os2Version2To4OpenTypeTable os2Version2To4OpenTypeTable = os2Table as Os2Version2To4OpenTypeTable;
			if (os2Version2To4OpenTypeTable != null)
			{
				dictionary2[NameToken.CapHeight] = new NumericToken(os2Version2To4OpenTypeTable.CapHeight);
				dictionary2[NameToken.Xheight] = new NumericToken(os2Version2To4OpenTypeTable.XHeight);
			}
			dictionary2[NameToken.StemV] = new NumericToken((decimal)bounds.Width * num * 0.13m);
			int num2 = 0;
			List<NumericToken> list = new List<NumericToken>();
			list.Add(NumericToken.Zero);
			List<NumericToken> list2 = list;
			foreach (KeyValuePair<char, byte> keyValuePair in this.characterMapping)
			{
				if ((int)keyValuePair.Value > num2)
				{
					num2 = (int)keyValuePair.Value;
				}
				int num3 = this.font.WindowsUnicodeCMap.CharacterCodeToGlyphIndex((int)keyValuePair.Key);
				decimal num4 = decimal.Round(this.font.TableRegister.HorizontalMetricsTable.GetAdvanceWidth(num3) * num, 2);
				list2.Add(new NumericToken(num4));
			}
			IndirectReferenceToken indirectReferenceToken2 = writer.WriteToken(new DictionaryToken(dictionary2));
			StreamToken token2 = DataCompresser.CompressToStream(ToUnicodeCMapBuilder.ConvertToCMapStream(this.characterMapping));
			IndirectReferenceToken indirectReferenceToken3 = writer.WriteToken(token2);
			Dictionary<NameToken, IToken> dictionary3 = new Dictionary<NameToken, IToken>();
			dictionary3.Add(NameToken.Type, NameToken.Font);
			dictionary3.Add(NameToken.Subtype, NameToken.TrueType);
			dictionary3.Add(NameToken.BaseFont, nameToken);
			dictionary3.Add(NameToken.FontDescriptor, indirectReferenceToken2);
			dictionary3.Add(NameToken.FirstChar, new NumericToken(0m));
			dictionary3.Add(NameToken.LastChar, new NumericToken(num2));
			dictionary3.Add(NameToken.Widths, new ArrayToken(list2));
			dictionary3.Add(NameToken.ToUnicode, indirectReferenceToken3);
			DictionaryToken token3 = new DictionaryToken(dictionary3);
			if (reservedIndirect != null)
			{
				return writer.WriteToken(token3, reservedIndirect);
			}
			return writer.WriteToken(token3);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public byte GetValueForCharacter(char character)
		{
			object obj = this.mappingLock;
			byte result;
			lock (obj)
			{
				byte b;
				if (this.characterMapping.TryGetValue(character, ref b))
				{
					result = b;
				}
				else
				{
					if (this.characterMappingCounter > 255)
					{
						throw new NotSupportedException("Cannot support more than 255 separate characters in a simple TrueType font, please submit an issue since we will need to add support for composite fonts with multi-byte character identifiers.");
					}
					int num = this.characterMappingCounter;
					this.characterMappingCounter = num + 1;
					byte b2 = (byte)num;
					this.characterMapping[character] = b2;
					b = b2;
					result = b;
				}
			}
			return result;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000A374 File Offset: 0x00008574
		private static ArrayToken GetBoundingBox(PdfRectangle boundingBox, decimal scaling)
		{
			return new ArrayToken(new NumericToken[]
			{
				new NumericToken(Math.Round((decimal)boundingBox.Left * scaling, 2)),
				new NumericToken(Math.Round((decimal)boundingBox.Bottom * scaling, 2)),
				new NumericToken(Math.Round((decimal)boundingBox.Right * scaling, 2)),
				new NumericToken(Math.Round((decimal)boundingBox.Top * scaling, 2))
			});
		}

		// Token: 0x0400009F RID: 159
		private readonly TrueTypeFont font;

		// Token: 0x040000A0 RID: 160
		private readonly IReadOnlyList<byte> fontFileBytes;

		// Token: 0x040000A1 RID: 161
		private readonly object mappingLock = new object();

		// Token: 0x040000A2 RID: 162
		private readonly Dictionary<char, byte> characterMapping = new Dictionary<char, byte>();

		// Token: 0x040000A3 RID: 163
		private int characterMappingCounter = 1;
	}
}
