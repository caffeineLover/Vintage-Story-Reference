using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000C1 RID: 193
	public interface ISwitchboardNode
	{
		// Token: 0x060005F0 RID: 1520
		bool HasSufficientPowerFor(WireNetworkKind networkKind);
	}
}
