using System;
using System.Collections.Generic;

namespace RPVoiceChat.Server
{
	// Token: 0x0200005F RID: 95
	public interface IVoiceMultiRouteProvider
	{
		// Token: 0x0600026F RID: 623
		bool TryGetRoutes(string playerUid, out IReadOnlyList<VoiceRoute> routes);
	}
}
