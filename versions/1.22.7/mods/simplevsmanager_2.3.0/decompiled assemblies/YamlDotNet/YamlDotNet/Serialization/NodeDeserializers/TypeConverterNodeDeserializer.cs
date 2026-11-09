using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000085 RID: 133
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TypeConverterNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002FC RID: 764 RVA: 0x0000B1AC File Offset: 0x000093AC
		public TypeConverterNodeDeserializer(IEnumerable<IYamlTypeConverter> converters)
		{
			this.converters = new TypeConverterCache(converters);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B1C0 File Offset: 0x000093C0
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			IYamlTypeConverter yamlTypeConverter;
			if (!this.converters.TryGetConverterForType(expectedType, out yamlTypeConverter))
			{
				value = null;
				return false;
			}
			value = yamlTypeConverter.ReadYaml(parser, expectedType, rootDeserializer);
			return true;
		}

		// Token: 0x040000CD RID: 205
		private readonly TypeConverterCache converters;
	}
}
