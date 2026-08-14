using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200015D RID: 349
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlConvertibleNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x00018FAA File Offset: 0x000171AA
		public YamlConvertibleNodeDeserializer(IObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00018FBC File Offset: 0x000171BC
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			if (typeof(IYamlConvertible).IsAssignableFrom(expectedType))
			{
				IYamlConvertible yamlConvertible = (IYamlConvertible)this.objectFactory.Create(expectedType);
				yamlConvertible.Read(parser, expectedType, (Type type) => nestedObjectDeserializer.Invoke(parser, type));
				value = yamlConvertible;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x0400032F RID: 815
		private readonly IObjectFactory objectFactory;
	}
}
