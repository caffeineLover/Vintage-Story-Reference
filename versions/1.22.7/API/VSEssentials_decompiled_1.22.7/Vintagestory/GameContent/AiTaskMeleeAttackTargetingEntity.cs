using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class AiTaskMeleeAttackTargetingEntity : AiTaskMeleeAttack
{
	private Entity? guardedEntity;

	private Entity? lastattackingEntity;

	private long lastattackingEntityFoundMs;

	public AiTaskMeleeAttackTargetingEntity(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
	}

	public override bool ShouldExecute()
	{
		if (entity.World.Rand.NextDouble() < 0.1)
		{
			guardedEntity = GetGuardedEntity();
		}
		if (guardedEntity == null)
		{
			return false;
		}
		if (entity.World.ElapsedMilliseconds - lastattackingEntityFoundMs > 30000)
		{
			lastattackingEntity = null;
		}
		if (attackedByEntity == guardedEntity)
		{
			attackedByEntity = null;
		}
		return base.ShouldExecute();
	}

	public override void StartExecute()
	{
		base.StartExecute();
		lastattackingEntityFoundMs = entity.World.ElapsedMilliseconds;
		lastattackingEntity = targetEntity;
	}

	public override bool CanSense(Entity e, double range)
	{
		if (!base.CanSense(e, range))
		{
			return false;
		}
		if (e == guardedEntity)
		{
			return false;
		}
		IAiTask[] tasks = e.GetBehavior<EntityBehaviorTaskAI>()?.TaskManager.ActiveTasksBySlot;
		if (e != lastattackingEntity || !e.Alive)
		{
			return tasks?.FirstOrDefault((IAiTask task) => task is AiTaskBaseTargetable aiTaskBaseTargetable && aiTaskBaseTargetable.TargetEntity == guardedEntity && aiTaskBaseTargetable.AggressiveTargeting) != null;
		}
		return true;
	}
}
