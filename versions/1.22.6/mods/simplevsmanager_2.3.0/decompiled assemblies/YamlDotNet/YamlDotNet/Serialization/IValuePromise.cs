using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000029 RID: 41
	public interface IValuePromise
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000EE RID: 238
		// (remove) Token: 0x060000EF RID: 239
		[Nullable(new byte[]
		{
			1,
			2
		})]
		event Action<object> ValueAvailable;
	}
}
