using System;
using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("gaits", "List of gaits in order of increasing speed for the rideable entity", "Vintagestory.GameContent.GaitMeta[]", "Required", "", false)]
[AddDocumentationProperty("idleGait", "The code of idle gait used when no other gait is triggered", "System.String", "Required", "idle", false)]
public class EntityBehaviorGait : EntityBehavior
{
	protected EntityAgent eagent;

	public readonly FastSmallDictionary<string, GaitMeta> Gaits = new FastSmallDictionary<string, GaitMeta>(1);

	protected GaitMeta currentGait;

	protected string? currentTurnAnimation;

	public double AngularVelocity;

	public double MoveSpeedModifier = 1.0;

	protected ILoadedSound? gaitSound;

	public FastSmallDictionary<EnumHabitat, GaitMeta> IdleGaits = new FastSmallDictionary<EnumHabitat, GaitMeta>(1);

	protected float notOnGroundAccum;

	public GaitMeta CurrentGait
	{
		get
		{
			return currentGait;
		}
		set
		{
			GaitMeta prevGait = currentGait;
			currentGait = value;
			if (entity.WatchedAttributes.GetString("currentgait") != value.Code)
			{
				entity.WatchedAttributes.SetString("currentgait", value.Code);
				OnGaitChangedFrom(prevGait);
			}
		}
	}

	public GaitMeta FallbackGait
	{
		get
		{
			if (CurrentGait.FallbackGaitCode != null)
			{
				return Gaits[CurrentGait.FallbackGaitCode];
			}
			return IdleGaits[CurrentGait.Environment];
		}
	}

	public bool IsIdle => IsIdleGait(CurrentGait);

	public event Action? GaitChangedForEnvironmentDelegate;

	public override string PropertyName()
	{
		return "gait";
	}

	public float GetYawMultiplier()
	{
		return CurrentGait?.YawMultiplier ?? 3.5f;
	}

	public virtual void SetIdle()
	{
		CurrentGait = IdleGaits.TryGetValue(CurrentGait.Environment) ?? IdleGaits[EnumHabitat.Land];
	}

	public virtual bool IsIdleGait(GaitMeta gait)
	{
		return gait == IdleGaits.TryGetValue(gait.Environment);
	}

	public GaitMeta CascadingFallbackGait(int n)
	{
		GaitMeta result = CurrentGait;
		while (n > 0)
		{
			result = ((result.FallbackGaitCode == null) ? IdleGaits[result.Environment] : Gaits[result.FallbackGaitCode]);
			n--;
		}
		return result;
	}

	public EntityBehaviorGait(Entity entity)
		: base(entity)
	{
		eagent = (EntityAgent)entity;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		ArgumentNullException.ThrowIfNull(attributes, "attributes");
		base.Initialize(properties, attributes);
		GaitMeta?[]? array = attributes["gaits"].AsArray<GaitMeta>();
		ArgumentNullException.ThrowIfNull(array, "gaitarray");
		GaitMeta[] array2 = array;
		foreach (GaitMeta gait in array2)
		{
			ArgumentNullException.ThrowIfNull(gait, "gait");
			gait.ClientSide = true;
			Gaits[gait.Code] = gait;
		}
		FastSmallDictionary<EnumHabitat, string> idleGaitCodes = attributes["idleGaits"].AsObject<FastSmallDictionary<EnumHabitat, string>>();
		if (idleGaitCodes != null)
		{
			foreach (KeyValuePair<EnumHabitat, string> entry in idleGaitCodes)
			{
				IdleGaits[entry.Key] = Gaits[entry.Value];
			}
			if (!IdleGaits.ContainsKey(EnumHabitat.Land))
			{
				throw new ArgumentException("JSON error. An idle gait usable on land must be provided for {0}", entity.Code);
			}
		}
		else
		{
			string idleGaitCode = attributes["idleGait"]?.AsString("idle");
			if (idleGaitCode == null || !Gaits.TryGetValue(idleGaitCode, out GaitMeta idle))
			{
				throw new ArgumentException("JSON error. No idle gait for {0}", entity.Code);
			}
			IdleGaits[EnumHabitat.Land] = idle;
		}
		if (entity.Api.Side == EnumAppSide.Client)
		{
			entity.WatchedAttributes.RegisterModifiedListener("currentgait", OnGaitChanged);
		}
		CurrentGait = IdleGaits[EnumHabitat.Land];
		if (entity.Api is ICoreClientAPI capi)
		{
			capi.Event.PauseResume += OnGamePauseResume;
		}
	}

	protected virtual void OnGaitChanged()
	{
		OnGaitChangedFrom(currentGait);
	}

	protected virtual void OnGaitChangedFrom(GaitMeta? prevGait)
	{
		currentGait = Gaits[entity.WatchedAttributes.GetString("currentgait")];
		if (currentGait.Code == prevGait?.Code)
		{
			return;
		}
		if (entity.Api is ICoreClientAPI capi && CurrentGait.Sound != prevGait?.Sound)
		{
			gaitSound?.Stop();
			gaitSound?.Dispose();
			gaitSound = null;
			if (CurrentGait.Sound != null)
			{
				gaitSound = capi.World.LoadSound(new SoundParams
				{
					Location = CurrentGait.Sound.Clone().WithPathPrefix("sounds/"),
					DisposeOnFinish = false,
					Position = entity.Pos.XYZ.ToVec3f(),
					ShouldLoop = true
				});
			}
			gaitSound?.Start();
		}
		if (CurrentGait.Animation != prevGait?.Animation)
		{
			if (prevGait != null && prevGait.Animation != null && prevGait.Animation != "jump")
			{
				eagent.AnimManager.StopAnimation(prevGait.Animation);
			}
			if (CurrentGait.Animation != null)
			{
				eagent.AnimManager.StartAnimation(CurrentGait);
				eagent.AnimManager.AnimationsDirty = true;
			}
		}
		if (CurrentGait.MoveSpeed == 0f)
		{
			eagent.Controls.StopAllMovement();
			if (currentTurnAnimation != null)
			{
				eagent.StopAnimation(currentTurnAnimation);
				eagent.AnimManager.AnimationsDirty = true;
				currentTurnAnimation = null;
			}
		}
	}

