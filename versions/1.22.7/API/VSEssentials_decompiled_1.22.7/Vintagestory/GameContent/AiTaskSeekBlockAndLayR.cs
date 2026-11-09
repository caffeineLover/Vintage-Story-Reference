using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskSeekBlockAndLayR : AiTaskBaseR
{
	protected class FailedAttempt
	{
		public long LastTryMs;

		public int Count;
	}

	protected POIRegistry porregistry;

	protected IAnimalNest targetPoi;

	protected float moveSpeed = 0.02f;

	protected bool nowStuck;

	protected bool laid;

	protected float sitDays = 1f;

	protected float layTime = 1f;

	protected double incubationDays = 5.0;

	protected string[] chickCodes;

	protected string[] nestTypes;

	protected double onGroundChance = 0.3;

	protected AssetLocation failBlockCode;

	protected float sitTimeNow;

	protected double sitEndDay;

	protected bool sitAnimStarted;

	protected float PortionsEatenForLay;

	protected string requiresNearbyEntityCode;

	protected float requiresNearbyEntityRange = 5f;

	protected AnimationMetaData sitAnimMeta;

	private Dictionary<IAnimalNest, FailedAttempt> failedSeekTargets = new Dictionary<IAnimalNest, FailedAttempt>();

	protected long lastPOISearchTotalMs;

	protected double attemptLayEggTotalHours;

	public AiTaskSeekBlockAndLayR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		porregistry = entity.Api.ModLoader.GetModSystem<POIRegistry>();
		entity.WatchedAttributes.SetBool("doesSit", value: true);
		moveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
		sitDays = taskConfig["sitDays"].AsFloat(1f);
		layTime = taskConfig["layTime"].AsFloat(1.5f);
		incubationDays = taskConfig["incubationDays"].AsDouble(5.0);
		if (taskConfig["sitAnimation"].Exists)
		{
			sitAnimMeta = new AnimationMetaData
			{
				Code = taskConfig["sitAnimation"].AsString()?.ToLowerInvariant(),
				Animation = taskConfig["sitAnimation"].AsString()?.ToLowerInvariant(),
				AnimationSpeed = taskConfig["sitAnimationSpeed"].AsFloat(1f)
			}.Init();
		}
		chickCodes = taskConfig["chickCodes"].AsArray<string>();
		if (chickCodes == null)
		{
			chickCodes = new string[1] { taskConfig["chickCode"].AsString() };
		}
		nestTypes = taskConfig["nestTypes"].AsArray<string>();
		PortionsEatenForLay = taskConfig["portionsEatenForLay"].AsFloat(3f);
		requiresNearbyEntityCode = taskConfig["requiresNearbyEntityCode"].AsString();
		requiresNearbyEntityRange = taskConfig["requiresNearbyEntityRange"].AsFloat(5f);
		string code = taskConfig["failBlockCode"].AsString();
		if (code != null)
		{
			failBlockCode = new AssetLocation(code);
		}
	}

	public override bool ShouldExecute()
	{
		if (entity.World.Rand.NextDouble() > 0.03)
		{
			return false;
		}
		if (lastPOISearchTotalMs + 15000 > entity.World.ElapsedMilliseconds)
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
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		PortionsEatenForLay = 3f;
		if (!DidConsumeFood(PortionsEatenForLay))
		{
			return false;
		}
		if (attemptLayEggTotalHours <= 0.0)
		{
			attemptLayEggTotalHours = entity.World.Calendar.TotalHours;
		}
		lastPOISearchTotalMs = entity.World.ElapsedMilliseconds;
		targetPoi = FindPOI(42) as IAnimalNest;
		if (targetPoi == null)
		{
			LayEggOnGround();
		}
		return targetPoi != null;
	}

	protected IPointOfInterest FindPOI(int radius)
	{
		return porregistry.GetWeightedNearestPoi(entity.Pos.XYZ, radius, delegate(IPointOfInterest poi)
		{
			if (poi.Type != "nest")
			{
				return false;
			}
			if (poi is IAnimalNest animalNest && !animalNest.Occupied(entity) && animalNest.IsSuitableFor(entity, nestTypes))
			{
				failedSeekTargets.TryGetValue(animalNest, out var value);
				if (value == null || value.Count < 4 || value.LastTryMs < world.ElapsedMilliseconds - 60000)
				{
					return true;
				}
			}
			return false;
		});
	}

	public float MinDistanceToTarget()
	{
		return 0.01f;
	}

	public override void StartExecute()
	{
		if (baseConfig.AnimationMeta != null)
		{
			baseConfig.AnimationMeta.EaseInSpeed = 1f;
			baseConfig.AnimationMeta.EaseOutSpeed = 1f;
			entity.AnimManager.StartAnimation(baseConfig.AnimationMeta);
		}
		nowStuck = false;
		sitTimeNow = 0f;
		laid = false;
		pathTraverser.NavigateTo_Async(targetPoi.Position, moveSpeed, MinDistanceToTarget() - 0.1f, OnGoalReached, OnStuck, null, 1000, 1);
		sitAnimStarted = false;
	}

	public override bool CanContinueExecute()
	{
		return pathTraverser.Ready;
	}

	protected virtual ItemStack MakeEggItem(string chickCode)
	{
		ICoreAPI api = entity.Api;
		JsonItemStack[] eggTypes = entity?.Properties.Attributes?["eggTypes"].AsArray<JsonItemStack>();
		ItemStack eggStack;
		if (eggTypes == null)
		{
			string fallbackCode = "egg-chicken-raw";
			if (entity != null)
			{
				api.Logger.Warning(string.Concat("No egg type specified for entity ", entity.Code, ", falling back to ", fallbackCode));
			}
			eggStack = new ItemStack(api.World.GetItem(fallbackCode));
		}
		else
		{
			JsonItemStack jsonEgg = eggTypes[api.World.Rand.Next(eggTypes.Length)];
			if (!jsonEgg.Resolve(api.World, null, printWarningOnError: false))
			{
				api.Logger.Warning(string.Concat(new string[6]
				{
					"Failed to resolve egg ",
					jsonEgg.Type.ToString(),
					" with code ",
					jsonEgg.Code,
					" for entity ",
					entity.Code
				}));
				return null;
			}
			eggStack = new ItemStack(jsonEgg.ResolvedItemstack.Collectible);
		}
		if (chickCode != null)
		{
			TreeAttribute chickTree = new TreeAttribute();
			chickCode = AssetLocation.Create(chickCode, entity?.Code.Domain ?? "game");
			chickTree.SetString("code", chickCode);
			EntityAgent entityAgent = entity;
			chickTree.SetInt("generation", (entityAgent != null) ? (entityAgent.WatchedAttributes.GetInt("generation") + 1) : 0);
			chickTree.SetDouble("incubationDays", incubationDays);
			EntityAgent eagent = entity;
			if (eagent != null)
			{
				chickTree.SetLong("herdID", eagent.HerdId);
			}
			eggStack.Attributes["chick"] = chickTree;
		}
		return eggStack;
	}

	public override bool ContinueExecute(float dt)
	{
		if (targetPoi.Occupied(entity))
		{
			onBadTarget();
			return false;
		}
		Vec3d pos = targetPoi.Position;
		double num = pos.HorizontalSquareDistanceTo(entity.Pos.X, entity.Pos.Z);
		pathTraverser.CurrentTarget.X = pos.X;
		pathTraverser.CurrentTarget.Y = pos.Y;
		pathTraverser.CurrentTarget.Z = pos.Z;
		float minDist = MinDistanceToTarget();
		if (num <= (double)minDist)
		{
			pathTraverser.Stop();
			if (baseConfig.AnimationMeta != null)
			{
				entity.AnimManager.StopAnimation(baseConfig.AnimationMeta.Code);
			}
			entity.GetBehavior<EntityBehaviorMultiply>();
			if (!targetPoi.IsSuitableFor(entity, nestTypes))
			{
				onBadTarget();
				return false;
			}
			targetPoi.SetOccupier(entity);
			if (sitAnimMeta != null && !sitAnimStarted)
			{
				entity.AnimManager.StartAnimation(sitAnimMeta);
				sitAnimStarted = true;
				sitEndDay = entity.World.Calendar.TotalDays + (double)sitDays;
			}
			sitTimeNow += dt;
			if (sitTimeNow >= layTime && !laid)
			{
				laid = true;
				string chickCode = null;
				if (GetRequiredEntityNearby() != null && chickCodes.Length != 0)
				{
					chickCode = chickCodes[entity.World.Rand.Next(chickCodes.Length)];
				}
				if (targetPoi.TryAddEgg(MakeEggItem(chickCode)))
				{
					ConsumeFood(PortionsEatenForLay);
					attemptLayEggTotalHours = -1.0;
					MakeLaySound();
					failedSeekTargets.Remove(targetPoi);
					return false;
				}
			}
			if (entity.World.Calendar.TotalDays >= sitEndDay)
			{
				failedSeekTargets.Remove(targetPoi);
				return false;
			}
		}
		else if (!pathTraverser.Active)
		{
			float rndx = (float)entity.World.Rand.NextDouble() * 0.3f - 0.15f;
			float rndz = (float)entity.World.Rand.NextDouble() * 0.3f - 0.15f;
			pathTraverser.NavigateTo(targetPoi.Position.AddCopy(rndx, 0f, rndz), moveSpeed, MinDistanceToTarget() - 0.15f, OnGoalReached, OnStuck, null, giveUpWhenNoPath: false, 500);
		}
		if (nowStuck)
		{
			return false;
		}
		if (attemptLayEggTotalHours > 0.0 && entity.World.Calendar.TotalHours - attemptLayEggTotalHours > 12.0)
		{
			LayEggOnGround();
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		attemptLayEggTotalHours = -1.0;
		pathTraverser.Stop();
		if (sitAnimMeta != null)
		{
			entity.AnimManager.StopAnimation(sitAnimMeta.Code);
		}
		targetPoi?.SetOccupier(null);
		if (cancelled)
		{
			cooldownUntilTotalHours = 0.0;
		}
	}

	protected void OnStuck()
	{
		nowStuck = true;
		onBadTarget();
	}

	protected void onBadTarget()
	{
		IAnimalNest newTarget = null;
		if (attemptLayEggTotalHours >= 0.0 && entity.World.Calendar.TotalHours - attemptLayEggTotalHours > 12.0)
		{
			LayEggOnGround();
		}
		else if (base.Rand.NextDouble() > 0.4)
		{
			newTarget = FindPOI(18) as IAnimalNest;
		}
		failedSeekTargets.TryGetValue(targetPoi, out var attempt);
		if (attempt == null)
		{
			attempt = (failedSeekTargets[targetPoi] = new FailedAttempt());
		}
		attempt.Count++;
		attempt.LastTryMs = world.ElapsedMilliseconds;
		if (newTarget != null)
		{
			targetPoi = newTarget;
			nowStuck = false;
			sitTimeNow = 0f;
			laid = false;
			pathTraverser.NavigateTo_Async(targetPoi.Position, moveSpeed, MinDistanceToTarget() - 0.1f, OnGoalReached, OnStuck, null, 1000, 1);
			sitAnimStarted = false;
		}
	}

	protected void OnGoalReached()
	{
		pathTraverser.Active = true;
		failedSeekTargets.Remove(targetPoi);
	}

	protected bool DidConsumeFood(float portion)
	{
		ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("hunger");
		if (tree == null)
		{
			return false;
		}
		return tree.GetFloat("saturation") >= portion;
	}

	protected bool ConsumeFood(float portion)
	{
		ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("hunger");
		if (tree == null)
		{
			return false;
		}
		float saturation = tree.GetFloat("saturation");
		if (saturation >= portion)
		{
			float portionEaten = ((entity.World.Rand.NextDouble() < 0.25) ? portion : 1f);
			tree.SetFloat("saturation", saturation - portionEaten);
			return true;
		}
		return false;
	}

	protected Entity GetRequiredEntityNearby()
	{
		if (requiresNearbyEntityCode == null)
		{
			return null;
		}
		return entity.World.GetNearestEntity(entity.Pos.XYZ, requiresNearbyEntityRange, requiresNearbyEntityRange, delegate(Entity e)
		{
			if (e.Alive && e.WildCardMatch(new AssetLocation(requiresNearbyEntityCode)))
			{
				ITreeAttribute treeAttribute = e.WatchedAttributes.GetTreeAttribute("hunger");
				if (!e.WatchedAttributes.GetBool("doesEat") || treeAttribute == null)
				{
					return true;
				}
				treeAttribute.SetFloat("saturation", Math.Max(0f, treeAttribute.GetFloat("saturation") - 1f));
				return true;
			}
			return false;
		});
	}

	protected void LayEggOnGround()
	{
		if (!(entity.World.Rand.NextDouble() > onGroundChance))
		{
			Block block = entity.World.GetBlock(failBlockCode);
			if (block != null && (TryPlace(block, 0, 0, 0) || TryPlace(block, 1, 0, 0) || TryPlace(block, 0, 0, -1) || TryPlace(block, -1, 0, 0) || TryPlace(block, 0, 0, 1)))
			{
				ConsumeFood(PortionsEatenForLay);
				attemptLayEggTotalHours = -1.0;
			}
		}
	}

	protected bool TryPlace(Block block, int dx, int dy, int dz)
	{
		IBlockAccessor blockAccess = entity.World.BlockAccessor;
		BlockPos pos = entity.Pos.XYZ.AsBlockPos.Add(dx, dy, dz);
		if (blockAccess.GetBlock(pos, 2).IsLiquid())
		{
			return false;
		}
		if (!blockAccess.GetBlock(pos).IsReplacableBy(block))
		{
			return false;
		}
		pos.Y--;
		if (blockAccess.GetMostSolidBlock(pos).CanAttachBlockAt(blockAccess, block, pos, BlockFacing.UP))
		{
			pos.Y++;
			blockAccess.SetBlock(block.BlockId, pos);
			BlockEntityTransient obj = blockAccess.GetBlockEntity(pos) as BlockEntityTransient;
			obj?.SetPlaceTime(entity.World.Calendar.TotalHours);
			if (obj != null && obj.IsDueTransition())
			{
				blockAccess.SetBlock(0, pos);
			}
			return true;
		}
		return false;
	}

	protected void MakeLaySound()
	{
		if (baseConfig.Sound == null)
		{
			return;
		}
		if (baseConfig.SoundStartMs > 0)
		{
			entity.World.RegisterCallback(delegate
			{
				entity.World.PlaySoundAt(baseConfig.Sound, entity, null, randomizePitch: true, baseConfig.SoundRange);
				lastSoundTotalMs = entity.World.ElapsedMilliseconds;
			}, baseConfig.SoundStartMs);
		}
		else
		{
			entity.World.PlaySoundAt(baseConfig.Sound, entity, null, randomizePitch: true, baseConfig.SoundRange);
			lastSoundTotalMs = entity.World.ElapsedMilliseconds;
		}
	}
}
