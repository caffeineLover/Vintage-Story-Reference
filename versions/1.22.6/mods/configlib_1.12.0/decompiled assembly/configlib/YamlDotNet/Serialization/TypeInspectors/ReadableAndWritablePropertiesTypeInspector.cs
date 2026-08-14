using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000111 RID: 273
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReadableAndWritablePropertiesTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x000156FE File Offset: 0x000138FE
		public ReadableAndWritablePropertiesTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			if (innerTypeDescriptor == null)
			{
				throw new ArgumentNullException("innerTypeDescriptor");
			}
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001571C File Offset: 0x0001391C
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.Where<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(type, container), (IPropertyDescriptor p) => p.CanWrite);
		}

		// Token: 0x040002B7 RID: 695
		private readonly ITypeInspector innerTypeDescriptor;
	}
}
