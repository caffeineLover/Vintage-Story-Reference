using System;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000003 RID: 3
	internal class CommentTokenizer : ITokenizer
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020DB File Offset: 0x000002DB
		public bool ReadsNextByte { get; } = 1;

		// Token: 0x06000006 RID: 6 RVA: 0x000020E4 File Offset: 0x000002E4
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 37)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (inputBytes.MoveNext() && !ReadHelper.IsEndOfLine(inputBytes.CurrentByte))
			{
				stringBuilder.Append((char)inputBytes.CurrentByte);
			}
			token = new CommentToken(stringBuilder.ToString());
			return true;
		}
	}
}
