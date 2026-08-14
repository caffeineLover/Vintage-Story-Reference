using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000298 RID: 664
	public class PivotHeaderScrollViewer : ScrollViewer
	{
		// Token: 0x060013BB RID: 5051 RVA: 0x0003A2C0 File Offset: 0x000384C0
		static PivotHeaderScrollViewer()
		{
			FrameworkElement.FlowDirectionProperty.OverrideMetadata(typeof(PivotHeaderScrollViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(PivotHeaderScrollViewer.OnFlowDirectionChanged)));
			ScrollViewer.HorizontalScrollBarVisibilityProperty.OverrideMetadata(typeof(PivotHeaderScrollViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(PivotHeaderScrollViewer.OnHorizontalScrollBarVisibilityChanged)));
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0003A391 File Offset: 0x00038591
		public PivotHeaderScrollViewer()
		{
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0003A3AB File Offset: 0x000385AB
		// (set) Token: 0x060013BE RID: 5054 RVA: 0x0003A3BD File Offset: 0x000385BD
		public bool CanScrollLeft
		{
			get
			{
				return (bool)base.GetValue(PivotHeaderScrollViewer.CanScrollLeftProperty);
			}
			private set
			{
				base.SetValue(PivotHeaderScrollViewer.CanScrollLeftPropertyKey, value);
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0003A3D0 File Offset: 0x000385D0
		// (set) Token: 0x060013C0 RID: 5056 RVA: 0x0003A3E2 File Offset: 0x000385E2
		public bool CanScrollRight
		{
			get
			{
				return (bool)base.GetValue(PivotHeaderScrollViewer.CanScrollRightProperty);
			}
			private set
			{
				base.SetValue(PivotHeaderScrollViewer.CanScrollRightPropertyKey, value);
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0003A3F8 File Offset: 0x000385F8
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			if (this._tabControl != null)
			{
				this._tabControl.SelectionChanged -= new SelectionChangedEventHandler(this.OnTabControlSelectionChanged);
			}
			base.OnVisualParentChanged(oldParent);
			this._tabControl = (base.TemplatedParent as TabControl);
			if (this._tabControl != null)
			{
				this._tabControl.SelectionChanged += new SelectionChangedEventHandler(this.OnTabControlSelectionChanged);
			}
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0003A45C File Offset: 0x0003865C
		protected override void OnScrollChanged(ScrollChangedEventArgs e)
		{
			base.OnScrollChanged(e);
			if (e.HorizontalChange != 0.0 || e.ExtentWidthChange != 0.0 || e.ViewportWidthChange != 0.0)
			{
				this.UpdateCanScrollHorizontally();
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0003A4A9 File Offset: 0x000386A9
		protected override void OnKeyDown(KeyEventArgs e)
		{
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0003A4AB File Offset: 0x000386AB
		private static void OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PivotHeaderScrollViewer)d).UpdateCanScrollHorizontally();
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0003A4B8 File Offset: 0x000386B8
		private static void OnHorizontalScrollBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PivotHeaderScrollViewer)d).UpdateCanScrollHorizontally();
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0003A4C8 File Offset: 0x000386C8
		private void UpdateCanScrollHorizontally()
		{
			bool flag = this.CanScrollHorizontallyInDirection(false);
			if (this.CanScrollLeft != flag)
			{
				this.CanScrollLeft = flag;
			}
			bool flag2 = this.CanScrollHorizontallyInDirection(true);
			if (this.CanScrollRight != flag2)
			{
				this.CanScrollRight = flag2;
			}
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0003A508 File Offset: 0x00038708
		private bool CanScrollHorizontallyInDirection(bool inPositiveDirection)
		{
			bool result = false;
			if (base.FlowDirection == 1)
			{
				inPositiveDirection = !inPositiveDirection;
			}
			if (base.HorizontalScrollBarVisibility != null)
			{
				double extentWidth = base.ExtentWidth;
				double viewportWidth = base.ViewportWidth;
				if (extentWidth > viewportWidth)
				{
					if (inPositiveDirection)
					{
						double num = extentWidth - viewportWidth;
						if (base.HorizontalOffset < num)
						{
							result = true;
						}
					}
					else if (base.HorizontalOffset > 0.0)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0003A568 File Offset: 0x00038768
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.BringSelectedTabItemIntoView();
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0003A570 File Offset: 0x00038770
		private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.BringSelectedTabItemIntoView();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0003A578 File Offset: 0x00038778
		private void BringSelectedTabItemIntoView()
		{
			if (this._tabControl != null)
			{
				TabItem selectedTabItem = PivotHeaderScrollViewer.GetSelectedTabItem(this._tabControl);
				if (selectedTabItem != null)
				{
					selectedTabItem.BringIntoView();
				}
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0003A5A4 File Offset: 0x000387A4
		private static TabItem GetSelectedTabItem(TabControl tabControl)
		{
			object selectedItem = tabControl.SelectedItem;
			if (selectedItem != null)
			{
				TabItem tabItem = selectedItem as TabItem;
				if (tabItem == null)
				{
					tabItem = (tabControl.ItemContainerGenerator.ContainerFromIndex(tabControl.SelectedIndex) as TabItem);
					if (tabItem == null || !PivotHeaderScrollViewer.EqualsEx(selectedItem, tabControl.ItemContainerGenerator.ItemFromContainer(tabItem)))
					{
						tabItem = (tabControl.ItemContainerGenerator.ContainerFromItem(selectedItem) as TabItem);
					}
				}
				return tabItem;
			}
			return null;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0003A608 File Offset: 0x00038808
		private static bool EqualsEx(object o1, object o2)
		{
			bool result;
			try
			{
				result = object.Equals(o1, o2);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0400069F RID: 1695
		private TabControl _tabControl;

		// Token: 0x040006A0 RID: 1696
		private static readonly DependencyPropertyKey CanScrollLeftPropertyKey = DependencyProperty.RegisterReadOnly("CanScrollLeft", typeof(bool), typeof(PivotHeaderScrollViewer), new PropertyMetadata(false));

		// Token: 0x040006A1 RID: 1697
		public static readonly DependencyProperty CanScrollLeftProperty = PivotHeaderScrollViewer.CanScrollLeftPropertyKey.DependencyProperty;

		// Token: 0x040006A2 RID: 1698
		private static readonly DependencyPropertyKey CanScrollRightPropertyKey = DependencyProperty.RegisterReadOnly("CanScrollRight", typeof(bool), typeof(PivotHeaderScrollViewer), new PropertyMetadata(false));

		// Token: 0x040006A3 RID: 1699
		public static readonly DependencyProperty CanScrollRightProperty = PivotHeaderScrollViewer.CanScrollRightPropertyKey.DependencyProperty;
	}
}
