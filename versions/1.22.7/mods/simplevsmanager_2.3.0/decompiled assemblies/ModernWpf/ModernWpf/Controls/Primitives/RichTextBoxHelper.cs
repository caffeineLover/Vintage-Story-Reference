using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200029E RID: 670
	public static class RichTextBoxHelper
	{
		// Token: 0x06001419 RID: 5145 RVA: 0x0003BB20 File Offset: 0x00039D20
		public static bool GetIsEnabled(RichTextBox richTextBox)
		{
			return (bool)richTextBox.GetValue(RichTextBoxHelper.IsEnabledProperty);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0003BB32 File Offset: 0x00039D32
		public static void SetIsEnabled(RichTextBox richTextBox, bool value)
		{
			richTextBox.SetValue(RichTextBoxHelper.IsEnabledProperty, value);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0003BB48 File Offset: 0x00039D48
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)d;
			bool flag = (bool)e.OldValue;
			if ((bool)e.NewValue)
			{
				richTextBox.TextChanged += new TextChangedEventHandler(RichTextBoxHelper.OnTextChanged);
				RichTextBoxHelper.UpdateIsEmpty(richTextBox);
				return;
			}
			richTextBox.TextChanged -= new TextChangedEventHandler(RichTextBoxHelper.OnTextChanged);
			richTextBox.ClearValue(RichTextBoxHelper.IsEmptyPropertyKey);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0003BBAD File Offset: 0x00039DAD
		public static bool GetIsEmpty(RichTextBox richTextBox)
		{
			return (bool)richTextBox.GetValue(RichTextBoxHelper.IsEmptyProperty);
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0003BBBF File Offset: 0x00039DBF
		private static void SetIsEmpty(RichTextBox richTextBox, bool value)
		{
			richTextBox.SetValue(RichTextBoxHelper.IsEmptyPropertyKey, value);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0003BBD4 File Offset: 0x00039DD4
		private static void UpdateIsEmpty(RichTextBox rtb)
		{
			bool flag;
			if (rtb.Document.Blocks.Count == 0)
			{
				flag = true;
			}
			else
			{
				TextPointer nextInsertionPosition = rtb.Document.ContentStart.GetNextInsertionPosition(1);
				TextPointer nextInsertionPosition2 = rtb.Document.ContentEnd.GetNextInsertionPosition(0);
				flag = (nextInsertionPosition.CompareTo(nextInsertionPosition2) == 0);
			}
			if (RichTextBoxHelper.GetIsEmpty(rtb) != flag)
			{
				RichTextBoxHelper.SetIsEmpty(rtb, flag);
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0003BC34 File Offset: 0x00039E34
		private static void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			RichTextBoxHelper.UpdateIsEmpty((RichTextBox)sender);
		}

		// Token: 0x040006B9 RID: 1721
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(RichTextBoxHelper), new PropertyMetadata(false, new PropertyChangedCallback(RichTextBoxHelper.OnIsEnabledChanged)));

		// Token: 0x040006BA RID: 1722
		private static readonly DependencyPropertyKey IsEmptyPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsEmpty", typeof(bool), typeof(RichTextBoxHelper), new PropertyMetadata(false));

		// Token: 0x040006BB RID: 1723
		public static readonly DependencyProperty IsEmptyProperty = RichTextBoxHelper.IsEmptyPropertyKey.DependencyProperty;
	}
}
