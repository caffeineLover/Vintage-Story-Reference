using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing.Exceptions
{
	// Token: 0x0200011C RID: 284
	public sealed class DocumentComposeException : Exception
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x00019F21 File Offset: 0x00018121
		[NullableContext(1)]
		internal DocumentComposeException(string message) : base(message)
		{
		}
	}
}
