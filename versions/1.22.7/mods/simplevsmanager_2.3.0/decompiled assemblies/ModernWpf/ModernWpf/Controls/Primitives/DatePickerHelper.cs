using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200028E RID: 654
	public static class DatePickerHelper
	{
		// Token: 0x06001344 RID: 4932 RVA: 0x00038977 File Offset: 0x00036B77
		public static bool GetIsEnabled(DatePicker datePicker)
		{
			return (bool)datePicker.GetValue(DatePickerHelper.IsEnabledProperty);
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00038989 File Offset: 0x00036B89
		public static void SetIsEnabled(DatePicker datePicker, bool value)
		{
			datePicker.SetValue(DatePickerHelper.IsEnabledProperty, value);
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0003899C File Offset: 0x00036B9C
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = (DatePicker)d;
			if ((bool)e.NewValue)
			{
				datePicker.Loaded += new RoutedEventHandler(DatePickerHelper.OnLoaded);
				return;
			}
			datePicker.Loaded -= new RoutedEventHandler(DatePickerHelper.OnLoaded);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x000389E4 File Offset: 0x00036BE4
		private static void OnLoaded(object sender, RoutedEventArgs e)
		{
			DatePicker datePicker = (DatePicker)sender;
			datePicker.Loaded -= new RoutedEventHandler(DatePickerHelper.OnLoaded);
			DatePickerTextBox templateChild = datePicker.GetTemplateChild("PART_TextBox");
			if (templateChild != null)
			{
				ContentControl templateChild2 = templateChild.GetTemplateChild("PART_Watermark");
				if (templateChild2 != null)
				{
					Binding binding = new Binding
					{
						Path = new PropertyPath(ControlHelper.PlaceholderTextProperty),
						Source = datePicker
					};
					BindingExpression bindingExpression = templateChild2.GetBindingExpression(ContentControl.ContentProperty);
					BindingBase bindingBase;
					if (bindingExpression != null)
					{
						MultiBinding multiBinding = new MultiBinding();
						multiBinding.Bindings.Add(binding);
						multiBinding.Bindings.Add(bindingExpression.ParentBinding);
						multiBinding.Converter = DatePickerHelper._watermarkConverter;
						bindingBase = multiBinding;
					}
					else
					{
						bindingBase = binding;
					}
					templateChild2.SetBinding(ContentControl.ContentProperty, bindingBase);
				}
			}
		}

		// Token: 0x04000671 RID: 1649
		private static readonly DatePickerHelper.FirstNotNullOrEmptyConverter _watermarkConverter = new DatePickerHelper.FirstNotNullOrEmptyConverter();

		// Token: 0x04000672 RID: 1650
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DatePickerHelper), new PropertyMetadata(new PropertyChangedCallback(DatePickerHelper.OnIsEnabledChanged)));

		// Token: 0x02000416 RID: 1046
		private class FirstNotNullOrEmptyConverter : IMultiValueConverter
		{
			// Token: 0x06001BFD RID: 7165 RVA: 0x0004AFB8 File Offset: 0x000491B8
			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				foreach (object obj in values)
				{
					string text = obj as string;
					if (text != null)
					{
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
					else if (obj != null)
					{
						return obj;
					}
				}
				return null;
			}

			// Token: 0x06001BFE RID: 7166 RVA: 0x0004AFF3 File Offset: 0x000491F3
			public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}
	}
}
