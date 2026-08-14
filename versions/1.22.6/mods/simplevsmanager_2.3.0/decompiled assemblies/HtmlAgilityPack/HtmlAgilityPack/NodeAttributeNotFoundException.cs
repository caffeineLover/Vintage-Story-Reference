using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	public class NodeAttributeNotFoundException : Exception
	{
		// Token: 0x06000182 RID: 386 RVA: 0x0000C87A File Offset: 0x0000AA7A
		public NodeAttributeNotFoundException()
		{
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000C882 File Offset: 0x0000AA82
		public NodeAttributeNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000C88B File Offset: 0x0000AA8B
		public NodeAttributeNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
