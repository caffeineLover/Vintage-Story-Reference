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
	// Token: 0x020001A0 RID: 416
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ThirdPersonAnimationsBehavior : EntityBehavior, IDisposable
	{
		// Token: 0x0600141F RID: 5151 RVA: 0x00063E6C File Offset: 0x0006206C
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

		// Token: 0x06001420 RID: 5152 RVA: 0x00064018 File Offset: 0x00062218
		public override string PropertyName()
		{
			return "ThirdPersonAnimations";
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00064020 File Offset: 0x00062220
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

		// Token: 0x06001422 RID: 5154 RVA: 0x000641AE File Offset: 0x000623AE
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.PartialDispose();
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x000641BD File Offset: 0x000623BD
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x000641C5 File Offset: 0x000623C5
		public PlayerItemFrame? FrameOverride { get; set; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x000641D0 File Offset: 0x000623D0
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

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x000641FC File Offset: 0x000623FC
		public bool HasActiveAnimationFrame
		{
			get
			{
				return this.FrameOverride != null || this._composer.AnyActiveAnimations();
			}
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00064228 File Offset: 0x00062428
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

		// Token: 0x06001428 RID: 5160 RVA: 0x000642BC File Offset: 0x000624BC
		public void Play(bool mainHand, string animation, string category = "main", float animationSpeed = 1f, float weight = 1f, bool easeOut = true)
		{
			AnimationRequestByCode request = new AnimationRequestByCode(animation, animationSpeed, weight, category, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1), easeOut, null, null);
			this.Play(request, mainHand);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00064300 File Offset: 0x00062500
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

		// Token: 0x0600142A RID: 5162 RVA: 0x000644CB File Offset: 0x000626CB
		public void Stop(string category)
		{
			this._composer.Stop(category);
			if (this._mainPlayer)
			{
				this._animationSystem.SendStopPacket(category, this.entity.EntityId);
			}
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x000644F8 File Offset: 0x000626F8
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

		// Token: 0x0600142C RID: 5164 RVA: 0x0006455C File Offset: 0x0006275C
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

		// Token: 0x0600142D RID: 5165 RVA: 0x000646F4 File Offset: 0x000628F4
		private void ApplyFrame(PlayerItemFrame frame, ElementPose pose, AnimatorBase animator, bool clearPose = false)
		{
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
				frame.Apply(pose, element, this._eyePosition, this._eyeHeight, this._pitch, this._composer.AnyActiveAnimations(), true);
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
				frame.Apply(extendedPoseValue, element, this._eyePosition, this._eyeHeight, this._pitch, this._composer.AnyActiveAnimations(), true);
				return;
			}
			frame.Apply(pose, element, this._eyePosition, this._eyeHeight, this._pitch, this._composer.AnyActiveAnimations(), true);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x000647E4 File Offset: 0x000629E4
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

		// Token: 0x0600142F RID: 5167 RVA: 0x00064831 File Offset: 0x00062A31
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

		// Token: 0x06001430 RID: 5168 RVA: 0x00064868 File Offset: 0x00062A68
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

		// Token: 0x06001431 RID: 5169 RVA: 0x00064B74 File Offset: 0x00062D74
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

		// Token: 0x06001432 RID: 5170 RVA: 0x00064E80 File Offset: 0x00063080
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

		// Token: 0x06001433 RID: 5171 RVA: 0x00064F18 File Offset: 0x00063118
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

		// Token: 0x06001434 RID: 5172 RVA: 0x00064F85 File Offset: 0x00063185
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

		// Token: 0x06001435 RID: 5173 RVA: 0x00064FC0 File Offset: 0x000631C0
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

		// Token: 0x06001436 RID: 5174 RVA: 0x00065034 File Offset: 0x00063234
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

		// Token: 0x06001437 RID: 5175 RVA: 0x000650CF File Offset: 0x000632CF
		private void PartialDispose()
		{
			this.DisposeCore(false);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x000650D8 File Offset: 0x000632D8
		public void Dispose()
		{
			this.DisposeCore(true);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x000650E4 File Offset: 0x000632E4
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

		// Token: 0x040009F6 RID: 2550
		private readonly Composer _composer;

		// Token: 0x040009F7 RID: 2551
		private readonly EntityPlayer _player;

		// Token: 0x040009F8 RID: 2552
		[Nullable(2)]
		private readonly AnimationsManager _animationsManager;

		// Token: 0x040009F9 RID: 2553
		private readonly AnimationSystemClient _animationSystem;

		// Token: 0x040009FA RID: 2554
		private readonly CombatOverhaulSystem _system;

		// Token: 0x040009FB RID: 2555
		private PlayerItemFrame _lastFrame = PlayerItemFrame.Zero;

		// Token: 0x040009FC RID: 2556
		private readonly List<string> _offhandCategories = new List<string>();

		// Token: 0x040009FD RID: 2557
		private readonly List<string> _mainHandCategories = new List<string>();

		// Token: 0x040009FE RID: 2558
		private readonly bool _mainPlayer;

		// Token: 0x040009FF RID: 2559
		private readonly Settings _settings;

		// Token: 0x04000A00 RID: 2560
		private bool _frameApplied;

		// Token: 0x04000A01 RID: 2561
		private int _offHandItemId;

		// Token: 0x04000A02 RID: 2562
		private int _mainHandItemId;

		// Token: 0x04000A03 RID: 2563
		private long _mainHandIdleTimer = -1L;

		// Token: 0x04000A04 RID: 2564
		private long _offHandIdleTimer = -1L;

		// Token: 0x04000A05 RID: 2565
		[Nullable(2)]
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A06 RID: 2566
		private bool _disposed;

		// Token: 0x04000A07 RID: 2567
		[Nullable(2)]
		private Animatable _animatable;

		// Token: 0x04000A08 RID: 2568
		private int _animatableItemId = -1;

		// Token: 0x04000A09 RID: 2569
		private float _pitch;

		// Token: 0x04000A0A RID: 2570
		private Vector3 _eyePosition;

		// Token: 0x04000A0B RID: 2571
		private float _eyeHeight;

		// Token: 0x04000A0C RID: 2572
		private long _ownerEntityId;

		// Token: 0x04000A0D RID: 2573
		private static readonly TimeSpan _readyTimeout = TimeSpan.FromSeconds(3L);

		// Token: 0x04000A0E RID: 2574
		private static Dictionary<string, ThirdPersonAnimationsBehavior> _existingBehaviors = new Dictionary<string, ThirdPersonAnimationsBehavior>();
	}
}
