using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VintageStoryModManager.Helpers
{
	// Token: 0x020000C5 RID: 197
	[NullableContext(1)]
	[Nullable(0)]
	public static class HoverEffectHelper
	{
		// Token: 0x06001002 RID: 4098 RVA: 0x00054D0D File Offset: 0x00052F0D
		public static bool GetDisableHoverEffects(DependencyObject obj)
		{
			return (bool)obj.GetValue(HoverEffectHelper.DisableHoverEffectsProperty);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00054D1F File Offset: 0x00052F1F
		public static void SetDisableHoverEffects(DependencyObject obj, bool value)
		{
			obj.SetValue(HoverEffectHelper.DisableHoverEffectsProperty, value);
		}

		// Token: 0x04000702 RID: 1794
		public static readonly DependencyProperty DisableHoverEffectsProperty = DependencyProperty.RegisterAttached("DisableHoverEffects", typeof(bool), typeof(HoverEffectHelper), new FrameworkPropertyMetadata(false, 32));
	}
}
