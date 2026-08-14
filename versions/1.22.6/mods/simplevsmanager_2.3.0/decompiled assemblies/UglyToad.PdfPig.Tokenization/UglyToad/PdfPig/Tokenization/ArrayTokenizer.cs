using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000002 RID: 2
	internal class ArrayTokenizer : ITokenizer
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public bool ReadsNextByte { get; }

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (currentByte != 91)
			{
				return false;
			}
			CoreTokenScanner coreTokenScanner = new CoreTokenScanner(inputBytes, ScannerScope.Array, null);
			List<IToken> list = new List<IToken>();
			IToken previousToken = null;
			while (!ArrayTokenizer.CurrentByteEndsCurrentArray(inputBytes, previousToken) && coreTokenScanner.MoveNext())
			{
				previousToken = coreTokenScanner.CurrentToken;
				if (!(coreTokenScanner.CurrentToken is CommentToken))
				{
					list.Add(coreTokenScanner.CurrentToken);
				}
			}
			token = new ArrayToken(list);
			return true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		private static bool CurrentByteEndsCurrentArray(IInputBytes inputBytes, IToken previousToken)
		{
			return inputBytes.CurrentByte == 93 && !(previousToken is ArrayToken);
		}
	}
}
