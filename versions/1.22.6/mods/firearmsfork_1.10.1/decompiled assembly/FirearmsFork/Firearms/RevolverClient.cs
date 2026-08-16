using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul;
using CombatOverhaul.Animations;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.RangedSystems.Aiming;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Firearms
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	public class RevolverClient : RangeWeaponClient
	{
		// Token: 0x0600022F RID: 559 RVA: 0x0000A698 File Offset: 0x00008898
		public RevolverClient(ICoreClientAPI api, Item item) : base(api, item)
		{
			AnimatableAttachable collectibleBehavior = item.GetCollectibleBehavior<AnimatableAttachable>(true);
			if (collectibleBehavior == null)
			{
				throw new Exception("Firearm should have AnimatableAttachable behavior.");
			}
			this.Attachable = collectibleBehavior;
			ClientAimingSystem aimingSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).AimingSystem;
			if (aimingSystem == null)
			{
				throw new Exception();
			}
			this.AimingSystem = aimingSystem;
			this.BulletTransform = new ModelTransform(item.Attributes["BulletTransform"].AsObject<ModelTransformNoDefaults>(null) ?? new ModelTransformNoDefaults(), ModelTransform.BlockDefaultTp());
			this.FlaskTransform = new ModelTransform(item.Attributes["FlaskTransform"].AsObject<ModelTransformNoDefaults>(null) ?? new ModelTransformNoDefaults(), ModelTransform.BlockDefaultTp());
			this.LoadingEquipmentTransform = new ModelTransform(item.Attributes["LoadingEquipmentTransform"].AsObject<ModelTransformNoDefaults>(null) ?? new ModelTransformNoDefaults(), ModelTransform.BlockDefaultTp());
			this.Stats = item.Attributes.AsObject<RevolverStats>(null);
			this.AimingStats = this.Stats.Aiming.ToStats();
			this.TwoHanded = this.Stats.TwoHanded;
			this.Stats.ApplyLoadStagesTemplate();
			FirearmsModSystem system = api.ModLoader.GetModSystem<FirearmsModSystem>(true);
			system.SettingsChanged += delegate(FirearmsSettings settings)
			{
				this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(settings.AimingCursorType, true);
			};
			this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(system.Settings.AimingCursorType, true);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000A810 File Offset: 0x00008A10
		public override void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
			this.Attachable.ClearAttachments(player.EntityId);
			this.AimingSystem.AimingState = 0;
			this.CurrentLoadStage = this.GetLoadStage(slot);
			this.CurrentReadyState = this.GetReadyState(slot);
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.CylinderPositionAnimation, "cylinder-position", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayFpAndTp(mainHand, currentStage.LoadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
			}
			switch (this.CurrentReadyState)
			{
			case RevolverReadyState.Unloaded:
			case RevolverReadyState.Ready:
				break;
			case RevolverReadyState.Fired:
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 == null)
				{
					return;
				}
				animationBehavior3.PlayFpAndTp(mainHand, currentStage.FiredAnimation, "lock", 1f, 1.1f, null, null, true);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000A900 File Offset: 0x00008B00
		public override void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
			RevolverState revolverState = (RevolverState)state;
			if (revolverState - RevolverState.StartLoading > 3)
			{
				if (revolverState - RevolverState.Aim <= 1)
				{
					this.RangedWeaponSystem.SendStatusChange(player, 3, mainHand);
				}
			}
			else
			{
				this.ReloadActionId++;
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
			}
			this.Attachable.ClearAttachments(player.EntityId);
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			this.AimingSystem.AimingState = 0;
			this.AimingSystem.StopAiming();
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior == null)
			{
				return;
			}
			playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000A9B0 File Offset: 0x00008BB0
		public override void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			base.OnRegistered(behavior, api);
			this.AnimationBehavior = behavior.entity.GetBehavior<FirstPersonAnimationsBehavior>();
			this.TpAnimationBehavior = behavior.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
			this.AimingAnimationController = new AimingAnimationController(this.AimingSystem, this.AnimationBehavior, this.AimingStats);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000AA04 File Offset: 0x00008C04
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000AA0C File Offset: 0x00008C0C
		protected int CurrentLoadStage
		{
			get
			{
				return this._currentLoadStage;
			}
			set
			{
				this._currentLoadStage = value;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000AA18 File Offset: 0x00008C18
		[ActionEventHandler(10, 2)]
		protected virtual bool StartLoad(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			RevolverClient.<>c__DisplayClass36_0 CS$<>8__locals1 = new RevolverClient.<>c__DisplayClass36_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.slot = slot;
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.mainHand = mainHand;
			if (!RangeWeaponClient.CheckState<RevolverState>(state, new RevolverState[1]) || !CS$<>8__locals1.mainHand)
			{
				return false;
			}
			if (!Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 13) || Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 0) || Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 1) || Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 2) || Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 3))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(CS$<>8__locals1.player, CS$<>8__locals1.mainHand, eventData))
			{
				return false;
			}
			if (eventData.AltPressed || !base.CheckForOtherHandEmpty(CS$<>8__locals1.mainHand, CS$<>8__locals1.player))
			{
				return false;
			}
			RevolverReadyState readyStage = this.GetReadyState(CS$<>8__locals1.slot);
			int maxLoadStage = this.Stats.LoadStages.Length - 1;
			if (readyStage == RevolverReadyState.Fired)
			{
				return false;
			}
			if (readyStage == RevolverReadyState.Ready && this.CurrentLoadStage == maxLoadStage)
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			if (!currentStage.CanLoad)
			{
				return false;
			}
			if (!this.CheckRequirement(this.Stats.LoadingRequirementWildcard, CS$<>8__locals1.player) || !this.CheckWadding(CS$<>8__locals1.player, currentStage.WaddingUsed) || !this.CheckFlask(CS$<>8__locals1.player, currentStage.PowderConsumption) || !this.CheckAmmo(CS$<>8__locals1.player, currentStage.BulletLoaded))
			{
				return false;
			}
			RevolverClient.<>c__DisplayClass36_0 CS$<>8__locals2 = CS$<>8__locals1;
			int reloadActionId = this.ReloadActionId + 1;
			this.ReloadActionId = reloadActionId;
			CS$<>8__locals2.reloadActionId = reloadActionId;
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopFpAndTp("cylinder-position");
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				bool mainHand2 = CS$<>8__locals1.mainHand;
				string startLoadingAnimation = currentStage.StartLoadingAnimation;
				float animationSpeed = this.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats.ProficiencyStat, CS$<>8__locals1.slot.Itemstack);
				animationBehavior2.PlayFpAndTp(mainHand2, startLoadingAnimation, this.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, () => CS$<>8__locals1.<>4__this.StartLoadCallback(CS$<>8__locals1.slot, CS$<>8__locals1.player, CS$<>8__locals1.mainHand, CS$<>8__locals1.<>4__this.CurrentLoadStage, CS$<>8__locals1.reloadActionId), null, true);
			}
			base.SetState<RevolverState>(RevolverState.StartLoading, CS$<>8__locals1.mainHand);
			this.RangedWeaponSystem.SendStatusChange(CS$<>8__locals1.player, 0, CS$<>8__locals1.mainHand);
			FirearmsReloadSafety.SetReloadWalkspeedPenalty(this.PlayerBehavior, CS$<>8__locals1.mainHand, "CombatOverhaul:held-item-mainhand", "CombatOverhaul:held-item-offhand", currentStage.LoadSpeedPenalty);
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AC64 File Offset: 0x00008E64
		protected virtual bool StartLoadCallback(ItemSlot slot, EntityPlayer player, bool mainHand, int stage, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.StartLoading
			}))
			{
				return true;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null && !playerBehavior.ActionListener.IsActive(10))
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.PlayFpAndTp(mainHand, currentStage.LoadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
				}
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					bool mainHand2 = mainHand;
					string cancelLoadingAnimation = currentStage.CancelLoadingAnimation;
					float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
					animationBehavior2.PlayFpAndTp(mainHand2, cancelLoadingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.FinishLoadCallback(slot, player, mainHand, this.CurrentLoadStage, reloadActionId), null, true);
				}
				base.SetState<RevolverState>(RevolverState.FinishLoading, mainHand);
				return true;
			}
			base.SetState<RevolverState>(RevolverState.Loading, mainHand);
			int nextStageIndex = currentStage.LoadToStage;
			RevolverLoadStageStats nextStage = this.Stats.LoadStages[nextStageIndex];
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				animationBehavior3.PlayFpAndTp(mainHand, nextStage.UnloadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
			if (animationBehavior4 != null)
			{
				bool mainHand3 = mainHand;
				string loadingAnimation = nextStage.LoadingAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior4.PlayFpAndTp(mainHand3, loadingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.LoadStageCallback(slot, player, mainHand, nextStageIndex, reloadActionId), delegate(string code)
				{
					this.LoadStageCallbackHandler(code, slot, player, mainHand, nextStageIndex, reloadActionId);
				}, true);
			}
			return true;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000AE58 File Offset: 0x00009058
		protected virtual bool LoadStageCallback(ItemSlot slot, EntityPlayer player, bool mainHand, int stage, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.Loading
			}))
			{
				return true;
			}
			if (stage == this.CurrentLoadStage && this.CurrentReadyState == RevolverReadyState.Ready)
			{
				return true;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
			if (!this.SendReloadPacket(slot, player, mainHand, stage))
			{
				RevolverLoadStageStats previousStage = this.Stats.LoadStages[currentStage.UnloadToStage];
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					bool mainHand2 = mainHand;
					string finishLoadingAnimation = previousStage.FinishLoadingAnimation;
					float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
					animationBehavior.PlayFpAndTp(mainHand2, finishLoadingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.FinishLoadCallback(slot, player, mainHand, stage, reloadActionId), null, true);
				}
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.PlayFpAndTp(mainHand, previousStage.LoadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
				}
				base.SetState<RevolverState>(RevolverState.FinishLoading, mainHand);
				return true;
			}
			this.CurrentLoadStage = stage;
			this.CurrentReadyState = RevolverReadyState.Ready;
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				animationBehavior3.PlayFpAndTp(mainHand, currentStage.LoadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
			}
			int nextStageIndex = currentStage.LoadToStage;
			RevolverLoadStageStats nextStage = this.Stats.LoadStages[nextStageIndex];
			if (!currentStage.CanLoad || !this.CheckRequirement(this.Stats.LoadingRequirementWildcard, player) || !this.CheckWadding(player, currentStage.WaddingUsed) || !this.CheckFlask(player, currentStage.PowderConsumption) || !this.CheckAmmo(player, currentStage.BulletLoaded))
			{
				FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
				if (animationBehavior4 != null)
				{
					bool mainHand3 = mainHand;
					string finishLoadingAnimation2 = currentStage.FinishLoadingAnimation;
					float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
					animationBehavior4.PlayFpAndTp(mainHand3, finishLoadingAnimation2, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.FinishLoadCallback(slot, player, mainHand, stage, reloadActionId), null, true);
				}
				base.SetState<RevolverState>(RevolverState.FinishLoading, mainHand);
				return true;
			}
			FirstPersonAnimationsBehavior animationBehavior5 = this.AnimationBehavior;
			if (animationBehavior5 != null)
			{
				bool mainHand4 = mainHand;
				string continueLoadingAnimation = currentStage.ContinueLoadingAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior5.PlayFpAndTp(mainHand4, continueLoadingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.ContinueLoadCallback(slot, player, mainHand, nextStageIndex, reloadActionId), null, true);
			}
			FirearmsReloadSafety.SetReloadWalkspeedPenalty(this.PlayerBehavior, mainHand, "CombatOverhaul:held-item-mainhand", "CombatOverhaul:held-item-offhand", nextStage.LoadSpeedPenalty);
			return true;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B174 File Offset: 0x00009374
		protected virtual bool ContinueLoadCallback(ItemSlot slot, EntityPlayer player, bool mainHand, int stage, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.Loading
			}))
			{
				return true;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand2 = mainHand;
				string loadingAnimation = currentStage.LoadingAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior.PlayFpAndTp(mainHand2, loadingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.LoadStageCallback(slot, player, mainHand, stage, reloadActionId), delegate(string code)
				{
					this.LoadStageCallbackHandler(code, slot, player, mainHand, stage, reloadActionId);
				}, true);
			}
			return true;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B25C File Offset: 0x0000945C
		protected virtual void LoadStageCallbackHandler(string code, ItemSlot slot, EntityPlayer player, bool mainHand, int stage, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.Loading
			}))
			{
				return;
			}
			if (!(code == "attach"))
			{
				if (!(code == "detach"))
				{
					return;
				}
				this.Attachable.ClearAttachments(player.EntityId);
			}
			else
			{
				ItemStack bulletAttachment = this.GetBulletAttachmentStack();
				if (bulletAttachment != null)
				{
					this.Attachable.SetAttachment(player.EntityId, "bullet", bulletAttachment, this.BulletTransform, true, false);
				}
				ItemSlot flaskSlot = null;
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
					if (WildcardUtil.Match(this.Stats.FlaskWildcard, slot.Itemstack.Item.Code.ToString()))
					{
						flaskSlot = slot;
						return false;
					}
					return true;
				});
				if (flaskSlot != null)
				{
					this.Attachable.SetAttachment(player.EntityId, "flask", flaskSlot.Itemstack, this.FlaskTransform, true, false);
					return;
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B338 File Offset: 0x00009538
		protected virtual bool FinishLoadCallback(ItemSlot slot, EntityPlayer player, bool mainHand, int stage, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.FinishLoading
			}))
			{
				return true;
			}
			base.SetState<RevolverState>(RevolverState.Idle, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.CylinderPositionAnimation, "cylinder-position", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayReadyAnimationFpAndTp(mainHand);
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			this.Attachable.ClearAttachments(player.EntityId);
			return true;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B404 File Offset: 0x00009604
		[ActionEventHandler(10, 2)]
		protected virtual bool Aim(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<RevolverState>(state, new RevolverState[1]) || !mainHand)
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (eventData.AltPressed || !base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.AimAnimation, this.AnimationCategory(mainHand), 1f, 1f, () => this.AimCallback(slot, player, mainHand, this.CurrentLoadStage), null, true);
			}
			base.SetState<RevolverState>(RevolverState.Aim, mainHand);
			this.AimingSystem.StartAiming(this.GetAimStats(slot));
			this.AimingSystem.AimingState = 3;
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Play(mainHand);
			}
			this.RangedWeaponSystem.SendStatusChange(player, 2, mainHand);
			return true;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000B537 File Offset: 0x00009737
		protected virtual bool AimCallback(ItemSlot slot, EntityPlayer player, bool mainHand, int stage)
		{
			return true;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000B53C File Offset: 0x0000973C
		[ActionEventHandler(10, 4)]
		protected virtual bool CancelLoading(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<RevolverState>(state, new RevolverState[]
			{
				RevolverState.Loading
			}) || !mainHand)
			{
				return false;
			}
			if (eventData.AltPressed)
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			int reloadActionId = this.ReloadActionId;
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.LoadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				bool mainHand2 = mainHand;
				string cancelLoadingAnimation = currentStage.CancelLoadingAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior2.PlayFpAndTp(mainHand2, cancelLoadingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.FinishLoadCallback(slot, player, mainHand, this.CurrentLoadStage, reloadActionId), null, true);
			}
			base.SetState<RevolverState>(RevolverState.FinishLoading, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			this.Attachable.ClearAttachments(player.EntityId);
			return true;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000B6B0 File Offset: 0x000098B0
		[ActionEventHandler(10, 4)]
		protected virtual bool Ease(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<RevolverState>(state, new RevolverState[]
			{
				RevolverState.Aim
			}) || !mainHand)
			{
				return false;
			}
			if (eventData.AltPressed)
			{
				return false;
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimationFpAndTp(mainHand);
			}
			this.AimingSystem.StopAiming();
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			base.SetState<RevolverState>(RevolverState.Idle, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, 3, mainHand);
			return true;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B72B File Offset: 0x0000992B
		[ActionEventHandler(9, 1)]
		protected virtual bool Shoot(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			return this.TryShoot(slot, player, ref state, eventData, mainHand);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B73A File Offset: 0x0000993A
		[ActionEventHandler(9, 2)]
		protected virtual bool ShootAutomatic(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Stats.AutomaticFire)
			{
				return false;
			}
			if (this.CurrentReadyState == RevolverReadyState.Ready)
			{
				return this.TryShoot(slot, player, ref state, eventData, mainHand);
			}
			return this.CurrentReadyState == RevolverReadyState.Fired && this.TryCock(slot, player, ref state, eventData, mainHand);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000B77C File Offset: 0x0000997C
		protected bool ShouldContinueAutomaticFire(bool mainHand)
		{
			if (this.Stats.AutomaticFire)
			{
				ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
				if (playerBehavior != null && playerBehavior.ActionListener.IsActive(9))
				{
					ActionsManagerPlayerBehavior playerBehavior2 = this.PlayerBehavior;
					return playerBehavior2 != null && playerBehavior2.ActionListener.IsActive(10);
				}
			}
			return false;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000B7CC File Offset: 0x000099CC
		protected virtual bool TryShoot(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand)
		{
			if (this.CurrentReadyState != RevolverReadyState.Ready)
			{
				return false;
			}
			if (!RangeWeaponClient.CheckState<RevolverState>(state, new RevolverState[]
			{
				RevolverState.Aim
			}) || !mainHand)
			{
				return false;
			}
			if (!this.ShotGateOpen(mainHand))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (eventData.AltPressed || !base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			if (!currentStage.CanFire)
			{
				return false;
			}
			base.SetState<RevolverState>(RevolverState.Shoot, mainHand);
			this.CurrentReadyState = RevolverReadyState.Fired;
			this.SendWeaponState(this.CurrentLoadStage, RevolverReadyState.Fired, slot, player, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, 4, mainHand);
			this.SetNextShotAllowed(mainHand, this.Api.World.ElapsedMilliseconds + (long)base.GetActionDelayMs(player, currentStage.ShootAnimation, this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack), 500, null));
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.UnloadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopFpAndTp("cylinder-position");
			}
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				animationBehavior3.StopFpAndTp("lock");
			}
			FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
			if (animationBehavior4 != null)
			{
				animationBehavior4.PlayFpAndTp(mainHand, currentStage.ShootAnimation, this.AnimationCategory(mainHand), 1f, 1f, () => this.ShootCallback(slot, player, mainHand), delegate(string callback)
				{
					this.ShootAnimationCallback(callback, slot, player, mainHand, this.CurrentLoadStage);
				}, true);
			}
			return true;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000B9D4 File Offset: 0x00009BD4
		protected virtual bool TryShootAutomaticContinuation(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (this.CurrentReadyState != RevolverReadyState.Ready)
			{
				return false;
			}
			if (!base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.Aim
			}) || !mainHand)
			{
				return false;
			}
			if (!this.ShotGateOpen(mainHand))
			{
				return false;
			}
			if (!base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			if (!currentStage.CanFire)
			{
				return false;
			}
			base.SetState<RevolverState>(RevolverState.Shoot, mainHand);
			this.CurrentReadyState = RevolverReadyState.Fired;
			this.SendWeaponState(this.CurrentLoadStage, RevolverReadyState.Fired, slot, player, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, 4, mainHand);
			this.SetNextShotAllowed(mainHand, this.Api.World.ElapsedMilliseconds + (long)base.GetActionDelayMs(player, currentStage.ShootAnimation, this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack), 500, null));
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.UnloadedAnimation, "cylinder-load-gates", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopFpAndTp("cylinder-position");
			}
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				animationBehavior3.StopFpAndTp("lock");
			}
			FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
			if (animationBehavior4 != null)
			{
				animationBehavior4.PlayFpAndTp(mainHand, currentStage.ShootAnimation, this.AnimationCategory(mainHand), 1f, 1f, () => this.ShootCallback(slot, player, mainHand), delegate(string callback)
				{
					this.ShootAnimationCallback(callback, slot, player, mainHand, this.CurrentLoadStage);
				}, true);
			}
			return true;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000BBBE File Offset: 0x00009DBE
		protected bool ShotGateOpen(bool mainHand)
		{
			return this.Api.World.ElapsedMilliseconds >= this.GetNextShotAllowed(mainHand);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000BBDC File Offset: 0x00009DDC
		protected long GetNextShotAllowed(bool mainHand)
		{
			if (!mainHand)
			{
				return this.NextShotAllowedOffHandMs;
			}
			return this.NextShotAllowedMainHandMs;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000BBEE File Offset: 0x00009DEE
		protected void SetNextShotAllowed(bool mainHand, long value)
		{
			if (mainHand)
			{
				this.NextShotAllowedMainHandMs = value;
				return;
			}
			this.NextShotAllowedOffHandMs = value;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000BC04 File Offset: 0x00009E04
		protected virtual bool ShootCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, currentStage.CylinderPositionAnimation, "cylinder-position", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayFpAndTp(mainHand, currentStage.FiredAnimation, "lock", 1f, 1.1f, null, null, true);
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null && !playerBehavior.ActionListener.IsActive(10))
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayReadyAnimationFpAndTp(mainHand);
				}
				this.AimingSystem.StopAiming();
				AimingAnimationController aimingAnimationController = this.AimingAnimationController;
				if (aimingAnimationController != null)
				{
					aimingAnimationController.Stop(mainHand);
				}
				base.SetState<RevolverState>(RevolverState.Idle, mainHand);
				this.RangedWeaponSystem.SendStatusChange(player, 3, mainHand);
				return true;
			}
			base.SetState<RevolverState>(RevolverState.Aim, mainHand);
			if (this.ShouldContinueAutomaticFire(mainHand) && this.TryCockAutomaticContinuation(slot, player, mainHand))
			{
				return true;
			}
			FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
			if (animationBehavior4 != null)
			{
				bool mainHand2 = mainHand;
				string aimAnimation = currentStage.AimAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior4.PlayFpAndTp(mainHand2, aimAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.AimCallback(slot, player, mainHand, this.CurrentLoadStage), null, true);
			}
			AimingAnimationController aimingAnimationController2 = this.AimingAnimationController;
			if (aimingAnimationController2 != null)
			{
				aimingAnimationController2.Play(mainHand);
			}
			return true;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000BDD1 File Offset: 0x00009FD1
		protected virtual void ShootAnimationCallback(string callback, ItemSlot slot, EntityPlayer player, bool mainHand, int stage)
		{
			if (callback == "shoot")
			{
				this.RangedWeaponSystem.SendStatusChange(player, 5, mainHand);
				this.SendShootPacket(slot, player, mainHand, stage);
				this.ApplyRecoil(slot, player, stage);
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000BE08 File Offset: 0x0000A008
		protected virtual void ApplyRecoil(ItemSlot slot, EntityPlayer player, int stage)
		{
			if (!FirearmsAmmoUtility.RecoilEnabled)
			{
				return;
			}
			IClientPlayer player2 = this.Api.World.Player;
			long? num;
			if (player2 == null)
			{
				num = default(long?);
			}
			else
			{
				EntityPlayer entity = player2.Entity;
				num = ((entity != null) ? new long?(entity.EntityId) : default(long?));
			}
			long? num2 = num;
			long entityId = player.EntityId;
			if (!(num2.GetValueOrDefault() == entityId & num2 != null))
			{
				return;
			}
			long now = this.Api.World.ElapsedMilliseconds;
			if (now - this.LastRecoilTimestampMs < 80L)
			{
				return;
			}
			this.LastRecoilTimestampMs = now;
			RecoilStats recoil = this.Stats.Recoil ?? RecoilStats.RevolverDefaults();
			if (!recoil.Enabled)
			{
				return;
			}
			float estimatedDamage = this.EstimateShotDamage(slot, stage);
			float verticalDeg = recoil.VerticalDegrees(estimatedDamage);
			float horizontalDeg = recoil.HorizontalDegrees(estimatedDamage, verticalDeg);
			FirearmsRecoilSystem.AddRecoil(verticalDeg, horizontalDeg, recoil);
			this.Api.World.AddCameraShake(recoil.CameraShake(estimatedDamage));
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000BEFC File Offset: 0x0000A0FC
		protected virtual float EstimateShotDamage(ItemSlot slot, int stage)
		{
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			float projectileBaseDamage = 1f;
			this.Inventory.Read(slot, "magazine");
			if (this.Inventory.Items.Count > 0)
			{
				ItemStack ammo = this.Inventory.Items[0];
				Item item = ammo.Item;
				ProjectileStats projectileStats2;
				if (item == null)
				{
					projectileStats2 = null;
				}
				else
				{
					ProjectileBehavior collectibleBehavior = item.GetCollectibleBehavior<ProjectileBehavior>(true);
					projectileStats2 = ((collectibleBehavior != null) ? collectibleBehavior.GetStats(ammo) : null);
				}
				ProjectileStats projectileStats = projectileStats2;
				if (((projectileStats != null) ? projectileStats.DamageStats : null) != null)
				{
					projectileBaseDamage = projectileStats.DamageStats.Damage;
				}
			}
			this.Inventory.Clear();
			float weaponMultiplier = Math.Max(0.1f, this.Stats.FiringStats.BulletDamageMultiplier * stackStats.DamageMultiplier);
			return Math.Max(0.1f, projectileBaseDamage * weaponMultiplier);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000BFCA File Offset: 0x0000A1CA
		[ActionEventHandler(9, 1)]
		protected virtual bool Cock(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			return this.TryCock(slot, player, ref state, eventData, mainHand);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000BFDC File Offset: 0x0000A1DC
		protected virtual bool TryCock(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand)
		{
			if (this.CurrentReadyState != RevolverReadyState.Fired)
			{
				return false;
			}
			if (!RangeWeaponClient.CheckState<RevolverState>(state, new RevolverState[]
			{
				RevolverState.Aim
			}) || !mainHand)
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (eventData.AltPressed || !base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			base.SetState<RevolverState>(RevolverState.Cocking, mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopFpAndTp("cylinder-position");
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopFpAndTp("lock");
			}
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				bool mainHand2 = mainHand;
				string cockingAnimation = currentStage.CockingAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior3.PlayFpAndTp(mainHand2, cockingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.CockCallback(slot, player, mainHand), null, true);
			}
			this.AimingSystem.StopAiming();
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			return true;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C13C File Offset: 0x0000A33C
		protected virtual bool TryCockAutomaticContinuation(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (this.CurrentReadyState != RevolverReadyState.Fired)
			{
				return false;
			}
			if (!base.CheckState<RevolverState>(mainHand, new RevolverState[]
			{
				RevolverState.Aim
			}) || !mainHand)
			{
				return false;
			}
			if (!base.CheckForOtherHandEmpty(mainHand, player))
			{
				return false;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			base.SetState<RevolverState>(RevolverState.Cocking, mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopFpAndTp("cylinder-position");
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopFpAndTp("lock");
			}
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				bool mainHand2 = mainHand;
				string cockingAnimation = currentStage.CockingAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, slot.Itemstack);
				animationBehavior3.PlayFpAndTp(mainHand2, cockingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, () => this.CockCallback(slot, player, mainHand), null, true);
			}
			this.AimingSystem.StopAiming();
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			return true;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000C280 File Offset: 0x0000A480
		protected virtual bool CockCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			base.SetState<RevolverState>(RevolverState.Aim, mainHand);
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[this.CurrentLoadStage];
			this.CurrentLoadStage = currentStage.UnloadToStage;
			RevolverLoadStageStats nextStage = this.Stats.LoadStages[this.CurrentLoadStage];
			if (nextStage.CanFire)
			{
				this.CurrentReadyState = RevolverReadyState.Ready;
				this.SendWeaponState(this.CurrentLoadStage, RevolverReadyState.Ready, slot, player, mainHand);
			}
			else
			{
				this.CurrentReadyState = RevolverReadyState.Unloaded;
				this.SendWeaponState(this.CurrentLoadStage, RevolverReadyState.Unloaded, slot, player, mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayFpAndTp(mainHand, nextStage.CylinderPositionAnimation, "cylinder-position", 1f, 1.1f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayFpAndTp(mainHand, nextStage.CockedAnimation, "lock", 1f, 1.1f, null, null, true);
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null && !playerBehavior.ActionListener.IsActive(10))
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayReadyAnimationFpAndTp(mainHand);
				}
				this.AimingSystem.StopAiming();
				AimingAnimationController aimingAnimationController = this.AimingAnimationController;
				if (aimingAnimationController != null)
				{
					aimingAnimationController.Stop(mainHand);
				}
				base.SetState<RevolverState>(RevolverState.Idle, mainHand);
				return true;
			}
			this.AimingSystem.StartAiming(this.GetAimStats(slot));
			this.AimingSystem.AimingState = 3;
			AimingAnimationController aimingAnimationController2 = this.AimingAnimationController;
			if (aimingAnimationController2 != null)
			{
				aimingAnimationController2.Play(mainHand);
			}
			if (this.ShouldContinueAutomaticFire(mainHand) && this.TryShootAutomaticContinuation(slot, player, mainHand))
			{
				return true;
			}
			FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
			if (animationBehavior4 != null)
			{
				animationBehavior4.PlayFpAndTp(mainHand, nextStage.AimAnimation, this.AnimationCategory(mainHand), 1f, 1f, () => this.AimCallback(slot, player, mainHand, this.CurrentLoadStage), null, true);
			}
			return true;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		protected virtual bool SendReloadPacket(ItemSlot slot, EntityPlayer player, bool mainHand, int stage)
		{
			ItemSlot ammoSlot = null;
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
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
				ItemStack itemstack2 = slot.Itemstack;
				if (((itemstack2 != null) ? itemstack2.Item : null) != null && slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()) && FirearmsAmmoUtility.HasEnoughBulletItems(slot, currentStage.BulletLoaded, this.Stats.BulletsLoadedPerBulletItem))
				{
					ammoSlot = slot;
					return false;
				}
				return true;
			});
			if (ammoSlot == null)
			{
				return true;
			}
			int bulletItemsRequired = FirearmsAmmoUtility.BulletItemsRequired(currentStage.BulletLoaded, this.Stats.BulletsLoadedPerBulletItem);
			int expectedStage = this.CurrentLoadStage;
			RevolverReadyState expectedReadyState = this.CurrentReadyState;
			this.RangedWeaponSystem.Reload(slot2, ammoSlot, bulletItemsRequired, mainHand, delegate(bool success)
			{
				this.LoadServerCallback(success, stage, mainHand, player);
			}, new byte[]
			{
				(byte)stage,
				1,
				(byte)expectedStage,
				(byte)expectedReadyState
			});
			return true;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C590 File Offset: 0x0000A790
		protected virtual void SendWeaponState(int stage, RevolverReadyState state, ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			this.RangedWeaponSystem.Reload(slot, slot, 0, mainHand, delegate(bool success)
			{
			}, new byte[]
			{
				(byte)stage,
				(byte)state
			});
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		protected virtual void LoadServerCallback(bool success, int stage, bool mainHand, EntityPlayer player)
		{
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		protected virtual void SendShootPacket(ItemSlot slot, EntityPlayer player, bool mainHand, int stage)
		{
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
			Vec3d position = player.LocalEyePos + player.Pos.XYZ;
			Vector3 targetDirection = this.AimingSystem.TargetVec;
			targetDirection = ClientAimingSystem.Zeroing(targetDirection, this.Stats.FiringStats.Zeroing);
			this.RangedWeaponSystem.Shoot(slot, currentStage.BulletsFired, new Vector3d((double)((float)position.X), (double)((float)position.Y), (double)((float)position.Z)), new Vector3d((double)targetDirection.X, (double)targetDirection.Y, (double)targetDirection.Z), mainHand, new Action<bool>(this.ShootServerCallback), new byte[]
			{
				(byte)stage,
				2
			});
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000C69D File Offset: 0x0000A89D
		protected virtual void ShootServerCallback(bool success)
		{
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
		protected AimingStats GetAimStats(ItemSlot slot)
		{
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			AimingStats aimingStats = this.AimingStats.Clone();
			aimingStats.AimDifficulty *= stackStats.AimingDifficulty;
			return aimingStats;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000C6D7 File Offset: 0x0000A8D7
		protected int GetLoadStage(ItemSlot slot)
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack == null)
			{
				return 0;
			}
			return itemstack.Attributes.GetAsInt("CombatOverhaul:loading-stage", 0);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000C6F5 File Offset: 0x0000A8F5
		protected RevolverReadyState GetReadyState(ItemSlot slot)
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack == null)
			{
				return RevolverReadyState.Unloaded;
			}
			return (RevolverReadyState)itemstack.Attributes.GetAsInt("CombatOverhaul:ready-stage", 0);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000C714 File Offset: 0x0000A914
		protected bool CheckRequirement(string requirementWildcard, EntityPlayer player)
		{
			if (requirementWildcard == "")
			{
				return true;
			}
			ItemSlot requirementSlot = null;
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
				if (WildcardUtil.Match(requirementWildcard, slot.Itemstack.Item.Code.ToString()))
				{
					requirementSlot = slot;
					return false;
				}
				return true;
			});
			if (requirementSlot == null)
			{
				this.Api.TriggerIngameError(this, "missingRequirement", Lang.Get(this.Stats.LoadingRequirementMessage, Array.Empty<object>()));
			}
			return requirementSlot != null;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000C794 File Offset: 0x0000A994
		protected bool CheckWadding(EntityPlayer player, int amount)
		{
			if (amount <= 0)
			{
				return true;
			}
			ItemSlot waddingSlot = null;
			player.WalkInventory(delegate(ItemSlot slot)
			{
				AssetLocation assetLocation;
				if (slot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						Item item = itemstack.Item;
						assetLocation = ((item != null) ? item.Code : null);
					}
				}
				if (assetLocation == null)
				{
					return true;
				}
				if (WildcardUtil.Match(this.Stats.WaddingWildcard, slot.Itemstack.Item.Code.ToString()) && slot.Itemstack.StackSize >= amount)
				{
					waddingSlot = slot;
					return false;
				}
				return true;
			});
			bool flag = waddingSlot != null;
			if (!flag)
			{
				this.Api.TriggerIngameError(this, "missingWadding", Lang.Get("maltiezfirearms:requirement-missing-wadding", Array.Empty<object>()));
			}
			return flag;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000C808 File Offset: 0x0000AA08
		protected bool CheckFlask(EntityPlayer player, int powderNeeded)
		{
			if (powderNeeded <= 0)
			{
				return true;
			}
			ItemSlot flaskSlot = null;
			player.WalkInventory(delegate(ItemSlot slot)
			{
				AssetLocation assetLocation;
				if (slot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						Item item = itemstack.Item;
						assetLocation = ((item != null) ? item.Code : null);
					}
				}
				if (assetLocation == null)
				{
					return true;
				}
				if (WildcardUtil.Match(this.Stats.FlaskWildcard, slot.Itemstack.Item.Code.ToString()) && slot.Itemstack.Item.GetRemainingDurability(slot.Itemstack) >= powderNeeded)
				{
					flaskSlot = slot;
					return false;
				}
				return true;
			});
			bool flag = flaskSlot != null;
			if (!flag)
			{
				this.Api.TriggerIngameError(this, "missingPowderflask", Lang.Get("maltiezfirearms:requirement-missing-powderflask", Array.Empty<object>()));
			}
			return flag;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000C87C File Offset: 0x0000AA7C
		protected bool CheckAmmo(EntityPlayer player, int amount)
		{
			ItemSlot ammoSlot = null;
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
				ItemStack itemstack2 = slot.Itemstack;
				if (((itemstack2 != null) ? itemstack2.Item : null) != null && slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()) && FirearmsAmmoUtility.HasEnoughBulletItems(slot, amount, this.Stats.BulletsLoadedPerBulletItem))
				{
					ammoSlot = slot;
					return false;
				}
				return true;
			});
			if (ammoSlot == null)
			{
				this.Api.TriggerIngameError(this, "missingAmmo", Lang.Get("maltiezfirearms:requirement-missing-ammo", Array.Empty<object>()));
				return false;
			}
			this.BulletSlot = ammoSlot;
			this.BulletAttachmentStack = RevolverClient.CreateAttachmentStack(ammoSlot.Itemstack);
			return true;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000C904 File Offset: 0x0000AB04
		[NullableContext(2)]
		protected ItemStack GetBulletAttachmentStack()
		{
			ItemSlot bulletSlot = this.BulletSlot;
			bool flag;
			if (bulletSlot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = bulletSlot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Item : null) != null);
			}
			if (flag && this.BulletSlot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, this.BulletSlot.Itemstack.Item.Code.ToString()))
			{
				this.BulletAttachmentStack = RevolverClient.CreateAttachmentStack(this.BulletSlot.Itemstack);
			}
			ItemStack bulletAttachmentStack = this.BulletAttachmentStack;
			if (bulletAttachmentStack == null)
			{
				return null;
			}
			return bulletAttachmentStack.Clone();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000C99C File Offset: 0x0000AB9C
		[NullableContext(2)]
		protected static ItemStack CreateAttachmentStack(ItemStack stack)
		{
			if (((stack != null) ? stack.Item : null) == null)
			{
				return null;
			}
			ItemStack itemStack = stack.Clone();
			itemStack.StackSize = 1;
			return itemStack;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000C9BC File Offset: 0x0000ABBC
		[return: Nullable(2)]
		protected ItemSlot GetAmmoSlot(EntityPlayer player, int amount)
		{
			ItemSlot ammoSlot = null;
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
				ItemStack itemstack2 = slot.Itemstack;
				if (((itemstack2 != null) ? itemstack2.Item : null) != null && slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()) && FirearmsAmmoUtility.HasEnoughBulletItems(slot, amount, this.Stats.BulletsLoadedPerBulletItem))
				{
					ammoSlot = slot;
					return false;
				}
				return true;
			});
			if (ammoSlot == null)
			{
				this.Api.TriggerIngameError(this, "missingAmmo", Lang.Get("maltiezfirearms:requirement-missing-ammo", Array.Empty<object>()));
				return null;
			}
			return ammoSlot;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000CA26 File Offset: 0x0000AC26
		protected string AnimationCategory(bool mainHand)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000CA36 File Offset: 0x0000AC36
		protected string ItemAnimationCategory(bool mainHand)
		{
			if (!mainHand)
			{
				return "itemOffhand";
			}
			return "item";
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000CA48 File Offset: 0x0000AC48
		protected void DebugAttach(EntityPlayer player)
		{
			ItemSlot ammoSlot1 = null;
			player.WalkInventory(delegate(ItemSlot slot)
			{
				AssetLocation assetLocation;
				if (slot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						Item item = itemstack.Item;
						assetLocation = ((item != null) ? item.Code : null);
					}
				}
				if (assetLocation == null)
				{
					return true;
				}
				if (WildcardUtil.Match(this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()))
				{
					ammoSlot1 = slot;
					return false;
				}
				return true;
			});
			if (ammoSlot1 != null)
			{
				this.Attachable.SetAttachment(player.EntityId, "bullet", ammoSlot1.Itemstack, this.BulletTransform, true, false);
			}
			ItemSlot ammoSlot2 = null;
			player.WalkInventory(delegate(ItemSlot slot)
			{
				AssetLocation assetLocation;
				if (slot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						Item item = itemstack.Item;
						assetLocation = ((item != null) ? item.Code : null);
					}
				}
				if (assetLocation == null)
				{
					return true;
				}
				if (WildcardUtil.Match(this.Stats.FlaskWildcard, slot.Itemstack.Item.Code.ToString()))
				{
					ammoSlot2 = slot;
					return false;
				}
				return true;
			});
			if (ammoSlot2 != null)
			{
				this.Attachable.SetAttachment(player.EntityId, "flask", ammoSlot2.Itemstack, this.FlaskTransform, true, false);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		protected float GetAnimationSpeed(EntityPlayer player, string stat, ItemStack stack)
		{
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(stack);
			return FirearmsReloadSafety.AnimationSpeed(base.GetAnimationSpeed(player, stat, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed);
		}

		// Token: 0x04000127 RID: 295
		[Nullable(2)]
		protected AimingAnimationController AimingAnimationController;

		// Token: 0x04000128 RID: 296
		protected readonly AnimatableAttachable Attachable;

		// Token: 0x04000129 RID: 297
		protected readonly ClientAimingSystem AimingSystem;

		// Token: 0x0400012A RID: 298
		protected readonly RevolverStats Stats;

		// Token: 0x0400012B RID: 299
		protected readonly AimingStats AimingStats;

		// Token: 0x0400012C RID: 300
		protected long LastRecoilTimestampMs = -1000L;

		// Token: 0x0400012D RID: 301
		protected long NextShotAllowedMainHandMs;

		// Token: 0x0400012E RID: 302
		protected long NextShotAllowedOffHandMs;

		// Token: 0x0400012F RID: 303
		protected readonly ItemInventoryBuffer Inventory = new ItemInventoryBuffer();

		// Token: 0x04000130 RID: 304
		protected readonly ModelTransform BulletTransform;

		// Token: 0x04000131 RID: 305
		protected readonly ModelTransform FlaskTransform;

		// Token: 0x04000132 RID: 306
		protected readonly ModelTransform LoadingEquipmentTransform;

		// Token: 0x04000133 RID: 307
		protected readonly ModelTransform PrimingEquipmentTransform;

		// Token: 0x04000134 RID: 308
		[Nullable(2)]
		protected FirstPersonAnimationsBehavior AnimationBehavior;

		// Token: 0x04000135 RID: 309
		[Nullable(2)]
		protected ThirdPersonAnimationsBehavior TpAnimationBehavior;

		// Token: 0x04000136 RID: 310
		protected const string InventoryId = "magazine";

		// Token: 0x04000137 RID: 311
		protected const string LoadingStageAttribute = "CombatOverhaul:loading-stage";

		// Token: 0x04000138 RID: 312
		protected const string WeaponReadyStageAttribute = "CombatOverhaul:ready-stage";

		// Token: 0x04000139 RID: 313
		protected const string PlayerStatsMainHandCategory = "CombatOverhaul:held-item-mainhand";

		// Token: 0x0400013A RID: 314
		protected const string PlayerStatsOffHandCategory = "CombatOverhaul:held-item-offhand";

		// Token: 0x0400013B RID: 315
		protected const string LoadingGatesAnimationCategory = "loading-gates";

		// Token: 0x0400013C RID: 316
		protected const string LockAnimationCategory = "lock";

		// Token: 0x0400013D RID: 317
		protected const string CylinderPositionAnimationCategory = "cylinder-position";

		// Token: 0x0400013E RID: 318
		protected const string CylinderLoadGatesAnimationCategory = "cylinder-load-gates";

		// Token: 0x0400013F RID: 319
		protected int ReloadActionId;

		// Token: 0x04000140 RID: 320
		[Nullable(2)]
		protected ItemSlot BulletSlot;

		// Token: 0x04000141 RID: 321
		[Nullable(2)]
		protected ItemStack BulletAttachmentStack;

		// Token: 0x04000142 RID: 322
		private int _currentLoadStage;

		// Token: 0x04000143 RID: 323
		protected RevolverReadyState CurrentReadyState;
	}
}
