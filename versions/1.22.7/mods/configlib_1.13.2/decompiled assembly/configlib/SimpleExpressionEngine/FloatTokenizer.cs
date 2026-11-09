using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleExpressionEngine
{
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	public class FloatTokenizer : ITokenizer<float>
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x0000E960 File Offset: 0x0000CB60
		public FloatTokenizer(TextReader reader)
		{
			this.mReader = reader;
			this.NextChar();
			this.NextToken();
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000E986 File Offset: 0x0000CB86
		public Token Token
		{
			get
			{
				return this.mCurrentToken;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000E98E File Offset: 0x0000CB8E
		public float Value
		{
			get
			{
				return this.mNumber;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000E996 File Offset: 0x0000CB96
		public string Identifier
		{
			get
			{
				return this.mIdentifier;
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000E99E File Offset: 0x0000CB9E
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

		// Token: 0x060002A6 RID: 678 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
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

		// Token: 0x060002A7 RID: 679 RVA: 0x0000EA34 File Offset: 0x0000CC34
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

		// Token: 0x060002A8 RID: 680 RVA: 0x0000EB20 File Offset: 0x0000CD20
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

		// Token: 0x060002A9 RID: 681 RVA: 0x0000EC18 File Offset: 0x0000CE18
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

		// Token: 0x04000143 RID: 323
		private readonly TextReader mReader;

		// Token: 0x04000144 RID: 324
		private char mCurrentCharacter;

		// Token: 0x04000145 RID: 325
		private char mPreviousCharacter;

		// Token: 0x04000146 RID: 326
		private char mBeforePreviousCharacter;

		// Token: 0x04000147 RID: 327
		private Token mCurrentToken;

		// Token: 0x04000148 RID: 328
		private float mNumber;

		// Token: 0x04000149 RID: 329
		private string mIdentifier = "";
	}
}
