using System;
using System.Collections.Generic;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B2 RID: 178
	public class FlowLayoutState
	{
		// Token: 0x060008F6 RID: 2294 RVA: 0x0002050C File Offset: 0x0001E70C
		internal void InitializeForContext(VirtualizingLayoutContext context, IFlowLayoutAlgorithmDelegates callbacks)
		{
			this.FlowAlgorithm.InitializeForContext(context, callbacks);
			if (this.m_lineSizeEstimationBuffer.Count == 0)
			{
				this.m_lineSizeEstimationBuffer.Resize(FlowLayoutState.BufferSize, 0.0);
				this.m_itemsPerLineEstimationBuffer.Resize(FlowLayoutState.BufferSize, 0.0);
			}
			((ILayoutContextOverrides)context).LayoutStateCore = this;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002056C File Offset: 0x0001E76C
		internal void UninitializeForContext(VirtualizingLayoutContext context)
		{
			this.FlowAlgorithm.UninitializeForContext(context);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002057C File Offset: 0x0001E77C
		internal void OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context)
		{
			if (this.TotalLinesMeasured == 0 || startIndex + countInLine != context.ItemCount)
			{
				int num = startIndex % this.m_lineSizeEstimationBuffer.Count;
				if (this.m_lineSizeEstimationBuffer[num] == 0.0)
				{
					int totalLinesMeasured = this.TotalLinesMeasured + 1;
					this.TotalLinesMeasured = totalLinesMeasured;
				}
				this.TotalLineSize -= this.m_lineSizeEstimationBuffer[num];
				this.TotalLineSize += lineSize;
				this.m_lineSizeEstimationBuffer[num] = lineSize;
				this.TotalItemsPerLine -= this.m_itemsPerLineEstimationBuffer[num];
				this.TotalItemsPerLine += (double)countInLine;
				this.m_itemsPerLineEstimationBuffer[num] = (double)countInLine;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00020644 File Offset: 0x0001E844
		internal FlowLayoutAlgorithm FlowAlgorithm { get; } = new FlowLayoutAlgorithm();

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0002064C File Offset: 0x0001E84C
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00020654 File Offset: 0x0001E854
		internal double TotalLineSize { get; private set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0002065D File Offset: 0x0001E85D
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00020665 File Offset: 0x0001E865
		internal int TotalLinesMeasured { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0002066E File Offset: 0x0001E86E
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00020676 File Offset: 0x0001E876
		internal double TotalItemsPerLine { get; private set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0002067F File Offset: 0x0001E87F
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00020687 File Offset: 0x0001E887
		internal Size SpecialElementDesiredSize { get; set; }

		// Token: 0x040003DA RID: 986
		private readonly List<double> m_lineSizeEstimationBuffer = new List<double>();

		// Token: 0x040003DB RID: 987
		private readonly List<double> m_itemsPerLineEstimationBuffer = new List<double>();

		// Token: 0x040003DC RID: 988
		private static readonly int BufferSize = 100;
	}
}
