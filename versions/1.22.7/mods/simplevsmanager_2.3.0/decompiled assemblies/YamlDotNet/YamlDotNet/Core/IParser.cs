using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020000CD RID: 205
	[NullableContext(2)]
	public interface IParser
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600051B RID: 1307
		ParsingEvent Current { get; }

		// Token: 0x0600051C RID: 1308
		bool MoveNext();
	}
}
