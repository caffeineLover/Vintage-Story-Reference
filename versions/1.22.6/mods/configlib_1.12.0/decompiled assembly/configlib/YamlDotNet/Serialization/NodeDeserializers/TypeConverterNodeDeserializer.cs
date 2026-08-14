using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200015B RID: 347
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TypeConverterNodeDeserializer : INodeDeserializer
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00018F32 File Offset: 0x00017132
		public TypeConverterNodeDeserializer(IEnumerable<IYamlTypeConverter> converters)
		{
			if (converters == null)
			{
				throw new ArgumentNullException("converters");
			}
			this.converters = converters;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00018F50 File Offset: 0x00017150
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			IYamlTypeConverter yamlTypeConverter = Enumerable.FirstOrDefault<IYamlTypeConverter>(this.converters, (IYamlTypeConverter c) => c.Accepts(expectedType));
			if (yamlTypeConverter == null)
			{
				value = null;
				return false;
			}
			value = yamlTypeConverter.ReadYaml(parser, expectedType);
			return true;
		}

		// Token: 0x0400032D RID: 813
		private readonly IEnumerable<IYamlTypeConverter> converters;
	}
}
