using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000052 RID: 82
	public static class WireNetworkHandler
	{
		// Token: 0x060001EA RID: 490 RVA: 0x0000CCBC File Offset: 0x0000AEBC
		public static void RegisterServerside(ICoreServerAPI api)
		{
			IServerNetworkChannel serverNetworkChannel = api.Network.RegisterChannel(WireNetworkHandler.NetworkChannel).RegisterMessageType(typeof(WireNetworkMessage));
			NetworkClientMessageHandler<WireNetworkMessage> messageHandler;
			if ((messageHandler = WireNetworkHandler.<>O.<0>__OnReceivedMessage_Server) == null)
			{
				messageHandler = (WireNetworkHandler.<>O.<0>__OnReceivedMessage_Server = new NetworkClientMessageHandler<WireNetworkMessage>(WireNetworkHandler.OnReceivedMessage_Server));
			}
			WireNetworkHandler.ServerChannel = serverNetworkChannel.SetMessageHandler<WireNetworkMessage>(messageHandler);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000CD10 File Offset: 0x0000AF10
		public static void RegisterClientside(ICoreClientAPI api)
		{
			IClientNetworkChannel clientNetworkChannel = api.Network.RegisterChannel(WireNetworkHandler.NetworkChannel).RegisterMessageType(typeof(WireNetworkMessage));
			NetworkServerMessageHandler<WireNetworkMessage> messageHandler;
			if ((messageHandler = WireNetworkHandler.<>O.<1>__OnReceivedMessage_Client) == null)
			{
				messageHandler = (WireNetworkHandler.<>O.<1>__OnReceivedMessage_Client = new NetworkServerMessageHandler<WireNetworkMessage>(WireNetworkHandler.OnReceivedMessage_Client));
			}
			WireNetworkHandler.ClientChannel = clientNetworkChannel.SetMessageHandler<WireNetworkMessage>(messageHandler);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000CD61 File Offset: 0x0000AF61
		private static void OnReceivedMessage_Client(WireNetworkMessage packet)
		{
			EventHandler<WireNetworkMessage> clientSideMessageReceived = WireNetworkHandler.ClientSideMessageReceived;
			if (clientSideMessageReceived == null)
			{
				return;
			}
			clientSideMessageReceived.Invoke(null, packet);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000CD74 File Offset: 0x0000AF74
		private static void OnReceivedMessage_Server(IServerPlayer fromPlayer, WireNetworkMessage packet)
		{
			packet = WireNetworkHandler.ApplyRoutingOnServer(packet);
			WireNetworkHandler.ServerChannel.BroadcastPacket<WireNetworkMessage>(packet, Array.Empty<IServerPlayer>());
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000CD90 File Offset: 0x0000AF90
		private static WireNetworkMessage ApplyRoutingOnServer(WireNetworkMessage packet)
		{
			if (packet == null)
			{
				return null;
			}
			if (packet.RouteMode != WireRouteMode.NamedEndpoint || string.IsNullOrWhiteSpace(packet.TargetEndpointName))
			{
				packet.RouteMode = WireRouteMode.All;
				packet.TargetEndpointName = null;
				packet.TargetPos = null;
				return packet;
			}
			BlockEntityTelegraph target = WireNetworkHandler.ResolveTelegraphByName(packet.NetworkUID, packet.TargetEndpointName);
			if (target == null)
			{
				packet.RouteMode = WireRouteMode.All;
				packet.TargetEndpointName = null;
				packet.TargetPos = null;
				return packet;
			}
			packet.TargetPos = target.Pos.Copy();
			return packet;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000CE10 File Offset: 0x0000B010
		public static WireNetwork AddNewNetwork(BEWireNode wireNode)
		{
			long networkId = WireNetworkHandler.AllocateNetworkId();
			WireNetwork network = new WireNetwork
			{
				networkID = networkId
			};
			WireNetworkHandler.AddNetwork(network);
			network.JoinNode(wireNode);
			wireNode.OnNetworkCreated(networkId);
			WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(wireNode, network);
			return network;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000CE4C File Offset: 0x0000B04C
		private static long AllocateNetworkId()
		{
			long candidate = Math.Max(WireNetworkHandler.nextNetworkId, (WireNetworkHandler.Networks.Count > 0) ? Enumerable.Max(WireNetworkHandler.Networks.Keys) : 0L) + 1L;
			while (WireNetworkHandler.Networks.ContainsKey(candidate))
			{
				candidate += 1L;
			}
			WireNetworkHandler.nextNetworkId = candidate;
			return candidate;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
		public static void InitializeFromSave(byte[] networkBytes, byte[] nextIdBytes)
		{
			WireNetworkHandler.Networks.Clear();
			if (networkBytes == null || networkBytes.Length == 0)
			{
				return;
			}
			List<WireNetworkSaveData> savedNetworks = SerializerUtil.Deserialize<List<WireNetworkSaveData>>(networkBytes);
			if (savedNetworks == null)
			{
				return;
			}
			foreach (WireNetworkSaveData savedNetwork in savedNetworks)
			{
				if (savedNetwork != null && savedNetwork.NetworkId != 0L)
				{
					WireNetwork network = WireNetwork.FromSaveData(savedNetwork);
					WireNetworkHandler.Networks[network.networkID] = network;
				}
			}
			if (nextIdBytes != null && nextIdBytes.Length != 0)
			{
				WireNetworkHandler.nextNetworkId = SerializerUtil.Deserialize<long>(nextIdBytes);
				return;
			}
			if (WireNetworkHandler.Networks.Count > 0)
			{
				WireNetworkHandler.nextNetworkId = Enumerable.Max(WireNetworkHandler.Networks.Keys);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000CF60 File Offset: 0x0000B160
		public static void RejoinLoadedNodes(ICoreServerAPI api)
		{
			IBlockAccessor blockAccessor;
			if (api == null)
			{
				blockAccessor = null;
			}
			else
			{
				IServerWorldAccessor world = api.World;
				blockAccessor = ((world != null) ? world.BlockAccessor : null);
			}
			IBlockAccessor accessor = blockAccessor;
			if (accessor == null)
			{
				return;
			}
			foreach (WireNetwork network in WireNetworkHandler.Networks.Values)
			{
				if (network != null)
				{
					foreach (WireNodeRef nodeRef in network.PersistedNodes)
					{
						if (!(((nodeRef != null) ? nodeRef.Pos : null) == null))
						{
							BEWireNode node = accessor.GetBlockEntity(nodeRef.Pos) as BEWireNode;
							if (node != null)
							{
								long authoritativeId = WireNetworkHandler.ResolveNetworkIdForPosition(nodeRef.Pos);
								if (authoritativeId != 0L && node.NetworkUID != authoritativeId)
								{
									node.NetworkUID = authoritativeId;
									node.MarkForUpdate();
								}
								if (node.NetworkUID == network.networkID)
								{
									network.JoinNode(node);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000D08C File Offset: 0x0000B28C
		public static void PersistToSaveGame(ISaveGame saveGame)
		{
			if (saveGame == null)
			{
				return;
			}
			List<WireNetworkSaveData> payload = Enumerable.ToList<WireNetworkSaveData>(Enumerable.Select<WireNetwork, WireNetworkSaveData>(Enumerable.Where<WireNetwork>(WireNetworkHandler.Networks.Values, (WireNetwork network) => network != null && network.networkID != 0L && network.HasPersistedNodes), (WireNetwork network) => network.ToSaveData()));
			saveGame.StoreData("rpvc:wire-networks", SerializerUtil.Serialize<List<WireNetworkSaveData>>(payload));
			saveGame.StoreData("rpvc:wire-network-nextid", SerializerUtil.Serialize<long>(WireNetworkHandler.nextNetworkId));
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000D11B File Offset: 0x0000B31B
		public static void AddNetwork(WireNetwork network)
		{
			if (network == null)
			{
				return;
			}
			if (WireNetworkHandler.Networks.ContainsKey(network.networkID))
			{
				return;
			}
			WireNetworkHandler.Networks.Add(network.networkID, network);
			if (network.Nodes.Count > 0)
			{
				network.RebuildTypedState();
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000D159 File Offset: 0x0000B359
		public static void RemoveNetwork(WireNetwork network)
		{
			if (network == null)
			{
				return;
			}
			WireNetworkHandler.Networks.Remove(network.networkID);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D170 File Offset: 0x0000B370
		public static WireNetwork GetNetwork(long networkID)
		{
			if (networkID == 0L)
			{
				return null;
			}
			WireNetwork net;
			if (!WireNetworkHandler.Networks.TryGetValue(networkID, ref net))
			{
				return null;
			}
			return net;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000D194 File Offset: 0x0000B394
		public static WireNetwork GetNetwork(BEWireNode node)
		{
			if (node == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<WireNetwork>(WireNetworkHandler.Networks.Values, (WireNetwork nw) => nw.Nodes.Contains(node));
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		public static void PropagateNetworkUIDToConnectedNodes(BEWireNode startNode, WireNetwork network)
		{
			if (((startNode != null) ? startNode.Pos : null) == null || network == null)
			{
				return;
			}
			ICoreAPI api = startNode.Api;
			IBlockAccessor blockAccessor;
			if (api == null)
			{
				blockAccessor = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				blockAccessor = ((world != null) ? world.BlockAccessor : null);
			}
			IBlockAccessor accessor = blockAccessor;
			foreach (BlockPos pos in WireTopologyRegistry.GetConnectedComponent(startNode.Pos))
			{
				network.UpsertPersistedNodeFromPosition(pos, accessor);
				BEWireNode node = ((accessor != null) ? accessor.GetBlockEntity(pos) : null) as BEWireNode;
				if (node != null && node.NetworkUID != network.networkID)
				{
					node.NetworkUID = network.networkID;
					network.JoinNode(node);
					node.MarkForUpdate();
				}
			}
			network.RebuildTypedState();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D2A8 File Offset: 0x0000B4A8
		public static long ResolveNetworkIdForPosition(BlockPos pos)
		{
			if (pos == null)
			{
				return 0L;
			}
			Func<WireNodeRef, bool> <>9__0;
			foreach (WireNetwork network in WireNetworkHandler.Networks.Values)
			{
				if (((network != null) ? network.PersistedNodes : null) != null)
				{
					IEnumerable<WireNodeRef> persistedNodes = network.PersistedNodes;
					Func<WireNodeRef, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = ((WireNodeRef nodeRef) => nodeRef.Pos != null && nodeRef.Pos.Equals(pos)));
					}
					if (Enumerable.Any<WireNodeRef>(persistedNodes, func))
					{
						return network.networkID;
					}
				}
			}
			return 0L;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D360 File Offset: 0x0000B560
		public static HashSet<BEWireNode> GetLoadedNodesAtPositions(IEnumerable<BlockPos> positions, IBlockAccessor accessor)
		{
			HashSet<BEWireNode> result = new HashSet<BEWireNode>();
			if (positions == null || accessor == null)
			{
				return result;
			}
			foreach (BlockPos pos in positions)
			{
				if (!(pos == null))
				{
					BEWireNode node = accessor.GetBlockEntity(pos) as BEWireNode;
					if (node != null)
					{
						result.Add(node);
					}
				}
			}
			return result;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D3D4 File Offset: 0x0000B5D4
		public static bool HasNetworkRootInPositions(IEnumerable<BlockPos> positions, IBlockAccessor accessor, IEnumerable<WireNodeRef> persistedNodes)
		{
			return WireNetworkHandler.CountNetworkRootsInPositions(positions, accessor, persistedNodes) > 0;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		public static int CountNetworkRootsInPositions(IEnumerable<BlockPos> positions, IBlockAccessor accessor, IEnumerable<WireNodeRef> persistedNodes)
		{
			if (positions == null)
			{
				return 0;
			}
			int count = 0;
			using (IEnumerator<BlockPos> enumerator = positions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BlockPos pos = enumerator.Current;
					if (!(pos == null))
					{
						if (((accessor != null) ? accessor.GetBlockEntity(pos) : null) is INetworkRoot)
						{
							count++;
						}
						else
						{
							WireNodeKind? wireNodeKind;
							if (persistedNodes == null)
							{
								wireNodeKind = default(WireNodeKind?);
							}
							else
							{
								WireNodeRef wireNodeRef = Enumerable.FirstOrDefault<WireNodeRef>(persistedNodes, (WireNodeRef nodeRef) => nodeRef.Pos != null && nodeRef.Pos.Equals(pos));
								wireNodeKind = ((wireNodeRef != null) ? new WireNodeKind?(wireNodeRef.Kind) : default(WireNodeKind?));
							}
							WireNodeKind? kind = wireNodeKind;
							if (kind.GetValueOrDefault() == WireNodeKind.Telegraph || kind.GetValueOrDefault() == WireNodeKind.Telephone)
							{
								count++;
							}
						}
					}
				}
			}
			return count;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public static void DetachPersistedPositions(WireNetwork network, IEnumerable<BlockPos> positions)
		{
			if (network == null || positions == null)
			{
				return;
			}
			foreach (BlockPos pos in positions)
			{
				network.RemovePersistedNode(pos);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D510 File Offset: 0x0000B710
		public static void RebuildNetworkState(long networkId)
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null)
			{
				return;
			}
			network.RebuildTypedState();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000D524 File Offset: 0x0000B724
		public static void RefreshTelegraphRoutingSnapshot(long networkId)
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null || network.Nodes.Count == 0)
			{
				return;
			}
			ICoreAPI api = network.Nodes[0].Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			Dictionary<BEWireNode, BlockEntitySwitchboard> activeEndpointOwners = WireNetworkHandler.ResolveOwnersForActiveEndpoints(network.Nodes);
			WireNetworkKind kindForPower = (network.CurrentType == WireNetworkKind.None) ? WireNetworkKind.Telegraph : network.CurrentType;
			foreach (BEWireNode node in network.Nodes.ToArray())
			{
				BlockEntityTelegraph telegraph = node as BlockEntityTelegraph;
				if (telegraph != null)
				{
					BlockEntitySwitchboard owner;
					bool managed = activeEndpointOwners.TryGetValue(telegraph, ref owner) && owner != null;
					bool overCapacity = WireNetworkHandler.IsOverCapacityForManagedComponent(network, kindForPower);
					bool advanced = managed && !overCapacity && owner.HasSufficientPowerFor(kindForPower);
					string disabledReason = overCapacity ? "Telegraph.Settings.DisabledCapacity" : "Telegraph.Settings.DisabledNoPower";
					telegraph.ApplyServerRoutingFlags(managed, advanced, disabledReason);
				}
				else
				{
					BlockEntityTelephone telephone = node as BlockEntityTelephone;
					if (telephone != null)
					{
						BlockEntitySwitchboard owner2;
						bool managed2 = activeEndpointOwners.TryGetValue(telephone, ref owner2) && owner2 != null;
						bool overCapacity2 = WireNetworkHandler.IsOverCapacityForManagedComponent(network, WireNetworkKind.Telephone);
						bool composeEnabled = managed2 && !overCapacity2 && owner2.HasSufficientPowerFor(WireNetworkKind.Telephone);
						string disabledReason2 = overCapacity2 ? "Telegraph.Settings.DisabledCapacity" : "Telegraph.Settings.DisabledNoPower";
						telephone.ApplyServerComposeFlags(managed2, composeEnabled, disabledReason2);
					}
				}
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000D67C File Offset: 0x0000B87C
		private static Dictionary<BEWireNode, BlockEntitySwitchboard> ResolveOwnersForActiveEndpoints(IEnumerable<BEWireNode> nodes)
		{
			object obj;
			if (nodes == null)
			{
				obj = null;
			}
			else
			{
				obj = Enumerable.ToList<BEWireNode>(Enumerable.Where<BEWireNode>(nodes, (BEWireNode n) => n != null));
			}
			object obj2 = obj ?? new List<BEWireNode>();
			Dictionary<BEWireNode, BlockEntitySwitchboard> owners = new Dictionary<BEWireNode, BlockEntitySwitchboard>();
			object obj3 = obj2;
			List<BlockEntitySwitchboard> switchboards = Enumerable.ToList<BlockEntitySwitchboard>(Enumerable.OfType<BlockEntitySwitchboard>(obj3));
			Func<BEWireNode, bool> func;
			if ((func = WireNetworkHandler.<>O.<2>__IsActiveEndpoint) == null)
			{
				func = (WireNetworkHandler.<>O.<2>__IsActiveEndpoint = new Func<BEWireNode, bool>(WireNetworkHandler.IsActiveEndpoint));
			}
			List<BEWireNode> activeEndpoints = Enumerable.ToList<BEWireNode>(Enumerable.Where<BEWireNode>(obj3, func));
			if (switchboards.Count == 0)
			{
				foreach (BEWireNode endpoint in activeEndpoints)
				{
					owners[endpoint] = null;
				}
				return owners;
			}
			foreach (BEWireNode endpoint2 in activeEndpoints)
			{
				owners[endpoint2] = WireNetworkHandler.FindNearestSwitchboardOwner(endpoint2, switchboards);
			}
			return owners;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000D790 File Offset: 0x0000B990
		private static BlockEntitySwitchboard FindNearestSwitchboardOwner(BEWireNode startNode, List<BlockEntitySwitchboard> allSwitchboards)
		{
			if (((startNode != null) ? startNode.Pos : null) == null)
			{
				return null;
			}
			ICoreAPI api = startNode.Api;
			IBlockAccessor blockAccessor;
			if (api == null)
			{
				blockAccessor = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				blockAccessor = ((world != null) ? world.BlockAccessor : null);
			}
			IBlockAccessor accessor = blockAccessor;
			HashSet<BlockPos> visited = new HashSet<BlockPos>();
			Queue<BlockPos> queue = new Queue<BlockPos>();
			queue.Enqueue(startNode.Pos.Copy());
			visited.Add(startNode.Pos.Copy());
			while (queue.Count > 0)
			{
				int levelCount = queue.Count;
				List<BlockEntitySwitchboard> levelCandidates = new List<BlockEntitySwitchboard>();
				for (int i = 0; i < levelCount; i++)
				{
					BlockPos currentPos = queue.Dequeue();
					BlockEntitySwitchboard switchboard = ((accessor != null) ? accessor.GetBlockEntity(currentPos) : null) as BlockEntitySwitchboard;
					if (switchboard != null)
					{
						levelCandidates.Add(switchboard);
					}
					foreach (BlockPos neighborPos in WireTopologyRegistry.GetNeighborPositions(currentPos))
					{
						if (!(neighborPos == null))
						{
							BlockPos neighborCopy = neighborPos.Copy();
							if (visited.Add(neighborCopy))
							{
								queue.Enqueue(neighborCopy);
							}
						}
					}
				}
				if (levelCandidates.Count > 0)
				{
					return Enumerable.First<BlockEntitySwitchboard>(Enumerable.ThenBy<BlockEntitySwitchboard, int>(Enumerable.ThenBy<BlockEntitySwitchboard, int>(Enumerable.OrderBy<BlockEntitySwitchboard, int>(levelCandidates, (BlockEntitySwitchboard sb) => sb.Pos.X), (BlockEntitySwitchboard sb) => sb.Pos.Y), (BlockEntitySwitchboard sb) => sb.Pos.Z));
				}
			}
			return Enumerable.FirstOrDefault<BlockEntitySwitchboard>(Enumerable.ThenBy<BlockEntitySwitchboard, int>(Enumerable.ThenBy<BlockEntitySwitchboard, int>(Enumerable.OrderBy<BlockEntitySwitchboard, int>(allSwitchboards, (BlockEntitySwitchboard sb) => sb.Pos.X), (BlockEntitySwitchboard sb) => sb.Pos.Y), (BlockEntitySwitchboard sb) => sb.Pos.Z));
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		private static bool IsActiveEndpoint(BEWireNode node)
		{
			return node != null && node.IsActiveEndpoint;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
		public static BlockEntitySwitchboard ResolveOwnerSwitchboard(BEWireNode endpointNode)
		{
			if (endpointNode == null || !WireNetworkHandler.IsActiveEndpoint(endpointNode) || endpointNode.NetworkUID == 0L)
			{
				return null;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(endpointNode.NetworkUID);
			if (network == null)
			{
				return null;
			}
			BlockEntitySwitchboard owner;
			WireNetworkHandler.ResolveOwnersForActiveEndpoints(network.Nodes).TryGetValue(endpointNode, ref owner);
			return owner;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000DA10 File Offset: 0x0000BC10
		public static string GetSubNetworkDisplayName(BEWireNode endpointNode)
		{
			BlockEntitySwitchboard blockEntitySwitchboard = WireNetworkHandler.ResolveOwnerSwitchboard(endpointNode);
			return ((blockEntitySwitchboard != null) ? blockEntitySwitchboard.GetNetworkCustomNameForEditor() : null) ?? "";
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000DA30 File Offset: 0x0000BC30
		public static string GetManagedRoutingDisabledReason(long networkId)
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null || !network.IsManagedBySwitchboard)
			{
				return null;
			}
			WireNetworkKind kind = (network.CurrentType == WireNetworkKind.None) ? WireNetworkKind.Telegraph : network.CurrentType;
			if (WireNetworkHandler.IsOverCapacityForManagedComponent(network, kind))
			{
				return "Telegraph.Settings.DisabledCapacity";
			}
			if (!network.HasPoweredSwitchboard)
			{
				return "Telegraph.Settings.DisabledNoPower";
			}
			return null;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000DA84 File Offset: 0x0000BC84
		public static bool CanConnectNodes(BEWireNode node1, BEWireNode node2, out string denialLangKey, out object[] denialArgs)
		{
			denialLangKey = null;
			denialArgs = Array.Empty<object>();
			if (node1 == null || node2 == null)
			{
				return false;
			}
			HashSet<BEWireNode> prospectiveComponent = WireNetworkHandler.GetProspectiveComponent(node1, node2);
			bool hasSwitchboard = Enumerable.Any<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.Switchboard);
			int telegraphCount = Enumerable.Count<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.Telegraph);
			int telephoneCount = Enumerable.Count<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.Telephone);
			int radioCount = Enumerable.Count<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNodeKindRules.IsRadioFamilyEndpoint(WireNetworkHandler.GetNodeKind(n)));
			int radioReceiverCount = Enumerable.Count<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.RadioReceiver);
			int radioConsoleCount = Enumerable.Count<BlockEntityRadioSupervisionConsole>(Enumerable.OfType<BlockEntityRadioSupervisionConsole>(prospectiveComponent));
			int speakerCount = Enumerable.Count<BlockEntitySpeaker>(Enumerable.OfType<BlockEntitySpeaker>(prospectiveComponent));
			int telephoneHandsetCount = Enumerable.Count<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(prospectiveComponent));
			int activeKinds = 0;
			if (telegraphCount > 0)
			{
				activeKinds++;
			}
			if (telephoneCount > 0)
			{
				activeKinds++;
			}
			if (radioCount > 0)
			{
				activeKinds++;
			}
			if (radioReceiverCount > 0)
			{
				activeKinds++;
			}
			if (radioReceiverCount > 0 && (telegraphCount > 0 || telephoneCount > 0 || radioCount > 0))
			{
				denialLangKey = "Wire.ConnectionDenied.RadioReceiverInvalidEndpoint";
				return false;
			}
			if (activeKinds > 1)
			{
				denialLangKey = "Wire.ConnectionDenied.MixedTypes";
				return false;
			}
			BlockEntityRadioEmitter repeater = node1 as BlockEntityRadioEmitter;
			if (repeater == null || !repeater.IsRepeaterMode)
			{
				BlockEntityRadioEmitter repeater2 = node2 as BlockEntityRadioEmitter;
				if (repeater2 == null || !repeater2.IsRepeaterMode)
				{
					if (radioConsoleCount > 1)
					{
						denialLangKey = "Wire.ConnectionDenied.RadioSingleConsole";
						return false;
					}
					if (!hasSwitchboard)
					{
						if (radioReceiverCount > 0)
						{
							if (radioReceiverCount > 1)
							{
								denialLangKey = "Wire.ConnectionDenied.RadioReceiverSingle";
								return false;
							}
							if (speakerCount > ServerConfigManager.RadioReceiverMaxWiredSpeakers)
							{
								denialLangKey = "Wire.ConnectionDenied.RadioReceiverSpeakerCapacity";
								denialArgs = new object[]
								{
									ServerConfigManager.RadioReceiverMaxWiredSpeakers
								};
								return false;
							}
							return true;
						}
						else
						{
							if (telephoneHandsetCount > 0 && speakerCount > 0 && speakerCount > ServerConfigManager.TelephoneBroadcastMaxSpeakers)
							{
								denialLangKey = "Wire.ConnectionDenied.TelephoneBroadcastSpeakerCapacity";
								denialArgs = new object[]
								{
									ServerConfigManager.TelephoneBroadcastMaxSpeakers
								};
								return false;
							}
							if (speakerCount > 0 && telephoneHandsetCount > 1)
							{
								denialLangKey = "Wire.ConnectionDenied.SpeakerNetworkSingleTelephone";
								return false;
							}
							if (speakerCount == 0 && telephoneCount > 2)
							{
								denialLangKey = "Wire.ConnectionDenied.NetworkCapacity";
								denialArgs = new object[]
								{
									WireNetworkHandler.GetKindDisplayName(WireNetworkKind.Telephone),
									2
								};
								return false;
							}
							if (radioCount > 0 && radioCount > ServerConfigManager.RadioNetworkMaxEndpoints)
							{
								denialLangKey = "Wire.ConnectionDenied.NetworkCapacity";
								denialArgs = new object[]
								{
									WireNetworkHandler.GetKindDisplayName(WireNetworkKind.Radio),
									ServerConfigManager.RadioNetworkMaxEndpoints
								};
								return false;
							}
							return true;
						}
					}
					else
					{
						if (speakerCount > 0 || radioReceiverCount > 0)
						{
							denialLangKey = ((radioReceiverCount > 0) ? "Wire.ConnectionDenied.RadioReceiverInvalidEndpoint" : "Wire.ConnectionDenied.SpeakerWithSwitchboard");
							return false;
						}
						WireNetworkKind targetKind = WireNetworkHandler.ResolveProspectiveKind(telegraphCount, telephoneCount, radioCount);
						WireNetworkRequirements requirements = WireNetworkTypeRules.GetRequirements(targetKind);
						if (requirements.MaxEndpoints > 0)
						{
							int switchboardCount = Enumerable.Count<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.Switchboard);
							int endpointCountByKind = WireNetworkHandler.GetEndpointCountByKind(targetKind, telegraphCount, telephoneCount, radioCount);
							int componentCapacity = requirements.MaxEndpoints * Math.Max(1, switchboardCount);
							if (endpointCountByKind > componentCapacity)
							{
								denialLangKey = ((targetKind == WireNetworkKind.Telegraph) ? "Wire.ConnectionDenied.TelegraphCapacity" : "Wire.ConnectionDenied.NetworkCapacity");
								object[] array2;
								if (targetKind != WireNetworkKind.Telegraph)
								{
									object[] array = new object[2];
									array[0] = WireNetworkHandler.GetKindDisplayName(targetKind);
									array2 = array;
									array[1] = componentCapacity;
								}
								else
								{
									(array2 = new object[1])[0] = componentCapacity;
								}
								denialArgs = array2;
								return false;
							}
						}
						return true;
					}
				}
			}
			denialLangKey = "Wire.ConnectionDenied.RadioRepeaterNoWire";
			return false;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
		private static bool IsOverCapacityForManagedComponent(WireNetwork network, WireNetworkKind kind)
		{
			if (network == null || !network.IsManagedBySwitchboard || kind == WireNetworkKind.None)
			{
				return false;
			}
			WireNetworkRequirements requirements = WireNetworkTypeRules.GetRequirements(kind);
			if (requirements.MaxEndpoints <= 0)
			{
				return false;
			}
			int switchboardCount = Enumerable.Count<BEWireNode>(network.Nodes, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.Switchboard);
			if (switchboardCount <= 0)
			{
				return false;
			}
			int num;
			switch (kind)
			{
			case WireNetworkKind.Telegraph:
				num = network.TelegraphEndpointCount;
				break;
			case WireNetworkKind.Telephone:
				num = network.TelephoneEndpointCount;
				break;
			case WireNetworkKind.Radio:
				num = network.RadioEndpointCount;
				break;
			default:
				num = 0;
				break;
			}
			int num2 = num;
			int capacity = requirements.MaxEndpoints * switchboardCount;
			return num2 > capacity;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000DE84 File Offset: 0x0000C084
		private static bool IsEndpointOfKind(BEWireNode endpoint, WireNetworkKind kind)
		{
			switch (kind)
			{
			case WireNetworkKind.Telegraph:
				return WireNetworkHandler.GetNodeKind(endpoint) == WireNodeKind.Telegraph;
			case WireNetworkKind.Telephone:
				return WireNetworkHandler.GetNodeKind(endpoint) == WireNodeKind.Telephone;
			case WireNetworkKind.Radio:
				return WireNodeKindRules.IsRadioFamilyEndpoint(WireNetworkHandler.GetNodeKind(endpoint));
			default:
				return false;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000DEBD File Offset: 0x0000C0BD
		private static WireNetworkKind ResolveProspectiveKind(int telegraphCount, int telephoneCount, int radioCount)
		{
			if (telegraphCount > 0)
			{
				return WireNetworkKind.Telegraph;
			}
			if (telephoneCount > 0)
			{
				return WireNetworkKind.Telephone;
			}
			if (radioCount > 0)
			{
				return WireNetworkKind.Radio;
			}
			return WireNetworkKind.None;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000DED2 File Offset: 0x0000C0D2
		private static int GetEndpointCountByKind(WireNetworkKind kind, int telegraphCount, int telephoneCount, int radioCount)
		{
			switch (kind)
			{
			case WireNetworkKind.Telegraph:
				return telegraphCount;
			case WireNetworkKind.Telephone:
				return telephoneCount;
			case WireNetworkKind.Radio:
				return radioCount;
			default:
				return 0;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000DEF1 File Offset: 0x0000C0F1
		private static string GetKindDisplayName(WireNetworkKind kind)
		{
			switch (kind)
			{
			case WireNetworkKind.Telegraph:
				return "telegraph";
			case WireNetworkKind.Telephone:
				return "telephone";
			case WireNetworkKind.Radio:
				return "radio";
			default:
				return "network";
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000DF20 File Offset: 0x0000C120
		public static BlockEntityTelegraph ResolveTelegraphByName(long networkUID, string endpointName)
		{
			if (networkUID == 0L || string.IsNullOrWhiteSpace(endpointName))
			{
				return null;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(networkUID);
			if (network == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<BlockEntityTelegraph>(Enumerable.OfType<BlockEntityTelegraph>(network.Nodes), (BlockEntityTelegraph t) => string.Equals(t.CustomEndpointName, endpointName, 5));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000DF74 File Offset: 0x0000C174
		public static bool IsNetworkNameTaken(long exceptNetworkId, string candidate)
		{
			if (string.IsNullOrWhiteSpace(candidate))
			{
				return false;
			}
			string normalized = candidate.Trim();
			return Enumerable.Any<WireNetwork>(WireNetworkHandler.Networks.Values, (WireNetwork network) => network != null && network.networkID != exceptNetworkId && !string.IsNullOrWhiteSpace(network.CustomName) && string.Equals(network.CustomName, normalized, 5));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public static bool TryRenameNetwork(long networkId, string candidate, out string failureLangKey)
		{
			failureLangKey = null;
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null)
			{
				failureLangKey = "Network.NoNetwork";
				return false;
			}
			string normalized = (candidate ?? "").Trim();
			if (normalized.Length == 0)
			{
				network.SetCustomName("");
				return true;
			}
			if (WireNetworkHandler.IsNetworkNameTaken(networkId, normalized))
			{
				failureLangKey = "Switchboard.Settings.NameAlreadyUsed";
				return false;
			}
			network.SetCustomName(normalized);
			return true;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000E024 File Offset: 0x0000C224
		public static string GetDisplayName(long networkId)
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null)
			{
				return networkId.ToString();
			}
			if (!string.IsNullOrWhiteSpace(network.CustomName))
			{
				return network.CustomName;
			}
			return network.networkID.ToString();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000E062 File Offset: 0x0000C262
		public static void SetPersistedNetworkName(long networkId, string customName)
		{
			if (networkId == 0L)
			{
				return;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null)
			{
				return;
			}
			network.SetCustomName((customName ?? "").Trim());
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000E087 File Offset: 0x0000C287
		public static string GetPersistedNetworkName(long networkId)
		{
			if (networkId == 0L)
			{
				return "";
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			return ((network != null) ? network.CustomName : null) ?? "";
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
		public static bool IsEndpointNameTaken(long networkUID, string candidate, BlockEntityTelegraph except = null)
		{
			if (networkUID == 0L || string.IsNullOrWhiteSpace(candidate))
			{
				return false;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(networkUID);
			return network != null && Enumerable.Any<BlockEntityTelegraph>(Enumerable.OfType<BlockEntityTelegraph>(network.Nodes), (BlockEntityTelegraph t) => t != except && string.Equals(t.CustomEndpointName, candidate, 5));
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000E10C File Offset: 0x0000C30C
		private static HashSet<BEWireNode> GetProspectiveComponent(BEWireNode node1, BEWireNode node2)
		{
			HashSet<BEWireNode> result = new HashSet<BEWireNode>();
			WireNetworkHandler.AddReachable(node1, result);
			WireNetworkHandler.AddReachable(node2, result);
			return result;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000E130 File Offset: 0x0000C330
		public static HashSet<BEWireNode> GetReachableNodes(BEWireNode startNode)
		{
			HashSet<BEWireNode> result = new HashSet<BEWireNode>();
			WireNetworkHandler.AddReachable(startNode, result);
			return result;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000E14C File Offset: 0x0000C34C
		private static void AddReachable(BEWireNode startNode, HashSet<BEWireNode> output)
		{
			if (((startNode != null) ? startNode.Pos : null) == null || output.Contains(startNode))
			{
				return;
			}
			ICoreAPI api = startNode.Api;
			IBlockAccessor blockAccessor;
			if (api == null)
			{
				blockAccessor = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				blockAccessor = ((world != null) ? world.BlockAccessor : null);
			}
			IBlockAccessor accessor = blockAccessor;
			HashSet<BlockPos> visited = new HashSet<BlockPos>();
			Queue<BlockPos> queue = new Queue<BlockPos>();
			BlockPos startPos = startNode.Pos.Copy();
			queue.Enqueue(startPos);
			visited.Add(startPos);
			output.Add(startNode);
			while (queue.Count > 0)
			{
				foreach (BlockPos neighborPos in WireTopologyRegistry.GetNeighborPositions(queue.Dequeue()))
				{
					if (!(neighborPos == null))
					{
						BlockPos neighborCopy = neighborPos.Copy();
						if (visited.Add(neighborCopy))
						{
							queue.Enqueue(neighborCopy);
							BEWireNode neighborNode = ((accessor != null) ? accessor.GetBlockEntity(neighborCopy) : null) as BEWireNode;
							if (neighborNode != null)
							{
								output.Add(neighborNode);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000E260 File Offset: 0x0000C460
		private static WireNodeKind GetNodeKind(BEWireNode node)
		{
			IWireTypedNode typedNode = node as IWireTypedNode;
			if (typedNode != null)
			{
				return typedNode.WireNodeKind;
			}
			return WireNodeKind.Infrastructure;
		}

		// Token: 0x040000CF RID: 207
		private static IClientNetworkChannel ClientChannel;

		// Token: 0x040000D0 RID: 208
		private static IServerNetworkChannel ServerChannel;

		// Token: 0x040000D1 RID: 209
		public static Dictionary<long, WireNetwork> Networks = new Dictionary<long, WireNetwork>();

		// Token: 0x040000D2 RID: 210
		private static long nextNetworkId;

		// Token: 0x040000D3 RID: 211
		public static EventHandler<WireNetworkMessage> ClientSideMessageReceived;

		// Token: 0x040000D4 RID: 212
		public static string NetworkChannel = "rpvc:wire-network";

		// Token: 0x02000138 RID: 312
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000521 RID: 1313
			public static NetworkClientMessageHandler<WireNetworkMessage> <0>__OnReceivedMessage_Server;

			// Token: 0x04000522 RID: 1314
			public static NetworkServerMessageHandler<WireNetworkMessage> <1>__OnReceivedMessage_Client;

			// Token: 0x04000523 RID: 1315
			public static Func<BEWireNode, bool> <2>__IsActiveEndpoint;
		}
	}
}
