using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200010C RID: 268
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CachedTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x00015569 File Offset: 0x00013769
		public CachedTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			if (innerTypeDescriptor == null)
			{
				throw new ArgumentNullException("innerTypeDescriptor");
			}
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00015594 File Offset: 0x00013794
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return this.cache.GetOrAdd(type, (Type t) => Enumerable.ToList<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(t, container)));
		}

		// Token: 0x040002AE RID: 686
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x040002AF RID: 687
		private readonly ConcurrentDictionary<Type, List<IPropertyDescriptor>> cache = new ConcurrentDictionary<Type, List<IPropertyDescriptor>>();
	}
}
