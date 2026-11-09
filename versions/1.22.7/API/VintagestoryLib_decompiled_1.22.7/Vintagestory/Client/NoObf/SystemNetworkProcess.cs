using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Common;
using Vintagestory.Common.Network.Packets;
using Vintagestory.Server.Systems;

namespace Vintagestory.Client.NoObf;

public class SystemNetworkProcess : ClientSystem
{
	private Stack<Packet_ServerChunk> cheapFixChunkQueue = new Stack<Packet_ServerChunk>();

	private int totalByteCount;

	private int deltaByteCount;

	private int totalUdpByteCount;

	private int deltaUdpByteCount;

	private readonly bool packetDebug;

	private bool doBenchmark;

	private readonly SortedDictionary<int, int> packetBenchmark = new SortedDictionary<int, int>();

	private readonly SortedDictionary<int, int> udpPacketBenchmark = new SortedDictionary<int, int>();

	private bool commitingMinimalUpdate;

	private readonly IClientNetworkChannel clientNetworkChannel;

	public static readonly Dictionary<int, string> ServerPacketNames;

	public bool DidReceiveUdp;

	private bool bulkPositions;

	public override string Name => "nwp";

	public int TotalBytesReceivedAndReceiving => totalByteCount + game.MainNetClient.CurrentlyReceivingBytes;

