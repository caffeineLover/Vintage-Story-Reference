using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	internal interface IDocumentCanvas
	{
		// Token: 0x060001B3 RID: 435
		[NullableContext(2)]
		void SetSemanticTree(SemanticTreeNode semanticTree);

		// Token: 0x060001B4 RID: 436
		void BeginDocument();

		// Token: 0x060001B5 RID: 437
		void EndDocument();

		// Token: 0x060001B6 RID: 438
		void BeginPage(Size size);

		// Token: 0x060001B7 RID: 439
		void EndPage();

		// Token: 0x060001B8 RID: 440
		IDrawingCanvas GetDrawingCanvas();
	}
}
