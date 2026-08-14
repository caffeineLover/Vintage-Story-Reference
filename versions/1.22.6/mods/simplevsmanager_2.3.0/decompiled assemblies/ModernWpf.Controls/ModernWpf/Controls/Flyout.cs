using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000041 RID: 65
	[ContentProperty("Content")]
	[StyleTypedProperty(Property = "FlyoutPresenterStyle", StyleTargetType = typeof(FlyoutPresenter))]
	public class Flyout : FlyoutBase
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00008722 File Offset: 0x00006922
		// (set) Token: 0x06000279 RID: 633 RVA: 0x00008734 File Offset: 0x00006934
		public UIElement Content
		{
			get
			{
				return (UIElement)base.GetValue(Flyout.ContentProperty);
			}
			set
			{
				base.SetValue(Flyout.ContentProperty, value);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00008742 File Offset: 0x00006942
		// (set) Token: 0x0600027B RID: 635 RVA: 0x00008754 File Offset: 0x00006954
		public Style FlyoutPresenterStyle
		{
			get
			{
				return (Style)base.GetValue(Flyout.FlyoutPresenterStyleProperty);
			}
			set
			{
				base.SetValue(Flyout.FlyoutPresenterStyleProperty, value);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600027C RID: 636 RVA: 0x00008762 File Offset: 0x00006962
		internal override PopupAnimation DesiredPopupAnimation
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00008768 File Offset: 0x00006968
		private bool IsPopupOpenDown
		{
			get
			{
				Point point;
				return this.TryGetPopupOffset(out point) && point.Y > 0.0;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00008794 File Offset: 0x00006994
		private bool IsPopupOpenRight
		{
			get
			{
				Point point;
				return this.TryGetPopupOffset(out point) && point.X > 0.0;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000087C0 File Offset: 0x000069C0
		protected override Control CreatePresenter()
		{
			FlyoutPresenter flyoutPresenter = new FlyoutPresenter();
			flyoutPresenter.SetBinding(ContentControl.ContentProperty, new Binding
			{
				Path = new PropertyPath(Flyout.ContentProperty),
				Source = this
			});
			flyoutPresenter.SetBinding(FrameworkElement.StyleProperty, new Binding
			{
				Path = new PropertyPath(Flyout.FlyoutPresenterStyleProperty),
				Source = this
			});
			return flyoutPresenter;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00008822 File Offset: 0x00006A22
		internal override void OnOpened()
		{
			if (base.AreOpenCloseAnimationsEnabled && SharedHelpers.IsAnimationsEnabled)
			{
				this.PlayOpenAnimation();
			}
			base.OnOpened();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008840 File Offset: 0x00006A40
		internal override void OnClosed()
		{
			if (this.m_openingStoryboard != null)
			{
				Control control = base.InternalPopup.Child as Control;
				if (control != null)
				{
					this.m_openingStoryboard.Stop(control);
				}
			}
			base.OnClosed();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000887C File Offset: 0x00006A7C
		private void PlayOpenAnimation()
		{
			Control control = (Control)base.InternalPopup.Child;
			this.EnsureOpeningStoryboard(control);
			Flyout.AnimateFrom animateFrom = this.GetAnimateFrom();
			this.UpdateFromOffsetKeyFrames(animateFrom);
			if (!(control.RenderTransform is TranslateTransform))
			{
				control.RenderTransform = new TranslateTransform();
			}
			if (animateFrom != Flyout.AnimateFrom.None)
			{
				BitmapCache cacheMode = new BitmapCache(VisualTreeHelper.GetDpi(control).PixelsPerDip);
				control.CacheMode = cacheMode;
			}
			this.m_openingStoryboard.Begin(control, true);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000088F4 File Offset: 0x00006AF4
		private void EnsureOpeningStoryboard(Control presenter)
		{
			if (this.m_openingStoryboard == null)
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.FromMilliseconds(83.0)));
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromMilliseconds(166.0)));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
				Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, presenter);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, Flyout.s_opacityPath);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(this.m_fromHorizontalOffsetKeyFrame = new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, Flyout.s_translateDuration, Flyout.s_decelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames4 = doubleAnimationUsingKeyFrames3;
				Storyboard.SetTarget(doubleAnimationUsingKeyFrames4, presenter);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4, Flyout.s_translateXPath);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames5 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(this.m_fromVerticalOffsetKeyFrame = new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, Flyout.s_translateDuration, Flyout.s_decelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames6 = doubleAnimationUsingKeyFrames5;
				Storyboard.SetTarget(doubleAnimationUsingKeyFrames6, presenter);
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames6, Flyout.s_translateYPath);
				Storyboard storyboard = new Storyboard();
				storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames4);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames6);
				storyboard.FillBehavior = 1;
				this.m_openingStoryboard = storyboard;
				this.m_openingStoryboard.Completed += delegate([Nullable(2)] object <p0>, EventArgs <p1>)
				{
					presenter.ClearValue(UIElement.CacheModeProperty);
				};
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00008AEC File Offset: 0x00006CEC
		private Flyout.AnimateFrom GetAnimateFrom()
		{
			if (base.InternalPopup.PlacementTarget != null)
			{
				switch (base.Placement)
				{
				case FlyoutPlacementMode.Top:
				case FlyoutPlacementMode.Bottom:
				case FlyoutPlacementMode.TopEdgeAlignedLeft:
				case FlyoutPlacementMode.TopEdgeAlignedRight:
				case FlyoutPlacementMode.BottomEdgeAlignedLeft:
				case FlyoutPlacementMode.BottomEdgeAlignedRight:
					if (!this.IsPopupOpenDown)
					{
						return Flyout.AnimateFrom.Bottom;
					}
					return Flyout.AnimateFrom.Top;
				case FlyoutPlacementMode.Left:
				case FlyoutPlacementMode.Right:
				case FlyoutPlacementMode.LeftEdgeAlignedTop:
				case FlyoutPlacementMode.LeftEdgeAlignedBottom:
				case FlyoutPlacementMode.RightEdgeAlignedTop:
				case FlyoutPlacementMode.RightEdgeAlignedBottom:
					if (!this.IsPopupOpenRight)
					{
						return Flyout.AnimateFrom.Right;
					}
					return Flyout.AnimateFrom.Left;
				}
			}
			return Flyout.AnimateFrom.None;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00008B64 File Offset: 0x00006D64
		private void UpdateFromOffsetKeyFrames(Flyout.AnimateFrom animateFrom)
		{
			switch (animateFrom)
			{
			case Flyout.AnimateFrom.None:
				this.m_fromHorizontalOffsetKeyFrame.Value = 0.0;
				this.m_fromVerticalOffsetKeyFrame.Value = 0.0;
				return;
			case Flyout.AnimateFrom.Top:
				this.m_fromHorizontalOffsetKeyFrame.Value = 0.0;
				this.m_fromVerticalOffsetKeyFrame.Value = -40.0;
				return;
			case Flyout.AnimateFrom.Bottom:
				this.m_fromHorizontalOffsetKeyFrame.Value = 0.0;
				this.m_fromVerticalOffsetKeyFrame.Value = 40.0;
				return;
			case Flyout.AnimateFrom.Left:
				this.m_fromHorizontalOffsetKeyFrame.Value = -40.0;
				this.m_fromVerticalOffsetKeyFrame.Value = 0.0;
				return;
			case Flyout.AnimateFrom.Right:
				this.m_fromHorizontalOffsetKeyFrame.Value = 40.0;
				this.m_fromVerticalOffsetKeyFrame.Value = 0.0;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00008C58 File Offset: 0x00006E58
		private bool TryGetPopupOffset(out Point offset)
		{
			UIElement child = base.InternalPopup.Child;
			UIElement placementTarget = base.InternalPopup.PlacementTarget;
			if (child != null && placementTarget != null && child.IsVisible && placementTarget.IsVisible)
			{
				offset = child.TranslatePoint(new Point(0.0, 0.0), placementTarget);
				return true;
			}
			offset = default(Point);
			return false;
		}

		// Token: 0x040000F2 RID: 242
		private const double c_translation = 40.0;

		// Token: 0x040000F3 RID: 243
		private static readonly TimeSpan s_translateDuration = TimeSpan.FromMilliseconds(367.0);

		// Token: 0x040000F4 RID: 244
		private static readonly PropertyPath s_opacityPath = new PropertyPath(UIElement.OpacityProperty);

		// Token: 0x040000F5 RID: 245
		private static readonly PropertyPath s_translateXPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)", Array.Empty<object>());

		// Token: 0x040000F6 RID: 246
		private static readonly PropertyPath s_translateYPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)", Array.Empty<object>());

		// Token: 0x040000F7 RID: 247
		private static readonly KeySpline s_decelerateKeySpline = new KeySpline(0.1, 0.9, 0.2, 1.0);

		// Token: 0x040000F8 RID: 248
		private static readonly BitmapCache s_bitmapCacheMode = new BitmapCache();

		// Token: 0x040000F9 RID: 249
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(Flyout));

		// Token: 0x040000FA RID: 250
		public static readonly DependencyProperty FlyoutPresenterStyleProperty = DependencyProperty.Register("FlyoutPresenterStyle", typeof(Style), typeof(Flyout));

		// Token: 0x040000FB RID: 251
		private Storyboard m_openingStoryboard;

		// Token: 0x040000FC RID: 252
		private DoubleKeyFrame m_fromHorizontalOffsetKeyFrame;

		// Token: 0x040000FD RID: 253
		private DoubleKeyFrame m_fromVerticalOffsetKeyFrame;

		// Token: 0x020000FB RID: 251
		private enum AnimateFrom
		{
			// Token: 0x04000568 RID: 1384
			None,
			// Token: 0x04000569 RID: 1385
			Top,
			// Token: 0x0400056A RID: 1386
			Bottom,
			// Token: 0x0400056B RID: 1387
			Left,
			// Token: 0x0400056C RID: 1388
			Right
		}
	}
}
