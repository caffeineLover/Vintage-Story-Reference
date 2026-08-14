using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskSeekFoodAndEat : AiTaskBase
{
	private AssetLocation eatSound;

	private POIRegistry porregistry;

	private IAnimalFoodSource targetPoi;

	private float moveSpeed = 0.02f;

	private long stuckatMs;

	private bool nowStuck;

	private float eatTime = 1f;

	private float eatTimeNow;

	private bool soundPlayed;

	private bool doConsumePortion = true;

	private bool eatAnimStarted;

	private bool eatLooseItems;

	private float quantityEaten;

	private AnimationMetaData eatAnimMeta;

	private AnimationMetaData eatAnimMetaLooseItems;

	private Dictionary<IAnimalFoodSource, FailedAttempt> failedSeekTargets = new Dictionary<IAnimalFoodSource, FailedAttempt>();

	private float extraTargetDist;

	private long lastPOISearchTotalMs;

	public CreatureDiet Diet;

	private EntityBehaviorMultiplyBase bhMultiply;

	private ICoreAPI api;

	public AiTaskSeekFoodAndEat(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		api = entity.Api;
		porregistry = api.ModLoader.GetModSystem<POIRegistry>();
		entity.WatchedAttributes.SetBool("doesEat", value: true);
		string eatsoundstring = taskConfig["eatSound"].AsString();
		if (eatsoundstring != null)
		{
			eatSound = new AssetLocation(eatsoundstring).WithPathPrefix("sounds/");
		}
		moveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
		eatTime = taskConfig["eatTime"].AsFloat(1.5f);
		doConsumePortion = taskConfig["doConsumePortion"].AsBool(defaultValue: true);
		eatLooseItems = taskConfig["eatLooseItems"].AsBool(defaultValue: true);
		Diet = entity.Properties.Attributes["creatureDiet"].AsObject<CreatureDiet>();
		if (Diet == null)
		{
			api.Logger.Warning("Creature " + entity.Code.ToShortString() + " has SeekFoodAndEat task but no Diet specified");
		}
		if (taskConfig["eatAnimation"].Exists)
		{
			eatAnimMeta = new AnimationMetaData
			{
				Code = taskConfig["eatAnimation"].AsString()?.ToLowerInvariant(),
				Animation = taskConfig["eatAnimation"].AsString()?.ToLowerInvariant(),
				AnimationSpeed = taskConfig["eatAnimationSpeed"].AsFloat(1f)
			}.Init();
		}
		if (taskConfig["eatAnimationLooseItems"].Exists)
		{
			eatAnimMetaLooseItems = new AnimationMetaData
			{
				Code = taskConfig["eatAnimationLooseItems"].AsString()?.ToLowerInvariant(),
				Animation = taskConfig["eatAnimationLooseItems"].AsString()?.ToLowerInvariant(),
				AnimationSpeed = taskConfig["eatAnimationSpeedLooseItems"].AsFloat(1f)
			}.Init();
		}
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.995;
	}

	public override void AfterInitialize()
	{
		bhMultiply = entity.GetBehavior<EntityBehaviorMultiplyBase>();
	}

	public override bool ShouldExecute()
	{
		if (entity.World.Rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		if (lastPOISearchTotalMs + 15000 > entity.World.ElapsedMilliseconds)
		{
			return false;
		}
		if (IsOnCooldown())
		{
			return false;
		}
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (bhMultiply != null && !bhMultiply.ShouldEat && entity.World.Rand.NextDouble() < 0.996)
		{
			return false;
		}
		if (Diet == null)
		{
			return false;
		}
		targetPoi = null;
		extraTargetDist = 0f;
		lastPOISearchTotalMs = entity.World.ElapsedMilliseconds;
		if (eatLooseItems)
		{
			api.ModLoader.GetModSystem<EntityPartitioning>().WalkEntities(entity.Pos.XYZ, 10.0, delegate(Entity e)
			{
				if (e is EntityItem entityItem && suitableFoodSource(entityItem.Itemstack))
				{
					targetPoi = new LooseItemFoodSource(entityItem);
					return false;
				}
				return true;
			}, EnumEntitySearchType.Inanimate);
		}
		if (targetPoi == null)
		{
			targetPoi = porregistry.GetNearestPoi(entity.Pos.XYZ, 48f, delegate(IPointOfInterest poi)
			{
				if (poi.Type != "food")
				{
					return false;
				}
				IAnimalFoodSource key;
				IAnimalFoodSource animalFoodSource = (key = poi as IAnimalFoodSource);
				if (animalFoodSource != null && animalFoodSource.IsSuitableFor(entity, Diet))
				{
					failedSeekTargets.TryGetValue(key, out var value);
					if (value == null || value.Count < 4 || value.LastTryMs < world.ElapsedMilliseconds - 60000)
					{
						return true;
					}
				}
				return false;
			}) as IAnimalFoodSource;
		}
		return targetPoi != null;
	}

	private bool suitableFoodSource(ItemStack itemStack)
	{
		EnumFoodCategory cat = itemStack?.Collectible.GetNutritionProperties(world, itemStack, entity)?.FoodCategory ?? EnumFoodCategory.NoNutrition;
		string[] tags = itemStack?.ItemAttributes?["foodTags"].AsArray<string>() ?? Array.Empty<string>();
		return Diet.Matches(cat, tags);
	}

	public float MinDistanceToTarget()
	{
		return Math.Max(extraTargetDist + 0.6f, entity.SelectionBox.XSize / 2f + 0.05f);
	}

	public override void StartExecute()
	{
		base.StartExecute();
		stuckatMs = -9999L;
		nowStuck = false;
		soundPlayed = false;
		eatTimeNow = 0f;
		pathTraverser.NavigateTo_Async(targetPoi.Position, moveSpeed, MinDistanceToTarget() - 0.1f, OnGoalReached, OnStuck, null, 1000, 1);
		eatAnimStarted = false;
	}

	public override bool CanContinueExecute()
	{
		return pathTraverser.Ready;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		Vec3d pos = targetPoi.Position;
		pathTraverser.CurrentTarget.X = pos.X;
		pathTraverser.CurrentTarget.Y = pos.Y;
		pathTraverser.CurrentTarget.Z = pos.Z;
		double num = entity.SelectionBox.ToDouble().Translate(entity.Pos.X, entity.Pos.Y, entity.Pos.Z).ShortestDistanceFrom(pos);
		float minDist = MinDistanceToTarget();
		if (num <= (double)minDist)
		{
			pathTraverser.Stop();
			if (animMeta != null && entity.AnimManager.IsAnimationActive(animMeta.Code))
			{
				entity.AnimManager.StopAnimation(animMeta.Code);
			}
			if (bhMultiply != null && !bhMultiply.ShouldEat)
			{
				return false;
			}
			if (!targetPoi.IsSuitableFor(entity, Diet))
			{
				return false;
			}
			if (eatAnimMeta != null && !eatAnimStarted)
			{
				entity.AnimManager.StartAnimation((targetPoi is LooseItemFoodSource && eatAnimMetaLooseItems != null) ? eatAnimMetaLooseItems : eatAnimMeta);
				eatAnimStarted = true;
			}
			eatTimeNow += dt;
			if (targetPoi is LooseItemFoodSource { ItemStack: not null } foodSource)
			{
				entity.World.SpawnCubeParticles(targetPoi.Position, foodSource.ItemStack, 0.25f, 1, 0.25f + 0.5f * (float)entity.World.Rand.NextDouble());
			}
			if (eatTimeNow > eatTime * 0.75f && !soundPlayed)
			{
				soundPlayed = true;
				if (eatSound != null)
				{
					entity.World.PlaySoundAt(eatSound, entity, null, randomizePitch: true, 16f);
				}
			}
			if (eatTimeNow >= eatTime)
			{
				ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("hunger");
				if (tree == null)
				{
					tree = (ITreeAttribute)(entity.WatchedAttributes["hunger"] = new TreeAttribute());
				}
				if (doConsumePortion)
				{
					float sat = targetPoi.ConsumeOnePortion(entity);
					quantityEaten += sat;
					tree.SetFloat("saturation", sat + tree.GetFloat("saturation"));
					entity.WatchedAttributes.SetDouble("lastMealEatenTotalHours", entity.World.Calendar.TotalHours);
					entity.WatchedAttributes.MarkPathDirty("hunger");
				}
				else
				{
					quantityEaten = 1f;
				}
				failedSeekTargets.Remove(targetPoi);
				return false;
			}
		}
		else if (!pathTraverser.Active)
		{
			float rndx = (float)entity.World.Rand.NextDouble() * 0.3f - 0.15f;
			float rndz = (float)entity.World.Rand.NextDouble() * 0.3f - 0.15f;
			if (!pathTraverser.NavigateTo(targetPoi.Position.AddCopy(rndx, 0f, rndz), moveSpeed, minDist - 0.15f, OnGoalReached, OnStuck, null, giveUpWhenNoPath: false, 500, 1))
			{
				return false;
			}
		}
		if (nowStuck && (float)entity.World.ElapsedMilliseconds > (float)stuckatMs + eatTime * 1000f)
		{
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		EntityBehaviorMultiply bh = entity.GetBehavior<EntityBehaviorMultiply>();
		if (bh != null && bh.PortionsLeftToEat > 0f && !bh.IsPregnant)
		{
			cooldownUntilTotalHours += MinCooldownHours + entity.World.Rand.NextDouble() * (MaxCooldownHours - MinCooldownHours);
		}
		else
		{
			cooldownUntilTotalHours = api.World.Calendar.TotalHours + MinCooldownHours + entity.World.Rand.NextDouble() * (MaxCooldownHours - MinCooldownHours);
		}
		pathTraverser.Stop();
		if (eatAnimMeta != null)
		{
			entity.AnimManager.StopAnimation(eatAnimMeta.Code);
		}
		if (eatAnimMetaLooseItems != null)
		{
			entity.AnimManager.StopAnimation(eatAnimMetaLooseItems.Code);
		}
		if (animMeta != null)
		{
			entity.AnimManager.StopAnimation(animMeta.Code);
		}
		if (cancelled)
		{
			cooldownUntilTotalHours = 0.0;
		}
		if (quantityEaten < 1f)
		{
			cooldownUntilTotalHours = 0.0;
		}
		else
		{
			quantityEaten = 0f;
		}
	}

	private void OnStuck()
	{
		stuckatMs = entity.World.ElapsedMilliseconds;
		nowStuck = true;
		failedSeekTargets.TryGetValue(targetPoi, out var attempt);
		if (attempt == null)
		{
			attempt = (failedSeekTargets[targetPoi] = new FailedAttempt());
		}
		attempt.Count++;
		attempt.LastTryMs = world.ElapsedMilliseconds;
	}

	private void OnGoalReached()
	{
		pathTraverser.Active = true;
		failedSeekTargets.Remove(targetPoi);
	}
}
