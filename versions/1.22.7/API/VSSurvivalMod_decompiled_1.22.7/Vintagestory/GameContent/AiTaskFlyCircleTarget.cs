using System;
using Newtonsoft.Json;
using VSEssentialsMod.Entity.AI.Task;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskFlyCircleTarget : AiTaskFlyCircle
{
	[JsonProperty]
	protected float seekingRangeVer = 25f;

	[JsonProperty]
	protected float seekingRangeHor = 25f;

	protected TimeSpan cooldownTime = TimeSpan.FromMilliseconds(1000L);

	protected TimeSpan targetRetentionTime = TimeSpan.FromSeconds(30L);

	protected int CurrentDimension => entity.Pos.Dimension;

	protected int TargetDimension => targetEntity?.Pos.Dimension ?? CurrentDimension;

	protected int OtherDimension
	{
		get
		{
			if (CurrentDimension != 0)
			{
				return 0;
			}
			return 2;
		}
	}

	public AiTaskFlyCircleTarget(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		cooldownTime = TimeSpan.FromMilliseconds(taskConfig["cooldownMs"].AsInt(1000));
		targetRetentionTime = TimeSpan.FromSeconds(taskConfig["targetRetentionTimeSec"].AsInt(30));
	}

	public override bool ShouldExecute()
	{
		long elapsedMs = entity.World.ElapsedMilliseconds;
		if (cooldownUntilMs > elapsedMs)
		{
			return false;
		}
		cooldownUntilMs = entity.World.ElapsedMilliseconds + (long)cooldownTime.TotalMilliseconds;
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		Vec3d pos = entity.Pos.XYZ.Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		if (entity.World.ElapsedMilliseconds - attackedByEntityMs > (long)targetRetentionTime.TotalMilliseconds)
		{
			attackedByEntity = null;
		}
		if (ShouldRetaliateForRange(15f))
		{
			targetEntity = attackedByEntity;
		}
		else
		{
			targetEntity = entity.World.GetNearestEntity(pos, seekingRangeHor, seekingRangeVer, (Entity e) => IsTargetableEntity(e, seekingRangeHor) && hasDirectContact(e, seekingRangeHor, seekingRangeVer));
			if (targetEntity == null)
			{
				pos.Y += dimensionOffset(CurrentDimension, OtherDimension);
				targetEntity = entity.World.GetNearestEntity(pos, seekingRangeHor, seekingRangeVer, (Entity e) => IsTargetableEntity(e, seekingRangeHor) && hasDirectContact(e, seekingRangeHor, seekingRangeVer));
			}
		}
		if (targetEntity != null)
		{
			return base.ShouldExecute();
		}
		return false;
	}

	public override void StartExecute()
	{
		timeSwitchToNormalWorld();
		base.StartExecute();
		CenterPos = targetEntity.Pos.XYZ;
		CenterPos.Y += dimensionOffset(TargetDimension, CurrentDimension);
	}

	public override bool ContinueExecute(float dt)
	{
		CenterPos = targetEntity.Pos.XYZ;
		CenterPos.Y += dimensionOffset(TargetDimension, CurrentDimension);
		return base.ContinueExecute(dt);
	}

	public override bool CanSensePlayer(EntityPlayer eplr, double range)
	{
		if (!friendlyTarget && AggressiveTargeting)
		{
			if (creatureHostility == EnumCreatureHostility.NeverHostile)
			{
				return false;
			}
			if (creatureHostility == EnumCreatureHostility.Passive && (bhEmo == null || (!IsInEmotionState("aggressiveondamage") && !IsInEmotionState("aggressivearoundentities"))))
			{
				return false;
			}
		}
		float rangeMul = eplr.Stats.GetBlended("animalSeekingRange");
		IPlayer player = eplr.Player;
		if (eplr.Controls.Sneak && eplr.OnGround)
		{
			rangeMul *= 0.6f;
		}
		EntityPos correctedToDimensionPos = eplr.Pos.Copy();
		correctedToDimensionPos.Dimension = CurrentDimension;
		if ((rangeMul == 1f || entity.Pos.DistanceTo(correctedToDimensionPos) < range * (double)rangeMul) && targetablePlayerMode(player))
		{
			return true;
		}
		return false;
	}

	protected double dimensionOffset(int fromDimension, int toDimension)
	{
		return (toDimension - fromDimension) * 32768;
	}

	protected override bool hasDirectContact(Entity targetEntity, float minDist, float minVerDist)
	{
		EntityPos correctedToDimensionPos = targetEntity.Pos.Copy();
		correctedToDimensionPos.Dimension = entity.Pos.Dimension;
		Cuboidd cuboidd = targetEntity.SelectionBox.ToDouble().Translate(targetEntity.Pos.X, targetEntity.Pos.Y, targetEntity.Pos.Z);
		tmpPos.Set(entity.Pos).Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		double dist = cuboidd.ShortestDistanceFrom(tmpPos);
		double vertDist = Math.Abs(cuboidd.ShortestVerticalDistanceFrom(tmpPos.Y));
		if (dist >= (double)minDist || vertDist >= (double)minVerDist)
		{
			return false;
		}
		rayTraceFrom.Set(entity.Pos);
		rayTraceFrom.Y += 1.0 / 32.0;
		rayTraceTo.Set(correctedToDimensionPos);
		rayTraceTo.Y += 1.0 / 32.0;
		bool directContact = false;
		entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref blockSel, ref entitySel);
		directContact = blockSel == null;
		if (!directContact)
		{
			rayTraceFrom.Y += entity.SelectionBox.Y2 * 7f / 16f;
			rayTraceTo.Y += targetEntity.SelectionBox.Y2 * 7f / 16f;
			entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref blockSel, ref entitySel);
			directContact = blockSel == null;
		}
		if (!directContact)
		{
			rayTraceFrom.Y += entity.SelectionBox.Y2 * 7f / 16f;
			rayTraceTo.Y += targetEntity.SelectionBox.Y2 * 7f / 16f;
			entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref blockSel, ref entitySel);
			directContact = blockSel == null;
		}
		if (!directContact)
		{
			return false;
		}
		return true;
	}

	protected void timeSwitchToNormalWorld()
	{
		Timeswitch system = entity.Api.ModLoader.GetModSystem<Timeswitch>();
		if (entity.Pos.Dimension != 0)
		{
			(entity as EntityErel)?.ChangeDimension(0);
			system?.ChangeEntityDimensionOnClient(entity, 0);
		}
	}
}
