using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

/// <summary>
/// A goal-directed entity which observes and acts upon an environment
/// </summary>
public class EntityAgent : Entity
{
	public enum EntityServerPacketId
	{
		Teleport = 1,
		Revive = 196,
		Emote = 197,
		Death = 198,
		Hurt = 199,
		PlayPlayerAnim = 200,
		PlayMusic = 201,
		StopMusic = 202,
		Talk = 203
	}

	public enum EntityClientPacketId
	{
		SitfloorEdge = 296
	}

	public float sidewaysSwivelAngle;

	/// <summary>
	/// True if all clients have to be informed about this entities death. Set to false once all clients have been notified
	/// </summary>
	public bool DeadNotify;

	protected long herdId;

	protected EntityControls controls;

	protected EntityControls servercontrols;

	protected bool alwaysRunIdle;

	public EnumEntityActivity CurrentControls;

	/// <summary>
	/// Whether or not the entity is allowed to despawn (Default: true)
	/// </summary>
	public bool AllowDespawn = true;

	protected bool RotateModelOnClimb;

	public AnimationMetaData curMountedAnim;

	protected bool ignoreTeleportCall;

	/// <summary>
	/// updated by GetWalkSpeedMultiplier()
	/// </summary>
	protected Block insideBlock;

	/// <summary>
	/// updated by GetWalkSpeedMultiplier()
	/// </summary>
	protected BlockPos insidePos = new BlockPos(0);

	public override bool IsCreature => true;

	/// <summary>
	/// No swivel when we are mounted
	/// </summary>
	public override bool CanSwivel
	{
		get
		{
			if (base.CanSwivel)
			{
				return MountedOn == null;
			}
			return false;
		}
	}

	public override bool CanStepPitch
	{
		get
		{
			if (base.CanStepPitch)
			{
				return MountedOn == null;
			}
			return false;
		}
	}

	/// <summary>
	/// The yaw of the agents body
	/// </summary>
	public virtual float BodyYaw { get; set; }

	/// <summary>
	/// The yaw of the agents body on the client, retrieved from the server (BehaviorInterpolatePosition lerps this value and sets BodyYaw)
	/// </summary>
	public virtual float BodyYawServer { get; set; }

	/// <summary>
	/// Unique identifier for a herd
	/// </summary>
	public long HerdId
	{
		get
		{
			return herdId;
		}
		set
		{
			WatchedAttributes.SetLong("herdId", value);
			herdId = value;
		}
	}

	public IMountableSeat MountedOn { get; protected set; }

	internal virtual bool LoadControlsFromServer => true;

	/// <summary>
	/// Item in the left hand slot of the entity agent.
	/// </summary>
	public virtual ItemSlot LeftHandItemSlot { get; set; }

	/// <summary>
	/// Item in the right hand slot of the entity agent.
	/// </summary>
	public virtual ItemSlot RightHandItemSlot { get; set; }

	public virtual ItemSlot ActiveHandItemSlot => RightHandItemSlot;

	/// <summary>
	/// Whether or not the entity should despawn.
	/// </summary>
	public override bool ShouldDespawn
	{
		get
		{
			if (!Alive)
			{
				return AllowDespawn;
			}
			return false;
		}
	}

	/// <summary>
	/// The controls for this entity.
	/// </summary>
	public EntityControls Controls => controls;

	/// <summary>
	/// The server controls for this entity
	/// </summary>
	public EntityControls ServerControls => servercontrols;

	public EntityAgent()
	{
		controls = new EntityControls();
		servercontrols = new EntityControls();
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
		RotateModelOnClimb = properties.RotateModelOnClimb;
		if (World.Side == EnumAppSide.Server)
		{
			servercontrols = controls;
		}
		WatchedAttributes.RegisterModifiedListener("mountedOn", updateMountedState);
		if (WatchedAttributes["mountedOn"] != null)
		{
			MountedOn = World.ClassRegistry.GetMountable(WatchedAttributes["mountedOn"] as TreeAttribute);
			if (MountedOn != null && TryMount(MountedOn) && Api.Side == EnumAppSide.Server)
			{
				Entity entity = MountedOn.MountSupplier?.OnEntity;
				if (entity != null)
				{
					Api.World.Logger.Audit("{0} loaded already mounted/seated on a {1} at {2}", GetName(), entity.Code.ToShortString(), entity.Pos.AsBlockPos);
				}
			}
		}
		herdId = WatchedAttributes.GetLong("herdId", 0L);
	}

