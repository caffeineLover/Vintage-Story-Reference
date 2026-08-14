using System;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common.Entities;

public class PModuleOnGround : PModule
{
	private const long MinimumJumpInterval = 500L;

	private long lastJump;

	private double groundDragFactor = 0.30000001192092896;

	private float accum;

	private float coyoteTimer;

	private float antiCoyoteTimer;

	private double motionDeltaX;

	private double motionDeltaZ;

	public override void Initialize(JsonObject config, Entity entity)
	{
		if (config != null)
		{
			groundDragFactor = 0.3 * (double)(float)config["groundDragFactor"].AsDouble(1.0);
		}
	}

	public override bool Applicable(Entity entity, EntityPos pos, EntityControls controls)
	{
		bool onGround = entity.OnGround && !entity.Swimming;
		if (onGround && antiCoyoteTimer <= 0f)
		{
			coyoteTimer = 0.15f;
		}
		if (coyoteTimer > 0f)
		{
			if (entity.Attributes.GetInt("dmgkb") > 0)
			{
				coyoteTimer = 0f;
				antiCoyoteTimer = 0.16f;
				return onGround;
			}
			return true;
		}
		return onGround;
	}

	public override void DoApply(float dt, Entity entity, EntityPos pos, EntityControls controls)
	{
		coyoteTimer -= dt;
		if (antiCoyoteTimer > 0f)
		{
			antiCoyoteTimer = Math.Max(0f, antiCoyoteTimer - dt);
		}
		float belowBlockDragMultiplier = entity.World.BlockAccessor.GetBlockRaw((int)pos.X, (int)(pos.InternalY - 0.05000000074505806), (int)pos.Z).DragMultiplier;
		float accum = Math.Min(1f, this.accum + dt);
		float frameTime = 1f / 60f;
		double multiplier = (entity as EntityAgent).GetWalkSpeedMultiplier(groundDragFactor);
		double walkX = controls.WalkVector.X * multiplier;
		double walkZ = controls.WalkVector.Z * multiplier;
		Vec3d motion = pos.Motion;
		double groundDrag = 1.0 - groundDragFactor;
		while (accum > frameTime)
		{
			accum -= frameTime;
			if (entity.Alive)
			{
				motionDeltaX += (walkX - motionDeltaX) * (double)belowBlockDragMultiplier;
				motionDeltaZ += (walkZ - motionDeltaZ) * (double)belowBlockDragMultiplier;
				motion.Add(motionDeltaX, 0.0, motionDeltaZ);
			}
			motion.X *= groundDrag;
			motion.Z *= groundDrag;
		}
		this.accum = accum;
		if (controls.Jump && entity.World.ElapsedMilliseconds - lastJump > 500 && entity.Alive)
		{
			EntityPlayer entityPlayer = entity as EntityPlayer;
			lastJump = entity.World.ElapsedMilliseconds;
			float jumpHeightMultiplier = MathF.Sqrt(MathF.Max(1f, entityPlayer?.Stats.GetBlended("jumpHeightMul") ?? 1f));
			pos.Motion.Y = GlobalConstants.BaseJumpForce * 1f / 60f * jumpHeightMultiplier;
			IPlayer player = entityPlayer?.World.PlayerByUid(entityPlayer.PlayerUID);
			entity.PlayEntitySound("jump", player);
		}
	}
}
