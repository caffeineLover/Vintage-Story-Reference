using System;

namespace Concentus.Structs
{
	// Token: 0x02000062 RID: 98
	internal class VorbisLayout
	{
		// Token: 0x06000248 RID: 584 RVA: 0x0001FD21 File Offset: 0x0001DF21
		internal VorbisLayout(int streams, int coupled_streams, byte[] map)
		{
			this.nb_streams = streams;
			this.nb_coupled_streams = coupled_streams;
			this.mapping = map;
		}

		// Token: 0x04000330 RID: 816
		internal int nb_streams;

		// Token: 0x04000331 RID: 817
		internal int nb_coupled_streams;

		// Token: 0x04000332 RID: 818
		internal byte[] mapping;

		// Token: 0x04000333 RID: 819
		internal static readonly VorbisLayout[] vorbis_mappings = new VorbisLayout[]
		{
			new VorbisLayout(1, 0, new byte[1]),
			new VorbisLayout(1, 1, new byte[]
			{
				default(byte),
				1
			}),
			new VorbisLayout(2, 1, new byte[]
			{
				default(byte),
				2,
				1
			}),
			new VorbisLayout(2, 2, new byte[]
			{
				0,
				1,
				2,
				3
			}),
			new VorbisLayout(3, 2, new byte[]
			{
				0,
				4,
				1,
				2,
				3
			}),
			new VorbisLayout(4, 2, new byte[]
			{
				0,
				4,
				1,
				2,
				3,
				5
			}),
			new VorbisLayout(4, 3, new byte[]
			{
				0,
				4,
				1,
				2,
				3,
				5,
				6
			}),
			new VorbisLayout(5, 3, new byte[]
			{
				0,
				6,
				1,
				2,
				3,
				4,
				5,
				7
			})
		};
	}
}
