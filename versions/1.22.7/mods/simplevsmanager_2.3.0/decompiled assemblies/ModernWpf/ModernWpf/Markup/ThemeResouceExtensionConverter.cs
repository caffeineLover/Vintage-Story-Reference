using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace ModernWpf.Markup
{
	// Token: 0x02000257 RID: 599
	public class ThemeResouceExtensionConverter : TypeConverter
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x00032B85 File Offset: 0x00030D85
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00032BA4 File Offset: 0x00030DA4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(InstanceDescriptor)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ThemeResourceExtension themeResourceExtension = value as ThemeResourceExtension;
			if (themeResourceExtension == null)
			{
				throw new ArgumentException(string.Format("{0} must be of type {1}", value, "ThemeResourceExtension"), "value");
			}
			return new InstanceDescriptor(typeof(ThemeResourceExtension).GetConstructor(new Type[]
			{
				typeof(object)
			}), new object[]
			{
				themeResourceExtension.ResourceKey
			});
		}
	}
}
