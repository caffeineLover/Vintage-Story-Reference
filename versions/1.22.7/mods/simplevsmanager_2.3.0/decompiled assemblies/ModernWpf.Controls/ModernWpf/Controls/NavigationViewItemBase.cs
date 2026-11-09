using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000064 RID: 100
	public class NavigationViewItemBase : ContentControl, IControlProtected
	{
		// Token: 0x060004E7 RID: 1255 RVA: 0x00012BF4 File Offset: 0x00010DF4
		static NavigationViewItemBase()
		{
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(NavigationViewItemBase), new FrameworkPropertyMetadata(1));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(NavigationViewItemBase), new FrameworkPropertyMetadata(1));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(NavigationViewItemBase), new FrameworkPropertyMetadata(3));
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00012CB1 File Offset: 0x00010EB1
		internal NavigationViewItemBase()
		{
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00012CB9 File Offset: 0x00010EB9
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x00012CCB File Offset: 0x00010ECB
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItemBase.IsSelectedProperty);
			}
			set
			{
				base.SetValue(NavigationViewItemBase.IsSelectedProperty, value);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060004EB RID: 1259 RVA: 0x00012CE0 File Offset: 0x00010EE0
		// (remove) Token: 0x060004EC RID: 1260 RVA: 0x00012D18 File Offset: 0x00010F18
		internal event DependencyPropertyChangedCallback IsSelectedChanged;

		// Token: 0x060004ED RID: 1261 RVA: 0x00012D4D File Offset: 0x00010F4D
		private static void OnIsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NavigationViewItemBase)sender).OnIsSelectedPropertyChanged(args);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00012D5B File Offset: 0x00010F5B
		private void OnIsSelectedPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.OnPropertyChangedPrivate(args);
			DependencyPropertyChangedCallback isSelectedChanged = this.IsSelectedChanged;
			if (isSelectedChanged == null)
			{
				return;
			}
			isSelectedChanged(this, args.Property);
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00012D7C File Offset: 0x00010F7C
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x00012D8E File Offset: 0x00010F8E
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(NavigationViewItemBase.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(NavigationViewItemBase.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00012DA1 File Offset: 0x00010FA1
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x00012DA9 File Offset: 0x00010FA9
		internal NavigationViewRepeaterPosition Position
		{
			get
			{
				return this.m_position;
			}
			set
			{
				if (this.m_position != value)
				{
					this.m_position = value;
					this.OnNavigationViewItemBasePositionChanged();
				}
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00012DC1 File Offset: 0x00010FC1
		private protected virtual void OnNavigationViewItemBasePositionChanged()
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00012DC4 File Offset: 0x00010FC4
		internal NavigationView GetNavigationView()
		{
			NavigationView result;
			if (this.m_navigationView != null && this.m_navigationView.TryGetTarget(ref result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00012DEB File Offset: 0x00010FEB
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x00012DF3 File Offset: 0x00010FF3
		internal int Depth
		{
			get
			{
				return this.m_depth;
			}
			set
			{
				if (this.m_depth != value)
				{
					this.m_depth = value;
					this.OnNavigationViewItemBaseDepthChanged();
				}
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00012E0B File Offset: 0x0001100B
		private protected virtual void OnNavigationViewItemBaseDepthChanged()
		{
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00012E0D File Offset: 0x0001100D
		private protected virtual void OnNavigationViewItemBaseIsSelectedChanged()
		{
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00012E10 File Offset: 0x00011010
		internal SplitView GetSplitView()
		{
			SplitView result = null;
			NavigationView navigationView = this.GetNavigationView();
			if (navigationView != null)
			{
				result = navigationView.GetSplitView();
			}
			return result;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00012E31 File Offset: 0x00011031
		internal void SetNavigationViewParent(NavigationView navigationView)
		{
			this.m_navigationView = new WeakReference<NavigationView>(navigationView);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00012E3F File Offset: 0x0001103F
		private void OnPropertyChangedPrivate(DependencyPropertyChangedEventArgs args)
		{
			if (args.Property == NavigationViewItemBase.IsSelectedProperty)
			{
				this.OnNavigationViewItemBaseIsSelectedChanged();
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00012E55 File Offset: 0x00011055
		DependencyObject IControlProtected.GetTemplateChild(string childName)
		{
			return base.GetTemplateChild(childName);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00012E5E File Offset: 0x0001105E
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x00012E66 File Offset: 0x00011066
		internal bool IsTopLevelItem
		{
			get
			{
				return this.m_isTopLevelItem;
			}
			set
			{
				this.m_isTopLevelItem = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00012E6F File Offset: 0x0001106F
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00012E77 File Offset: 0x00011077
		internal bool CreatedByNavigationViewItemsFactory
		{
			get
			{
				return this.m_createdByNavigationViewItemsFactory;
			}
			set
			{
				this.m_createdByNavigationViewItemsFactory = value;
			}
		}

		// Token: 0x04000250 RID: 592
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(NavigationViewItemBase), new PropertyMetadata(false, new PropertyChangedCallback(NavigationViewItemBase.OnIsSelectedPropertyChanged)));

		// Token: 0x04000252 RID: 594
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(NavigationViewItemBase));

		// Token: 0x04000253 RID: 595
		internal const int c_itemIndentation = 25;

		// Token: 0x04000254 RID: 596
		private protected WeakReference<NavigationView> m_navigationView;

		// Token: 0x04000255 RID: 597
		private NavigationViewRepeaterPosition m_position;

		// Token: 0x04000256 RID: 598
		private int m_depth;

		// Token: 0x04000257 RID: 599
		private bool m_isTopLevelItem;

		// Token: 0x04000258 RID: 600
		private bool m_createdByNavigationViewItemsFactory;
	}
}
