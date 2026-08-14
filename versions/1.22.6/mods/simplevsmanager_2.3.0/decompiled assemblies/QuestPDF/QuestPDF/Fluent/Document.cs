using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Fluent
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Document : IDocument
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x0000AE2D File Offset: 0x0000902D
		static Document()
		{
			SkNativeDependencyCompatibilityChecker.Test();
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000AE34 File Offset: 0x00009034
		private Action<IDocumentContainer> ContentSource { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000AE3C File Offset: 0x0000903C
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0000AE44 File Offset: 0x00009044
		private DocumentMetadata Metadata { get; set; } = DocumentMetadata.Default;

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000AE4D File Offset: 0x0000904D
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0000AE55 File Offset: 0x00009055
		private DocumentSettings Settings { get; set; } = DocumentSettings.Default;

		// Token: 0x06000419 RID: 1049 RVA: 0x0000AE5E File Offset: 0x0000905E
		private Document(Action<IDocumentContainer> contentSource)
		{
			this.ContentSource = contentSource;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000AE83 File Offset: 0x00009083
		public static Document Create(Action<IDocumentContainer> handler)
		{
			return new Document(handler);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000AE8B File Offset: 0x0000908B
		public Document WithMetadata(DocumentMetadata metadata)
		{
			this.Metadata = (metadata ?? this.Metadata);
			return this;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000AE9F File Offset: 0x0000909F
		public Document WithSettings(DocumentSettings settings)
		{
			this.Settings = (settings ?? this.Settings);
			return this;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000AEB3 File Offset: 0x000090B3
		public static MergedDocument Merge(IEnumerable<IDocument> documents)
		{
			return new MergedDocument(documents);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000AEBB File Offset: 0x000090BB
		public static MergedDocument Merge(params IDocument[] documents)
		{
			return new MergedDocument(documents);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000AEC3 File Offset: 0x000090C3
		public DocumentMetadata GetMetadata()
		{
			return this.Metadata;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000AECB File Offset: 0x000090CB
		public DocumentSettings GetSettings()
		{
			return this.Settings;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000AED3 File Offset: 0x000090D3
		public void Compose(IDocumentContainer container)
		{
			this.ContentSource.Invoke(container);
		}
	}
}
