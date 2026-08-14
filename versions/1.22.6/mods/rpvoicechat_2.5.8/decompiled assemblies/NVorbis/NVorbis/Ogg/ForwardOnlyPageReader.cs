using System;
using System.Collections.Generic;
using System.IO;
using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x0200001B RID: 27
	internal class ForwardOnlyPageReader : PageReaderBase
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006B3F File Offset: 0x00004D3F
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00006B46 File Offset: 0x00004D46
		internal static Func<IPageReader, int, IForwardOnlyPacketProvider> CreatePacketProvider { get; set; } = (IPageReader pr, int ss) => new ForwardOnlyPacketProvider(pr, ss);

		// Token: 0x0600012B RID: 299 RVA: 0x00006B4E File Offset: 0x00004D4E
		public ForwardOnlyPageReader(Stream stream, bool closeOnDispose, Func<NVorbis.Contracts.IPacketProvider, bool> newStreamCallback) : base(stream, closeOnDispose)
		{
			this._newStreamCallback = newStreamCallback;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006B6C File Offset: 0x00004D6C
		protected override bool AddPage(int streamSerial, byte[] pageBuf, bool isResync)
		{
			IForwardOnlyPacketProvider forwardOnlyPacketProvider;
			if (!this._packetProviders.TryGetValue(streamSerial, ref forwardOnlyPacketProvider))
			{
				forwardOnlyPacketProvider = ForwardOnlyPageReader.CreatePacketProvider.Invoke(this, streamSerial);
				if (forwardOnlyPacketProvider.AddPage(pageBuf, isResync))
				{
					this._packetProviders.Add(streamSerial, forwardOnlyPacketProvider);
					if (this._newStreamCallback.Invoke(forwardOnlyPacketProvider))
					{
						return true;
					}
					this._packetProviders.Remove(streamSerial);
				}
				return false;
			}
			if (forwardOnlyPacketProvider.AddPage(pageBuf, isResync))
			{
				if ((pageBuf[5] & 4) != 0)
				{
					forwardOnlyPacketProvider.SetEndOfStream();
					this._packetProviders.Remove(streamSerial);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006BF4 File Offset: 0x00004DF4
		protected override void SetEndOfStreams()
		{
			foreach (KeyValuePair<int, IForwardOnlyPacketProvider> keyValuePair in this._packetProviders)
			{
				keyValuePair.Value.SetEndOfStream();
			}
			this._packetProviders.Clear();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006C58 File Offset: 0x00004E58
		public override bool ReadPageAt(long offset)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000095 RID: 149
		private readonly Dictionary<int, IForwardOnlyPacketProvider> _packetProviders = new Dictionary<int, IForwardOnlyPacketProvider>();

		// Token: 0x04000096 RID: 150
		private readonly Func<NVorbis.Contracts.IPacketProvider, bool> _newStreamCallback;
	}
}
