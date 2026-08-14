using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public class InvalidTokenException : ConfigLibException
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000024C1 File Offset: 0x000006C1
		public InvalidTokenException()
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024C9 File Offset: 0x000006C9
		public InvalidTokenException(string message) : base(message)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024D2 File Offset: 0x000006D2
		public InvalidTokenException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
