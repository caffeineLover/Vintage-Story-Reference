using System;
using Concentus.Structs;

namespace Concentus
{
	// Token: 0x0200000F RID: 15
	internal static class OpusMultistream
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00005038 File Offset: 0x00003238
		internal static int validate_layout(ChannelLayout layout)
		{
			int num = layout.nb_streams + layout.nb_coupled_streams;
			if (num > 255)
			{
				return 0;
			}
			for (int i = 0; i < layout.nb_channels; i++)
			{
				if ((int)layout.mapping[i] >= num && layout.mapping[i] != 255)
				{
					return 0;
				}
			}
			return 1;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000508C File Offset: 0x0000328C
		internal static int get_left_channel(ChannelLayout layout, int stream_id, int prev)
		{
			for (int i = (prev < 0) ? 0 : (prev + 1); i < layout.nb_channels; i++)
			{
				if ((int)layout.mapping[i] == stream_id * 2)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000050C4 File Offset: 0x000032C4
		internal static int get_right_channel(ChannelLayout layout, int stream_id, int prev)
		{
			for (int i = (prev < 0) ? 0 : (prev + 1); i < layout.nb_channels; i++)
			{
				if ((int)layout.mapping[i] == stream_id * 2 + 1)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005100 File Offset: 0x00003300
		internal static int get_mono_channel(ChannelLayout layout, int stream_id, int prev)
		{
			for (int i = (prev < 0) ? 0 : (prev + 1); i < layout.nb_channels; i++)
			{
				if ((int)layout.mapping[i] == stream_id + layout.nb_coupled_streams)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
