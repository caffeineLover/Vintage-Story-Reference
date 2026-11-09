using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000127 RID: 295
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ImageDocumentCanvas : IDocumentCanvas, IDisposable
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0001AADE File Offset: 0x00018CDE
		private ImageGenerationSettings Settings { get; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001AAE6 File Offset: 0x00018CE6
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0001AAEE File Offset: 0x00018CEE
		private SkBitmap Bitmap { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0001AAF7 File Offset: 0x00018CF7
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x0001AAFF File Offset: 0x00018CFF
		[Nullable(2)]
		private SkCanvas CurrentPageCanvas { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0001AB08 File Offset: 0x00018D08
		private ProxyDrawingCanvas DrawingCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0001AB10 File Offset: 0x00018D10
		internal ICollection<byte[]> Images { get; } = new List<byte[]>();

		// Token: 0x06000A64 RID: 2660 RVA: 0x0001AB18 File Offset: 0x00018D18
		public ImageDocumentCanvas(ImageGenerationSettings settings)
		{
			this.Settings = settings;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001AB40 File Offset: 0x00018D40
		~ImageDocumentCanvas()
		{
			this.WarnThatFinalizerIsReached<ImageDocumentCanvas>();
			this.Dispose();
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0001AB74 File Offset: 0x00018D74
		public void Dispose()
		{
			SkCanvas currentPageCanvas = this.CurrentPageCanvas;
			if (currentPageCanvas != null)
			{
				currentPageCanvas.Dispose();
			}
			SkBitmap bitmap = this.Bitmap;
			if (bitmap != null)
			{
				bitmap.Dispose();
			}
			ProxyDrawingCanvas drawingCanvas = this.DrawingCanvas;
			if (drawingCanvas != null)
			{
				drawingCanvas.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0001ABAF File Offset: 0x00018DAF
		[NullableContext(2)]
		public void SetSemanticTree(SemanticTreeNode semanticTree)
		{
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0001ABB1 File Offset: 0x00018DB1
		public void BeginDocument()
		{
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0001ABB3 File Offset: 0x00018DB3
		public void EndDocument()
		{
			SkCanvas currentPageCanvas = this.CurrentPageCanvas;
			if (currentPageCanvas != null)
			{
				currentPageCanvas.Dispose();
			}
			SkBitmap bitmap = this.Bitmap;
			if (bitmap == null)
			{
				return;
			}
			bitmap.Dispose();
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0001ABD8 File Offset: 0x00018DD8
		public void BeginPage(Size size)
		{
			float num = (float)this.Settings.RasterDpi / 72f;
			this.Bitmap = new SkBitmap((int)(size.Width * num), (int)(size.Height * num));
			this.CurrentPageCanvas = SkCanvas.CreateFromBitmap(this.Bitmap);
			this.CurrentPageCanvas.Scale(num, num);
			if (this.Settings.ImageFormat == ImageFormat.Jpeg)
			{
				using (SkPaint skPaint = new SkPaint())
				{
					skPaint.SetSolidColor(Colors.White);
					this.CurrentPageCanvas.DrawRectangle(new SkRect(0f, 0f, size.Width, size.Height), skPaint);
				}
			}
			this.DrawingCanvas.Target = new SkiaDrawingCanvas(size.Width, size.Height);
			this.DrawingCanvas.SetZIndex(0);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0001ACC4 File Offset: 0x00018EC4
		public void EndPage()
		{
			using (DocumentPageSnapshot snapshot = this.DrawingCanvas.GetSnapshot())
			{
				snapshot.DrawOnSkCanvas(this.CurrentPageCanvas);
				this.CurrentPageCanvas.Save();
				this.CurrentPageCanvas.Dispose();
				this.CurrentPageCanvas = null;
				using (SkData skData = this.<EndPage>g__EncodeBitmap|24_0())
				{
					byte[] array = skData.ToBytes();
					this.Images.Add(array);
					this.Bitmap.Dispose();
				}
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0001AD60 File Offset: 0x00018F60
		public IDrawingCanvas GetDrawingCanvas()
		{
			return this.DrawingCanvas;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0001AD68 File Offset: 0x00018F68
		[CompilerGenerated]
		private SkData <EndPage>g__EncodeBitmap|24_0()
		{
			SkData result;
			switch (this.Settings.ImageFormat)
			{
			case ImageFormat.Jpeg:
				result = this.Bitmap.EncodeAsJpeg(this.Settings.ImageCompressionQuality.ToQualityValue());
				break;
			case ImageFormat.Png:
				result = this.Bitmap.EncodeAsPng();
				break;
			case ImageFormat.Webp:
				result = this.Bitmap.EncodeAsWebp(this.Settings.ImageCompressionQuality.ToQualityValue());
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}
	}
}
