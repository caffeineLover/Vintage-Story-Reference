using System;
using System.Collections.Generic;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x0200001C RID: 28
	internal class Packet : DataPacket
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00006C76 File Offset: 0x00004E76
		internal Packet(IReadOnlyList<int> dataParts, IPacketReader packetReader, Memory<byte> initialData)
		{
			this._dataParts = dataParts;
			this._packetReader = packetReader;
			this._data = initialData;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00006C93 File Offset: 0x00004E93
		protected override int TotalBits
		{
			get
			{
				return (this._dataCount + this._data.Length) * 8;
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006CAC File Offset: 0x00004EAC
		protected unsafe override int ReadNextByte()
		{
			if (this._dataIndex == this._dataParts.Count)
			{
				return -1;
			}
			byte result = *this._data.Span[this._dataOfs];
			int num = this._dataOfs + 1;
			this._dataOfs = num;
			if (num == this._data.Length)
			{
				this._dataOfs = 0;
				this._dataCount += this._data.Length;
				num = this._dataIndex + 1;
				this._dataIndex = num;
				if (num < this._dataParts.Count)
				{
					this._data = this._packetReader.GetPacketData(this._dataParts[this._dataIndex]);
					return (int)result;
				}
				this._data = Memory<byte>.Empty;
			}
			return (int)result;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006D74 File Offset: 0x00004F74
		public override void Reset()
		{
			this._dataIndex = 0;
			this._dataOfs = 0;
			if (this._dataParts.Count > 0)
			{
				this._data = this._packetReader.GetPacketData(this._dataParts[0]);
			}
			base.Reset();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006DC0 File Offset: 0x00004FC0
		public override void Done()
		{
			IPacketReader packetReader = this._packetReader;
			if (packetReader != null)
			{
				packetReader.InvalidatePacketCache(this);
			}
			base.Done();
		}

		// Token: 0x04000097 RID: 151
		private IReadOnlyList<int> _dataParts;

		// Token: 0x04000098 RID: 152
		private IPacketReader _packetReader;

		// Token: 0x04000099 RID: 153
		private int _dataCount;

		// Token: 0x0400009A RID: 154
		private Memory<byte> _data;

		// Token: 0x0400009B RID: 155
		private int _dataIndex;

		// Token: 0x0400009C RID: 156
		private int _dataOfs;
	}
}
