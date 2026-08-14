using System;

namespace Concentus.Celt
{
	// Token: 0x02000085 RID: 133
	internal static class CeltConstants
	{
		// Token: 0x04000436 RID: 1078
		internal const int Q15ONE = 32767;

		// Token: 0x04000437 RID: 1079
		internal const float CELT_SIG_SCALE = 32768f;

		// Token: 0x04000438 RID: 1080
		internal const int SIG_SHIFT = 12;

		// Token: 0x04000439 RID: 1081
		internal const int NORM_SCALING = 16384;

		// Token: 0x0400043A RID: 1082
		internal const int DB_SHIFT = 10;

		// Token: 0x0400043B RID: 1083
		internal const int EPSILON = 1;

		// Token: 0x0400043C RID: 1084
		internal const int VERY_SMALL = 0;

		// Token: 0x0400043D RID: 1085
		internal const short VERY_LARGE16 = 32767;

		// Token: 0x0400043E RID: 1086
		internal const short Q15_ONE = 32767;

		// Token: 0x0400043F RID: 1087
		internal const int COMBFILTER_MAXPERIOD = 1024;

		// Token: 0x04000440 RID: 1088
		internal const int COMBFILTER_MINPERIOD = 15;

		// Token: 0x04000441 RID: 1089
		internal const int DECODE_BUFFER_SIZE = 2048;

		// Token: 0x04000442 RID: 1090
		internal const int BITALLOC_SIZE = 11;

		// Token: 0x04000443 RID: 1091
		internal const int MAX_PERIOD = 1024;

		// Token: 0x04000444 RID: 1092
		internal const int TOTAL_MODES = 1;

		// Token: 0x04000445 RID: 1093
		internal const int MAX_PSEUDO = 40;

		// Token: 0x04000446 RID: 1094
		internal const int LOG_MAX_PSEUDO = 6;

		// Token: 0x04000447 RID: 1095
		internal const int CELT_MAX_PULSES = 128;

		// Token: 0x04000448 RID: 1096
		internal const int MAX_FINE_BITS = 8;

		// Token: 0x04000449 RID: 1097
		internal const int FINE_OFFSET = 21;

		// Token: 0x0400044A RID: 1098
		internal const int QTHETA_OFFSET = 4;

		// Token: 0x0400044B RID: 1099
		internal const int QTHETA_OFFSET_TWOPHASE = 16;

		// Token: 0x0400044C RID: 1100
		internal const int PLC_PITCH_LAG_MAX = 720;

		// Token: 0x0400044D RID: 1101
		internal const int PLC_PITCH_LAG_MIN = 100;

		// Token: 0x0400044E RID: 1102
		internal const int LPC_ORDER = 24;
	}
}
