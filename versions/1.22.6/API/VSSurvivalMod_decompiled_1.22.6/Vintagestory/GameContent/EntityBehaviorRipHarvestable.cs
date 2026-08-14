using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("RipHarvest", "Use on a collectible type. If true and the collectible is used to attack a dead entity with this behavior, the entity will be harvested.", "System.Boolean", "Optional", "False", true)]
public class EntityBehaviorRipHarvestable(Entity entity) : EntityBehavior(entity)
{
	protected float ripHarvestChance = 0.25f;

	public override string PropertyName()
	{
		return "ripharvestable";
	}

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
	{
		if (!(damage > 0f) || entity.World.Side != EnumAppSide.Server)
		{
			return;
		}
		EntityAgent eagent = damageSource.SourceEntity as EntityAgent;
		ItemSlot attackWeaponSlot = eagent?.RightHandItemSlot;
		if (attackWeaponSlot?.Itemstack?.ItemAttributes != null && attackWeaponSlot.Itemstack.ItemAttributes.IsTrue("ripHarvest") && entity.World.Rand.NextDouble() < (double)ripHarvestChance)
		{
			EntityBehaviorHarvestable ebh = entity.GetBehavior<EntityBehaviorHarvestable>();
			ebh?.GenerateDrops((eagent as EntityPlayer).Player);
			ItemSlot lootSlot = ebh.Inventory.FirstNonEmptySlot;
			if (lootSlot != null)
			{
				entity.World.SpawnItemEntity(lootSlot.TakeOutWhole(), entity.Pos.XYZ);
			}
		}
	}
}
