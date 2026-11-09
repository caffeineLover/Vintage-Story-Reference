using System;

namespace UglyToad.PdfPig.Logging
{
	// Token: 0x020000E4 RID: 228
	public interface ILog
	{
		// Token: 0x06000437 RID: 1079
		void Debug(string message);

		// Token: 0x06000438 RID: 1080
		void Debug(string message, Exception ex);

		// Token: 0x06000439 RID: 1081
		void Warn(string message);

		// Token: 0x0600043A RID: 1082
		void Error(string message);

		// Token: 0x0600043B RID: 1083
		void Error(string message, Exception ex);
	}
}
