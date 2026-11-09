using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000057 RID: 87
	[NullableContext(1)]
	[Nullable(0)]
	public class NamingConventionTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x0600023C RID: 572 RVA: 0x00007BB0 File Offset: 0x00005DB0
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

		// Token: 0x0600023D RID: 573 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public override string GetEnumName(Type enumType, string name)
		{
			return this.innerTypeDescriptor.GetEnumName(enumType, name);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00007BF3 File Offset: 0x00005DF3
		public override string GetEnumValue(object enumValue)
		{
			return this.innerTypeDescriptor.GetEnumValue(enumValue);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00007C01 File Offset: 0x00005E01
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

		// Token: 0x0400008D RID: 141
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x0400008E RID: 142
		private readonly INamingConvention namingConvention;
	}
}
