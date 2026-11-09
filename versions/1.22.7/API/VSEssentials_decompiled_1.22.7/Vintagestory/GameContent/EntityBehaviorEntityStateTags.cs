using System.Runtime.CompilerServices;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorEntityStateTags(Entity entity) : EntityBehavior(entity)
{
	protected bool Swimming;

	protected bool FeetInLiquid;

	protected bool OnGround;

	protected bool Flying;

	protected bool Aiming;

	protected bool Moving;

	protected bool Alive;

	protected bool Sprinting;

	protected bool Sneaking;

	protected bool Armed;

	protected bool ArmedMelee;

	protected bool ArmedRanged;

	protected bool HoldingOpenFire;

	protected static TagSetFast TagSwimming;

	protected static TagSetFast TagFeetInLiquid;

	protected static TagSetFast TagFlying;

	protected static TagSetFast TagOnGround;

	protected static TagSetFast TagMoving;

	protected static TagSetFast TagAlive;

	protected static TagSetFast TagAiming;

	protected static TagSetFast TagSprinting;

	protected static TagSetFast TagSneaking;

	protected static TagSetFast TagArmed;

	protected static TagSetFast TagArmedMelee;

	protected static TagSetFast TagArmedRanged;

	protected static TagSetFast TagHoldingOpenFire;

	protected static TagSetFast TagMaskInitialUpdate;

	protected static TagSetFast TagMaskAgentInitialUpdate;

	protected static TagSet CollectibleTagWeapon = TagSet.Empty;

	protected static TagSet CollectibleTagWeaponMelee = TagSet.Empty;

	protected static TagSet CollectibleTagWeaponRanged = TagSet.Empty;

	protected static TagSet CollectibleTagHasOpenFire = TagSet.Empty;

	protected float TimeSinceUpdateSec;

	[DocumentAsJson("Optional", "1", false)]
	protected float UpdatePeriodSec = 1f;

	public override string PropertyName()
	{
		return "entityStateTags";
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		if (attributes.KeyExists("updatePeriodSec"))
		{
			UpdatePeriodSec = attributes["updatePeriodSec"].AsFloat(UpdatePeriodSec);
		}
		TagSetFast tags = entity.Tags;
		TagsInitialUpdate(ref tags);
		if (entity.Tags != tags)
		{
			entity.Tags = tags;
			entity.MarkTagsDirty();
		}
		TimeSinceUpdateSec = (float)entity.World.Rand.NextDouble() * UpdatePeriodSec;
	}

	public override void OnGameTick(float deltaTime)
	{
		if (entity.Api.Side != EnumAppSide.Server)
		{
			return;
		}
		TimeSinceUpdateSec += deltaTime;
		if (!(TimeSinceUpdateSec < UpdatePeriodSec))
		{
			TimeSinceUpdateSec = 0f;
			TagSetFast tags = entity.Tags;
			TagsUpdate(ref tags);
			if (entity.Tags != tags)
			{
				entity.Tags = tags;
				entity.MarkTagsDirty();
			}
			entity.World.FrameProfiler.Mark("statetagsupdate");
		}
	}

	public static void SetupTagIds(ICoreAPI api)
	{
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagSwimming, "state-swimming");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagFeetInLiquid, "state-feet-in-liquid");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagFlying, "state-flying");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagOnGround, "state-on-ground");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagMoving, "state-moving");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagAlive, "state-alive");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagAiming, "state-aiming");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagSprinting, "state-sprinting");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagSneaking, "state-sneaking");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagArmed, "state-armed");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagArmedMelee, "state-armed-melee");
		api.EntityTagRegistry.TryCreateTagSetAndLogIssues(out TagArmedRanged, "state-armed-ranged");
		TagMaskInitialUpdate = ~(TagSwimming | TagFeetInLiquid | TagOnGround | TagAlive);
		TagMaskAgentInitialUpdate = ~(TagMoving | TagAiming | TagFlying | TagSneaking | TagSprinting);
		api.CollectibleTagRegistry.TryCreateTagSetAndLogIssues(out CollectibleTagWeapon, "weapon");
		api.CollectibleTagRegistry.TryCreateTagSetAndLogIssues(out CollectibleTagWeaponMelee, "weapon-melee");
		api.CollectibleTagRegistry.TryCreateTagSetAndLogIssues(out CollectibleTagWeaponRanged, "weapon-ranged");
		api.CollectibleTagRegistry.TryCreateTagSetAndLogIssues(out CollectibleTagHasOpenFire, "has-open-fire");
	}

	protected virtual void TagsInitialUpdate(ref TagSetFast tags)
	{
		EntityTagsInitialUpdate(ref tags);
		if (entity is EntityAgent entityAgent)
		{
			EntityAgentTagsInitialUpdate(entityAgent, ref tags);
			EntityAgentHandItemsTagsInitialUpdate(entityAgent, ref tags);
		}
	}

	protected virtual void TagsUpdate(ref TagSetFast tags)
	{
		EntityTagsUpdate(ref tags);
		if (entity is EntityAgent entityAgent)
		{
			EntityAgentTagsUpdate(entityAgent, ref tags);
			EntityAgentHandItemsTagsUpdate(entityAgent, ref tags);
		}
	}

	protected virtual void EntityTagsInitialUpdate(ref TagSetFast tags)
	{
		tags &= TagMaskInitialUpdate;
		EntityTagsUpdate(ref tags);
	}

	protected virtual void EntityAgentTagsInitialUpdate(EntityAgent entityAgent, ref TagSetFast tags)
	{
		tags &= TagMaskAgentInitialUpdate;
		EntityAgentTagsUpdate(entityAgent, ref tags);
	}

	protected virtual void EntityAgentHandItemsTagsInitialUpdate(EntityAgent entityAgent, ref TagSetFast tags)
	{
		TagSet rightHandItemTags = (entityAgent.RightHandItemSlot?.Itemstack)?.Collectible?.Tags ?? TagSet.Empty;
		Armed = rightHandItemTags.Overlaps(in CollectibleTagWeapon);
		ArmedMelee = rightHandItemTags.Overlaps(in CollectibleTagWeaponMelee);
		ArmedRanged = rightHandItemTags.Overlaps(in CollectibleTagWeaponRanged);
		HoldingOpenFire = rightHandItemTags.Overlaps(in CollectibleTagHasOpenFire);
		TagSet leftHandItemTags = (entityAgent.LeftHandItemSlot?.Itemstack)?.Collectible?.Tags ?? TagSet.Empty;
		Armed = Armed || leftHandItemTags.Overlaps(in CollectibleTagWeapon);
		ArmedMelee = ArmedMelee || leftHandItemTags.Overlaps(in CollectibleTagWeaponMelee);
		ArmedRanged = ArmedRanged || leftHandItemTags.Overlaps(in CollectibleTagWeaponRanged);
		HoldingOpenFire = HoldingOpenFire || leftHandItemTags.Overlaps(in CollectibleTagHasOpenFire);
		InitializeTag(ref tags, Armed, TagArmed);
		InitializeTag(ref tags, ArmedMelee, TagArmedMelee);
		InitializeTag(ref tags, ArmedRanged, TagArmedRanged);
		InitializeTag(ref tags, HoldingOpenFire, TagHoldingOpenFire);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected virtual void UpdateTag(ref TagSetFast tags, ref bool storedValue, TagSetFast mask, bool newValue)
	{
		if (storedValue != newValue)
		{
			storedValue = newValue;
			if (newValue)
			{
				tags |= mask;
			}
			else
			{
				tags &= ~mask;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected virtual void InitializeTag(ref TagSetFast tags, bool newValue, TagSetFast mask)
	{
		if (newValue)
		{
			tags |= mask;
		}
		else
		{
			tags &= ~mask;
		}
	}

	protected virtual void EntityTagsUpdate(ref TagSetFast tags)
	{
		UpdateTag(ref tags, ref Swimming, TagSwimming, entity.Swimming);
		UpdateTag(ref tags, ref FeetInLiquid, TagFeetInLiquid, entity.FeetInLiquid && !Swimming);
		UpdateTag(ref tags, ref OnGround, TagOnGround, entity.OnGround);
		UpdateTag(ref tags, ref Alive, TagAlive, entity.Alive);
	}

	protected virtual void EntityAgentTagsUpdate(EntityAgent entityAgent, ref TagSetFast tags)
	{
		EntityControls controls = entityAgent.Controls;
		UpdateTag(ref tags, ref Moving, TagMoving, controls.Forward || controls.Backward || controls.Right || controls.Left || controls.Jump || controls.Gliding);
		UpdateTag(ref tags, ref Aiming, TagAiming, controls.IsAiming);
		UpdateTag(ref tags, ref Flying, TagFlying, controls.IsFlying);
		UpdateTag(ref tags, ref Sneaking, TagSneaking, controls.Sneak);
		UpdateTag(ref tags, ref Sprinting, TagSprinting, controls.Sprint);
	}

	protected virtual void EntityAgentHandItemsTagsUpdate(EntityAgent entityAgent, ref TagSetFast tags)
	{
		CollectibleObject item = (entityAgent.ActiveHandItemSlot?.Itemstack)?.Collectible;
		if (item == null)
		{
			UpdateTag(ref tags, ref Armed, TagArmed, newValue: false);
			UpdateTag(ref tags, ref ArmedMelee, TagArmedMelee, newValue: false);
			UpdateTag(ref tags, ref ArmedRanged, TagArmedRanged, newValue: false);
			UpdateTag(ref tags, ref HoldingOpenFire, TagHoldingOpenFire, newValue: false);
		}
		else
		{
			UpdateTag(ref tags, ref Armed, TagArmed, CollectibleTagWeapon.Overlaps(in item.Tags));
			UpdateTag(ref tags, ref ArmedMelee, TagArmedMelee, CollectibleTagWeaponMelee.Overlaps(in item.Tags));
			UpdateTag(ref tags, ref ArmedRanged, TagArmedRanged, CollectibleTagWeaponRanged.Overlaps(in item.Tags));
			UpdateTag(ref tags, ref HoldingOpenFire, TagHoldingOpenFire, CollectibleTagHasOpenFire.Overlaps(in item.Tags));
		}
	}
}
