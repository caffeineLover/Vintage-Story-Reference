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
	// Token: 0x0200005A RID: 90
	public class GameServer : IDisposable
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000EC18 File Offset: 0x0000CE18
		public GameServer(ICoreServerAPI sapi, List<INetworkServer> serverTransports, IEnumerable<IVoiceRouteProvider> voiceRouteProviders, IEnumerable<IVoiceRecipientExpander> voiceRecipientExpanders = null)
		{
			this.api = sapi;
			this._initialTransports = serverTransports;
			this.voiceRouteProviders = Enumerable.ToList<IVoiceRouteProvider>(Enumerable.Where<IVoiceRouteProvider>(voiceRouteProviders, (IVoiceRouteProvider provider) => provider != null));
			IReadOnlyList<IVoiceRecipientExpander> readOnlyList;
			if (voiceRecipientExpanders == null)
			{
				readOnlyList = null;
			}
			else
			{
				readOnlyList = Enumerable.ToList<IVoiceRecipientExpander>(Enumerable.Where<IVoiceRecipientExpander>(voiceRecipientExpanders, (IVoiceRecipientExpander expander) => expander != null));
			}
			this.voiceRecipientExpanders = (readOnlyList ?? new List<IVoiceRecipientExpander>());
			this.voiceBanManager = new VoiceBanManager(sapi);
			this.handshakeChannel = sapi.Network.RegisterChannel("RPVCHandshake").RegisterMessageType<ConnectionRequest>().RegisterMessageType<ConnectionInfo>().SetMessageHandler<ConnectionInfo>(new NetworkClientMessageHandler<ConnectionInfo>(this.FinalizeHandshake));
			this.voiceBanChannel = sapi.Network.RegisterChannel("RPVoiceBan").RegisterMessageType<VoiceBanStatusPacket>();
			this.listenerUpdateTickListener = sapi.Event.RegisterGameTickListener(new Action<float>(this.RebuildVoiceRoutingSnapshot), 500, 0);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000ED60 File Offset: 0x0000CF60
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

		// Token: 0x0600023D RID: 573 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
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

		// Token: 0x0600023E RID: 574 RVA: 0x0000EF0B File Offset: 0x0000D10B
		public void PlayerJoined(IServerPlayer player)
		{
			this.InitHandshake(player);
			this.SendAllBannedPlayersStatus(player);
			if (this.voiceBanManager.IsPlayerBanned(player.PlayerUID))
			{
				this.NotifyAllPlayersBanStatus(player.PlayerUID, true);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000EF3C File Offset: 0x0000D13C
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

		// Token: 0x06000240 RID: 576 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		public void SetProgramMicAudioSink(Func<AudioPacket, bool> sink)
		{
			this.tryConsumeProgramMicAudio = sink;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000EFBC File Offset: 0x0000D1BC
		public void SendAudioToAllClientsInRange(AudioPacket packet)
		{
			if (this.voiceBanManager.IsPlayerBanned(packet.PlayerId))
			{
				return;
			}
			Func<AudioPacket, bool> func = this.tryConsumeProgramMicAudio;
			if (func != null && func.Invoke(packet))
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

		// Token: 0x06000242 RID: 578 RVA: 0x0000F06C File Offset: 0x0000D26C
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
			Dictionary<string, RoutedVoiceRecipient> dictionary;
			if ((dictionary = GameServer.tlsRoutedRecipients) == null)
			{
				dictionary = (GameServer.tlsRoutedRecipients = new Dictionary<string, RoutedVoiceRecipient>(64));
			}
			Dictionary<string, RoutedVoiceRecipient> recipients = dictionary;
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
				if (route.IsAcousticEmission && !(route.EmissionPos == null) && route.RangeBlocks > 0)
				{
					grid.CollectNear(route.Dimension, route.EmissionPos.X, route.EmissionPos.Z, route.RangeBlocks, candidates, true);
					for (int candidateIndex = 0; candidateIndex < candidates.Count; candidateIndex++)
					{
						this.TryAccumulateRoutedRecipient(packet, route, candidates[candidateIndex], recipients);
					}
					candidates.Clear();
				}
			}
			for (int expanderIndex = 0; expanderIndex < this.voiceRecipientExpanders.Count; expanderIndex++)
			{
				this.voiceRecipientExpanders[expanderIndex].ExpandRoutedRecipients(packet, routes, recipients);
			}
			foreach (RoutedVoiceRecipient recipient in recipients.Values)
			{
				AudioPacket routedPacket = GameServer.CloneAudioPacket(packet);
				routedPacket.TransmissionRangeBlocks = recipient.Route.RangeBlocks;
				routedPacket.HasSourcePositionOverride = true;
				routedPacket.SourcePosX = recipient.Route.EmissionPos.X;
				routedPacket.SourcePosY = recipient.Route.EmissionPos.Y;
				routedPacket.SourcePosZ = recipient.Route.EmissionPos.Z;
				routedPacket.IsGlobalBroadcast = false;
				routedPacket.IgnoreDistanceReduction = false;
				this.SendPacket(routedPacket, recipient.PlayerUID);
			}
			recipients.Clear();
			candidates.Clear();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000F294 File Offset: 0x0000D494
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

		// Token: 0x06000244 RID: 580 RVA: 0x0000F374 File Offset: 0x0000D574
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

		// Token: 0x06000245 RID: 581 RVA: 0x0000F45C File Offset: 0x0000D65C
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

		// Token: 0x06000246 RID: 582 RVA: 0x0000F52C File Offset: 0x0000D72C
		private void LaunchServer(INetworkServer server)
		{
			string transportID = server.GetTransportID();
			Logger.server.Notification("Launching " + transportID + " server");
			server.Launch();
			this.activeServers.Add(server);
			this.serverByTransportID.Add(transportID, server);
			Logger.server.Notification(transportID + " server started");
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000F590 File Offset: 0x0000D790
		private void InitHandshake(IServerPlayer player)
		{
			ConnectionRequest connectionRequest = this.GetConnectionRequest();
			this.handshakeChannel.SendPacket<ConnectionRequest>(connectionRequest, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000F5BC File Offset: 0x0000D7BC
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

		// Token: 0x06000249 RID: 585 RVA: 0x0000F714 File Offset: 0x0000D914
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

		// Token: 0x0600024A RID: 586 RVA: 0x0000F800 File Offset: 0x0000DA00
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

		// Token: 0x0600024B RID: 587 RVA: 0x0000F890 File Offset: 0x0000DA90
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

		// Token: 0x0600024C RID: 588 RVA: 0x0000F8EC File Offset: 0x0000DAEC
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

		// Token: 0x0600024D RID: 589 RVA: 0x0000F95C File Offset: 0x0000DB5C
		public VoiceBanManager GetVoiceBanManager()
		{
			return this.voiceBanManager;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000F964 File Offset: 0x0000DB64
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

		// Token: 0x0600024F RID: 591 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
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

		// Token: 0x06000250 RID: 592 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
		private void TryAccumulateRoutedRecipient(AudioPacket packet, VoiceRoute route, GridPlayer candidate, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			if (candidate.Dimension != route.Dimension)
			{
				return;
			}
			if (packet.PlayerId == candidate.PlayerUID)
			{
				if (!string.IsNullOrEmpty(route.RadioFrequency) && GameServer.SquareDistance(candidate.X, candidate.Y, candidate.Z, route.EmissionPos) <= 4.0)
				{
					return;
				}
				bool devicesVoiceFeedback;
				if (!this.devicesVoiceFeedbackByPlayer.TryGetValue(candidate.PlayerUID, ref devicesVoiceFeedback) || !devicesVoiceFeedback)
				{
					return;
				}
			}
			double distanceSq = GameServer.SquareDistance(candidate.X, candidate.Y, candidate.Z, route.EmissionPos);
			GameServer.TrySetRoutedRecipient(candidate.PlayerUID, route, distanceSq, recipients);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000FAA8 File Offset: 0x0000DCA8
		private static void TrySetRoutedRecipient(string playerUid, VoiceRoute route, double distanceSq, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			double maxDistanceSq = (double)route.RangeBlocks * (double)route.RangeBlocks;
			if (distanceSq > maxDistanceSq)
			{
				return;
			}
			RoutedVoiceRecipient existing;
			if (recipients.TryGetValue(playerUid, ref existing) && existing.DistanceSq <= distanceSq)
			{
				return;
			}
			recipients[playerUid] = new RoutedVoiceRecipient(playerUid, route, distanceSq);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		private static double SquareDistance(GridPlayer a, GridPlayer b)
		{
			double num = a.X - b.X;
			double dy = a.Y - b.Y;
			double dz = a.Z - b.Z;
			return num * num + dy * dy + dz * dz;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000FB30 File Offset: 0x0000DD30
		private static double SquareDistance(double x, double y, double z, Vec3d other)
		{
			double num = x - other.X;
			double dy = y - other.Y;
			double dz = z - other.Z;
			return num * num + dy * dy + dz * dz;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000FB64 File Offset: 0x0000DD64
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

		// Token: 0x06000255 RID: 597 RVA: 0x0000FC90 File Offset: 0x0000DE90
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

		// Token: 0x040000E3 RID: 227
		private ICoreServerAPI api;

		// Token: 0x040000E4 RID: 228
		private List<INetworkServer> _initialTransports;

		// Token: 0x040000E5 RID: 229
		private List<INetworkServer> activeServers = new List<INetworkServer>();

		// Token: 0x040000E6 RID: 230
		private IServerNetworkChannel handshakeChannel;

		// Token: 0x040000E7 RID: 231
		private IServerNetworkChannel voiceBanChannel;

		// Token: 0x040000E8 RID: 232
		private Dictionary<string, INetworkServer> serverByTransportID = new Dictionary<string, INetworkServer>();

		// Token: 0x040000E9 RID: 233
		private ConnectionRequest connectionRequest;

		// Token: 0x040000EA RID: 234
		private VoiceBanManager voiceBanManager;

		// Token: 0x040000EB RID: 235
		private long listenerUpdateTickListener;

		// Token: 0x040000EC RID: 236
		private ConcurrentDictionary<string, HashSet<IPlayer>> playerListeners = new ConcurrentDictionary<string, HashSet<IPlayer>>();

		// Token: 0x040000ED RID: 237
		private readonly ConcurrentDictionary<string, bool> devicesVoiceFeedbackByPlayer = new ConcurrentDictionary<string, bool>();

		// Token: 0x040000EE RID: 238
		private readonly IReadOnlyList<IVoiceRouteProvider> voiceRouteProviders;

		// Token: 0x040000EF RID: 239
		private readonly IReadOnlyList<IVoiceRecipientExpander> voiceRecipientExpanders;

		// Token: 0x040000F0 RID: 240
		private Func<AudioPacket, bool> tryConsumeProgramMicAudio;

		// Token: 0x040000F1 RID: 241
		private volatile Grid voiceGrid = Grid.Empty;

		// Token: 0x040000F2 RID: 242
		[ThreadStatic]
		private static List<GridPlayer> tlsGridCandidates;

		// Token: 0x040000F3 RID: 243
		[ThreadStatic]
		private static Dictionary<string, RoutedVoiceRecipient> tlsRoutedRecipients;

		// Token: 0x02000141 RID: 321
		private struct MegaphoneInfo
		{
			// Token: 0x04000544 RID: 1348
			public bool HasMegaphone;

			// Token: 0x04000545 RID: 1349
			public bool HasEnhancedMegaphone;
		}
	}
}
