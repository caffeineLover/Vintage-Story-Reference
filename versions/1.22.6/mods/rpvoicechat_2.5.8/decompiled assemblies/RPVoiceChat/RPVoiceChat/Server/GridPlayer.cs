using System;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x02000041 RID: 65
	internal readonly struct GridPlayer
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		public GridPlayer(IServerPlayer player, int dimension, double x, double y, double z, bool isSpectator)
		{
			this.Player = player;
			this.PlayerUID = (((player != null) ? player.PlayerUID : null) ?? "");
			this.PlayerName = (((player != null) ? player.PlayerName : null) ?? "");
			this.Dimension = dimension;
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.IsSpectator = isSpectator;
		}

		// Token: 0x040000AD RID: 173
		public readonly IServerPlayer Player;

		// Token: 0x040000AE RID: 174
		public readonly string PlayerUID;

		// Token: 0x040000AF RID: 175
		public readonly string PlayerName;

		// Token: 0x040000B0 RID: 176
		public readonly int Dimension;

		// Token: 0x040000B1 RID: 177
		public readonly double X;

		// Token: 0x040000B2 RID: 178
		public readonly double Y;

		// Token: 0x040000B3 RID: 179
		public readonly double Z;

		// Token: 0x040000B4 RID: 180
		public readonly bool IsSpectator;
	}
}
