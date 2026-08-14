using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000129 RID: 297
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SvgDocumentCanvas : IDocumentCanvas, IDisposable
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0001B44C File Offset: 0x0001964C
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x0001B454 File Offset: 0x00019654
		[Nullable(2)]
		private SkCanvas CurrentPageCanvas { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0001B45D File Offset: 0x0001965D
		private ProxyDrawingCanvas DrawingCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0001B465 File Offset: 0x00019665
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0001B46D File Offset: 0x0001966D
		private MemoryStream WriteStream { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0001B476 File Offset: 0x00019676
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0001B47E File Offset: 0x0001967E
		private SkWriteStream SkiaStream { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0001B487 File Offset: 0x00019687
		internal ICollection<string> Images { get; } = new List<string>();

		// Token: 0x06000A8D RID: 2701 RVA: 0x0001B490 File Offset: 0x00019690
		~SvgDocumentCanvas()
		{
			this.WarnThatFinalizerIsReached<SvgDocumentCanvas>();
			this.Dispose();
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0001B4C4 File Offset: 0x000196C4
		public void Dispose()
		{
			SkCanvas currentPageCanvas = this.CurrentPageCanvas;
			if (currentPageCanvas != null)
			{
				currentPageCanvas.Dispose();
			}
			MemoryStream writeStream = this.WriteStream;
			if (writeStream != null)
			{
				writeStream.Dispose();
			}
			SkWriteStream skiaStream = this.SkiaStream;
			if (skiaStream != null)
			{
				skiaStream.Dispose();
			}
			ProxyDrawingCanvas drawingCanvas = this.DrawingCanvas;
			if (drawingCanvas != null)
			{
				drawingCanvas.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0001B51B File Offset: 0x0001971B
		[NullableContext(2)]
		public void SetSemanticTree(SemanticTreeNode semanticTree)
		{
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0001B51D File Offset: 0x0001971D
		public void BeginDocument()
		{
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0001B51F File Offset: 0x0001971F
		public void EndDocument()
		{
			SkCanvas currentPageCanvas = this.CurrentPageCanvas;
			if (currentPageCanvas != null)
			{
				currentPageCanvas.Dispose();
			}
			MemoryStream writeStream = this.WriteStream;
			if (writeStream != null)
			{
				writeStream.Dispose();
			}
			SkWriteStream skiaStream = this.SkiaStream;
			if (skiaStream == null)
			{
				return;
			}
			skiaStream.Dispose();
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0001B554 File Offset: 0x00019754
		public void BeginPage(Size size)
		{
			MemoryStream writeStream = this.WriteStream;
			if (writeStream != null)
			{
				writeStream.Dispose();
			}
			SkWriteStream skiaStream = this.SkiaStream;
			if (skiaStream != null)
			{
				skiaStream.Dispose();
			}
			this.WriteStream = new MemoryStream();
			this.SkiaStream = new SkWriteStream(this.WriteStream);
			this.CurrentPageCanvas = SkSvgCanvas.CreateSvg(size.Width, size.Height, this.SkiaStream);
			this.DrawingCanvas.Target = new SkiaDrawingCanvas(size.Width, size.Height);
			this.DrawingCanvas.SetZIndex(0);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001B5E4 File Offset: 0x000197E4
		public void EndPage()
		{
			using (DocumentPageSnapshot snapshot = this.DrawingCanvas.GetSnapshot())
			{
				snapshot.DrawOnSkCanvas(this.CurrentPageCanvas);
				this.CurrentPageCanvas.Save();
				this.CurrentPageCanvas.Dispose();
				this.CurrentPageCanvas = null;
				this.SkiaStream.Flush();
				byte[] array = this.WriteStream.ToArray();
				string @string = Encoding.UTF8.GetString(array);
				this.Images.Add(@string);
				this.SkiaStream.Dispose();
				this.WriteStream.Dispose();
			}
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001B688 File Offset: 0x00019888
		public IDrawingCanvas GetDrawingCanvas()
		{
			return this.DrawingCanvas;
		}
	}
}
