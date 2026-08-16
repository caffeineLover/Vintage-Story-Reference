using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019F RID: 415
	[NullableContext(1)]
	[Nullable(0)]
	public class SoundsSynchronizerServer
	{
		// Token: 0x0600141D RID: 5149 RVA: 0x00063DA0 File Offset: 0x00061FA0
		public SoundsSynchronizerServer(ICoreServerAPI api)
		{
			this._api = api;
			this._api.Network.RegisterChannel("CombatOverhaul:sounds").RegisterMessageType<SoundPacket>().SetMessageHandler<SoundPacket>(new NetworkClientMessageHandler<SoundPacket>(this.HandlePacket));
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x00063DDC File Offset: 0x00061FDC
		private void HandlePacket(IServerPlayer player, SoundPacket packet)
		{
			if (SoundsSynchronizerClient.IsDefaultPitch(packet.Pitch))
			{
				this._api.World.PlaySoundAt(new AssetLocation(packet.Code), player.Entity, player, packet.RandomizePitch, packet.Range, packet.Volume);
				return;
			}
			this._api.World.PlaySoundAt(SoundsSynchronizerClient.ToSoundAttributes(packet.Code, packet.RandomizePitch, packet.Range, packet.Volume, packet.Pitch), player.Entity, player, 1f);
		}

		// Token: 0x040009F4 RID: 2548
		private readonly ICoreServerAPI _api;
	}
}
