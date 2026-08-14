using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000018 RID: 24
	[NullableContext(1)]
	[Nullable(0)]
	public class NodeNotFoundException : Exception
	{
		// Token: 0x0600017F RID: 383 RVA: 0x0000C85F File Offset: 0x0000AA5F
		public NodeNotFoundException()
		{
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000C867 File Offset: 0x0000AA67
		public NodeNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000C870 File Offset: 0x0000AA70
		public NodeNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
