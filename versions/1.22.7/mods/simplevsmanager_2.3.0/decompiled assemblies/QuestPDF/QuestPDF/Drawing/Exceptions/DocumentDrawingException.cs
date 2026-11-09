using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing.Exceptions
{
	// Token: 0x0200011D RID: 285
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DocumentDrawingException : Exception
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x00019F2A File Offset: 0x0001812A
		internal DocumentDrawingException(string message) : base(message)
		{
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00019F33 File Offset: 0x00018133
		internal DocumentDrawingException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
