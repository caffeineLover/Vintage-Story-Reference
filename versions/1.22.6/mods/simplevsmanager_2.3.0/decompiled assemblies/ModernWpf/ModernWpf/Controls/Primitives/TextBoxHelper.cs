using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A8 RID: 680
	public static class TextBoxHelper
	{
		// Token: 0x06001459 RID: 5209 RVA: 0x0003C771 File Offset: 0x0003A971
		public static bool GetIsEnabled(TextBox textBox)
		{
			return (bool)textBox.GetValue(TextBoxHelper.IsEnabledProperty);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0003C783 File Offset: 0x0003A983
		public static void SetIsEnabled(TextBox textBox, bool value)
		{
			textBox.SetValue(TextBoxHelper.IsEnabledProperty, value);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0003C798 File Offset: 0x0003A998
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)d;
			if ((bool)e.NewValue)
			{
				textBox.Loaded += new RoutedEventHandler(TextBoxHelper.OnLoaded);
				textBox.TextChanged += new TextChangedEventHandler(TextBoxHelper.OnTextChanged);
				TextBoxHelper.UpdateHasText(textBox);
				return;
			}
			textBox.Loaded -= new RoutedEventHandler(TextBoxHelper.OnLoaded);
			textBox.TextChanged -= new TextChangedEventHandler(TextBoxHelper.OnTextChanged);
			textBox.ClearValue(TextBoxHelper.HasTextPropertyKey);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0003C814 File Offset: 0x0003AA14
		public static bool GetHasText(TextBox textBox)
		{
			return (bool)textBox.GetValue(TextBoxHelper.HasTextProperty);
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0003C826 File Offset: 0x0003AA26
		private static void SetHasText(TextBox textBox, bool value)
		{
			textBox.SetValue(TextBoxHelper.HasTextPropertyKey, value);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0003C839 File Offset: 0x0003AA39
		private static void UpdateHasText(TextBox textBox)
		{
			TextBoxHelper.SetHasText(textBox, !string.IsNullOrEmpty(textBox.Text));
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0003C84F File Offset: 0x0003AA4F
		public static bool GetIsDeleteButton(Button button)
		{
			return (bool)button.GetValue(TextBoxHelper.IsDeleteButtonProperty);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0003C861 File Offset: 0x0003AA61
		public static void SetIsDeleteButton(Button button, bool value)
		{
			button.SetValue(TextBoxHelper.IsDeleteButtonProperty, value);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0003C874 File Offset: 0x0003AA74
		private static void OnIsDeleteButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = (Button)d;
			if ((bool)e.OldValue)
			{
				button.Click -= new RoutedEventHandler(TextBoxHelper.OnDeleteButtonClick);
			}
			if ((bool)e.NewValue)
			{
				button.Click += new RoutedEventHandler(TextBoxHelper.OnDeleteButtonClick);
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0003C8C8 File Offset: 0x0003AAC8
		public static bool GetIsDeleteButtonVisible(TextBox textBox)
		{
			return (bool)textBox.GetValue(TextBoxHelper.IsDeleteButtonVisibleProperty);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0003C8DA File Offset: 0x0003AADA
		public static void SetIsDeleteButtonVisible(TextBox textBox, bool value)
		{
			textBox.SetValue(TextBoxHelper.IsDeleteButtonVisibleProperty, value);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0003C8ED File Offset: 0x0003AAED
		private static void OnIsDeleteButtonVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxHelper.UpdateVisualStates((TextBox)d, (bool)e.NewValue);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0003C906 File Offset: 0x0003AB06
		private static void OnLoaded(object sender, RoutedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			TextBoxHelper.UpdateVisualStates(textBox, TextBoxHelper.GetIsDeleteButtonVisible(textBox));
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0003C919 File Offset: 0x0003AB19
		private static void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			TextBoxHelper.UpdateHasText((TextBox)sender);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0003C928 File Offset: 0x0003AB28
		private static void OnDeleteButtonClick(object sender, RoutedEventArgs e)
		{
			TextBox textBox = ((Button)sender).TemplatedParent as TextBox;
			if (textBox != null)
			{
				textBox.SetCurrentValue(TextBox.TextProperty, null);
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0003C955 File Offset: 0x0003AB55
		private static void UpdateVisualStates(TextBox textBox, bool isDeleteButtonVisible)
		{
			VisualStateManager.GoToState(textBox, isDeleteButtonVisible ? "ButtonVisible" : "ButtonCollapsed", true);
		}

		// Token: 0x040006D1 RID: 1745
		private const string ButtonStatesGroup = "ButtonStates";

		// Token: 0x040006D2 RID: 1746
		private const string ButtonVisibleState = "ButtonVisible";

		// Token: 0x040006D3 RID: 1747
		private const string ButtonCollapsedState = "ButtonCollapsed";

		// Token: 0x040006D4 RID: 1748
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(new PropertyChangedCallback(TextBoxHelper.OnIsEnabledChanged)));

		// Token: 0x040006D5 RID: 1749
		private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasText", typeof(bool), typeof(TextBoxHelper), null);

		// Token: 0x040006D6 RID: 1750
		public static readonly DependencyProperty HasTextProperty = TextBoxHelper.HasTextPropertyKey.DependencyProperty;

		// Token: 0x040006D7 RID: 1751
		public static readonly DependencyProperty IsDeleteButtonProperty = DependencyProperty.RegisterAttached("IsDeleteButton", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(new PropertyChangedCallback(TextBoxHelper.OnIsDeleteButtonChanged)));

		// Token: 0x040006D8 RID: 1752
		public static readonly DependencyProperty IsDeleteButtonVisibleProperty = DependencyProperty.RegisterAttached("IsDeleteButtonVisible", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(new PropertyChangedCallback(TextBoxHelper.OnIsDeleteButtonVisibleChanged)));
	}
}
