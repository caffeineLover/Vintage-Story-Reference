using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x0200001C RID: 28
	public class TokenWriter : ITokenWriter
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00007C34 File Offset: 0x00005E34
		public static TokenWriter Instance { get; }

		// Token: 0x060000D1 RID: 209 RVA: 0x00007C3C File Offset: 0x00005E3C
		public void WriteToken(IToken token, Stream outputStream)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			ArrayToken arrayToken = token as ArrayToken;
			if (arrayToken != null)
			{
				this.WriteArray(arrayToken, outputStream);
				return;
			}
			BooleanToken booleanToken = token as BooleanToken;
			if (booleanToken != null)
			{
				this.WriteBoolean(booleanToken, outputStream);
				return;
			}
			CommentToken commentToken = token as CommentToken;
			if (commentToken != null)
			{
				this.WriteComment(commentToken, outputStream);
				return;
			}
			DictionaryToken dictionaryToken = token as DictionaryToken;
			if (dictionaryToken != null)
			{
				this.WriteDictionary(dictionaryToken, outputStream);
				return;
			}
			HexToken hexToken = token as HexToken;
			if (hexToken != null)
			{
				this.WriteHex(hexToken, outputStream);
				return;
			}
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken != null)
			{
				this.WriteIndirectReference(indirectReferenceToken, outputStream);
				return;
			}
			NameToken nameToken = token as NameToken;
			if (nameToken != null)
			{
				this.WriteName(nameToken, outputStream);
				return;
			}
			if (token is NullToken)
			{
				outputStream.Write(TokenWriter.Null, 0, TokenWriter.Null.Length);
				this.WriteWhitespace(outputStream);
				return;
			}
			NumericToken numericToken = token as NumericToken;
			if (numericToken != null)
			{
				this.WriteNumber(numericToken, outputStream);
				return;
			}
			ObjectToken objectToken = token as ObjectToken;
			if (objectToken != null)
			{
				this.WriteObject(objectToken, outputStream);
				return;
			}
			StreamToken streamToken = token as StreamToken;
			if (streamToken != null)
			{
				this.WriteStream(streamToken, outputStream);
				return;
			}
			StringToken stringToken = token as StringToken;
			if (stringToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to write token type of ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(token.GetType());
				defaultInterpolatedStringHandler.AppendLiteral(" but was not known.");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.WriteString(stringToken, outputStream);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00007DC4 File Offset: 0x00005FC4
		public void WriteCrossReferenceTable(IReadOnlyDictionary<IndirectReference, long> objectOffsets, IndirectReference catalogToken, Stream outputStream, IndirectReference? documentInformationReference)
		{
			if (objectOffsets.Count == 0)
			{
				throw new InvalidOperationException("Could not write empty cross reference table.");
			}
			this.WriteLineBreak(outputStream);
			long position = outputStream.Position;
			outputStream.Write(TokenWriter.Xref, 0, TokenWriter.Xref.Length);
			this.WriteLineBreak(outputStream);
			List<TokenWriter.XrefSeries> list = new List<TokenWriter.XrefSeries>();
			List<KeyValuePair<IndirectReference, long>> list2 = Enumerable.ToList<KeyValuePair<IndirectReference, long>>(Enumerable.OrderBy<KeyValuePair<IndirectReference, long>, long>(objectOffsets, (KeyValuePair<IndirectReference, long> x) => x.Key.ObjectNumber));
			long first = 0L;
			long num = 0L;
			List<TokenWriter.XrefSeries.OffsetAndGeneration> list3 = new List<TokenWriter.XrefSeries.OffsetAndGeneration>();
			list3.Add(null);
			List<TokenWriter.XrefSeries.OffsetAndGeneration> list4 = list3;
			foreach (KeyValuePair<IndirectReference, long> keyValuePair in list2)
			{
				if (keyValuePair.Key.ObjectNumber - num == 1L)
				{
					num = keyValuePair.Key.ObjectNumber;
					list4.Add(new TokenWriter.XrefSeries.OffsetAndGeneration(keyValuePair.Value, (long)keyValuePair.Key.Generation));
				}
				else
				{
					list.Add(new TokenWriter.XrefSeries(first, list4));
					List<TokenWriter.XrefSeries.OffsetAndGeneration> list5 = new List<TokenWriter.XrefSeries.OffsetAndGeneration>();
					list5.Add(new TokenWriter.XrefSeries.OffsetAndGeneration(keyValuePair.Value, (long)keyValuePair.Key.Generation));
					list4 = list5;
					num = keyValuePair.Key.ObjectNumber;
					first = keyValuePair.Key.ObjectNumber;
				}
			}
			if (list4.Count > 0)
			{
				list.Add(new TokenWriter.XrefSeries(first, list4));
			}
			foreach (TokenWriter.XrefSeries xrefSeries in list)
			{
				this.WriteLong(xrefSeries.First, outputStream);
				this.WriteWhitespace(outputStream);
				this.WriteLong((long)xrefSeries.Offsets.Count, outputStream);
				this.WriteWhitespace(outputStream);
				this.WriteLineBreak(outputStream);
				foreach (TokenWriter.XrefSeries.OffsetAndGeneration offsetAndGeneration in xrefSeries.Offsets)
				{
					if (offsetAndGeneration != null)
					{
						byte[] array = OtherEncodings.StringAsLatin1Bytes(offsetAndGeneration.Offset.ToString("D10", CultureInfo.InvariantCulture));
						outputStream.Write(array, 0, array.Length);
						this.WriteWhitespace(outputStream);
						byte[] array2 = OtherEncodings.StringAsLatin1Bytes(offsetAndGeneration.Generation.ToString("D5", CultureInfo.InvariantCulture));
						outputStream.Write(array2, 0, array2.Length);
						this.WriteWhitespace(outputStream);
						outputStream.WriteByte(TokenWriter.InUseEntry);
						this.WriteWhitespace(outputStream);
						this.WriteLineBreak(outputStream);
					}
					else
					{
						this.WriteFirstXrefEmptyEntry(outputStream);
					}
				}
			}
			outputStream.Write(TokenWriter.Trailer, 0, TokenWriter.Trailer.Length);
			this.WriteLineBreak(outputStream);
			ArrayToken arrayToken = new ArrayToken(new IToken[]
			{
				new HexToken(Guid.NewGuid().ToString("N").ToCharArray()),
				new HexToken(Guid.NewGuid().ToString("N").ToCharArray())
			});
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Size, new NumericToken(objectOffsets.Count + 1));
			dictionary.Add(NameToken.Root, new IndirectReferenceToken(catalogToken));
			dictionary.Add(NameToken.Id, arrayToken);
			Dictionary<NameToken, IToken> dictionary2 = dictionary;
			if (documentInformationReference != null)
			{
				dictionary2[NameToken.Info] = new IndirectReferenceToken(documentInformationReference.Value);
			}
			DictionaryToken dictionary3 = new DictionaryToken(dictionary2);
			this.WriteDictionary(dictionary3, outputStream);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.StartXref, 0, TokenWriter.StartXref.Length);
			this.WriteLineBreak(outputStream);
			this.WriteLong(position, outputStream);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.Eof, 0, TokenWriter.Eof.Length);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000081E0 File Offset: 0x000063E0
		public void WriteObject(long objectNumber, int generation, byte[] data, Stream outputStream)
		{
			this.WriteLong(objectNumber, outputStream);
			this.WriteWhitespace(outputStream);
			this.WriteInt(generation, outputStream);
			this.WriteWhitespace(outputStream);
			outputStream.Write(TokenWriter.ObjStart, 0, TokenWriter.ObjStart.Length);
			this.WriteLineBreak(outputStream);
			outputStream.Write(data, 0, data.Length);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.ObjEnd, 0, TokenWriter.ObjEnd.Length);
			this.WriteLineBreak(outputStream);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000825B File Offset: 0x0000645B
		protected void WriteHex(HexToken hex, Stream stream)
		{
			stream.WriteByte(TokenWriter.HexStart);
			stream.WriteText(hex.GetHexString(), false);
			stream.WriteByte(TokenWriter.HexEnd);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00008280 File Offset: 0x00006480
		protected void WriteArray(ArrayToken array, Stream outputStream)
		{
			outputStream.WriteByte(TokenWriter.ArrayStart);
			this.WriteWhitespace(outputStream);
			for (int i = 0; i < array.Data.Count; i++)
			{
				IToken token = array.Data[i];
				this.WriteToken(token, outputStream);
			}
			outputStream.WriteByte(TokenWriter.ArrayEnd);
			this.WriteWhitespace(outputStream);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000082DC File Offset: 0x000064DC
		protected void WriteBoolean(BooleanToken boolean, Stream outputStream)
		{
			byte[] array = boolean.Data ? TokenWriter.TrueBytes : TokenWriter.FalseBytes;
			outputStream.Write(array, 0, array.Length);
			this.WriteWhitespace(outputStream);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00008310 File Offset: 0x00006510
		protected void WriteComment(CommentToken comment, Stream outputStream)
		{
			byte[] array = OtherEncodings.StringAsLatin1Bytes(comment.Data);
			outputStream.WriteByte(TokenWriter.Comment);
			outputStream.Write(array, 0, array.Length);
			this.WriteLineBreak(outputStream);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00008348 File Offset: 0x00006548
		protected void WriteDictionary(DictionaryToken dictionary, Stream outputStream)
		{
			outputStream.Write(TokenWriter.DictionaryStart, 0, TokenWriter.DictionaryStart.Length);
			foreach (KeyValuePair<string, IToken> keyValuePair in dictionary.Data)
			{
				this.WriteName(keyValuePair.Key, outputStream);
				if (keyValuePair.Value == null)
				{
					this.WriteToken(NullToken.Instance, outputStream);
				}
				else
				{
					this.WriteToken(keyValuePair.Value, outputStream);
				}
			}
			outputStream.Write(TokenWriter.DictionaryEnd, 0, TokenWriter.DictionaryEnd.Length);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000083E8 File Offset: 0x000065E8
		protected virtual void WriteIndirectReference(IndirectReferenceToken reference, Stream outputStream)
		{
			this.WriteLong(reference.Data.ObjectNumber, outputStream);
			this.WriteWhitespace(outputStream);
			this.WriteInt(reference.Data.Generation, outputStream);
			this.WriteWhitespace(outputStream);
			outputStream.WriteByte(TokenWriter.RByte);
			this.WriteWhitespace(outputStream);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000843F File Offset: 0x0000663F
		protected virtual void WriteName(NameToken name, Stream outputStream)
		{
			this.WriteName(name.Data, outputStream);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00008450 File Offset: 0x00006650
		private void WriteName(string name, Stream outputStream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < name.Length; i++)
			{
				char c = name.get_Chars(i);
				if (c < '!' || c > '~' || TokenWriter.DelimiterChars.Contains(c))
				{
					string @string = Hex.GetString(new byte[]
					{
						(byte)c
					});
					stringBuilder.Append('#').Append(@string);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			byte[] array = OtherEncodings.StringAsLatin1Bytes(stringBuilder.ToString());
			outputStream.WriteByte(TokenWriter.NameStart);
			outputStream.Write(array, 0, array.Length);
			this.WriteWhitespace(outputStream);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000084F0 File Offset: 0x000066F0
		protected virtual void WriteNumber(NumericToken number, Stream outputStream)
		{
			if (!number.HasDecimalPlaces)
			{
				this.WriteInt(number.Int, outputStream);
			}
			else
			{
				byte[] array = OtherEncodings.StringAsLatin1Bytes(number.Data.ToString("G", CultureInfo.InvariantCulture));
				outputStream.Write(array, 0, array.Length);
			}
			this.WriteWhitespace(outputStream);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00008544 File Offset: 0x00006744
		protected virtual void WriteObject(ObjectToken objectToken, Stream outputStream)
		{
			this.WriteLong(objectToken.Number.ObjectNumber, outputStream);
			this.WriteWhitespace(outputStream);
			this.WriteInt(objectToken.Number.Generation, outputStream);
			this.WriteWhitespace(outputStream);
			outputStream.Write(TokenWriter.ObjStart, 0, TokenWriter.ObjStart.Length);
			this.WriteLineBreak(outputStream);
			this.WriteToken(objectToken.Data, outputStream);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.ObjEnd, 0, TokenWriter.ObjEnd.Length);
			this.WriteLineBreak(outputStream);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000085D4 File Offset: 0x000067D4
		protected virtual void WriteStream(StreamToken streamToken, Stream outputStream)
		{
			this.WriteDictionary(streamToken.StreamDictionary, outputStream);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.StreamStart, 0, TokenWriter.StreamStart.Length);
			this.WriteLineBreak(outputStream);
			outputStream.Write(Enumerable.ToArray<byte>(streamToken.Data), 0, streamToken.Data.Count);
			this.WriteLineBreak(outputStream);
			outputStream.Write(TokenWriter.StreamEnd, 0, TokenWriter.StreamEnd.Length);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00008648 File Offset: 0x00006848
		protected virtual void WriteString(StringToken stringToken, Stream outputStream)
		{
			outputStream.WriteByte(TokenWriter.StringStart);
			if (stringToken.EncodedWith == null)
			{
				if (Enumerable.Any<char>(stringToken.Data, (char x) => x > 'ÿ'))
				{
					byte[] bytes = new StringToken(stringToken.Data, 2).GetBytes();
					outputStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					for (int i = 0; i < stringToken.Data.Length; i++)
					{
						int num = (int)stringToken.Data.get_Chars(i);
						int num2;
						if (num == 40 || num == 41)
						{
							outputStream.WriteByte(92);
							outputStream.WriteByte((byte)num);
						}
						else if ((num2 = Array.IndexOf<int>(TokenWriter.EscapeNeeded, num)) > -1)
						{
							outputStream.WriteByte(92);
							outputStream.WriteByte((byte)TokenWriter.Escaped[num2]);
						}
						else if (num < 32 || num > 126)
						{
							int num3 = num / 64;
							int num4 = (num - num3 * 64) / 8;
							int num5 = num % 8;
							outputStream.WriteByte(92);
							outputStream.WriteByte((byte)(num3 + 48));
							outputStream.WriteByte((byte)(num4 + 48));
							outputStream.WriteByte((byte)(num5 + 48));
						}
						else
						{
							outputStream.WriteByte((byte)num);
						}
					}
				}
			}
			else
			{
				byte[] bytes2 = stringToken.GetBytes();
				outputStream.Write(bytes2, 0, bytes2.Length);
			}
			outputStream.WriteByte(TokenWriter.StringEnd);
			this.WriteWhitespace(outputStream);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000087A8 File Offset: 0x000069A8
		protected virtual void WriteInt(int value, Stream outputStream)
		{
			byte[] array = OtherEncodings.StringAsLatin1Bytes(value.ToString("G", CultureInfo.InvariantCulture));
			outputStream.Write(array, 0, array.Length);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000087D7 File Offset: 0x000069D7
		protected virtual void WriteLineBreak(Stream outputStream)
		{
			outputStream.WriteNewLine();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000087E0 File Offset: 0x000069E0
		protected virtual void WriteLong(long value, Stream outputStream)
		{
			byte[] array = OtherEncodings.StringAsLatin1Bytes(value.ToString("G", CultureInfo.InvariantCulture));
			outputStream.Write(array, 0, array.Length);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000880F File Offset: 0x00006A0F
		protected virtual void WriteWhitespace(Stream outputStream)
		{
			outputStream.WriteByte(TokenWriter.Whitespace);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000881C File Offset: 0x00006A1C
		private void WriteFirstXrefEmptyEntry(Stream outputStream)
		{
			outputStream.WriteText(new string('0', 10), false);
			outputStream.WriteWhiteSpace();
			outputStream.WriteText("65535", false);
			outputStream.WriteWhiteSpace();
			outputStream.WriteText("f", false);
			outputStream.WriteWhiteSpace();
			outputStream.WriteNewLine();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008869 File Offset: 0x00006A69
		private static byte GetByte(string value)
		{
			byte[] array = OtherEncodings.StringAsLatin1Bytes(value);
			if (array.Length > 1)
			{
				throw new InvalidOperationException();
			}
			return array[0];
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008888 File Offset: 0x00006A88
		// Note: this type is marked as 'beforefieldinit'.
		static TokenWriter()
		{
			HashSet<char> hashSet = new HashSet<char>();
			hashSet.Add('(');
			hashSet.Add(')');
			hashSet.Add('<');
			hashSet.Add('>');
			hashSet.Add('[');
			hashSet.Add(']');
			hashSet.Add('{');
			hashSet.Add('}');
			hashSet.Add('/');
			hashSet.Add('%');
			TokenWriter.DelimiterChars = hashSet;
			TokenWriter.Instance = new TokenWriter();
			TokenWriter.EscapeNeeded = new int[]
			{
				13,
				10,
				9,
				8,
				12,
				92
			};
			TokenWriter.Escaped = new int[]
			{
				114,
				110,
				116,
				98,
				102,
				92
			};
		}

		// Token: 0x0400006B RID: 107
		private static readonly byte ArrayStart = TokenWriter.GetByte("[");

		// Token: 0x0400006C RID: 108
		private static readonly byte ArrayEnd = TokenWriter.GetByte("]");

		// Token: 0x0400006D RID: 109
		private static readonly byte[] DictionaryStart = OtherEncodings.StringAsLatin1Bytes("<<");

		// Token: 0x0400006E RID: 110
		private static readonly byte[] DictionaryEnd = OtherEncodings.StringAsLatin1Bytes(">>");

		// Token: 0x0400006F RID: 111
		private static readonly byte Comment = TokenWriter.GetByte("%");

		// Token: 0x04000070 RID: 112
		private static readonly byte[] Eof = OtherEncodings.StringAsLatin1Bytes("%%EOF");

		// Token: 0x04000071 RID: 113
		private static readonly byte[] FalseBytes = OtherEncodings.StringAsLatin1Bytes("false");

		// Token: 0x04000072 RID: 114
		private static readonly byte HexStart = TokenWriter.GetByte("<");

		// Token: 0x04000073 RID: 115
		private static readonly byte HexEnd = TokenWriter.GetByte(">");

		// Token: 0x04000074 RID: 116
		private static readonly byte InUseEntry = TokenWriter.GetByte("n");

		// Token: 0x04000075 RID: 117
		private static readonly byte NameStart = TokenWriter.GetByte("/");

		// Token: 0x04000076 RID: 118
		private static readonly byte[] Null = OtherEncodings.StringAsLatin1Bytes("null");

		// Token: 0x04000077 RID: 119
		private static readonly byte[] ObjStart = OtherEncodings.StringAsLatin1Bytes("obj");

		// Token: 0x04000078 RID: 120
		private static readonly byte[] ObjEnd = OtherEncodings.StringAsLatin1Bytes("endobj");

		// Token: 0x04000079 RID: 121
		private static readonly byte RByte = TokenWriter.GetByte("R");

		// Token: 0x0400007A RID: 122
		private static readonly byte[] StartXref = OtherEncodings.StringAsLatin1Bytes("startxref");

		// Token: 0x0400007B RID: 123
		protected static readonly byte[] StreamStart = OtherEncodings.StringAsLatin1Bytes("stream");

		// Token: 0x0400007C RID: 124
		protected static readonly byte[] StreamEnd = OtherEncodings.StringAsLatin1Bytes("endstream");

		// Token: 0x0400007D RID: 125
		private static readonly byte StringStart = TokenWriter.GetByte("(");

		// Token: 0x0400007E RID: 126
		private static readonly byte StringEnd = TokenWriter.GetByte(")");

		// Token: 0x0400007F RID: 127
		private static readonly byte[] Trailer = OtherEncodings.StringAsLatin1Bytes("trailer");

		// Token: 0x04000080 RID: 128
		private static readonly byte[] TrueBytes = OtherEncodings.StringAsLatin1Bytes("true");

		// Token: 0x04000081 RID: 129
		private static readonly byte Whitespace = TokenWriter.GetByte(" ");

		// Token: 0x04000082 RID: 130
		private static readonly byte[] Xref = OtherEncodings.StringAsLatin1Bytes("xref");

		// Token: 0x04000083 RID: 131
		private static readonly HashSet<char> DelimiterChars;

		// Token: 0x04000085 RID: 133
		private static readonly int[] EscapeNeeded;

		// Token: 0x04000086 RID: 134
		private static readonly int[] Escaped;

		// Token: 0x0200023C RID: 572
		private class XrefSeries
		{
			// Token: 0x170003EA RID: 1002
			// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00040207 File Offset: 0x0003E407
			public long First { get; }

			// Token: 0x170003EB RID: 1003
			// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0004020F File Offset: 0x0003E40F
			public IReadOnlyList<TokenWriter.XrefSeries.OffsetAndGeneration> Offsets { get; }

			// Token: 0x06000D2B RID: 3371 RVA: 0x00040217 File Offset: 0x0003E417
			public XrefSeries(long first, IReadOnlyList<TokenWriter.XrefSeries.OffsetAndGeneration> offsets)
			{
				this.First = first;
				this.Offsets = offsets;
			}

			// Token: 0x020002E0 RID: 736
			public class OffsetAndGeneration
			{
				// Token: 0x1700042B RID: 1067
				// (get) Token: 0x06000F6E RID: 3950 RVA: 0x00044F8B File Offset: 0x0004318B
				public long Offset { get; }

				// Token: 0x1700042C RID: 1068
				// (get) Token: 0x06000F6F RID: 3951 RVA: 0x00044F93 File Offset: 0x00043193
				public long Generation { get; }

				// Token: 0x06000F70 RID: 3952 RVA: 0x00044F9B File Offset: 0x0004319B
				public OffsetAndGeneration(long offset, long generation)
				{
					this.Offset = offset;
					this.Generation = generation;
				}
			}
		}
	}
}
