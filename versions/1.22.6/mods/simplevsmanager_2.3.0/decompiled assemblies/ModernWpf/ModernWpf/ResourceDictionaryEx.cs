using System;
using System.Collections.Generic;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x02000225 RID: 549
	public class ResourceDictionaryEx : ResourceDictionary
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0002D42F File Offset: 0x0002B62F
		public Dictionary<object, ResourceDictionary> ThemeDictionaries { get; } = new Dictionary<object, ResourceDictionary>();

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0002D437 File Offset: 0x0002B637
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x0002D43F File Offset: 0x0002B63F
		internal ResourceDictionary MergedAppThemeDictionary { get; set; }

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002D448 File Offset: 0x0002B648
		internal void Update(string themeKey)
		{
			ResourceDictionary resourceDictionary;
			if (!this.ThemeDictionaries.TryGetValue(themeKey, ref resourceDictionary))
			{
				if (this._mergedThemeDictionary != null)
				{
					base.MergedDictionaries.Remove(this._mergedThemeDictionary);
					this._mergedThemeDictionary = null;
				}
				return;
			}
			if (this._mergedThemeDictionary == null)
			{
				int num;
				if (this.MergedAppThemeDictionary != null)
				{
					num = base.MergedDictionaries.IndexOf(this.MergedAppThemeDictionary) + 1;
				}
				else
				{
					num = 0;
				}
				base.MergedDictionaries.Insert(num, resourceDictionary);
				this._mergedThemeDictionary = resourceDictionary;
				return;
			}
			if (this._mergedThemeDictionary == resourceDictionary)
			{
				return;
			}
			int num2 = base.MergedDictionaries.IndexOf(this._mergedThemeDictionary);
			base.MergedDictionaries[num2] = resourceDictionary;
			this._mergedThemeDictionary = resourceDictionary;
		}

		// Token: 0x04000412 RID: 1042
		private ResourceDictionary _mergedThemeDictionary;
	}
}
