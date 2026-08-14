using System;
using System.ComponentModel;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x02000214 RID: 532
	internal static class DesignMode
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0002CB09 File Offset: 0x0002AD09
		public static bool DesignModeEnabled
		{
			get
			{
				return DesignMode._designModeEnabled.Value;
			}
		}

		// Token: 0x04000402 RID: 1026
		private static readonly Lazy<bool> _designModeEnabled = new Lazy<bool>(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));
	}
}
