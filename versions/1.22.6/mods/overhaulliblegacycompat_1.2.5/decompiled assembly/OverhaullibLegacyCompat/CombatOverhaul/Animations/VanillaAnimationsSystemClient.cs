using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A3 RID: 419
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VanillaAnimationsSystemClient
	{
		// Token: 0x06001441 RID: 5185 RVA: 0x00065232 File Offset: 0x00063432
		public VanillaAnimationsSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._channel = api.Network.RegisterChannel("CombatOverhaul:vanilla-animations").RegisterMessageType<VanillaAnimationStartPacket>().RegisterMessageType<VanillaAnimationStopPacket>();
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00065261 File Offset: 0x00063461
		public void StartAnimation(string code)
		{
			this._api.World.Player.Entity.StartAnimation(code);
			this._channel.SendPacket<VanillaAnimationStartPacket>(new VanillaAnimationStartPacket
			{
				Code = code
			});
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00065295 File Offset: 0x00063495
		public void StopAnimation(string code)
		{
			this._api.World.Player.Entity.StopAnimation(code);
			this._channel.SendPacket<VanillaAnimationStopPacket>(new VanillaAnimationStopPacket
			{
				Code = code
			});
		}

		// Token: 0x04000A11 RID: 2577
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A12 RID: 2578
		private readonly IClientNetworkChannel _channel;
	}
}
