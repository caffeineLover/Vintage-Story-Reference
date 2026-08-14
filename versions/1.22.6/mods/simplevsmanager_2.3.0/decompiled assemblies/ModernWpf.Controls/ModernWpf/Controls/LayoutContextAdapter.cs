using System;
using System.Collections.Generic;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B9 RID: 185
	internal class LayoutContextAdapter : VirtualizingLayoutContext
	{
		// Token: 0x06000936 RID: 2358 RVA: 0x00020B51 File Offset: 0x0001ED51
		public LayoutContextAdapter(NonVirtualizingLayoutContext nonVirtualizingContext)
		{
			this.m_nonVirtualizingContext = new WeakReference<NonVirtualizingLayoutContext>(nonVirtualizingContext);
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00020B68 File Offset: 0x0001ED68
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00020B8C File Offset: 0x0001ED8C
		protected override object LayoutStateCore
		{
			get
			{
				NonVirtualizingLayoutContext nonVirtualizingLayoutContext;
				if (this.m_nonVirtualizingContext.TryGetTarget(ref nonVirtualizingLayoutContext))
				{
					return nonVirtualizingLayoutContext.LayoutState;
				}
				return null;
			}
			set
			{
				NonVirtualizingLayoutContext nonVirtualizingLayoutContext;
				if (this.m_nonVirtualizingContext.TryGetTarget(ref nonVirtualizingLayoutContext))
				{
					((ILayoutContextOverrides)nonVirtualizingLayoutContext).LayoutStateCore = value;
				}
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00020BB0 File Offset: 0x0001EDB0
		protected override int ItemCountCore()
		{
			NonVirtualizingLayoutContext nonVirtualizingLayoutContext;
			if (this.m_nonVirtualizingContext.TryGetTarget(ref nonVirtualizingLayoutContext))
			{
				return nonVirtualizingLayoutContext.Children.Count;
			}
			return 0;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00020BDC File Offset: 0x0001EDDC
		protected override object GetItemAtCore(int index)
		{
			NonVirtualizingLayoutContext nonVirtualizingLayoutContext;
			if (this.m_nonVirtualizingContext.TryGetTarget(ref nonVirtualizingLayoutContext))
			{
				return nonVirtualizingLayoutContext.Children[index];
			}
			return null;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00020C08 File Offset: 0x0001EE08
		protected override UIElement GetOrCreateElementAtCore(int index, ElementRealizationOptions options)
		{
			NonVirtualizingLayoutContext nonVirtualizingLayoutContext;
			if (this.m_nonVirtualizingContext.TryGetTarget(ref nonVirtualizingLayoutContext))
			{
				return nonVirtualizingLayoutContext.Children[index];
			}
			return null;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00020C32 File Offset: 0x0001EE32
		protected override void RecycleElementCore(UIElement element)
		{
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00020C34 File Offset: 0x0001EE34
		protected int GetElementIndexCore(UIElement element)
		{
			NonVirtualizingLayoutContext nonVirtualizingLayoutContext;
			if (this.m_nonVirtualizingContext.TryGetTarget(ref nonVirtualizingLayoutContext))
			{
				IReadOnlyList<UIElement> children = nonVirtualizingLayoutContext.Children;
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i] == element)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00020C75 File Offset: 0x0001EE75
		protected override Rect RealizationRectCore()
		{
			return new Rect(0.0, 0.0, double.PositiveInfinity, double.PositiveInfinity);
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00020CA0 File Offset: 0x0001EEA0
		protected override int RecommendedAnchorIndexCore
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x00020CA3 File Offset: 0x0001EEA3
		// (set) Token: 0x06000941 RID: 2369 RVA: 0x00020CBC File Offset: 0x0001EEBC
		protected override Point LayoutOriginCore
		{
			get
			{
				return new Point(0.0, 0.0);
			}
			set
			{
				if (value != new Point(0.0, 0.0))
				{
					throw new ArgumentOutOfRangeException("value", "LayoutOrigin must be at (0,0) when RealizationRect is infinite sized.");
				}
			}
		}

		// Token: 0x040003E4 RID: 996
		private readonly WeakReference<NonVirtualizingLayoutContext> m_nonVirtualizingContext;
	}
}
