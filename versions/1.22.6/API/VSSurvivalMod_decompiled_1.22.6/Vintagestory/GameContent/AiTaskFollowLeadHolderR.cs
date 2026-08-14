using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class AiTaskFollowLeadHolderR : AiTaskBaseTargetableR
{
	protected ClothManager clothManager;

	protected EntityBehaviorRopeTieable? ropeTieableBehavior;

	protected long lastGoalReachedMs;

	protected ClothSystem? clothSystem;

	private AiTaskFollowLeadHolderConfig Config => GetConfig<AiTaskFollowLeadHolderConfig>();

	public AiTaskFollowLeadHolderR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		clothManager = entity.World.Api.ModLoader.GetModSystem<ClothManager>();
		ropeTieableBehavior = entity.GetBehavior<EntityBehaviorRopeTieable>();
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskFollowLeadHolderConfig>(entity, taskConfig, aiConfig);
	}

	public override void AfterInitialize()
	{
		base.AfterInitialize();
		ropeTieableBehavior = entity.GetBehavior<EntityBehaviorRopeTieable>();
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		if (GetOwnGeneration() < Config.MinGeneration)
		{
			return false;
		}
		if (entity.World.ElapsedMilliseconds - lastGoalReachedMs < Config.GoalReachedCooldownMs)
		{
			return false;
		}
		int[] clothIds = ropeTieableBehavior?.ClothIds?.value;
		if (clothIds == null)
		{
			return false;
		}
		int[] array = clothIds;
		foreach (int clothId in array)
		{
			clothSystem = clothManager.GetClothSystem(clothId);
			if (clothSystem == null)
			{
				continue;
			}
			ClothPoint point = GetPinnedToPoint(entity);
			if (point != null)
			{
				targetEntity = point.PinnedToEntity;
				if (targetEntity.Pos.DistanceTo(entity.Pos) < (double)Config.MaxDistanceToTarget)
				{
					return false;
				}
				if (!IsTargetableEntity(targetEntity, Config.SeekingRange))
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	public override void StartExecute()
	{
		if (targetEntity != null)
		{
			pathTraverser.WalkTowards(targetEntity.Pos.XYZ, Config.MoveSpeed, MinDistanceToTarget(Config.ExtraMinDistanceToTarget), OnGoalReached, OnStuck, Config.AiCreatureType);
			base.StartExecute();
		}
	}

	public override bool ContinueExecute(float dt)
	{
		if (targetEntity == null || clothSystem == null)
		{
			return false;
		}
		if (!ContinueExecute(dt))
		{
			return false;
		}
		float minDistance = MinDistanceToTarget();
		double num = targetEntity.Pos.DistanceTo(entity.Pos.XYZ);
		if (num > (double)Config.MaxDistanceToTarget)
		{
			pathTraverser.WalkTowards(targetEntity.Pos.XYZ, Config.MoveSpeed, minDistance, OnGoalReached, OnStuck, Config.AiCreatureType);
		}
		if (num < (double)minDistance)
		{
			return false;
		}
		if (GetPinnedToPoint(entity) == null)
		{
			return false;
		}
		return true;
	}

	protected virtual void OnGoalReached()
	{
		lastGoalReachedMs = entity.World.ElapsedMilliseconds;
	}

	protected virtual void OnStuck()
	{
	}

	protected virtual ClothPoint? GetPinnedToPoint(EntityAgent entity)
	{
		if (clothSystem == null)
		{
			return null;
		}
		if (clothSystem.FirstPoint.PinnedToEntity != null && clothSystem.FirstPoint.PinnedToEntity.EntityId != entity.EntityId)
		{
			return clothSystem.FirstPoint;
		}
		if (clothSystem.LastPoint.PinnedToEntity != null && clothSystem.LastPoint.PinnedToEntity.EntityId != entity.EntityId)
		{
			return clothSystem.LastPoint;
		}
		return null;
	}
}