	public override void OnEntitySpawn()
	{
		base.OnEntitySpawn();
		if (!WatchedAttributes.GetBool("noSpawnAnim"))
		{
			string spawnAnim = base.Properties.Attributes?["spawnAnimation"].AsString();
			if (spawnAnim != null)
			{
				StartAnimation(spawnAnim);
			}
		}
	}

	/// <summary>
	/// Are the eyes of this entity submerged in liquid?
	/// </summary>
	/// <returns></returns>
	public bool IsEyesSubmerged()
	{
		BlockPos pos = base.Pos.AsBlockPos.Add(0f, (float)(Swimming ? base.Properties.SwimmingEyeHeight : base.Properties.EyeHeight), 0f);
		return World.BlockAccessor.GetBlock(pos).MatterState == EnumMatterState.Liquid;
	}

	/// <summary>
	/// Attempts to mount this entity on a target.
	/// </summary>
	/// <param name="onmount">The mount to mount</param>
	/// <returns>Whether it was mounted or not.</returns>
	public virtual bool TryMount(IMountableSeat onmount)
	{
		if (!onmount.CanMount(this))
		{
			return false;
		}
		onmount.Controls.FromInt(Controls.ToInt());
		if (MountedOn != null && MountedOn != onmount)
		{
			IMountableSeat seat = MountedOn.MountSupplier.GetSeatOfMountedEntity(this);
			if (seat != null)
			{
				seat.DoTeleportOnUnmount = false;
			}
			if (!TryUnmount())
			{
				return false;
			}
			if (seat != null)
			{
				seat.DoTeleportOnUnmount = true;
			}
		}
		doMount(onmount);
		TreeAttribute mountableTree = new TreeAttribute();
		onmount.MountableToTreeAttributes(mountableTree);
		WatchedAttributes["mountedOn"] = mountableTree;
		if (World.Side == EnumAppSide.Server)
		{
			EntityBehavior sebh = MountedOn?.MountSupplier.OnEntity?.GetBehavior("seatable");
			if (sebh != null)
			{
				sebh.ToBytes(forClient: true);
				MountedOn?.MountSupplier.OnEntity.WatchedAttributes.MarkPathDirty("seatdata");
			}
			WatchedAttributes.MarkPathDirty("mountedOn");
		}
		return true;
	}

	protected virtual void updateMountedState()
	{
		if (WatchedAttributes.HasAttribute("mountedOn"))
		{
			IMountableSeat mountable = World.ClassRegistry.GetMountable(WatchedAttributes["mountedOn"] as TreeAttribute);
			IMountableSeat seat = MountedOn?.MountSupplier.GetSeatOfMountedEntity(this);
			if (MountedOn != null && mountable != null && (MountedOn.Entity?.EntityId != mountable.Entity?.EntityId || mountable.SeatId != seat?.SeatId))
			{
				if (seat != null)
				{
					seat.DoTeleportOnUnmount = false;
				}
				TryUnmount();
				if (seat != null)
				{
					seat.DoTeleportOnUnmount = true;
				}
			}
			if (MountedOn == null || (mountable != null && mountable.SeatId != seat?.SeatId))
			{
				doMount(mountable);
			}
		}
		else
		{
			TryUnmount();
		}
	}

	protected virtual void doMount(IMountableSeat mountable)
	{
		MountedOn = mountable;
		controls.StopAllMovement();
		if (mountable == null)
		{
			WatchedAttributes.RemoveAttribute("mountedOn");
			return;
		}
		mountable.Entity?.AnimManager?.StopAllAnimations();
		if (MountedOn?.SuggestedAnimation != null)
		{
			curMountedAnim = MountedOn.SuggestedAnimation;
			AnimManager?.StopAllAnimations();
			AnimManager?.StartAnimation(curMountedAnim);
		}
		mountable.DidMount(this);
	}

