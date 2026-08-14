using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200010E RID: 270
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CompositeTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x000155EB File Offset: 0x000137EB
		public CompositeTypeInspector(params ITypeInspector[] typeInspectors) : this(typeInspectors)
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000155F4 File Offset: 0x000137F4
		public CompositeTypeInspector(IEnumerable<ITypeInspector> typeInspectors)
		{
			List<ITypeInspector> list = (typeInspectors != null) ? Enumerable.ToList<ITypeInspector>(typeInspectors) : null;
			if (list == null)
			{
				throw new ArgumentNullException("typeInspectors");
			}
			this.typeInspectors = list;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00015620 File Offset: 0x00013820
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.SelectMany<ITypeInspector, IPropertyDescriptor>(this.typeInspectors, (ITypeInspector i) => i.GetProperties(type, container));
		}

		// Token: 0x040002B2 RID: 690
		private readonly IEnumerable<ITypeInspector> typeInspectors;
	}
}
