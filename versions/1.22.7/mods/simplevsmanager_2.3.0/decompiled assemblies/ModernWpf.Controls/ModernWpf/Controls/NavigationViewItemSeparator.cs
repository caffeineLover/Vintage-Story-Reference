using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x02000069 RID: 105
	public class NavigationViewItemSeparator : NavigationViewItemBase
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x00013102 File Offset: 0x00011302
		static NavigationViewItemSeparator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItemSeparator), new FrameworkPropertyMetadata(typeof(NavigationViewItemSeparator)));
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00013130 File Offset: 0x00011330
		private void UpdateVisualState(bool useTransitions)
		{
			if (this.m_appliedTemplate)
			{
				string stateName = (base.Position != NavigationViewRepeaterPosition.TopPrimary && base.Position != NavigationViewRepeaterPosition.TopFooter) ? (this.m_isClosedCompact ? "HorizontalLineCompact" : "HorizontalLine") : "VerticalLine";
				VisualStateUtil.GoToStateIfGroupExists(this, "NavigationSeparatorLineStates", stateName, false);
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00013180 File Offset: 0x00011380
		public override void OnApplyTemplate()
		{
			this.m_appliedTemplate = false;
			base.OnApplyTemplate();
			Grid templateChildT = CppWinRTHelpers.GetTemplateChildT<Grid>("NavigationViewItemSeparatorRootGrid", this);
			if (templateChildT != null)
			{
				this.m_rootGrid = templateChildT;
			}
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				splitView.IsPaneOpenChanged += this.OnSplitViewPropertyChanged;
				splitView.DisplayModeChanged += this.OnSplitViewPropertyChanged;
				this.UpdateIsClosedCompact(false);
			}
			this.m_appliedTemplate = true;
			this.UpdateVisualState(false);
			this.UpdateItemIndentation();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000131F9 File Offset: 0x000113F9
		private protected override void OnNavigationViewItemBaseDepthChanged()
		{
			this.UpdateVisualState(false);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00013202 File Offset: 0x00011402
		private protected override void OnNavigationViewItemBasePositionChanged()
		{
			this.UpdateVisualState(false);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001320B File Offset: 0x0001140B
		private void OnSplitViewPropertyChanged(DependencyObject sender, DependencyProperty args)
		{
			this.UpdateIsClosedCompact(true);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00013214 File Offset: 0x00011414
		private void UpdateItemIndentation()
		{
			Grid rootGrid = this.m_rootGrid;
			if (rootGrid != null)
			{
				Thickness margin = rootGrid.Margin;
				int num = base.Depth * 25;
				rootGrid.Margin = new Thickness((double)num, margin.Top, margin.Right, margin.Bottom);
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00013260 File Offset: 0x00011460
		private void UpdateIsClosedCompact(bool updateVisualState)
		{
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				this.m_isClosedCompact = (!splitView.IsPaneOpen && (splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay || splitView.DisplayMode == SplitViewDisplayMode.CompactInline));
				if (updateVisualState)
				{
					this.UpdateVisualState(false);
				}
			}
		}

		// Token: 0x04000266 RID: 614
		private const string c_rootGrid = "NavigationViewItemSeparatorRootGrid";

		// Token: 0x04000267 RID: 615
		private bool m_appliedTemplate;

		// Token: 0x04000268 RID: 616
		private bool m_isClosedCompact;

		// Token: 0x04000269 RID: 617
		private Grid m_rootGrid;
	}
}
