using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000059 RID: 89
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MergedDocument : IDocument
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00005437 File Offset: 0x00003637
		internal IReadOnlyList<IDocument> Documents { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000543F File Offset: 0x0000363F
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00005447 File Offset: 0x00003647
		internal MergedDocumentPageNumberStrategy PageNumberStrategy { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00005450 File Offset: 0x00003650
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00005458 File Offset: 0x00003658
		internal DocumentMetadata Metadata { get; private set; } = DocumentMetadata.Default;

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00005461 File Offset: 0x00003661
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00005469 File Offset: 0x00003669
		internal DocumentSettings Settings { get; private set; } = DocumentSettings.Default;

		// Token: 0x06000212 RID: 530 RVA: 0x00005472 File Offset: 0x00003672
		internal MergedDocument(IEnumerable<IDocument> documents)
		{
			List<IDocument> list = (documents != null) ? Enumerable.ToList<IDocument>(documents) : null;
			if (list == null)
			{
				throw new NullReferenceException("documents");
			}
			this.Documents = list;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000054B4 File Offset: 0x000036B4
		public void Compose(IDocumentContainer container)
		{
			foreach (IDocument document in this.Documents)
			{
				document.Compose(container);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00005500 File Offset: 0x00003700
		public DocumentMetadata GetMetadata()
		{
			return this.Metadata;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00005508 File Offset: 0x00003708
		public DocumentSettings GetSettings()
		{
			return this.Settings;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00005510 File Offset: 0x00003710
		public MergedDocument UseOriginalPageNumbers()
		{
			this.PageNumberStrategy = MergedDocumentPageNumberStrategy.Original;
			return this;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000551A File Offset: 0x0000371A
		public MergedDocument UseContinuousPageNumbers()
		{
			this.PageNumberStrategy = MergedDocumentPageNumberStrategy.Continuous;
			return this;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00005524 File Offset: 0x00003724
		public MergedDocument WithMetadata(DocumentMetadata metadata)
		{
			this.Metadata = (metadata ?? this.Metadata);
			return this;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00005538 File Offset: 0x00003738
		public MergedDocument WithSettings(DocumentSettings settings)
		{
			this.Settings = (settings ?? this.Settings);
			return this;
		}
	}
}
