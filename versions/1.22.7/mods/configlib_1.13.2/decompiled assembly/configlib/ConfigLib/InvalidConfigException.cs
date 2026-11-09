using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public class InvalidConfigException : ConfigLibException
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000025E1 File Offset: 0x000007E1
		public InvalidConfigException()
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000025E9 File Offset: 0x000007E9
		public InvalidConfigException(string message) : base(message)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000025F2 File Offset: 0x000007F2
		public InvalidConfigException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
