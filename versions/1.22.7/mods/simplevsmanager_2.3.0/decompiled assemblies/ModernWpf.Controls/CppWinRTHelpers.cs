using System;
using System.Windows;
using ModernWpf.Controls;

// Token: 0x02000005 RID: 5
internal static class CppWinRTHelpers
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002090 File Offset: 0x00000290
	public static WinRTReturn GetTemplateChildT<WinRTReturn>(string childName, IControlProtected controlProtected) where WinRTReturn : DependencyObject
	{
		DependencyObject templateChild = controlProtected.GetTemplateChild(childName);
		if (templateChild != null)
		{
			return templateChild as WinRTReturn;
		}
		return default(WinRTReturn);
	}
}
