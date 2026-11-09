using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001B2 RID: 434
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VanillaAnimationsSystemServer
	{
		// Token: 0x060014BC RID: 5308 RVA: 0x00066D74 File Offset: 0x00064F74
		public VanillaAnimationsSystemServer(ICoreServerAPI api)
		{
			api.Network.RegisterChannel("CombatOverhaul:vanilla-animations").RegisterMessageType<VanillaAnimationStartPacket>().RegisterMessageType<VanillaAnimationStopPacket>().SetMessageHandler<VanillaAnimationStartPacket>(new NetworkClientMessageHandler<VanillaAnimationStartPacket>(this.StartAnimation)).SetMessageHandler<VanillaAnimationStopPacket>(new NetworkClientMessageHandler<VanillaAnimationStopPacket>(this.StopAnimation));
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00066DC4 File Offset: 0x00064FC4
		private void StartAnimation(IServerPlayer player, VanillaAnimationStartPacket packet)
		{
			player.Entity.StartAnimation(packet.Code);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00066DD7 File Offset: 0x00064FD7
		private void StopAnimation(IServerPlayer player, VanillaAnimationStopPacket packet)
		{
			player.Entity.StopAnimation(packet.Code);
		}
	}
}
