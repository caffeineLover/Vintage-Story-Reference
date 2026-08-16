using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019E RID: 414
	[NullableContext(1)]
	[Nullable(0)]
	public class SoundsSynchronizerClient
	{
		// Token: 0x06001414 RID: 5140 RVA: 0x00063B34 File Offset: 0x00061D34
		public SoundsSynchronizerClient(ICoreClientAPI api)
		{
			this._api = api;
			this._channel = this._api.Network.RegisterChannel("CombatOverhaul:sounds").RegisterMessageType<SoundPacket>();
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x00063B84 File Offset: 0x00061D84
		public void Play(SoundFrame frame)
		{
			this.Play(frame, 1f);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00063B94 File Offset: 0x00061D94
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

		// Token: 0x06001417 RID: 5143 RVA: 0x00063C12 File Offset: 0x00061E12
		public void Play(string code, bool randomizedPitch = false, float range = 32f, float volume = 1f, bool synchronize = true)
		{
			this.Play(code, randomizedPitch, range, volume, synchronize, 1f);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00063C28 File Offset: 0x00061E28
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

		// Token: 0x06001419 RID: 5145 RVA: 0x00063C88 File Offset: 0x00061E88
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

		// Token: 0x0600141A RID: 5146 RVA: 0x00063D04 File Offset: 0x00061F04
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

		// Token: 0x0600141B RID: 5147 RVA: 0x00063D6B File Offset: 0x00061F6B
		internal static float NormalizePitch(float pitch)
		{
			if (pitch > 0f)
			{
				return Math.Max(0.01f, pitch);
			}
			return 1f;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00063D86 File Offset: 0x00061F86
		internal static bool IsDefaultPitch(float pitch)
		{
			return Math.Abs(SoundsSynchronizerClient.NormalizePitch(pitch) - 1f) < 0.0001f;
		}

		// Token: 0x040009F1 RID: 2545
		private readonly ICoreClientAPI _api;

		// Token: 0x040009F2 RID: 2546
		private readonly IClientNetworkChannel _channel;

		// Token: 0x040009F3 RID: 2547
		private readonly NatFloat _random = new NatFloat(0.5f, 0.5f, 0);
	}
}
