using System;
using System.Collections.Generic;
using Concentus.Common.CPlusPlus;
using Concentus.Enums;

namespace Concentus.Structs
{
	// Token: 0x0200005E RID: 94
	public class OpusPacketInfo
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0001EA1F File Offset: 0x0001CC1F
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0001EA27 File Offset: 0x0001CC27
		public byte TOCByte { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0001EA30 File Offset: 0x0001CC30
		// (set) Token: 0x06000223 RID: 547 RVA: 0x0001EA38 File Offset: 0x0001CC38
		public IList<byte[]> Frames { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0001EA41 File Offset: 0x0001CC41
		// (set) Token: 0x06000225 RID: 549 RVA: 0x0001EA49 File Offset: 0x0001CC49
		public int PayloadOffset { get; private set; }

		// Token: 0x06000226 RID: 550 RVA: 0x0001EA52 File Offset: 0x0001CC52
		private OpusPacketInfo(byte toc, IList<byte[]> frames, int payloadOffset)
		{
			this.TOCByte = toc;
			this.Frames = frames;
			this.PayloadOffset = payloadOffset;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0001EA6F File Offset: 0x0001CC6F
		[Obsolete("Use Span<> overrides if possible")]
		public static OpusPacketInfo ParseOpusPacket(byte[] packet, int packet_offset, int len)
		{
			return OpusPacketInfo.ParseOpusPacket(MemoryExtensions.AsSpan<byte>(packet, packet_offset, len));
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0001EA84 File Offset: 0x0001CC84
		public static OpusPacketInfo ParseOpusPacket(ReadOnlySpan<byte> packet)
		{
			int numFrames = OpusPacketInfo.GetNumFrames(packet);
			byte[][] array = new byte[numFrames][];
			int[] array2 = new int[numFrames];
			short[] array3 = new short[numFrames];
			byte toc;
			int payloadOffset;
			int num2;
			int num = OpusPacketInfo.opus_packet_parse_impl(packet, 0, packet.Length, 0, out toc, array, array2, 0, array3, 0, out payloadOffset, out num2);
			if (num < 0)
			{
				throw new OpusException("An error occurred while parsing the packet", num);
			}
			IList<byte[]> list = new List<byte[]>();
			for (int i = 0; i < numFrames; i++)
			{
				byte[] array4 = new byte[(int)array3[i]];
				MemoryExtensions.AsSpan<byte>(array[i], array2[i], array4.Length).CopyTo(array4);
				list.Add(array4);
			}
			return new OpusPacketInfo(toc, list, payloadOffset);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0001EB40 File Offset: 0x0001CD40
		public int NumSamplesPerFrame(int Fs)
		{
			int num;
			if ((this.TOCByte & 128) != 0)
			{
				num = (this.TOCByte >> 3 & 3);
				num = (Fs << num) / 400;
			}
			else if ((this.TOCByte & 96) == 96)
			{
				num = (((this.TOCByte & 8) != 0) ? (Fs / 50) : (Fs / 100));
			}
			else
			{
				num = (this.TOCByte >> 3 & 3);
				if (num == 3)
				{
					num = Fs * 60 / 1000;
				}
				else
				{
					num = (Fs << num) / 100;
				}
			}
			return num;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0001EBC0 File Offset: 0x0001CDC0
		public unsafe static int GetNumSamplesPerFrame(ReadOnlySpan<byte> packet, int Fs)
		{
			int num;
			if ((*packet[0] & 128) != 0)
			{
				num = (*packet[0] >> 3 & 3);
				num = (Fs << num) / 400;
			}
			else if ((*packet[0] & 96) == 96)
			{
				num = (((*packet[0] & 8) != 0) ? (Fs / 50) : (Fs / 100));
			}
			else
			{
				num = (*packet[0] >> 3 & 3);
				if (num == 3)
				{
					num = Fs * 60 / 1000;
				}
				else
				{
					num = (Fs << num) / 100;
				}
			}
			return num;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0001EC50 File Offset: 0x0001CE50
		public OpusBandwidth Bandwidth
		{
			get
			{
				OpusBandwidth opusBandwidth;
				if ((this.TOCByte & 128) != 0)
				{
					opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND + (this.TOCByte >> 5 & 3);
					if (opusBandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
					{
						opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
					}
				}
				else if ((this.TOCByte & 96) == 96)
				{
					opusBandwidth = (((this.TOCByte & 16) != 0) ? OpusBandwidth.OPUS_BANDWIDTH_FULLBAND : OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND);
				}
				else
				{
					opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND + (this.TOCByte >> 5 & 3);
				}
				return opusBandwidth;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0001ECC8 File Offset: 0x0001CEC8
		public unsafe static OpusBandwidth GetBandwidth(ReadOnlySpan<byte> packet)
		{
			OpusBandwidth opusBandwidth;
			if ((*packet[0] & 128) != 0)
			{
				opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND + (*packet[0] >> 5 & 3);
				if (opusBandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
				{
					opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
				}
			}
			else if ((*packet[0] & 96) == 96)
			{
				opusBandwidth = (((*packet[0] & 16) != 0) ? OpusBandwidth.OPUS_BANDWIDTH_FULLBAND : OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND);
			}
			else
			{
				opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND + (*packet[0] >> 5 & 3);
			}
			return opusBandwidth;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0001ED4C File Offset: 0x0001CF4C
		public int NumEncodedChannels
		{
			get
			{
				if ((this.TOCByte & 4) == 0)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0001ED5B File Offset: 0x0001CF5B
		public unsafe static int GetNumEncodedChannels(ReadOnlySpan<byte> packet)
		{
			if ((*packet[0] & 4) == 0)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001ED70 File Offset: 0x0001CF70
		public unsafe static int GetNumFrames(ReadOnlySpan<byte> packet)
		{
			if (packet.Length < 1)
			{
				return -1;
			}
			int num = (int)(*packet[0] & 3);
			if (num == 0)
			{
				return 1;
			}
			if (num != 3)
			{
				return 2;
			}
			if (packet.Length < 2)
			{
				return -4;
			}
			return (int)(*packet[1] & 63);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001EDBC File Offset: 0x0001CFBC
		public static int GetNumSamples(ReadOnlySpan<byte> packet, int Fs)
		{
			int numFrames = OpusPacketInfo.GetNumFrames(packet);
			if (numFrames < 0)
			{
				return numFrames;
			}
			int num = numFrames * OpusPacketInfo.GetNumSamplesPerFrame(packet, Fs);
			if (num * 25 > Fs * 3)
			{
				return -4;
			}
			return num;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001EDED File Offset: 0x0001CFED
		[Obsolete("Use Span<> overrides if possible")]
		public static int GetNumSamples(OpusDecoder dec, byte[] packet, int packet_offset, int len)
		{
			return OpusPacketInfo.GetNumSamples(MemoryExtensions.AsSpan<byte>(packet, packet_offset, len), dec.Fs);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0001EE07 File Offset: 0x0001D007
		public static int GetNumSamples(OpusDecoder dec, ReadOnlySpan<byte> packet)
		{
			return OpusPacketInfo.GetNumSamples(packet, dec.Fs);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0001EE18 File Offset: 0x0001D018
		public unsafe static OpusMode GetEncoderMode(ReadOnlySpan<byte> packet)
		{
			OpusMode result;
			if ((*packet[0] & 128) != 0)
			{
				result = OpusMode.MODE_CELT_ONLY;
			}
			else if ((*packet[0] & 96) == 96)
			{
				result = OpusMode.MODE_HYBRID;
			}
			else
			{
				result = OpusMode.MODE_SILK_ONLY;
			}
			return result;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0001EE60 File Offset: 0x0001D060
		public OpusMode EncoderMode
		{
			get
			{
				OpusMode result;
				if ((this.TOCByte & 128) != 0)
				{
					result = OpusMode.MODE_CELT_ONLY;
				}
				else if ((this.TOCByte & 96) == 96)
				{
					result = OpusMode.MODE_HYBRID;
				}
				else
				{
					result = OpusMode.MODE_SILK_ONLY;
				}
				return result;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
		internal unsafe static int encode_size(int size, Span<byte> data, int data_ptr)
		{
			if (size < 252)
			{
				*data[data_ptr] = (byte)size;
				return 1;
			}
			*data[data_ptr] = (byte)(252 + (size & 3));
			*data[data_ptr + 1] = (byte)(size - (int)(*data[data_ptr]) >> 2);
			return 2;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0001EEF0 File Offset: 0x0001D0F0
		internal unsafe static int parse_size(ReadOnlySpan<byte> data, int data_ptr, int len, BoxedValueShort size)
		{
			if (len < 1)
			{
				size.Val = -1;
				return -1;
			}
			if (*data[data_ptr] < 252)
			{
				size.Val = (short)(*data[data_ptr]);
				return 1;
			}
			if (len < 2)
			{
				size.Val = -1;
				return -1;
			}
			size.Val = (short)(4 * *data[data_ptr + 1] + *data[data_ptr]);
			return 2;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001EF58 File Offset: 0x0001D158
		internal unsafe static int opus_packet_parse_impl(ReadOnlySpan<byte> data, int data_ptr, int len, int self_delimited, out byte out_toc, byte[][] frames, Span<int> frames_ptrs, int frames_ptr, Span<short> sizes, int sizes_ptr, out int payload_offset, out int packet_offset)
		{
			int num = 0;
			int num2 = data_ptr;
			out_toc = 0;
			payload_offset = 0;
			packet_offset = 0;
			if (sizes.IsEmpty || len < 0)
			{
				return -1;
			}
			if (len == 0)
			{
				return -4;
			}
			int numSamplesPerFrame = OpusPacketInfo.GetNumSamplesPerFrame(data.Slice(data_ptr), 48000);
			int num3 = 0;
			byte b = *data[data_ptr++];
			len--;
			int num4 = len;
			int num5;
			switch (b & 3)
			{
			case 0:
				num5 = 1;
				break;
			case 1:
				num5 = 2;
				num3 = 1;
				if (self_delimited == 0)
				{
					if ((len & 1) != 0)
					{
						return -4;
					}
					num4 = len / 2;
					*sizes[sizes_ptr] = (short)num4;
				}
				break;
			case 2:
			{
				num5 = 2;
				BoxedValueShort boxedValueShort = new BoxedValueShort(*sizes[sizes_ptr]);
				int num6 = OpusPacketInfo.parse_size(data, data_ptr, len, boxedValueShort);
				*sizes[sizes_ptr] = boxedValueShort.Val;
				len -= num6;
				if (*sizes[sizes_ptr] < 0 || (int)(*sizes[sizes_ptr]) > len)
				{
					return -4;
				}
				data_ptr += num6;
				num4 = len - (int)(*sizes[sizes_ptr]);
				break;
			}
			default:
			{
				if (len < 1)
				{
					return -4;
				}
				byte b2 = *data[data_ptr++];
				num5 = (int)(b2 & 63);
				if (num5 <= 0 || numSamplesPerFrame * num5 > 5760)
				{
					return -4;
				}
				len--;
				if ((b2 & 64) != 0)
				{
					while (len > 0)
					{
						int num7 = (int)(*data[data_ptr++]);
						len--;
						int num8 = (num7 == 255) ? 254 : num7;
						len -= num8;
						num += num8;
						if (num7 != 255)
						{
							goto IL_18D;
						}
					}
					return -4;
				}
				IL_18D:
				if (len < 0)
				{
					return -4;
				}
				num3 = (((b2 & 128) == 0) ? 1 : 0);
				if (num3 == 0)
				{
					num4 = len;
					for (int i = 0; i < num5 - 1; i++)
					{
						BoxedValueShort boxedValueShort = new BoxedValueShort(*sizes[sizes_ptr + i]);
						int num6 = OpusPacketInfo.parse_size(data, data_ptr, len, boxedValueShort);
						*sizes[sizes_ptr + i] = boxedValueShort.Val;
						len -= num6;
						if (*sizes[sizes_ptr + i] < 0 || (int)(*sizes[sizes_ptr + i]) > len)
						{
							return -4;
						}
						data_ptr += num6;
						num4 -= num6 + (int)(*sizes[sizes_ptr + i]);
					}
					if (num4 < 0)
					{
						return -4;
					}
				}
				else if (self_delimited == 0)
				{
					num4 = len / num5;
					if (num4 * num5 != len)
					{
						return -4;
					}
					for (int i = 0; i < num5 - 1; i++)
					{
						*sizes[sizes_ptr + i] = (short)num4;
					}
				}
				break;
			}
			}
			if (self_delimited != 0)
			{
				BoxedValueShort boxedValueShort2 = new BoxedValueShort(*sizes[sizes_ptr + num5 - 1]);
				int num6 = OpusPacketInfo.parse_size(data, data_ptr, len, boxedValueShort2);
				*sizes[sizes_ptr + num5 - 1] = boxedValueShort2.Val;
				len -= num6;
				if (*sizes[sizes_ptr + num5 - 1] < 0 || (int)(*sizes[sizes_ptr + num5 - 1]) > len)
				{
					return -4;
				}
				data_ptr += num6;
				if (num3 != 0)
				{
					if ((int)(*sizes[sizes_ptr + num5 - 1]) * num5 > len)
					{
						return -4;
					}
					for (int i = 0; i < num5 - 1; i++)
					{
						*sizes[sizes_ptr + i] = *sizes[sizes_ptr + num5 - 1];
					}
				}
				else if (num6 + (int)(*sizes[sizes_ptr + num5 - 1]) > num4)
				{
					return -4;
				}
			}
			else
			{
				if (num4 > 1275)
				{
					return -4;
				}
				*sizes[sizes_ptr + num5 - 1] = (short)num4;
			}
			payload_offset = data_ptr - num2;
			for (int i = 0; i < num5; i++)
			{
				if (frames != null)
				{
					byte[] array = new byte[data.Length];
					data.CopyTo(MemoryExtensions.AsSpan<byte>(array));
					frames[frames_ptr + i] = array;
				}
				if (!frames_ptrs.IsEmpty)
				{
					*frames_ptrs[frames_ptr + i] = data_ptr;
				}
				data_ptr += (int)(*sizes[sizes_ptr + i]);
			}
			packet_offset = num + (data_ptr - num2);
			out_toc = b;
			return num5;
		}
	}
}
