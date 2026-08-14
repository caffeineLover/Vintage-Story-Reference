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
	// Token: 0x02000090 RID: 144
	public class WireNetwork : CommunicationNetworkBase
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00019617 File Offset: 0x00017817
		public override NetworkTransportType TransportType
		{
			get
			{
				return NetworkTransportType.Wired;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001961A File Offset: 0x0001781A
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00019622 File Offset: 0x00017822
		public List<BEWireNode> Nodes { get; private set; } = new List<BEWireNode>();

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001962B File Offset: 0x0001782B
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00019633 File Offset: 0x00017833
		public List<WireNodeRef> PersistedNodes { get; private set; } = new List<WireNodeRef>();

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060004AF RID: 1199 RVA: 0x0001963C File Offset: 0x0001783C
		// (remove) Token: 0x060004B0 RID: 1200 RVA: 0x00019674 File Offset: 0x00017874
		public event Action<BEWireNode, string> OnReceivedSignal;

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000196A9 File Offset: 0x000178A9
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x000196B1 File Offset: 0x000178B1
		public WireNetworkKind CurrentType { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000196BA File Offset: 0x000178BA
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x000196C2 File Offset: 0x000178C2
		public bool IsManagedBySwitchboard { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x000196CB File Offset: 0x000178CB
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x000196D3 File Offset: 0x000178D3
		public bool HasPoweredSwitchboard { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x000196DC File Offset: 0x000178DC
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x000196E4 File Offset: 0x000178E4
		public int TelegraphEndpointCount { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000196ED File Offset: 0x000178ED
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x000196F5 File Offset: 0x000178F5
		public int TelephoneEndpointCount { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000196FE File Offset: 0x000178FE
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00019706 File Offset: 0x00017906
		public int RadioEndpointCount { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001970F File Offset: 0x0001790F
		public bool AdvancedTelegraphFeaturesEnabled
		{
			get
			{
				return this.IsManagedBySwitchboard && this.HasPoweredSwitchboard;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00019721 File Offset: 0x00017921
		public bool HasPersistedNodes
		{
			get
			{
				return this.PersistedNodes.Count > 0;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00019750 File Offset: 0x00017950
		public static WireNetwork FromSaveData(WireNetworkSaveData data)
		{
			WireNetwork wireNetwork = new WireNetwork();
			wireNetwork.networkID = data.NetworkId;
			wireNetwork.SetCustomName(data.CustomName ?? "");
			wireNetwork.PersistedNodes = ((data.Nodes != null) ? new List<WireNodeRef>(data.Nodes) : new List<WireNodeRef>());
			return wireNetwork;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000197A3 File Offset: 0x000179A3
		public WireNetworkSaveData ToSaveData()
		{
			return new WireNetworkSaveData
			{
				NetworkId = this.networkID,
				CustomName = base.CustomName,
				Nodes = new List<WireNodeRef>(this.PersistedNodes)
			};
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000197D3 File Offset: 0x000179D3
		public void AddNode(BEWireNode node)
		{
			this.JoinNode(node);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000197DC File Offset: 0x000179DC
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

		// Token: 0x060004C4 RID: 1220 RVA: 0x00019848 File Offset: 0x00017A48
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

		// Token: 0x060004C5 RID: 1221 RVA: 0x000198B0 File Offset: 0x00017AB0
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

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001994C File Offset: 0x00017B4C
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

		// Token: 0x060004C7 RID: 1223 RVA: 0x000199C0 File Offset: 0x00017BC0
		public void RemovePersistedNode(BlockPos pos)
		{
			if (pos == null)
			{
				return;
			}
			this.PersistedNodes.RemoveAll((WireNodeRef existing) => existing.Pos != null && existing.Pos.Equals(pos));
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00019A04 File Offset: 0x00017C04
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

		// Token: 0x060004C9 RID: 1225 RVA: 0x00019A88 File Offset: 0x00017C88
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

		// Token: 0x060004CA RID: 1226 RVA: 0x00019B40 File Offset: 0x00017D40
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

		// Token: 0x060004CB RID: 1227 RVA: 0x00019B74 File Offset: 0x00017D74
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

		// Token: 0x060004CC RID: 1228 RVA: 0x00019BE4 File Offset: 0x00017DE4
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

		// Token: 0x060004CD RID: 1229 RVA: 0x00019C74 File Offset: 0x00017E74
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

		// Token: 0x060004CE RID: 1230 RVA: 0x00019CE8 File Offset: 0x00017EE8
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
