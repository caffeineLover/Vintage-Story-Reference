using System;
using System.Text;
using RPVoiceChat.Util;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B1 RID: 177
	public class BlockEntityConnector : BEWireNode, IWireConnectable
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x0001D916 File Offset: 0x0001BB16
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			this.DisplayNetworkStatus(forPlayer, dsc);
			base.DisplayConnections(forPlayer, dsc);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001D928 File Offset: 0x0001BB28
		protected override void DisplayNetworkStatus(IPlayer forPlayer, StringBuilder dsc)
		{
			if (base.NetworkUID > 0L)
			{
				dsc.AppendLine(UIUtils.I18n("RelayNode.NetworkId", new object[]
				{
					base.NetworkUID
				}));
				return;
			}
			dsc.AppendLine(UIUtils.I18n("RelayNode.NoNetwork"));
		}
	}
}
