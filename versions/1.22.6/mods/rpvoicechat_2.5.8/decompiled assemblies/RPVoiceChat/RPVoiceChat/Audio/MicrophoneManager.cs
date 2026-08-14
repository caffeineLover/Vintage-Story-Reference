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
	// Token: 0x020000C7 RID: 199
	public class MicrophoneManager : IDisposable
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000700 RID: 1792 RVA: 0x000276B0 File Offset: 0x000258B0
		// (remove) Token: 0x06000701 RID: 1793 RVA: 0x000276E8 File Offset: 0x000258E8
		public event Action<AudioData> OnBufferRecorded;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000702 RID: 1794 RVA: 0x00027720 File Offset: 0x00025920
		// (remove) Token: 0x06000703 RID: 1795 RVA: 0x00027758 File Offset: 0x00025958
		public event Action<VoiceLevel> VoiceLevelUpdated;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000704 RID: 1796 RVA: 0x00027790 File Offset: 0x00025990
		// (remove) Token: 0x06000705 RID: 1797 RVA: 0x000277C8 File Offset: 0x000259C8
		public event Action TransmissionStateChanged;

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000277FD File Offset: 0x000259FD
		private double _maxVolume
		{
			get
			{
				return (double)ServerConfigManager.MaxVolumeLimit;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00027805 File Offset: 0x00025A05
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0002780D File Offset: 0x00025A0D
		public double Amplitude { get; private set; }

		// Token: 0x06000709 RID: 1801 RVA: 0x00027818 File Offset: 0x00025A18
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

		// Token: 0x0600070A RID: 1802 RVA: 0x00027934 File Offset: 0x00025B34
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

		// Token: 0x0600070B RID: 1803 RVA: 0x00027961 File Offset: 0x00025B61
		public double GetMaxInputThreshold()
		{
			return this.maxInputThreshold;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00027969 File Offset: 0x00025B69
		public double GetInputThreshold()
		{
			return this.inputThreshold;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00027971 File Offset: 0x00025B71
		public void SetThreshold(float threshold)
		{
			this.inputThreshold = (double)threshold * this.maxInputThreshold;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00027982 File Offset: 0x00025B82
		public void SetGain(float newGain)
		{
			this.gain = newGain;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002798B File Offset: 0x00025B8B
		public void SetDenoisingSensitivity(float sensitivity)
		{
			IDenoiser denoiser = this.denoiser;
			if (denoiser == null)
			{
				return;
			}
			denoiser.SetBackgroundNoiseThreshold(sensitivity);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002799E File Offset: 0x00025B9E
		public void SetDenoisingStrength(float strength)
		{
			IDenoiser denoiser = this.denoiser;
			if (denoiser == null)
			{
				return;
			}
			denoiser.SetVoiceDenoisingStrength(strength);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000279B1 File Offset: 0x00025BB1
		public void SetTransmissionRange(int rangeBlocks)
		{
			this.customTransmissionRange = Math.Max(0, rangeBlocks);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000279C0 File Offset: 0x00025BC0
		public int GetTransmissionRange()
		{
			return this.customTransmissionRange;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000279C8 File Offset: 0x00025BC8
		public void ResetTransmissionRange()
		{
			this.customTransmissionRange = 0;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000279D4 File Offset: 0x00025BD4
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

		// Token: 0x06000715 RID: 1813 RVA: 0x00027A0E File Offset: 0x00025C0E
		public void SetIgnoreDistanceReduction(bool ignore)
		{
			this.ignoreDistanceReduction = ignore;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00027A17 File Offset: 0x00025C17
		public bool GetIgnoreDistanceReduction()
		{
			return this.ignoreDistanceReduction;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00027A1F File Offset: 0x00025C1F
		public void SetWallThicknessOverride(float thickness)
		{
			this.wallThicknessOverride = thickness;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00027A28 File Offset: 0x00025C28
		public float GetWallThicknessOverride()
		{
			return this.wallThicknessOverride;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00027A30 File Offset: 0x00025C30
		public void ResetWallThicknessOverride()
		{
			this.wallThicknessOverride = -1f;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00027A3D File Offset: 0x00025C3D
		public void SetGlobalBroadcast(bool enabled)
		{
			this.isGlobalBroadcast = enabled;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00027A46 File Offset: 0x00025C46
		public bool GetGlobalBroadcast()
		{
			return this.isGlobalBroadcast;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00027A4E File Offset: 0x00025C4E
		public void ResetGlobalBroadcast()
		{
			this.isGlobalBroadcast = false;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00027A58 File Offset: 0x00025C58
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

		// Token: 0x0600071E RID: 1822 RVA: 0x00027AE4 File Offset: 0x00025CE4
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

		// Token: 0x0600071F RID: 1823 RVA: 0x00027C40 File Offset: 0x00025E40
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
						goto IL_2A5;
					}
				}
				encodedAudio = this.codec.Encode(samplesToEncode);
				IL_2A5:
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

		// Token: 0x06000720 RID: 1824 RVA: 0x00027FA0 File Offset: 0x000261A0
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
			bool isPTTKeyPressed = this.capi.Input.KeyboardKeyState[this.capi.Input.GetHotKeyByCode("voicechatPTT").CurrentMapping.KeyCode];
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
				if (justStartedTransmitting && !ModConfig.ClientConfig.PushToTalkEnabled && previousData != null)
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

		// Token: 0x06000721 RID: 1825 RVA: 0x00028118 File Offset: 0x00026318
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

		// Token: 0x06000722 RID: 1826 RVA: 0x000282CC File Offset: 0x000264CC
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

		// Token: 0x06000723 RID: 1827 RVA: 0x0002838C File Offset: 0x0002658C
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

		// Token: 0x06000724 RID: 1828 RVA: 0x000283E4 File Offset: 0x000265E4
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

		// Token: 0x06000725 RID: 1829 RVA: 0x0002842E File Offset: 0x0002662E
		private void SetInputFormat(ALFormat format)
		{
			this.InputChannelCount = AudioUtils.ChannelsPerFormat(format);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0002843C File Offset: 0x0002663C
		private void SetOutputFormat(ALFormat format)
		{
			this.OutputFormat = format;
			this.OutputChannelCount = AudioUtils.ChannelsPerFormat(format);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00028454 File Offset: 0x00026654
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

		// Token: 0x06000728 RID: 1832 RVA: 0x00028513 File Offset: 0x00026713
		public string[] GetInputDeviceNames()
		{
			List<string> availableDevices = OpenALAudioCapture.GetAvailableDevices();
			availableDevices.Insert(0, "Default");
			return availableDevices.ToArray();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002852C File Offset: 0x0002672C
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

		// Token: 0x0600072A RID: 1834 RVA: 0x0002858E File Offset: 0x0002678E
		public VoiceLevel GetVoiceLevel()
		{
			return this.voiceLevel;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00028596 File Offset: 0x00026796
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

		// Token: 0x0600072C RID: 1836 RVA: 0x000285C0 File Offset: 0x000267C0
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

		// Token: 0x0600072D RID: 1837 RVA: 0x000285F4 File Offset: 0x000267F4
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

		// Token: 0x04000327 RID: 807
		public static int Frequency = 48000;

		// Token: 0x04000328 RID: 808
		private IAudioCapture capture;

		// Token: 0x04000329 RID: 809
		private Thread audioCaptureThread;

		// Token: 0x0400032A RID: 810
		private CancellationTokenSource audioCaptureCTS;

		// Token: 0x0400032B RID: 811
		private int BufferSize = (int)((double)MicrophoneManager.Frequency * 0.5);

		// Token: 0x0400032C RID: 812
		private int InputChannelCount;

		// Token: 0x0400032D RID: 813
		private const byte SampleSize = 2;

		// Token: 0x0400032E RID: 814
		private IAudioCodec codec;

		// Token: 0x0400032F RID: 815
		private IDenoiser denoiser;

		// Token: 0x04000330 RID: 816
		private ALFormat OutputFormat;

		// Token: 0x04000331 RID: 817
		private int OutputChannelCount;

		// Token: 0x04000332 RID: 818
		private float gain;

		// Token: 0x04000333 RID: 819
		private short maxSampleValue;

		// Token: 0x04000334 RID: 820
		private List<float> recentGainLimits = new List<float>();

		// Token: 0x04000335 RID: 821
		private ConcurrentQueue<float> recentGainLimitsQueue = new ConcurrentQueue<float>();

		// Token: 0x04000337 RID: 823
		public bool IsDenoisingAvailable;

		// Token: 0x04000338 RID: 824
		public bool Transmitting;

		// Token: 0x04000339 RID: 825
		public bool AudioWizardActive;

		// Token: 0x0400033A RID: 826
		private const int deactivationWindow = 4;

		// Token: 0x0400033B RID: 827
		private int stepsSinceLastTransmission = 4;

		// Token: 0x0400033C RID: 828
		private bool transmittingOnPreviousStep;

		// Token: 0x0400033D RID: 829
		private AudioData previousBuffer;

		// Token: 0x0400033E RID: 830
		private ICoreClientAPI capi;

		// Token: 0x0400033F RID: 831
		private VoiceLevel voiceLevel = VoiceLevel.Talking;

		// Token: 0x04000340 RID: 832
		private double inputThreshold;

		// Token: 0x04000341 RID: 833
		private double maxInputThreshold;

		// Token: 0x04000342 RID: 834
		private List<double> recentAmplitudes = new List<double>();

		// Token: 0x04000343 RID: 835
		private int customTransmissionRange;

		// Token: 0x04000344 RID: 836
		private bool ignoreDistanceReduction;

		// Token: 0x04000345 RID: 837
		private float wallThicknessOverride = -1f;

		// Token: 0x04000346 RID: 838
		private bool isGlobalBroadcast;
	}
}
