using System;

namespace RPVoiceChat.Server
{
	// Token: 0x02000060 RID: 96
	public readonly struct RoutedVoiceRecipient
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0001024B File Offset: 0x0000E44B
		public RoutedVoiceRecipient(string playerUid, VoiceRoute route, double distanceSq)
		{
			this.PlayerUID = playerUid;
			this.Route = route;
			this.DistanceSq = distanceSq;
		}

		// Token: 0x04000107 RID: 263
		public readonly string PlayerUID;

		// Token: 0x04000108 RID: 264
		public readonly VoiceRoute Route;

		// Token: 0x04000109 RID: 265
		public readonly double DistanceSq;
	}
}
