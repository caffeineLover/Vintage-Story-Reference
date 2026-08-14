using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D6 RID: 214
	internal class XrefOffsetValidator
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x000192DF File Offset: 0x000174DF
		public XrefOffsetValidator(ILog log)
		{
			this.log = log;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000192F0 File Offset: 0x000174F0
		public long CheckXRefOffset(long startXRefOffset, ISeekableTokenScanner scanner, IInputBytes inputBytes, bool isLenientParsing)
		{
			if (!isLenientParsing)
			{
				return startXRefOffset;
			}
			if (startXRefOffset >= inputBytes.Length)
			{
				return this.CalculateXRefFixedOffset(startXRefOffset, scanner, inputBytes);
			}
			scanner.Seek(startXRefOffset);
			scanner.MoveNext();
			if (scanner.CurrentToken == OperatorToken.Xref)
			{
				return startXRefOffset;
			}
			if (startXRefOffset <= 0L)
			{
				return -1L;
			}
			if (this.CheckXRefStreamOffset(startXRefOffset, scanner, true))
			{
				return startXRefOffset;
			}
			return this.CalculateXRefFixedOffset(startXRefOffset, scanner, inputBytes);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00019354 File Offset: 0x00017554
		private long CalculateXRefFixedOffset(long objectOffset, ISeekableTokenScanner scanner, IInputBytes inputBytes)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (objectOffset < 0L)
			{
				ILog log = this.log;
				defaultInterpolatedStringHandler..ctor(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid object offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(objectOffset);
				defaultInterpolatedStringHandler.AppendLiteral(" when searching for a xref table/stream");
				log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return 0L;
			}
			long num = this.BruteForceSearchForXref(objectOffset, scanner, inputBytes);
			if (num > -1L)
			{
				ILog log2 = this.log;
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Fixed reference for xref table/stream ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(objectOffset);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(num);
				log2.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
				return num;
			}
			ILog log3 = this.log;
			defaultInterpolatedStringHandler..ctor(50, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Can't find the object xref table/stream at offset ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(objectOffset);
			log3.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return 0L;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001942C File Offset: 0x0001762C
		private long BruteForceSearchForXref(long xrefOffset, ISeekableTokenScanner scanner, IInputBytes reader)
		{
			long result = -1L;
			long num = -1L;
			long num2 = -1L;
			this.BruteForceSearchForTables(reader);
			this.BfSearchForXRefStreams(reader);
			if (this.bfSearchXRefTablesOffsets != null)
			{
				num = XrefOffsetValidator.SearchNearestValue(this.bfSearchXRefTablesOffsets, xrefOffset);
			}
			if (this.bfSearchXRefStreamsOffsets != null)
			{
				num2 = XrefOffsetValidator.SearchNearestValue(this.bfSearchXRefStreamsOffsets, xrefOffset);
			}
			if (num > -1L && num2 > -1L)
			{
				long num3 = xrefOffset - num;
				long num4 = xrefOffset - num2;
				if (Math.Abs(num3) > Math.Abs(num4))
				{
					result = num2;
					this.bfSearchXRefStreamsOffsets.Remove(num2);
				}
				else
				{
					result = num;
					this.bfSearchXRefTablesOffsets.Remove(num);
				}
			}
			else if (num > -1L)
			{
				result = num;
				this.bfSearchXRefTablesOffsets.Remove(num);
			}
			else if (num2 > -1L)
			{
				result = num2;
				this.bfSearchXRefStreamsOffsets.Remove(num2);
			}
			return result;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000194E8 File Offset: 0x000176E8
		private void BruteForceSearchForTables(IInputBytes bytes)
		{
			if (this.bfSearchXRefTablesOffsets != null)
			{
				return;
			}
			this.bfSearchXRefTablesOffsets = new List<long>();
			long currentOffset = bytes.CurrentOffset;
			bytes.Seek(XrefOffsetValidator.MinimumSearchOffset);
			while (bytes.MoveNext() && !bytes.IsAtEnd())
			{
				if (ReadHelper.IsString(bytes, "xref"))
				{
					long currentOffset2 = bytes.CurrentOffset;
					bytes.Seek(currentOffset2 - 1L);
					if (ReadHelper.IsWhitespace(bytes.CurrentByte))
					{
						this.bfSearchXRefTablesOffsets.Add(currentOffset2);
					}
					bytes.Seek(currentOffset2 + 4L);
				}
			}
			bytes.Seek(currentOffset);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00019578 File Offset: 0x00017778
		private void BfSearchForXRefStreams(IInputBytes bytes)
		{
			if (this.bfSearchXRefStreamsOffsets != null)
			{
				return;
			}
			this.bfSearchXRefStreamsOffsets = new List<long>();
			long currentOffset = bytes.CurrentOffset;
			bytes.Seek(XrefOffsetValidator.MinimumSearchOffset);
			string text = " obj";
			while (bytes.MoveNext() && !bytes.IsAtEnd())
			{
				if (ReadHelper.IsString(bytes, "xref"))
				{
					long num = -1L;
					long currentOffset2 = bytes.CurrentOffset;
					bool flag = false;
					int num2 = 1;
					while (num2 < 40 && !flag)
					{
						long num3 = currentOffset2 - (long)(num2 * 10);
						if (num3 > 0L)
						{
							bytes.Seek(num3);
							for (int i = 0; i < 10; i++)
							{
								if (ReadHelper.IsString(bytes, text))
								{
									long num4 = num3 - 1L;
									bytes.Seek(num4);
									byte? b = bytes.Peek();
									if (b != null && ReadHelper.IsDigit((int)b.Value))
									{
										num4 -= 1L;
										bytes.Seek(num4);
										if (ReadHelper.IsSpace((int)bytes.CurrentByte))
										{
											int num5 = 0;
											bytes.Seek(num4 -= 1L);
											while (num4 > XrefOffsetValidator.MinimumSearchOffset && ReadHelper.IsDigit((int)bytes.CurrentByte))
											{
												bytes.Seek(num4 -= 1L);
												num5++;
											}
											if (num5 > 0)
											{
												bytes.MoveNext();
												num = bytes.CurrentOffset;
											}
										}
									}
									flag = true;
									break;
								}
								num3 += 1L;
								bytes.MoveNext();
							}
						}
						num2++;
					}
					if (num > -1L)
					{
						this.bfSearchXRefStreamsOffsets.Add(num);
					}
					bytes.Seek(currentOffset2 + 5L);
				}
			}
			bytes.Seek(currentOffset);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00019714 File Offset: 0x00017914
		private static long SearchNearestValue(List<long> values, long offset)
		{
			long result = -1L;
			long? num = default(long?);
			int num2 = -1;
			int count = values.Count;
			for (int i = 0; i < count; i++)
			{
				long num3 = offset - values[i];
				if (num == null || Math.Abs(num.Value) > Math.Abs(num3))
				{
					num = new long?(num3);
					num2 = i;
				}
			}
			if (num2 > -1)
			{
				result = values[num2];
			}
			return result;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00019788 File Offset: 0x00017988
		private bool CheckXRefStreamOffset(long startXRefOffset, ISeekableTokenScanner scanner, bool isLenient)
		{
			if (!isLenient || startXRefOffset == 0L)
			{
				return true;
			}
			scanner.Seek(startXRefOffset);
			NumericToken numericToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (scanner.TryReadToken<NumericToken>(ref numericToken))
			{
				try
				{
					NumericToken numericToken2;
					if (!scanner.TryReadToken<NumericToken>(ref numericToken2))
					{
						ILog log = this.log;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 3);
						defaultInterpolatedStringHandler.AppendLiteral("When checking offset at ");
						defaultInterpolatedStringHandler.AppendFormatted<long>(startXRefOffset);
						defaultInterpolatedStringHandler.AppendLiteral(" did not find the generation number. Got: ");
						defaultInterpolatedStringHandler.AppendFormatted<NumericToken>(numericToken);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<NumericToken>(numericToken2);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						log.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					scanner.MoveNext();
					if (scanner.CurrentToken != OperatorToken.StartObject)
					{
						scanner.Seek(startXRefOffset);
						return false;
					}
					DictionaryToken dictionaryToken;
					if (!scanner.TryReadToken<DictionaryToken>(ref dictionaryToken))
					{
						scanner.Seek(startXRefOffset);
					}
					IToken token;
					if (dictionaryToken.TryGet(NameToken.Type, ref token) && NameToken.Xref.Equals(token))
					{
						return true;
					}
					return false;
				}
				catch (Exception ex)
				{
					this.log.Error("Couldn't read the xref stream object.", ex);
					return false;
				}
			}
			ILog log2 = this.log;
			defaultInterpolatedStringHandler..ctor(82, 1);
			defaultInterpolatedStringHandler.AppendLiteral("When looking for the cross reference stream object we sought a number but found: ");
			defaultInterpolatedStringHandler.AppendFormatted<IToken>(scanner.CurrentToken);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			log2.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return false;
		}

		// Token: 0x04000209 RID: 521
		private static readonly long MinimumSearchOffset = 6L;

		// Token: 0x0400020A RID: 522
		private readonly ILog log;

		// Token: 0x0400020B RID: 523
		private List<long> bfSearchXRefTablesOffsets;

		// Token: 0x0400020C RID: 524
		private List<long> bfSearchXRefStreamsOffsets;
	}
}
