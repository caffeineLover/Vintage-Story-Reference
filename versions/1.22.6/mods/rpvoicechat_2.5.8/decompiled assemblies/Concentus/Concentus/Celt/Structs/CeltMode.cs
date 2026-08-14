using System;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000095 RID: 149
	internal class CeltMode
	{
		// Token: 0x06000536 RID: 1334 RVA: 0x0003611C File Offset: 0x0003431C
		private CeltMode()
		{
		}

		// Token: 0x040004C6 RID: 1222
		internal int Fs;

		// Token: 0x040004C7 RID: 1223
		internal int overlap;

		// Token: 0x040004C8 RID: 1224
		internal int nbEBands;

		// Token: 0x040004C9 RID: 1225
		internal int effEBands;

		// Token: 0x040004CA RID: 1226
		internal int[] preemph = new int[4];

		// Token: 0x040004CB RID: 1227
		internal short[] eBands;

		// Token: 0x040004CC RID: 1228
		internal int maxLM;

		// Token: 0x040004CD RID: 1229
		internal int nbShortMdcts;

		// Token: 0x040004CE RID: 1230
		internal int shortMdctSize;

		// Token: 0x040004CF RID: 1231
		internal int nbAllocVectors;

		// Token: 0x040004D0 RID: 1232
		internal byte[] allocVectors;

		// Token: 0x040004D1 RID: 1233
		internal short[] logN;

		// Token: 0x040004D2 RID: 1234
		internal int[] window;

		// Token: 0x040004D3 RID: 1235
		internal MDCTLookup mdct = new MDCTLookup();

		// Token: 0x040004D4 RID: 1236
		internal PulseCache cache = new PulseCache();

		// Token: 0x040004D5 RID: 1237
		internal static readonly CeltMode mode48000_960_120 = new CeltMode
		{
			Fs = 48000,
			overlap = 120,
			nbEBands = 21,
			effEBands = 21,
			preemph = new int[]
			{
				27853,
				0,
				4096,
				8192
			},
			eBands = Concentus.Celt.Tables.eband5ms,
			maxLM = 3,
			nbShortMdcts = 8,
			shortMdctSize = 120,
			nbAllocVectors = 11,
			allocVectors = Concentus.Celt.Tables.band_allocation,
			logN = Concentus.Celt.Tables.logN400,
			window = Concentus.Celt.Tables.window120,
			mdct = new MDCTLookup
			{
				n = 1920,
				maxshift = 3,
				kfft = new FFTState[]
				{
					Concentus.Celt.Tables.fft_state48000_960_0,
					Concentus.Celt.Tables.fft_state48000_960_1,
					Concentus.Celt.Tables.fft_state48000_960_2,
					Concentus.Celt.Tables.fft_state48000_960_3
				},
				trig = Concentus.Celt.Tables.mdct_twiddles960
			},
			cache = new PulseCache
			{
				size = 392,
				index = Concentus.Celt.Tables.cache_index50,
				bits = Concentus.Celt.Tables.cache_bits50,
				caps = Concentus.Celt.Tables.cache_caps50
			}
		};
	}
}
