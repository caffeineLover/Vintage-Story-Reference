using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace ModernWpf.Markup
{
	// Token: 0x02000253 RID: 595
	public class StaticColorExtension : StaticResourceExtension
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x00032A8B File Offset: 0x00030C8B
		public StaticColorExtension()
		{
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00032A93 File Offset: 0x00030C93
		public StaticColorExtension(object resourceKey) : base(resourceKey)
		{
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00032A9C File Offset: 0x00030C9C
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			object result = base.ProvideValue(serviceProvider);
			IProvideValueTarget provideValueTarget = ((serviceProvider != null) ? serviceProvider.GetService(typeof(IProvideValueTarget)) : null) as IProvideValueTarget;
			if (provideValueTarget != null)
			{
				SolidColorBrush solidColorBrush = provideValueTarget.TargetObject as SolidColorBrush;
				if (solidColorBrush != null)
				{
					ThemeResourceHelper.SetColorKey(solidColorBrush, base.ResourceKey);
				}
			}
			return result;
		}
	}
}
