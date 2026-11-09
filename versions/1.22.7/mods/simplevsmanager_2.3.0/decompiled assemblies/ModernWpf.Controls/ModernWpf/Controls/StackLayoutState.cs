using System;
using System.Collections.Generic;

namespace ModernWpf.Controls
{
	// Token: 0x020000BF RID: 191
	public class StackLayoutState
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x000213F3 File Offset: 0x0001F5F3
		internal void InitializeForContext(VirtualizingLayoutContext context, IFlowLayoutAlgorithmDelegates callbacks)
		{
			this.FlowAlgorithm.InitializeForContext(context, callbacks);
			if (this.m_estimationBuffer.Count == 0)
			{
				this.m_estimationBuffer.Resize(100, 0.0);
			}
			((ILayoutContextOverrides)context).LayoutStateCore = this;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002142C File Offset: 0x0001F62C
		internal void UninitializeForContext(VirtualizingLayoutContext context)
		{
			this.FlowAlgorithm.UninitializeForContext(context);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002143C File Offset: 0x0001F63C
		internal void OnElementMeasured(int elementIndex, double majorSize, double minorSize)
		{
			int num = elementIndex % this.m_estimationBuffer.Count;
			if (this.m_estimationBuffer[num] == 0.0)
			{
				int totalElementsMeasured = this.TotalElementsMeasured;
				this.TotalElementsMeasured = totalElementsMeasured + 1;
			}
			this.TotalElementSize -= this.m_estimationBuffer[num];
			this.TotalElementSize += majorSize;
			this.m_estimationBuffer[num] = majorSize;
			this.MaxArrangeBounds = Math.Max(this.MaxArrangeBounds, minorSize);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000214C9 File Offset: 0x0001F6C9
		internal void OnMeasureStart()
		{
			this.MaxArrangeBounds = 0.0;
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x000214DA File Offset: 0x0001F6DA
		internal FlowLayoutAlgorithm FlowAlgorithm { get; } = new FlowLayoutAlgorithm();

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x000214E2 File Offset: 0x0001F6E2
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x000214EA File Offset: 0x0001F6EA
		internal double TotalElementSize { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x000214F3 File Offset: 0x0001F6F3
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x000214FB File Offset: 0x0001F6FB
		internal double MaxArrangeBounds { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00021504 File Offset: 0x0001F704
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0002150C File Offset: 0x0001F70C
		internal int TotalElementsMeasured { get; private set; }

		// Token: 0x040003F1 RID: 1009
		private readonly List<double> m_estimationBuffer = new List<double>();

		// Token: 0x040003F2 RID: 1010
		private const int BufferSize = 100;
	}
}
