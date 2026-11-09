using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("physics", "A set of properties used for physics modules", "Vintagestory.API.Datastructures.JsonObject", "Optional", "None", true)]
public class EntityBehaviorControlledPhysics : PhysicsBehaviorBase, IPhysicsTickable, IRemotePhysics
{
	protected const double collisionboxReductionForInsideBlocksCheck = 0.009;

	protected bool smoothStepping;

	protected readonly List<PModule> physicsModules = new List<PModule>();

	protected readonly List<PModule> customModules = new List<PModule>();

	protected Vec3d newPos = new Vec3d();

	protected readonly Vec3d prevPos = new Vec3d();

	protected readonly BlockPos tmpPos = new BlockPos(0);

	protected readonly Cuboidd entityBox = new Cuboidd();

	protected readonly List<FastVec3i> traversed = new List<FastVec3i>(4);

	protected readonly List<Block> traversedBlocks = new List<Block>(4);

	protected readonly IComparer<FastVec3i> fastVec3IComparer = new FastVec3iComparer();

	protected readonly Vec3d moveDelta = new Vec3d();

	protected double prevYMotion;

	protected double prevPrevYMotion;

	protected bool onGroundBefore;

	protected bool feetInLiquidBefore;

	protected bool swimmingBefore;

	protected float knockBackCounter;

	protected Cuboidf sneakTestCollisionbox = new Cuboidf();

	protected readonly Cuboidd steppingCollisionBox = new Cuboidd();

	protected readonly Vec3d steppingTestVec = new Vec3d();

	protected readonly Vec3d steppingTestMotion = new Vec3d();

	[DocumentAsJson("Optional", "0.6", false)]
	public float StepHeight = 0.6f;

	[DocumentAsJson("Optional", "0.07", false)]
	public float stepUpSpeed = 0.07f;

	[DocumentAsJson("Optional", "0.07", false)]
	public float climbUpSpeed = 0.07f;

	[DocumentAsJson("Optional", "0.035", false)]
	public float climbDownSpeed = 0.035f;

	[DocumentAsJson("Optional", "0.355", false)]
	public float climbAnywhereStepThreshold = 0.375f;

	public bool EnableModulesApplication { get; set; } = true;

	public Entity Entity => entity;

	public bool Ticking { get; set; }

	public override bool ThreadSafe => true;

	public void SetState(EntityPos pos, float dt)
	{
		float dtFactor = dt * 60f;
		Entity entity = base.entity;
		prevPos.Set(pos);
		prevPrevYMotion = prevYMotion;
		prevYMotion = pos.Motion.Y;
		onGroundBefore = entity.OnGround;
		feetInLiquidBefore = entity.FeetInLiquid;
		swimmingBefore = entity.Swimming;
		traversed.Clear();
		traversedBlocks.Clear();
		if (entity.AdjustCollisionBoxToAnimation)
		{
			AdjustCollisionBoxToAnimation(dtFactor);
		}
	}

	public EntityBehaviorControlledPhysics(Entity entity)
		: base(entity)
	{
	}

	public virtual void SetModules()
	{
		physicsModules.Add(new PModuleWind());
		physicsModules.Add(new PModuleOnGround());
		physicsModules.Add(new PModuleInLiquid());
		physicsModules.Add(new PModuleInAir());
		physicsModules.Add(new PModuleGravity());
		physicsModules.Add(new PModuleMotionDrag());
		physicsModules.Add(new PModuleKnockback());
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		Init();
		SetProperties(properties, attributes);
		if (entity.Api is ICoreServerAPI esapi)
		{
			esapi.Server.AddPhysicsTickable(this);
		}
		entity.PhysicsUpdateWatcher?.Invoke(0f, entity.Pos.XYZ);
		if (entity.Api.Side != EnumAppSide.Client)
		{
			return;
		}
		EnumHandling handling = EnumHandling.Handled;
		OnReceivedServerPos(isTeleport: true, ref handling);
		entity.Attributes.RegisterModifiedListener("dmgkb", delegate
		{
			if (entity.Attributes.GetInt("dmgkb") == 1)
			{
				knockBackCounter = 2f;
			}
		});
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		base.AfterInitialized(onFirstSpawn);
	}

	public override void OnGameTick(float deltaTime)
	{
		if (entity.World is IServerWorldAccessor)
		{
			if (mountableSupplier?.Controller is EntityPlayer { Alive: not false })
			{
				callOnEntityInside();
				entity.AfterPhysicsTick?.Invoke();
			}
		}
		else
		{
			entity.AfterPhysicsTick?.Invoke();
		}
	}

