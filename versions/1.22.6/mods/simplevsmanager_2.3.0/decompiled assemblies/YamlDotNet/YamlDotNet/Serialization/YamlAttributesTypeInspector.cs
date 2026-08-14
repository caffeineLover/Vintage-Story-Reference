using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000041 RID: 65
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x00006878 File Offset: 0x00004A78
		public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006887 File Offset: 0x00004A87
		public override string GetEnumName(Type enumType, string name)
		{
			return this.innerTypeDescriptor.GetEnumName(enumType, name);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00006896 File Offset: 0x00004A96
		public override string GetEnumValue(object enumValue)
		{
			return this.innerTypeDescriptor.GetEnumValue(enumValue);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000068A4 File Offset: 0x00004AA4
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.OrderBy<IPropertyDescriptor, int>(Enumerable.Select<IPropertyDescriptor, IPropertyDescriptor>(Enumerable.Where<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(type, container), (IPropertyDescriptor p) => p.GetCustomAttribute<YamlIgnoreAttribute>() == null), delegate(IPropertyDescriptor p)
			{
				PropertyDescriptor propertyDescriptor = new PropertyDescriptor(p);
				YamlMemberAttribute customAttribute = p.GetCustomAttribute<YamlMemberAttribute>();
				if (customAttribute != null)
				{
					if (customAttribute.SerializeAs != null)
					{
						propertyDescriptor.TypeOverride = customAttribute.SerializeAs;
					}
					propertyDescriptor.Order = customAttribute.Order;
					propertyDescriptor.ScalarStyle = customAttribute.ScalarStyle;
					if (customAttribute.Alias != null)
					{
						propertyDescriptor.Name = customAttribute.Alias;
					}
				}
				return propertyDescriptor;
			}), (IPropertyDescriptor p) => p.Order);
		}

		// Token: 0x04000070 RID: 112
		private readonly ITypeInspector innerTypeDescriptor;
	}
}
