using System;
using System.IO;
using System.Runtime.CompilerServices;
using Concentus;
using RPVoiceChat.Config;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000FE RID: 254
	public class OpusCodec : IAudioCodec
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00032617 File Offset: 0x00030817
		public string Name { get; } = "Opus";

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0003261F File Offset: 0x0003081F
		public int SampleRate { get; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00032627 File Offset: 0x00030827
		public int Channels { get; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0003262F File Offset: 0x0003082F
		public int FrameSize { get; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00032637 File Offset: 0x00030837
		private int NormalBitrate
		{
			get
			{
				return ServerConfigManager.NormalBitrate;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0003263E File Offset: 0x0003083E
		private int BroadcastBitrate
		{
			get
			{
				return ServerConfigManager.BroadcastBitrate;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00032645 File Offset: 0x00030845
		private int ProgramStreamBitrate
		{
			get
			{
				return Math.Max(this.NormalBitrate, 48000);
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00032658 File Offset: 0x00030858
		public OpusCodec(int frequency, int channelCount)
		{
			this.SampleRate = frequency;
			this.Channels = channelCount;
			this.FrameSize = this.SampleRate / 100 * this.Channels;
			this.encoder = OpusCodecFactory.CreateEncoder(this.SampleRate, this.Channels, 2048, null);
			this.decoder = OpusCodecFactory.CreateDecoder(this.SampleRate, this.Channels, null);
			this.SetNormalQuality();
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000326D8 File Offset: 0x000308D8
		private void SetNormalQuality()
		{
			this.encoder.Bitrate = this.NormalBitrate;
			this.encoder.Complexity = 6;
			this.encoder.SignalType = 3001;
			this.encoder.ForceMode = 1000;
			this.encoder.UseDTX = false;
			this.encoder.UseInbandFEC = false;
			this.encoder.UseVBR = true;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00032748 File Offset: 0x00030948
		private void SetBroadcastQuality()
		{
			this.encoder.Bitrate = this.BroadcastBitrate;
			this.encoder.Complexity = 5;
			this.encoder.SignalType = 3001;
			this.encoder.ForceMode = 1000;
			this.encoder.UseDTX = true;
			this.encoder.UseInbandFEC = false;
			this.encoder.UseVBR = true;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000327B8 File Offset: 0x000309B8
		private void SetProgramStreamQuality()
		{
			this.encoder.Bitrate = this.ProgramStreamBitrate;
			this.encoder.Complexity = 5;
			this.encoder.SignalType = 3002;
			this.encoder.ForceMode = -1000;
			this.encoder.UseDTX = false;
			this.encoder.UseInbandFEC = false;
			this.encoder.UseVBR = true;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00032826 File Offset: 0x00030A26
		public byte[] Encode(short[] pcmData)
		{
			return this.EncodeInternal(pcmData, OpusCodec.EncoderProfile.Voice);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00032830 File Offset: 0x00030A30
		public byte[] EncodeForBroadcast(short[] pcmData)
		{
			return this.EncodeInternal(pcmData, OpusCodec.EncoderProfile.Broadcast);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0003283A File Offset: 0x00030A3A
		public byte[] EncodeForProgramStream(short[] pcmData)
		{
			return this.EncodeInternal(pcmData, OpusCodec.EncoderProfile.ProgramStream);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00032844 File Offset: 0x00030A44
		private byte[] EncodeInternal(short[] pcmData, OpusCodec.EncoderProfile profile)
		{
			if (this.lastProfile != profile)
			{
				if (profile != OpusCodec.EncoderProfile.Broadcast)
				{
					if (profile != OpusCodec.EncoderProfile.ProgramStream)
					{
						this.SetNormalQuality();
					}
					else
					{
						this.SetProgramStreamQuality();
					}
				}
				else
				{
					this.SetBroadcastQuality();
				}
				this.lastProfile = profile;
			}
			byte[] encodedBuffer = new byte[1276];
			byte[] result2;
			using (MemoryStream encoded = new MemoryStream())
			{
				try
				{
					int pcmOffset = 0;
					while (pcmOffset < pcmData.Length && pcmOffset + this.FrameSize <= pcmData.Length)
					{
						Span<short> pcmSpan = new Span<short>(pcmData, pcmOffset, this.FrameSize);
						Span<byte> encodedSpan = new Span<byte>(encodedBuffer);
						int encodedLength = this.encoder.Encode(pcmSpan, this.FrameSize / this.Channels, encodedSpan, encodedBuffer.Length);
						if (encodedLength > 0)
						{
							byte[] packetSize = BitConverter.GetBytes(encodedLength);
							encoded.Write(packetSize, 0, packetSize.Length);
							encoded.Write(encodedBuffer, 0, encodedLength);
						}
						pcmOffset += this.FrameSize;
					}
				}
				catch (Exception e)
				{
					Logger client = Logger.client;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Couldn't encode audio:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					client.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				byte[] result = encoded.ToArray();
				if (result.Length == 0)
				{
					result2 = new byte[0];
				}
				else
				{
					result2 = result;
				}
			}
			return result2;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0003298C File Offset: 0x00030B8C
		public byte[] Decode(byte[] encodedData)
		{
			if (encodedData == null || encodedData.Length == 0)
			{
				return new byte[0];
			}
			int samplesPerFrame = this.FrameSize / this.Channels;
			if (samplesPerFrame <= 0)
			{
				Logger client = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(65, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[OpusCodec] Invalid frame size calculation: FrameSize=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.FrameSize);
				defaultInterpolatedStringHandler.AppendLiteral(", Channels=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Channels);
				client.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return new byte[0];
			}
			short[] decodedBuffer = new short[this.FrameSize];
			byte[] result;
			using (MemoryStream decoded = new MemoryStream())
			{
				using (MemoryStream stream = new MemoryStream(encodedData))
				{
					using (BinaryReader reader = new BinaryReader(stream))
					{
						long streamLength = stream.Length;
						try
						{
							while (stream.Position < streamLength)
							{
								long remainingBytes = streamLength - stream.Position;
								if (remainingBytes < 4L)
								{
									break;
								}
								int packetSize = reader.ReadInt32();
								if (packetSize <= 0 || packetSize > 1276)
								{
									break;
								}
								remainingBytes -= 4L;
								if (remainingBytes < (long)packetSize)
								{
									break;
								}
								byte[] encodedPacket = reader.ReadBytes(packetSize);
								if (encodedPacket.Length != 0 && encodedPacket.Length == packetSize)
								{
									ReadOnlySpan<byte> encodedSpan = new ReadOnlySpan<byte>(encodedPacket);
									Span<short> decodedSpan = new Span<short>(decodedBuffer);
									int decodedLength = this.decoder.Decode(encodedSpan, decodedSpan, samplesPerFrame, false);
									if (decodedLength > 0 && decodedLength <= samplesPerFrame)
									{
										byte[] decodedPacket = AudioUtils.ShortsToBytes(decodedBuffer, 0, decodedLength * this.Channels);
										decoded.Write(decodedPacket, 0, decodedPacket.Length);
									}
								}
							}
						}
						catch (OpusException)
						{
						}
						catch (Exception e)
						{
							Logger client2 = Logger.client;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(23, 1);
							defaultInterpolatedStringHandler2.AppendLiteral("Couldn't decode audio:\n");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(e);
							client2.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
						result = decoded.ToArray();
					}
				}
			}
			return result;
		}

		// Token: 0x040003FA RID: 1018
		public const string _Name = "Opus";

		// Token: 0x040003FF RID: 1023
		private IOpusEncoder encoder;

		// Token: 0x04000400 RID: 1024
		private IOpusDecoder decoder;

		// Token: 0x04000401 RID: 1025
		private OpusCodec.EncoderProfile lastProfile;

		// Token: 0x0200018A RID: 394
		private enum EncoderProfile
		{
			// Token: 0x04000629 RID: 1577
			Voice,
			// Token: 0x0400062A RID: 1578
			Broadcast,
			// Token: 0x0400062B RID: 1579
			ProgramStream
		}
	}
}
