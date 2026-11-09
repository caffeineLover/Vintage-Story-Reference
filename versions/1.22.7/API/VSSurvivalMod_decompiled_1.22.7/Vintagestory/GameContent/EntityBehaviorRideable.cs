using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("saddleBreaksRequired", "How many saddle breaks are required before entity can be ridden", "Vintagestory.API.MathTools.NatFloat", "Optional", "None", true)]
[AddDocumentationProperty("controlScheme", "Use this on a collectible type. Overrides control scheme for when entity is ridden", "System.String", "Recommended", "None", true)]
public class EntityBehaviorRideable(Entity entity) : EntityBehaviorSeatable(entity), IMountable, IRenderer, IDisposable, IMountableListener
{
	public FastSmallDictionary<EnumHabitat, List<GaitMeta>> RideableGaitOrder = new FastSmallDictionary<EnumHabitat, List<GaitMeta>>(1);

	public bool IsInMidJump;

	protected float coyoteTimer;

	protected long lastJumpMs;

	protected bool jumpNow;

	protected EntityAgent eagent = (EntityAgent)entity;

	protected FastSmallDictionary<string, ControlMeta> Controls = new FastSmallDictionary<string, ControlMeta>(1);

	protected FastSmallDictionary<EnumHabitat, string?[]>? GaitOrderCodes;

	protected ICoreClientAPI? capi;

	protected EntityBehaviorGait ebg;

	public int MinGeneration;

	protected GaitMeta? saddleBreakGait;

	protected string? saddleBreakGaitCode;

	protected ControlMeta? curControlMeta;

	protected EnumControlScheme scheme;

	[DocumentAsJson("Optional", "0", false)]
	protected float saddleBreakDayInterval;

	[DocumentAsJson("Optional", "None", false)]
	protected string? tamedEntityCode;

	protected bool prevForwardKey;

	protected bool prevBackwardKey;

	protected bool prevSprintKey;

	protected bool prevPrevForwardKey;

	protected bool prevPrevBackwardKey;

	protected bool prevPrevSprintKey;

	private float angularMotionWild = 0.1f;

	private long mountedTotalMs;

	public Vec3f MountAngle { get; set; } = new Vec3f();

	public EntityPos SeatPosition => entity.Pos;

	public double RenderOrder => 1.0;

	public int RenderRange => 100;

	public virtual float SpeedMultiplier => 1f;

	public Entity Mount => entity;

	public ControlMeta? CurrentControlMeta => curControlMeta;

	public int RemainingSaddleBreaks
	{
		get
		{
			return entity.WatchedAttributes.GetInt("remainingSaddleBreaksRequired");
		}
		set
		{
			entity.WatchedAttributes.SetInt("remainingSaddleBreaksRequired", value);
		}
	}

	public double LastSaddleBreakTotalDays
	{
		get
		{
			return entity.WatchedAttributes.GetDouble("lastSaddlebreakTotalDays");
		}
		set
		{
			entity.WatchedAttributes.SetDouble("lastSaddlebreakTotalDays", value);
		}
	}

	public double LastDismountTotalHours
	{
		get
		{
			return entity.WatchedAttributes.GetDouble("lastDismountTotalHours");
		}
		set
		{
			entity.WatchedAttributes.SetDouble("lastDismountTotalHours", value);
		}
	}

	public event CanRideDelegate? CanRide;

	public event CanRideDelegate? CanTurn;

