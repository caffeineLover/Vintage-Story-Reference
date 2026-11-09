using System;
using System.Runtime.InteropServices;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x0200010D RID: 269
	public class RNNoiseDenoiser : IDenoiser, IDisposable
	{
		// Token: 0x060009ED RID: 2541 RVA: 0x00036F04 File Offset: 0x00035104
		public RNNoiseDenoiser(float backgroundNoiseThreshold, float voiceDenoisingStrength)
		{
			this.handle = RNNoise.Create(IntPtr.Zero);
			this.SetBackgroundNoiseThreshold(backgroundNoiseThreshold);
			this.SetVoiceDenoisingStrength(voiceDenoisingStrength);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00036F38 File Offset: 0x00035138
		public void Denoise(ref short[] _pcms)
		{
			float[] rawAudio = Array.ConvertAll<short, float>(_pcms, (short e) => (float)e);
			using (RNNoiseDenoiser.PointerSource pcmBuffer = new RNNoiseDenoiser.PointerSource(rawAudio))
			{
				float? lastDenoisingStrength = default(float?);
				for (int offset = 0; offset < rawAudio.Length; offset += 480)
				{
					int remainingDataLength = rawAudio.Length - offset;
					float[] denoisedAudio = new float[480];
					int frameLength = 480;
					using (RNNoiseDenoiser.PointerSource denoisedBuffer = new RNNoiseDenoiser.PointerSource(denoisedAudio))
					{
						IntPtr inPtr = pcmBuffer.ptr + (IntPtr)(offset * 4);
						IntPtr outPtr = denoisedBuffer.ptr;
						if (remainingDataLength < 480)
						{
							frameLength = remainingDataLength;
							Array.Copy(rawAudio, offset, denoisedAudio, 0, frameLength);
							inPtr = outPtr;
						}
						bool flag = RNNoise.DenoiseFrame(this.handle, outPtr, inPtr) > this.sensitivity;
						int crossfadeSteps = Math.Min(30, frameLength);
						float targetDenoisingStrength = flag ? this.strength : 1f;
						float denoisingStrength = lastDenoisingStrength.GetValueOrDefault(targetDenoisingStrength);
						float strengthDeltaPerStep = (targetDenoisingStrength - denoisingStrength) / (float)crossfadeSteps;
						for (int i = 0; i < frameLength; i++)
						{
							if (i < crossfadeSteps)
							{
								denoisingStrength = (float)Math.Round((double)(denoisingStrength + strengthDeltaPerStep), 3);
								denoisingStrength = GameMath.Clamp(denoisingStrength, 0f, 1f);
							}
							denoisedAudio[i] = denoisedAudio[i] * denoisingStrength + rawAudio[offset + i] * (1f - denoisingStrength);
						}
						lastDenoisingStrength = new float?(targetDenoisingStrength);
						Array.Copy(denoisedAudio, 0, rawAudio, offset, frameLength);
					}
				}
			}
			_pcms = Array.ConvertAll<float, short>(rawAudio, (float e) => (short)GameMath.Clamp(e, -32768f, 32767f));
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00037114 File Offset: 0x00035314
		public void SetBackgroundNoiseThreshold(float value)
		{
			this.sensitivity = GameMath.Clamp(value, 0f, 1f);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0003712C File Offset: 0x0003532C
		public void SetVoiceDenoisingStrength(float value)
		{
			this.strength = GameMath.Clamp(value, 0f, 1f);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00037144 File Offset: 0x00035344
		public bool SupportsFormat(int frequency, int channels, int bits)
		{
			return frequency == 48000 && channels == 1 && bits == 16;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00037159 File Offset: 0x00035359
		public void Dispose()
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			RNNoise.Destroy(this.handle);
		}

		// Token: 0x0400047E RID: 1150
		private const int FRAME_SIZE = 480;

		// Token: 0x0400047F RID: 1151
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x04000480 RID: 1152
		private float sensitivity;

		// Token: 0x04000481 RID: 1153
		private float strength;

		// Token: 0x02000194 RID: 404
		private class PointerSource : IDisposable
		{
			// Token: 0x06000BA9 RID: 2985 RVA: 0x0003BDF0 File Offset: 0x00039FF0
			public PointerSource(Array buffer)
			{
				this.handle = GCHandle.Alloc(buffer, 3);
				this.ptr = this.handle.AddrOfPinnedObject();
			}

			// Token: 0x06000BAA RID: 2986 RVA: 0x0003BE16 File Offset: 0x0003A016
			public void Dispose()
			{
				this.handle.Free();
			}

			// Token: 0x04000649 RID: 1609
			private GCHandle handle;

			// Token: 0x0400064A RID: 1610
			public IntPtr ptr;
		}
	}
}
