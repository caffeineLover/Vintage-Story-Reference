using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000B9 RID: 185
	public class WireNetwork : CommunicationNetworkBase
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001FAF3 File Offset: 0x0001DCF3
		public override NetworkTransportType TransportType
		{
			get
			{
				return NetworkTransportType.Wired;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0001FAF6 File Offset: 0x0001DCF6
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x0001FAFE File Offset: 0x0001DCFE
		public List<BEWireNode> Nodes { get; private set; } = new List<BEWireNode>();

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001FB07 File Offset: 0x0001DD07
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001FB0F File Offset: 0x0001DD0F
		public List<WireNodeRef> PersistedNodes { get; private set; } = new List<WireNodeRef>();

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060005CA RID: 1482 RVA: 0x0001FB18 File Offset: 0x0001DD18
		// (remove) Token: 0x060005CB RID: 1483 RVA: 0x0001FB50 File Offset: 0x0001DD50
		public event Action<BEWireNode, string> OnReceivedSignal;

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0001FB85 File Offset: 0x0001DD85
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0001FB8D File Offset: 0x0001DD8D
		public WireNetworkKind CurrentType { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001FB96 File Offset: 0x0001DD96
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0001FB9E File Offset: 0x0001DD9E
		public bool IsManagedBySwitchboard { get; private set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001FBA7 File Offset: 0x0001DDA7
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0001FBAF File Offset: 0x0001DDAF
		public bool HasPoweredSwitchboard { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001FBB8 File Offset: 0x0001DDB8
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		public int TelegraphEndpointCount { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001FBC9 File Offset: 0x0001DDC9
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x0001FBD1 File Offset: 0x0001DDD1
		public int TelephoneEndpointCount { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001FBDA File Offset: 0x0001DDDA
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001FBE2 File Offset: 0x0001DDE2
		public int RadioEndpointCount { get; private set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001FBEB File Offset: 0x0001DDEB
		public bool AdvancedTelegraphFeaturesEnabled
		{
			get
			{
				return this.IsManagedBySwitchboard && this.HasPoweredSwitchboard;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001FBFD File Offset: 0x0001DDFD
		public bool HasPersistedNodes
		{
			get
			{
				return this.PersistedNodes.Count > 0;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001FC2C File Offset: 0x0001DE2C
		public static WireNetwork FromSaveData(WireNetworkSaveData data)
		{
			WireNetwork wireNetwork = new WireNetwork();
			wireNetwork.networkID = data.NetworkId;
			wireNetwork.SetCustomName(data.CustomName ?? "");
			wireNetwork.PersistedNodes = ((data.Nodes != null) ? new List<WireNodeRef>(data.Nodes) : new List<WireNodeRef>());
			return wireNetwork;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001FC7F File Offset: 0x0001DE7F
		public WireNetworkSaveData ToSaveData()
		{
			return new WireNetworkSaveData
			{
				NetworkId = this.networkID,
				CustomName = base.CustomName,
				Nodes = new List<WireNodeRef>(this.PersistedNodes)
			};
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001FCAF File Offset: 0x0001DEAF
		public void AddNode(BEWireNode node)
		{
			this.JoinNode(node);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001FCB8 File Offset: 0x0001DEB8
		public void JoinNode(BEWireNode node)
		{
			if (node == null)
			{
				return;
			}
			this.UpsertPersistedNode(node);
			if (this.Nodes.Contains(node))
			{
				if (node.NetworkUID != this.networkID)
				{
					node.NetworkUID = this.networkID;
					WireNetwork.MarkNodeDirty(node);
				}
				return;
			}
			this.Nodes.Add(node);
			node.NetworkUID = this.networkID;
			this.RebuildTypedState();
			WireNetwork.MarkNodeDirty(node);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001FD24 File Offset: 0x0001DF24
		public void DetachNode(BEWireNode node)
		{
			if (node == null || !this.Nodes.Remove(node))
			{
				return;
			}
			ICoreAPI api = node.Api;
			if (api != null && api.Side == 1)
			{
				BlockEntityTelegraph detachedTelegraph = node as BlockEntityTelegraph;
				if (detachedTelegraph != null)
				{
					detachedTelegraph.ApplyServerRoutingFlags(false, false, "Telegraph.Settings.DisabledNoPower");
				}
			}
			WireNetwork.MarkNodeDirty(node);
			if (this.Nodes.Count > 0)
			{
				this.RebuildTypedState();
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001FD8C File Offset: 0x0001DF8C
		public void RemoveNode(BEWireNode node)
		{
			if (node == null)
			{
				return;
			}
			this.Nodes.Remove(node);
			this.RemovePersistedNode(node.Pos);
			node.NetworkUID = 0L;
			ICoreAPI api = node.Api;
			if (api != null && api.Side == 1)
			{
				BlockEntityTelegraph detachedTelegraph = node as BlockEntityTelegraph;
				if (detachedTelegraph != null)
				{
					detachedTelegraph.ApplyServerRoutingFlags(false, false, "Telegraph.Settings.DisabledNoPower");
				}
			}
			WireNetwork.MarkNodeDirty(node);
			if (this.Nodes.Count == 0 && this.PersistedNodes.Count == 0)
			{
				WireNetworkHandler.RemoveNetwork(this);
				return;
			}
			if (this.Nodes.Count > 0)
			{
				this.RebuildTypedState();
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001FE28 File Offset: 0x0001E028
		public void UpsertPersistedNode(BEWireNode node)
		{
			if (((node != null) ? node.Pos : null) == null)
			{
				return;
			}
			WireNodeRef nodeRef = WireNetwork.CreateNodeRef(node);
			int index = this.PersistedNodes.FindIndex((WireNodeRef existing) => existing.Pos != null && existing.Pos.Equals(nodeRef.Pos));
			if (index >= 0)
			{
				this.PersistedNodes[index] = nodeRef;
				return;
			}
			this.PersistedNodes.Add(nodeRef);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001FE9C File Offset: 0x0001E09C
		public void RemovePersistedNode(BlockPos pos)
		{
			if (pos == null)
			{
				return;
			}
			this.PersistedNodes.RemoveAll((WireNodeRef existing) => existing.Pos != null && existing.Pos.Equals(pos));
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001FEE0 File Offset: 0x0001E0E0
		public void UpsertPersistedNodeFromPosition(BlockPos pos, IBlockAccessor accessor)
		{
			if (pos == null)
			{
				return;
			}
			BEWireNode node = ((accessor != null) ? accessor.GetBlockEntity(pos) : null) as BEWireNode;
			if (node != null)
			{
				this.UpsertPersistedNode(node);
				return;
			}
			if (this.PersistedNodes.FindIndex((WireNodeRef existing) => existing.Pos != null && existing.Pos.Equals(pos)) < 0)
			{
				this.PersistedNodes.Add(new WireNodeRef(pos.Copy(), WireNodeKind.Infrastructure));
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001FF64 File Offset: 0x0001E164
		public void MergePersistedNodesFrom(WireNetwork otherNetwork)
		{
			if (otherNetwork == null)
			{
				return;
			}
			using (List<WireNodeRef>.Enumerator enumerator = otherNetwork.PersistedNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WireNodeRef nodeRef = enumerator.Current;
					WireNodeRef nodeRef2 = nodeRef;
					if (!(((nodeRef2 != null) ? nodeRef2.Pos : null) == null))
					{
						int index = this.PersistedNodes.FindIndex((WireNodeRef existing) => existing.Pos != null && existing.Pos.Equals(nodeRef.Pos));
						if (index >= 0)
						{
							this.PersistedNodes[index] = nodeRef;
						}
						else
						{
							this.PersistedNodes.Add(nodeRef);
						}
					}
				}
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002001C File Offset: 0x0001E21C
		private static WireNodeRef CreateNodeRef(BEWireNode node)
		{
			WireNodeKind kind = WireNodeKind.Infrastructure;
			IWireTypedNode typedNode = node as IWireTypedNode;
			if (typedNode != null)
			{
				kind = typedNode.WireNodeKind;
			}
			return new WireNodeRef(node.Pos.Copy(), kind);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00020050 File Offset: 0x0001E250
		private static void MarkNodeDirty(BEWireNode node)
		{
			ICoreAPI api = node.Api;
			if (api != null && api.Side == 2)
			{
				((ICoreClientAPI)node.Api).Event.EnqueueMainThreadTask(delegate()
				{
					node.MarkDirty(true, null);
				}, "MarkDirty");
				return;
			}
			node.MarkDirty(true, null);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000200C0 File Offset: 0x0001E2C0
		public void SendSignal(BEWireNode sender, string message)
		{
			Action<BEWireNode, string> onReceivedSignal = this.OnReceivedSignal;
			if (onReceivedSignal != null)
			{
				onReceivedSignal.Invoke(sender, message);
			}
			foreach (BEWireNode node in this.Nodes)
			{
				if (node != sender)
				{
					node.SendSignal(new WireNetworkMessage
					{
						NetworkUID = this.networkID,
						SenderPos = sender.Pos,
						Message = message
					});
				}
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00020150 File Offset: 0x0001E350
		public void MergeFrom(WireNetwork otherNetwork)
		{
			if (otherNetwork == null || otherNetwork == this)
			{
				return;
			}
			this.MergePersistedNodesFrom(otherNetwork);
			foreach (BEWireNode node in Enumerable.ToList<BEWireNode>(otherNetwork.Nodes))
			{
				this.JoinNode(node);
			}
			WireNetworkHandler.RemoveNetwork(otherNetwork);
			this.RebuildTypedState();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000201C4 File Offset: 0x0001E3C4
		public void RebuildTypedState()
		{
			int telegraph = 0;
			int telephone = 0;
			int radio = 0;
			bool hasSwitchboard = false;
			foreach (BEWireNode bewireNode in this.Nodes)
			{
				IWireTypedNode typedNode = bewireNode as IWireTypedNode;
				if (typedNode != null)
				{
					switch (typedNode.WireNodeKind)
					{
					case WireNodeKind.Telegraph:
						telegraph++;
						break;
					case WireNodeKind.Telephone:
						telephone++;
						break;
					case WireNodeKind.Radio:
					case WireNodeKind.RadioConsole:
					case WireNodeKind.RadioEmitter:
						radio++;
						break;
					case WireNodeKind.Switchboard:
						hasSwitchboard = true;
						break;
					}
				}
			}
			this.TelegraphEndpointCount = telegraph;
			this.TelephoneEndpointCount = telephone;
			this.RadioEndpointCount = radio;
			this.IsManagedBySwitchboard = hasSwitchboard;
			int activeKinds = 0;
			if (telegraph > 0)
			{
				activeKinds++;
			}
			if (telephone > 0)
			{
				activeKinds++;
			}
			if (radio > 0)
			{
				activeKinds++;
			}
			if (activeKinds > 1)
			{
				this.CurrentType = WireNetworkKind.None;
			}
			else if (telegraph > 0)
			{
				this.CurrentType = WireNetworkKind.Telegraph;
			}
			else if (telephone > 0)
			{
				this.CurrentType = WireNetworkKind.Telephone;
			}
			else if (radio > 0)
			{
				this.CurrentType = WireNetworkKind.Radio;
			}
			else
			{
				this.CurrentType = WireNetworkKind.None;
			}
			this.HasPoweredSwitchboard = false;
			if (hasSwitchboard)
			{
				foreach (BEWireNode bewireNode2 in this.Nodes)
				{
					ISwitchboardNode switchboardNode = bewireNode2 as ISwitchboardNode;
					if (switchboardNode != null && switchboardNode.HasSufficientPowerFor(this.CurrentType))
					{
						this.HasPoweredSwitchboard = true;
						break;
					}
				}
			}
			WireNetworkHandler.RefreshTelegraphRoutingSnapshot(this.networkID);
		}
	}
}
