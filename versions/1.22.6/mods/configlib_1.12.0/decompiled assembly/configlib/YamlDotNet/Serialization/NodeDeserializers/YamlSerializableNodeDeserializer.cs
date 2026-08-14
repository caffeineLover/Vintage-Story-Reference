using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200015F RID: 351
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlSerializableNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x00019039 File Offset: 0x00017239
		public YamlSerializableNodeDeserializer(IObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00019048 File Offset: 0x00017248
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

		// Token: 0x04000332 RID: 818
		private readonly IObjectFactory objectFactory;
	}
}
