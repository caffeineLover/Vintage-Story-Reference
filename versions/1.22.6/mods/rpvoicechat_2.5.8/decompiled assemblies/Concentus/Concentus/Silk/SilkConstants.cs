using System;

namespace Concentus.Silk
{
	// Token: 0x02000039 RID: 57
	internal static class SilkConstants
	{
		// Token: 0x0400005B RID: 91
		internal const int ENCODER_NUM_CHANNELS = 2;

		// Token: 0x0400005C RID: 92
		internal const int DECODER_NUM_CHANNELS = 2;

		// Token: 0x0400005D RID: 93
		internal const int MAX_FRAMES_PER_PACKET = 3;

		// Token: 0x0400005E RID: 94
		internal const int MIN_TARGET_RATE_BPS = 5000;

		// Token: 0x0400005F RID: 95
		internal const int MAX_TARGET_RATE_BPS = 80000;

		// Token: 0x04000060 RID: 96
		internal const int TARGET_RATE_TAB_SZ = 8;

		// Token: 0x04000061 RID: 97
		internal const int LBRR_NB_MIN_RATE_BPS = 12000;

		// Token: 0x04000062 RID: 98
		internal const int LBRR_MB_MIN_RATE_BPS = 14000;

		// Token: 0x04000063 RID: 99
		internal const int LBRR_WB_MIN_RATE_BPS = 16000;

		// Token: 0x04000064 RID: 100
		internal const int NB_SPEECH_FRAMES_BEFORE_DTX = 10;

		// Token: 0x04000065 RID: 101
		internal const int MAX_CONSECUTIVE_DTX = 20;

		// Token: 0x04000066 RID: 102
		internal const int MAX_FS_KHZ = 16;

		// Token: 0x04000067 RID: 103
		internal const int MAX_API_FS_KHZ = 48;

		// Token: 0x04000068 RID: 104
		internal const int TYPE_NO_VOICE_ACTIVITY = 0;

		// Token: 0x04000069 RID: 105
		internal const int TYPE_UNVOICED = 1;

		// Token: 0x0400006A RID: 106
		internal const int TYPE_VOICED = 2;

		// Token: 0x0400006B RID: 107
		internal const int CODE_INDEPENDENTLY = 0;

		// Token: 0x0400006C RID: 108
		internal const int CODE_INDEPENDENTLY_NO_LTP_SCALING = 1;

		// Token: 0x0400006D RID: 109
		internal const int CODE_CONDITIONALLY = 2;

		// Token: 0x0400006E RID: 110
		internal const int STEREO_QUANT_TAB_SIZE = 16;

		// Token: 0x0400006F RID: 111
		internal const int STEREO_QUANT_SUB_STEPS = 5;

		// Token: 0x04000070 RID: 112
		internal const int STEREO_INTERP_LEN_MS = 8;

		// Token: 0x04000071 RID: 113
		internal const float STEREO_RATIO_SMOOTH_COEF = 0.01f;

		// Token: 0x04000072 RID: 114
		internal const int PITCH_EST_MIN_LAG_MS = 2;

		// Token: 0x04000073 RID: 115
		internal const int PITCH_EST_MAX_LAG_MS = 18;

		// Token: 0x04000074 RID: 116
		internal const int MAX_NB_SUBFR = 4;

		// Token: 0x04000075 RID: 117
		internal const int LTP_MEM_LENGTH_MS = 20;

		// Token: 0x04000076 RID: 118
		internal const int SUB_FRAME_LENGTH_MS = 5;

		// Token: 0x04000077 RID: 119
		internal const int MAX_SUB_FRAME_LENGTH = 80;

		// Token: 0x04000078 RID: 120
		internal const int MAX_FRAME_LENGTH_MS = 20;

		// Token: 0x04000079 RID: 121
		internal const int MAX_FRAME_LENGTH = 320;

		// Token: 0x0400007A RID: 122
		internal const int LA_PITCH_MS = 2;

		// Token: 0x0400007B RID: 123
		internal const int LA_PITCH_MAX = 32;

		// Token: 0x0400007C RID: 124
		internal const int MAX_FIND_PITCH_LPC_ORDER = 16;

		// Token: 0x0400007D RID: 125
		internal const int FIND_PITCH_LPC_WIN_MS = 24;

