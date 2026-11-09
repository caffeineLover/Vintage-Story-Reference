using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C1 RID: 449
	internal class FilterProviderWithLookup : ILookupFilterProvider, IFilterProvider
	{
		// Token: 0x06000ABF RID: 2751 RVA: 0x000364E7 File Offset: 0x000346E7
		public FilterProviderWithLookup(IFilterProvider inner)
		{
			this.inner = inner;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000364F6 File Offset: 0x000346F6
		public IReadOnlyList<IFilter> GetFilters(DictionaryToken dictionary)
		{
			return this.inner.GetFilters(dictionary);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00036504 File Offset: 0x00034704
		public IReadOnlyList<IFilter> GetNamedFilters(IReadOnlyList<NameToken> names)
		{
			return this.inner.GetNamedFilters(names);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00036512 File Offset: 0x00034712
		public IReadOnlyList<IFilter> GetAllFilters()
		{
			return this.inner.GetAllFilters();
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00036520 File Offset: 0x00034720
		public IReadOnlyList<IFilter> GetFilters(DictionaryToken dictionary, IPdfTokenScanner scanner)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			IToken objectOrDefault = dictionary.GetObjectOrDefault(NameToken.Filter, NameToken.F);
			if (objectOrDefault == null)
			{
				return EmptyArray<IFilter>.Instance;
			}
			ArrayToken arrayToken = objectOrDefault as ArrayToken;
			if (arrayToken != null)
			{
				NameToken[] array = new NameToken[arrayToken.Data.Count];
				for (int i = 0; i < arrayToken.Data.Count; i++)
				{
					NameToken nameToken = (NameToken)arrayToken.Data[i];
					array[i] = nameToken;
				}
				return this.GetNamedFilters(array);
			}
			NameToken nameToken2 = objectOrDefault as NameToken;
			if (nameToken2 != null)
			{
				return this.GetNamedFilters(new NameToken[]
				{
					nameToken2
				});
			}
			IndirectReferenceToken indirectReferenceToken = objectOrDefault as IndirectReferenceToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (indirectReferenceToken == null)
			{
				defaultInterpolatedStringHandler..ctor(88, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The filter for the stream was not a valid object. Expected name or array, instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(objectOrDefault);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NameToken nameToken3;
			if (DirectObjectFinder.TryGet<NameToken>(indirectReferenceToken, scanner, out nameToken3))
			{
				return this.GetNamedFilters(new NameToken[]
				{
					nameToken3
				});
			}
			ArrayToken arrayToken2;
			if (DirectObjectFinder.TryGet<ArrayToken>(indirectReferenceToken, scanner, out arrayToken2))
			{
				return this.GetNamedFilters(Enumerable.ToList<NameToken>(Enumerable.Select<IToken, NameToken>(arrayToken2.Data, (IToken x) => (NameToken)x)));
			}
			defaultInterpolatedStringHandler..ctor(88, 1);
			defaultInterpolatedStringHandler.AppendLiteral("The filter for the stream was not a valid object. Expected name or array, instead got: ");
			defaultInterpolatedStringHandler.AppendFormatted<IToken>(objectOrDefault);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0400057B RID: 1403
		private readonly IFilterProvider inner;
	}
}
