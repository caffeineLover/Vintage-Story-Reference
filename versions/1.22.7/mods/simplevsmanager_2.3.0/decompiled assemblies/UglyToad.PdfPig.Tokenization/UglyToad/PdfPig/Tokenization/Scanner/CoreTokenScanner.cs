using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x0200000E RID: 14
	public class CoreTokenScanner : ISeekableTokenScanner, ITokenScanner
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000340B File Offset: 0x0000160B
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00003413 File Offset: 0x00001613
		public long CurrentTokenStart { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000341C File Offset: 0x0000161C
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003424 File Offset: 0x00001624
		public IToken CurrentToken { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000032 RID: 50 RVA: 0x0000342D File Offset: 0x0000162D
		public long CurrentPosition
		{
			get
			{
				return this.inputBytes.CurrentOffset;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000343A File Offset: 0x0000163A
		public long Length
		{
			get
			{
				return this.inputBytes.Length;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003448 File Offset: 0x00001648
		public CoreTokenScanner(IInputBytes inputBytes, ScannerScope scope = ScannerScope.None, IReadOnlyDictionary<NameToken, IReadOnlyList<NameToken>> namedDictionaryRequiredKeys = null)
		{
			if (inputBytes == null)
			{
				throw new ArgumentNullException("inputBytes");
			}
			this.inputBytes = inputBytes;
			this.scope = scope;
			this.namedDictionaryRequiredKeys = namedDictionaryRequiredKeys;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000034AC File Offset: 0x000016AC
		public bool TryReadToken<T>(out T token) where T : class, IToken
		{
			token = default(T);
			if (!this.MoveNext())
			{
				return false;
			}
			T t = this.CurrentToken as T;
			if (t != null)
			{
				token = t;
				return true;
			}
			return false;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000034ED File Offset: 0x000016ED
		public void Seek(long position)
		{
			this.inputBytes.Seek(position);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000034FC File Offset: 0x000016FC
		public bool MoveNext()
		{
			int num = 0;
			bool flag = false;
			while ((this.hasBytePreRead && !this.inputBytes.IsAtEnd()) || this.inputBytes.MoveNext())
			{
				this.hasBytePreRead = false;
				byte currentByte = this.inputBytes.CurrentByte;
				char c = (char)currentByte;
				ITokenizer tokenizer = null;
				foreach (ValueTuple<byte, ITokenizer> valueTuple in this.customTokenizers)
				{
					if (currentByte == valueTuple.Item1)
					{
						tokenizer = valueTuple.Item2;
						break;
					}
				}
				if (tokenizer == null)
				{
					if (ReadHelper.IsWhitespace(currentByte))
					{
						flag = false;
						continue;
					}
					if (flag && c != '>')
					{
						continue;
					}
					char c2 = c;
					switch (c2)
					{
					case '%':
						tokenizer = CoreTokenScanner.CommentTokenizer;
						goto IL_1FF;
					case '&':
					case '\'':
					case ')':
					case '*':
					case ',':
					case ':':
					case ';':
					case '=':
						break;
					case '(':
						tokenizer = this.StringTokenizer;
						goto IL_1FF;
					case '+':
					case '-':
					case '.':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						tokenizer = this.NumericTokenizer;
						goto IL_1FF;
					case '/':
						tokenizer = CoreTokenScanner.NameTokenizer;
						goto IL_1FF;
					case '<':
					{
						byte? b = this.inputBytes.Peek();
						int? num2 = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
						int num3 = 60;
						if (!(num2.GetValueOrDefault() == num3 & num2 != null))
						{
							tokenizer = CoreTokenScanner.HexTokenizer;
							goto IL_1FF;
						}
						flag = true;
						tokenizer = CoreTokenScanner.DictionaryTokenizer;
						if (this.namedDictionaryRequiredKeys == null)
						{
							goto IL_1FF;
						}
						NameToken nameToken = this.CurrentToken as NameToken;
						IReadOnlyList<NameToken> requiredKeys;
						if (nameToken != null && this.namedDictionaryRequiredKeys.TryGetValue(nameToken, ref requiredKeys))
						{
							tokenizer = new DictionaryTokenizer(requiredKeys);
							goto IL_1FF;
						}
						goto IL_1FF;
					}
					case '>':
						if (this.scope == ScannerScope.Dictionary)
						{
							num++;
							if (num == 2)
							{
								return false;
							}
							goto IL_1FF;
						}
						break;
					default:
						if (c2 == '[')
						{
							tokenizer = CoreTokenScanner.ArrayTokenizer;
							goto IL_1FF;
						}
						if (c2 == ']')
						{
							if (this.scope == ScannerScope.Array)
							{
								return false;
							}
						}
						break;
					}
					tokenizer = this.PlainTokenizer;
				}
				IL_1FF:
				this.CurrentTokenStart = this.inputBytes.CurrentOffset - 1L;
				IToken token;
				if (tokenizer != null && tokenizer.TryTokenize(currentByte, this.inputBytes, out token))
				{
					OperatorToken operatorToken = token as OperatorToken;
					if (operatorToken != null)
					{
						if (operatorToken.Data == "BI")
						{
							this.isInInlineImage = true;
						}
						else if (this.isInInlineImage && operatorToken.Data == "ID")
						{
							IReadOnlyList<byte> readOnlyList = this.ReadInlineImageData();
							this.isInInlineImage = false;
							this.CurrentToken = new InlineImageDataToken(readOnlyList);
							this.hasBytePreRead = false;
							return true;
						}
					}
					this.CurrentToken = token;
					this.hasBytePreRead = tokenizer.ReadsNextByte;
					return true;
				}
				flag = true;
				this.hasBytePreRead = false;
			}
			return false;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000037FC File Offset: 0x000019FC
		public void RegisterCustomTokenizer(byte firstByte, ITokenizer tokenizer)
		{
			if (tokenizer == null)
			{
				throw new ArgumentNullException("tokenizer");
			}
			this.customTokenizers.Add(new ValueTuple<byte, ITokenizer>(firstByte, tokenizer));
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003820 File Offset: 0x00001A20
		public void DeregisterCustomTokenizer(ITokenizer tokenizer)
		{
			this.customTokenizers.RemoveAll(([TupleElementNames(new string[]
			{
				"firstByte",
				"tokenizer"
			})] ValueTuple<byte, ITokenizer> x) => x.Item2 == tokenizer);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003854 File Offset: 0x00001A54
		public IReadOnlyList<byte> RecoverFromIncorrectEndImage(long lastEndImageOffset)
		{
			List<byte> list = new List<byte>();
			this.inputBytes.Seek(lastEndImageOffset);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!this.inputBytes.MoveNext() || this.inputBytes.CurrentByte != 69)
			{
				defaultInterpolatedStringHandler..ctor(111, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to recover the image data stream for an inline image at offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(lastEndImageOffset);
				defaultInterpolatedStringHandler.AppendLiteral(". ");
				defaultInterpolatedStringHandler.AppendLiteral("Expected to read byte 'E' instead got ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.inputBytes.CurrentByte);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			list.Add(this.inputBytes.CurrentByte);
			if (!this.inputBytes.MoveNext() || this.inputBytes.CurrentByte != 73)
			{
				defaultInterpolatedStringHandler..ctor(132, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to recover the image data stream for an inline image at offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(lastEndImageOffset);
				defaultInterpolatedStringHandler.AppendLiteral(". ");
				defaultInterpolatedStringHandler.AppendLiteral("Expected to read second byte 'I' following 'E' instead got ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.inputBytes.CurrentByte);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			list.Add(this.inputBytes.CurrentByte);
			list.AddRange(this.ReadUntilEndImage(lastEndImageOffset));
			this.inputBytes.MoveNext();
			return list;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000039AC File Offset: 0x00001BAC
		private IReadOnlyList<byte> ReadInlineImageData()
		{
			if (!ReadHelper.IsWhitespace(this.inputBytes.CurrentByte))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No whitespace character following the image data (ID) operator. Position: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this.inputBytes.CurrentOffset);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			long startsAt = this.inputBytes.CurrentOffset - 2L;
			return this.ReadUntilEndImage(startsAt);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003A24 File Offset: 0x00001C24
		private List<byte> ReadUntilEndImage(long startsAt)
		{
			List<byte> list = new List<byte>();
			byte b = 0;
			while (this.inputBytes.MoveNext())
			{
				if (this.inputBytes.CurrentByte == 73 && b == 69)
				{
					byte[] array = new byte[6];
					long currentOffset = this.inputBytes.CurrentOffset;
					int num = this.inputBytes.Read(array, default(int?));
					bool flag = true;
					if (num == array.Length)
					{
						bool flag2 = false;
						foreach (byte b2 in array)
						{
							if (ReadHelper.IsWhitespace(b2))
							{
								flag2 = true;
							}
							else
							{
								if (b2 > 127)
								{
									flag = false;
									break;
								}
								if (b2 < 32 && b2 != 13 && b2 != 10 && b2 != 9)
								{
									flag = false;
									break;
								}
							}
						}
						if (!flag2)
						{
							flag = false;
						}
					}
					this.inputBytes.Seek(currentOffset);
					if (flag)
					{
						list.RemoveAt(list.Count - 1);
						return list;
					}
				}
				list.Add(this.inputBytes.CurrentByte);
				b = this.inputBytes.CurrentByte;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(71, 1);
			defaultInterpolatedStringHandler.AppendLiteral("No end of inline image data (EI) was found for image data at position ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(startsAt);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x04000017 RID: 23
		private static readonly ArrayTokenizer ArrayTokenizer = new ArrayTokenizer();

		// Token: 0x04000018 RID: 24
		private static readonly CommentTokenizer CommentTokenizer = new CommentTokenizer();

		// Token: 0x04000019 RID: 25
		private static readonly DictionaryTokenizer DictionaryTokenizer = new DictionaryTokenizer(null);

		// Token: 0x0400001A RID: 26
		private static readonly HexTokenizer HexTokenizer = new HexTokenizer();

		// Token: 0x0400001B RID: 27
		private static readonly NameTokenizer NameTokenizer = new NameTokenizer();

		// Token: 0x0400001C RID: 28
		private readonly PlainTokenizer PlainTokenizer = new PlainTokenizer();

		// Token: 0x0400001D RID: 29
		private readonly NumericTokenizer NumericTokenizer = new NumericTokenizer();

		// Token: 0x0400001E RID: 30
		private readonly StringTokenizer StringTokenizer = new StringTokenizer();

		// Token: 0x0400001F RID: 31
		private readonly ScannerScope scope;

		// Token: 0x04000020 RID: 32
		private readonly IReadOnlyDictionary<NameToken, IReadOnlyList<NameToken>> namedDictionaryRequiredKeys;

		// Token: 0x04000021 RID: 33
		private readonly IInputBytes inputBytes;

		// Token: 0x04000022 RID: 34
		[TupleElementNames(new string[]
		{
			"firstByte",
			"tokenizer"
		})]
		private readonly List<ValueTuple<byte, ITokenizer>> customTokenizers = new List<ValueTuple<byte, ITokenizer>>();

		// Token: 0x04000025 RID: 37
		private bool hasBytePreRead;

		// Token: 0x04000026 RID: 38
		private bool isInInlineImage;
	}
}
