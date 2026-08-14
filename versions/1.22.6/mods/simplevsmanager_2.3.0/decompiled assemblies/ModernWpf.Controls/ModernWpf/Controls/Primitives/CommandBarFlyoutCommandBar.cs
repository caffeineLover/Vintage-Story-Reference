using System;
using System.Windows;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E3 RID: 227
	[TemplatePart(Name = "PART_ToolBar", Type = typeof(CommandBarFlyoutToolBar))]
	public class CommandBarFlyoutCommandBar : CommandBar
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x00026911 File Offset: 0x00024B11
		static CommandBarFlyoutCommandBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBarFlyoutCommandBar), new FrameworkPropertyMetadata(typeof(CommandBarFlyoutCommandBar)));
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00026936 File Offset: 0x00024B36
		public CommandBarFlyoutCommandBar()
		{
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00026950 File Offset: 0x00024B50
		internal WeakReference<CommandBarFlyout> OwningFlyout
		{
			get
			{
				return this.m_owningFlyout;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00026958 File Offset: 0x00024B58
		internal void SetOwningFlyout(CommandBarFlyout owningFlyout)
		{
			this.m_owningFlyout = new WeakReference<CommandBarFlyout>(owningFlyout);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00026966 File Offset: 0x00024B66
		internal bool HasOpenAnimation()
		{
			return this.m_toolBar != null && this.m_toolBar.HasOpenAnimation();
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002697D File Offset: 0x00024B7D
		internal void PlayOpenAnimation()
		{
			CommandBarFlyoutToolBar toolBar = this.m_toolBar;
			if (toolBar == null)
			{
				return;
			}
			toolBar.PlayOpenAnimation();
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002698F File Offset: 0x00024B8F
		internal bool HasCloseAnimation()
		{
			return this.m_toolBar != null && this.m_toolBar.HasCloseAnimation();
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000269A6 File Offset: 0x00024BA6
		internal void PlayCloseAnimation(Action onCompleteFunc)
		{
			CommandBarFlyoutToolBar toolBar = this.m_toolBar;
			if (toolBar == null)
			{
				return;
			}
			toolBar.PlayCloseAnimation(onCompleteFunc);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000269B9 File Offset: 0x00024BB9
		internal void ClearShadow()
		{
			CommandBarFlyoutToolBar toolBar = this.m_toolBar;
			if (toolBar == null)
			{
				return;
			}
			toolBar.ClearShadow();
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000269CB File Offset: 0x00024BCB
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.m_toolBar = (base.GetTemplateChild("PART_ToolBar") as CommandBarFlyoutToolBar);
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000269E9 File Offset: 0x00024BE9
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				InputManager.Current.PostProcessInput += new ProcessInputEventHandler(this.OnPostProcessInput);
				return;
			}
			InputManager.Current.PostProcessInput -= new ProcessInputEventHandler(this.OnPostProcessInput);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00026A26 File Offset: 0x00024C26
		private void OnPostProcessInput(object sender, ProcessInputEventArgs e)
		{
			if (e.StagingItem.Input.RoutedEvent == Mouse.MouseUpEvent)
			{
				e.StagingItem.Input.Handled = true;
			}
		}

		// Token: 0x040004A5 RID: 1189
		private CommandBarFlyoutToolBar m_toolBar;

		// Token: 0x040004A6 RID: 1190
		private WeakReference<CommandBarFlyout> m_owningFlyout;
	}
}
