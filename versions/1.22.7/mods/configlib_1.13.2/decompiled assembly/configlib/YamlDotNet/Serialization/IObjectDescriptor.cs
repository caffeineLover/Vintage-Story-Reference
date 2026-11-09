using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	public interface IObjectDescriptor
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600038E RID: 910
		[Nullable(2)]
		object Value { [NullableContext(2)] get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600038F RID: 911
		Type Type { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000390 RID: 912
		Type StaticType { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000391 RID: 913
		ScalarStyle ScalarStyle { get; }
	}
}
