using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class ItemShield : Item, IAttachableToEntity
{
	protected IAttachableToEntity? AttachableToEntity;

	public const string RaiseShieldLeftAnim = "raiseshield-left";

	public const string RaiseShieldRightAnim = "raiseshield-right";

	public int RequiresBehindSlots { get; set; }

	string? IAttachableToEntity.GetCategoryCode(ItemStack stack)
	{
		return AttachableToEntity?.GetCategoryCode(stack);
	}

	CompositeShape? IAttachableToEntity.GetAttachedShape(ItemStack stack, string slotCode)
	{
		return AttachableToEntity?.GetAttachedShape(stack, slotCode);
	}

	string[]? IAttachableToEntity.GetDisableElements(ItemStack stack)
	{
		return AttachableToEntity?.GetDisableElements(stack);
	}

	string[]? IAttachableToEntity.GetKeepElements(ItemStack stack)
	{
		return AttachableToEntity?.GetKeepElements(stack);
	}

	string? IAttachableToEntity.GetTexturePrefixCode(ItemStack stack)
	{
		return AttachableToEntity?.GetTexturePrefixCode(stack);
	}

	void IAttachableToEntity.CollectTextures(ItemStack itemstack, Shape intoShape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		AttachableToEntity?.CollectTextures(itemstack, intoShape, texturePrefixCode, intoDict);
	}

	public bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return Attributes["isAttachable"].AsBool(defaultValue: true);
	}

	public override void OnLoaded(ICoreAPI coreApi)
	{
		base.OnLoaded(coreApi);
		AttachableToEntity = IAttachableToEntity.FromAttributes(this);
	}

	private static void SetAnimActive(IAnimationManager anim, string name, bool active)
	{
		bool isActive = anim.IsAnimationActive(name);
		if (active && !isActive)
		{
			anim.StartAnimation(name);
		}
		else if (!active && isActive)
		{
			anim.StopAnimation(name);
		}
	}

	public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
	{
		bool num = byEntity.LeftHandItemSlot == slot;
		string animThisHand = (num ? "raiseshield-left" : "raiseshield-right");
		string animOtherHand = (num ? "raiseshield-right" : "raiseshield-left");
		bool shouldRaise = byEntity.Controls.Sneak && !byEntity.Controls.RightMouseDown;
		SetAnimActive(byEntity.AnimManager, animThisHand, shouldRaise);
		SetAnimActive(byEntity.AnimManager, animOtherHand, active: false);
		base.OnHeldIdle(slot, byEntity);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		JsonObject shieldAttributes = inSlot.Itemstack?.ItemAttributes?["shield"];
		if (shieldAttributes != null && shieldAttributes.Exists)
		{
			if (shieldAttributes["protectionChance"]["active-projectile"].Exists)
			{
				float activeProjectileProtChance = shieldAttributes["protectionChance"]["active-projectile"].AsFloat();
				float passiveProjectileProtChance = shieldAttributes["protectionChance"]["passive-projectile"].AsFloat();
				float projectileDamageAbsorption = shieldAttributes["projectileDamageAbsorption"].AsFloat();
				dsc.AppendLine("<strong>" + Lang.GetWithFallback("shield-projectile-protection", "Projectile protection") + "</strong>");
				dsc.AppendLine(Lang.Get("shield-stats", (int)(100f * activeProjectileProtChance), (int)(100f * passiveProjectileProtChance), projectileDamageAbsorption));
				dsc.AppendLine();
			}
			float damageAbsorption = shieldAttributes["damageAbsorption"].AsFloat();
			float activeProtChance = shieldAttributes["protectionChance"]["active"].AsFloat();
			float passiveProtChance = shieldAttributes["protectionChance"]["passive"].AsFloat();
			dsc.AppendLine("<strong>" + Lang.GetWithFallback("shield-melee-protection", "Melee attack protection") + "</strong>");
			dsc.AppendLine(Lang.Get("shield-stats", (int)(100f * activeProtChance), (int)(100f * passiveProtChance), damageAbsorption));
			dsc.AppendLine();
		}
	}
}
