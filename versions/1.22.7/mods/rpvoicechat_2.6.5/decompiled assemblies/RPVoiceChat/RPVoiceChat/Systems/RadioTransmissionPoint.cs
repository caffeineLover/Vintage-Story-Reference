using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000045 RID: 69
	public readonly struct RadioTransmissionPoint
	{
		// Token: 0x06000192 RID: 402 RVA: 0x0000B6EF File Offset: 0x000098EF
		public RadioTransmissionPoint(Vec3d position, int rangeBlocks, string frequency, int dimension, bool isRepeaterRelay)
		{
			this.Position = position;
			this.RangeBlocks = rangeBlocks;
			this.Frequency = frequency;
			this.Dimension = dimension;
			this.IsRepeaterRelay = isRepeaterRelay;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000B716 File Offset: 0x00009916
		public Vec3d Position { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000B71E File Offset: 0x0000991E
		public int RangeBlocks { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000B726 File Offset: 0x00009926
		public string Frequency { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000B72E File Offset: 0x0000992E
		public int Dimension { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000B736 File Offset: 0x00009936
		public bool IsRepeaterRelay { get; }
	}
}
