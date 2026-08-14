using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Structs
{
	// Token: 0x0200005F RID: 95
	public class OpusRepacketizer
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0001F315 File Offset: 0x0001D515
		public void Reset()
		{
			this.nb_frames = 0;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0001F31E File Offset: 0x0001D51E
		public OpusRepacketizer()
		{
			this.Reset();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0001F354 File Offset: 0x0001D554
		internal unsafe int opus_repacketizer_cat_impl(Span<byte> data, int data_ptr, int len, int self_delimited)
		{
			if (len < 1)
			{
				return -4;
			}
			if (this.nb_frames == 0)
			{
				this.toc = *data[data_ptr];
				this.framesize = OpusPacketInfo.GetNumSamplesPerFrame(data.Slice(data_ptr), 8000);
			}
			else if ((this.toc & 252) != (*data[data_ptr] & 252))
			{
				return -4;
			}
			int numFrames = OpusPacketInfo.GetNumFrames(data.Slice(data_ptr, len));
			if (numFrames < 1)
			{
				return -4;
			}
			if ((numFrames + this.nb_frames) * this.framesize > 960)
			{
				return -4;
			}
			byte b;
			int num2;
			int num = OpusPacketInfo.opus_packet_parse_impl(data, data_ptr, len, self_delimited, out b, this.frames, this.frames_ptrs, this.nb_frames, this.len, this.nb_frames, out num2, out num2);
			if (num < 1)
			{
				return num;
			}
			this.nb_frames += numFrames;
			return 0;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0001F445 File Offset: 0x0001D645
		public int AddPacket(Span<byte> data, int data_offset, int len)
		{
			return this.opus_repacketizer_cat_impl(data, data_offset, len, 0);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0001F451 File Offset: 0x0001D651
		public int GetNumFrames()
		{
			return this.nb_frames;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0001F45C File Offset: 0x0001D65C
		internal unsafe int opus_repacketizer_out_range_impl(int begin, int end, Span<byte> data, int data_ptr, int maxlen, int self_delimited, int pad)
		{
			if (begin < 0 || begin >= end || end > this.nb_frames)
			{
				return -1;
			}
			int num = end - begin;
			int num2 = (self_delimited != 0 && true + (this.len[num - 1] >= 252)) ? 1 : 0;
			int i = data_ptr;
			if (num == 1)
			{
				num2 += (int)(this.len[0] + 1);
				if (num2 > maxlen)
				{
					return -2;
				}
				*data[i++] = (this.toc & 252);
			}
			else if (num == 2)
			{
				if (this.len[1] == this.len[0])
				{
					num2 += (int)(2 * this.len[0] + 1);
					if (num2 > maxlen)
					{
						return -2;
					}
					*data[i++] = ((this.toc & 252) | 1);
				}
				else
				{
					num2 += (int)(this.len[0] + this.len[1] + 2 + ((this.len[0] >= 252) ? 1 : 0));
					if (num2 > maxlen)
					{
						return -2;
					}
					*data[i++] = ((this.toc & 252) | 2);
					i += OpusPacketInfo.encode_size((int)this.len[0], data, i);
				}
			}
			if (num > 2 || (pad != 0 && num2 < maxlen))
			{
				num2 = ((self_delimited != 0 && true + (this.len[num - 1] >= 252)) ? 1 : 0);
				int num3 = 0;
				for (int j = 1; j < num; j++)
				{
					if (this.len[j] != this.len[0])
					{
						num3 = 1;
						break;
					}
				}
				if (num3 != 0)
				{
					num2 += 2;
					for (int j = 0; j < num - 1; j++)
					{
						num2 += (int)(((true + (this.len[j] >= 252)) ? 1 : 0) + this.len[j]);
					}
					num2 += (int)this.len[num - 1];
					if (num2 > maxlen)
					{
						return -2;
					}
					i = data_ptr + 1;
					*data[data_ptr] = ((this.toc & 252) | 3);
					*data[i++] = (byte)(num | 128);
				}
				else
				{
					num2 += num * (int)this.len[0] + 2;
					if (num2 > maxlen)
					{
						return -2;
					}
					i = data_ptr + 1;
					*data[data_ptr] = ((this.toc & 252) | 3);
					*data[i++] = (byte)num;
				}
				int num4 = (pad != 0) ? (maxlen - num2) : 0;
				if (num4 != 0)
				{
					ref byte ptr = data[data_ptr + 1];
					ptr |= 64;
					int num5 = (num4 - 1) / 255;
					for (int j = 0; j < num5; j++)
					{
						*data[i++] = byte.MaxValue;
					}
					*data[i++] = (byte)(num4 - 255 * num5 - 1);
					num2 += num4;
				}
				if (num3 != 0)
				{
					for (int j = 0; j < num - 1; j++)
					{
						i += OpusPacketInfo.encode_size((int)this.len[j], data, i);
					}
				}
			}
			if (self_delimited != 0)
			{
				int num6 = OpusPacketInfo.encode_size((int)this.len[num - 1], data, i);
				i += num6;
			}
			for (int j = begin; j < num + begin; j++)
			{
				MemoryExtensions.AsSpan<byte>(this.frames[j], this.frames_ptrs[j], (int)this.len[j]).CopyTo(data.Slice(i));
				i += (int)this.len[j];
			}
			if (pad != 0)
			{
				while (i < data_ptr + maxlen)
				{
					*data[i++] = 0;
				}
			}
			return num2;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0001F7B7 File Offset: 0x0001D9B7
		public int CreatePacket(int begin, int end, byte[] data, int data_offset, int maxlen)
		{
			return this.opus_repacketizer_out_range_impl(begin, end, data, data_offset, maxlen, 0, 0);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0001F7CD File Offset: 0x0001D9CD
		public int CreatePacket(byte[] data, int data_offset, int maxlen)
		{
			return this.opus_repacketizer_out_range_impl(0, this.nb_frames, data, data_offset, maxlen, 0, 0);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0001F7E8 File Offset: 0x0001D9E8
		public static int PadPacket(Span<byte> data, int data_offset, int len, int new_len)
		{
			OpusRepacketizer opusRepacketizer = new OpusRepacketizer();
			if (len < 1)
			{
				return -1;
			}
			if (len == new_len)
			{
				return 0;
			}
			if (len > new_len)
			{
				return -1;
			}
			opusRepacketizer.Reset();
			Arrays.MemMoveByte(data, data_offset, data_offset + new_len - len, len);
			opusRepacketizer.AddPacket(data, data_offset + new_len - len, len);
			int num = opusRepacketizer.opus_repacketizer_out_range_impl(0, opusRepacketizer.nb_frames, data, data_offset, new_len, 0, 1);
			if (num > 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0001F848 File Offset: 0x0001DA48
		public static int UnpadPacket(byte[] data, int data_offset, int len)
		{
			if (len < 1)
			{
				return -1;
			}
			OpusRepacketizer opusRepacketizer = new OpusRepacketizer();
			opusRepacketizer.Reset();
			int num = opusRepacketizer.AddPacket(data, data_offset, len);
			if (num < 0)
			{
				return num;
			}
			return opusRepacketizer.opus_repacketizer_out_range_impl(0, opusRepacketizer.nb_frames, data, data_offset, len, 0, 0);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0001F898 File Offset: 0x0001DA98
		public static int PadMultistreamPacket(byte[] data, int data_offset, int len, int new_len, int nb_streams)
		{
			short[] array = new short[48];
			if (len < 1)
			{
				return -1;
			}
			if (len == new_len)
			{
				return 0;
			}
			if (len > new_len)
			{
				return -1;
			}
			int num = new_len - len;
			for (int i = 0; i < nb_streams - 1; i++)
			{
				if (len <= 0)
				{
					return -4;
				}
				byte b;
				int num3;
				int num4;
				int num2 = OpusPacketInfo.opus_packet_parse_impl(data, data_offset, len, 1, out b, null, null, 0, array, 0, out num3, out num4);
				if (num2 < 0)
				{
					return num2;
				}
				data_offset += num4;
				len -= num4;
			}
			return OpusRepacketizer.PadPacket(data, data_offset, len, len + num);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0001F924 File Offset: 0x0001DB24
		public static int UnpadMultistreamPacket(byte[] data, int data_offset, int len, int nb_streams)
		{
			short[] array = new short[48];
			OpusRepacketizer opusRepacketizer = new OpusRepacketizer();
			if (len < 1)
			{
				return -1;
			}
			int num = data_offset;
			int num2 = 0;
			for (int i = 0; i < nb_streams; i++)
			{
				int self_delimited = ((i != nb_streams) - true) ? 1 : 0;
				if (len <= 0)
				{
					return -4;
				}
				opusRepacketizer.Reset();
				byte b;
				int num4;
				int num5;
				int num3 = OpusPacketInfo.opus_packet_parse_impl(data, data_offset, len, self_delimited, out b, null, null, 0, array, 0, out num4, out num5);
				if (num3 < 0)
				{
					return num3;
				}
				num3 = opusRepacketizer.opus_repacketizer_cat_impl(data, data_offset, num5, self_delimited);
				if (num3 < 0)
				{
					return num3;
				}
				num3 = opusRepacketizer.opus_repacketizer_out_range_impl(0, opusRepacketizer.nb_frames, data, num, len, self_delimited, 0);
				if (num3 < 0)
				{
					return num3;
				}
				num2 += num3;
				num += num3;
				data_offset += num5;
				len -= num5;
			}
			return num2;
		}

		// Token: 0x04000303 RID: 771
		internal byte toc;

		// Token: 0x04000304 RID: 772
		internal int nb_frames;

		// Token: 0x04000305 RID: 773
		internal readonly byte[][] frames = new byte[48][];

		// Token: 0x04000306 RID: 774
		internal readonly int[] frames_ptrs = new int[48];

		// Token: 0x04000307 RID: 775
		internal readonly short[] len = new short[48];

		// Token: 0x04000308 RID: 776
		internal int framesize;
	}
}
