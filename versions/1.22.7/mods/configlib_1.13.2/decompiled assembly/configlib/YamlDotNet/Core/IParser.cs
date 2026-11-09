using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001B6 RID: 438
	[NullableContext(2)]
	public interface IParser
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600097E RID: 2430
		ParsingEvent Current { get; }

		// Token: 0x0600097F RID: 2431
		bool MoveNext();
	}
}
