using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public sealed class EntityErel : EntityAgent
{
	private ErelAnimManager erelAnimManager;

	private ILoadedSound aliveSound;

	private ILoadedSound glideSound;

	private AiTaskManager taskManager;

	private readonly Dictionary<string, int[]> attackCooldowns = new Dictionary<string, int[]>();

	private float nextFlyIdleSec = -1f;

	private float nextFlapCruiseSec = -1f;

	private float prevYaw;

	private float annoyCheckAccum;

	private bool wasAtBossFightArea;

	private EntityBehaviorHealth healthBehavior;

	private int previousTickDimension;

	public override bool CanSwivel => false;

	public override bool CanSwivelNow => false;

	public override bool StoreWithChunk => false;

	public override bool CanAttackInside => true;

	public override bool AllowOutsideLoadedRange => true;

	public override bool AlwaysActive => true;

	public long LastAttackTime { get; set; }

	public double LastAnnoyedTotalDays
	{
		get
		{
			return WatchedAttributes.GetDouble("lastannoyedtotaldays", -9999999.0);
		}
		set
		{
			WatchedAttributes.SetDouble("lastannoyedtotaldays", value);
		}
	}

	public bool Annoyed
	{
		get
		{
			return WatchedAttributes.GetBool("annoyed");
		}
		set
		{
			WatchedAttributes.SetBool("annoyed", value);
		}
	}

	public bool StandingOnGround
	{
		get
		{
			return WatchedAttributes.GetBool("standingOnGround");
		}
		set
		{
			WatchedAttributes.SetBool("standingOnGround", value);
		}
	}

	static EntityErel()
	{
		AiTaskRegistry.Register<AiTaskFlyCircle>("flycircle");
		AiTaskRegistry.Register<AiTaskFlyCircleIfEntity>("flycircleifentity");
		AiTaskRegistry.Register<AiTaskFlyCircleTarget>("flycircletarget");
		AiTaskRegistry.Register<AiTaskFlyWander>("flywander");
		AiTaskRegistry.Register<AiTaskFlySwoopAttack>("flyswoopattack");
		AiTaskRegistry.Register<AiTaskFlyDiveAttack>("flydiveattack");
		AiTaskRegistry.Register<AiTaskFireFeathersAttack>("firefeathersattack");
		AiTaskRegistry.Register<AiTaskFlyLeave>("flyleave");
	}

	public EntityErel()
	{
		SimulationRange = 1024;
		AnimManager = (erelAnimManager = new ErelAnimManager());
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
		WatchedAttributes.SetBool("showHealthbar", value: true);
		if (api is ICoreClientAPI capi)
		{
			aliveSound = capi.World.LoadSound(new SoundParams
			{
				DisposeOnFinish = false,
				Location = new AssetLocation("sounds/creature/erel/alive"),
				ShouldLoop = true,
				Range = 48f
			});
			aliveSound.Start();
			glideSound = capi.World.LoadSound(new SoundParams
			{
				DisposeOnFinish = false,
				Location = new AssetLocation("sounds/creature/erel/glide"),
				ShouldLoop = true,
				Range = 24f
			});
		}
		healthBehavior = GetBehavior<EntityBehaviorHealth>();
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		base.AfterInitialized(onFirstSpawn);
		Api.ModLoader.GetModSystem<ModSystemDevastationEffects>().SetErelAnnoyed(Annoyed);
		if (World.Side == EnumAppSide.Server)
		{
			taskManager = GetBehavior<EntityBehaviorTaskAI>().TaskManager;
			taskManager.OnShouldExecuteTask += TaskManager_OnShouldExecuteTask;
			if (Annoyed)
			{
				taskManager.GetTask<AiTaskFlyLeave>().AllowExecute = true;
			}
			attackCooldowns["swoop"] = new int[2]
			{
				taskManager.GetTask<AiTaskFlySwoopAttack>().MinCooldownMs,
				taskManager.GetTask<AiTaskFlySwoopAttack>().MaxCooldownMs
			};
			attackCooldowns["dive"] = new int[2]
			{
				taskManager.GetTask<AiTaskFlyDiveAttack>().MinCooldownMs,
				taskManager.GetTask<AiTaskFlyDiveAttack>().MaxCooldownMs
			};
			attackCooldowns["feathers"] = new int[2]
			{
				taskManager.GetTask<AiTaskFireFeathersAttack>().MinCooldownMs,
				taskManager.GetTask<AiTaskFireFeathersAttack>().MaxCooldownMs
			};
		}
		updateAnnoyedState();
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		aliveSound?.Dispose();
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if (Api.Side == EnumAppSide.Server)
		{
			doOccasionalFlapping(dt);
			annoyCheckAccum += dt;
			if (annoyCheckAccum > 1f)
			{
				annoyCheckAccum = 0f;
				updateAnnoyedState();
				toggleBossFightModeNearTower();
				stopAttacksOutsideDevaRange();
			}
		}
		else
		{
			aliveSound.SetPosition((float)base.Pos.X, (float)base.Pos.InternalY, (float)base.Pos.Z);
			glideSound.SetPosition((float)base.Pos.X, (float)base.Pos.InternalY, (float)base.Pos.Z);
			if (AnimManager.IsAnimationActive("fly-flapactive", "fly-flapactive-fast") && glideSound.IsPlaying)
			{
				glideSound.Stop();
			}
			else if ((AnimManager.IsAnimationActive("fly-idle", "fly-flapcruise") || AnimManager.ActiveAnimationsByAnimCode.Count == 0) && !glideSound.IsPlaying)
			{
				glideSound.Start();
			}
			setCurrentShape(base.Pos.Dimension);
		}
		if (!AnimManager.IsAnimationActive("dive", "slam"))
		{
			base.Pos.Motion.Length();
			_ = 0.01;
		}
	}

	public override bool ReceiveDamage(DamageSource damageSource, float damage)
	{
		if (!inTowerRange())
		{
			damage /= 2f;
		}
		if (World.Side == EnumAppSide.Server)
		{
			int x = nearbyPlayerCount();
			damage *= 1f / (1f + (float)Math.Sqrt((x - 1) / 4));
		}
		if (healthBehavior != null && healthBehavior.Health - damage < 0f)
		{
			float reducedDamage = MathF.Max(healthBehavior.Health - 1f, 0f);
			return base.ReceiveDamage(damageSource, reducedDamage);
		}
		return base.ReceiveDamage(damageSource, damage);
	}

	public void ChangeDimension(int dim)
	{
		if (base.Pos.Dimension != dim)
		{
			spawnTeleportParticles(base.Pos);
		}
		base.Pos.Dimension = dim;
		long newchunkindex3d = Api.World.ChunkProvider.ChunkIndex3D(base.Pos);
		Api.World.UpdateEntityChunk(this, newchunkindex3d);
	}

	public void ChangeDimensionNoParticles(int dim)
	{
		base.Pos.Dimension = dim;
		base.Pos.Dimension = dim;
		long newchunkindex3d = Api.World.ChunkProvider.ChunkIndex3D(base.Pos);
		Api.World.UpdateEntityChunk(this, newchunkindex3d);
	}

	protected override void OnTesselation(ref Shape entityShape, string shapePathForLogging, ref bool shapeIsCloned)
	{
		base.OnTesselation(ref entityShape, shapePathForLogging, ref shapeIsCloned);
		AnimManager.LoadAnimator(World.Api, this, entityShape, AnimManager.Animator?.Animations, requirePosesOnServer, null, "head");
	}

	private void spawnTeleportParticles(EntityPos pos)
	{
		int r = 53;
		int g = 221;
		int b = 172;
		SimpleParticleProperties teleportParticles = new SimpleParticleProperties(300f, 400f, (r << 16) | (g << 8) | b | 0x64000000, new Vec3d(pos.X - 2.5, pos.Y, pos.Z - 2.5), new Vec3d(pos.X + 2.5, pos.Y + 5.8, pos.Z + 2.5), new Vec3f(-0.7f, -0.7f, -0.7f), new Vec3f(1.4f, 1.4f, 1.4f), 2f, 0f, 0.15f, 0.3f, EnumParticleModel.Quad);
		teleportParticles.addLifeLength = 1f;
		teleportParticles.OpacityEvolve = EvolvingNatFloat.create(EnumTransformFunction.QUADRATIC, -10f);
		int dim = pos.Dimension;
		Api.World.SpawnParticles(teleportParticles);
		Api.World.PlaySoundAt(new AssetLocation("sounds/effect/timeswitch"), pos.X, pos.Y, pos.Z, null, randomizePitch: false, 128f);
		teleportParticles.MinPos.Y += dim * 32768;
		Api.World.SpawnParticles(teleportParticles);
		Api.World.PlaySoundAt(new AssetLocation("sounds/effect/timeswitch"), pos.X, pos.Y + (double)(dim * 32768), pos.Z, null, randomizePitch: false, 128f);
	}

	private int nearbyPlayerCount()
	{
		int cnt = 0;
		IPlayer[] allOnlinePlayers = World.AllOnlinePlayers;
		for (int i = 0; i < allOnlinePlayers.Length; i++)
		{
			IServerPlayer plr = (IServerPlayer)allOnlinePlayers[i];
			if (plr.ConnectionState == EnumClientState.Playing && plr.WorldData.CurrentGameMode == EnumGameMode.Survival)
			{
				double dx = plr.Entity.Pos.X - base.Pos.X;
				double dz = plr.Entity.Pos.Z - base.Pos.Z;
				if (Math.Abs(dx) <= 7.0 && Math.Abs(dz) <= 7.0)
				{
					cnt++;
				}
			}
		}
		return cnt;
	}

	private void toggleBossFightModeNearTower()
	{
		ModSystemDevastationEffects msdevaeff = Api.ModLoader.GetModSystem<ModSystemDevastationEffects>();
		Vec3d loc = ((base.Pos.Dimension == 0) ? msdevaeff.DevaLocationPresent : msdevaeff.DevaLocationPast);
		WatchedAttributes.SetBool("showHealthbar", base.Pos.InternalY > loc.Y + 70.0);
		AiTaskFlyCircleIfEntity ctask = taskManager.GetTask<AiTaskFlyCircleIfEntity>();
		bool atBossFightArea = ctask.getEntity() != null && base.Pos.XYZ.HorizontalSquareDistanceTo(ctask.CenterPos) < 4900f;
		AiTaskFlySwoopAttack swoopAtta = taskManager.GetTask<AiTaskFlySwoopAttack>();
		AiTaskFlyDiveAttack diveAtta = taskManager.GetTask<AiTaskFlyDiveAttack>();
		AiTaskFireFeathersAttack feathersAtta = taskManager.GetTask<AiTaskFireFeathersAttack>();
		feathersAtta.Enabled = atBossFightArea;
		diveAtta.Enabled = atBossFightArea;
		if (wasAtBossFightArea && !atBossFightArea)
		{
			swoopAtta.MinCooldownMs = attackCooldowns["swoop"][0];
			swoopAtta.MaxCooldownMs = attackCooldowns["swoop"][1];
			diveAtta.MinCooldownMs = attackCooldowns["dive"][0];
			diveAtta.MaxCooldownMs = attackCooldowns["dive"][1];
			feathersAtta.MinCooldownMs = attackCooldowns["feathers"][0];
			feathersAtta.MaxCooldownMs = attackCooldowns["feathers"][1];
		}
		if (!wasAtBossFightArea && atBossFightArea)
		{
			swoopAtta.MinCooldownMs = attackCooldowns["swoop"][0] / 2;
			swoopAtta.MaxCooldownMs = attackCooldowns["swoop"][1] / 2;
			diveAtta.MinCooldownMs = attackCooldowns["dive"][0] / 2;
			diveAtta.MaxCooldownMs = attackCooldowns["dive"][1] / 2;
			feathersAtta.MinCooldownMs = attackCooldowns["feathers"][0] / 2;
			feathersAtta.MaxCooldownMs = attackCooldowns["feathers"][1] / 2;
		}
		wasAtBossFightArea = atBossFightArea;
	}

	private void updateAnnoyedState()
	{
		if (Api.Side == EnumAppSide.Client)
		{
			return;
		}
		if (!Annoyed)
		{
			if ((double)(healthBehavior.Health / healthBehavior.MaxHealth) < 0.02)
			{
				Api.World.PlaySoundAt("sounds/creature/erel/annoyed", this, null, randomizePitch: false, 1024f);
				AnimManager.StartAnimation("defeat");
				LastAnnoyedTotalDays = Api.World.Calendar.TotalDays;
				Annoyed = true;
				taskManager.GetTask<AiTaskFlyLeave>().AllowExecute = true;
				Api.ModLoader.GetModSystem<ModSystemDevastationEffects>().SetErelAnnoyed(on: true);
			}
		}
		else if (Api.World.Calendar.TotalDays - LastAnnoyedTotalDays > 14.0)
		{
			Annoyed = false;
			healthBehavior.Health = healthBehavior.MaxHealth;
			taskManager.GetTask<AiTaskFlyLeave>().AllowExecute = false;
			Api.ModLoader.GetModSystem<ModSystemDevastationEffects>().SetErelAnnoyed(on: false);
		}
	}

	private void doOccasionalFlapping(float dt)
	{
		float turnSpeed = Math.Abs(GameMath.AngleRadDistance(prevYaw, base.Pos.Yaw));
		double flyspeed = base.Pos.Motion.Length();
		if (AnimManager.IsAnimationActive("dive", "slam"))
		{
			return;
		}
		if ((base.Pos.Motion.Y >= 0.03 || (double)turnSpeed > 0.05 || flyspeed < 0.15) && (AnimManager.IsAnimationActive("fly-idle", "fly-flapcruise") || AnimManager.ActiveAnimationsByAnimCode.Count == 0))
		{
			AnimManager.StopAnimation("fly-flapcruise");
			AnimManager.StopAnimation("fly-idle");
			AnimManager.StartAnimation("fly-flapactive-fast");
			return;
		}
		if (base.Pos.Motion.Y <= 0.01 && (double)turnSpeed < 0.03 && flyspeed >= 0.35 && AnimManager.IsAnimationActive("fly-flapactive", "fly-flapactive-fast"))
		{
			AnimManager.StopAnimation("fly-flapactive");
			AnimManager.StopAnimation("fly-flapactive-fast");
			AnimManager.StartAnimation("fly-idle");
		}
		prevYaw = base.Pos.Yaw;
		if (nextFlyIdleSec > 0f)
		{
			nextFlyIdleSec -= dt;
			if (nextFlyIdleSec < 0f)
			{
				AnimManager.StopAnimation("fly-flapcruise");
				AnimManager.StartAnimation("fly-idle");
				return;
			}
		}
		if (nextFlapCruiseSec < 0f)
		{
			nextFlapCruiseSec = (float)Api.World.Rand.NextDouble() * 15f + 5f;
		}
		else if (AnimManager.IsAnimationActive("fly-idle"))
		{
			nextFlapCruiseSec -= dt;
			if (nextFlapCruiseSec < 0f)
			{
				AnimManager.StopAnimation("fly-idle");
				AnimManager.StartAnimation("fly-flapcruise");
				nextFlyIdleSec = (float)(Api.World.Rand.NextDouble() * 4.0 + 1.0) * 130f / 30f;
			}
		}
	}

	private void stopAttacksOutsideDevaRange()
	{
		if (!outSideDevaRange())
		{
			return;
		}
		IAiTask[] activeTasksBySlot = taskManager.ActiveTasksBySlot;
		foreach (IAiTask t in activeTasksBySlot)
		{
			if (t is AiTaskFlySwoopAttack || t is AiTaskFlyDiveAttack || t is AiTaskFireFeathersAttack || t is AiTaskFlyCircleTarget)
			{
				taskManager.StopTask(t.GetType());
			}
		}
	}

	private bool outSideDevaRange()
	{
		return distanceToTower() > 600.0;
	}

	private bool inTowerRange()
	{
		return distanceToTower() < 100.0;
	}

	private double distanceToTower()
	{
		ModSystemDevastationEffects msdevaeff = Api.ModLoader.GetModSystem<ModSystemDevastationEffects>();
		Vec3d loc = ((base.Pos.Dimension == 0) ? msdevaeff.DevaLocationPresent : msdevaeff.DevaLocationPast);
		return base.Pos.DistanceTo(loc);
	}

	private void setCurrentShape(int dimension)
	{
		if (base.Properties.Client.Renderer is EntityShapeRenderer renderer)
		{
			if (dimension == 2)
			{
				renderer.OverrideEntityShape = base.Properties.Client.LoadedAlternateShapes[0];
			}
			else
			{
				renderer.OverrideEntityShape = base.Properties.Client.LoadedAlternateShapes[1];
			}
			if (previousTickDimension != dimension)
			{
				renderer.TesselateShape();
				previousTickDimension = dimension;
			}
		}
	}

	private bool TaskManager_OnShouldExecuteTask(IAiTask t)
	{
		if ((t is AiTaskFlySwoopAttack || t is AiTaskFlyDiveAttack || t is AiTaskFireFeathersAttack || t is AiTaskFlyCircleTarget) && outSideDevaRange())
		{
			return false;
		}
		return true;
	}
}
