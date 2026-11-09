using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200010C RID: 268
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NamingConventionTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x00014D24 File Offset: 0x00012F24
		public NamingConventionTypeInspector(ITypeInspector innerTypeDescriptor, INamingConvention namingConvention)
		{
			if (innerTypeDescriptor == null)
			{
				throw new ArgumentNullException("innerTypeDescriptor");
			}
			this.innerTypeDescriptor = innerTypeDescriptor;
			if (namingConvention == null)
			{
				throw new ArgumentNullException("namingConvention");
			}
			this.namingConvention = namingConvention;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00014D58 File Offset: 0x00012F58
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Select<IPropertyDescriptor, IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(type, container), delegate(IPropertyDescriptor p)
			{
				YamlMemberAttribute customAttribute = p.GetCustomAttribute<YamlMemberAttribute>();
				if (customAttribute != null && !customAttribute.ApplyNamingConventions)
				{
					return p;
				}
				return new PropertyDescriptor(p)
				{
					Name = this.namingConvention.Apply(p.Name)
				};
			});
		}

		// Token: 0x04000287 RID: 647
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x04000288 RID: 648
		private readonly INamingConvention namingConvention;
	}
}
