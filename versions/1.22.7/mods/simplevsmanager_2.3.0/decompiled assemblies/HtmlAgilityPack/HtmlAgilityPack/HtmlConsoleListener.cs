using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	internal class HtmlConsoleListener : TraceListener
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002EFC File Offset: 0x000010FC
		public override void Write(string Message)
		{
			this.Write(Message, "");
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002F0A File Offset: 0x0000110A
		public override void Write(string Message, string Category)
		{
			Console.Write("T:" + Category + ": " + Message);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002F22 File Offset: 0x00001122
		public override void WriteLine(string Message)
		{
			this.Write(Message + "\n");
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F35 File Offset: 0x00001135
		public override void WriteLine(string Message, string Category)
		{
			this.Write(Message + "\n", Category);
		}
	}
}
