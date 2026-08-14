using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class EntityProjectile : EntityProjectileBase, IProjectile
{
	public override bool ApplyGravity => !base.Stuck;

	public override bool IsInteractable => false;

	void IProjectile.PreInitialize()
	{
		base.Collectible = true;
		SetInitialRotation();
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
		collisionTestBox = SelectionBox.Clone().OmniGrowBy(0.05f);
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if (ShouldDespawn)
		{
			return;
		}
		EntityPos pos = base.Pos;
		base.Stuck = base.Collided || collTester.IsColliding(World.BlockAccessor, collisionTestBox, pos.XYZ) || WatchedAttributes.GetBool("stuck");
		if (Api.Side == EnumAppSide.Server)
		{
			WatchedAttributes.SetBool("stuck", base.Stuck);
		}
		double impactSpeed = Math.Max(motionBeforeCollide.Length(), pos.Motion.Length());
		if (base.Stuck)
		{
			IsColliding(pos, impactSpeed);
			entitiesHit.Clear();
			return;
		}
		SetRotationFromMotion();
		if (!TryAttackEntity(impactSpeed))
		{
			beforeCollided = false;
			motionBeforeCollide.Set(pos.Motion.X, pos.Motion.Y, pos.Motion.Z);
		}
	}

	public override void OnCollided()
	{
		EntityPos pos = base.Pos;
		IsColliding(base.Pos, Math.Max(motionBeforeCollide.Length(), pos.Motion.Length()));
		motionBeforeCollide.Set(pos.Motion.X, pos.Motion.Y, pos.Motion.Z);
	}

	protected virtual void IsColliding(EntityPos pos, double impactSpeed)
	{
		pos.Motion.Set(0.0, 0.0, 0.0);
		if (beforeCollided || !(World is IServerWorldAccessor) || World.ElapsedMilliseconds <= msCollide + 500)
		{
			return;
		}
		if (impactSpeed >= 0.07)
		{
			World.PlaySoundAt(impactSound, this, null, randomizePitch: false);
			WatchedAttributes.MarkAllDirty();
			if (base.DamageStackOnImpact)
			{
				base.ProjectileStack.Collectible.DamageItem(World, this, new DummySlot(base.ProjectileStack));
				if (((base.ProjectileStack == null || base.ProjectileStack.Collectible.GetRemainingDurability(base.ProjectileStack) != 0) ? 1 : 0) <= (false ? 1 : 0))
				{
					Die();
				}
			}
		}
		TryAttackEntity(impactSpeed);
		msCollide = World.ElapsedMilliseconds;
		beforeCollided = true;
	}

	public virtual void SetInitialRotation()
	{
		EntityPos pos = base.Pos;
		double speed = pos.Motion.Length();
		if (speed > 0.01)
		{
			pos.Pitch = 0f;
			pos.Yaw = (float)Math.PI + (float)Math.Atan2(pos.Motion.X / speed, pos.Motion.Z / speed);
			pos.Roll = 0f - (float)Math.Asin(GameMath.Clamp((0.0 - pos.Motion.Y) / speed, -1.0, 1.0));
		}
	}

	public virtual void SetRotationFromMotion()
	{
		EntityPos pos = base.Pos;
		double speed = pos.Motion.Length();
		if (speed > 0.01)
		{
			pos.Pitch = 0f;
			pos.Yaw = (float)Math.PI + (float)Math.Atan2(pos.Motion.X / speed, pos.Motion.Z / speed) + GameMath.Cos((float)(World.ElapsedMilliseconds - msLaunch) / 200f) * 0.03f;
			pos.Roll = 0f - (float)Math.Asin(GameMath.Clamp((0.0 - pos.Motion.Y) / speed, -1.0, 1.0)) + GameMath.Sin((float)(World.ElapsedMilliseconds - msLaunch) / 200f) * 0.03f;
		}
	}
}
