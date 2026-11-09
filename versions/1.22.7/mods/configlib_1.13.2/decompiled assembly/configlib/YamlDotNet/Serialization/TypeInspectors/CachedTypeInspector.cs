using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000108 RID: 264
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CachedTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005B4 RID: 1460 RVA: 0x00014C21 File Offset: 0x00012E21
		public CachedTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			if (innerTypeDescriptor == null)
			{
				throw new ArgumentNullException("innerTypeDescriptor");
			}
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00014C4C File Offset: 0x00012E4C
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return this.cache.GetOrAdd(type, (Type t) => Enumerable.ToList<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(t, container)));
		}

		// Token: 0x04000280 RID: 640
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x04000281 RID: 641
		private readonly ConcurrentDictionary<Type, List<IPropertyDescriptor>> cache = new ConcurrentDictionary<Type, List<IPropertyDescriptor>>();
	}
}
