using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x0200009D RID: 157
	internal class CompactFontFormatTopLevelDictionaryReader : CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>
	{
		// Token: 0x060003BE RID: 958 RVA: 0x00019FD0 File Offset: 0x000181D0
		public override CompactFontFormatTopLevelDictionary Read(CompactFontFormatData data, IReadOnlyList<string> stringIndex)
		{
			CompactFontFormatTopLevelDictionary compactFontFormatTopLevelDictionary = new CompactFontFormatTopLevelDictionary();
			base.ReadDictionary(compactFontFormatTopLevelDictionary, data, stringIndex);
			return compactFontFormatTopLevelDictionary;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00019FF0 File Offset: 0x000181F0
		protected override void ApplyOperation(CompactFontFormatTopLevelDictionary dictionary, List<CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.Operand> operands, CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.OperandKey key, IReadOnlyList<string> stringIndex)
		{
			switch (key.Byte0)
			{
			case 0:
				dictionary.Version = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
				return;
			case 1:
				dictionary.Notice = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
				return;
			case 2:
				dictionary.FullName = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
				return;
			case 3:
				dictionary.FamilyName = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
				return;
			case 4:
				dictionary.Weight = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
				return;
			case 5:
				dictionary.FontBoundingBox = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetBoundingBox(operands);
				return;
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				break;
			case 12:
				if (key.Byte1 == null)
				{
					throw new InvalidOperationException("A single byte sequence beginning with 12 was found.");
				}
				switch (key.Byte1.Value)
				{
				case 0:
					dictionary.Copyright = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
					return;
				case 1:
					dictionary.IsFixedPitch = (operands[0].Decimal == 1m);
					return;
				case 2:
					dictionary.ItalicAngle = operands[0].Decimal;
					return;
				case 3:
					dictionary.UnderlinePosition = operands[0].Decimal;
					return;
				case 4:
					dictionary.UnderlineThickness = operands[0].Decimal;
					return;
				case 5:
					dictionary.PaintType = operands[0].Decimal;
					return;
				case 6:
					dictionary.CharStringType = (CompactFontFormatCharStringType)CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 2);
					return;
				case 7:
				{
					decimal[] array = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.ToArray(operands);
					if (array.Length == 4)
					{
						dictionary.FontMatrix = TransformationMatrix.FromArray(array);
						return;
					}
					if (array.Length == 6)
					{
						dictionary.FontMatrix = TransformationMatrix.FromArray(array);
						return;
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(56, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Expected four values for the font matrix, instead got: ");
					defaultInterpolatedStringHandler.AppendFormatted<decimal[]>(array);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				case 8:
					dictionary.StrokeWidth = operands[0].Decimal;
					return;
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
					break;
				case 20:
					dictionary.SyntheticBaseFontIndex = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 21:
					dictionary.PostScript = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
					return;
				case 22:
					dictionary.BaseFontName = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
					return;
				case 23:
					dictionary.BaseFontBlend = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.ReadDeltaToArray(operands);
					return;
				case 30:
				{
					string @string = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
					operands.RemoveAt(0);
					string string2 = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
					operands.RemoveAt(0);
					int intOrDefault = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					dictionary.CidFontOperators.Ros = new RegistryOrderingSupplement
					{
						Registry = @string,
						Ordering = string2,
						Supplement = intOrDefault
					};
					dictionary.IsCidFont = true;
					return;
				}
				case 31:
					dictionary.CidFontOperators.Version = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 32:
					dictionary.CidFontOperators.Revision = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 33:
					dictionary.CidFontOperators.Type = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 34:
					dictionary.CidFontOperators.Count = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 35:
					dictionary.CidFontOperators.UidBase = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 36:
					dictionary.CidFontOperators.FontDictionaryArray = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 37:
					dictionary.CidFontOperators.FontDictionarySelect = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
					return;
				case 38:
					dictionary.CidFontOperators.FontName = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetString(operands, stringIndex);
					return;
				default:
					return;
				}
				break;
			case 13:
				dictionary.UniqueId = ((operands.Count > 0) ? operands[0].Decimal : 0m);
				return;
			case 14:
				dictionary.Xuid = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.ToArray(operands);
				return;
			case 15:
				dictionary.CharSetOffset = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
				return;
			case 16:
				dictionary.EncodingOffset = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
				return;
			case 17:
				dictionary.CharStringsOffset = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
				return;
			case 18:
			{
				int intOrDefault2 = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
				operands.RemoveAt(0);
				int intOrDefault3 = CompactFontFormatDictionaryReader<CompactFontFormatTopLevelDictionary, CompactFontFormatTopLevelDictionary>.GetIntOrDefault(operands, 0);
				dictionary.PrivateDictionaryLocation = new CompactFontFormatTopLevelDictionary.SizeAndOffset?(new CompactFontFormatTopLevelDictionary.SizeAndOffset(intOrDefault2, intOrDefault3));
				break;
			}
			default:
				return;
			}
		}
	}
}
