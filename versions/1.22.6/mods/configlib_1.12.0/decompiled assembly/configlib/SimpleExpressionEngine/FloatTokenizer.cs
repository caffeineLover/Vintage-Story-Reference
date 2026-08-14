using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleExpressionEngine
{
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	public class FloatTokenizer : ITokenizer<float>
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0000F2A8 File Offset: 0x0000D4A8
		public FloatTokenizer(TextReader reader)
		{
			this.mReader = reader;
			this.NextChar();
			this.NextToken();
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000F2CE File Offset: 0x0000D4CE
		public Token Token
		{
			get
			{
				return this.mCurrentToken;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000F2D6 File Offset: 0x0000D4D6
		public float Value
		{
			get
			{
				return this.mNumber;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000F2DE File Offset: 0x0000D4DE
		public string Identifier
		{
			get
			{
				return this.mIdentifier;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F2E6 File Offset: 0x0000D4E6
		public void NextToken()
		{
			while (char.IsWhiteSpace(this.mCurrentCharacter))
			{
				this.NextChar();
			}
			if (this.ProcessSpecialSymbol())
			{
				return;
			}
			if (this.ProcessDigit())
			{
				return;
			}
			this.ProcessIdentifier();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F318 File Offset: 0x0000D518
		private void NextChar()
		{
			int code = this.mReader.Read();
			char character = (code < 0) ? '\0' : ((char)code);
			if (!char.IsWhiteSpace(character))
			{
				if (!char.IsWhiteSpace(this.mPreviousCharacter))
				{
					this.mBeforePreviousCharacter = this.mPreviousCharacter;
				}
				if (!char.IsWhiteSpace(this.mCurrentCharacter))
				{
					this.mPreviousCharacter = this.mCurrentCharacter;
				}
			}
			this.mCurrentCharacter = character;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F37C File Offset: 0x0000D57C
		private bool ProcessSpecialSymbol()
		{
			char c = this.mCurrentCharacter;
			if (c != '\0')
			{
				switch (c)
				{
				case '(':
					this.NextChar();
					this.mCurrentToken = Token.OpenParenthesis;
					return true;
				case ')':
					this.NextChar();
					this.mCurrentToken = Token.CloseParenthesis;
					return true;
				case '*':
					this.NextChar();
					this.mCurrentToken = Token.Multiply;
					return true;
				case '+':
					if (this.mPreviousCharacter == 'E' && char.IsDigit(this.mBeforePreviousCharacter))
					{
						return false;
					}
					this.NextChar();
					this.mCurrentToken = Token.Add;
					return true;
				case ',':
					this.NextChar();
					this.mCurrentToken = Token.Comma;
					return true;
				case '-':
					if (this.mPreviousCharacter == 'E' && char.IsDigit(this.mBeforePreviousCharacter))
					{
						return false;
					}
					this.NextChar();
					this.mCurrentToken = Token.Subtract;
					return true;
				case '/':
					this.NextChar();
					this.mCurrentToken = Token.Divide;
					return true;
				}
				return false;
			}
			this.mCurrentToken = Token.EOF;
			return true;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F468 File Offset: 0x0000D668
		private bool ProcessDigit()
		{
			if ((!char.IsDigit(this.mCurrentCharacter) && this.mCurrentCharacter != '.') || (!char.IsDigit(this.mPreviousCharacter) && this.mCurrentCharacter == 'E') || (this.mCurrentCharacter == '-' && this.mPreviousCharacter != 'E') || (this.mCurrentCharacter == '+' && this.mPreviousCharacter != 'E'))
			{
				return false;
			}
			StringBuilder numberString = new StringBuilder();
			bool haveDecimalPoint = false;
			while (char.IsDigit(this.mCurrentCharacter) || (!haveDecimalPoint && this.mCurrentCharacter == '.') || this.mCurrentCharacter == 'E' || (this.mPreviousCharacter == 'E' && this.mCurrentCharacter == '-') || (this.mPreviousCharacter == 'E' && this.mCurrentCharacter == '+'))
			{
				numberString.Append(this.mCurrentCharacter);
				haveDecimalPoint = (this.mCurrentCharacter == '.');
				this.NextChar();
			}
			this.mNumber = float.Parse(numberString.ToString(), CultureInfo.InvariantCulture);
			this.mCurrentToken = Token.Number;
			return true;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F560 File Offset: 0x0000D760
		private bool ProcessIdentifier()
		{
			if (!char.IsLetter(this.mCurrentCharacter) && this.mCurrentCharacter != '_')
			{
				return false;
			}
			StringBuilder identifier = new StringBuilder();
			while (char.IsLetterOrDigit(this.mCurrentCharacter) || this.mCurrentCharacter == '_')
			{
				identifier.Append(this.mCurrentCharacter);
				this.NextChar();
			}
			this.mIdentifier = identifier.ToString();
			this.mCurrentToken = Token.Identifier;
			return true;
		}

		// Token: 0x04000171 RID: 369
		private readonly TextReader mReader;

		// Token: 0x04000172 RID: 370
		private char mCurrentCharacter;

		// Token: 0x04000173 RID: 371
		private char mPreviousCharacter;

		// Token: 0x04000174 RID: 372
		private char mBeforePreviousCharacter;

		// Token: 0x04000175 RID: 373
		private Token mCurrentToken;

		// Token: 0x04000176 RID: 374
		private float mNumber;

		// Token: 0x04000177 RID: 375
		private string mIdentifier = "";
	}
}
