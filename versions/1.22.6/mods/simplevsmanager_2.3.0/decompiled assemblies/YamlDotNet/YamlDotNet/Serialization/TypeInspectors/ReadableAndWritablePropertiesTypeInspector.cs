using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000058 RID: 88
	[NullableContext(1)]
	[Nullable(0)]
	public class ReadableAndWritablePropertiesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000241 RID: 577 RVA: 0x00007C62 File Offset: 0x00005E62
		public ReadableAndWritablePropertiesTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			if (innerTypeDescriptor == null)
			{
				throw new ArgumentNullException("innerTypeDescriptor");
			}
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00007C80 File Offset: 0x00005E80
		public override string GetEnumName(Type enumType, string name)
		{
			return this.innerTypeDescriptor.GetEnumName(enumType, name);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00007C8F File Offset: 0x00005E8F
		public override string GetEnumValue(object enumValue)
		{
			return this.innerTypeDescriptor.GetEnumValue(enumValue);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00007C9D File Offset: 0x00005E9D
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Where<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(type, container), (IPropertyDescriptor p) => p.CanWrite);
		}

		// Token: 0x0400008F RID: 143
		private readonly ITypeInspector innerTypeDescriptor;
	}
}
