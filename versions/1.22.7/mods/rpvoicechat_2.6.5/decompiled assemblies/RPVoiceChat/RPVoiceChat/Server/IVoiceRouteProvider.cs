using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Server
{
	// Token: 0x0200005E RID: 94
	public interface IVoiceRouteProvider
	{
		// Token: 0x0600026E RID: 622
		bool TryGetRoute(string playerUid, out Vec3d emissionPos, out int rangeBlocks);
	}
}
