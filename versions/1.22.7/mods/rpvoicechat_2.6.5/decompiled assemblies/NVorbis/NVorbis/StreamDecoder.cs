using System;
using System.IO;
using System.Text;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000013 RID: 19
	public sealed class StreamDecoder : IStreamDecoder, IDisposable
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000494A File Offset: 0x00002B4A
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00004951 File Offset: 0x00002B51
		internal static Func<IFactory> CreateFactory { get; set; } = () => new Factory();

		// Token: 0x06000082 RID: 130 RVA: 0x00004959 File Offset: 0x00002B59
		public StreamDecoder(NVorbis.Contracts.IPacketProvider packetProvider) : this(packetProvider, new Factory())
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004968 File Offset: 0x00002B68
		internal StreamDecoder(NVorbis.Contracts.IPacketProvider packetProvider, IFactory factory)
		{
			if (packetProvider == null)
			{
				throw new ArgumentNullException("packetProvider");
			}
			this._packetProvider = packetProvider;
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this._factory = factory;
			this._stats = new StreamStats();
			this._currentPosition = 0L;
			this.ClipSamples = true;
			IPacket packet = this._packetProvider.PeekNextPacket();
			if (!this.ProcessHeaderPackets(packet))
			{
				this._packetProvider = null;
				packet.Reset();
				throw StreamDecoder.GetInvalidStreamException(packet);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000049EC File Offset: 0x00002BEC
		private static Exception GetInvalidStreamException(IPacket packet)
		{
			Exception result;
			try
			{
				ulong num = packet.ReadBits(64);
				if (num == 7233173838382854223UL)
				{
					result = new ArgumentException("Found OPUS bitstream.");
				}
				else if ((num & 255UL) == 127UL)
				{
					result = new ArgumentException("Found FLAC bitstream.");
				}
				else if (num == 2314885909937746003UL)
				{
					result = new ArgumentException("Found Speex bitstream.");
				}
				else if (num == 28254585843050854UL)
				{
					result = new ArgumentException("Found Skeleton metadata bitstream.");
				}
				else if ((num & 72057594037927680UL) == 27428895509214208UL)
				{
					result = new ArgumentException("Found Theora bitsream.");
				}
				else
				{
					result = new ArgumentException("Could not find Vorbis data to decode.");
				}
			}
			finally
			{
				packet.Reset();
			}
			return result;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004AB4 File Offset: 0x00002CB4
		private bool ProcessHeaderPackets(IPacket packet)
		{
			if (!StreamDecoder.ProcessHeaderPacket(packet, new Func<IPacket, bool>(this.LoadStreamHeader), delegate(IPacket _)
			{
				this._packetProvider.GetNextPacket().Done();
			}))
			{
				return false;
			}
			if (!StreamDecoder.ProcessHeaderPacket(this._packetProvider.GetNextPacket(), new Func<IPacket, bool>(this.LoadComments), delegate(IPacket pkt)
			{
				pkt.Done();
			}))
			{
				return false;
			}
			if (!StreamDecoder.ProcessHeaderPacket(this._packetProvider.GetNextPacket(), new Func<IPacket, bool>(this.LoadBooks), delegate(IPacket pkt)
			{
				pkt.Done();
			}))
			{
				return false;
			}
			this._currentPosition = 0L;
			this.ResetDecoder();
			return true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004B70 File Offset: 0x00002D70
		private static bool ProcessHeaderPacket(IPacket packet, Func<IPacket, bool> processAction, Action<IPacket> doneAction)
		{
			if (packet != null)
			{
				try
				{
					return processAction.Invoke(packet);
				}
				finally
				{
					doneAction.Invoke(packet);
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004BA8 File Offset: 0x00002DA8
		private static bool ValidateHeader(IPacket packet, byte[] expected)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				if ((ulong)expected[i] != packet.ReadBits(8))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004BD4 File Offset: 0x00002DD4
		private static string ReadString(IPacket packet)
		{
			int num = (int)packet.ReadBits(32);
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			if (packet.Read(array, 0, num) < num)
			{
				throw new InvalidDataException("Could not read full string!");
			}
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004C20 File Offset: 0x00002E20
		private bool LoadStreamHeader(IPacket packet)
		{
			if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureStream))
			{
				return false;
			}
			this._channels = (byte)packet.ReadBits(8);
			this._sampleRate = (int)packet.ReadBits(32);
			this.UpperBitrate = (int)packet.ReadBits(32);
			this.NominalBitrate = (int)packet.ReadBits(32);
			this.LowerBitrate = (int)packet.ReadBits(32);
			this._block0Size = 1 << (int)packet.ReadBits(4);
			this._block1Size = 1 << (int)packet.ReadBits(4);
			if (this.NominalBitrate == 0 && this.UpperBitrate > 0 && this.LowerBitrate > 0)
			{
				this.NominalBitrate = (this.UpperBitrate + this.LowerBitrate) / 2;
			}
			this._stats.SetSampleRate(this._sampleRate);
			this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
			return true;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004D0C File Offset: 0x00002F0C
		private bool LoadComments(IPacket packet)
		{
			if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureComments))
			{
				return false;
			}
			this._vendor = StreamDecoder.ReadString(packet);
			this._comments = new string[packet.ReadBits(32)];
			for (int i = 0; i < this._comments.Length; i++)
			{
				this._comments[i] = StreamDecoder.ReadString(packet);
			}
			this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
			return true;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004D88 File Offset: 0x00002F88
		private bool LoadBooks(IPacket packet)
		{
			if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureBooks))
			{
				return false;
			}
			IMdct mdct = this._factory.CreateMdct();
			IHuffman huffman = this._factory.CreateHuffman();
			ICodebook[] array = new ICodebook[packet.ReadBits(8) + 1UL];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this._factory.CreateCodebook();
				array[i].Init(packet, huffman);
			}
			int num = (int)packet.ReadBits(6) + 1;
			packet.SkipBits(16 * num);
			IFloor[] array2 = new IFloor[packet.ReadBits(6) + 1UL];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this._factory.CreateFloor(packet);
				array2[j].Init(packet, (int)this._channels, this._block0Size, this._block1Size, array);
			}
			IResidue[] array3 = new IResidue[packet.ReadBits(6) + 1UL];
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k] = this._factory.CreateResidue(packet);
				array3[k].Init(packet, (int)this._channels, array);
			}
			IMapping[] array4 = new IMapping[packet.ReadBits(6) + 1UL];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = this._factory.CreateMapping(packet);
				array4[l].Init(packet, (int)this._channels, array2, array3, mdct);
			}
			this._modes = new IMode[packet.ReadBits(6) + 1UL];
			for (int m = 0; m < this._modes.Length; m++)
			{
				this._modes[m] = this._factory.CreateMode();
				this._modes[m].Init(packet, (int)this._channels, this._block0Size, this._block1Size, array4);
			}
			if (!packet.ReadBit())
			{
				throw new InvalidDataException("Book packet did not end on correct bit!");
			}
			this._modeFieldBits = Utils.ilog(this._modes.Length - 1);
			this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
			return true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004FA1 File Offset: 0x000031A1
		private void ResetDecoder()
		{
			this._prevPacketBuf = null;
			this._prevPacketStart = 0;
			this._prevPacketEnd = 0;
			this._prevPacketStop = 0;
			this._nextPacketBuf = null;
			this._eosFound = false;
			this._hasClipped = false;
			this._hasPosition = false;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004FDC File Offset: 0x000031DC
		public int Read(Span<float> buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count % (int)this._channels != 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must be a multiple of Channels!");
			}
			if (this._packetProvider == null)
			{
				throw new ObjectDisposedException("StreamDecoder");
			}
			if (count == 0)
			{
				return 0;
			}
			int i = offset;
			int num = offset + count;
			while (i < num)
			{
				if (this._prevPacketStart == this._prevPacketEnd)
				{
					if (this._eosFound)
					{
						this._nextPacketBuf = null;
						this._prevPacketBuf = null;
						break;
					}
					long? num2;
					if (!this.ReadNextPacket((i - offset) / (int)this._channels, out num2))
					{
						this._prevPacketEnd = this._prevPacketStop;
					}
					if (num2 != null && !this._hasPosition)
					{
						this._hasPosition = true;
						this._currentPosition = num2.Value - (long)(this._prevPacketEnd - this._prevPacketStart) - (long)((i - offset) / (int)this._channels);
					}
				}
				int num3 = Math.Min((num - i) / (int)this._channels, this._prevPacketEnd - this._prevPacketStart);
				if (num3 > 0)
				{
					if (this.ClipSamples)
					{
						i += this.ClippingCopyBuffer(buffer, i, num3);
					}
					else
					{
						i += this.CopyBuffer(buffer, i, num3);
					}
				}
			}
			count = i - offset;
			this._currentPosition += (long)(count / (int)this._channels);
			return count;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000514C File Offset: 0x0000334C
		private unsafe int ClippingCopyBuffer(Span<float> target, int targetIndex, int count)
		{
			int num = targetIndex;
			while (count > 0)
			{
				for (int i = 0; i < (int)this._channels; i++)
				{
					*target[num++] = Utils.ClipValue(this._prevPacketBuf[i][this._prevPacketStart], ref this._hasClipped);
				}
				this._prevPacketStart++;
				count--;
			}
			return num - targetIndex;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000051B0 File Offset: 0x000033B0
		private unsafe int CopyBuffer(Span<float> target, int targetIndex, int count)
		{
			int num = targetIndex;
			while (count > 0)
			{
				for (int i = 0; i < (int)this._channels; i++)
				{
					*target[num++] = this._prevPacketBuf[i][this._prevPacketStart];
				}
				this._prevPacketStart++;
				count--;
			}
			return num - targetIndex;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005208 File Offset: 0x00003408
		private bool ReadNextPacket(int bufferedSamples, out long? samplePosition)
		{
			int num;
			int num2;
			int prevPacketStop;
			bool flag;
			int bits;
			int waste;
			int container;
			float[][] array = this.DecodeNextPacket(out num, out num2, out prevPacketStop, out flag, out samplePosition, out bits, out waste, out container);
			this._eosFound = (this._eosFound || flag);
			if (array == null)
			{
				this._stats.AddPacket(0, bits, waste, container);
				return false;
			}
			if (samplePosition != null && flag)
			{
				long num3 = this._currentPosition + (long)bufferedSamples + (long)num2 - (long)num;
				int num4 = (int)(samplePosition.Value - num3);
				if (num4 < 0)
				{
					num2 += num4;
				}
			}
			if (this._prevPacketEnd > 0)
			{
				StreamDecoder.OverlapBuffers(this._prevPacketBuf, array, this._prevPacketStart, this._prevPacketStop, num, (int)this._channels);
				this._prevPacketStart = num;
			}
			else if (this._prevPacketBuf == null)
			{
				this._prevPacketStart = num2;
			}
			this._stats.AddPacket(num2 - this._prevPacketStart, bits, waste, container);
			this._nextPacketBuf = this._prevPacketBuf;
			this._prevPacketEnd = num2;
			this._prevPacketStop = prevPacketStop;
			this._prevPacketBuf = array;
			return true;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005300 File Offset: 0x00003500
		private float[][] DecodeNextPacket(out int packetStartindex, out int packetValidLength, out int packetTotalLength, out bool isEndOfStream, out long? samplePosition, out int bitsRead, out int bitsRemaining, out int containerOverheadBits)
		{
			IPacket packet = null;
			float[][] result;
			try
			{
				if ((packet = this._packetProvider.GetNextPacket()) == null)
				{
					isEndOfStream = true;
				}
				else
				{
					isEndOfStream = packet.IsEndOfStream;
					if (packet.IsResync)
					{
						this._hasPosition = false;
					}
					containerOverheadBits = packet.ContainerOverheadBits;
					if (packet.ReadBit())
					{
						bitsRemaining = packet.BitsRemaining + 1;
					}
					else
					{
						IMode mode = this._modes[(int)packet.ReadBits(this._modeFieldBits)];
						if (this._nextPacketBuf == null)
						{
							this._nextPacketBuf = new float[(int)this._channels][];
							for (int i = 0; i < (int)this._channels; i++)
							{
								this._nextPacketBuf[i] = new float[this._block1Size];
							}
						}
						if (mode.Decode(packet, this._nextPacketBuf, out packetStartindex, out packetValidLength, out packetTotalLength))
						{
							samplePosition = packet.GranulePosition;
							bitsRead = packet.BitsRead;
							bitsRemaining = packet.BitsRemaining;
							return this._nextPacketBuf;
						}
						bitsRemaining = packet.BitsRead + packet.BitsRemaining;
					}
				}
				packetStartindex = 0;
				packetValidLength = 0;
				packetTotalLength = 0;
				samplePosition = default(long?);
				bitsRead = 0;
				bitsRemaining = 0;
				containerOverheadBits = 0;
				result = null;
			}
			finally
			{
				if (packet != null)
				{
					packet.Done();
				}
			}
			return result;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005444 File Offset: 0x00003644
		private static void OverlapBuffers(float[][] previous, float[][] next, int prevStart, int prevLen, int nextStart, int channels)
		{
			while (prevStart < prevLen)
			{
				for (int i = 0; i < channels; i++)
				{
					next[i][nextStart] += previous[i][prevStart];
				}
				prevStart++;
				nextStart++;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005482 File Offset: 0x00003682
		public void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0)
		{
			this.SeekTo((long)((double)this.SampleRate * timePosition.TotalSeconds), seekOrigin);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000549C File Offset: 0x0000369C
		public void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0)
		{
			if (this._packetProvider == null)
			{
				throw new ObjectDisposedException("StreamDecoder");
			}
			if (!this._packetProvider.CanSeek)
			{
				throw new InvalidOperationException("Seek is not supported by the Contracts.IPacketProvider instance.");
			}
			switch (seekOrigin)
			{
			case 0:
				break;
			case 1:
				samplePosition = this.SamplePosition - samplePosition;
				break;
			case 2:
				samplePosition = this.TotalSamples - samplePosition;
				break;
			default:
				throw new ArgumentOutOfRangeException("seekOrigin");
			}
			if (samplePosition < 0L)
			{
				throw new ArgumentOutOfRangeException("samplePosition");
			}
			int num;
			if (samplePosition == 0L)
			{
				this._packetProvider.SeekTo(0L, 0, new GetPacketGranuleCount(this.GetPacketGranules));
				num = 0;
			}
			else
			{
				long num2 = this._packetProvider.SeekTo(samplePosition, 1, new GetPacketGranuleCount(this.GetPacketGranules));
				num = (int)(samplePosition - num2);
			}
			this.ResetDecoder();
			this._hasPosition = true;
			long? num3;
			if (!this.ReadNextPacket(0, out num3))
			{
				this._eosFound = true;
				if (this._packetProvider.GetGranuleCount() != samplePosition)
				{
					throw new InvalidOperationException("Could not read pre-roll packet!  Try seeking again prior to reading more samples.");
				}
				this._prevPacketStart = this._prevPacketStop;
				this._currentPosition = samplePosition;
				return;
			}
			else
			{
				if (!this.ReadNextPacket(0, out num3))
				{
					this.ResetDecoder();
					this._eosFound = true;
					throw new InvalidOperationException("Could not read pre-roll packet!  Try seeking again prior to reading more samples.");
				}
				this._prevPacketStart += num;
				this._currentPosition = samplePosition;
				return;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000055E0 File Offset: 0x000037E0
		private int GetPacketGranules(IPacket curPacket)
		{
			if (curPacket.IsResync)
			{
				return 0;
			}
			if (curPacket.ReadBit())
			{
				return 0;
			}
			int num = (int)curPacket.ReadBits(this._modeFieldBits);
			if (num < 0 || num >= this._modes.Length)
			{
				return 0;
			}
			return this._modes[num].GetPacketSampleCount(curPacket);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000562E File Offset: 0x0000382E
		public void Dispose()
		{
			IDisposable disposable = this._packetProvider as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			this._packetProvider = null;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000564D File Offset: 0x0000384D
		public int Channels
		{
			get
			{
				return (int)this._channels;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00005655 File Offset: 0x00003855
		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000565D File Offset: 0x0000385D
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00005665 File Offset: 0x00003865
		public int UpperBitrate { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000566E File Offset: 0x0000386E
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00005676 File Offset: 0x00003876
		public int NominalBitrate { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000567F File Offset: 0x0000387F
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00005687 File Offset: 0x00003887
		public int LowerBitrate { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00005690 File Offset: 0x00003890
		public ITagData Tags
		{
			get
			{
				ITagData result;
				if ((result = this._tags) == null)
				{
					result = (this._tags = new TagData(this._vendor, this._comments));
				}
				return result;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000056C1 File Offset: 0x000038C1
		public TimeSpan TotalTime
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.TotalSamples / (double)this._sampleRate);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000056D7 File Offset: 0x000038D7
		public long TotalSamples
		{
			get
			{
				NVorbis.Contracts.IPacketProvider packetProvider = this._packetProvider;
				if (packetProvider == null)
				{
					throw new ObjectDisposedException("StreamDecoder");
				}
				return packetProvider.GetGranuleCount();
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000056F3 File Offset: 0x000038F3
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00005709 File Offset: 0x00003909
		public TimeSpan TimePosition
		{
			get
			{
				return TimeSpan.FromSeconds((double)this._currentPosition / (double)this._sampleRate);
			}
			set
			{
				this.SeekTo(value, 0);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00005713 File Offset: 0x00003913
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000571B File Offset: 0x0000391B
		public long SamplePosition
		{
			get
			{
				return this._currentPosition;
			}
			set
			{
				this.SeekTo(value, 0);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00005725 File Offset: 0x00003925
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000572D File Offset: 0x0000392D
		public bool ClipSamples { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005736 File Offset: 0x00003936
		public bool HasClipped
		{
			get
			{
				return this._hasClipped;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000573E File Offset: 0x0000393E
		public bool IsEndOfStream
		{
			get
			{
				return this._eosFound && this._prevPacketBuf == null;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005753 File Offset: 0x00003953
		public IStreamStats Stats
		{
			get
			{
				return this._stats;
			}
		}

		// Token: 0x0400004E RID: 78
		private NVorbis.Contracts.IPacketProvider _packetProvider;

		// Token: 0x0400004F RID: 79
		private IFactory _factory;

		// Token: 0x04000050 RID: 80
		private StreamStats _stats;

		// Token: 0x04000051 RID: 81
		private byte _channels;

		// Token: 0x04000052 RID: 82
		private int _sampleRate;

		// Token: 0x04000053 RID: 83
		private int _block0Size;

		// Token: 0x04000054 RID: 84
		private int _block1Size;

		// Token: 0x04000055 RID: 85
		private IMode[] _modes;

		// Token: 0x04000056 RID: 86
		private int _modeFieldBits;

		// Token: 0x04000057 RID: 87
		private string _vendor;

		// Token: 0x04000058 RID: 88
		private string[] _comments;

		// Token: 0x04000059 RID: 89
		private ITagData _tags;

		// Token: 0x0400005A RID: 90
		private long _currentPosition;

		// Token: 0x0400005B RID: 91
		private bool _hasClipped;

		// Token: 0x0400005C RID: 92
		private bool _hasPosition;

		// Token: 0x0400005D RID: 93
		private bool _eosFound;

		// Token: 0x0400005E RID: 94
		private float[][] _nextPacketBuf;

		// Token: 0x0400005F RID: 95
		private float[][] _prevPacketBuf;

		// Token: 0x04000060 RID: 96
		private int _prevPacketStart;

		// Token: 0x04000061 RID: 97
		private int _prevPacketEnd;

		// Token: 0x04000062 RID: 98
		private int _prevPacketStop;

		// Token: 0x04000063 RID: 99
		private static readonly byte[] PacketSignatureStream = new byte[]
		{
			1,
			118,
			111,
			114,
			98,
			105,
			115,
			0,
			0,
			0,
			0
		};

		// Token: 0x04000064 RID: 100
		private static readonly byte[] PacketSignatureComments = new byte[]
		{
			3,
			118,
			111,
			114,
			98,
			105,
			115
		};

		// Token: 0x04000065 RID: 101
		private static readonly byte[] PacketSignatureBooks = new byte[]
		{
			5,
			118,
			111,
			114,
			98,
			105,
			115
		};
	}
}
