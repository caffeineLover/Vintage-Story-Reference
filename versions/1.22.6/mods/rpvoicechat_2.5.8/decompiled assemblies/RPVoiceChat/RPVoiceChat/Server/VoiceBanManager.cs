using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x02000045 RID: 69
	public class VoiceBanManager
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x0000C796 File Offset: 0x0000A996
		public VoiceBanManager(ICoreServerAPI sapi)
		{
			this.api = sapi;
			this.banConfigManager = new VoiceBanConfigManager(sapi);
			this.LoadBannedPlayers();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000C7C2 File Offset: 0x0000A9C2
		public bool IsPlayerBanned(string playerUID)
		{
			return this.bannedPlayers.Contains(playerUID);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		public bool BanPlayer(string playerUID)
		{
			if (this.bannedPlayers.Add(playerUID))
			{
				this.SaveBannedPlayers();
				return true;
			}
			return false;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C7E9 File Offset: 0x0000A9E9
		public bool UnbanPlayer(string playerUID)
		{
			if (this.bannedPlayers.Remove(playerUID))
			{
				this.SaveBannedPlayers();
				return true;
			}
			return false;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C802 File Offset: 0x0000AA02
		public List<string> GetBannedPlayers()
		{
			return Enumerable.ToList<string>(this.bannedPlayers);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000C810 File Offset: 0x0000AA10
		public string GetPlayerName(string playerUID)
		{
			IPlayer player = this.api.World.PlayerByUid(playerUID);
			if (player != null)
			{
				return player.PlayerName;
			}
			IPlayer player2 = Enumerable.FirstOrDefault<IPlayer>(this.api.World.AllPlayers, (IPlayer p) => p.PlayerUID == playerUID);
			return ((player2 != null) ? player2.PlayerName : null) ?? playerUID;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000C884 File Offset: 0x0000AA84
		private void LoadBannedPlayers()
		{
			try
			{
				List<string> bannedList = this.banConfigManager.LoadBannedPlayers();
				this.bannedPlayers = new HashSet<string>(bannedList);
			}
			catch (Exception e)
			{
				Logger.server.Warning("Error loading voice ban blacklist: " + e.Message);
				this.bannedPlayers = new HashSet<string>();
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000C8E4 File Offset: 0x0000AAE4
		private void SaveBannedPlayers()
		{
			try
			{
				List<string> bannedList = Enumerable.ToList<string>(this.bannedPlayers);
				this.banConfigManager.SaveBannedPlayers(bannedList);
			}
			catch (Exception e)
			{
				Logger.server.Error("Error saving voice ban blacklist: " + e.Message);
			}
		}

		// Token: 0x040000B8 RID: 184
		private ICoreServerAPI api;

		// Token: 0x040000B9 RID: 185
		private HashSet<string> bannedPlayers = new HashSet<string>();

		// Token: 0x040000BA RID: 186
		private VoiceBanConfigManager banConfigManager;
	}
}
