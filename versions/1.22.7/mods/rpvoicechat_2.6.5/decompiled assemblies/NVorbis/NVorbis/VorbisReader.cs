using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NVorbis.Contracts;
using NVorbis.Ogg;

namespace NVorbis
{
	// Token: 0x02000017 RID: 23
	public sealed class VorbisReader : IVorbisReader, IDisposable
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005E32 File Offset: 0x00004032
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00005E39 File Offset: 0x00004039
		internal static Func<Stream, bool, NVorbis.Contracts.IContainerReader> CreateContainerReader { get; set; } = (Stream s, bool cod) => new ContainerReader(s, cod);

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005E41 File Offset: 0x00004041
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00005E48 File Offset: 0x00004048
		internal static Func<NVorbis.Contracts.IPacketProvider, IStreamDecoder> CreateStreamDecoder { get; set; } = (NVorbis.Contracts.IPacketProvider pp) => new StreamDecoder(pp, new Factory());

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000D6 RID: 214 RVA: 0x00005E50 File Offset: 0x00004050
		// (remove) Token: 0x060000D7 RID: 215 RVA: 0x00005E88 File Offset: 0x00004088
		public event EventHandler<NewStreamEventArgs> NewStream;

		// Token: 0x060000D8 RID: 216 RVA: 0x00005EBD File Offset: 0x000040BD
		public VorbisReader(string fileName) : this(File.OpenRead(fileName), true)
		{
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005ECC File Offset: 0x000040CC
		public VorbisReader(Stream stream, bool closeOnDispose = true)
		{
			this._decoders = new List<IStreamDecoder>();
			NVorbis.Contracts.IContainerReader containerReader = VorbisReader.CreateContainerReader.Invoke(stream, closeOnDispose);
			containerReader.NewStreamCallback = new NewStreamHandler(this.ProcessNewStream);
			if (!containerReader.TryInit() || this._decoders.Count == 0)
			{
				containerReader.NewStreamCallback = null;
				containerReader.Dispose();
				if (closeOnDispose)
				{
					stream.Dispose();
				}
				throw new ArgumentException("Could not load the specified container!", "containerReader");
			}
			this._closeOnDispose = closeOnDispose;
			this._containerReader = containerReader;
			this._streamDecoder = this._decoders[0];
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005F64 File Offset: 0x00004164
		[Obsolete("Use \"new StreamDecoder(Contracts.IPacketProvider)\" and the container's NewStreamCallback or Streams property instead.", true)]
		public VorbisReader(NVorbis.Contracts.IContainerReader containerReader)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005F71 File Offset: 0x00004171
		[Obsolete("Use \"new StreamDecoder(Contracts.IPacketProvider)\" instead.", true)]
		public VorbisReader(NVorbis.Contracts.IPacketProvider packetProvider)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005F80 File Offset: 0x00004180
		private bool ProcessNewStream(NVorbis.Contracts.IPacketProvider packetProvider)
		{
			IStreamDecoder streamDecoder = VorbisReader.CreateStreamDecoder.Invoke(packetProvider);
			streamDecoder.ClipSamples = true;
			NewStreamEventArgs newStreamEventArgs = new NewStreamEventArgs(streamDecoder);
			EventHandler<NewStreamEventArgs> newStream = this.NewStream;
			if (newStream != null)
			{
				newStream.Invoke(this, newStreamEventArgs);
			}
			if (!newStreamEventArgs.IgnoreStream)
			{
				this._decoders.Add(streamDecoder);
				return true;
			}
			return false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005FD4 File Offset: 0x000041D4
		public void Dispose()
		{
			if (this._decoders != null)
			{
				foreach (IStreamDecoder streamDecoder in this._decoders)
				{
					streamDecoder.Dispose();
				}
				this._decoders.Clear();
			}
			if (this._containerReader != null)
			{
				this._containerReader.NewStreamCallback = null;
				if (this._closeOnDispose)
				{
					this._containerReader.Dispose();
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00006060 File Offset: 0x00004260
		public IReadOnlyList<IStreamDecoder> Streams
		{
			get
			{
				return this._decoders;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00006068 File Offset: 0x00004268
		public int Channels
		{
			get
			{
				return this._streamDecoder.Channels;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006075 File Offset: 0x00004275
		public int SampleRate
		{
			get
			{
				return this._streamDecoder.SampleRate;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00006082 File Offset: 0x00004282
		public int UpperBitrate
		{
			get
			{
				return this._streamDecoder.UpperBitrate;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000608F File Offset: 0x0000428F
		public int NominalBitrate
		{
			get
			{
				return this._streamDecoder.NominalBitrate;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000609C File Offset: 0x0000429C
		public int LowerBitrate
		{
			get
			{
				return this._streamDecoder.LowerBitrate;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000060A9 File Offset: 0x000042A9
		public ITagData Tags
		{
			get
			{
				return this._streamDecoder.Tags;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000060B6 File Offset: 0x000042B6
		[Obsolete("Use .Tags.EncoderVendor instead.")]
		public string Vendor
		{
			get
			{
				return this._streamDecoder.Tags.EncoderVendor;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000060C8 File Offset: 0x000042C8
		[Obsolete("Use .Tags.All instead.")]
		public string[] Comments
		{
			get
			{
				return Enumerable.ToArray<string>(Enumerable.SelectMany<KeyValuePair<string, IReadOnlyList<string>>, string, string>(this._streamDecoder.Tags.All, (KeyValuePair<string, IReadOnlyList<string>> k) => k.Value, (KeyValuePair<string, IReadOnlyList<string>> kvp, string Item) => kvp.Key + "=" + Item));
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000612D File Offset: 0x0000432D
		[Obsolete("No longer supported.  Will receive a new stream when parameters change.", true)]
		public bool IsParameterChange
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00006134 File Offset: 0x00004334
		public long ContainerOverheadBits
		{
			get
			{
				NVorbis.Contracts.IContainerReader containerReader = this._containerReader;
				if (containerReader == null)
				{
					return 0L;
				}
				return containerReader.ContainerBits;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00006148 File Offset: 0x00004348
		public long ContainerWasteBits
		{
			get
			{
				NVorbis.Contracts.IContainerReader containerReader = this._containerReader;
				if (containerReader == null)
				{
					return 0L;
				}
				return containerReader.WasteBits;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000615C File Offset: 0x0000435C
		public int StreamIndex
		{
			get
			{
				return this._decoders.IndexOf(this._streamDecoder);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EB RID: 235 RVA: 0x0000616F File Offset: 0x0000436F
		[Obsolete("Use .Streams.Count instead.")]
		public int StreamCount
		{
			get
			{
				return this._decoders.Count;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000617C File Offset: 0x0000437C
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00006189 File Offset: 0x00004389
		[Obsolete("Use VorbisReader.TimePosition instead.")]
		public TimeSpan DecodedTime
		{
			get
			{
				return this._streamDecoder.TimePosition;
			}
			set
			{
				this.TimePosition = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00006192 File Offset: 0x00004392
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000619F File Offset: 0x0000439F
		[Obsolete("Use VorbisReader.SamplePosition instead.")]
		public long DecodedPosition
		{
			get
			{
				return this._streamDecoder.SamplePosition;
			}
			set
			{
				this.SamplePosition = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000061A8 File Offset: 0x000043A8
		public TimeSpan TotalTime
		{
			get
			{
				return this._streamDecoder.TotalTime;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x000061B5 File Offset: 0x000043B5
		public long TotalSamples
		{
			get
			{
				return this._streamDecoder.TotalSamples;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000061C2 File Offset: 0x000043C2
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000061CF File Offset: 0x000043CF
		public TimeSpan TimePosition
		{
			get
			{
				return this._streamDecoder.TimePosition;
			}
			set
			{
				this._streamDecoder.TimePosition = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000061DD File Offset: 0x000043DD
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x000061EA File Offset: 0x000043EA
		public long SamplePosition
		{
			get
			{
				return this._streamDecoder.SamplePosition;
			}
			set
			{
				this._streamDecoder.SamplePosition = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000061F8 File Offset: 0x000043F8
		public bool IsEndOfStream
		{
			get
			{
				return this._streamDecoder.IsEndOfStream;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006205 File Offset: 0x00004405
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00006212 File Offset: 0x00004412
		public bool ClipSamples
		{
			get
			{
				return this._streamDecoder.ClipSamples;
			}
			set
			{
				this._streamDecoder.ClipSamples = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006220 File Offset: 0x00004420
		public bool HasClipped
		{
			get
			{
				return this._streamDecoder.HasClipped;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000622D File Offset: 0x0000442D
		public IStreamStats StreamStats
		{
			get
			{
				return this._streamDecoder.Stats;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000623A File Offset: 0x0000443A
		[Obsolete("Use Streams[*].Stats instead.", true)]
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006241 File Offset: 0x00004441
		public bool FindNextStream()
		{
			return this._containerReader != null && this._containerReader.FindNextStream();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006258 File Offset: 0x00004458
		public bool SwitchStreams(int index)
		{
			if (index < 0 || index >= this._decoders.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			IStreamDecoder streamDecoder = this._decoders[index];
			IStreamDecoder streamDecoder2 = this._streamDecoder;
			if (streamDecoder == streamDecoder2)
			{
				return false;
			}
			streamDecoder.ClipSamples = streamDecoder2.ClipSamples;
			this._streamDecoder = streamDecoder;
			return streamDecoder.Channels != streamDecoder2.Channels || streamDecoder.SampleRate != streamDecoder2.SampleRate;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000062D0 File Offset: 0x000044D0
		public void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0)
		{
			this._streamDecoder.SeekTo(timePosition, seekOrigin);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000062DF File Offset: 0x000044DF
		public void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0)
		{
			this._streamDecoder.SeekTo(samplePosition, seekOrigin);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000062EE File Offset: 0x000044EE
		public int ReadSamples(float[] buffer, int offset, int count)
		{
			count -= count % this._streamDecoder.Channels;
			if (count > 0)
			{
				return this._streamDecoder.Read(buffer, offset, count);
			}
			return 0;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000631C File Offset: 0x0000451C
		public int ReadSamples(Span<float> buffer)
		{
			int count = buffer.Length - buffer.Length % this._streamDecoder.Channels;
			if (!buffer.IsEmpty)
			{
				return this._streamDecoder.Read(buffer, 0, count);
			}
			return 0;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000635E File Offset: 0x0000455E
		[Obsolete("No longer needed.", true)]
		public void ClearParameterChange()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400007A RID: 122
		private readonly List<IStreamDecoder> _decoders;

		// Token: 0x0400007B RID: 123
		private readonly NVorbis.Contracts.IContainerReader _containerReader;

		// Token: 0x0400007C RID: 124
		private readonly bool _closeOnDispose;

		// Token: 0x0400007D RID: 125
		private IStreamDecoder _streamDecoder;
	}
}
