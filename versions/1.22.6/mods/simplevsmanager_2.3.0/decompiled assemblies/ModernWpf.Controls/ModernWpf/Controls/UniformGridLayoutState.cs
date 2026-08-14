using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x020000C3 RID: 195
	public class UniformGridLayoutState
	{
		// Token: 0x060009A8 RID: 2472 RVA: 0x00021FFD File Offset: 0x000201FD
		internal void InitializeForContext(VirtualizingLayoutContext context, IFlowLayoutAlgorithmDelegates callbacks)
		{
			this.FlowAlgorithm.InitializeForContext(context, callbacks);
			((ILayoutContextOverrides)context).LayoutStateCore = this;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00022013 File Offset: 0x00020213
		internal void UninitializeForContext(VirtualizingLayoutContext context)
		{
			this.FlowAlgorithm.UninitializeForContext(context);
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00022021 File Offset: 0x00020221
		internal FlowLayoutAlgorithm FlowAlgorithm { get; } = new FlowLayoutAlgorithm();

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00022029 File Offset: 0x00020229
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x00022031 File Offset: 0x00020231
		internal double EffectiveItemWidth { get; private set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0002203A File Offset: 0x0002023A
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x00022042 File Offset: 0x00020242
		internal double EffectiveItemHeight { get; private set; }

		// Token: 0x060009AF RID: 2479 RVA: 0x0002204C File Offset: 0x0002024C
		internal void EnsureElementSize(Size availableSize, VirtualizingLayoutContext context, double layoutItemWidth, double LayoutItemHeight, UniformGridLayoutItemsStretch stretch, Orientation orientation, double minRowSpacing, double minColumnSpacing, uint maxItemsPerLine)
		{
			if (maxItemsPerLine == 0U)
			{
				maxItemsPerLine = 1U;
			}
			if (context.ItemCount > 0)
			{
				UIElement elementIfRealized = this.FlowAlgorithm.GetElementIfRealized(0);
				if (elementIfRealized != null)
				{
					elementIfRealized.Measure(availableSize);
					this.SetSize(elementIfRealized.DesiredSize, layoutItemWidth, LayoutItemHeight, availableSize, stretch, orientation, minRowSpacing, minColumnSpacing, maxItemsPerLine);
					return;
				}
				UIElement orCreateElementAt = context.GetOrCreateElementAt(0, ElementRealizationOptions.ForceCreate);
				if (orCreateElementAt != null)
				{
					orCreateElementAt.Measure(availableSize);
					this.SetSize(orCreateElementAt.DesiredSize, layoutItemWidth, LayoutItemHeight, availableSize, stretch, orientation, minRowSpacing, minColumnSpacing, maxItemsPerLine);
					context.RecycleElement(orCreateElementAt);
				}
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000220D0 File Offset: 0x000202D0
		private void SetSize(Size desiredItemSize, double layoutItemWidth, double LayoutItemHeight, Size availableSize, UniformGridLayoutItemsStretch stretch, Orientation orientation, double minRowSpacing, double minColumnSpacing, uint maxItemsPerLine)
		{
			if (maxItemsPerLine == 0U)
			{
				maxItemsPerLine = 1U;
			}
			this.EffectiveItemWidth = (double.IsNaN(layoutItemWidth) ? desiredItemSize.Width : layoutItemWidth);
			this.EffectiveItemHeight = (double.IsNaN(LayoutItemHeight) ? desiredItemSize.Height : LayoutItemHeight);
			double num = (orientation == null) ? availableSize.Width : availableSize.Height;
			double num2 = (orientation == 1) ? minRowSpacing : minColumnSpacing;
			double num3 = (orientation == null) ? this.EffectiveItemWidth : this.EffectiveItemHeight;
			double num4 = 0.0;
			if (!double.IsInfinity(num))
			{
				uint num5 = Math.Min(maxItemsPerLine, (uint)Math.Max(1.0, num / (num3 + num2)));
				if (num5 == 0U)
				{
					num5 = 1U;
				}
				double num6 = num5 * (num3 + num2) - num2;
				num4 = (double)((int)(num - num6) / (int)num5);
			}
			if (stretch != UniformGridLayoutItemsStretch.Fill)
			{
				if (stretch == UniformGridLayoutItemsStretch.Uniform)
				{
					double num7 = ((orientation == null) ? this.EffectiveItemHeight : this.EffectiveItemWidth) * (num4 / num3);
					if (orientation == null)
					{
						this.EffectiveItemWidth += num4;
						this.EffectiveItemHeight += num7;
						return;
					}
					this.EffectiveItemHeight += num4;
					this.EffectiveItemWidth += num7;
				}
				return;
			}
			if (orientation == null)
			{
				this.EffectiveItemWidth += num4;
				return;
			}
			this.EffectiveItemHeight += num4;
		}
	}
}
