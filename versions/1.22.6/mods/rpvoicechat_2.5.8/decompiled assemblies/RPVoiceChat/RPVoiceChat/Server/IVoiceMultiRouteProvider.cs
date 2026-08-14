using System;
using System.Collections.Generic;

namespace RPVoiceChat.Server
{
	// Token: 0x02000044 RID: 68
	public interface IVoiceMultiRouteProvider
	{
		// Token: 0x060001D6 RID: 470
		bool TryGetRoutes(string playerUid, out IReadOnlyList<VoiceRoute> routes);
	}
}
