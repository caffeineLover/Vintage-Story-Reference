using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Audio.Effects;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000100 RID: 256
	public class MicrophoneManager : IDisposable
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600091B RID: 2331 RVA: 0x00032BD8 File Offset: 0x00030DD8
		// (remove) Token: 0x0600091C RID: 2332 RVA: 0x00032C10 File Offset: 0x00030E10
		public event Action<AudioData> OnBufferRecorded;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600091D RID: 2333 RVA: 0x00032C48 File Offset: 0x00030E48
		// (remove) Token: 0x0600091E RID: 2334 RVA: 0x00032C80 File Offset: 0x00030E80
		public event Action<VoiceLevel> VoiceLevelUpdated;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600091F RID: 2335 RVA: 0x00032CB8 File Offset: 0x00030EB8
		// (remove) Token: 0x06000920 RID: 2336 RVA: 0x00032CF0 File Offset: 0x00030EF0
		public event Action TransmissionStateChanged;

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00032D25 File Offset: 0x00030F25
		private double _maxVolume
		{
			get
			{
				return (double)ServerConfigManager.MaxVolumeLimit;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00032D2D File Offset: 0x00030F2D
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x00032D35 File Offset: 0x00030F35
		public double Amplitude { get; private set; }

		// Token: 0x06000924 RID: 2340 RVA: 0x00032D40 File Offset: 0x00030F40
		public MicrophoneManager(ICoreClientAPI capi)
		{
			this.audioCaptureThread = new Thread(new ParameterizedThreadStart(this.CaptureAudio));
			this.audioCaptureCTS = new CancellationTokenSource();
			this.capi = capi;
			this.maxSampleValue = (short)(this._maxVolume * 32767.0);
			this.maxInputThreshold = this._maxVolume / 2.0;
			this.SetThreshold(ModConfig.ClientConfig.InputThreshold);
			this.SetGain(ModConfig.ClientConfig.InputGain);
			this.SetOutputFormat(4353);
			this.SetCodec("Opus");
			this.capture = this.CreateNewCapture(ModConfig.ClientConfig.InputDevice, default(ALFormat?));
			this.denoiser = this.TryLoadDenoiser();
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00032E67 File Offset: 0x00031067
		public void Launch()
		{
			this.audioCaptureThread.Start(this.audioCaptureCTS.Token);
			IAudioCapture audioCapture = this.capture;
			if (audioCapture == null)
			{
				return;
			}
			audioCapture.Start();
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00032E94 File Offset: 0x00031094
		public double GetMaxInputThreshold()
		{
			return this.maxInputThreshold;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00032E9C File Offset: 0x0003109C
		public double GetInputThreshold()
		{
			return this.inputThreshold;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00032EA4 File Offset: 0x000310A4
		public void SetThreshold(float threshold)
		{
			this.inputThreshold = (double)threshold * this.maxInputThreshold;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00032EB5 File Offset: 0x000310B5
		public void SetGain(float newGain)
		{
			this.gain = newGain;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00032EBE File Offset: 0x000310BE
		public void SetDenoisingSensitivity(float sensitivity)
		{
			IDenoiser denoiser = this.denoiser;
			if (denoiser == null)
			{
				return;
			}
			denoiser.SetBackgroundNoiseThreshold(sensitivity);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00032ED1 File Offset: 0x000310D1
		public void SetDenoisingStrength(float strength)
		{
			IDenoiser denoiser = this.denoiser;
			if (denoiser == null)
			{
				return;
			}
			denoiser.SetVoiceDenoisingStrength(strength);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00032EE4 File Offset: 0x000310E4
		public void SetTransmissionRange(int rangeBlocks)
		{
			this.customTransmissionRange = Math.Max(0, rangeBlocks);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00032EF3 File Offset: 0x000310F3
		public int GetTransmissionRange()
		{
			return this.customTransmissionRange;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00032EFB File Offset: 0x000310FB
		public void ResetTransmissionRange()
		{
			this.customTransmissionRange = 0;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00032F04 File Offset: 0x00031104
		public void SetTalkieHeldTransmit(bool active)
		{
			if (active)
			{
				this.talkieHeldTransmitDepth++;
				return;
			}
			this.talkieHeldTransmitDepth = Math.Max(0, this.talkieHeldTransmitDepth - 1);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00032F2C File Offset: 0x0003112C
		public List<float> GetRecentGainLimits()
		{
			List<float> recentGainLimits = new List<float>();
			while (this.recentGainLimitsQueue.Count > 0)
			{
				float gainLimit;
				if (this.recentGainLimitsQueue.TryDequeue(ref gainLimit))
				{
					recentGainLimits.Add(gainLimit);
				}
			}
			return recentGainLimits;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00032F68 File Offset: 0x00031168
		public List<double> GetRecentAmplitudes()
		{
			List<double> amplitudes = new List<double>();
			while (this.recentAmplitudesQueue.Count > 0)
			{
				double amplitude;
				if (this.recentAmplitudesQueue.TryDequeue(ref amplitude))
				{
					amplitudes.Add(amplitude);
				}
			}
			return amplitudes;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00032FA4 File Offset: 0x000311A4
		public void ClearCalibrationSamples()
		{
			float num;
			while (this.recentGainLimitsQueue.TryDequeue(ref num))
			{
			}
			double num2;
			while (this.recentAmplitudesQueue.TryDequeue(ref num2))
			{
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00032FCF File Offset: 0x000311CF
		public void SetIgnoreDistanceReduction(bool ignore)
		{
			this.ignoreDistanceReduction = ignore;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00032FD8 File Offset: 0x000311D8
		public bool GetIgnoreDistanceReduction()
		{
			return this.ignoreDistanceReduction;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00032FE0 File Offset: 0x000311E0
		public void SetWallThicknessOverride(float thickness)
		{
			this.wallThicknessOverride = thickness;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00032FE9 File Offset: 0x000311E9
		public float GetWallThicknessOverride()
		{
			return this.wallThicknessOverride;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00032FF1 File Offset: 0x000311F1
		public void ResetWallThicknessOverride()
		{
			this.wallThicknessOverride = -1f;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00032FFE File Offset: 0x000311FE
		public void SetGlobalBroadcast(bool enabled)
		{
			this.isGlobalBroadcast = enabled;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00033007 File Offset: 0x00031207
		public bool GetGlobalBroadcast()
		{
			return this.isGlobalBroadcast;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0003300F File Offset: 0x0003120F
		public void ResetGlobalBroadcast()
		{
			this.isGlobalBroadcast = false;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00033018 File Offset: 0x00031218
		private void CaptureAudio(object cancellationToken)
		{
			CancellationToken ct = (CancellationToken)cancellationToken;
			while (!ct.IsCancellationRequested)
			{
				try
				{
					int sleepMs = 100;
					ct.WaitHandle.WaitOne(sleepMs);
					if (ct.IsCancellationRequested)
					{
						break;
					}
					this.UpdateCaptureAudioSamples();
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception e)
				{
					Logger.client.Warning("Error in audio capture thread: " + e.Message);
					if (ct.IsCancellationRequested)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x000330A4 File Offset: 0x000312A4
		private void UpdateCaptureAudioSamples()
		{
			IClientPlayer player = this.capi.World.Player;
			EntityPlayer clientEntity = (player != null) ? player.Entity : null;
			if (clientEntity == null || this.capture == null)
			{
				return;
			}
			string targetCodec = WorldConfig.GetBool("encode-audio", false) ? "Opus" : "Dummy";
			this.SetCodec(targetCodec);
			int availableSamples = this.capture.AvailableSamples;
			int frameSize = this.codec.FrameSize;
			int samplesToRead = availableSamples - availableSamples % frameSize;
			if (samplesToRead <= 0)
			{
				return;
			}
			byte[] sampleBuffer = new byte[samplesToRead * 2 * this.InputChannelCount];
			this.capture.ReadSamples(sampleBuffer, samplesToRead);
			bool isMuted = ModConfig.ClientConfig.IsMuted;
			bool isSleeping = clientEntity.AnimManager.IsAnimationActive(new string[]
			{
				"sleep"
			});
			bool flag = isMuted || isSleeping || !clientEntity.Alive;
			bool forceProcessing = this.AudioWizardActive;
			if (!flag || forceProcessing)
			{
				AudioData data = this.ProcessAudio(sampleBuffer);
				this.TransmitAudio(data, this.previousBuffer);
				this.previousBuffer = data;
				return;
			}
			if (this.recentAmplitudes.Count == 0)
			{
				return;
			}
			this.recentAmplitudes.Clear();
			this.Amplitude = 0.0;
			this.Transmitting = false;
			Action transmissionStateChanged = this.TransmissionStateChanged;
			if (transmissionStateChanged != null)
			{
				transmissionStateChanged.Invoke();
			}
			this.transmittingOnPreviousStep = this.Transmitting;
			this.previousBuffer = null;
			this.stepsSinceLastTransmission = 4;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00033200 File Offset: 0x00031400
		private unsafe AudioData ProcessAudio(byte[] rawSamples)
		{
			int rawSampleSize = 2 * this.InputChannelCount;
			int pcmCount = rawSamples.Length / rawSampleSize;
			bool flag;
			if (ModConfig.ClientConfig.Denoising)
			{
				IDenoiser denoiser = this.denoiser;
				flag = (denoiser != null && denoiser.SupportsFormat(MicrophoneManager.Frequency, this.OutputChannelCount, 16));
			}
			else
			{
				flag = false;
			}
			bool willDenoise = flag;
			short[] pcms = willDenoise ? new short[pcmCount] : ArrayPool<short>.Shared.Rent(pcmCount);
			bool rented = !willDenoise;
			if (rented && pcms != null)
			{
				Array.Clear(pcms, 0, Math.Min(pcmCount, pcms.Length));
			}
			bool[] usedChannels = this.DetectAudioChannels(rawSamples);
			int usedChannelCount = 0;
			for (int i = 0; i < usedChannels.Length; i++)
			{
				if (usedChannels[i])
				{
					usedChannelCount++;
				}
			}
			if (usedChannelCount == 0)
			{
				usedChannelCount = 1;
			}
			double peakPcmValue = 1.0;
			AudioData result;
			try
			{
				Span<short> shortSpan = MemoryMarshal.Cast<byte, short>(MemoryExtensions.AsSpan<byte>(rawSamples, 0, rawSamples.Length));
				int pcmIndex = 0;
				for (int rawSampleShortIndex = 0; rawSampleShortIndex < shortSpan.Length; rawSampleShortIndex += this.InputChannelCount)
				{
					double pcm = 0.0;
					for (int channelIndex = 0; channelIndex < this.InputChannelCount; channelIndex++)
					{
						if (usedChannels[channelIndex])
						{
							pcm += (double)(*shortSpan[rawSampleShortIndex + channelIndex]);
						}
					}
					pcm /= (double)Math.Max(usedChannelCount, 1);
					double abs = Math.Abs(pcm);
					if (abs > peakPcmValue)
					{
						peakPcmValue = abs;
					}
					pcms[pcmIndex++] = (short)pcm;
				}
				float maxSafeGain = Math.Min(this.gain, (float)((double)this.maxSampleValue / peakPcmValue));
				this.recentGainLimits.Add(maxSafeGain);
				if (this.recentGainLimits.Count > 10)
				{
					this.recentGainLimits.RemoveAt(0);
				}
				this.recentGainLimitsQueue.Enqueue(maxSafeGain);
				if (this.recentGainLimitsQueue.Count > 10)
				{
					float num;
					this.recentGainLimitsQueue.TryDequeue(ref num);
				}
				float volumeAmplification = (this.recentGainLimits.Count > 0) ? Math.Min(maxSafeGain, Enumerable.Average(this.recentGainLimits)) : maxSafeGain;
				if (willDenoise)
				{
					this.denoiser.Denoise(ref pcms);
				}
				double sampleSquareSum = 0.0;
				for (int j = 0; j < pcmCount; j++)
				{
					short amplified = (short)GameMath.Clamp((float)pcms[j] * volumeAmplification, -32768f, 32767f);
					pcms[j] = amplified;
					sampleSquareSum += Math.Pow((double)((float)amplified / 32767f), 2.0);
				}
				double amplitude = Math.Sqrt(sampleSquareSum / (double)Math.Max(1, pcmCount));
				this.recentAmplitudesQueue.Enqueue(amplitude);
				if (this.recentAmplitudesQueue.Count > 20)
				{
					double num2;
					this.recentAmplitudesQueue.TryDequeue(ref num2);
				}
				short[] samplesToEncode = pcms;
				if (rented && pcms.Length > pcmCount)
				{
					samplesToEncode = new short[pcmCount];
					Array.Copy(pcms, 0, samplesToEncode, 0, pcmCount);
				}
				byte[] encodedAudio;
				if (this.isGlobalBroadcast)
				{
					OpusCodec opusCodec = this.codec as OpusCodec;
					if (opusCodec != null)
					{
						encodedAudio = opusCodec.EncodeForBroadcast(samplesToEncode);
						goto IL_2CF;
					}
				}
				encodedAudio = this.codec.Encode(samplesToEncode);
				IL_2CF:
				result = new AudioData
				{
					data = encodedAudio,
					frequency = MicrophoneManager.Frequency,
					format = this.OutputFormat,
					amplitude = amplitude,
					voiceLevel = this.voiceLevel,
					codec = this.codec.Name,
					transmissionRangeBlocks = this.customTransmissionRange,
					ignoreDistanceReduction = this.ignoreDistanceReduction,
					wallThicknessOverride = this.wallThicknessOverride,
					isGlobalBroadcast = this.isGlobalBroadcast
				};
			}
			finally
			{
				if (rented)
				{
					ArrayPool<short>.Shared.Return(pcms, false);
				}
			}
			return result;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00033588 File Offset: 0x00031788
		private void TransmitAudio(AudioData data, AudioData previousData = null)
		{
			this.recentAmplitudes.Add(data.amplitude);
			if (this.recentAmplitudes.Count > 3)
			{
				this.recentAmplitudes.RemoveAt(0);
			}
			if (data.amplitude >= this.inputThreshold)
			{
				this.Amplitude = data.amplitude;
			}
			else
			{
				this.Amplitude = Math.Max(data.amplitude, Enumerable.Average(this.recentAmplitudes));
			}
			bool isPTTKeyPressed = this.capi.Input.KeyboardKeyState[this.capi.Input.GetHotKeyByCode("voicechatPTT").CurrentMapping.KeyCode] || this.talkieHeldTransmitDepth > 0;
			bool isAboveInputThreshold = this.Amplitude >= this.inputThreshold;
			this.Transmitting = (ModConfig.ClientConfig.PushToTalkEnabled ? isPTTKeyPressed : isAboveInputThreshold);
			this.stepsSinceLastTransmission++;
			if (this.Transmitting)
			{
				this.stepsSinceLastTransmission = 0;
			}
			this.Transmitting = (this.stepsSinceLastTransmission < 4);
			bool justStartedTransmitting = !this.transmittingOnPreviousStep && this.Transmitting;
			if (this.Transmitting != this.transmittingOnPreviousStep)
			{
				Action transmissionStateChanged = this.TransmissionStateChanged;
				if (transmissionStateChanged != null)
				{
					transmissionStateChanged.Invoke();
				}
			}
			this.transmittingOnPreviousStep = this.Transmitting;
			if (this.Transmitting || this.AudioWizardActive)
			{
				if (justStartedTransmitting && !ModConfig.ClientConfig.PushToTalkEnabled && previousData != null && previousData.amplitude < this.inputThreshold)
				{
					Action<AudioData> onBufferRecorded = this.OnBufferRecorded;
					if (onBufferRecorded != null)
					{
						onBufferRecorded.Invoke(previousData);
					}
				}
				Action<AudioData> onBufferRecorded2 = this.OnBufferRecorded;
				if (onBufferRecorded2 == null)
				{
					return;
				}
				onBufferRecorded2.Invoke(data);
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0003371C File Offset: 0x0003191C
		private IAudioCapture CreateNewCapture(string deviceName, ALFormat? captureFormat = null)
		{
			ALFormat format = captureFormat ?? this.GetDefaultInputFormat();
			IAudioCapture audioCapture = this.capture;
			if (((audioCapture != null) ? audioCapture.CurrentDevice : null) == deviceName)
			{
				IAudioCapture audioCapture2 = this.capture;
				if (audioCapture2 != null && audioCapture2.SampleFormat == format)
				{
					return this.capture;
				}
			}
			IAudioCapture audioCapture3 = this.capture;
			if (audioCapture3 != null)
			{
				audioCapture3.Stop();
			}
			IAudioCapture audioCapture4 = this.capture;
			if (audioCapture4 != null)
			{
				audioCapture4.Dispose();
			}
			IAudioCapture newCapture = null;
			try
			{
				newCapture = new OpenALAudioCapture(deviceName, MicrophoneManager.Frequency, format, this.BufferSize);
				format = ((newCapture != null) ? newCapture.SampleFormat : format);
				deviceName = (((newCapture != null) ? newCapture.CurrentDevice : null) ?? deviceName);
				Logger client = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Succesfully created an audio capture device with arguments: ");
				defaultInterpolatedStringHandler.AppendFormatted(deviceName);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(MicrophoneManager.Frequency);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<ALFormat>(format);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.BufferSize);
				client.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			catch (Exception e)
			{
				Logger client2 = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(51, 3);
				defaultInterpolatedStringHandler2.AppendLiteral("Could not create audio capture device ");
				defaultInterpolatedStringHandler2.AppendFormatted(deviceName);
				defaultInterpolatedStringHandler2.AppendLiteral(" in ");
				defaultInterpolatedStringHandler2.AppendFormatted<ALFormat>(format);
				defaultInterpolatedStringHandler2.AppendLiteral(" format:\n");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(e);
				client2.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			this.SetInputFormat(format);
			ModConfig.ClientConfig.InputDevice = (deviceName ?? "Default");
			return newCapture;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x000338D0 File Offset: 0x00031AD0
		private void SetCodec(string codecName)
		{
			IAudioCodec audioCodec = this.codec;
			if (((audioCodec != null) ? audioCodec.Name : null) == codecName)
			{
				IAudioCodec audioCodec2 = this.codec;
				int? num = (audioCodec2 != null) ? new int?(audioCodec2.Channels) : default(int?);
				int outputChannelCount = this.OutputChannelCount;
				if (num.GetValueOrDefault() == outputChannelCount & num != null)
				{
					return;
				}
			}
			IAudioCodec audioCodec3;
			if (!(codecName == "Opus"))
			{
				if (!(codecName == "Dummy"))
				{
					throw new ArgumentException(codecName + " is not a valid codec name");
				}
				audioCodec3 = new DummyCodec(MicrophoneManager.Frequency, this.OutputChannelCount);
			}
			else
			{
				audioCodec3 = new OpusCodec(MicrophoneManager.Frequency, this.OutputChannelCount);
			}
			this.codec = audioCodec3;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00033990 File Offset: 0x00031B90
		private IDenoiser TryLoadDenoiser()
		{
			IDenoiser denoiser = null;
			try
			{
				denoiser = new RNNoiseDenoiser(ModConfig.ClientConfig.InputThreshold, ModConfig.ClientConfig.DenoisingStrength);
				this.IsDenoisingAvailable = true;
			}
			catch (DllNotFoundException)
			{
				Logger.client.Error("Can't find denoising library, denoising won't be available");
			}
			return denoiser;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000339E8 File Offset: 0x00031BE8
		private ALFormat GetDefaultInputFormat()
		{
			ALFormat format = 4353;
			if ((AL.Get(45060) ?? "").Contains("AL_EXT_MCFORMATS"))
			{
				format = 4613;
			}
			else
			{
				Logger.client.Warning("Multichannel audio capture is not available.");
			}
			return format;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00033A32 File Offset: 0x00031C32
		private void SetInputFormat(ALFormat format)
		{
			this.InputChannelCount = AudioUtils.ChannelsPerFormat(format);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00033A40 File Offset: 0x00031C40
		private void SetOutputFormat(ALFormat format)
		{
			this.OutputFormat = format;
			this.OutputChannelCount = AudioUtils.ChannelsPerFormat(format);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00033A58 File Offset: 0x00031C58
		private bool[] DetectAudioChannels(byte[] rawSamples)
		{
			bool[] used = new bool[this.InputChannelCount];
			int[] sampleSums = new int[this.InputChannelCount];
			int rawSampleSize = 2 * this.InputChannelCount;
			int monoSampleSize = 2;
			int maxIndex = Math.Min(rawSamples.Length, rawSampleSize * 10);
			for (int rawSampleIndex = 0; rawSampleIndex < maxIndex; rawSampleIndex += rawSampleSize)
			{
				for (int channelIndex = 0; channelIndex < this.InputChannelCount; channelIndex++)
				{
					int sampleIndex = rawSampleIndex + channelIndex * monoSampleSize;
					int sample = (int)BitConverter.ToInt16(rawSamples, sampleIndex);
					sampleSums[channelIndex] += Math.Abs(sample);
				}
			}
			bool guessUsedChannels = ModConfig.ClientConfig.ChannelGuessing;
			for (int channelIndex2 = 0; channelIndex2 < this.InputChannelCount; channelIndex2++)
			{
				if (sampleSums[channelIndex2] / 10 > 5 || !guessUsedChannels)
				{
					used[channelIndex2] = true;
				}
			}
			return used;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00033B18 File Offset: 0x00031D18
		public string[] GetInputDeviceNames()
		{
			List<string> devices = OpenALAudioCapture.GetAvailableDevices();
			if (devices.Count == 0)
			{
				return new string[]
				{
					"Default"
				};
			}
			devices.Insert(0, "Default");
			return devices.ToArray();
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00033B54 File Offset: 0x00031D54
		public bool HasCaptureHardware()
		{
			return OpenALAudioCapture.GetAvailableDevices().Count > 0;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00033B63 File Offset: 0x00031D63
		public bool CanUseMicrophoneCapture()
		{
			return this.capture != null || this.HasCaptureHardware();
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00033B78 File Offset: 0x00031D78
		public VoiceLevel CycleVoiceLevel()
		{
			if (this.voiceLevel == VoiceLevel.Talking)
			{
				this.voiceLevel = VoiceLevel.Shouting;
			}
			else if (this.voiceLevel == VoiceLevel.Shouting)
			{
				this.voiceLevel = VoiceLevel.Whispering;
			}
			else if (this.voiceLevel == VoiceLevel.Whispering)
			{
				this.voiceLevel = VoiceLevel.Talking;
			}
			Action<VoiceLevel> voiceLevelUpdated = this.VoiceLevelUpdated;
			if (voiceLevelUpdated != null)
			{
				voiceLevelUpdated.Invoke(this.voiceLevel);
			}
			return this.voiceLevel;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00033BDA File Offset: 0x00031DDA
		public VoiceLevel GetVoiceLevel()
		{
			return this.voiceLevel;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00033BE2 File Offset: 0x00031DE2
		public void SetVoiceLevel(VoiceLevel newLevel)
		{
			if (this.voiceLevel != newLevel)
			{
				this.voiceLevel = newLevel;
				Action<VoiceLevel> voiceLevelUpdated = this.VoiceLevelUpdated;
				if (voiceLevelUpdated == null)
				{
					return;
				}
				voiceLevelUpdated.Invoke(this.voiceLevel);
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00033C0C File Offset: 0x00031E0C
		public void SetInputDevice(string deviceId)
		{
			this.capture = this.CreateNewCapture(deviceId, default(ALFormat?));
			IAudioCapture audioCapture = this.capture;
			if (audioCapture == null)
			{
				return;
			}
			audioCapture.Start();
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00033C40 File Offset: 0x00031E40
		public void Dispose()
		{
			CancellationTokenSource cancellationTokenSource = this.audioCaptureCTS;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			CancellationTokenSource cancellationTokenSource2 = this.audioCaptureCTS;
			if (cancellationTokenSource2 != null)
			{
				cancellationTokenSource2.Dispose();
			}
			IAudioCapture audioCapture = this.capture;
			if (audioCapture != null)
			{
				audioCapture.Stop();
			}
			IAudioCapture audioCapture2 = this.capture;
			if (audioCapture2 != null)
			{
				audioCapture2.Dispose();
			}
			IDenoiser denoiser = this.denoiser;
			if (denoiser == null)
			{
				return;
			}
			denoiser.Dispose();
		}

		// Token: 0x04000406 RID: 1030
		public static int Frequency = 48000;

		// Token: 0x04000407 RID: 1031
		private IAudioCapture capture;

		// Token: 0x04000408 RID: 1032
		private Thread audioCaptureThread;

		// Token: 0x04000409 RID: 1033
		private CancellationTokenSource audioCaptureCTS;

		// Token: 0x0400040A RID: 1034
		private int BufferSize = (int)((double)MicrophoneManager.Frequency * 0.5);

		// Token: 0x0400040B RID: 1035
		private int InputChannelCount;

		// Token: 0x0400040C RID: 1036
		private const byte SampleSize = 2;

		// Token: 0x0400040D RID: 1037
		private IAudioCodec codec;

		// Token: 0x0400040E RID: 1038
		private IDenoiser denoiser;

		// Token: 0x0400040F RID: 1039
		private ALFormat OutputFormat;

		// Token: 0x04000410 RID: 1040
		private int OutputChannelCount;

		// Token: 0x04000411 RID: 1041
		private float gain;

		// Token: 0x04000412 RID: 1042
		private short maxSampleValue;

		// Token: 0x04000413 RID: 1043
		private List<float> recentGainLimits = new List<float>();

		// Token: 0x04000414 RID: 1044
		private ConcurrentQueue<float> recentGainLimitsQueue = new ConcurrentQueue<float>();

		// Token: 0x04000415 RID: 1045
		private ConcurrentQueue<double> recentAmplitudesQueue = new ConcurrentQueue<double>();

		// Token: 0x04000417 RID: 1047
		public bool IsDenoisingAvailable;

		// Token: 0x04000418 RID: 1048
		public bool Transmitting;

		// Token: 0x04000419 RID: 1049
		public bool AudioWizardActive;

		// Token: 0x0400041A RID: 1050
		private const int deactivationWindow = 4;

		// Token: 0x0400041B RID: 1051
		private int stepsSinceLastTransmission = 4;

		// Token: 0x0400041C RID: 1052
		private bool transmittingOnPreviousStep;

		// Token: 0x0400041D RID: 1053
		private AudioData previousBuffer;

		// Token: 0x0400041E RID: 1054
		private ICoreClientAPI capi;

		// Token: 0x0400041F RID: 1055
		private VoiceLevel voiceLevel = VoiceLevel.Talking;

		// Token: 0x04000420 RID: 1056
		private double inputThreshold;

		// Token: 0x04000421 RID: 1057
		private double maxInputThreshold;

		// Token: 0x04000422 RID: 1058
		private List<double> recentAmplitudes = new List<double>();

		// Token: 0x04000423 RID: 1059
		private int customTransmissionRange;

		// Token: 0x04000424 RID: 1060
		private int talkieHeldTransmitDepth;

		// Token: 0x04000425 RID: 1061
		private bool ignoreDistanceReduction;

		// Token: 0x04000426 RID: 1062
		private float wallThicknessOverride = -1f;

		// Token: 0x04000427 RID: 1063
		private bool isGlobalBroadcast;
	}
}
