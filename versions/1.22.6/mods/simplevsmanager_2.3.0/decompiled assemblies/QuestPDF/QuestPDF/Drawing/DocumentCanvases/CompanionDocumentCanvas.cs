using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Companion;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000125 RID: 293
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CompanionDocumentCanvas : IDocumentCanvas, IDisposable
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0001A8F2 File Offset: 0x00018AF2
		private ProxyDrawingCanvas DrawingCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0001A8FA File Offset: 0x00018AFA
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x0001A902 File Offset: 0x00018B02
		private Size CurrentPageSize { get; set; } = Size.Zero;

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0001A90B File Offset: 0x00018B0B
		private ICollection<CompanionPageSnapshot> PageSnapshots { get; } = new List<CompanionPageSnapshot>();

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0001A913 File Offset: 0x00018B13
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x0001A91B File Offset: 0x00018B1B
		internal CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement Hierarchy { get; set; }

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001A924 File Offset: 0x00018B24
		public CompanionDocumentSnapshot GetContent()
		{
			return new CompanionDocumentSnapshot
			{
				Pictures = this.PageSnapshots,
				Hierarchy = this.Hierarchy
			};
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001A944 File Offset: 0x00018B44
		~CompanionDocumentCanvas()
		{
			this.WarnThatFinalizerIsReached<CompanionDocumentCanvas>();
			this.Dispose();
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0001A978 File Offset: 0x00018B78
		public void Dispose()
		{
			this.DrawingCanvas.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0001A98B File Offset: 0x00018B8B
		[NullableContext(2)]
		public void SetSemanticTree(SemanticTreeNode semanticTree)
		{
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0001A98D File Offset: 0x00018B8D
		public void BeginDocument()
		{
			this.PageSnapshots.Clear();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0001A99A File Offset: 0x00018B9A
		public void EndDocument()
		{
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0001A99C File Offset: 0x00018B9C
		public void BeginPage(Size size)
		{
			this.CurrentPageSize = size;
			this.DrawingCanvas.Target = new SkiaDrawingCanvas(size.Width, size.Height);
			this.DrawingCanvas.SetZIndex(0);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0001A9D0 File Offset: 0x00018BD0
		public void EndPage()
		{
			using (SkPictureRecorder skPictureRecorder = new SkPictureRecorder())
			{
				using (SkCanvas skCanvas = skPictureRecorder.BeginRecording(this.CurrentPageSize.Width, this.CurrentPageSize.Height))
				{
					using (DocumentPageSnapshot snapshot = this.DrawingCanvas.GetSnapshot())
					{
						snapshot.DrawOnSkCanvas(skCanvas);
						skCanvas.Save();
						SkPicture picture = skPictureRecorder.EndRecording();
						this.PageSnapshots.Add(new CompanionPageSnapshot(picture, this.CurrentPageSize));
					}
				}
			}
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0001AA80 File Offset: 0x00018C80
		public IDrawingCanvas GetDrawingCanvas()
		{
			return this.DrawingCanvas;
		}
	}
}
