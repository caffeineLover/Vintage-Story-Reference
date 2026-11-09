using System;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

/// <summary>
/// Passive physics system for entities.
/// <br />Uses the "passivephysics" code
/// </summary>
/// <example><code lang="json">
/// "behaviors": [
///  {
///     "code": "passivephysics"
///     "groundDragFactor": 1,
///     "airDragFactor": 0.25,
///     "gravityFactor": 0.75
///  }
/// ]
/// </code></example>
[DocumentAsJson]
[AddDocumentationProperty("waterDragFactor", "Gravity drag factor when in water", "System.Double", "Optional", "1", false)]
[AddDocumentationProperty("airDragFactor", "Gravity drag factor when falling. Overrides airDragFallingFactor when present", "System.Double", "Optional", "1", false)]
[AddDocumentationProperty("airDragFallingFactor", "Gravity drag factor when falling", "System.Double", "Optional", "1", false)]
[AddDocumentationProperty("groundDragFactor", "Horizontal drag factor when on the ground", "System.Double", "Optional", "1", false)]
[AddDocumentationProperty("gravityFactor", "Multiplier for gravity strength", "System.Double", "Optional", "1", false)]
public class EntityBehaviorPassivePhysics : PhysicsBehaviorBase, IPhysicsTickable, IRemotePhysics
{
	protected readonly Vec3d prevPos = new Vec3d();

	protected double motionBeforeY;

	protected bool feetInLiquidBefore;

	protected bool onGroundBefore;

	protected bool swimmingBefore;

	protected bool collidedBefore;

	protected Vec3d newPos = new Vec3d();

	/// <summary>
	/// The amount of drag while travelling through water.
	/// </summary>
	public double WaterDragValue = GlobalConstants.WaterDrag;

	/// <summary>
	/// The amount of drag while travelling through the air.
	/// </summary>
	public double AirDragValue = GlobalConstants.AirDragAlways;

	/// <summary>
	/// The amount of drag while travelling on the ground.
	/// </summary>
	public double GroundDragValue = 0.699999988079071;

	/// <summary>
	/// The amount of drag while travelling on the ground.
	/// </summary>
	public double BoyancyMul = 1.0;

	/// <summary>
	/// The amount of gravity applied per tick to this entity.
	/// </summary>
	public double GravityPerSecond = GlobalConstants.GravityPerSecond;

	/// <summary>
	/// If set, will test for entity collision every tick (expensive)
	/// </summary>
	public Action<float> OnPhysicsTickCallback;

	/// <summary>
	/// True when theres 1000 currently loaded entities. This will cause dropped items to enter a physics dormancy state if the conditions are right. Set by ModSystemDormancyStateChecker in EntityItemRenderer.cs
	/// </summary>
	public static bool UsePhysicsDormancyStateServer;

	/// <summary>
	/// Used for performance, to put near-stationary passive entities (eg. EntityItem) into a low update state
	/// </summary>
	protected int restingCounter;

	[ThreadStatic]
	private static BlockPos tmpPos;

	public Entity Entity => entity;

	public bool Ticking { get; set; } = true;

	public EntityBehaviorPassivePhysics(Entity entity)
		: base(entity)
	{
	}

	public void SetState(EntityPos pos)
	{
		prevPos.Set(pos);
		motionBeforeY = pos.Motion.Y;
		Entity entity = base.entity;
		onGroundBefore = entity.OnGround;
		feetInLiquidBefore = entity.FeetInLiquid;
		swimmingBefore = entity.Swimming;
		collidedBefore = entity.Collided;
	}

