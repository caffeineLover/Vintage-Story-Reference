using System;

namespace UglyToad.PdfPig.Logging
{
	// Token: 0x020000E5 RID: 229
	internal class NoOpLog : ILog
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x0001A4C7 File Offset: 0x000186C7
		public void Debug(string message)
		{
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001A4C9 File Offset: 0x000186C9
		public void Debug(string message, Exception ex)
		{
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001A4CB File Offset: 0x000186CB
		public void Warn(string message)
		{
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001A4CD File Offset: 0x000186CD
		public void Error(string message)
		{
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001A4CF File Offset: 0x000186CF
		public void Error(string message, Exception ex)
		{
		}
	}
}
