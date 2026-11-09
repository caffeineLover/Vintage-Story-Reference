using System;
using UglyToad.PdfPig.Content;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C7 RID: 199
	internal class ParsingCachingProviders
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00016D5A File Offset: 0x00014F5A
		public IResourceStore ResourceContainer { get; }

		// Token: 0x060003CA RID: 970 RVA: 0x00016D62 File Offset: 0x00014F62
		public ParsingCachingProviders(IResourceStore resourceContainer)
		{
			if (resourceContainer == null)
			{
				throw new ArgumentNullException("resourceContainer");
			}
			this.ResourceContainer = resourceContainer;
		}
	}
}