	protected override IMountableSeat CreateSeat(string seatId, SeatConfig config)
	{
		return new EntityRideableSeat(this, seatId, config);
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		capi = entity.Api as ICoreClientAPI;
		if (attributes["saddleBreaksRequired"].Exists)
		{
			if (!entity.WatchedAttributes.HasAttribute("remainingSaddleBreaksRequired") && entity.Api.Side == EnumAppSide.Server)
			{
				RemainingSaddleBreaks = GameMath.RoundRandom(entity.Api.World.Rand, attributes["saddleBreaksRequired"].AsObject<NatFloat>().nextFloat(1f, entity.Api.World.Rand));
			}
			saddleBreakDayInterval = attributes["saddleBreakDayInterval"].AsFloat();
			tamedEntityCode = attributes["tamedEntityCode"].AsString();
			saddleBreakGaitCode = attributes["saddleBreakGait"].AsString();
		}
		Controls = attributes["controls"].AsObject(Controls);
		foreach (KeyValuePair<string, ControlMeta> control in Controls)
		{
			if (control.Value.PassengerAnim == null && !attributes["controls"][control.Key]["passengerAnim"].Exists)
			{
				control.Value.PassengerAnim = control.Value.RiderAnim;
			}
		}
		MinGeneration = attributes["minGeneration"].AsInt();
		GaitOrderCodes = attributes["rideableGaitOrder"].AsObject<FastSmallDictionary<EnumHabitat, string[]>>();
		string[] oldFormatLandGaits = attributes["rideableGaitOrder"].AsArray<string>();
		if (GaitOrderCodes == null && oldFormatLandGaits != null)
		{
			GaitOrderCodes = new FastSmallDictionary<EnumHabitat, string[]>(1);
			GaitOrderCodes[EnumHabitat.Land] = oldFormatLandGaits;
			entity.Api.Logger.Warning(string.Concat("Outdated json format for rideableGaitOrder on entity ", entity.Code, ", should list gaits by environment (e.g. Land, Sea) explicitly"));
		}
		foreach (ControlMeta value in Controls.Values)
		{
			value.RiderAnim?.Init();
			value.PassengerAnim?.Init();
		}
		curControlMeta = Controls["idle"];
		capi?.Event.RegisterRenderer(this, EnumRenderStage.Before, "rideablesim");
		if (entity.Api.Event is IServerEventAPI serverEvents)
		{
			serverEvents.MountGaitReceived += ReceiveGaitFromClient;
		}
	}

	private void ReceiveGaitFromClient(Entity mountEntity, string gaitCode)
	{
		if (mountEntity == entity && gaitCode != null && !(gaitCode == ebg.CurrentGait.Code))
		{
			ebg.Gaits.TryGetValue(gaitCode, out GaitMeta gait);
			if (gait != null)
			{
				ebg.CurrentGait = gait;
			}
		}
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		base.AfterInitialized(onFirstSpawn);
		ebg = eagent.GetBehavior<EntityBehaviorGait>();
		if (ebg == null)
		{
			throw new Exception("EntityBehaviorGait not found on rideable entity. Ensure the entity's json file correctly provides one.");
		}
		ArgumentNullException.ThrowIfNull(GaitOrderCodes, "GaitOrderCodes");
		foreach (KeyValuePair<EnumHabitat, string[]> entry in GaitOrderCodes)
		{
			List<GaitMeta> order = RideableGaitOrder.TryGetValue(entry.Key);
			if (order == null)
			{
				order = new List<GaitMeta>();
				RideableGaitOrder[entry.Key] = order;
			}
			string[] value = entry.Value;
			foreach (string str in value)
			{
				if (str == null)
				{
					throw new Exception("Invalid gait code found in rideable behavior");
				}
				GaitMeta gait = ebg?.Gaits[str];
				if (gait == null)
				{
					entity.Api.Logger.Warning($"Unable to find definition of gait {str} from entity {entity.Code}");
					continue;
				}
				if (gait.Environment != entry.Key)
				{
					entity.Api.Logger.Warning($"Wrong environment list for gait {str} from entity {entity.Code}: entered as part of {entry.Key} but gait has environment {gait.Environment}");
				}
				order.Add(gait);
			}
		}
		GaitOrderCodes = null;
		saddleBreakGait = ebg.Gaits.FirstOrDefault<KeyValuePair<string, GaitMeta>>((KeyValuePair<string, GaitMeta> g) => g.Value.Code == saddleBreakGaitCode).Value;
	}

