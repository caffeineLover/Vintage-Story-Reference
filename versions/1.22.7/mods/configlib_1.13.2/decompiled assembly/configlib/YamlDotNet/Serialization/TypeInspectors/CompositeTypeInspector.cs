using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200010A RID: 266
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CompositeTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x00014CA3 File Offset: 0x00012EA3
		public CompositeTypeInspector(params ITypeInspector[] typeInspectors) : this(typeInspectors)
		{
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00014CAC File Offset: 0x00012EAC
		public CompositeTypeInspector(IEnumerable<ITypeInspector> typeInspectors)
		{
			List<ITypeInspector> list = (typeInspectors != null) ? Enumerable.ToList<ITypeInspector>(typeInspectors) : null;
			if (list == null)
			{
				throw new ArgumentNullException("typeInspectors");
			}
			this.typeInspectors = list;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014CD8 File Offset: 0x00012ED8
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.SelectMany<ITypeInspector, IPropertyDescriptor>(this.typeInspectors, (ITypeInspector i) => i.GetProperties(type, container));
		}

		// Token: 0x04000284 RID: 644
		private readonly IEnumerable<ITypeInspector> typeInspectors;
	}
}
