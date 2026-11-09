using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000156 RID: 342
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolBagSystemClient
	{
		// Token: 0x060010CF RID: 4303 RVA: 0x00051F90 File Offset: 0x00050190
		public ToolBagSystemClient(ICoreClientAPI api)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<ToolBagPacket>();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00051FB3 File Offset: 0x000501B3
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

		// Token: 0x040007C2 RID: 1986
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x040007C3 RID: 1987
		private readonly IClientNetworkChannel _clientChannel;
	}
}