	public void SpeedUp(bool toggleFastest)
	{
		ebg.CurrentGait = GetNextGait(forward: true, toggleFastest);
	}

	public void SlowDown()
	{
		ebg.CurrentGait = GetNextGait(forward: false, toggleFastest: false);
	}

	public virtual GaitMeta GetNextGait(bool forward, bool toggleFastest, GaitMeta? currentGait = null)
	{
		if (currentGait == null)
		{
			currentGait = ebg.CurrentGait;
		}
		ArgumentNullException.ThrowIfNull(currentGait, "currentGait");
		List<GaitMeta> order = RideableGaitOrder.TryGetValue(currentGait.Environment);
		if (order == null)
		{
			return currentGait;
		}
		if (this.IsBeingControlled())
		{
			int currentIndex = order.FindIndex((GaitMeta g) => g.Code == currentGait.Code);
			if (currentIndex < 0)
			{
				return order.FirstOrDefault((GaitMeta g) => (!forward) ? (g.MoveSpeed < currentGait.MoveSpeed) : (g.MoveSpeed > currentGait.MoveSpeed), order[order.Count - 1]);
			}
			int nextIndex = (forward ? (currentIndex + 1) : (currentIndex - 1));
			if (nextIndex < 0)
			{
				nextIndex = 0;
			}
			toggleFastest = toggleFastest && currentIndex > 0 && order[currentIndex - 1].HasForwardMotion;
			if (nextIndex >= order.Count)
			{
				nextIndex = (toggleFastest ? (currentIndex - 1) : (order.Count - 1));
			}
			return order[nextIndex];
		}
		return ebg.IdleGaits.TryGetValue(currentGait.Environment) ?? ebg.IdleGaits[EnumHabitat.Land];
	}

	protected virtual bool IsSprinting()
	{
		List<GaitMeta> order = RideableGaitOrder.TryGetValue(ebg.CurrentGait.Environment);
		if (order == null || order.Count < 2)
		{
			return false;
		}
		if (order[order.Count - 1].Code != ebg.CurrentGait.Code)
		{
			return false;
		}
		for (int i = order.Count - 2; i >= 0; i--)
		{
			if (order[i].HasForwardMotion)
			{
				return true;
			}
		}
		return false;
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		capi?.Event.UnregisterRenderer(this, EnumRenderStage.Before);
		if (entity.Api.Event is IServerEventAPI serverEvents)
		{
			serverEvents.MountGaitReceived -= ReceiveGaitFromClient;
		}
		if (entity.Api.Side == EnumAppSide.Server)
		{
			EntityBehaviorTaskAI taskAI = entity.GetBehavior<EntityBehaviorTaskAI>();
			if (taskAI != null)
			{
				taskAI.TaskManager.OnShouldExecuteTask -= TaskManager_OnShouldExecuteTask;
			}
			EntityBehaviorAttachable attachableBehavior = entity.GetBehavior<EntityBehaviorAttachable>();
			if (attachableBehavior != null)
			{
				attachableBehavior.Inventory.SlotModified -= Inventory_SlotModified;
			}
		}
		else
		{
			entity.WatchedAttributes.UnregisterListener(UpdateControlScheme);
			ebg.GaitChangedForEnvironmentDelegate -= BehaviorGait_OnGaitChangedForEnvironment;
		}
	}

	public virtual void UnmountPassengers()
	{
		IMountableSeat[] seats = base.Seats;
		for (int i = 0; i < seats.Length; i++)
		{
			(seats[i].Passenger as EntityAgent)?.TryUnmount();
		}
	}

	public override void OnEntityLoaded()
	{
		SetupTaskBlocker();
	}

	public override void OnEntitySpawn()
	{
		SetupTaskBlocker();
	}