	/// <summary>
	/// Attempts to un-mount the player.
	/// </summary>
	/// <returns>Whether or not unmounting was successful</returns>
	public bool TryUnmount()
	{
		IMountableSeat mountedOn = MountedOn;
		if (mountedOn != null && !mountedOn.CanUnmount(this))
		{
			return false;
		}
		if (curMountedAnim != null)
		{
			AnimManager?.StopAnimation(curMountedAnim.Animation);
			curMountedAnim = null;
		}
		IMountableSeat oldMountedOn = MountedOn;
		MountedOn = null;
		oldMountedOn?.DidUnmount(this);
		if (WatchedAttributes.HasAttribute("mountedOn"))
		{
			WatchedAttributes.RemoveAttribute("mountedOn");
		}
		if (World.Side == EnumAppSide.Server)
		{
			EntityBehavior sebh = oldMountedOn?.MountSupplier.OnEntity?.GetBehavior("seatable");
			if (sebh != null)
			{
				sebh.ToBytes(forClient: true);
				oldMountedOn?.MountSupplier.OnEntity.WatchedAttributes.MarkPathDirty("seatdata");
			}
		}
		if (Api.Side == EnumAppSide.Server && oldMountedOn != null)
		{
			Entity entity = oldMountedOn.MountSupplier?.OnEntity;
			if (entity != null)
			{
				Api.World.Logger.Audit("{0} dismounts/disembarks from a {1} at {2}", GetName(), entity.Code.ToShortString(), entity.Pos.AsBlockPos);
			}
		}
		return true;
	}

