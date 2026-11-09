using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000D9 RID: 217
	[NullableContext(1)]
	[Nullable(0)]
	public class SemanticErrorException : YamlException
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x000167FA File Offset: 0x000149FA
		public SemanticErrorException(string message) : base(message)
		{
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00016803 File Offset: 0x00014A03
		public SemanticErrorException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001680E File Offset: 0x00014A0E
		public SemanticErrorException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
