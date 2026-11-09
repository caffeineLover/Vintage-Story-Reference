using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000009 RID: 9
	[Obsolete("Moved to NVorbis.Contracts.IContainerReader", true)]
	public interface IContainerReader : NVorbis.Contracts.IContainerReader, IDisposable
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000052 RID: 82
		[Obsolete("Use Streams.Select(s => s.StreamSerial).ToArray() instead.", true)]
		int[] StreamSerials { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000053 RID: 83
		[Obsolete("No longer supported.", true)]
		int PagesRead { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000054 RID: 84
		// (remove) Token: 0x06000055 RID: 85
		[Obsolete("Moved to NewStreamCallback.", true)]
		event EventHandler<NewStreamEventArgs> NewStream;

		// Token: 0x06000056 RID: 86
		[Obsolete("Renamed to TryInit().", true)]
		bool Init();

		// Token: 0x06000057 RID: 87
		[Obsolete("No longer supported.", true)]
		int GetTotalPageCount();
	}
}
