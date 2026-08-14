using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Server
{
	// Token: 0x02000043 RID: 67
	public interface IVoiceRouteProvider
	{
		// Token: 0x060001D5 RID: 469
		bool TryGetRoute(string playerUid, out Vec3d emissionPos, out int rangeBlocks);
	}
}