	protected virtual void SetupTaskBlocker()
	{
		EntityBehaviorAttachable attachableBehavior = entity.GetBehavior<EntityBehaviorAttachable>();
		if (entity.Api.Side == EnumAppSide.Server)
		{
			EntityBehaviorTaskAI taskAI = entity.GetBehavior<EntityBehaviorTaskAI>();
			if (taskAI != null)
			{
				taskAI.TaskManager.OnShouldExecuteTask += TaskManager_OnShouldExecuteTask;
			}
			if (attachableBehavior != null)
			{
				attachableBehavior.Inventory.SlotModified += Inventory_SlotModified;
			}
		}
		else
		{
			if (attachableBehavior != null)
			{
				entity.WatchedAttributes.RegisterModifiedListener(attachableBehavior.InventoryClassName, UpdateControlScheme);
			}
			ebg.GaitChangedForEnvironmentDelegate += BehaviorGait_OnGaitChangedForEnvironment;
		}
	}

	protected virtual void Inventory_SlotModified(int obj)
	{
		UpdateControlScheme();
		ebg?.SetIdle();
	}

	protected virtual void UpdateControlScheme()
	{
		EntityBehaviorAttachable attachableBehavior = entity.GetBehavior<EntityBehaviorAttachable>();
		if (attachableBehavior == null)
		{
			return;
		}
		scheme = EnumControlScheme.Hold;
		foreach (ItemSlot slot in attachableBehavior.Inventory)
		{
			if (slot.Empty)
			{
				continue;
			}
			string sch = slot.Itemstack.ItemAttributes?["controlScheme"].AsString();
			if (sch != null)
			{
				if (Enum.TryParse<EnumControlScheme>(sch, out scheme))
				{
					break;
				}
				scheme = EnumControlScheme.Hold;
			}
		}
	}

	protected virtual void BehaviorGait_OnGaitChangedForEnvironment()
	{
		List<GaitMeta> order = RideableGaitOrder.TryGetValue(ebg.CurrentGait.Environment);
		if (order != null)
		{
			if (!order.Any((GaitMeta g) => g.Code == ebg.CurrentGait.Code))
			{
				SpeedUp(toggleFastest: false);
			}
			if (prevSprintKey && scheme == EnumControlScheme.Hold)
			{
				SpeedUp(toggleFastest: false);
			}
		}
	}

	protected virtual bool TaskManager_OnShouldExecuteTask(IAiTask task)
	{
		if (task is AiTaskWander && entity.Api.World.Calendar.TotalHours - LastDismountTotalHours < 24.0)
		{
			return false;
		}
		return !base.Seats.Any((IMountableSeat seat) => seat.Passenger != null);
	}

	public virtual void OnRenderFrame(float dt, EnumRenderStage stage)
	{
		if (!capi.IsGamePaused)
		{
			UpdateAngleAndMotion(dt);
		}
	}

	protected virtual void UpdateAngleAndMotion(float dt)
	{
		dt = Math.Min(0.5f, dt);
		float step = GlobalConstants.PhysicsFrameTime;
		double angularMotion = SeatsToMotion(step);
		if (jumpNow)
		{
			UpdateRidingState();
		}
		ebg.AngularVelocity = angularMotion;
		entity.Pos.Yaw += (float)angularMotion * dt * 30f;
		entity.Pos.Yaw = entity.Pos.Yaw % ((float)Math.PI * 2f);
		if (entity.World.ElapsedMilliseconds - lastJumpMs < 2000 && entity.World.ElapsedMilliseconds - lastJumpMs > 200 && entity.OnGround)
		{
			eagent.StopAnimation("jump");
			eagent.AnimManager.AnimationsDirty = true;
		}
	}

