using System;
using System.Text;
using RPVoiceChat.Util;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E3 RID: 227
	public class BlockEntityConnector : BEWireNode, IWireConnectable
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x00024F42 File Offset: 0x00023142
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			this.DisplayNetworkStatus(forPlayer, dsc);
			base.DisplayConnections(forPlayer, dsc);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00024F54 File Offset: 0x00023154
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
