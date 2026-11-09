using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class AiTaskUseInventory : AiTaskBase
{
	protected AssetLocation useSound;

	protected float useTime = 1f;

	protected float useTimeNow;

	protected bool soundPlayed;

	protected bool doConsumePortion = true;

	protected HashSet<EnumFoodCategory> eatItemCategories = new HashSet<EnumFoodCategory>();

	protected HashSet<AssetLocation> eatItemCodes = new HashSet<AssetLocation>();

	protected bool isEdible;

	public AiTaskUseInventory(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		JsonObject soundCfg = taskConfig["useSound"];
		if (soundCfg.Exists)
		{
			string eatsoundstring = soundCfg.AsString();
			if (eatsoundstring != null)
			{
				useSound = new AssetLocation(eatsoundstring).WithPathPrefix("sounds/");
			}
		}
		useTime = taskConfig["useTime"].AsFloat(1.5f);
		EnumFoodCategory[] array = taskConfig["eatItemCategories"].AsArray(Array.Empty<EnumFoodCategory>());
		foreach (EnumFoodCategory val in array)
		{
			eatItemCategories.Add(val);
		}
		AssetLocation[] array2 = taskConfig["eatItemCodes"].AsArray(Array.Empty<AssetLocation>());
		foreach (AssetLocation val2 in array2)
		{
			eatItemCodes.Add(val2);
		}
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.995;
	}

	public override bool ShouldExecute()
	{
		if (entity.World.Rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		if (cooldownUntilMs > entity.World.ElapsedMilliseconds)
		{
			return false;
		}
		if (cooldownUntilTotalHours > entity.World.Calendar.TotalHours)
		{
			return false;
		}
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		EntityBehaviorMultiplyBase bh = entity.GetBehavior<EntityBehaviorMultiplyBase>();
		if (bh != null && !bh.ShouldEat && entity.World.Rand.NextDouble() < 0.996)
		{
			return false;
		}
		ItemSlot leftSlot = entity.LeftHandItemSlot;
		if (leftSlot.Empty)
		{
			return false;
		}
		isEdible = false;
		EnumFoodCategory? cat = leftSlot.Itemstack.Collectible?.GetNutritionProperties(world, leftSlot.Itemstack, entity)?.FoodCategory;
		if (cat.HasValue && eatItemCategories.Contains(cat.Value))
		{
			isEdible = true;
			return true;
		}
		AssetLocation code = leftSlot.Itemstack?.Collectible?.Code;
		if (code != null && eatItemCodes.Contains(code))
		{
			isEdible = true;
			return true;
		}
		if (!leftSlot.Empty)
		{
			entity.World.SpawnItemEntity(leftSlot.TakeOutWhole(), entity.Pos.XYZ);
		}
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		soundPlayed = false;
		useTimeNow = 0f;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		useTimeNow += dt;
		if (useTimeNow > useTime * 0.75f && !soundPlayed)
		{
			soundPlayed = true;
			if (useSound != null)
			{
				entity.World.PlaySoundAt(useSound, entity, null, randomizePitch: true, 16f);
			}
		}
		if (entity.LeftHandItemSlot == null || entity.LeftHandItemSlot.Empty)
		{
			return false;
		}
		entity.World.SpawnCubeParticles(entity.Pos.XYZ, entity.LeftHandItemSlot.Itemstack, 0.25f, 1, 0.25f + 0.5f * (float)entity.World.Rand.NextDouble());
		if (useTimeNow >= useTime)
		{
			if (isEdible)
			{
				ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("hunger");
				if (tree == null)
				{
					tree = (ITreeAttribute)(entity.WatchedAttributes["hunger"] = new TreeAttribute());
				}
				if (doConsumePortion)
				{
					float sat = 1f;
					tree.SetFloat("saturation", sat + tree.GetFloat("saturation"));
				}
			}
			entity.LeftHandItemSlot.TakeOut(1);
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		if (cancelled)
		{
			cooldownUntilTotalHours = 0.0;
		}
	}
}
