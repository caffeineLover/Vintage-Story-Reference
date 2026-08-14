using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	[Nullable(0)]
	public class TypeDiscriminatingNodeDeserializerOptions : ITypeDiscriminatingNodeDeserializerOptions
	{
		// Token: 0x06000361 RID: 865 RVA: 0x0000C480 File Offset: 0x0000A680
		public void AddTypeDiscriminator(ITypeDiscriminator discriminator)
		{
			this.discriminators.Add(discriminator);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C48E File Offset: 0x0000A68E
		public void AddKeyValueTypeDiscriminator<[Nullable(2)] T>(string discriminatorKey, IDictionary<string, Type> valueTypeMapping)
		{
			this.discriminators.Add(new KeyValueTypeDiscriminator(typeof(T), discriminatorKey, valueTypeMapping));
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public void AddUniqueKeyTypeDiscriminator<[Nullable(2)] T>(IDictionary<string, Type> uniqueKeyTypeMapping)
		{
			this.discriminators.Add(new UniqueKeyTypeDiscriminator(typeof(T), uniqueKeyTypeMapping));
		}

		// Token: 0x040000F9 RID: 249
		internal readonly List<ITypeDiscriminator> discriminators = new List<ITypeDiscriminator>();
	}
}
