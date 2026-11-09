using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	public class ConfigLibException : Exception
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000025C6 File Offset: 0x000007C6
		public ConfigLibException()
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000025CE File Offset: 0x000007CE
		public ConfigLibException(string message) : base(message)
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000025D7 File Offset: 0x000007D7
		public ConfigLibException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
