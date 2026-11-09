using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001B1 RID: 433
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VanillaAnimationsSystemClient
	{
		// Token: 0x060014B9 RID: 5305 RVA: 0x00066CDA File Offset: 0x00064EDA
		public VanillaAnimationsSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._channel = api.Network.RegisterChannel("CombatOverhaul:vanilla-animations").RegisterMessageType<VanillaAnimationStartPacket>().RegisterMessageType<VanillaAnimationStopPacket>();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00066D09 File Offset: 0x00064F09
		public void StartAnimation(string code)
		{
			this._api.World.Player.Entity.StartAnimation(code);
			this._channel.SendPacket<VanillaAnimationStartPacket>(new VanillaAnimationStartPacket
			{
				Code = code
			});
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00066D3D File Offset: 0x00064F3D
		public void StopAnimation(string code)
		{
			this._api.World.Player.Entity.StopAnimation(code);
			this._channel.SendPacket<VanillaAnimationStopPacket>(new VanillaAnimationStopPacket
			{
				Code = code
			});
		}

		// Token: 0x04000A39 RID: 2617
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A3A RID: 2618
		private readonly IClientNetworkChannel _channel;
	}
}