	static SystemNetworkProcess()
	{
		ServerPacketNames = new Dictionary<int, string>();
		FieldInfo[] infos = typeof(Packet_ServerIdEnum).GetFields();
		for (int i = 0; i < infos.Length; i++)
		{
			if ((infos[i].Attributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope && !(infos[i].FieldType != typeof(int)))
			{
				ServerPacketNames[(int)infos[i].GetValue(null)] = infos[i].Name;
			}
		}
	}

	public SystemNetworkProcess(ClientMain game)
		: base(game)
	{
		totalByteCount = 0;
		game.RegisterGameTickListener(UpdatePacketCount, 1000);
		game.RegisterGameTickListener(ClientUdpTick, 15);
		game.PacketHandlers[78] = HandleRequestPositionTcp;
		game.PacketHandlers[79] = EnqueueUdpPacket;
		game.PacketHandlers[80] = HandleEntitySpawnPosition;
		game.PacketHandlers[81] = HandleDidReceiveUdp;
		game.api.ChatCommands.Create("netbenchmark").WithDescription("Toggles network benchmarking").HandleWith(CmdBenchmark);
		clientNetworkChannel = game.api.Network.RegisterChannel("EntityAnims");
		clientNetworkChannel.RegisterMessageType<AnimationPacket>().RegisterMessageType<BulkAnimationPacket>().RegisterMessageType<EntityTagPacket>()
			.SetMessageHandler<AnimationPacket>(HandleAnimationPacket)
			.SetMessageHandler<BulkAnimationPacket>(HandleBulkAnimationPacket)
			.SetMessageHandler<EntityTagPacket>(HandleTagPacket);
		packetDebug = ClientSettings.Inst.Bool["packetDebug"];
	}

	private void UdpConnectionRequestFromServer()
	{
		if (!DidReceiveUdp)
		{
			game.Logger.Debug("UDP: Server send UDP connect");
			DidReceiveUdp = true;
		}
	}

	private void HandleDidReceiveUdp(Packet_Server packet)
	{
		game.UdpTryConnect = false;
		game.Logger.Debug("UDP: Server send DidReceiveUdp");
		TyronThreadPool.QueueTask(async delegate
		{
			for (int i = 0; i < 20; i++)
			{
				await Task.Delay(500);
				if (game.disposed)
				{
					return;
				}
				if (DidReceiveUdp || game.FallBackToTcp)
				{
					break;
				}
			}
			if (!DidReceiveUdp)
			{
				Packet_Client packetClient = new Packet_Client
				{
					Id = 34
				};
				game.Logger.Debug("UDP: Server did not receive any UDP packets and requests position updates over TCP");
				game.SendPacketClient(packetClient);
			}
			else
			{
				game.Logger.Debug("UDP: Client can receive UDP packets");
			}
		}, "UDPCheckForTCPFallback");
	}

	private void HandleEntitySpawnPosition(Packet_Server packet)
	{
		HandleSinglePacket(packet.EntityPosition);
	}

	private void EnqueueUdpPacket(Packet_Server packet)
	{
		game.UdpNetClient.EnqueuePacket(packet.UdpPacket);
	}

	private void HandleRequestPositionTcp(Packet_Server packet)
	{
		game.Logger.Notification("UDP: Server requested to fallback to use only TCP");
		game.FallBackToTcp = true;
		game.UdpTryConnect = false;
	}

	public void StartUdpConnectRequest(string token)
	{
		if (ClientSettings.ForceUdpOverTcp && !game.IsSingleplayer)
		{
			game.Logger.Notification("UDP: is disabled in clientsettings: forceUdpOverTcp , using only TCP now");
			Packet_Client packetClient = new Packet_Client
			{
				Id = 34
			};
			game.SendPacketClient(packetClient);
			return;
		}
		game.UdpTryConnect = true;
		game.UdpNetClient.DidReceiveUdpConnectionRequest += UdpConnectionRequestFromServer;
		TyronThreadPool.QueueTask(async delegate
		{
			Packet_ConnectionPacket con = new Packet_ConnectionPacket
			{
				LoginToken = token
			};
			Packet_UdpPacket udpPacket = new Packet_UdpPacket
			{
				Id = 1,
				ConnectionPacket = con
			};
			game.Logger.Debug("UDP: sending connection requests");
			while (game.UdpTryConnect && !game.disposed)
			{
				game.UdpNetClient.Send(udpPacket);
				if (game.IsSingleplayer)
				{
					game.UdpTryConnect = false;
					DidReceiveUdp = true;
					break;
				}
				await Task.Delay(500);
			}
		}, "UDPClientConnectionRequests");
		if (game.IsSingleplayer)
		{
			return;
		}
		TyronThreadPool.QueueTask(async delegate
		{
			Packet_UdpPacket udpPacket2 = new Packet_UdpPacket
			{
				Id = 7
			};
			while (!game.disposed && !game.FallBackToTcp)
			{
				game.UdpNetClient.Send(udpPacket2);
				await Task.Delay(10000);
			}
		}, "UDPServerConnectionKeepAlive");
	}

	public void HandleTagPacket(EntityTagPacket packet)
	{
		Entity entity = game.GetEntityById(packet.EntityId);
		if (entity != null)
		{
			entity.Tags = new TagSetFast(Vector256.Create((ulong)packet.TagsBitmask1, (ulong)packet.TagsBitmask2, (ulong)packet.TagsBitmask3, (ulong)packet.TagsBitmask4));
		}
	}

	public void HandleAnimationPacket(AnimationPacket packet)
	{
		Entity entity = game.GetEntityById(packet.entityId);
		if (entity != null && entity.Properties?.Client?.LoadedShapeForEntity?.Animations != null)
		{
			float[] speeds = new float[packet.activeAnimationSpeedsCount];
			for (int x = 0; x < speeds.Length; x++)
			{
				speeds[x] = CollectibleNet.DeserializeFloatPrecise(packet.activeAnimationSpeeds[x]);
			}
			entity.OnReceivedServerAnimations(packet.activeAnimations, packet.activeAnimationsCount, speeds);
		}
	}

	public void HandleBulkAnimationPacket(BulkAnimationPacket bulkPacket)
	{
		if (bulkPacket.Packets == null)
		{
			return;
		}
		for (int i = 0; i < bulkPacket.Packets.Length; i++)
		{
			AnimationPacket packet = bulkPacket.Packets[i];
			Entity entity = game.GetEntityById(packet.entityId);
			if (entity != null && entity.Properties?.Client?.LoadedShapeForEntity?.Animations != null)
			{
				float[] speeds = new float[packet.activeAnimationSpeedsCount];
				for (int x = 0; x < speeds.Length; x++)
				{
					speeds[x] = CollectibleNet.DeserializeFloatPrecise(packet.activeAnimationSpeeds[x]);
				}
				entity.OnReceivedServerAnimations(packet.activeAnimations, packet.activeAnimationsCount, speeds);
			}
		}
	}

	private void ClientUdpTick(float obj)
	{
		if (game.UdpNetClient == null)
		{
			return;
		}
		IEnumerable<Packet_UdpPacket> packets = game.UdpNetClient.ReadMessage();
		if (packets == null)
		{
			return;
		}
		foreach (Packet_UdpPacket packet in packets)
		{
			int udpByteCount = packet.Length;
			if (packetDebug)
			{
				ScreenManager.EnqueueMainThreadTask(delegate
				{
					game.Logger.VerboseDebug("Received UDP packet id {0}, dataLength {1}", packet.Id, udpByteCount);
				});
			}
			UpdateUdpStatsAndBenchmark(packet, udpByteCount);
			switch (packet.Id)
			{
			case 4:
				HandleBulkPacket(packet.BulkPositions);
				break;
			case 5:
				HandleSinglePacket(packet.EntityPosition);
				break;
			case 6:
				game.HandleCustomUdpPackets(packet.ChannelPacket);
				break;
			}
		}
	}

	private void UpdateUdpStatsAndBenchmark(Packet_UdpPacket packet, int udpByteCount)
	{
		if (doBenchmark)
		{
			if (udpPacketBenchmark.TryGetValue(packet.Id, out var benchmark))
			{
				udpPacketBenchmark[packet.Id] = benchmark + udpByteCount;
			}
			else
			{
				udpPacketBenchmark[packet.Id] = udpByteCount;
			}
		}
		totalUdpByteCount += udpByteCount;
		deltaUdpByteCount += udpByteCount;
	}

	private TextCommandResult CmdBenchmark(TextCommandCallingArgs textCommandCallingArgs)
	{
		doBenchmark = !doBenchmark;
		if (!doBenchmark)
		{
			StringBuilder str = new StringBuilder();
			foreach (KeyValuePair<int, int> val in packetBenchmark)
			{
				ServerPacketNames.TryGetValue(val.Key, out var packetName);
				str.AppendLine(packetName + ": " + ((val.Value > 9999) ? (((float)val.Value / 1024f).ToString("#.#") + "kb") : (val.Value + "b")));
			}
			foreach (KeyValuePair<int, int> val2 in udpPacketBenchmark)
			{
				string packetName2 = val2.Key.ToString();
				str.AppendLine(packetName2 + ": " + ((val2.Value > 9999) ? (((float)val2.Value / 1024f).ToString("#.#") + "kb") : (val2.Value + "b")));
			}
			return TextCommandResult.Success(str.ToString());
		}
		packetBenchmark.Clear();
		return TextCommandResult.Success("Benchmarking started. Stop it after a while to get results.");
	}

	private void UpdatePacketCount(float dt)
	{
		if (game.extendedDebugInfo)
		{
			string deltaByte = ((deltaByteCount > 1024) ? (((float)deltaByteCount / 1024f).ToString("0.0") + "kb/s") : (deltaByteCount + "b/s"));
			string deltaUdpByte = ((deltaUdpByteCount > 1024) ? (((float)deltaUdpByteCount / 1024f).ToString("0.0") + "kb/s") : (deltaUdpByteCount + "b/s"));
			game.DebugScreenInfo["incomingbytes"] = "Network TCP/UDP: " + ((float)totalByteCount / 1024f).ToString("#.#", GlobalConstants.DefaultCultureInfo) + " kb, " + deltaByte + " / " + ((float)totalUdpByteCount / 1024f).ToString("#.#", GlobalConstants.DefaultCultureInfo) + " kb, " + deltaUdpByte;
		}
		else
		{
			game.DebugScreenInfo["incomingbytes"] = "";
		}
		deltaByteCount = 0;
		deltaUdpByteCount = 0;
	}

	public override void OnSeperateThreadGameTick(float dt)
	{
		if (game.MainNetClient == null)
		{
			return;
		}
		while (true)
		{
			NetIncomingMessage msg = game.MainNetClient.ReadMessage();
			if (msg != null)
			{
				totalByteCount += msg.originalMessageLength;
				deltaByteCount += msg.originalMessageLength;
				TryReadPacket(msg.message, msg.messageLength);
				continue;
			}
			break;
		}
	}

	public void TryReadPacket(byte[] data, int dataLength)
	{
		Packet_Server packet = new Packet_Server();
		Packet_ServerSerializer.DeserializeBuffer(data, dataLength, packet);
		if (game.disposed)
		{
			return;
		}
		if (packetDebug)
		{
			ScreenManager.EnqueueMainThreadTask(delegate
			{
				game.Logger.VerboseDebug("Received packet id {0}, dataLength {1}", packet.Id, dataLength);
			});
		}
		if (doBenchmark)
		{
			if (packetBenchmark.TryGetValue(packet.Id, out var benchmark))
			{
				packetBenchmark[packet.Id] = benchmark + data.Length;
			}
			else
			{
				packetBenchmark[packet.Id] = data.Length;
			}
		}
		if (ProcessInBackground(packet))
		{
			return;
		}
		ProcessPacketTask task = new ProcessPacketTask
		{
			game = game,
			packet = packet
		};
		if (packet.Id == 73)
		{
			game.ServerReady = true;
			if (game.IsSingleplayer && game.GameLaunchTasks.Count > 0)
			{
				game.Logger.VerboseDebug("ServerIdentification packet received; will wait until block tesselation is complete to handle it");
			}
		}
		if (false)
		{
			string taskId = "readpacket" + packet.Id;
			game.EnqueueMainThreadTask(delegate
			{
				game.EnqueueMainThreadTask(delegate
				{
					game.EnqueueMainThreadTask(delegate
					{
						game.EnqueueMainThreadTask(delegate
						{
							game.EnqueueMainThreadTask(task.Run, taskId);
						}, taskId);
					}, taskId);
				}, taskId);
			}, taskId);
		}
		else
		{
			game.EnqueueMainThreadTask(task.Run, "readpacket" + packet.Id);
		}
		game.LastReceivedMilliseconds = game.Platform.EllapsedMs;
	}

	public override int SeperateThreadTickIntervalMs()
	{
		return 1;
	}

	private bool ProcessInBackground(Packet_Server packet)
	{
		switch (packet.Id)
		{
		case 4:
			game.WorldMap.ServerChunkSize = packet.LevelInitialize.ServerChunkSize;
			game.WorldMap.MapChunkSize = packet.LevelInitialize.ServerMapChunkSize;
			game.WorldMap.regionSize = packet.LevelInitialize.ServerMapRegionSize;
			game.WorldMap.MaxViewDistance = packet.LevelInitialize.MaxViewDistance;
			return false;
		case 10:
		{
			if (!game.BlocksReceivedAndLoaded)
			{
				for (int i3 = 0; i3 < packet.Chunks.ChunksCount; i3++)
				{
					cheapFixChunkQueue.Push(packet.Chunks.Chunks[i3]);
				}
				return true;
			}
			while (cheapFixChunkQueue.Count > 0)
			{
				Packet_ServerChunk p = cheapFixChunkQueue.Pop();
				game.WorldMap.LoadChunkFromPacket(p);
				RuntimeStats.chunksReceived++;
			}
			for (int i4 = 0; i4 < packet.Chunks.ChunksCount; i4++)
			{
				Packet_ServerChunk p2 = packet.Chunks.Chunks[i4];
				game.WorldMap.LoadChunkFromPacket(p2);
				RuntimeStats.chunksReceived++;
			}
			return true;
		}
		case 19:
			game.PacketHandlers[packet.Id]?.Invoke(packet);
			return true;
		case 21:
			game.EnqueueGameLaunchTask(delegate
			{
				game.PacketHandlers[packet.Id]?.Invoke(packet);
			}, "worldmetadatareceived");
			return true;
		case 17:
		{
			long index2d = game.WorldMap.MapChunkIndex2D(packet.MapChunk.ChunkX, packet.MapChunk.ChunkZ);
			game.WorldMap.MapChunks.TryGetValue(index2d, out var mapchunk);
			if (mapchunk == null)
			{
				mapchunk = new ClientMapChunk();
			}
			mapchunk.UpdateFromPacket(packet.MapChunk);
			game.WorldMap.MapChunks[index2d] = mapchunk;
			return true;
		}
		case 42:
		{
			long index2d2 = game.WorldMap.MapRegionIndex2D(packet.MapRegion.RegionX, packet.MapRegion.RegionZ);
			game.WorldMap.MapRegions.TryGetValue(index2d2, out var region);
			if (region == null)
			{
				region = new ClientMapRegion();
			}
			region.UpdateFromPacket(packet);
			game.WorldMap.MapRegions[index2d2] = region;
			game.EnqueueMainThreadTask(delegate
			{
				game.api.eventapi.TriggerMapregionLoaded(new Vec2i(packet.MapRegion.RegionX, packet.MapRegion.RegionZ), region);
			}, "mapregionloadedevent");
			return true;
		}
		case 47:
		{
			if (!game.Spawned)
			{
				return true;
			}
			int[] liquidLayer2;
			KeyValuePair<BlockPos[], int[]> pair2 = BlockTypeNet.UnpackSetBlocks(packet.SetBlocks.SetBlocks, out liquidLayer2);
			game.EnqueueMainThreadTask(delegate
			{
				BlockPos[] key = pair2.Key;
				int[] value = pair2.Value;
				for (int j = 0; j < key.Length; j++)
				{
					game.WorldMap.BulkBlockAccess.SetBlock(value[j], key[j]);
					game.eventManager?.TriggerBlockChanged(game, key[j], null);
				}
				if (liquidLayer2 != null)
				{
					for (int k = 0; k < key.Length; k++)
					{
						game.WorldMap.BulkBlockAccess.SetBlock(liquidLayer2[k], key[k], 2);
					}
					game.WorldMap.BulkBlockAccess.Commit();
				}
			}, "setblocks");
			return true;
		}
		case 63:
		{
			int[] liquidLayer;
			KeyValuePair<BlockPos[], int[]> pair = BlockTypeNet.UnpackSetBlocks(packet.SetBlocks.SetBlocks, out liquidLayer);
			game.EnqueueMainThreadTask(delegate
			{
				BlockPos[] key = pair.Key;
				int[] value = pair.Value;
				if (game.BlocksReceivedAndLoaded)
				{
					for (int j = 0; j < key.Length; j++)
					{
						game.WorldMap.NoRelightBulkBlockAccess.SetBlock(value[j], key[j]);
						game.eventManager?.TriggerBlockChanged(game, key[j], null);
					}
				}
				else
				{
					for (int k = 0; k < key.Length; k++)
					{
						game.WorldMap.NoRelightBulkBlockAccess.SetBlock(value[k], key[k]);
					}
				}
				game.WorldMap.NoRelightBulkBlockAccess.Commit();
				if (liquidLayer != null)
				{
					for (int l = 0; l < key.Length; l++)
					{
						game.WorldMap.NoRelightBulkBlockAccess.SetBlock(liquidLayer[l], key[l], 2);
					}
					game.WorldMap.NoRelightBulkBlockAccess.Commit();
				}
			}, "setblocksnorelight");
			return true;
		}
		case 70:
		{
			while (commitingMinimalUpdate)
			{
				Thread.Sleep(5);
			}
			int[] liquidLayer3;
			KeyValuePair<BlockPos[], int[]> pair3 = BlockTypeNet.UnpackSetBlocks(packet.SetBlocks.SetBlocks, out liquidLayer3);
			BlockPos[] positions = pair3.Key;
			int[] blockids = pair3.Value;
			for (int i = 0; i < positions.Length; i++)
			{
				BlockPos pos = positions[i];
				if (game.WorldMap.IsPosLoaded(pos))
				{
					game.WorldMap.BulkMinimalBlockAccess.SetBlock(blockids[i], pos);
				}
			}
			if (liquidLayer3 != null)
			{
				for (int i2 = 0; i2 < positions.Length; i2++)
				{
					game.WorldMap.BulkMinimalBlockAccess.SetBlock(liquidLayer3[i2], positions[i2], 2);
				}
			}
			commitingMinimalUpdate = true;
			game.EnqueueMainThreadTask(delegate
			{
				game.WorldMap.BulkMinimalBlockAccess.Commit();
				commitingMinimalUpdate = false;
			}, "setblocksminimal");
			return true;
		}
		case 71:
		{
			if (!game.Spawned)
			{
				return true;
			}
			long chunkIndex;
			Dictionary<int, Block> newDecors = BlockTypeNet.UnpackSetDecors(packet.SetDecors.SetDecors, game.WorldMap.World, out chunkIndex);
			game.EnqueueMainThreadTask(delegate
			{
				game.WorldMap.BulkBlockAccess.SetDecorsBulk(chunkIndex, newDecors);
			}, "setdecors");
			return true;
		}
		case 74:
			if (!game.Spawned)
			{
				return true;
			}
			game.EnqueueMainThreadTask(delegate
			{
				game.WorldMap.UnloadMapRegion(packet.UnloadMapRegion.RegionX, packet.UnloadMapRegion.RegionZ);
			}, "unloadmapregion");
			return true;
		default:
			return false;
		}
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Misc;
	}

	public void HandleSinglePacket(Packet_EntityPosition packet)
	{
		if (packet == null)
		{
			return;
		}
		Entity entity = game.GetEntityById(packet.EntityId);
		if (entity == null)
		{
			return;
		}
		int currentTick = entity.Attributes.GetInt("tick");
		if (currentTick == 0 && bulkPositions)
		{
			entity.Attributes.SetInt("tick", packet.Tick);
		}
		else
		{
			if (packet.Tick <= currentTick)
			{
				return;
			}
			entity.Attributes.SetInt("tickDiff", Math.Min(packet.Tick - currentTick, 5));
			entity.Attributes.SetInt("tick", packet.Tick);
			entity.Pos.SetFromPacket(packet, entity, sanityCheck: false);
			if (entity is EntityAgent agent)
			{
				agent.Controls.FromInt(packet.Controls & 0x210);
				if (agent.EntityId != game.EntityPlayer.EntityId)
				{
					agent.ServerControls.FromInt(packet.Controls);
				}
			}
			((entity.SidedProperties == null) ? null : entity.GetInterface<IMountable>()?.ControllingControls)?.FromInt(packet.MountControls);
			entity.OnReceivedServerPos(packet.Teleport);
			entity.Tags = new TagSetFast(Vector256.Create((ulong)packet.Tags.Part1, (ulong)packet.Tags.Part2, (ulong)packet.Tags.Part3, (ulong)packet.Tags.Part4));
		}
	}

	public void HandleBulkPacket(Packet_BulkEntityPosition bulkPacket)
	{
		if (bulkPacket.EntityPositions != null)
		{
			bulkPositions = true;
			Packet_EntityPosition[] entityPositions = bulkPacket.EntityPositions;
			foreach (Packet_EntityPosition packet in entityPositions)
			{
				HandleSinglePacket(packet);
			}
			bulkPositions = false;
		}
	}
}
