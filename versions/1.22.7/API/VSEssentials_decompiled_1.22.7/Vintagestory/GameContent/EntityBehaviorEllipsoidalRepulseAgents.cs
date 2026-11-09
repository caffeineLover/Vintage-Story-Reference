using System;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorEllipsoidalRepulseAgents : EntityBehaviorRepulseAgents, ICustomRepulseBehavior
{
	[DocumentAsJson("Required", "", false)]
	protected Vec3d offset;

	[DocumentAsJson("Required", "", false)]
	protected Vec3d radius;

	public EntityBehaviorEllipsoidalRepulseAgents(Entity entity)
		: base(entity)
	{
		entity.customRepulseBehavior = true;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		offset = attributes["offset"].AsObject(new Vec3d());
		radius = attributes["radius"].AsObject<Vec3d>();
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		touchdist = Math.Max(radius.X, radius.Z);
		entity.BHRepulseAgents = this;
		entity.AfterPhysicsTick = base.AfterPhysicsTick;
	}

	public override void UpdateColSelBoxes()
	{
		touchdist = Math.Max(radius.X, radius.Z);
	}

	public override float GetTouchDistance(ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		return (float)Math.Max(radius.X, radius.Z) + 0.5f;
	}

	public bool Repulse(Entity e, Vec3d pushVector)
	{
		if (!(e.BHRepulseAgents is EntityBehaviorRepulseAgents theirRepulse))
		{
			return true;
		}
		Entity ourEntity = entity;
		Vec3d radius = this.radius;
		double theirRepulseY = theirRepulse.ownPosRepulseY;
		if (theirRepulseY > ownPosRepulseY + radius.Y || ownPosRepulseY > theirRepulseY + (double)e.SelectionBox.Height)
		{
			return true;
		}
		double dx = ownPosRepulseX - theirRepulse.ownPosRepulseX;
		double dz = ownPosRepulseZ - theirRepulse.ownPosRepulseZ;
		float yaw = ourEntity.Pos.Yaw;
		double dist = RelDistanceToEllipsoid(dx, dz, radius.X, radius.Z, yaw);
		if (dist >= 1.0)
		{
			return true;
		}
		double pushForce = -1.0 * (1.0 - dist);
		double px = dx * pushForce;
		double py = 0.0;
		double pz = dz * pushForce;
		float mySize = ourEntity.SelectionBox.Length * ourEntity.SelectionBox.Height;
		float pushDiff = GameMath.Clamp(e.SelectionBox.Length * e.SelectionBox.Height / mySize, 0f, 1f) / 1.5f;
		if (e.OnGround)
		{
			pushDiff *= 10f;
		}
		pushVector.Add(px * (double)pushDiff, py * (double)pushDiff * 0.75, pz * (double)pushDiff);
		return true;
	}

	public double RelDistanceToEllipsoid(double x, double z, double wdt, double len, double yaw)
	{
		double num = x * Math.Cos(yaw) - z * Math.Sin(yaw);
		double zPrime = x * Math.Sin(yaw) + z * Math.Cos(yaw);
		double num2 = num + offset.X;
		zPrime += offset.Z;
		return num2 * num2 / (wdt * wdt) + zPrime * zPrime / (len * len);
	}
}
