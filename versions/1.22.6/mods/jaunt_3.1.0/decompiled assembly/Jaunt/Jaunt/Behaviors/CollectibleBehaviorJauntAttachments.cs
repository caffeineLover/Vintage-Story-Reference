using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x0200000D RID: 13
	public class CollectibleBehaviorJauntAttachments : CollectibleBehavior, IAttachedInteractions, IAttachedListener
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003BDC File Offset: 0x00001DDC
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00003BE3 File Offset: 0x00001DE3
		public static ILogger Logger { get; private set; }

		// Token: 0x06000080 RID: 128 RVA: 0x00003BEB File Offset: 0x00001DEB
		public CollectibleBehaviorJauntAttachments(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003BF4 File Offset: 0x00001DF4
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this._api = api;
			JsonObject attributes = this.collObj.Attributes;
			string text;
			if (attributes == null)
			{
				text = null;
			}
			else
			{
				JsonObject jsonObject = attributes["useSound"];
				text = ((jsonObject != null) ? jsonObject.AsString(null) : null);
			}
			string sound = text;
			if (!string.IsNullOrEmpty(sound))
			{
				this.interactSound = new AssetLocation(sound);
			}
			else
			{
				this.interactSound = new AssetLocation("game", "sounds/effect/writing1");
			}
			this.armorerror = Lang.Get("jaunt:armorerror", Array.Empty<object>());
			this.blanketerror = Lang.Get("jaunt:blanketerror", Array.Empty<object>());
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003C90 File Offset: 0x00001E90
		public bool OnTryAttach(ItemSlot itemslot, int slotIndex, Entity toEntity)
		{
			foreach (ItemSlot slot in toEntity.GetBehavior<EntityBehaviorRideableAccessories>().Inventory)
			{
				if (!slot.Empty)
				{
					if (slot.Itemstack.Collectible.Attributes["isMountArmor"].AsBool(false) && itemslot.Itemstack.Collectible.Attributes["isBlanket"].AsBool(false))
					{
						ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
						if (coreClientAPI != null)
						{
							coreClientAPI.TriggerIngameError("itemslot", "jaunt", this.blanketerror);
						}
						return false;
					}
					if (slot.Itemstack.Collectible.Attributes["isBlanket"].AsBool(false) && itemslot.Itemstack.Collectible.Attributes["isMountArmor"].AsBool(false))
					{
						ICoreClientAPI coreClientAPI2 = this._api as ICoreClientAPI;
						if (coreClientAPI2 != null)
						{
							coreClientAPI2.TriggerIngameError("itemslot", "jaunt", this.armorerror);
						}
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003DDC File Offset: 0x00001FDC
		public void OnAttached(ItemSlot itemslot, int slotIndex, Entity toEntity, EntityAgent byEntity)
		{
			JsonObject jsonObject = itemslot.Itemstack.ItemAttributes["useSound"];
			if (jsonObject != null && jsonObject.Exists && this._api.Side == 1)
			{
				byEntity.World.PlaySoundAt(new AssetLocation(this.interactSound), byEntity, null, true, 32f, 1f);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003E44 File Offset: 0x00002044
		public void OnDetached(ItemSlot bagSlot, int slotIndex, Entity fromEntity, EntityAgent byEntity)
		{
			JsonObject jsonObject = bagSlot.Itemstack.ItemAttributes["useSound"];
			if (jsonObject != null && jsonObject.Exists && this._api.Side == 1)
			{
				byEntity.World.PlaySoundAt(new AssetLocation(this.interactSound), byEntity, null, true, 32f, 1f);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003EAC File Offset: 0x000020AC
		public void OnReceivedClientPacket(ItemSlot bagSlot, int slotIndex, Entity onEntity, IServerPlayer player, int packetid, byte[] data, ref EnumHandling handled, Action onRequireSave)
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003EAE File Offset: 0x000020AE
		public bool OnTryDetach(ItemSlot itemslot, int slotIndex, Entity fromEntity)
		{
			return true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003EB1 File Offset: 0x000020B1
		public void OnEntityDeath(ItemSlot itemslot, int slotIndex, Entity onEntity, DamageSource damageSourceForDeath)
		{
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003EB3 File Offset: 0x000020B3
		public void OnEntityDespawn(ItemSlot itemslot, int slotIndex, Entity onEntity, EntityDespawnData despawn)
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003EB5 File Offset: 0x000020B5
		public virtual void OnInteract(ItemSlot bagSlot, int slotIndex, Entity onEntity, EntityAgent byEntity, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled, Action onRequireSave)
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003EB8 File Offset: 0x000020B8
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			float damageReduction = inSlot.Itemstack.Collectible.Attributes["damageReduction"].AsFloat(0f);
			if (damageReduction > 0f)
			{
				float durabilityLeft = (float)inSlot.Itemstack.Collectible.GetRemainingDurability(inSlot.Itemstack);
				float maxDurability = (float)inSlot.Itemstack.Collectible.GetMaxDurability(inSlot.Itemstack);
				float durabilityPercent = (maxDurability > 0f) ? (durabilityLeft / maxDurability) : 0f;
				int damageReductionInPercent = (int)Math.Round((double)(damageReduction * durabilityPercent * 100f));
				dsc.AppendLine(Lang.Get("jaunt-percent-protection", new object[]
				{
					damageReductionInPercent
				}));
			}
			float speedmodifier = inSlot.Itemstack.Collectible.Attributes["speedmodifier"].AsFloat(0f);
			if (speedmodifier > 0f)
			{
				int effectiveSpeedModifier = (int)Math.Round((double)((speedmodifier - 1f) * 100f));
				dsc.AppendLine(Lang.Get("jaunt-walkspeed", new object[]
				{
					effectiveSpeedModifier
				}));
			}
		}

		// Token: 0x04000044 RID: 68
		public ICoreAPI _api;

		// Token: 0x04000046 RID: 70
		private AssetLocation interactSound;

		// Token: 0x04000047 RID: 71
		private string armorerror;

		// Token: 0x04000048 RID: 72
		private string blanketerror;
	}
}