	public override void Die(EnumDespawnReason reason = EnumDespawnReason.Death, DamageSource damageSourceForDeath = null)
	{
		if (Alive && reason == EnumDespawnReason.Death)
		{
			PlayEntitySound("death");
			if (damageSourceForDeath?.GetCauseEntity() is EntityPlayer player)
			{
				Api.Logger.Audit("Player {0} killed {1} at {2}", player.GetName(), Code, base.Pos.AsBlockPos);
			}
		}
		if (reason != EnumDespawnReason.Death)
		{
			AllowDespawn = true;
		}
		controls.WalkVector.Set(0.0, 0.0, 0.0);
		controls.FlyVector.Set(0.0, 0.0, 0.0);
		ClimbingOnFace = null;
		base.Die(reason, damageSourceForDeath);
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		if (despawn != null && despawn.Reason == EnumDespawnReason.Removed && (this is EntityHumanoid || MountedOn != null))
		{
			TryUnmount();
		}
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
	{
		EnumHandling handled = EnumHandling.PassThrough;
		foreach (EntityBehavior behavior in base.SidedProperties.Behaviors)
		{
			behavior.OnInteract(byEntity, slot, hitPosition, mode, ref handled);
			if (handled == EnumHandling.PreventSubsequent)
			{
				break;
			}
		}
		if (handled == EnumHandling.PreventDefault || handled == EnumHandling.PreventSubsequent || mode != EnumInteractMode.Attack)
		{
			return;
		}
		float damage = ((slot.Itemstack == null) ? 0.5f : slot.Itemstack.Collectible.GetAttackPower(slot.Itemstack));
		int damagetier = ((slot.Itemstack != null) ? slot.Itemstack.Collectible.GetToolTier(slot) : 0);
		float dmgMultiplier = byEntity.Stats.GetBlended("meleeWeaponsDamage");
		JsonObject attributes = base.Properties.Attributes;
		if (attributes != null && attributes["isMechanical"].AsBool())
		{
			dmgMultiplier *= byEntity.Stats.GetBlended("mechanicalsDamage");
		}
		damage *= dmgMultiplier;
		bool isCriticalhit = false;
		damage = slot.Itemstack?.Collectible.GetDamageToEntity(damage, this, slot.Itemstack, ref isCriticalhit) ?? damage;
		if (isCriticalhit)
		{
			Vec2f sele = base.Properties.SelectionBoxSize;
			World.SpawnParticles(30f, -1, base.Pos.XYZ.AddCopy((0f - sele.X) * 0.9f, 0f, (0f - sele.X) * 0.9f), base.Pos.XYZ.AddCopy(sele.X * 0.9f, sele.Y, sele.X * 0.9f), new Vec3f(-0.5f, -0.5f, -0.5f), new Vec3f(0.5f, 0.5f, 0.5f), 0.15f, 0f, 0.2f);
		}
		IPlayer byPlayer = null;
		if (byEntity is EntityPlayer && !IsActivityRunning("invulnerable"))
		{
			byPlayer = (byEntity as EntityPlayer).Player;
			World.PlaySoundAt(new AssetLocation("sounds/player/slap"), base.Pos.X, base.Pos.InternalY, base.Pos.Z, byPlayer);
			slot?.Itemstack?.Collectible.OnAttackingWith(byEntity.World, byEntity, this, slot);
		}
		if (Api.Side == EnumAppSide.Client && damage > 1f && !IsActivityRunning("invulnerable"))
		{
			JsonObject attributes2 = base.Properties.Attributes;
			if (attributes2 != null && attributes2["spawnDamageParticles"].AsBool())
			{
				Vec3d vec3d = base.Pos.XYZ + hitPosition;
				Vec3d minPos = vec3d.AddCopy(-0.15, -0.15, -0.15);
				Vec3d maxPos = vec3d.AddCopy(0.15, 0.15, 0.15);
				int textureSubId = base.Properties.Client.FirstTexture.Baked.TextureSubId;
				Vec3f tmp = new Vec3f();
				for (int i = 0; i < 10; i++)
				{
					int color = (Api as ICoreClientAPI).EntityTextureAtlas.GetRandomColor(textureSubId);
					tmp.Set(1f - 2f * (float)World.Rand.NextDouble(), 2f * (float)World.Rand.NextDouble(), 1f - 2f * (float)World.Rand.NextDouble());
					World.SpawnParticles(1f, color, minPos, maxPos, tmp, tmp, 1.5f, 1f, 0.25f + (float)World.Rand.NextDouble() * 0.25f, EnumParticleModel.Cube, byPlayer);
				}
			}
		}
		DamageSource dmgSource = new DamageSource
		{
			Source = (((byEntity as EntityPlayer).Player != null) ? EnumDamageSource.Player : EnumDamageSource.Entity),
			SourceEntity = byEntity,
			Type = EnumDamageType.BluntAttack,
			HitPosition = hitPosition,
			DamageTier = damagetier
		};
		IMountable im = GetInterface<IMountable>();
		if ((im == null || im.Controller != byEntity) && ReceiveDamage(dmgSource, damage))
		{
			byEntity.DidAttack(dmgSource, this);
		}
	}

	public override void TeleportToDouble(double x, double y, double z, Action onTeleported = null)
	{
		if (ignoreTeleportCall)
		{
			return;
		}
		ignoreTeleportCall = true;
		if (MountedOn != null)
		{
			if (MountedOn.Entity != null)
			{
				MountedOn.Entity.TeleportToDouble(x, y, z, onTeleported);
				ignoreTeleportCall = false;
				return;
			}
			TryUnmount();
		}
		if (Code.Path.StartsWith("villager") && Api is ICoreServerAPI sapi)
		{
			string name = "-";
			try
			{
				name = WatchedAttributes.GetTreeAttribute("nametag")?.GetString("name") ?? "-";
			}
			catch (Exception)
			{
			}
			Vec3d target = new Vec3d(x, y, z);
			sapi.Logger.Log(EnumLogType.Worldgen, "In " + sapi.World.WorldName + ", villager " + name + " teleported to " + target.AsBlockPos);
		}
		base.TeleportToDouble(x, y, z, onTeleported);
		ignoreTeleportCall = false;
	}

	public virtual void DidAttack(DamageSource source, EntityAgent targetEntity)
	{
		EnumHandling handled = EnumHandling.PassThrough;
		foreach (EntityBehavior behavior in base.SidedProperties.Behaviors)
		{
			behavior.DidAttack(source, targetEntity, ref handled);
			if (handled == EnumHandling.PreventSubsequent)
			{
				break;
			}
		}
		if (handled != EnumHandling.PreventDefault)
		{
			_ = 3;
		}
	}

	public override bool ShouldReceiveDamage(DamageSource damageSource, float damage)
	{
		if (!Alive)
		{
			return false;
		}
		if (damageSource.CauseEntity is EntityPlayer fromPlayer && !fromPlayer.Player.HasPrivilege("attackcreatures"))
		{
			return false;
		}
		return true;
	}

	public override bool ReceiveDamage(DamageSource damageSource, float damage)
	{
		return base.ReceiveDamage(damageSource, damage);
	}

	/// <summary>
	/// Recieves the saturation from a food source.
	/// </summary>
	/// <param name="saturation">The amount of saturation recieved.</param>
	/// <param name="foodCat">The cat of food... err Category of food.</param>
	/// <param name="saturationLossDelay">The delay before the loss of saturation</param>
	/// <param name="nutritionGainMultiplier"></param>
	public virtual void ReceiveSaturation(float saturation, EnumFoodCategory foodCat = EnumFoodCategory.Unknown, float saturationLossDelay = 10f, float nutritionGainMultiplier = 1f)
	{
		if (!Alive || !ShouldReceiveSaturation(saturation, foodCat, saturationLossDelay))
		{
			return;
		}
		foreach (EntityBehavior behavior in base.SidedProperties.Behaviors)
		{
			behavior.OnEntityReceiveSaturation(saturation, foodCat, saturationLossDelay, nutritionGainMultiplier);
		}
	}

	/// <summary>
	/// Whether or not the target should recieve saturation.
	/// </summary>
	/// <param name="saturation">The amount of saturation recieved.</param>
	/// <param name="foodCat">The cat of food... err Category of food.</param>
	/// <param name="saturationLossDelay">The delay before the loss of saturation</param>
	/// <param name="nutritionGainMultiplier"></param>
	public virtual bool ShouldReceiveSaturation(float saturation, EnumFoodCategory foodCat = EnumFoodCategory.Unknown, float saturationLossDelay = 10f, float nutritionGainMultiplier = 1f)
	{
		return true;
	}

	public override void OnGameTick(float dt)
	{
		if (MountedOn != null)
		{
			AnimationMetaData nowSuggestedAnim = MountedOn.SuggestedAnimation;
			if (curMountedAnim?.Code != nowSuggestedAnim?.Code)
			{
				if (curMountedAnim != null)
				{
					AnimManager?.StopAnimation(curMountedAnim.Code);
				}
				if (nowSuggestedAnim != null)
				{
					AnimManager?.StartAnimation(nowSuggestedAnim);
				}
				curMountedAnim = nowSuggestedAnim;
			}
		}
		else if (curMountedAnim != null)
		{
			AnimManager?.StopAnimation(curMountedAnim.Code);
			curMountedAnim = null;
		}
		if (Api is ICoreClientAPI)
		{
			if (Alive)
			{
				CurrentControls = (EnumEntityActivity)(((!servercontrols.TriesToMove && ((!servercontrols.Jump && !servercontrols.Sneak) || !servercontrols.IsClimbing)) ? 1 : 2) | ((Swimming && !servercontrols.FloorSitting) ? 32 : 0) | (servercontrols.FloorSitting ? 512 : 0) | ((servercontrols.Sneak && !servercontrols.IsClimbing && !servercontrols.FloorSitting && !Swimming) ? 8 : 0) | ((servercontrols.TriesToMove && servercontrols.Sprint && !Swimming && !servercontrols.Sneak) ? 4 : 0) | (servercontrols.IsFlying ? (servercontrols.Gliding ? 8192 : 16) : 0) | (servercontrols.IsClimbing ? 256 : 0) | ((servercontrols.Jump && OnGround) ? 64 : 0) | ((!OnGround && !Swimming && !FeetInLiquid && !servercontrols.IsClimbing && !servercontrols.IsFlying && base.Pos.Motion.Y < -0.05) ? 128 : 0) | ((MountedOn != null) ? 16384 : 0));
			}
			else
			{
				CurrentControls = EnumEntityActivity.Dead;
			}
			CurrentControls = ((CurrentControls == EnumEntityActivity.None) ? EnumEntityActivity.Idle : CurrentControls);
			if (MountedOn != null && MountedOn.SkipIdleAnimation)
			{
				CurrentControls &= ~EnumEntityActivity.Idle;
			}
			HandleHandAnimations(dt);
			AnimationMetaData defaultAnim = null;
			bool anyAverageAnimActive = false;
			bool skipDefaultAnim = false;
			AnimationMetaData[] animations = base.Properties.Client.Animations;
			int i = 0;
			while (animations != null && i < animations.Length)
			{
				AnimationMetaData anim = animations[i];
				bool wasActive = AnimManager.IsAnimationActive(anim.Animation);
				bool isDefaultAnim = anim != null && anim.TriggeredBy?.DefaultAnim == true;
				bool nowActive = anim.Matches((int)CurrentControls) || (isDefaultAnim && CurrentControls == EnumEntityActivity.Idle);
				anyAverageAnimActive |= !isDefaultAnim && wasActive && anim.BlendMode == EnumAnimationBlendMode.Average;
				skipDefaultAnim |= (nowActive || (wasActive && !anim.WasStartedFromTrigger)) && anim.SupressDefaultAnimation;
				if (isDefaultAnim)
				{
					defaultAnim = anim;
				}
				if (!onAnimControls(anim, wasActive, nowActive))
				{
					if (!wasActive && nowActive)
					{
						anim.WasStartedFromTrigger = true;
						AnimManager.StartAnimation(anim);
					}
					if (!isDefaultAnim && wasActive && !nowActive && anim.WasStartedFromTrigger)
					{
						anim.WasStartedFromTrigger = false;
						AnimManager.StopAnimation(anim.Animation);
					}
				}
				i++;
			}
			if (defaultAnim != null && Alive && !skipDefaultAnim)
			{
				if (anyAverageAnimActive || MountedOn != null)
				{
					if (!alwaysRunIdle && AnimManager.IsAnimationActive(defaultAnim.Animation))
					{
						AnimManager.StopAnimation(defaultAnim.Animation);
					}
				}
				else
				{
					defaultAnim.WasStartedFromTrigger = true;
					if (!AnimManager.IsAnimationActive(defaultAnim.Animation))
					{
						AnimManager.StartAnimation(defaultAnim);
					}
				}
			}
			if ((!Alive || skipDefaultAnim) && defaultAnim != null)
			{
				AnimManager.StopAnimation(defaultAnim.Code);
			}
			bool isSelf = (Api as ICoreClientAPI).World.Player.Entity.EntityId == EntityId;
			Block block = insideBlock;
			if (block != null && block.GetBlockMaterial(Api.World.BlockAccessor, insidePos) == EnumBlockMaterial.Snow && isSelf)
			{
				SpawnSnowStepParticles();
			}
		}
		else
		{
			HandleHandAnimations(dt);
			if (RotateModelOnClimb)
			{
				if (!OnGround && Alive && Controls.IsClimbing && ClimbingOnFace != null && (double)ClimbingOnCollBox.Y2 > 0.2)
				{
					base.Pos.Pitch = (float)ClimbingOnFace.HorizontalAngleIndex * ((float)Math.PI / 2f);
				}
				else
				{
					base.Pos.Pitch = 0f;
				}
			}
		}
		World.FrameProfiler.Mark("entityAgent-ticking");
		base.OnGameTick(dt);
	}

	protected virtual void SpawnSnowStepParticles()
	{
		ICoreClientAPI capi = Api as ICoreClientAPI;
		EntityPos herepos = base.Pos;
		float val = (float)Math.Sqrt(base.Pos.Motion.X * base.Pos.Motion.X + base.Pos.Motion.Z * base.Pos.Motion.Z);
		if (Api.World.Rand.NextDouble() < (double)(10f * val))
		{
			Random rand = capi.World.Rand;
			Vec3f velo = new Vec3f(1f - 2f * (float)rand.NextDouble() + GameMath.Clamp((float)base.Pos.Motion.X * 15f, -5f, 5f), 0.5f + 3.5f * (float)rand.NextDouble(), 1f - 2f * (float)rand.NextDouble() + GameMath.Clamp((float)base.Pos.Motion.Z * 15f, -5f, 5f));
			float radius = Math.Min(SelectionBox.XSize, SelectionBox.ZSize) * 0.9f;
			World.SpawnCubeParticles(herepos.AsBlockPos, herepos.XYZ.Add(0.0, 0.0, 0.0), radius, 2 + (int)(rand.NextDouble() * (double)val * 5.0), 0.5f + (float)rand.NextDouble() * 0.5f, null, velo);
		}
	}

	protected virtual void SpawnFloatingSediment(IAsyncParticleManager manager)
	{
		ICoreClientAPI capi = Api as ICoreClientAPI;
		EntityPos herepos = base.Pos;
		double width = SelectionBox.XSize * 0.75f;
		Entity.SplashParticleProps.BasePos.Set(herepos.X - width / 2.0, herepos.InternalY + 0.0, herepos.Z - width / 2.0);
		Entity.SplashParticleProps.AddPos.Set(width, 0.5, width);
		float mot = (float)herepos.Motion.Length();
		Entity.SplashParticleProps.AddVelocity.Set((float)herepos.Motion.X * 20f, 0f, (float)herepos.Motion.Z * 20f);
		float f = base.Properties.Attributes?["extraSplashParticlesMul"].AsFloat(1f) ?? 1f;
		Entity.SplashParticleProps.QuantityMul = 0.15f * mot * 5f * 2f * f;
		World.SpawnParticles(Entity.SplashParticleProps);
		SpawnWaterMovementParticles(Math.Max(Swimming ? 0.04f : 0f, mot * 5f));
		FloatingSedimentParticles FloatingSedimentParticles = new FloatingSedimentParticles();
		FloatingSedimentParticles.SedimentPos.SetAndCorrectDimension((int)herepos.X, (int)herepos.InternalY - 1, (int)herepos.Z);
		Block block = (FloatingSedimentParticles.SedimentBlock = World.BlockAccessor.GetBlock(FloatingSedimentParticles.SedimentPos));
		if (insideBlock != null && (block.BlockMaterial == EnumBlockMaterial.Gravel || block.BlockMaterial == EnumBlockMaterial.Soil || block.BlockMaterial == EnumBlockMaterial.Sand))
		{
			FloatingSedimentParticles.BasePos.Set(Entity.SplashParticleProps.BasePos);
			FloatingSedimentParticles.AddPos.Set(Entity.SplashParticleProps.AddPos);
			FloatingSedimentParticles.quantity = mot * 150f;
			FloatingSedimentParticles.waterColor = insideBlock.GetColor(capi, FloatingSedimentParticles.SedimentPos);
			manager.Spawn(FloatingSedimentParticles);
		}
	}

	protected virtual bool onAnimControls(AnimationMetaData anim, bool wasActive, bool nowActive)
	{
		return false;
	}

	protected virtual void HandleHandAnimations(float dt)
	{
	}

	/// <summary>
	/// Gets the walk speed multiplier.
	/// </summary>
	/// <param name="groundDragFactor">The amount of drag provided by the current ground. (Default: 0.3)</param>
	public virtual double GetWalkSpeedMultiplier(double groundDragFactor = 0.3)
	{
		int y1 = (int)(base.Pos.InternalY - 0.05000000074505806);
		int y2 = (int)(base.Pos.InternalY + 0.009999999776482582);
		Block belowBlock = World.BlockAccessor.GetBlockRaw((int)base.Pos.X, y1, (int)base.Pos.Z);
		insidePos.Set((int)base.Pos.X, y2, (int)base.Pos.Z);
		insidePos.SetDimension(base.Pos.Dimension);
		insideBlock = World.BlockAccessor.GetBlock(insidePos);
		double multiplier = (servercontrols.Sneak ? ((double)GlobalConstants.SneakSpeedMultiplier) : 1.0) * (servercontrols.Sprint ? GlobalConstants.SprintSpeedMultiplier : 1.0);
		if (FeetInLiquid)
		{
			multiplier /= 2.5;
		}
		return multiplier * (double)(belowBlock.WalkSpeedMultiplier * ((y1 == y2) ? 1f : insideBlock.WalkSpeedMultiplier));
	}

	/// <summary>
	/// Serializes the slots contents to be stored in the SaveGame
	/// </summary>
	/// <returns></returns>
	public override void ToBytes(BinaryWriter writer, bool forClient)
	{
		if (MountedOn != null)
		{
			TreeAttribute mountableTree = new TreeAttribute();
			MountedOn?.MountableToTreeAttributes(mountableTree);
			WatchedAttributes["mountedOn"] = mountableTree;
		}
		else if (WatchedAttributes.HasAttribute("mountedOn"))
		{
			WatchedAttributes.RemoveAttribute("mountedOn");
		}
		base.ToBytes(writer, forClient);
		controls.ToBytes(writer);
	}

	/// <summary>
	/// Loads the entity from a stored byte array from the SaveGame
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="forClient"></param>
	public override void FromBytes(BinaryReader reader, bool forClient)
	{
		try
		{
			base.FromBytes(reader, forClient);
			controls.FromBytes(reader, LoadControlsFromServer);
		}
		catch (EndOfStreamException innerException)
		{
			throw new Exception("EndOfStreamException thrown while reading entity, you might be able to recover your savegame through repair mode", innerException);
		}
	}

	/// <summary>
	/// Relevant only for entities with heads, implemented in EntityAgent.  Other sub-classes of Entity (if not EntityAgent) should similarly override this if the headYaw/headPitch are relevant to them
	/// </summary>
	protected override void SetHeadPositionToWatchedAttributes()
	{
		WatchedAttributes["headYaw"] = new FloatAttribute(base.Pos.HeadYaw);
		WatchedAttributes["headPitch"] = new FloatAttribute(base.Pos.HeadPitch);
	}

	/// <summary>
	/// Relevant only for entities with heads, implemented in EntityAgent.  Other sub-classes of Entity (if not EntityAgent) should similarly override this if the headYaw/headPitch are relevant to them
	/// </summary>
	protected override void GetHeadPositionFromWatchedAttributes()
	{
		base.Pos.HeadYaw = WatchedAttributes.GetFloat("headYaw");
		base.Pos.HeadPitch = WatchedAttributes.GetFloat("headPitch");
	}

	/// <summary>
	/// Attempts to stop the hand  action.
	/// </summary>
	/// <param name="isCancel">Whether or not the action is cancelled or stopped.</param>
	/// <param name="cancelReason">The reason for stopping the action.</param>
	/// <returns>Whether the stop was cancelled or not.</returns>
	public virtual bool TryStopHandAction(bool isCancel, EnumItemUseCancelReason cancelReason = EnumItemUseCancelReason.ReleasedMouse)
	{
		if (controls.HandUse == EnumHandInteract.None || RightHandItemSlot?.Itemstack == null)
		{
			return true;
		}
		float secondsPassed = (float)(World.ElapsedMilliseconds - controls.UsingBeginMS) / 1000f;
		if (isCancel)
		{
			controls.HandUse = RightHandItemSlot.Itemstack.Collectible.OnHeldUseCancel(secondsPassed, RightHandItemSlot, this, null, null, cancelReason);
		}
		else
		{
			controls.HandUse = EnumHandInteract.None;
			RightHandItemSlot.Itemstack.Collectible.OnHeldUseStop(secondsPassed, RightHandItemSlot, this, null, null, controls.HandUse);
		}
		return controls.HandUse == EnumHandInteract.None;
	}

	/// <summary>
	/// This walks the inventory for the entity agent.
	/// </summary>
	/// <param name="handler">the event to fire while walking the inventory.</param>
	public virtual void WalkInventory(OnInventorySlot handler)
	{
	}

	public override void UpdateDebugAttributes()
	{
		base.UpdateDebugAttributes();
		DebugAttributes.SetString("Herd Id", HerdId.ToString() ?? "");
	}

	public override bool TryGiveItemStack(ItemStack itemstack)
	{
		if (itemstack == null || itemstack.StackSize == 0)
		{
			return false;
		}
		List<EntityBehavior> bhs = base.SidedProperties?.Behaviors;
		EnumHandling handling = EnumHandling.PassThrough;
		if (bhs != null)
		{
			foreach (EntityBehavior item in bhs)
			{
				item.TryGiveItemStack(itemstack, ref handling);
				if (handling == EnumHandling.PreventSubsequent)
				{
					break;
				}
			}
		}
		return handling != EnumHandling.PassThrough;
	}

	/// <summary>
	/// If true, then this entity will not retaliate if attacked by the specified eOther
	/// </summary>
	/// <param name="eOther"></param>
	public bool ToleratesDamageFrom(Entity eOther)
	{
		bool tolerate = false;
		foreach (EntityBehavior item in base.SidedProperties?.Behaviors)
		{
			EnumHandling handling = EnumHandling.PassThrough;
			bool thisTolerate = item.ToleratesDamageFrom(eOther, ref handling);
			if (handling != EnumHandling.PassThrough)
			{
				tolerate = thisTolerate;
			}
			if (handling == EnumHandling.PreventSubsequent)
			{
				return thisTolerate;
			}
		}
		return tolerate;
	}

	public override string GetInfoText()
	{
		StringBuilder infotext = new StringBuilder();
		infotext.Append(base.GetInfoText());
		if (Api is ICoreClientAPI capi && capi.Settings.Bool["extendedDebugInfo"])
		{
			infotext.AppendLine("<font color=\"#bbbbbb\">Herd id: " + HerdId + "</font>");
			if (DebugAttributes.HasAttribute("AI Tasks"))
			{
				StringBuilder stringBuilder = infotext;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(39, 1, stringBuilder);
				handler.AppendLiteral("<font color=\"#bbbbbb\">AI tasks: ");
				handler.AppendFormatted(DebugAttributes.GetString("AI Tasks"));
				handler.AppendLiteral("</font>");
				stringBuilder.AppendLine(ref handler);
			}
		}
		return infotext.ToString();
	}
}
