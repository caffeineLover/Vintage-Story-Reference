using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000157 RID: 343
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TypeConverterNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x000185EA File Offset: 0x000167EA
		public TypeConverterNodeDeserializer(IEnumerable<IYamlTypeConverter> converters)
		{
			if (converters == null)
			{
				throw new ArgumentNullException("converters");
			}
			this.converters = converters;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00018608 File Offset: 0x00016808
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

		// Token: 0x040002FF RID: 767
		private readonly IEnumerable<IYamlTypeConverter> converters;
	}
}
