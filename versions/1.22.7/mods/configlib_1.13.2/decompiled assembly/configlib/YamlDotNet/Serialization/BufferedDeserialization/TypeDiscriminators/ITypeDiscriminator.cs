using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x02000177 RID: 375
	[NullableContext(1)]
	public interface ITypeDiscriminator
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000773 RID: 1907
		Type BaseType { get; }

		// Token: 0x06000774 RID: 1908
		bool TryDiscriminate(IParser buffer, [Nullable(2)] out Type suggestedType);
	}
}
