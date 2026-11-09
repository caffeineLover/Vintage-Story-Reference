using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectileEntity : Entity
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x000347F2 File Offset: 0x000329F2
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x000347FA File Offset: 0x000329FA
		[Nullable(2)]
		public ProjectileServer ServerProjectile { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00034803 File Offset: 0x00032A03
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0003480B File Offset: 0x00032A0B
		public Guid ProjectileId { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00034814 File Offset: 0x00032A14
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x0003481C File Offset: 0x00032A1C
		[Nullable(2)]
		public ItemStack ProjectileStack { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00034825 File Offset: 0x00032A25
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0003482D File Offset: 0x00032A2D
		[Nullable(2)]
		public ItemStack WeaponStack { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00034836 File Offset: 0x00032A36
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0003483E File Offset: 0x00032A3E
		public int DurabilityDamageOnImpact { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00034847 File Offset: 0x00032A47
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x0003484F File Offset: 0x00032A4F
		public float DropOnImpactChance { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00034858 File Offset: 0x00032A58
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x00034860 File Offset: 0x00032A60
		[Nullable(2)]
		public Action<Guid> ClearCallback { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00034869 File Offset: 0x00032A69
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x00034871 File Offset: 0x00032A71
		public float ColliderRadius { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0003487A File Offset: 0x00032A7A
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x00034882 File Offset: 0x00032A82
		public float PenetrationDistance { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0003488B File Offset: 0x00032A8B
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x00034893 File Offset: 0x00032A93
		public float PenetrationStrength { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0003489C File Offset: 0x00032A9C
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x000348A4 File Offset: 0x00032AA4
		public long ShooterId { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000348AD File Offset: 0x00032AAD
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x000348B5 File Offset: 0x00032AB5
		public long OwnerId { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x000348BE File Offset: 0x00032ABE
		public Vec3d ShooterPosition { get; } = new Vec3d(0.0, 0.0, 0.0);

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x000348C6 File Offset: 0x00032AC6
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x000348CE File Offset: 0x00032ACE
		public Vec3d PreviousPosition { get; private set; } = new Vec3d(0.0, 0.0, 0.0);

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x000348D7 File Offset: 0x00032AD7
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x000348DF File Offset: 0x00032ADF
		public Vec3d PreviousVelocity { get; private set; } = new Vec3d(0.0, 0.0, 0.0);

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x000348E8 File Offset: 0x00032AE8
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x000348F0 File Offset: 0x00032AF0
		public List<long> CollidedWith { get; set; } = new List<long>();

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x000348F9 File Offset: 0x00032AF9
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x00034901 File Offset: 0x00032B01
		public bool IgnoreInvFrames { get; set; } = true;

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0003490A File Offset: 0x00032B0A
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x00034912 File Offset: 0x00032B12
		public bool CanBeCollected { get; set; } = true;

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0003491B File Offset: 0x00032B1B
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00034923 File Offset: 0x00032B23
		public float FlightSeconds { get; private set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0003492C File Offset: 0x00032B2C
		public IReadOnlyList<IProjectileMotionModifier> MotionModifiers
		{
			get
			{
				return this._motionModifiers;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00034934 File Offset: 0x00032B34
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x0003493C File Offset: 0x00032B3C
		public bool Stuck
		{
			get
			{
				return this.StuckInternal;
			}
			set
			{
				this.StuckInternal = value;
				if (this.Api.Side == 1)
				{
					this.WatchedAttributes.SetBool("stuck", this.StuckInternal);
				}
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00034969 File Offset: 0x00032B69
		public override bool ApplyGravity
		{
			get
			{
				return !this.Stuck;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00034974 File Offset: 0x00032B74
		public override bool IsInteractable
		{
			get
			{
				return this.IsInteractableValue;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0003497C File Offset: 0x00032B7C
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x00034984 File Offset: 0x00032B84
		public virtual bool IsInteractableValue { get; set; }

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060009D7 RID: 2519 RVA: 0x00034990 File Offset: 0x00032B90
		// (remove) Token: 0x060009D8 RID: 2520 RVA: 0x000349C4 File Offset: 0x00032BC4
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1,
			1
		})]
		public static event Action<ProjectileEntity, EntityAgent, ItemSlot, Vec3d, EnumInteractMode> OnInteracted;

		// Token: 0x060009D9 RID: 2521 RVA: 0x000349F8 File Offset: 0x00032BF8
		public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
		{
			base.Initialize(properties, api, InChunkIndex3d);
			this._motionModifiers = Enumerable.ToArray<IProjectileMotionModifier>(Enumerable.OrderBy<IProjectileMotionModifier, int>(Enumerable.OfType<IProjectileMotionModifier>(base.SidedProperties.Behaviors), (IProjectileMotionModifier modifier) => modifier.Priority));
			this.FlightSeconds = this.Attributes.GetFloat("combatOverhaulProjectileFlightSeconds", 0f);
			this.SpawnTime = TimeSpan.FromMilliseconds(this.World.ElapsedMilliseconds);
			this.CollisionTestBox = this.SelectionBox.Clone();
			ProjectilePhysicsBehavior physicsBehavior = this.GetBehavior<ProjectilePhysicsBehavior>();
			if (physicsBehavior == null)
			{
				ICoreAPI api2 = this.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(110, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Projectile ");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Code);
				defaultInterpolatedStringHandler.AppendLiteral(" does not have 'ProjectilePhysicsBehavior', update this mod to support latest Overhaul lib version.");
				LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.PhysicsBehavior = physicsBehavior;
			if (physicsBehavior.Config.ColliderRadius == 0.0)
			{
				physicsBehavior.Config.ColliderRadius = (double)this.ColliderRadius;
			}
			physicsBehavior.OnPhysicsTickCallback = new Action<float>(this.OnPhysicsTickCallback);
			this.PreviousPosition.Set(base.Pos.X, base.Pos.Y, base.Pos.Z);
			this.PreviousVelocity.Set(base.Pos.Motion.X, base.Pos.Motion.Y, base.Pos.Motion.Z);
			this.StartingPos.Set(base.Pos.X, base.Pos.Y, base.Pos.Z);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00034BB4 File Offset: 0x00032DB4
		public override void OnGameTick(float dt)
		{
			if (this.Api.Side == 1 && this._motionModifiers.Length != 0 && this.Alive && !this.ShouldDespawn && !this.Stuck)
			{
				this.FlightSeconds += Math.Max(0f, dt);
				this.Attributes.SetFloat("combatOverhaulProjectileFlightSeconds", this.FlightSeconds);
				this.ApplyMotionModifiers(dt);
				if (!this.Alive || this.ShouldDespawn)
				{
					return;
				}
			}
			base.OnGameTick(dt);
			if (this.ShouldDespawn)
			{
				return;
			}
			if (this._terrainBreakPending && this.World.ElapsedMilliseconds >= this._terrainBreakAtMs)
			{
				this._terrainBreakPending = false;
				if (!this.ApplyImpactDurabilityDamage())
				{
					return;
				}
				this.TryDestroyOnCollision();
				if (this.ShouldDespawn || !this.Alive)
				{
					return;
				}
			}
			if (this.Api.Side == 1 && this.Stuck && !base.Collided)
			{
				this.WatchedAttributes.SetBool("stuck", false);
			}
			this.Stuck = (base.Collided || this.WatchedAttributes.GetBool("stuck", false));
			if (this.Api.Side == 1)
			{
				this.WatchedAttributes.SetBool("stuck", this.Stuck);
			}
			if (this.PhysicsBehavior != null)
			{
				this.PhysicsBehavior.Stuck = this.Stuck;
			}
			if (!this.Stuck)
			{
				this.SetRotation();
			}
			double impactSpeed = Math.Max(this.MotionBeforeCollide.Length(), base.SidedPos.Motion.Length());
			if (this.Stuck)
			{
				this.OnTerrainCollision(base.SidedPos, impactSpeed);
			}
			this.MotionBeforeCollide.Set(base.SidedPos.Motion.X, base.SidedPos.Motion.Y, base.SidedPos.Motion.Z);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00034D98 File Offset: 0x00032F98
		public override bool CanCollect(Entity byEntity)
		{
			return this.CanBeCollected && this.Alive && ProjectileEntity.IsCollectibleProjectileStack(this.ProjectileStack) && TimeSpan.FromMilliseconds(this.World.ElapsedMilliseconds) - this.SpawnTime > this.CollisionDelay && base.ServerPos.Motion.Length() < 0.01;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00034E08 File Offset: 0x00033008
		[return: Nullable(2)]
		public override ItemStack OnCollected(Entity byEntity)
		{
			Action<Guid> clearCallback = this.ClearCallback;
			if (clearCallback != null)
			{
				clearCallback.Invoke(this.ProjectileId);
			}
			ItemStack projectileStack = this.ProjectileStack;
			if (projectileStack != null)
			{
				projectileStack.ResolveBlockOrItem(this.World);
			}
			if (!this.CanBeCollected || !ProjectileEntity.IsCollectibleProjectileStack(this.ProjectileStack))
			{
				return null;
			}
			return this.ProjectileStack;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00034E64 File Offset: 0x00033064
		public override void OnCollided()
		{
			EntityPos sidedPos = base.SidedPos;
			this.OnTerrainCollision(base.SidedPos, Math.Max(this.MotionBeforeCollide.Length(), sidedPos.Motion.Length()));
			this.MotionBeforeCollide.Set(sidedPos.Motion.X, sidedPos.Motion.Y, sidedPos.Motion.Z);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00034ECC File Offset: 0x000330CC
		public override void ToBytes(BinaryWriter writer, bool forClient)
		{
			base.ToBytes(writer, forClient);
			writer.Write(this.ShooterId);
			writer.Write(this.ProjectileId.ToString());
			writer.Write(this.ProjectileStack != null);
			ItemStack projectileStack = this.ProjectileStack;
			if (projectileStack != null)
			{
				projectileStack.ToBytes(writer);
			}
			writer.Write(this.WeaponStack != null);
			ItemStack weaponStack = this.WeaponStack;
			if (weaponStack != null)
			{
				weaponStack.ToBytes(writer);
			}
			writer.Write(this.OwnerId);
			writer.Write(this.IgnoreInvFrames);
			writer.Write(this.CanBeCollected);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00034F70 File Offset: 0x00033170
		public override void FromBytes(BinaryReader reader, bool fromServer)
		{
			base.FromBytes(reader, fromServer);
			try
			{
				this.ShooterId = reader.ReadInt64();
				this.ProjectileId = Guid.Parse(reader.ReadString());
				if (reader.ReadBoolean())
				{
					this.ProjectileStack = new ItemStack(reader);
				}
				if (reader.ReadBoolean())
				{
					this.WeaponStack = new ItemStack(reader);
				}
				this.OwnerId = reader.ReadInt64();
				this.IgnoreInvFrames = reader.ReadBoolean();
				this.CanBeCollected = reader.ReadBoolean();
			}
			catch (Exception exception)
			{
				ICoreAPI api = this.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error on restoring projectile ");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Code);
				defaultInterpolatedStringHandler.AppendLiteral(" from bytes:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00035050 File Offset: 0x00033250
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			base.OnEntityDespawn(despawn);
			Action<Guid> clearCallback = this.ClearCallback;
			if (clearCallback == null)
			{
				return;
			}
			clearCallback.Invoke(this.ProjectileId);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00035070 File Offset: 0x00033270
		public void SetRotation()
		{
			EntityPos pos = (this.World is IServerWorldAccessor) ? base.ServerPos : base.Pos;
			double speed = pos.Motion.Length();
			if (speed > 0.01)
			{
				pos.Pitch = 0f;
				pos.Yaw = 3.1415927f + (float)Math.Atan2(pos.Motion.X / speed, pos.Motion.Z / speed) + GameMath.Cos((float)(TimeSpan.FromMilliseconds(this.World.ElapsedMilliseconds) - this.SpawnTime).TotalMilliseconds / 200f) * 0.03f;
				pos.Roll = -(float)Math.Asin(GameMath.Clamp(-pos.Motion.Y / speed, -1.0, 1.0)) + GameMath.Sin((float)(TimeSpan.FromMilliseconds(this.World.ElapsedMilliseconds) - this.SpawnTime).TotalMilliseconds / 200f) * 0.03f;
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00035189 File Offset: 0x00033389
		public void OnCollisionWithEntity(Entity target, string collider)
		{
			this.WatchedAttributes.MarkAllDirty();
			this._terrainBreakPending = false;
			if (!this.ApplyImpactDurabilityDamage())
			{
				return;
			}
			this.TryDestroyOnCollision();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000351AC File Offset: 0x000333AC
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode)
		{
			Action<ProjectileEntity, EntityAgent, ItemSlot, Vec3d, EnumInteractMode> onInteracted = ProjectileEntity.OnInteracted;
			if (onInteracted != null)
			{
				onInteracted.Invoke(this, byEntity, itemslot, hitPosition, mode);
			}
			base.OnInteract(byEntity, itemslot, hitPosition, mode);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000351D0 File Offset: 0x000333D0
		protected virtual void ApplyMotionModifiers(float dt)
		{
			ProjectileMotionContext context = new ProjectileMotionContext(this, base.ServerPos, this.FlightSeconds);
			IProjectileMotionModifier[] motionModifiers = this._motionModifiers;
			for (int i = 0; i < motionModifiers.Length; i++)
			{
				motionModifiers[i].ModifyMotion(context, dt);
				if (!this.Alive || this.ShouldDespawn)
				{
					break;
				}
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00035220 File Offset: 0x00033420
		protected void OnPhysicsTickCallback(float dtFac)
		{
			if (this.ShouldDespawn || !this.Alive)
			{
				return;
			}
			if (!this.Stuck && this.ServerProjectile != null)
			{
				this.ServerProjectile.TryCollide();
			}
			this.PreviousPosition.Set(base.SidedPos.X, base.SidedPos.Y, base.SidedPos.Z);
			this.PreviousVelocity.Set(base.SidedPos.Motion.X, base.SidedPos.Motion.Y, base.SidedPos.Motion.Z);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000352C4 File Offset: 0x000334C4
		protected void OnTerrainCollision(EntityPos pos, double impactSpeed)
		{
			pos.Motion.Set(0.0, 0.0, 0.0);
			if (this.BeforeCollided || !(this.World is IServerWorldAccessor) || this.World.ElapsedMilliseconds <= this.MsCollide + 500L)
			{
				return;
			}
			if (impactSpeed >= 0.07)
			{
				this.World.PlaySoundAt(new AssetLocation("sounds/arrow-impact"), this, null, false, 32f, 1f);
				this.WatchedAttributes.MarkAllDirty();
			}
			this.MsCollide = this.World.ElapsedMilliseconds;
			this.BeforeCollided = true;
			if (this.ServerProjectile != null)
			{
				this._terrainBreakPending = true;
				this._terrainBreakAtMs = this.World.ElapsedMilliseconds + 150L;
				return;
			}
			if (!this.ApplyImpactDurabilityDamage())
			{
				return;
			}
			this.TryDestroyOnCollision();
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x000353B0 File Offset: 0x000335B0
		private bool ApplyImpactDurabilityDamage()
		{
			if (this._impactDurabilityApplied || this.DurabilityDamageOnImpact <= 0)
			{
				return true;
			}
			this._impactDurabilityApplied = true;
			ItemStack projectileStack = this.ProjectileStack;
			if (((projectileStack != null) ? projectileStack.Collectible : null) == null)
			{
				this.CanBeCollected = false;
				this.Die(0, null);
				return false;
			}
			DummySlot projectileSlot = new DummySlot(this.ProjectileStack);
			this.ProjectileStack.Collectible.DamageItem(this.Api.World, this, projectileSlot, this.DurabilityDamageOnImpact, true);
			this.ProjectileStack = projectileSlot.Itemstack;
			if (ProjectileEntity.IsCollectibleProjectileStack(this.ProjectileStack))
			{
				return true;
			}
			this.CanBeCollected = false;
			this.Die(0, null);
			return false;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00035458 File Offset: 0x00033658
		protected virtual void TryDestroyOnCollision()
		{
			float random = (float)this.Rand.NextDouble();
			if (this.DropOnImpactChance <= random)
			{
				this.World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), this, null, true, 32f, 0.5f);
				this.Die(0, null);
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000354A8 File Offset: 0x000336A8
		[NullableContext(2)]
		private static bool IsCollectibleProjectileStack(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
			{
				return false;
			}
			if (stack.Collectible.GetMaxDurability(stack) > 0 && stack.Collectible.GetRemainingDurability(stack) <= 0)
			{
				return false;
			}
			foreach (ValueTuple<string, string> valueTuple in ProjectileEntity.ToolsmithPartDurabilityAttributes)
			{
				string currentAttribute = valueTuple.Item1;
				string maxAttribute = valueTuple.Item2;
				int maxPartDurability = stack.Attributes.GetInt(maxAttribute, 0);
				if (maxPartDurability > 0 && stack.Attributes.GetInt(currentAttribute, maxPartDurability) <= 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400046E RID: 1134
		private const string FlightSecondsAttribute = "combatOverhaulProjectileFlightSeconds";

		// Token: 0x0400046F RID: 1135
		[TupleElementNames(new string[]
		{
			"Current",
			"Max"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private static readonly ValueTuple<string, string>[] ToolsmithPartDurabilityAttributes = new ValueTuple<string, string>[]
		{
			new ValueTuple<string, string>("tinkeredToolHandleDurability", "tinkeredToolHandleMaxDurability"),
			new ValueTuple<string, string>("tinkeredToolBindingDurability", "tinkeredToolBindingMaxDurability")
		};

		// Token: 0x04000483 RID: 1155
		private const int TerrainBreakDelayMs = 150;

		// Token: 0x04000484 RID: 1156
		private bool _terrainBreakPending;

		// Token: 0x04000485 RID: 1157
		private long _terrainBreakAtMs;

		// Token: 0x04000486 RID: 1158
		private bool _impactDurabilityApplied;

		// Token: 0x04000489 RID: 1161
		protected readonly TimeSpan CollisionDelay = TimeSpan.FromMilliseconds(500L);

		// Token: 0x0400048A RID: 1162
		protected TimeSpan SpawnTime = TimeSpan.Zero;

		// Token: 0x0400048B RID: 1163
		protected bool StuckInternal;

		// Token: 0x0400048C RID: 1164
		protected readonly CollisionTester CollTester = new CollisionTester();

		// Token: 0x0400048D RID: 1165
		[Nullable(2)]
		protected Cuboidf CollisionTestBox;

		// Token: 0x0400048E RID: 1166
		protected Vec3d MotionBeforeCollide = new Vec3d();

		// Token: 0x0400048F RID: 1167
		protected bool BeforeCollided;

		// Token: 0x04000490 RID: 1168
		protected long MsCollide;

		// Token: 0x04000491 RID: 1169
		protected Random Rand = new Random();

		// Token: 0x04000492 RID: 1170
		protected Vector3d NewPosition;

		// Token: 0x04000493 RID: 1171
		protected bool SetPosition;

		// Token: 0x04000494 RID: 1172
		protected Vec3d StartingPos = new Vec3d();

		// Token: 0x04000495 RID: 1173
		[Nullable(2)]
		protected ProjectilePhysicsBehavior PhysicsBehavior;

		// Token: 0x04000496 RID: 1174
		private IProjectileMotionModifier[] _motionModifiers = Array.Empty<IProjectileMotionModifier>();
	}
}
