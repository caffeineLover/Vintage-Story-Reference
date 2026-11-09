using System;
using System.Runtime.CompilerServices;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000064 RID: 100
	public class HealthCheckException : NoStackTraceException
	{
		// Token: 0x0600027B RID: 635 RVA: 0x0001040C File Offset: 0x0000E60C
		public HealthCheckException(NetworkSide side)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NetworkSide>(side);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("failed readiness probe. Aborting to prevent silent malfunction.");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0400010F RID: 271
		private const string msg = "failed readiness probe. Aborting to prevent silent malfunction.";
	}
}