	public virtual void SetProperties(JsonObject attributes)
	{
		WaterDragValue = 1.0 - (1.0 - WaterDragValue) * attributes["waterDragFactor"].AsDouble(1.0);
		JsonObject airDragFactor = attributes["airDragFactor"];
		double airDrag = (airDragFactor.Exists ? airDragFactor.AsDouble(1.0) : attributes["airDragFallingFactor"].AsDouble(1.0));
		AirDragValue = 1.0 - (1.0 - AirDragValue) * airDrag;
		if (entity.WatchedAttributes.HasAttribute("airDragFactor"))
		{
			AirDragValue = 1f - (1f - GlobalConstants.AirDragAlways) * (float)entity.WatchedAttributes.GetDouble("airDragFactor");
		}
		GroundDragValue = 0.3 * attributes["groundDragFactor"].AsDouble(1.0);
		GravityPerSecond *= attributes["gravityFactor"].AsDouble(1.0);
		BoyancyMul = attributes["boyancyMul"].AsDouble(1.0);
		if (entity.WatchedAttributes.HasAttribute("gravityFactor"))
		{
			GravityPerSecond = GlobalConstants.GravityPerSecond * (float)entity.WatchedAttributes.GetDouble("gravityFactor");
		}
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		Init();
		SetProperties(attributes);
		if (entity.Api is ICoreServerAPI esapi)
		{
			esapi.Server.AddPhysicsTickable(this);
			return;
		}
		EnumHandling handling = EnumHandling.Handled;
		OnReceivedServerPos(isTeleport: true, ref handling);
	}

	public void OnReceivedClientPos(int version)
	{
		if (version > previousVersion)
		{
			previousVersion = version;
			HandleRemotePhysics(1f / 15f, isTeleport: true);
		}
		else
		{
			HandleRemotePhysics(1f / 15f, isTeleport: false);
		}
	}

	public void HandleRemotePhysics(float dt, bool isTeleport)
	{
		if (nPos == null)
		{
			nPos = new Vec3d();
			nPos.Set(entity.Pos);
		}
		float dtFactor = dt * 60f;
		EntityPos lPos = base.lPos;
		lPos.SetFrom(nPos);
		nPos.Set(entity.Pos);
		Vec3d lPosMotion = lPos.Motion;
		if (isTeleport)
		{
			lPos.SetFrom(nPos);
		}
		lPosMotion.X = (nPos.X - lPos.X) / (double)dtFactor;
		lPosMotion.Y = (nPos.Y - lPos.Y) / (double)dtFactor;
		lPosMotion.Z = (nPos.Z - lPos.Z) / (double)dtFactor;
		if (lPosMotion.Length() > 20.0)
		{
			lPosMotion.Set(0.0, 0.0, 0.0);
		}
		entity.Pos.Motion.Set(lPosMotion);
		PhysicsBehaviorBase.collisionTester.NewTick(lPos);
		SetState(lPos);
		RemoteMotionAndCollision(lPos, dtFactor);
		ApplyTests(lPos);
	}

	public void RemoteMotionAndCollision(EntityPos pos, float dtFactor)
	{
		double gravityStrength = GravityPerSecond / 60.0 * (double)dtFactor + Math.Max(0.0, -0.014999999664723873 * pos.Motion.Y * (double)dtFactor);
		pos.Motion.Y -= gravityStrength;
		PhysicsBehaviorBase.collisionTester.ApplyTerrainCollision(entity, pos, dtFactor, ref newPos, 0f, CollisionYExtra);
		bool falling = pos.Motion.Y < 0.0;
		entity.OnGround = entity.CollidedVertically && falling;
		pos.Motion.Y += gravityStrength;
		pos.SetPos(nPos);
	}

