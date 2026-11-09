using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing.Exceptions
{
	// Token: 0x0200011E RID: 286
	public sealed class DocumentLayoutException : Exception
	{
		// Token: 0x060009CA RID: 2506 RVA: 0x00019F3D File Offset: 0x0001813D
		[NullableContext(1)]
		internal DocumentLayoutException(string message) : base(message)
		{
		}
	}
}
