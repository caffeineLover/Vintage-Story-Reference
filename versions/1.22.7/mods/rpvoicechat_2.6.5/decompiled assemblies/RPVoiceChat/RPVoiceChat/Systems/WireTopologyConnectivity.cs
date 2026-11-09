using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000056 RID: 86
	public static class WireTopologyConnectivity
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000E47F File Offset: 0x0000C67F
		public static void NotifyNode(ICoreServerAPI api, BEWireNode node)
		{
			if (api == null || node == null)
			{
				return;
			}
			WireTopologyConnectivity.NotifyAffectedNodes(api, new BEWireNode[]
			{
				node
			});
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000E498 File Offset: 0x0000C698
		public static void NotifyAffectedNodes(ICoreServerAPI api, IEnumerable<BEWireNode> nodes)
		{
			if (api == null || nodes == null)
			{
				return;
			}
			RadioConnectivityValidator.ValidateNodesInComponents(nodes);
			TelephoneConnectivityValidator.ValidateNodesInComponents(nodes);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
		public static void NotifyComponents(ICoreServerAPI api, IEnumerable<BEWireNode> componentA, IEnumerable<BEWireNode> componentB)
		{
			if (api == null)
			{
				return;
			}
			List<BEWireNode> merged = new List<BEWireNode>();
			if (componentA != null)
			{
				merged.AddRange(componentA);
			}
			if (componentB != null)
			{
				merged.AddRange(componentB);
			}
			WireTopologyConnectivity.NotifyAffectedNodes(api, merged);
		}
	}
}
