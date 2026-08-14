using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class PageContext : IPageContext
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00005633 File Offset: 0x00003833
		// (set) Token: 0x06000241 RID: 577 RVA: 0x0000563B File Offset: 0x0000383B
		public bool IsInitialRenderingPhase { get; private set; } = true;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00005644 File Offset: 0x00003844
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000564C File Offset: 0x0000384C
		public int DocumentLength { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00005655 File Offset: 0x00003855
		private List<DocumentLocation> Locations { get; } = new List<DocumentLocation>();

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000565D File Offset: 0x0000385D
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00005665 File Offset: 0x00003865
		public int CurrentDocumentId { get; private set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000566E File Offset: 0x0000386E
		// (set) Token: 0x06000248 RID: 584 RVA: 0x00005676 File Offset: 0x00003876
		public int CurrentPage { get; private set; }

		// Token: 0x06000249 RID: 585 RVA: 0x0000567F File Offset: 0x0000387F
		internal void SetDocumentId(int id)
		{
			this.CurrentDocumentId = id;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00005688 File Offset: 0x00003888
		internal void ProceedToNextRenderingPhase()
		{
			this.IsInitialRenderingPhase = false;
			this.CurrentPage = 0;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00005698 File Offset: 0x00003898
		internal void DecrementPageNumber()
		{
			int currentPage = this.CurrentPage;
			this.CurrentPage = currentPage - 1;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000056B8 File Offset: 0x000038B8
		internal void IncrementPageNumber()
		{
			int currentPage = this.CurrentPage;
			this.CurrentPage = currentPage + 1;
			this.DocumentLength = Math.Max(this.DocumentLength, this.CurrentPage);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000056EC File Offset: 0x000038EC
		public void SetSectionPage(string name)
		{
			DocumentLocation documentLocation = this.GetLocation(name);
			if (documentLocation == null)
			{
				documentLocation = new DocumentLocation
				{
					DocumentId = this.CurrentDocumentId,
					Name = name,
					PageStart = this.CurrentPage,
					PageEnd = this.CurrentPage
				};
				this.Locations.Add(documentLocation);
			}
			if (documentLocation.PageEnd < this.CurrentPage)
			{
				documentLocation.PageEnd = this.CurrentPage;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000575C File Offset: 0x0000395C
		[return: Nullable(2)]
		public DocumentLocation GetLocation(string name)
		{
			return this.Locations.Find((DocumentLocation x) => x.DocumentId == this.CurrentDocumentId && x.Name == name);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00005794 File Offset: 0x00003994
		public string GetDocumentLocationName(string locationName)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurrentDocumentId);
			defaultInterpolatedStringHandler.AppendLiteral(" | ");
			defaultInterpolatedStringHandler.AppendFormatted(locationName);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000057D2 File Offset: 0x000039D2
		private List<PageElementLocation> ContentCapturedPositions { get; } = new List<PageElementLocation>();

		// Token: 0x06000251 RID: 593 RVA: 0x000057DA File Offset: 0x000039DA
		public void CaptureContentPosition(PageElementLocation location)
		{
			this.ContentCapturedPositions.Add(location);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000057E8 File Offset: 0x000039E8
		public ICollection<PageElementLocation> GetContentCapturedPositions(string id)
		{
			return Enumerable.ToList<PageElementLocation>(Enumerable.Where<PageElementLocation>(this.ContentCapturedPositions, (PageElementLocation x) => x.Id == id));
		}
	}
}
