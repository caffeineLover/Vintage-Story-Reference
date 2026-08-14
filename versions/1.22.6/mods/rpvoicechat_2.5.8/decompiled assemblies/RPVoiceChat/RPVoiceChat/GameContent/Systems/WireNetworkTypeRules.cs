using System;
using RPVoiceChat.Config;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x02000096 RID: 150
	public static class WireNetworkTypeRules
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x00019E84 File Offset: 0x00018084
		private static float PercentToNormalized(int valuePercent)
		{
			if (valuePercent > 0)
			{
				return (float)valuePercent / 100f;
			}
			return 0f;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00019E98 File Offset: 0x00018098
		public static WireNetworkKind ResolveKindForSwitchboardPowerCheck(WireNetworkKind kind)
		{
			if (kind == WireNetworkKind.None)
			{
				return WireNetworkKind.Telegraph;
			}
			return kind;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00019EA0 File Offset: 0x000180A0
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
