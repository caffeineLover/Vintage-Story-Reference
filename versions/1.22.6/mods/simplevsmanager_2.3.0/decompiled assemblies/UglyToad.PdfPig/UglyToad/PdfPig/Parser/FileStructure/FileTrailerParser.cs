using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D5 RID: 213
	internal static class FileTrailerParser
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x00019070 File Offset: 0x00017270
		public static long GetFirstCrossReferenceOffset(IInputBytes bytes, ISeekableTokenScanner scanner, bool isLenientParsing)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (scanner == null)
			{
				throw new ArgumentNullException("scanner");
			}
			long length = bytes.Length;
			int offsetFromEnd = (length < 2048L) ? ((int)length) : 2048;
			long startXrefPosition = FileTrailerParser.GetStartXrefPosition(bytes, offsetFromEnd);
			scanner.Seek(startXrefPosition);
			OperatorToken operatorToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!scanner.TryReadToken<OperatorToken>(ref operatorToken) || operatorToken.Data != "startxref")
			{
				defaultInterpolatedStringHandler..ctor(87, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The start xref position we found was not correct. Found ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(startXrefPosition);
				defaultInterpolatedStringHandler.AppendLiteral(" but it was occupied by token ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NumericToken numericToken = null;
			while (scanner.MoveNext())
			{
				NumericToken numericToken2 = scanner.CurrentToken as NumericToken;
				if (numericToken2 != null)
				{
					numericToken = numericToken2;
					break;
				}
				if (!(scanner.CurrentToken is CommentToken))
				{
					defaultInterpolatedStringHandler..ctor(50, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Found an unexpected token following 'startxref': ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (numericToken == null)
			{
				defaultInterpolatedStringHandler..ctor(81, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find the numeric value following 'startxref'. Searching from position ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(startXrefPosition);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return numericToken.Long;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000191E0 File Offset: 0x000173E0
		private static long GetStartXrefPosition(IInputBytes bytes, int offsetFromEnd)
		{
			List<int> list = new List<int>();
			int num = 0;
			long length = bytes.Length;
			int num2 = 1;
			long num3 = Math.Max(0L, length - (long)(offsetFromEnd * num2));
			do
			{
				num2 *= 2;
				bytes.Seek(num3);
				while (bytes.MoveNext())
				{
					if (bytes.CurrentByte == FileTrailerParser.StartXRefBytes[num])
					{
						num++;
					}
					else
					{
						num = 0;
					}
					if (num == FileTrailerParser.StartXRefBytes.Length)
					{
						list.Add((int)bytes.CurrentOffset - FileTrailerParser.StartXRefBytes.Length);
						num = 0;
					}
				}
				num3 = Math.Max(0L, length - (long)(offsetFromEnd * num2));
			}
			while (list.Count == 0 && num3 > 0L);
			if (list.Count == 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(57, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find the startxref within the last ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(offsetFromEnd);
				defaultInterpolatedStringHandler.AppendLiteral(" characters.");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return (long)list[list.Count - 1];
		}

		// Token: 0x04000207 RID: 519
		private const int EndOfFileSearchRange = 2048;

		// Token: 0x04000208 RID: 520
		private static readonly byte[] StartXRefBytes = new byte[]
		{
			115,
			116,
			97,
			114,
			116,
			120,
			114,
			101,
			102
		};
	}
}
