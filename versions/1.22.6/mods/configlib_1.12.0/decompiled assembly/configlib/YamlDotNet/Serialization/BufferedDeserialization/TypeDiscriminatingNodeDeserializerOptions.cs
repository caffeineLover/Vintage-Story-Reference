using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x0200017A RID: 378
	[NullableContext(1)]
	[Nullable(0)]
	public class TypeDiscriminatingNodeDeserializerOptions : ITypeDiscriminatingNodeDeserializerOptions
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x0001A147 File Offset: 0x00018347
		public void AddTypeDiscriminator(ITypeDiscriminator discriminator)
		{
			this.discriminators.Add(discriminator);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001A155 File Offset: 0x00018355
		public void AddKeyValueTypeDiscriminator<[Nullable(2)] T>(string discriminatorKey, IDictionary<string, Type> valueTypeMapping)
		{
			this.discriminators.Add(new KeyValueTypeDiscriminator(typeof(T), discriminatorKey, valueTypeMapping));
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001A173 File Offset: 0x00018373
		public void AddUniqueKeyTypeDiscriminator<[Nullable(2)] T>(IDictionary<string, Type> uniqueKeyTypeMapping)
		{
			this.discriminators.Add(new UniqueKeyTypeDiscriminator(typeof(T), uniqueKeyTypeMapping));
		}

		// Token: 0x0400035F RID: 863
		internal readonly List<ITypeDiscriminator> discriminators = new List<ITypeDiscriminator>();
	}
}
