using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E7 RID: 231
	public class CommandBarOverflowPanel : ToolBarOverflowPanel
	{
		// Token: 0x06000BCF RID: 3023 RVA: 0x000287D8 File Offset: 0x000269D8
		public CommandBarOverflowPanel()
		{
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x000287F2 File Offset: 0x000269F2
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x000287FA File Offset: 0x000269FA
		internal bool HasToggleButton { get; private set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00028803 File Offset: 0x00026A03
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x0002880B File Offset: 0x00026A0B
		internal bool HasMenuIcon { get; private set; }

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00028814 File Offset: 0x00026A14
		protected override Size MeasureOverride(Size constraint)
		{
			base.MeasureOverride(constraint);
			Size result = default(Size);
			UIElementCollection internalChildren = base.InternalChildren;
			Size size = constraint;
			size.Height = double.PositiveInfinity;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					AppBarSeparator appBarSeparator = uielement as AppBarSeparator;
					if (appBarSeparator != null && this.IsPrimaryCommand(appBarSeparator))
					{
						this.UpdateSeparatorVisibility(i, appBarSeparator);
					}
					uielement.Measure(size);
					Size desiredSize = uielement.DesiredSize;
					result.Width = Math.Max(result.Width, desiredSize.Width);
					result.Height += desiredSize.Height;
				}
				i++;
			}
			return result;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000288D0 File Offset: 0x00026AD0
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			Rect rect;
			rect..ctor(arrangeBounds);
			double num = 0.0;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					rect.Y += num;
					num = uielement.DesiredSize.Height;
					rect.Height = num;
					rect.Width = Math.Max(arrangeBounds.Width, uielement.DesiredSize.Width);
					uielement.Arrange(rect);
				}
				i++;
			}
			return arrangeBounds;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0002896C File Offset: 0x00026B6C
		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			if (visualAdded != null)
			{
				this.UpdateChildrenApplicationViewState();
			}
			AppBarSeparator appBarSeparator = visualRemoved as AppBarSeparator;
			if (appBarSeparator != null && this.IsPrimaryCommand(appBarSeparator))
			{
				this.RestoreSeparatorVisibility(appBarSeparator);
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000289A4 File Offset: 0x00026BA4
		private void UpdateChildrenApplicationViewState()
		{
			bool flag = false;
			bool flag2 = false;
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement.IsVisible)
				{
					AppBarButton appBarButton = uielement as AppBarButton;
					if (appBarButton != null)
					{
						if (!flag2 && appBarButton.Icon != null)
						{
							flag2 = true;
						}
					}
					else
					{
						AppBarToggleButton appBarToggleButton = uielement as AppBarToggleButton;
						if (appBarToggleButton != null)
						{
							if (!flag2 && appBarToggleButton.Icon != null)
							{
								flag2 = true;
							}
							if (!flag)
							{
								flag = true;
							}
						}
					}
					if (flag2 && flag)
					{
						break;
					}
				}
				i++;
			}
			this.HasToggleButton = flag;
			this.HasMenuIcon = flag2;
			int j = 0;
			int count2 = internalChildren.Count;
			while (j < count2)
			{
				IAppBarElement appBarElement = internalChildren[j] as IAppBarElement;
				if (appBarElement != null)
				{
					appBarElement.UpdateApplicationViewState();
				}
				j++;
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00028A66 File Offset: 0x00026C66
		private bool IsPrimaryCommand(DependencyObject element)
		{
			return ToolBar.GetOverflowMode(element) != 1;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00028A74 File Offset: 0x00026C74
		private void UpdateSeparatorVisibility(int index, AppBarSeparator separator)
		{
			Visibility visibility = separator.Visibility;
			if (index == 0)
			{
				if (visibility == null)
				{
					separator.SetCurrentValue(UIElement.VisibilityProperty, 2);
					return;
				}
			}
			else
			{
				this.RestoreSeparatorVisibility(separator);
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00028AA8 File Offset: 0x00026CA8
		private void RestoreSeparatorVisibility(AppBarSeparator separator)
		{
			if (separator.Visibility == 2 && DependencyPropertyHelper.GetValueSource(separator, UIElement.VisibilityProperty).IsCurrent)
			{
				separator.InvalidateProperty(UIElement.VisibilityProperty);
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00028ADE File Offset: 0x00026CDE
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.UpdateChildrenApplicationViewState();
		}
	}
}
