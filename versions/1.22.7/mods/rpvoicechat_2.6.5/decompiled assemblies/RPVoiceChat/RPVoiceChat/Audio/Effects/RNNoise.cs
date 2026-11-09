using System;
using System.Runtime.InteropServices;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x0200010C RID: 268
	internal static class RNNoise
	{
		// Token: 0x060009EA RID: 2538
		[DllImport("RNNoise", EntryPoint = "rnnoise_create", ExactSpelling = true)]
		public static extern IntPtr Create(IntPtr model);

		// Token: 0x060009EB RID: 2539
		[DllImport("RNNoise", EntryPoint = "rnnoise_destroy", ExactSpelling = true)]
		public static extern void Destroy(IntPtr state);

		// Token: 0x060009EC RID: 2540
		[DllImport("RNNoise", EntryPoint = "rnnoise_process_frame", ExactSpelling = true)]
		public static extern float DenoiseFrame(IntPtr state, IntPtr dataOut, IntPtr dataIn);

		// Token: 0x0400047C RID: 1148
		private const string Lib = "RNNoise";

		// Token: 0x0400047D RID: 1149
		public const int FRAME_SIZE = 480;
	}
}
