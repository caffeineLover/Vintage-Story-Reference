using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskGotoEntityConversable : AiTaskBaseR
{
	protected bool stuck;

	protected float currentFollowTime;

	protected string animationCode = "walk";

	public Entity TargetEntity { get; }

	public float MoveSpeed { get; set; } = 0.02f;

	public float SeekingRange { get; set; } = 25f;

	public float MaxFollowTime { get; set; } = 60f;

	public float AllowedExtraDistance { get; set; }

	public bool Finished => !pathTraverser.Ready;

	public AiTaskGotoEntityConversable(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		world.Logger.Error("This AI task 'AiTaskGotoEntityConversable' can only be created from code.");
		throw new InvalidOperationException("This AI task can only be created from code.");
	}

	public AiTaskGotoEntityConversable(EntityAgent entity, Entity target)
		: base(entity)
	{
		TargetEntity = target;
		baseConfig.AnimationMeta = new AnimationMetaData
		{
			Code = animationCode,
			Animation = animationCode,
			AnimationSpeed = 1f
		}.Init();
	}

	public override bool ShouldExecute()
	{
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		stuck = false;
		pathTraverser.NavigateTo_Async(TargetEntity.Pos.XYZ, MoveSpeed, MinDistanceToTarget(), OnGoalReached, OnStuck, null, 999);
		currentFollowTime = 0f;
	}

	public override bool CanContinueExecute()
	{
		return pathTraverser.Ready;
	}

	public override bool ContinueExecute(float dt)
	{
		currentFollowTime += dt;
		pathTraverser.CurrentTarget.X = TargetEntity.Pos.X;
		pathTraverser.CurrentTarget.Y = TargetEntity.Pos.Y;
		pathTraverser.CurrentTarget.Z = TargetEntity.Pos.Z;
		Cuboidd cuboidd = TargetEntity.SelectionBox.ToDouble().Translate(TargetEntity.Pos.X, TargetEntity.Pos.Y, TargetEntity.Pos.Z);
		Vec3d pos = entity.Pos.XYZ.Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		double distance = cuboidd.ShortestDistanceFrom(pos);
		float minDistance = MinDistanceToTarget();
		if (currentFollowTime < MaxFollowTime && distance < (double)(SeekingRange * SeekingRange) && distance > (double)minDistance)
		{
			return !stuck;
		}
		return false;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		pathTraverser.Stop();
	}

	public virtual bool TargetReached()
	{
		Cuboidd cuboidd = TargetEntity.SelectionBox.ToDouble().Translate(TargetEntity.Pos.X, TargetEntity.Pos.Y, TargetEntity.Pos.Z);
		Vec3d pos = entity.Pos.XYZ.Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		double num = cuboidd.ShortestDistanceFrom(pos);
		float minDistance = MinDistanceToTarget();
		return num < (double)minDistance;
	}

	protected virtual void OnStuck()
	{
		stuck = true;
	}

	protected virtual void OnGoalReached()
	{
		pathTraverser.Active = true;
	}

	protected virtual float MinDistanceToTarget()
	{
		return AllowedExtraDistance + Math.Max(0.8f, TargetEntity.SelectionBox.XSize / 2f + entity.SelectionBox.XSize / 2f);
	}
}
