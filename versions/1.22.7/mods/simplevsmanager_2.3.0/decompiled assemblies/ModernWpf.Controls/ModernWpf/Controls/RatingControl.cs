using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000087 RID: 135
	public class RatingControl : Control
	{
		// Token: 0x0600069E RID: 1694 RVA: 0x00018138 File Offset: 0x00016338
		static RatingControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(typeof(RatingControl)));
			Control.FontFamilyProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(RatingControl.OnFontFamilyPropertyChanged)));
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000183BC File Offset: 0x000165BC
		private double RenderingRatingFontSize
		{
			get
			{
				return 32.0;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x000183C7 File Offset: 0x000165C7
		private double ActualRatingFontSize
		{
			get
			{
				return this.RenderingRatingFontSize / 2.0;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x000183D9 File Offset: 0x000165D9
		private double ItemSpacing
		{
			get
			{
				return 8.0;
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000183E4 File Offset: 0x000165E4
		private void UpdateCaptionMargins()
		{
			TextBlock captionTextBlock = this.m_captionTextBlock;
			if (captionTextBlock != null)
			{
				Thickness margin = captionTextBlock.Margin;
				margin.Top = 22.0 - this.ActualRatingFontSize * 0.8;
				captionTextBlock.Margin = margin;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001842C File Offset: 0x0001662C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.RecycleEvents();
			TextBlock textBlock = base.GetTemplateChild("Caption") as TextBlock;
			if (textBlock != null)
			{
				this.m_captionTextBlock = textBlock;
				textBlock.SizeChanged += new SizeChangedEventHandler(this.OnCaptionSizeChanged);
				this.UpdateCaptionMargins();
			}
			StackPanel stackPanel = base.GetTemplateChild("RatingBackgroundStackPanel") as StackPanel;
			if (stackPanel != null)
			{
				this.m_backgroundStackPanel = stackPanel;
				stackPanel.LostMouseCapture += new MouseEventHandler(this.OnPointerCaptureLostBackgroundStackPanel);
				stackPanel.MouseMove += new MouseEventHandler(this.OnPointerMovedOverBackgroundStackPanel);
				stackPanel.MouseEnter += new MouseEventHandler(this.OnPointerEnteredBackgroundStackPanel);
				stackPanel.MouseLeave += new MouseEventHandler(this.OnPointerExitedBackgroundStackPanel);
				stackPanel.MouseDown += new MouseButtonEventHandler(this.OnPointerPressedBackgroundStackPanel);
				stackPanel.MouseUp += new MouseButtonEventHandler(this.OnPointerReleasedBackgroundStackPanel);
			}
			this.m_foregroundStackPanel = (base.GetTemplateChild("RatingForegroundStackPanel") as StackPanel);
			base.SetValue(FocusVisualHelper.FocusVisualMarginProperty, RatingControl.c_focusVisualMargin);
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
			this.StampOutRatingItems();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00018544 File Offset: 0x00016744
		private double CoerceValueBetweenMinAndMax(double value)
		{
			if (value < 0.0)
			{
				value = -1.0;
			}
			else if (value <= 1.0)
			{
				value = 1.0;
			}
			else if (value > (double)this.MaxRating)
			{
				value = (double)this.MaxRating;
			}
			return value;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00018597 File Offset: 0x00016797
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RatingControlAutomationPeer(this);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000185A0 File Offset: 0x000167A0
		private void StampOutRatingItems()
		{
			if (this.m_backgroundStackPanel == null || this.m_foregroundStackPanel == null)
			{
				return;
			}
			this.m_backgroundStackPanel.Children.Clear();
			if (this.IsItemInfoPresentAndFontInfo())
			{
				this.PopulateStackPanelWithItems("BackgroundGlyphDefaultTemplate", this.m_backgroundStackPanel, RatingControlStates.Unset);
			}
			else if (this.IsItemInfoPresentAndImageInfo())
			{
				this.PopulateStackPanelWithItems("BackgroundImageDefaultTemplate", this.m_backgroundStackPanel, RatingControlStates.Unset);
			}
			else if (this.IsItemInfoPresentAndPathInfo())
			{
				this.PopulateStackPanelWithItems("BackgroundPathDefaultTemplate", this.m_backgroundStackPanel, RatingControlStates.Unset);
			}
			this.m_foregroundStackPanel.Children.Clear();
			if (this.IsItemInfoPresentAndFontInfo())
			{
				this.PopulateStackPanelWithItems("ForegroundGlyphDefaultTemplate", this.m_foregroundStackPanel, RatingControlStates.Set);
			}
			else if (this.IsItemInfoPresentAndImageInfo())
			{
				this.PopulateStackPanelWithItems("ForegroundImageDefaultTemplate", this.m_foregroundStackPanel, RatingControlStates.Set);
			}
			else if (this.IsItemInfoPresentAndPathInfo())
			{
				this.PopulateStackPanelWithItems("ForegroundPathDefaultTemplate", this.m_foregroundStackPanel, RatingControlStates.Set);
			}
			this.UpdateRatingItemsAppearance();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00018688 File Offset: 0x00016888
		private void ReRenderCaption()
		{
			if (this.m_captionTextBlock != null)
			{
				this.ResetControlWidth();
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00018698 File Offset: 0x00016898
		private void UpdateRatingItemsAppearance()
		{
			if (this.m_foregroundStackPanel != null)
			{
				double placeholderValue = this.PlaceholderValue;
				double value = this.Value;
				double num = 0.0;
				if (this.m_isPointerOver)
				{
					num = Math.Ceiling(this.m_mousePercentage * (double)this.MaxRating);
					if (value == -1.0)
					{
						if (placeholderValue == -1.0)
						{
							VisualStateManager.GoToState(this, "PointerOverPlaceholder", false);
							this.CustomizeStackPanel(this.m_foregroundStackPanel, RatingControlStates.PointerOverPlaceholder);
						}
						else
						{
							VisualStateManager.GoToState(this, "PointerOverUnselected", false);
							this.CustomizeStackPanel(this.m_foregroundStackPanel, RatingControlStates.PointerOverPlaceholder);
						}
					}
					else
					{
						VisualStateManager.GoToState(this, "PointerOverSet", false);
						this.CustomizeStackPanel(this.m_foregroundStackPanel, RatingControlStates.PointerOverSet);
					}
				}
				else if (value > -1.0)
				{
					num = value;
					VisualStateManager.GoToState(this, "Set", false);
					this.CustomizeStackPanel(this.m_foregroundStackPanel, RatingControlStates.Set);
				}
				else if (placeholderValue > -1.0)
				{
					num = placeholderValue;
					VisualStateManager.GoToState(this, "Placeholder", false);
					this.CustomizeStackPanel(this.m_foregroundStackPanel, RatingControlStates.Placeholder);
				}
				if (!base.IsEnabled)
				{
					VisualStateManager.GoToState(this, "Disabled", false);
					this.CustomizeStackPanel(this.m_foregroundStackPanel, RatingControlStates.Disabled);
				}
				int num2 = 0;
				foreach (object obj in this.m_foregroundStackPanel.Children)
				{
					double num3 = this.RenderingRatingFontSize;
					if ((double)(num2 + 1) > num)
					{
						if ((double)num2 < num)
						{
							num3 *= num - Math.Floor(num);
						}
						else
						{
							num3 = 0.0;
						}
					}
					RectangleGeometry clip = new RectangleGeometry(new Rect(0.0, 0.0, num3, this.RenderingRatingFontSize));
					((UIElement)obj).Clip = clip;
					num2++;
				}
				this.ResetControlWidth();
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00018884 File Offset: 0x00016A84
		private void ApplyScaleExpressionAnimation(UIElement uiElement, int starIndex)
		{
			ScaleTransform scaleTransform = uiElement.RenderTransform as ScaleTransform;
			if (scaleTransform == null)
			{
				scaleTransform = new ScaleTransform();
				uiElement.RenderTransform = scaleTransform;
			}
			scaleTransform.ScaleX = 0.5;
			scaleTransform.ScaleY = 0.5;
			scaleTransform.CenterX = 16.0;
			scaleTransform.CenterY = 25.6;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000188EC File Offset: 0x00016AEC
		private void PopulateStackPanelWithItems(string templateName, StackPanel stackPanel, RatingControlStates state)
		{
			DataTemplate dataTemplate = (DataTemplate)Application.Current.FindResource(templateName);
			for (int i = 0; i < this.MaxRating; i++)
			{
				UIElement uielement = dataTemplate.LoadContent() as UIElement;
				if (uielement != null)
				{
					this.CustomizeRatingItem(uielement, state);
					stackPanel.Children.Add(uielement);
					this.ApplyScaleExpressionAnimation(uielement, i);
				}
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00018948 File Offset: 0x00016B48
		private void CustomizeRatingItem(UIElement ui, RatingControlStates type)
		{
			if (this.IsItemInfoPresentAndFontInfo())
			{
				TextBlock textBlock = ui as TextBlock;
				if (textBlock != null)
				{
					textBlock.FontFamily = base.FontFamily;
					textBlock.Text = this.GetAppropriateGlyph(type);
					return;
				}
			}
			else if (this.IsItemInfoPresentAndImageInfo())
			{
				Image image = ui as Image;
				if (image != null)
				{
					image.Source = this.GetAppropriateImageSource(type);
					image.Width = this.RenderingRatingFontSize;
					image.Height = this.RenderingRatingFontSize;
					return;
				}
			}
			else if (this.IsItemInfoPresentAndPathInfo())
			{
				FontIconFallback fontIconFallback = ui as FontIconFallback;
				if (fontIconFallback != null)
				{
					fontIconFallback.Data = this.GetAppropriatePathData(type);
				}
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000189D8 File Offset: 0x00016BD8
		private void CustomizeStackPanel(StackPanel stackPanel, RatingControlStates state)
		{
			foreach (object obj in stackPanel.Children)
			{
				UIElement ui = (UIElement)obj;
				this.CustomizeRatingItem(ui, state);
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00018A34 File Offset: 0x00016C34
		private bool IsItemInfoPresentAndFontInfo()
		{
			return this.m_infoType == RatingInfoType.Font;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00018A3F File Offset: 0x00016C3F
		private bool IsItemInfoPresentAndImageInfo()
		{
			return this.m_infoType == RatingInfoType.Image;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00018A4A File Offset: 0x00016C4A
		private bool IsItemInfoPresentAndPathInfo()
		{
			return this.m_infoType == RatingInfoType.Path;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00018A58 File Offset: 0x00016C58
		private string GetAppropriateGlyph(RatingControlStates type)
		{
			this.IsItemInfoPresentAndFontInfo();
			RatingItemFontInfo ratingItemFontInfo = (RatingItemFontInfo)this.ItemInfo;
			switch (type)
			{
			case RatingControlStates.Disabled:
				return this.GetNextGlyphIfNull(ratingItemFontInfo.DisabledGlyph, RatingControlStates.Set);
			case RatingControlStates.PointerOverSet:
				return this.GetNextGlyphIfNull(ratingItemFontInfo.PointerOverGlyph, RatingControlStates.Set);
			case RatingControlStates.PointerOverPlaceholder:
				return this.GetNextGlyphIfNull(ratingItemFontInfo.PointerOverPlaceholderGlyph, RatingControlStates.Placeholder);
			case RatingControlStates.Placeholder:
				return this.GetNextGlyphIfNull(ratingItemFontInfo.PlaceholderGlyph, RatingControlStates.Set);
			case RatingControlStates.Unset:
				return this.GetNextGlyphIfNull(ratingItemFontInfo.UnsetGlyph, RatingControlStates.Set);
			case RatingControlStates.Null:
				return string.Empty;
			}
			return ratingItemFontInfo.Glyph;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00018AEE File Offset: 0x00016CEE
		private string GetNextGlyphIfNull(string glyph, RatingControlStates fallbackType)
		{
			if (!string.IsNullOrEmpty(glyph))
			{
				return glyph;
			}
			if (fallbackType == RatingControlStates.Null)
			{
				return string.Empty;
			}
			return this.GetAppropriateGlyph(fallbackType);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00018B0C File Offset: 0x00016D0C
		private ImageSource GetAppropriateImageSource(RatingControlStates type)
		{
			this.IsItemInfoPresentAndImageInfo();
			RatingItemImageInfo ratingItemImageInfo = (RatingItemImageInfo)this.ItemInfo;
			switch (type)
			{
			case RatingControlStates.Disabled:
				return this.GetNextImageIfNull(ratingItemImageInfo.DisabledImage, RatingControlStates.Set);
			case RatingControlStates.PointerOverSet:
				return this.GetNextImageIfNull(ratingItemImageInfo.PointerOverImage, RatingControlStates.Set);
			case RatingControlStates.PointerOverPlaceholder:
				return this.GetNextImageIfNull(ratingItemImageInfo.PointerOverPlaceholderImage, RatingControlStates.Placeholder);
			case RatingControlStates.Placeholder:
				return this.GetNextImageIfNull(ratingItemImageInfo.PlaceholderImage, RatingControlStates.Set);
			case RatingControlStates.Unset:
				return this.GetNextImageIfNull(ratingItemImageInfo.UnsetImage, RatingControlStates.Set);
			case RatingControlStates.Null:
				return null;
			}
			return ratingItemImageInfo.Image;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00018B9E File Offset: 0x00016D9E
		private ImageSource GetNextImageIfNull(ImageSource image, RatingControlStates fallbackType)
		{
			if (image != null)
			{
				return image;
			}
			if (fallbackType == RatingControlStates.Null)
			{
				return null;
			}
			return this.GetAppropriateImageSource(fallbackType);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00018BB4 File Offset: 0x00016DB4
		private Geometry GetAppropriatePathData(RatingControlStates type)
		{
			this.IsItemInfoPresentAndPathInfo();
			RatingItemPathInfo ratingItemPathInfo = (RatingItemPathInfo)this.ItemInfo;
			switch (type)
			{
			case RatingControlStates.Disabled:
				return this.GetNextGeometryIfNull(ratingItemPathInfo.DisabledData, RatingControlStates.Set);
			case RatingControlStates.PointerOverSet:
				return this.GetNextGeometryIfNull(ratingItemPathInfo.PointerOverData, RatingControlStates.Set);
			case RatingControlStates.PointerOverPlaceholder:
				return this.GetNextGeometryIfNull(ratingItemPathInfo.PointerOverPlaceholderData, RatingControlStates.Placeholder);
			case RatingControlStates.Placeholder:
				return this.GetNextGeometryIfNull(ratingItemPathInfo.PlaceholderData, RatingControlStates.Set);
			case RatingControlStates.Unset:
				return this.GetNextGeometryIfNull(ratingItemPathInfo.UnsetData, RatingControlStates.Set);
			case RatingControlStates.Null:
				return null;
			}
			return ratingItemPathInfo.Data;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00018C46 File Offset: 0x00016E46
		private Geometry GetNextGeometryIfNull(Geometry geometry, RatingControlStates fallbackType)
		{
			if (geometry != null)
			{
				return geometry;
			}
			if (fallbackType == RatingControlStates.Null)
			{
				return null;
			}
			return this.GetAppropriatePathData(fallbackType);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00018C5C File Offset: 0x00016E5C
		private void ResetControlWidth()
		{
			double width = this.CalculateTotalRatingControlWidth();
			base.Width = width;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00018C78 File Offset: 0x00016E78
		private void ChangeRatingBy(double change, bool originatedFromMouse)
		{
			if (change != 0.0)
			{
				double value = this.Value;
				double num;
				if (value != -1.0)
				{
					if ((double)((int)this.Value) != this.Value)
					{
						if (change == -1.0)
						{
							num = (double)((int)this.Value);
						}
						else
						{
							num = (double)((int)this.Value) + change;
						}
					}
					else
					{
						num = value;
						num += change;
					}
				}
				else
				{
					num = (double)this.InitialSetValue;
				}
				this.SetRatingTo(num, originatedFromMouse);
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00018CFC File Offset: 0x00016EFC
		private void SetRatingTo(double newRating, bool originatedFromMouse)
		{
			double value = this.Value;
			double num = Math.Min(newRating, (double)this.MaxRating);
			num = Math.Max(num, 0.0);
			if (value > -1.0 || num != 0.0)
			{
				if (!this.IsClearEnabled && num <= 0.0)
				{
					base.SetCurrentValue(RatingControl.ValueProperty, 1.0);
				}
				else if (num == value && this.IsClearEnabled && (num != (double)this.MaxRating || originatedFromMouse))
				{
					base.SetCurrentValue(RatingControl.ValueProperty, -1.0);
				}
				else if (num > 0.0)
				{
					base.SetCurrentValue(RatingControl.ValueProperty, num);
				}
				else
				{
					base.SetCurrentValue(RatingControl.ValueProperty, -1.0);
				}
				TypedEventHandler<RatingControl, object> valueChanged = this.ValueChanged;
				if (valueChanged == null)
				{
					return;
				}
				valueChanged.Invoke(this, null);
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00018E04 File Offset: 0x00017004
		private void PrivateOnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (property == RatingControl.MaxRatingProperty)
			{
				int num = (int)args.NewValue;
				int num2 = Math.Max(1, num);
				if (this.Value > (double)num2)
				{
					this.Value = (double)num2;
				}
				if (this.PlaceholderValue > (double)num2)
				{
					this.PlaceholderValue = (double)num2;
				}
				if (num2 != num)
				{
					base.SetValue(property, num2);
					return;
				}
			}
			else if (property == RatingControl.PlaceholderValueProperty || property == RatingControl.ValueProperty)
			{
				double num3 = (double)args.NewValue;
				double num4 = this.CoerceValueBetweenMinAndMax(num3);
				if (num3 != num4)
				{
					base.SetValue(property, num4);
					return;
				}
			}
			if (property == RatingControl.CaptionProperty)
			{
				this.OnCaptionChanged(args);
				return;
			}
			if (property == RatingControl.InitialSetValueProperty)
			{
				this.OnInitialSetValueChanged(args);
				return;
			}
			if (property == RatingControl.IsClearEnabledProperty)
			{
				this.OnIsClearEnabledChanged(args);
				return;
			}
			if (property == RatingControl.IsReadOnlyProperty)
			{
				this.OnIsReadOnlyChanged(args);
				return;
			}
			if (property == RatingControl.ItemInfoProperty)
			{
				this.OnItemInfoChanged(args);
				return;
			}
			if (property == RatingControl.MaxRatingProperty)
			{
				this.OnMaxRatingChanged(args);
				return;
			}
			if (property == RatingControl.PlaceholderValueProperty)
			{
				this.OnPlaceholderValueChanged(args);
				return;
			}
			if (property == RatingControl.ValueProperty)
			{
				this.OnValueChanged(args);
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00018F25 File Offset: 0x00017125
		private void OnCaptionChanged(DependencyPropertyChangedEventArgs args)
		{
			this.ReRenderCaption();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00018F2D File Offset: 0x0001712D
		private static void OnFontFamilyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).OnFontFamilyChanged();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00018F3C File Offset: 0x0001713C
		private void OnFontFamilyChanged()
		{
			if (this.m_backgroundStackPanel != null)
			{
				for (int i = 0; i < this.MaxRating; i++)
				{
					TextBlock textBlock = this.m_backgroundStackPanel.Children[i] as TextBlock;
					if (textBlock != null)
					{
						this.CustomizeRatingItem(textBlock, RatingControlStates.Unset);
					}
					TextBlock textBlock2 = this.m_foregroundStackPanel.Children[i] as TextBlock;
					if (textBlock2 != null)
					{
						this.CustomizeRatingItem(textBlock2, RatingControlStates.Set);
					}
				}
			}
			this.UpdateRatingItemsAppearance();
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00018FAC File Offset: 0x000171AC
		private void OnInitialSetValueChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00018FAE File Offset: 0x000171AE
		private void OnIsClearEnabledChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00018FB0 File Offset: 0x000171B0
		private void OnIsReadOnlyChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00018FB4 File Offset: 0x000171B4
		private void OnItemInfoChanged(DependencyPropertyChangedEventArgs args)
		{
			bool flag = false;
			if (this.ItemInfo == null)
			{
				this.m_infoType = RatingInfoType.None;
			}
			else if (this.ItemInfo is RatingItemFontInfo)
			{
				if (this.m_infoType != RatingInfoType.Font && this.m_backgroundStackPanel != null)
				{
					this.m_infoType = RatingInfoType.Font;
					this.StampOutRatingItems();
					flag = true;
				}
			}
			else if (this.ItemInfo is RatingItemPathInfo)
			{
				if (this.m_infoType != RatingInfoType.Path)
				{
					this.m_infoType = RatingInfoType.Path;
					this.StampOutRatingItems();
					flag = true;
				}
			}
			else if (this.m_infoType != RatingInfoType.Image)
			{
				this.m_infoType = RatingInfoType.Image;
				this.StampOutRatingItems();
				flag = true;
			}
			if (this.m_backgroundStackPanel != null && !flag)
			{
				for (int i = 0; i < this.MaxRating; i++)
				{
					this.CustomizeRatingItem(this.m_backgroundStackPanel.Children[i], RatingControlStates.Unset);
					this.CustomizeRatingItem(this.m_foregroundStackPanel.Children[i], RatingControlStates.Set);
				}
			}
			this.UpdateRatingItemsAppearance();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00019094 File Offset: 0x00017294
		private void OnMaxRatingChanged(DependencyPropertyChangedEventArgs args)
		{
			this.StampOutRatingItems();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001909C File Offset: 0x0001729C
		private void OnPlaceholderValueChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateRatingItemsAppearance();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000190A4 File Offset: 0x000172A4
		private void OnValueChanged(DependencyPropertyChangedEventArgs args)
		{
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
			if (automationPeer != null)
			{
				((RatingControlAutomationPeer)automationPeer).RaisePropertyChangedEvent(this.Value);
			}
			this.UpdateRatingItemsAppearance();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000190D2 File Offset: 0x000172D2
		private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			this.UpdateRatingItemsAppearance();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000190DA File Offset: 0x000172DA
		private void OnCaptionSizeChanged(object sender, SizeChangedEventArgs args)
		{
			this.ResetControlWidth();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x000190E2 File Offset: 0x000172E2
		private void OnPointerCaptureLostBackgroundStackPanel(object sender, MouseEventArgs args)
		{
			this.PointerExitedImpl(args, false);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000190EC File Offset: 0x000172EC
		private void OnPointerMovedOverBackgroundStackPanel(object sender, MouseEventArgs args)
		{
			if (!this.IsReadOnly)
			{
				double x = args.GetPosition(this.m_backgroundStackPanel).X;
				this.m_mousePercentage = x / this.CalculateActualRatingWidth();
				this.UpdateRatingItemsAppearance();
				args.Handled = true;
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00019131 File Offset: 0x00017331
		private void OnPointerEnteredBackgroundStackPanel(object sender, MouseEventArgs args)
		{
			if (!this.IsReadOnly)
			{
				this.m_isPointerOver = true;
				args.Handled = true;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00019149 File Offset: 0x00017349
		private void OnPointerExitedBackgroundStackPanel(object sender, MouseEventArgs args)
		{
			this.PointerExitedImpl(args, true);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00019153 File Offset: 0x00017353
		private void PointerExitedImpl(MouseEventArgs args, bool resetScaleAnimation = true)
		{
			args.GetPosition(this.m_backgroundStackPanel);
			if (resetScaleAnimation)
			{
				this.m_isPointerOver = false;
			}
			if (!this.m_isPointerDown)
			{
				this.UpdateRatingItemsAppearance();
			}
			args.Handled = true;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00019181 File Offset: 0x00017381
		private void OnPointerPressedBackgroundStackPanel(object sender, MouseButtonEventArgs args)
		{
			if (!this.IsReadOnly)
			{
				this.m_isPointerDown = true;
				this.m_backgroundStackPanel.CaptureMouse();
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000191A0 File Offset: 0x000173A0
		private void OnPointerReleasedBackgroundStackPanel(object sender, MouseButtonEventArgs args)
		{
			if (!this.IsReadOnly)
			{
				double num = args.GetPosition(this.m_backgroundStackPanel).X / this.CalculateActualRatingWidth();
				this.SetRatingTo(Math.Ceiling(num * (double)this.MaxRating), true);
			}
			if (this.m_isPointerDown)
			{
				this.m_isPointerDown = false;
				this.UpdateRatingItemsAppearance();
			}
			this.m_backgroundStackPanel.ReleaseMouseCapture();
			base.Focus();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00019210 File Offset: 0x00017410
		private double CalculateTotalRatingControlWidth()
		{
			double num = this.CalculateActualRatingWidth();
			string text = (string)base.GetValue(RatingControl.CaptionProperty);
			double num2 = 0.0;
			if (text.Length > 0)
			{
				num2 = this.ItemSpacing;
			}
			double num3 = 0.0;
			if (this.m_captionTextBlock != null)
			{
				num3 = this.m_captionTextBlock.ActualWidth;
			}
			return num + num2 + num3;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001926E File Offset: 0x0001746E
		private double CalculateActualRatingWidth()
		{
			return (double)this.MaxRating * this.ActualRatingFontSize + (double)(this.MaxRating - 1) * this.ItemSpacing;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00019290 File Offset: 0x00017490
		protected override void OnKeyDown(KeyEventArgs eventArgs)
		{
			if (eventArgs.Handled)
			{
				return;
			}
			if (!this.IsReadOnly)
			{
				bool handled = false;
				Key key = eventArgs.Key;
				double num = 1.0;
				if (base.FlowDirection == 1)
				{
					num *= -1.0;
				}
				Key key2 = eventArgs.Key;
				if (key2 == 24)
				{
					key = 25;
					num = 1.0;
				}
				else if (key2 == 26)
				{
					key = 23;
					num = 1.0;
				}
				switch (key)
				{
				case 21:
					this.SetRatingTo((double)this.MaxRating, false);
					handled = true;
					break;
				case 22:
					this.SetRatingTo(0.0, false);
					handled = true;
					break;
				case 23:
					this.ChangeRatingBy(-1.0 * num, false);
					handled = true;
					break;
				case 25:
					this.ChangeRatingBy(1.0 * num, false);
					handled = true;
					break;
				}
				eventArgs.Handled = handled;
			}
			base.OnKeyDown(eventArgs);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00019384 File Offset: 0x00017584
		private void RecycleEvents()
		{
			StackPanel backgroundStackPanel = this.m_backgroundStackPanel;
			if (backgroundStackPanel != null)
			{
				backgroundStackPanel.LostMouseCapture -= new MouseEventHandler(this.OnPointerCaptureLostBackgroundStackPanel);
				backgroundStackPanel.MouseMove -= new MouseEventHandler(this.OnPointerMovedOverBackgroundStackPanel);
				backgroundStackPanel.MouseEnter -= new MouseEventHandler(this.OnPointerEnteredBackgroundStackPanel);
				backgroundStackPanel.MouseLeave -= new MouseEventHandler(this.OnPointerExitedBackgroundStackPanel);
				backgroundStackPanel.MouseDown -= new MouseButtonEventHandler(this.OnPointerPressedBackgroundStackPanel);
				backgroundStackPanel.MouseUp -= new MouseButtonEventHandler(this.OnPointerReleasedBackgroundStackPanel);
			}
			TextBlock captionTextBlock = this.m_captionTextBlock;
			if (captionTextBlock != null)
			{
				captionTextBlock.SizeChanged -= new SizeChangedEventHandler(this.OnCaptionSizeChanged);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00019423 File Offset: 0x00017623
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x00019435 File Offset: 0x00017635
		public string Caption
		{
			get
			{
				return (string)base.GetValue(RatingControl.CaptionProperty);
			}
			set
			{
				base.SetValue(RatingControl.CaptionProperty, value);
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00019443 File Offset: 0x00017643
		private static void OnCaptionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00019451 File Offset: 0x00017651
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00019463 File Offset: 0x00017663
		public int InitialSetValue
		{
			get
			{
				return (int)base.GetValue(RatingControl.InitialSetValueProperty);
			}
			set
			{
				base.SetValue(RatingControl.InitialSetValueProperty, value);
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00019476 File Offset: 0x00017676
		private static void OnInitialSetValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00019484 File Offset: 0x00017684
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00019496 File Offset: 0x00017696
		public bool IsClearEnabled
		{
			get
			{
				return (bool)base.GetValue(RatingControl.IsClearEnabledProperty);
			}
			set
			{
				base.SetValue(RatingControl.IsClearEnabledProperty, value);
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000194A9 File Offset: 0x000176A9
		private static void OnIsClearEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x000194B7 File Offset: 0x000176B7
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x000194C9 File Offset: 0x000176C9
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(RatingControl.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(RatingControl.IsReadOnlyProperty, value);
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000194DC File Offset: 0x000176DC
		private static void OnIsReadOnlyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000194EA File Offset: 0x000176EA
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x000194FC File Offset: 0x000176FC
		public RatingItemInfo ItemInfo
		{
			get
			{
				return (RatingItemInfo)base.GetValue(RatingControl.ItemInfoProperty);
			}
			set
			{
				base.SetValue(RatingControl.ItemInfoProperty, value);
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001950A File Offset: 0x0001770A
		private static void OnItemInfoPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00019518 File Offset: 0x00017718
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0001952A File Offset: 0x0001772A
		public int MaxRating
		{
			get
			{
				return (int)base.GetValue(RatingControl.MaxRatingProperty);
			}
			set
			{
				base.SetValue(RatingControl.MaxRatingProperty, value);
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001953D File Offset: 0x0001773D
		private static void OnMaxRatingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001954B File Offset: 0x0001774B
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001955D File Offset: 0x0001775D
		public double PlaceholderValue
		{
			get
			{
				return (double)base.GetValue(RatingControl.PlaceholderValueProperty);
			}
			set
			{
				base.SetValue(RatingControl.PlaceholderValueProperty, value);
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00019570 File Offset: 0x00017770
		private static void OnPlaceholderValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001957E File Offset: 0x0001777E
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00019590 File Offset: 0x00017790
		public double Value
		{
			get
			{
				return (double)base.GetValue(RatingControl.ValueProperty);
			}
			set
			{
				base.SetValue(RatingControl.ValueProperty, value);
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000195A3 File Offset: 0x000177A3
		private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((RatingControl)sender).PrivateOnPropertyChanged(args);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x000195B1 File Offset: 0x000177B1
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x000195C3 File Offset: 0x000177C3
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(RatingControl.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(RatingControl.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060006EC RID: 1772 RVA: 0x000195D8 File Offset: 0x000177D8
		// (remove) Token: 0x060006ED RID: 1773 RVA: 0x00019610 File Offset: 0x00017810
		public event TypedEventHandler<RatingControl, object> ValueChanged;

		// Token: 0x04000314 RID: 788
		private const double c_horizontalScaleAnimationCenterPoint = 0.5;

		// Token: 0x04000315 RID: 789
		private const double c_verticalScaleAnimationCenterPoint = 0.8;

		// Token: 0x04000316 RID: 790
		private static readonly Thickness c_focusVisualMargin = new Thickness(-8.0, -7.0, -8.0, 0.0);

		// Token: 0x04000317 RID: 791
		private const int c_defaultRatingFontSizeForRendering = 32;

		// Token: 0x04000318 RID: 792
		private const int c_defaultItemSpacing = 8;

		// Token: 0x04000319 RID: 793
		private const double c_defaultCaptionTopMargin = 22.0;

		// Token: 0x0400031A RID: 794
		private const double c_noValueSetSentinel = -1.0;

		// Token: 0x0400031B RID: 795
		private TextBlock m_captionTextBlock;

		// Token: 0x0400031C RID: 796
		private StackPanel m_backgroundStackPanel;

		// Token: 0x0400031D RID: 797
		private StackPanel m_foregroundStackPanel;

		// Token: 0x0400031E RID: 798
		private bool m_isPointerOver;

		// Token: 0x0400031F RID: 799
		private bool m_isPointerDown;

		// Token: 0x04000320 RID: 800
		private double m_mousePercentage;

		// Token: 0x04000321 RID: 801
		private RatingInfoType m_infoType = RatingInfoType.Font;

		// Token: 0x04000322 RID: 802
		public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(RatingControl), new PropertyMetadata(string.Empty, new PropertyChangedCallback(RatingControl.OnCaptionPropertyChanged)));

		// Token: 0x04000323 RID: 803
		public static readonly DependencyProperty InitialSetValueProperty = DependencyProperty.Register("InitialSetValue", typeof(int), typeof(RatingControl), new PropertyMetadata(1, new PropertyChangedCallback(RatingControl.OnInitialSetValuePropertyChanged)));

		// Token: 0x04000324 RID: 804
		public static readonly DependencyProperty IsClearEnabledProperty = DependencyProperty.Register("IsClearEnabled", typeof(bool), typeof(RatingControl), new PropertyMetadata(true, new PropertyChangedCallback(RatingControl.OnIsClearEnabledPropertyChanged)));

		// Token: 0x04000325 RID: 805
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RatingControl), new PropertyMetadata(false, new PropertyChangedCallback(RatingControl.OnIsReadOnlyPropertyChanged)));

		// Token: 0x04000326 RID: 806
		public static readonly DependencyProperty ItemInfoProperty = DependencyProperty.Register("ItemInfo", typeof(RatingItemInfo), typeof(RatingControl), new PropertyMetadata(new PropertyChangedCallback(RatingControl.OnItemInfoPropertyChanged)));

		// Token: 0x04000327 RID: 807
		public static readonly DependencyProperty MaxRatingProperty = DependencyProperty.Register("MaxRating", typeof(int), typeof(RatingControl), new PropertyMetadata(5, new PropertyChangedCallback(RatingControl.OnMaxRatingPropertyChanged)));

		// Token: 0x04000328 RID: 808
		public static readonly DependencyProperty PlaceholderValueProperty = DependencyProperty.Register("PlaceholderValue", typeof(double), typeof(RatingControl), new PropertyMetadata(-1.0, new PropertyChangedCallback(RatingControl.OnPlaceholderValuePropertyChanged)));

		// Token: 0x04000329 RID: 809
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata(-1.0, new PropertyChangedCallback(RatingControl.OnValuePropertyChanged)));

		// Token: 0x0400032A RID: 810
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(RatingControl));
	}
}
