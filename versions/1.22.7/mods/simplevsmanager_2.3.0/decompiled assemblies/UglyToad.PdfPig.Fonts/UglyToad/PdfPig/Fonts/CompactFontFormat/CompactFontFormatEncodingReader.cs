using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets;
using UglyToad.PdfPig.Fonts.Encodings;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000084 RID: 132
	internal static class CompactFontFormatEncodingReader
	{
		// Token: 0x06000305 RID: 773 RVA: 0x0001543C File Offset: 0x0001363C
		public static Encoding ReadEncoding(CompactFontFormatData data, ICompactFontFormatCharset charset, IReadOnlyList<string> stringIndex)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte b = data.ReadCard8();
			int num = (int)(b & 127);
			if (num == 0)
			{
				return CompactFontFormatEncodingReader.ReadFormat0Encoding(data, charset, stringIndex, b);
			}
			if (num != 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(71, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The provided format ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(b);
				defaultInterpolatedStringHandler.AppendLiteral(" for this Compact Font Format encoding was invalid.");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return CompactFontFormatEncodingReader.ReadFormat1Encoding(data, charset, stringIndex, b);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000154B8 File Offset: 0x000136B8
		private static CompactFontFormatFormat0Encoding ReadFormat0Encoding(CompactFontFormatData data, ICompactFontFormatCharset charset, IReadOnlyList<string> stringIndex, byte format)
		{
			byte b = data.ReadCard8();
			List<ValueTuple<int, int, string>> list = new List<ValueTuple<int, int, string>>();
			for (int i = 1; i <= (int)b; i++)
			{
				byte b2 = data.ReadCard8();
				int stringIdByGlyphId = charset.GetStringIdByGlyphId(i);
				string text = CompactFontFormatEncodingReader.ReadString(stringIdByGlyphId, stringIndex);
				list.Add(new ValueTuple<int, int, string>((int)b2, stringIdByGlyphId, text));
			}
			IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> supplements = new List<CompactFontFormatBuiltInEncoding.Supplement>();
			if (CompactFontFormatEncodingReader.HasSupplement(format))
			{
				supplements = CompactFontFormatEncodingReader.ReadSupplement(data, stringIndex);
			}
			return new CompactFontFormatFormat0Encoding(list, supplements);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00015528 File Offset: 0x00013728
		private static CompactFontFormatFormat1Encoding ReadFormat1Encoding(CompactFontFormatData data, ICompactFontFormatCharset charset, IReadOnlyList<string> stringIndex, byte format)
		{
			byte b = data.ReadCard8();
			List<ValueTuple<int, int, string>> list = new List<ValueTuple<int, int, string>>();
			int num = 1;
			for (int i = 0; i < (int)b; i++)
			{
				int num2 = (int)data.ReadCard8();
				int num3 = (int)data.ReadCard8();
				for (int j = 0; j < 1 + num3; j++)
				{
					int stringIdByGlyphId = charset.GetStringIdByGlyphId(num);
					int num4 = num2 + j;
					string text = CompactFontFormatEncodingReader.ReadString(stringIdByGlyphId, stringIndex);
					list.Add(new ValueTuple<int, int, string>(num4, stringIdByGlyphId, text));
					num++;
				}
			}
			IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> supplements = new List<CompactFontFormatBuiltInEncoding.Supplement>();
			if (CompactFontFormatEncodingReader.HasSupplement(format))
			{
				supplements = CompactFontFormatEncodingReader.ReadSupplement(data, stringIndex);
			}
			return new CompactFontFormatFormat1Encoding((int)b, list, supplements);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000155C4 File Offset: 0x000137C4
		private static IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> ReadSupplement(CompactFontFormatData dataInput, IReadOnlyList<string> stringIndex)
		{
			CompactFontFormatBuiltInEncoding.Supplement[] array = new CompactFontFormatBuiltInEncoding.Supplement[(int)dataInput.ReadCard8()];
			for (int i = 0; i < array.Length; i++)
			{
				byte code = dataInput.ReadCard8();
				int num = dataInput.ReadSid();
				string name = CompactFontFormatEncodingReader.ReadString(num, stringIndex);
				array[i] = new CompactFontFormatBuiltInEncoding.Supplement((int)code, num, name);
			}
			return array;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00015610 File Offset: 0x00013810
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

		// Token: 0x0600030A RID: 778 RVA: 0x0001565E File Offset: 0x0001385E
		private static bool HasSupplement(byte format)
		{
			return (format & 128) > 0;
		}
	}
}
