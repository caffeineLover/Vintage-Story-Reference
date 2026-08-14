using System;
using System.IO;
using System.Runtime.CompilerServices;
using Concentus;
using RPVoiceChat.Config;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C5 RID: 197
	public class OpusCodec : IAudioCodec
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00027183 File Offset: 0x00025383
		public string Name { get; } = "Opus";

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002718B File Offset: 0x0002538B
		public int SampleRate { get; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00027193 File Offset: 0x00025393
		public int Channels { get; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002719B File Offset: 0x0002539B
		public int FrameSize { get; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x000271A3 File Offset: 0x000253A3
		private int NormalBitrate
		{
			get
			{
				return ServerConfigManager.NormalBitrate;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000271AA File Offset: 0x000253AA
		private int BroadcastBitrate
		{
			get
			{
				return ServerConfigManager.BroadcastBitrate;
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000271B4 File Offset: 0x000253B4
		public OpusCodec(int frequency, int channelCount)
		{
			this.SampleRate = frequency;
			this.Channels = channelCount;
			this.FrameSize = this.SampleRate / 100 * this.Channels;
			this.encoder = OpusCodecFactory.CreateEncoder(this.SampleRate, this.Channels, 2048, null);
			this.decoder = OpusCodecFactory.CreateDecoder(this.SampleRate, this.Channels, null);
			this.SetNormalQuality();
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00027234 File Offset: 0x00025434
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

		// Token: 0x060006F3 RID: 1779 RVA: 0x000272A4 File Offset: 0x000254A4
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

		// Token: 0x060006F4 RID: 1780 RVA: 0x00027312 File Offset: 0x00025512
		public byte[] Encode(short[] pcmData)
		{
			return this.EncodeInternal(pcmData, false);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002731C File Offset: 0x0002551C
		public byte[] EncodeForBroadcast(short[] pcmData)
		{
			return this.EncodeInternal(pcmData, true);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00027328 File Offset: 0x00025528
		private byte[] EncodeInternal(short[] pcmData, bool isBroadcast)
		{
			if (this.lastWasBroadcast != isBroadcast)
			{
				if (isBroadcast)
				{
					this.SetBroadcastQuality();
				}
				else
				{
					this.SetNormalQuality();
				}
				this.lastWasBroadcast = isBroadcast;
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

		// Token: 0x060006F7 RID: 1783 RVA: 0x00027464 File Offset: 0x00025664
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

		// Token: 0x0400031B RID: 795
		public const string _Name = "Opus";

		// Token: 0x04000320 RID: 800
		private IOpusEncoder encoder;

		// Token: 0x04000321 RID: 801
		private IOpusDecoder decoder;

		// Token: 0x04000322 RID: 802
		private bool lastWasBroadcast;
	}
}
