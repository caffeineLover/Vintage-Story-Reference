using System;
using System.Text;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B5 RID: 437
	internal sealed class Parser
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x00034DCD File Offset: 0x00032FCD
		private Parser()
		{
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00034DD5 File Offset: 0x00032FD5
		public static void Parse(string input, Parser.SyntaxHandler handler)
		{
			new Parser.Tokenizer(input, handler).Tokenize();
		}

		// Token: 0x020002A0 RID: 672
		internal enum State
		{
			// Token: 0x040008BE RID: 2238
			NEWLINE,
			// Token: 0x040008BF RID: 2239
			WHITESPACE,
			// Token: 0x040008C0 RID: 2240
			COMMENT,
			// Token: 0x040008C1 RID: 2241
			TOKEN
		}

		// Token: 0x020002A1 RID: 673
		public interface SyntaxHandler
		{
			// Token: 0x06000EBD RID: 3773
			void NewLine(string text);

			// Token: 0x06000EBE RID: 3774
			void Whitespace(string text);

			// Token: 0x06000EBF RID: 3775
			void Token(string text);

			// Token: 0x06000EC0 RID: 3776
			void Comment(string text);
		}

		// Token: 0x020002A2 RID: 674
		public abstract class AbstractSyntaxHandler : Parser.SyntaxHandler
		{
			// Token: 0x06000EC1 RID: 3777 RVA: 0x00043358 File Offset: 0x00041558
			public void Comment(string text)
			{
			}

			// Token: 0x06000EC2 RID: 3778 RVA: 0x0004335A File Offset: 0x0004155A
			public void NewLine(string text)
			{
			}

			// Token: 0x06000EC3 RID: 3779 RVA: 0x0004335C File Offset: 0x0004155C
			public void Whitespace(string text)
			{
			}

			// Token: 0x06000EC4 RID: 3780
			public abstract void Token(string text);
		}

		// Token: 0x020002A3 RID: 675
		internal class Tokenizer
		{
			// Token: 0x06000EC6 RID: 3782 RVA: 0x00043366 File Offset: 0x00041566
			internal Tokenizer(string text, Parser.SyntaxHandler syntaxHandler)
			{
				this.input = text;
				this.handler = syntaxHandler;
			}

			// Token: 0x06000EC7 RID: 3783 RVA: 0x0004338E File Offset: 0x0004158E
			private bool HasMore()
			{
				return this.index < this.input.Length;
			}

			// Token: 0x06000EC8 RID: 3784 RVA: 0x000433A3 File Offset: 0x000415A3
			private char CurrentChar()
			{
				return this.input.get_Chars(this.index);
			}

			// Token: 0x06000EC9 RID: 3785 RVA: 0x000433B6 File Offset: 0x000415B6
			private char NextChar()
			{
				this.index++;
				if (!this.HasMore())
				{
					return '\u0004';
				}
				return this.CurrentChar();
			}

			// Token: 0x06000ECA RID: 3786 RVA: 0x000433D6 File Offset: 0x000415D6
			private char Peek()
			{
				if (this.index < this.input.Length - 1)
				{
					return this.input.get_Chars(this.index + 1);
				}
				return '\u0004';
			}

			// Token: 0x06000ECB RID: 3787 RVA: 0x00043404 File Offset: 0x00041604
			private Parser.State NextState()
			{
				char c = this.CurrentChar();
				if (c <= '\r')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\f':
						case '\r':
							this.state = Parser.State.NEWLINE;
							goto IL_5C;
						case '\v':
							goto IL_55;
						default:
							goto IL_55;
						}
					}
				}
				else if (c != ' ')
				{
					if (c != '%')
					{
						goto IL_55;
					}
					this.state = Parser.State.COMMENT;
					goto IL_5C;
				}
				this.state = Parser.State.WHITESPACE;
				goto IL_5C;
				IL_55:
				this.state = Parser.State.TOKEN;
				IL_5C:
				return this.state;
			}

			// Token: 0x06000ECC RID: 3788 RVA: 0x00043474 File Offset: 0x00041674
			internal void Tokenize()
			{
				while (this.HasMore())
				{
					this.buffer.Length = 0;
					this.NextState();
					switch (this.state)
					{
					case Parser.State.NEWLINE:
						this.ScanNewLine();
						break;
					case Parser.State.WHITESPACE:
						this.ScanWhitespace();
						break;
					case Parser.State.COMMENT:
						this.ScanComment();
						break;
					default:
						this.ScanToken();
						break;
					}
				}
			}

			// Token: 0x06000ECD RID: 3789 RVA: 0x000434D8 File Offset: 0x000416D8
			private void ScanNewLine()
			{
				char c = this.CurrentChar();
				this.buffer.Append(c);
				if (c == '\r' && this.Peek() == '\n')
				{
					this.buffer.Append(this.NextChar());
				}
				this.handler.NewLine(this.buffer.ToString());
				this.NextChar();
			}

			// Token: 0x06000ECE RID: 3790 RVA: 0x00043538 File Offset: 0x00041738
			private void ScanWhitespace()
			{
				this.buffer.Append(this.CurrentChar());
				bool flag = true;
				while (this.HasMore() && flag)
				{
					char c = this.NextChar();
					if (c == '\0' || c == '\t' || c == ' ')
					{
						this.buffer.Append(c);
					}
					else
					{
						flag = false;
					}
				}
				this.handler.Whitespace(this.buffer.ToString());
			}

			// Token: 0x06000ECF RID: 3791 RVA: 0x000435A0 File Offset: 0x000417A0
			private void ScanComment()
			{
				this.buffer.Append(this.CurrentChar());
				bool flag = true;
				while (this.HasMore() && flag)
				{
					char c = this.NextChar();
					switch (c)
					{
					case '\n':
					case '\f':
					case '\r':
						flag = false;
						continue;
					}
					this.buffer.Append(c);
				}
				this.handler.Comment(this.buffer.ToString());
			}

			// Token: 0x06000ED0 RID: 3792 RVA: 0x00043618 File Offset: 0x00041818
			private void ScanToken()
			{
				char c = this.CurrentChar();
				this.buffer.Append(c);
				if (c == '{' || c == '}')
				{
					this.handler.Token(this.buffer.ToString());
					this.NextChar();
					return;
				}
				bool flag = true;
				while (this.HasMore() && flag)
				{
					c = this.NextChar();
					if (c <= '\r')
					{
						if (c != '\0' && c != '\u0004')
						{
							switch (c)
							{
							case '\t':
							case '\n':
							case '\f':
							case '\r':
								break;
							case '\v':
								goto IL_85;
							default:
								goto IL_85;
							}
						}
					}
					else if (c != ' ' && c != '{' && c != '}')
					{
						goto IL_85;
					}
					flag = false;
					continue;
					IL_85:
					this.buffer.Append(c);
				}
				this.handler.Token(this.buffer.ToString());
			}

			// Token: 0x040008C2 RID: 2242
			private const char NUL = '\0';

			// Token: 0x040008C3 RID: 2243
			private const char EOT = '\u0004';

			// Token: 0x040008C4 RID: 2244
			private const char TAB = '\t';

			// Token: 0x040008C5 RID: 2245
			private const char FF = '\f';

			// Token: 0x040008C6 RID: 2246
			private const char CR = '\r';

			// Token: 0x040008C7 RID: 2247
			private const char LF = '\n';

			// Token: 0x040008C8 RID: 2248
			private const char SPACE = ' ';

			// Token: 0x040008C9 RID: 2249
			private readonly string input;

			// Token: 0x040008CA RID: 2250
			private int index;

			// Token: 0x040008CB RID: 2251
			private readonly Parser.SyntaxHandler handler;

			// Token: 0x040008CC RID: 2252
			private Parser.State state = Parser.State.WHITESPACE;

			// Token: 0x040008CD RID: 2253
			private readonly StringBuilder buffer = new StringBuilder();
		}
	}
}
