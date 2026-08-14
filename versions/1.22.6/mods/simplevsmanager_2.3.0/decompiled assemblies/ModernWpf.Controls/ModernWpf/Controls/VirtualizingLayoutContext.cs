using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000C8 RID: 200
	public class VirtualizingLayoutContext : LayoutContext, IVirtualizingLayoutContextOverrides
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x00022471 File Offset: 0x00020671
		public object GetItemAt(int index)
		{
			return this.GetItemAtCore(index);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002247A File Offset: 0x0002067A
		public UIElement GetOrCreateElementAt(int index)
		{
			return this.GetOrCreateElementAtCore(index, ElementRealizationOptions.None);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00022484 File Offset: 0x00020684
		public UIElement GetOrCreateElementAt(int index, ElementRealizationOptions options)
		{
			return this.GetOrCreateElementAtCore(index, options);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0002248E File Offset: 0x0002068E
		public void RecycleElement(UIElement element)
		{
			this.RecycleElementCore(element);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00022497 File Offset: 0x00020697
		protected virtual int ItemCountCore()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0002249E File Offset: 0x0002069E
		protected virtual object GetItemAtCore(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000224A5 File Offset: 0x000206A5
		protected virtual Rect RealizationRectCore()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000224AC File Offset: 0x000206AC
		protected virtual UIElement GetOrCreateElementAtCore(int index, ElementRealizationOptions options)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000224B3 File Offset: 0x000206B3
		protected virtual void RecycleElementCore(UIElement element)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x000224BA File Offset: 0x000206BA
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x000224C2 File Offset: 0x000206C2
		public Point LayoutOrigin
		{
			get
			{
				return this.LayoutOriginCore;
			}
			set
			{
				this.LayoutOriginCore = value;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x000224CB File Offset: 0x000206CB
		public int ItemCount
		{
			get
			{
				return this.ItemCountCore();
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x000224D3 File Offset: 0x000206D3
		public Rect RealizationRect
		{
			get
			{
				return this.RealizationRectCore();
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x000224DB File Offset: 0x000206DB
		public int RecommendedAnchorIndex
		{
			get
			{
				return this.RecommendedAnchorIndexCore;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x000224E4 File Offset: 0x000206E4
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x000224F6 File Offset: 0x000206F6
		protected virtual Point LayoutOriginCore
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x000224FD File Offset: 0x000206FD
		protected virtual int RecommendedAnchorIndexCore
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00022504 File Offset: 0x00020704
		internal NonVirtualizingLayoutContext GetNonVirtualizingContextAdapter()
		{
			if (this.m_contextAdapter == null)
			{
				this.m_contextAdapter = new VirtualLayoutContextAdapter(this);
			}
			return this.m_contextAdapter;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00022520 File Offset: 0x00020720
		int IVirtualizingLayoutContextOverrides.ItemCountCore()
		{
			return this.ItemCountCore();
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00022528 File Offset: 0x00020728
		object IVirtualizingLayoutContextOverrides.GetItemAtCore(int index)
		{
			return this.GetItemAtCore(index);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00022531 File Offset: 0x00020731
		UIElement IVirtualizingLayoutContextOverrides.GetOrCreateElementAtCore(int index, ElementRealizationOptions options)
		{
			return this.GetOrCreateElementAtCore(index, options);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002253B File Offset: 0x0002073B
		void IVirtualizingLayoutContextOverrides.RecycleElementCore(UIElement element)
		{
			this.RecycleElementCore(element);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00022544 File Offset: 0x00020744
		Rect IVirtualizingLayoutContextOverrides.RealizationRectCore()
		{
			return this.RealizationRectCore();
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0002254C File Offset: 0x0002074C
		int IVirtualizingLayoutContextOverrides.RecommendedAnchorIndexCore
		{
			get
			{
				return this.RecommendedAnchorIndexCore;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00022554 File Offset: 0x00020754
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0002255C File Offset: 0x0002075C
		Point IVirtualizingLayoutContextOverrides.LayoutOriginCore
		{
			get
			{
				return this.LayoutOriginCore;
			}
			set
			{
				this.LayoutOriginCore = value;
			}
		}

		// Token: 0x04000420 RID: 1056
		private NonVirtualizingLayoutContext m_contextAdapter;
	}
}
