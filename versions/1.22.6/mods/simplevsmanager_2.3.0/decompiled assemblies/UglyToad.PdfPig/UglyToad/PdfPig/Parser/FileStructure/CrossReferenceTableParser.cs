using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Parser.Parts.CrossReference;
using UglyToad.PdfPig.Tokenization;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D3 RID: 211
	internal static class CrossReferenceTableParser
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00018948 File Offset: 0x00016B48
		public static CrossReferenceTablePart Parse(ISeekableTokenScanner scanner, long offset, bool isLenientParsing)
		{
			CrossReferenceTablePartBuilder crossReferenceTablePartBuilder = new CrossReferenceTablePartBuilder
			{
				Offset = offset,
				XRefType = CrossReferenceType.Table
			};
			if (scanner.CurrentPosition != offset)
			{
				scanner.Seek(offset);
			}
			scanner.MoveNext();
			OperatorToken operatorToken = scanner.CurrentToken as OperatorToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (operatorToken != null)
			{
				if (!(operatorToken.Data == "xref"))
				{
					defaultInterpolatedStringHandler..ctor(39, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unexpected operator in xref position: ");
					defaultInterpolatedStringHandler.AppendFormatted<OperatorToken>(operatorToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				scanner.MoveNext();
			}
			NumericToken numericToken = scanner.CurrentToken as NumericToken;
			if (numericToken != null)
			{
				NumericToken numericToken2;
				if (!scanner.TryReadToken<NumericToken>(ref numericToken2))
				{
					defaultInterpolatedStringHandler..ctor(49, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Unexpected token following xref and ");
					defaultInterpolatedStringHandler.AppendFormatted<NumericToken>(numericToken);
					defaultInterpolatedStringHandler.AppendLiteral(". We found: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				TableSubsectionDefinition tableSubsectionDefinition = new TableSubsectionDefinition(numericToken.Long, numericToken2.Int);
				EndOfLineTokenizer endOfLineTokenizer = new EndOfLineTokenizer();
				scanner.RegisterCustomTokenizer(13, endOfLineTokenizer);
				scanner.RegisterCustomTokenizer(10, endOfLineTokenizer);
				bool flag = false;
				List<IToken> list = new List<IToken>();
				int objectCount = 0;
				while (scanner.MoveNext())
				{
					if (scanner.CurrentToken is EndOfLineToken)
					{
						if (flag)
						{
							flag = false;
							objectCount = CrossReferenceTableParser.ProcessTokens(list, crossReferenceTablePartBuilder, isLenientParsing, objectCount, ref tableSubsectionDefinition);
							list.Clear();
						}
					}
					else if (!(scanner.CurrentToken is CommentToken))
					{
						OperatorToken operatorToken2 = scanner.CurrentToken as OperatorToken;
						bool flag2 = operatorToken2 != null && (operatorToken2.Data == "f" || operatorToken2.Data == "n");
						if (!(scanner.CurrentToken is NumericToken) && !flag2)
						{
							break;
						}
						flag = true;
						list.Add(scanner.CurrentToken);
					}
				}
				if (list.Count > 0)
				{
					CrossReferenceTableParser.ProcessTokens(list, crossReferenceTablePartBuilder, isLenientParsing, objectCount, ref tableSubsectionDefinition);
				}
				scanner.DeregisterCustomTokenizer(endOfLineTokenizer);
			}
			crossReferenceTablePartBuilder.Dictionary = CrossReferenceTableParser.ParseTrailer(scanner, isLenientParsing);
			return crossReferenceTablePartBuilder.Build();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00018B68 File Offset: 0x00016D68
		private static int ProcessTokens(List<IToken> tokens, CrossReferenceTablePartBuilder builder, bool isLenientParsing, int objectCount, ref TableSubsectionDefinition definition)
		{
			CrossReferenceTableParser.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.tokens = tokens;
			if (objectCount == definition.Count)
			{
				if (CS$<>8__locals1.tokens.Count == 2)
				{
					NumericToken numericToken = CS$<>8__locals1.tokens[0] as NumericToken;
					if (numericToken != null)
					{
						NumericToken numericToken2 = CS$<>8__locals1.tokens[1] as NumericToken;
						if (numericToken2 != null)
						{
							definition = new TableSubsectionDefinition(numericToken.Long, numericToken2.Int);
							return 0;
						}
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(72, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Found a line with 2 unexpected entries in the cross reference table: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(CS$<>8__locals1.tokens[0]);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(CS$<>8__locals1.tokens[1]);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (CS$<>8__locals1.tokens.Count > 2)
			{
				OperatorToken operatorToken = CS$<>8__locals1.tokens[CS$<>8__locals1.tokens.Count - 1] as OperatorToken;
				if (operatorToken != null)
				{
					if (operatorToken.Data == "f")
					{
						return objectCount + 1;
					}
					if (operatorToken.Data != "n")
					{
						if (!isLenientParsing)
						{
							throw new PdfDocumentFormatException(CrossReferenceTableParser.<ProcessTokens>g__GetErrorMessage|3_0(ref CS$<>8__locals1));
						}
						return objectCount;
					}
					else
					{
						NumericToken numericToken3 = CS$<>8__locals1.tokens[0] as NumericToken;
						if (numericToken3 != null)
						{
							NumericToken numericToken4 = CS$<>8__locals1.tokens[1] as NumericToken;
							if (numericToken4 != null)
							{
								builder.Add(definition.FirstNumber + (long)objectCount, numericToken4.Int, numericToken3.Long);
								return objectCount + 1;
							}
						}
					}
				}
				else if (!isLenientParsing)
				{
					throw new PdfDocumentFormatException(CrossReferenceTableParser.<ProcessTokens>g__GetErrorMessage|3_0(ref CS$<>8__locals1));
				}
				return objectCount;
			}
			if (!isLenientParsing)
			{
				throw new PdfDocumentFormatException(CrossReferenceTableParser.<ProcessTokens>g__GetErrorMessage|3_0(ref CS$<>8__locals1));
			}
			return objectCount;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00018D1C File Offset: 0x00016F1C
		private static DictionaryToken ParseTrailer(ISeekableTokenScanner scanner, bool isLenientParsing)
		{
			OperatorToken operatorToken = scanner.CurrentToken as OperatorToken;
			if (operatorToken == null || !(operatorToken.Data == "trailer"))
			{
				if (isLenientParsing)
				{
					bool flag = false;
					while (scanner.MoveNext())
					{
						OperatorToken operatorToken2 = scanner.CurrentToken as OperatorToken;
						if (operatorToken2 != null && operatorToken2.Data == "trailer")
						{
							flag = true;
							break;
						}
					}
					DictionaryToken result;
					if (flag && scanner.TryReadToken<DictionaryToken>(ref result))
					{
						return result;
					}
				}
				throw new PdfDocumentFormatException("No trailer dictionary was present.");
			}
			DictionaryToken result2;
			if (!scanner.TryReadToken<DictionaryToken>(ref result2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected to find a dictionary in the trailer but instead found: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result2;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00018DE4 File Offset: 0x00016FE4
		[CompilerGenerated]
		internal static string <ProcessTokens>g__GetErrorMessage|3_0(ref CrossReferenceTableParser.<>c__DisplayClass3_0 A_0)
		{
			return "Invalid line format in xref table: [" + string.Join(", ", Enumerable.Select<IToken, string>(A_0.tokens, (IToken x) => x.ToString())) + "]";
		}

		// Token: 0x04000205 RID: 517
		private const string InUseEntry = "n";

		// Token: 0x04000206 RID: 518
		private const string FreeEntry = "f";
	}
}
