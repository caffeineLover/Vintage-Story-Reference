using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000086 RID: 134
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlConvertibleNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002FE RID: 766 RVA: 0x0000B1F1 File Offset: 0x000093F1
		public YamlConvertibleNodeDeserializer(IObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000B200 File Offset: 0x00009400
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
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

		// Token: 0x040000CE RID: 206
		private readonly IObjectFactory objectFactory;
	}
}
