using System;
using System.Runtime.CompilerServices;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000047 RID: 71
	public class HealthCheckException : NoStackTraceException
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000C940 File Offset: 0x0000AB40
		public HealthCheckException(NetworkSide side)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NetworkSide>(side);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("failed readiness probe. Aborting to prevent silent malfunction.");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x040000BD RID: 189
		private const string msg = "failed readiness probe. Aborting to prevent silent malfunction.";
	}
}
