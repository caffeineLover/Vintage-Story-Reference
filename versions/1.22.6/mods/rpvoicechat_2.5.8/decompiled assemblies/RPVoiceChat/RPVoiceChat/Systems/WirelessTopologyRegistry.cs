using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000037 RID: 55
	public static class WirelessTopologyRegistry
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000914D File Offset: 0x0000734D
		public static int LinkCount
		{
			get
			{
				return WirelessTopologyRegistry.graph.LinkCount;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00009159 File Offset: 0x00007359
		public static void Clear()
		{
			WirelessTopologyRegistry.graph.Clear();
			WirelessTopologyRegistry.membersByNetwork.Clear();
			WirelessTopologyRegistry.networkByMemberKey.Clear();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000917C File Offset: 0x0000737C
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

		// Token: 0x06000147 RID: 327 RVA: 0x00009260 File Offset: 0x00007460
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

		// Token: 0x06000148 RID: 328 RVA: 0x000092C9 File Offset: 0x000074C9
		public static void RegisterAntenna(BlockPos antennaPos, long networkId)
		{
			WirelessTopologyRegistry.RegisterMember(networkId, TopologyNodeRef.FromBlock(antennaPos), true);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000092D8 File Offset: 0x000074D8
		public static void BindTalkie(string playerUid, long networkId)
		{
			WirelessTopologyRegistry.RegisterMember(networkId, TopologyNodeRef.FromPlayer(playerUid), true);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000092E8 File Offset: 0x000074E8
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

		// Token: 0x0600014B RID: 331 RVA: 0x0000936C File Offset: 0x0000756C
		public static void UnregisterAntenna(BlockPos antennaPos)
		{
			WirelessTopologyRegistry.UnregisterNode(TopologyNodeRef.FromBlock(antennaPos));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009379 File Offset: 0x00007579
		public static void UnbindTalkie(string playerUid)
		{
			WirelessTopologyRegistry.UnregisterNode(TopologyNodeRef.FromPlayer(playerUid));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009386 File Offset: 0x00007586
		public static bool LinkWireless(TopologyNodeRef a, TopologyNodeRef b)
		{
			return WirelessTopologyRegistry.graph.AddLink(a, b, TopologyLinkKind.Wireless, true);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00009396 File Offset: 0x00007596
		public static bool UnlinkWireless(TopologyNodeRef a, TopologyNodeRef b)
		{
			return WirelessTopologyRegistry.graph.RemoveLink(a, b, TopologyLinkKind.Wireless);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000093A8 File Offset: 0x000075A8
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

		// Token: 0x06000150 RID: 336 RVA: 0x000093DB File Offset: 0x000075DB
		public static long ResolveNetworkIdForAntenna(BlockPos antennaPos)
		{
			return WirelessTopologyRegistry.ResolveNetworkId(TopologyNodeRef.FromBlock(antennaPos));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000093E8 File Offset: 0x000075E8
		public static long ResolveNetworkIdForTalkie(string playerUid)
		{
			return WirelessTopologyRegistry.ResolveNetworkId(TopologyNodeRef.FromPlayer(playerUid));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000093F8 File Offset: 0x000075F8
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

		// Token: 0x06000153 RID: 339 RVA: 0x00009430 File Offset: 0x00007630
		public static IEnumerable<TopologyNodeRef> GetMembers(long networkId)
		{
			WirelessTopologyRegistry.<GetMembers>d__20 <GetMembers>d__ = new WirelessTopologyRegistry.<GetMembers>d__20(-2);
			<GetMembers>d__.<>3__networkId = networkId;
			return <GetMembers>d__;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009440 File Offset: 0x00007640
		public static IEnumerable<BlockPos> GetAntennas(long networkId)
		{
			WirelessTopologyRegistry.<GetAntennas>d__21 <GetAntennas>d__ = new WirelessTopologyRegistry.<GetAntennas>d__21(-2);
			<GetAntennas>d__.<>3__networkId = networkId;
			return <GetAntennas>d__;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009450 File Offset: 0x00007650
		public static IEnumerable<string> GetBoundTalkies(long networkId)
		{
			WirelessTopologyRegistry.<GetBoundTalkies>d__22 <GetBoundTalkies>d__ = new WirelessTopologyRegistry.<GetBoundTalkies>d__22(-2);
			<GetBoundTalkies>d__.<>3__networkId = networkId;
			return <GetBoundTalkies>d__;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009460 File Offset: 0x00007660
		public static HashSet<TopologyNodeRef> GetWirelessComponent(TopologyNodeRef start)
		{
			return WirelessTopologyRegistry.graph.GetConnectedComponent(start, new TopologyLinkKind?(TopologyLinkKind.Wireless));
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009474 File Offset: 0x00007674
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

		// Token: 0x04000080 RID: 128
		public const string TopologyDataKey = "rpvc:wireless-topology";

		// Token: 0x04000081 RID: 129
		private static readonly CommunicationTopologyGraph graph = new CommunicationTopologyGraph();

		// Token: 0x04000082 RID: 130
		private static readonly Dictionary<long, HashSet<string>> membersByNetwork = new Dictionary<long, HashSet<string>>();

		// Token: 0x04000083 RID: 131
		private static readonly Dictionary<string, long> networkByMemberKey = new Dictionary<string, long>();

		// Token: 0x020000E3 RID: 227
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040003B2 RID: 946
			public static Func<string, TopologyNodeRef> <0>__FromKey;
		}
	}
}
