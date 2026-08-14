using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Properties
{
	// Token: 0x02000096 RID: 150
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.14.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00050B30 File Offset: 0x0004ED30
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040005D9 RID: 1497
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
