using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E8 RID: 232
	public class CommandBarOverflowPresenter : ContentControl
	{
		// Token: 0x06000BDC RID: 3036 RVA: 0x00028AE6 File Offset: 0x00026CE6
		static CommandBarOverflowPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBarOverflowPresenter), new FrameworkPropertyMetadata(typeof(CommandBarOverflowPresenter)));
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00028B24 File Offset: 0x00026D24
		public CommandBarOverflowPresenter()
		{
			base.Loaded += new RoutedEventHandler(this.OnLoaded);
			base.Unloaded += new RoutedEventHandler(this.OnUnloaded);
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00028B50 File Offset: 0x00026D50
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x00028B62 File Offset: 0x00026D62
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(CommandBarOverflowPresenter.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(CommandBarOverflowPresenter.CornerRadiusProperty, value);
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00028B75 File Offset: 0x00026D75
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateVisualState(false);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00028B84 File Offset: 0x00026D84
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (base.IsVisible)
			{
				this.UpdateVisualState(true);
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00028B95 File Offset: 0x00026D95
		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			this.UpdateVisualState(false);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00028BA0 File Offset: 0x00026DA0
		private void UpdateVisualState(bool useTransitions)
		{
			string text = "DisplayModeDefault";
			VisualStateManager.GoToState(this, text, useTransitions);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00028BBC File Offset: 0x00026DBC
		private bool IsPopupOpenDown()
		{
			CommandBarToolBar commandBarToolBar = base.TemplatedParent as CommandBarToolBar;
			return commandBarToolBar == null || base.TranslatePoint(new Point(0.0, 0.0), commandBarToolBar).Y > 0.0;
		}

		// Token: 0x040004E2 RID: 1250
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(CommandBarOverflowPresenter));
	}
}
