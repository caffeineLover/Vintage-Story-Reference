using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000A5 RID: 165
	internal class RepeaterLayoutContext : VirtualizingLayoutContext
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x0001C881 File Offset: 0x0001AA81
		public RepeaterLayoutContext(ItemsRepeater owner)
		{
			this.m_owner = new WeakReference<ItemsRepeater>(owner);
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0001C895 File Offset: 0x0001AA95
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0001C8A2 File Offset: 0x0001AAA2
		protected override object LayoutStateCore
		{
			get
			{
				return this.GetOwner().LayoutState;
			}
			set
			{
				this.GetOwner().LayoutState = value;
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001C8B0 File Offset: 0x0001AAB0
		protected override int ItemCountCore()
		{
			ItemsSourceView itemsSourceView = this.GetOwner().ItemsSourceView;
			if (itemsSourceView != null)
			{
				return itemsSourceView.Count;
			}
			return 0;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
		protected override UIElement GetOrCreateElementAtCore(int index, ElementRealizationOptions options)
		{
			return this.GetOwner().GetElementImpl(index, (options & ElementRealizationOptions.ForceCreate) == ElementRealizationOptions.ForceCreate, (options & ElementRealizationOptions.SuppressAutoRecycle) == ElementRealizationOptions.SuppressAutoRecycle);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001C8EE File Offset: 0x0001AAEE
		protected override object GetItemAtCore(int index)
		{
			return this.GetOwner().ItemsSourceView.GetAt(index);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001C901 File Offset: 0x0001AB01
		protected override void RecycleElementCore(UIElement element)
		{
			this.GetOwner().ClearElementImpl(element);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001C90F File Offset: 0x0001AB0F
		protected override Rect RealizationRectCore()
		{
			return this.GetOwner().RealizationWindow;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001C91C File Offset: 0x0001AB1C
		protected override int RecommendedAnchorIndexCore
		{
			get
			{
				int result = -1;
				ItemsRepeater owner = this.GetOwner();
				UIElement suggestedAnchor = owner.SuggestedAnchor;
				if (suggestedAnchor != null)
				{
					result = owner.GetElementIndex(suggestedAnchor);
				}
				return result;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0001C945 File Offset: 0x0001AB45
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0001C952 File Offset: 0x0001AB52
		protected override Point LayoutOriginCore
		{
			get
			{
				return this.GetOwner().LayoutOrigin;
			}
			set
			{
				this.GetOwner().LayoutOrigin = value;
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001C960 File Offset: 0x0001AB60
		private ItemsRepeater GetOwner()
		{
			ItemsRepeater result;
			this.m_owner.TryGetTarget(ref result);
			return result;
		}

		// Token: 0x04000395 RID: 917
		private readonly WeakReference<ItemsRepeater> m_owner;
	}
}
