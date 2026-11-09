using System;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Server
{
	// Token: 0x0200005D RID: 93
	public readonly struct VoiceRoute
	{
		// Token: 0x06000265 RID: 613 RVA: 0x000101D4 File Offset: 0x0000E3D4
		public VoiceRoute(Vec3d emissionPos, int rangeBlocks)
		{
			this = new VoiceRoute(emissionPos, rangeBlocks, 0, null, true);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000101E1 File Offset: 0x0000E3E1
		public VoiceRoute(Vec3d emissionPos, int rangeBlocks, int dimension)
		{
			this = new VoiceRoute(emissionPos, rangeBlocks, dimension, null, true);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000101EE File Offset: 0x0000E3EE
		public VoiceRoute(Vec3d emissionPos, int rangeBlocks, int dimension, string radioFrequency)
		{
			this = new VoiceRoute(emissionPos, rangeBlocks, dimension, radioFrequency, true);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000101FC File Offset: 0x0000E3FC
		public VoiceRoute(Vec3d emissionPos, int rangeBlocks, int dimension, string radioFrequency, bool acousticEmission)
		{
			this.EmissionPos = emissionPos;
			this.RangeBlocks = rangeBlocks;
			this.Dimension = dimension;
			this.RadioFrequency = radioFrequency;
			this.IsAcousticEmission = acousticEmission;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00010223 File Offset: 0x0000E423
		public Vec3d EmissionPos { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0001022B File Offset: 0x0000E42B
		public int RangeBlocks { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00010233 File Offset: 0x0000E433
		public int Dimension { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0001023B File Offset: 0x0000E43B
		public string RadioFrequency { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00010243 File Offset: 0x0000E443
		public bool IsAcousticEmission { get; }
	}
}
