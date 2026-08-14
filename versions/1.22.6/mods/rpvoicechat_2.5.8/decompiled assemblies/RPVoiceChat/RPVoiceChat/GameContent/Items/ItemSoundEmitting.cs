using System;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x0200009D RID: 157
	public class ItemSoundEmitting : Item
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x00019FB4 File Offset: 0x000181B4
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

		// Token: 0x060004DC RID: 1244 RVA: 0x0001A05A File Offset: 0x0001825A
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

		// Token: 0x060004DD RID: 1245 RVA: 0x0001A08C File Offset: 0x0001828C
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

		// Token: 0x060004DE RID: 1246 RVA: 0x0001A118 File Offset: 0x00018318
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

		// Token: 0x060004DF RID: 1247 RVA: 0x0001A164 File Offset: 0x00018364
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001A1C0 File Offset: 0x000183C0
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

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001A206 File Offset: 0x00018406
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 1;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001A20C File Offset: 0x0001840C
		public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
		{
			return true;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001A20F File Offset: 0x0001840F
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			return true;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001A212 File Offset: 0x00018412
		private void PlaySound(string soundKey, IPlayer player)
		{
			this.PlaySound(soundKey, player, false);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001A220 File Offset: 0x00018420
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

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001A2D4 File Offset: 0x000184D4
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

		// Token: 0x04000269 RID: 617
		private Random rand = new Random();

		// Token: 0x0400026A RID: 618
		public const float MaxGain = 2f;

		// Token: 0x0400026B RID: 619
		protected int AudibleDistance = 16;

		// Token: 0x0400026C RID: 620
		protected float DefaultVolume = 1f;

		// Token: 0x0400026D RID: 621
		private float soundDuration = 2f;

		// Token: 0x0400026E RID: 622
		private bool isUsable = true;

		// Token: 0x0400026F RID: 623
		private bool cooldownActive;
	}
}
