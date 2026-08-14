using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000286 RID: 646
	public static class ControlHelper
	{
		// Token: 0x060012E0 RID: 4832 RVA: 0x00037593 File Offset: 0x00035793
		public static CornerRadius GetCornerRadius(Control control)
		{
			return (CornerRadius)control.GetValue(ControlHelper.CornerRadiusProperty);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000375A5 File Offset: 0x000357A5
		public static void SetCornerRadius(Control control, CornerRadius value)
		{
			control.SetValue(ControlHelper.CornerRadiusProperty, value);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x000375B8 File Offset: 0x000357B8
		public static object GetHeader(Control control)
		{
			return control.GetValue(ControlHelper.HeaderProperty);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x000375C5 File Offset: 0x000357C5
		public static void SetHeader(Control control, object value)
		{
			control.SetValue(ControlHelper.HeaderProperty, value);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x000375D3 File Offset: 0x000357D3
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ControlHelper.UpdateHeaderVisibility((Control)d);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x000375E0 File Offset: 0x000357E0
		public static DataTemplate GetHeaderTemplate(Control control)
		{
			return (DataTemplate)control.GetValue(ControlHelper.HeaderTemplateProperty);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x000375F2 File Offset: 0x000357F2
		public static void SetHeaderTemplate(Control control, DataTemplate value)
		{
			control.SetValue(ControlHelper.HeaderTemplateProperty, value);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00037600 File Offset: 0x00035800
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ControlHelper.UpdateHeaderVisibility((Control)d);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0003760D File Offset: 0x0003580D
		public static Visibility GetHeaderVisibility(Control control)
		{
			return (Visibility)control.GetValue(ControlHelper.HeaderVisibilityProperty);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0003761F File Offset: 0x0003581F
		private static void SetHeaderVisibility(Control control, Visibility value)
		{
			control.SetValue(ControlHelper.HeaderVisibilityPropertyKey, value);
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00037634 File Offset: 0x00035834
		private static void UpdateHeaderVisibility(Control control)
		{
			Visibility value;
			if (ControlHelper.GetHeaderTemplate(control) != null)
			{
				value = 0;
			}
			else
			{
				value = (ControlHelper.IsNullOrEmptyString(ControlHelper.GetHeader(control)) ? 2 : 0);
			}
			ControlHelper.SetHeaderVisibility(control, value);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00037666 File Offset: 0x00035866
		public static string GetPlaceholderText(Control control)
		{
			return (string)control.GetValue(ControlHelper.PlaceholderTextProperty);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00037678 File Offset: 0x00035878
		public static void SetPlaceholderText(Control control, string value)
		{
			control.SetValue(ControlHelper.PlaceholderTextProperty, value);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00037686 File Offset: 0x00035886
		private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ControlHelper.UpdatePlaceholderTextVisibility((Control)d);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00037693 File Offset: 0x00035893
		public static Visibility GetPlaceholderTextVisibility(Control control)
		{
			return (Visibility)control.GetValue(ControlHelper.PlaceholderTextVisibilityProperty);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x000376A5 File Offset: 0x000358A5
		private static void SetPlaceholderTextVisibility(Control control, Visibility value)
		{
			control.SetValue(ControlHelper.PlaceholderTextVisibilityPropertyKey, value);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x000376B8 File Offset: 0x000358B8
		private static void UpdatePlaceholderTextVisibility(Control control)
		{
			ControlHelper.SetPlaceholderTextVisibility(control, string.IsNullOrEmpty(ControlHelper.GetPlaceholderText(control)) ? 2 : 0);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x000376D1 File Offset: 0x000358D1
		public static Brush GetPlaceholderForeground(Control control)
		{
			return (Brush)control.GetValue(ControlHelper.PlaceholderForegroundProperty);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000376E3 File Offset: 0x000358E3
		public static void SetPlaceholderForeground(Control control, Brush value)
		{
			control.SetValue(ControlHelper.PlaceholderForegroundProperty, value);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000376F1 File Offset: 0x000358F1
		public static object GetDescription(Control control)
		{
			return control.GetValue(ControlHelper.DescriptionProperty);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x000376FE File Offset: 0x000358FE
		public static void SetDescription(Control control, object value)
		{
			control.SetValue(ControlHelper.DescriptionProperty, value);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0003770C File Offset: 0x0003590C
		private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ControlHelper.UpdateDescriptionVisibility((Control)d);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00037719 File Offset: 0x00035919
		public static Visibility GetDescriptionVisibility(Control control)
		{
			return (Visibility)control.GetValue(ControlHelper.DescriptionVisibilityProperty);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0003772B File Offset: 0x0003592B
		private static void SetDescriptionVisibility(Control control, Visibility value)
		{
			control.SetValue(ControlHelper.DescriptionVisibilityPropertyKey, value);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0003773E File Offset: 0x0003593E
		private static void UpdateDescriptionVisibility(Control control)
		{
			ControlHelper.SetDescriptionVisibility(control, ControlHelper.IsNullOrEmptyString(ControlHelper.GetDescription(control)) ? 2 : 0);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00037758 File Offset: 0x00035958
		internal static bool IsNullOrEmptyString(object obj)
		{
			if (obj != null)
			{
				string text = obj as string;
				return text != null && string.IsNullOrEmpty(text);
			}
			return true;
		}

		// Token: 0x0400062E RID: 1582
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ControlHelper), null);

		// Token: 0x0400062F RID: 1583
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.RegisterAttached("Header", typeof(object), typeof(ControlHelper), new FrameworkPropertyMetadata(new PropertyChangedCallback(ControlHelper.OnHeaderChanged)));

		// Token: 0x04000630 RID: 1584
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(ControlHelper), new FrameworkPropertyMetadata(new PropertyChangedCallback(ControlHelper.OnHeaderTemplateChanged)));

		// Token: 0x04000631 RID: 1585
		private static readonly DependencyPropertyKey HeaderVisibilityPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HeaderVisibility", typeof(Visibility), typeof(ControlHelper), new FrameworkPropertyMetadata(2));

		// Token: 0x04000632 RID: 1586
		public static readonly DependencyProperty HeaderVisibilityProperty = ControlHelper.HeaderVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04000633 RID: 1587
		public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.RegisterAttached("PlaceholderText", typeof(string), typeof(ControlHelper), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(ControlHelper.OnPlaceholderTextChanged)));

		// Token: 0x04000634 RID: 1588
		private static readonly DependencyPropertyKey PlaceholderTextVisibilityPropertyKey = DependencyProperty.RegisterAttachedReadOnly("PlaceholderTextVisibility", typeof(Visibility), typeof(ControlHelper), new FrameworkPropertyMetadata(2));

		// Token: 0x04000635 RID: 1589
		public static readonly DependencyProperty PlaceholderTextVisibilityProperty = ControlHelper.PlaceholderTextVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04000636 RID: 1590
		public static readonly DependencyProperty PlaceholderForegroundProperty = DependencyProperty.RegisterAttached("PlaceholderForeground", typeof(Brush), typeof(ControlHelper), null);

		// Token: 0x04000637 RID: 1591
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.RegisterAttached("Description", typeof(object), typeof(ControlHelper), new FrameworkPropertyMetadata(new PropertyChangedCallback(ControlHelper.OnDescriptionChanged)));

		// Token: 0x04000638 RID: 1592
		private static readonly DependencyPropertyKey DescriptionVisibilityPropertyKey = DependencyProperty.RegisterAttachedReadOnly("DescriptionVisibility", typeof(Visibility), typeof(ControlHelper), new FrameworkPropertyMetadata(2));

		// Token: 0x04000639 RID: 1593
		public static readonly DependencyProperty DescriptionVisibilityProperty = ControlHelper.DescriptionVisibilityPropertyKey.DependencyProperty;
	}
}
