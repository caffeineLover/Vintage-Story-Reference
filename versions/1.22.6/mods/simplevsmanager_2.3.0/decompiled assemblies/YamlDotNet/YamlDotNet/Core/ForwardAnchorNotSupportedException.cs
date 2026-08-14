using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ForwardAnchorNotSupportedException : YamlException
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x000117B6 File Offset: 0x0000F9B6
		public ForwardAnchorNotSupportedException(string message) : base(message)
		{
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000117BF File Offset: 0x0000F9BF
		public ForwardAnchorNotSupportedException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000117CA File Offset: 0x0000F9CA
		public ForwardAnchorNotSupportedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
