using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000294 RID: 660
	public static class MultiSelectHelper
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x00039AE8 File Offset: 0x00037CE8
		public static SelectionMode GetSelectionMode(ListBoxItem container)
		{
			return (SelectionMode)container.GetValue(MultiSelectHelper.SelectionModeProperty);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00039AFA File Offset: 0x00037CFA
		public static void SetSelectionMode(ListBoxItem container, SelectionMode value)
		{
			container.SetValue(MultiSelectHelper.SelectionModeProperty, value);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00039B10 File Offset: 0x00037D10
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBoxItem listBoxItem = (ListBoxItem)d;
			MultiSelectHelper.UpdateVisualState(listBoxItem, (SelectionMode)e.NewValue, listBoxItem.IsVisible);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00039B3C File Offset: 0x00037D3C
		private static void UpdateVisualState(ListBoxItem control, SelectionMode selectionMode, bool useTransitions)
		{
			VisualStateManager.GoToState(control, (selectionMode == 1) ? "MultiSelectEnabled" : "MultiSelectDisabled", useTransitions);
		}

		// Token: 0x0400068D RID: 1677
		private const string MultiSelectStatesGroup = "MultiSelectStates";

		// Token: 0x0400068E RID: 1678
		private const string MultiSelectDisabledState = "MultiSelectDisabled";

		// Token: 0x0400068F RID: 1679
		private const string MultiSelectEnabledState = "MultiSelectEnabled";

		// Token: 0x04000690 RID: 1680
		public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(SelectionMode), typeof(MultiSelectHelper), new PropertyMetadata(0, new PropertyChangedCallback(MultiSelectHelper.OnSelectionModeChanged)));
	}
}
