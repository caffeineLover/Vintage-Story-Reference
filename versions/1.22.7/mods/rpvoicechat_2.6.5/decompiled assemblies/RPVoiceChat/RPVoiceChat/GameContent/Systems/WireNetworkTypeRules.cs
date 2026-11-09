using System;
using RPVoiceChat.Config;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000BF RID: 191
	public static class WireNetworkTypeRules
	{
		// Token: 0x060005EC RID: 1516 RVA: 0x00020368 File Offset: 0x0001E568
		private static float PercentToNormalized(int valuePercent)
		{
			if (valuePercent > 0)
			{
				return (float)valuePercent / 100f;
			}
			return 0f;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0002037C File Offset: 0x0001E57C
		public static WireNetworkKind ResolveKindForSwitchboardPowerCheck(WireNetworkKind kind)
		{
			if (kind == WireNetworkKind.None)
			{
				return WireNetworkKind.Telegraph;
			}
			return kind;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00020384 File Offset: 0x0001E584
		public static WireNetworkRequirements GetRequirements(WireNetworkKind kind)
		{
			kind = WireNetworkTypeRules.ResolveKindForSwitchboardPowerCheck(kind);
			switch (kind)
			{
			case WireNetworkKind.Telegraph:
				return new WireNetworkRequirements(WireNetworkTypeRules.PercentToNormalized(ServerConfigManager.TelegraphNetworkMinPowerPercent), ServerConfigManager.TelegraphNetworkMaxEndpoints);
			case WireNetworkKind.Telephone:
				return new WireNetworkRequirements(WireNetworkTypeRules.PercentToNormalized(ServerConfigManager.TelephoneNetworkMinPowerPercent), ServerConfigManager.TelephoneNetworkMaxEndpoints);
			case WireNetworkKind.Radio:
				return new WireNetworkRequirements(WireNetworkTypeRules.PercentToNormalized(ServerConfigManager.RadioNetworkMinPowerPercent), ServerConfigManager.RadioNetworkMaxEndpoints);
			default:
				return new WireNetworkRequirements(0f, 0);
			}
		}
	}
}
