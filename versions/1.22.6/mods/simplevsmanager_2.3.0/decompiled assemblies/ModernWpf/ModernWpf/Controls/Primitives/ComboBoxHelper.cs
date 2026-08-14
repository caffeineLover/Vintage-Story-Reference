using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000285 RID: 645
	public sealed class ComboBoxHelper
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x000372C3 File Offset: 0x000354C3
		internal ComboBoxHelper()
		{
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x000372CB File Offset: 0x000354CB
		public static Style GetTextBoxStyle(ComboBox comboBox)
		{
			return (Style)comboBox.GetValue(ComboBoxHelper.TextBoxStyleProperty);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x000372DD File Offset: 0x000354DD
		public static void SetTextBoxStyle(ComboBox comboBox, Style value)
		{
			comboBox.SetValue(ComboBoxHelper.TextBoxStyleProperty, value);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x000372EB File Offset: 0x000354EB
		public static bool GetKeepInteriorCornersSquare(ComboBox comboBox)
		{
			return (bool)comboBox.GetValue(ComboBoxHelper.KeepInteriorCornersSquareProperty);
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000372FD File Offset: 0x000354FD
		public static void SetKeepInteriorCornersSquare(ComboBox comboBox, bool value)
		{
			comboBox.SetValue(ComboBoxHelper.KeepInteriorCornersSquareProperty, value);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00037310 File Offset: 0x00035510
		private static void OnKeepInteriorCornersSquareChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			ComboBox comboBox = sender as ComboBox;
			if (comboBox != null)
			{
				if ((bool)args.NewValue)
				{
					comboBox.DropDownOpened += new EventHandler(ComboBoxHelper.OnDropDownOpened);
					comboBox.DropDownClosed += new EventHandler(ComboBoxHelper.OnDropDownClosed);
					return;
				}
				comboBox.DropDownOpened -= new EventHandler(ComboBoxHelper.OnDropDownOpened);
				comboBox.DropDownClosed -= new EventHandler(ComboBoxHelper.OnDropDownClosed);
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00037380 File Offset: 0x00035580
		private static void OnDropDownOpened(object sender, object args)
		{
			ComboBox comboBox = (ComboBox)sender;
			comboBox.Dispatcher.BeginInvoke(delegate()
			{
				ComboBoxHelper.UpdateCornerRadius(comboBox, true);
			}, Array.Empty<object>());
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x000373C1 File Offset: 0x000355C1
		private static void OnDropDownClosed(object sender, object args)
		{
			ComboBoxHelper.UpdateCornerRadius((ComboBox)sender, false);
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x000373D0 File Offset: 0x000355D0
		private static void UpdateCornerRadius(ComboBox comboBox, bool isDropDownOpen)
		{
			CornerRadius cornerRadius = ControlHelper.GetCornerRadius(comboBox);
			CornerRadius cornerRadius2 = (CornerRadius)ComboBoxHelper.ResourceLookup(comboBox, "OverlayCornerRadius");
			if (isDropDownOpen)
			{
				bool flag = ComboBoxHelper.IsPopupOpenDown(comboBox);
				CornerRadiusFilterConverter cornerRadiusFilterConverter = new CornerRadiusFilterConverter();
				CornerRadiusFilterKind filterKind = flag ? CornerRadiusFilterKind.Bottom : CornerRadiusFilterKind.Top;
				cornerRadius2 = cornerRadiusFilterConverter.Convert(cornerRadius2, filterKind);
				CornerRadiusFilterKind filterKind2 = flag ? CornerRadiusFilterKind.Top : CornerRadiusFilterKind.Bottom;
				cornerRadius = cornerRadiusFilterConverter.Convert(cornerRadius, filterKind2);
			}
			Border templateChild = ComboBoxHelper.GetTemplateChild<Border>("PopupBorder", comboBox);
			if (templateChild != null)
			{
				templateChild.CornerRadius = cornerRadius2;
			}
			if (comboBox.IsEditable)
			{
				TextBox templateChild2 = ComboBoxHelper.GetTemplateChild<TextBox>("PART_EditableTextBox", comboBox);
				if (templateChild2 != null)
				{
					ControlHelper.SetCornerRadius(templateChild2, cornerRadius);
					return;
				}
			}
			else
			{
				Border templateChild3 = ComboBoxHelper.GetTemplateChild<Border>("Background", comboBox);
				if (templateChild3 != null)
				{
					templateChild3.CornerRadius = cornerRadius;
				}
				Border templateChild4 = ComboBoxHelper.GetTemplateChild<Border>("HighlightBackground", comboBox);
				if (templateChild4 != null)
				{
					templateChild4.CornerRadius = cornerRadius;
				}
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00037494 File Offset: 0x00035694
		private static bool IsPopupOpenDown(ComboBox comboBox)
		{
			double num = 0.0;
			Border templateChild = ComboBoxHelper.GetTemplateChild<Border>("PopupBorder", comboBox);
			if (templateChild != null)
			{
				TextBox templateChild2 = ComboBoxHelper.GetTemplateChild<TextBox>("PART_EditableTextBox", comboBox);
				if (templateChild2 != null)
				{
					num = templateChild.TranslatePoint(new Point(0.0, 0.0), templateChild2).Y;
				}
			}
			return num > 0.0;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000374FC File Offset: 0x000356FC
		private static object ResourceLookup(Control control, object key)
		{
			return control.TryFindResource(key);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00037505 File Offset: 0x00035705
		private static T GetTemplateChild<T>(string childName, Control control) where T : DependencyObject
		{
			ControlTemplate template = control.Template;
			return ((template != null) ? template.FindName(childName, control) : null) as T;
		}

		// Token: 0x04000627 RID: 1575
		private const string c_popupBorderName = "PopupBorder";

		// Token: 0x04000628 RID: 1576
		private const string c_editableTextName = "PART_EditableTextBox";

		// Token: 0x04000629 RID: 1577
		private const string c_backgroundName = "Background";

		// Token: 0x0400062A RID: 1578
		private const string c_highlightBackgroundName = "HighlightBackground";

		// Token: 0x0400062B RID: 1579
		private const string c_overlayCornerRadiusKey = "OverlayCornerRadius";

		// Token: 0x0400062C RID: 1580
		public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.RegisterAttached("TextBoxStyle", typeof(Style), typeof(ComboBoxHelper), null);

		// Token: 0x0400062D RID: 1581
		public static readonly DependencyProperty KeepInteriorCornersSquareProperty = DependencyProperty.RegisterAttached("KeepInteriorCornersSquare", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(ComboBoxHelper.OnKeepInteriorCornersSquareChanged)));
	}
}