		// Token: 0x0400007E RID: 126
		internal const int FIND_PITCH_LPC_WIN_MS_2_SF = 14;

		// Token: 0x0400007F RID: 127
		internal const int FIND_PITCH_LPC_WIN_MAX = 384;

		// Token: 0x04000080 RID: 128
		internal const int LA_SHAPE_MS = 5;

		// Token: 0x04000081 RID: 129
		internal const int LA_SHAPE_MAX = 80;

		// Token: 0x04000082 RID: 130
		internal const int SHAPE_LPC_WIN_MAX = 240;

		// Token: 0x04000083 RID: 131
		internal const int MIN_QGAIN_DB = 2;

		// Token: 0x04000084 RID: 132
		internal const int MAX_QGAIN_DB = 88;

		// Token: 0x04000085 RID: 133
		internal const int N_LEVELS_QGAIN = 64;

		// Token: 0x04000086 RID: 134
		internal const int MAX_DELTA_GAIN_QUANT = 36;

		// Token: 0x04000087 RID: 135
		internal const int MIN_DELTA_GAIN_QUANT = -4;

		// Token: 0x04000088 RID: 136
		internal const short OFFSET_VL_Q10 = 32;

		// Token: 0x04000089 RID: 137
		internal const short OFFSET_VH_Q10 = 100;

		// Token: 0x0400008A RID: 138
		internal const short OFFSET_UVL_Q10 = 100;

		// Token: 0x0400008B RID: 139
		internal const short OFFSET_UVH_Q10 = 240;

		// Token: 0x0400008C RID: 140
		internal const int QUANT_LEVEL_ADJUST_Q10 = 80;

		// Token: 0x0400008D RID: 141
		internal const int MAX_LPC_STABILIZE_ITERATIONS = 16;

		// Token: 0x0400008E RID: 142
		internal const float MAX_PREDICTION_POWER_GAIN = 10000f;

		// Token: 0x0400008F RID: 143
		internal const float MAX_PREDICTION_POWER_GAIN_AFTER_RESET = 100f;

		// Token: 0x04000090 RID: 144
		internal const int SILK_MAX_ORDER_LPC = 16;

		// Token: 0x04000091 RID: 145
		internal const int MAX_LPC_ORDER = 16;

		// Token: 0x04000092 RID: 146
		internal const int MIN_LPC_ORDER = 10;

		// Token: 0x04000093 RID: 147
		internal const int LTP_ORDER = 5;

		// Token: 0x04000094 RID: 148
		internal const int NB_LTP_CBKS = 3;

		// Token: 0x04000095 RID: 149
		internal const int USE_HARM_SHAPING = 1;

		// Token: 0x04000096 RID: 150
		internal const int MAX_SHAPE_LPC_ORDER = 16;

		// Token: 0x04000097 RID: 151
		internal const int HARM_SHAPE_FIR_TAPS = 3;

		// Token: 0x04000098 RID: 152
		internal const int MAX_DEL_DEC_STATES = 4;

		// Token: 0x04000099 RID: 153
		internal const int LTP_BUF_LENGTH = 512;

		// Token: 0x0400009A RID: 154
		internal const int LTP_MASK = 511;

		// Token: 0x0400009B RID: 155
		internal const int DECISION_DELAY = 32;

		// Token: 0x0400009C RID: 156
		internal const int DECISION_DELAY_MASK = 31;

		// Token: 0x0400009D RID: 157
		internal const int SHELL_CODEC_FRAME_LENGTH = 16;

		// Token: 0x0400009E RID: 158
		internal const int LOG2_SHELL_CODEC_FRAME_LENGTH = 4;

		// Token: 0x0400009F RID: 159
		internal const int MAX_NB_SHELL_BLOCKS = 20;

		// Token: 0x040000A0 RID: 160
		internal const int N_RATE_LEVELS = 10;

		// Token: 0x040000A1 RID: 161
		internal const int SILK_MAX_PULSES = 16;

		// Token: 0x040000A2 RID: 162
		internal const int MAX_MATRIX_SIZE = 16;

		// Token: 0x040000A3 RID: 163
		internal static readonly int NSQ_LPC_BUF_LENGTH = Math.Max(16, 32);

		// Token: 0x040000A4 RID: 164
		internal const int VAD_N_BANDS = 4;

