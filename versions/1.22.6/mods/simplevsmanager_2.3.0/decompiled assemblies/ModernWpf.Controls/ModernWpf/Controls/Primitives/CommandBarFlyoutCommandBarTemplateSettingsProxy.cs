using System;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E5 RID: 229
	public class CommandBarFlyoutCommandBarTemplateSettingsProxy : Freezable
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00027145 File Offset: 0x00025345
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x00027157 File Offset: 0x00025357
		public CommandBarFlyoutCommandBarTemplateSettings FlyoutTemplateSettings
		{
			get
			{
				return (CommandBarFlyoutCommandBarTemplateSettings)base.GetValue(CommandBarFlyoutCommandBarTemplateSettingsProxy.FlyoutTemplateSettingsProperty);
			}
			set
			{
				base.SetValue(CommandBarFlyoutCommandBarTemplateSettingsProxy.FlyoutTemplateSettingsProperty, value);
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00027165 File Offset: 0x00025365
		protected override Freezable CreateInstanceCore()
		{
			return new CommandBarFlyoutCommandBarTemplateSettingsProxy();
		}

		// Token: 0x040004CF RID: 1231
		public static readonly DependencyProperty FlyoutTemplateSettingsProperty = DependencyProperty.Register("FlyoutTemplateSettings", typeof(CommandBarFlyoutCommandBarTemplateSettings), typeof(CommandBarFlyoutCommandBarTemplateSettingsProxy), null);
	}
}
