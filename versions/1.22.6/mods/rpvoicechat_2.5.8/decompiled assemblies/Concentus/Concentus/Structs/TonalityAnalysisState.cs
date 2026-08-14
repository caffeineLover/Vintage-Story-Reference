using System;
using Concentus.Celt.Structs;
using Concentus.Common.CPlusPlus;

namespace Concentus.Structs
{
	// Token: 0x02000061 RID: 97
	internal class TonalityAnalysisState
	{
		// Token: 0x06000246 RID: 582 RVA: 0x0001FA2C File Offset: 0x0001DC2C
		internal TonalityAnalysisState()
		{
			for (int i = 0; i < 200; i++)
			{
				this.info[i] = new AnalysisInfo();
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0001FB40 File Offset: 0x0001DD40
		internal void Reset()
		{
			Arrays.MemSetFloat(this.angle, 0f, 240);
			Arrays.MemSetFloat(this.d_angle, 0f, 240);
			Arrays.MemSetFloat(this.d2_angle, 0f, 240);
			Arrays.MemSetInt(this.inmem, 0, 720);
			this.mem_fill = 0;
			Arrays.MemSetFloat(this.prev_band_tonality, 0f, 18);
			this.prev_tonality = 0f;
			for (int i = 0; i < 8; i++)
			{
				Arrays.MemSetFloat(this.E[i], 0f, 18);
			}
			Arrays.MemSetFloat(this.lowE, 0f, 18);
			Arrays.MemSetFloat(this.highE, 0f, 18);
			Arrays.MemSetFloat(this.meanE, 0f, 21);
			Arrays.MemSetFloat(this.mem, 0f, 32);
			Arrays.MemSetFloat(this.cmean, 0f, 8);
			Arrays.MemSetFloat(this.std, 0f, 9);
			this.music_prob = 0f;
			this.Etracker = 0f;
			this.lowECount = 0f;
			this.E_count = 0;
			this.last_music = 0;
			this.last_transition = 0;
			this.count = 0;
			Arrays.MemSetFloat(this.subframe_mem, 0f, 3);
			this.analysis_offset = 0;
			Arrays.MemSetFloat(this.pspeech, 0f, 200);
			Arrays.MemSetFloat(this.pmusic, 0f, 200);
			this.speech_confidence = 0f;
			this.music_confidence = 0f;
			this.speech_confidence_count = 0;
			this.music_confidence_count = 0;
			this.write_pos = 0;
			this.read_pos = 0;
			this.read_subframe = 0;
			for (int j = 0; j < 200; j++)
			{
				this.info[j].Reset();
			}
		}

		// Token: 0x0400030E RID: 782
		internal bool enabled;

		// Token: 0x0400030F RID: 783
		internal readonly float[] angle = new float[240];

		// Token: 0x04000310 RID: 784
		internal readonly float[] d_angle = new float[240];

		// Token: 0x04000311 RID: 785
		internal readonly float[] d2_angle = new float[240];

		// Token: 0x04000312 RID: 786
		internal readonly int[] inmem = new int[720];

		// Token: 0x04000313 RID: 787
		internal int mem_fill;

		// Token: 0x04000314 RID: 788
		internal readonly float[] prev_band_tonality = new float[18];

		// Token: 0x04000315 RID: 789
		internal float prev_tonality;

		// Token: 0x04000316 RID: 790
		internal readonly float[][] E = Arrays.InitTwoDimensionalArray<float>(8, 18);

		// Token: 0x04000317 RID: 791
		internal readonly float[] lowE = new float[18];

		// Token: 0x04000318 RID: 792
		internal readonly float[] highE = new float[18];

		// Token: 0x04000319 RID: 793
		internal readonly float[] meanE = new float[21];

		// Token: 0x0400031A RID: 794
		internal readonly float[] mem = new float[32];

		// Token: 0x0400031B RID: 795
		internal readonly float[] cmean = new float[8];

		// Token: 0x0400031C RID: 796
		internal readonly float[] std = new float[9];

		// Token: 0x0400031D RID: 797
		internal float music_prob;

		// Token: 0x0400031E RID: 798
		internal float Etracker;

		// Token: 0x0400031F RID: 799
		internal float lowECount;

		// Token: 0x04000320 RID: 800
		internal int E_count;

		// Token: 0x04000321 RID: 801
		internal int last_music;

		// Token: 0x04000322 RID: 802
		internal int last_transition;

		// Token: 0x04000323 RID: 803
		internal int count;

		// Token: 0x04000324 RID: 804
		internal readonly float[] subframe_mem = new float[3];

		// Token: 0x04000325 RID: 805
		internal int analysis_offset;

		// Token: 0x04000326 RID: 806
		internal readonly float[] pspeech = new float[200];

		// Token: 0x04000327 RID: 807
		internal readonly float[] pmusic = new float[200];

		// Token: 0x04000328 RID: 808
		internal float speech_confidence;

		// Token: 0x04000329 RID: 809
		internal float music_confidence;

		// Token: 0x0400032A RID: 810
		internal int speech_confidence_count;

		// Token: 0x0400032B RID: 811
		internal int music_confidence_count;

		// Token: 0x0400032C RID: 812
		internal int write_pos;

		// Token: 0x0400032D RID: 813
		internal int read_pos;

		// Token: 0x0400032E RID: 814
		internal int read_subframe;

		// Token: 0x0400032F RID: 815
		internal readonly AnalysisInfo[] info = new AnalysisInfo[200];
	}
}
