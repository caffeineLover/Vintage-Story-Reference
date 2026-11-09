using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001AD RID: 429
	[NullableContext(1)]
	[Nullable(0)]
	public class SoundsSynchronizerServer
	{
		// Token: 0x06001495 RID: 5269 RVA: 0x00065820 File Offset: 0x00063A20
		public SoundsSynchronizerServer(ICoreServerAPI api)
		{
			this._api = api;
			this._api.Network.RegisterChannel("CombatOverhaul:sounds").RegisterMessageType<SoundPacket>().SetMessageHandler<SoundPacket>(new NetworkClientMessageHandler<SoundPacket>(this.HandlePacket));
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0006585C File Offset: 0x00063A5C
		private void HandlePacket(IServerPlayer player, SoundPacket packet)
		{
			if (SoundsSynchronizerClient.IsDefaultPitch(packet.Pitch))
			{
				this._api.World.PlaySoundAt(new AssetLocation(packet.Code), player.Entity, player, packet.RandomizePitch, packet.Range, packet.Volume);
				return;
			}
			this._api.World.PlaySoundAt(SoundsSynchronizerClient.ToSoundAttributes(packet.Code, packet.RandomizePitch, packet.Range, packet.Volume, packet.Pitch), player.Entity, player, 1f);
		}

		// Token: 0x04000A1C RID: 2588
		private readonly ICoreServerAPI _api;
	}
}
