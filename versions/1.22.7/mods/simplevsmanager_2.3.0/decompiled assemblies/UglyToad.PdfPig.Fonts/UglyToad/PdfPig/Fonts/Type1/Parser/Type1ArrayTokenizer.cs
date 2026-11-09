using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x0200000B RID: 11
	public class Type1ArrayTokenizer : ITokenizer
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002BAC File Offset: 0x00000DAC
		public bool ReadsNextByte { get; }

		// Token: 0x06000046 RID: 70 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 123)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (inputBytes.MoveNext() && inputBytes.CurrentByte != 125)
			{
				stringBuilder.Append((char)inputBytes.CurrentByte);
			}
			string[] array = stringBuilder.ToString().Split(new string[]
			{
				" "
			}, 1);
			List<IToken> list = new List<IToken>();
			foreach (string text in array)
			{
				if (char.IsNumber(text.get_Chars(0)) || text.get_Chars(0) == '-')
				{
					decimal num;
					if (decimal.TryParse(text, 4, CultureInfo.InvariantCulture, ref num))
					{
						list.Add(new NumericToken(num));
					}
					else
					{
						list.Add(OperatorToken.Create(text));
					}
				}
				else if (text.get_Chars(0) == '/')
				{
					list.Add(NameToken.Create(text.Substring(1)));
				}
				else if (text.get_Chars(0) == '(' && text.get_Chars(text.Length - 1) == ')')
				{
					list.Add(new StringToken(text, 0));
				}
				else
				{
					list.Add(OperatorToken.Create(text));
				}
			}
			token = new ArrayToken(list);
			return true;
		}
	}
}
