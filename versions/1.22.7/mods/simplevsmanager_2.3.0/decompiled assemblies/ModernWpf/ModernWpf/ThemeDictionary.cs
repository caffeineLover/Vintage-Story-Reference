using System;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x02000227 RID: 551
	public static class ThemeDictionary
	{
		// Token: 0x06000FE9 RID: 4073 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		public static void SetKey(ResourceDictionary themeDictionary, string key)
		{
			ResourceDictionary baseThemeDictionary = ThemeDictionary.GetBaseThemeDictionary(key);
			themeDictionary.MergedDictionaries.Insert(0, baseThemeDictionary);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0002D5F1 File Offset: 0x0002B7F1
		private static ResourceDictionary GetBaseThemeDictionary(string key)
		{
			ThemeResources themeResources = ThemeResources.Current;
			return ((themeResources != null) ? themeResources.TryGetThemeDictionary(key) : null) ?? ThemeManager.GetDefaultThemeDictionary(key);
		}
	}
}
