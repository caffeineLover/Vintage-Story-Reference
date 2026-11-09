using System;
using System.Windows;
using System.Windows.Input;

namespace ModernWpf.Input
{
	// Token: 0x02000259 RID: 601
	internal static class FocusManagerEx
	{
		// Token: 0x06001133 RID: 4403 RVA: 0x00032C44 File Offset: 0x00030E44
		public static UIElement FindNextFocusableElement(FocusNavigationDirection focusNavigationDirection)
		{
			UIElement uielement = Keyboard.FocusedElement as UIElement;
			if (uielement != null)
			{
				return uielement.PredictFocus(focusNavigationDirection) as UIElement;
			}
			return null;
		}
	}
}