	public void SetProperties(EntityProperties properties, JsonObject attributes)
	{
		StepHeight = attributes["stepHeight"].AsFloat(0.6f);
		stepUpSpeed = attributes["stepUpSpeed"].AsFloat(0.07f);
		climbUpSpeed = attributes["climbUpSpeed"].AsFloat(0.07f);
		climbDownSpeed = attributes["climbDownSpeed"].AsFloat(0.035f);
		climbAnywhereStepThreshold = attributes["climbAnywhereStepThreshold"].AsFloat(0.375f);
		sneakTestCollisionbox = entity.CollisionBox.Clone().OmniNotDownGrowBy(-0.1f);
		sneakTestCollisionbox.Y2 /= 2f;
		SetModules();
		JsonObject physics = properties?.Attributes?["physics"];
		for (int i = 0; i < physicsModules.Count; i++)
		{
			physicsModules[i].Initialize(physics, entity);
		}
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
		EntityPos serverPos = entity.Pos;
		if (nPos == null)
		{
			nPos = new Vec3d();
			nPos.Set(serverPos);
		}
		float dtFactor = dt * 60f;
		lPos.SetFrom(nPos);
		nPos.Set(serverPos);
		if (isTeleport)
		{
			lPos.SetFrom(nPos);
		}
		lPos.Dimension = entity.Pos.Dimension;
		tmpPos.dimension = lPos.Dimension;
		lPos.Motion.X = (nPos.X - lPos.X) / (double)dtFactor;
		lPos.Motion.Y = (nPos.Y - lPos.Y) / (double)dtFactor;
		lPos.Motion.Z = (nPos.Z - lPos.Z) / (double)dtFactor;
		if (lPos.Motion.Length() > 20.0)
		{
			lPos.Motion.Set(0.0, 0.0, 0.0);
		}
		entity.Pos.Motion.Set(lPos.Motion);
		serverPos.Motion.Set(lPos.Motion);
		PhysicsBehaviorBase.collisionTester.NewTick(lPos);
		EntityAgent agent = entity as EntityAgent;
		if (agent?.MountedOn != null)
		{
			entity.Swimming = false;
			entity.OnGround = false;
			if (capi != null)
			{
				entity.Pos.SetPos(agent.MountedOn.SeatPosition);
			}
			serverPos.Motion.X = 0.0;
			serverPos.Motion.Y = 0.0;
			serverPos.Motion.Z = 0.0;
		}
		else
		{
			SetState(lPos, dt);
			RemoteMotionAndCollision(lPos, dtFactor);
			ApplyTests(lPos, ((EntityAgent)entity).Controls, dt, remote: true);
			if (knockBackCounter > 0f)
			{
				knockBackCounter -= dt;
				return;
			}
			knockBackCounter = 0f;
			entity.Attributes.SetInt("dmgkb", 0);
		}
	}

	public void RemoteMotionAndCollision(EntityPos pos, float dtFactor)
	{
		double gravityStrength = (double)(1f / 60f * dtFactor) + Math.Max(0.0, -0.014999999664723873 * pos.Motion.Y * (double)dtFactor);
		pos.Motion.Y -= gravityStrength;
		PhysicsBehaviorBase.collisionTester.ApplyTerrainCollision(entity, pos, dtFactor, ref newPos, 0f, 0f);
		bool falling = lPos.Motion.Y < 0.0;
		entity.OnGround = entity.CollidedVertically && falling;
		pos.Motion.Y += gravityStrength;
		pos.SetPos(nPos);
	}

	public void MotionAndCollision(EntityPos pos, EntityControls controls, float dt)
	{
		if (!EnableModulesApplication)
		{
			return;
		}
		Entity entity = base.entity;
		foreach (PModule physicsModule in physicsModules)
		{
			if (physicsModule.Applicable(entity, pos, controls))
			{
				physicsModule.DoApply(dt, entity, pos, controls);
			}
		}
		foreach (PModule physicsModule2 in customModules)
		{
			if (physicsModule2.Applicable(entity, pos, controls))
			{
				physicsModule2.DoApply(dt, entity, pos, controls);
			}
		}
	}