		// Token: 0x040000A5 RID: 165
		internal const int VAD_INTERNAL_SUBFRAMES_LOG2 = 2;

		// Token: 0x040000A6 RID: 166
		internal const int VAD_INTERNAL_SUBFRAMES = 4;

		// Token: 0x040000A7 RID: 167
		internal const int VAD_NOISE_LEVEL_SMOOTH_COEF_Q16 = 1024;

		// Token: 0x040000A8 RID: 168
		internal const int VAD_NOISE_LEVELS_BIAS = 50;

		// Token: 0x040000A9 RID: 169
		internal const int VAD_NEGATIVE_OFFSET_Q5 = 128;

		// Token: 0x040000AA RID: 170
		internal const int VAD_SNR_FACTOR_Q16 = 45000;

		// Token: 0x040000AB RID: 171
		internal const int VAD_SNR_SMOOTH_COEF_Q18 = 4096;

		// Token: 0x040000AC RID: 172
		internal const int LSF_COS_TAB_SZ = 128;

		// Token: 0x040000AD RID: 173
		internal const int NLSF_W_Q = 2;

		// Token: 0x040000AE RID: 174
		internal const int NLSF_VQ_MAX_VECTORS = 32;

		// Token: 0x040000AF RID: 175
		internal const int NLSF_VQ_MAX_SURVIVORS = 32;

		// Token: 0x040000B0 RID: 176
		internal const int NLSF_QUANT_MAX_AMPLITUDE = 4;

		// Token: 0x040000B1 RID: 177
		internal const int NLSF_QUANT_MAX_AMPLITUDE_EXT = 10;

		// Token: 0x040000B2 RID: 178
		internal const float NLSF_QUANT_LEVEL_ADJ = 0.1f;

		// Token: 0x040000B3 RID: 179
		internal const int NLSF_QUANT_DEL_DEC_STATES_LOG2 = 2;

		// Token: 0x040000B4 RID: 180
		internal const int NLSF_QUANT_DEL_DEC_STATES = 4;

		// Token: 0x040000B5 RID: 181
		internal const int TRANSITION_TIME_MS = 5120;

		// Token: 0x040000B6 RID: 182
		internal const int TRANSITION_NB = 3;

		// Token: 0x040000B7 RID: 183
		internal const int TRANSITION_NA = 2;

		// Token: 0x040000B8 RID: 184
		internal const int TRANSITION_INT_NUM = 5;

		// Token: 0x040000B9 RID: 185
		internal const int TRANSITION_FRAMES = 256;

		// Token: 0x040000BA RID: 186
		internal const int TRANSITION_INT_STEPS = 64;

		// Token: 0x040000BB RID: 187
		internal const int BWE_AFTER_LOSS_Q16 = 63570;

		// Token: 0x040000BC RID: 188
		internal const int CNG_BUF_MASK_MAX = 255;

		// Token: 0x040000BD RID: 189
		internal const int CNG_GAIN_SMTH_Q16 = 4634;

		// Token: 0x040000BE RID: 190
		internal const int CNG_NLSF_SMTH_Q16 = 16348;

		// Token: 0x040000BF RID: 191
		internal const int PE_MAX_FS_KHZ = 16;

		// Token: 0x040000C0 RID: 192
		internal const int PE_MAX_NB_SUBFR = 4;

		// Token: 0x040000C1 RID: 193
		internal const int PE_SUBFR_LENGTH_MS = 5;

		// Token: 0x040000C2 RID: 194
		internal const int PE_LTP_MEM_LENGTH_MS = 20;

		// Token: 0x040000C3 RID: 195
		internal const int PE_MAX_FRAME_LENGTH_MS = 40;

		// Token: 0x040000C4 RID: 196
		internal const int PE_MAX_FRAME_LENGTH = 640;

		// Token: 0x040000C5 RID: 197
		internal const int PE_MAX_FRAME_LENGTH_ST_1 = 160;

		// Token: 0x040000C6 RID: 198
		internal const int PE_MAX_FRAME_LENGTH_ST_2 = 320;

		// Token: 0x040000C7 RID: 199
		internal const int PE_MAX_LAG_MS = 18;

		// Token: 0x040000C8 RID: 200
		internal const int PE_MIN_LAG_MS = 2;

