using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009B RID: 155
	[NullableContext(1)]
	public interface IObjectDescriptor
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003CA RID: 970
		[Nullable(2)]
		object Value { [NullableContext(2)] get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003CB RID: 971
		Type Type { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003CC RID: 972
		Type StaticType { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003CD RID: 973
		ScalarStyle ScalarStyle { get; }
	}
}
