using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200005C RID: 92
	[NullableContext(1)]
	internal interface IPageContext
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000233 RID: 563
		bool IsInitialRenderingPhase { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000234 RID: 564
		int DocumentLength { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000235 RID: 565
		int CurrentPage { get; }

		// Token: 0x06000236 RID: 566
		void SetSectionPage(string name);

		// Token: 0x06000237 RID: 567
		[return: Nullable(2)]
		DocumentLocation GetLocation(string name);

		// Token: 0x06000238 RID: 568
		string GetDocumentLocationName(string locationName);

		// Token: 0x06000239 RID: 569
		void CaptureContentPosition(PageElementLocation location);

		// Token: 0x0600023A RID: 570
		ICollection<PageElementLocation> GetContentCapturedPositions(string id);
	}
}
