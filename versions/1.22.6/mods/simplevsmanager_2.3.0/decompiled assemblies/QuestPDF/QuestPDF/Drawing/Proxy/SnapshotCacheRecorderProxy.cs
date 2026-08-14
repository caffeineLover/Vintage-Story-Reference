using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x02000119 RID: 281
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SnapshotCacheRecorderProxy : ElementProxy, IDisposable
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00019C28 File Offset: 0x00017E28
		private ProxyDrawingCanvas RecorderCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x00019C30 File Offset: 0x00017E30
		[TupleElementNames(new string[]
		{
			"pageNumber",
			"availableWidth",
			"availableHeight"
		})]
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private Dictionary<ValueTuple<int, float, float>, SpacePlan> MeasureCache { [return: TupleElementNames(new string[]
		{
			"pageNumber",
			"availableWidth",
			"availableHeight"
		})] [return: Nullable(new byte[]
		{
			1,
			0
		})] get; } = new Dictionary<ValueTuple<int, float, float>, SpacePlan>();

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00019C38 File Offset: 0x00017E38
		private Dictionary<int, DocumentPageSnapshot> DrawCache { get; } = new Dictionary<int, DocumentPageSnapshot>();

		// Token: 0x060009B8 RID: 2488 RVA: 0x00019C40 File Offset: 0x00017E40
		~SnapshotCacheRecorderProxy()
		{
			this.WarnThatFinalizerIsReached<SnapshotCacheRecorderProxy>();
			this.Dispose();
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00019C74 File Offset: 0x00017E74
		public void Dispose()
		{
			ProxyDrawingCanvas recorderCanvas = this.RecorderCanvas;
			if (recorderCanvas != null)
			{
				recorderCanvas.Dispose();
			}
			foreach (DocumentPageSnapshot documentPageSnapshot in this.DrawCache.Values)
			{
				documentPageSnapshot.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00019CE0 File Offset: 0x00017EE0
		public SnapshotCacheRecorderProxy(Element child)
		{
			base.Child = child;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00019D10 File Offset: 0x00017F10
		private void Initialize()
		{
			if (base.Child.Canvas == this.RecorderCanvas)
			{
				return;
			}
			base.Child.VisitChildren(delegate(Element x)
			{
				x.Canvas = this.RecorderCanvas;
			});
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00019D40 File Offset: 0x00017F40
		internal override SpacePlan Measure(Size availableSpace)
		{
			this.Initialize();
			ValueTuple<int, float, float> valueTuple = new ValueTuple<int, float, float>(base.PageContext.CurrentPage, availableSpace.Width, availableSpace.Height);
			SpacePlan result;
			if (this.MeasureCache.TryGetValue(valueTuple, ref result))
			{
				return result;
			}
			this.RecorderCanvas.Target = new DiscardDrawingCanvas();
			SpacePlan spacePlan = base.Measure(availableSpace);
			this.RecorderCanvas.Target = null;
			this.MeasureCache[valueTuple] = spacePlan;
			return spacePlan;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00019DB8 File Offset: 0x00017FB8
		internal override void Draw(Size availableSpace)
		{
			DocumentPageSnapshot documentPageSnapshot;
			if (this.DrawCache.TryGetValue(base.PageContext.CurrentPage, ref documentPageSnapshot))
			{
				base.Canvas.DrawSnapshot(documentPageSnapshot);
				documentPageSnapshot.Dispose();
				this.DrawCache.Remove(base.PageContext.CurrentPage);
				return;
			}
			using (SkiaDrawingCanvas skiaDrawingCanvas = new SkiaDrawingCanvas(Size.Max.Width, Size.Max.Height))
			{
				this.RecorderCanvas.Target = skiaDrawingCanvas;
				this.RecorderCanvas.SetZIndex(base.Canvas.GetZIndex());
				this.RecorderCanvas.SetMatrix(base.Canvas.GetCurrentMatrix());
				base.Draw(availableSpace);
				this.DrawCache[base.PageContext.CurrentPage] = skiaDrawingCanvas.GetSnapshot();
				this.RecorderCanvas.Target = null;
			}
		}
	}
}