	public virtual double SeatsToMotion(float dt)
	{
		double angularMotion = 0.0;
		jumpNow = false;
		coyoteTimer -= dt;
		base.Controller = null;
		IMountableSeat[] seats = base.Seats;
		foreach (IMountableSeat seat in seats)
		{
			if (seat.Config.Controllable && seat.Passenger != null)
			{
				base.Controller = seat.Passenger;
				break;
			}
		}
		seats = base.Seats;
		foreach (IMountableSeat seat2 in seats)
		{
			if (entity.OnGround)
			{
				coyoteTimer = 0.15f;
			}
			if (seat2.Passenger == null)
			{
				continue;
			}
			if (seat2.Passenger is EntityPlayer eplr)
			{
				eplr.Controls.LeftMouseDown = seat2.Controls.LeftMouseDown;
				if (eplr.HeadYawLimits == null)
				{
					eplr.BodyYawLimits = new AngleConstraint(entity.Pos.Yaw + seat2.Config.MountRotation.Y * ((float)Math.PI / 180f), seat2.Config.BodyYawLimit ?? ((float)Math.PI / 2f));
					eplr.HeadYawLimits = new AngleConstraint(entity.Pos.Yaw + seat2.Config.MountRotation.Y * ((float)Math.PI / 180f), (float)Math.PI / 2f);
				}
				else
				{
					eplr.BodyYawLimits.X = entity.Pos.Yaw + seat2.Config.MountRotation.Y * ((float)Math.PI / 180f);
					eplr.BodyYawLimits.Y = seat2.Config.BodyYawLimit ?? ((float)Math.PI / 2f);
					eplr.HeadYawLimits.X = entity.Pos.Yaw + seat2.Config.MountRotation.Y * ((float)Math.PI / 180f);
					eplr.HeadYawLimits.Y = (float)Math.PI / 2f;
				}
			}
			if (base.Controller != seat2.Passenger)
			{
				continue;
			}
			EntityControls controls = seat2.Controls;
			bool canride = true;
			bool canturn = true;
			if (RemainingSaddleBreaks > 0)
			{
				if (entity.Api.World.Rand.NextDouble() < 0.05)
				{
					angularMotionWild = ((float)entity.Api.World.Rand.NextDouble() * 2f - 1f) / 10f;
				}
				angularMotion = angularMotionWild;
				canturn = false;
			}
			if (this.CanRide != null && (controls.Jump || controls.TriesToMove))
			{
				Delegate[] invocationList = this.CanRide.GetInvocationList();
				for (int j = 0; j < invocationList.Length; j++)
				{
					if (!((CanRideDelegate)invocationList[j])(seat2, out var errMsg))
					{
						if (capi != null && seat2.Passenger == capi.World.Player.Entity)
						{
							capi.TriggerIngameError(this, "cantride", Lang.Get("cantride-" + errMsg));
						}
						canride = false;
						break;
					}
				}
			}
			if (this.CanTurn != null && (controls.Left || controls.Right))
			{
				Delegate[] invocationList = this.CanTurn.GetInvocationList();
				for (int j = 0; j < invocationList.Length; j++)
				{
					if (!((CanRideDelegate)invocationList[j])(seat2, out var errMsg2))
					{
						if (capi != null && seat2.Passenger == capi.World.Player.Entity)
						{
							capi.TriggerIngameError(this, "cantride", Lang.Get("cantride-" + errMsg2));
						}
						canturn = false;
						break;
					}
				}
			}
			if (!canride)
			{
				continue;
			}
			if (controls.Jump && entity.World.ElapsedMilliseconds - lastJumpMs > 1500 && entity.Alive && (entity.OnGround || coyoteTimer > 0f || (entity.Api.Side == EnumAppSide.Client && entity.EntityId != base.Controller.EntityId)))
			{
				lastJumpMs = entity.World.ElapsedMilliseconds;
				jumpNow = true;
			}
			if (entity.Api.Side != EnumAppSide.Server)
			{
				bool wasIdle = ebg.IsIdle;
				if (controls.Forward && !prevForwardKey && !prevPrevForwardKey)
				{
					SpeedUp(toggleFastest: false);
				}
				if (controls.Backward && !prevBackwardKey && !prevPrevBackwardKey)
				{
					SlowDown();
				}
				if (controls.Sprint && (wasIdle || (!prevSprintKey && !prevPrevSprintKey)))
				{
					if (ebg.CurrentGait.HasForwardMotion)
					{
						SpeedUp(toggleFastest: true);
					}
					else if (ebg.CurrentGait.HasBackwardMotion)
					{
						SlowDown();
					}
				}
				if (scheme == EnumControlScheme.Hold)
				{
					if (IsSprinting() && !controls.Sprint)
					{
						SlowDown();
					}
					if ((!controls.Forward && !controls.Backward && !ebg.IsIdle) || (!controls.Forward && ebg.CurrentGait.HasForwardMotion) || (!controls.Backward && ebg.CurrentGait.HasBackwardMotion))
					{
						ebg.SetIdle();
					}
					if (controls.Forward && !controls.Backward && !ebg.CurrentGait.HasForwardMotion)
					{
						SpeedUp(toggleFastest: false);
					}
					if (controls.Backward && !controls.Forward && !ebg.CurrentGait.HasBackwardMotion)
					{
						SlowDown();
					}
				}
				prevPrevForwardKey = prevForwardKey;
				prevPrevBackwardKey = prevBackwardKey;
				prevPrevSprintKey = prevSprintKey;
				prevForwardKey = controls.Forward;
				prevBackwardKey = controls.Backward;
				prevSprintKey = controls.Sprint;
			}
			if (canturn && (controls.Left || controls.Right))
			{
				float dir = (controls.Left ? 1 : (-1));
				angularMotion += (double)(ebg.GetYawMultiplier() * dir * dt);
			}
		}
		return angularMotion;
	}

