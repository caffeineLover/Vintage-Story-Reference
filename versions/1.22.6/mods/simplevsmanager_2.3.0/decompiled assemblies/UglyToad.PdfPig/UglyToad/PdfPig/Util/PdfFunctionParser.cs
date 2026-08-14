using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000034 RID: 52
	internal static class PdfFunctionParser
	{
		// Token: 0x06000149 RID: 329 RVA: 0x0000C94C File Offset: 0x0000AB4C
		public static PdfFunction Create(IToken function, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			StreamToken streamToken = null;
			StreamToken streamToken2;
			DictionaryToken dictionaryToken;
			if (DirectObjectFinder.TryGet<StreamToken>(function, scanner, out streamToken2))
			{
				dictionaryToken = streamToken2.StreamDictionary;
				streamToken = new StreamToken(streamToken2.StreamDictionary, streamToken2.Decode(filterProvider, scanner));
			}
			else
			{
				DictionaryToken dictionaryToken2;
				if (!DirectObjectFinder.TryGet<DictionaryToken>(function, scanner, out dictionaryToken2))
				{
					throw new ArgumentException("function");
				}
				dictionaryToken = dictionaryToken2;
			}
			ArrayToken domain;
			if (!dictionaryToken.TryGet(NameToken.Domain, scanner, out domain))
			{
				throw new ArgumentNullException(NameToken.Domain);
			}
			ArrayToken range;
			dictionaryToken.TryGet(NameToken.Range, scanner, out range);
			int @int = (dictionaryToken.Data[NameToken.FunctionType] as NumericToken).Int;
			switch (@int)
			{
			case 0:
				if (streamToken == null)
				{
					throw new NotImplementedException("PdfFunctionType0 not stream");
				}
				return PdfFunctionParser.CreatePdfFunctionType0(streamToken, domain, range, scanner);
			case 2:
				return PdfFunctionParser.CreatePdfFunctionType2(dictionaryToken, domain, range, scanner);
			case 3:
				return PdfFunctionParser.CreatePdfFunctionType3(dictionaryToken, domain, range, scanner, filterProvider);
			case 4:
				if (streamToken == null)
				{
					throw new NotImplementedException("PdfFunctionType4 not stream");
				}
				return PdfFunctionParser.CreatePdfFunctionType4(streamToken, domain, range, scanner);
			}
			throw new IOException("Error: Unknown function type " + @int.ToString());
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000CA6C File Offset: 0x0000AC6C
		private static PdfFunctionType0 CreatePdfFunctionType0(StreamToken functionStream, ArrayToken domain, ArrayToken range, IPdfTokenScanner scanner)
		{
			if (range == null)
			{
				throw new ArgumentException("Could not retrieve Range in type 0 function.");
			}
			ArrayToken arrayToken;
			if (!functionStream.StreamDictionary.TryGet(NameToken.Size, scanner, out arrayToken))
			{
				throw new ArgumentNullException(NameToken.Size);
			}
			NumericToken numericToken;
			if (!functionStream.StreamDictionary.TryGet(NameToken.BitsPerSample, scanner, out numericToken))
			{
				throw new ArgumentNullException(NameToken.BitsPerSample);
			}
			int order = 1;
			NumericToken numericToken2;
			if (functionStream.StreamDictionary.TryGet(NameToken.Order, scanner, out numericToken2))
			{
				order = numericToken2.Int;
			}
			ArrayToken arrayToken2;
			if (!functionStream.StreamDictionary.TryGet(NameToken.Encode, scanner, out arrayToken2) || arrayToken2 == null)
			{
				List<NumericToken> list = new List<NumericToken>();
				int length = arrayToken.Length;
				for (int i = 0; i < length; i++)
				{
					list.Add(new NumericToken(0m));
					list.Add(new NumericToken((long)(arrayToken[i] as NumericToken).Int - 1L));
				}
				arrayToken2 = new ArrayToken(list);
			}
			ArrayToken arrayToken3;
			if (!functionStream.StreamDictionary.TryGet(NameToken.Decode, scanner, out arrayToken3) || arrayToken3 == null)
			{
				arrayToken3 = range;
			}
			return new PdfFunctionType0(functionStream, domain, range, arrayToken, numericToken.Int, order, arrayToken2, arrayToken3);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		private static PdfFunctionType2 CreatePdfFunctionType2(DictionaryToken functionDictionary, ArrayToken domain, ArrayToken range, IPdfTokenScanner scanner)
		{
			ArrayToken arrayToken;
			if (!functionDictionary.TryGet(NameToken.C0, scanner, out arrayToken) || arrayToken.Length == 0)
			{
				List<NumericToken> list = new List<NumericToken>();
				list.Add(new NumericToken(0m));
				arrayToken = new ArrayToken(list);
			}
			ArrayToken arrayToken2;
			if (!functionDictionary.TryGet(NameToken.C1, scanner, out arrayToken2) || arrayToken2.Length == 0)
			{
				List<NumericToken> list2 = new List<NumericToken>();
				list2.Add(new NumericToken(1m));
				arrayToken2 = new ArrayToken(list2);
			}
			NumericToken numericToken;
			if (!functionDictionary.TryGet(NameToken.N, scanner, out numericToken))
			{
				throw new ArgumentNullException(NameToken.N);
			}
			return new PdfFunctionType2(functionDictionary, domain, range, arrayToken, arrayToken2, numericToken.Double);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000CC40 File Offset: 0x0000AE40
		private static PdfFunctionType3 CreatePdfFunctionType3(DictionaryToken functionDictionary, ArrayToken domain, ArrayToken range, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			List<PdfFunction> list = new List<PdfFunction>();
			ArrayToken arrayToken;
			if (functionDictionary.TryGet(NameToken.Functions, scanner, out arrayToken))
			{
				using (IEnumerator<IToken> enumerator = arrayToken.Data.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IToken token = enumerator.Current;
						StreamToken function;
						if (DirectObjectFinder.TryGet<StreamToken>(token, scanner, out function))
						{
							list.Add(PdfFunctionParser.Create(function, scanner, filterProvider));
						}
						else
						{
							DictionaryToken function2;
							if (!DirectObjectFinder.TryGet<DictionaryToken>(token, scanner, out function2))
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
								defaultInterpolatedStringHandler.AppendLiteral("Could not find function for token '");
								defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
								defaultInterpolatedStringHandler.AppendLiteral("' inside type 3 function.");
								throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							list.Add(PdfFunctionParser.Create(function2, scanner, filterProvider));
						}
					}
					goto IL_CF;
				}
				goto IL_BF;
				IL_CF:
				ArrayToken bounds;
				if (!functionDictionary.TryGet(NameToken.Bounds, scanner, out bounds))
				{
					throw new ArgumentNullException(NameToken.Bounds);
				}
				ArrayToken encode;
				if (!functionDictionary.TryGet(NameToken.Encode, scanner, out encode))
				{
					throw new ArgumentNullException(NameToken.Encode);
				}
				return new PdfFunctionType3(functionDictionary, domain, range, list, bounds, encode);
			}
			IL_BF:
			throw new ArgumentNullException(NameToken.Functions);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000CD78 File Offset: 0x0000AF78
		private static PdfFunctionType4 CreatePdfFunctionType4(StreamToken functionStream, ArrayToken domain, ArrayToken range, IPdfTokenScanner scanner)
		{
			if (range == null)
			{
				throw new ArgumentException("Could not retrieve Range in type 4 function.");
			}
			return new PdfFunctionType4(functionStream, domain, range);
		}
	}
}
