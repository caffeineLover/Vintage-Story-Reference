using System;

namespace Concentus.Structs
{
	// Token: 0x02000058 RID: 88
	internal class ChannelLayout
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00018D58 File Offset: 0x00016F58
		internal void Reset()
		{
			this.nb_channels = 0;
			this.nb_streams = 0;
			this.nb_coupled_streams = 0;
			MemoryExtensions.AsSpan<byte>(this.mapping).Clear();
		}

		// Token: 0x040002B3 RID: 691
		internal int nb_channels;

		// Token: 0x040002B4 RID: 692
		internal int nb_streams;

		// Token: 0x040002B5 RID: 693
		internal int nb_coupled_streams;

		// Token: 0x040002B6 RID: 694
		internal readonly byte[] mapping = new byte[256];
	}
}
