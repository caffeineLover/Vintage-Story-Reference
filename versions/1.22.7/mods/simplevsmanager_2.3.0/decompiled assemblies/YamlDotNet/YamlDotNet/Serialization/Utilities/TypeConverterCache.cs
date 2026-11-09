using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TypeConverterCache
	{
		// Token: 0x0600022A RID: 554 RVA: 0x0000769D File Offset: 0x0000589D
		public TypeConverterCache([Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<IYamlTypeConverter> typeConverters) : this(((typeConverters != null) ? Enumerable.ToArray<IYamlTypeConverter>(typeConverters) : null) ?? Array.Empty<IYamlTypeConverter>())
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000076BA File Offset: 0x000058BA
		public TypeConverterCache(IYamlTypeConverter[] typeConverters)
		{
			this.typeConverters = typeConverters;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool TryGetConverterForType(Type type, [Nullable(2)] [NotNullWhen(true)] out IYamlTypeConverter typeConverter)
		{
			ValueTuple<bool, IYamlTypeConverter> orAdd = this.cache.GetOrAdd<IYamlTypeConverter[]>(type, ([Nullable(1)] Type t, IYamlTypeConverter[] tc) => TypeConverterCache.LookupTypeConverter(t, tc), this.typeConverters);
			typeConverter = orAdd.Item2;
			return orAdd.Item1;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00007724 File Offset: 0x00005924
		public IYamlTypeConverter GetConverterByType(Type converter)
		{
			foreach (IYamlTypeConverter yamlTypeConverter in this.typeConverters)
			{
				if (yamlTypeConverter.GetType() == converter)
				{
					return yamlTypeConverter;
				}
			}
			throw new ArgumentException("IYamlTypeConverter of type " + converter.FullName + " not found", "converter");
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000777C File Offset: 0x0000597C
		[return: TupleElementNames(new string[]
		{
			"HasMatch",
			"TypeConverter"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		private static ValueTuple<bool, IYamlTypeConverter> LookupTypeConverter(Type type, IYamlTypeConverter[] typeConverters)
		{
			foreach (IYamlTypeConverter yamlTypeConverter in typeConverters)
			{
				if (yamlTypeConverter.Accepts(type))
				{
					return new ValueTuple<bool, IYamlTypeConverter>(true, yamlTypeConverter);
				}
			}
			return new ValueTuple<bool, IYamlTypeConverter>(false, null);
		}

		// Token: 0x04000086 RID: 134
		private readonly IYamlTypeConverter[] typeConverters;

		// Token: 0x04000087 RID: 135
		[TupleElementNames(new string[]
		{
			"HasMatch",
			"TypeConverter"
		})]
		[Nullable(new byte[]
		{
			1,
			1,
			0,
			2
		})]
		private readonly ConcurrentDictionary<Type, ValueTuple<bool, IYamlTypeConverter>> cache = new ConcurrentDictionary<Type, ValueTuple<bool, IYamlTypeConverter>>();
	}
}
