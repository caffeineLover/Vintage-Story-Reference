using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000BE RID: 190
	public readonly struct WireNetworkRequirements
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x00020358 File Offset: 0x0001E558
		public WireNetworkRequirements(float minPowerPercent, int maxEndpoints)
		{
			this.MinPowerPercent = minPowerPercent;
			this.MaxEndpoints = maxEndpoints;
		}

		// Token: 0x0400030C RID: 780
		public readonly float MinPowerPercent;

		// Token: 0x0400030D RID: 781
		public readonly int MaxEndpoints;
	}
}
