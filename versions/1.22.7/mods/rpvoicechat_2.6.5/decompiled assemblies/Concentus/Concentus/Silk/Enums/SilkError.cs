using System;

namespace Concentus.Silk.Enums
{
	// Token: 0x02000057 RID: 87
	internal static class SilkError
	{
		// Token: 0x040002A3 RID: 675
		internal static int SILK_NO_ERROR = 0;

		// Token: 0x040002A4 RID: 676
		internal static int SILK_ENC_INPUT_INVALID_NO_OF_SAMPLES = -101;

		// Token: 0x040002A5 RID: 677
		internal static int SILK_ENC_FS_NOT_SUPPORTED = -102;

		// Token: 0x040002A6 RID: 678
		internal static int SILK_ENC_PACKET_SIZE_NOT_SUPPORTED = -103;

		// Token: 0x040002A7 RID: 679
		internal static int SILK_ENC_PAYLOAD_BUF_TOO_SHORT = -104;

		// Token: 0x040002A8 RID: 680
		internal static int SILK_ENC_INVALID_LOSS_RATE = -105;

		// Token: 0x040002A9 RID: 681
		internal static int SILK_ENC_INVALID_COMPLEXITY_SETTING = -106;

		// Token: 0x040002AA RID: 682
		internal static int SILK_ENC_INVALID_INBAND_FEC_SETTING = -107;

		// Token: 0x040002AB RID: 683
		internal static int SILK_ENC_INVALID_DTX_SETTING = -108;

		// Token: 0x040002AC RID: 684
		internal static int SILK_ENC_INVALID_CBR_SETTING = -109;

		// Token: 0x040002AD RID: 685
		internal static int SILK_ENC_INTERNAL_ERROR = -110;

		// Token: 0x040002AE RID: 686
		internal static int SILK_ENC_INVALID_NUMBER_OF_CHANNELS_ERROR = -111;

		// Token: 0x040002AF RID: 687
		internal static int SILK_DEC_INVALID_SAMPLING_FREQUENCY = -200;

		// Token: 0x040002B0 RID: 688
		internal static int SILK_DEC_PAYLOAD_TOO_LARGE = -201;

		// Token: 0x040002B1 RID: 689
		internal static int SILK_DEC_PAYLOAD_ERROR = -202;

		// Token: 0x040002B2 RID: 690
		internal static int SILK_DEC_INVALID_FRAME_SIZE = -203;
	}
}
