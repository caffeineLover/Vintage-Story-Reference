using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000038 RID: 56
	public static class WireNetworkHandler
	{
		// Token: 0x06000159 RID: 345 RVA: 0x000094F4 File Offset: 0x000076F4
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

		// Token: 0x0600015A RID: 346 RVA: 0x00009548 File Offset: 0x00007748
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

		// Token: 0x0600015B RID: 347 RVA: 0x00009599 File Offset: 0x00007799
		private static void OnReceivedMessage_Client(WireNetworkMessage packet)
		{
			EventHandler<WireNetworkMessage> clientSideMessageReceived = WireNetworkHandler.ClientSideMessageReceived;
			if (clientSideMessageReceived == null)
			{
				return;
			}
			clientSideMessageReceived.Invoke(null, packet);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000095AC File Offset: 0x000077AC
		private static void OnReceivedMessage_Server(IServerPlayer fromPlayer, WireNetworkMessage packet)
		{
			packet = WireNetworkHandler.ApplyRoutingOnServer(packet);
			WireNetworkHandler.ServerChannel.BroadcastPacket<WireNetworkMessage>(packet, Array.Empty<IServerPlayer>());
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000095C8 File Offset: 0x000077C8
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

		// Token: 0x0600015E RID: 350 RVA: 0x00009648 File Offset: 0x00007848
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

		// Token: 0x0600015F RID: 351 RVA: 0x00009684 File Offset: 0x00007884
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

		// Token: 0x06000160 RID: 352 RVA: 0x000096DC File Offset: 0x000078DC
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

		// Token: 0x06000161 RID: 353 RVA: 0x00009798 File Offset: 0x00007998
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

		// Token: 0x06000162 RID: 354 RVA: 0x000098C4 File Offset: 0x00007AC4
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

		// Token: 0x06000163 RID: 355 RVA: 0x00009953 File Offset: 0x00007B53
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

		// Token: 0x06000164 RID: 356 RVA: 0x00009991 File Offset: 0x00007B91
		public static void RemoveNetwork(WireNetwork network)
		{
			if (network == null)
			{
				return;
			}
			WireNetworkHandler.Networks.Remove(network.networkID);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000099A8 File Offset: 0x00007BA8
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

		// Token: 0x06000166 RID: 358 RVA: 0x000099CC File Offset: 0x00007BCC
		public static WireNetwork GetNetwork(BEWireNode node)
		{
			if (node == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<WireNetwork>(WireNetworkHandler.Networks.Values, (WireNetwork nw) => nw.Nodes.Contains(node));
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009A0C File Offset: 0x00007C0C
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

		// Token: 0x06000168 RID: 360 RVA: 0x00009AE0 File Offset: 0x00007CE0
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

		// Token: 0x06000169 RID: 361 RVA: 0x00009B98 File Offset: 0x00007D98
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

		// Token: 0x0600016A RID: 362 RVA: 0x00009C0C File Offset: 0x00007E0C
		public static bool HasNetworkRootInPositions(IEnumerable<BlockPos> positions, IBlockAccessor accessor, IEnumerable<WireNodeRef> persistedNodes)
		{
			return WireNetworkHandler.CountNetworkRootsInPositions(positions, accessor, persistedNodes) > 0;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009C1C File Offset: 0x00007E1C
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

		// Token: 0x0600016C RID: 364 RVA: 0x00009CF8 File Offset: 0x00007EF8
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

		// Token: 0x0600016D RID: 365 RVA: 0x00009D48 File Offset: 0x00007F48
		public static void RebuildNetworkState(long networkId)
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			if (network == null)
			{
				return;
			}
			network.RebuildTypedState();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00009D5C File Offset: 0x00007F5C
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

		// Token: 0x0600016F RID: 367 RVA: 0x00009EB4 File Offset: 0x000080B4
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

		// Token: 0x06000170 RID: 368 RVA: 0x00009FC8 File Offset: 0x000081C8
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

		// Token: 0x06000171 RID: 369 RVA: 0x0000A1F0 File Offset: 0x000083F0
		private static bool IsActiveEndpoint(BEWireNode node)
		{
			return node != null && node.IsActiveEndpoint;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A200 File Offset: 0x00008400
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

		// Token: 0x06000173 RID: 371 RVA: 0x0000A248 File Offset: 0x00008448
		public static string GetSubNetworkDisplayName(BEWireNode endpointNode)
		{
			BlockEntitySwitchboard blockEntitySwitchboard = WireNetworkHandler.ResolveOwnerSwitchboard(endpointNode);
			return ((blockEntitySwitchboard != null) ? blockEntitySwitchboard.GetNetworkCustomNameForEditor() : null) ?? "";
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A268 File Offset: 0x00008468
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

		// Token: 0x06000175 RID: 373 RVA: 0x0000A2BC File Offset: 0x000084BC
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
			int radioCount = Enumerable.Count<BEWireNode>(prospectiveComponent, (BEWireNode n) => WireNetworkHandler.GetNodeKind(n) == WireNodeKind.Radio);
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
			if (activeKinds > 1)
			{
				denialLangKey = "Wire.ConnectionDenied.MixedTypes";
				return false;
			}
			if (!hasSwitchboard)
			{
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
				if (radioCount > 1)
				{
					denialLangKey = "Wire.ConnectionDenied.NetworkCapacity";
					denialArgs = new object[]
					{
						WireNetworkHandler.GetKindDisplayName(WireNetworkKind.Radio),
						1
					};
					return false;
				}
				return true;
			}
			else
			{
				if (speakerCount > 0)
				{
					denialLangKey = "Wire.ConnectionDenied.SpeakerWithSwitchboard";
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

		// Token: 0x06000176 RID: 374 RVA: 0x0000A4F4 File Offset: 0x000086F4
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

		// Token: 0x06000177 RID: 375 RVA: 0x0000A594 File Offset: 0x00008794
		private static bool IsEndpointOfKind(BEWireNode endpoint, WireNetworkKind kind)
		{
			switch (kind)
			{
			case WireNetworkKind.Telegraph:
				return WireNetworkHandler.GetNodeKind(endpoint) == WireNodeKind.Telegraph;
			case WireNetworkKind.Telephone:
				return WireNetworkHandler.GetNodeKind(endpoint) == WireNodeKind.Telephone;
			case WireNetworkKind.Radio:
				return WireNetworkHandler.GetNodeKind(endpoint) == WireNodeKind.Radio;
			default:
				return false;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A5CB File Offset: 0x000087CB
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

		// Token: 0x06000179 RID: 377 RVA: 0x0000A5E0 File Offset: 0x000087E0
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

		// Token: 0x0600017A RID: 378 RVA: 0x0000A5FF File Offset: 0x000087FF
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

		// Token: 0x0600017B RID: 379 RVA: 0x0000A630 File Offset: 0x00008830
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

		// Token: 0x0600017C RID: 380 RVA: 0x0000A684 File Offset: 0x00008884
		public static bool IsNetworkNameTaken(long exceptNetworkId, string candidate)
		{
			if (string.IsNullOrWhiteSpace(candidate))
			{
				return false;
			}
			string normalized = candidate.Trim();
			return Enumerable.Any<WireNetwork>(WireNetworkHandler.Networks.Values, (WireNetwork network) => network != null && network.networkID != exceptNetworkId && !string.IsNullOrWhiteSpace(network.CustomName) && string.Equals(network.CustomName, normalized, 5));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A6D0 File Offset: 0x000088D0
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

		// Token: 0x0600017E RID: 382 RVA: 0x0000A734 File Offset: 0x00008934
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

		// Token: 0x0600017F RID: 383 RVA: 0x0000A772 File Offset: 0x00008972
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

		// Token: 0x06000180 RID: 384 RVA: 0x0000A797 File Offset: 0x00008997
		public static string GetPersistedNetworkName(long networkId)
		{
			if (networkId == 0L)
			{
				return "";
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(networkId);
			return ((network != null) ? network.CustomName : null) ?? "";
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public static bool IsEndpointNameTaken(long networkUID, string candidate, BlockEntityTelegraph except = null)
		{
			if (networkUID == 0L || string.IsNullOrWhiteSpace(candidate))
			{
				return false;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(networkUID);
			return network != null && Enumerable.Any<BlockEntityTelegraph>(Enumerable.OfType<BlockEntityTelegraph>(network.Nodes), (BlockEntityTelegraph t) => t != except && string.Equals(t.CustomEndpointName, candidate, 5));
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A81C File Offset: 0x00008A1C
		private static HashSet<BEWireNode> GetProspectiveComponent(BEWireNode node1, BEWireNode node2)
		{
			HashSet<BEWireNode> result = new HashSet<BEWireNode>();
			WireNetworkHandler.AddReachable(node1, result);
			WireNetworkHandler.AddReachable(node2, result);
			return result;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000A840 File Offset: 0x00008A40
		public static HashSet<BEWireNode> GetReachableNodes(BEWireNode startNode)
		{
			HashSet<BEWireNode> result = new HashSet<BEWireNode>();
			WireNetworkHandler.AddReachable(startNode, result);
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000A85C File Offset: 0x00008A5C
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

		// Token: 0x06000185 RID: 389 RVA: 0x0000A970 File Offset: 0x00008B70
		private static WireNodeKind GetNodeKind(BEWireNode node)
		{
			IWireTypedNode typedNode = node as IWireTypedNode;
			if (typedNode != null)
			{
				return typedNode.WireNodeKind;
			}
			return WireNodeKind.Infrastructure;
		}

		// Token: 0x04000084 RID: 132
		private static IClientNetworkChannel ClientChannel;

		// Token: 0x04000085 RID: 133
		private static IServerNetworkChannel ServerChannel;

		// Token: 0x04000086 RID: 134
		public static Dictionary<long, WireNetwork> Networks = new Dictionary<long, WireNetwork>();

		// Token: 0x04000087 RID: 135
		private static long nextNetworkId;

		// Token: 0x04000088 RID: 136
		public static EventHandler<WireNetworkMessage> ClientSideMessageReceived;

		// Token: 0x04000089 RID: 137
		public static string NetworkChannel = "rpvc:wire-network";

		// Token: 0x020000E8 RID: 232
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040003C8 RID: 968
			public static NetworkClientMessageHandler<WireNetworkMessage> <0>__OnReceivedMessage_Server;

			// Token: 0x040003C9 RID: 969
			public static NetworkServerMessageHandler<WireNetworkMessage> <1>__OnReceivedMessage_Client;

			// Token: 0x040003CA RID: 970
			public static Func<BEWireNode, bool> <2>__IsActiveEndpoint;
		}
	}
}
