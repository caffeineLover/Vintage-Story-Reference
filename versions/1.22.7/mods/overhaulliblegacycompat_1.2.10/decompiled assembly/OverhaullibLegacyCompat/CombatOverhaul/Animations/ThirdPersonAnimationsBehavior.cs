using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration;
using CombatOverhaul.Integration.Transpilers;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001AE RID: 430
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThirdPersonAnimationsBehavior : EntityBehavior, IDisposable
	{
		// Token: 0x06001497 RID: 5271 RVA: 0x000658EC File Offset: 0x00063AEC
		public ThirdPersonAnimationsBehavior(Entity entity) : base(entity)
		{
			EntityPlayer player = entity as EntityPlayer;
			if (player == null)
			{
				throw new ArgumentException("Only for players");
			}
			this._player = player;
			this._api = (player.Api as ICoreClientAPI);
			this._animationsManager = player.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true).PlayerAnimationsManager;
			AnimationSystemClient clientTpAnimationSystem = player.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true).ClientTpAnimationSystem;
			if (clientTpAnimationSystem == null)
			{
				throw new Exception();
			}
			this._animationSystem = clientTpAnimationSystem;
			this._system = player.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			this._settings = this._system.Settings;
			this._composer = new Composer(null, null, player);
			AnimationPatches.AnimationBehaviors[player.EntityId] = this;
			AnimationPatches.ActiveEntities.Add(player.EntityId);
			this._system.OnDispose += new Action(this.Dispose);
			EntityPlayer entityPlayer = entity as EntityPlayer;
			string text = (entityPlayer != null) ? entityPlayer.PlayerUID : null;
			ICoreClientAPI api = this._api;
			this._mainPlayer = (text == ((api != null) ? api.Settings.String["playeruid"] : null));
			if (player.Api.Side == 2)
			{
				ThirdPersonAnimationsBehavior previousBehavior;
				if (ThirdPersonAnimationsBehavior._existingBehaviors.TryGetValue(this._player.PlayerUID, ref previousBehavior))
				{
					previousBehavior.PartialDispose();
				}
				ThirdPersonAnimationsBehavior._existingBehaviors[this._player.PlayerUID] = this;
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00065A98 File Offset: 0x00063C98
		public override string PropertyName()
		{
			return "ThirdPersonAnimations";
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00065AA0 File Offset: 0x00063CA0
		public override void OnGameTick(float deltaTime)
		{
			if (!this._player.IsRendered || this._player.RightHandItemSlot == null || this._player.LeftHandItemSlot == null)
			{
				return;
			}
			ItemStack itemstack = this._player.RightHandItemSlot.Itemstack;
			int? num;
			if (itemstack == null)
			{
				num = default(int?);
			}
			else
			{
				Item item = itemstack.Item;
				num = ((item != null) ? new int?(item.Id) : default(int?));
			}
			int? num2 = num;
			int mainHandItemId = num2.GetValueOrDefault();
			ItemStack itemstack2 = this._player.LeftHandItemSlot.Itemstack;
			int? num3;
			if (itemstack2 == null)
			{
				num3 = default(int?);
			}
			else
			{
				Item item2 = itemstack2.Item;
				num3 = ((item2 != null) ? new int?(item2.Id) : default(int?));
			}
			num2 = num3;
			int offhandItemId = num2.GetValueOrDefault();
			if (!this._mainPlayer && (this._mainHandItemId != mainHandItemId || this._offHandItemId != offhandItemId) && !this.ItemHasIdleAnimations(true) && !this.ItemHasIdleAnimations(false))
			{
				this._composer.StopAll();
			}
			if (this._mainHandItemId != mainHandItemId)
			{
				this._mainHandItemId = mainHandItemId;
				this.MainHandItemChanged();
			}
			if (this._offHandItemId != offhandItemId)
			{
				this._offHandItemId = offhandItemId;
				this.OffhandItemChanged();
			}
			if (this._api != null && this._ownerEntityId == 0L)
			{
				IClientWorldAccessor world = this._api.World;
				long? num4;
				if (world == null)
				{
					num4 = default(long?);
				}
				else
				{
					IClientPlayer player = world.Player;
					if (player == null)
					{
						num4 = default(long?);
					}
					else
					{
						EntityPlayer entity = player.Entity;
						num4 = ((entity != null) ? new long?(entity.EntityId) : default(long?));
					}
				}
				long? num5 = num4;
				this._ownerEntityId = num5.GetValueOrDefault();
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00065C2E File Offset: 0x00063E2E
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.PartialDispose();
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x00065C3D File Offset: 0x00063E3D
		// (set) Token: 0x0600149C RID: 5276 RVA: 0x00065C45 File Offset: 0x00063E45
		public PlayerItemFrame? FrameOverride { get; set; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x00065C50 File Offset: 0x00063E50
		public PlayerItemFrame CurrentFrame
		{
			get
			{
				PlayerItemFrame? frameOverride = this.FrameOverride;
				if (frameOverride == null)
				{
					return this._lastFrame;
				}
				return frameOverride.GetValueOrDefault();
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x00065C7C File Offset: 0x00063E7C
		public bool HasActiveAnimationFrame
		{
			get
			{
				return this.FrameOverride != null || this._composer.AnyActiveAnimations();
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00065CA8 File Offset: 0x00063EA8
		public void Play(AnimationRequestByCode requestByCode, bool mainHand = true)
		{
			if (this._animationsManager == null)
			{
				return;
			}
			Animation animation;
			if (!this._animationsManager.GetAnimation(out animation, requestByCode.Animation, this._player, false))
			{
				LoggerUtil.Verbose(this._api, this, "Animation '" + requestByCode.Animation + "' was not found");
				return;
			}
			AnimationRequest request = new AnimationRequest(animation, requestByCode);
			this.PlayRequest(request, mainHand);
			this.StopIdleTimer(mainHand);
			if (this._mainPlayer)
			{
				this._animationSystem.SendPlayPacket(requestByCode, mainHand, this.entity.EntityId, this.GetCurrentItemId(mainHand));
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00065D3C File Offset: 0x00063F3C
		public void Play(bool mainHand, string animation, string category = "main", float animationSpeed = 1f, float weight = 1f, bool easeOut = true)
		{
			AnimationRequestByCode request = new AnimationRequestByCode(animation, animationSpeed, weight, category, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1), easeOut, null, null);
			this.Play(request, mainHand);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00065D80 File Offset: 0x00063F80
		public void PlayReadyAnimation(bool mainHand = true)
		{
			if (mainHand)
			{
				ItemStack itemstack = this._player.RightHandItemSlot.Itemstack;
				IHasIdleAnimations hasIdleAnimations;
				if (itemstack == null)
				{
					hasIdleAnimations = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					hasIdleAnimations = ((collectible != null) ? collectible.GetCollectibleInterface<IHasIdleAnimations>() : null);
				}
				IHasIdleAnimations staticAnimation = hasIdleAnimations;
				ItemStack itemstack2 = this._player.RightHandItemSlot.Itemstack;
				IHasDynamicIdleAnimations hasDynamicIdleAnimations;
				if (itemstack2 == null)
				{
					hasDynamicIdleAnimations = null;
				}
				else
				{
					CollectibleObject collectible2 = itemstack2.Collectible;
					hasDynamicIdleAnimations = ((collectible2 != null) ? collectible2.GetCollectibleInterface<IHasDynamicIdleAnimations>() : null);
				}
				IHasDynamicIdleAnimations dynamicAnimation = hasDynamicIdleAnimations;
				if (staticAnimation != null)
				{
					this.Play(staticAnimation.ReadyAnimation, mainHand);
					this.StartIdleTimer(staticAnimation.IdleAnimation, mainHand);
				}
				if (dynamicAnimation != null)
				{
					AnimationRequestByCode? readyAnimation = dynamicAnimation.GetReadyAnimation(this._player, this._player.RightHandItemSlot, mainHand);
					AnimationRequestByCode? idleAnimation = dynamicAnimation.GetIdleAnimation(this._player, this._player.RightHandItemSlot, mainHand);
					if (readyAnimation != null && idleAnimation != null)
					{
						this.Play(readyAnimation.Value, mainHand);
						this.StartIdleTimer(idleAnimation.Value, mainHand);
						return;
					}
				}
			}
			else
			{
				ItemStack itemstack3 = this._player.LeftHandItemSlot.Itemstack;
				IHasIdleAnimations hasIdleAnimations2;
				if (itemstack3 == null)
				{
					hasIdleAnimations2 = null;
				}
				else
				{
					CollectibleObject collectible3 = itemstack3.Collectible;
					hasIdleAnimations2 = ((collectible3 != null) ? collectible3.GetCollectibleInterface<IHasIdleAnimations>() : null);
				}
				IHasIdleAnimations staticAnimation2 = hasIdleAnimations2;
				ItemStack itemstack4 = this._player.LeftHandItemSlot.Itemstack;
				IHasDynamicIdleAnimations hasDynamicIdleAnimations2;
				if (itemstack4 == null)
				{
					hasDynamicIdleAnimations2 = null;
				}
				else
				{
					CollectibleObject collectible4 = itemstack4.Collectible;
					hasDynamicIdleAnimations2 = ((collectible4 != null) ? collectible4.GetCollectibleInterface<IHasDynamicIdleAnimations>() : null);
				}
				IHasDynamicIdleAnimations dynamicAnimation2 = hasDynamicIdleAnimations2;
				if (staticAnimation2 != null)
				{
					this.Play(staticAnimation2.ReadyAnimation, mainHand);
					this.StartIdleTimer(staticAnimation2.IdleAnimation, mainHand);
				}
				if (dynamicAnimation2 != null)
				{
					AnimationRequestByCode? readyAnimation2 = dynamicAnimation2.GetReadyAnimation(this._player, this._player.LeftHandItemSlot, mainHand);
					AnimationRequestByCode? idleAnimation2 = dynamicAnimation2.GetIdleAnimation(this._player, this._player.LeftHandItemSlot, mainHand);
					if (readyAnimation2 != null && idleAnimation2 != null)
					{
						this.Play(readyAnimation2.Value, mainHand);
						this.StartIdleTimer(idleAnimation2.Value, mainHand);
					}
				}
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00065F4B File Offset: 0x0006414B
		public void Stop(string category)
		{
			this._composer.Stop(category);
			if (this._mainPlayer)
			{
				this._animationSystem.SendStopPacket(category, this.entity.EntityId);
			}
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00065F78 File Offset: 0x00064178
		public void OnFrame(Entity targetEntity, ElementPose pose, AnimatorBase animator)
		{
			this._frameApplied = true;
			if (!targetEntity.IsRendered || DebugWindowManager.PlayAnimationsInThirdPerson || this.IsFirstPerson(targetEntity))
			{
				return;
			}
			if (this.FrameOverride != null)
			{
				this.ApplyFrame(this.FrameOverride.Value, pose, animator, false);
				return;
			}
			this.ApplyFrame(this._lastFrame, pose, animator, false);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00065FDC File Offset: 0x000641DC
		internal void OnBeforeFrame(Entity targetEntity, float dt)
		{
			if (this._settings.DisableThirdPersonAnimations)
			{
				return;
			}
			if (this.entity.EntityId != targetEntity.EntityId || !targetEntity.IsRendered)
			{
				return;
			}
			this._lastFrame = this._composer.Compose(TimeSpan.FromSeconds((double)dt));
			if (this._composer.AnyActiveAnimations())
			{
				EntityAgent entityAgent = this.entity as EntityAgent;
				int? num;
				if (entityAgent == null)
				{
					num = default(int?);
				}
				else
				{
					ItemSlot rightHandItemSlot = entityAgent.RightHandItemSlot;
					if (rightHandItemSlot == null)
					{
						num = default(int?);
					}
					else
					{
						ItemStack itemstack = rightHandItemSlot.Itemstack;
						if (itemstack == null)
						{
							num = default(int?);
						}
						else
						{
							Item item = itemstack.Item;
							num = ((item != null) ? new int?(item.Id) : default(int?));
						}
					}
				}
				int? num2 = num;
				int mainHandItemId = num2.GetValueOrDefault();
				if (mainHandItemId != this._animatableItemId)
				{
					this._animatableItemId = mainHandItemId;
					EntityAgent entityAgent2 = this.entity as EntityAgent;
					object obj;
					if (entityAgent2 == null)
					{
						obj = null;
					}
					else
					{
						ItemSlot rightHandItemSlot2 = entityAgent2.RightHandItemSlot;
						if (rightHandItemSlot2 == null)
						{
							obj = null;
						}
						else
						{
							ItemStack itemstack2 = rightHandItemSlot2.Itemstack;
							if (itemstack2 == null)
							{
								obj = null;
							}
							else
							{
								Item item2 = itemstack2.Item;
								obj = ((item2 != null) ? item2.GetCollectibleBehavior(typeof(Animatable), true) : null);
							}
						}
					}
					this._animatable = (obj as Animatable);
				}
				this._pitch = targetEntity.Pos.HeadPitch;
				this._eyePosition = new Vector3((float)this.entity.LocalEyePos.X, (float)this.entity.LocalEyePos.Y, (float)this.entity.LocalEyePos.Z);
				this._eyeHeight = (float)this.entity.Properties.EyeHeight;
			}
			this._frameApplied = false;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00066174 File Offset: 0x00064374
		private void ApplyFrame(PlayerItemFrame frame, ElementPose pose, AnimatorBase animator, bool clearPose = false)
		{
			Vector3 eyePosition;
			eyePosition..ctor(this._eyePosition.X, EditorPreviewEyeAnchor.ResolveY(this._eyePosition.Y, this._eyeHeight, clearPose), this._eyePosition.Z);
			ExtendedElementPose extendedPose = pose as ExtendedElementPose;
			if (extendedPose == null)
			{
				return;
			}
			EnumAnimatedElement element = extendedPose.ElementNameEnum;
			ExtendedElementPose extendedPoseValue = extendedPose;
			if (clearPose)
			{
				pose.Clear();
			}
			if (element == EnumAnimatedElement.Unknown)
			{
				frame.Apply(pose, element, eyePosition, this._eyeHeight, this._pitch, this._composer.AnyActiveAnimations(), true);
				return;
			}
			if (this._animatable != null && frame.DetachedAnchor)
			{
				this._animatable.DetachedAnchor = true;
			}
			if (this._animatable != null && frame.SwitchArms)
			{
				this._animatable.SwitchArms = true;
			}
			if (element == EnumAnimatedElement.LowerTorso && !clearPose)
			{
				return;
			}
			if (extendedPoseValue != null)
			{
				frame.Apply(extendedPoseValue, element, eyePosition, this._eyeHeight, this._pitch, this._composer.AnyActiveAnimations(), true);
				return;
			}
			frame.Apply(pose, element, eyePosition, this._eyeHeight, this._pitch, this._composer.AnyActiveAnimations(), true);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0006628C File Offset: 0x0006448C
		private bool IsFirstPerson(Entity entity)
		{
			if (this._ownerEntityId != entity.EntityId)
			{
				return false;
			}
			ICoreClientAPI coreClientAPI = entity.Api as ICoreClientAPI;
			if (coreClientAPI != null)
			{
				IClientWorldAccessor world = coreClientAPI.World;
				if (world != null)
				{
					IClientPlayer player = world.Player;
					if (player != null)
					{
						return player.CameraMode == 0;
					}
				}
			}
			return false;
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x000662D9 File Offset: 0x000644D9
		private void PlayRequest(AnimationRequest request, bool mainHand = true)
		{
			this._composer.Play(request);
			if (mainHand)
			{
				this._mainHandCategories.Add(request.Category);
				return;
			}
			this._offhandCategories.Add(request.Category);
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00066310 File Offset: 0x00064510
		private void MainHandItemChanged()
		{
			if (!this._mainPlayer)
			{
				foreach (string category in this._mainHandCategories)
				{
					this.Stop(category);
				}
				this._mainHandCategories.Clear();
				return;
			}
			this.StopIdleTimer(true);
			ItemStack itemstack = this._player.RightHandItemSlot.Itemstack;
			IHasIdleAnimations hasIdleAnimations;
			if (itemstack == null)
			{
				hasIdleAnimations = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				hasIdleAnimations = ((collectible != null) ? collectible.GetCollectibleInterface<IHasIdleAnimations>() : null);
			}
			IHasIdleAnimations staticAnimation = hasIdleAnimations;
			ItemStack itemstack2 = this._player.RightHandItemSlot.Itemstack;
			IHasDynamicIdleAnimations hasDynamicIdleAnimations;
			if (itemstack2 == null)
			{
				hasDynamicIdleAnimations = null;
			}
			else
			{
				CollectibleObject collectible2 = itemstack2.Collectible;
				hasDynamicIdleAnimations = ((collectible2 != null) ? collectible2.GetCollectibleInterface<IHasDynamicIdleAnimations>() : null);
			}
			IHasDynamicIdleAnimations dynamicAnimation = hasDynamicIdleAnimations;
			string readyCategory;
			if (staticAnimation != null)
			{
				string readyCategory = staticAnimation.ReadyAnimation.Category;
				IEnumerable<string> mainHandCategories = this._mainHandCategories;
				Func<string, bool> func;
				Func<string, bool> <>9__0;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = ((string element) => element != readyCategory));
				}
				foreach (string category2 in Enumerable.Where<string>(mainHandCategories, func))
				{
					this.Stop(category2);
				}
				this._mainHandCategories.Clear();
				this.Play(staticAnimation.ReadyAnimation, true);
				this.StartIdleTimer(staticAnimation.IdleAnimation, true);
				return;
			}
			if (dynamicAnimation == null)
			{
				foreach (string category3 in this._mainHandCategories)
				{
					this.Stop(category3);
				}
				this._mainHandCategories.Clear();
				return;
			}
			AnimationRequestByCode? readyAnimation = dynamicAnimation.GetReadyAnimation(this._player, this._player.RightHandItemSlot, true);
			AnimationRequestByCode? idleAnimation = dynamicAnimation.GetIdleAnimation(this._player, this._player.RightHandItemSlot, true);
			if (readyAnimation == null || idleAnimation == null)
			{
				foreach (string category4 in this._mainHandCategories)
				{
					this.Stop(category4);
				}
				this._mainHandCategories.Clear();
				return;
			}
			readyCategory = readyAnimation.Value.Category;
			IEnumerable<string> mainHandCategories2 = this._mainHandCategories;
			Func<string, bool> <>9__1;
			Func<string, bool> func2;
			if ((func2 = <>9__1) == null)
			{
				func2 = (<>9__1 = ((string element) => element != readyCategory));
			}
			foreach (string category5 in Enumerable.Where<string>(mainHandCategories2, func2))
			{
				this.Stop(category5);
			}
			this._mainHandCategories.Clear();
			this.Play(readyAnimation.Value, true);
			this.StartIdleTimer(idleAnimation.Value, true);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0006661C File Offset: 0x0006481C
		private void OffhandItemChanged()
		{
			if (!this._mainPlayer)
			{
				foreach (string category in this._offhandCategories)
				{
					this.Stop(category);
				}
				this._offhandCategories.Clear();
				return;
			}
			this.StopIdleTimer(false);
			ItemStack itemstack = this._player.LeftHandItemSlot.Itemstack;
			IHasIdleAnimations hasIdleAnimations;
			if (itemstack == null)
			{
				hasIdleAnimations = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				hasIdleAnimations = ((collectible != null) ? collectible.GetCollectibleInterface<IHasIdleAnimations>() : null);
			}
			IHasIdleAnimations staticAnimation = hasIdleAnimations;
			ItemStack itemstack2 = this._player.LeftHandItemSlot.Itemstack;
			IHasDynamicIdleAnimations hasDynamicIdleAnimations;
			if (itemstack2 == null)
			{
				hasDynamicIdleAnimations = null;
			}
			else
			{
				CollectibleObject collectible2 = itemstack2.Collectible;
				hasDynamicIdleAnimations = ((collectible2 != null) ? collectible2.GetCollectibleInterface<IHasDynamicIdleAnimations>() : null);
			}
			IHasDynamicIdleAnimations dynamicAnimation = hasDynamicIdleAnimations;
			string readyCategory;
			if (staticAnimation != null)
			{
				string readyCategory = staticAnimation.ReadyAnimation.Category;
				IEnumerable<string> offhandCategories = this._offhandCategories;
				Func<string, bool> func;
				Func<string, bool> <>9__0;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = ((string element) => element != readyCategory));
				}
				foreach (string category2 in Enumerable.Where<string>(offhandCategories, func))
				{
					this.Stop(category2);
				}
				this._offhandCategories.Clear();
				this.Play(staticAnimation.ReadyAnimation, false);
				this.StartIdleTimer(staticAnimation.IdleAnimation, false);
				return;
			}
			if (dynamicAnimation == null)
			{
				foreach (string category3 in this._offhandCategories)
				{
					this.Stop(category3);
				}
				this._offhandCategories.Clear();
				return;
			}
			AnimationRequestByCode? readyAnimation = dynamicAnimation.GetReadyAnimation(this._player, this._player.LeftHandItemSlot, false);
			AnimationRequestByCode? idleAnimation = dynamicAnimation.GetIdleAnimation(this._player, this._player.LeftHandItemSlot, false);
			if (readyAnimation == null || idleAnimation == null)
			{
				foreach (string category4 in this._offhandCategories)
				{
					this.Stop(category4);
				}
				this._offhandCategories.Clear();
				return;
			}
			readyCategory = readyAnimation.Value.Category;
			IEnumerable<string> offhandCategories2 = this._offhandCategories;
			Func<string, bool> <>9__1;
			Func<string, bool> func2;
			if ((func2 = <>9__1) == null)
			{
				func2 = (<>9__1 = ((string element) => element != readyCategory));
			}
			foreach (string category5 in Enumerable.Where<string>(offhandCategories2, func2))
			{
				this.Stop(category5);
			}
			this._offhandCategories.Clear();
			this.Play(readyAnimation.Value, false);
			this.StartIdleTimer(idleAnimation.Value, false);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00066928 File Offset: 0x00064B28
		private void StartIdleTimer(AnimationRequestByCode request, bool mainHand)
		{
			ICoreClientAPI api = this._api;
			if ((api != null && api.IsGamePaused) || !this._mainPlayer)
			{
				return;
			}
			this.StopIdleTimer(mainHand);
			ICoreClientAPI api2 = this._api;
			long timer = (api2 != null) ? api2.World.RegisterCallback(delegate(float _)
			{
				this.PlayIdleAnimation(request, mainHand);
			}, (int)ThirdPersonAnimationsBehavior._readyTimeout.TotalMilliseconds) : -1L;
			if (mainHand)
			{
				this._mainHandIdleTimer = timer;
				return;
			}
			this._offHandIdleTimer = timer;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000669C0 File Offset: 0x00064BC0
		private void StopIdleTimer(bool mainHand)
		{
			if (mainHand)
			{
				if (this._mainHandIdleTimer != -1L)
				{
					ICoreClientAPI api = this._api;
					if (api != null)
					{
						api.World.UnregisterCallback(this._mainHandIdleTimer);
					}
					this._mainHandIdleTimer = -1L;
					return;
				}
			}
			else if (this._offHandIdleTimer != -1L)
			{
				ICoreClientAPI api2 = this._api;
				if (api2 != null)
				{
					api2.World.UnregisterCallback(this._offHandIdleTimer);
				}
				this._offHandIdleTimer = -1L;
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00066A2D File Offset: 0x00064C2D
		private void PlayIdleAnimation(AnimationRequestByCode request, bool mainHand)
		{
			if (mainHand && this._mainHandIdleTimer == -1L)
			{
				return;
			}
			if (!mainHand && this._offHandIdleTimer == -1L)
			{
				return;
			}
			if (mainHand)
			{
				this._mainHandIdleTimer = -1L;
			}
			else
			{
				this._offHandIdleTimer = -1L;
			}
			this.Play(request, mainHand);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00066A68 File Offset: 0x00064C68
		private bool ItemHasIdleAnimations(bool mainHand)
		{
			ItemSlot slot = mainHand ? this._player.RightHandItemSlot : this._player.LeftHandItemSlot;
			if (slot == null)
			{
				return false;
			}
			ItemStack itemstack = slot.Itemstack;
			bool flag;
			if (itemstack == null)
			{
				flag = (null != null);
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				flag = (((collectible != null) ? collectible.GetCollectibleInterface<IHasIdleAnimations>() : null) != null);
			}
			ItemStack itemstack2 = slot.Itemstack;
			IHasDynamicIdleAnimations hasDynamicIdleAnimations;
			if (itemstack2 == null)
			{
				hasDynamicIdleAnimations = null;
			}
			else
			{
				CollectibleObject collectible2 = itemstack2.Collectible;
				hasDynamicIdleAnimations = ((collectible2 != null) ? collectible2.GetCollectibleInterface<IHasDynamicIdleAnimations>() : null);
			}
			IHasDynamicIdleAnimations dynamicAnimation = hasDynamicIdleAnimations;
			return flag || dynamicAnimation != null;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00066ADC File Offset: 0x00064CDC
		private int GetCurrentItemId(bool mainHand)
		{
			int? num2;
			if (!mainHand)
			{
				ItemStack itemstack = this._player.LeftHandItemSlot.Itemstack;
				int? num;
				if (itemstack == null)
				{
					num = default(int?);
				}
				else
				{
					Item item = itemstack.Item;
					num = ((item != null) ? new int?(item.Id) : default(int?));
				}
				num2 = num;
				return num2.GetValueOrDefault();
			}
			ItemStack itemstack2 = this._player.RightHandItemSlot.Itemstack;
			int? num3;
			if (itemstack2 == null)
			{
				num3 = default(int?);
			}
			else
			{
				Item item2 = itemstack2.Item;
				num3 = ((item2 != null) ? new int?(item2.Id) : default(int?));
			}
			num2 = num3;
			return num2.GetValueOrDefault();
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00066B77 File Offset: 0x00064D77
		private void PartialDispose()
		{
			this.DisposeCore(false);
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00066B80 File Offset: 0x00064D80
		public void Dispose()
		{
			this.DisposeCore(true);
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00066B8C File Offset: 0x00064D8C
		private void DisposeCore(bool clearExisting)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this.StopIdleTimer(true);
			this.StopIdleTimer(false);
			this._composer.StopAll();
			this._offhandCategories.Clear();
			this._mainHandCategories.Clear();
			ThirdPersonAnimationsBehavior behavior;
			if (AnimationPatches.AnimationBehaviors.TryGetValue(this._player.EntityId, ref behavior) && behavior == this)
			{
				AnimationPatches.AnimationBehaviors.Remove(this._player.EntityId);
				AnimationPatches.ActiveEntities.Remove(this._player.EntityId);
			}
			this._system.OnDispose -= new Action(this.Dispose);
			if (clearExisting)
			{
				ThirdPersonAnimationsBehavior._existingBehaviors.Clear();
				return;
			}
			ThirdPersonAnimationsBehavior existing;
			if (ThirdPersonAnimationsBehavior._existingBehaviors.TryGetValue(this._player.PlayerUID, ref existing) && existing == this)
			{
				ThirdPersonAnimationsBehavior._existingBehaviors.Remove(this._player.PlayerUID);
			}
		}

		// Token: 0x04000A1E RID: 2590
		private readonly Composer _composer;

		// Token: 0x04000A1F RID: 2591
		private readonly EntityPlayer _player;

		// Token: 0x04000A20 RID: 2592
		[Nullable(2)]
		private readonly AnimationsManager _animationsManager;

		// Token: 0x04000A21 RID: 2593
		private readonly AnimationSystemClient _animationSystem;

		// Token: 0x04000A22 RID: 2594
		private readonly CombatOverhaulSystem _system;

		// Token: 0x04000A23 RID: 2595
		private PlayerItemFrame _lastFrame = PlayerItemFrame.Zero;

		// Token: 0x04000A24 RID: 2596
		private readonly List<string> _offhandCategories = new List<string>();

		// Token: 0x04000A25 RID: 2597
		private readonly List<string> _mainHandCategories = new List<string>();

		// Token: 0x04000A26 RID: 2598
		private readonly bool _mainPlayer;

		// Token: 0x04000A27 RID: 2599
		private readonly Settings _settings;

		// Token: 0x04000A28 RID: 2600
		private bool _frameApplied;

		// Token: 0x04000A29 RID: 2601
		private int _offHandItemId;

		// Token: 0x04000A2A RID: 2602
		private int _mainHandItemId;

		// Token: 0x04000A2B RID: 2603
		private long _mainHandIdleTimer = -1L;

		// Token: 0x04000A2C RID: 2604
		private long _offHandIdleTimer = -1L;

		// Token: 0x04000A2D RID: 2605
		[Nullable(2)]
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A2E RID: 2606
		private bool _disposed;

		// Token: 0x04000A2F RID: 2607
		[Nullable(2)]
		private Animatable _animatable;

		// Token: 0x04000A30 RID: 2608
		private int _animatableItemId = -1;

		// Token: 0x04000A31 RID: 2609
		private float _pitch;

		// Token: 0x04000A32 RID: 2610
		private Vector3 _eyePosition;

		// Token: 0x04000A33 RID: 2611
		private float _eyeHeight;

		// Token: 0x04000A34 RID: 2612
		private long _ownerEntityId;

		// Token: 0x04000A35 RID: 2613
		private static readonly TimeSpan _readyTimeout = TimeSpan.FromSeconds(3L);

		// Token: 0x04000A36 RID: 2614
		private static Dictionary<string, ThirdPersonAnimationsBehavior> _existingBehaviors = new Dictionary<string, ThirdPersonAnimationsBehavior>();
	}
}
