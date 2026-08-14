using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x0200017B RID: 379
	[NullableContext(1)]
	public interface ITypeDiscriminator
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060007AF RID: 1967
		Type BaseType { get; }

		// Token: 0x060007B0 RID: 1968
		bool TryDiscriminate(IParser buffer, [Nullable(2)] out Type suggestedType);
	}
}
