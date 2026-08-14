using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000077 RID: 119
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModManagerTraceListener : TraceListener
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x000445C3 File Offset: 0x000427C3
		public ModManagerTraceListener(ModActivityLoggingService loggingService)
		{
			this._loggingService = loggingService;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000445D2 File Offset: 0x000427D2
		[NullableContext(2)]
		public override void Write(string message)
		{
			if (!string.IsNullOrWhiteSpace(message))
			{
				this._loggingService.LogDiagnostic(message.Trim());
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x000445ED File Offset: 0x000427ED
		[NullableContext(2)]
		public override void WriteLine(string message)
		{
			if (!string.IsNullOrWhiteSpace(message))
			{
				this._loggingService.LogDiagnostic(message.Trim());
			}
		}

		// Token: 0x04000505 RID: 1285
		private readonly ModActivityLoggingService _loggingService;
	}
}
