using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	public class ConfigLibException : Exception
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000024A6 File Offset: 0x000006A6
		public ConfigLibException()
		{
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024AE File Offset: 0x000006AE
		public ConfigLibException(string message) : base(message)
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000024B7 File Offset: 0x000006B7
		public ConfigLibException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
