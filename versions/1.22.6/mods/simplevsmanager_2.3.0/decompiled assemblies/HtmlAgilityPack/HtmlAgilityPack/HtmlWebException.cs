using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000023 RID: 35
	public class HtmlWebException : Exception
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000E903 File Offset: 0x0000CB03
		[NullableContext(1)]
		public HtmlWebException(string message) : base(message)
		{
		}
	}
}
