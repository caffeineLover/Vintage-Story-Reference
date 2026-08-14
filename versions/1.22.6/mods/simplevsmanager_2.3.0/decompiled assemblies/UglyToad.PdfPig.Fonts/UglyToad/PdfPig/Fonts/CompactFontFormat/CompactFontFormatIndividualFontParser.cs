using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets;
using UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x0200008E RID: 142
	internal class CompactFontFormatIndividualFontParser
	{
		// Token: 0x0600033C RID: 828 RVA: 0x00016B92 File Offset: 0x00014D92
		public CompactFontFormatIndividualFontParser(CompactFontFormatTopLevelDictionaryReader topLevelDictionaryReader, CompactFontFormatPrivateDictionaryReader privateDictionaryReader)
		{
			this.topLevelDictionaryReader = topLevelDictionaryReader;
			this.privateDictionaryReader = privateDictionaryReader;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00016BA8 File Offset: 0x00014DA8
		public CompactFontFormatFont Parse(CompactFontFormatData data, string name, IReadOnlyList<byte> topDictionaryIndex, IReadOnlyList<string> stringIndex, CompactFontFormatIndex globalSubroutineIndex)
		{
			CompactFontFormatData compactFontFormatData = new CompactFontFormatData(Enumerable.ToArray<byte>(topDictionaryIndex));
			CompactFontFormatTopLevelDictionary compactFontFormatTopLevelDictionary = this.topLevelDictionaryReader.Read(compactFontFormatData, stringIndex);
			CompactFontFormatPrivateDictionary compactFontFormatPrivateDictionary = CompactFontFormatPrivateDictionary.GetDefault();
			if (compactFontFormatTopLevelDictionary.PrivateDictionaryLocation != null && compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Size > 0)
			{
				CompactFontFormatData compactFontFormatData2 = data.SnapshotPortion(compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Offset, compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Size);
				compactFontFormatPrivateDictionary = this.privateDictionaryReader.Read(compactFontFormatData2, stringIndex);
			}
			if (compactFontFormatTopLevelDictionary.CharStringsOffset < 0)
			{
				throw new InvalidOperationException("Expected CFF to contain a CharString offset.");
			}
			CompactFontFormatIndex compactFontFormatIndex = CompactFontFormatIndex.None;
			if (compactFontFormatPrivateDictionary.LocalSubroutineOffset != null && compactFontFormatTopLevelDictionary.PrivateDictionaryLocation != null)
			{
				data.Seek(compactFontFormatPrivateDictionary.LocalSubroutineOffset.Value + compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Offset);
				compactFontFormatIndex = CompactFontFormatIndexReader.ReadDictionaryData(data);
			}
			data.Seek(compactFontFormatTopLevelDictionary.CharStringsOffset);
			CompactFontFormatIndex compactFontFormatIndex2 = CompactFontFormatIndexReader.ReadDictionaryData(data);
			ICompactFontFormatCharset charset;
			if (compactFontFormatTopLevelDictionary.CharSetOffset >= 0)
			{
				int charSetOffset = compactFontFormatTopLevelDictionary.CharSetOffset;
				if (!compactFontFormatTopLevelDictionary.IsCidFont && charSetOffset == 0)
				{
					charset = CompactFontFormatIsoAdobeCharset.Value;
				}
				else if (!compactFontFormatTopLevelDictionary.IsCidFont && charSetOffset == 1)
				{
					charset = CompactFontFormatExpertCharset.Value;
				}
				else if (!compactFontFormatTopLevelDictionary.IsCidFont && charSetOffset == 2)
				{
					charset = CompactFontFormatExpertSubsetCharset.Value;
				}
				else
				{
					charset = CompactFontFormatIndividualFontParser.ReadCharset(data, compactFontFormatTopLevelDictionary, compactFontFormatIndex2, stringIndex);
				}
			}
			else if (compactFontFormatTopLevelDictionary.IsCidFont)
			{
				charset = new CompactFontFormatEmptyCharset(compactFontFormatIndex2.Count);
			}
			else
			{
				charset = CompactFontFormatIsoAdobeCharset.Value;
			}
			if (compactFontFormatTopLevelDictionary.IsCidFont)
			{
				return this.ReadCidFont(data, compactFontFormatTopLevelDictionary, compactFontFormatIndex2.Count, stringIndex, compactFontFormatPrivateDictionary, charset, globalSubroutineIndex, compactFontFormatIndex, compactFontFormatIndex2);
			}
			int encodingOffset = compactFontFormatTopLevelDictionary.EncodingOffset;
			Encoding fontEncoding = null;
			if (encodingOffset != -1)
			{
				if (encodingOffset == 0)
				{
					fontEncoding = CompactFontFormatStandardEncoding.Instance;
				}
				else if (encodingOffset == 1)
				{
					fontEncoding = CompactFontFormatExpertEncoding.Instance;
				}
				else
				{
					data.Seek(encodingOffset);
					fontEncoding = CompactFontFormatEncodingReader.ReadEncoding(data, charset, stringIndex);
				}
			}
			CompactFontFormatSubroutinesSelector subroutinesSelector = new CompactFontFormatSubroutinesSelector(globalSubroutineIndex, compactFontFormatIndex);
			Type2CharStrings type2CharStrings = CompactFontFormatIndividualFontParser.ReadCharStrings(data, compactFontFormatTopLevelDictionary, compactFontFormatIndex2, subroutinesSelector, charset);
			return new CompactFontFormatFont(compactFontFormatTopLevelDictionary, compactFontFormatPrivateDictionary, charset, Union<Type1CharStrings, Type2CharStrings>.Two(type2CharStrings), fontEncoding);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00016DDC File Offset: 0x00014FDC
		private static ICompactFontFormatCharset ReadCharset(CompactFontFormatData data, CompactFontFormatTopLevelDictionary topDictionary, CompactFontFormatIndex charStringIndex, IReadOnlyList<string> stringIndex)
		{
			data.Seek(topDictionary.CharSetOffset);
			byte b = data.ReadCard8();
			if (b == 0)
			{
				List<ValueTuple<int, int, string>> list = new List<ValueTuple<int, int, string>>();
				for (int i = 1; i < charStringIndex.Count; i++)
				{
					int num = data.ReadSid();
					list.Add(new ValueTuple<int, int, string>(i, num, CompactFontFormatIndividualFontParser.ReadString(num, stringIndex)));
				}
				return new CompactFontFormatFormat0Charset(list);
			}
			if (b - 1 > 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unrecognized format for the Charset table in a CFF font. Got: ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(b);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<ValueTuple<int, int, string>> list2 = new List<ValueTuple<int, int, string>>();
			for (int j = 1; j < charStringIndex.Count; j++)
			{
				int num2 = data.ReadSid();
				ushort num3 = (b == 1) ? ((ushort)data.ReadCard8()) : data.ReadCard16();
				list2.Add(new ValueTuple<int, int, string>(j, num2, CompactFontFormatIndividualFontParser.ReadString(num2, stringIndex)));
				for (int k = 0; k < (int)num3; k++)
				{
					j++;
					int num4 = num2 + k + 1;
					list2.Add(new ValueTuple<int, int, string>(j, num4, CompactFontFormatIndividualFontParser.ReadString(num4, stringIndex)));
				}
			}
			if (b == 1)
			{
				return new CompactFontFormatFormat1Charset(list2);
			}
			return new CompactFontFormatFormat2Charset(list2);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00016F18 File Offset: 0x00015118
		private static string ReadString(int index, IReadOnlyList<string> stringIndex)
		{
			if (index >= 0 && index <= 390)
			{
				return CompactFontFormatStandardStrings.GetName(index);
			}
			if (index - 391 < stringIndex.Count)
			{
				return stringIndex[index - 391];
			}
			return "SID" + index.ToString();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00016F68 File Offset: 0x00015168
		private static Type2CharStrings ReadCharStrings(CompactFontFormatData data, CompactFontFormatTopLevelDictionary topDictionary, CompactFontFormatIndex charStringIndex, CompactFontFormatSubroutinesSelector subroutinesSelector, ICompactFontFormatCharset charset)
		{
			data.Seek(topDictionary.CharStringsOffset);
			CompactFontFormatCharStringType charStringType = topDictionary.CharStringType;
			if (charStringType == CompactFontFormatCharStringType.Type1)
			{
				throw new NotImplementedException("Type 1 CharStrings are not currently supported in CFF font.");
			}
			if (charStringType != CompactFontFormatCharStringType.Type2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(41, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected CharString type in CFF font: ");
				defaultInterpolatedStringHandler.AppendFormatted<CompactFontFormatCharStringType>(topDictionary.CharStringType);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return Type2CharStringParser.Parse(charStringIndex, subroutinesSelector, charset);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00016FE4 File Offset: 0x000151E4
		private CompactFontFormatCidFont ReadCidFont(CompactFontFormatData data, CompactFontFormatTopLevelDictionary topLevelDictionary, int numberOfGlyphs, IReadOnlyList<string> stringIndex, CompactFontFormatPrivateDictionary privateDictionary, ICompactFontFormatCharset charset, CompactFontFormatIndex globalSubroutines, CompactFontFormatIndex localSubroutinesTop, CompactFontFormatIndex charStringIndex)
		{
			int fontDictionaryArray = topLevelDictionary.CidFontOperators.FontDictionaryArray;
			data.Seek(fontDictionaryArray);
			CompactFontFormatIndex compactFontFormatIndex = CompactFontFormatIndexReader.ReadDictionaryData(data);
			List<CompactFontFormatPrivateDictionary> list = new List<CompactFontFormatPrivateDictionary>();
			List<CompactFontFormatTopLevelDictionary> list2 = new List<CompactFontFormatTopLevelDictionary>();
			List<CompactFontFormatIndex> list3 = new List<CompactFontFormatIndex>();
			foreach (IReadOnlyList<byte> dataBytes in compactFontFormatIndex)
			{
				CompactFontFormatTopLevelDictionary compactFontFormatTopLevelDictionary = this.topLevelDictionaryReader.Read(new CompactFontFormatData(dataBytes), stringIndex);
				if (compactFontFormatTopLevelDictionary.PrivateDictionaryLocation == null)
				{
					throw new InvalidFontFormatException("The CID keyed Compact Font Format font did not contain a private dictionary for the font dictionary.");
				}
				CompactFontFormatData compactFontFormatData = data.SnapshotPortion(compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Offset, compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Size);
				CompactFontFormatPrivateDictionary compactFontFormatPrivateDictionary = this.privateDictionaryReader.Read(compactFontFormatData, stringIndex);
				if (compactFontFormatPrivateDictionary.LocalSubroutineOffset != null && compactFontFormatPrivateDictionary.LocalSubroutineOffset.Value > 0)
				{
					data.Seek(compactFontFormatTopLevelDictionary.PrivateDictionaryLocation.Value.Offset + compactFontFormatPrivateDictionary.LocalSubroutineOffset.Value);
					CompactFontFormatIndex compactFontFormatIndex2 = CompactFontFormatIndexReader.ReadDictionaryData(data);
					list3.Add(compactFontFormatIndex2);
				}
				else
				{
					list3.Add(null);
				}
				list2.Add(compactFontFormatTopLevelDictionary);
				list.Add(compactFontFormatPrivateDictionary);
			}
			data.Seek(topLevelDictionary.CidFontOperators.FontDictionarySelect);
			byte b = data.ReadCard8();
			ICompactFontFormatFdSelect fdSelect;
			if (b != 0)
			{
				if (b != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid Font Dictionary Select format: ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(b);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				fdSelect = CompactFontFormatIndividualFontParser.ReadFormat3FdSelect(data, topLevelDictionary.CidFontOperators.Ros);
			}
			else
			{
				fdSelect = CompactFontFormatIndividualFontParser.ReadFormat0FdSelect(data, numberOfGlyphs, topLevelDictionary.CidFontOperators.Ros);
			}
			CompactFontFormatSubroutinesSelector subroutinesSelector = new CompactFontFormatSubroutinesSelector(globalSubroutines, localSubroutinesTop, fdSelect, list3);
			Union<Type1CharStrings, Type2CharStrings>.Case2 charStrings = Union<Type1CharStrings, Type2CharStrings>.Two(CompactFontFormatIndividualFontParser.ReadCharStrings(data, topLevelDictionary, charStringIndex, subroutinesSelector, charset));
			return new CompactFontFormatCidFont(topLevelDictionary, privateDictionary, charset, charStrings, list2, list, fdSelect);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0001721C File Offset: 0x0001541C
		private static CompactFontFormat0FdSelect ReadFormat0FdSelect(CompactFontFormatData data, int numberOfGlyphs, RegistryOrderingSupplement registryOrderingSupplement)
		{
			int[] array = new int[numberOfGlyphs];
			for (int i = 0; i < numberOfGlyphs; i++)
			{
				array[i] = (int)data.ReadCard8();
			}
			return new CompactFontFormat0FdSelect(registryOrderingSupplement, array);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001724C File Offset: 0x0001544C
		private static CompactFontFormat3FdSelect ReadFormat3FdSelect(CompactFontFormatData data, RegistryOrderingSupplement registryOrderingSupplement)
		{
			ushort num = data.ReadCard16();
			CompactFontFormat3FdSelect.Range3[] array = new CompactFontFormat3FdSelect.Range3[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				ushort first = data.ReadCard16();
				byte fontDictionary = data.ReadCard8();
				array[i] = new CompactFontFormat3FdSelect.Range3((int)first, (int)fontDictionary);
			}
			ushort sentinel = data.ReadCard16();
			return new CompactFontFormat3FdSelect(registryOrderingSupplement, array, (int)sentinel);
		}

		// Token: 0x04000317 RID: 791
		private readonly CompactFontFormatTopLevelDictionaryReader topLevelDictionaryReader;

		// Token: 0x04000318 RID: 792
		private readonly CompactFontFormatPrivateDictionaryReader privateDictionaryReader;
	}
}
