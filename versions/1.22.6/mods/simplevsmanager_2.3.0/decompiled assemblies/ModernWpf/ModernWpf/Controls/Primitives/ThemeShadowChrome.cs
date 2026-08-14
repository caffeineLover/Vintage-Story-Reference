using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002AA RID: 682
	public class ThemeShadowChrome : Decorator
	{
		// Token: 0x0600146D RID: 5229 RVA: 0x0003CA98 File Offset: 0x0003AC98
		static ThemeShadowChrome()
		{
			ThemeShadowChrome.s_bg1 = new SolidColorBrush(Colors.Black)
			{
				Opacity = 0.11
			};
			ThemeShadowChrome.s_bg2 = new SolidColorBrush(Colors.Black)
			{
				Opacity = 0.13
			};
			ThemeShadowChrome.s_bg3 = new SolidColorBrush(Colors.Black)
			{
				Opacity = 0.18
			};
			ThemeShadowChrome.s_bg4 = new SolidColorBrush(Colors.Black)
			{
				Opacity = 0.22
			};
			ThemeShadowChrome.s_bg1.Freeze();
			ThemeShadowChrome.s_bg2.Freeze();
			ThemeShadowChrome.s_bg3.Freeze();
			ThemeShadowChrome.s_bg4.Freeze();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0003CC64 File Offset: 0x0003AE64
		public ThemeShadowChrome()
		{
			this._bitmapCache = new BitmapCache(VisualTreeHelper.GetDpi(this).PixelsPerDip);
			this._background = new Grid
			{
				CacheMode = this._bitmapCache,
				Focusable = false,
				IsHitTestVisible = false,
				SnapsToDevicePixels = false
			};
			base.AddVisualChild(this._background);
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0003CCEC File Offset: 0x0003AEEC
		// (set) Token: 0x06001470 RID: 5232 RVA: 0x0003CCFE File Offset: 0x0003AEFE
		public bool IsShadowEnabled
		{
			get
			{
				return (bool)base.GetValue(ThemeShadowChrome.IsShadowEnabledProperty);
			}
			set
			{
				base.SetValue(ThemeShadowChrome.IsShadowEnabledProperty, value);
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0003CD11 File Offset: 0x0003AF11
		private static void OnIsShadowEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThemeShadowChrome)d).OnIsShadowEnabledChanged();
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0003CD20 File Offset: 0x0003AF20
		private void OnIsShadowEnabledChanged()
		{
			if (base.IsInitialized)
			{
				if (this.IsShadowEnabled)
				{
					this.EnsureShadows();
					this._background.Children.Add(this._shadow1);
					this._background.Children.Add(this._shadow2);
					this._background.Visibility = 0;
				}
				else
				{
					this._background.Children.Clear();
					this._background.Visibility = 2;
				}
				this.OnVisualParentChanged();
				this.UpdatePopupMargin();
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0003CDA7 File Offset: 0x0003AFA7
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x0003CDB9 File Offset: 0x0003AFB9
		public double Depth
		{
			get
			{
				return (double)base.GetValue(ThemeShadowChrome.DepthProperty);
			}
			set
			{
				base.SetValue(ThemeShadowChrome.DepthProperty, value);
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0003CDCC File Offset: 0x0003AFCC
		private static void OnDepthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThemeShadowChrome)d).OnDepthChanged();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0003CDD9 File Offset: 0x0003AFD9
		private void OnDepthChanged()
		{
			if (base.IsInitialized)
			{
				this.UpdateShadow1();
				this.UpdateShadow2();
				this.UpdatePopupMargin();
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0003CDF5 File Offset: 0x0003AFF5
		// (set) Token: 0x06001478 RID: 5240 RVA: 0x0003CE07 File Offset: 0x0003B007
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(ThemeShadowChrome.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(ThemeShadowChrome.CornerRadiusProperty, value);
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0003CE1A File Offset: 0x0003B01A
		private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThemeShadowChrome)d).OnCornerRadiusChanged(e);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0003CE28 File Offset: 0x0003B028
		private void OnCornerRadiusChanged(DependencyPropertyChangedEventArgs e)
		{
			CornerRadius cornerRadius = (CornerRadius)e.NewValue;
			if (this._shadow1 != null)
			{
				this._shadow1.CornerRadius = cornerRadius;
			}
			if (this._shadow2 != null)
			{
				this._shadow2.CornerRadius = cornerRadius;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0003CE6A File Offset: 0x0003B06A
		// (set) Token: 0x0600147C RID: 5244 RVA: 0x0003CE7C File Offset: 0x0003B07C
		private Thickness PopupMargin
		{
			get
			{
				return (Thickness)base.GetValue(ThemeShadowChrome.PopupMarginProperty);
			}
			set
			{
				base.SetValue(ThemeShadowChrome.PopupMarginProperty, value);
			}
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0003CE8F File Offset: 0x0003B08F
		private static void OnPopupMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThemeShadowChrome)d).OnPopupMarginChanged(e);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0003CE9D File Offset: 0x0003B09D
		private void OnPopupMarginChanged(DependencyPropertyChangedEventArgs e)
		{
			this.ApplyPopupMargin();
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0003CEA8 File Offset: 0x0003B0A8
		private void UpdatePopupMargin()
		{
			if (this.IsShadowEnabled)
			{
				double depth = this.Depth;
				double num = 0.9 * depth;
				double num2 = 0.4 * depth;
				this.PopupMargin = new Thickness(num, num, num, num + num2);
				return;
			}
			base.ClearValue(ThemeShadowChrome.PopupMarginProperty);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0003CEF9 File Offset: 0x0003B0F9
		private void ApplyPopupMargin()
		{
			if (this._parentPopupControl != null)
			{
				if (base.ReadLocalValue(ThemeShadowChrome.PopupMarginProperty) == DependencyProperty.UnsetValue)
				{
					this._parentPopupControl.ClearMargin();
					return;
				}
				this._parentPopupControl.SetMargin(this.PopupMargin);
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0003CF32 File Offset: 0x0003B132
		protected override int VisualChildrenCount
		{
			get
			{
				if (!this.IsShadowEnabled)
				{
					return base.VisualChildrenCount;
				}
				if (this.Child != null)
				{
					return 2;
				}
				return 1;
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0003CF4E File Offset: 0x0003B14E
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (base.IsInitialized)
			{
				this.OnVisualParentChanged();
			}
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0003CF65 File Offset: 0x0003B165
		protected override Visual GetVisualChild(int index)
		{
			if (!this.IsShadowEnabled)
			{
				return base.GetVisualChild(index);
			}
			if (index == 0)
			{
				return this._background;
			}
			if (index == 1 && this.Child != null)
			{
				return this.Child;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0003CF9E File Offset: 0x0003B19E
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.OnIsShadowEnabledChanged();
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0003CFAD File Offset: 0x0003B1AD
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.IsShadowEnabled)
			{
				this._background.Measure(constraint);
			}
			return base.MeasureOverride(constraint);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0003CFCA File Offset: 0x0003B1CA
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this.IsShadowEnabled)
			{
				this._background.Arrange(new Rect(arrangeSize));
			}
			return base.ArrangeOverride(arrangeSize);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0003CFEC File Offset: 0x0003B1EC
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);
			this._bitmapCache.RenderAtScale = newDpi.PixelsPerDip;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0003D008 File Offset: 0x0003B208
		private void OnVisualParentChanged()
		{
			if (this.IsShadowEnabled)
			{
				ThemeShadowChrome.PopupControl parentPopupControl = null;
				DependencyObject visualParent = base.VisualParent;
				ContextMenu contextMenu = visualParent as ContextMenu;
				if (contextMenu != null)
				{
					parentPopupControl = new ThemeShadowChrome.PopupControl(contextMenu);
				}
				else
				{
					ToolTip toolTip = visualParent as ToolTip;
					if (toolTip != null)
					{
						parentPopupControl = new ThemeShadowChrome.PopupControl(toolTip);
					}
					else
					{
						Popup popup = this.FindParentPopup(this);
						if (popup != null)
						{
							parentPopupControl = new ThemeShadowChrome.PopupControl(popup);
						}
					}
				}
				this.SetParentPopupControl(parentPopupControl);
				return;
			}
			this.SetParentPopupControl(null);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0003D070 File Offset: 0x0003B270
		private void EnsureShadows()
		{
			if (this._shadow1 == null)
			{
				this._shadow1 = this.CreateShadowElement();
				this.UpdateShadow1();
			}
			if (this._shadow2 == null)
			{
				this._shadow2 = this.CreateShadowElement();
				this.UpdateShadow2();
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0003D0A6 File Offset: 0x0003B2A6
		private Border CreateShadowElement()
		{
			return new Border
			{
				CornerRadius = this.CornerRadius,
				Effect = new BlurEffect(),
				RenderTransform = new TranslateTransform()
			};
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0003D0D0 File Offset: 0x0003B2D0
		private void UpdateShadow1()
		{
			if (this._shadow1 != null)
			{
				double depth = this.Depth;
				((BlurEffect)this._shadow1.Effect).Radius = 0.9 * depth;
				((TranslateTransform)this._shadow1.RenderTransform).Y = 0.4 * depth;
				this._shadow1.Background = ((depth >= 32.0) ? ThemeShadowChrome.s_bg4 : ThemeShadowChrome.s_bg2);
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0003D150 File Offset: 0x0003B350
		private void UpdateShadow2()
		{
			if (this._shadow2 != null)
			{
				double depth = this.Depth;
				((BlurEffect)this._shadow2.Effect).Radius = 0.225 * depth;
				((TranslateTransform)this._shadow2.RenderTransform).Y = 0.075 * depth;
				this._shadow2.Background = ((depth >= 32.0) ? ThemeShadowChrome.s_bg3 : ThemeShadowChrome.s_bg1);
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0003D1CF File Offset: 0x0003B3CF
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ClearMarginAdjustment();
			base.UpdateLayout();
			this.AdjustMargin();
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0003D1E3 File Offset: 0x0003B3E3
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (base.IsVisible)
			{
				this.AdjustMargin();
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0003D1F4 File Offset: 0x0003B3F4
		private void AdjustMargin()
		{
			if (this._parentPopupControl != null)
			{
				Thickness margin = base.Margin;
				if (margin != default(Thickness))
				{
					UIElement uielement = base.VisualParent as UIElement;
					if (uielement != null)
					{
						double width = uielement.RenderSize.Width;
						double actualWidth = base.ActualWidth;
						if (width > 0.0 && actualWidth > 0.0 && width < actualWidth + margin.Left + margin.Right)
						{
							double num = (width - actualWidth) / 2.0;
							ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(num, margin.Top, num, margin.Bottom), TimeSpan.Zero);
							base.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnimation);
							base.UpdateLayout();
						}
					}
				}
			}
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0003D2CA File Offset: 0x0003B4CA
		private void ClearMarginAdjustment()
		{
			base.BeginAnimation(FrameworkElement.MarginProperty, null);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0003D2D8 File Offset: 0x0003B4D8
		private void SetParentPopupControl(ThemeShadowChrome.PopupControl value)
		{
			if (this._parentPopupControl == value)
			{
				return;
			}
			if (this._popupPositioner != null)
			{
				this._popupPositioner.Dispose();
				this._popupPositioner = null;
			}
			if (this._parentPopupControl != null)
			{
				this._parentPopupControl.Opened -= new EventHandler(this.OnParentPopupControlOpened);
				this._parentPopupControl.Closed -= new EventHandler(this.OnParentPopupControlClosed);
				this._parentPopupControl.ClearMargin();
				this._parentPopupControl.Dispose();
			}
			this._parentPopupControl = value;
			if (this._parentPopupControl != null)
			{
				this._parentPopupControl.Opened += new EventHandler(this.OnParentPopupControlOpened);
				this._parentPopupControl.Closed += new EventHandler(this.OnParentPopupControlClosed);
				this.ApplyPopupMargin();
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0003D398 File Offset: 0x0003B598
		private void OnParentPopupControlOpened(object sender, EventArgs e)
		{
			if (this._popupPositioner != null)
			{
				return;
			}
			if (this._parentPopupControl != null)
			{
				FrameworkElement control = this._parentPopupControl.Control;
				if (control != null)
				{
					ToolTip toolTip = control as ToolTip;
					if (toolTip != null)
					{
						Thumb thumb = toolTip.PlacementTarget as Thumb;
						if (thumb != null && thumb.TemplatedParent is Slider)
						{
							return;
						}
					}
					Popup popup = (control as Popup) ?? (control.Parent as Popup);
					if (popup != null && PopupPositioner.IsSupported)
					{
						this._popupPositioner = new PopupPositioner(popup);
					}
				}
			}
			if (this._popupPositioner == null)
			{
				this.PositionParentPopupControl();
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0003D426 File Offset: 0x0003B626
		private void OnParentPopupControlClosed(object sender, EventArgs e)
		{
			this.ClearMarginAdjustment();
			this.ResetTransform();
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0003D434 File Offset: 0x0003B634
		private void PositionParentPopupControl()
		{
			ThemeShadowChrome.<>c__DisplayClass48_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.popup = this._parentPopupControl;
			if (CS$<>8__locals1.popup != null)
			{
				CustomPlacementMode? customPlacementMode = default(CustomPlacementMode?);
				PlacementMode placement = CS$<>8__locals1.popup.Placement;
				if (placement != 2)
				{
					if (placement != 10)
					{
						if (placement == 11)
						{
							CustomPlacementMode customPlacementMode2;
							if (this.TryGetCustomPlacementMode(out customPlacementMode2))
							{
								customPlacementMode = new CustomPlacementMode?(customPlacementMode2);
							}
						}
					}
					else
					{
						customPlacementMode = new CustomPlacementMode?(CustomPlacementMode.TopEdgeAlignedLeft);
					}
				}
				else
				{
					customPlacementMode = new CustomPlacementMode?(CustomPlacementMode.BottomEdgeAlignedLeft);
				}
				if (customPlacementMode != null && !this.EnsureEdgesAligned(customPlacementMode.Value))
				{
					CustomPlacementMode? customPlacementMode3 = customPlacementMode;
					CustomPlacementMode customPlacementMode4 = CustomPlacementMode.BottomEdgeAlignedLeft;
					if (customPlacementMode3.GetValueOrDefault() == customPlacementMode4 & customPlacementMode3 != null)
					{
						if (this.<PositionParentPopupControl>g__shouldAlignRightEdges|48_0(ref CS$<>8__locals1))
						{
							this.EnsureEdgesAligned(CustomPlacementMode.BottomEdgeAlignedRight);
							return;
						}
					}
					else
					{
						customPlacementMode3 = customPlacementMode;
						customPlacementMode4 = CustomPlacementMode.TopEdgeAlignedLeft;
						if ((customPlacementMode3.GetValueOrDefault() == customPlacementMode4 & customPlacementMode3 != null) && this.<PositionParentPopupControl>g__shouldAlignRightEdges|48_0(ref CS$<>8__locals1))
						{
							this.EnsureEdgesAligned(CustomPlacementMode.TopEdgeAlignedRight);
						}
					}
				}
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0003D51E File Offset: 0x0003B71E
		private bool TryGetCustomPlacementMode(out CustomPlacementMode placement)
		{
			ThemeShadowChrome.PopupControl parentPopupControl = this._parentPopupControl;
			return this.TryGetCustomPlacementMode((parentPopupControl != null) ? parentPopupControl.Control : null, out placement) || this.TryGetCustomPlacementMode(base.VisualParent, out placement);
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0003D54F File Offset: 0x0003B74F
		private bool TryGetCustomPlacementMode(DependencyObject element, out CustomPlacementMode placement)
		{
			if (element != null && element.ReadLocalValue(CustomPopupPlacementHelper.PlacementProperty) != DependencyProperty.UnsetValue)
			{
				placement = CustomPopupPlacementHelper.GetPlacement(element);
				return true;
			}
			placement = CustomPlacementMode.Top;
			return false;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0003D574 File Offset: 0x0003B774
		private bool TryGetOffsetToTarget(InterestPoint targetInterestPoint, InterestPoint childInterestPoint, out Vector offset)
		{
			ThemeShadowChrome.PopupControl parentPopupControl = this._parentPopupControl;
			if (parentPopupControl != null)
			{
				UIElement placementTarget = parentPopupControl.PlacementTarget;
				if (placementTarget != null && base.IsVisible && placementTarget.IsVisible)
				{
					offset = Helper.GetOffset(this, childInterestPoint, placementTarget, targetInterestPoint, parentPopupControl.PlacementRectangle);
					if (Math.Abs(offset.X) < 0.5)
					{
						offset.X = 0.0;
					}
					if (Math.Abs(offset.Y) < 0.5)
					{
						offset.Y = 0.0;
					}
					return true;
				}
			}
			offset = default(Vector);
			return false;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0003D610 File Offset: 0x0003B810
		private bool EnsureEdgesAligned(CustomPlacementMode placement)
		{
			Vector vector = ThemeShadowChrome.s_noTranslation;
			switch (placement)
			{
			case CustomPlacementMode.TopEdgeAlignedLeft:
			{
				Vector offset;
				if (this.TryGetOffsetToTarget(InterestPoint.TopLeft, InterestPoint.BottomLeft, out offset))
				{
					vector = this.<EnsureEdgesAligned>g__getTranslation|52_0(true, true, offset);
				}
				break;
			}
			case CustomPlacementMode.TopEdgeAlignedRight:
			{
				Vector offset;
				if (this.TryGetOffsetToTarget(InterestPoint.TopRight, InterestPoint.BottomRight, out offset))
				{
					vector = this.<EnsureEdgesAligned>g__getTranslation|52_0(true, false, offset);
				}
				break;
			}
			case CustomPlacementMode.BottomEdgeAlignedLeft:
			{
				Vector offset;
				if (this.TryGetOffsetToTarget(InterestPoint.BottomLeft, InterestPoint.TopLeft, out offset))
				{
					vector = this.<EnsureEdgesAligned>g__getTranslation|52_0(false, true, offset);
				}
				break;
			}
			case CustomPlacementMode.BottomEdgeAlignedRight:
			{
				Vector offset;
				if (this.TryGetOffsetToTarget(InterestPoint.BottomRight, InterestPoint.TopRight, out offset))
				{
					vector = this.<EnsureEdgesAligned>g__getTranslation|52_0(false, false, offset);
				}
				break;
			}
			}
			if (vector != ThemeShadowChrome.s_noTranslation)
			{
				this.SetupTransform(vector);
				return true;
			}
			this.ResetTransform();
			return false;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0003D6B8 File Offset: 0x0003B8B8
		private void SetupTransform(Vector translation)
		{
			if (this._transform == null)
			{
				this._transform = new TranslateTransform();
				base.RenderTransform = this._transform;
			}
			this._transform.X = translation.X;
			this._transform.Y = translation.Y;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0003D708 File Offset: 0x0003B908
		private void ResetTransform()
		{
			if (this._transform != null)
			{
				this._transform.ClearValue(TranslateTransform.XProperty);
				this._transform.ClearValue(TranslateTransform.YProperty);
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0003D734 File Offset: 0x0003B934
		private Popup FindParentPopup(FrameworkElement element)
		{
			DependencyObject parent = element.Parent;
			Popup popup = parent as Popup;
			if (popup != null)
			{
				return popup;
			}
			FrameworkElement frameworkElement = parent as FrameworkElement;
			if (frameworkElement != null)
			{
				return this.FindParentPopup(frameworkElement);
			}
			FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(element) as FrameworkElement;
			if (frameworkElement2 != null)
			{
				return this.FindParentPopup(frameworkElement2);
			}
			return null;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0003D780 File Offset: 0x0003B980
		[CompilerGenerated]
		private bool <PositionParentPopupControl>g__shouldAlignRightEdges|48_0(ref ThemeShadowChrome.<>c__DisplayClass48_0 A_1)
		{
			UIElement placementTarget = A_1.popup.PlacementTarget;
			if (placementTarget != null)
			{
				double width = placementTarget.RenderSize.Width;
				if (base.ActualWidth > 0.0 && width > 0.0)
				{
					if (base.ActualWidth == width)
					{
						return true;
					}
					Vector vector;
					if (base.ActualWidth > width && this.TryGetOffsetToTarget(InterestPoint.TopRight, InterestPoint.TopRight, out vector) && vector.X < 0.0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0003D800 File Offset: 0x0003BA00
		[CompilerGenerated]
		private Vector <EnsureEdgesAligned>g__getTranslation|52_0(bool top, bool left, Vector offset)
		{
			double num = 0.0;
			double num2 = 0.0;
			if ((left && offset.X > 0.0) || (!left && offset.X < 0.0) || Math.Abs(offset.X) < 0.5)
			{
				num = -offset.X;
			}
			if ((top && offset.Y < this.PopupMargin.Top) || (!top && offset.Y > -this.PopupMargin.Bottom) || Math.Abs(offset.Y) < 0.5)
			{
				num2 = -offset.Y;
			}
			return new Vector(num, num2);
		}

		// Token: 0x040006DA RID: 1754
		public static readonly DependencyProperty IsShadowEnabledProperty = DependencyProperty.Register("IsShadowEnabled", typeof(bool), typeof(ThemeShadowChrome), new PropertyMetadata(true, new PropertyChangedCallback(ThemeShadowChrome.OnIsShadowEnabledChanged)));

		// Token: 0x040006DB RID: 1755
		public static readonly DependencyProperty DepthProperty = DependencyProperty.Register("Depth", typeof(double), typeof(ThemeShadowChrome), new PropertyMetadata(32.0, new PropertyChangedCallback(ThemeShadowChrome.OnDepthChanged)));

		// Token: 0x040006DC RID: 1756
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ThemeShadowChrome), new PropertyMetadata(default(CornerRadius), new PropertyChangedCallback(ThemeShadowChrome.OnCornerRadiusChanged)));

		// Token: 0x040006DD RID: 1757
		private static readonly DependencyProperty PopupMarginProperty = DependencyProperty.Register("PopupMargin", typeof(Thickness), typeof(ThemeShadowChrome), new PropertyMetadata(default(Thickness), new PropertyChangedCallback(ThemeShadowChrome.OnPopupMarginChanged)));

		// Token: 0x040006DE RID: 1758
		private readonly Grid _background;

		// Token: 0x040006DF RID: 1759
		private readonly BitmapCache _bitmapCache;

		// Token: 0x040006E0 RID: 1760
		private Border _shadow1;

		// Token: 0x040006E1 RID: 1761
		private Border _shadow2;

		// Token: 0x040006E2 RID: 1762
		private ThemeShadowChrome.PopupControl _parentPopupControl;

		// Token: 0x040006E3 RID: 1763
		private TranslateTransform _transform;

		// Token: 0x040006E4 RID: 1764
		private PopupPositioner _popupPositioner;

		// Token: 0x040006E5 RID: 1765
		private static readonly Brush s_bg1;

		// Token: 0x040006E6 RID: 1766
		private static readonly Brush s_bg2;

		// Token: 0x040006E7 RID: 1767
		private static readonly Brush s_bg3;

		// Token: 0x040006E8 RID: 1768
		private static readonly Brush s_bg4;

		// Token: 0x040006E9 RID: 1769
		private static readonly Vector s_noTranslation = new Vector(0.0, 0.0);

		// Token: 0x02000421 RID: 1057
		private class PopupControl : IDisposable
		{
			// Token: 0x06001C1B RID: 7195 RVA: 0x0004B39C File Offset: 0x0004959C
			public PopupControl(ContextMenu contextMenu)
			{
				this._contextMenu = contextMenu;
				this._contextMenu.Opened += new RoutedEventHandler(this.OnOpened);
				this._contextMenu.Closed += new RoutedEventHandler(this.OnClosed);
			}

			// Token: 0x06001C1C RID: 7196 RVA: 0x0004B3D9 File Offset: 0x000495D9
			public PopupControl(ToolTip toolTip)
			{
				this._toolTip = toolTip;
				this._toolTip.Opened += new RoutedEventHandler(this.OnOpened);
				this._toolTip.Closed += new RoutedEventHandler(this.OnClosed);
			}

			// Token: 0x06001C1D RID: 7197 RVA: 0x0004B416 File Offset: 0x00049616
			public PopupControl(Popup popup)
			{
				this._popup = popup;
				this._popup.Opened += new EventHandler(this.OnOpened);
				this._popup.Closed += new EventHandler(this.OnClosed);
			}

			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x06001C1E RID: 7198 RVA: 0x0004B453 File Offset: 0x00049653
			public FrameworkElement Control
			{
				get
				{
					ContextMenu result;
					if ((result = this._contextMenu) == null)
					{
						result = (this._toolTip ?? this._popup);
					}
					return result;
				}
			}

			// Token: 0x17000522 RID: 1314
			// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0004B46F File Offset: 0x0004966F
			public PlacementMode Placement
			{
				get
				{
					if (this._contextMenu != null)
					{
						return this._contextMenu.Placement;
					}
					if (this._toolTip != null)
					{
						return this._toolTip.Placement;
					}
					if (this._popup != null)
					{
						return this._popup.Placement;
					}
					return 0;
				}
			}

			// Token: 0x17000523 RID: 1315
			// (get) Token: 0x06001C20 RID: 7200 RVA: 0x0004B4B0 File Offset: 0x000496B0
			public UIElement PlacementTarget
			{
				get
				{
					if (this._contextMenu != null)
					{
						return this._contextMenu.PlacementTarget;
					}
					if (this._toolTip != null)
					{
						return this._toolTip.PlacementTarget;
					}
					if (this._popup != null)
					{
						return this._popup.PlacementTarget ?? (VisualTreeHelper.GetParent(this._popup) as UIElement);
					}
					return null;
				}
			}

			// Token: 0x17000524 RID: 1316
			// (get) Token: 0x06001C21 RID: 7201 RVA: 0x0004B510 File Offset: 0x00049710
			public Rect PlacementRectangle
			{
				get
				{
					if (this._contextMenu != null)
					{
						return this._contextMenu.PlacementRectangle;
					}
					if (this._toolTip != null)
					{
						return this._toolTip.PlacementRectangle;
					}
					if (this._popup != null)
					{
						return this._popup.PlacementRectangle;
					}
					return Rect.Empty;
				}
			}

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x06001C22 RID: 7202 RVA: 0x0004B55E File Offset: 0x0004975E
			private FrameworkElement ChildAsFE
			{
				get
				{
					ContextMenu result;
					if ((result = this._contextMenu) == null && (result = this._toolTip) == null)
					{
						Popup popup = this._popup;
						result = (((popup != null) ? popup.Child : null) as FrameworkElement);
					}
					return result;
				}
			}

			// Token: 0x14000024 RID: 36
			// (add) Token: 0x06001C23 RID: 7203 RVA: 0x0004B58C File Offset: 0x0004978C
			// (remove) Token: 0x06001C24 RID: 7204 RVA: 0x0004B5C4 File Offset: 0x000497C4
			public event EventHandler Opened;

			// Token: 0x14000025 RID: 37
			// (add) Token: 0x06001C25 RID: 7205 RVA: 0x0004B5FC File Offset: 0x000497FC
			// (remove) Token: 0x06001C26 RID: 7206 RVA: 0x0004B634 File Offset: 0x00049834
			public event EventHandler Closed;

			// Token: 0x06001C27 RID: 7207 RVA: 0x0004B66C File Offset: 0x0004986C
			public void SetMargin(Thickness margin)
			{
				FrameworkElement childAsFE = this.ChildAsFE;
				if (childAsFE != null)
				{
					childAsFE.Margin = margin;
				}
			}

			// Token: 0x06001C28 RID: 7208 RVA: 0x0004B68A File Offset: 0x0004988A
			public void ClearMargin()
			{
				FrameworkElement childAsFE = this.ChildAsFE;
				if (childAsFE == null)
				{
					return;
				}
				childAsFE.ClearValue(FrameworkElement.MarginProperty);
			}

			// Token: 0x06001C29 RID: 7209 RVA: 0x0004B6A4 File Offset: 0x000498A4
			public void Dispose()
			{
				if (this._contextMenu != null)
				{
					this._contextMenu.Opened -= new RoutedEventHandler(this.OnOpened);
					this._contextMenu.Closed -= new RoutedEventHandler(this.OnClosed);
					this._contextMenu = null;
					return;
				}
				if (this._toolTip != null)
				{
					this._toolTip.Opened -= new RoutedEventHandler(this.OnOpened);
					this._toolTip.Closed -= new RoutedEventHandler(this.OnClosed);
					this._toolTip = null;
					return;
				}
				if (this._popup != null)
				{
					this._popup.Opened -= new EventHandler(this.OnOpened);
					this._popup.Closed -= new EventHandler(this.OnClosed);
					this._popup = null;
				}
			}

			// Token: 0x06001C2A RID: 7210 RVA: 0x0004B76A File Offset: 0x0004996A
			private void OnOpened(object sender, EventArgs e)
			{
				EventHandler opened = this.Opened;
				if (opened == null)
				{
					return;
				}
				opened.Invoke(this, e);
			}

			// Token: 0x06001C2B RID: 7211 RVA: 0x0004B77E File Offset: 0x0004997E
			private void OnClosed(object sender, EventArgs e)
			{
				EventHandler closed = this.Closed;
				if (closed == null)
				{
					return;
				}
				closed.Invoke(this, e);
			}

			// Token: 0x04000A37 RID: 2615
			private ContextMenu _contextMenu;

			// Token: 0x04000A38 RID: 2616
			private ToolTip _toolTip;

			// Token: 0x04000A39 RID: 2617
			private Popup _popup;
		}
	}
}
