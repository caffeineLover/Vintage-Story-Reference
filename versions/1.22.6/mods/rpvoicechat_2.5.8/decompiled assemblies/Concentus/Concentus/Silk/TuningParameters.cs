using System;

namespace Concentus.Silk
{
	// Token: 0x0200003E RID: 62
	internal class TuningParameters
	{
		// Token: 0x04000152 RID: 338
		internal const int BITRESERVOIR_DECAY_TIME_MS = 500;

		// Token: 0x04000153 RID: 339
		internal const float FIND_PITCH_WHITE_NOISE_FRACTION = 0.001f;

		// Token: 0x04000154 RID: 340
		internal const float FIND_PITCH_BANDWIDTH_EXPANSION = 0.99f;

		// Token: 0x04000155 RID: 341
		internal const float FIND_LPC_COND_FAC = 1E-05f;

		// Token: 0x04000156 RID: 342
		internal const float FIND_LTP_COND_FAC = 1E-05f;

		// Token: 0x04000157 RID: 343
		internal const float LTP_DAMPING = 0.05f;

		// Token: 0x04000158 RID: 344
		internal const float LTP_SMOOTHING = 0.1f;

		// Token: 0x04000159 RID: 345
		internal const float MU_LTP_QUANT_NB = 0.03f;

		// Token: 0x0400015A RID: 346
		internal const float MU_LTP_QUANT_MB = 0.025f;

		// Token: 0x0400015B RID: 347
		internal const float MU_LTP_QUANT_WB = 0.02f;

		// Token: 0x0400015C RID: 348
		internal const float MAX_SUM_LOG_GAIN_DB = 250f;

		// Token: 0x0400015D RID: 349
		internal const float VARIABLE_HP_SMTH_COEF1 = 0.1f;

		// Token: 0x0400015E RID: 350
		internal const float VARIABLE_HP_SMTH_COEF2 = 0.015f;

		// Token: 0x0400015F RID: 351
		internal const float VARIABLE_HP_MAX_DELTA_FREQ = 0.4f;

		// Token: 0x04000160 RID: 352
		internal const int VARIABLE_HP_MIN_CUTOFF_HZ = 60;

		// Token: 0x04000161 RID: 353
		internal const int VARIABLE_HP_MAX_CUTOFF_HZ = 100;

		// Token: 0x04000162 RID: 354
		internal const float SPEECH_ACTIVITY_DTX_THRES = 0.05f;

		// Token: 0x04000163 RID: 355
		internal const float LBRR_SPEECH_ACTIVITY_THRES = 0.3f;

		// Token: 0x04000164 RID: 356
		internal const float BG_SNR_DECR_dB = 2f;

		// Token: 0x04000165 RID: 357
		internal const float HARM_SNR_INCR_dB = 2f;

		// Token: 0x04000166 RID: 358
		internal const float SPARSE_SNR_INCR_dB = 2f;

		// Token: 0x04000167 RID: 359
		internal const float SPARSENESS_THRESHOLD_QNT_OFFSET = 0.75f;

		// Token: 0x04000168 RID: 360
		internal const float WARPING_MULTIPLIER = 0.015f;

		// Token: 0x04000169 RID: 361
		internal const float SHAPE_WHITE_NOISE_FRACTION = 5E-05f;

		// Token: 0x0400016A RID: 362
		internal const float BANDWIDTH_EXPANSION = 0.95f;

		// Token: 0x0400016B RID: 363
		internal const float LOW_RATE_BANDWIDTH_EXPANSION_DELTA = 0.01f;

		// Token: 0x0400016C RID: 364
		internal const float LOW_RATE_HARMONIC_BOOST = 0.1f;

		// Token: 0x0400016D RID: 365
		internal const float LOW_INPUT_QUALITY_HARMONIC_BOOST = 0.1f;

		// Token: 0x0400016E RID: 366
		internal const float HARMONIC_SHAPING = 0.3f;

		// Token: 0x0400016F RID: 367
		internal const float HIGH_RATE_OR_LOW_QUALITY_HARMONIC_SHAPING = 0.2f;

		// Token: 0x04000170 RID: 368
		internal const float HP_NOISE_COEF = 0.25f;

		// Token: 0x04000171 RID: 369
		internal const float HARM_HP_NOISE_COEF = 0.35f;

		// Token: 0x04000172 RID: 370
		internal const float INPUT_TILT = 0.05f;

		// Token: 0x04000173 RID: 371
		internal const float HIGH_RATE_INPUT_TILT = 0.1f;

		// Token: 0x04000174 RID: 372
		internal const float LOW_FREQ_SHAPING = 4f;

		// Token: 0x04000175 RID: 373
		internal const float LOW_QUALITY_LOW_FREQ_SHAPING_DECR = 0.5f;

		// Token: 0x04000176 RID: 374
		internal const float SUBFR_SMTH_COEF = 0.4f;

		// Token: 0x04000177 RID: 375
		internal const float LAMBDA_OFFSET = 1.2f;

		// Token: 0x04000178 RID: 376
		internal const float LAMBDA_SPEECH_ACT = -0.2f;

		// Token: 0x04000179 RID: 377
		internal const float LAMBDA_DELAYED_DECISIONS = -0.05f;

		// Token: 0x0400017A RID: 378
		internal const float LAMBDA_INPUT_QUALITY = -0.1f;

		// Token: 0x0400017B RID: 379
		internal const float LAMBDA_CODING_QUALITY = -0.2f;

		// Token: 0x0400017C RID: 380
		internal const float LAMBDA_QUANT_OFFSET = 0.8f;

		// Token: 0x0400017D RID: 381
		internal const int REDUCE_BITRATE_10_MS_BPS = 2200;

		// Token: 0x0400017E RID: 382
		internal const int MAX_BANDWIDTH_SWITCH_DELAY_MS = 5000;
	}
}
