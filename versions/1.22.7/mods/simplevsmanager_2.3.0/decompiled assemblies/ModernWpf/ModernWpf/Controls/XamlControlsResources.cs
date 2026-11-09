using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200026E RID: 622
	public class XamlControlsResources : ResourceDictionary
	{
		// Token: 0x060011B8 RID: 4536 RVA: 0x00033F07 File Offset: 0x00032107
		public XamlControlsResources()
		{
			base.MergedDictionaries.Add(XamlControlsResources.ControlsResources);
			base.MergedDictionaries.Add(XamlControlsResources.UISettingsResources);
			if (DesignMode.DesignModeEnabled)
			{
				ResourceDictionary compactResources = XamlControlsResources.CompactResources;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00033F3C File Offset: 0x0003213C
		// (set) Token: 0x060011BA RID: 4538 RVA: 0x00033F44 File Offset: 0x00032144
		public bool UseCompactResources
		{
			get
			{
				return this._useCompactResources;
			}
			set
			{
				if (this._useCompactResources != value)
				{
					this._useCompactResources = value;
					if (this.UseCompactResources)
					{
						base.MergedDictionaries.Add(XamlControlsResources.CompactResources);
						return;
					}
					base.MergedDictionaries.Remove(XamlControlsResources.CompactResources);
				}
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00033F80 File Offset: 0x00032180
		internal static ResourceDictionary ControlsResources
		{
			get
			{
				if (XamlControlsResources._controlsResources == null)
				{
					XamlControlsResources._controlsResources = new ResourceDictionary
					{
						Source = PackUriHelper.GetAbsoluteUri("ControlsResources.xaml")
					};
				}
				return XamlControlsResources._controlsResources;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x00033FA8 File Offset: 0x000321A8
		internal static ResourceDictionary CompactResources
		{
			get
			{
				if (XamlControlsResources._compactResources == null)
				{
					XamlControlsResources._compactResources = new ResourceDictionary
					{
						Source = PackUriHelper.GetAbsoluteUri("DensityStyles/Compact.xaml")
					};
				}
				return XamlControlsResources._compactResources;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x00033FD0 File Offset: 0x000321D0
		internal static ResourceDictionary UISettingsResources
		{
			get
			{
				ResourceDictionary result;
				if ((result = XamlControlsResources._uiSettingsResources) == null)
				{
					result = (XamlControlsResources._uiSettingsResources = new UISettingsResources());
				}
				return result;
			}
		}

		// Token: 0x040004ED RID: 1261
		private static ResourceDictionary _controlsResources;

		// Token: 0x040004EE RID: 1262
		private static ResourceDictionary _compactResources;

		// Token: 0x040004EF RID: 1263
		private static ResourceDictionary _uiSettingsResources;

		// Token: 0x040004F0 RID: 1264
		private bool _useCompactResources;
	}
}