	protected virtual void OnGamePauseResume(bool isPaused)
	{
		if (isPaused)
		{
			gaitSound?.Pause();
			return;
		}
		ILoadedSound? loadedSound = gaitSound;
		if (loadedSound != null && loadedSound.IsPaused)
		{
			gaitSound?.Start();
		}
	}

	public override void OnGameTick(float dt)
	{
		UpdateGaitForEnvironment();
		if (entity.Api.Side != EnumAppSide.Client)
		{
			return;
		}
		if (entity.OnGround)
		{
			notOnGroundAccum = 0f;
		}
		else
		{
			notOnGroundAccum += dt;
		}
		if (gaitSound != null)
		{
			gaitSound.SetPosition((float)entity.Pos.X, (float)entity.Pos.Y, (float)entity.Pos.Z);
			if ((double)notOnGroundAccum > 0.2)
			{
				if (!gaitSound.IsPaused)
				{
					gaitSound.Pause();
				}
			}
			else if (gaitSound.IsPaused)
			{
				gaitSound.Start();
			}
		}
		Move(dt);
		UpdateTurnAnimation();
	}

	public virtual void UpdateGaitForEnvironment()
	{
		EnumHabitat targetEnvironment = ((!entity.Swimming) ? EnumHabitat.Land : EnumHabitat.Sea);
		if (CurrentGait.Environment == targetEnvironment)
		{
			return;
		}
		GaitMeta closest = null;
		foreach (GaitMeta gait in Gaits.Values)
		{
			if (gait.Natural && gait.Environment == targetEnvironment && gait.Direction == CurrentGait.Direction && (closest == null || Math.Abs(gait.MoveSpeed - CurrentGait.MoveSpeed) < Math.Abs(closest.MoveSpeed - CurrentGait.MoveSpeed)))
			{
				closest = gait;
			}
		}
		CurrentGait = closest;
		this.GaitChangedForEnvironmentDelegate?.Invoke();
	}

	protected virtual void Move(float dt)
	{
		EntityControls controls = eagent.Controls;
		double cosYaw = Math.Cos(entity.Pos.Yaw);
		double sinYaw = Math.Sin(entity.Pos.Yaw);
		controls.WalkVector.Set(sinYaw, 0.0, cosYaw);
		controls.WalkVector.Mul((double)(CurrentGait.MoveSpeed * GlobalConstants.OverallSpeedMultiplier * (float)CurrentGait.Direction) * MoveSpeedModifier);
		if (entity.Properties.RotateModelOnClimb && controls.IsClimbing && entity.ClimbingOnFace != null && entity.Alive)
		{
			BlockFacing climbingOnFace = entity.ClimbingOnFace;
			if (Math.Sign(climbingOnFace.Normali.X) == Math.Sign(controls.WalkVector.X))
			{
				controls.WalkVector.X = 0.0;
			}
			if (Math.Sign(climbingOnFace.Normali.Z) == Math.Sign(controls.WalkVector.Z))
			{
				controls.WalkVector.Z = 0.0;
			}
		}
		if (entity.Swimming)
		{
			controls.FlyVector.Set(controls.WalkVector);
			Vec3d pos = entity.Pos.XYZ;
			Block inblock = entity.World.BlockAccessor.GetBlockRaw((int)pos.X, (int)pos.Y, (int)pos.Z, 2);
			Block aboveblock = entity.World.BlockAccessor.GetBlockRaw((int)pos.X, (int)(pos.Y + 1.0), (int)pos.Z, 2);
			float swimlineSubmergedness = GameMath.Clamp((float)(int)pos.Y + (float)inblock.LiquidLevel / 8f + (aboveblock.IsLiquid() ? 1.125f : 0f) - (float)pos.Y - (float)entity.SwimmingOffsetY, 0f, 1f);
			swimlineSubmergedness = Math.Min(1f, swimlineSubmergedness + 0.075f);
			controls.FlyVector.Y = GameMath.Clamp(controls.FlyVector.Y, 0.019999999552965164, 0.03999999910593033) * (double)swimlineSubmergedness * 3.0;
			if (entity.CollidedHorizontally)
			{
				controls.FlyVector.Y = 0.05000000074505806;
			}
			eagent.Pos.Motion.Y += ((double)swimlineSubmergedness - 0.1) / 300.0;
		}
	}

	protected virtual void UpdateTurnAnimation()
	{
		string nowTurnAnim = null;
		if (!CurrentGait.HasBackwardMotion)
		{
			if (AngularVelocity > 0.001)
			{
				nowTurnAnim = "turn-left";
			}
			else if (AngularVelocity < -0.001)
			{
				nowTurnAnim = "turn-right";
			}
		}
		if (nowTurnAnim != currentTurnAnimation)
		{
			if (currentTurnAnimation != null)
			{
				eagent.StopAnimation(currentTurnAnimation);
				eagent.AnimManager.AnimationsDirty = true;
			}
			if (nowTurnAnim == null)
			{
				currentTurnAnimation = null;
				return;
			}
			string anim = (currentTurnAnimation = ((!CurrentGait.HasMotion) ? "idle-" : "") + nowTurnAnim);
			eagent.StartAnimation(anim);
		}
	}
}
