using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	public interface ITypeDiscriminator
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000365 RID: 869
		Type BaseType { get; }

		// Token: 0x06000366 RID: 870
		bool TryDiscriminate(IParser buffer, [Nullable(2)] out Type suggestedType);
	}
}
