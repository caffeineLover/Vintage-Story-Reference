using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x02000062 RID: 98
	public class VoiceBanManager
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00010262 File Offset: 0x0000E462
		public VoiceBanManager(ICoreServerAPI sapi)
		{
			this.api = sapi;
			this.banConfigManager = new VoiceBanConfigManager(sapi);
			this.LoadBannedPlayers();
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0001028E File Offset: 0x0000E48E
		public bool IsPlayerBanned(string playerUID)
		{
			return this.bannedPlayers.Contains(playerUID);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001029C File Offset: 0x0000E49C
		public bool BanPlayer(string playerUID)
		{
			if (this.bannedPlayers.Add(playerUID))
			{
				this.SaveBannedPlayers();
				return true;
			}
			return false;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000102B5 File Offset: 0x0000E4B5
		public bool UnbanPlayer(string playerUID)
		{
			if (this.bannedPlayers.Remove(playerUID))
			{
				this.SaveBannedPlayers();
				return true;
			}
			return false;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000102CE File Offset: 0x0000E4CE
		public List<string> GetBannedPlayers()
		{
			return Enumerable.ToList<string>(this.bannedPlayers);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000102DC File Offset: 0x0000E4DC
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

		// Token: 0x06000278 RID: 632 RVA: 0x00010350 File Offset: 0x0000E550
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

		// Token: 0x06000279 RID: 633 RVA: 0x000103B0 File Offset: 0x0000E5B0
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

		// Token: 0x0400010A RID: 266
		private ICoreServerAPI api;

		// Token: 0x0400010B RID: 267
		private HashSet<string> bannedPlayers = new HashSet<string>();

		// Token: 0x0400010C RID: 268
		private VoiceBanConfigManager banConfigManager;
	}
}
