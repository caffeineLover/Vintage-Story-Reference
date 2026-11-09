using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using VintageStoryModManager.Views.Dialogs;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000076 RID: 118
	[NullableContext(2)]
	[Nullable(0)]
	public static class ModManagerMessageBox
	{
		// Token: 0x06000AD8 RID: 2776 RVA: 0x000444D3 File Offset: 0x000426D3
		public static MessageBoxResult Show([Nullable(1)] string messageBoxText, [Nullable(1)] string caption, MessageBoxButton button, MessageBoxImage icon, MessageDialogExtraButton extraButton = null, MessageDialogButtonContentOverrides buttonContentOverrides = null, string fixedFooterText = null)
		{
			return ModManagerMessageBox.ShowInternal(null, messageBoxText, caption, button, icon, extraButton, buttonContentOverrides, fixedFooterText);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x000444E5 File Offset: 0x000426E5
		[NullableContext(1)]
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, [Nullable(2)] MessageDialogExtraButton extraButton = null, [Nullable(2)] MessageDialogButtonContentOverrides buttonContentOverrides = null, [Nullable(2)] string fixedFooterText = null)
		{
			return ModManagerMessageBox.ShowInternal(owner, messageBoxText, caption, button, icon, extraButton, buttonContentOverrides, fixedFooterText);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x000444F8 File Offset: 0x000426F8
		private static MessageBoxResult ShowInternal(Window owner, [Nullable(1)] string messageBoxText, [Nullable(1)] string caption, MessageBoxButton button, MessageBoxImage icon, MessageDialogExtraButton extraButton, MessageDialogButtonContentOverrides buttonContentOverrides, string fixedFooterText)
		{
			Window window = owner ?? ModManagerMessageBox.GetActiveWindow();
			bool flag = window != null && window.IsVisible;
			MessageDialogWindow messageDialogWindow = new MessageDialogWindow();
			messageDialogWindow.Owner = (flag ? window : null);
			messageDialogWindow.WindowStartupLocation = (flag ? 2 : 1);
			messageDialogWindow.Topmost = (window == null || !flag);
			messageDialogWindow.Initialize(messageBoxText, caption, button, icon, extraButton, buttonContentOverrides, fixedFooterText);
			messageDialogWindow.ShowDialog();
			return messageDialogWindow.Result;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0004456C File Offset: 0x0004276C
		private static Window GetActiveWindow()
		{
			if (Application.Current == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<Window>(Enumerable.OfType<Window>(Application.Current.Windows), (Window window) => window.IsActive) ?? Application.Current.MainWindow;
		}
	}
}
