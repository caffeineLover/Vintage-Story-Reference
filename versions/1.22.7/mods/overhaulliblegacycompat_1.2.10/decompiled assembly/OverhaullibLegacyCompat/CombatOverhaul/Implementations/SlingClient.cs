using System;
using System.IO;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Inputs;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.RangedSystems.Aiming;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	public class SlingClient : RangeWeaponClient
	{
		// Token: 0x06000777 RID: 1911 RVA: 0x0002C374 File Offset: 0x0002A574
		public SlingClient(ICoreClientAPI api, Item item, AmmoSelector ammoSelector) : base(api, item)
		{
			this.Api = api;
			AnimatableAttachable collectibleBehavior = item.GetCollectibleBehavior<AnimatableAttachable>(true);
			if (collectibleBehavior == null)
			{
				throw new Exception("Sling should have AnimatableAttachable behavior.");
			}
			this.Attachable = collectibleBehavior;
			this.BulletTransform = new ModelTransform(item.Attributes["BulletTransform"].AsObject<ModelTransformNoDefaults>(null), ModelTransform.BlockDefaultTp());
			ClientAimingSystem aimingSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).AimingSystem;
			if (aimingSystem == null)
			{
				throw new Exception();
			}
			this.AimingSystem = aimingSystem;
			this.Stats = item.Attributes.AsObject<SlingStats>(null);
			this.AimingStats = this.Stats.Aiming.ToStats();
			this.AmmoSelector = ammoSelector;
			this.TwoHanded = this.Stats.TwoHanded;
			this.Settings = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002C44A File Offset: 0x0002A64A
		public override void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
			this.Attachable.ClearAttachments(player.EntityId);
			this.AttachmentSystem.SendClearPacket(player.EntityId);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002C470 File Offset: 0x0002A670
		public override void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
			SlingState slingState = (SlingState)state;
			if (slingState != SlingState.Load)
			{
				if (slingState - SlingState.WindUp <= 2)
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
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002C4EB File Offset: 0x0002A6EB
		public override void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			base.OnRegistered(behavior, api);
			this.AimingAnimationController = new AimingAnimationController(this.AimingSystem, this.AnimationBehavior, this.AimingStats);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002C514 File Offset: 0x0002A714
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
			if (this.CanBlockWithOtherHand(player, mainHand))
			{
				return false;
			}
			ItemSlot bulletSlot = this.GetBulletSlot(player);
			if (bulletSlot == null)
			{
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			this.RangedWeaponSystem.Reload(slot, bulletSlot, 1, mainHand, new Action<bool>(this.ReloadCallback), null);
			FirstPersonAnimationsBehavior animationBehavior;
			if ((animationBehavior = this.AnimationBehavior) != null)
			{
				animationBehavior.Play(mainHand, this.Stats.LoadAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * stackStats.ReloadSpeed, 1000f, () => this.LoadAnimationCallback(player, mainHand), delegate(string code)
				{
					this.LoadAnimationCallback(code, bulletSlot.Itemstack, player);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.Stats.LoadAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * stackStats.ReloadSpeed, 1000f, true);
			}
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Play(mainHand);
			}
			state = 1;
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.StartLoading, mainHand);
			this.AfterLoad = true;
			return true;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
		protected virtual void ReloadCallback(bool success)
		{
			SlingState state = base.GetState<SlingState>(true);
			if (success)
			{
				if (state == SlingState.Load)
				{
					base.SetState<SlingState>(SlingState.PreLoaded, true);
					return;
				}
				if (state == SlingState.PreLoaded)
				{
					base.SetState<SlingState>(SlingState.Loaded, true);
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
				base.SetState<SlingState>(SlingState.Unloaded, true);
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002C728 File Offset: 0x0002A928
		protected virtual void LoadAnimationCallback(string code, ItemStack stack, EntityPlayer player)
		{
			if (code == "attach")
			{
				this.Attachable.SetAttachment(player.EntityId, "bullet", stack, this.BulletTransform, true, false);
				this.AttachmentSystem.SendAttachPacket(player.EntityId, "bullet", stack, this.BulletTransform);
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002C780 File Offset: 0x0002A980
		protected virtual bool LoadAnimationCallback(EntityPlayer player, bool mainHand)
		{
			SlingState state = base.GetState<SlingState>(mainHand);
			if (state != SlingState.Load)
			{
				if (state == SlingState.PreLoaded)
				{
					this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
					base.SetState<SlingState>(SlingState.Loaded, mainHand);
				}
			}
			else
			{
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
				base.SetState<SlingState>(SlingState.PreLoaded, mainHand);
			}
			return true;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002C7CC File Offset: 0x0002A9CC
		[ActionEventHandler(10, ActionState.Active)]
		protected virtual bool Swing(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
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
			AnimationRequestByCode request = new AnimationRequestByCode(this.AfterLoad ? this.Stats.WindUpAfterLoadAnimation : this.Stats.WindUpAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, () => this.WindUpCallback(slot, player, mainHand), null);
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
			this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(this.Settings.SlingsAimingCursorType, true);
			this.AimingStats.VerticalLimit = this.Settings.SlingsAimingVerticalLimit * this.Stats.Aiming.VerticalLimit;
			this.AimingStats.HorizontalLimit = this.Settings.SlingsAimingHorizontalLimit * this.Stats.Aiming.HorizontalLimit;
			this.AimingSystem.ResetAim();
			this.AimingSystem.StartAiming(this.AimingStats);
			this.AimingSystem.AimingState = WeaponAimingState.Blocked;
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.StartAiming, mainHand);
			this.AfterLoad = false;
			state = 4;
			if (!this.AimingSystem.Aiming)
			{
				AimingStats aimingStats = this.AimingStats.Clone();
				aimingStats.AimDifficulty *= stackStats.AimingDifficulty;
				this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(this.Settings.SlingsAimingCursorType, true);
				this.AimingStats.VerticalLimit = this.Settings.SlingsAimingVerticalLimit * this.Stats.Aiming.VerticalLimit;
				this.AimingStats.HorizontalLimit = this.Settings.SlingsAimingHorizontalLimit * this.Stats.Aiming.HorizontalLimit;
				this.AimingSystem.StartAiming(aimingStats);
				this.AimingSystem.AimingState = WeaponAimingState.Blocked;
				AimingAnimationController aimingAnimationController = this.AimingAnimationController;
				if (aimingAnimationController != null)
				{
					aimingAnimationController.Play(mainHand);
				}
			}
			return true;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002CA4C File Offset: 0x0002AC4C
		protected virtual bool WindUpCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (base.CheckState<SlingState>(true, new SlingState[]
			{
				SlingState.WindUp
			}))
			{
				base.SetState<SlingState>(SlingState.Swinging, true);
				this.AimingSystem.AimingState = WeaponAimingState.PartCharge;
				this.CurrentSwingSpeed = this.Stats.MinSwingSpeed;
				AnimationRequestByCode request = new AnimationRequestByCode(this.Stats.SwingAnimation, this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * this.CurrentSwingSpeed * this.Stats.SwingAnimationSpeed, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, () => this.WindUpCallback(slot, player, mainHand), null);
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.Play(request, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					tpAnimationBehavior.Play(request, true);
				}
				this.ReleaseWhenReady = false;
				return true;
			}
			if (this.ReleaseWhenReady && base.CheckState<SlingState>(true, new SlingState[]
			{
				SlingState.Swinging
			}))
			{
				this.ReleaseWhenReady = false;
				this.Released = false;
				base.SetState<SlingState>(SlingState.Releasing, true);
				AnimationRequestByCode request2 = new AnimationRequestByCode(this.Stats.ReleaseAnimation, this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * this.CurrentSwingSpeed * this.Stats.SwingAnimationSpeed, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, () => this.ShootCallback(slot, player, mainHand), delegate(string code)
				{
					this.ShootCallback(code, slot, player, mainHand);
				});
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.Play(request2, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
				if (tpAnimationBehavior2 != null)
				{
					tpAnimationBehavior2.Play(request2, true);
				}
			}
			if (base.CheckState<SlingState>(true, new SlingState[]
			{
				SlingState.Swinging
			}))
			{
				this.CurrentSwingSpeed = GameMath.Clamp(this.CurrentSwingSpeed + this.Stats.SwingSpeedPerSwing, this.Stats.MinSwingSpeed, this.Stats.MaxSwingSpeed);
				AnimationRequestByCode request3 = new AnimationRequestByCode(this.Stats.SwingAnimation, this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * this.CurrentSwingSpeed * this.Stats.SwingAnimationSpeed, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, () => this.WindUpCallback(slot, player, mainHand), null);
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.Play(request3, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior3 = this.TpAnimationBehavior;
				if (tpAnimationBehavior3 != null)
				{
					tpAnimationBehavior3.Play(request3, true);
				}
				if (this.CurrentSwingSpeed >= this.Stats.MaxSwingSpeed)
				{
					this.AimingSystem.AimingState = WeaponAimingState.FullCharge;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002CD40 File Offset: 0x0002AF40
		[ActionEventHandler(10, ActionState.Released)]
		protected virtual bool Release(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (RangeWeaponClient.CheckState<SlingState>(state, new SlingState[]
			{
				SlingState.Load,
				SlingState.PreLoaded
			}))
			{
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
				this.Attachable.ClearAttachments(player.EntityId);
				this.AttachmentSystem.SendClearPacket(player.EntityId);
				state = 0;
				this.ReleaseWhenReady = false;
				this.AimingSystem.StopAiming();
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndLoading, mainHand);
				return true;
			}
			if (RangeWeaponClient.CheckState<SlingState>(state, new SlingState[]
			{
				SlingState.Loaded,
				SlingState.WindUp
			}))
			{
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
				state = 3;
				this.AfterLoad = false;
				this.ReleaseWhenReady = false;
				this.AimingSystem.StopAiming();
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
				return true;
			}
			if (state == 5)
			{
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.TriggeredShot, mainHand);
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
				this.ReleaseWhenReady = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002CE6C File Offset: 0x0002B06C
		protected virtual void ShootCallback(string code, ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (code == "detach")
			{
				this.Attachable.ClearAttachments(player.EntityId);
				this.AttachmentSystem.SendClearPacket(player.EntityId);
				return;
			}
			if (!(code == "release"))
			{
				return;
			}
			if (this.Released)
			{
				return;
			}
			this.Released = true;
			Vec3d position = player.LocalEyePos + player.Pos.XYZ;
			Vector3 targetDirection = this.AimingSystem.TargetVec;
			targetDirection = ClientAimingSystem.Zeroing(targetDirection, this.Stats.Zeroing);
			this.RangedWeaponSystem.Shoot(slot, 1, new Vector3d((double)((float)position.X), (double)((float)position.Y), (double)((float)position.Z)), new Vector3d((double)targetDirection.X, (double)targetDirection.Y, (double)targetDirection.Z), mainHand, delegate(bool _)
			{
			}, this.GetAdditionalData(slot, player, mainHand));
			this.Api.World.AddCameraShake(this.Stats.ScreenShakeStrength * this.CurrentSwingSpeed / this.Stats.MaxSwingSpeed);
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.SpawnedProjectile, mainHand);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002CFA9 File Offset: 0x0002B1A9
		protected virtual bool ShootCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			base.SetState<SlingState>(SlingState.Unloaded, true);
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
			return true;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
		protected virtual byte[] GetAdditionalData(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			float swingSpeed = this.CurrentSwingSpeed / this.Stats.MaxSwingSpeed;
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(swingSpeed);
			binaryWriter.Write(this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f));
			return memoryStream.ToArray();
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002D038 File Offset: 0x0002B238
		[return: Nullable(2)]
		protected virtual ItemSlot GetBulletSlot(EntityPlayer player)
		{
			ItemSlot bulletSlot = null;
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
					bulletSlot = slot;
					return false;
				}
				return true;
			});
			if (bulletSlot == null)
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
					if (slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, slot.Itemstack.Item.Code.ToString()))
					{
						bulletSlot = slot;
						return false;
					}
					return true;
				});
			}
			return bulletSlot;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002D08B File Offset: 0x0002B28B
		protected bool CanBlockWithOtherHand(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = (mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot).Itemstack;
			object obj;
			if (itemstack == null)
			{
				obj = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				obj = ((collectible != null) ? collectible.GetCollectibleInterface<IHasMeleeWeaponActions>() : null);
			}
			object obj2 = obj;
			return obj2 != null && obj2.CanBlock(player, !mainHand);
		}

		// Token: 0x04000352 RID: 850
		protected new readonly ICoreClientAPI Api;

		// Token: 0x04000353 RID: 851
		protected readonly AnimatableAttachable Attachable;

		// Token: 0x04000354 RID: 852
		protected readonly ModelTransform BulletTransform;

		// Token: 0x04000355 RID: 853
		protected readonly ClientAimingSystem AimingSystem;

		// Token: 0x04000356 RID: 854
		protected readonly SlingStats Stats;

		// Token: 0x04000357 RID: 855
		protected readonly AimingStats AimingStats;

		// Token: 0x04000358 RID: 856
		protected readonly AmmoSelector AmmoSelector;

		// Token: 0x04000359 RID: 857
		protected readonly Settings Settings;

		// Token: 0x0400035A RID: 858
		[Nullable(2)]
		protected AimingAnimationController AimingAnimationController;

		// Token: 0x0400035B RID: 859
		protected bool AfterLoad;

		// Token: 0x0400035C RID: 860
		protected float CurrentSwingSpeed;

		// Token: 0x0400035D RID: 861
		protected bool ReleaseWhenReady;

		// Token: 0x0400035E RID: 862
		protected bool Released;
	}
}