	protected virtual void UpdateRidingState()
	{
		if (!AnyMounted())
		{
			return;
		}
		if (RemainingSaddleBreaks > 0)
		{
			if (entity.Api.World.Rand.NextDouble() < 0.05)
			{
				jumpNow = true;
			}
			if (saddleBreakGait != null)
			{
				ebg.CurrentGait = saddleBreakGait;
			}
			if (entity.Api.World.ElapsedMilliseconds - mountedTotalMs > 4000)
			{
				DoSaddleBreak();
				return;
			}
		}
		bool isInMidJump = IsInMidJump;
		IsInMidJump &= (entity.World.ElapsedMilliseconds - lastJumpMs < 500 || !entity.OnGround) && !entity.Swimming;
		if (isInMidJump && !IsInMidJump)
		{
			ControlMeta meta = Controls["jump"];
			IMountableSeat[] seats = base.Seats;
			foreach (IMountableSeat seat in seats)
			{
				AnimationMetaData anim = meta.GetSeatAnimation(seat);
				if (anim != null)
				{
					seat.Passenger?.AnimManager?.StopAnimation(anim.Code);
				}
			}
		}
		ControlMeta nowControlMeta = Controls.FirstOrDefault<KeyValuePair<string, ControlMeta>>((KeyValuePair<string, ControlMeta> c) => c.Key == ebg.CurrentGait.Code).Value;
		if (nowControlMeta == null)
		{
			nowControlMeta = Controls["idle"];
		}
		eagent.Controls.Jump = jumpNow;
		if (jumpNow)
		{
			IsInMidJump = true;
			jumpNow = false;
			if (eagent.Properties.Client.Renderer is EntityShapeRenderer esr)
			{
				esr.LastJumpMs = capi.InWorldEllapsedMilliseconds;
			}
			nowControlMeta = Controls["jump"];
			entity.AnimManager.StartAnimation(ebg.Gaits["jump"]);
			eagent.AnimManager.AnimationsDirty = true;
			IPlayer player = ((entity is EntityPlayer entityPlayer) ? entityPlayer.World.PlayerByUid(entityPlayer.PlayerUID) : null);
			entity.PlayEntitySound("jump", player);
		}
		curControlMeta = nowControlMeta;
	}

