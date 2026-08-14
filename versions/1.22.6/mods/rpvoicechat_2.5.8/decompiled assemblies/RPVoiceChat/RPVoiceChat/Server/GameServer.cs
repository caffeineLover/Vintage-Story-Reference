using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.Networking;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Server
{
	// Token: 0x0200003F RID: 63
	public class GameServer : IDisposable
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x0000B284 File Offset: 0x00009484
		public GameServer(ICoreServerAPI sapi, List<INetworkServer> serverTransports, IEnumerable<IVoiceRouteProvider> voiceRouteProviders)
		{
			this.api = sapi;
			this._initialTransports = serverTransports;
			this.voiceRouteProviders = Enumerable.ToList<IVoiceRouteProvider>(Enumerable.Where<IVoiceRouteProvider>(voiceRouteProviders, (IVoiceRouteProvider provider) => provider != null));
			this.voiceBanManager = new VoiceBanManager(sapi);
			this.handshakeChannel = sapi.Network.RegisterChannel("RPVCHandshake").RegisterMessageType<ConnectionRequest>().RegisterMessageType<ConnectionInfo>().SetMessageHandler<ConnectionInfo>(new NetworkClientMessageHandler<ConnectionInfo>(this.FinalizeHandshake));
			this.voiceBanChannel = sapi.Network.RegisterChannel("RPVoiceBan").RegisterMessageType<VoiceBanStatusPacket>();
			this.listenerUpdateTickListener = sapi.Event.RegisterGameTickListener(new Action<float>(this.RebuildVoiceRoutingSnapshot), 500, 0);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B38C File Offset: 0x0000958C
		private void RebuildVoiceRoutingSnapshot(float gameTick)
		{
			Grid newGrid = Grid.Build(this.api, ServerConfigManager.GridCellSizeBlocks);
			ConcurrentDictionary<string, HashSet<IPlayer>> newListeners = new ConcurrentDictionary<string, HashSet<IPlayer>>();
			bool othersHearSpectators = WorldConfig.GetBool("others-hear-spectators", true);
			List<GridPlayer> candidates = new List<GridPlayer>(64);
			IReadOnlyList<GridPlayer> players = newGrid.Players;
			for (int i = 0; i < players.Count; i++)
			{
				GridPlayer transmittingPlayer = players[i];
				GameServer.MegaphoneInfo playerMegaphoneInfo = this.GetPlayerMegaphoneInfo(transmittingPlayer.Player);
				bool isGlobalBroadcast = playerMegaphoneInfo.HasEnhancedMegaphone;
				int effectiveDistance = GameServer.ResolveEffectiveListenerDistance(playerMegaphoneInfo);
				double squareDistance = isGlobalBroadcast ? double.MaxValue : ((double)effectiveDistance * (double)effectiveDistance);
				HashSet<IPlayer> listeners = new HashSet<IPlayer>();
				if (isGlobalBroadcast)
				{
					for (int candidateIndex = 0; candidateIndex < players.Count; candidateIndex++)
					{
						GameServer.TryAddNormalVoiceListener(transmittingPlayer, players[candidateIndex], listeners, isGlobalBroadcast, squareDistance, othersHearSpectators);
					}
				}
				else
				{
					newGrid.CollectNear(transmittingPlayer.Dimension, transmittingPlayer.X, transmittingPlayer.Z, effectiveDistance, candidates, true);
					for (int candidateIndex2 = 0; candidateIndex2 < candidates.Count; candidateIndex2++)
					{
						GameServer.TryAddNormalVoiceListener(transmittingPlayer, candidates[candidateIndex2], listeners, isGlobalBroadcast, squareDistance, othersHearSpectators);
					}
					candidates.Clear();
				}
				newListeners[transmittingPlayer.PlayerUID] = listeners;
			}
			this.voiceGrid = newGrid;
			this.playerListeners = newListeners;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B4D4 File Offset: 0x000096D4
		public void Launch()
		{
			this.LaunchServers();
			if (this.activeServers.Count == 0)
			{
				throw new Exception("Failed to launch any server");
			}
			this.api.Event.PlayerNowPlaying += new PlayerDelegate(this.PlayerJoined);
			this.api.Event.PlayerDisconnect += new PlayerDelegate(this.PlayerLeft);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000B537 File Offset: 0x00009737
		public void PlayerJoined(IServerPlayer player)
		{
			this.InitHandshake(player);
			this.SendAllBannedPlayersStatus(player);
			if (this.voiceBanManager.IsPlayerBanned(player.PlayerUID))
			{
				this.NotifyAllPlayersBanStatus(player.PlayerUID, true);
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000B568 File Offset: 0x00009768
		public void PlayerLeft(IServerPlayer player)
		{
			bool flag;
			this.devicesVoiceFeedbackByPlayer.TryRemove(player.PlayerUID, ref flag);
			foreach (INetworkServer networkServer in this.activeServers)
			{
				IExtendedNetworkServer extendedServer = networkServer as IExtendedNetworkServer;
				if (extendedServer != null)
				{
					extendedServer.PlayerDisconnected(player.PlayerUID);
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000B5DC File Offset: 0x000097DC
		public void SendAudioToAllClientsInRange(AudioPacket packet)
		{
			if (this.voiceBanManager.IsPlayerBanned(packet.PlayerId))
			{
				return;
			}
			IReadOnlyList<VoiceRoute> routes;
			if (this.TryResolveVoiceRoutes(packet.PlayerId, out routes))
			{
				this.SendRoutedVoiceAudio(packet, routes);
				return;
			}
			HashSet<IPlayer> recipients;
			if (!this.playerListeners.TryGetValue(packet.PlayerId, ref recipients))
			{
				return;
			}
			foreach (IPlayer recipient in recipients)
			{
				this.SendPacket(packet, recipient.PlayerUID);
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000B674 File Offset: 0x00009874
		private void SendRoutedVoiceAudio(AudioPacket packet, IReadOnlyList<VoiceRoute> routes)
		{
			if (routes == null || routes.Count == 0)
			{
				return;
			}
			Grid grid = this.voiceGrid ?? Grid.Empty;
			if (grid.IsEmpty)
			{
				grid = Grid.Build(this.api, ServerConfigManager.GridCellSizeBlocks);
			}
			Dictionary<string, GameServer.RoutedVoiceRecipient> dictionary;
			if ((dictionary = GameServer.tlsRoutedRecipients) == null)
			{
				dictionary = (GameServer.tlsRoutedRecipients = new Dictionary<string, GameServer.RoutedVoiceRecipient>(64));
			}
			Dictionary<string, GameServer.RoutedVoiceRecipient> recipients = dictionary;
			List<GridPlayer> list;
			if ((list = GameServer.tlsGridCandidates) == null)
			{
				list = (GameServer.tlsGridCandidates = new List<GridPlayer>(64));
			}
			List<GridPlayer> candidates = list;
			recipients.Clear();
			candidates.Clear();
			for (int routeIndex = 0; routeIndex < routes.Count; routeIndex++)
			{
				VoiceRoute route = routes[routeIndex];
				if (!(route.EmissionPos == null) && route.RangeBlocks > 0)
				{
					grid.CollectNear(route.Dimension, route.EmissionPos.X, route.EmissionPos.Z, route.RangeBlocks, candidates, true);
					for (int candidateIndex = 0; candidateIndex < candidates.Count; candidateIndex++)
					{
						this.TryAccumulateRoutedRecipient(packet, route, candidates[candidateIndex], recipients);
					}
					candidates.Clear();
				}
			}
			foreach (GameServer.RoutedVoiceRecipient recipient in recipients.Values)
			{
				AudioPacket routedPacket = GameServer.CloneAudioPacket(packet);
				routedPacket.TransmissionRangeBlocks = recipient.Route.RangeBlocks;
				routedPacket.HasSourcePositionOverride = true;
				routedPacket.SourcePosX = recipient.Route.EmissionPos.X;
				routedPacket.SourcePosY = recipient.Route.EmissionPos.Y;
				routedPacket.SourcePosZ = recipient.Route.EmissionPos.Z;
				this.SendPacket(routedPacket, recipient.PlayerUID);
			}
			recipients.Clear();
			candidates.Clear();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000B854 File Offset: 0x00009A54
		private static AudioPacket CloneAudioPacket(AudioPacket src)
		{
			return new AudioPacket
			{
				PlayerId = src.PlayerId,
				AudioData = src.AudioData,
				Length = src.Length,
				VoiceLevel = src.VoiceLevel,
				Frequency = src.Frequency,
				Format = src.Format,
				SequenceNumber = src.SequenceNumber,
				Codec = src.Codec,
				TransmissionRangeBlocks = src.TransmissionRangeBlocks,
				EffectiveRange = src.EffectiveRange,
				IgnoreDistanceReduction = src.IgnoreDistanceReduction,
				WallThicknessOverride = src.WallThicknessOverride,
				IsGlobalBroadcast = src.IsGlobalBroadcast,
				HasSourcePositionOverride = src.HasSourcePositionOverride,
				SourcePosX = src.SourcePosX,
				SourcePosY = src.SourcePosY,
				SourcePosZ = src.SourcePosZ
			};
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B934 File Offset: 0x00009B34
		private bool TryResolveVoiceRoutes(string playerUid, out IReadOnlyList<VoiceRoute> routes)
		{
			routes = null;
			if (this.voiceRouteProviders.Count == 0)
			{
				return false;
			}
			foreach (IVoiceRouteProvider voiceRouteProvider in this.voiceRouteProviders)
			{
				IVoiceMultiRouteProvider multiRouteProvider = voiceRouteProvider as IVoiceMultiRouteProvider;
				if (multiRouteProvider != null && multiRouteProvider.TryGetRoutes(playerUid, out routes) && routes != null && routes.Count > 0)
				{
					return true;
				}
			}
			using (IEnumerator<IVoiceRouteProvider> enumerator = this.voiceRouteProviders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vec3d emissionPos;
					int rangeBlocks;
					if (enumerator.Current.TryGetRoute(playerUid, out emissionPos, out rangeBlocks) && emissionPos != null && rangeBlocks > 0)
					{
						routes = new VoiceRoute[]
						{
							new VoiceRoute(emissionPos, rangeBlocks)
						};
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000BA1C File Offset: 0x00009C1C
		private void LaunchServers()
		{
			this.activeServers = new List<INetworkServer>();
			foreach (INetworkServer transport in this._initialTransports)
			{
				try
				{
					this.LaunchServer(transport);
					transport.AudioPacketReceived += new Action<AudioPacket>(this.SendAudioToAllClientsInRange);
				}
				catch (Exception e)
				{
					Logger server = Logger.server;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to launch ");
					defaultInterpolatedStringHandler.AppendFormatted(transport.GetTransportID());
					defaultInterpolatedStringHandler.AppendLiteral(" server:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					server.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					transport.Dispose();
				}
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000BAEC File Offset: 0x00009CEC
		private void LaunchServer(INetworkServer server)
		{
			string transportID = server.GetTransportID();
			Logger.server.Notification("Launching " + transportID + " server");
			server.Launch();
			this.activeServers.Add(server);
			this.serverByTransportID.Add(transportID, server);
			Logger.server.Notification(transportID + " server started");
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000BB50 File Offset: 0x00009D50
		private void InitHandshake(IServerPlayer player)
		{
			ConnectionRequest connectionRequest = this.GetConnectionRequest();
			this.handshakeChannel.SendPacket<ConnectionRequest>(connectionRequest, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000BB7C File Offset: 0x00009D7C
		private void FinalizeHandshake(IServerPlayer player, ConnectionInfo playerConnection)
		{
			string playerTransport = playerConnection.Transport;
			if (!this.serverByTransportID.ContainsKey(playerTransport))
			{
				return;
			}
			this.devicesVoiceFeedbackByPlayer[player.PlayerUID] = playerConnection.DevicesVoiceFeedback;
			IExtendedNetworkServer extendedServer = this.serverByTransportID[playerTransport] as IExtendedNetworkServer;
			if (extendedServer == null)
			{
				return;
			}
			try
			{
				playerConnection.Address = NetworkUtils.ParseIP(player.IpAddress).MapToIPv4().ToString();
				if (extendedServer != null)
				{
					extendedServer.PlayerConnected(player.PlayerUID, playerConnection);
				}
			}
			catch (Exception e)
			{
				Logger server = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(139, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Server failed to establish connection with ");
				defaultInterpolatedStringHandler.AppendFormatted(player.PlayerUID);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(player.PlayerName);
				defaultInterpolatedStringHandler.AppendLiteral(") over ");
				defaultInterpolatedStringHandler.AppendLiteral("requested transport: ");
				defaultInterpolatedStringHandler.AppendFormatted(playerTransport);
				defaultInterpolatedStringHandler.AppendLiteral(".\nServer will attempt to use other available transports to deliver ");
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				string text2 = "packets to this client. Mismatch between server and client transports can result in unstable behavior!\n";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(26, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Player address: ");
				defaultInterpolatedStringHandler2.AppendFormatted(player.IpAddress);
				defaultInterpolatedStringHandler2.AppendLiteral(", Reason: ");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(e);
				server.Warning(text + text2 + defaultInterpolatedStringHandler2.ToStringAndClear());
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000BCD4 File Offset: 0x00009ED4
		private void SendPacket(NetworkPacket packet, string playerId)
		{
			foreach (INetworkServer server in this.activeServers)
			{
				try
				{
					if (server.SendPacket(packet, playerId))
					{
						return;
					}
				}
				catch (Exception e)
				{
					Logger server2 = Logger.server;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Couldn't use ");
					defaultInterpolatedStringHandler.AppendFormatted(server.GetTransportID());
					defaultInterpolatedStringHandler.AppendLiteral(" server to deliver a packet to ");
					defaultInterpolatedStringHandler.AppendFormatted(playerId);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(e.Message);
					server2.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			Logger.server.Error("Failed to deliver a packet to " + playerId + ": All active servers refused to serve the player");
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000BDC0 File Offset: 0x00009FC0
		private ConnectionRequest GetConnectionRequest()
		{
			if (this.connectionRequest != null)
			{
				return this.connectionRequest;
			}
			List<ConnectionInfo> serverConnectionInfos = new List<ConnectionInfo>();
			foreach (INetworkServer server in this.activeServers)
			{
				ConnectionInfo connectionInfo = server.GetConnectionInfo();
				connectionInfo.Transport = server.GetTransportID();
				serverConnectionInfos.Add(connectionInfo);
			}
			this.connectionRequest = new ConnectionRequest(serverConnectionInfos);
			return this.connectionRequest;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000BE50 File Offset: 0x0000A050
		public void NotifyAllPlayersBanStatus(string playerUID, bool isBanned)
		{
			VoiceBanStatusPacket packet = new VoiceBanStatusPacket(playerUID, isBanned);
			foreach (IServerPlayer player in this.api.World.AllOnlinePlayers)
			{
				if (player.ConnectionState == 4)
				{
					this.voiceBanChannel.SendPacket<VoiceBanStatusPacket>(packet, new IServerPlayer[]
					{
						player
					});
				}
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		private void SendAllBannedPlayersStatus(IServerPlayer player)
		{
			foreach (string playerId in this.voiceBanManager.GetBannedPlayers())
			{
				VoiceBanStatusPacket packet = new VoiceBanStatusPacket(playerId, true);
				this.voiceBanChannel.SendPacket<VoiceBanStatusPacket>(packet, new IServerPlayer[]
				{
					player
				});
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public VoiceBanManager GetVoiceBanManager()
		{
			return this.voiceBanManager;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BF24 File Offset: 0x0000A124
		private static int ResolveEffectiveListenerDistance(GameServer.MegaphoneInfo megaphoneInfo)
		{
			if (megaphoneInfo.HasEnhancedMegaphone)
			{
				return int.MaxValue;
			}
			if (megaphoneInfo.HasMegaphone)
			{
				return ServerConfigManager.MegaphoneAudibleDistance + 10;
			}
			return WorldConfig.GetInt(VoiceLevel.Shouting, default(int?)) + 10;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000BF64 File Offset: 0x0000A164
		private static void TryAddNormalVoiceListener(GridPlayer transmittingPlayer, GridPlayer candidate, HashSet<IPlayer> listeners, bool isGlobalBroadcast, double squareDistance, bool othersHearSpectators)
		{
			if (candidate.PlayerUID == transmittingPlayer.PlayerUID)
			{
				return;
			}
			if (!othersHearSpectators && transmittingPlayer.IsSpectator && !candidate.IsSpectator)
			{
				return;
			}
			if (!isGlobalBroadcast && GameServer.SquareDistance(transmittingPlayer, candidate) > squareDistance)
			{
				return;
			}
			listeners.Add(candidate.Player);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		private void TryAccumulateRoutedRecipient(AudioPacket packet, VoiceRoute route, GridPlayer candidate, Dictionary<string, GameServer.RoutedVoiceRecipient> recipients)
		{
			if (candidate.Dimension != route.Dimension)
			{
				return;
			}
			bool devicesVoiceFeedback;
			bool allowEmitterFeedback = this.devicesVoiceFeedbackByPlayer.TryGetValue(candidate.PlayerUID, ref devicesVoiceFeedback) && devicesVoiceFeedback;
			if (packet.PlayerId == candidate.PlayerUID && !allowEmitterFeedback)
			{
				return;
			}
			double distanceSq = GameServer.SquareDistance(candidate.X, candidate.Y, candidate.Z, route.EmissionPos);
			GameServer.TrySetRoutedRecipient(candidate.PlayerUID, route, distanceSq, recipients);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000C030 File Offset: 0x0000A230
		private static void TrySetRoutedRecipient(string playerUid, VoiceRoute route, double distanceSq, Dictionary<string, GameServer.RoutedVoiceRecipient> recipients)
		{
			double maxDistanceSq = (double)route.RangeBlocks * (double)route.RangeBlocks;
			if (distanceSq > maxDistanceSq)
			{
				return;
			}
			GameServer.RoutedVoiceRecipient existing;
			if (recipients.TryGetValue(playerUid, ref existing) && existing.DistanceSq <= distanceSq)
			{
				return;
			}
			recipients[playerUid] = new GameServer.RoutedVoiceRecipient(playerUid, route, distanceSq);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000C078 File Offset: 0x0000A278
		private static double SquareDistance(GridPlayer a, GridPlayer b)
		{
			double num = a.X - b.X;
			double dy = a.Y - b.Y;
			double dz = a.Z - b.Z;
			return num * num + dy * dy + dz * dz;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
		private static double SquareDistance(double x, double y, double z, Vec3d other)
		{
			double num = x - other.X;
			double dy = y - other.Y;
			double dz = z - other.Z;
			return num * num + dy * dy + dz * dz;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		private GameServer.MegaphoneInfo GetPlayerMegaphoneInfo(IPlayer player)
		{
			GameServer.MegaphoneInfo info = default(GameServer.MegaphoneInfo);
			if (((player != null) ? player.Entity : null) == null)
			{
				return info;
			}
			ItemSlot activeSlot = player.Entity.RightHandItemSlot;
			bool flag;
			if (activeSlot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = activeSlot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Item : null) != null);
			}
			if (flag)
			{
				AssetLocation code = activeSlot.Itemstack.Item.Code;
				string itemCode = ((code != null) ? code.ToString() : null) ?? "";
				if (itemCode == "rpvoicechat:enhancedmegaphone")
				{
					info.HasEnhancedMegaphone = true;
					info.HasMegaphone = true;
				}
				else if (itemCode == "rpvoicechat:megaphone")
				{
					info.HasMegaphone = true;
				}
			}
			ItemSlot leftSlot = player.Entity.LeftHandItemSlot;
			bool flag2;
			if (leftSlot == null)
			{
				flag2 = (null != null);
			}
			else
			{
				ItemStack itemstack2 = leftSlot.Itemstack;
				flag2 = (((itemstack2 != null) ? itemstack2.Item : null) != null);
			}
			if (flag2)
			{
				AssetLocation code2 = leftSlot.Itemstack.Item.Code;
				string itemCode2 = ((code2 != null) ? code2.ToString() : null) ?? "";
				if (itemCode2 == "rpvoicechat:enhancedmegaphone")
				{
					info.HasEnhancedMegaphone = true;
					info.HasMegaphone = true;
				}
				else if (itemCode2 == "rpvoicechat:megaphone")
				{
					info.HasMegaphone = true;
				}
			}
			return info;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C218 File Offset: 0x0000A418
		public void Dispose()
		{
			try
			{
				foreach (INetworkServer networkServer in this.activeServers)
				{
					networkServer.Dispose();
				}
			}
			catch (Exception e)
			{
				Logger.server.Warning("Error disposing servers: " + e.Message);
			}
			finally
			{
				try
				{
					this.api.Event.PlayerNowPlaying -= new PlayerDelegate(this.PlayerJoined);
					this.api.Event.PlayerDisconnect -= new PlayerDelegate(this.PlayerLeft);
					this.api.Event.UnregisterGameTickListener(this.listenerUpdateTickListener);
				}
				catch (Exception e2)
				{
					Logger.server.Warning("Error unsubscribing server events: " + e2.Message);
				}
			}
		}

		// Token: 0x04000098 RID: 152
		private ICoreServerAPI api;

		// Token: 0x04000099 RID: 153
		private List<INetworkServer> _initialTransports;

		// Token: 0x0400009A RID: 154
		private List<INetworkServer> activeServers = new List<INetworkServer>();

		// Token: 0x0400009B RID: 155
		private IServerNetworkChannel handshakeChannel;

		// Token: 0x0400009C RID: 156
		private IServerNetworkChannel voiceBanChannel;

		// Token: 0x0400009D RID: 157
		private Dictionary<string, INetworkServer> serverByTransportID = new Dictionary<string, INetworkServer>();

		// Token: 0x0400009E RID: 158
		private ConnectionRequest connectionRequest;

		// Token: 0x0400009F RID: 159
		private VoiceBanManager voiceBanManager;

		// Token: 0x040000A0 RID: 160
		private long listenerUpdateTickListener;

		// Token: 0x040000A1 RID: 161
		private ConcurrentDictionary<string, HashSet<IPlayer>> playerListeners = new ConcurrentDictionary<string, HashSet<IPlayer>>();

		// Token: 0x040000A2 RID: 162
		private readonly ConcurrentDictionary<string, bool> devicesVoiceFeedbackByPlayer = new ConcurrentDictionary<string, bool>();

		// Token: 0x040000A3 RID: 163
		private readonly IReadOnlyList<IVoiceRouteProvider> voiceRouteProviders;

		// Token: 0x040000A4 RID: 164
		private volatile Grid voiceGrid = Grid.Empty;

		// Token: 0x040000A5 RID: 165
		[ThreadStatic]
		private static List<GridPlayer> tlsGridCandidates;

		// Token: 0x040000A6 RID: 166
		[ThreadStatic]
		private static Dictionary<string, GameServer.RoutedVoiceRecipient> tlsRoutedRecipients;

		// Token: 0x020000F1 RID: 241
		private struct MegaphoneInfo
		{
			// Token: 0x040003EA RID: 1002
			public bool HasMegaphone;

			// Token: 0x040003EB RID: 1003
			public bool HasEnhancedMegaphone;
		}

		// Token: 0x020000F2 RID: 242
		private readonly struct RoutedVoiceRecipient
		{
			// Token: 0x0600082B RID: 2091 RVA: 0x0002BC3F File Offset: 0x00029E3F
			public RoutedVoiceRecipient(string playerUid, VoiceRoute route, double distanceSq)
			{
				this.PlayerUID = playerUid;
				this.Route = route;
				this.DistanceSq = distanceSq;
			}

			// Token: 0x040003EC RID: 1004
			public readonly string PlayerUID;

			// Token: 0x040003ED RID: 1005
			public readonly VoiceRoute Route;

			// Token: 0x040003EE RID: 1006
			public readonly double DistanceSq;
		}
	}
}
