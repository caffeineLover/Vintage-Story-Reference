using System;
using System.Collections.Generic;
using System.IO;
using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x02000018 RID: 24
	public sealed class ContainerReader : NVorbis.Contracts.IContainerReader, IDisposable
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00006391 File Offset: 0x00004591
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00006398 File Offset: 0x00004598
		internal static Func<Stream, bool, Func<NVorbis.Contracts.IPacketProvider, bool>, IPageReader> CreatePageReader { get; set; } = (Stream s, bool cod, Func<NVorbis.Contracts.IPacketProvider, bool> cb) => new PageReader(s, cod, cb);

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000063A0 File Offset: 0x000045A0
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000063A7 File Offset: 0x000045A7
		internal static Func<Stream, bool, Func<NVorbis.Contracts.IPacketProvider, bool>, IPageReader> CreateForwardOnlyPageReader { get; set; } = (Stream s, bool cod, Func<NVorbis.Contracts.IPacketProvider, bool> cb) => new ForwardOnlyPageReader(s, cod, cb);

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000063AF File Offset: 0x000045AF
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000063B7 File Offset: 0x000045B7
		public NewStreamHandler NewStreamCallback { get; set; }

		// Token: 0x0600010A RID: 266 RVA: 0x000063C0 File Offset: 0x000045C0
		public IReadOnlyList<NVorbis.Contracts.IPacketProvider> GetStreams()
		{
			List<NVorbis.Contracts.IPacketProvider> list = new List<NVorbis.Contracts.IPacketProvider>(this._packetProviders.Count);
			for (int i = 0; i < this._packetProviders.Count; i++)
			{
				NVorbis.Contracts.IPacketProvider packetProvider;
				if (this._packetProviders[i].TryGetTarget(ref packetProvider))
				{
					list.Add(packetProvider);
				}
				else
				{
					list.RemoveAt(i);
					i--;
				}
			}
			return list;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000641E File Offset: 0x0000461E
		public bool CanSeek { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00006426 File Offset: 0x00004626
		public long WasteBits
		{
			get
			{
				return this._reader.WasteBits;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006433 File Offset: 0x00004633
		public long ContainerBits
		{
			get
			{
				return this._reader.ContainerBits;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006440 File Offset: 0x00004640
		public ContainerReader(Stream stream, bool closeOnDispose)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._packetProviders = new List<WeakReference<NVorbis.Contracts.IPacketProvider>>();
			if (stream.CanSeek)
			{
				this._reader = ContainerReader.CreatePageReader.Invoke(stream, closeOnDispose, new Func<NVorbis.Contracts.IPacketProvider, bool>(this.ProcessNewStream));
				this.CanSeek = 1;
				return;
			}
			this._reader = ContainerReader.CreateForwardOnlyPageReader.Invoke(stream, closeOnDispose, new Func<NVorbis.Contracts.IPacketProvider, bool>(this.ProcessNewStream));
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000064B8 File Offset: 0x000046B8
		public bool TryInit()
		{
			return this.FindNextStream();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000064C0 File Offset: 0x000046C0
		public bool FindNextStream()
		{
			this._reader.Lock();
			bool result;
			try
			{
				this._foundStream = false;
				while (this._reader.ReadNextPage())
				{
					if (this._foundStream)
					{
						return true;
					}
				}
				result = false;
			}
			finally
			{
				this._reader.Release();
			}
			return result;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000651C File Offset: 0x0000471C
		private bool ProcessNewStream(NVorbis.Contracts.IPacketProvider packetProvider)
		{
			bool flag = this._reader.Release();
			bool result;
			try
			{
				NewStreamHandler newStreamCallback = this.NewStreamCallback;
				if (newStreamCallback == null || newStreamCallback(packetProvider))
				{
					this._packetProviders.Add(new WeakReference<NVorbis.Contracts.IPacketProvider>(packetProvider));
					this._foundStream = true;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			finally
			{
				if (flag)
				{
					this._reader.Lock();
				}
			}
			return result;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000658C File Offset: 0x0000478C
		public void Dispose()
		{
			IPageReader reader = this._reader;
			if (reader != null)
			{
				reader.Dispose();
			}
			this._reader = null;
		}

		// Token: 0x04000081 RID: 129
		private IPageReader _reader;

		// Token: 0x04000082 RID: 130
		private List<WeakReference<NVorbis.Contracts.IPacketProvider>> _packetProviders;

		// Token: 0x04000083 RID: 131
		private bool _foundStream;
	}
}
