using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x0200000A RID: 10
	[Obsolete("Moved to NVorbis.Contracts.IPacketProvider", true)]
	public interface IPacketProvider : NVorbis.Contracts.IPacketProvider
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000058 RID: 88
		[Obsolete("Moved to per-stream IStreamStats instance on IStreamDecoder.Stats or VorbisReader.Stats.", true)]
		long ContainerBits { get; }

		// Token: 0x06000059 RID: 89
		[Obsolete("No longer supported.", true)]
		int GetTotalPageCount();

		// Token: 0x0600005A RID: 90
		[Obsolete("Getting a packet by index is no longer supported.", true)]
		DataPacket GetPacket(int packetIndex);

		// Token: 0x0600005B RID: 91
		[Obsolete("Moved to long SeekTo(long, int, GetPacketGranuleCount)", true)]
		DataPacket FindPacket(long granulePos, Func<DataPacket, DataPacket, int> packetGranuleCountCallback);

		// Token: 0x0600005C RID: 92
		[Obsolete("Seeking to a specified packet is no longer supported.  See SeekTo(...) instead.", true)]
		void SeekToPacket(DataPacket packet, int preRoll);

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600005D RID: 93
		// (remove) Token: 0x0600005E RID: 94
		[Obsolete("No longer supported.", true)]
		event EventHandler ParameterChange;
	}
}
