using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityFish : EntityAgent
{
	protected AiTaskManager? taskManager;

	public double NoBaitBobberSeekChance { get; set; } = 0.02;

	public double BaitBobberSeekChance { get; set; } = 0.5;

	public double FleeAwaySpeed { get; set; } = 0.5;

	static EntityFish()
	{
		AiTaskRegistry.Register<AiTaskFishMoveFast>("fishmovefast");
		AiTaskRegistry.Register<AiTaskFishOutOfWater>("fishoutofwater");
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		base.AfterInitialized(onFirstSpawn);
		if (Api.Side == EnumAppSide.Server)
		{
			taskManager = GetBehavior<EntityBehaviorTaskAI>()?.TaskManager;
			if (taskManager?.GetTask("seekbobber") is AiTaskSeekEntity seekBobberTask)
			{
				seekBobberTask.OnIsSuitableTarget = bobberBaitCheck;
			}
		}
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
	{
		if (mode != EnumInteractMode.Attack || !Swimming)
		{
			base.OnInteract(byEntity, slot, hitPosition, mode);
		}
		else if (World.Side == EnumAppSide.Server)
		{
			TryFleeAway();
		}
	}

	public virtual void BoltForward(double speed)
	{
		base.Pos.Motion.X = Math.Sin(base.Pos.Yaw) * speed;
		base.Pos.Motion.Z = Math.Cos(base.Pos.Yaw) * speed;
	}

	public virtual bool TryFleeAway()
	{
		if (taskManager == null)
		{
			return false;
		}
		taskManager.ExecuteTask<AiTaskFleeEntity>();
		if (taskManager.IsTaskActive<AiTaskFleeEntity>())
		{
			BoltForward(FleeAwaySpeed);
			return true;
		}
		return false;
	}

	protected virtual bool bobberBaitCheck(Entity entity)
	{
		if ((entity as EntityBobber)?.BaitStack == null)
		{
			return World.Rand.NextDouble() < NoBaitBobberSeekChance;
		}
		return World.Rand.NextDouble() < BaitBobberSeekChance;
	}
}
