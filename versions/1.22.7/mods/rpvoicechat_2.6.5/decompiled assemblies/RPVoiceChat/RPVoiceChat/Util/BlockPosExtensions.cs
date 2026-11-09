using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Util
{
	// Token: 0x02000027 RID: 39
	public static class BlockPosExtensions
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00007C83 File Offset: 0x00005E83
		public static Vec3d ToWorldCenter(this BlockPos pos)
		{
			return new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5);
		}
	}
}
