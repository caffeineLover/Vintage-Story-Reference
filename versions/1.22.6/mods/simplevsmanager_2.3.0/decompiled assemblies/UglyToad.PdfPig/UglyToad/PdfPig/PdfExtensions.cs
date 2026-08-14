using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig
{
	// Token: 0x02000008 RID: 8
	public static class PdfExtensions
	{
		// Token: 0x06000030 RID: 48 RVA: 0x0000272C File Offset: 0x0000092C
		internal static bool TryGet<T>(this DictionaryToken dictionary, NameToken name, IPdfTokenScanner tokenScanner, out T token) where T : IToken
		{
			token = default(T);
			IToken token2;
			if (dictionary.TryGet(name, ref token2) && token2 is T)
			{
				T t = (T)((object)token2);
				token = t;
				return true;
			}
			IndirectReferenceToken indirectReferenceToken = token2 as IndirectReferenceToken;
			return indirectReferenceToken != null && DirectObjectFinder.TryGet<T>(indirectReferenceToken, tokenScanner, out token);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000277C File Offset: 0x0000097C
		internal static T Get<T>(this DictionaryToken dictionary, NameToken name, IPdfTokenScanner scanner) where T : class, IToken
		{
			IToken token;
			T t;
			if (dictionary.TryGet(name, ref token))
			{
				t = (token as T);
				if (t != null)
				{
					return t;
				}
			}
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Dictionary does not contain token with name ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name);
				defaultInterpolatedStringHandler.AppendLiteral(" of type ");
				defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			t = DirectObjectFinder.Get<T>(indirectReferenceToken, scanner);
			return t;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002814 File Offset: 0x00000A14
		public static IReadOnlyList<byte> Decode(this StreamToken stream, IFilterProvider filterProvider)
		{
			IReadOnlyList<IFilter> filters = filterProvider.GetFilters(stream.StreamDictionary);
			IReadOnlyList<byte> readOnlyList = stream.Data;
			for (int i = 0; i < filters.Count; i++)
			{
				readOnlyList = filters[i].Decode(readOnlyList, stream.StreamDictionary, i);
			}
			return readOnlyList;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000285C File Offset: 0x00000A5C
		internal static IReadOnlyList<byte> Decode(this StreamToken stream, ILookupFilterProvider filterProvider, IPdfTokenScanner scanner)
		{
			IReadOnlyList<IFilter> filters = filterProvider.GetFilters(stream.StreamDictionary, scanner);
			IReadOnlyList<byte> readOnlyList = stream.Data;
			for (int i = 0; i < filters.Count; i++)
			{
				readOnlyList = filters[i].Decode(readOnlyList, stream.StreamDictionary, i);
			}
			return readOnlyList;
		}
	}
}
