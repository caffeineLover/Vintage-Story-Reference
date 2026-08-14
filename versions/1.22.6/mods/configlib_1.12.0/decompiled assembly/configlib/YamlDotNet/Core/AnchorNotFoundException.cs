using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001A9 RID: 425
	[NullableContext(1)]
	[Nullable(0)]
	public class AnchorNotFoundException : YamlException
	{
		// Token: 0x0600091F RID: 2335 RVA: 0x0001D23C File Offset: 0x0001B43C
		public AnchorNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001D245 File Offset: 0x0001B445
		public AnchorNotFoundException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001D250 File Offset: 0x0001B450
		public AnchorNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
