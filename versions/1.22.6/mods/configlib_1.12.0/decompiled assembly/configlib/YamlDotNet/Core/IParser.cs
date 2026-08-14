using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001BA RID: 442
	[NullableContext(2)]
	public interface IParser
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060009BA RID: 2490
		ParsingEvent Current { get; }

		// Token: 0x060009BB RID: 2491
		bool MoveNext();
	}
}
