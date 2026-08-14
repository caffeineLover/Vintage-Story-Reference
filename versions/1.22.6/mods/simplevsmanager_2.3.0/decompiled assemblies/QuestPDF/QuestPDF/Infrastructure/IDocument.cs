using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000048 RID: 72
	[NullableContext(1)]
	public interface IDocument
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00004EE9 File Offset: 0x000030E9
		DocumentMetadata GetMetadata()
		{
			return DocumentMetadata.Default;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00004EF0 File Offset: 0x000030F0
		DocumentSettings GetSettings()
		{
			return DocumentSettings.Default;
		}

		// Token: 0x060001B2 RID: 434
		void Compose(IDocumentContainer container);
	}
}