	public void MotionAndCollision(EntityPos pos, float dt)
	{
		float dtFactor = 60f * dt;
		Entity entity = base.entity;
		Vec3d motion = pos.Motion;
		IBlockAccessor blockAccessor = entity.World.BlockAccessor;
		int dimension = pos.Dimension;
		if (onGroundBefore)
		{
			if (motion.HorLength() < 1E-05)
			{
				motion.X = 0.0;
				motion.Z = 0.0;
			}
			else if (!feetInLiquidBefore)
			{
				Block belowBlock = blockAccessor.GetBlockRaw((int)pos.X, (int)(pos.InternalY - 0.05000000074505806), (int)pos.Z, 1);
				double friction = 1.0 - GroundDragValue * (double)belowBlock.DragMultiplier;
				motion.X *= friction;
				motion.Z *= friction;
			}
		}
		Block insideFluid = null;
		if (feetInLiquidBefore || swimmingBefore)
		{
			motion.Scale(Math.Pow(WaterDragValue, dt * 33f));
			if ((object)tmpPos == null)
			{
				tmpPos = new BlockPos(pos.Dimension);
			}
			tmpPos.Set(pos);
			insideFluid = blockAccessor.GetBlock(tmpPos, 2);
			if (feetInLiquidBefore && insideFluid is IBlockFlowing { IsStill: false } blockFlowing)
			{
				float pushStrength = 300f / GameMath.Clamp(entity.MaterialDensity, 750f, 2500f) * dtFactor;
				FastVec3f pushVector = blockFlowing.GetPushVector(tmpPos);
				motion.Add(pushVector * pushStrength);
			}
		}
		else
		{
			motion.Scale((float)Math.Pow(AirDragValue, dt * 33f));
		}
		if (entity.ApplyGravity)
		{
			double gravityStrength = GravityPerSecond / 60.0 * (double)dtFactor;
			if (entity.Swimming)
			{
				float boyancy = GameMath.Clamp(1f - entity.MaterialDensity / (float)insideFluid.MaterialDensity, -1f, 1f);
				Block aboveFluid = blockAccessor.GetBlockRaw((int)pos.X, (int)(pos.InternalY + 1.0), (int)pos.Z, 2);
				float swimLineSubmergedness = GameMath.Clamp((float)(int)pos.Y + (float)insideFluid.LiquidLevel / 8f + (aboveFluid.IsLiquid() ? 1.125f : 0f) - (float)pos.Y - (entity.SelectionBox.Y2 - (float)entity.SwimmingOffsetY), 0f, 1f);
				double boyancyStrength = GameMath.Clamp(60f * boyancy * swimLineSubmergedness, -0.5f, 1.5f) - 1f;
				double waterDrag = GameMath.Clamp(10.0 * Math.Abs(motion.Length() * (double)dtFactor) - 0.019999999552965164, 1.0, 1.25);
				motion.Y += gravityStrength * boyancyStrength * BoyancyMul;
				motion.Mul(1.0 / waterDrag);
			}
			else
			{
				motion.Y -= gravityStrength;
			}
		}
		double nextX = motion.X * (double)dtFactor + pos.X;
		double nextY = motion.Y * (double)dtFactor + pos.Y;
		double nextZ = motion.Z * (double)dtFactor + pos.Z;
		applyCollision(pos, dtFactor);
		Vec3d newPos = this.newPos;
		if (blockAccessor.IsNotTraversable((int)nextX, (int)pos.Y, (int)pos.Z, dimension))
		{
			newPos.X = pos.X;
		}
		if (blockAccessor.IsNotTraversable((int)pos.X, (int)nextY, (int)pos.Z, dimension))
		{
			newPos.Y = pos.Y;
		}
		if (blockAccessor.IsNotTraversable((int)pos.X, (int)pos.Y, (int)nextZ, dimension))
		{
			newPos.Z = pos.Z;
		}
		pos.SetPos(newPos);
		if ((nextX < newPos.X && motion.X < 0.0) || (nextX > newPos.X && motion.X > 0.0))
		{
			motion.X = 0.0;
		}
		if ((nextY < newPos.Y && motion.Y < 0.0) || (nextY > newPos.Y && motion.Y > 0.0))
		{
			motion.Y = 0.0;
		}
		if ((nextZ < newPos.Z && motion.Z < 0.0) || (nextZ > newPos.Z && motion.Z > 0.0))
		{
			motion.Z = 0.0;
		}
	}

	protected virtual void applyCollision(EntityPos pos, float dtFactor)
	{
		PhysicsBehaviorBase.collisionTester.ApplyTerrainCollision(entity, pos, dtFactor, ref newPos, 0f, CollisionYExtra);
	}

