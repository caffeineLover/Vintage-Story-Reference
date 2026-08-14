using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityRideableSeat : EntitySeat
{
	protected EntityPos seatPos = new EntityPos();

	protected Matrixf modelmat = new Matrixf();

	protected string RideableClassName = "rideableanimal";

	public override EnumMountAngleMode AngleMode => EnumMountAngleMode.FixateYaw;

	public override AnimationMetaData SuggestedAnimation => (mountedEntity as EntityBehaviorRideable)?.CurrentControlMeta?.GetSeatAnimation(this);

	public override Vec3f LocalEyePos
	{
		get
		{
			modelmat.Identity();
			if (Entity.AnimManager?.Animator?.GetAttachmentPointPose(config.APName) != null)
			{
				modelmat.RotateY((float)Math.PI / 2f + Entity.Pos.Yaw);
				modelmat.RotateX((Entity.Properties.Client.Renderer as EntityShapeRenderer)?.nowSwivelRad ?? 0f);
				modelmat.Translate(config.EyeOffsetX, config.EyeHeight, 0f);
				modelmat.RotateY(-(float)Math.PI / 2f - Entity.Pos.Yaw);
			}
			return modelmat.TransformVector(new Vec4f(0f, 0f, 0f, 1f)).XYZ;
		}
	}

	public override EntityPos SeatPosition
	{
		get
		{
			if (config.MountOffset != null)
			{
				return oldSeatPosition;
			}
			modelmat.Identity();
			float pitchApplied = 0.55f;
			AttachmentPointAndPose apap = Entity.AnimManager?.Animator?.GetAttachmentPointPose(config.APName);
			if (apap != null)
			{
				EntityShapeRenderer esr = Entity.Properties.Client.Renderer as EntityShapeRenderer;
				modelmat.RotateY(Entity.Pos.Yaw + (float)Math.PI / 2f);
				if (config.RiderOffset != null)
				{
					modelmat.Translate(config.RiderOffset);
				}
				modelmat.Translate(0f, Entity.SelectionBox.Y2 / 2f, 0f);
				modelmat.RotateZ((float)mountedEntity.StepPitch * (1f - pitchApplied));
				if (esr != null)
				{
					modelmat.RotateX(esr.nowSwivelRad + esr.xangle);
					modelmat.RotateY(esr.yangle);
					modelmat.RotateZ(esr.zangle);
				}
				modelmat.Translate(0f, (0f - Entity.SelectionBox.Y2) / 2f, 0f);
				float mountScale = Entity.Properties.Client.Size;
				modelmat.Scale(mountScale, mountScale, mountScale);
				modelmat.Translate(-0.5f, 0f, -0.5f);
				apap.MulUncentered(modelmat);
				modelmat.RotateY(-(float)Math.PI / 2f);
			}
			Vec4f translation = modelmat.TransformVector(new Vec4f(0f, 0f, 0f, 1f));
			seatPos.SetFrom(mountedEntity.Position).Add(translation.X, translation.Y, translation.Z);
			seatPos.Roll = 0f;
			seatPos.Yaw = Entity.Pos.Yaw;
			seatPos.Pitch = (float)mountedEntity.StepPitch * pitchApplied;
			if (apap != null)
			{
				seatPos.Roll += (float)apap.AttachPoint.RotationX;
				seatPos.Yaw += (float)apap.AttachPoint.RotationY;
				seatPos.Pitch += (float)apap.AttachPoint.RotationZ;
			}
			return seatPos;
		}
	}

	public override Matrixf RenderTransform
	{
		get
		{
			if (config.MountOffset != null)
			{
				return oldRenderTransform;
			}
			modelmat.Identity();
			float riderScale = base.Passenger?.Properties?.Client?.Size ?? 1f;
			if (config.RiderOffset != null && riderScale != 1f)
			{
				modelmat.Translate(config.RiderOffset.Clone().Mul(riderScale - 1f));
			}
			modelmat.RotateX(config.MountRotation.X * ((float)Math.PI / 180f));
			modelmat.RotateZ(config.MountRotation.Z * ((float)Math.PI / 180f));
			return modelmat;
		}
	}

	private EntityPos oldSeatPosition
	{
		get
		{
			loadAttachPointTransform();
			Vec4f rotvec = modelmat.TransformVector(new Vec4f(0f, 0f, 0f, 1f));
			seatPos.SetFrom(mountedEntity.Position).Add(rotvec.X, rotvec.Y, rotvec.Z);
			seatPos.Pitch = (float)mountedEntity.StepPitch * 0.55f;
			return seatPos;
		}
	}

	private Matrixf oldRenderTransform
	{
		get
		{
			loadAttachPointTransform();
			Vec4f rotvec = modelmat.TransformVector(new Vec4f(0f, 0f, 0f, 1f));
			return new Matrixf().Translate(0f - rotvec.X, 0f - rotvec.Y, 0f - rotvec.Z).Mul(modelmat).RotateDeg(config.MountRotation);
		}
	}

	public override float FpHandPitchFollow => 0.2f;

	private void loadAttachPointTransform()
	{
		modelmat.Identity();
		AttachmentPointAndPose apap = Entity.AnimManager?.Animator?.GetAttachmentPointPose(config.APName);
		if (apap != null)
		{
			EntityShapeRenderer esr = Entity.Properties.Client.Renderer as EntityShapeRenderer;
			modelmat.RotateY((float)Math.PI / 2f + Entity.Pos.Yaw);
			modelmat.Translate(0.0, 0.6, 0.0);
			if (esr != null)
			{
				modelmat.RotateX(esr.nowSwivelRad + esr.xangle);
				modelmat.RotateY(esr.yangle);
				modelmat.RotateZ(esr.zangle);
			}
			modelmat.Translate(0.0, -0.6, 0.0);
			modelmat.Translate(-0.5, 0.5, -0.5);
			apap.Mul(modelmat);
			if (config.MountOffset != null)
			{
				modelmat.Translate(config.MountOffset);
			}
			modelmat.Translate(-1.0, -0.5, -1.0);
			modelmat.RotateY((float)Math.PI / 2f - Entity.Pos.Yaw);
		}
	}

	public EntityRideableSeat(IMountable mountablesupplier, string seatId, SeatConfig config)
		: base(mountablesupplier, seatId, config)
	{
	}

	public override bool CanMount(EntityAgent entityAgent)
	{
		if (!(entityAgent is EntityPlayer player))
		{
			return false;
		}
		EntityBehaviorRideable rideable = Entity.GetBehavior<EntityBehaviorRideable>();
		if (rideable != null && Entity.WatchedAttributes.GetInt("generation") < rideable.MinGeneration)
		{
			(player.World.Api as ICoreClientAPI)?.TriggerIngameError(this, "toowildtoride", Lang.Get("mount-interact-toowildtoride"));
			return false;
		}
		EntityBehaviorOwnable ebho = Entity.GetBehavior<EntityBehaviorOwnable>();
		if (ebho != null && !ebho.IsOwner(player) && config.Controllable)
		{
			(player.World.Api as ICoreClientAPI)?.TriggerIngameError(this, "requiersownership", Lang.Get("mount-interact-requiresownership"));
			return false;
		}
		return true;
	}

	public static IMountableSeat? GetMountable(IWorldAccessor world, TreeAttribute tree)
	{
		return (world.GetEntityById(tree.GetLong("entityIdMount", 0L))?.GetBehavior<EntityBehaviorSeatable>())?.Seats.FirstOrDefault((IMountableSeat seat) => seat.SeatId == tree.GetString("seatId"));
	}

	public override void MountableToTreeAttributes(TreeAttribute tree)
	{
		base.MountableToTreeAttributes(tree);
		tree.SetLong("entityIdMount", Entity.EntityId);
		tree.SetString("className", RideableClassName);
	}

	public override void DidMount(EntityAgent entityAgent)
	{
		base.DidMount(entityAgent);
		if (Entity != null)
		{
			Entity.GetBehavior<EntityBehaviorTaskAI>()?.TaskManager.StopTasks();
			Entity.StartAnimation("idle");
			if (entityAgent.Api is ICoreClientAPI capi && capi.World.Player.Entity.EntityId == entityAgent.EntityId)
			{
				capi.Input.MouseYaw = Entity.Pos.Yaw;
			}
		}
		(mountedEntity as IMountableListener)?.DidMount(entityAgent);
		(Entity as IMountableListener)?.DidMount(entityAgent);
		ArgumentNullException.ThrowIfNull(Entity, "Entity");
		Entity.Api.Event.TriggerEntityMounted(entityAgent, this);
	}

	public override void DidUnmount(EntityAgent entityAgent)
	{
		if (entityAgent.World.Side == EnumAppSide.Server && base.DoTeleportOnUnmount)
		{
			tryTeleportToFreeLocation();
		}
		if (entityAgent is EntityPlayer eplr)
		{
			eplr.BodyYawLimits = null;
			eplr.HeadYawLimits = null;
		}
		base.DidUnmount(entityAgent);
		(mountedEntity as IMountableListener)?.DidUnmount(entityAgent);
		(Entity as IMountableListener)?.DidUnmount(entityAgent);
		Entity.Api.Event.TriggerEntityUnmounted(entityAgent, this);
	}

	protected virtual void tryTeleportToFreeLocation()
	{
		IWorldAccessor world = base.Passenger.World;
		IBlockAccessor ba = base.Passenger.World.BlockAccessor;
		Vec3d rightPos = Entity.Pos.XYZ.Add(EntityPos.GetViewVector(0f, Entity.Pos.Yaw + (float)Math.PI / 2f)).Add(0.0, 0.01, 0.0);
		Vec3d leftPos = Entity.Pos.XYZ.Add(EntityPos.GetViewVector(0f, Entity.Pos.Yaw - (float)Math.PI / 2f)).Add(0.0, 0.01, 0.0);
		if (GameMath.AngleRadDistance(base.Passenger.Pos.Yaw, Entity.Pos.Yaw + (float)Math.PI / 2f) < (float)Math.PI / 2f)
		{
			Vec3d vec3d = leftPos;
			leftPos = rightPos;
			rightPos = vec3d;
		}
		if (ba.GetBlockRaw((int)rightPos.X, (int)(rightPos.Y - 0.1), (int)rightPos.Z, 4).SideSolid[BlockFacing.UP.Index] && !world.CollisionTester.IsColliding(ba, base.Passenger.CollisionBox, rightPos, alsoCheckTouch: false))
		{
			base.Passenger.TeleportTo(rightPos);
		}
		else if (ba.GetBlockRaw((int)leftPos.X, (int)(leftPos.Y - 0.1), (int)leftPos.Z, 4).SideSolid[BlockFacing.UP.Index] && !world.CollisionTester.IsColliding(ba, base.Passenger.CollisionBox, leftPos, alsoCheckTouch: false))
		{
			base.Passenger.TeleportTo(leftPos);
		}
	}
}
