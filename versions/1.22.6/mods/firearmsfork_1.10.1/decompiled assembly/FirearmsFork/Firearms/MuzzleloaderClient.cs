using System;
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
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	public class MuzzleloaderClient : RangeWeaponClient
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00006460 File Offset: 0x00004660
		public MuzzleloaderClient(ICoreClientAPI api, Item item) : base(api, item)
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
			this.PrimingEquipmentTransform = new ModelTransform(item.Attributes["PrimingEquipmentTransform"].AsObject<ModelTransformNoDefaults>(null) ?? new ModelTransformNoDefaults(), ModelTransform.BlockDefaultTp());
			this.LoadingEquipmentTransform = new ModelTransform(item.Attributes["LoadingEquipmentTransform"].AsObject<ModelTransformNoDefaults>(null) ?? new ModelTransformNoDefaults(), ModelTransform.BlockDefaultTp());
			this.Stats = item.Attributes.AsObject<MuzzleloaderStats>(null);
			this.AimingStats = this.Stats.Aiming.ToStats();
			FirearmsModSystem system = api.ModLoader.GetModSystem<FirearmsModSystem>(true);
			system.SettingsChanged += delegate(FirearmsSettings settings)
			{
				this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(settings.AimingCursorType, true);
			};
			this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(system.Settings.AimingCursorType, true);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000065E8 File Offset: 0x000047E8
		public override void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
			this.Attachable.ClearAttachments(player.EntityId);
			this.AimingSystem.AimingState = 0;
			MuzzleloaderLoadingStage stage = MuzzleloaderClient.GetLoadingStage<MuzzleloaderLoadingStage>(slot);
			MuzzleloaderState weaponState2;
			switch (stage)
			{
			case MuzzleloaderLoadingStage.Unloaded:
				weaponState2 = MuzzleloaderState.Unloaded;
				break;
			case MuzzleloaderLoadingStage.Loading:
				weaponState2 = MuzzleloaderState.Loaded;
				break;
			case MuzzleloaderLoadingStage.Priming:
				weaponState2 = MuzzleloaderState.Primed;
				break;
			default:
				weaponState2 = MuzzleloaderState.Unloaded;
				break;
			}
			MuzzleloaderState weaponState = weaponState2;
			state = (int)weaponState;
			if (stage != MuzzleloaderLoadingStage.Loading)
			{
				if (stage == MuzzleloaderLoadingStage.Priming)
				{
					FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
					if (animationBehavior != null)
					{
						animationBehavior.Play(mainHand, this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
					if (tpAnimationBehavior != null)
					{
						tpAnimationBehavior.Play(mainHand, this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
					}
				}
			}
			else
			{
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.Play(mainHand, this.GetLoadingAnimation(slot, this.Stats.LoadedAnimation), this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
				if (tpAnimationBehavior2 != null)
				{
					tpAnimationBehavior2.Play(mainHand, this.GetLoadingAnimation(slot, this.Stats.LoadedAnimation), this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
				}
			}
			string takeOutAnimation = mainHand ? this.Stats.TakeOutAnimation : this.Stats.TakeOutAnimationOffhand;
			if (takeOutAnimation != "")
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.Play(mainHand, takeOutAnimation, this.AnimationCategory(mainHand), base.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f), 1f, delegate()
					{
						FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
						if (animationBehavior4 != null)
						{
							animationBehavior4.PlayReadyAnimation(mainHand);
						}
						ThirdPersonAnimationsBehavior tpAnimationBehavior4 = this.TpAnimationBehavior;
						if (tpAnimationBehavior4 != null)
						{
							tpAnimationBehavior4.PlayReadyAnimation(mainHand);
						}
						this.SetState<MuzzleloaderState>(weaponState, mainHand);
						return true;
					}, null, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior3 = this.TpAnimationBehavior;
				if (tpAnimationBehavior3 == null)
				{
					return;
				}
				tpAnimationBehavior3.Play(mainHand, takeOutAnimation, this.AnimationCategory(mainHand), base.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f), 1f, true);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006834 File Offset: 0x00004A34
		public override void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
			switch (state)
			{
			case 1:
			case 3:
			case 5:
				this.ReloadActionId++;
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
				break;
			case 7:
			case 8:
				this.RangedWeaponSystem.SendStatusChange(player, 3, mainHand);
				break;
			}
			this.Attachable.ClearAttachments(player.EntityId);
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			this.AimingSystem.AimingState = 0;
			this.AimingSystem.StopAiming();
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopAllVanillaAnimations(mainHand);
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior == null)
			{
				return;
			}
			playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006910 File Offset: 0x00004B10
		public override void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			base.OnRegistered(behavior, api);
			this.AnimationBehavior = behavior.entity.GetBehavior<FirstPersonAnimationsBehavior>();
			this.TpAnimationBehavior = behavior.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
			this.AimingAnimationController = new AimingAnimationController(this.AimingSystem, this.AnimationBehavior, this.AimingStats);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006964 File Offset: 0x00004B64
		[ActionEventHandler(10, 2)]
		protected virtual bool Load(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			MuzzleloaderClient.<>c__DisplayClass25_0 CS$<>8__locals1 = new MuzzleloaderClient.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.mainHand = mainHand;
			CS$<>8__locals1.slot = slot2;
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[1]) || !CS$<>8__locals1.mainHand)
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
			if (this.TryRestoreReadyStateFromStoredStage(CS$<>8__locals1.slot, CS$<>8__locals1.mainHand))
			{
				return true;
			}
			if (this.Stats.LoadingRequirementWildcard != "" && !MuzzleloaderClient.CheckRequirement(this.Stats.LoadingRequirementWildcard, CS$<>8__locals1.player))
			{
				this.Api.TriggerIngameError(this, "missingRequirement", Lang.Get(this.Stats.LoadingRequirementMessage, Array.Empty<object>()));
				return false;
			}
			if (!this.CheckWadding(CS$<>8__locals1.player) || !this.CheckFlask(CS$<>8__locals1.player, this.Stats.LoadPowderConsumption))
			{
				return false;
			}
			CS$<>8__locals1.ammoSlot = null;
			CS$<>8__locals1.player.WalkInventory(delegate(ItemSlot slot)
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
				if (((itemstack2 != null) ? itemstack2.Item : null) != null && slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(CS$<>8__locals1.<>4__this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()) && FirearmsAmmoUtility.HasEnoughBulletItems(slot, CS$<>8__locals1.<>4__this.Stats.BulletLoadedPerReload, CS$<>8__locals1.<>4__this.Stats.BulletsLoadedPerBulletItem))
				{
					CS$<>8__locals1.ammoSlot = slot;
					return false;
				}
				return true;
			});
			if (CS$<>8__locals1.ammoSlot == null)
			{
				this.Api.TriggerIngameError(this, "missingAmmo", Lang.Get("maltiezfirearms:requirement-missing-ammo", Array.Empty<object>()));
				return false;
			}
			CS$<>8__locals1.lastAmmoToLoad = (this.SpaceLeftInMagazine(CS$<>8__locals1.slot) <= this.Stats.BulletLoadedPerReload);
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(CS$<>8__locals1.slot.Itemstack);
			float animationSpeed = FirearmsReloadSafety.AnimationSpeed(base.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats.ProficiencyStat, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed);
			MuzzleloaderClient.<>c__DisplayClass25_0 CS$<>8__locals2 = CS$<>8__locals1;
			int reloadActionId = this.ReloadActionId + 1;
			this.ReloadActionId = reloadActionId;
			CS$<>8__locals2.reloadActionId = reloadActionId;
			this.RangedWeaponSystem.SendStatusChange(CS$<>8__locals1.player, 0, CS$<>8__locals1.mainHand);
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Loading, CS$<>8__locals1.mainHand);
			string loadAnimation = this.GetLoadingAnimation(CS$<>8__locals1.slot, this.Stats.LoadAnimation);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Stop(this.ItemAnimationCategory(CS$<>8__locals1.mainHand));
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Stop(this.ItemAnimationCategory(CS$<>8__locals1.mainHand));
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.Play(CS$<>8__locals1.mainHand, loadAnimation, this.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, null, delegate(string callback)
				{
					CS$<>8__locals1.<>4__this.LoadAnimationCallback(callback, CS$<>8__locals1.ammoSlot, CS$<>8__locals1.player, CS$<>8__locals1.mainHand, CS$<>8__locals1.reloadActionId);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.Play(CS$<>8__locals1.mainHand, loadAnimation, this.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, true);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayVanillaAnimation(this.Stats.LoadTpAnimation, CS$<>8__locals1.mainHand);
				}
			}
			this.Attachable.SetAttachment(CS$<>8__locals1.player.EntityId, "bullet", CS$<>8__locals1.ammoSlot.Itemstack, this.BulletTransform, true, false);
			FirearmsReloadSafety.SetReloadWalkspeedPenalty(this.PlayerBehavior, CS$<>8__locals1.mainHand, "CombatOverhaul:held-item-mainhand", "CombatOverhaul:held-item-offhand", this.Stats.LoadSpeedPenalty);
			this.Api.World.RegisterCallback(delegate(float _)
			{
				CS$<>8__locals1.<>4__this.LoadCallback(CS$<>8__locals1.slot, CS$<>8__locals1.player, CS$<>8__locals1.mainHand, CS$<>8__locals1.lastAmmoToLoad, CS$<>8__locals1.reloadActionId);
			}, base.GetActionDelayMs(CS$<>8__locals1.player, loadAnimation, animationSpeed, 2500, null));
			return true;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006CD4 File Offset: 0x00004ED4
		protected virtual void LoadAnimationCallback(string callback, ItemSlot bulletSlot, EntityPlayer player, bool mainHand, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Loading
			}))
			{
				return;
			}
			if (!(callback == "attach"))
			{
				if (!(callback == "detach"))
				{
					return;
				}
				this.Attachable.ClearAttachments(player.EntityId);
			}
			else
			{
				this.Attachable.SetAttachment(player.EntityId, "bullet", bulletSlot.Itemstack, this.BulletTransform, true, false);
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
				}
				ItemSlot loadingSlot = null;
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
					if (WildcardUtil.Match(this.Stats.LoadingRequirementWildcard, slot.Itemstack.Item.Code.ToString()))
					{
						loadingSlot = slot;
						return false;
					}
					return true;
				});
				if (loadingSlot != null)
				{
					this.Attachable.SetAttachment(player.EntityId, "loading", loadingSlot.Itemstack, this.LoadingEquipmentTransform, true, false);
					return;
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006DFC File Offset: 0x00004FFC
		protected virtual bool LoadCallback(ItemSlot slot, EntityPlayer player, bool mainHand, bool lastAmmoToLoad, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Loading
			}))
			{
				return true;
			}
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
				if (((itemstack2 != null) ? itemstack2.Item : null) != null && slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()) && FirearmsAmmoUtility.HasEnoughBulletItems(slot, this.Stats.BulletLoadedPerReload, this.Stats.BulletsLoadedPerBulletItem))
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
			int expectedMagazineCount = this.LeftInMagazine(slot2);
			this.PutIntoMagazine(slot2, ammoSlot);
			this.RangedWeaponSystem.Reload(slot2, ammoSlot, FirearmsAmmoUtility.BulletItemsRequired(this.Stats.BulletLoadedPerReload, this.Stats.BulletsLoadedPerBulletItem), mainHand, delegate(bool success)
			{
				this.LoadServerCallback(success, lastAmmoToLoad, mainHand, reloadActionId, slot2, expectedMagazineCount);
			}, MuzzleloaderClient.SerializeLoadingStageAndMagazineCount<MuzzleloaderLoadingStage>(MuzzleloaderLoadingStage.Loading, expectedMagazineCount));
			this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
			this.Attachable.ClearAttachments(player.EntityId);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, this.GetLoadingAnimation(slot2, this.Stats.LoadedAnimation), this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.GetLoadingAnimation(slot2, this.Stats.LoadedAnimation), this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.PlayReadyAnimation(mainHand);
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007002 File Offset: 0x00005202
		protected virtual void LoadServerCallback(bool success, bool lastAmmoToLoad, bool mainHand, int reloadActionId, ItemSlot slot, int expectedMagazineCount)
		{
			if (reloadActionId != this.ReloadActionId)
			{
				return;
			}
			if (success)
			{
				base.SetState<MuzzleloaderState>(lastAmmoToLoad ? MuzzleloaderState.Loaded : MuzzleloaderState.Unloaded, mainHand);
				return;
			}
			this.RestoreMagazineCount(slot, expectedMagazineCount);
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Unloaded, mainHand);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007034 File Offset: 0x00005234
		[ActionEventHandler(10, 2)]
		protected virtual bool Prime(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			MuzzleloaderClient.<>c__DisplayClass29_0 CS$<>8__locals1 = new MuzzleloaderClient.<>c__DisplayClass29_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.mainHand = mainHand;
			CS$<>8__locals1.slot = slot;
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[]
			{
				MuzzleloaderState.Loaded
			}) || !CS$<>8__locals1.mainHand)
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
			if (this.Stats.PrimingRequirementWildcard != "" && !MuzzleloaderClient.CheckRequirement(this.Stats.PrimingRequirementWildcard, CS$<>8__locals1.player))
			{
				this.Api.TriggerIngameError(this, "missingRequirement", Lang.Get(this.Stats.PrimingRequirementMessage, Array.Empty<object>()));
				return false;
			}
			if (!this.CheckFlask(CS$<>8__locals1.player, this.Stats.PrimePowderConsumption))
			{
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(CS$<>8__locals1.slot.Itemstack);
			float animationSpeed = FirearmsReloadSafety.AnimationSpeed(base.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats.ProficiencyStat, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed);
			MuzzleloaderClient.<>c__DisplayClass29_0 CS$<>8__locals2 = CS$<>8__locals1;
			int reloadActionId = this.ReloadActionId + 1;
			this.ReloadActionId = reloadActionId;
			CS$<>8__locals2.reloadActionId = reloadActionId;
			this.RangedWeaponSystem.SendStatusChange(CS$<>8__locals1.player, 0, CS$<>8__locals1.mainHand);
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Priming, CS$<>8__locals1.mainHand);
			string primeAnimation = this.Stats.PrimeAnimation;
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Stop(this.ItemAnimationCategory(CS$<>8__locals1.mainHand));
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.Play(CS$<>8__locals1.mainHand, primeAnimation, this.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, null, delegate(string callback)
				{
					CS$<>8__locals1.<>4__this.PrimeAnimationCallback(callback, CS$<>8__locals1.player, CS$<>8__locals1.mainHand, CS$<>8__locals1.reloadActionId);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Stop(this.ItemAnimationCategory(CS$<>8__locals1.mainHand));
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.Play(CS$<>8__locals1.mainHand, primeAnimation, this.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, true);
			}
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				animationBehavior3.StopAllVanillaAnimations(CS$<>8__locals1.mainHand);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior4 = this.AnimationBehavior;
				if (animationBehavior4 != null)
				{
					animationBehavior4.PlayVanillaAnimation(this.Stats.PrimeTpAnimation, CS$<>8__locals1.mainHand);
				}
			}
			FirearmsReloadSafety.SetReloadWalkspeedPenalty(this.PlayerBehavior, CS$<>8__locals1.mainHand, "CombatOverhaul:held-item-mainhand", "CombatOverhaul:held-item-offhand", this.Stats.PrimeSpeedPenalty);
			this.Api.World.RegisterCallback(delegate(float _)
			{
				CS$<>8__locals1.<>4__this.PrimeCallback(CS$<>8__locals1.mainHand, CS$<>8__locals1.player, CS$<>8__locals1.slot, CS$<>8__locals1.reloadActionId);
			}, base.GetActionDelayMs(CS$<>8__locals1.player, primeAnimation, animationSpeed, 1200, null));
			return true;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000072F8 File Offset: 0x000054F8
		protected virtual void PrimeAnimationCallback(string callback, EntityPlayer player, bool mainHand, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Priming
			}))
			{
				return;
			}
			if (!(callback == "attach"))
			{
				if (!(callback == "detach"))
				{
					return;
				}
				this.Attachable.ClearAttachments(player.EntityId);
			}
			else
			{
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
				}
				ItemSlot primingSlot = null;
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
					if (WildcardUtil.Match(this.Stats.PrimingRequirementWildcard, slot.Itemstack.Item.Code.ToString()))
					{
						primingSlot = slot;
						return false;
					}
					return true;
				});
				if (primingSlot != null)
				{
					this.Attachable.SetAttachment(player.EntityId, "priming", primingSlot.Itemstack, this.PrimingEquipmentTransform, true, false);
					return;
				}
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000073F8 File Offset: 0x000055F8
		protected virtual bool PrimeCallback(bool mainHand, EntityPlayer player, ItemSlot slot, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId || !base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Priming
			}))
			{
				return true;
			}
			this.RangedWeaponSystem.Load(slot, mainHand, delegate(bool success)
			{
				this.PrimeServerCallback(success, mainHand, reloadActionId);
			}, 0, MuzzleloaderClient.SerializeLoadingStage<MuzzleloaderLoadingStage>(MuzzleloaderLoadingStage.Priming));
			this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
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
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.Play(mainHand, this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.Play(mainHand, this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			return true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007553 File Offset: 0x00005753
		protected virtual void PrimeServerCallback(bool success, bool mainHand, int reloadActionId)
		{
			if (reloadActionId != this.ReloadActionId)
			{
				return;
			}
			if (base.CheckState<MuzzleloaderState>(true, new MuzzleloaderState[]
			{
				MuzzleloaderState.Priming
			}) && success)
			{
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Primed, mainHand);
				return;
			}
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Loaded, mainHand);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007588 File Offset: 0x00005788
		[ActionEventHandler(10, 2)]
		protected virtual bool Cocking(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[]
			{
				MuzzleloaderState.Primed
			}))
			{
				return false;
			}
			if (!mainHand && !this.CanUseOffhand(player))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (this.Stats.CockingRequirementWildcard != "" && !MuzzleloaderClient.CheckRequirement(this.Stats.CockingRequirementWildcard, player))
			{
				this.Api.TriggerIngameError(this, "missingRequirement", Lang.Get(this.Stats.CockingRequirementMessage, Array.Empty<object>()));
				return false;
			}
			if (this.Stats.CockingAnimation.Length == 0)
			{
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Cocked, mainHand);
				if (this.Stats.CockedAnimation.Length != 0)
				{
					string cockedAnimation = this.GetShootingAnimation(slot, this.Stats.CockedAnimation);
					FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
					if (animationBehavior != null)
					{
						animationBehavior.Play(mainHand, (this.Stats.CockedAnimation.Length != 0) ? cockedAnimation : this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
					if (tpAnimationBehavior != null)
					{
						tpAnimationBehavior.Play(mainHand, (this.Stats.CockedAnimation.Length != 0) ? cockedAnimation : this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
					}
				}
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			float animationSpeed = FirearmsReloadSafety.AnimationSpeed(base.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f) * stackStats.ReloadSpeed * this.Stats.ReloadAnimationSpeed);
			string cockingAnimation = this.GetShootingAnimation(slot, mainHand ? this.Stats.CockingAnimation : this.Stats.CockingAnimationOffhand);
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.Play(mainHand, cockingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.Play(mainHand, cockingAnimation, this.AnimationCategory(mainHand), animationSpeed, 1f, true);
			}
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Cocking, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, 0, mainHand);
			this.Api.World.RegisterCallback(delegate(float _)
			{
				this.CockingCallback(mainHand, player);
			}, base.GetActionDelayMs(player, cockingAnimation, animationSpeed, 600, null));
			return true;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007846 File Offset: 0x00005A46
		protected virtual bool CockingCallback(bool mainHand, EntityPlayer player)
		{
			if (base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Cocking
			}))
			{
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Cocked, mainHand);
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
			}
			return true;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007874 File Offset: 0x00005A74
		[ActionEventHandler(10, 2)]
		protected virtual bool Aim(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[]
			{
				MuzzleloaderState.Cocked
			}))
			{
				return false;
			}
			if (!mainHand && !this.CanUseOffhand(player))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			AimingStats aimingStats = this.AimingStats.Clone();
			aimingStats.AimDifficulty *= stackStats.AimingDifficulty;
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Aim, mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, mainHand ? this.Stats.AimAnimation : this.Stats.AimAnimationOffhand, this.AnimationCategory(mainHand), 1f, 1f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, mainHand ? this.Stats.AimAnimation : this.Stats.AimAnimationOffhand, this.AnimationCategory(mainHand), 1f, 1f, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopAllVanillaAnimations(mainHand);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayVanillaAnimation(this.Stats.AimTpAnimation, mainHand);
				}
			}
			this.AimingSystem.StartAiming(aimingStats);
			this.AimingSystem.AimingState = 3;
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Play(mainHand);
			}
			this.RangedWeaponSystem.SendStatusChange(player, 2, mainHand);
			return true;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000079E4 File Offset: 0x00005BE4
		[ActionEventHandler(10, 4)]
		protected virtual bool Ease(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			switch (state)
			{
			case 1:
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Unloaded, mainHand);
				this.Attachable.ClearAttachments(player.EntityId);
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
				break;
			case 3:
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Loaded, mainHand);
				this.Attachable.ClearAttachments(player.EntityId);
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
				break;
			case 5:
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Primed, mainHand);
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
				break;
			case 7:
				this.Inventory.Read(slot, "magazine");
				if (this.Inventory.Items.Count == 0)
				{
					base.SetState<MuzzleloaderState>(MuzzleloaderState.Unloaded, mainHand);
				}
				else
				{
					base.SetState<MuzzleloaderState>(MuzzleloaderState.Cocked, mainHand);
					if (this.Stats.CockedAnimation.Length != 0)
					{
						string cockedAnimation = this.GetShootingAnimation(slot, this.Stats.CockedAnimation);
						FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
						if (animationBehavior != null)
						{
							animationBehavior.Play(mainHand, (this.Stats.CockedAnimation.Length != 0) ? cockedAnimation : this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
						}
						ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
						if (tpAnimationBehavior != null)
						{
							tpAnimationBehavior.Play(mainHand, (this.Stats.CockedAnimation.Length != 0) ? cockedAnimation : this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
						}
					}
				}
				this.Inventory.Clear();
				this.RangedWeaponSystem.SendStatusChange(player, 3, mainHand);
				break;
			case 8:
				return true;
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.PlayReadyAnimation(mainHand);
			}
			this.AimingSystem.StopAiming();
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
			if (animationBehavior3 != null)
			{
				animationBehavior3.StopAllVanillaAnimations(mainHand);
			}
			return false;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007C21 File Offset: 0x00005E21
		[ActionEventHandler(9, 1)]
		protected virtual bool Shoot(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			return this.TryShoot(slot, player, ref state, eventData, mainHand);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007C30 File Offset: 0x00005E30
		[ActionEventHandler(9, 2)]
		protected virtual bool ShootAutomatic(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			return this.Stats.AutomaticFire && this.TryShoot(slot, player, ref state, eventData, mainHand);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007C50 File Offset: 0x00005E50
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

		// Token: 0x0600016F RID: 367 RVA: 0x00007CA0 File Offset: 0x00005EA0
		protected virtual bool TryShoot(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand)
		{
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[]
			{
				MuzzleloaderState.Aim
			}))
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
			this.Inventory.Read(slot, "magazine");
			if (this.Inventory.Items.Count == 0)
			{
				this.Inventory.Clear();
				return false;
			}
			this.Inventory.Clear();
			this.RangedWeaponSystem.SendStatusChange(player, 4, mainHand);
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Shoot, mainHand);
			string shootAnimation = this.GetShootingAnimation(slot, mainHand ? this.Stats.ShootAnimation : this.Stats.ShootAnimationOffhand);
			int shootDurationMs = base.GetActionDelayMs(player, shootAnimation, 1f, 500, null);
			this.SetNextShotAllowed(mainHand, this.Api.World.ElapsedMilliseconds + (long)shootDurationMs);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Stop(this.ItemAnimationCategory(mainHand));
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.Play(mainHand, shootAnimation, this.AnimationCategory(mainHand), 1f, 1f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Stop(this.ItemAnimationCategory(mainHand));
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.Play(mainHand, shootAnimation, this.AnimationCategory(mainHand), 1f, 1f, true);
			}
			this.Api.World.RegisterCallback(delegate(float _)
			{
				this.ShootAnimationCallback("shoot", slot, player, mainHand);
			}, base.GetActionDelayMs(player, shootAnimation, 1f, 100, "shoot"));
			this.Api.World.RegisterCallback(delegate(float _)
			{
				this.ShootCallback(slot, player, mainHand);
			}, shootDurationMs);
			return true;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007EC8 File Offset: 0x000060C8
		protected virtual bool TryShootAutomaticContinuation(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (!base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Aim
			}))
			{
				return false;
			}
			if (!this.ShotGateOpen(mainHand))
			{
				return false;
			}
			this.Inventory.Read(slot, "magazine");
			if (this.Inventory.Items.Count == 0)
			{
				this.Inventory.Clear();
				return false;
			}
			this.Inventory.Clear();
			this.RangedWeaponSystem.SendStatusChange(player, 4, mainHand);
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Shoot, mainHand);
			string shootAnimation = this.GetShootingAnimation(slot, mainHand ? this.Stats.ShootAnimation : this.Stats.ShootAnimationOffhand);
			int shootDurationMs = base.GetActionDelayMs(player, shootAnimation, 1f, 500, null);
			this.SetNextShotAllowed(mainHand, this.Api.World.ElapsedMilliseconds + (long)shootDurationMs);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Stop(this.ItemAnimationCategory(mainHand));
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.Play(mainHand, shootAnimation, this.AnimationCategory(mainHand), 1f, 1f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Stop(this.ItemAnimationCategory(mainHand));
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
			if (tpAnimationBehavior2 != null)
			{
				tpAnimationBehavior2.Play(mainHand, shootAnimation, this.AnimationCategory(mainHand), 1f, 1f, true);
			}
			this.Api.World.RegisterCallback(delegate(float _)
			{
				this.ShootAnimationCallback("shoot", slot, player, mainHand);
			}, base.GetActionDelayMs(player, shootAnimation, 1f, 100, "shoot"));
			this.Api.World.RegisterCallback(delegate(float _)
			{
				this.ShootCallback(slot, player, mainHand);
			}, shootDurationMs);
			return true;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000080DC File Offset: 0x000062DC
		protected virtual bool ShootCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (!base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Shoot
			}))
			{
				return true;
			}
			this.Inventory.Read(slot, "magazine");
			bool ammoLeft = this.Inventory.Items.Count > 0;
			this.Inventory.Clear();
			bool flag;
			if (ammoLeft)
			{
				ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
				flag = (playerBehavior != null && playerBehavior.ActionListener.IsActive(10));
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Aim, mainHand);
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.Play(mainHand, mainHand ? this.Stats.AimAnimation : this.Stats.AimAnimationOffhand, this.AnimationCategory(mainHand), 1f, 1f, null, null, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					tpAnimationBehavior.Play(mainHand, mainHand ? this.Stats.AimAnimation : this.Stats.AimAnimationOffhand, this.AnimationCategory(mainHand), 1f, 1f, true);
				}
				AimingAnimationController aimingAnimationController = this.AimingAnimationController;
				if (aimingAnimationController != null)
				{
					aimingAnimationController.Play(mainHand);
				}
				if (this.ShouldContinueAutomaticFire(mainHand))
				{
					this.TryShootAutomaticContinuation(slot, player, mainHand);
				}
				return true;
			}
			base.SetState<MuzzleloaderState>(ammoLeft ? MuzzleloaderState.Cocked : MuzzleloaderState.Unloaded, mainHand);
			if (ammoLeft && this.Stats.CockedAnimation.Length != 0)
			{
				string cockedAnimation = this.GetShootingAnimation(slot, this.Stats.CockedAnimation);
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.Play(mainHand, cockedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
				if (tpAnimationBehavior2 != null)
				{
					tpAnimationBehavior2.Play(mainHand, cockedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
				}
			}
			else
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayReadyAnimation(mainHand);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior3 = this.TpAnimationBehavior;
				if (tpAnimationBehavior3 != null)
				{
					tpAnimationBehavior3.PlayReadyAnimation(mainHand);
				}
			}
			this.AimingSystem.StopAiming();
			AimingAnimationController aimingAnimationController2 = this.AimingAnimationController;
			if (aimingAnimationController2 != null)
			{
				aimingAnimationController2.Stop(mainHand);
			}
			this.RangedWeaponSystem.SendStatusChange(player, 3, mainHand);
			return true;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000082D4 File Offset: 0x000064D4
		protected virtual void ShootAnimationCallback(string callback, ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (!base.CheckState<MuzzleloaderState>(mainHand, new MuzzleloaderState[]
			{
				MuzzleloaderState.Shoot
			}))
			{
				return;
			}
			if (callback == "shoot")
			{
				Vec3d position = player.LocalEyePos + player.Pos.XYZ;
				Vector3 targetDirection = this.AimingSystem.TargetVec;
				targetDirection = ClientAimingSystem.Zeroing(targetDirection, this.Stats.Zeroing);
				this.RangedWeaponSystem.Shoot(slot, this.Stats.BulletsFiredPerShot, new Vector3d((double)((float)position.X), (double)((float)position.Y), (double)((float)position.Z)), new Vector3d((double)targetDirection.X, (double)targetDirection.Y, (double)targetDirection.Z), mainHand, new Action<bool>(this.ShootServerCallback), null);
				this.RangedWeaponSystem.SendStatusChange(player, 5, mainHand);
				this.ApplyRecoil(slot, player);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000083B0 File Offset: 0x000065B0
		protected virtual void ApplyRecoil(ItemSlot slot, EntityPlayer player)
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
			RecoilStats recoil = this.Stats.Recoil ?? RecoilStats.MuzzleloaderDefaults();
			if (!recoil.Enabled)
			{
				return;
			}
			float estimatedDamage = this.EstimateShotDamage(slot);
			float verticalDeg = recoil.VerticalDegrees(estimatedDamage);
			float horizontalDeg = recoil.HorizontalDegrees(estimatedDamage, verticalDeg);
			FirearmsRecoilSystem.AddRecoil(verticalDeg, horizontalDeg, recoil);
			this.Api.World.AddCameraShake(recoil.CameraShake(estimatedDamage));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000084A4 File Offset: 0x000066A4
		protected virtual float EstimateShotDamage(ItemSlot slot)
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
			float weaponMultiplier = Math.Max(0.1f, this.Stats.BulletDamageMultiplier * stackStats.DamageMultiplier);
			return Math.Max(0.1f, projectileBaseDamage * weaponMultiplier);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000856D File Offset: 0x0000676D
		protected virtual void ShootServerCallback(bool success)
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000856F File Offset: 0x0000676F
		protected bool ShotGateOpen(bool mainHand)
		{
			return this.Api.World.ElapsedMilliseconds >= this.GetNextShotAllowed(mainHand);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000858D File Offset: 0x0000678D
		protected long GetNextShotAllowed(bool mainHand)
		{
			if (!mainHand)
			{
				return this.NextShotAllowedOffHandMs;
			}
			return this.NextShotAllowedMainHandMs;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000859F File Offset: 0x0000679F
		protected void SetNextShotAllowed(bool mainHand, long value)
		{
			if (mainHand)
			{
				this.NextShotAllowedMainHandMs = value;
				return;
			}
			this.NextShotAllowedOffHandMs = value;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000085B4 File Offset: 0x000067B4
		[ActionEventHandler(10, 2)]
		protected virtual bool Cancel(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			IMountableSeat mountedOn = player.MountedOn;
			if (((mountedOn != null) ? mountedOn.Entity : null) != null || player.OnGround || !this.Stats.CancelReloadOnInAir)
			{
				return false;
			}
			if (state == 1)
			{
				this.ReloadActionId++;
				ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
				if (playerBehavior != null)
				{
					playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
				}
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Unloaded, mainHand);
				this.Attachable.ClearAttachments(player.EntityId);
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
				this.AimingSystem.StopAiming();
				AimingAnimationController aimingAnimationController = this.AimingAnimationController;
				if (aimingAnimationController != null)
				{
					aimingAnimationController.Stop(mainHand);
				}
				this.RangedWeaponSystem.SendStatusChange(player, 1, mainHand);
			}
			return false;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000869C File Offset: 0x0000689C
		protected void PutIntoMagazine(ItemSlot slot, ItemSlot ammoSlot)
		{
			this.Inventory.Read(slot, "magazine");
			FirearmsAmmoUtility.TryLoadBulletsWithoutConsuming(this.Inventory, ammoSlot, this.Stats.BulletLoadedPerReload, this.Stats.BulletsLoadedPerBulletItem);
			this.Inventory.Write(slot);
			this.Inventory.Clear();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000086F4 File Offset: 0x000068F4
		protected void RestoreMagazineCount(ItemSlot slot, int expectedMagazineCount)
		{
			if (expectedMagazineCount < 0)
			{
				return;
			}
			this.Inventory.Read(slot, "magazine");
			while (this.Inventory.Items.Count > expectedMagazineCount)
			{
				this.Inventory.Items.RemoveAt(this.Inventory.Items.Count - 1);
			}
			this.Inventory.Write(slot);
			this.Inventory.Clear();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008768 File Offset: 0x00006968
		protected bool TryRestoreReadyStateFromStoredStage(ItemSlot slot, bool mainHand)
		{
			MuzzleloaderLoadingStage stage = MuzzleloaderClient.GetLoadingStage<MuzzleloaderLoadingStage>(slot);
			int ammoCount = this.LeftInMagazine(slot);
			if (ammoCount <= 0)
			{
				return false;
			}
			MuzzleloaderLoadingStage muzzleloaderLoadingStage = stage;
			if (muzzleloaderLoadingStage != MuzzleloaderLoadingStage.Loading)
			{
				if (muzzleloaderLoadingStage == MuzzleloaderLoadingStage.Priming)
				{
					base.SetState<MuzzleloaderState>(MuzzleloaderState.Primed, mainHand);
					FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
					if (animationBehavior != null)
					{
						animationBehavior.Play(mainHand, this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
					if (tpAnimationBehavior != null)
					{
						tpAnimationBehavior.Play(mainHand, this.Stats.PrimedAnimation, this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
					}
					return true;
				}
			}
			else if (ammoCount >= this.Stats.MagazineSize)
			{
				base.SetState<MuzzleloaderState>(MuzzleloaderState.Loaded, mainHand);
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.Play(mainHand, this.GetLoadingAnimation(slot, this.Stats.LoadedAnimation), this.ItemAnimationCategory(mainHand), 1f, 0.001f, null, null, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
				if (tpAnimationBehavior2 != null)
				{
					tpAnimationBehavior2.Play(mainHand, this.GetLoadingAnimation(slot, this.Stats.LoadedAnimation), this.ItemAnimationCategory(mainHand), 1f, 0.001f, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008888 File Offset: 0x00006A88
		protected int LeftInMagazine(ItemSlot slot)
		{
			this.Inventory.Read(slot, "magazine");
			int count = this.Inventory.Items.Count;
			this.Inventory.Clear();
			return count;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000088B6 File Offset: 0x00006AB6
		protected int SpaceLeftInMagazine(ItemSlot slot)
		{
			this.Inventory.Read(slot, "magazine");
			int result = this.Stats.MagazineSize - this.Inventory.Items.Count;
			this.Inventory.Clear();
			return result;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000088F0 File Offset: 0x00006AF0
		protected string GetLoadingAnimation(ItemSlot slot, string[] animations)
		{
			if (this.Stats.MagazineSize == 1)
			{
				return animations[0];
			}
			this.Inventory.Read(slot, "magazine");
			int count = this.Inventory.Items.Count;
			int ammoPerLoad = this.Stats.BulletLoadedPerReload;
			int animationsCount = animations.Length;
			int index = count / ammoPerLoad % animationsCount;
			this.Inventory.Clear();
			return animations[index];
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008954 File Offset: 0x00006B54
		protected virtual string GetShootingAnimation(ItemSlot slot, string[] animations)
		{
			if (this.Stats.MagazineSize == 1)
			{
				return animations[0];
			}
			this.Inventory.Read(slot, "magazine");
			int count = this.Inventory.Items.Count;
			int ammoPerShot = this.Stats.BulletsFiredPerShot;
			int animationsCount = animations.Length;
			int index = count / ammoPerShot % animationsCount;
			this.Inventory.Clear();
			return animations[index];
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000089B8 File Offset: 0x00006BB8
		protected bool CheckWadding(EntityPlayer player)
		{
			if (this.Stats.WaddingUsedPerReload <= 0)
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
				if (WildcardUtil.Match(this.Stats.WaddingWildcard, slot.Itemstack.Item.Code.ToString()) && slot.Itemstack.StackSize >= this.Stats.WaddingUsedPerReload)
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

		// Token: 0x06000182 RID: 386 RVA: 0x00008A28 File Offset: 0x00006C28
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

		// Token: 0x06000183 RID: 387 RVA: 0x00008A99 File Offset: 0x00006C99
		[NullableContext(0)]
		[return: Nullable(1)]
		protected static byte[] SerializeLoadingStage<TStage>(TStage stage) where TStage : struct, Enum
		{
			return BitConverter.GetBytes((int)Enum.ToObject(typeof(TStage), stage));
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008ABC File Offset: 0x00006CBC
		[NullableContext(0)]
		[return: Nullable(1)]
		protected static byte[] SerializeLoadingStageAndMagazineCount<TStage>(TStage stage, int expectedMagazineCount) where TStage : struct, Enum
		{
			byte[] data = new byte[8];
			BitConverter.GetBytes((int)Enum.ToObject(typeof(TStage), stage)).CopyTo(data, 0);
			BitConverter.GetBytes(expectedMagazineCount).CopyTo(data, 4);
			return data;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008B04 File Offset: 0x00006D04
		[NullableContext(0)]
		protected static TStage GetLoadingStage<TStage>([Nullable(1)] ItemSlot slot) where TStage : struct, Enum
		{
			ItemStack itemstack = slot.Itemstack;
			int stage = (itemstack != null) ? itemstack.Attributes.GetAsInt("CombatOverhaul:loading-stage", 0) : 0;
			return (TStage)((object)Enum.ToObject(typeof(TStage), stage));
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008B44 File Offset: 0x00006D44
		protected static bool CheckRequirement(string requirementWildcard, EntityPlayer player)
		{
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
				if (WildcardUtil.Match(requirementWildcard, slot.Itemstack.Item.Code.ToString()))
				{
					flaskSlot = slot;
					return false;
				}
				return true;
			});
			return flaskSlot != null;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008B80 File Offset: 0x00006D80
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
			ItemSlot ammoSlot4 = null;
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
				if (WildcardUtil.Match(this.Stats.PrimingRequirementWildcard, slot.Itemstack.Item.Code.ToString()))
				{
					ammoSlot4 = slot;
					return false;
				}
				return true;
			});
			if (ammoSlot4 != null)
			{
				this.Attachable.SetAttachment(player.EntityId, "priming", ammoSlot4.Itemstack, this.PrimingEquipmentTransform, true, false);
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008C7B File Offset: 0x00006E7B
		protected string AnimationCategory(bool mainHand)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008C8B File Offset: 0x00006E8B
		protected string ItemAnimationCategory(bool mainHand)
		{
			if (!mainHand)
			{
				return "itemOffhand";
			}
			return "item";
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008C9C File Offset: 0x00006E9C
		protected bool CanUseOffhand(EntityPlayer player)
		{
			ItemSlot rightHandItemSlot = player.RightHandItemSlot;
			object obj;
			if (rightHandItemSlot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = rightHandItemSlot.Itemstack;
				obj = ((itemstack != null) ? itemstack.Item : null);
			}
			if (!(obj is IHasRangedWeaponLogic))
			{
				return true;
			}
			bool result;
			switch (base.GetState<MuzzleloaderState>(true))
			{
			case MuzzleloaderState.Unloaded:
				result = true;
				break;
			case MuzzleloaderState.Loading:
				result = true;
				break;
			case MuzzleloaderState.Loaded:
				result = true;
				break;
			case MuzzleloaderState.Priming:
				result = true;
				break;
			case MuzzleloaderState.Primed:
				result = true;
				break;
			case MuzzleloaderState.Cocking:
				result = true;
				break;
			case MuzzleloaderState.Cocked:
				result = false;
				break;
			case MuzzleloaderState.Aim:
				result = false;
				break;
			case MuzzleloaderState.Shoot:
				result = false;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x040000C2 RID: 194
		[Nullable(2)]
		protected AimingAnimationController AimingAnimationController;

		// Token: 0x040000C3 RID: 195
		protected readonly AnimatableAttachable Attachable;

		// Token: 0x040000C4 RID: 196
		protected readonly ClientAimingSystem AimingSystem;

		// Token: 0x040000C5 RID: 197
		protected readonly MuzzleloaderStats Stats;

		// Token: 0x040000C6 RID: 198
		protected readonly AimingStats AimingStats;

		// Token: 0x040000C7 RID: 199
		protected long LastRecoilTimestampMs = -1000L;

		// Token: 0x040000C8 RID: 200
		protected long NextShotAllowedMainHandMs;

		// Token: 0x040000C9 RID: 201
		protected long NextShotAllowedOffHandMs;

		// Token: 0x040000CA RID: 202
		protected readonly ItemInventoryBuffer Inventory = new ItemInventoryBuffer();

		// Token: 0x040000CB RID: 203
		protected readonly ModelTransform BulletTransform;

		// Token: 0x040000CC RID: 204
		protected readonly ModelTransform FlaskTransform;

		// Token: 0x040000CD RID: 205
		protected readonly ModelTransform LoadingEquipmentTransform;

		// Token: 0x040000CE RID: 206
		protected readonly ModelTransform PrimingEquipmentTransform;

		// Token: 0x040000CF RID: 207
		[Nullable(2)]
		protected FirstPersonAnimationsBehavior AnimationBehavior;

		// Token: 0x040000D0 RID: 208
		[Nullable(2)]
		protected ThirdPersonAnimationsBehavior TpAnimationBehavior;

		// Token: 0x040000D1 RID: 209
		protected const string InventoryId = "magazine";

		// Token: 0x040000D2 RID: 210
		protected const string LoadingStageAttribute = "CombatOverhaul:loading-stage";

		// Token: 0x040000D3 RID: 211
		[Nullable(2)]
		protected ItemSlot BulletSlot;

		// Token: 0x040000D4 RID: 212
		protected const string PlayerStatsMainHandCategory = "CombatOverhaul:held-item-mainhand";

		// Token: 0x040000D5 RID: 213
		protected const string PlayerStatsOffHandCategory = "CombatOverhaul:held-item-offhand";

		// Token: 0x040000D6 RID: 214
		protected int ReloadActionId;
	}
}
