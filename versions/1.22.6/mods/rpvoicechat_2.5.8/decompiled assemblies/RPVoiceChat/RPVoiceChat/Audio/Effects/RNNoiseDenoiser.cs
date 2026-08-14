using System;
using System.Runtime.InteropServices;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D2 RID: 210
	public class RNNoiseDenoiser : IDenoiser, IDisposable
	{
		// Token: 0x060007A0 RID: 1952 RVA: 0x0002A734 File Offset: 0x00028934
		public RNNoiseDenoiser(float backgroundNoiseThreshold, float voiceDenoisingStrength)
		{
			this.handle = RNNoise.Create(IntPtr.Zero);
			this.SetBackgroundNoiseThreshold(backgroundNoiseThreshold);
			this.SetVoiceDenoisingStrength(voiceDenoisingStrength);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002A768 File Offset: 0x00028968
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

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002A944 File Offset: 0x00028B44
		public void SetBackgroundNoiseThreshold(float value)
		{
			this.sensitivity = GameMath.Clamp(value, 0f, 1f);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002A95C File Offset: 0x00028B5C
		public void SetVoiceDenoisingStrength(float value)
		{
			this.strength = GameMath.Clamp(value, 0f, 1f);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002A974 File Offset: 0x00028B74
		public bool SupportsFormat(int frequency, int channels, int bits)
		{
			return frequency == 48000 && channels == 1 && bits == 16;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002A989 File Offset: 0x00028B89
		public void Dispose()
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			RNNoise.Destroy(this.handle);
		}

		// Token: 0x04000384 RID: 900
		private const int FRAME_SIZE = 480;

		// Token: 0x04000385 RID: 901
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x04000386 RID: 902
		private float sensitivity;

		// Token: 0x04000387 RID: 903
		private float strength;

		// Token: 0x02000136 RID: 310
		private class PointerSource : IDisposable
		{
			// Token: 0x060008CA RID: 2250 RVA: 0x0002DA76 File Offset: 0x0002BC76
			public PointerSource(Array buffer)
			{
				this.handle = GCHandle.Alloc(buffer, 3);
				this.ptr = this.handle.AddrOfPinnedObject();
			}

			// Token: 0x060008CB RID: 2251 RVA: 0x0002DA9C File Offset: 0x0002BC9C
			public void Dispose()
			{
				this.handle.Free();
			}

			// Token: 0x040004B2 RID: 1202
			private GCHandle handle;

			// Token: 0x040004B3 RID: 1203
			public IntPtr ptr;
		}
	}
}
