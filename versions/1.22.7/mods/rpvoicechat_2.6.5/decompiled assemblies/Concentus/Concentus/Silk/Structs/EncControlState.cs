using System;
using Concentus.Silk.Enums;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000043 RID: 67
	internal class EncControlState
	{
		// Token: 0x06000136 RID: 310 RVA: 0x000142C8 File Offset: 0x000124C8
		internal void Reset()
		{
			this.nChannelsAPI = 0;
			this.nChannelsInternal = 0;
			this.API_sampleRate = 0;
			this.maxInternalSampleRate = 0;
			this.minInternalSampleRate = 0;
			this.desiredInternalSampleRate = 0;
			this.payloadSize_ms = 0;
			this.bitRate = 0;
			this.packetLossPercentage = 0;
			this.complexity = 0;
			this.useInBandFEC = 0;
			this.useDTX = 0;
			this.useCBR = 0;
			this.maxBits = 0;
			this.toMono = 0;
			this.opusCanSwitch = 0;
			this.reducedDependency = 0;
			this.internalSampleRate = 0;
			this.allowBandwidthSwitch = 0;
			this.inWBmodeWithoutVariableLP = 0;
			this.stereoWidth_Q14 = 0;
			this.switchReady = 0;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00014370 File Offset: 0x00012570
		internal int check_control_input()
		{
			if ((this.API_sampleRate != 8000 && this.API_sampleRate != 12000 && this.API_sampleRate != 16000 && this.API_sampleRate != 24000 && this.API_sampleRate != 32000 && this.API_sampleRate != 44100 && this.API_sampleRate != 48000) || (this.desiredInternalSampleRate != 8000 && this.desiredInternalSampleRate != 12000 && this.desiredInternalSampleRate != 16000) || (this.maxInternalSampleRate != 8000 && this.maxInternalSampleRate != 12000 && this.maxInternalSampleRate != 16000) || (this.minInternalSampleRate != 8000 && this.minInternalSampleRate != 12000 && this.minInternalSampleRate != 16000) || this.minInternalSampleRate > this.desiredInternalSampleRate || this.maxInternalSampleRate < this.desiredInternalSampleRate || this.minInternalSampleRate > this.maxInternalSampleRate)
			{
				return SilkError.SILK_ENC_FS_NOT_SUPPORTED;
			}
			if (this.payloadSize_ms != 10 && this.payloadSize_ms != 20 && this.payloadSize_ms != 40 && this.payloadSize_ms != 60)
			{
				return SilkError.SILK_ENC_PACKET_SIZE_NOT_SUPPORTED;
			}
			if (this.packetLossPercentage < 0 || this.packetLossPercentage > 100)
			{
				return SilkError.SILK_ENC_INVALID_LOSS_RATE;
			}
			if (this.useDTX < 0 || this.useDTX > 1)
			{
				return SilkError.SILK_ENC_INVALID_DTX_SETTING;
			}
			if (this.useCBR < 0 || this.useCBR > 1)
			{
				return SilkError.SILK_ENC_INVALID_CBR_SETTING;
			}
			if (this.useInBandFEC < 0 || this.useInBandFEC > 1)
			{
				return SilkError.SILK_ENC_INVALID_INBAND_FEC_SETTING;
			}
			if (this.nChannelsAPI < 1 || this.nChannelsAPI > 2)
			{
				return SilkError.SILK_ENC_INVALID_NUMBER_OF_CHANNELS_ERROR;
			}
			if (this.nChannelsInternal < 1 || this.nChannelsInternal > 2)
			{
				return SilkError.SILK_ENC_INVALID_NUMBER_OF_CHANNELS_ERROR;
			}
			if (this.nChannelsInternal > this.nChannelsAPI)
			{
				return SilkError.SILK_ENC_INVALID_NUMBER_OF_CHANNELS_ERROR;
			}
			if (this.complexity < 0 || this.complexity > 10)
			{
				return SilkError.SILK_ENC_INVALID_COMPLEXITY_SETTING;
			}
			return SilkError.SILK_NO_ERROR;
		}

		// Token: 0x0400018C RID: 396
		internal int nChannelsAPI;

		// Token: 0x0400018D RID: 397
		internal int nChannelsInternal;

		// Token: 0x0400018E RID: 398
		internal int API_sampleRate;

		// Token: 0x0400018F RID: 399
		internal int maxInternalSampleRate;

		// Token: 0x04000190 RID: 400
		internal int minInternalSampleRate;

		// Token: 0x04000191 RID: 401
		internal int desiredInternalSampleRate;

		// Token: 0x04000192 RID: 402
		internal int payloadSize_ms;

		// Token: 0x04000193 RID: 403
		internal int bitRate;

		// Token: 0x04000194 RID: 404
		internal int packetLossPercentage;

		// Token: 0x04000195 RID: 405
		internal int complexity;

		// Token: 0x04000196 RID: 406
		internal int useInBandFEC;

		// Token: 0x04000197 RID: 407
		internal int useDTX;

		// Token: 0x04000198 RID: 408
		internal int useCBR;

		// Token: 0x04000199 RID: 409
		internal int maxBits;

		// Token: 0x0400019A RID: 410
		internal int toMono;

		// Token: 0x0400019B RID: 411
		internal int opusCanSwitch;

		// Token: 0x0400019C RID: 412
		internal int reducedDependency;

		// Token: 0x0400019D RID: 413
		internal int internalSampleRate;

		// Token: 0x0400019E RID: 414
		internal int allowBandwidthSwitch;

		// Token: 0x0400019F RID: 415
		internal int inWBmodeWithoutVariableLP;

		// Token: 0x040001A0 RID: 416
		internal int stereoWidth_Q14;

		// Token: 0x040001A1 RID: 417
		internal int switchReady;
	}
}
