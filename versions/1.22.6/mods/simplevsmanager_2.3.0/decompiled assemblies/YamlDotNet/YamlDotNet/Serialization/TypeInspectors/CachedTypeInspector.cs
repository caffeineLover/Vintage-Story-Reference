using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public class CachedTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000233 RID: 563 RVA: 0x000078F5 File Offset: 0x00005AF5
		public CachedTypeInspector(ITypeInspector innerTypeDescriptor)
		{
			if (innerTypeDescriptor == null)
			{
				throw new ArgumentNullException("innerTypeDescriptor");
			}
			this.innerTypeDescriptor = innerTypeDescriptor;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00007934 File Offset: 0x00005B34
		public override string GetEnumName(Type enumType, string name)
		{
			ConcurrentDictionary<string, string> orAdd = this.enumNameCache.GetOrAdd(enumType, (Type _) => new ConcurrentDictionary<string, string>());
			return orAdd.GetOrAdd<ValueTuple<Type, ITypeInspector>>(name, delegate(string n, [TupleElementNames(new string[]
			{
				"enumType",
				"innerTypeDescriptor"
			})] [Nullable(0)] ValueTuple<Type, ITypeInspector> context)
			{
				Type item = context.Item1;
				ITypeInspector item2 = context.Item2;
				return item2.GetEnumName(item, n);
			}, new ValueTuple<Type, ITypeInspector>(enumType, this.innerTypeDescriptor));
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000079A4 File Offset: 0x00005BA4
		public override string GetEnumValue(object enumValue)
		{
			return this.enumValueCache.GetOrAdd<ValueTuple<object, ITypeInspector>>(enumValue, delegate(object _, [TupleElementNames(new string[]
			{
				"enumValue",
				"innerTypeDescriptor"
			})] [Nullable(0)] ValueTuple<object, ITypeInspector> context)
			{
				object item = context.Item1;
				ITypeInspector item2 = context.Item2;
				return item2.GetEnumValue(item);
			}, new ValueTuple<object, ITypeInspector>(enumValue, this.innerTypeDescriptor));
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000079EA File Offset: 0x00005BEA
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return this.cache.GetOrAdd<ValueTuple<object, ITypeInspector>>(type, delegate(Type t, [TupleElementNames(new string[]
			{
				"container",
				"innerTypeDescriptor"
			})] [Nullable(0)] ValueTuple<object, ITypeInspector> context)
			{
				object item = context.Item1;
				ITypeInspector item2 = context.Item2;
				return Enumerable.ToList<IPropertyDescriptor>(item2.GetProperties(t, item));
			}, new ValueTuple<object, ITypeInspector>(container, this.innerTypeDescriptor));
		}

		// Token: 0x04000088 RID: 136
		private readonly ITypeInspector innerTypeDescriptor;

		// Token: 0x04000089 RID: 137
		private readonly ConcurrentDictionary<Type, List<IPropertyDescriptor>> cache = new ConcurrentDictionary<Type, List<IPropertyDescriptor>>();

		// Token: 0x0400008A RID: 138
		private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> enumNameCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();

		// Token: 0x0400008B RID: 139
		private readonly ConcurrentDictionary<object, string> enumValueCache = new ConcurrentDictionary<object, string>();
	}
}
