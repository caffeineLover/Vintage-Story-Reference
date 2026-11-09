using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200004A RID: 74
	public static class TelephoneConnectivityValidator
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x0000BD20 File Offset: 0x00009F20
		public static void ValidateNode(BEWireNode node)
		{
			BlockEntityTelephone telephone = node as BlockEntityTelephone;
			if (telephone != null)
			{
				TelephoneConnectivityValidator.ValidateTelephone(telephone);
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000BD3D File Offset: 0x00009F3D
		public static void ValidateTelephone(BlockEntityTelephone telephone)
		{
			if (telephone != null)
			{
				telephone.InvalidateCallIfDisconnected();
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000BD48 File Offset: 0x00009F48
		public static void ValidateNodesInComponents(IEnumerable<BEWireNode> nodes)
		{
			if (nodes == null)
			{
				return;
			}
			foreach (BEWireNode node in nodes)
			{
				TelephoneConnectivityValidator.ValidateNode(node);
			}
		}
	}
}
