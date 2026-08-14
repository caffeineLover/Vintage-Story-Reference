using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x02000098 RID: 152
	internal class CompactFontFormatPrivateDictionaryReader : CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>
	{
		// Token: 0x0600036A RID: 874 RVA: 0x00019A18 File Offset: 0x00017C18
		public override CompactFontFormatPrivateDictionary Read(CompactFontFormatData data, IReadOnlyList<string> stringIndex)
		{
			CompactFontFormatPrivateDictionary.Builder builder = new CompactFontFormatPrivateDictionary.Builder();
			base.ReadDictionary(builder, data, stringIndex);
			return builder.Build();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00019A3C File Offset: 0x00017C3C
		protected override void ApplyOperation(CompactFontFormatPrivateDictionary.Builder dictionary, List<CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.Operand> operands, CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.OperandKey operandKey, IReadOnlyList<string> stringIndex)
		{
			switch (operandKey.Byte0)
			{
			case 6:
				dictionary.BlueValues = CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.ReadDeltaToIntArray(operands);
				return;
			case 7:
				dictionary.OtherBlues = CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.ReadDeltaToIntArray(operands);
				return;
			case 8:
				dictionary.FamilyBlues = CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.ReadDeltaToIntArray(operands);
				return;
			case 9:
				dictionary.FamilyOtherBlues = CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.ReadDeltaToIntArray(operands);
				return;
			case 10:
				dictionary.StandardHorizontalWidth = new decimal?(operands[0].Decimal);
				return;
			case 11:
				dictionary.StandardVerticalWidth = new decimal?(operands[0].Decimal);
				return;
			case 12:
				if (operandKey.Byte1 == null)
				{
					throw new InvalidOperationException("In the CFF private dictionary, got the operation key 12 without a second byte.");
				}
				switch (operandKey.Byte1.Value)
				{
				case 9:
					dictionary.BlueScale = new decimal?(operands[0].Decimal);
					return;
				case 10:
					dictionary.BlueShift = operands[0].Int;
					return;
				case 11:
					dictionary.BlueFuzz = operands[0].Int;
					return;
				case 12:
					dictionary.StemSnapHorizontalWidths = CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.ReadDeltaToArray(operands);
					return;
				case 13:
					dictionary.StemSnapVerticalWidths = CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.ReadDeltaToArray(operands);
					return;
				case 14:
					dictionary.ForceBold = new bool?(operands[0].Decimal == 1m);
					return;
				case 15:
				case 16:
					break;
				case 17:
					dictionary.LanguageGroup = operands[0].Int;
					return;
				case 18:
					dictionary.ExpansionFactor = new decimal?(operands[0].Decimal);
					return;
				case 19:
					dictionary.InitialRandomSeed = operands[0].Decimal;
					return;
				default:
					return;
				}
				break;
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
				break;
			case 19:
				dictionary.LocalSubroutineOffset = new int?(CompactFontFormatDictionaryReader<CompactFontFormatPrivateDictionary, CompactFontFormatPrivateDictionary.Builder>.GetIntOrDefault(operands, -1));
				return;
			case 20:
				dictionary.DefaultWidthX = operands[0].Decimal;
				return;
			case 21:
				dictionary.NominalWidthX = operands[0].Decimal;
				break;
			default:
				return;
			}
		}
	}
}
