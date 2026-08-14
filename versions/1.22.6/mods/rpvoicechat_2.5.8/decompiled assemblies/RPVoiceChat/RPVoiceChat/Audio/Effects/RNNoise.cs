using System;
using System.Runtime.InteropServices;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D1 RID: 209
	internal static class RNNoise
	{
		// Token: 0x0600079D RID: 1949
		[DllImport("RNNoise", EntryPoint = "rnnoise_create", ExactSpelling = true)]
		public static extern IntPtr Create(IntPtr model);

		// Token: 0x0600079E RID: 1950
		[DllImport("RNNoise", EntryPoint = "rnnoise_destroy", ExactSpelling = true)]
		public static extern void Destroy(IntPtr state);

		// Token: 0x0600079F RID: 1951
		[DllImport("RNNoise", EntryPoint = "rnnoise_process_frame", ExactSpelling = true)]
		public static extern float DenoiseFrame(IntPtr state, IntPtr dataOut, IntPtr dataIn);

		// Token: 0x04000382 RID: 898
		private const string Lib = "RNNoise";

		// Token: 0x04000383 RID: 899
		public const int FRAME_SIZE = 480;
	}
}
