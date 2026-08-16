using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A4 RID: 420
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VanillaAnimationsSystemServer
	{
		// Token: 0x06001444 RID: 5188 RVA: 0x000652CC File Offset: 0x000634CC
		public VanillaAnimationsSystemServer(ICoreServerAPI api)
		{
			api.Network.RegisterChannel("CombatOverhaul:vanilla-animations").RegisterMessageType<VanillaAnimationStartPacket>().RegisterMessageType<VanillaAnimationStopPacket>().SetMessageHandler<VanillaAnimationStartPacket>(new NetworkClientMessageHandler<VanillaAnimationStartPacket>(this.StartAnimation)).SetMessageHandler<VanillaAnimationStopPacket>(new NetworkClientMessageHandler<VanillaAnimationStopPacket>(this.StopAnimation));
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0006531C File Offset: 0x0006351C
		private void StartAnimation(IServerPlayer player, VanillaAnimationStartPacket packet)
		{
			player.Entity.StartAnimation(packet.Code);
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0006532F File Offset: 0x0006352F
		private void StopAnimation(IServerPlayer player, VanillaAnimationStopPacket packet)
		{
			player.Entity.StopAnimation(packet.Code);
		}
	}
}
