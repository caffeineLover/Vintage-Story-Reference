using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x0200021B RID: 539
	internal static class MenuDropAlignmentHelper
	{
		// Token: 0x06000FB8 RID: 4024 RVA: 0x0002CDAC File Offset: 0x0002AFAC
		static MenuDropAlignmentHelper()
		{
			try
			{
				MenuDropAlignmentHelper._menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", 40);
			}
			catch (Exception)
			{
			}
			if (MenuDropAlignmentHelper._menuDropAlignmentField != null)
			{
				MenuDropAlignmentHelper.EnsureStandardPopupAlignment();
				SystemParameters.StaticPropertyChanged += new PropertyChangedEventHandler(MenuDropAlignmentHelper.SystemParameters_StaticPropertyChanged);
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002CE0C File Offset: 0x0002B00C
		public static void EnsureStandardPopupAlignment()
		{
			if (SystemParameters.MenuDropAlignment)
			{
				try
				{
					MenuDropAlignmentHelper._menuDropAlignmentField.SetValue(null, false);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0002CE48 File Offset: 0x0002B048
		private static void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "MenuDropAlignment")
			{
				MenuDropAlignmentHelper.EnsureStandardPopupAlignment();
			}
		}

		// Token: 0x0400040A RID: 1034
		private static readonly FieldInfo _menuDropAlignmentField;
	}
}
