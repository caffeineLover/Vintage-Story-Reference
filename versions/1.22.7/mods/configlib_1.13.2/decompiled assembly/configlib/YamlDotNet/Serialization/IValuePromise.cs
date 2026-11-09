using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A3 RID: 163
	public interface IValuePromise
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060003C0 RID: 960
		// (remove) Token: 0x060003C1 RID: 961
		[Nullable(new byte[]
		{
			1,
			2
		})]
		event Action<object> ValueAvailable;
	}
}
