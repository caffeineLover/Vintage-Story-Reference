using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityBehaviorRevivableSimple : EntityBehavior
{
	private ItemStack reviveStack;

	public EntityBehaviorRevivableSimple(Entity entity)
		: base(entity)
	{
	}

	public override string PropertyName()
	{
		return "revivablesimple";
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		JsonItemStack jstack = attributes["reviveStack"].AsObject<JsonItemStack>();
		jstack.Resolve(entity.Api.World, $"entity {entity.Code} revivablesimple stack", printWarningOnError: true);
		reviveStack = jstack.ResolvedItemstack;
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
	{
		if (!byEntity.Controls.ShiftKey || itemslot.Empty)
		{
			return;
		}
		EntityBehaviorHealth ebh = entity.GetBehavior<EntityBehaviorHealth>();
		if (ebh != null && ebh.Health < ebh.MaxHealth && reviveStack != null && itemslot.Itemstack.Satisfies(reviveStack))
		{
			entity.World.PlaySoundAt(new AssetLocation("sounds/effect/latch"), entity, null, randomizePitch: true, 16f);
			itemslot.TakeOut(1);
			itemslot.MarkDirty();
			float health = ebh.Health;
			if (!entity.Alive)
			{
				entity.Revive();
			}
			ebh.Health = health + 15f;
		}
	}

	public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
	{
		if (entity.Alive)
		{
			return null;
		}
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "entityinteraction-revive",
				Itemstacks = new ItemStack[1] { reviveStack },
				HotKeyCode = "shift",
				MouseButton = EnumMouseButton.Right
			}
		};
	}
}
