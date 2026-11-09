using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x0200000B RID: 11
	[Obsolete("Moved to NVorbis.Contracts.IStreamStats", true)]
	public interface IVorbisStreamStatus : IStreamStats
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005F RID: 95
		[Obsolete("No longer supported.", true)]
		TimeSpan PageLatency { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000060 RID: 96
		[Obsolete("No longer supported.", true)]
		TimeSpan PacketLatency { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000061 RID: 97
		[Obsolete("No longer supported.", true)]
		TimeSpan SecondLatency { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000062 RID: 98
		[Obsolete("No longer supported.", true)]
		int PagesRead { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000063 RID: 99
		[Obsolete("No longer supported.", true)]
		int TotalPages { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100
		[Obsolete("Use IStreamDecoder.HasClipped instead.  VorbisReader.HasClipped will return the same value for the stream it is handling.", true)]
		bool Clipped { get; }
	}
}
