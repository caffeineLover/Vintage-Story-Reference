using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemClosedBeenade : Item
{
	protected AssetLocation thrownEntityTypeCode;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		string defaultCode = "thrownbeenade";
		thrownEntityTypeCode = AssetLocation.Create(Attributes?["thrownEntityTypeCode"].AsString(defaultCode) ?? defaultCode, Code.Domain);
	}

	public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
	{
		return null;
	}

	public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (blockSel == null || !(byEntity.World.BlockAccessor.GetBlock(blockSel.Position) is BlockSkep))
		{
			byEntity.Attributes.SetInt("aiming", 1);
			byEntity.AnimManager.StartAnimation("aim");
			handling = EnumHandHandling.PreventDefault;
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (byEntity.World is IClientWorldAccessor)
		{
			ModelTransform modelTransform = new ModelTransform();
			modelTransform.EnsureDefaultValues();
			float offset = GameMath.Clamp(secondsUsed * 3f, 0f, 2f);
			modelTransform.Translation.Set(offset, (0f - offset) / 4f, 0f);
		}
		return true;
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		byEntity.Attributes.SetInt("aiming", 0);
		byEntity.StopAnimation("aim");
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (byEntity.Attributes.GetInt("aiming") == 0)
		{
			return;
		}
		byEntity.Attributes.SetInt("aiming", 0);
		byEntity.StopAnimation("aim");
		if (secondsUsed < 0.35f)
		{
			return;
		}
		float damage = 0.5f;
		ItemStack stack = slot.TakeOut(1);
		slot.MarkDirty();
		IPlayer byPlayer = null;
		if (byEntity is EntityPlayer)
		{
			byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
		}
		byEntity.World.PlaySoundAt(new AssetLocation("sounds/player/throw"), byEntity, byPlayer, randomizePitch: false, 8f);
		EntityProperties type = byEntity.World.GetEntityType(thrownEntityTypeCode);
		Entity entity = byEntity.World.ClassRegistry.CreateEntity(type);
		((EntityThrownBeenade)entity).FiredBy = byEntity;
		((EntityThrownBeenade)entity).Damage = damage;
		((EntityThrownBeenade)entity).ProjectileStack = stack;
		EntityProjectileBase.SpawnProjectile(entity, byEntity, 0.5, 0.75, 0.1, 0.4, -0.21, 20.0);
		byEntity.StartAnimation("throw");
		if (byEntity is EntityPlayer)
		{
			RefillSlotIfEmpty(slot, byEntity, (ItemStack itemstack) => itemstack.Collectible.Code == Code);
		}
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		_ = inSlot.Itemstack.Collectible.Attributes;
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "heldhelp-throw",
				MouseButton = EnumMouseButton.Right
			}
		}.Append(base.GetHeldInteractionHelp(inSlot));
	}
}
