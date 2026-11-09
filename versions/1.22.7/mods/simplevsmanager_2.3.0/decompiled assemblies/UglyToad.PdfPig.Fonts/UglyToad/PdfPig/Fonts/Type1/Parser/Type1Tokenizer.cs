using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x02000012 RID: 18
	internal class Type1Tokenizer
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00004EA4 File Offset: 0x000030A4
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00004EAC File Offset: 0x000030AC
		public Type1Token CurrentToken { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00004EB5 File Offset: 0x000030B5
		public IReadOnlyList<string> Comments
		{
			get
			{
				return this.comments;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004EC0 File Offset: 0x000030C0
		public Type1Tokenizer(IInputBytes bytes)
		{
			this.bytes = bytes;
			this.comments = new List<string>();
			this.CurrentToken = this.ReadNextToken();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004F12 File Offset: 0x00003112
		public Type1Token GetNext()
		{
			this.CurrentToken = this.ReadNextToken();
			return this.CurrentToken;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004F28 File Offset: 0x00003128
		private Type1Token ReadNextToken()
		{
			this.previousToken = this.CurrentToken;
			char c;
			for (;;)
			{
				bool flag = false;
				while (this.bytes.MoveNext())
				{
					byte currentByte = this.bytes.CurrentByte;
					c = (char)currentByte;
					if (c <= '>')
					{
						if (c <= '/')
						{
							switch (c)
							{
							case '%':
								this.comments.Add(this.ReadComment());
								continue;
							case '&':
							case '\'':
								break;
							case '(':
								goto IL_A3;
							case ')':
								goto IL_AA;
							default:
								if (c == '/')
								{
									goto IL_D6;
								}
								break;
							}
						}
						else
						{
							if (c == '<')
							{
								goto IL_EC;
							}
							if (c == '>')
							{
								goto IL_156;
							}
						}
					}
					else if (c <= ']')
					{
						if (c == '[')
						{
							goto IL_B5;
						}
						if (c == ']')
						{
							goto IL_BD;
						}
					}
					else
					{
						if (c == '{')
						{
							goto IL_C5;
						}
						if (c == '}')
						{
							goto IL_CD;
						}
					}
					if (ReadHelper.IsWhitespace(currentByte))
					{
						flag = true;
					}
					else
					{
						if (currentByte != 0)
						{
							goto IL_1D9;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					goto Block_23;
				}
			}
			IL_A3:
			return this.ReadString();
			IL_AA:
			throw new InvalidOperationException("Encountered an end of string ')' outside of string.");
			IL_B5:
			return new Type1Token(c, Type1Token.TokenType.StartArray);
			IL_BD:
			return new Type1Token(c, Type1Token.TokenType.EndArray);
			IL_C5:
			return new Type1Token(c, Type1Token.TokenType.StartProc);
			IL_CD:
			return new Type1Token(c, Type1Token.TokenType.EndProc);
			IL_D6:
			return new Type1Token(this.ReadLiteral(default(char?)), Type1Token.TokenType.Literal);
			IL_EC:
			byte? b = this.bytes.Peek();
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			int num2 = 60;
			if (num.GetValueOrDefault() == num2 & num != null)
			{
				this.bytes.MoveNext();
				return new Type1Token("<<", Type1Token.TokenType.StartDict);
			}
			return new Type1Token(c, Type1Token.TokenType.Name);
			IL_156:
			b = this.bytes.Peek();
			num = ((b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?));
			num2 = 62;
			if (num.GetValueOrDefault() == num2 & num != null)
			{
				this.bytes.MoveNext();
				return new Type1Token(">>", Type1Token.TokenType.EndDict);
			}
			return new Type1Token(c, Type1Token.TokenType.Name);
			IL_1D9:
			Type1Token result;
			if (this.TryReadNumber(c, out result))
			{
				return result;
			}
			string text = this.ReadLiteral(new char?(c));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (text == null)
			{
				defaultInterpolatedStringHandler..ctor(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The binary portion of the type 1 font was invalid at position ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this.bytes.CurrentOffset);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (!text.Equals("RD", 5) && !text.Equals("-|"))
			{
				return new Type1Token(text, Type1Token.TokenType.Name);
			}
			if (this.previousToken.Type == Type1Token.TokenType.Integer)
			{
				return this.ReadCharString(this.previousToken.AsInt());
			}
			defaultInterpolatedStringHandler..ctor(42, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Expected integer token before ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" at offset ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.bytes.CurrentOffset);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			Block_23:
			return null;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005224 File Offset: 0x00003424
		private Type1Token ReadString()
		{
			this.stringBuffer.Clear();
			while (this.bytes.MoveNext())
			{
				char currentByte = (char)this.bytes.CurrentByte;
				if (currentByte <= '\r')
				{
					if (currentByte == '\n' || currentByte == '\r')
					{
						this.stringBuffer.Append("\n");
						continue;
					}
				}
				else
				{
					if (currentByte == '(')
					{
						this.openParens++;
						this.stringBuffer.Append('(');
						continue;
					}
					if (currentByte != ')')
					{
						if (currentByte == '\\')
						{
							char c = this.<ReadString>g__GetNext|16_0();
							if (c <= 'b')
							{
								if (c <= ')')
								{
									if (c != '(')
									{
										if (c == ')')
										{
											this.stringBuffer.Append(')');
										}
									}
									else
									{
										this.stringBuffer.Append('(');
									}
								}
								else if (c != '\\')
								{
									if (c == 'b')
									{
										this.stringBuffer.Append('\b');
									}
								}
								else
								{
									this.stringBuffer.Append('\\');
								}
							}
							else
							{
								if (c <= 'n')
								{
									if (c == 'f')
									{
										this.stringBuffer.Append('\f');
										goto IL_169;
									}
									if (c != 'n')
									{
										goto IL_169;
									}
								}
								else if (c != 'r')
								{
									if (c != 't')
									{
										goto IL_169;
									}
									this.stringBuffer.Append('\t');
									goto IL_169;
								}
								this.stringBuffer.Append("\n");
							}
							IL_169:
							if (char.IsDigit(c))
							{
								int num = Convert.ToInt32(new string(new char[]
								{
									c,
									this.<ReadString>g__GetNext|16_0(),
									this.<ReadString>g__GetNext|16_0()
								}), 8);
								this.stringBuffer.Append((char)num);
								continue;
							}
							continue;
						}
					}
					else
					{
						if (this.openParens == 0)
						{
							return new Type1Token(this.stringBuffer.ToString(), Type1Token.TokenType.String);
						}
						this.stringBuffer.Append(')');
						this.openParens--;
						continue;
					}
				}
				this.stringBuffer.Append(currentByte);
			}
			return null;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000540C File Offset: 0x0000360C
		private bool TryReadNumber(char c, out Type1Token numberToken)
		{
			numberToken = null;
			long currentOffset = this.bytes.CurrentOffset;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = null;
			bool flag = false;
			if (c == '+' || c == '-')
			{
				stringBuilder.Append(c);
				c = this.<TryReadNumber>g__GetNext|17_0();
			}
			while (char.IsDigit(c))
			{
				stringBuilder.Append(c);
				c = this.<TryReadNumber>g__GetNext|17_0();
				flag = true;
			}
			if (c == '.')
			{
				stringBuilder.Append(c);
				c = this.<TryReadNumber>g__GetNext|17_0();
			}
			else if (c == '#')
			{
				stringBuilder2 = stringBuilder;
				stringBuilder = new StringBuilder();
				c = this.<TryReadNumber>g__GetNext|17_0();
			}
			else
			{
				if (stringBuilder.Length == 0 || !flag)
				{
					this.bytes.Seek(currentOffset);
					return false;
				}
				this.bytes.Seek(this.bytes.CurrentOffset - 1L);
				numberToken = new Type1Token(stringBuilder.ToString(), Type1Token.TokenType.Integer);
				return true;
			}
			if (char.IsDigit(c))
			{
				stringBuilder.Append(c);
				c = this.<TryReadNumber>g__GetNext|17_0();
				while (char.IsDigit(c))
				{
					stringBuilder.Append(c);
					c = this.<TryReadNumber>g__GetNext|17_0();
				}
				if (c == 'E')
				{
					stringBuilder.Append(c);
					c = this.<TryReadNumber>g__GetNext|17_0();
					if (c == '-')
					{
						stringBuilder.Append(c);
						c = this.<TryReadNumber>g__GetNext|17_0();
					}
					if (!char.IsDigit(c))
					{
						this.bytes.Seek(currentOffset);
						return false;
					}
					stringBuilder.Append(c);
					c = this.<TryReadNumber>g__GetNext|17_0();
					while (char.IsDigit(c))
					{
						stringBuilder.Append(c);
						c = this.<TryReadNumber>g__GetNext|17_0();
					}
				}
				this.bytes.Seek(this.bytes.CurrentOffset - 1L);
				if (stringBuilder2 != null)
				{
					numberToken = new Type1Token(Convert.ToInt32(stringBuilder.ToString(), int.Parse(stringBuilder2.ToString(), CultureInfo.InvariantCulture)).ToString(), Type1Token.TokenType.Integer);
				}
				else
				{
					numberToken = new Type1Token(stringBuilder.ToString(), Type1Token.TokenType.Real);
				}
				return true;
			}
			this.bytes.Seek(currentOffset);
			return false;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000055DC File Offset: 0x000037DC
		private string ReadLiteral(char? previousCharacter = null)
		{
			this.literalBuffer.Clear();
			if (previousCharacter != null)
			{
				this.literalBuffer.Append(previousCharacter);
			}
			do
			{
				byte? b = this.bytes.Peek();
				if (b == null)
				{
					break;
				}
				char value = (char)b.Value;
				if (char.IsWhiteSpace(value) || value == '(' || value == ')' || value == '<' || value == '>' || value == '[' || value == ']' || value == '{' || value == '}' || value == '/' || value == '%')
				{
					break;
				}
				this.literalBuffer.Append(value);
			}
			while (this.bytes.MoveNext());
			string text = this.literalBuffer.ToString();
			if (text.Length != 0)
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005698 File Offset: 0x00003898
		private string ReadComment()
		{
			this.commentBuffer.Clear();
			while (this.bytes.MoveNext())
			{
				char currentByte = (char)this.bytes.CurrentByte;
				if (ReadHelper.IsEndOfLine(currentByte))
				{
					break;
				}
				this.commentBuffer.Append(currentByte);
			}
			return this.commentBuffer.ToString();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000056EC File Offset: 0x000038EC
		private Type1DataToken ReadCharString(int length)
		{
			this.bytes.MoveNext();
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				this.bytes.MoveNext();
				array[i] = this.bytes.CurrentByte;
			}
			return new Type1DataToken(Type1Token.TokenType.Charstring, array);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000573A File Offset: 0x0000393A
		[CompilerGenerated]
		private char <ReadString>g__GetNext|16_0()
		{
			this.bytes.MoveNext();
			return (char)this.bytes.CurrentByte;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005753 File Offset: 0x00003953
		[CompilerGenerated]
		private char <TryReadNumber>g__GetNext|17_0()
		{
			this.bytes.MoveNext();
			return (char)this.bytes.CurrentByte;
		}

		// Token: 0x04000054 RID: 84
		private readonly StringBuilder commentBuffer = new StringBuilder();

		// Token: 0x04000055 RID: 85
		private readonly StringBuilder literalBuffer = new StringBuilder();

		// Token: 0x04000056 RID: 86
		private readonly StringBuilder stringBuffer = new StringBuilder();

		// Token: 0x04000057 RID: 87
		private readonly IInputBytes bytes;

		// Token: 0x04000058 RID: 88
		private readonly List<string> comments;

		// Token: 0x04000059 RID: 89
		private int openParens;

		// Token: 0x0400005A RID: 90
		private Type1Token previousToken;
	}
}
