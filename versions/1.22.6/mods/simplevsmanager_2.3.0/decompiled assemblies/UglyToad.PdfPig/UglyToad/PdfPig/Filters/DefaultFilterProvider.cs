using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C0 RID: 448
	public class DefaultFilterProvider : IFilterProvider
	{
		// Token: 0x06000AB9 RID: 2745 RVA: 0x000361C8 File Offset: 0x000343C8
		private DefaultFilterProvider()
		{
			Ascii85Filter ascii85Filter = new Ascii85Filter();
			AsciiHexDecodeFilter asciiHexDecodeFilter = new AsciiHexDecodeFilter();
			CcittFaxDecodeFilter ccittFaxDecodeFilter = new CcittFaxDecodeFilter();
			DctDecodeFilter dctDecodeFilter = new DctDecodeFilter();
			FlateFilter flateFilter = new FlateFilter();
			Jbig2DecodeFilter jbig2DecodeFilter = new Jbig2DecodeFilter();
			JpxDecodeFilter jpxDecodeFilter = new JpxDecodeFilter();
			RunLengthFilter runLengthFilter = new RunLengthFilter();
			LzwFilter lzwFilter = new LzwFilter();
			Dictionary<string, IFilter> dictionary = new Dictionary<string, IFilter>();
			dictionary.Add(NameToken.Ascii85Decode.Data, ascii85Filter);
			dictionary.Add(NameToken.Ascii85DecodeAbbreviation.Data, ascii85Filter);
			dictionary.Add(NameToken.AsciiHexDecode.Data, asciiHexDecodeFilter);
			dictionary.Add(NameToken.AsciiHexDecodeAbbreviation.Data, asciiHexDecodeFilter);
			dictionary.Add(NameToken.CcittfaxDecode.Data, ccittFaxDecodeFilter);
			dictionary.Add(NameToken.CcittfaxDecodeAbbreviation.Data, ccittFaxDecodeFilter);
			dictionary.Add(NameToken.DctDecode.Data, dctDecodeFilter);
			dictionary.Add(NameToken.DctDecodeAbbreviation.Data, dctDecodeFilter);
			dictionary.Add(NameToken.FlateDecode.Data, flateFilter);
			dictionary.Add(NameToken.FlateDecodeAbbreviation.Data, flateFilter);
			dictionary.Add(NameToken.Jbig2Decode.Data, jbig2DecodeFilter);
			dictionary.Add(NameToken.JpxDecode.Data, jpxDecodeFilter);
			dictionary.Add(NameToken.RunLengthDecode.Data, runLengthFilter);
			dictionary.Add(NameToken.RunLengthDecodeAbbreviation.Data, runLengthFilter);
			dictionary.Add(NameToken.LzwDecode, lzwFilter);
			dictionary.Add(NameToken.LzwDecodeAbbreviation, lzwFilter);
			this.filterInstances = dictionary;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0003633C File Offset: 0x0003453C
		public IReadOnlyList<IFilter> GetFilters(DictionaryToken dictionary)
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
				IFilter[] array = new IFilter[arrayToken.Data.Count];
				for (int i = 0; i < arrayToken.Data.Count; i++)
				{
					string data = ((NameToken)arrayToken.Data[i]).Data;
					array[i] = this.GetFilterStrict(data);
				}
				return array;
			}
			NameToken nameToken = objectOrDefault as NameToken;
			if (nameToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(88, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The filter for the stream was not a valid object. Expected name or array, instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(objectOrDefault);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new IFilter[]
			{
				this.GetFilterStrict(nameToken.Data)
			};
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00036428 File Offset: 0x00034628
		public IReadOnlyList<IFilter> GetNamedFilters(IReadOnlyList<NameToken> names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			List<IFilter> list = new List<IFilter>();
			foreach (NameToken nameToken in names)
			{
				list.Add(this.GetFilterStrict(nameToken));
			}
			return list;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00036490 File Offset: 0x00034690
		private IFilter GetFilterStrict(string name)
		{
			IFilter result;
			if (!this.filterInstances.TryGetValue(name, ref result))
			{
				throw new NotSupportedException("The filter with the name " + name + " is not supported yet. Please raise an issue.");
			}
			return result;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000364C4 File Offset: 0x000346C4
		public IReadOnlyList<IFilter> GetAllFilters()
		{
			return Enumerable.ToList<IFilter>(Enumerable.Distinct<IFilter>(this.filterInstances.Values));
		}

		// Token: 0x04000579 RID: 1401
		private readonly IReadOnlyDictionary<string, IFilter> filterInstances;

		// Token: 0x0400057A RID: 1402
		public static readonly IFilterProvider Instance = new DefaultFilterProvider();
	}
}
