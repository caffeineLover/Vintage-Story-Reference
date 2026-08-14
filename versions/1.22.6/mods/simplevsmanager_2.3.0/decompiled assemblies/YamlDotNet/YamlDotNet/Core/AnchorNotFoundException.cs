using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000C1 RID: 193
	[NullableContext(1)]
	[Nullable(0)]
	public class AnchorNotFoundException : YamlException
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x0000EC65 File Offset: 0x0000CE65
		public AnchorNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000EC6E File Offset: 0x0000CE6E
		public AnchorNotFoundException(in Mark start, in Mark end, string message) : base(start, end, message)
		{
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000EC79 File Offset: 0x0000CE79
		public AnchorNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
