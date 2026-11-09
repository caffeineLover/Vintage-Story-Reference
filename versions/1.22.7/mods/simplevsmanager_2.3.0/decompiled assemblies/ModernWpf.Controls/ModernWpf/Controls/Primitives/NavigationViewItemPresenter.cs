using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ModernWpf.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000F0 RID: 240
	public class NavigationViewItemPresenter : ContentControl, IControlProtected
	{
		// Token: 0x06000C51 RID: 3153 RVA: 0x0002A204 File Offset: 0x00028404
		static NavigationViewItemPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItemPresenter), new FrameworkPropertyMetadata(typeof(NavigationViewItemPresenter)));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(NavigationViewItemPresenter), new FrameworkPropertyMetadata(1));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(NavigationViewItemPresenter), new FrameworkPropertyMetadata(1));
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0002A2C8 File Offset: 0x000284C8
		public NavigationViewItemPresenter()
		{
			InputHelper.SetIsTapEnabled(this, true);
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x0002A2F1 File Offset: 0x000284F1
		// (set) Token: 0x06000C54 RID: 3156 RVA: 0x0002A303 File Offset: 0x00028503
		public IconElement Icon
		{
			get
			{
				return (IconElement)base.GetValue(NavigationViewItemPresenter.IconProperty);
			}
			set
			{
				base.SetValue(NavigationViewItemPresenter.IconProperty, value);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0002A311 File Offset: 0x00028511
		// (set) Token: 0x06000C56 RID: 3158 RVA: 0x0002A323 File Offset: 0x00028523
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItemPresenter.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(NavigationViewItemPresenter.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0002A336 File Offset: 0x00028536
		// (set) Token: 0x06000C58 RID: 3160 RVA: 0x0002A348 File Offset: 0x00028548
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(NavigationViewItemPresenter.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(NavigationViewItemPresenter.CornerRadiusProperty, value);
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0002A35C File Offset: 0x0002855C
		public override void OnApplyTemplate()
		{
			this.m_helper.Init(this);
			Grid templateChildT = CppWinRTHelpers.GetTemplateChildT<Grid>("PresenterContentRootGrid", this);
			if (templateChildT != null)
			{
				this.m_contentGrid = templateChildT;
			}
			NavigationViewItem navigationViewItem = this.GetNavigationViewItem();
			if (navigationViewItem != null)
			{
				Grid templateChildT2 = CppWinRTHelpers.GetTemplateChildT<Grid>("ExpandCollapseChevron", this);
				if (templateChildT2 != null)
				{
					this.m_expandCollapseChevron = templateChildT2;
					InputHelper.SetIsTapEnabled(templateChildT2, true);
					InputHelper.AddTappedHandler(templateChildT2, new TappedEventHandler(navigationViewItem.OnExpandCollapseChevronTapped));
				}
				navigationViewItem.UpdateVisualStateNoTransition();
				navigationViewItem.UpdateIsClosedCompact();
				NavigationView navigationView = navigationViewItem.GetNavigationView();
				if (navigationView != null && navigationView.PaneDisplayMode != NavigationViewPaneDisplayMode.Top)
				{
					this.UpdateCompactPaneLength(this.m_compactPaneLengthValue, true);
				}
			}
			FrameworkElement templateRoot = ControlExtensions.GetTemplateRoot(this);
			if (templateRoot != null)
			{
				this.m_chevronExpandedStoryboard = (templateRoot.Resources["ExpandCollapseRotateExpandedStoryboard"] as Storyboard);
				this.m_chevronCollapsedStoryboard = (templateRoot.Resources["ExpandCollapseRotateCollapsedStoryboard"] as Storyboard);
			}
			this.m_expandCollapseRotateTransform = CppWinRTHelpers.GetTemplateChildT<RotateTransform>("ExpandCollapseChevronRotateTransform", this);
			this.UpdateMargin();
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0002A44C File Offset: 0x0002864C
		internal void RotateExpandCollapseChevron(bool isExpanded)
		{
			if (isExpanded)
			{
				Storyboard chevronExpandedStoryboard = this.m_chevronExpandedStoryboard;
				if (chevronExpandedStoryboard != null)
				{
					chevronExpandedStoryboard.Begin();
				}
				if (this.m_expandCollapseRotateTransform != null)
				{
					this.m_expandCollapseRotateTransform.Angle = 180.0;
					return;
				}
			}
			else
			{
				Storyboard chevronCollapsedStoryboard = this.m_chevronCollapsedStoryboard;
				if (chevronCollapsedStoryboard != null)
				{
					chevronCollapsedStoryboard.Begin();
				}
				if (this.m_expandCollapseRotateTransform != null)
				{
					this.m_expandCollapseRotateTransform.Angle = 0.0;
				}
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0002A4B5 File Offset: 0x000286B5
		internal UIElement GetSelectionIndicator()
		{
			return this.m_helper.GetSelectionIndicator();
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0002A4C4 File Offset: 0x000286C4
		private NavigationViewItem GetNavigationViewItem()
		{
			NavigationViewItem result = null;
			NavigationViewItem ancestorOfType = SharedHelpers.GetAncestorOfType<NavigationViewItem>(VisualTreeHelper.GetParent(this));
			if (ancestorOfType != null)
			{
				result = ancestorOfType;
			}
			return result;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0002A4E5 File Offset: 0x000286E5
		internal void UpdateContentLeftIndentation(double leftIndentation)
		{
			this.m_leftIndentation = leftIndentation;
			this.UpdateMargin();
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002A4F4 File Offset: 0x000286F4
		private void UpdateMargin()
		{
			Grid contentGrid = this.m_contentGrid;
			if (contentGrid != null)
			{
				Thickness margin = contentGrid.Margin;
				contentGrid.Margin = new Thickness(this.m_leftIndentation, margin.Top, margin.Right, margin.Bottom);
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0002A538 File Offset: 0x00028738
		internal void UpdateCompactPaneLength(double compactPaneLength, bool shouldUpdate)
		{
			this.m_compactPaneLengthValue = compactPaneLength;
			if (shouldUpdate)
			{
				ColumnDefinition templateChildT = CppWinRTHelpers.GetTemplateChildT<ColumnDefinition>("IconColumn", this);
				if (templateChildT != null)
				{
					ColumnDefinitionHelper.SetPixelWidth(templateChildT, compactPaneLength);
				}
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0002A568 File Offset: 0x00028768
		internal void UpdateClosedCompactVisualState(bool isTopLevelItem, bool isClosedCompact)
		{
			string text = (isClosedCompact && isTopLevelItem) ? "ClosedCompactAndTopLevelItem" : "NotClosedCompactAndTopLevelItem";
			VisualStateManager.GoToState(this, text, false);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0002A590 File Offset: 0x00028790
		DependencyObject IControlProtected.GetTemplateChild(string childName)
		{
			return base.GetTemplateChild(childName);
		}

		// Token: 0x04000525 RID: 1317
		private const string c_contentGrid = "PresenterContentRootGrid";

		// Token: 0x04000526 RID: 1318
		private const string c_expandCollapseChevron = "ExpandCollapseChevron";

		// Token: 0x04000527 RID: 1319
		private const string c_expandCollapseRotateExpandedStoryboard = "ExpandCollapseRotateExpandedStoryboard";

		// Token: 0x04000528 RID: 1320
		private const string c_expandCollapseRotateCollapsedStoryboard = "ExpandCollapseRotateCollapsedStoryboard";

		// Token: 0x04000529 RID: 1321
		private const string c_expandCollapseRotateTransform = "ExpandCollapseChevronRotateTransform";

		// Token: 0x0400052A RID: 1322
		private const string c_iconBoxColumnDefinitionName = "IconColumn";

		// Token: 0x0400052B RID: 1323
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(IconElement), typeof(NavigationViewItemPresenter), null);

		// Token: 0x0400052C RID: 1324
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(NavigationViewItemPresenter));

		// Token: 0x0400052D RID: 1325
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(NavigationViewItemPresenter));

		// Token: 0x0400052E RID: 1326
		private double m_compactPaneLengthValue = 40.0;

		// Token: 0x0400052F RID: 1327
		private NavigationViewItemHelper<NavigationViewItemPresenter> m_helper = new NavigationViewItemHelper<NavigationViewItemPresenter>();

		// Token: 0x04000530 RID: 1328
		private Grid m_contentGrid;

		// Token: 0x04000531 RID: 1329
		private Grid m_expandCollapseChevron;

		// Token: 0x04000532 RID: 1330
		private double m_leftIndentation;

		// Token: 0x04000533 RID: 1331
		private Storyboard m_chevronExpandedStoryboard;

		// Token: 0x04000534 RID: 1332
		private Storyboard m_chevronCollapsedStoryboard;

		// Token: 0x04000535 RID: 1333
		private RotateTransform m_expandCollapseRotateTransform;
	}
}
