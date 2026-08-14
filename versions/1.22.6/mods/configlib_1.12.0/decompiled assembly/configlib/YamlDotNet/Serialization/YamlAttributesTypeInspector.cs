using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.TypeInspectors;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000F1 RID: 241
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x0001434C File Offset: 0x0001254C
		public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001435C File Offset: 0x0001255C
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

		// Token: 0x04000286 RID: 646
		private readonly ITypeInspector innerTypeDescriptor;
	}
}
