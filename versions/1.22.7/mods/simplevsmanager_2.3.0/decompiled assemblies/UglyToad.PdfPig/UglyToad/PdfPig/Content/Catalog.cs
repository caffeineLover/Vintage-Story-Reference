using System;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001DE RID: 478
	public class Catalog
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x000397C0 File Offset: 0x000379C0
		[NotNull]
		public DictionaryToken CatalogDictionary { get; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x000397C8 File Offset: 0x000379C8
		internal NamedDestinations NamedDestinations { get; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x000397D0 File Offset: 0x000379D0
		internal Pages Pages { get; }

		// Token: 0x06000B57 RID: 2903 RVA: 0x000397D8 File Offset: 0x000379D8
		internal Catalog(DictionaryToken catalogDictionary, Pages pages, NamedDestinations namedDestinations)
		{
			if (catalogDictionary == null)
			{
				throw new ArgumentNullException("catalogDictionary");
			}
			this.CatalogDictionary = catalogDictionary;
			if (pages == null)
			{
				throw new ArgumentNullException("pages");
			}
			this.Pages = pages;
			this.NamedDestinations = namedDestinations;
		}
	}
}
