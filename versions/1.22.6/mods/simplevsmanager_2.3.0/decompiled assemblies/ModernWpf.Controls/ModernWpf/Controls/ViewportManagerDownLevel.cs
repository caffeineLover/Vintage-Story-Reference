using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ModernWpf.Controls
{
	// Token: 0x020000AD RID: 173
	internal class ViewportManagerDownLevel : ViewportManager
	{
		// Token: 0x06000879 RID: 2169 RVA: 0x0001D81E File Offset: 0x0001BA1E
		public ViewportManagerDownLevel(ItemsRepeater owner)
		{
			this.m_owner = owner;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001D858 File Offset: 0x0001BA58
		public override UIElement SuggestedAnchor
		{
			get
			{
				UIElement uielement = this.m_makeAnchorElement;
				UIElement owner = this.m_owner;
				if (uielement == null)
				{
					IRepeaterScrollingSurface innerScrollableScroller = this.m_innerScrollableScroller;
					UIElement uielement2 = (innerScrollableScroller != null) ? innerScrollableScroller.AnchorElement : null;
					if (uielement2 != null)
					{
						UIElement uielement3 = uielement2;
						for (UIElement uielement4 = CachedVisualTreeHelpers.GetParent(uielement3) as UIElement; uielement4 != null; uielement4 = (CachedVisualTreeHelpers.GetParent(uielement3) as UIElement))
						{
							if (uielement4 == owner)
							{
								uielement = uielement3;
								break;
							}
							uielement3 = uielement4;
						}
					}
				}
				return uielement;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0001D8BB File Offset: 0x0001BABB
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x0001D8C3 File Offset: 0x0001BAC3
		public override double HorizontalCacheLength
		{
			get
			{
				return this.m_maximumHorizontalCacheLength;
			}
			set
			{
				if (this.m_maximumHorizontalCacheLength != value)
				{
					this.ValidateCacheLength(value);
					this.m_maximumHorizontalCacheLength = value;
					this.ResetCacheBuffer();
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001D8E2 File Offset: 0x0001BAE2
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x0001D8EA File Offset: 0x0001BAEA
		public override double VerticalCacheLength
		{
			get
			{
				return this.m_maximumVerticalCacheLength;
			}
			set
			{
				if (this.m_maximumVerticalCacheLength != value)
				{
					this.ValidateCacheLength(value);
					this.m_maximumVerticalCacheLength = value;
					this.ResetCacheBuffer();
				}
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001D90C File Offset: 0x0001BB0C
		public override Rect GetLayoutVisibleWindow()
		{
			Rect visibleWindow = this.m_visibleWindow;
			if (this.m_makeAnchorElement != null && this.m_isAnchorOutsideRealizedRange)
			{
				visibleWindow.X = 0.0;
				visibleWindow.Y = 0.0;
			}
			else if (this.HasScrollers)
			{
				visibleWindow.X += this.m_layoutExtent.X + this.m_expectedViewportShift.X;
				visibleWindow.Y += this.m_layoutExtent.Y + this.m_expectedViewportShift.Y;
			}
			return visibleWindow;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001D9A8 File Offset: 0x0001BBA8
		public override Rect GetLayoutRealizationWindow()
		{
			Rect layoutVisibleWindow = this.GetLayoutVisibleWindow();
			if (this.HasScrollers)
			{
				layoutVisibleWindow.X -= this.m_horizontalCacheBufferPerSide;
				layoutVisibleWindow.Y -= this.m_verticalCacheBufferPerSide;
				layoutVisibleWindow.Width += this.m_horizontalCacheBufferPerSide * 2.0;
				layoutVisibleWindow.Height += this.m_verticalCacheBufferPerSide * 2.0;
			}
			return layoutVisibleWindow;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001DA2C File Offset: 0x0001BC2C
		public override void SetLayoutExtent(Rect extent)
		{
			this.m_expectedViewportShift.X = this.m_expectedViewportShift.X + (this.m_layoutExtent.X - extent.X);
			this.m_expectedViewportShift.Y = this.m_expectedViewportShift.Y + (this.m_layoutExtent.Y - extent.Y);
			this.m_layoutExtent = extent;
			IRepeaterScrollingSurface outerScroller = this.GetOuterScroller();
			if (outerScroller != null)
			{
				((UIElement)outerScroller).InvalidateArrange();
			}
			if (this.m_horizontalScroller != null && this.m_horizontalScroller != outerScroller)
			{
				((UIElement)this.m_horizontalScroller).InvalidateArrange();
			}
			if (this.m_verticalScroller != null && this.m_verticalScroller != outerScroller)
			{
				((UIElement)this.m_verticalScroller).InvalidateArrange();
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001DAE1 File Offset: 0x0001BCE1
		public override Point GetOrigin()
		{
			return new Point(this.m_layoutExtent.X, this.m_layoutExtent.Y);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001DAFE File Offset: 0x0001BCFE
		public override void OnLayoutChanged(bool isVirtualizing)
		{
			this.m_managingViewportDisabled = !isVirtualizing;
			this.m_layoutExtent = default(Rect);
			this.m_expectedViewportShift = default(Point);
			this.ResetCacheBuffer();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001DB28 File Offset: 0x0001BD28
		public override void OnElementPrepared(UIElement element)
		{
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001DB2A File Offset: 0x0001BD2A
		public override void OnElementCleared(UIElement element)
		{
			if (this.m_horizontalScroller != null)
			{
				this.m_horizontalScroller.UnregisterAnchorCandidate(element);
			}
			if (this.m_verticalScroller != null && this.m_verticalScroller != this.m_horizontalScroller)
			{
				this.m_verticalScroller.UnregisterAnchorCandidate(element);
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001DB62 File Offset: 0x0001BD62
		public override void OnOwnerMeasuring()
		{
			if (this.m_managingViewportDisabled)
			{
				return;
			}
			this.EnsureScrollers();
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001DB74 File Offset: 0x0001BD74
		public override void OnOwnerArranged()
		{
			if (this.m_managingViewportDisabled)
			{
				return;
			}
			this.m_expectedViewportShift = default(Point);
			if (this.HasScrollers)
			{
				double num = this.m_maximumHorizontalCacheLength * this.m_visibleWindow.Width / 2.0;
				double num2 = this.m_maximumVerticalCacheLength * this.m_visibleWindow.Height / 2.0;
				if (this.m_horizontalCacheBufferPerSide < num || this.m_verticalCacheBufferPerSide < num2)
				{
					this.m_horizontalCacheBufferPerSide += 40.0;
					this.m_verticalCacheBufferPerSide += 40.0;
					this.m_horizontalCacheBufferPerSide = Math.Min(this.m_horizontalCacheBufferPerSide, num);
					this.m_verticalCacheBufferPerSide = Math.Min(this.m_verticalCacheBufferPerSide, num2);
					this.RegisterCacheBuildWork();
				}
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001DC48 File Offset: 0x0001BE48
		public override void OnMakeAnchor(UIElement anchor, bool isAnchorOutsideRealizedRange)
		{
			this.m_makeAnchorElement = anchor;
			this.m_isAnchorOutsideRealizedRange = isAnchorOutsideRealizedRange;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001DC58 File Offset: 0x0001BE58
		public override void ResetScrollers()
		{
			this.m_parentScrollers.Clear();
			this.m_horizontalScroller = null;
			this.m_verticalScroller = null;
			this.m_innerScrollableScroller = null;
			this.m_ensuredScrollers = false;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0001DC81 File Offset: 0x0001BE81
		public override UIElement MadeAnchor
		{
			get
			{
				return this.m_makeAnchorElement;
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001DC89 File Offset: 0x0001BE89
		private void OnCacheBuildActionCompleted()
		{
			this.m_cacheBuildAction = null;
			this.m_owner.InvalidateMeasure();
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001DC9D File Offset: 0x0001BE9D
		private void OnViewportChanged(IRepeaterScrollingSurface sender, bool isFinal)
		{
			if (!this.m_managingViewportDisabled)
			{
				if (isFinal)
				{
					this.m_makeAnchorElement = null;
					this.m_isAnchorOutsideRealizedRange = false;
				}
				this.TryInvalidateMeasure();
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001DCC0 File Offset: 0x0001BEC0
		private void OnPostArrange(IRepeaterScrollingSurface sender)
		{
			if (!this.m_managingViewportDisabled)
			{
				this.UpdateViewport();
				if (this.m_visibleWindow == default(Rect))
				{
					this.m_layoutExtent = default(Rect);
					return;
				}
				if (this.m_horizontalScroller != null || this.m_verticalScroller != null)
				{
					UIElementCollection children = this.m_owner.Children;
					for (int i = 0; i < children.Count; i++)
					{
						UIElement element = children[i];
						if (ItemsRepeater.GetVirtualizationInfo(element).IsHeldByLayout)
						{
							if (this.m_horizontalScroller != null)
							{
								this.m_horizontalScroller.RegisterAnchorCandidate(element);
							}
							if (this.m_verticalScroller != null && this.m_verticalScroller != this.m_horizontalScroller)
							{
								this.m_verticalScroller.RegisterAnchorCandidate(element);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001DD79 File Offset: 0x0001BF79
		private void OnConfigurationChanged(IRepeaterScrollingSurface sender)
		{
			this.m_ensuredScrollers = false;
			this.TryInvalidateMeasure();
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001DD88 File Offset: 0x0001BF88
		private void EnsureScrollers()
		{
			if (!this.m_ensuredScrollers)
			{
				this.ResetScrollers();
				for (DependencyObject parent = CachedVisualTreeHelpers.GetParent(this.m_owner); parent != null; parent = CachedVisualTreeHelpers.GetParent(parent))
				{
					IRepeaterScrollingSurface repeaterScrollingSurface = parent as IRepeaterScrollingSurface;
					if (repeaterScrollingSurface != null && this.AddScroller(repeaterScrollingSurface))
					{
						break;
					}
				}
				if (this.m_parentScrollers.Empty<ViewportManagerDownLevel.ScrollerInfo>())
				{
					this.UpdateViewport();
				}
				else
				{
					this.m_parentScrollers.Last<ViewportManagerDownLevel.ScrollerInfo>().Scroller.PostArrange += this.OnPostArrange;
				}
				this.m_ensuredScrollers = true;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001DE0E File Offset: 0x0001C00E
		private bool HasScrollers
		{
			get
			{
				return this.m_horizontalScroller != null || this.m_verticalScroller != null;
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001DE24 File Offset: 0x0001C024
		private bool AddScroller(IRepeaterScrollingSurface scroller)
		{
			bool isHorizontallyScrollable = scroller.IsHorizontallyScrollable;
			bool isVerticallyScrollable = scroller.IsVerticallyScrollable;
			bool result = (this.m_horizontalScroller != null || isHorizontallyScrollable) && (this.m_verticalScroller != null || isVerticallyScrollable);
			bool flag = this.m_horizontalScroller == null && isHorizontallyScrollable;
			bool flag2 = this.m_verticalScroller == null && isVerticallyScrollable;
			bool flag3 = this.m_innerScrollableScroller == null && (flag || flag2);
			if (flag)
			{
				this.m_horizontalScroller = scroller;
			}
			if (flag2)
			{
				this.m_verticalScroller = scroller;
			}
			if (flag3)
			{
				this.m_innerScrollableScroller = scroller;
			}
			ViewportManagerDownLevel.ScrollerInfo scrollerInfo = new ViewportManagerDownLevel.ScrollerInfo(scroller);
			scroller.ConfigurationChanged += this.OnConfigurationChanged;
			if (flag || flag2)
			{
				scroller.ViewportChanged += this.OnViewportChanged;
			}
			this.m_parentScrollers.Add(scrollerInfo);
			return result;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001DEE0 File Offset: 0x0001C0E0
		private void UpdateViewport()
		{
			Rect visibleWindow = this.m_visibleWindow;
			Rect rect = (this.m_horizontalScroller != null) ? this.m_horizontalScroller.GetRelativeViewport(this.m_owner) : default(Rect);
			Rect rect2 = (this.m_verticalScroller != null) ? ((this.m_verticalScroller == this.m_horizontalScroller) ? rect : this.m_verticalScroller.GetRelativeViewport(this.m_owner)) : default(Rect);
			Rect visibleWindow2 = this.HasScrollers ? new Rect((this.m_horizontalScroller != null) ? rect.X : rect2.X, (this.m_verticalScroller != null) ? rect2.Y : rect.Y, (this.m_horizontalScroller != null) ? rect.Width : rect2.Width, (this.m_verticalScroller != null) ? rect2.Height : rect.Height) : new Rect(0.0, 0.0, double.MaxValue, double.MaxValue);
			if (-visibleWindow2.X <= ItemsRepeater.ClearedElementsArrangePosition.X && -visibleWindow2.Y <= ItemsRepeater.ClearedElementsArrangePosition.Y)
			{
				this.m_visibleWindow = default(Rect);
			}
			else
			{
				this.m_visibleWindow = visibleWindow2;
			}
			if (Math.Abs(this.m_visibleWindow.X - visibleWindow.X) > 1.0 || Math.Abs(this.m_visibleWindow.Y - visibleWindow.Y) > 1.0 || this.m_visibleWindow.Width != visibleWindow.Width || this.m_visibleWindow.Height != visibleWindow.Height)
			{
				this.TryInvalidateMeasure();
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001E0AB File Offset: 0x0001C2AB
		private void ResetCacheBuffer()
		{
			this.m_horizontalCacheBufferPerSide = 0.0;
			this.m_verticalCacheBufferPerSide = 0.0;
			if (!this.m_managingViewportDisabled)
			{
				this.RegisterCacheBuildWork();
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001E0D9 File Offset: 0x0001C2D9
		private void ValidateCacheLength(double cacheLength)
		{
			if (cacheLength < 0.0 || double.IsInfinity(cacheLength) || double.IsNaN(cacheLength))
			{
				throw new ArgumentOutOfRangeException("The maximum cache length must be equal or superior to zero.");
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001E104 File Offset: 0x0001C304
		private void RegisterCacheBuildWork()
		{
			if (this.m_owner.Layout != null && this.m_cacheBuildAction == null)
			{
				ItemsRepeater owner = this.m_owner;
				this.m_cacheBuildAction = this.m_owner.Dispatcher.BeginInvoke(delegate()
				{
					this.OnCacheBuildActionCompleted();
				}, 3, Array.Empty<object>());
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001E158 File Offset: 0x0001C358
		private void TryInvalidateMeasure()
		{
			if (this.m_visibleWindow != default(Rect))
			{
				this.m_owner.InvalidateMeasure();
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001E188 File Offset: 0x0001C388
		private IRepeaterScrollingSurface GetOuterScroller()
		{
			IRepeaterScrollingSurface result = null;
			if (!this.m_parentScrollers.Empty<ViewportManagerDownLevel.ScrollerInfo>())
			{
				result = this.m_parentScrollers.Last<ViewportManagerDownLevel.ScrollerInfo>().Scroller;
			}
			return result;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001E1BC File Offset: 0x0001C3BC
		private string GetLayoutId()
		{
			string result = null;
			Layout layout = this.m_owner.Layout;
			if (layout != null)
			{
				result = layout.LayoutId;
			}
			return result;
		}

		// Token: 0x040003A5 RID: 933
		private const double CacheBufferPerSideInflationPixelDelta = 40.0;

		// Token: 0x040003A6 RID: 934
		private readonly ItemsRepeater m_owner;

		// Token: 0x040003A7 RID: 935
		private bool m_ensuredScrollers;

		// Token: 0x040003A8 RID: 936
		private readonly List<ViewportManagerDownLevel.ScrollerInfo> m_parentScrollers = new List<ViewportManagerDownLevel.ScrollerInfo>();

		// Token: 0x040003A9 RID: 937
		private IRepeaterScrollingSurface m_horizontalScroller;

		// Token: 0x040003AA RID: 938
		private IRepeaterScrollingSurface m_verticalScroller;

		// Token: 0x040003AB RID: 939
		private IRepeaterScrollingSurface m_innerScrollableScroller;

		// Token: 0x040003AC RID: 940
		private UIElement m_makeAnchorElement;

		// Token: 0x040003AD RID: 941
		private bool m_isAnchorOutsideRealizedRange;

		// Token: 0x040003AE RID: 942
		private DispatcherOperation m_cacheBuildAction;

		// Token: 0x040003AF RID: 943
		private Rect m_visibleWindow;

		// Token: 0x040003B0 RID: 944
		private Rect m_layoutExtent;

		// Token: 0x040003B1 RID: 945
		private Point m_expectedViewportShift;

		// Token: 0x040003B2 RID: 946
		private double m_maximumHorizontalCacheLength = 2.0;

		// Token: 0x040003B3 RID: 947
		private double m_maximumVerticalCacheLength = 2.0;

		// Token: 0x040003B4 RID: 948
		private double m_horizontalCacheBufferPerSide;

		// Token: 0x040003B5 RID: 949
		private double m_verticalCacheBufferPerSide;

		// Token: 0x040003B6 RID: 950
		private bool m_managingViewportDisabled;

		// Token: 0x0200011F RID: 287
		private struct ScrollerInfo
		{
			// Token: 0x06000CE4 RID: 3300 RVA: 0x0002B69C File Offset: 0x0002989C
			public ScrollerInfo(IRepeaterScrollingSurface scroller)
			{
				this.Scroller = scroller;
			}

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0002B6A5 File Offset: 0x000298A5
			public readonly IRepeaterScrollingSurface Scroller { get; }
		}
	}
}
