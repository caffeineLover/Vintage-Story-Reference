using System;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace RPVoiceChat.GameContent.BlockEntityBehavior
{
	// Token: 0x020000C0 RID: 192
	public class BEBehaviorSoundable : BlockEntityBehavior, IBlockEntityRungBehavior
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x00026D4A File Offset: 0x00024F4A
		public BEBehaviorSoundable(BlockEntity blockEntity) : base(blockEntity)
		{
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00026D65 File Offset: 0x00024F65
		public void OnRung()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			this.PlaySoundAtBlock();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00026D88 File Offset: 0x00024F88
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

		// Token: 0x060006CF RID: 1743 RVA: 0x00026ED8 File Offset: 0x000250D8
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

		// Token: 0x060006D0 RID: 1744 RVA: 0x00026F83 File Offset: 0x00025183
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

		// Token: 0x04000305 RID: 773
		private readonly Random _random = new Random();

		// Token: 0x04000306 RID: 774
		private bool _isUsable = true;

		// Token: 0x04000307 RID: 775
		private bool _cooldownActive;

		// Token: 0x04000308 RID: 776
		public const float MaxGain = 2f;
	}
}
