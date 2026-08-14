using System;

namespace RPVoiceChat
{
	// Token: 0x02000023 RID: 35
	public class NoStackTraceException : Exception
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00006E82 File Offset: 0x00005082
		public override string StackTrace
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006E85 File Offset: 0x00005085
		public NoStackTraceException(string message) : base(message)
		{
		}
	}
}
