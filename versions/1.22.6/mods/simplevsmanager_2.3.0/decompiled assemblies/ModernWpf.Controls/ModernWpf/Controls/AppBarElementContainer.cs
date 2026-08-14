using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x0200001C RID: 28
	public class AppBarElementContainer : ContentControl, ICommandBarElement
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00004EE8 File Offset: 0x000030E8
		static AppBarElementContainer()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarElementContainer), new FrameworkPropertyMetadata(typeof(AppBarElementContainer)));
			ToolBar.OverflowModeProperty.OverrideMetadata(typeof(AppBarElementContainer), new FrameworkPropertyMetadata(new PropertyChangedCallback(AppBarElementContainer.OnOverflowModePropertyChanged)));
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004F77 File Offset: 0x00003177
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00004F89 File Offset: 0x00003189
		public bool IsCompact
		{
			get
			{
				return (bool)base.GetValue(AppBarElementContainer.IsCompactProperty);
			}
			set
			{
				base.SetValue(AppBarElementContainer.IsCompactProperty, value);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00004F9C File Offset: 0x0000319C
		public bool IsInOverflow
		{
			get
			{
				return (bool)base.GetValue(AppBarElementContainer.IsInOverflowProperty);
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004FAE File Offset: 0x000031AE
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == ToolBar.IsOverflowItemProperty)
			{
				AppBarElementProperties.UpdateIsInOverflow(this);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004FCB File Offset: 0x000031CB
		private static void OnOverflowModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarElementProperties.UpdateIsInOverflow(d);
		}

		// Token: 0x0400004D RID: 77
		public static readonly DependencyProperty IsCompactProperty = AppBarElementProperties.IsCompactProperty.AddOwner(typeof(AppBarElementContainer));

		// Token: 0x0400004E RID: 78
		public static readonly DependencyProperty IsInOverflowProperty = AppBarElementProperties.IsInOverflowProperty.AddOwner(typeof(AppBarElementContainer));
	}
}