		// Token: 0x040000C9 RID: 201
		internal const int PE_MAX_LAG = 288;

		// Token: 0x040000CA RID: 202
		internal const int PE_MIN_LAG = 32;

		// Token: 0x040000CB RID: 203
		internal const int PE_D_SRCH_LENGTH = 24;

		// Token: 0x040000CC RID: 204
		internal const int PE_NB_STAGE3_LAGS = 5;

		// Token: 0x040000CD RID: 205
		internal const int PE_NB_CBKS_STAGE2 = 3;

		// Token: 0x040000CE RID: 206
		internal const int PE_NB_CBKS_STAGE2_EXT = 11;

		// Token: 0x040000CF RID: 207
		internal const int PE_NB_CBKS_STAGE3_MAX = 34;

		// Token: 0x040000D0 RID: 208
		internal const int PE_NB_CBKS_STAGE3_MID = 24;

		// Token: 0x040000D1 RID: 209
		internal const int PE_NB_CBKS_STAGE3_MIN = 16;

		// Token: 0x040000D2 RID: 210
		internal const int PE_NB_CBKS_STAGE3_10MS = 12;

		// Token: 0x040000D3 RID: 211
		internal const int PE_NB_CBKS_STAGE2_10MS = 3;

		// Token: 0x040000D4 RID: 212
		internal const float PE_SHORTLAG_BIAS = 0.2f;

		// Token: 0x040000D5 RID: 213
		internal const float PE_PREVLAG_BIAS = 0.2f;

		// Token: 0x040000D6 RID: 214
		internal const float PE_FLATCONTOUR_BIAS = 0.05f;

		// Token: 0x040000D7 RID: 215
		internal const int SILK_PE_MIN_COMPLEX = 0;

		// Token: 0x040000D8 RID: 216
		internal const int SILK_PE_MID_COMPLEX = 1;

		// Token: 0x040000D9 RID: 217
		internal const int SILK_PE_MAX_COMPLEX = 2;

		// Token: 0x040000DA RID: 218
		internal const float BWE_COEF = 0.99f;

		// Token: 0x040000DB RID: 219
		internal const int V_PITCH_GAIN_START_MIN_Q14 = 11469;

		// Token: 0x040000DC RID: 220
		internal const int V_PITCH_GAIN_START_MAX_Q14 = 15565;

		// Token: 0x040000DD RID: 221
		internal const int MAX_PITCH_LAG_MS = 18;

		// Token: 0x040000DE RID: 222
		internal const int RAND_BUF_SIZE = 128;

		// Token: 0x040000DF RID: 223
		internal const int RAND_BUF_MASK = 127;

		// Token: 0x040000E0 RID: 224
		internal const int LOG2_INV_LPC_GAIN_HIGH_THRES = 3;

		// Token: 0x040000E1 RID: 225
		internal const int LOG2_INV_LPC_GAIN_LOW_THRES = 8;

		// Token: 0x040000E2 RID: 226
		internal const int PITCH_DRIFT_FAC_Q16 = 655;

		// Token: 0x040000E3 RID: 227
		internal const int SILK_RESAMPLER_MAX_FIR_ORDER = 36;

		// Token: 0x040000E4 RID: 228
		internal const int SILK_RESAMPLER_MAX_IIR_ORDER = 6;

		// Token: 0x040000E5 RID: 229
		internal const int RESAMPLER_DOWN_ORDER_FIR0 = 18;

		// Token: 0x040000E6 RID: 230
		internal const int RESAMPLER_DOWN_ORDER_FIR1 = 24;

		// Token: 0x040000E7 RID: 231
		internal const int RESAMPLER_DOWN_ORDER_FIR2 = 36;

		// Token: 0x040000E8 RID: 232
		internal const int RESAMPLER_ORDER_FIR_12 = 8;

		// Token: 0x040000E9 RID: 233
		internal const int RESAMPLER_MAX_BATCH_SIZE_MS = 10;

		// Token: 0x040000EA RID: 234
		internal const int RESAMPLER_MAX_FS_KHZ = 48;

		// Token: 0x040000EB RID: 235
		internal const int RESAMPLER_MAX_BATCH_SIZE_IN = 480;

		// Token: 0x040000EC RID: 236
		internal const int SILK_MAX_FRAMES_PER_PACKET = 3;
	}
}
