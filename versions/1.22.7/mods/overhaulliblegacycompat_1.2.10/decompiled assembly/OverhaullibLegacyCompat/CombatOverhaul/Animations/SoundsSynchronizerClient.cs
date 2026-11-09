using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001AC RID: 428
	[NullableContext(1)]
	[Nullable(0)]
	public class SoundsSynchronizerClient
	{
		// Token: 0x0600148C RID: 5260 RVA: 0x000655B4 File Offset: 0x000637B4
		public SoundsSynchronizerClient(ICoreClientAPI api)
		{
			this._api = api;
			this._channel = this._api.Network.RegisterChannel("CombatOverhaul:sounds").RegisterMessageType<SoundPacket>();
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00065604 File Offset: 0x00063804
		public void Play(SoundFrame frame)
		{
			this.Play(frame, 1f);
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00065614 File Offset: 0x00063814
		public void Play(SoundFrame frame, float animationSpeed)
		{
			int index = Math.Clamp((int)Math.Floor((double)this._random.nextFloat((float)frame.Code.Length)), 0, frame.Code.Length - 1);
			float pitch = frame.GetPitch(animationSpeed);
			this.PlayFor(frame.Code[index], frame.RandomizePitch, frame.Range, frame.Volume, pitch);
			if (frame.Synchronize)
			{
				this._channel.SendPacket<SoundPacket>(new SoundPacket(frame, index, pitch));
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00065692 File Offset: 0x00063892
		public void Play(string code, bool randomizedPitch = false, float range = 32f, float volume = 1f, bool synchronize = true)
		{
			this.Play(code, randomizedPitch, range, volume, synchronize, 1f);
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000656A8 File Offset: 0x000638A8
		public void Play(string code, bool randomizedPitch, float range, float volume, bool synchronize, float pitch)
		{
			pitch = SoundsSynchronizerClient.NormalizePitch(pitch);
			this.PlayFor(code, randomizedPitch, range, volume, pitch);
			SoundPacket packet = new SoundPacket
			{
				Code = code,
				RandomizePitch = randomizedPitch,
				Range = range,
				Volume = volume,
				Pitch = pitch
			};
			if (synchronize)
			{
				this._channel.SendPacket<SoundPacket>(packet);
			}
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00065708 File Offset: 0x00063908
		private void PlayFor(string code, bool randomizedPitch, float range, float volume, float pitch)
		{
			pitch = SoundsSynchronizerClient.NormalizePitch(pitch);
			if (SoundsSynchronizerClient.IsDefaultPitch(pitch))
			{
				this._api.World.PlaySoundFor(new AssetLocation(code), this._api.World.Player, randomizedPitch, range, volume);
				return;
			}
			this._api.World.PlaySoundFor(SoundsSynchronizerClient.ToSoundAttributes(code, randomizedPitch, range, volume, pitch), this._api.World.Player, 1f);
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00065784 File Offset: 0x00063984
		internal static SoundAttributes ToSoundAttributes(string code, bool randomizedPitch, float range, float volume, float pitch)
		{
			pitch = SoundsSynchronizerClient.NormalizePitch(pitch);
			SoundAttributes result;
			result..ctor();
			result.Location = new AssetLocation(code);
			result.Pitch = new NatFloat(pitch, randomizedPitch ? (pitch * 0.25f) : 0f, 0);
			result.Volume = new NatFloat(volume, 0f, 0);
			result.Range = range;
			return result;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x000657EB File Offset: 0x000639EB
		internal static float NormalizePitch(float pitch)
		{
			if (pitch > 0f)
			{
				return Math.Max(0.01f, pitch);
			}
			return 1f;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00065806 File Offset: 0x00063A06
		internal static bool IsDefaultPitch(float pitch)
		{
			return Math.Abs(SoundsSynchronizerClient.NormalizePitch(pitch) - 1f) < 0.0001f;
		}

		// Token: 0x04000A19 RID: 2585
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A1A RID: 2586
		private readonly IClientNetworkChannel _channel;

		// Token: 0x04000A1B RID: 2587
		private readonly NatFloat _random = new NatFloat(0.5f, 0.5f, 0);
	}
}
