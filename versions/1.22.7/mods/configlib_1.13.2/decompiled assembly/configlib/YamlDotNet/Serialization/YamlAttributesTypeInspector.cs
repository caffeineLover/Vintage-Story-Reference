using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x00013A04 File Offset: 0x00011C04
		public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00013A14 File Offset: 0x00011C14
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

		// Token: 0x04000258 RID: 600
		private readonly ITypeInspector innerTypeDescriptor;
	}
}
