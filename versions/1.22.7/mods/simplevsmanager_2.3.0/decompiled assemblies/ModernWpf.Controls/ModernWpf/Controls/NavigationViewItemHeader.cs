using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x02000067 RID: 103
	public class NavigationViewItemHeader : NavigationViewItemBase
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x00012F40 File Offset: 0x00011140
		static NavigationViewItemHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationViewItemHeader), new FrameworkPropertyMetadata(typeof(NavigationViewItemHeader)));
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00012F70 File Offset: 0x00011170
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				splitView.IsPaneOpenChanged += this.OnSplitViewPropertyChanged;
				splitView.DisplayModeChanged += this.OnSplitViewPropertyChanged;
				this.UpdateIsClosedCompact();
			}
			Grid grid = base.GetTemplateChild("NavigationViewItemHeaderRootGrid") as Grid;
			if (grid != null)
			{
				this.m_rootGrid = grid;
			}
			this.UpdateVisualState(false);
			this.UpdateItemIndentation();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00012FDF File Offset: 0x000111DF
		private void OnSplitViewPropertyChanged(DependencyObject sender, DependencyProperty args)
		{
			if (args == SplitView.IsPaneOpenProperty || args == SplitView.DisplayModeProperty)
			{
				this.UpdateIsClosedCompact();
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00012FF8 File Offset: 0x000111F8
		private void UpdateIsClosedCompact()
		{
			SplitView splitView = base.GetSplitView();
			if (splitView != null)
			{
				this.m_isClosedCompact = (!splitView.IsPaneOpen && (splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay || splitView.DisplayMode == SplitViewDisplayMode.CompactInline));
				this.UpdateVisualState(true);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001303C File Offset: 0x0001123C
		private void UpdateVisualState(bool useTransitions)
		{
			VisualStateManager.GoToState(this, (this.m_isClosedCompact && base.IsTopLevelItem) ? "HeaderTextCollapsed" : "HeaderTextVisible", useTransitions);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00013062 File Offset: 0x00011262
		private protected override void OnNavigationViewItemBaseDepthChanged()
		{
			this.UpdateItemIndentation();
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001306C File Offset: 0x0001126C
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

		// Token: 0x0400025F RID: 607
		private const string c_rootGrid = "NavigationViewItemHeaderRootGrid";

		// Token: 0x04000260 RID: 608
		private bool m_isClosedCompact;

		// Token: 0x04000261 RID: 609
		private Grid m_rootGrid;
	}
}
