using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B9 RID: 185
	public class BEWireNode : BlockEntity, IWireConnectable
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00022BFE File Offset: 0x00020DFE
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00022C06 File Offset: 0x00020E06
		public long NetworkUID { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00022C0F File Offset: 0x00020E0F
		public BlockPos Position
		{
			get
			{
				return this.Pos;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00022C17 File Offset: 0x00020E17
		public string NodeUID
		{
			get
			{
				return this.Pos.ToString();
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00022C24 File Offset: 0x00020E24
		protected virtual int MaxConnections
		{
			get
			{
				return ServerConfigManager.TelegraphMaxConnectionsPerNode;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00022C2B File Offset: 0x00020E2B
		public virtual bool IsActiveEndpoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00022C2E File Offset: 0x00020E2E
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00022C36 File Offset: 0x00020E36
		protected EventHandler<WireNetworkMessage> OnReceivedSignalEvent { get; set; }

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000657 RID: 1623 RVA: 0x00022C40 File Offset: 0x00020E40
		// (remove) Token: 0x06000658 RID: 1624 RVA: 0x00022C78 File Offset: 0x00020E78
		public event Action OnConnectionsChanged;

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00022CAD File Offset: 0x00020EAD
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00022CB5 File Offset: 0x00020EB5
		public Vec3f WireAttachmentOffset { get; protected set; } = new Vec3f(0.5f, 0.5f, 0.5f);

		// Token: 0x0600065B RID: 1627 RVA: 0x00022CC0 File Offset: 0x00020EC0
		protected virtual void SetWireAttachmentOffset()
		{
			Vec3f configuredOffset;
			if (this.TryGetWireAttachmentOffsetFromAttributes(out configuredOffset))
			{
				this.WireAttachmentOffset = configuredOffset;
				return;
			}
			this.WireAttachmentOffset = new Vec3f(0.5f, 0.45f, 0.5f);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00022CFC File Offset: 0x00020EFC
		protected virtual bool TryGetWireAttachmentOffsetFromAttributes(out Vec3f offset)
		{
			offset = null;
			Block block = base.Block;
			JsonObject attrs = (block != null) ? block.Attributes : null;
			if (attrs == null)
			{
				return false;
			}
			JsonObject jsonObject = attrs["wireAttachmentOffset"];
			float[] single = (jsonObject != null) ? jsonObject.AsArray<float>(null, "game") : null;
			if (single != null && single.Length == 3)
			{
				offset = new Vec3f(single[0], single[1], single[2]);
				return true;
			}
			JsonObject map = attrs["wireAttachmentOffsets"];
			if (map == null)
			{
				return false;
			}
			string wireOrientationCode = this.GetWireOrientationCode();
			string side = this.GetWireSideCode();
			foreach (string key in BEWireNode.GetWireOffsetCandidates(wireOrientationCode, side))
			{
				JsonObject jsonObject2 = map[key];
				float[] values = (jsonObject2 != null) ? jsonObject2.AsArray<float>(null, "game") : null;
				if (values != null && values.Length == 3)
				{
					offset = new Vec3f(values[0], values[1], values[2]);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00022DDC File Offset: 0x00020FDC
		protected virtual string GetWireOrientationCode()
		{
			Block block = base.Block;
			if (((block != null) ? block.Variant : null) == null)
			{
				return "up";
			}
			string orientation;
			if (base.Block.Variant.TryGetValue("orientation", ref orientation) && !string.IsNullOrWhiteSpace(orientation))
			{
				return orientation;
			}
			string side;
			if (base.Block.Variant.TryGetValue("side", ref side) && !string.IsNullOrWhiteSpace(side))
			{
				return side;
			}
			return "up";
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00022E50 File Offset: 0x00021050
		protected virtual string GetWireSideCode()
		{
			Block block = base.Block;
			if (((block != null) ? block.Variant : null) == null)
			{
				return null;
			}
			string side;
			if (base.Block.Variant.TryGetValue("side", ref side) && !string.IsNullOrWhiteSpace(side))
			{
				return side;
			}
			return null;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00022E98 File Offset: 0x00021098
		private static string[] GetWireOffsetCandidates(string orientation, string side)
		{
			string combined = (!string.IsNullOrWhiteSpace(side)) ? (orientation + "-" + side) : null;
			string[] result;
			if (!(orientation == "north"))
			{
				if (!(orientation == "east"))
				{
					if (!(orientation == "south"))
					{
						if (!(orientation == "west"))
						{
							if (!(orientation == "down"))
							{
								if (!(orientation == "up"))
								{
									string[] array2;
									if (combined == null)
									{
										string[] array = new string[2];
										array[0] = orientation;
										array2 = array;
										array[1] = "default";
									}
									else
									{
										string[] array3 = new string[3];
										array3[0] = combined;
										array3[1] = orientation;
										array2 = array3;
										array3[2] = "default";
									}
									result = array2;
								}
								else
								{
									string[] array5;
									if (combined == null)
									{
										string[] array4 = new string[3];
										array4[0] = "up";
										array4[1] = "floor";
										array5 = array4;
										array4[2] = "default";
									}
									else
									{
										string[] array6 = new string[4];
										array6[0] = combined;
										array6[1] = "up";
										array6[2] = "floor";
										array5 = array6;
										array6[3] = "default";
									}
									result = array5;
								}
							}
							else
							{
								string[] array8;
								if (combined == null)
								{
									string[] array7 = new string[3];
									array7[0] = "down";
									array7[1] = "ceiling";
									array8 = array7;
									array7[2] = "default";
								}
								else
								{
									string[] array9 = new string[4];
									array9[0] = combined;
									array9[1] = "down";
									array9[2] = "ceiling";
									array8 = array9;
									array9[3] = "default";
								}
								result = array8;
							}
						}
						else
						{
							string[] array11;
							if (combined == null)
							{
								string[] array10 = new string[4];
								array10[0] = "west";
								array10[1] = "wall";
								array10[2] = "side";
								array11 = array10;
								array10[3] = "default";
							}
							else
							{
								string[] array12 = new string[5];
								array12[0] = combined;
								array12[1] = "west";
								array12[2] = "wall";
								array12[3] = "side";
								array11 = array12;
								array12[4] = "default";
							}
							result = array11;
						}
					}
					else
					{
						string[] array14;
						if (combined == null)
						{
							string[] array13 = new string[4];
							array13[0] = "south";
							array13[1] = "wall";
							array13[2] = "side";
							array14 = array13;
							array13[3] = "default";
						}
						else
						{
							string[] array15 = new string[5];
							array15[0] = combined;
							array15[1] = "south";
							array15[2] = "wall";
							array15[3] = "side";
							array14 = array15;
							array15[4] = "default";
						}
						result = array14;
					}
				}
				else
				{
					string[] array17;
					if (combined == null)
					{
						string[] array16 = new string[4];
						array16[0] = "east";
						array16[1] = "wall";
						array16[2] = "side";
						array17 = array16;
						array16[3] = "default";
					}
					else
					{
						string[] array18 = new string[5];
						array18[0] = combined;
						array18[1] = "east";
						array18[2] = "wall";
						array18[3] = "side";
						array17 = array18;
						array18[4] = "default";
					}
					result = array17;
				}
			}
			else
			{
				string[] array20;
				if (combined == null)
				{
					string[] array19 = new string[4];
					array19[0] = "north";
					array19[1] = "wall";
					array19[2] = "side";
					array20 = array19;
					array19[3] = "default";
				}
				else
				{
					string[] array21 = new string[5];
					array21[0] = combined;
					array21[1] = "north";
					array21[2] = "wall";
					array21[3] = "side";
					array20 = array21;
					array21[4] = "default";
				}
				result = array20;
			}
			return result;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002314A File Offset: 0x0002134A
		public virtual Vec3f GetWireAttachmentOffsetFor(BlockPos otherNodePos)
		{
			return this.WireAttachmentOffset;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00023154 File Offset: 0x00021354
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.SetWireAttachmentOffset();
			if (api.Side == 2)
			{
				ICoreClientAPI capi = api as ICoreClientAPI;
				WireNodeRenderer wireRenderer = new WireNodeRenderer(this, capi);
				this.renderer = wireRenderer;
				WireNetworkHandler.ClientSideMessageReceived = (EventHandler<WireNetworkMessage>)Delegate.Combine(WireNetworkHandler.ClientSideMessageReceived, new EventHandler<WireNetworkMessage>(this.OnReceivedMessage));
			}
			if (this.Api.Side == 1)
			{
				long authoritativeNetworkId = WireNetworkHandler.ResolveNetworkIdForPosition(this.Pos);
				if (authoritativeNetworkId != 0L && authoritativeNetworkId != this.NetworkUID)
				{
					this.NetworkUID = authoritativeNetworkId;
				}
				if (this.NetworkUID == 0L)
				{
					if (BEWireNode.IsNetworkRoot(this))
					{
						WireNetworkHandler.AddNewNetwork(this);
					}
				}
				else
				{
					WireNetwork existing = WireNetworkHandler.GetNetwork(this.NetworkUID);
					if (existing != null)
					{
						if (!existing.Nodes.Contains(this))
						{
							existing.AddNode(this);
						}
						WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(this, existing);
					}
					else
					{
						this.EnsureRegisteredInNetworkFromSerializedId();
						WireNetwork recovered = WireNetworkHandler.GetNetwork(this.NetworkUID);
						if (recovered != null)
						{
							WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(this, recovered);
						}
					}
				}
			}
			this.ResolvePendingConnectionsAndNotify();
			this.EnsurePendingConnectionRetryListener();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00023250 File Offset: 0x00021450
		private void EnsureRegisteredInNetworkFromSerializedId()
		{
			if (this.NetworkUID == 0L)
			{
				return;
			}
			WireNetworkHandler.AddNetwork(new WireNetwork
			{
				networkID = this.NetworkUID
			});
			WireNetwork net = WireNetworkHandler.GetNetwork(this.NetworkUID);
			if (net == null)
			{
				return;
			}
			if (!net.Nodes.Contains(this))
			{
				net.AddNode(this);
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000232A1 File Offset: 0x000214A1
		public void MarkForUpdate()
		{
			base.MarkDirty(true, null);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000232AB File Offset: 0x000214AB
		public virtual void OnNetworkCreated(long networkID)
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000232AD File Offset: 0x000214AD
		public IReadOnlyList<WireConnection> GetConnections()
		{
			return this.connections.AsReadOnly();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000232BC File Offset: 0x000214BC
		private void ResolvePendingConnectionsAndNotify()
		{
			ICoreAPI api = this.Api;
			bool flag;
			if (api == null)
			{
				flag = (null != null);
			}
			else
			{
				IWorldAccessor world = api.World;
				flag = (((world != null) ? world.BlockAccessor : null) != null);
			}
			if (!flag)
			{
				return;
			}
			if (this.pendingConnectionPositions == null || this.pendingConnectionPositions.Count == 0)
			{
				return;
			}
			for (int i = this.pendingConnectionPositions.Count - 1; i >= 0; i--)
			{
				BlockPos otherPos = this.pendingConnectionPositions[i];
				if (otherPos == null || otherPos.Equals(this.Pos))
				{
					this.pendingConnectionPositions.RemoveAt(i);
				}
				else
				{
					BEWireNode otherBe = this.Api.World.BlockAccessor.GetBlockEntity(otherPos) as BEWireNode;
					if (otherBe != null)
					{
						WireConnection connection = new WireConnection(this, otherBe);
						this.AddConnection(connection);
						otherBe.AddConnection(connection);
						this.pendingConnectionPositions.RemoveAt(i);
					}
				}
			}
			this.EnsurePendingConnectionRetryListener();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00023394 File Offset: 0x00021594
		private void EnsurePendingConnectionRetryListener()
		{
			if (this.Api == null)
			{
				return;
			}
			if (this.pendingConnectionPositions == null || this.pendingConnectionPositions.Count == 0)
			{
				if (this.pendingConnectionRetryListener != 0L)
				{
					this.UnregisterGameTickListener(this.pendingConnectionRetryListener);
					this.pendingConnectionRetryListener = 0L;
				}
				return;
			}
			if (this.pendingConnectionRetryListener != 0L)
			{
				return;
			}
			this.pendingConnectionRetryListener = this.RegisterGameTickListener(delegate(float _)
			{
				this.ResolvePendingConnectionsAndNotify();
			}, 2000, 0);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00023404 File Offset: 0x00021604
		public void AddConnection(WireConnection connection)
		{
			if (connection == null)
			{
				return;
			}
			if (this.connections.Count >= this.MaxConnections)
			{
				return;
			}
			if (!this.connections.Contains(connection))
			{
				this.connections.Add(connection);
				this.RegisterTopologyEdge(connection.GetOtherBlockPos(this.Pos));
				this.MarkForUpdate();
				Action onConnectionsChanged = this.OnConnectionsChanged;
				if (onConnectionsChanged == null)
				{
					return;
				}
				onConnectionsChanged.Invoke();
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002346B File Offset: 0x0002166B
		private void RegisterTopologyEdge(BlockPos otherPos)
		{
			if (otherPos == null)
			{
				return;
			}
			WireTopologyRegistry.AddEdge(this.Pos, otherPos, true);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00023488 File Offset: 0x00021688
		private void RegisterSerializedTopologyEdges()
		{
			foreach (BlockPos otherPos in this.pendingConnectionPositions)
			{
				this.RegisterTopologyEdge(otherPos);
			}
			foreach (WireConnection connection in this.connections)
			{
				this.RegisterTopologyEdge(connection.GetOtherBlockPos(this.Pos));
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00023528 File Offset: 0x00021728
		public bool HasConnection(WireConnection connection)
		{
			return this.connections.Contains(connection);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00023536 File Offset: 0x00021736
		public bool CanAcceptMoreConnections()
		{
			return this.connections.Count < this.MaxConnections;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002354B File Offset: 0x0002174B
		public int GetMaxConnections()
		{
			return this.MaxConnections;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00023554 File Offset: 0x00021754
		public bool Connect(WireConnection connection, out string failureLangKey, out object[] failureArgs)
		{
			failureLangKey = null;
			failureArgs = Array.Empty<object>();
			if (connection == null || connection.Node1 == null || connection.Node2 == null)
			{
				return false;
			}
			BEWireNode node = connection.Node1 as BEWireNode;
			if (node != null)
			{
				BEWireNode node2 = connection.Node2 as BEWireNode;
				if (node2 != null)
				{
					if (!node.CanAcceptMoreConnections() || !node2.CanAcceptMoreConnections())
					{
						failureLangKey = "Wire.ConnectionDenied.MaxConnections";
						failureArgs = new object[]
						{
							Math.Min(node.GetMaxConnections(), node2.GetMaxConnections())
						};
						return false;
					}
					if (node.HasConnection(connection) || node2.HasConnection(connection))
					{
						return false;
					}
					if (!WireNetworkHandler.CanConnectNodes(node, node2, out failureLangKey, out failureArgs))
					{
						return false;
					}
					connection.Node1.AddConnection(connection);
					connection.Node2.AddConnection(connection);
					Action onConnectionsChanged = this.OnConnectionsChanged;
					if (onConnectionsChanged != null)
					{
						onConnectionsChanged.Invoke();
					}
					if (this.Api.Side == 1)
					{
						BEWireNode wireNode = node;
						BEWireNode wireNode2 = node2;
						WireNetwork net = WireNetworkHandler.GetNetwork(wireNode.NetworkUID);
						WireNetwork net2 = WireNetworkHandler.GetNetwork(wireNode2.NetworkUID);
						if (net == null && net2 == null && !BEWireNode.IsNetworkRoot(wireNode) && !BEWireNode.IsNetworkRoot(wireNode2))
						{
							return false;
						}
						if (net == null && net2 == null)
						{
							HashSet<BEWireNode> hashSet = new HashSet<BEWireNode>();
							hashSet.Add(wireNode);
							hashSet.Add(wireNode2);
							BEWireNode.CreateNetworkForComponent(hashSet);
						}
						else if (net != null && net2 == null)
						{
							net.AddNode(wireNode2);
							WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(wireNode2, net);
						}
						else if (net == null && net2 != null)
						{
							net2.AddNode(wireNode);
							WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(wireNode, net2);
						}
						else if (net != null && net2 != null && net != net2)
						{
							if (net.Nodes.Count >= net2.Nodes.Count)
							{
								net.MergeFrom(net2);
								WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(wireNode, net);
							}
							else
							{
								net2.MergeFrom(net);
								WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(wireNode2, net2);
							}
						}
					}
					if (this.NetworkUID != 0L)
					{
						WireNetworkHandler.RebuildNetworkState(this.NetworkUID);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00023728 File Offset: 0x00021928
		private void OnReceivedMessage(object sender, WireNetworkMessage e)
		{
			if (e.NetworkUID != this.NetworkUID)
			{
				return;
			}
			if (e.RouteMode == WireRouteMode.NamedEndpoint && e.TargetPos != null && !this.Pos.Equals(e.TargetPos))
			{
				return;
			}
			if (e.SenderPos == this.Pos && this.Api.Side == 2)
			{
				IClientPlayer player = (this.Api as ICoreClientAPI).World.Player;
				if (((player != null) ? player.PlayerUID : null) == e.SenderPlayerUID)
				{
					return;
				}
			}
			EventHandler<WireNetworkMessage> onReceivedSignalEvent = this.OnReceivedSignalEvent;
			if (onReceivedSignalEvent != null)
			{
				onReceivedSignalEvent.Invoke(this, e);
			}
			foreach (WireConnection wireConnection in this.connections)
			{
				BEWireNode other = wireConnection.GetOtherNode(this);
				if (other != null && other.Pos != e.SenderPos)
				{
					other.SendSignal(new WireNetworkMessage
					{
						NetworkUID = this.NetworkUID,
						SenderPos = this.Pos,
						Message = e.Message,
						RouteMode = e.RouteMode,
						TargetEndpointName = e.TargetEndpointName,
						TargetPos = e.TargetPos
					});
				}
			}
			using (IEnumerator<BlockPos> enumerator2 = WireTopologyRegistry.GetNeighborPositions(this.Pos).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BlockPos neighborPos = enumerator2.Current;
					if (!(e.SenderPos != null) || !neighborPos.Equals(e.SenderPos))
					{
						BEWireNode neighbor = this.Api.World.BlockAccessor.GetBlockEntity(neighborPos) as BEWireNode;
						if (neighbor != null && neighbor.NetworkUID == this.NetworkUID && !Enumerable.Any<WireConnection>(this.connections, (WireConnection conn) => neighborPos.Equals(conn.GetOtherBlockPos(this.Pos))))
						{
							neighbor.SendSignal(new WireNetworkMessage
							{
								NetworkUID = this.NetworkUID,
								SenderPos = this.Pos,
								Message = e.Message,
								RouteMode = e.RouteMode,
								TargetEndpointName = e.TargetEndpointName,
								TargetPos = e.TargetPos
							});
						}
					}
				}
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00023998 File Offset: 0x00021B98
		public void SendSignal(WireNetworkMessage wireNetworkMessage)
		{
			if (this.Api.Side != 1)
			{
				return;
			}
			ICoreServerAPI coreServerAPI = this.Api as ICoreServerAPI;
			if (coreServerAPI == null)
			{
				return;
			}
			coreServerAPI.Network.GetChannel(WireNetworkHandler.NetworkChannel).SendPacket<WireNetworkMessage>(wireNetworkMessage, Array.Empty<IServerPlayer>());
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000239D4 File Offset: 0x00021BD4
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			this.pendingConnectionRetryListener = 0L;
			if (this.Api.Side != 2)
			{
				foreach (WireConnection connection in new List<WireConnection>(this.connections))
				{
					BEWireNode otherNode = connection.GetOtherNode(this);
					if (otherNode != null)
					{
						otherNode.RemoveConnection(connection);
					}
					if (otherNode != null)
					{
						otherNode.MarkForUpdate();
					}
				}
				this.connections.Clear();
				if (this.Api.Side == 1)
				{
					WireTopologyRegistry.RemoveAllEdgesAt(this.Pos);
				}
				if (this.NetworkUID != 0L)
				{
					WireNetwork network = WireNetworkHandler.GetNetwork(this.NetworkUID);
					if (network != null)
					{
						network.RemoveNode(this);
					}
				}
				this.MarkDirty(true, null);
				return;
			}
			foreach (WireConnection connection2 in new List<WireConnection>(this.connections))
			{
				BEWireNode otherNode2 = connection2.GetOtherNode(this);
				if (otherNode2 != null)
				{
					otherNode2.RemoveConnection(connection2);
				}
				if (otherNode2 != null)
				{
					otherNode2.MarkForUpdate();
				}
			}
			if (this.renderer != null)
			{
				ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.Event.UnregisterRenderer(this.renderer, 1);
				}
				IDisposable disposable = this.renderer;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				this.renderer = null;
			}
			this.connections.Clear();
			Action onConnectionsChanged = this.OnConnectionsChanged;
			if (onConnectionsChanged == null)
			{
				return;
			}
			onConnectionsChanged.Invoke();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00023B6C File Offset: 0x00021D6C
		public void RemoveConnection(WireConnection connection)
		{
			if (this.connections.Remove(connection))
			{
				WireTopologyRegistry.RemoveEdge(this.Pos, connection.GetOtherBlockPos(this.Pos));
				this.MarkForUpdate();
				Action onConnectionsChanged = this.OnConnectionsChanged;
				if (onConnectionsChanged != null)
				{
					onConnectionsChanged.Invoke();
				}
				if (this.Api.Side == 1)
				{
					this.RecalculateNetworksAfterDisconnection();
					WireNetworkHandler.RebuildNetworkState(this.NetworkUID);
				}
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00023BD5 File Offset: 0x00021DD5
		private static bool IsNetworkRoot(BEWireNode node)
		{
			return node is INetworkRoot;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00023BE0 File Offset: 0x00021DE0
		private static HashSet<BEWireNode> FindConnectedComponent(BEWireNode startNode, HashSet<BEWireNode> filterNodes = null)
		{
			HashSet<BEWireNode> component = WireNetworkHandler.GetReachableNodes(startNode);
			if (filterNodes == null)
			{
				return component;
			}
			component.RemoveWhere((BEWireNode node) => node != startNode && !filterNodes.Contains(node));
			return component;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00023C2C File Offset: 0x00021E2C
		private static BEWireNode FindNetworkRoot(IEnumerable<BEWireNode> component)
		{
			if (!Enumerable.Any<BEWireNode>(component))
			{
				return null;
			}
			foreach (BEWireNode node in component)
			{
				if (BEWireNode.IsNetworkRoot(node))
				{
					return node;
				}
			}
			return Enumerable.FirstOrDefault<BEWireNode>(component);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00023C8C File Offset: 0x00021E8C
		private static void CreateNetworkForComponent(HashSet<BEWireNode> component)
		{
			if (component.Count == 0)
			{
				return;
			}
			if (!Enumerable.Any<BEWireNode>(component, (BEWireNode node) => BEWireNode.IsNetworkRoot(node)))
			{
				BEWireNode.RemoveNodesFromNetwork(component);
				return;
			}
			BEWireNode rootNode = BEWireNode.FindNetworkRoot(component);
			if (rootNode == null)
			{
				return;
			}
			WireNetwork network = BEWireNode.TryReuseNetworkID(rootNode);
			if (network == null)
			{
				network = WireNetworkHandler.AddNewNetwork(rootNode);
			}
			WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(rootNode, network);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00023CF4 File Offset: 0x00021EF4
		private static WireNetwork TryReuseNetworkID(BEWireNode rootNode)
		{
			INetworkRoot networkRoot = rootNode as INetworkRoot;
			if (networkRoot != null && networkRoot.CreatedNetworkID != 0L)
			{
				WireNetwork existingNetwork = WireNetworkHandler.GetNetwork(networkRoot.CreatedNetworkID);
				if (existingNetwork != null && existingNetwork.Nodes.Contains(rootNode))
				{
					return existingNetwork;
				}
				if (existingNetwork == null)
				{
					WireNetwork wireNetwork = new WireNetwork();
					wireNetwork.networkID = networkRoot.CreatedNetworkID;
					WireNetworkHandler.AddNetwork(wireNetwork);
					wireNetwork.AddNode(rootNode);
					rootNode.NetworkUID = networkRoot.CreatedNetworkID;
					rootNode.MarkForUpdate();
					return wireNetwork;
				}
			}
			return null;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00023D68 File Offset: 0x00021F68
		private static void RemoveNodesFromNetwork(IEnumerable<BEWireNode> nodes)
		{
			foreach (BEWireNode node in nodes)
			{
				if (node.NetworkUID != 0L)
				{
					WireNetwork network = WireNetworkHandler.GetNetwork(node.NetworkUID);
					if (network != null)
					{
						network.RemoveNode(node);
					}
					node.NetworkUID = 0L;
					node.MarkForUpdate();
				}
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00023DD8 File Offset: 0x00021FD8
		private void HandleIsolatedNode()
		{
			if (BEWireNode.IsNetworkRoot(this))
			{
				INetworkRoot networkRoot = this as INetworkRoot;
				if (networkRoot != null && networkRoot.CreatedNetworkID != 0L)
				{
					WireNetwork existingNetwork = WireNetworkHandler.GetNetwork(networkRoot.CreatedNetworkID);
					if (existingNetwork != null && existingNetwork.Nodes.Contains(this) && existingNetwork.Nodes.Count == 1)
					{
						return;
					}
				}
				if (this.NetworkUID != 0L)
				{
					WireNetwork network = WireNetworkHandler.GetNetwork(this.NetworkUID);
					if (network != null)
					{
						network.RemoveNode(this);
					}
				}
				if (BEWireNode.TryReuseNetworkID(this) == null)
				{
					WireNetworkHandler.AddNewNetwork(this);
					return;
				}
			}
			else if (this.NetworkUID != 0L)
			{
				WireNetwork network2 = WireNetworkHandler.GetNetwork(this.NetworkUID);
				if (network2 == null)
				{
					return;
				}
				network2.RemoveNode(this);
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00023E78 File Offset: 0x00022078
		private static HashSet<BEWireNode> FindOtherComponent(HashSet<BEWireNode> thisComponent, List<BEWireNode> allNetworkNodes)
		{
			HashSet<BEWireNode> processed = new HashSet<BEWireNode>(thisComponent);
			HashSet<BEWireNode> filterSet = new HashSet<BEWireNode>(allNetworkNodes);
			BEWireNode otherComponentStart = Enumerable.FirstOrDefault<BEWireNode>(allNetworkNodes, (BEWireNode node) => !processed.Contains(node));
			if (otherComponentStart == null)
			{
				return new HashSet<BEWireNode>();
			}
			return BEWireNode.FindConnectedComponent(otherComponentStart, filterSet);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00023EC4 File Offset: 0x000220C4
		private static void RemoveComponentFromNetwork(HashSet<BEWireNode> component, WireNetwork network)
		{
			foreach (BEWireNode node in component)
			{
				node.NetworkUID = 0L;
				network.Nodes.Remove(node);
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00023F20 File Offset: 0x00022120
		private static void EnsureComponentHasNetworkUID(HashSet<BEWireNode> component, WireNetwork network, long networkID)
		{
			foreach (BEWireNode node in component)
			{
				if (network.Nodes.Contains(node))
				{
					node.NetworkUID = networkID;
					node.MarkForUpdate();
				}
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00023F84 File Offset: 0x00022184
		private static void SplitNetworkWithBothRoots(HashSet<BEWireNode> component, HashSet<BEWireNode> otherComponent, WireNetwork network)
		{
			if (component.Count >= otherComponent.Count)
			{
				BEWireNode.RemoveComponentFromNetwork(otherComponent, network);
				BEWireNode.EnsureComponentHasNetworkUID(component, network, network.networkID);
				BEWireNode.CreateNetworkForComponent(otherComponent);
				return;
			}
			BEWireNode.RemoveComponentFromNetwork(component, network);
			BEWireNode.EnsureComponentHasNetworkUID(otherComponent, network, network.networkID);
			BEWireNode.CreateNetworkForComponent(component);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00023FDC File Offset: 0x000221DC
		private void RecalculateNetworksAfterDisconnection()
		{
			if (this.connections.Count == 0)
			{
				this.HandleIsolatedNode();
				return;
			}
			if (this.NetworkUID == 0L)
			{
				return;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(this.NetworkUID);
			if (network == null)
			{
				return;
			}
			HashSet<BlockPos> componentPositions = WireTopologyRegistry.GetConnectedComponent(this.Pos);
			List<BlockPos> detachedPositions = Enumerable.ToList<BlockPos>(Enumerable.Where<BlockPos>(Enumerable.ToList<BlockPos>(Enumerable.Select<WireNodeRef, BlockPos>(Enumerable.Where<WireNodeRef>(network.PersistedNodes, (WireNodeRef nodeRef) => nodeRef.Pos != null), (WireNodeRef nodeRef) => nodeRef.Pos)), (BlockPos pos) => !Enumerable.Any<BlockPos>(componentPositions, (BlockPos componentPos) => componentPos.Equals(pos))));
			if (detachedPositions.Count == 0)
			{
				return;
			}
			IBlockAccessor accessor = this.Api.World.BlockAccessor;
			HashSet<BEWireNode> component = WireNetworkHandler.GetLoadedNodesAtPositions(componentPositions, accessor);
			HashSet<BEWireNode> otherComponent = WireNetworkHandler.GetLoadedNodesAtPositions(detachedPositions, accessor);
			int networkRootsInComponent = WireNetworkHandler.CountNetworkRootsInPositions(componentPositions, accessor, network.PersistedNodes);
			int networkRootsInOther = WireNetworkHandler.CountNetworkRootsInPositions(detachedPositions, accessor, network.PersistedNodes);
			WireNetworkHandler.DetachPersistedPositions(network, detachedPositions);
			if (networkRootsInComponent > 0 && networkRootsInOther > 0)
			{
				BEWireNode.SplitNetworkWithBothRoots(component, otherComponent, network);
				return;
			}
			if (networkRootsInComponent == 0)
			{
				BEWireNode.RemoveNodesFromNetwork(component);
				BEWireNode.EnsureComponentHasNetworkUID(otherComponent, network, network.networkID);
				WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(Enumerable.FirstOrDefault<BEWireNode>(otherComponent), network);
				return;
			}
			if (networkRootsInOther == 0)
			{
				BEWireNode.RemoveNodesFromNetwork(otherComponent);
				BEWireNode.EnsureComponentHasNetworkUID(component, network, network.networkID);
				WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(this, network);
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0002414E File Offset: 0x0002234E
		public override void OnBlockBroken(IPlayer byPlayer)
		{
			base.OnBlockBroken(byPlayer);
			this.OnBlockRemoved();
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00024160 File Offset: 0x00022360
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.pendingConnectionRetryListener = 0L;
			if (this.NetworkUID != 0L)
			{
				WireNetwork network = WireNetworkHandler.GetNetwork(this.NetworkUID);
				if (network != null)
				{
					network.DetachNode(this);
				}
			}
			if (this.Api.Side == 2)
			{
				WireNetworkHandler.ClientSideMessageReceived = (EventHandler<WireNetworkMessage>)Delegate.Remove(WireNetworkHandler.ClientSideMessageReceived, new EventHandler<WireNetworkMessage>(this.OnReceivedMessage));
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000241C8 File Offset: 0x000223C8
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
		{
			base.FromTreeAttributes(tree, worldAccessForResolve);
			this.NetworkUID = tree.GetLong("rpvc:networkUID", 0L);
			if (this.NetworkUID != 0L)
			{
				WireNetwork existingNetwork = WireNetworkHandler.GetNetwork(this.NetworkUID);
				if (existingNetwork == null)
				{
					this.EnsureRegisteredInNetworkFromSerializedId();
				}
				else if (!existingNetwork.Nodes.Contains(this))
				{
					existingNetwork.AddNode(this);
				}
			}
			this.connections.Clear();
			this.pendingConnectionPositions.Clear();
			if (!tree.HasAttribute("rpvc:connections"))
			{
				return;
			}
			TreeArrayAttribute connArray = tree["rpvc:connections"] as TreeArrayAttribute;
			if (((connArray != null) ? connArray.value : null) == null || connArray.value.Length == 0)
			{
				return;
			}
			TreeAttribute[] value = connArray.value;
			for (int i = 0; i < value.Length; i++)
			{
				BlockPos otherPos = TreeAttributeUtil.GetBlockPos(value[i], "otherNodePos", null);
				if (otherPos != null && !otherPos.Equals(this.Pos))
				{
					this.pendingConnectionPositions.Add(otherPos);
				}
			}
			this.RegisterSerializedTopologyEdges();
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				this.ResolvePendingConnectionsAndNotify();
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000242E0 File Offset: 0x000224E0
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetLong("rpvc:networkUID", this.NetworkUID);
			List<TreeAttribute> connectionList = new List<TreeAttribute>();
			foreach (WireConnection wireConnection in this.connections)
			{
				BlockPos otherPos = wireConnection.GetOtherBlockPos(this.Pos);
				if (!(otherPos == null))
				{
					TreeAttribute connAttr = new TreeAttribute();
					TreeAttributeUtil.SetBlockPos(connAttr, "otherNodePos", otherPos);
					connectionList.Add(connAttr);
				}
			}
			tree["rpvc:connections"] = new TreeArrayAttribute(connectionList.ToArray());
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00024390 File Offset: 0x00022590
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			this.DisplayNetworkStatus(forPlayer, dsc);
			this.DisplayConnections(forPlayer, dsc);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000243A4 File Offset: 0x000225A4
		protected virtual void DisplayNetworkStatus(IPlayer forPlayer, StringBuilder dsc)
		{
			if (this.NetworkUID == 0L)
			{
				if (BEWireNode.IsNetworkRoot(this))
				{
					ICoreAPI api = this.Api;
					if (api != null && api.Side == 2)
					{
						dsc.AppendLine(UIUtils.I18n("Network.connecting"));
						return;
					}
				}
				dsc.AppendLine(UIUtils.I18n("Network.NoNetwork"));
				return;
			}
			ICoreAPI api2 = this.Api;
			if (api2 != null && api2.Side == 1 && WireNetworkHandler.GetNetwork(this.NetworkUID) == null)
			{
				dsc.AppendLine(UIUtils.I18n("Network.NoNetwork"));
				return;
			}
			WireNetwork resolvedNetwork = WireNetworkHandler.GetNetwork(this.NetworkUID);
			if (resolvedNetwork != null && resolvedNetwork.IsManagedBySwitchboard && resolvedNetwork.HasPoweredSwitchboard)
			{
				dsc.AppendLine(UIUtils.I18n("Network.NetworkId", new object[]
				{
					WireNetworkHandler.GetDisplayName(this.NetworkUID)
				}));
				string subNetworkName = WireNetworkHandler.GetSubNetworkDisplayName(this);
				if (!string.IsNullOrWhiteSpace(subNetworkName))
				{
					dsc.AppendLine(UIUtils.I18n("Network.SubNetworkName", new object[]
					{
						subNetworkName
					}));
				}
				return;
			}
			dsc.AppendLine(UIUtils.I18n("Network.NetworkId", new object[]
			{
				this.NetworkUID
			}));
			string subNetworkNameFallback = WireNetworkHandler.GetSubNetworkDisplayName(this);
			if (!string.IsNullOrWhiteSpace(subNetworkNameFallback))
			{
				dsc.AppendLine(UIUtils.I18n("Network.SubNetworkName", new object[]
				{
					subNetworkNameFallback
				}));
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000244F0 File Offset: 0x000226F0
		protected void DisplayConnections(IPlayer forPlayer, StringBuilder dsc)
		{
			if (this.connections.Count == 0 && this.pendingConnectionPositions.Count > 0)
			{
				foreach (BlockPos otherPos in this.pendingConnectionPositions)
				{
					BEWireNode otherBe = this.Api.World.BlockAccessor.GetBlockEntity(otherPos) as BEWireNode;
					if (otherBe != null)
					{
						WireConnection connection = new WireConnection(this, otherBe);
						this.AddConnection(connection);
					}
				}
			}
			if (this.connections.Count > 0)
			{
				dsc.AppendLine(UIUtils.I18n("Network.Connections"));
				foreach (WireConnection connection2 in this.connections)
				{
					string key = "Network.ConnectionBetween";
					object[] array = new object[2];
					int num = 0;
					IWireConnectable node = connection2.Node1;
					object obj;
					if (node == null)
					{
						obj = null;
					}
					else
					{
						BlockPos position = node.Position;
						obj = ((position != null) ? position.ToString() : null);
					}
					array[num] = obj;
					int num2 = 1;
					IWireConnectable node2 = connection2.Node2;
					object obj2;
					if (node2 == null)
					{
						obj2 = null;
					}
					else
					{
						BlockPos position2 = node2.Position;
						obj2 = ((position2 != null) ? position2.ToString() : null);
					}
					array[num2] = obj2;
					dsc.AppendLine(UIUtils.I18n(key, array));
				}
				dsc.AppendLine();
				return;
			}
			dsc.AppendLine(UIUtils.I18n("Network.NoConnections"));
		}

		// Token: 0x040002D5 RID: 725
		private readonly List<WireConnection> connections = new List<WireConnection>();

		// Token: 0x040002D6 RID: 726
		private List<BlockPos> pendingConnectionPositions = new List<BlockPos>();

		// Token: 0x040002D7 RID: 727
		private IRenderer renderer;

		// Token: 0x040002D8 RID: 728
		private long pendingConnectionRetryListener;
	}
}
