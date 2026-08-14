using System;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000092 RID: 146
	internal class AnalysisInfo
	{
		// Token: 0x06000508 RID: 1288 RVA: 0x00033075 File Offset: 0x00031275
		internal AnalysisInfo()
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00033080 File Offset: 0x00031280
		internal void Assign(AnalysisInfo other)
		{
			this.valid = other.valid;
			this.tonality = other.tonality;
			this.tonality_slope = other.tonality_slope;
			this.noisiness = other.noisiness;
			this.activity = other.activity;
			this.music_prob = other.music_prob;
			this.bandwidth = other.bandwidth;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000330E4 File Offset: 0x000312E4
		internal void Reset()
		{
			this.valid = 0;
			this.tonality = 0f;
			this.tonality_slope = 0f;
			this.noisiness = 0f;
			this.activity = 0f;
			this.music_prob = 0f;
			this.bandwidth = 0;
		}

		// Token: 0x04000478 RID: 1144
		internal int valid;

		// Token: 0x04000479 RID: 1145
		internal float tonality;

		// Token: 0x0400047A RID: 1146
		internal float tonality_slope;

		// Token: 0x0400047B RID: 1147
		internal float noisiness;

		// Token: 0x0400047C RID: 1148
		internal float activity;

		// Token: 0x0400047D RID: 1149
		internal float music_prob;

		// Token: 0x0400047E RID: 1150
		internal int bandwidth;
	}
}
