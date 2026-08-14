using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Server
{
	// Token: 0x02000042 RID: 66
	public readonly struct VoiceRoute
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000C75C File Offset: 0x0000A95C
		public VoiceRoute(Vec3d emissionPos, int rangeBlocks)
		{
			this = new VoiceRoute(emissionPos, rangeBlocks, 0);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000C767 File Offset: 0x0000A967
		public VoiceRoute(Vec3d emissionPos, int rangeBlocks, int dimension)
		{
			this.EmissionPos = emissionPos;
			this.RangeBlocks = rangeBlocks;
			this.Dimension = dimension;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000C77E File Offset: 0x0000A97E
		public Vec3d EmissionPos { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000C786 File Offset: 0x0000A986
		public int RangeBlocks { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000C78E File Offset: 0x0000A98E
		public int Dimension { get; }
	}
}
