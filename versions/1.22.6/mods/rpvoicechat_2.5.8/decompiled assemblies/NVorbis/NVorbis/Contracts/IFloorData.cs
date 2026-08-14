using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000027 RID: 39
	internal interface IFloorData
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001B6 RID: 438
		bool ExecuteChannel { get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001B7 RID: 439
		// (set) Token: 0x060001B8 RID: 440
		bool ForceEnergy { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001B9 RID: 441
		// (set) Token: 0x060001BA RID: 442
		bool ForceNoEnergy { get; set; }
	}
}
