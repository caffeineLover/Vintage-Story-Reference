using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RPVoiceChat.Config;
using RPVoiceChat.Networking;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Server;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Server;

namespace RPVoiceChat
{
	// Token: 0x02000021 RID: 33
	public class RPVoiceChatServer : RPVoiceChatMod
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00005E58 File Offset: 0x00004058
		public override void StartServerSide(ICoreServerAPI api)
		{
			RPVoiceChatMod.sapi = api;
			WorldConfig.Set(VoiceLevel.Whispering, WorldConfig.GetInt(VoiceLevel.Whispering, default(int?)));
			WorldConfig.Set(VoiceLevel.Talking, WorldConfig.GetInt(VoiceLevel.Talking, default(int?)));
			WorldConfig.Set(VoiceLevel.Shouting, WorldConfig.GetInt(VoiceLevel.Shouting, default(int?)));
			WorldConfig.Set("player-nametag-targeted-only", WorldConfig.GetPlayerNametagTargetedOnly());
			WorldConfig.Set("force-speaker-nametag", WorldConfig.GetForceSpeakerNametag());
			WorldConfig.Set("use-nametag-dynamic-range", WorldConfig.GetBool("use-nametag-dynamic-range", true));
			WorldConfig.Set("nametag-fallback-range", ServerConfigManager.NametagFallbackRenderRange);
			RPVoiceChatMod.sapi.Event.PlayerJoin += new PlayerDelegate(this.OnPlayerJoin);
			this.BroadcastNametagConfigChanged();
			WorldConfig.Set("encode-audio", WorldConfig.GetBool("encode-audio", true));
			WorldConfig.Set("others-hear-spectators", WorldConfig.GetBool("others-hear-spectators", true));
			WorldConfig.Set("wall-thickness-weighting", WorldConfig.GetFloat("wall-thickness-weighting", 2f));
			this.registerCommands();
			WireNetworkHandler.RegisterServerside(api);
			bool forwardPorts = !ModConfig.ServerConfig.ManualPortForwarding;
			List<INetworkServer> list = new List<INetworkServer>();
			list.Add(new NativeNetworkServer(api));
			List<INetworkServer> networkTransports = list;
			if (ModConfig.ServerConfig.UseCustomNetworkServers)
			{
				networkTransports.Insert(0, new UDPNetworkServer(ModConfig.ServerConfig.ServerPort, ModConfig.ServerConfig.ServerIP, forwardPorts));
				networkTransports.Insert(1, new TCPNetworkServer(ModConfig.ServerConfig.ServerPort, ModConfig.ServerConfig.ServerIP, forwardPorts));
			}
			List<IVoiceRouteProvider> voiceRouteProviders = RPVoiceChatServer.CollectVoiceRouteProviders(api);
			this.server = new GameServer(RPVoiceChatMod.sapi, networkTransports, voiceRouteProviders);
			this.server.Launch();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005FF4 File Offset: 0x000041F4
		private static List<IVoiceRouteProvider> CollectVoiceRouteProviders(ICoreServerAPI api)
		{
			List<IVoiceRouteProvider> providers = new List<IVoiceRouteProvider>();
			if (((api != null) ? api.ModLoader : null) == null)
			{
				return providers;
			}
			foreach (string propertyName in new string[]
			{
				"Systems",
				"ModSystems",
				"LoadedSystems"
			})
			{
				try
				{
					PropertyInfo property = api.ModLoader.GetType().GetProperty(propertyName, 52);
					IEnumerable systems = ((property != null) ? property.GetValue(api.ModLoader) : null) as IEnumerable;
					if (systems != null)
					{
						foreach (object obj in systems)
						{
							IVoiceRouteProvider provider = obj as IVoiceRouteProvider;
							if (provider != null && !providers.Contains(provider))
							{
								providers.Add(provider);
							}
						}
						if (providers.Count > 0)
						{
							return providers;
						}
					}
				}
				catch
				{
				}
			}
			return providers;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006104 File Offset: 0x00004304
		public override void StartPre(ICoreAPI api)
		{
			base.StartPre(api);
			WorldConfig.Set("additional-content", ModConfig.ServerConfig.AdditionalContent);
			WorldConfig.Set("technology-content", ModConfig.ServerConfig.TechnologyContent);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006135 File Offset: 0x00004335
		public override double ExecuteOrder()
		{
			return 1.02;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006140 File Offset: 0x00004340
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 1;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006148 File Offset: 0x00004348
		private void registerCommands()
		{
			CommandArgumentParsers parsers = RPVoiceChatMod.sapi.ChatCommands.Parsers;
			IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(RPVoiceChatMod.sapi.ChatCommands.GetOrCreate("rpvc").WithAlias(new string[]
			{
				"rpvoice",
				"rpvoicechat"
			}).RequiresPrivilege(Privilege.controlserver), "shout"), UIUtils.I18n("Command.Shout.Desc")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Int("distance")
			}).HandleWith(new OnCommandDelegate(this.SetShoutHandler))), "talk"), UIUtils.I18n("Command.Talk.Desc")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Int("distance")
			}).HandleWith(new OnCommandDelegate(this.SetTalkHandler))), "whisper"), UIUtils.I18n("Command.Whisper.Desc")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Int("distance")
			}).HandleWith(new OnCommandDelegate(this.SetWhisperHandler))), "info"), UIUtils.I18n("Command.Info.Desc")).HandleWith(new OnCommandDelegate(this.DisplayInfoHandler))), "reset"), UIUtils.I18n("Command.Reset.Desc")).HandleWith(new OnCommandDelegate(this.ResetDistanceHandler))), "playerNametagTargetedOnly"), UIUtils.I18n("Command.PlayerNametagTargetedOnly.Desc")).WithAdditionalInformation(UIUtils.I18n("Command.PlayerNametagTargetedOnly.Help")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Bool("state", "on")
			}).HandleWith(new OnCommandDelegate(this.TogglePlayerNametagTargetedOnly))), "forceSpeakerNametag"), UIUtils.I18n("Command.ForceSpeakerNametag.Desc")).WithAdditionalInformation(UIUtils.I18n("Command.ForceSpeakerNametag.Help")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Bool("state", "on")
			}).HandleWith(new OnCommandDelegate(this.ToggleForceSpeakerNametag))), "useNametagDynamicRange"), UIUtils.I18n("Command.UseNametagDynamicRange.Desc")).WithAdditionalInformation(UIUtils.I18n("Command.UseNametagDynamicRange.Help")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Bool("state", "on")
			}).HandleWith(new OnCommandDelegate(this.ToggleUseNametagDynamicRange))), "encodeAudio"), UIUtils.I18n("Command.EncodeAudio.Desc")).WithAdditionalInformation(UIUtils.I18n("Command.EncodeAudio.Help")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Bool("state", "on")
			}).HandleWith(new OnCommandDelegate(this.ToggleAudioEncoding))), "hearSpectators"), UIUtils.I18n("Command.OthersHearSpectators.Desc")).WithAdditionalInformation(UIUtils.I18n("Command.OthersHearSpectators.Help")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Bool("state", "on")
			}).HandleWith(new OnCommandDelegate(this.ToggleOthersHearSpectators))), "voiceBan"), UIUtils.I18n("Command.VoiceBan.Desc")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Word("player")
			}).HandleWith(new OnCommandDelegate(this.VoiceBanHandler))), "voiceUnban"), UIUtils.I18n("Command.VoiceUnban.Desc")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.Word("player")
			}).HandleWith(new OnCommandDelegate(this.VoiceUnbanHandler))), "voiceBanList"), UIUtils.I18n("Command.VoiceBanList.Desc")).HandleWith(new OnCommandDelegate(this.VoiceBanListHandler))), "announce"), UIUtils.I18n("Command.Announce.Desc")).WithAdditionalInformation(UIUtils.I18n("Command.Announce.Help")).WithArgs(new ICommandArgumentParser[]
			{
				parsers.All("title | message | duration | glass")
			}).HandleWith(new OnCommandDelegate(this.AnnounceHandler)));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000065B8 File Offset: 0x000047B8
		private TextCommandResult ToggleOthersHearSpectators(TextCommandCallingArgs args)
		{
			bool state = (bool)args[0];
			WorldConfig.Set("others-hear-spectators", state);
			string stateAsText = state ? "Enabled" : "Disabled";
			return TextCommandResult.Success(UIUtils.I18n("Command.OthersHearSpectators.Success." + stateAsText), null);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006604 File Offset: 0x00004804
		private TextCommandResult ToggleAudioEncoding(TextCommandCallingArgs args)
		{
			bool state = (bool)args[0];
			WorldConfig.Set("encode-audio", state);
			string stateAsText = state ? "Enabled" : "Disabled";
			return TextCommandResult.Success(UIUtils.I18n("Command.EncodeAudio.Success." + stateAsText), null);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006650 File Offset: 0x00004850
		private TextCommandResult ToggleForceSpeakerNametag(TextCommandCallingArgs args)
		{
			bool state = (bool)args[0];
			WorldConfig.Set("force-speaker-nametag", state);
			this.BroadcastNametagConfigChanged();
			string stateAsText = state ? "Enabled" : "Disabled";
			return TextCommandResult.Success(UIUtils.I18n("Command.ForceSpeakerNametag.Success." + stateAsText), null);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000066A4 File Offset: 0x000048A4
		private TextCommandResult TogglePlayerNametagTargetedOnly(TextCommandCallingArgs args)
		{
			bool state = (bool)args[0];
			WorldConfig.Set("player-nametag-targeted-only", state);
			this.BroadcastNametagConfigChanged();
			string stateAsText = state ? "Enabled" : "Disabled";
			return TextCommandResult.Success(UIUtils.I18n("Command.PlayerNametagTargetedOnly.Success." + stateAsText), null);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000066F8 File Offset: 0x000048F8
		private TextCommandResult ToggleUseNametagDynamicRange(TextCommandCallingArgs args)
		{
			bool state = (bool)args[0];
			WorldConfig.Set("use-nametag-dynamic-range", state);
			this.BroadcastNametagConfigChanged();
			string stateAsText = state ? "Enabled" : "Disabled";
			return TextCommandResult.Success(UIUtils.I18n("Command.UseNametagDynamicRange.Success." + stateAsText), null);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006749 File Offset: 0x00004949
		private void OnPlayerJoin(IServerPlayer player)
		{
			this.SendNametagConfigToPlayer(player);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006754 File Offset: 0x00004954
		private void SendNametagConfigToPlayer(IServerPlayer player)
		{
			NametagConfigChangedPacket packet = new NametagConfigChangedPacket(WorldConfig.GetForceSpeakerNametag(), WorldConfig.GetPlayerNametagTargetedOnly(), WorldConfig.GetBool("use-nametag-dynamic-range", true));
			RPVoiceChatMod.NametagConfigServerChannel.SendPacket<NametagConfigChangedPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006794 File Offset: 0x00004994
		private void BroadcastNametagConfigChanged()
		{
			foreach (IServerPlayer player in RPVoiceChatMod.sapi.World.AllOnlinePlayers)
			{
				if (player.ConnectionState == 4)
				{
					this.SendNametagConfigToPlayer(player);
				}
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000067D8 File Offset: 0x000049D8
		private TextCommandResult ResetDistanceHandler(TextCommandCallingArgs args)
		{
			WorldConfig.Set(VoiceLevel.Whispering, 5);
			WorldConfig.Set(VoiceLevel.Talking, 15);
			WorldConfig.Set(VoiceLevel.Shouting, 25);
			return TextCommandResult.Success(UIUtils.I18n("Command.Reset.Success"), null);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006804 File Offset: 0x00004A04
		private TextCommandResult DisplayInfoHandler(TextCommandCallingArgs args)
		{
			int whisper = WorldConfig.GetInt(VoiceLevel.Whispering, default(int?));
			int talk = WorldConfig.GetInt(VoiceLevel.Talking, default(int?));
			int shout = WorldConfig.GetInt(VoiceLevel.Shouting, default(int?));
			bool forceSpeakerNametag = WorldConfig.GetForceSpeakerNametag();
			bool encoding = WorldConfig.GetBool("encode-audio", false);
			bool useNametagDynamicRange = WorldConfig.GetBool("use-nametag-dynamic-range", true);
			return TextCommandResult.Success(UIUtils.I18n("Command.Info.Success", new object[]
			{
				whisper,
				talk,
				shout,
				forceSpeakerNametag,
				encoding,
				useNametagDynamicRange
			}), null);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000068B4 File Offset: 0x00004AB4
		private TextCommandResult SetWhisperHandler(TextCommandCallingArgs args)
		{
			int distance = (int)args[0];
			WorldConfig.Set(VoiceLevel.Whispering, distance);
			return TextCommandResult.Success(UIUtils.I18n("Command.Whisper.Success", new object[]
			{
				distance
			}), null);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000068F4 File Offset: 0x00004AF4
		private TextCommandResult SetTalkHandler(TextCommandCallingArgs args)
		{
			int distance = (int)args[0];
			WorldConfig.Set(VoiceLevel.Talking, distance);
			return TextCommandResult.Success(UIUtils.I18n("Command.Talk.Success", new object[]
			{
				distance
			}), null);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006938 File Offset: 0x00004B38
		private TextCommandResult SetShoutHandler(TextCommandCallingArgs args)
		{
			int distance = (int)args[0];
			WorldConfig.Set(VoiceLevel.Shouting, distance);
			return TextCommandResult.Success(UIUtils.I18n("Command.Shout.Success", new object[]
			{
				distance
			}), null);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000697C File Offset: 0x00004B7C
		private TextCommandResult VoiceBanHandler(TextCommandCallingArgs args)
		{
			string playerIdentifier = (string)args[0];
			IPlayer targetPlayer = this.FindPlayer(playerIdentifier);
			if (targetPlayer == null)
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.VoiceBan.PlayerNotFound"), "");
			}
			VoiceBanManager banManager = this.server.GetVoiceBanManager();
			if (banManager.IsPlayerBanned(targetPlayer.PlayerUID))
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.VoiceBan.AlreadyBanned", new object[]
				{
					targetPlayer.PlayerName
				}), "");
			}
			banManager.BanPlayer(targetPlayer.PlayerUID);
			this.server.NotifyAllPlayersBanStatus(targetPlayer.PlayerUID, true);
			return TextCommandResult.Success(UIUtils.I18n("Command.VoiceBan.Success", new object[]
			{
				targetPlayer.PlayerName
			}), null);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006A34 File Offset: 0x00004C34
		private TextCommandResult VoiceUnbanHandler(TextCommandCallingArgs args)
		{
			string playerIdentifier = (string)args[0];
			IPlayer targetPlayer = this.FindPlayer(playerIdentifier);
			if (targetPlayer == null)
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.VoiceUnban.PlayerNotFound"), "");
			}
			VoiceBanManager banManager = this.server.GetVoiceBanManager();
			if (!banManager.IsPlayerBanned(targetPlayer.PlayerUID))
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.VoiceUnban.NotBanned", new object[]
				{
					targetPlayer.PlayerName
				}), "");
			}
			banManager.UnbanPlayer(targetPlayer.PlayerUID);
			this.server.NotifyAllPlayersBanStatus(targetPlayer.PlayerUID, false);
			return TextCommandResult.Success(UIUtils.I18n("Command.VoiceUnban.Success", new object[]
			{
				targetPlayer.PlayerName
			}), null);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006AEC File Offset: 0x00004CEC
		private IPlayer FindPlayer(string identifier)
		{
			IPlayer player = RPVoiceChatMod.sapi.World.PlayerByUid(identifier);
			if (player != null)
			{
				return player;
			}
			player = Enumerable.FirstOrDefault<IPlayer>(RPVoiceChatMod.sapi.World.AllOnlinePlayers, (IPlayer p) => p.PlayerName.Equals(identifier, 5));
			if (player != null)
			{
				return player;
			}
			return Enumerable.FirstOrDefault<IPlayer>(RPVoiceChatMod.sapi.World.AllPlayers, (IPlayer p) => p.PlayerName.Equals(identifier, 5));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006B6C File Offset: 0x00004D6C
		private TextCommandResult VoiceBanListHandler(TextCommandCallingArgs args)
		{
			VoiceBanManager banManager = this.server.GetVoiceBanManager();
			List<string> bannedPlayers = banManager.GetBannedPlayers();
			if (bannedPlayers.Count == 0)
			{
				return TextCommandResult.Success(UIUtils.I18n("Command.VoiceBanList.Empty"), null);
			}
			List<string> playerNames = new List<string>();
			foreach (string playerUID in bannedPlayers)
			{
				string playerName = banManager.GetPlayerName(playerUID);
				playerNames.Add(playerName + " (" + playerUID + ")");
			}
			return TextCommandResult.Success(UIUtils.I18n("Command.VoiceBanList.Success", new object[]
			{
				string.Join(", ", playerNames)
			}), null);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006C30 File Offset: 0x00004E30
		private TextCommandResult AnnounceHandler(TextCommandCallingArgs args)
		{
			string fullText = (string)args[0];
			if (string.IsNullOrWhiteSpace(fullText))
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.Announce.Usage"), "");
			}
			string[] parts = fullText.Split('|', 0);
			if (parts.Length < 2)
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.Announce.MissingSeparator"), "");
			}
			string title = parts[0].Trim();
			string message = parts[1].Trim();
			double duration = 5.0;
			bool showBackground = true;
			if (parts.Length >= 3)
			{
				string durationStr = parts[2].Trim();
				if (!string.IsNullOrWhiteSpace(durationStr) && (!double.TryParse(durationStr, ref duration) || duration <= 0.0))
				{
					return TextCommandResult.Error(UIUtils.I18n("Command.Announce.InvalidDuration", new object[]
					{
						durationStr
					}), "");
				}
			}
			if (parts.Length >= 4)
			{
				string bgFlag = parts[3].Trim().ToLowerInvariant();
				if (bgFlag == "glass" || bgFlag == "transparent" || bgFlag == "1")
				{
					showBackground = false;
				}
			}
			if (string.IsNullOrWhiteSpace(title))
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.Announce.EmptyTitle"), "");
			}
			if (string.IsNullOrWhiteSpace(message))
			{
				return TextCommandResult.Error(UIUtils.I18n("Command.Announce.EmptyMessage"), "");
			}
			AnnouncePacket packet = new AnnouncePacket(title, message, duration, showBackground);
			foreach (IServerPlayer player in RPVoiceChatMod.sapi.World.AllOnlinePlayers)
			{
				if (player.ConnectionState == 4)
				{
					RPVoiceChatMod.AnnounceServerChannel.SendPacket<AnnouncePacket>(packet, new IServerPlayer[]
					{
						player
					});
				}
			}
			int playerCount = Enumerable.Count<IPlayer>(RPVoiceChatMod.sapi.World.AllOnlinePlayers);
			if (showBackground)
			{
				return TextCommandResult.Success(UIUtils.I18n("Command.Announce.Success", new object[]
				{
					playerCount,
					duration
				}), null);
			}
			return TextCommandResult.Success(UIUtils.I18n("Command.Announce.SuccessGlass", new object[]
			{
				playerCount,
				duration
			}), null);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006E46 File Offset: 0x00005046
		public override void Dispose()
		{
			if (RPVoiceChatMod.sapi != null)
			{
				RPVoiceChatMod.sapi.Event.PlayerJoin -= new PlayerDelegate(this.OnPlayerJoin);
			}
			GameServer gameServer = this.server;
			if (gameServer == null)
			{
				return;
			}
			gameServer.Dispose();
		}

		// Token: 0x04000057 RID: 87
		private GameServer server;
	}
}
