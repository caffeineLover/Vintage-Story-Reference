using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorRepulseAgents : EntityBehavior
{
	protected Vec3d pushVector = new Vec3d();

	protected EntityPartitioning partitionUtil;

	[DocumentAsJson("Optional", "true", false)]
	protected bool movable = true;

	protected bool ignorePlayers;

	protected double touchdist;

	private IClientWorldAccessor cworld;

	public double ownPosRepulseX;

	public double ownPosRepulseY;

	public double ownPosRepulseZ;

	public float mySize;

	protected int dimension;

	public override bool ThreadSafe => true;

	public EntityBehaviorRepulseAgents(Entity entity)
		: base(entity)
	{
		entity.hasRepulseBehavior = true;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		movable = attributes["movable"].AsBool(defaultValue: true);
		partitionUtil = entity.Api.ModLoader.GetModSystem<EntityPartitioning>();
		ignorePlayers = entity is EntityPlayer && entity.World.Config.GetAsBool("player2PlayerCollisions", defaultValue: true);
		cworld = entity.World as IClientWorldAccessor;
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		touchdist = entity.touchDistance;
		entity.BHRepulseAgents = this;
		entity.AfterPhysicsTick = AfterPhysicsTick;
	}

	public override void UpdateColSelBoxes()
	{
		touchdist = entity.touchDistance;
	}

	public void AfterPhysicsTick()
	{
		Entity obj = entity;
		EntityPos SidedPos = obj.Pos;
		Cuboidf CollisionBox = obj.CollisionBox;
		Cuboidf OriginCollisionBox = obj.OriginCollisionBox;
		ownPosRepulseX = SidedPos.X + (double)(CollisionBox.X2 - OriginCollisionBox.X2);
		ownPosRepulseY = SidedPos.Y + (double)(CollisionBox.Y2 - OriginCollisionBox.Y2);
		ownPosRepulseZ = SidedPos.Z + (double)(CollisionBox.Z2 - OriginCollisionBox.Z2);
	}

	public override void OnGameTick(float deltaTime)
	{
		Entity entity = base.entity;
		if (entity.State == EnumEntityState.Inactive || !entity.IsInteractable || !movable)
		{
			return;
		}
		EntityAgent eagent = entity as EntityAgent;
		if (eagent?.MountedOn != null || entity.World.ElapsedMilliseconds < 2000)
		{
			return;
		}
		Vec3d pushVector = this.pushVector;
		pushVector.Set(0.0, 0.0, 0.0);
		mySize = entity.SelectionBox.Length * entity.SelectionBox.Height * (float)((eagent == null || !eagent.Controls.Sneak) ? 1 : 2);
		dimension = entity.Pos.Dimension;
		if (cworld != null && entity != cworld.Player.Entity)
		{
			WalkEntity(cworld.Player.Entity);
		}
		else
		{
			partitionUtil.WalkEntities(ownPosRepulseX, ownPosRepulseY, ownPosRepulseZ, touchdist + partitionUtil.LargestTouchDistance + 0.1, WalkEntity, IsInRangePartition, EnumEntitySearchType.Creatures);
		}
		if (pushVector.X != 0.0 || pushVector.Z != 0.0 || pushVector.Y != 0.0)
		{
			pushVector.X = GameMath.Clamp(pushVector.X, -3.0, 3.0) / 30.0;
			pushVector.Y = GameMath.Clamp(pushVector.Y, -3.0, 0.5) / 30.0;
			pushVector.Z = GameMath.Clamp(pushVector.Z, -3.0, 3.0) / 30.0;
			if (cworld != null && entity == cworld.Player.Entity)
			{
				entity.Pos.Motion.Add(pushVector);
			}
			else
			{
				entity.Pos.Motion.Add(pushVector);
			}
		}
	}

	private bool WalkEntity(Entity e)
	{
		Entity ourEntity = entity;
		if (e == ourEntity || !(e.BHRepulseAgents is EntityBehaviorRepulseAgents theirRepulse) || !e.IsInteractable || (ignorePlayers && e is EntityPlayer))
		{
			return true;
		}
		if (e is EntityAgent eagent && eagent.MountedOn?.Entity == ourEntity)
		{
			return true;
		}
		if (e.Pos.Dimension != dimension)
		{
			return true;
		}
		if (theirRepulse is ICustomRepulseBehavior custom)
		{
			return custom.Repulse(ourEntity, pushVector);
		}
		double dx = ownPosRepulseX - theirRepulse.ownPosRepulseX;
		double dy = ownPosRepulseY - theirRepulse.ownPosRepulseY;
		double dz = ownPosRepulseZ - theirRepulse.ownPosRepulseZ;
		double distSq = dx * dx + dy * dy + dz * dz;
		double minDistSq = ourEntity.touchDistanceSq + e.touchDistanceSq;
		if (distSq >= minDistSq)
		{
			return true;
		}
		double pushForce = (1.0 - distSq / minDistSq) / (double)Math.Max(0.001f, GameMath.Sqrt(distSq));
		double px = dx * pushForce;
		double py = dy * pushForce;
		double pz = dz * pushForce;
		float pushDiff = GameMath.Clamp(e.SelectionBox.Length * e.SelectionBox.Height / mySize, 0f, 1f);
		if (ourEntity.OnGround)
		{
			pushDiff *= 3f;
		}
		pushVector.Add(px * (double)pushDiff, py * (double)pushDiff * 0.75, pz * (double)pushDiff);
		return true;
	}

	public override string PropertyName()
	{
		return "repulseagents";
	}

	private bool IsInRangePartition(Entity e, double posX, double posY, double posZ, double radiusSq)
	{
		return true;
	}
}
