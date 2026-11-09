using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001A5 RID: 421
	[NullableContext(1)]
	[Nullable(0)]
	public class AnchorNotFoundException : YamlException
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public AnchorNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001C8FD File Offset: 0x0001AAFD
		public AnchorNotFoundException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001C908 File Offset: 0x0001AB08
		public AnchorNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
