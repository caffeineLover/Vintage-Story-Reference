using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	public class InvalidConfigException : ConfigLibException
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000024C1 File Offset: 0x000006C1
		public InvalidConfigException()
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024C9 File Offset: 0x000006C9
		public InvalidConfigException(string message) : base(message)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024D2 File Offset: 0x000006D2
		public InvalidConfigException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
