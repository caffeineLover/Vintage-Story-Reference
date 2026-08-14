using System;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x0200000B RID: 11
	internal class PlainTokenizer : ITokenizer
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002E79 File Offset: 0x00001079
		public bool ReadsNextByte { get; } = 1;

		// Token: 0x06000023 RID: 35 RVA: 0x00002E84 File Offset: 0x00001084
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (ReadHelper.IsWhitespace(currentByte))
			{
				return false;
			}
			StringBuilder stringBuilder = this.stringBuilder;
			stringBuilder.Append((char)currentByte);
			while (inputBytes.MoveNext() && !ReadHelper.IsWhitespace(inputBytes.CurrentByte) && inputBytes.CurrentByte != 60 && inputBytes.CurrentByte != 91 && inputBytes.CurrentByte != 47 && inputBytes.CurrentByte != 93 && inputBytes.CurrentByte != 62 && inputBytes.CurrentByte != 40 && inputBytes.CurrentByte != 41)
			{
				stringBuilder.Append((char)inputBytes.CurrentByte);
			}
			string text = stringBuilder.ToString();
			stringBuilder.Clear();
			if (!(text == "true"))
			{
				if (!(text == "false"))
				{
					if (!(text == "null"))
					{
						token = OperatorToken.Create(text);
					}
					else
					{
						token = NullToken.Instance;
					}
				}
				else
				{
					token = BooleanToken.False;
				}
			}
			else
			{
				token = BooleanToken.True;
			}
			return true;
		}

		// Token: 0x04000010 RID: 16
		private readonly StringBuilder stringBuilder = new StringBuilder();
	}
}
