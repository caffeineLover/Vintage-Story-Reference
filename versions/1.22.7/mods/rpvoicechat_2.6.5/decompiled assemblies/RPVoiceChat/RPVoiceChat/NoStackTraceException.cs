using System;

namespace RPVoiceChat
{
	// Token: 0x02000023 RID: 35
	public class NoStackTraceException : Exception
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000768A File Offset: 0x0000588A
		public override string StackTrace
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000768D File Offset: 0x0000588D
		public NoStackTraceException(string message) : base(message)
		{
		}
	}
}
