using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class EntityBoat : Entity, IRenderer, IDisposable, ISeatInstSupplier, IMountableListener
{
	public double ForwardSpeed;

	public double AngularVelocity;

	private ModSystemBoatingSoundAndRatlineStamina modsysSounds;

	private double swimmingOffsetY;

	public Dictionary<string, string> MountAnimations = new Dictionary<string, string>();

	private bool requiresPaddlingTool;

	private bool unfurlSails;

	private string weatherVaneAnimCode;

	private long CurrentlyControllingEntityId;

	private ICoreClientAPI capi;

	private float curRotMountAngleZ;

	public Vec3f mountAngle = new Vec3f();

	public override double FrustumSphereRadius => base.FrustumSphereRadius * 2.5;

	public override bool IsCreature => true;

	public override bool ApplyGravity => true;

	public override bool IsInteractable => true;

	public override float MaterialDensity => 100f;

	public override double SwimmingOffsetY => swimmingOffsetY;

	public virtual float SpeedMultiplier { get; set; } = 1f;

	public double RenderOrder => 0.0;

	public int RenderRange => 999;

	public string CreatedByPlayername => WatchedAttributes.GetString("createdByPlayername");

	public string CreatedByPlayerUID => WatchedAttributes.GetString("createdByPlayerUID");

	protected int sailPosition
	{
		get
		{
			return WatchedAttributes.GetInt("sailPosition");
		}
		set
		{
			WatchedAttributes.SetInt("sailPosition", value);
		}
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		swimmingOffsetY = properties.Attributes["swimmingOffsetY"].AsDouble();
		SpeedMultiplier = properties.Attributes["speedMultiplier"].AsFloat(1f);
		MountAnimations = properties.Attributes["mountAnimations"].AsObject<Dictionary<string, string>>();
		weatherVaneAnimCode = properties.Attributes["weatherVaneAnimCode"].AsString();
		base.Initialize(properties, api, InChunkIndex3d);
		WatchedAttributes.RegisterModifiedListener("sailPosition", MarkShapeModified);
		requiresPaddlingTool = properties.Attributes["requiresPaddlingTool"].AsBool();
		unfurlSails = properties.Attributes["unfurlSails"].AsBool();
		capi = api as ICoreClientAPI;
		if (capi != null)
		{
			capi.Event.RegisterRenderer(this, EnumRenderStage.Before, "boatsim");
			modsysSounds = api.ModLoader.GetModSystem<ModSystemBoatingSoundAndRatlineStamina>();
		}
	}

	public override void OnTesselation(ref Shape entityShape, string shapePathForLogging)
	{
		Shape shape = entityShape;
		if (unfurlSails)
		{
			if (shape == entityShape)
			{
				entityShape = entityShape.Clone();
			}
			switch (sailPosition)
			{
			case 0:
				entityShape.RemoveElementByName("SailUnfurled");
				entityShape.RemoveElementByName("SailHalf");
				break;
			case 1:
				entityShape.RemoveElementByName("SailFurled");
				entityShape.RemoveElementByName("SailUnfurled");
				break;
			case 2:
				entityShape.RemoveElementByName("SailFurled");
				entityShape.RemoveElementByName("SailHalf");
				break;
			}
		}
		base.OnTesselation(ref entityShape, shapePathForLogging);
	}

	public virtual void OnRenderFrame(float dt, EnumRenderStage stage)
	{
		if (capi.IsGamePaused)
		{
			return;
		}
		updateBoatAngleAndMotion(dt);
		long ellapseMs = capi.InWorldEllapsedMilliseconds;
		float forwardpitch = 0f;
		if (Swimming)
		{
			double gamespeed = capi.World.Calendar.SpeedOfTime / 60f;
			float intensity = (0.15f + GlobalConstants.CurrentWindSpeedClient.X * 0.9f) * (unfurlSails ? 0.7f : 1f);
			float diff = (float)Math.PI / 360f * intensity;
			mountAngle.X = GameMath.Sin((float)((double)ellapseMs / 1000.0 * 2.0 * gamespeed)) * 8f * diff;
			mountAngle.Y = GameMath.Cos((float)((double)ellapseMs / 2000.0 * 2.0 * gamespeed)) * 3f * diff;
			mountAngle.Z = (0f - GameMath.Sin((float)((double)ellapseMs / 3000.0 * 2.0 * gamespeed))) * 8f * diff;
			curRotMountAngleZ += ((float)AngularVelocity * 5f * (float)Math.Sign(ForwardSpeed) - curRotMountAngleZ) * dt * 5f;
			forwardpitch = (0f - (float)ForwardSpeed) * 1.3f * (unfurlSails ? 0.5f : 1f);
		}
		if (!(base.Properties.Client.Renderer is EntityShapeRenderer esr))
		{
			return;
		}
		esr.xangle = mountAngle.X + curRotMountAngleZ;
		esr.yangle = mountAngle.Y;
		esr.zangle = mountAngle.Z + forwardpitch;
		if (AnimManager.Animator != null)
		{
			if (weatherVaneAnimCode != null && !AnimManager.IsAnimationActive(weatherVaneAnimCode))
			{
				AnimManager.StartAnimation(weatherVaneAnimCode);
			}
			float targetWindDir = GameMath.Mod((float)Math.Atan2(GlobalConstants.CurrentWindSpeedClient.X, GlobalConstants.CurrentWindSpeedClient.Z) + (float)Math.PI * 2f - base.Pos.Yaw, (float)Math.PI * 2f);
			RunningAnimation anim = AnimManager.GetAnimationState(weatherVaneAnimCode);
			if (anim != null)
			{
				anim.CurrentFrame = targetWindDir * (180f / (float)Math.PI) / 10f;
				anim.BlendedWeight = 1f;
				anim.EasingFactor = 1f;
			}
		}
	}

	public override void OnGameTick(float dt)
	{
		if (World.Side == EnumAppSide.Server)
		{
			_ = World.ElapsedMilliseconds;
			if (base.IsOnFire && World.ElapsedMilliseconds - OnFireBeginTotalMs > 10000)
			{
				Die();
			}
			updateBoatAngleAndMotion(dt);
		}
		base.OnGameTick(dt);
	}

	public override void OnAsyncParticleTick(float dt, IAsyncParticleManager manager)
	{
		base.OnAsyncParticleTick(dt, manager);
		double disturbance = Math.Abs(ForwardSpeed) + Math.Abs(AngularVelocity);
		if (disturbance > 0.01)
		{
			float minx = -3f;
			float addx = 6f;
			float minz = -0.75f;
			float addz = 1.5f;
			EntityPos herepos = base.Pos;
			Random rnd = Api.World.Rand;
			Entity.SplashParticleProps.AddVelocity.Set((float)herepos.Motion.X * 20f, (float)herepos.Motion.Y, (float)herepos.Motion.Z * 20f);
			Entity.SplashParticleProps.AddPos.Set(0.10000000149011612, 0.0, 0.10000000149011612);
			Entity.SplashParticleProps.QuantityMul = 0.5f * (float)disturbance;
			double y = herepos.Y - 0.15;
			for (int i = 0; i < 10; i++)
			{
				float dx = minx + (float)rnd.NextDouble() * addx;
				float dz = minz + (float)rnd.NextDouble() * addz;
				double yaw = (double)(base.Pos.Yaw + (float)Math.PI / 2f) + Math.Atan2(dx, dz);
				double dist = Math.Sqrt(dx * dx + dz * dz);
				Entity.SplashParticleProps.BasePos.Set(herepos.X + Math.Sin(yaw) * dist, y, herepos.Z + Math.Cos(yaw) * dist);
				manager.Spawn(Entity.SplashParticleProps);
			}
		}
	}

	protected virtual void updateBoatAngleAndMotion(float dt)
	{
		dt = Math.Min(0.5f, dt);
		float step = GlobalConstants.PhysicsFrameTime;
		Vec2d motion = SeatsToMotion(step);
		if (!Swimming)
		{
			if (!unfurlSails || sailPosition > 0)
			{
				return;
			}
			bool nudgeForwards = false;
			IMountableSeat[] seats = GetBehavior<EntityBehaviorSeatable>().Seats;
			for (int i = 0; i < seats.Length; i++)
			{
				EntityBoatSeat seat = seats[i] as EntityBoatSeat;
				if (seat.Config.Controllable && seat.Passenger is EntityPlayer && seat.Passenger.EntityId == CurrentlyControllingEntityId)
				{
					EntityControls controls = seat.controls;
					if (controls.Forward || controls.Backward)
					{
						nudgeForwards = true;
						break;
					}
				}
			}
			if (!nudgeForwards)
			{
				return;
			}
			motion *= 0.5;
		}
		ForwardSpeed += (motion.X * (double)SpeedMultiplier - ForwardSpeed) * (double)dt;
		AngularVelocity += (motion.Y * (double)SpeedMultiplier - AngularVelocity) * (double)dt;
		EntityPos pos = base.Pos;
		if (ForwardSpeed != 0.0)
		{
			Vec3d targetmotion = pos.GetViewVector().Mul((float)(0.0 - ForwardSpeed)).ToVec3d();
			pos.Motion.X = targetmotion.X;
			pos.Motion.Z = targetmotion.Z;
		}
		EntityBehaviorPassivePhysicsMultiBox bh = GetBehavior<EntityBehaviorPassivePhysicsMultiBox>();
		bool canTurn = true;
		if (AngularVelocity != 0.0)
		{
			float yawDelta = (float)AngularVelocity * dt * 30f;
			if (bh.AdjustCollisionBoxesToYaw(dt, push: true, base.Pos.Yaw + yawDelta))
			{
				pos.Yaw += yawDelta;
			}
			else
			{
				canTurn = false;
			}
		}
		else
		{
			canTurn = bh.AdjustCollisionBoxesToYaw(dt, push: true, base.Pos.Yaw);
		}
		if (!canTurn)
		{
			if (bh.AdjustCollisionBoxesToYaw(dt, push: true, base.Pos.Yaw - 0.1f))
			{
				pos.Yaw -= 0.0002f;
			}
			else if (bh.AdjustCollisionBoxesToYaw(dt, push: true, base.Pos.Yaw + 0.1f))
			{
				pos.Yaw += 0.0002f;
			}
		}
		pos.Roll = 0f;
	}

	protected virtual bool HasPaddle(Entity entity)
	{
		if (!requiresPaddlingTool)
		{
			return true;
		}
		if (!(entity is EntityAgent agent))
		{
			return false;
		}
		if (agent.RightHandItemSlot == null || agent.RightHandItemSlot.Empty)
		{
			return false;
		}
		return agent.RightHandItemSlot.Itemstack.Collectible.Attributes?.IsTrue("paddlingTool") ?? false;
	}

	public virtual Vec2d SeatsToMotion(float dt)
	{
		int seatsRowing = 0;
		double linearMotion = 0.0;
		double angularMotion = 0.0;
		EntityBehaviorSeatable bh = GetBehavior<EntityBehaviorSeatable>();
		bh.Controller = null;
		IMountableSeat[] seats = bh.Seats;
		for (int i = 0; i < seats.Length; i++)
		{
			EntityBoatSeat seat = seats[i] as EntityBoatSeat;
			if (seat.Passenger == null)
			{
				continue;
			}
			if (!(seat.Passenger is EntityPlayer))
			{
				seat.Passenger.Pos.Yaw = base.Pos.Yaw;
			}
			if (seat.Config.BodyYawLimit.HasValue && seat.Passenger is EntityPlayer eplr)
			{
				if (eplr.BodyYawLimits == null)
				{
					eplr.BodyYawLimits = new AngleConstraint(base.Pos.Yaw + seat.Config.MountRotation.Y * ((float)Math.PI / 180f), seat.Config.BodyYawLimit.Value);
					eplr.HeadYawLimits = new AngleConstraint(base.Pos.Yaw + seat.Config.MountRotation.Y * ((float)Math.PI / 180f), (float)Math.PI / 2f);
				}
				else
				{
					eplr.BodyYawLimits.X = base.Pos.Yaw + seat.Config.MountRotation.Y * ((float)Math.PI / 180f);
					eplr.BodyYawLimits.Y = seat.Config.BodyYawLimit.Value;
					eplr.HeadYawLimits.X = base.Pos.Yaw + seat.Config.MountRotation.Y * ((float)Math.PI / 180f);
					eplr.HeadYawLimits.Y = (float)Math.PI / 2f;
				}
			}
			if (!seat.Config.Controllable || bh.Controller != null)
			{
				continue;
			}
			EntityControls controls = seat.controls;
			if (seat.Passenger.EntityId != CurrentlyControllingEntityId)
			{
				continue;
			}
			bh.Controller = seat.Passenger;
			if (!HasPaddle(seat.Passenger))
			{
				seat.Passenger.AnimManager?.StopAnimation(MountAnimations["ready"]);
				seat.actionAnim = null;
				continue;
			}
			if (controls.Left == controls.Right && capi == null)
			{
				StopAnimation("turnLeft");
				StopAnimation("turnRight");
			}
			if (controls.Left && !controls.Right)
			{
				StartAnimation("turnLeft");
				StopAnimation("turnRight");
			}
			if (controls.Right && !controls.Left)
			{
				StopAnimation("turnLeft");
				StartAnimation("turnRight");
			}
			float str = ((++seatsRowing == 1) ? 1f : 0.5f);
			if (unfurlSails && sailPosition > 0)
			{
				linearMotion += (double)(str * dt * (float)sailPosition * 1.5f);
			}
			if (!controls.TriesToMove)
			{
				seat.actionAnim = null;
				if (seat.Passenger.AnimManager != null && !seat.Passenger.AnimManager.IsAnimationActive(MountAnimations["ready"]))
				{
					seat.Passenger.AnimManager.StartAnimation(MountAnimations["ready"]);
				}
				continue;
			}
			if (controls.Right && !controls.Backward && !controls.Forward)
			{
				seat.actionAnim = MountAnimations["backwards"];
			}
			else
			{
				seat.actionAnim = MountAnimations[controls.Backward ? "backwards" : "forwards"];
			}
			seat.Passenger.AnimManager?.StopAnimation(MountAnimations["ready"]);
			if (controls.Left || controls.Right)
			{
				float dir = (controls.Left ? 1 : (-1));
				angularMotion += (double)(str * dir * dt);
			}
			if (controls.Forward || controls.Backward)
			{
				float dir2 = (controls.Forward ? 1 : (-1));
				if (Math.Abs(GameMath.AngleRadDistance(base.Pos.Yaw, seat.Passenger.Pos.Yaw)) > (float)Math.PI / 2f && requiresPaddlingTool)
				{
					dir2 *= -1f;
				}
				float ctrlstr = 2f;
				if (unfurlSails)
				{
					ctrlstr = ((sailPosition == 0) ? 0.4f : 0f);
				}
				linearMotion += (double)(str * dir2 * dt * ctrlstr);
			}
		}
		return new Vec2d(linearMotion, angularMotion);
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode)
	{
		if (mode == EnumInteractMode.Interact && AllowPickup() && IsEmpty() && tryPickup(byEntity, mode))
		{
			return;
		}
		EntityBehaviorSelectionBoxes? behavior = GetBehavior<EntityBehaviorSelectionBoxes>();
		if (behavior != null && behavior.IsAPCode((byEntity as EntityPlayer).EntitySelection, "LowerMastAP"))
		{
			sailPosition = (ushort)((sailPosition + 1) % 3);
		}
		EnumHandling handled = EnumHandling.PassThrough;
		foreach (EntityBehavior behavior2 in base.SidedProperties.Behaviors)
		{
			behavior2.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
			if (handled == EnumHandling.PreventSubsequent)
			{
				break;
			}
		}
	}

	private bool AllowPickup()
	{
		return base.Properties.Attributes?["rightClickPickup"].AsBool() ?? false;
	}

	private bool IsEmpty()
	{
		EntityBehaviorSeatable? behavior = GetBehavior<EntityBehaviorSeatable>();
		EntityBehaviorRideableAccessories bhr = GetBehavior<EntityBehaviorRideableAccessories>();
		if (!behavior.AnyMounted())
		{
			return bhr?.Inventory.Empty ?? true;
		}
		return false;
	}

	private bool tryPickup(EntityAgent byEntity, EnumInteractMode mode)
	{
		if (byEntity.Controls.ShiftKey)
		{
			ItemStack stack = new ItemStack(World.GetItem(Code));
			if (!byEntity.TryGiveItemStack(stack))
			{
				World.SpawnItemEntity(stack, base.Pos.XYZ);
			}
			Api.World.Logger.Audit("{0} Picked up 1x{1} at {2}.", byEntity.GetName(), stack.Collectible.Code, base.Pos);
			Die();
			return true;
		}
		return false;
	}

	public override bool CanCollect(Entity byEntity)
	{
		return false;
	}

	public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player)
	{
		WorldInteraction[] wis = base.GetInteractionHelp(world, es, player);
		EntityBehaviorSelectionBoxes? behavior = GetBehavior<EntityBehaviorSelectionBoxes>();
		if (behavior != null && behavior.IsAPCode(es, "LowerMastAP"))
		{
			wis = wis.Append(new WorldInteraction
			{
				ActionLangCode = "sailboat-unfurlsails",
				MouseButton = EnumMouseButton.Right
			});
		}
		return wis;
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		capi?.Event.UnregisterRenderer(this, EnumRenderStage.Before);
	}

	public void Dispose()
	{
	}

	public IMountableSeat CreateSeat(IMountable mountable, string seatId, SeatConfig config)
	{
		return new EntityBoatSeat(mountable, seatId, config);
	}

	public void DidUnmount(EntityAgent entityAgent)
	{
		if (CurrentlyControllingEntityId == entityAgent.EntityId)
		{
			EntityBehaviorSeatable bh = GetBehavior<EntityBehaviorSeatable>();
			CurrentlyControllingEntityId = bh.Seats.FirstOrDefault((IMountableSeat seat) => seat.CanControl && seat.Passenger != null)?.Passenger.EntityId ?? 0;
		}
		MarkShapeModified();
	}

	public void DidMount(EntityAgent entityAgent)
	{
		if (entityAgent.MountedOn.CanControl && CurrentlyControllingEntityId <= 0)
		{
			CurrentlyControllingEntityId = entityAgent.EntityId;
		}
		MarkShapeModified();
	}

	public override string GetInfoText()
	{
		string text = base.GetInfoText();
		if (CreatedByPlayername != null)
		{
			text = text + "\n" + Lang.Get("entity-createdbyplayer", CreatedByPlayername);
		}
		return text;
	}

	public override bool InRangeOf(Vec3d position, float horRangeSq, float vertRange)
	{
		return base.Pos.InRangeOf(position, horRangeSq + 64f, vertRange);
	}
}