	public void ApplyTests(EntityPos pos)
	{
		Entity entity = base.entity;
		IBlockAccessor blockAccessor = entity.World.BlockAccessor;
		bool falling = pos.Motion.Y <= 0.0;
		entity.OnGround = entity.CollidedVertically && falling;
		Block fluidBlock = blockAccessor.GetBlockRaw((int)pos.X, (int)pos.InternalY, (int)pos.Z, 2);
		entity.FeetInLiquid = fluidBlock.MatterState == EnumMatterState.Liquid;
		entity.InLava = fluidBlock.LiquidCode == "lava";
		if (entity.FeetInLiquid)
		{
			Block aboveBlockFluid = blockAccessor.GetBlockRaw((int)pos.X, (int)(pos.InternalY + 1.0), (int)pos.Z, 2);
			float swimlineSubmergedness = (float)(int)pos.Y + (float)fluidBlock.LiquidLevel / 8f + (aboveBlockFluid.IsLiquid() ? 1.125f : 0f) - (float)pos.Y - (entity.SelectionBox.Y2 - (float)entity.SwimmingOffsetY);
			entity.Swimming = swimlineSubmergedness > 0f;
			if (!feetInLiquidBefore && !(entity is EntityAgent { MountedOn: not null }) && !IsFirstTick(entity))
			{
				entity.OnCollideWithLiquid();
			}
		}
		else
		{
			entity.Swimming = false;
			if (swimmingBefore || feetInLiquidBefore)
			{
				entity.OnExitedLiquid();
			}
		}
		if (!collidedBefore && entity.Collided)
		{
			entity.OnCollided();
		}
		if (entity.OnGround)
		{
			if (!onGroundBefore)
			{
				entity.OnFallToGround(motionBeforeY);
			}
			entity.PositionBeforeFalling.Set(newPos);
		}
		if (GlobalConstants.OutsideWorld(pos.X, pos.Y, pos.Z, entity.World.BlockAccessor))
		{
			entity.DespawnReason = new EntityDespawnData
			{
				Reason = EnumDespawnReason.Death,
				DamageSourceForDeath = new DamageSource
				{
					Source = EnumDamageSource.Fall
				}
			};
			return;
		}
		Cuboidd entityBox = PhysicsBehaviorBase.collisionTester.entityBox;
		int xMax = (int)entityBox.X2;
		int yMax = (int)entityBox.Y2;
		int zMax = (int)entityBox.Z2;
		int zMin = (int)entityBox.Z1;
		BlockPos tmpPos = PhysicsBehaviorBase.collisionTester.tmpPos;
		tmpPos.SetDimension(entity.Pos.Dimension);
		for (int y = (int)entityBox.Y1; y <= yMax; y++)
		{
			for (int x = (int)entityBox.X1; x <= xMax; x++)
			{
				for (int z = zMin; z <= zMax; z++)
				{
					tmpPos.Set(x, y, z);
					blockAccessor.GetBlock(tmpPos).OnEntityInside(entity.World, entity, tmpPos);
				}
			}
		}
		OnPhysicsTickCallback?.Invoke(0f);
		entity.PhysicsUpdateWatcher?.Invoke(0f, prevPos);
	}

	public void OnPhysicsTick(float dt)
	{
		Entity entity = base.entity;
		if (entity.State != EnumEntityState.Active || !Ticking)
		{
			return;
		}
		EntityPos pos = entity.Pos;
		if (UsePhysicsDormancyStateServer)
		{
			if (entity.OnGround && entity.NearestPlayerDistance > 7f && pos.Motion.LengthSq() < 0.0001)
			{
				restingCounter++;
				if (restingCounter > 90 && restingCounter % 15 != 0)
				{
					return;
				}
			}
			else
			{
				restingCounter = 0;
			}
		}
		IMountable mountable = mountableSupplier;
		if (mountable == null || !mountable.IsBeingControlled() || entity.World.Side != EnumAppSide.Server)
		{
			PhysicsBehaviorBase.collisionTester.AssignToEntity(this, pos.Dimension);
			int loops = ((!(pos.Motion.Length() > 0.1)) ? 1 : 10);
			float newDt = dt / (float)loops;
			for (int i = 0; i < loops; i++)
			{
				SetState(pos);
				MotionAndCollision(pos, newDt);
				ApplyTests(pos);
			}
			entity.Pos.SetFrom(pos);
		}
	}

	public void AfterPhysicsTick(float dt)
	{
		entity.AfterPhysicsTick?.Invoke();
	}

	protected virtual bool IsFirstTick(Entity entity)
	{
		EntityPos prevServerPos = entity.PreviousServerPos;
		if (prevServerPos.X == 0.0 && prevServerPos.Y == 0.0 && prevServerPos.Z == 0.0 && prevPos.X == 0.0 && prevPos.Y == 0.0)
		{
			return prevPos.Z == 0.0;
		}
		return false;
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		if (sapi != null)
		{
			sapi.Server.RemovePhysicsTickable(this);
		}
	}

	public override string PropertyName()
	{
		return "entitypassivephysics";
	}
}
