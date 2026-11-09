using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000092 RID: 146
	public static class CompactFontFormatParser
	{
		// Token: 0x0600034E RID: 846 RVA: 0x000173F8 File Offset: 0x000155F8
		public static CompactFontFormatFontCollection Parse(CompactFontFormatData data)
		{
			string text = CompactFontFormatParser.ReadTag(data);
			if (text == "OTTO")
			{
				throw new NotSupportedException("Currently tagged CFF data is not supported.");
			}
			if (text == "ttcf")
			{
				throw new NotSupportedException("True Type Collection fonts are not currently supported.");
			}
			if (!(text == "\0\u0001\0\0"))
			{
				data.Seek(0);
				CompactFontFormatHeader header = CompactFontFormatParser.ReadHeader(data);
				string[] array = CompactFontFormatParser.ReadStringIndex(data);
				CompactFontFormatIndex compactFontFormatIndex = CompactFontFormatIndexReader.ReadDictionaryData(data);
				string[] stringIndex = CompactFontFormatParser.ReadStringIndex(data);
				CompactFontFormatIndex globalSubroutineIndex = CompactFontFormatIndexReader.ReadDictionaryData(data);
				Dictionary<string, CompactFontFormatFont> dictionary = new Dictionary<string, CompactFontFormatFont>();
				CompactFontFormatIndividualFontParser compactFontFormatIndividualFontParser = new CompactFontFormatIndividualFontParser(new CompactFontFormatTopLevelDictionaryReader(), new CompactFontFormatPrivateDictionaryReader());
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					dictionary[text2] = compactFontFormatIndividualFontParser.Parse(data, text2, compactFontFormatIndex[i], stringIndex, globalSubroutineIndex);
				}
				return new CompactFontFormatFontCollection(header, dictionary);
			}
			throw new NotSupportedException("OpenType fonts containing a true type font are not currently supported.");
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000174D9 File Offset: 0x000156D9
		private static string ReadTag(CompactFontFormatData data)
		{
			return data.ReadString(4, OtherEncodings.Iso88591);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x000174E8 File Offset: 0x000156E8
		private static CompactFontFormatHeader ReadHeader(CompactFontFormatData data)
		{
			byte majorVersion = data.ReadCard8();
			byte minorVersion = data.ReadCard8();
			byte sizeInBytes = data.ReadCard8();
			byte offsetSize = data.ReadOffsize();
			return new CompactFontFormatHeader(majorVersion, minorVersion, sizeInBytes, offsetSize);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00017518 File Offset: 0x00015718
		private static string[] ReadStringIndex(CompactFontFormatData data)
		{
			int[] array = CompactFontFormatIndexReader.ReadIndex(data);
			if (array.Length == 0)
			{
				return EmptyArray<string>.Instance;
			}
			int num = array.Length - 1;
			string[] array2 = new string[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = array[i + 1] - array[i];
				if (num2 < 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(48, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Negative object length ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler.AppendLiteral(". Current position: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(data.Position);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				array2[i] = data.ReadString(num2, OtherEncodings.Iso88591);
			}
			return array2;
		}

		// Token: 0x0400031E RID: 798
		private const string TagOtto = "OTTO";

		// Token: 0x0400031F RID: 799
		private const string TagTtcf = "ttcf";

		// Token: 0x04000320 RID: 800
		private const string TagTtfonly = "\0\u0001\0\0";
	}
}
