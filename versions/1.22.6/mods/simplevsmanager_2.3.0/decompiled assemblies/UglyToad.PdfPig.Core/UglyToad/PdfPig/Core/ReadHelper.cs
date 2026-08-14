using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000011 RID: 17
	public static class ReadHelper
	{
		// Token: 0x06000070 RID: 112 RVA: 0x000039F4 File Offset: 0x00001BF4
		public static string ReadLine(IInputBytes bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (bytes.IsAtEnd())
			{
				throw new InvalidOperationException("Error: End-of-File, expected line");
			}
			StringBuilder stringBuilder = new StringBuilder(11);
			byte b = 0;
			while (bytes.MoveNext())
			{
				b = bytes.CurrentByte;
				if (ReadHelper.IsEndOfLine(b))
				{
					break;
				}
				stringBuilder.Append((char)b);
			}
			if (ReadHelper.IsCarriageReturn(b) && ReadHelper.IsLineFeed(bytes.Peek()))
			{
				bytes.MoveNext();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A70 File Offset: 0x00001C70
		public static void SkipSpaces(IInputBytes bytes)
		{
			bytes.MoveNext();
			byte currentByte = bytes.CurrentByte;
			while (ReadHelper.IsWhitespace(currentByte) || currentByte == 37)
			{
				if (currentByte == 37)
				{
					bytes.MoveNext();
					currentByte = bytes.CurrentByte;
					while (!ReadHelper.IsEndOfLine(currentByte))
					{
						bytes.MoveNext();
						currentByte = bytes.CurrentByte;
					}
				}
				else
				{
					bytes.MoveNext();
					currentByte = bytes.CurrentByte;
				}
			}
			if (!bytes.IsAtEnd())
			{
				bytes.Seek(bytes.CurrentOffset - 1L);
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003AEC File Offset: 0x00001CEC
		public static bool IsEndOfName(int ch)
		{
			return ReadHelper.EndOfNameCharacters.Contains(ch);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003AF9 File Offset: 0x00001CF9
		public static bool IsWhitespace(byte c)
		{
			return c == 0 || c == 32 || c == 10 || c == 13 || c == 9 || c == 12;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003B19 File Offset: 0x00001D19
		public static bool IsEndOfLine(char c)
		{
			return ReadHelper.IsEndOfLine((byte)c);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003B22 File Offset: 0x00001D22
		public static bool IsEndOfLine(byte b)
		{
			return ReadHelper.IsLineFeed(new byte?(b)) || ReadHelper.IsCarriageReturn(b);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003B3C File Offset: 0x00001D3C
		public static bool IsLineFeed(byte? c)
		{
			int num = 10;
			byte? b = c;
			int? num2 = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			return num == num2.GetValueOrDefault() & num2 != null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003B7F File Offset: 0x00001D7F
		public static bool IsCarriageReturn(byte c)
		{
			return 13 == c;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003B88 File Offset: 0x00001D88
		public static bool IsString(IInputBytes bytes, string s)
		{
			bool result = true;
			long currentOffset = bytes.CurrentOffset;
			for (int i = 0; i < s.Length; i++)
			{
				char c = s.get_Chars(i);
				if ((char)bytes.CurrentByte != c)
				{
					result = false;
					break;
				}
				bytes.MoveNext();
			}
			bytes.Seek(currentOffset);
			return result;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003BD8 File Offset: 0x00001DD8
		public static long ReadLong(IInputBytes bytes)
		{
			ReadHelper.SkipSpaces(bytes);
			StringBuilder stringBuilder = ReadHelper.ReadStringNumber(bytes);
			long result;
			try
			{
				result = long.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				byte[] array = OtherEncodings.StringAsLatin1Bytes(stringBuilder.ToString());
				bytes.Seek(bytes.CurrentOffset - (long)array.Length);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error: Expected a long type at offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(bytes.CurrentOffset);
				defaultInterpolatedStringHandler.AppendLiteral(", instead got '");
				defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(stringBuilder);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
			}
			return result;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003C88 File Offset: 0x00001E88
		public static bool IsDigit(int c)
		{
			return c >= 48 && c <= 57;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003C9C File Offset: 0x00001E9C
		public static int ReadInt(IInputBytes bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			ReadHelper.SkipSpaces(bytes);
			StringBuilder stringBuilder = ReadHelper.ReadStringNumber(bytes);
			int result;
			try
			{
				result = int.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
			}
			catch (Exception inner)
			{
				bytes.Seek(bytes.CurrentOffset - (long)OtherEncodings.StringAsLatin1Bytes(stringBuilder.ToString()).Length);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error: Expected an integer type at offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(bytes.CurrentOffset);
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear(), inner);
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003D38 File Offset: 0x00001F38
		public static bool IsSpace(int c)
		{
			return c == 32;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003D3F File Offset: 0x00001F3F
		public static bool IsHex(byte b)
		{
			return ReadHelper.IsHex((char)b);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003D47 File Offset: 0x00001F47
		public static bool IsHex(char ch)
		{
			return char.IsDigit(ch) || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003D6C File Offset: 0x00001F6C
		public static bool IsValidUtf8(byte[] input)
		{
			bool result;
			try
			{
				Decoder decoder = Encoding.UTF8.GetDecoder();
				int charCount = decoder.GetCharCount(input, 0, input.Length);
				char[] array = new char[charCount];
				int num;
				int num2;
				decoder.Convert(input, 0, input.Length, array, 0, charCount, true, ref num, ref num2, ref result);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003DC8 File Offset: 0x00001FC8
		private static StringBuilder ReadStringNumber(IInputBytes reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte currentByte;
			while (reader.MoveNext() && (currentByte = reader.CurrentByte) != 32 && currentByte != 10 && currentByte != 13 && currentByte != 60 && currentByte != 91 && currentByte != 40 && currentByte != 0)
			{
				stringBuilder.Append((char)currentByte);
				if (stringBuilder.Length > ReadHelper.MaximumNumberStringLength)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(54, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Number '");
					defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(stringBuilder);
					defaultInterpolatedStringHandler.AppendLiteral("' is getting too long, stop reading at offset ");
					defaultInterpolatedStringHandler.AppendFormatted<long>(reader.CurrentOffset);
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (!reader.IsAtEnd())
			{
				reader.Seek(reader.CurrentOffset - 1L);
			}
			return stringBuilder;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003E84 File Offset: 0x00002084
		// Note: this type is marked as 'beforefieldinit'.
		static ReadHelper()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(32);
			hashSet.Add(13);
			hashSet.Add(10);
			hashSet.Add(9);
			hashSet.Add(62);
			hashSet.Add(60);
			hashSet.Add(91);
			hashSet.Add(47);
			hashSet.Add(93);
			hashSet.Add(41);
			hashSet.Add(40);
			hashSet.Add(0);
			hashSet.Add(12);
			ReadHelper.EndOfNameCharacters = hashSet;
			ReadHelper.MaximumNumberStringLength = long.MaxValue.ToString("D").Length;
		}

		// Token: 0x0400001E RID: 30
		public const byte AsciiLineFeed = 10;

		// Token: 0x0400001F RID: 31
		public const byte AsciiCarriageReturn = 13;

		// Token: 0x04000020 RID: 32
		private static readonly HashSet<int> EndOfNameCharacters;

		// Token: 0x04000021 RID: 33
		private static readonly int MaximumNumberStringLength;
	}
}
