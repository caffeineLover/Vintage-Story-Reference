using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014A RID: 330
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolBagSystemClient
	{
		// Token: 0x0600105B RID: 4187 RVA: 0x00050688 File Offset: 0x0004E888
		public ToolBagSystemClient(ICoreClientAPI api)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<ToolBagPacket>();
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x000506AB File Offset: 0x0004E8AB
		public void Send(string toolBagId, int toolBagIndex, bool mainHand, int slotIndex)
		{
			this._clientChannel.SendPacket<ToolBagPacket>(new ToolBagPacket
			{
				ToolBagId = toolBagId,
				ToolBagIndex = toolBagIndex,
				MainHand = mainHand,
				SlotIndex = slotIndex
			});
		}

		// Token: 0x0400079C RID: 1948
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x0400079D RID: 1949
		private readonly IClientNetworkChannel _clientChannel;
	}
}
