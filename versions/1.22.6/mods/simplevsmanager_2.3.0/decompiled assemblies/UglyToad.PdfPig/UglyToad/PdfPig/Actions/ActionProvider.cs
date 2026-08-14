using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x02000204 RID: 516
	internal static class ActionProvider
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x0003DE98 File Offset: 0x0003C098
		internal static bool TryGetAction(DictionaryToken dictionary, NamedDestinations namedDestinations, IPdfTokenScanner pdfScanner, ILog log, out PdfAction result)
		{
			result = null;
			DictionaryToken dictionaryToken;
			if (!dictionary.TryGet(NameToken.A, pdfScanner, out dictionaryToken))
			{
				return false;
			}
			NameToken nameToken;
			if (!dictionaryToken.TryGet(NameToken.S, pdfScanner, out nameToken))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No action type (/S) specified for action: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (nameToken.Equals(NameToken.GoTo))
			{
				ExplicitDestination destination;
				if (DestinationProvider.TryGetDestination(dictionaryToken, NameToken.D, namedDestinations, pdfScanner, log, false, out destination))
				{
					result = new GoToAction(destination);
					return true;
				}
			}
			else if (nameToken.Equals(NameToken.GoToR))
			{
				string filename;
				ExplicitDestination destination2;
				if (dictionaryToken.TryGetOptionalStringDirect(NameToken.F, pdfScanner, out filename) && DestinationProvider.TryGetDestination(dictionaryToken, NameToken.D, namedDestinations, pdfScanner, log, true, out destination2))
				{
					result = new GoToRAction(destination2, filename);
					return true;
				}
			}
			else if (nameToken.Equals(NameToken.GoToE))
			{
				ExplicitDestination destination3;
				if (DestinationProvider.TryGetDestination(dictionaryToken, NameToken.D, namedDestinations, pdfScanner, log, true, out destination3))
				{
					string fileSpecification;
					if (!dictionaryToken.TryGetOptionalStringDirect(NameToken.F, pdfScanner, out fileSpecification))
					{
						fileSpecification = null;
					}
					result = new GoToEAction(destination3, fileSpecification);
					return true;
				}
			}
			else if (nameToken.Equals(NameToken.Uri))
			{
				string uri;
				if (!dictionaryToken.TryGetOptionalStringDirect(NameToken.Uri, pdfScanner, out uri))
				{
					uri = null;
				}
				result = new UriAction(uri);
				return true;
			}
			return false;
		}
	}
}
