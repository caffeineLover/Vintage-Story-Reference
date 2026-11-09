using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityBoatSeat : EntityRideableSeat
{
	public string actionAnim;

	public override EnumMountAngleMode AngleMode => config.AngleMode;

	private Dictionary<string, string> animations => (Entity as EntityBoat).MountAnimations;

	public override AnimationMetaData SuggestedAnimation
	{
		get
		{
			if (actionAnim == null)
			{
				return null;
			}
			Entity passenger = base.Passenger;
			AnimationMetaData ameta = default(AnimationMetaData);
			if (passenger != null && passenger.Properties?.Client.AnimationsByMetaCode?.TryGetValue(actionAnim, out ameta) == true)
			{
				return ameta;
			}
			return null;
		}
	}

	public EntityBoatSeat(IMountable mountablesupplier, string seatId, SeatConfig config)
		: base(mountablesupplier, seatId, config)
	{
		RideableClassName = "boat";
	}

	public override bool CanMount(EntityAgent entityAgent)
	{
		JsonObject attributes = config.Attributes;
		if (attributes != null && attributes["ropeTieablesOnly"].AsBool())
		{
			return entityAgent.HasBehavior<EntityBehaviorRopeTieable>();
		}
		return base.CanMount(entityAgent);
	}

	public override void DidMount(EntityAgent entityAgent)
	{
		base.DidMount(entityAgent);
		entityAgent.AnimManager.StartAnimation(config.Animation ?? animations["idle"]);
	}

	public override void DidUnmount(EntityAgent entityAgent)
	{
		if (base.Passenger != null)
		{
			base.Passenger.AnimManager?.StopAnimation(animations["ready"]);
			base.Passenger.AnimManager?.StopAnimation(animations["forwards"]);
			base.Passenger.AnimManager?.StopAnimation(animations["backwards"]);
			base.Passenger.AnimManager?.StopAnimation(animations["idle"]);
			base.Passenger.AnimManager?.StopAnimation(config.Animation);
			base.Passenger.Pos.Roll = 0f;
		}
		base.DidUnmount(entityAgent);
	}

	protected override void tryTeleportToFreeLocation()
	{
		IWorldAccessor world = base.Passenger.World;
		IBlockAccessor ba = base.Passenger.World.BlockAccessor;
		double shortestDistance = 99.0;
		Vec3d shortestTargetPos = null;
		for (int dx = -4; dx <= 4; dx++)
		{
			for (int dy = 0; dy < 2; dy++)
			{
				for (int dz = -4; dz <= 4; dz++)
				{
					Vec3d targetPos = base.Passenger.Pos.XYZ.AsBlockPos.ToVec3d().Add((double)dx + 0.5, (double)dy + 0.1, (double)dz + 0.5);
					Block block = ba.GetBlockRaw((int)targetPos.X, (int)(targetPos.Y - 0.15), (int)targetPos.Z, 4);
					if (ba.GetBlockRaw((int)targetPos.X, (int)targetPos.Y, (int)targetPos.Z, 2).Id == 0 && block.SideSolid[BlockFacing.UP.Index] && !world.CollisionTester.IsColliding(ba, base.Passenger.CollisionBox, targetPos, alsoCheckTouch: false))
					{
						float dist = targetPos.DistanceTo(base.Passenger.Pos.XYZ);
						if ((double)dist < shortestDistance)
						{
							shortestDistance = dist;
							shortestTargetPos = targetPos;
						}
					}
				}
			}
		}
		if (shortestTargetPos != null)
		{
			base.Passenger.TeleportTo(shortestTargetPos);
			return;
		}
		bool found = false;
		int dx2 = -1;
		while (!found && dx2 <= 1)
		{
			int dz2 = -1;
			while (!found && dz2 <= 1)
			{
				Vec3d targetPos2 = base.Passenger.Pos.XYZ.AsBlockPos.ToVec3d().Add((double)dx2 + 0.5, 1.1, (double)dz2 + 0.5);
				if (!world.CollisionTester.IsColliding(ba, base.Passenger.CollisionBox, targetPos2, alsoCheckTouch: false))
				{
					base.Passenger.TeleportTo(targetPos2);
					found = true;
					break;
				}
				dz2++;
			}
			dx2++;
		}
	}
}
