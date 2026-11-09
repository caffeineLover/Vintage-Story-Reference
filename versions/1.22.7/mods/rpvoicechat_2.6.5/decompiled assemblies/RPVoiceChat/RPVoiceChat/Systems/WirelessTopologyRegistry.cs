using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000051 RID: 81
	public static class WirelessTopologyRegistry
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000C915 File Offset: 0x0000AB15
		public static int LinkCount
		{
			get
			{
				return WirelessTopologyRegistry.graph.LinkCount;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C921 File Offset: 0x0000AB21
		public static void Clear()
		{
			WirelessTopologyRegistry.graph.Clear();
			WirelessTopologyRegistry.membersByNetwork.Clear();
			WirelessTopologyRegistry.networkByMemberKey.Clear();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000C944 File Offset: 0x0000AB44
		public static void LoadFromSave(byte[] data)
		{
			WirelessTopologyRegistry.Clear();
			if (data == null || data.Length == 0)
			{
				return;
			}
			WirelessTopologyData payload = SerializerUtil.Deserialize<WirelessTopologyData>(data);
			if (payload == null)
			{
				return;
			}
			WirelessTopologyRegistry.graph.LoadFromData(payload.Links ?? new CommunicationTopologyData());
			if (payload.Networks == null)
			{
				return;
			}
			foreach (WirelessNetworkMembership membership in payload.Networks)
			{
				if (membership != null && membership.NetworkId != 0L && membership.Members != null)
				{
					foreach (TopologyNodeRef member in membership.Members)
					{
						WirelessTopologyRegistry.RegisterMember(membership.NetworkId, member, false);
					}
				}
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000CA28 File Offset: 0x0000AC28
		public static byte[] ToSaveBytes()
		{
			WirelessTopologyData wirelessTopologyData = new WirelessTopologyData();
			wirelessTopologyData.Links = (SerializerUtil.Deserialize<CommunicationTopologyData>(WirelessTopologyRegistry.graph.ToSaveBytes()) ?? new CommunicationTopologyData());
			wirelessTopologyData.Networks = Enumerable.ToList<WirelessNetworkMembership>(Enumerable.Select<KeyValuePair<long, HashSet<string>>, WirelessNetworkMembership>(WirelessTopologyRegistry.membersByNetwork, delegate(KeyValuePair<long, HashSet<string>> entry)
			{
				WirelessNetworkMembership wirelessNetworkMembership = new WirelessNetworkMembership();
				wirelessNetworkMembership.NetworkId = entry.Key;
				IEnumerable<string> value = entry.Value;
				Func<string, TopologyNodeRef> func;
				if ((func = WirelessTopologyRegistry.<>O.<0>__FromKey) == null)
				{
					func = (WirelessTopologyRegistry.<>O.<0>__FromKey = new Func<string, TopologyNodeRef>(TopologyNodeRef.FromKey));
				}
				wirelessNetworkMembership.Members = Enumerable.ToList<TopologyNodeRef>(Enumerable.Where<TopologyNodeRef>(Enumerable.Select<string, TopologyNodeRef>(value, func), (TopologyNodeRef node) => node != null));
				return wirelessNetworkMembership;
			}));
			return SerializerUtil.Serialize<WirelessTopologyData>(wirelessTopologyData);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000CA91 File Offset: 0x0000AC91
		public static void RegisterAntenna(BlockPos antennaPos, long networkId)
		{
			WirelessTopologyRegistry.RegisterMember(networkId, TopologyNodeRef.FromBlock(antennaPos), true);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000CAA0 File Offset: 0x0000ACA0
		public static void BindTalkie(string playerUid, long networkId)
		{
			WirelessTopologyRegistry.RegisterMember(networkId, TopologyNodeRef.FromPlayer(playerUid), true);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
		public static void UnregisterNode(TopologyNodeRef node)
		{
			if (node == null || !node.IsValid)
			{
				return;
			}
			long networkId;
			if (WirelessTopologyRegistry.networkByMemberKey.TryGetValue(node.Key, ref networkId))
			{
				HashSet<string> members;
				WirelessTopologyRegistry.membersByNetwork.TryGetValue(networkId, ref members);
				if (members != null)
				{
					members.Remove(node.Key);
				}
				if (members != null && members.Count == 0)
				{
					WirelessTopologyRegistry.membersByNetwork.Remove(networkId);
				}
				WirelessTopologyRegistry.networkByMemberKey.Remove(node.Key);
			}
			WirelessTopologyRegistry.graph.RemoveAllLinksAt(node, new TopologyLinkKind?(TopologyLinkKind.Wireless));
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000CB34 File Offset: 0x0000AD34
		public static void UnregisterAntenna(BlockPos antennaPos)
		{
			WirelessTopologyRegistry.UnregisterNode(TopologyNodeRef.FromBlock(antennaPos));
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000CB41 File Offset: 0x0000AD41
		public static void UnbindTalkie(string playerUid)
		{
			WirelessTopologyRegistry.UnregisterNode(TopologyNodeRef.FromPlayer(playerUid));
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000CB4E File Offset: 0x0000AD4E
		public static bool LinkWireless(TopologyNodeRef a, TopologyNodeRef b)
		{
			return WirelessTopologyRegistry.graph.AddLink(a, b, TopologyLinkKind.Wireless, true);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000CB5E File Offset: 0x0000AD5E
		public static bool UnlinkWireless(TopologyNodeRef a, TopologyNodeRef b)
		{
			return WirelessTopologyRegistry.graph.RemoveLink(a, b, TopologyLinkKind.Wireless);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000CB70 File Offset: 0x0000AD70
		public static long ResolveNetworkId(TopologyNodeRef node)
		{
			if (node == null || !node.IsValid)
			{
				return 0L;
			}
			long networkId;
			if (!WirelessTopologyRegistry.networkByMemberKey.TryGetValue(node.Key, ref networkId))
			{
				return 0L;
			}
			return networkId;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000CBA3 File Offset: 0x0000ADA3
		public static long ResolveNetworkIdForAntenna(BlockPos antennaPos)
		{
			return WirelessTopologyRegistry.ResolveNetworkId(TopologyNodeRef.FromBlock(antennaPos));
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		public static long ResolveNetworkIdForTalkie(string playerUid)
		{
			return WirelessTopologyRegistry.ResolveNetworkId(TopologyNodeRef.FromPlayer(playerUid));
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
		public static long ResolveNetworkIdFromWiredBlock(BlockPos blockPos)
		{
			if (blockPos == null)
			{
				return 0L;
			}
			long wiredNetworkId = WireNetworkHandler.ResolveNetworkIdForPosition(blockPos);
			if (wiredNetworkId == 0L)
			{
				return 0L;
			}
			long wirelessNetworkId = WirelessTopologyRegistry.ResolveNetworkId(TopologyNodeRef.FromBlock(blockPos));
			if (wirelessNetworkId == 0L)
			{
				return wiredNetworkId;
			}
			return wirelessNetworkId;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
		public static IEnumerable<TopologyNodeRef> GetMembers(long networkId)
		{
			WirelessTopologyRegistry.<GetMembers>d__20 <GetMembers>d__ = new WirelessTopologyRegistry.<GetMembers>d__20(-2);
			<GetMembers>d__.<>3__networkId = networkId;
			return <GetMembers>d__;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000CC08 File Offset: 0x0000AE08
		public static IEnumerable<BlockPos> GetAntennas(long networkId)
		{
			WirelessTopologyRegistry.<GetAntennas>d__21 <GetAntennas>d__ = new WirelessTopologyRegistry.<GetAntennas>d__21(-2);
			<GetAntennas>d__.<>3__networkId = networkId;
			return <GetAntennas>d__;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000CC18 File Offset: 0x0000AE18
		public static IEnumerable<string> GetBoundTalkies(long networkId)
		{
			WirelessTopologyRegistry.<GetBoundTalkies>d__22 <GetBoundTalkies>d__ = new WirelessTopologyRegistry.<GetBoundTalkies>d__22(-2);
			<GetBoundTalkies>d__.<>3__networkId = networkId;
			return <GetBoundTalkies>d__;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000CC28 File Offset: 0x0000AE28
		public static HashSet<TopologyNodeRef> GetWirelessComponent(TopologyNodeRef start)
		{
			return WirelessTopologyRegistry.graph.GetConnectedComponent(start, new TopologyLinkKind?(TopologyLinkKind.Wireless));
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		private static void RegisterMember(long networkId, TopologyNodeRef node, bool rebuildIndexes = true)
		{
			if (networkId == 0L || node == null || !node.IsValid)
			{
				return;
			}
			HashSet<string> members;
			if (!WirelessTopologyRegistry.membersByNetwork.TryGetValue(networkId, ref members))
			{
				members = new HashSet<string>();
				WirelessTopologyRegistry.membersByNetwork[networkId] = members;
			}
			members.Add(node.Key);
			WirelessTopologyRegistry.networkByMemberKey[node.Key] = networkId;
		}

		// Token: 0x040000CB RID: 203
		public const string TopologyDataKey = "rpvc:wireless-topology";

		// Token: 0x040000CC RID: 204
		private static readonly CommunicationTopologyGraph graph = new CommunicationTopologyGraph();

		// Token: 0x040000CD RID: 205
		private static readonly Dictionary<long, HashSet<string>> membersByNetwork = new Dictionary<long, HashSet<string>>();

		// Token: 0x040000CE RID: 206
		private static readonly Dictionary<string, long> networkByMemberKey = new Dictionary<string, long>();

		// Token: 0x02000133 RID: 307
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400050B RID: 1291
			public static Func<string, TopologyNodeRef> <0>__FromKey;
		}
	}
}
