using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200001D RID: 29
	[NullableContext(1)]
	public interface IObjectDescriptor
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B7 RID: 183
		[Nullable(2)]
		object Value { [NullableContext(2)] get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B8 RID: 184
		Type Type { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B9 RID: 185
		Type StaticType { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000BA RID: 186
		ScalarStyle ScalarStyle { get; }
	}
}
