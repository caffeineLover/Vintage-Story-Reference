using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace ModernWpf.Markup
{
	// Token: 0x02000250 RID: 592
	[TypeConverter(typeof(DynamicColorExtensionConverter))]
	public class DynamicColorExtension : DynamicResourceExtension
	{
		// Token: 0x0600111B RID: 4379 RVA: 0x00032939 File Offset: 0x00030B39
		public DynamicColorExtension()
		{
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00032941 File Offset: 0x00030B41
		public DynamicColorExtension(object resourceKey) : base(resourceKey)
		{
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0003294C File Offset: 0x00030B4C
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
