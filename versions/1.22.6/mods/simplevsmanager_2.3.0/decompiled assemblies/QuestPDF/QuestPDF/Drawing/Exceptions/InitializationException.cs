using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing.Exceptions
{
	// Token: 0x0200011F RID: 287
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InitializationException : Exception
	{
		// Token: 0x060009CB RID: 2507 RVA: 0x00019F46 File Offset: 0x00018146
		internal InitializationException(string message) : base(message)
		{
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00019F4F File Offset: 0x0001814F
		internal InitializationException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
