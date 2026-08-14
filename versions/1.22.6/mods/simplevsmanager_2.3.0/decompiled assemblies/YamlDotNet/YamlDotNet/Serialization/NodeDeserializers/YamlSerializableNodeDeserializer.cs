using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000087 RID: 135
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlSerializableNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0000B269 File Offset: 0x00009469
		public YamlSerializableNodeDeserializer(IObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000B278 File Offset: 0x00009478
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
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

		// Token: 0x040000CF RID: 207
		private readonly IObjectFactory objectFactory;
	}
}
