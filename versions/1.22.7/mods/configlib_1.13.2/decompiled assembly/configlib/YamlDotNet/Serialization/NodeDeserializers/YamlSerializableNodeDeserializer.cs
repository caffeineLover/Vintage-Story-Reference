using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200015B RID: 347
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlSerializableNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x000186F1 File Offset: 0x000168F1
		public YamlSerializableNodeDeserializer(IObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00018700 File Offset: 0x00016900
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			if (typeof(IYamlSerializable).IsAssignableFrom(expectedType))
			{
				IYamlSerializable yamlSerializable = (IYamlSerializable)this.objectFactory.Create(expectedType);
				yamlSerializable.ReadYaml(parser);
				value = yamlSerializable;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x04000304 RID: 772
		private readonly IObjectFactory objectFactory;
	}
}
