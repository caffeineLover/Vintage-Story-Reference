using System;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000051 RID: 81
	internal class SilkShapeState
	{
		// Token: 0x0600016D RID: 365 RVA: 0x00018A3F File Offset: 0x00016C3F
		internal void Reset()
		{
			this.LastGainIndex = 0;
			this.HarmBoost_smth_Q16 = 0;
			this.HarmShapeGain_smth_Q16 = 0;
			this.Tilt_smth_Q16 = 0;
		}

		// Token: 0x04000283 RID: 643
		internal sbyte LastGainIndex;

		// Token: 0x04000284 RID: 644
		internal int HarmBoost_smth_Q16;

		// Token: 0x04000285 RID: 645
		internal int HarmShapeGain_smth_Q16;

		// Token: 0x04000286 RID: 646
		internal int Tilt_smth_Q16;
	}
}
