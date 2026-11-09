using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace ModernWpf.Markup
{
	// Token: 0x02000251 RID: 593
	public class DynamicColorExtensionConverter : TypeConverter
	{
		// Token: 0x0600111E RID: 4382 RVA: 0x0003299A File Offset: 0x00030B9A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000329B8 File Offset: 0x00030BB8
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
			DynamicColorExtension dynamicColorExtension = value as DynamicColorExtension;
			if (dynamicColorExtension == null)
			{
				throw new ArgumentException(string.Format("{0} must be of type {1}", value, "DynamicColorExtension"), "value");
			}
			return new InstanceDescriptor(typeof(DynamicColorExtension).GetConstructor(new Type[]
			{
				typeof(object)
			}), new object[]
			{
				dynamicColorExtension.ResourceKey
			});
		}
	}
}