	protected virtual void DoSaddleBreak()
	{
		IMountableSeat[] seats = base.Seats;
		foreach (IMountableSeat seat in seats)
		{
			if (seat?.Passenger != null)
			{
				EntityAgent eagent = seat.Passenger as EntityAgent;
				if (entity.Api.World.Rand.NextDouble() < 0.5)
				{
					eagent?.ReceiveDamage(new DamageSource
					{
						CauseEntity = entity,
						DamageTier = 1,
						Source = EnumDamageSource.Entity,
						SourcePos = base.Position.XYZ,
						Type = EnumDamageType.BluntAttack
					}, 1f + (float)entity.Api.World.Rand.Next(8) / 4f);
				}
				eagent?.TryUnmount();
			}
		}
		jumpNow = false;
		ebg.SetIdle();
		if (entity.Api.World.Calendar.TotalDays - LastSaddleBreakTotalDays > (double)saddleBreakDayInterval)
		{
			RemainingSaddleBreaks--;
			LastSaddleBreakTotalDays = entity.Api.World.Calendar.TotalDays;
			if (RemainingSaddleBreaks <= 0)
			{
				ConvertToTamedAnimal();
			}
		}
	}

	protected virtual void ConvertToTamedAnimal()
	{
		ICoreAPI api = entity.World.Api;
		if (api.Side != EnumAppSide.Client)
		{
			EntityProperties etype = api.World.GetEntityType(AssetLocation.Create(tamedEntityCode, entity.Code.Domain));
			if (etype != null)
			{
				Entity entitytamed = api.World.ClassRegistry.CreateEntity(etype);
				entitytamed.Pos.SetFrom(entity.Pos);
				entitytamed.WatchedAttributes = entity.WatchedAttributes.Clone();
				entity.Die(EnumDespawnReason.Expire);
				api.World.SpawnEntity(entitytamed);
			}
		}
	}

	public override void OnEntityDeath(DamageSource damageSourceForDeath)
	{
		IMountableSeat[] seats = base.Seats;
		for (int i = 0; i < seats.Length; i++)
		{
			(seats[i]?.Entity as EntityAgent)?.TryUnmount();
		}
		base.OnEntityDeath(damageSourceForDeath);
	}

	public override void OnGameTick(float dt)
	{
		if (entity.Api.Side == EnumAppSide.Server)
		{
			UpdateAngleAndMotion(dt);
		}
		UpdateRidingState();
		if (!AnyMounted() && eagent.Controls.TriesToMove && eagent?.MountedOn != null)
		{
			eagent.TryUnmount();
		}
	}

	public override string PropertyName()
	{
		return "rideable";
	}

	public void Dispose()
	{
	}

	public virtual void DidUnmount(EntityAgent entityAgent)
	{
		ebg.SetIdle();
		curControlMeta = null;
		LastDismountTotalHours = entity.World.Calendar.TotalHours;
		if (capi == null)
		{
			return;
		}
		IMountable iMountable = entity.GetInterface<IMountable>();
		if (iMountable?.Controller != null && iMountable.Controller.EntityId == capi.World.Player.Entity.EntityId)
		{
			EntityBehaviorInterpolatePosition ebInterpolate = entity.GetBehavior<EntityBehaviorInterpolatePosition>();
			if (ebInterpolate != null)
			{
				EnumHandling handling = EnumHandling.Handled;
				ebInterpolate.OnReceivedServerPos(isTeleport: true, ref handling);
			}
		}
	}

	public virtual void DidMount(EntityAgent entityAgent)
	{
		UpdateControlScheme();
		mountedTotalMs = entity.Api.World.ElapsedMilliseconds;
	}

	public override bool ToleratesDamageFrom(Entity eOther, ref EnumHandling handling)
	{
		if (eOther != null && base.Controller == eOther)
		{
			handling = EnumHandling.PreventDefault;
			return true;
		}
		return false;
	}

	public override void GetInfoText(StringBuilder infotext)
	{
		if (RemainingSaddleBreaks > 0)
		{
			infotext.AppendLine(Lang.Get("required-saddlebreak-amount-every-day-amount-to-fully-tame", RemainingSaddleBreaks, saddleBreakDayInterval));
		}
		base.GetInfoText(infotext);
	}
}
