using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x0200009B RID: 155
	[ContentProperty("ScrollViewer")]
	public class ItemsRepeaterScrollHost : Panel, IRepeaterScrollingSurface
	{
		// Token: 0x060007AA RID: 1962 RVA: 0x0001B490 File Offset: 0x00019690
		public ItemsRepeaterScrollHost()
		{
			this.m_pendingBringIntoView = new ItemsRepeaterScrollHost.BringIntoViewState(this);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001B4B8 File Offset: 0x000196B8
		protected override Size MeasureOverride(Size availableSize)
		{
			Size result = default(Size);
			ScrollViewer scrollViewer = this.ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.Measure(availableSize);
				result = scrollViewer.DesiredSize;
			}
			return result;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001B4E8 File Offset: 0x000196E8
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = finalSize;
			ScrollViewer scrollViewer = this.ScrollViewer;
			if (scrollViewer != null)
			{
				object obj = scrollViewer != null && this.HasPendingBringIntoView && !this.m_pendingBringIntoView.ChangeViewCalled;
				Rect previousBounds = default(Rect);
				object obj2 = obj;
				UIElement uielement = (obj2 != null) ? null : this.GetAnchorElement(ref previousBounds);
				scrollViewer.Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
				this.m_pendingViewportShift = 0.0;
				if (obj2 != null)
				{
					this.ApplyPendingChangeView(scrollViewer);
				}
				else if (uielement != null)
				{
					this.m_pendingViewportShift = this.TrackElement(uielement, previousBounds, scrollViewer);
				}
				else if (scrollViewer == null)
				{
					this.m_pendingBringIntoView.Reset();
				}
				this.m_candidates.Clear();
				this.m_isAnchorElementDirty = true;
				PostArrangeEventHandler postArrange = this.PostArrange;
				if (postArrange == null)
				{
					return result;
				}
				postArrange(this);
			}
			return result;
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0001B5C0 File Offset: 0x000197C0
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0001B5C8 File Offset: 0x000197C8
		public double HorizontalAnchorRatio
		{
			get
			{
				return this.m_horizontalEdge;
			}
			set
			{
				this.m_horizontalEdge = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001B5D1 File Offset: 0x000197D1
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0001B5D9 File Offset: 0x000197D9
		public double VerticalAnchorRatio
		{
			get
			{
				return this.m_verticalEdge;
			}
			set
			{
				this.m_verticalEdge = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0001B5E4 File Offset: 0x000197E4
		public UIElement CurrentAnchor
		{
			get
			{
				Rect empty = Rect.Empty;
				return this.GetAnchorElement(ref empty);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001B600 File Offset: 0x00019800
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0001B630 File Offset: 0x00019830
		public ScrollViewer ScrollViewer
		{
			get
			{
				ScrollViewer result = null;
				UIElementCollection children = base.Children;
				if (children.Count > 0)
				{
					result = (children[0] as ScrollViewer);
				}
				return result;
			}
			set
			{
				ScrollViewer scrollViewer = this.ScrollViewer;
				if (scrollViewer != null)
				{
					scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollViewerScrollChanged);
					scrollViewer.SizeChanged -= new SizeChangedEventHandler(this.OnScrollViewerSizeChanged);
				}
				UIElementCollection children = base.Children;
				children.Clear();
				children.Add(value);
				value.ScrollChanged += new ScrollChangedEventHandler(this.OnScrollViewerScrollChanged);
				value.SizeChanged += new SizeChangedEventHandler(this.OnScrollViewerSizeChanged);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001B6A2 File Offset: 0x000198A2
		bool IRepeaterScrollingSurface.IsHorizontallyScrollable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0001B6A5 File Offset: 0x000198A5
		bool IRepeaterScrollingSurface.IsVerticallyScrollable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001B6A8 File Offset: 0x000198A8
		UIElement IRepeaterScrollingSurface.AnchorElement
		{
			get
			{
				Rect empty = Rect.Empty;
				return this.GetAnchorElement(ref empty);
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060007B7 RID: 1975 RVA: 0x0001B6C3 File Offset: 0x000198C3
		// (remove) Token: 0x060007B8 RID: 1976 RVA: 0x0001B6CC File Offset: 0x000198CC
		event ViewportChangedEventHandler IRepeaterScrollingSurface.ViewportChanged
		{
			add
			{
				this.ViewportChanged += value;
			}
			remove
			{
				this.ViewportChanged -= value;
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060007B9 RID: 1977 RVA: 0x0001B6D5 File Offset: 0x000198D5
		// (remove) Token: 0x060007BA RID: 1978 RVA: 0x0001B6DE File Offset: 0x000198DE
		event PostArrangeEventHandler IRepeaterScrollingSurface.PostArrange
		{
			add
			{
				this.PostArrange += value;
			}
			remove
			{
				this.PostArrange -= value;
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060007BB RID: 1979 RVA: 0x0001B6E7 File Offset: 0x000198E7
		// (remove) Token: 0x060007BC RID: 1980 RVA: 0x0001B6F0 File Offset: 0x000198F0
		event ConfigurationChangedEventHandler IRepeaterScrollingSurface.ConfigurationChanged
		{
			add
			{
				this.ConfigurationChanged += value;
			}
			remove
			{
				this.ConfigurationChanged -= value;
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001B6F9 File Offset: 0x000198F9
		void IRepeaterScrollingSurface.RegisterAnchorCandidate(UIElement element)
		{
			if ((!double.IsNaN(this.HorizontalAnchorRatio) || !double.IsNaN(this.VerticalAnchorRatio)) && this.ScrollViewer != null)
			{
				this.m_candidates.Add(new ItemsRepeaterScrollHost.CandidateInfo(element));
				this.m_isAnchorElementDirty = true;
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001B738 File Offset: 0x00019938
		void IRepeaterScrollingSurface.UnregisterAnchorCandidate(UIElement element)
		{
			int num = this.m_candidates.FindIndex((ItemsRepeaterScrollHost.CandidateInfo c) => c.Element == element);
			if (num >= 0)
			{
				this.m_candidates.RemoveAt(num);
				this.m_isAnchorElementDirty = true;
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001B784 File Offset: 0x00019984
		Rect IRepeaterScrollingSurface.GetRelativeViewport(UIElement element)
		{
			ScrollViewer scrollViewer = this.ScrollViewer;
			if (scrollViewer != null)
			{
				bool hasPendingBringIntoView = this.HasPendingBringIntoView;
				GeneralTransform generalTransform = Extensions.SafeTransformToVisual(element, hasPendingBringIntoView ? scrollViewer.GetContentTemplateRoot() : scrollViewer);
				double num = 1.0;
				double num2 = scrollViewer.ViewportWidth / num;
				double num3 = scrollViewer.ViewportHeight / num;
				Point point = generalTransform.Transform(default(Point));
				point.X = -point.X;
				point.Y = -point.Y + this.m_pendingViewportShift;
				if (hasPendingBringIntoView)
				{
					point.X += this.m_pendingBringIntoView.ChangeViewOffset.X;
					point.Y += this.m_pendingBringIntoView.ChangeViewOffset.Y;
				}
				return new Rect(point.X, point.Y, num2, num3);
			}
			return default(Rect);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001B872 File Offset: 0x00019A72
		internal void StartBringIntoView(UIElement element, double alignmentX, double alignmentY, double offsetX, double offsetY, bool animate)
		{
			this.m_pendingBringIntoView = new ItemsRepeaterScrollHost.BringIntoViewState(element, alignmentX, alignmentY, offsetX, offsetY, animate);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001B888 File Offset: 0x00019A88
		private void ApplyPendingChangeView(ScrollViewer scrollViewer)
		{
			ItemsRepeaterScrollHost.BringIntoViewState pendingBringIntoView = this.m_pendingBringIntoView;
			pendingBringIntoView.ChangeViewCalled = true;
			Rect layoutSlot = CachedVisualTreeHelpers.GetLayoutSlot((FrameworkElement)pendingBringIntoView.TargetElement);
			Rect rect = Extensions.SafeTransformToVisual(pendingBringIntoView.TargetElement, scrollViewer.GetContentTemplateRoot()).TransformBounds(new Rect(0.0, 0.0, layoutSlot.Width, layoutSlot.Height));
			Point point;
			point..ctor(scrollViewer.ViewportWidth - rect.Width, scrollViewer.ViewportHeight - rect.Height);
			Point changeViewOffset;
			changeViewOffset..ctor(Math.Max(0.0, Math.Min(rect.X + pendingBringIntoView.OffsetX - point.X * pendingBringIntoView.AlignmentX, scrollViewer.ExtentWidth - scrollViewer.ViewportWidth)), Math.Max(0.0, Math.Min(rect.Y + pendingBringIntoView.OffsetY - point.Y * pendingBringIntoView.AlignmentY, scrollViewer.ExtentHeight - scrollViewer.ViewportHeight)));
			pendingBringIntoView.ChangeViewOffset = changeViewOffset;
			scrollViewer.ChangeView(new double?(changeViewOffset.X), new double?(changeViewOffset.Y), default(float?), !pendingBringIntoView.Animate);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001B9D0 File Offset: 0x00019BD0
		private double TrackElement(UIElement element, Rect previousBounds, ScrollViewer scrollViewer)
		{
			Rect layoutSlot = LayoutInformation.GetLayoutSlot((FrameworkElement)element);
			Rect rect = Extensions.SafeTransformToVisual(element, scrollViewer.GetContentTemplateRoot()).TransformBounds(new Rect(0.0, 0.0, layoutSlot.Width, layoutSlot.Height));
			double num = previousBounds.Y + this.HorizontalAnchorRatio * previousBounds.Height;
			double num2 = rect.Y + this.HorizontalAnchorRatio * rect.Height - num;
			double num3 = num2;
			double num4 = (this.HasPendingBringIntoView && !this.m_pendingBringIntoView.Animate) ? this.m_pendingBringIntoView.ChangeViewOffset.Y : scrollViewer.VerticalOffset;
			if (num4 + num3 < 0.0)
			{
				num3 = -num4;
			}
			else if (num4 + scrollViewer.ViewportHeight + num3 > scrollViewer.ExtentHeight)
			{
				num3 = scrollViewer.ExtentHeight - scrollViewer.ViewportHeight - num4;
			}
			if (scrollViewer.ScrollableHeight == 0.0)
			{
				num3 = 0.0;
			}
			if (Math.Abs(num3) > 1.0)
			{
				scrollViewer.ChangeView(default(double?), new double?(num4 + num3), default(float?), true);
			}
			else
			{
				num3 = 0.0;
				if (Math.Abs(num2) > 1.0)
				{
					ViewportChangedEventHandler viewportChanged = this.ViewportChanged;
					if (viewportChanged != null)
					{
						viewportChanged(this, true);
					}
				}
			}
			return num3;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001BB4C File Offset: 0x00019D4C
		private UIElement GetAnchorElement(ref Rect relativeBounds)
		{
			if (this.m_isAnchorElementDirty)
			{
				ScrollViewer scrollViewer = this.ScrollViewer;
				if (scrollViewer != null)
				{
					double num = ((this.HasPendingBringIntoView && !this.m_pendingBringIntoView.Animate) ? this.m_pendingBringIntoView.ChangeViewOffset.Y : scrollViewer.VerticalOffset) + this.HorizontalAnchorRatio * scrollViewer.ViewportHeight + this.m_pendingViewportShift;
					ItemsRepeaterScrollHost.CandidateInfo candidateInfo = null;
					double num2 = 3.4028234663852886E+38;
					foreach (ItemsRepeaterScrollHost.CandidateInfo candidateInfo2 in this.m_candidates)
					{
						UIElement element = candidateInfo2.Element;
						if (!candidateInfo2.IsRelativeBoundsSet)
						{
							Rect layoutSlot = LayoutInformation.GetLayoutSlot((FrameworkElement)element);
							GeneralTransform generalTransform = Extensions.SafeTransformToVisual(element, scrollViewer.GetContentTemplateRoot());
							candidateInfo2.RelativeBounds = generalTransform.TransformBounds(new Rect(0.0, 0.0, layoutSlot.Width, layoutSlot.Height));
						}
						double num3 = Math.Abs(candidateInfo2.RelativeBounds.Y + this.HorizontalAnchorRatio * candidateInfo2.RelativeBounds.Height - num);
						if (num3 < num2)
						{
							candidateInfo = candidateInfo2;
							num2 = num3;
						}
					}
					if (candidateInfo != null)
					{
						this.m_anchorElement = candidateInfo.Element;
						this.m_anchorElementRelativeBounds = candidateInfo.RelativeBounds;
					}
					else
					{
						this.m_anchorElement = null;
						this.m_anchorElementRelativeBounds = ItemsRepeaterScrollHost.CandidateInfo.InvalidBounds;
					}
				}
				this.m_isAnchorElementDirty = false;
			}
			if (relativeBounds != Rect.Empty)
			{
				relativeBounds = this.m_anchorElementRelativeBounds;
			}
			return this.m_anchorElement;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001BD04 File Offset: 0x00019F04
		private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.ViewportWidthChange != 0.0 || e.ViewportHeightChange != 0.0)
			{
				base.InvalidateArrange();
			}
			if (e.HorizontalChange == 0.0 && e.VerticalChange == 0.0)
			{
				return;
			}
			this.m_pendingViewportShift = 0.0;
			if (this.HasPendingBringIntoView && this.m_pendingBringIntoView.ChangeViewCalled)
			{
				this.m_pendingBringIntoView.Reset();
			}
			ViewportChangedEventHandler viewportChanged = this.ViewportChanged;
			if (viewportChanged == null)
			{
				return;
			}
			viewportChanged(this, true);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001BD9D File Offset: 0x00019F9D
		private void OnScrollViewerSizeChanged(object sender, SizeChangedEventArgs args)
		{
			ViewportChangedEventHandler viewportChanged = this.ViewportChanged;
			if (viewportChanged == null)
			{
				return;
			}
			viewportChanged(this, true);
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001BDB1 File Offset: 0x00019FB1
		private bool HasPendingBringIntoView
		{
			get
			{
				return this.m_pendingBringIntoView.TargetElement != null;
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060007C7 RID: 1991 RVA: 0x0001BDC4 File Offset: 0x00019FC4
		// (remove) Token: 0x060007C8 RID: 1992 RVA: 0x0001BDFC File Offset: 0x00019FFC
		private event ViewportChangedEventHandler ViewportChanged;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060007C9 RID: 1993 RVA: 0x0001BE34 File Offset: 0x0001A034
		// (remove) Token: 0x060007CA RID: 1994 RVA: 0x0001BE6C File Offset: 0x0001A06C
		private event PostArrangeEventHandler PostArrange;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060007CB RID: 1995 RVA: 0x0001BEA4 File Offset: 0x0001A0A4
		// (remove) Token: 0x060007CC RID: 1996 RVA: 0x0001BEDC File Offset: 0x0001A0DC
		private event ConfigurationChangedEventHandler ConfigurationChanged;

		// Token: 0x04000374 RID: 884
		private readonly List<ItemsRepeaterScrollHost.CandidateInfo> m_candidates = new List<ItemsRepeaterScrollHost.CandidateInfo>();

		// Token: 0x04000375 RID: 885
		private UIElement m_anchorElement;

		// Token: 0x04000376 RID: 886
		private Rect m_anchorElementRelativeBounds;

		// Token: 0x04000377 RID: 887
		private bool m_isAnchorElementDirty = true;

		// Token: 0x04000378 RID: 888
		private double m_horizontalEdge;

		// Token: 0x04000379 RID: 889
		private double m_verticalEdge;

		// Token: 0x0400037A RID: 890
		private ItemsRepeaterScrollHost.BringIntoViewState m_pendingBringIntoView;

		// Token: 0x0400037B RID: 891
		private double m_pendingViewportShift;

		// Token: 0x02000116 RID: 278
		private class CandidateInfo
		{
			// Token: 0x06000CBF RID: 3263 RVA: 0x0002B484 File Offset: 0x00029684
			public CandidateInfo(UIElement element)
			{
				this.RelativeBounds = ItemsRepeaterScrollHost.CandidateInfo.InvalidBounds;
				this.Element = element;
			}

			// Token: 0x17000239 RID: 569
			// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0002B49E File Offset: 0x0002969E
			public UIElement Element { get; }

			// Token: 0x1700023A RID: 570
			// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0002B4A6 File Offset: 0x000296A6
			// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0002B4AE File Offset: 0x000296AE
			public Rect RelativeBounds { get; set; }

			// Token: 0x1700023B RID: 571
			// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0002B4B7 File Offset: 0x000296B7
			public bool IsRelativeBoundsSet
			{
				get
				{
					return this.RelativeBounds != ItemsRepeaterScrollHost.CandidateInfo.InvalidBounds;
				}
			}

			// Token: 0x040005A2 RID: 1442
			public static Rect InvalidBounds = Rect.Empty;
		}

		// Token: 0x02000117 RID: 279
		private class BringIntoViewState
		{
			// Token: 0x06000CC5 RID: 3269 RVA: 0x0002B4D5 File Offset: 0x000296D5
			public BringIntoViewState(UIElement owner)
			{
				this.TargetElement = owner;
			}

			// Token: 0x06000CC6 RID: 3270 RVA: 0x0002B4E4 File Offset: 0x000296E4
			public BringIntoViewState(UIElement targetElement, double alignmentX, double alignmentY, double offsetX, double offsetY, bool animate)
			{
				this.TargetElement = targetElement;
				this.AlignmentX = alignmentX;
				this.AlignmentY = alignmentY;
				this.OffsetX = offsetX;
				this.OffsetY = offsetY;
				this.Animate = animate;
				this.ChangeViewCalled = false;
				this.ChangeViewOffset = default(Point);
			}

			// Token: 0x1700023C RID: 572
			// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0002B53A File Offset: 0x0002973A
			// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0002B542 File Offset: 0x00029742
			public UIElement TargetElement { get; private set; }

			// Token: 0x1700023D RID: 573
			// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0002B54B File Offset: 0x0002974B
			// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0002B553 File Offset: 0x00029753
			public double AlignmentX { get; private set; }

			// Token: 0x1700023E RID: 574
			// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0002B55C File Offset: 0x0002975C
			// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0002B564 File Offset: 0x00029764
			public double AlignmentY { get; private set; }

			// Token: 0x1700023F RID: 575
			// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0002B56D File Offset: 0x0002976D
			// (set) Token: 0x06000CCE RID: 3278 RVA: 0x0002B575 File Offset: 0x00029775
			public double OffsetX { get; private set; }

			// Token: 0x17000240 RID: 576
			// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0002B57E File Offset: 0x0002977E
			// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x0002B586 File Offset: 0x00029786
			public double OffsetY { get; private set; }

			// Token: 0x17000241 RID: 577
			// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0002B58F File Offset: 0x0002978F
			public bool Animate { get; }

			// Token: 0x17000242 RID: 578
			// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0002B597 File Offset: 0x00029797
			// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0002B59F File Offset: 0x0002979F
			public bool ChangeViewCalled { get; set; }

			// Token: 0x17000243 RID: 579
			// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0002B5A8 File Offset: 0x000297A8
			// (set) Token: 0x06000CD5 RID: 3285 RVA: 0x0002B5B0 File Offset: 0x000297B0
			public Point ChangeViewOffset { get; set; }

			// Token: 0x06000CD6 RID: 3286 RVA: 0x0002B5BC File Offset: 0x000297BC
			public void Reset()
			{
				this.TargetElement = null;
				this.AlignmentX = (this.AlignmentY = (this.OffsetX = (this.OffsetY = 0.0)));
			}
		}
	}
}
