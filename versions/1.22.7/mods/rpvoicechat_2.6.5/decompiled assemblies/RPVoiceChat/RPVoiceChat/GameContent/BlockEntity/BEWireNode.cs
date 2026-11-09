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
	// Token: 0x020000F2 RID: 242
	public class BEWireNode : BlockEntity, IWireConnectable
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0002DF2E File Offset: 0x0002C12E
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x0002DF36 File Offset: 0x0002C136
		public long NetworkUID { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0002DF3F File Offset: 0x0002C13F
		public BlockPos Position
		{
			get
			{
				return this.Pos;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0002DF47 File Offset: 0x0002C147
		public string NodeUID
		{
			get
			{
				return this.Pos.ToString();
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0002DF54 File Offset: 0x0002C154
		protected virtual int MaxConnections
		{
			get
			{
				return ServerConfigManager.TelegraphMaxConnectionsPerNode;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0002DF5B File Offset: 0x0002C15B
		public virtual bool IsActiveEndpoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0002DF5E File Offset: 0x0002C15E
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0002DF66 File Offset: 0x0002C166
		protected EventHandler<WireNetworkMessage> OnReceivedSignalEvent { get; set; }

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600086F RID: 2159 RVA: 0x0002DF70 File Offset: 0x0002C170
		// (remove) Token: 0x06000870 RID: 2160 RVA: 0x0002DFA8 File Offset: 0x0002C1A8
		public event Action OnConnectionsChanged;

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0002DFDD File Offset: 0x0002C1DD
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0002DFE5 File Offset: 0x0002C1E5
		public Vec3f WireAttachmentOffset { get; protected set; } = new Vec3f(0.5f, 0.5f, 0.5f);

		// Token: 0x06000873 RID: 2163 RVA: 0x0002DFF0 File Offset: 0x0002C1F0
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

		// Token: 0x06000874 RID: 2164 RVA: 0x0002E02C File Offset: 0x0002C22C
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

		// Token: 0x06000875 RID: 2165 RVA: 0x0002E10C File Offset: 0x0002C30C
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

		// Token: 0x06000876 RID: 2166 RVA: 0x0002E180 File Offset: 0x0002C380
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

		// Token: 0x06000877 RID: 2167 RVA: 0x0002E1C8 File Offset: 0x0002C3C8
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

		// Token: 0x06000878 RID: 2168 RVA: 0x0002E47A File Offset: 0x0002C67A
		public virtual Vec3f GetWireAttachmentOffsetFor(BlockPos otherNodePos)
		{
			return this.WireAttachmentOffset;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0002E484 File Offset: 0x0002C684
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
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002E578 File Offset: 0x0002C778
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

		// Token: 0x0600087B RID: 2171 RVA: 0x0002E5C9 File Offset: 0x0002C7C9
		public void MarkForUpdate()
		{
			base.MarkDirty(true, null);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002E5D3 File Offset: 0x0002C7D3
		public virtual void OnNetworkCreated(long networkID)
		{
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0002E5D5 File Offset: 0x0002C7D5
		public IReadOnlyList<WireConnection> GetConnections()
		{
			return this.connections.AsReadOnly();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
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

		// Token: 0x0600087F RID: 2175 RVA: 0x0002E6BC File Offset: 0x0002C8BC
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

		// Token: 0x06000880 RID: 2176 RVA: 0x0002E72C File Offset: 0x0002C92C
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

		// Token: 0x06000881 RID: 2177 RVA: 0x0002E793 File Offset: 0x0002C993
		private void RegisterTopologyEdge(BlockPos otherPos)
		{
			if (otherPos == null)
			{
				return;
			}
			WireTopologyRegistry.AddEdge(this.Pos, otherPos, true);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002E7B0 File Offset: 0x0002C9B0
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

		// Token: 0x06000883 RID: 2179 RVA: 0x0002E850 File Offset: 0x0002CA50
		public bool HasConnection(WireConnection connection)
		{
			return this.connections.Contains(connection);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0002E85E File Offset: 0x0002CA5E
		public bool CanAcceptMoreConnections()
		{
			return this.connections.Count < this.MaxConnections;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0002E873 File Offset: 0x0002CA73
		public int GetMaxConnections()
		{
			return this.MaxConnections;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0002E87C File Offset: 0x0002CA7C
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

		// Token: 0x06000887 RID: 2183 RVA: 0x0002EA50 File Offset: 0x0002CC50
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

		// Token: 0x06000888 RID: 2184 RVA: 0x0002ECC0 File Offset: 0x0002CEC0
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

		// Token: 0x06000889 RID: 2185 RVA: 0x0002ECFC File Offset: 0x0002CEFC
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

		// Token: 0x0600088A RID: 2186 RVA: 0x0002EE94 File Offset: 0x0002D094
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
					ICoreServerAPI sapi = this.Api as ICoreServerAPI;
					if (sapi != null)
					{
						WireTopologyConnectivity.NotifyNode(sapi, this);
					}
				}
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0002EF13 File Offset: 0x0002D113
		private static bool IsNetworkRoot(BEWireNode node)
		{
			return node is INetworkRoot;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002EF20 File Offset: 0x0002D120
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

		// Token: 0x0600088D RID: 2189 RVA: 0x0002EF6C File Offset: 0x0002D16C
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

		// Token: 0x0600088E RID: 2190 RVA: 0x0002EFCC File Offset: 0x0002D1CC
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

		// Token: 0x0600088F RID: 2191 RVA: 0x0002F034 File Offset: 0x0002D234
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

		// Token: 0x06000890 RID: 2192 RVA: 0x0002F0A8 File Offset: 0x0002D2A8
		private static void RemoveNodesFromNetwork(IEnumerable<BEWireNode> nodes)
		{
			List<BEWireNode> nodeList = (nodes != null) ? Enumerable.ToList<BEWireNode>(nodes) : null;
			if (nodeList == null || nodeList.Count == 0)
			{
				return;
			}
			ICoreServerAPI sapi = null;
			foreach (BEWireNode node in nodeList)
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
				if (sapi == null)
				{
					sapi = (node.Api as ICoreServerAPI);
				}
			}
			if (sapi != null)
			{
				WireTopologyConnectivity.NotifyAffectedNodes(sapi, nodeList);
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002F150 File Offset: 0x0002D350
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
				if (network2 != null)
				{
					network2.RemoveNode(this);
				}
				this.NetworkUID = 0L;
				this.MarkForUpdate();
				ICoreServerAPI sapi = this.Api as ICoreServerAPI;
				if (sapi != null)
				{
					WireTopologyConnectivity.NotifyNode(sapi, this);
				}
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002F218 File Offset: 0x0002D418
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

		// Token: 0x06000893 RID: 2195 RVA: 0x0002F264 File Offset: 0x0002D464
		private static void RemoveComponentFromNetwork(HashSet<BEWireNode> component, WireNetwork network)
		{
			ICoreServerAPI sapi = null;
			foreach (BEWireNode node in component)
			{
				node.NetworkUID = 0L;
				network.Nodes.Remove(node);
				node.MarkForUpdate();
				if (sapi == null)
				{
					sapi = (node.Api as ICoreServerAPI);
				}
			}
			if (sapi != null)
			{
				WireTopologyConnectivity.NotifyAffectedNodes(sapi, component);
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002F2E4 File Offset: 0x0002D4E4
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

		// Token: 0x06000895 RID: 2197 RVA: 0x0002F348 File Offset: 0x0002D548
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

		// Token: 0x06000896 RID: 2198 RVA: 0x0002F3A0 File Offset: 0x0002D5A0
		private void RecalculateNetworksAfterDisconnection()
		{
			ICoreServerAPI serverApi = this.Api as ICoreServerAPI;
			if (this.connections.Count == 0)
			{
				this.HandleIsolatedNode();
				if (serverApi != null)
				{
					WireTopologyConnectivity.NotifyNode(serverApi, this);
				}
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
			}
			else if (networkRootsInComponent == 0)
			{
				BEWireNode.RemoveNodesFromNetwork(component);
				BEWireNode.EnsureComponentHasNetworkUID(otherComponent, network, network.networkID);
				WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(Enumerable.FirstOrDefault<BEWireNode>(otherComponent), network);
			}
			else if (networkRootsInOther == 0)
			{
				BEWireNode.RemoveNodesFromNetwork(otherComponent);
				BEWireNode.EnsureComponentHasNetworkUID(component, network, network.networkID);
				WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(this, network);
			}
			if (serverApi != null)
			{
				WireTopologyConnectivity.NotifyComponents(serverApi, component, otherComponent);
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002F53C File Offset: 0x0002D73C
		public override void OnBlockBroken(IPlayer byPlayer)
		{
			base.OnBlockBroken(byPlayer);
			this.OnBlockRemoved();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002F54C File Offset: 0x0002D74C
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

		// Token: 0x06000899 RID: 2201 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
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
				ICoreClientAPI capi = this.Api as ICoreClientAPI;
				if (capi != null)
				{
					capi.Event.EnqueueMainThreadTask(new Action(this.ResolvePendingConnectionsAndNotify), "rpvoicechat:ResolvePendingWires");
				}
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0002F6F8 File Offset: 0x0002D8F8
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

		// Token: 0x0600089B RID: 2203 RVA: 0x0002F7A8 File Offset: 0x0002D9A8
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			this.DisplayNetworkStatus(forPlayer, dsc);
			this.DisplayConnections(forPlayer, dsc);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002F7BC File Offset: 0x0002D9BC
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
					this.NetworkUID
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

		// Token: 0x0600089D RID: 2205 RVA: 0x0002F908 File Offset: 0x0002DB08
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

		// Token: 0x040003B4 RID: 948
		private readonly List<WireConnection> connections = new List<WireConnection>();

		// Token: 0x040003B5 RID: 949
		private List<BlockPos> pendingConnectionPositions = new List<BlockPos>();

		// Token: 0x040003B6 RID: 950
		private IRenderer renderer;

		// Token: 0x040003B7 RID: 951
		private long pendingConnectionRetryListener;
	}
}
