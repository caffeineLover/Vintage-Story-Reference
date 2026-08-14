using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Util
{
	// Token: 0x02000027 RID: 39
	public static class BlockPosExtensions
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00007443 File Offset: 0x00005643
		public static Vec3d ToWorldCenter(this BlockPos pos)
		{
			return new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5);
		}
	}
}