	public void ApplyTests(EntityPos pos, EntityControls controls, float dt, bool remote)
	{
		Entity entity = base.entity;
		EntityProperties entityProperties = entity.Properties;
		IBlockAccessor blockAccessor = entity.World.BlockAccessor;
		float dtFactor = dt * 60f;
		BlockPos tmpPos = this.tmpPos;
		tmpPos.SetDimension(pos.Dimension);
		Cuboidd entityBox = this.entityBox;
		Vec3d motion = pos.Motion;
		Vec3d newPos = this.newPos;
		if (double.IsNaN(motion.X) || double.IsNaN(motion.Y) || double.IsNaN(motion.Z) || double.IsNaN(pos.X) || double.IsNaN(pos.Y) || double.IsNaN(pos.Z))
		{
			throw new ArgumentException("Given pos contained NaN: " + pos?.ToString() + " for entity " + entity?.Code);
		}
		if (float.IsNaN(dt))
		{
			throw new ArgumentException("Given dt was NaN for entity " + entity?.Code);
		}
		controls.IsClimbing = false;
		entity.ClimbingOnFace = null;
		entity.ClimbingIntoFace = null;
		if (entityProperties.CanClimb)
		{
			bool canClimbAnywhere = entityProperties.CanClimbAnywhere && entity.Alive;
			int searchBlockLayer = ((!canClimbAnywhere) ? 1 : 0);
			int height = (int)Math.Ceiling(entity.CollisionBox.Y2);
			entityBox.SetAndTranslate(entity.CollisionBox, pos.X, pos.Y, pos.Z);
			tmpPos.Set((int)pos.X, 0, (int)pos.Z);
			for (int dy = 0; dy < height; dy++)
			{
				tmpPos.Y = (int)pos.Y + dy;
				Block inBlock = blockAccessor.GetBlock(tmpPos, searchBlockLayer);
				if (!inBlock.IsClimbable(tmpPos) && !canClimbAnywhere)
				{
					continue;
				}
				Cuboidf[] collisionBoxes = inBlock.GetCollisionBoxes(blockAccessor, tmpPos);
				if (collisionBoxes == null)
				{
					continue;
				}
				for (int i = 0; i < collisionBoxes.Length; i++)
				{
					double distance = entityBox.ShortestDistanceFrom(collisionBoxes[i], tmpPos);
					controls.IsClimbing |= distance < (double)entityProperties.ClimbTouchDistance;
					if (controls.IsClimbing)
					{
						entity.ClimbingOnFace = null;
						break;
					}
				}
			}
			if (canClimbAnywhere && controls.WalkVector.LengthSq() > 1E-05)
			{
				BlockFacing walkIntoFace = BlockFacing.FromVector(controls.WalkVector.X, controls.WalkVector.Y, controls.WalkVector.Z);
				if (walkIntoFace != null)
				{
					double selfy = pos.Y + (double)walkIntoFace.Normali.Y;
					tmpPos.Set((int)pos.X + walkIntoFace.Normali.X, (int)pos.Y + walkIntoFace.Normali.Y, (int)pos.Z + walkIntoFace.Normali.Z);
					Cuboidf[] collisionBoxes2 = blockAccessor.GetBlock(tmpPos, 0).GetCollisionBoxes(blockAccessor, tmpPos);
					if (collisionBoxes2 == null || collisionBoxes2.Length == 0)
					{
						entity.ClimbingIntoFace = null;
					}
					else
					{
						entity.ClimbingIntoFace = ((!entity.OnGround || (double)collisionBoxes2.Max((Cuboidf box) => box.Y2) > (double)climbAnywhereStepThreshold + selfy - (double)(int)selfy) ? walkIntoFace : null);
					}
				}
			}
			if (!controls.IsClimbing && !controls.IsStepping)
			{
				float touchDistance = entityProperties.ClimbTouchDistance;
				int baseY = (int)pos.Y;
				for (int i2 = 0; i2 < 4; i2++)
				{
					tmpPos.IterateHorizontalOffsets(i2);
					Cuboidf[] collisionBoxes3;
					int j;
					for (int dy2 = 0; dy2 < height; dy2++)
					{
						tmpPos.Y = baseY + dy2;
						Block inBlock2 = blockAccessor.GetBlock(tmpPos, searchBlockLayer);
						if (!inBlock2.IsClimbable(tmpPos) && !canClimbAnywhere)
						{
							continue;
						}
						collisionBoxes3 = inBlock2.GetCollisionBoxes(blockAccessor, tmpPos);
						if (collisionBoxes3 == null)
						{
							continue;
						}
						j = 0;
						while (j < collisionBoxes3.Length)
						{
							double distance2 = entityBox.ShortestDistanceFrom(collisionBoxes3[j], tmpPos);
							if ((!((double)collisionBoxes3[j].Y2 > (double)climbAnywhereStepThreshold + pos.Y - (double)(int)pos.Y) && entity.OnGround) || !(distance2 < (double)touchDistance))
							{
								j++;
								continue;
							}
							goto IL_03fc;
						}
					}
					continue;
					IL_03fc:
					controls.IsClimbing = true;
					entity.ClimbingOnFace = BlockFacing.HORIZONTALS[i2];
					entity.ClimbingOnCollBox = collisionBoxes3[j];
					break;
				}
			}
		}
		if (double.IsNaN(motion.X) || double.IsNaN(motion.Y) || double.IsNaN(motion.Z))
		{
			throw new Exception("Partially updated motion contains NaN: " + motion?.ToString() + " for entity " + entity?.Code);
		}
		if (!remote)
		{
			if (controls.IsClimbing && controls.WalkVector.Y == 0.0)
			{
				if (double.IsNaN(climbUpSpeed) || double.IsNaN(climbDownSpeed))
				{
					throw new ArgumentException("Unexpected NaN: climbUpSpeed=" + climbUpSpeed + ", climbDownSpeed=" + climbDownSpeed + " for entity " + entity?.Code);
				}
				motion.Y = (controls.Sneak ? Math.Max(0f - climbUpSpeed, motion.Y - (double)climbUpSpeed) : motion.Y);
				if (controls.Jump)
				{
					motion.Y = climbDownSpeed * dt * 60f;
				}
			}
			double nextX = motion.X * (double)dtFactor + pos.X;
			double nextY = motion.Y * (double)dtFactor + pos.Y;
			double nextZ = motion.Z * (double)dtFactor + pos.Z;
			moveDelta.Set(motion.X * (double)dtFactor, prevYMotion * (double)dtFactor, motion.Z * (double)dtFactor);
			PhysicsBehaviorBase.collisionTester.ApplyTerrainCollision(entity, pos, dtFactor, ref newPos, 0f, CollisionYExtra);
			controls.IsStepping = HandleSteppingOnBlocks(pos, moveDelta, dtFactor, controls);
			HandleSneaking(pos, controls, dt);
			int x = (int)pos.X;
			int y = (int)pos.Y;
			int z = (int)pos.Z;
			if (entity.CollidedHorizontally && !controls.IsClimbing && !controls.IsStepping && entityProperties.Habitat != EnumHabitat.Underwater)
			{
				if (blockAccessor.GetBlockRaw(x, (int)(pos.InternalY + 0.5), z, 2).LiquidLevel >= 7 || blockAccessor.GetBlockRaw(x, (int)pos.InternalY, z, 2).LiquidLevel >= 7 || blockAccessor.GetBlockRaw(x, (int)(pos.InternalY - 0.05), z, 2).LiquidLevel >= 7)
				{
					motion.Y += 0.2 * (double)dt;
					controls.IsStepping = true;
				}
				else
				{
					double absX = Math.Abs(motion.X);
					double absZ = Math.Abs(motion.Z);
					if (absX > absZ)
					{
						if (absZ < 0.001)
						{
							motion.Z += ((motion.Z < 0.0) ? (-0.0025) : 0.0025);
						}
					}
					else if (absX < 0.001)
					{
						motion.X += ((motion.X < 0.0) ? (-0.0025) : 0.0025);
					}
				}
			}
			if (double.IsNaN(motion.X) || double.IsNaN(motion.Y) || double.IsNaN(motion.Z))
			{
				throw new Exception("Proposed motion contains NaN: " + motion?.ToString() + " for entity " + entity?.Code);
			}
			float halfWidth = entity.CollisionBox.Width / 2f;
			if (blockAccessor.IsNotTraversable((int)(nextX + (double)(halfWidth * (float)Math.Sign(motion.X))), y, z, pos.Dimension))
			{
				newPos.X = pos.X;
			}
			if (blockAccessor.IsNotTraversable(x, (int)nextY, z, pos.Dimension))
			{
				newPos.Y = pos.Y;
			}
			if (blockAccessor.IsNotTraversable(x, y, (int)(nextZ + (double)(halfWidth * (float)Math.Sign(motion.Z))), pos.Dimension))
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
		bool falling = prevYMotion <= 0.0;
		entity.OnGround = entity.CollidedVertically && falling;
		float offX = entity.CollisionBox.X2 - entity.OriginCollisionBox.X2;
		float offZ = entity.CollisionBox.Z2 - entity.OriginCollisionBox.Z2;
		int posX = (int)(pos.X + (double)offX);
		int posY = (int)pos.InternalY;
		int posZ = (int)(pos.Z + (double)offZ);
		int swimmingY = (int)(pos.InternalY + entity.SwimmingOffsetY);
		Block blockFluid = blockAccessor.GetBlockRaw(posX, posY, posZ, 2);
		Block middleWOIBlock = ((swimmingY == posY) ? blockFluid : blockAccessor.GetBlockRaw(posX, swimmingY, posZ, 2));
		entity.Swimming = middleWOIBlock.IsLiquid();
		entity.OnGround = (entity.CollidedVertically && falling && !controls.IsClimbing) || controls.IsStepping;
		if (blockFluid.IsLiquid())
		{
			Block aboveBlock = blockAccessor.GetBlockRaw(posX, posY + 1, posZ, 2);
			entity.FeetInLiquid = (double)((float)(blockFluid.LiquidLevel + ((aboveBlock.LiquidLevel > 0) ? 1 : 0)) / 8f) >= pos.Y - (double)(int)pos.Y;
			entity.InLava = blockFluid.LiquidCode == "lava";
			if (!feetInLiquidBefore && entity.FeetInLiquid && !IsFirstTick(entity))
			{
				entity.OnCollideWithLiquid();
			}
		}
		else
		{
			entity.FeetInLiquid = false;
			entity.InLava = false;
		}
		if (!onGroundBefore && entity.OnGround)
		{
			entity.OnFallToGround(Math.Min(prevPrevYMotion, prevYMotion));
		}
		if ((swimmingBefore || feetInLiquidBefore) && !entity.Swimming && !entity.FeetInLiquid)
		{
			entity.OnExitedLiquid();
		}
		if (!falling || entity.OnGround || controls.IsClimbing)
		{
			entity.PositionBeforeFalling.Set(pos);
		}
		Cuboidd cuboidd = PhysicsBehaviorBase.collisionTester.entityBox;
		int xMax = (int)(cuboidd.X2 - 0.009);
		int yMax = (int)(cuboidd.Y2 - 0.009);
		int zMax = (int)(cuboidd.Z2 - 0.009);
		int xMin = (int)(cuboidd.X1 + 0.009);
		int zMin = (int)(cuboidd.Z1 + 0.009);
		for (int y2 = (int)(cuboidd.Y1 + 0.009); y2 <= yMax; y2++)
		{
			for (int x2 = xMin; x2 <= xMax; x2++)
			{
				for (int z2 = zMin; z2 <= zMax; z2++)
				{
					tmpPos.Set(x2, y2, z2);
					Block block = blockAccessor.GetBlock(tmpPos);
					if (block.Id != 0)
					{
						FastVec3i posTraversed = new FastVec3i(x2, y2, z2);
						int index = traversed.BinarySearch(posTraversed, fastVec3IComparer);
						if (index < 0)
						{
							index = ~index;
						}
						traversed.Insert(index, posTraversed);
						traversedBlocks.Insert(index, block);
					}
				}
			}
		}
		entity.PhysicsUpdateWatcher?.Invoke(0f, prevPos);
	}

	public virtual void OnPhysicsTick(float dt)
	{
		Entity entity = base.entity;
		if (entity.State != EnumEntityState.Active)
		{
			return;
		}
		EntityPos pos = entity.Pos;
		PhysicsBehaviorBase.collisionTester.AssignToEntity(this, pos.Dimension);
		EntityControls controls = ((EntityAgent)entity).Controls;
		EntityAgent agent = entity as EntityAgent;
		if (agent?.MountedOn != null)
		{
			AdjustMountedPositionFor(agent);
			return;
		}
		SetState(pos, dt);
		MotionAndCollision(pos, controls, dt);
		ApplyTests(pos, controls, dt, remote: false);
		IMountable mount = mountableSupplier;
		if (mount == null)
		{
			return;
		}
		IMountableSeat[] seats = mount.Seats;
		for (int i = 0; i < seats.Length; i++)
		{
			if (seats[i]?.Passenger is EntityAgent { MountedOn: not null } ea)
			{
				AdjustMountedPositionFor(ea);
			}
		}
	}

	private void AdjustMountedPositionFor(EntityAgent entity)
	{
		entity.Swimming = false;
		entity.OnGround = false;
		EntityPos pos = entity.Pos;
		if (!(entity is EntityPlayer))
		{
			pos.SetFrom(entity.MountedOn.SeatPosition);
		}
		else
		{
			pos.SetPos(entity.MountedOn.SeatPosition);
		}
		pos.Motion.X = 0.0;
		pos.Motion.Y = 0.0;
		pos.Motion.Z = 0.0;
	}

	public virtual void AfterPhysicsTick(float dt)
	{
		Entity entity = base.entity;
		if (entity.State == EnumEntityState.Active && (mountableSupplier == null || capi != null || !mountableSupplier.IsBeingControlled()))
		{
			BlockPos tmpPos = this.tmpPos;
			List<Block> traversedBlocks = this.traversedBlocks;
			List<FastVec3i> traversed = this.traversed;
			for (int i = 0; i < traversedBlocks.Count; i++)
			{
				tmpPos.Set(traversed[i]);
				traversedBlocks[i].OnEntityInside(entity.World, entity, tmpPos);
			}
			entity.AfterPhysicsTick?.Invoke();
		}
	}

	public void HandleSneaking(EntityPos pos, EntityControls controls, float dt)
	{
		if (!controls.Sneak || !entity.OnGround || pos.Motion.Y > 0.0)
		{
			return;
		}
		Vec3d testPosition = new Vec3d();
		testPosition.Set(pos.X, pos.InternalY - (double)(GlobalConstants.GravityPerSecond * dt), pos.Z);
		if (!PhysicsBehaviorBase.collisionTester.IsColliding(entity.World.BlockAccessor, sneakTestCollisionbox, testPosition))
		{
			return;
		}
		tmpPos.Set((int)pos.X, (int)pos.Y - 1, (int)pos.Z);
		Block belowBlock = entity.World.BlockAccessor.GetBlock(tmpPos);
		testPosition.Set(newPos.X, newPos.Y - (double)(GlobalConstants.GravityPerSecond * dt) + (double)pos.DimensionYAdjustment, pos.Z);
		if (!PhysicsBehaviorBase.collisionTester.IsColliding(entity.World.BlockAccessor, sneakTestCollisionbox, testPosition))
		{
			if (belowBlock.IsClimbable(tmpPos))
			{
				newPos.X += (pos.X - newPos.X) / 10.0;
			}
			else
			{
				newPos.X = pos.X;
			}
		}
		testPosition.Set(pos.X, newPos.Y - (double)(GlobalConstants.GravityPerSecond * dt) + (double)pos.DimensionYAdjustment, newPos.Z);
		if (!PhysicsBehaviorBase.collisionTester.IsColliding(entity.World.BlockAccessor, sneakTestCollisionbox, testPosition))
		{
			if (belowBlock.IsClimbable(tmpPos))
			{
				newPos.Z += (pos.Z - newPos.Z) / 10.0;
			}
			else
			{
				newPos.Z = pos.Z;
			}
		}
	}

	protected virtual bool HandleSteppingOnBlocks(EntityPos pos, Vec3d moveDelta, float dtFac, EntityControls controls)
	{
		if (controls.WalkVector.X == 0.0 && controls.WalkVector.Z == 0.0)
		{
			return false;
		}
		if ((!entity.OnGround && !entity.Swimming) || entity.Properties.Habitat == EnumHabitat.Underwater)
		{
			return false;
		}
		steppingCollisionBox.SetAndTranslate(entity.CollisionBox, pos.X, pos.Y, pos.Z);
		steppingCollisionBox.Y2 = Math.Max(steppingCollisionBox.Y1 + (double)StepHeight, steppingCollisionBox.Y2);
		Vec3d walkVec = controls.WalkVector;
		Cuboidd steppableBox = FindSteppableCollisionBox(steppingCollisionBox, moveDelta.Y, walkVec);
		if (steppableBox != null)
		{
			Vec3d testMotion = steppingTestMotion;
			testMotion.Set(moveDelta.X, moveDelta.Y, moveDelta.Z);
			if (TryStep(pos, testMotion, dtFac, steppableBox, steppingCollisionBox))
			{
				return true;
			}
			Vec3d testVec = steppingTestVec;
			testMotion.Z = 0.0;
			if (TryStep(pos, testMotion, dtFac, FindSteppableCollisionBox(steppingCollisionBox, moveDelta.Y, testVec.Set(walkVec.X, walkVec.Y, 0.0)), steppingCollisionBox))
			{
				return true;
			}
			testMotion.Set(0.0, moveDelta.Y, moveDelta.Z);
			if (TryStep(pos, testMotion, dtFac, FindSteppableCollisionBox(steppingCollisionBox, moveDelta.Y, testVec.Set(0.0, walkVec.Y, walkVec.Z)), steppingCollisionBox))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool TryStep(EntityPos pos, Vec3d moveDelta, float dtFac, Cuboidd steppableBox, Cuboidd entityCollisionBox)
	{
		if (steppableBox == null)
		{
			return false;
		}
		double heightDiff = steppableBox.Y2 - entityCollisionBox.Y1 + 0.03;
		Vec3d stepPos = newPos.OffsetCopy(moveDelta.X, heightDiff, moveDelta.Z);
		if (!PhysicsBehaviorBase.collisionTester.IsColliding(entity.World.BlockAccessor, entity.CollisionBox, stepPos, alsoCheckTouch: false))
		{
			pos.Y += stepUpSpeed * dtFac;
			PhysicsBehaviorBase.collisionTester.ApplyTerrainCollision(entity, pos, dtFac, ref newPos);
			return true;
		}
		return false;
	}

	public static bool GetCollidingCollisionBox(IBlockAccessor blockAccessor, Cuboidf entityBoxRel, Vec3d pos, out CachedCuboidList blocks, BlockPos tmpPos, bool alsoCheckTouch = true)
	{
		blocks = new CachedCuboidList();
		Vec3d blockPosVec = new Vec3d();
		Cuboidd entityBox = entityBoxRel.ToDouble().Translate(pos);
		int minX = (int)((double)entityBoxRel.MinX + pos.X);
		int num = (int)((double)entityBoxRel.MinY + pos.Y - 1.0);
		int minZ = (int)((double)entityBoxRel.MinZ + pos.Z);
		int maxX = (int)Math.Ceiling((double)entityBoxRel.MaxX + pos.X);
		int maxY = (int)Math.Ceiling((double)entityBoxRel.MaxY + pos.Y);
		int maxZ = (int)Math.Ceiling((double)entityBoxRel.MaxZ + pos.Z);
		for (int y = num; y <= maxY; y++)
		{
			for (int x = minX; x <= maxX; x++)
			{
				for (int z = minZ; z <= maxZ; z++)
				{
					tmpPos.Set(x, y, z);
					Block block = blockAccessor.GetBlock(tmpPos);
					blockPosVec.Set(x, y, z);
					Cuboidf[] collisionBoxes = block.GetCollisionBoxes(blockAccessor, tmpPos);
					if (collisionBoxes == null)
					{
						continue;
					}
					foreach (Cuboidf collBox in collisionBoxes)
					{
						if (collBox != null && (alsoCheckTouch ? entityBox.IntersectsOrTouches(collBox, blockPosVec) : entityBox.Intersects(collBox, blockPosVec)))
						{
							blocks.Add(collBox, x, tmpPos.InternalY, z, block);
						}
					}
				}
			}
		}
		return blocks.Count > 0;
	}

	public Cuboidd FindSteppableCollisionBox(Cuboidd entityCollisionBox, double motionY, Vec3d walkVector)
	{
		Cuboidd steppableBox = null;
		CachedCuboidListFaster blocks = PhysicsBehaviorBase.collisionTester.CollisionBoxList;
		int maxCount = blocks.Count;
		BlockPos pos = new BlockPos(entity.Pos.Dimension);
		for (int i = 0; i < maxCount; i++)
		{
			Block block = blocks.blocks[i];
			if (block.CollisionBoxes != null && !block.CanStep && entity.CollisionBox.Height < 5f * block.CollisionBoxes[0].Height)
			{
				continue;
			}
			pos.Set(blocks.positions[i]);
			if (!block.SideIsSolid(pos, 4) && !block.SideIsSolid(pos, 5))
			{
				pos.Down();
				Block blockBelow = entity.World.BlockAccessor.GetMostSolidBlock(pos);
				pos.Up();
				if (blockBelow.CollisionBoxes != null && !blockBelow.CanStep && entity.CollisionBox.Height < 5f * blockBelow.CollisionBoxes[0].Height)
				{
					continue;
				}
			}
			Cuboidd collisionBox = blocks.cuboids[i];
			EnumIntersect intersect = CollisionTester.AabbIntersect(collisionBox, entityCollisionBox, walkVector);
			if (intersect != EnumIntersect.NoIntersect)
			{
				if ((intersect == EnumIntersect.Stuck && !block.AllowStepWhenStuck) || (intersect == EnumIntersect.IntersectY && motionY > 0.0))
				{
					return null;
				}
				double heightDiff = collisionBox.Y2 - entityCollisionBox.Y1;
				if (!(heightDiff <= 0.0) && heightDiff <= (double)StepHeight && (steppableBox == null || steppableBox.Y2 < collisionBox.Y2))
				{
					steppableBox = collisionBox;
				}
			}
		}
		return steppableBox;
	}

	public List<Cuboidd> FindSteppableCollisionboxSmooth(Cuboidd entityCollisionBox, Cuboidd entitySensorBox, double motionY, Vec3d walkVector)
	{
		List<Cuboidd> steppableBoxes = new List<Cuboidd>();
		GetCollidingCollisionBox(entity.World.BlockAccessor, entitySensorBox.ToFloat(), new Vec3d(), out var blocks, tmpPos);
		for (int i = 0; i < blocks.Count; i++)
		{
			Cuboidd collisionbox = blocks.cuboids[i];
			Block block = blocks.blocks[i];
			if (!block.CanStep && block.CollisionBoxes != null && entity.CollisionBox.Height < 5f * block.CollisionBoxes[0].Height)
			{
				continue;
			}
			BlockPos pos = blocks.positions[i];
			if (!block.SideIsSolid(pos, 4) && !block.SideIsSolid(pos, 5))
			{
				pos.Down();
				Block blockBelow = entity.World.BlockAccessor.GetMostSolidBlock(pos);
				pos.Up();
				if (!blockBelow.CanStep && blockBelow.CollisionBoxes != null && entity.CollisionBox.Height < 5f * blockBelow.CollisionBoxes[0].Height)
				{
					continue;
				}
			}
			EnumIntersect intersect = CollisionTester.AabbIntersect(collisionbox, entityCollisionBox, walkVector);
			if ((intersect == EnumIntersect.Stuck && !block.AllowStepWhenStuck) || (intersect == EnumIntersect.IntersectY && motionY > 0.0))
			{
				return null;
			}
			double heightDiff = collisionbox.Y2 - entityCollisionBox.Y1;
			if (!(heightDiff <= (entity.CollidedVertically ? 0.0 : (-0.05))) && heightDiff <= (double)StepHeight)
			{
				steppableBoxes.Add(collisionbox);
			}
		}
		return steppableBoxes;
	}

	public void AdjustCollisionBoxToAnimation(float dtFac)
	{
		AttachmentPointAndPose apap = entity.AnimManager.Animator?.GetAttachmentPointPose("Center");
		if (apap != null)
		{
			float[] hitboxOff = new float[4] { 0f, 0f, 0f, 1f };
			AttachmentPoint ap = apap.AttachPoint;
			CompositeShape shape = entity.Properties.Client.Shape;
			float rotX = shape?.rotateX ?? 0f;
			float rotY = shape?.rotateY ?? 0f;
			float rotZ = shape?.rotateZ ?? 0f;
			Span<float> ModelMat = stackalloc float[16];
			Mat4f.NewIdentity(ModelMat);
			Mat4f.Translate(ModelMat, 0f, entity.CollisionBox.Y2 / 2f, 0f);
			double[] quat = Quaterniond.Create();
			Quaterniond.RotateX(quat, quat, entity.Pos.Pitch + rotX * ((float)Math.PI / 180f));
			Quaterniond.RotateY(quat, quat, entity.Pos.Yaw + (rotY + 90f) * ((float)Math.PI / 180f));
			Quaterniond.RotateZ(quat, quat, entity.Pos.Roll + rotZ * ((float)Math.PI / 180f));
			Mat4f.MulQuat(ModelMat, quat);
			float scale = entity.Properties.Client.Size;
			Mat4f.Translate(ModelMat, 0f, (0f - entity.CollisionBox.Y2) / 2f, 0f);
			Mat4f.Scale(ModelMat, scale, scale, scale);
			Mat4f.Translate(ModelMat, -0.5f, 0f, -0.5f);
			Mat4f.Mul(ModelMat, apap.AnimModelMatrix);
			Mat4f.Translate(ModelMat, (float)ap.PosX / 16f, (float)ap.PosY / 16f, (float)ap.PosZ / 16f);
			EntityPos entityPos = entity.Pos;
			Span<float> endVec = stackalloc float[3];
			Mat4f.MulWithVec4(ModelMat, hitboxOff, endVec);
			float motionX = endVec[0] - (entity.CollisionBox.X1 - entity.OriginCollisionBox.X1);
			float motionZ = endVec[2] - (entity.CollisionBox.Z1 - entity.OriginCollisionBox.Z1);
			if ((double)Math.Abs(motionX) > 1E-05 || (double)Math.Abs(motionZ) > 1E-05)
			{
				EntityPos posMoved = entityPos.Copy();
				posMoved.Motion.X = motionX;
				posMoved.Motion.Z = motionZ;
				moveDelta.Set(motionX, posMoved.Motion.Y, motionZ);
				PhysicsBehaviorBase.collisionTester.ApplyTerrainCollision(entity, posMoved, dtFac, ref newPos);
				double reflectX = (newPos.X - entityPos.X) / (double)dtFac - (double)motionX;
				double reflectZ = (newPos.Z - entityPos.Z) / (double)dtFac - (double)motionZ;
				entityPos.Motion.X = reflectX;
				entityPos.Motion.Z = reflectZ;
				entity.CollisionBox.Set(entity.OriginCollisionBox);
				entity.CollisionBox.Translate(endVec[0], 0f, endVec[2]);
				entity.SelectionBox.Set(entity.OriginSelectionBox);
				entity.SelectionBox.Translate(endVec[0], 0f, endVec[2]);
			}
		}
	}

	protected void callOnEntityInside()
	{
		EntityPos pos = entity.Pos;
		IWorldAccessor world = entity.World;
		Cuboidd entityBox = PhysicsBehaviorBase.collisionTester.entityBox;
		entityBox.SetAndTranslate(entity.CollisionBox, pos.X, pos.Y, pos.Z);
		entityBox.RemoveRoundingErrors();
		BlockPos minPos = new BlockPos((int)entityBox.X1, (int)entityBox.Y1, (int)entityBox.Z1, pos.Dimension);
		BlockPos maxPos = new BlockPos((int)entityBox.X2, (int)entityBox.Y2, (int)entityBox.Z2, pos.Dimension);
		world.BlockAccessor.WalkBlocks(minPos, maxPos, delegate(Block block, int x, int y, int z)
		{
			if (block.Id != 0)
			{
				minPos.Set(x, y, z);
				block.OnEntityInside(world, entity, minPos);
			}
		});
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
		return "entitycontrolledphysics";
	}
}
