using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000DB RID: 219
	internal static class NamedDestinationsProvider
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x00019C8C File Offset: 0x00017E8C
		internal static NamedDestinations Read(DictionaryToken catalogDictionary, IPdfTokenScanner pdfScanner, Pages pages, ILog log)
		{
			Dictionary<string, ExplicitDestination> dictionary = new Dictionary<string, ExplicitDestination>();
			DictionaryToken dictionaryToken;
			if (catalogDictionary.TryGet(NameToken.Dests, pdfScanner, out dictionaryToken))
			{
				using (IEnumerator<KeyValuePair<string, IToken>> enumerator = dictionaryToken.Data.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, IToken> keyValuePair = enumerator.Current;
						ExplicitDestination explicitDestination;
						if (NamedDestinationsProvider.TryReadExplicitDestination(keyValuePair.Value, pdfScanner, pages, log, false, out explicitDestination))
						{
							dictionary[keyValuePair.Key] = explicitDestination;
						}
					}
					goto IL_D6;
				}
			}
			DictionaryToken dictionary2;
			if (catalogDictionary.TryGet(NameToken.Names, pdfScanner, out dictionary2) && dictionary2.TryGet(NameToken.Dests, pdfScanner, out dictionaryToken))
			{
				NameTreeParser.FlattenNameTree<ExplicitDestination>(dictionaryToken, pdfScanner, delegate(IToken value)
				{
					ExplicitDestination result;
					if (NamedDestinationsProvider.TryReadExplicitDestination(value, pdfScanner, pages, log, false, out result))
					{
						return result;
					}
					return null;
				}, dictionary);
			}
			IL_D6:
			return new NamedDestinations(dictionary, pages);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00019D8C File Offset: 0x00017F8C
		private static bool TryReadExplicitDestination(IToken value, IPdfTokenScanner pdfScanner, Pages pages, ILog log, bool isRemoteDestination, out ExplicitDestination destination)
		{
			destination = null;
			ArrayToken explicitDestinationArray;
			DictionaryToken dictionary;
			return (DirectObjectFinder.TryGet<ArrayToken>(value, pdfScanner, out explicitDestinationArray) && NamedDestinationsProvider.TryGetExplicitDestination(explicitDestinationArray, pages, log, isRemoteDestination, out destination)) || (DirectObjectFinder.TryGet<DictionaryToken>(value, pdfScanner, out dictionary) && dictionary.TryGet(NameToken.D, pdfScanner, out explicitDestinationArray) && NamedDestinationsProvider.TryGetExplicitDestination(explicitDestinationArray, pages, log, isRemoteDestination, out destination));
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00019DE4 File Offset: 0x00017FE4
		internal static bool TryGetExplicitDestination(ArrayToken explicitDestinationArray, Pages pages, ILog log, bool isRemoteDestination, out ExplicitDestination destination)
		{
			NamedDestinationsProvider.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.explicitDestinationArray = explicitDestinationArray;
			destination = null;
			if (CS$<>8__locals1.explicitDestinationArray == null || CS$<>8__locals1.explicitDestinationArray.Length == 0)
			{
				return false;
			}
			IToken token = CS$<>8__locals1.explicitDestinationArray[0];
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			int pageNumber;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (indirectReferenceToken != null)
			{
				if (isRemoteDestination)
				{
					string message = "TryGetExplicitDestination Cannot use indirect reference for remote destination.";
					if (log != null)
					{
						log.Error(message);
					}
					return false;
				}
				PageTreeNode pageByReference = pages.GetPageByReference(indirectReferenceToken.Data);
				if (pageByReference == null || pageByReference.PageNumber == null)
				{
					return false;
				}
				pageNumber = pageByReference.PageNumber.Value;
			}
			else
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken == null)
				{
					defaultInterpolatedStringHandler..ctor(36, 2);
					defaultInterpolatedStringHandler.AppendFormatted("TryGetExplicitDestination");
					defaultInterpolatedStringHandler.AppendLiteral(" No page number given in 'Dest': '");
					defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(CS$<>8__locals1.explicitDestinationArray);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					string message2 = defaultInterpolatedStringHandler.ToStringAndClear();
					if (log != null)
					{
						log.Error(message2);
					}
					return false;
				}
				pageNumber = numericToken.Int + 1;
			}
			NameToken nameToken = null;
			if (CS$<>8__locals1.explicitDestinationArray.Length > 1)
			{
				nameToken = (CS$<>8__locals1.explicitDestinationArray[1] as NameToken);
			}
			if (nameToken == null)
			{
				defaultInterpolatedStringHandler..ctor(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Missing name token as second argument to explicit destination: ");
				defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(CS$<>8__locals1.explicitDestinationArray);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				string message3 = defaultInterpolatedStringHandler.ToStringAndClear();
				if (log != null)
				{
					log.Error(message3);
				}
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitPage, ExplicitDestinationCoordinates.Empty);
				return true;
			}
			if (nameToken.Equals(NameToken.XYZ))
			{
				decimal? left = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(2, ref CS$<>8__locals1);
				decimal? top = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(3, ref CS$<>8__locals1);
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.XyzCoordinates, new ExplicitDestinationCoordinates(left, top));
				return true;
			}
			if (nameToken.Equals(NameToken.Fit))
			{
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitPage, ExplicitDestinationCoordinates.Empty);
				return true;
			}
			if (nameToken.Equals(NameToken.FitH))
			{
				decimal? top2 = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(2, ref CS$<>8__locals1);
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitHorizontally, new ExplicitDestinationCoordinates(default(decimal?), top2));
				return true;
			}
			if (nameToken.Equals(NameToken.FitV))
			{
				decimal? left2 = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(2, ref CS$<>8__locals1);
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitVertically, new ExplicitDestinationCoordinates(left2));
				return true;
			}
			if (nameToken.Equals(NameToken.FitR))
			{
				decimal? left3 = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(2, ref CS$<>8__locals1);
				decimal? bottom = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(3, ref CS$<>8__locals1);
				decimal? right = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(4, ref CS$<>8__locals1);
				decimal? top3 = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(5, ref CS$<>8__locals1);
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitRectangle, new ExplicitDestinationCoordinates(left3, top3, right, bottom));
				return true;
			}
			if (nameToken.Equals(NameToken.FitB))
			{
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitBoundingBox, ExplicitDestinationCoordinates.Empty);
				return true;
			}
			if (nameToken.Equals(NameToken.FitBH))
			{
				decimal? top4 = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(2, ref CS$<>8__locals1);
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitBoundingBoxHorizontally, new ExplicitDestinationCoordinates(default(decimal?), top4));
				return true;
			}
			if (nameToken.Equals(NameToken.FitBV))
			{
				decimal? left4 = NamedDestinationsProvider.<TryGetExplicitDestination>g__GetPossibleEntry|2_0(2, ref CS$<>8__locals1);
				destination = new ExplicitDestination(pageNumber, ExplicitDestinationType.FitBoundingBoxVertically, new ExplicitDestinationCoordinates(left4));
				return true;
			}
			return false;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001A0F4 File Offset: 0x000182F4
		[CompilerGenerated]
		internal static decimal? <TryGetExplicitDestination>g__GetPossibleEntry|2_0(int index, ref NamedDestinationsProvider.<>c__DisplayClass2_0 A_1)
		{
			if (index >= A_1.explicitDestinationArray.Length)
			{
				return default(decimal?);
			}
			NumericToken numericToken = A_1.explicitDestinationArray[index] as NumericToken;
			if (numericToken != null)
			{
				return new decimal?(numericToken.Data);
			}
			return default(decimal?);
		}
	}
}
