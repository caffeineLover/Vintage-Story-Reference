using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x02000020 RID: 32
	public class AppBarSeparator : Control, ICommandBarElement, IAppBarElement
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00005524 File Offset: 0x00003724
		static AppBarSeparator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarSeparator), new FrameworkPropertyMetadata(typeof(AppBarSeparator)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(AppBarSeparator), new FrameworkPropertyMetadata(false));
			ToolBar.OverflowModeProperty.OverrideMetadata(typeof(AppBarSeparator), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarSeparator.OnOverflowModePropertyChanged)));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000055E3 File Offset: 0x000037E3
		public AppBarSeparator()
		{
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000055FD File Offset: 0x000037FD
		// (set) Token: 0x06000144 RID: 324 RVA: 0x0000560F File Offset: 0x0000380F
		public bool IsCompact
		{
			get
			{
				return (bool)base.GetValue(AppBarSeparator.IsCompactProperty);
			}
			set
			{
				base.SetValue(AppBarSeparator.IsCompactProperty, value);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00005622 File Offset: 0x00003822
		public bool IsInOverflow
		{
			get
			{
				return (bool)base.GetValue(AppBarSeparator.IsInOverflowProperty);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00005634 File Offset: 0x00003834
		private AppBarElementApplicationViewState ApplicationViewState
		{
			get
			{
				return (AppBarElementApplicationViewState)base.GetValue(AppBarSeparator.ApplicationViewStateProperty);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005648 File Offset: 0x00003848
		private void UpdateApplicationViewState()
		{
			AppBarElementApplicationViewState appBarElementApplicationViewState;
			if (this.IsInOverflow && base.IsVisible)
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.Overflow;
			}
			else if (this.IsCompact)
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.Compact;
			}
			else
			{
				appBarElementApplicationViewState = AppBarElementApplicationViewState.FullSize;
			}
			base.SetValue(AppBarElementProperties.ApplicationViewStatePropertyKey, appBarElementApplicationViewState);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005688 File Offset: 0x00003888
		void IAppBarElement.UpdateApplicationViewState()
		{
			this.UpdateApplicationViewState();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005690 File Offset: 0x00003890
		void IAppBarElement.ApplyApplicationViewState()
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005699 File Offset: 0x00003899
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateVisualState(false);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000056A8 File Offset: 0x000038A8
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == ToolBar.IsOverflowItemProperty)
			{
				AppBarElementProperties.UpdateIsInOverflow(this);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000056C5 File Offset: 0x000038C5
		private static void OnOverflowModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarElementProperties.UpdateIsInOverflow(d);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000056CD File Offset: 0x000038CD
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateApplicationViewState();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000056D8 File Offset: 0x000038D8
		private void UpdateVisualState(bool useTransitions = true)
		{
			VisualStateManager.GoToState(this, this.ApplicationViewState.ToString(), useTransitions);
		}

		// Token: 0x0400005C RID: 92
		public static readonly DependencyProperty IsCompactProperty = AppBarElementProperties.IsCompactProperty.AddOwner(typeof(AppBarSeparator));

		// Token: 0x0400005D RID: 93
		public static readonly DependencyProperty IsInOverflowProperty = AppBarElementProperties.IsInOverflowProperty.AddOwner(typeof(AppBarSeparator));

		// Token: 0x0400005E RID: 94
		private static readonly DependencyProperty ApplicationViewStateProperty = AppBarElementProperties.ApplicationViewStateProperty.AddOwner(typeof(AppBarSeparator));
	}
}
