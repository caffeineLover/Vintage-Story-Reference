using System;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x0200005C RID: 92
	internal readonly struct GridPlayer
	{
		// Token: 0x06000264 RID: 612 RVA: 0x00010164 File Offset: 0x0000E364
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

		// Token: 0x040000FA RID: 250
		public readonly IServerPlayer Player;

		// Token: 0x040000FB RID: 251
		public readonly string PlayerUID;

		// Token: 0x040000FC RID: 252
		public readonly string PlayerName;

		// Token: 0x040000FD RID: 253
		public readonly int Dimension;

		// Token: 0x040000FE RID: 254
		public readonly double X;

		// Token: 0x040000FF RID: 255
		public readonly double Y;

		// Token: 0x04000100 RID: 256
		public readonly double Z;

		// Token: 0x04000101 RID: 257
		public readonly bool IsSpectator;
	}
}
