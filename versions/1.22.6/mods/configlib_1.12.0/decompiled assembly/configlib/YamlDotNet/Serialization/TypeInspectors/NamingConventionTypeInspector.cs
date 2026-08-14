using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000110 RID: 272
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NamingConventionTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x0001566C File Offset: 0x0001386C
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

		// Token: 0x060005FA RID: 1530 RVA: 0x000156A0 File Offset: 0x000138A0
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

		// Token: 0x040002B5 RID: 693
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x040002B6 RID: 694
		private readonly INamingConvention namingConvention;
	}
}
