using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000159 RID: 345
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlConvertibleNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000705 RID: 1797 RVA: 0x00018662 File Offset: 0x00016862
		public YamlConvertibleNodeDeserializer(IObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00018674 File Offset: 0x00016874
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

		// Token: 0x04000301 RID: 769
		private readonly IObjectFactory objectFactory;
	}
}
