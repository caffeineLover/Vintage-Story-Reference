using System;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000080 RID: 128
	public enum RadioSettingsOperation
	{
		// Token: 0x04000195 RID: 405
		SetFrequency,
		// Token: 0x04000196 RID: 406
		SetDisplayName,
		// Token: 0x04000197 RID: 407
		SetEmitterMode,
		// Token: 0x04000198 RID: 408
		SetReceiverFrequency,
		// Token: 0x04000199 RID: 409
		SetRepeaterFrequency,
		// Token: 0x0400019A RID: 410
		SetMicrophoneTransmit,
		// Token: 0x0400019B RID: 411
		SetMixingConsoleHlsUrl,
		// Token: 0x0400019C RID: 412
		SetMixingConsoleOnAir,
		// Token: 0x0400019D RID: 413
		SetReceiverEnabled,
		// Token: 0x0400019E RID: 414
		SetReceiverPlaybackRange,
		// Token: 0x0400019F RID: 415
		SetReceiverPlaybackVolume
	}
}
