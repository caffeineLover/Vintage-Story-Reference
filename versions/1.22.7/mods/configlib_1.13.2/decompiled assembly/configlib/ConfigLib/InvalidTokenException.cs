using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	public class InvalidTokenException : ConfigLibException
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000025E1 File Offset: 0x000007E1
		public InvalidTokenException()
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000025E9 File Offset: 0x000007E9
		public InvalidTokenException(string message) : base(message)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000025F2 File Offset: 0x000007F2
		public InvalidTokenException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
