using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	public class StatsSystemClient
	{
		// Token: 0x06000094 RID: 148 RVA: 0x0000398A File Offset: 0x00001B8A
		public StatsSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<StatsPacket>();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000039B4 File Offset: 0x00001BB4
		public void SetStat(string stat, string category, float value)
		{
			this._clientChannel.SendPacket<StatsPacket>(new StatsPacket
			{
				Stat = stat,
				Category = category,
				Value = value
			});
			this._api.World.Player.Entity.Stats.Set(stat, category, value, false);
		}

		// Token: 0x04000027 RID: 39
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x04000028 RID: 40
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x04000029 RID: 41
		private readonly ICoreClientAPI _api;
	}
}
