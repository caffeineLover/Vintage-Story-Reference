using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Compatibility;
using CombatOverhaul.Inputs;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.RangedSystems.Aiming;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	public class BowClient : RangeWeaponClient
	{
		// Token: 0x0600057D RID: 1405 RVA: 0x0001FA34 File Offset: 0x0001DC34
		public BowClient(ICoreClientAPI api, Item item, AmmoSelector ammoSelector) : base(api, item)
		{
			this.Api = api;
			AnimatableAttachable collectibleBehavior = item.GetCollectibleBehavior<AnimatableAttachable>(true);
			if (collectibleBehavior == null)
			{
				throw new Exception("Bow should have AnimatableAttachable behavior.");
			}
			this.Attachable = collectibleBehavior;
			this.ArrowTransform = new ModelTransform(item.Attributes["ArrowTransform"].AsObject<ModelTransformNoDefaults>(null), ModelTransform.BlockDefaultTp());
			ClientAimingSystem aimingSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).AimingSystem;
			if (aimingSystem == null)
			{
				throw new Exception();
			}
			this.AimingSystem = aimingSystem;
			this.Stats = item.Attributes.AsObject<BowStats>(null);
			this.ArcheringCompat = new ArcheringBowCompat(api);
			this.AimingStats = this.Stats.Aiming.ToStats();
			this.AmmoSelector = ammoSelector;
			this.Settings = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001FB05 File Offset: 0x0001DD05
		public override void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
			this.Attachable.ClearAttachments(player.EntityId);
			this.AttachmentSystem.SendClearPacket(player.EntityId);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001FB2C File Offset: 0x0001DD2C
		public override void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
			BowState bowState = (BowState)state;
			if (bowState != BowState.Load)
			{
				if (bowState - BowState.Draw <= 1)
				{
					this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
				}
			}
			else
			{
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
			}
			this.Attachable.ClearAttachments(player.EntityId);
			this.AttachmentSystem.SendClearPacket(player.EntityId);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetState(0, true);
			}
			this.AimingSystem.StopAiming();
			this.StopArcheringAiming();
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior == null)
			{
				return;
			}
			animationBehavior.StopVanillaAnimation(this.Stats.TpAimAnimation, mainHand);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001FBC9 File Offset: 0x0001DDC9
		public override void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			base.OnRegistered(behavior, api);
			this.AimingAnimationController = new AimingAnimationController(this.AimingSystem, this.AnimationBehavior, this.AimingStats);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001FBF0 File Offset: 0x0001DDF0
		protected override bool RequiresEmptyOtherHand(bool mainHand, EntityPlayer player)
		{
			return this.Settings.BowTwoHanded;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001FC00 File Offset: 0x0001DE00
		[ActionEventHandler(10, ActionState.Active)]
		protected virtual bool Load(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (state != 0 || !base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			ItemSlot arrowSlot = this.GetArrowSlot(player);
			if (arrowSlot == null)
			{
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.StartLoading, mainHand);
			this.Attachable.SetAttachment(player.EntityId, "Arrow", arrowSlot.Itemstack, this.ArrowTransform, true, false);
			this.AttachmentSystem.SendAttachPacket(player.EntityId, "Arrow", arrowSlot.Itemstack, this.ArrowTransform);
			this.RangedWeaponSystem.Reload(slot, arrowSlot, 1, mainHand, delegate(bool success)
			{
				this.ReloadCallback(success, player, mainHand);
			}, null);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, this.Stats.LoadAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed, 1f, () => this.LoadAnimationCallback(player, mainHand), null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.Stats.LoadAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed, 1f, true);
			}
			ArcheringBowAimingSettings archeringSettings = this.ArcheringCompat.GetSettings(slot.Itemstack.Item);
			AimingStats aimingStats = this.CreateAimingStats(stackStats, false, archeringSettings);
			this.AimingSystem.ResetAim();
			this.AimingSystem.StartAiming(aimingStats);
			this.AimingSystem.AimingState = WeaponAimingState.Blocked;
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Play(mainHand);
			}
			state = 1;
			this.LogMountedAimState(player, "load-start");
			this.AfterLoad = true;
			return true;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001FE40 File Offset: 0x0001E040
		protected virtual void ReloadCallback(bool success, EntityPlayer player, bool mainHand)
		{
			BowState state = base.GetState<BowState>(true);
			if (success)
			{
				if (state == BowState.Load)
				{
					base.SetState<BowState>(BowState.PreLoaded, true);
					return;
				}
				if (state == BowState.PreLoaded)
				{
					base.SetState<BowState>(BowState.Loaded, true);
					return;
				}
			}
			else
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.PlayReadyAnimation(true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					tpAnimationBehavior.PlayReadyAnimation(true);
				}
				base.SetState<BowState>(BowState.Unloaded, true);
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
		protected virtual bool LoadAnimationCallback(EntityPlayer player, bool mainHand)
		{
			BowState state = base.GetState<BowState>(true);
			if (state != BowState.Load)
			{
				if (state == BowState.PreLoaded)
				{
					this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
					base.SetState<BowState>(BowState.Loaded, true);
				}
			}
			else
			{
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
				base.SetState<BowState>(BowState.PreLoaded, true);
			}
			return true;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001FEEC File Offset: 0x0001E0EC
		[ActionEventHandler(10, ActionState.Active)]
		protected virtual bool Aim(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (state != 3 || !base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			ArcheringBowAimingSettings archeringSettings = this.ArcheringCompat.GetSettings(slot.Itemstack.Item);
			bool useArcheringDrawTime = archeringSettings != null && archeringSettings.EnableDrawTimeTweaks;
			string animation = this.AfterLoad ? this.Stats.DrawAfterLoadAnimation : this.Stats.DrawAnimation;
			float animationSpeed = this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed;
			float weight = 1f;
			string category = "main";
			TimeSpan easeOutDuration = TimeSpan.FromSeconds(0.2);
			TimeSpan easeInDuration = TimeSpan.FromSeconds(0.2);
			bool easeOut = true;
			Func<bool> finishCallback;
			if (!useArcheringDrawTime)
			{
				finishCallback = (() => this.FullLoadCallback(player, mainHand));
			}
			else
			{
				finishCallback = (() => true);
			}
			AnimationRequestByCode request = new AnimationRequestByCode(animation, animationSpeed, weight, category, easeOutDuration, easeInDuration, easeOut, finishCallback, null);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(request, mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(request, mainHand);
			}
			this.AfterLoad = false;
			state = 4;
			this.LogMountedAimState(player, "draw-start");
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.StartAiming, mainHand);
			if (!this.AimingSystem.Aiming)
			{
				AimingStats aimingStats = this.CreateAimingStats(stackStats, true, archeringSettings);
				this.AimingSystem.StartAiming(aimingStats);
				this.AimingSystem.AimingState = WeaponAimingState.Blocked;
				AimingAnimationController aimingAnimationController = this.AimingAnimationController;
				if (aimingAnimationController != null)
				{
					aimingAnimationController.Play(mainHand);
				}
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.PlayVanillaAnimation(this.Stats.TpAimAnimation, mainHand);
				}
			}
			if (archeringSettings != null)
			{
				this.StartArcheringAiming(player, mainHand, archeringSettings);
			}
			return true;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00020114 File Offset: 0x0001E314
		[ActionEventHandler(9, ActionState.Active)]
		protected virtual bool Denock(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			ArcheringBowAimingSettings activeArcheringSettings = this.ActiveArcheringSettings;
			if (activeArcheringSettings == null || !activeArcheringSettings.EnableDenockingWithLeftClick)
			{
				return false;
			}
			if (!RangeWeaponClient.CheckState<BowState>(state, new BowState[]
			{
				BowState.Load,
				BowState.PreLoaded,
				BowState.Loaded,
				BowState.Draw,
				BowState.Drawn
			}))
			{
				return false;
			}
			this.AimingSystem.StopAiming();
			this.StopArcheringAiming();
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopVanillaAnimation(this.Stats.TpAimAnimation, mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(mainHand);
			}
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
			if (RangeWeaponClient.CheckState<BowState>(state, new BowState[]
			{
				BowState.Load,
				BowState.PreLoaded
			}))
			{
				this.Attachable.ClearAttachments(player.EntityId);
				this.AttachmentSystem.SendClearPacket(player.EntityId);
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
				state = 0;
			}
			else
			{
				state = 3;
			}
			this.AfterLoad = false;
			return true;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00020214 File Offset: 0x0001E414
		[ActionEventHandler(10, ActionState.Released)]
		protected virtual bool Shoot(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			this.AimingSystem.StopAiming();
			this.StopArcheringAiming();
			this.LogMountedAimState(player, "release");
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopVanillaAnimation(this.Stats.TpAimAnimation, mainHand);
			}
			if (RangeWeaponClient.CheckState<BowState>(state, new BowState[]
			{
				BowState.Load,
				BowState.PreLoaded
			}))
			{
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.PlayReadyAnimation(mainHand);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					tpAnimationBehavior.PlayReadyAnimation(mainHand);
				}
				this.Attachable.ClearAttachments(player.EntityId);
				this.AttachmentSystem.SendClearPacket(player.EntityId);
				state = 0;
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
				return true;
			}
			if (RangeWeaponClient.CheckState<BowState>(state, new BowState[]
			{
				BowState.Draw,
				BowState.Loaded
			}))
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayReadyAnimation(mainHand);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
				if (tpAnimationBehavior2 != null)
				{
					tpAnimationBehavior2.PlayReadyAnimation(mainHand);
				}
				state = 3;
				this.AfterLoad = false;
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
				return true;
			}
			if (state != 5)
			{
				return false;
			}
			FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
			if (animationBehavior4 != null)
			{
				animationBehavior4.Play(mainHand, this.Stats.ReleaseAnimation, "main", 1f, 1f, () => this.ShootCallback(slot, player, mainHand), null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior3 = this.TpAnimationBehavior;
			if (tpAnimationBehavior3 != null)
			{
				tpAnimationBehavior3.Play(mainHand, this.Stats.ReleaseAnimation, "main", 1f, 1f, true);
			}
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.TriggeredShot, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
			return true;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0002042C File Offset: 0x0001E62C
		private AimingStats CreateAimingStats(ItemStackRangedStats stackStats, bool applyStackDifficulty, [Nullable(2)] ArcheringBowAimingSettings archeringSettings)
		{
			AimingStats aimingStats = this.AimingStats.Clone();
			if (applyStackDifficulty)
			{
				aimingStats.AimDifficulty *= stackStats.AimingDifficulty;
			}
			aimingStats.CursorType = Enum.Parse<AimingCursorType>(this.Settings.BowsAimingCursorType, true);
			aimingStats.VerticalLimit = this.Settings.BowsAimingVerticalLimit * this.Stats.Aiming.VerticalLimit;
			aimingStats.HorizontalLimit = this.Settings.BowsAimingHorizontalLimit * this.Stats.Aiming.HorizontalLimit;
			if (archeringSettings != null)
			{
				if (archeringSettings.DisableVanillaReticle)
				{
					aimingStats.CursorType = AimingCursorType.None;
				}
				aimingStats.AimDifficulty = 0f;
				aimingStats.AimDrift = 0f;
				aimingStats.AimTwitch = 0f;
				aimingStats.AnimationFollowX = 0f;
				aimingStats.AnimationFollowY = 0f;
				aimingStats.AnimationOffsetX = 0f;
				aimingStats.AnimationOffsetY = 0f;
			}
			return aimingStats;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00020518 File Offset: 0x0001E718
		private void StartArcheringAiming(EntityPlayer player, bool mainHand, ArcheringBowAimingSettings settings)
		{
			this.ActiveArcheringSettings = settings;
			this.ArcheringDrawPlayer = player;
			this.ArcheringDrawMainHand = mainHand;
			this.ArcheringDrawStartMs = this.Api.World.ElapsedMilliseconds;
			if (this.ArcheringSession == null)
			{
				this.ArcheringSession = new ArcheringBowAimingSession(this.Api);
			}
			this.ArcheringSession.Start(settings, player, delegate
			{
				BowState state = this.GetState<BowState>(mainHand);
				return this.ActiveArcheringSettings == settings && (state == BowState.Draw || state == BowState.Drawn);
			});
			this.StopArcheringDrawTimer();
			if (settings.EnableDrawTimeTweaks)
			{
				this.ArcheringDrawTickListenerId = this.Api.World.RegisterGameTickListener(delegate(float _)
				{
					this.TickArcheringDraw();
				}, 20, 0);
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000205E4 File Offset: 0x0001E7E4
		private void TickArcheringDraw()
		{
			if (this.ActiveArcheringSettings == null || this.ArcheringDrawPlayer == null)
			{
				this.StopArcheringDrawTimer();
				return;
			}
			if (base.GetState<BowState>(this.ArcheringDrawMainHand) != BowState.Draw)
			{
				this.StopArcheringDrawTimer();
				return;
			}
			if ((float)(this.Api.World.ElapsedMilliseconds - this.ArcheringDrawStartMs) < this.ActiveArcheringSettings.DrawTime * 1000f)
			{
				return;
			}
			this.FullLoadCallback(this.ArcheringDrawPlayer, this.ArcheringDrawMainHand);
			ArcheringBowAimingSession archeringSession = this.ArcheringSession;
			if (archeringSession != null)
			{
				archeringSession.SetCharged();
			}
			this.StopArcheringDrawTimer();
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00020674 File Offset: 0x0001E874
		private void StopArcheringAiming()
		{
			this.StopArcheringDrawTimer();
			ArcheringBowAimingSession archeringSession = this.ArcheringSession;
			if (archeringSession != null)
			{
				archeringSession.Stop();
			}
			this.ActiveArcheringSettings = null;
			this.ArcheringDrawPlayer = null;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0002069B File Offset: 0x0001E89B
		private void StopArcheringDrawTimer()
		{
			if (this.ArcheringDrawTickListenerId == 0L)
			{
				return;
			}
			this.Api.World.UnregisterGameTickListener(this.ArcheringDrawTickListenerId);
			this.ArcheringDrawTickListenerId = 0L;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000206C4 File Offset: 0x0001E8C4
		protected virtual bool ShootCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.SpawnedProjectile, mainHand);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetState(0, mainHand);
			}
			Vec3d position = player.LocalEyePos + player.Pos.XYZ;
			this.AimingSystem.RefreshTargetVec();
			Vector3 targetDirection = this.AimingSystem.TargetVec;
			targetDirection = ClientAimingSystem.Zeroing(targetDirection, this.Stats.Zeroing);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(15, 3);
			defaultInterpolatedStringHandler.AppendLiteral("shoot target=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(targetDirection.X, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<float>(targetDirection.Y, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<float>(targetDirection.Z, "0.000");
			this.LogMountedAimState(player, defaultInterpolatedStringHandler.ToStringAndClear());
			this.RangedWeaponSystem.Shoot(slot, 1, new Vector3d((double)((float)position.X), (double)((float)position.Y), (double)((float)position.Z)), new Vector3d((double)targetDirection.X, (double)targetDirection.Y, (double)targetDirection.Z), mainHand, delegate(bool _)
			{
			}, null);
			this.Attachable.ClearAttachments(player.EntityId);
			this.AttachmentSystem.SendClearPacket(player.EntityId);
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(mainHand);
			}
			this.Api.World.AddCameraShake(this.Stats.ScreenShakeStrength);
			return true;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0002087E File Offset: 0x0001EA7E
		protected virtual bool FullLoadCallback(EntityPlayer player, bool mainHand)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetState(5, true);
			}
			this.AimingSystem.AimingState = WeaponAimingState.FullCharge;
			ArcheringBowAimingSession archeringSession = this.ArcheringSession;
			if (archeringSession != null)
			{
				archeringSession.SetCharged();
			}
			this.LogMountedAimState(player, "full-draw");
			return true;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000208C0 File Offset: 0x0001EAC0
		private void LogMountedAimState(EntityPlayer player, string action)
		{
			IMountableSeat mountedOn = player.MountedOn;
			if (mountedOn == null)
			{
				return;
			}
			Vector3 target = this.AimingSystem.TargetVec;
			ICoreAPI api = this.Api;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(102, 13);
			defaultInterpolatedStringHandler.AppendLiteral("Mounted bow ");
			defaultInterpolatedStringHandler.AppendFormatted(action);
			defaultInterpolatedStringHandler.AppendLiteral(": mount=");
			Entity entity = mountedOn.Entity;
			defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>((entity != null) ? entity.Code : null);
			defaultInterpolatedStringHandler.AppendLiteral(" seat=");
			defaultInterpolatedStringHandler.AppendFormatted(mountedOn.SeatId);
			defaultInterpolatedStringHandler.AppendLiteral(" angleMode=");
			defaultInterpolatedStringHandler.AppendFormatted<EnumMountAngleMode>(mountedOn.AngleMode);
			defaultInterpolatedStringHandler.AppendLiteral(" bodyYawLimit=");
			SeatConfig config = mountedOn.Config;
			defaultInterpolatedStringHandler.AppendFormatted(((config != null) ? ((config.BodyYawLimit != null) ? config.BodyYawLimit.GetValueOrDefault().ToString() : null) : null) ?? "none");
			defaultInterpolatedStringHandler.AppendLiteral(" state=");
			defaultInterpolatedStringHandler.AppendFormatted<BowState>(base.GetState<BowState>(true));
			defaultInterpolatedStringHandler.AppendLiteral(" aimingState=");
			defaultInterpolatedStringHandler.AppendFormatted<WeaponAimingState>(this.AimingSystem.AimingState);
			defaultInterpolatedStringHandler.AppendLiteral(" pitch=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(player.Pos.Pitch, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(" yaw=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(player.Pos.Yaw, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(" bodyYaw=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(player.BodyYaw, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(" target=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(target.X, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<float>(target.Y, "0.000");
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<float>(target.Z, "0.000");
			LoggerUtil.Verbose(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00020AAC File Offset: 0x0001ECAC
		[return: Nullable(2)]
		protected virtual ItemSlot GetArrowSlot(EntityPlayer player)
		{
			ItemSlot arrowSlot = null;
			player.WalkInventory(delegate(ItemSlot slot)
			{
				bool flag;
				if (slot == null)
				{
					flag = (null != null);
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					flag = (((itemstack != null) ? itemstack.Item : null) != null);
				}
				if (!flag)
				{
					return true;
				}
				if (slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.AmmoSelector.SelectedAmmo, slot.Itemstack.Item.Code.ToString()))
				{
					arrowSlot = slot;
					return false;
				}
				return true;
			});
			if (arrowSlot == null)
			{
				player.WalkInventory(delegate(ItemSlot slot)
				{
					bool flag;
					if (slot == null)
					{
						flag = (null != null);
					}
					else
					{
						ItemStack itemstack = slot.Itemstack;
						flag = (((itemstack != null) ? itemstack.Item : null) != null);
					}
					if (!flag)
					{
						return true;
					}
					if (slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.ArrowWildcard, slot.Itemstack.Item.Code.ToString()))
					{
						arrowSlot = slot;
						return false;
					}
					return true;
				});
			}
			return arrowSlot;
		}

		// Token: 0x04000256 RID: 598
		protected new readonly ICoreClientAPI Api;

		// Token: 0x04000257 RID: 599
		protected readonly AnimatableAttachable Attachable;

		// Token: 0x04000258 RID: 600
		protected readonly ModelTransform ArrowTransform;

		// Token: 0x04000259 RID: 601
		protected readonly ClientAimingSystem AimingSystem;

		// Token: 0x0400025A RID: 602
		protected readonly BowStats Stats;

		// Token: 0x0400025B RID: 603
		protected readonly AimingStats AimingStats;

		// Token: 0x0400025C RID: 604
		protected readonly AmmoSelector AmmoSelector;

		// Token: 0x0400025D RID: 605
		protected readonly Settings Settings;

		// Token: 0x0400025E RID: 606
		private readonly ArcheringBowCompat ArcheringCompat;

		// Token: 0x0400025F RID: 607
		[Nullable(2)]
		protected AimingAnimationController AimingAnimationController;

		// Token: 0x04000260 RID: 608
		[Nullable(2)]
		private ArcheringBowAimingSession ArcheringSession;

		// Token: 0x04000261 RID: 609
		[Nullable(2)]
		private ArcheringBowAimingSettings ActiveArcheringSettings;

		// Token: 0x04000262 RID: 610
		private long ArcheringDrawStartMs;

		// Token: 0x04000263 RID: 611
		private long ArcheringDrawTickListenerId;

		// Token: 0x04000264 RID: 612
		[Nullable(2)]
		private EntityPlayer ArcheringDrawPlayer;

		// Token: 0x04000265 RID: 613
		private bool ArcheringDrawMainHand;

		// Token: 0x04000266 RID: 614
		protected bool AfterLoad;
	}
}
