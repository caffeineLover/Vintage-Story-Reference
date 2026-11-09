using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x02000176 RID: 374
	[NullableContext(1)]
	[Nullable(0)]
	public class TypeDiscriminatingNodeDeserializerOptions : ITypeDiscriminatingNodeDeserializerOptions
	{
		// Token: 0x0600076F RID: 1903 RVA: 0x000197FF File Offset: 0x000179FF
		public void AddTypeDiscriminator(ITypeDiscriminator discriminator)
		{
			this.discriminators.Add(discriminator);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001980D File Offset: 0x00017A0D
		public void AddKeyValueTypeDiscriminator<[Nullable(2)] T>(string discriminatorKey, IDictionary<string, Type> valueTypeMapping)
		{
			this.discriminators.Add(new KeyValueTypeDiscriminator(typeof(T), discriminatorKey, valueTypeMapping));
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001982B File Offset: 0x00017A2B
		public void AddUniqueKeyTypeDiscriminator<[Nullable(2)] T>(IDictionary<string, Type> uniqueKeyTypeMapping)
		{
			this.discriminators.Add(new UniqueKeyTypeDiscriminator(typeof(T), uniqueKeyTypeMapping));
		}

		// Token: 0x04000331 RID: 817
		internal readonly List<ITypeDiscriminator> discriminators = new List<ITypeDiscriminator>();
	}
}
