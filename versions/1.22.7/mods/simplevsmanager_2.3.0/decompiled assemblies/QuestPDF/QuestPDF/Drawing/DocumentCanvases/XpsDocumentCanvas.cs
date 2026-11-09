using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x0200012A RID: 298
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class XpsDocumentCanvas : IDocumentCanvas, IDisposable
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0001B6AE File Offset: 0x000198AE
		private SkDocument Document { get; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0001B6B6 File Offset: 0x000198B6
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x0001B6BE File Offset: 0x000198BE
		[Nullable(2)]
		private SkCanvas CurrentPageCanvas { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0001B6C7 File Offset: 0x000198C7
		private ProxyDrawingCanvas DrawingCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001B6CF File Offset: 0x000198CF
		public XpsDocumentCanvas(SkWriteStream stream, DocumentSettings documentSettings)
		{
			this.Document = XpsDocumentCanvas.CreateXps(stream, documentSettings);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001B6F0 File Offset: 0x000198F0
		private static SkDocument CreateXps(SkWriteStream stream, DocumentSettings documentSettings)
		{
			SkDocument result;
			try
			{
				result = SkXpsDocument.Create(stream, (float)documentSettings.ImageRasterDpi);
			}
			catch (TypeInitializationException inner)
			{
				throw new InitializationException("XPS", inner);
			}
			return result;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001B72C File Offset: 0x0001992C
		~XpsDocumentCanvas()
		{
			this.WarnThatFinalizerIsReached<XpsDocumentCanvas>();
			this.Dispose();
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0001B760 File Offset: 0x00019960
		public void Dispose()
		{
			SkDocument document = this.Document;
			if (document != null)
			{
				document.Dispose();
			}
			SkCanvas currentPageCanvas = this.CurrentPageCanvas;
			if (currentPageCanvas != null)
			{
				currentPageCanvas.Dispose();
			}
			ProxyDrawingCanvas drawingCanvas = this.DrawingCanvas;
			if (drawingCanvas != null)
			{
				drawingCanvas.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0001B79B File Offset: 0x0001999B
		[NullableContext(2)]
		public void SetSemanticTree(SemanticTreeNode semanticTree)
		{
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0001B79D File Offset: 0x0001999D
		public void BeginDocument()
		{
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001B79F File Offset: 0x0001999F
		public void EndDocument()
		{
			SkDocument document = this.Document;
			if (document != null)
			{
				document.Close();
			}
			SkDocument document2 = this.Document;
			if (document2 == null)
			{
				return;
			}
			document2.Dispose();
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0001B7C4 File Offset: 0x000199C4
		public void BeginPage(Size size)
		{
			SkDocument document = this.Document;
			this.CurrentPageCanvas = ((document != null) ? document.BeginPage(size.Width, size.Height) : null);
			this.DrawingCanvas.Target = new SkiaDrawingCanvas(size.Width, size.Height);
			this.DrawingCanvas.SetZIndex(0);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001B820 File Offset: 0x00019A20
		public void EndPage()
		{
			using (DocumentPageSnapshot snapshot = this.DrawingCanvas.GetSnapshot())
			{
				snapshot.DrawOnSkCanvas(this.CurrentPageCanvas);
				this.CurrentPageCanvas.Save();
				this.CurrentPageCanvas.Dispose();
				this.CurrentPageCanvas = null;
				this.Document.EndPage();
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001B88C File Offset: 0x00019A8C
		public IDrawingCanvas GetDrawingCanvas()
		{
			return this.DrawingCanvas;
		}
	}
}
