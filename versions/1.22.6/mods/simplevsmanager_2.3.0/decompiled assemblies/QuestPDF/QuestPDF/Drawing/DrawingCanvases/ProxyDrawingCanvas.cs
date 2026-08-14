using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Drawing.DrawingCanvases
{
	// Token: 0x02000121 RID: 289
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ProxyDrawingCanvas : IDrawingCanvas, IDisposable
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0001A08C File Offset: 0x0001828C
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x0001A094 File Offset: 0x00018294
		public IDrawingCanvas Target { get; set; }

		// Token: 0x060009F1 RID: 2545 RVA: 0x0001A0A0 File Offset: 0x000182A0
		~ProxyDrawingCanvas()
		{
			this.WarnThatFinalizerIsReached<ProxyDrawingCanvas>();
			this.Dispose();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0001A0D4 File Offset: 0x000182D4
		public void Dispose()
		{
			IDisposable disposable = this.Target as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001A0F2 File Offset: 0x000182F2
		public DocumentPageSnapshot GetSnapshot()
		{
			return this.Target.GetSnapshot();
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001A0FF File Offset: 0x000182FF
		public void DrawSnapshot(DocumentPageSnapshot snapshot)
		{
			this.Target.DrawSnapshot(snapshot);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001A10D File Offset: 0x0001830D
		public void Save()
		{
			this.Target.Save();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0001A11A File Offset: 0x0001831A
		public void Restore()
		{
			this.Target.Restore();
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001A127 File Offset: 0x00018327
		public void SetZIndex(int index)
		{
			this.Target.SetZIndex(index);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0001A135 File Offset: 0x00018335
		public int GetZIndex()
		{
			return this.Target.GetZIndex();
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0001A142 File Offset: 0x00018342
		public SkCanvasMatrix GetCurrentMatrix()
		{
			return this.Target.GetCurrentMatrix();
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0001A14F File Offset: 0x0001834F
		public void SetMatrix(SkCanvasMatrix matrix)
		{
			this.Target.SetMatrix(matrix);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0001A15D File Offset: 0x0001835D
		public void Translate(Position vector)
		{
			this.Target.Translate(vector);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0001A16B File Offset: 0x0001836B
		public void Scale(float scaleX, float scaleY)
		{
			this.Target.Scale(scaleX, scaleY);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001A17A File Offset: 0x0001837A
		public void Rotate(float angle)
		{
			this.Target.Rotate(angle);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001A188 File Offset: 0x00018388
		public void DrawLine(Position start, Position end, SkPaint paint)
		{
			this.Target.DrawLine(start, end, paint);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001A198 File Offset: 0x00018398
		public void DrawRectangle(Position vector, Size size, SkPaint paint)
		{
			this.Target.DrawRectangle(vector, size, paint);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0001A1A8 File Offset: 0x000183A8
		public void DrawComplexBorder(SkRoundedRect innerRect, SkRoundedRect outerRect, SkPaint paint)
		{
			this.Target.DrawComplexBorder(innerRect, outerRect, paint);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001A1B8 File Offset: 0x000183B8
		public void DrawShadow(SkRoundedRect shadowRect, SkBoxShadow shadow)
		{
			this.Target.DrawShadow(shadowRect, shadow);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001A1C7 File Offset: 0x000183C7
		public void DrawParagraph(SkParagraph paragraph, int lineFrom, int lineTo)
		{
			this.Target.DrawParagraph(paragraph, lineFrom, lineTo);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001A1D7 File Offset: 0x000183D7
		public void DrawImage(SkImage image, Size size)
		{
			this.Target.DrawImage(image, size);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0001A1E6 File Offset: 0x000183E6
		public void DrawPicture(SkPicture picture)
		{
			this.Target.DrawPicture(picture);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0001A1F4 File Offset: 0x000183F4
		public void DrawSvgPath(string path, Color color)
		{
			this.Target.DrawSvgPath(path, color);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0001A203 File Offset: 0x00018403
		public void DrawSvg(SkSvgImage svgImage, Size size)
		{
			this.Target.DrawSvg(svgImage, size);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0001A212 File Offset: 0x00018412
		public void DrawOverflowArea(SkRect area)
		{
			this.Target.DrawOverflowArea(area);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0001A220 File Offset: 0x00018420
		public void ClipOverflowArea(SkRect availableSpace, SkRect requiredSpace)
		{
			this.Target.ClipOverflowArea(availableSpace, requiredSpace);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001A22F File Offset: 0x0001842F
		public void ClipRectangle(SkRect clipArea)
		{
			this.Target.ClipRectangle(clipArea);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001A23D File Offset: 0x0001843D
		public void ClipRoundedRectangle(SkRoundedRect clipArea)
		{
			this.Target.ClipRoundedRectangle(clipArea);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0001A24B File Offset: 0x0001844B
		public void DrawHyperlink(Size size, string url, [Nullable(2)] string description)
		{
			this.Target.DrawHyperlink(size, url, description);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0001A25B File Offset: 0x0001845B
		public void DrawSectionLink(Size size, string sectionName, [Nullable(2)] string description)
		{
			this.Target.DrawSectionLink(size, sectionName, description);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0001A26B File Offset: 0x0001846B
		public void DrawSection(string sectionName)
		{
			this.Target.DrawSection(sectionName);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0001A279 File Offset: 0x00018479
		public void SetSemanticNodeId(int nodeId)
		{
			this.Target.SetSemanticNodeId(nodeId);
		}
	}
}
