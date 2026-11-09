using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	[Nullable(0)]
	public class StatsSystemServer
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00003A0A File Offset: 0x00001C0A
		public StatsSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<StatsPacket>().SetMessageHandler<StatsPacket>(new NetworkClientMessageHandler<StatsPacket>(this.HandlePacket));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003A40 File Offset: 0x00001C40
		private void HandlePacket(IServerPlayer player, StatsPacket packet)
		{
			float value;
			if (!StatsSystemServer.TryValidate(packet, out value))
			{
				this.LogRejectedPacket(player, packet);
				return;
			}
			player.Entity.Stats.Set(packet.Stat, packet.Category, value, false);
			Action<IServerPlayer, string, float> onStatSet = this.OnStatSet;
			if (onStatSet == null)
			{
				return;
			}
			onStatSet.Invoke(player, packet.Stat, value);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003A98 File Offset: 0x00001C98
		private static bool TryValidate(StatsPacket packet, out float value)
		{
			value = 0f;
			if (!string.Equals(packet.Stat, "walkspeed", 4) || !StatsSystemServer.IsAllowedCategory(packet.Category) || float.IsNaN(packet.Value) || float.IsInfinity(packet.Value))
			{
				return false;
			}
			value = Math.Clamp(packet.Value, -0.95f, 0f);
			return true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003B00 File Offset: 0x00001D00
		private static bool IsAllowedCategory(string category)
		{
			return string.Equals(category, "CombatOverhaul:held-item-mainhand", 4) || string.Equals(category, "CombatOverhaul:held-item-offhand", 4);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003B20 File Offset: 0x00001D20
		private void LogRejectedPacket(IServerPlayer player, StatsPacket packet)
		{
			if (this._rejectedPacketLogs >= 5)
			{
				return;
			}
			this._rejectedPacketLogs++;
			ICoreAPI api = this._api;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(62, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Rejected stats packet from '");
			defaultInterpolatedStringHandler.AppendFormatted(player.PlayerName);
			defaultInterpolatedStringHandler.AppendLiteral("' (");
			defaultInterpolatedStringHandler.AppendFormatted(player.PlayerUID);
			defaultInterpolatedStringHandler.AppendLiteral("): stat='");
			defaultInterpolatedStringHandler.AppendFormatted(packet.Stat);
			defaultInterpolatedStringHandler.AppendLiteral("', category='");
			defaultInterpolatedStringHandler.AppendFormatted(packet.Category);
			defaultInterpolatedStringHandler.AppendLiteral("', value=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(packet.Value);
			LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600009B RID: 155 RVA: 0x00003BE0 File Offset: 0x00001DE0
		// (remove) Token: 0x0600009C RID: 156 RVA: 0x00003C18 File Offset: 0x00001E18
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		internal event Action<IServerPlayer, string, float> OnStatSet;

		// Token: 0x0400002A RID: 42
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x0400002B RID: 43
		private const string _walkSpeedStat = "walkspeed";

		// Token: 0x0400002C RID: 44
		private const string _mainHandCategory = "CombatOverhaul:held-item-mainhand";

		// Token: 0x0400002D RID: 45
		private const string _offHandCategory = "CombatOverhaul:held-item-offhand";

		// Token: 0x0400002E RID: 46
		private const float _minWalkSpeedModifier = -0.95f;

		// Token: 0x0400002F RID: 47
		private const float _maxWalkSpeedModifier = 0f;

		// Token: 0x04000030 RID: 48
		private const int _maxRejectedPacketLogs = 5;

		// Token: 0x04000031 RID: 49
		private readonly ICoreServerAPI _api;

		// Token: 0x04000032 RID: 50
		private int _rejectedPacketLogs;
	}
}
