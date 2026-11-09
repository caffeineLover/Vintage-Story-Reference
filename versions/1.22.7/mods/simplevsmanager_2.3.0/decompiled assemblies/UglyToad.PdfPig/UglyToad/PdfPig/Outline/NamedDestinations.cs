using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000DA RID: 218
	internal class NamedDestinations
	{
		// Token: 0x06000415 RID: 1045 RVA: 0x00019C54 File Offset: 0x00017E54
		internal NamedDestinations(IReadOnlyDictionary<string, ExplicitDestination> namedDestinations, Pages pages)
		{
			this.namedDestinations = namedDestinations;
			this.pages = pages;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00019C6A File Offset: 0x00017E6A
		internal bool TryGet(string name, out ExplicitDestination destination)
		{
			return this.namedDestinations.TryGetValue(name, ref destination);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00019C79 File Offset: 0x00017E79
		internal bool TryGetExplicitDestination(ArrayToken explicitDestinationArray, ILog log, bool isRemoteDestination, out ExplicitDestination destination)
		{
			return NamedDestinationsProvider.TryGetExplicitDestination(explicitDestinationArray, this.pages, log, isRemoteDestination, out destination);
		}

		// Token: 0x04000214 RID: 532
		private readonly IReadOnlyDictionary<string, ExplicitDestination> namedDestinations;

		// Token: 0x04000215 RID: 533
		private readonly Pages pages;
	}
}
