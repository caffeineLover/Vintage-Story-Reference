using System;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x020000C9 RID: 201
	public class ItemSoundEmitting : Item
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x00020DD8 File Offset: 0x0001EFD8
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			JsonObject attributes = this.Attributes;
			int defaultDistance = (attributes != null) ? attributes["soundAudibleDistance"].AsInt(this.AudibleDistance) : this.AudibleDistance;
			JsonObject attributes2 = this.Attributes;
			float defaultVolume = (attributes2 != null) ? attributes2["soundVolume"].AsFloat(this.DefaultVolume) : this.DefaultVolume;
			JsonObject attributes3 = this.Attributes;
			this.soundDuration = ((attributes3 != null) ? attributes3["soundDuration"].AsFloat(this.soundDuration) : this.soundDuration);
			this.AudibleDistance = this.GetConfiguredAudibleDistance(api, defaultDistance);
			this.DefaultVolume = defaultVolume;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00020E7E File Offset: 0x0001F07E
		private int GetConfiguredAudibleDistance(ICoreAPI api, int defaultDistance)
		{
			if (api.Side != 1)
			{
				return defaultDistance;
			}
			if (base.GetType().Name.ToLower() == "soundemitting")
			{
				return this.GetSoundEmittingDistance();
			}
			return defaultDistance;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00020EB0 File Offset: 0x0001F0B0
		private int GetSoundEmittingDistance()
		{
			AssetLocation code = this.Code;
			string text;
			if (code == null)
			{
				text = null;
			}
			else
			{
				string text2 = code.ToString();
				text = ((text2 != null) ? text2.ToLower() : null);
			}
			string itemCode = text ?? "";
			if (itemCode == "handbell")
			{
				return ServerConfigManager.HandbellAudibleDistance;
			}
			if (itemCode == "royalhorn")
			{
				return ServerConfigManager.RoyalhornAudibleDistance;
			}
			if (itemCode == "warhorn")
			{
				return ServerConfigManager.WarhornAudibleDistance;
			}
			JsonObject attributes = this.Attributes;
			if (attributes == null)
			{
				return 16;
			}
			return attributes["soundAudibleDistance"].AsInt(16);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00020F3C File Offset: 0x0001F13C
		public override void OnAttackingWith(IWorldAccessor world, Entity byEntity, Entity attackedEntity, ItemSlot slot)
		{
			base.OnAttackingWith(world, byEntity, attackedEntity, slot);
			if (!EnumAppSideExtensions.IsServer(world.Side))
			{
				return;
			}
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				this.PlaySound("rightClickSounds", player.World.PlayerByUid(player.PlayerUID));
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00020F88 File Offset: 0x0001F188
		public override bool OnBlockBrokenWith(IWorldAccessor world, Entity byEntity, ItemSlot slot, BlockSelection blockSel, float dropQuantityMultiplier = 1f)
		{
			if (!EnumAppSideExtensions.IsServer(world.Side))
			{
				return base.OnBlockBrokenWith(world, byEntity, slot, blockSel, dropQuantityMultiplier);
			}
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				this.PlaySound("breakBlockSounds", player.World.PlayerByUid(player.PlayerUID));
			}
			return base.OnBlockBrokenWith(world, byEntity, slot, blockSel, dropQuantityMultiplier);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (!EnumAppSideExtensions.IsServer(byEntity.World.Side))
			{
				return;
			}
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				this.PlaySound("rightClickSounds", player.World.PlayerByUid(player.PlayerUID));
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0002102A File Offset: 0x0001F22A
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 1;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00021030 File Offset: 0x0001F230
		public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
		{
			return true;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00021033 File Offset: 0x0001F233
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			return true;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00021036 File Offset: 0x0001F236
		private void PlaySound(string soundKey, IPlayer player)
		{
			this.PlaySound(soundKey, player, false);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00021044 File Offset: 0x0001F244
		private void PlaySound(string soundKey, IPlayer player, bool dualCall)
		{
			if (!this.isUsable || player == null)
			{
				return;
			}
			JsonObject attributes = this.Attributes;
			string[] soundList = (attributes != null) ? attributes[soundKey].AsArray<string>(Array.Empty<string>(), "game") : null;
			if (soundList == null || soundList.Length == 0)
			{
				return;
			}
			string chosenSound = soundList[this.rand.Next(soundList.Length)];
			float volume = Math.Clamp(this.DefaultVolume * PlayerListener.ItemGain, 0f, 2f);
			player.Entity.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/" + chosenSound + ".ogg"), player, dualCall ? player : null, false, (float)this.AudibleDistance, volume);
			this.StartSoundDurationCooldown();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000210F8 File Offset: 0x0001F2F8
		private void StartSoundDurationCooldown()
		{
			if (this.cooldownActive)
			{
				return;
			}
			this.cooldownActive = true;
			this.isUsable = false;
			this.api.World.RegisterCallback(delegate(float dt)
			{
				this.isUsable = true;
				this.cooldownActive = false;
			}, (int)(this.soundDuration * 1000f));
		}

		// Token: 0x04000316 RID: 790
		private Random rand = new Random();

		// Token: 0x04000317 RID: 791
		public const float MaxGain = 2f;

		// Token: 0x04000318 RID: 792
		protected int AudibleDistance = 16;

		// Token: 0x04000319 RID: 793
		protected float DefaultVolume = 1f;

		// Token: 0x0400031A RID: 794
		private float soundDuration = 2f;

		// Token: 0x0400031B RID: 795
		private bool isUsable = true;

		// Token: 0x0400031C RID: 796
		private bool cooldownActive;
	}
}
