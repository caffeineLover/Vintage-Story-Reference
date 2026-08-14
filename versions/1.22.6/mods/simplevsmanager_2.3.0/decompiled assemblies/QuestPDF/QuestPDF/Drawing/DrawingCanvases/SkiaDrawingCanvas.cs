using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Drawing.DrawingCanvases
{
	// Token: 0x02000122 RID: 290
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkiaDrawingCanvas : IDrawingCanvas, IDisposable
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001A28F File Offset: 0x0001848F
		public float Width { get; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0001A297 File Offset: 0x00018497
		public float Height { get; }

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001A29F File Offset: 0x0001849F
		public SkiaDrawingCanvas(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001A2C0 File Offset: 0x000184C0
		~SkiaDrawingCanvas()
		{
			this.Dispose();
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0001A2EC File Offset: 0x000184EC
		public void Dispose()
		{
			SkCanvas currentCanvas = this.CurrentCanvas;
			if (currentCanvas != null)
			{
				currentCanvas.Dispose();
			}
			this.CurrentCanvas = null;
			foreach (ValueTuple<SkPictureRecorder, SkCanvas> valueTuple in this.ZIndexCanvases.Values)
			{
				valueTuple.Item2.Dispose();
				valueTuple.Item1.Dispose();
			}
			this.ZIndexCanvases.Clear();
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0001A374 File Offset: 0x00018574
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0001A37C File Offset: 0x0001857C
		private SkCanvas CurrentCanvas { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0001A385 File Offset: 0x00018585
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0001A38D File Offset: 0x0001858D
		private int CurrentZIndex { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0001A396 File Offset: 0x00018596
		[TupleElementNames(new string[]
		{
			"PictureRecorder",
			"Canvas"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private IDictionary<int, ValueTuple<SkPictureRecorder, SkCanvas>> ZIndexCanvases { [return: TupleElementNames(new string[]
		{
			"PictureRecorder",
			"Canvas"
		})] [return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] get; } = new Dictionary<int, ValueTuple<SkPictureRecorder, SkCanvas>>();

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001A3A0 File Offset: 0x000185A0
		private SkCanvas GetCanvasForZIndex(int zIndex)
		{
			ValueTuple<SkPictureRecorder, SkCanvas> valueTuple;
			if (this.ZIndexCanvases.TryGetValue(zIndex, ref valueTuple))
			{
				return valueTuple.Item2;
			}
			SkPictureRecorder skPictureRecorder = new SkPictureRecorder();
			SkCanvas skCanvas = skPictureRecorder.BeginRecording(this.Width, this.Height);
			this.ZIndexCanvases.Add(zIndex, new ValueTuple<SkPictureRecorder, SkCanvas>(skPictureRecorder, skCanvas));
			return skCanvas;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001A3F1 File Offset: 0x000185F1
		public DocumentPageSnapshot GetSnapshot()
		{
			DocumentPageSnapshot documentPageSnapshot = new DocumentPageSnapshot();
			documentPageSnapshot.Layers = Enumerable.ToList<DocumentPageSnapshot.LayerSnapshot>(Enumerable.Select<KeyValuePair<int, ValueTuple<SkPictureRecorder, SkCanvas>>, DocumentPageSnapshot.LayerSnapshot>(this.ZIndexCanvases, delegate([TupleElementNames(new string[]
			{
				"PictureRecorder",
				"Canvas"
			})] KeyValuePair<int, ValueTuple<SkPictureRecorder, SkCanvas>> zindex)
			{
				DocumentPageSnapshot.LayerSnapshot result;
				using (SkPictureRecorder item = zindex.Value.Item1)
				{
					SkPicture picture = item.EndRecording();
					zindex.Value.Item2.Dispose();
					result = new DocumentPageSnapshot.LayerSnapshot
					{
						ZIndex = zindex.Key,
						Picture = picture
					};
				}
				return result;
			}));
			return documentPageSnapshot;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001A430 File Offset: 0x00018630
		public void DrawSnapshot(DocumentPageSnapshot snapshot)
		{
			foreach (DocumentPageSnapshot.LayerSnapshot layerSnapshot in Enumerable.OrderBy<DocumentPageSnapshot.LayerSnapshot, int>(snapshot.Layers, (DocumentPageSnapshot.LayerSnapshot x) => x.ZIndex))
			{
				SkCanvas canvasForZIndex = this.GetCanvasForZIndex(layerSnapshot.ZIndex);
				canvasForZIndex.Save();
				canvasForZIndex.SetCurrentMatrix(SkCanvasMatrix.Identity);
				canvasForZIndex.DrawPicture(layerSnapshot.Picture);
				canvasForZIndex.Restore();
			}
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0001A4C8 File Offset: 0x000186C8
		public void Save()
		{
			this.CurrentCanvas.Save();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0001A4D5 File Offset: 0x000186D5
		public void Restore()
		{
			this.CurrentCanvas.Restore();
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001A4E4 File Offset: 0x000186E4
		public void SetZIndex(int index)
		{
			SkCanvas currentCanvas = this.CurrentCanvas;
			SkCanvasMatrix currentMatrix = (currentCanvas != null) ? currentCanvas.GetCurrentMatrix() : SkCanvasMatrix.Identity;
			this.CurrentZIndex = index;
			this.CurrentCanvas = this.GetCanvasForZIndex(this.CurrentZIndex);
			this.CurrentCanvas.SetCurrentMatrix(currentMatrix);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001A52D File Offset: 0x0001872D
		public int GetZIndex()
		{
			return this.CurrentZIndex;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001A535 File Offset: 0x00018735
		public SkCanvasMatrix GetCurrentMatrix()
		{
			return this.CurrentCanvas.GetCurrentMatrix();
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001A542 File Offset: 0x00018742
		public void SetMatrix(SkCanvasMatrix matrix)
		{
			this.CurrentCanvas.SetCurrentMatrix(matrix);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001A550 File Offset: 0x00018750
		public void Translate(Position vector)
		{
			this.CurrentCanvas.Translate(vector.X, vector.Y);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001A569 File Offset: 0x00018769
		public void Scale(float scaleX, float scaleY)
		{
			this.CurrentCanvas.Scale(scaleX, scaleY);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0001A578 File Offset: 0x00018778
		public void Rotate(float angle)
		{
			this.CurrentCanvas.Rotate(angle);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001A588 File Offset: 0x00018788
		public void DrawLine(Position start, Position end, SkPaint paint)
		{
			SkPoint start2 = new SkPoint(start.X, start.Y);
			SkPoint end2 = new SkPoint(end.X, end.Y);
			this.CurrentCanvas.DrawLine(start2, end2, paint);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0001A5CC File Offset: 0x000187CC
		public void DrawRectangle(Position vector, Size size, SkPaint paint)
		{
			SkRect position = new SkRect(vector.X, vector.Y, vector.X + size.Width, vector.Y + size.Height);
			this.CurrentCanvas.DrawRectangle(position, paint);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0001A613 File Offset: 0x00018813
		public void DrawComplexBorder(SkRoundedRect innerRect, SkRoundedRect outerRect, SkPaint paint)
		{
			this.CurrentCanvas.DrawComplexBorder(innerRect, outerRect, paint);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0001A623 File Offset: 0x00018823
		public void DrawShadow(SkRoundedRect shadowRect, SkBoxShadow shadow)
		{
			this.CurrentCanvas.DrawShadow(shadowRect, shadow);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001A632 File Offset: 0x00018832
		public void DrawParagraph(SkParagraph paragraph, int lineFrom, int lineTo)
		{
			this.CurrentCanvas.DrawParagraph(paragraph, new int?(lineFrom), new int?(lineTo));
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0001A64C File Offset: 0x0001884C
		public void DrawImage(SkImage image, Size size)
		{
			this.CurrentCanvas.DrawImage(image, size.Width, size.Height);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0001A666 File Offset: 0x00018866
		public void DrawPicture(SkPicture picture)
		{
			this.CurrentCanvas.DrawPicture(picture);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0001A674 File Offset: 0x00018874
		public void DrawSvgPath(string path, Color color)
		{
			this.CurrentCanvas.DrawSvgPath(path, color);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0001A688 File Offset: 0x00018888
		public void DrawSvg(SkSvgImage svgImage, Size size)
		{
			this.CurrentCanvas.DrawSvg(svgImage, size.Width, size.Height);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0001A6A2 File Offset: 0x000188A2
		public void DrawOverflowArea(SkRect area)
		{
			this.CurrentCanvas.DrawOverflowArea(area);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001A6B0 File Offset: 0x000188B0
		public void ClipOverflowArea(SkRect availableSpace, SkRect requiredSpace)
		{
			this.CurrentCanvas.ClipOverflowArea(availableSpace, requiredSpace);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001A6BF File Offset: 0x000188BF
		public void ClipRectangle(SkRect clipArea)
		{
			this.CurrentCanvas.ClipRectangle(clipArea);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001A6CD File Offset: 0x000188CD
		public void ClipRoundedRectangle(SkRoundedRect clipArea)
		{
			this.CurrentCanvas.ClipRoundedRectangle(clipArea);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001A6DB File Offset: 0x000188DB
		public void DrawHyperlink(Size size, string url, [Nullable(2)] string description)
		{
			this.CurrentCanvas.AnnotateUrl(size.Width, size.Height, url, description);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0001A6F6 File Offset: 0x000188F6
		public void DrawSectionLink(Size size, string sectionName, [Nullable(2)] string description)
		{
			this.CurrentCanvas.AnnotateDestinationLink(size.Width, size.Height, sectionName, description);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001A711 File Offset: 0x00018911
		public void DrawSection(string sectionName)
		{
			this.CurrentCanvas.AnnotateDestination(sectionName);
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0001A71F File Offset: 0x0001891F
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x0001A727 File Offset: 0x00018927
		private int ArtifactNestingDepth { get; set; }

		// Token: 0x06000A38 RID: 2616 RVA: 0x0001A730 File Offset: 0x00018930
		public void MarkCurrentContentAsArtifact(bool isArtifact)
		{
			this.ArtifactNestingDepth += (isArtifact ? 1 : -1);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0001A748 File Offset: 0x00018948
		public void SetSemanticNodeId(int nodeId)
		{
			bool flag = this.ArtifactNestingDepth > 0;
			bool flag2 = nodeId < 0;
			if (flag && !flag2)
			{
				return;
			}
			this.CurrentCanvas.SetSemanticNodeId(nodeId);
		}
	}
}
