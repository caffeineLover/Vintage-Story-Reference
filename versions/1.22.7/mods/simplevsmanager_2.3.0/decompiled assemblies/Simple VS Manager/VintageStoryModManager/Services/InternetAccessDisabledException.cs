using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000065 RID: 101
	public sealed class InternetAccessDisabledException : InvalidOperationException
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0003A3A5 File Offset: 0x000385A5
		public InternetAccessDisabledException() : base(InternetAccessDisabledException.DefaultMessage)
		{
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0003A3B2 File Offset: 0x000385B2
		[NullableContext(2)]
		public InternetAccessDisabledException(string message) : base(string.IsNullOrWhiteSpace(message) ? InternetAccessDisabledException.DefaultMessage : message)
		{
		}

		// Token: 0x04000496 RID: 1174
		[Nullable(1)]
		private static readonly string DefaultMessage = DevConfig.InternetAccessDisabledMessage;
	}
}
