using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser.Parts.CrossReference;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D2 RID: 210
	internal class CrossReferenceParser
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x0001826B File Offset: 0x0001646B
		public CrossReferenceParser(ILog log, XrefOffsetValidator offsetValidator, CrossReferenceStreamParser crossReferenceStreamParser)
		{
			this.log = log;
			this.offsetValidator = offsetValidator;
			this.crossReferenceStreamParser = crossReferenceStreamParser;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00018288 File Offset: 0x00016488
		public CrossReferenceTable Parse(IInputBytes bytes, bool isLenientParsing, long crossReferenceLocation, long offsetCorrection, IPdfTokenScanner pdfScanner, ISeekableTokenScanner tokenScanner)
		{
			long num = this.offsetValidator.CheckXRefOffset(crossReferenceLocation, tokenScanner, bytes, isLenientParsing);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (num > -1L)
			{
				crossReferenceLocation = num;
				ILog log = this.log;
				defaultInterpolatedStringHandler..ctor(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Found the first cross reference table or stream at ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(num);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				log.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			CrossReferenceTableBuilder crossReferenceTableBuilder = new CrossReferenceTableBuilder();
			HashSet<long> hashSet = new HashSet<long>();
			long num2 = crossReferenceLocation;
			int num3 = 0;
			while (num2 > 0L && num3 < 100)
			{
				ILog log2 = this.log;
				defaultInterpolatedStringHandler..ctor(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Reading cross reference table or stream at ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				log2.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
				if (num2 >= bytes.Length)
				{
					break;
				}
				tokenScanner.Seek(num2);
				tokenScanner.MoveNext();
				OperatorToken operatorToken = tokenScanner.CurrentToken as OperatorToken;
				if (operatorToken != null && operatorToken.Data == "xref")
				{
					num3 = 0;
					this.log.Debug("Element was cross reference table.");
					CrossReferenceTablePart crossReferenceTablePart = CrossReferenceTableParser.Parse(tokenScanner, num2, isLenientParsing);
					long num4 = crossReferenceTablePart.GetPreviousOffset();
					if (num4 >= 0L)
					{
						num4 += offsetCorrection;
					}
					num2 = num4;
					DictionaryToken dictionaryToken = crossReferenceTablePart.Dictionary;
					CrossReferenceTablePart crossReferenceTablePart2 = null;
					if (dictionaryToken.ContainsKey(NameToken.XrefStm))
					{
						this.log.Debug("Cross reference table contained reference to stream. Reading the stream.");
						long offset = crossReferenceTablePart.Offset;
						int num5 = ((NumericToken)dictionaryToken.Data[NameToken.XrefStm]).Int;
						num = this.offsetValidator.CheckXRefOffset((long)num5, tokenScanner, bytes, isLenientParsing);
						if (num > -1L && num != (long)num5)
						{
							ILog log3 = this.log;
							defaultInterpolatedStringHandler..ctor(44, 2);
							defaultInterpolatedStringHandler.AppendLiteral("/XRefStm offset ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num5);
							defaultInterpolatedStringHandler.AppendLiteral(" is incorrect, corrected to ");
							defaultInterpolatedStringHandler.AppendFormatted<long>(num);
							log3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							num5 = (int)num;
							dictionaryToken = dictionaryToken.With(NameToken.XrefStm, new NumericToken(num5));
							crossReferenceTablePart = new CrossReferenceTablePart(crossReferenceTablePart.ObjectOffsets, (long)num5, crossReferenceTablePart.Previous, dictionaryToken, crossReferenceTablePart.Type, new long?(offset));
						}
						if (num5 > 0)
						{
							try
							{
								this.TryParseCrossReferenceStream((long)num5, pdfScanner, new long?(offset), out crossReferenceTablePart2);
								goto IL_2A9;
							}
							catch (InvalidOperationException ex)
							{
								if (isLenientParsing)
								{
									this.log.Error("Failed to parse /XRefStm at offset " + num5.ToString(), ex);
									goto IL_2A9;
								}
								throw;
							}
						}
						if (!isLenientParsing)
						{
							throw new PdfDocumentFormatException("Skipped XRef stream due to a corrupt offset:" + num5.ToString());
						}
						this.log.Error("Skipped XRef stream due to a corrupt offset:" + num5.ToString());
					}
					IL_2A9:
					crossReferenceTableBuilder.Add(crossReferenceTablePart);
					if (crossReferenceTablePart2 != null)
					{
						crossReferenceTableBuilder.Add(crossReferenceTablePart2);
					}
				}
				else
				{
					if (!(tokenScanner.CurrentToken is NumericToken))
					{
						ILog log4 = this.log;
						defaultInterpolatedStringHandler..ctor(127, 3);
						defaultInterpolatedStringHandler.AppendLiteral("The cross reference found at this location (");
						defaultInterpolatedStringHandler.AppendFormatted<long>(num2);
						defaultInterpolatedStringHandler.AppendLiteral(") was not a table or stream. ");
						defaultInterpolatedStringHandler.AppendLiteral("Found token (");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(tokenScanner.CurrentToken);
						defaultInterpolatedStringHandler.AppendLiteral(") ending at ");
						defaultInterpolatedStringHandler.AppendFormatted<long>(tokenScanner.CurrentPosition);
						defaultInterpolatedStringHandler.AppendLiteral(" instead. Seeking next token.");
						log4.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
						long currentPosition = tokenScanner.CurrentPosition;
						if (num3 == 0)
						{
							long num6 = Math.Max(0L, num2 - 64L);
							bytes.Seek(num6);
							byte[] array = new byte[128];
							bytes.Read(array, default(int?));
							int num7 = OtherEncodings.BytesAsLatin1String(array).IndexOf("xref", 5);
							if (num7 >= 0)
							{
								num2 = num6 + (long)num7;
								num3++;
								continue;
							}
						}
						num2 = currentPosition;
						num3++;
						continue;
					}
					this.log.Debug("Element was cross reference stream.");
					num3 = 0;
					tokenScanner.Seek(num2);
					CrossReferenceTablePart crossReferenceTablePart3;
					if (!this.TryParseCrossReferenceStream(num2, pdfScanner, default(long?), out crossReferenceTablePart3))
					{
						long num8;
						if (!this.TryBruteForceXrefTableLocate(bytes, num2, out num8))
						{
							throw new PdfDocumentFormatException();
						}
						num2 = num8;
						num3++;
						continue;
					}
					else
					{
						crossReferenceTableBuilder.Add(crossReferenceTablePart3);
						num2 = crossReferenceTablePart3.Previous;
						if (num2 >= 0L)
						{
							num2 += offsetCorrection;
						}
						if (num2 > 0L)
						{
							num = this.offsetValidator.CheckXRefOffset(num2, tokenScanner, bytes, isLenientParsing);
							if (num > -1L && num != num2)
							{
								num2 = num;
								crossReferenceTablePart3.FixOffset(num2);
							}
						}
					}
				}
				if (hashSet.Contains(num2))
				{
					throw new PdfDocumentFormatException("The cross references formed an infinite loop.");
				}
				hashSet.Add(num2);
			}
			if (num3 == 100)
			{
				throw new PdfDocumentFormatException("The cross reference was not found.");
			}
			CrossReferenceTable crossReferenceTable = crossReferenceTableBuilder.Build(crossReferenceLocation, offsetCorrection, this.log);
			IReadOnlyDictionary<IndirectReference, long> objectOffsets;
			if (!CrossReferenceObjectOffsetValidator.ValidateCrossReferenceOffsets(bytes, crossReferenceTable, this.log, out objectOffsets))
			{
				crossReferenceTable = new CrossReferenceTable(crossReferenceTable.Type, objectOffsets, crossReferenceTable.Trailer, crossReferenceTable.CrossReferenceOffsets);
			}
			return crossReferenceTable;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00018790 File Offset: 0x00016990
		private bool TryParseCrossReferenceStream(long objByteOffset, IPdfTokenScanner pdfScanner, long? fromTableAtOffset, out CrossReferenceTablePart xrefTablePart)
		{
			xrefTablePart = null;
			pdfScanner.Seek(objByteOffset);
			pdfScanner.MoveNext();
			ObjectToken objectToken = (ObjectToken)pdfScanner.CurrentToken;
			if (objectToken != null)
			{
				StreamToken streamToken = objectToken.Data as StreamToken;
				if (streamToken != null)
				{
					xrefTablePart = this.crossReferenceStreamParser.Parse(objByteOffset, fromTableAtOffset, streamToken);
					return true;
				}
			}
			ILog log = this.log;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(72, 1);
			defaultInterpolatedStringHandler.AppendLiteral("When reading a cross reference stream object found a non-stream object: ");
			defaultInterpolatedStringHandler.AppendFormatted<IToken>((objectToken != null) ? objectToken.Data : null);
			log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return false;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001881C File Offset: 0x00016A1C
		private bool TryBruteForceXrefTableLocate(IInputBytes bytes, long expectedOffset, out long actualOffset)
		{
			actualOffset = expectedOffset;
			bytes.Seek(expectedOffset - 1L);
			byte currentByte = bytes.CurrentByte;
			while (bytes.MoveNext())
			{
				byte b = currentByte;
				currentByte = bytes.CurrentByte;
				if (currentByte == 120 && ReadHelper.IsWhitespace(b) && ReadHelper.IsString(bytes, "xref"))
				{
					actualOffset = bytes.CurrentOffset;
					return true;
				}
			}
			long num = expectedOffset - 1L;
			if (num < 0L)
			{
				return false;
			}
			bytes.Seek(num);
			byte[] array = new byte[5];
			while (bytes.Read(array, default(int?)) == array.Length)
			{
				for (int i = 1; i < array.Length; i++)
				{
					byte b2 = array[i - 1];
					byte b3 = array[i];
					bool flag = b3 == 120 && ReadHelper.IsWhitespace(b2);
					if ((b2 == 120 && b3 == 114) || flag)
					{
						long num2 = num + (long)i + (flag ? 1L : 0L);
						bytes.Seek(num2);
						if (ReadHelper.IsString(bytes, "xref"))
						{
							actualOffset = num2;
							return true;
						}
					}
				}
				num -= (long)array.Length;
				if (num < 0L)
				{
					break;
				}
				bytes.Seek(num);
			}
			bytes.Read(array, default(int?));
			return false;
		}

		// Token: 0x04000202 RID: 514
		private readonly ILog log;

		// Token: 0x04000203 RID: 515
		private readonly XrefOffsetValidator offsetValidator;

		// Token: 0x04000204 RID: 516
		private readonly CrossReferenceStreamParser crossReferenceStreamParser;
	}
}
