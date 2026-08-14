using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000126 RID: 294
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class FreeDocumentCanvas : IDocumentCanvas
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0001AAB1 File Offset: 0x00018CB1
		private DiscardDrawingCanvas DrawingCanvas { get; } = new DiscardDrawingCanvas();

		// Token: 0x06000A56 RID: 2646 RVA: 0x0001AAB9 File Offset: 0x00018CB9
		[NullableContext(2)]
		public void SetSemanticTree(SemanticTreeNode semanticTree)
		{
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0001AABB File Offset: 0x00018CBB
		public void BeginDocument()
		{
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001AABD File Offset: 0x00018CBD
		public void EndDocument()
		{
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0001AABF File Offset: 0x00018CBF
		public void BeginPage(Size size)
		{
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0001AAC1 File Offset: 0x00018CC1
		public void EndPage()
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0001AAC3 File Offset: 0x00018CC3
		public IDrawingCanvas GetDrawingCanvas()
		{
			return this.DrawingCanvas;
		}
	}
}
