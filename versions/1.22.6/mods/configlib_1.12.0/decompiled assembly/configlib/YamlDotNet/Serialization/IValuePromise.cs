using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A7 RID: 167
	public interface IValuePromise
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060003FC RID: 1020
		// (remove) Token: 0x060003FD RID: 1021
		[Nullable(new byte[]
		{
			1,
			2
		})]
		event Action<object> ValueAvailable;
	}
}
