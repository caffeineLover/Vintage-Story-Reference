using System;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Outline.Destinations
{
	// Token: 0x020000E0 RID: 224
	internal static class DestinationProvider
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x0001A39C File Offset: 0x0001859C
		internal static bool TryGetDestination(DictionaryToken dictionary, NameToken destinationToken, NamedDestinations namedDestinations, IPdfTokenScanner pdfScanner, ILog log, bool isRemoteDestination, out ExplicitDestination destination)
		{
			ArrayToken explicitDestinationArray;
			if (dictionary.TryGet(destinationToken, pdfScanner, out explicitDestinationArray))
			{
				return namedDestinations.TryGetExplicitDestination(explicitDestinationArray, log, isRemoteDestination, out destination);
			}
			IDataToken<string> dataToken;
			if (dictionary.TryGet(destinationToken, pdfScanner, out dataToken))
			{
				return namedDestinations.TryGet(dataToken.Data, out destination);
			}
			destination = null;
			return false;
		}
	}
}
