using System;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace RPVoiceChat.GameContent.BlockEntityBehavior
{
	// Token: 0x020000F9 RID: 249
	public class BEBehaviorSoundable : BlockEntityBehavior, IBlockEntityRungBehavior
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x000321DE File Offset: 0x000303DE
		public BEBehaviorSoundable(BlockEntity blockEntity) : base(blockEntity)
		{
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x000321F9 File Offset: 0x000303F9
		public void OnRung()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			this.PlaySoundAtBlock();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0003221C File Offset: 0x0003041C
		private void PlaySoundAtBlock()
		{
			if (!this._isUsable)
			{
				return;
			}
			Block block = this.Blockentity.Block;
			if (((block != null) ? block.Attributes : null) == null)
			{
				return;
			}
			string[] soundList = block.Attributes["blockInteractSounds"].AsArray<string>(Array.Empty<string>(), "game");
			if (soundList == null || soundList.Length == 0)
			{
				return;
			}
			this._isUsable = false;
			string sound = soundList[this._random.Next(soundList.Length)];
			float finalVolume = Math.Clamp(block.Attributes["soundVolume"].AsFloat(1f) * PlayerListener.BlockGain, 0f, 2f);
			int audibleDistance = this.GetAudibleDistance(block);
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/" + sound + ".ogg"), (double)this.Blockentity.Pos.X + 0.5, (double)this.Blockentity.Pos.Y + 0.5, (double)this.Blockentity.Pos.Z + 0.5, null, false, (float)audibleDistance, finalVolume);
			float soundDuration = block.Attributes["soundDuration"].AsFloat(2f);
			this.StartSoundDurationCooldown(soundDuration);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0003236C File Offset: 0x0003056C
		private int GetAudibleDistance(Block block)
		{
			JsonObject attributes = block.Attributes;
			int fromAttributes = (attributes != null) ? attributes["soundAudibleDistance"].AsInt(16) : 16;
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return fromAttributes;
			}
			AssetLocation code = block.Code;
			string text;
			if (code == null)
			{
				text = null;
			}
			else
			{
				string path = code.Path;
				text = ((path != null) ? path.ToLowerInvariant() : null);
			}
			string blockCode = text ?? "";
			if (blockCode.StartsWith("callbell"))
			{
				return ServerConfigManager.CallbellAudibleDistance;
			}
			if (blockCode.StartsWith("carillonbell"))
			{
				return ServerConfigManager.CarillonbellAudibleDistance;
			}
			if (blockCode.StartsWith("churchbell"))
			{
				return ServerConfigManager.ChurchbellAudibleDistance;
			}
			return fromAttributes;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00032417 File Offset: 0x00030617
		private void StartSoundDurationCooldown(float soundDurationSeconds)
		{
			if (this._cooldownActive)
			{
				return;
			}
			this._cooldownActive = true;
			this.Api.World.RegisterCallback(delegate(float _)
			{
				this._isUsable = true;
				this._cooldownActive = false;
			}, (int)(soundDurationSeconds * 1000f));
		}

		// Token: 0x040003E4 RID: 996
		private readonly Random _random = new Random();

		// Token: 0x040003E5 RID: 997
		private bool _isUsable = true;

		// Token: 0x040003E6 RID: 998
		private bool _cooldownActive;

		// Token: 0x040003E7 RID: 999
		public const float MaxGain = 2f;
	}
}
