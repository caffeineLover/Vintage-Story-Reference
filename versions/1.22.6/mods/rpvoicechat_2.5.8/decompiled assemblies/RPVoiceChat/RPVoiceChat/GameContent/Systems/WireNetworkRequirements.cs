using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x02000095 RID: 149
	public readonly struct WireNetworkRequirements
	{
		// Token: 0x060004D0 RID: 1232 RVA: 0x00019E74 File Offset: 0x00018074
		public WireNetworkRequirements(float minPowerPercent, int maxEndpoints)
		{
			this.MinPowerPercent = minPowerPercent;
			this.MaxEndpoints = maxEndpoints;
		}

		// Token: 0x04000267 RID: 615
		public readonly float MinPowerPercent;

		// Token: 0x04000268 RID: 616
		public readonly int MaxEndpoints;
	}
}
