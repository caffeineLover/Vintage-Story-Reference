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
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000196 RID: 406
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FirstPersonAnimationsBehavior : EntityBehavior, IDisposable
	{
		// Token: 0x06001394 RID: 5012 RVA: 0x00060208 File Offset: 0x0005E408
		public FirstPersonAnimationsBehavior(Entity entity) : base(entity)
		{
			EntityPlayer player = entity as EntityPlayer;
			if (player == null)
			{
				throw new ArgumentException("Only for players");
			}
			this._player = player;
			ICoreClientAPI coreClientAPI = player.Api as ICoreClientAPI;
			if (coreClientAPI == null)
			{
				throw new ArgumentException("Only client side");
			}
			this._api = coreClientAPI;
			AnimationsManager playerAnimationsManager = player.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true).PlayerAnimationsManager;
			if (playerAnimationsManager == null)
			{
				throw new Exception();
			}
			this._animationsManager = playerAnimationsManager;
			VanillaAnimationsSystemClient clientVanillaAnimations = player.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true).ClientVanillaAnimations;
			if (clientVanillaAnimations == null)
			{
				throw new Exception();
			}
			this._vanillaAnimationsManager = clientVanillaAnimations;
			this._system = player.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			this._settings = this._system.Settings;
			SoundsSynchronizerClient clientSoundsSynchronizer = this._system.ClientSoundsSynchronizer;
			if (clientSoundsSynchronizer == null)
			{
				throw new Exception();
			}
			SoundsSynchronizerClient soundsManager = clientSoundsSynchronizer;
			ParticleEffectsManager particleEffectsManager2 = player.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true).ParticleEffectsManager;
			if (particleEffectsManager2 == null)
			{
				throw new Exception();
			}
			ParticleEffectsManager particleEffectsManager = particleEffectsManager2;
			this._composer = new Composer(soundsManager, particleEffectsManager, player);
			this._mainHandIdleAnimationsController = new IdleAnimationsController(player, delegate(AnimationRequestByCode request)
			{
				this.PlayImpl(request, true);
			}, delegate
			{
				this.Stop("main");
			}, () => this._player.RightHandItemSlot, true);
			this._offHandIdleAnimationsController = new IdleAnimationsController(player, delegate(AnimationRequestByCode request)
			{
				this.PlayImpl(request, false);
			}, delegate
			{
				this.Stop("mainOffhand");
			}, () => this._player.LeftHandItemSlot, false);
			this.TryActivateMainPlayer();
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000603E4 File Offset: 0x0005E5E4
		public override string PropertyName()
		{
			return "CombatOverhaul:FirstPersonAnimations";
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000603EB File Offset: 0x0005E5EB
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.Dispose();
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000603FA File Offset: 0x0005E5FA
		public override void AfterInitialized(bool onFirstSpawn)
		{
			this.TryActivateMainPlayer();
			if (!this._mainPlayer)
			{
				return;
			}
			this.ResolveThirdPersonAnimations();
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00060414 File Offset: 0x0005E614
		public override void OnGameTick(float deltaTime)
		{
			this.TryActivateMainPlayer();
			if (!this._mainPlayer || this._player.RightHandItemSlot == null || this._player.LeftHandItemSlot == null)
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
			if (this._mainHandItemId != mainHandItemId)
			{
				this._mainHandItemId = mainHandItemId;
				this.InHandItemChanged(true);
			}
			if (this._offHandItemId != offhandItemId)
			{
				this._offHandItemId = offhandItemId;
				this.InHandItemChanged(false);
			}
			this._mainHandIdleAnimationsController.Update();
			this._offHandIdleAnimationsController.Update();
			foreach (ValueTuple<AnimationRequest, bool, bool, int> valueTuple in this._playRequests)
			{
				AnimationRequest request = valueTuple.Item1;
				bool mainHand = valueTuple.Item2;
				if (!valueTuple.Item3)
				{
					this.PlayRequest(request, mainHand);
				}
			}
			this._playRequests.Clear();
			this._settingsUpdateTimeSec += deltaTime;
			if (this._settingsUpdateTimeSec > 3f)
			{
				this._settingsUpdateTimeSec = 0f;
				this._settingsFOV = ClientSettings.FieldOfView;
				this._settingsHandsFOV = ClientSettings.FieldOfView;
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
			this.AdvanceMountedAnimationFallback(deltaTime);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0006063C File Offset: 0x0005E83C
		public void OnFrame(Entity entity, ElementPose pose, AnimatorBase animator)
		{
			this._frameApplied = true;
			if (!DebugWindowManager.PlayAnimationsInThirdPerson && !this.IsFirstPerson(entity))
			{
				return;
			}
			if (!this._composer.AnyActiveAnimations() && this.FrameOverride == null)
			{
				if (this._resetFov)
				{
					this.SetFov(1f, false);
					this._player.HeadBobbingAmplitude /= this._previousHeadBobbingAmplitudeFactor;
					this._previousHeadBobbingAmplitudeFactor = 1f;
					this._resetFov = false;
				}
				return;
			}
			if (this.FrameOverride != null)
			{
				this.ApplyFrame(this.FrameOverride.Value, pose, animator, false);
				return;
			}
			this.ApplyFrame(this._lastFrame, pose, animator, false);
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x000606F4 File Offset: 0x0005E8F4
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x000606FC File Offset: 0x0005E8FC
		public PlayerItemFrame? FrameOverride { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x00060708 File Offset: 0x0005E908
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

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x00060734 File Offset: 0x0005E934
		public bool HasActiveAnimationFrame
		{
			get
			{
				return this.FrameOverride != null || this._composer.AnyActiveAnimations();
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x0006075E File Offset: 0x0005E95E
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x00060765 File Offset: 0x0005E965
		public static float CurrentFov { get; set; } = (float)ClientSettings.FieldOfView;

		// Token: 0x060013A0 RID: 5024 RVA: 0x00060770 File Offset: 0x0005E970
		public void Play(AnimationRequest request, bool mainHand = true)
		{
			this.TryActivateMainPlayer();
			if (request.Category == this.GetIdleAnimationCategory(mainHand))
			{
				(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Pause();
			}
			this._playRequests.Add(new ValueTuple<AnimationRequest, bool, bool, int>(request, mainHand, false, this.CurrentItemId(mainHand)));
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x000607C8 File Offset: 0x0005E9C8
		public void Play(AnimationRequestByCode requestByCode, bool mainHand = true)
		{
			Animation animation = this.GetAnimationFromRequest(requestByCode);
			if (animation == null)
			{
				return;
			}
			AnimationRequest request = new AnimationRequest(animation, requestByCode);
			this.Play(request, mainHand);
			ICoreClientAPI coreClientAPI = this.entity.Api as ICoreClientAPI;
			this._immersiveFpModeSetting = (coreClientAPI != null && coreClientAPI.Settings.Bool["immersiveFpMode"]);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00060824 File Offset: 0x0005EA24
		public void Play(bool mainHand, string animation, string category = "main", float animationSpeed = 1f, float weight = 1f, [Nullable(2)] Func<bool> callback = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> callbackHandler = null, bool easeOut = true)
		{
			AnimationRequestByCode request = new AnimationRequestByCode(animation, animationSpeed, weight, category, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1), easeOut, callback, callbackHandler);
			this.Play(request, mainHand);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00060868 File Offset: 0x0005EA68
		public void PlayReadyAnimation(bool mainHand = true)
		{
			this.TryActivateMainPlayer();
			(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Start();
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00060888 File Offset: 0x0005EA88
		public void Stop(string category)
		{
			this._composer.Stop(category);
			for (int index = 0; index < this._playRequests.Count; index++)
			{
				if (this._playRequests[index].Item1.Category == category)
				{
					this._playRequests[index] = new ValueTuple<AnimationRequest, bool, bool, int>(new AnimationRequest(), this._playRequests[index].Item2, true, -1);
				}
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000608FE File Offset: 0x0005EAFE
		public void PlayVanillaAnimation(string code, bool mainHand)
		{
			if (code == "")
			{
				return;
			}
			VanillaAnimationsSystemClient vanillaAnimationsManager = this._vanillaAnimationsManager;
			if (vanillaAnimationsManager != null)
			{
				vanillaAnimationsManager.StartAnimation(code);
			}
			if (mainHand)
			{
				this._mainHandVanillaAnimations.Add(code);
				return;
			}
			this._offhandVanillaAnimations.Add(code);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0006093E File Offset: 0x0005EB3E
		public void StopVanillaAnimation(string code, bool mainHand)
		{
			VanillaAnimationsSystemClient vanillaAnimationsManager = this._vanillaAnimationsManager;
			if (vanillaAnimationsManager != null)
			{
				vanillaAnimationsManager.StopAnimation(code);
			}
			if (mainHand)
			{
				this._mainHandVanillaAnimations.Remove(code);
				return;
			}
			this._offhandVanillaAnimations.Remove(code);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00060970 File Offset: 0x0005EB70
		public void StopAllVanillaAnimations(bool mainHand)
		{
			foreach (string code in (mainHand ? this._mainHandVanillaAnimations : this._offhandVanillaAnimations))
			{
				VanillaAnimationsSystemClient vanillaAnimationsManager = this._vanillaAnimationsManager;
				if (vanillaAnimationsManager != null)
				{
					vanillaAnimationsManager.StopAnimation(code);
				}
			}
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000609DC File Offset: 0x0005EBDC
		public void SetSpeedModifier(AnimationSpeedModifierDelegate modifier)
		{
			this._composer.SetSpeedModifier(modifier);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x000609EA File Offset: 0x0005EBEA
		public void StopSpeedModifier()
		{
			this._composer.StopSpeedModifier();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x000609F7 File Offset: 0x0005EBF7
		public bool IsSpeedModifierActive()
		{
			return this._composer.IsSpeedModifierActive();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00060A04 File Offset: 0x0005EC04
		public void PlayFpAndTp(AnimationRequestByCode requestByCode, bool mainHand = true)
		{
			Animation animation = this.GetAnimationFromRequest(requestByCode);
			if (animation == null)
			{
				return;
			}
			AnimationRequest request = new AnimationRequest(animation, requestByCode);
			this.Play(request, mainHand);
			ICoreClientAPI coreClientAPI = this.entity.Api as ICoreClientAPI;
			this._immersiveFpModeSetting = (coreClientAPI != null && coreClientAPI.Settings.Bool["immersiveFpMode"]);
			AnimationRequestByCode tpRequest = new AnimationRequestByCode(requestByCode, requestByCode.AnimationSpeed, null, null);
			ThirdPersonAnimationsBehavior thirdPersonAnimations = this._thirdPersonAnimations;
			if (thirdPersonAnimations == null)
			{
				return;
			}
			thirdPersonAnimations.Play(tpRequest, mainHand);
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00060A84 File Offset: 0x0005EC84
		public void PlayFpAndTp(bool mainHand, string animation, string category = "main", float animationSpeed = 1f, float weight = 1f, [Nullable(2)] Func<bool> callback = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> callbackHandler = null, bool easeOut = true)
		{
			AnimationRequestByCode request = new AnimationRequestByCode(animation, animationSpeed, weight, category, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1), easeOut, callback, callbackHandler);
			this.Play(request, mainHand);
			AnimationRequestByCode tpRequest = new AnimationRequestByCode(request, request.AnimationSpeed, null, null);
			ThirdPersonAnimationsBehavior thirdPersonAnimations = this._thirdPersonAnimations;
			if (thirdPersonAnimations == null)
			{
				return;
			}
			thirdPersonAnimations.Play(tpRequest, mainHand);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00060AEA File Offset: 0x0005ECEA
		public void PlayReadyAnimationFpAndTp(bool mainHand = true)
		{
			(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Start();
			ThirdPersonAnimationsBehavior thirdPersonAnimations = this._thirdPersonAnimations;
			if (thirdPersonAnimations == null)
			{
				return;
			}
			thirdPersonAnimations.PlayReadyAnimation(mainHand);
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00060B14 File Offset: 0x0005ED14
		public void StopFpAndTp(string category)
		{
			this._composer.Stop(category);
			for (int index = 0; index < this._playRequests.Count; index++)
			{
				if (this._playRequests[index].Item1.Category == category)
				{
					this._playRequests[index] = new ValueTuple<AnimationRequest, bool, bool, int>(new AnimationRequest(), this._playRequests[index].Item2, true, -1);
				}
			}
			ThirdPersonAnimationsBehavior thirdPersonAnimations = this._thirdPersonAnimations;
			if (thirdPersonAnimations == null)
			{
				return;
			}
			thirdPersonAnimations.Stop(category);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00060B9C File Offset: 0x0005ED9C
		private bool TryActivateMainPlayer()
		{
			if (this._disposed)
			{
				return false;
			}
			if (this._mainPlayer)
			{
				return true;
			}
			if (!this.IsLocalPlayerEntity())
			{
				return false;
			}
			this._mainPlayer = true;
			this._ownerEntityId = this._player.EntityId;
			AnimationPatches.FirstPersonAnimationBehavior = this;
			AnimationPatches.OwnerEntityId = this._player.EntityId;
			if (!this._registeredDisposeHook)
			{
				this._system.OnDispose += new Action(this.Dispose);
				this._registeredDisposeHook = true;
			}
			this.ResolveThirdPersonAnimations();
			if (!this._reportedMainPlayerActivation)
			{
				this._reportedMainPlayerActivation = true;
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("First-person animation behavior activated for local player '");
				defaultInterpolatedStringHandler.AppendFormatted(this._player.PlayerUID);
				defaultInterpolatedStringHandler.AppendLiteral("' entity ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._player.EntityId);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return true;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00060C98 File Offset: 0x0005EE98
		private bool IsLocalPlayerEntity()
		{
			IClientWorldAccessor world = this._api.World;
			IClientPlayer localPlayer = (world != null) ? world.Player : null;
			if (((localPlayer != null) ? localPlayer.Entity : null) != null && localPlayer.Entity.EntityId == this._player.EntityId)
			{
				return true;
			}
			string localPlayerUid = (localPlayer != null) ? localPlayer.PlayerUID : null;
			if (string.IsNullOrEmpty(localPlayerUid))
			{
				try
				{
					ISettings settings = this._api.Settings;
					string text;
					if (settings == null)
					{
						text = null;
					}
					else
					{
						ISettingsClass<string> @string = settings.String;
						text = ((@string != null) ? @string["playeruid"] : null);
					}
					localPlayerUid = text;
				}
				catch
				{
					localPlayerUid = null;
				}
			}
			return !string.IsNullOrEmpty(localPlayerUid) && this._player.PlayerUID == localPlayerUid;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00060D58 File Offset: 0x0005EF58
		private void ResolveThirdPersonAnimations()
		{
			if (this._thirdPersonAnimations == null)
			{
				this._thirdPersonAnimations = this.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00060D73 File Offset: 0x0005EF73
		internal void OnBeforeFrame(Entity targetEntity, float dt)
		{
			if (!FirstPersonAnimationsBehavior.IsOwner(targetEntity))
			{
				return;
			}
			this.AdvanceAnimationFrame(targetEntity, dt, true);
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00060D88 File Offset: 0x0005EF88
		private void AdvanceMountedAnimationFallback(float dt)
		{
			if (this._player.MountedOn == null || !this._composer.AnyActiveAnimations())
			{
				return;
			}
			if (this._api.World.ElapsedMilliseconds - this._lastAnimationAdvanceMs <= 50L)
			{
				return;
			}
			this.AdvanceAnimationFrame(this._player, dt, false);
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00060DDC File Offset: 0x0005EFDC
		private void AdvanceAnimationFrame(Entity targetEntity, float dt, bool applyRenderState)
		{
			this._lastFrame = this._composer.Compose(TimeSpan.FromSeconds((double)dt));
			this._lastAnimationAdvanceMs = this._api.World.ElapsedMilliseconds;
			if (applyRenderState && this._composer.AnyActiveAnimations())
			{
				if (this._lastFrame.Player.FovMultiplier != 1f)
				{
					this.SetFov(this._lastFrame.Player.FovMultiplier, true);
				}
				this._resetFov = true;
				EntityAgent entityAgent = targetEntity as EntityAgent;
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
					EntityAgent entityAgent2 = targetEntity as EntityAgent;
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
				this._eyePosition = new Vector3((float)targetEntity.LocalEyePos.X, (float)targetEntity.LocalEyePos.Y, (float)targetEntity.LocalEyePos.Z);
				this._eyeHeight = (float)targetEntity.Properties.EyeHeight;
				if (Math.Abs(this._lastFrame.Player.PitchFollow - 0.8f) >= 1E-06f)
				{
					EntityPlayerShapeRenderer renderer = targetEntity.Properties.Client.Renderer as EntityPlayerShapeRenderer;
					if (renderer != null)
					{
						renderer.HeldItemPitchFollowOverride = new float?(this._lastFrame.Player.PitchFollow);
					}
				}
				else
				{
					EntityPlayerShapeRenderer renderer2 = targetEntity.Properties.Client.Renderer as EntityPlayerShapeRenderer;
					if (renderer2 != null)
					{
						renderer2.HeldItemPitchFollowOverride = default(float?);
					}
				}
				if (this._settings.SwitchFromImmersiveFirstPerson)
				{
					ICoreClientAPI clientApi = this.entity.Api as ICoreClientAPI;
					if (clientApi != null && clientApi.Settings.Bool["immersiveFpMode"])
					{
						clientApi.Settings.Bool["immersiveFpMode"] = false;
						this._restIfpSetting = true;
					}
				}
			}
			else if (applyRenderState)
			{
				ICoreClientAPI clientApi2 = this.entity.Api as ICoreClientAPI;
				if (clientApi2 != null && this._restIfpSetting)
				{
					clientApi2.Settings.Bool["immersiveFpMode"] = true;
					this._restIfpSetting = false;
				}
			}
			this._frameApplied = false;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00061088 File Offset: 0x0005F288
		private void PlayImpl(AnimationRequestByCode requestByCode, bool mainHand = true)
		{
			this.TryActivateMainPlayer();
			Animation animation = this.GetAnimationFromRequest(requestByCode);
			if (animation == null)
			{
				return;
			}
			AnimationRequest request = new AnimationRequest(animation, requestByCode);
			this._playRequests.Add(new ValueTuple<AnimationRequest, bool, bool, int>(request, mainHand, false, this.CurrentItemId(mainHand)));
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000610CC File Offset: 0x0005F2CC
		private void ApplyFrame(PlayerItemFrame frame, ElementPose pose, AnimatorBase animator, bool clearPose = false)
		{
			Vector3 eyePosition;
			eyePosition..ctor(this._eyePosition.X, EditorPreviewEyeAnchor.ResolveY(this._eyePosition.Y, this._eyeHeight, clearPose), this._eyePosition.Z);
			ExtendedElementPose extendedPoseValue = null;
			ExtendedElementPose extendedPose = pose as ExtendedElementPose;
			EnumAnimatedElement element;
			if (extendedPose != null)
			{
				element = extendedPose.ElementNameEnum;
				extendedPoseValue = extendedPose;
			}
			else
			{
				ShapeElement forElement = pose.ForElement;
				element = ExtendedElementPose.GetElementNameEnum((forElement != null) ? forElement.Name : null);
			}
			if (clearPose && element != EnumAnimatedElement.Unknown)
			{
				pose.Clear();
			}
			if (element == EnumAnimatedElement.Unknown)
			{
				frame.Apply(pose, element, eyePosition, this._eyeHeight, 0f, false, true);
				return;
			}
			if (element == EnumAnimatedElement.LowerTorso && this.IsImmersiveFirstPerson(this._player) && !clearPose)
			{
				return;
			}
			if (this.IsImmersiveFirstPerson(this._player) && !clearPose)
			{
				PlayerRenderingPatches.SetOffset(0f);
			}
			if (extendedPoseValue != null)
			{
				frame.Apply(extendedPoseValue, element, eyePosition, this._eyeHeight, 0f, false, true);
			}
			else
			{
				frame.Apply(pose, element, eyePosition, this._eyeHeight, 0f, false, true);
			}
			this._player.HeadBobbingAmplitude /= this._previousHeadBobbingAmplitudeFactor;
			this._previousHeadBobbingAmplitudeFactor = frame.Player.BobbingAmplitude;
			this._player.HeadBobbingAmplitude *= this._previousHeadBobbingAmplitudeFactor;
			if (this._animatable != null && frame.DetachedAnchor)
			{
				this._animatable.DetachedAnchor = true;
			}
			if (this._animatable != null && frame.SwitchArms)
			{
				this._animatable.SwitchArms = true;
			}
			this._resetFov = true;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0006124C File Offset: 0x0005F44C
		private static bool IsOwner(Entity entity)
		{
			ICoreClientAPI coreClientAPI = entity.Api as ICoreClientAPI;
			long? num = (coreClientAPI != null) ? new long?(coreClientAPI.World.Player.Entity.EntityId) : default(long?);
			long entityId = entity.EntityId;
			return num.GetValueOrDefault() == entityId & num != null;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000612A8 File Offset: 0x0005F4A8
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

		// Token: 0x060013B9 RID: 5049 RVA: 0x000612F5 File Offset: 0x0005F4F5
		private bool IsImmersiveFirstPerson(Entity entity)
		{
			return this._immersiveFpModeSetting && this.IsFirstPerson(entity);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00061308 File Offset: 0x0005F508
		public static void SetFirstPersonHandsPitch(IClientPlayer player, float value)
		{
			EntityPlayerShapeRenderer renderer = player.Entity.Properties.Client.Renderer as EntityPlayerShapeRenderer;
			if (renderer == null)
			{
				return;
			}
			renderer.HeldItemPitchFollowOverride = new float?(0.8f * value);
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00061348 File Offset: 0x0005F548
		private void SetFov(float multiplier, bool equalizeFov = true)
		{
			ICoreClientAPI api = this._api;
			ClientMain client = ((api != null) ? api.World : null) as ClientMain;
			if (client == null)
			{
				return;
			}
			PlayerCamera camera = client.MainCamera;
			if (camera == null)
			{
				return;
			}
			float equalizeMultiplier = MathF.Sqrt((float)this._settingsFOV / (float)this._settingsHandsFOV);
			PlayerRenderingPatches.HandsFovMultiplier = multiplier * (equalizeFov ? equalizeMultiplier : 1f);
			camera.Fov = (float)this._settingsFOV * 0.017453292f * multiplier;
			FirstPersonAnimationsBehavior.CurrentFov = (float)this._settingsFOV * multiplier;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000613C6 File Offset: 0x0005F5C6
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

		// Token: 0x060013BD RID: 5053 RVA: 0x000613FC File Offset: 0x0005F5FC
		private void StopRequestFromPreviousItem(bool mainHand)
		{
			int currentItem = this.CurrentItemId(mainHand);
			for (int index = 0; index < this._playRequests.Count; index++)
			{
				if (this._playRequests[index].Item4 != currentItem)
				{
					this._playRequests[index] = new ValueTuple<AnimationRequest, bool, bool, int>(new AnimationRequest(), this._playRequests[index].Item2, true, -1);
				}
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00061464 File Offset: 0x0005F664
		private string GetIdleAnimationCategory(bool mainHand)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00061474 File Offset: 0x0005F674
		private void InHandItemChanged(bool mainHand)
		{
			(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Stop();
			string readyCategory = this.GetIdleAnimationCategory(mainHand);
			List<string> categories = mainHand ? this._mainHandCategories : this._offhandCategories;
			IEnumerable<string> enumerable = categories;
			Func<string, bool> <>9__0;
			Func<string, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((string element) => element != readyCategory));
			}
			foreach (string category in Enumerable.Where<string>(enumerable, func))
			{
				this._composer.Stop(category);
			}
			this.StopRequestFromPreviousItem(mainHand);
			categories.Clear();
			(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Start();
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00061548 File Offset: 0x0005F748
		private int CurrentItemId(bool mainHand)
		{
			int? num2;
			if (mainHand)
			{
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
				num2 = num;
				return num2.GetValueOrDefault();
			}
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
			return num2.GetValueOrDefault();
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000615E4 File Offset: 0x0005F7E4
		[NullableContext(2)]
		private Animation GetAnimationFromRequest(AnimationRequestByCode request)
		{
			if (this._animationsManager == null)
			{
				return null;
			}
			Animation animation;
			if (!this._animationsManager.GetAnimation(out animation, request.Animation, this._player, true))
			{
				LoggerUtil.Verbose(this._api, this, "Animation '" + request.Animation + "' was not found");
				return null;
			}
			return animation;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0006163C File Offset: 0x0005F83C
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			if (this._registeredDisposeHook)
			{
				this._system.OnDispose -= new Action(this.Dispose);
				this._registeredDisposeHook = false;
			}
			this._mainHandIdleAnimationsController.Stop();
			this._offHandIdleAnimationsController.Stop();
			this._composer.StopAll();
			this._playRequests.Clear();
			if (AnimationPatches.FirstPersonAnimationBehavior == this)
			{
				AnimationPatches.FirstPersonAnimationBehavior = null;
			}
			if (AnimationPatches.OwnerEntityId == this._player.EntityId)
			{
				AnimationPatches.OwnerEntityId = 0L;
			}
		}

		// Token: 0x040009A1 RID: 2465
		private readonly Composer _composer;

		// Token: 0x040009A2 RID: 2466
		private readonly EntityPlayer _player;

		// Token: 0x040009A3 RID: 2467
		private readonly AnimationsManager _animationsManager;

		// Token: 0x040009A4 RID: 2468
		private readonly VanillaAnimationsSystemClient _vanillaAnimationsManager;

		// Token: 0x040009A5 RID: 2469
		private readonly CombatOverhaulSystem _system;

		// Token: 0x040009A6 RID: 2470
		private PlayerItemFrame _lastFrame = PlayerItemFrame.Zero;

		// Token: 0x040009A7 RID: 2471
		private readonly List<string> _offhandCategories = new List<string>();

		// Token: 0x040009A8 RID: 2472
		private readonly List<string> _mainHandCategories = new List<string>();

		// Token: 0x040009A9 RID: 2473
		private readonly HashSet<string> _offhandVanillaAnimations = new HashSet<string>();

		// Token: 0x040009AA RID: 2474
		private readonly HashSet<string> _mainHandVanillaAnimations = new HashSet<string>();

		// Token: 0x040009AB RID: 2475
		private bool _mainPlayer;

		// Token: 0x040009AC RID: 2476
		private bool _registeredDisposeHook;

		// Token: 0x040009AD RID: 2477
		private bool _disposed;

		// Token: 0x040009AE RID: 2478
		private bool _reportedMainPlayerActivation;

		// Token: 0x040009AF RID: 2479
		private readonly Settings _settings;

		// Token: 0x040009B0 RID: 2480
		private readonly IdleAnimationsController _mainHandIdleAnimationsController;

		// Token: 0x040009B1 RID: 2481
		private readonly IdleAnimationsController _offHandIdleAnimationsController;

		// Token: 0x040009B2 RID: 2482
		[Nullable(2)]
		private ThirdPersonAnimationsBehavior _thirdPersonAnimations;

		// Token: 0x040009B3 RID: 2483
		private bool _frameApplied;

		// Token: 0x040009B4 RID: 2484
		private int _offHandItemId;

		// Token: 0x040009B5 RID: 2485
		private int _mainHandItemId;

		// Token: 0x040009B6 RID: 2486
		private bool _resetFov;

		// Token: 0x040009B7 RID: 2487
		private readonly ICoreClientAPI _api;

		// Token: 0x040009B8 RID: 2488
		[TupleElementNames(new string[]
		{
			"request",
			"mainHand",
			"skip",
			"itemId"
		})]
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private readonly List<ValueTuple<AnimationRequest, bool, bool, int>> _playRequests = new List<ValueTuple<AnimationRequest, bool, bool, int>>();

		// Token: 0x040009B9 RID: 2489
		private float _previousHeadBobbingAmplitudeFactor = 1f;

		// Token: 0x040009BA RID: 2490
		private int _settingsFOV = ClientSettings.FieldOfView;

		// Token: 0x040009BB RID: 2491
		private int _settingsHandsFOV = ClientSettings.FpHandsFoV;

		// Token: 0x040009BC RID: 2492
		private const float _settingsUpdatePeriodSec = 3f;

		// Token: 0x040009BD RID: 2493
		private float _settingsUpdateTimeSec;

		// Token: 0x040009BE RID: 2494
		[Nullable(2)]
		private Animatable _animatable;

		// Token: 0x040009BF RID: 2495
		private int _animatableItemId = -1;

		// Token: 0x040009C0 RID: 2496
		private Vector3 _eyePosition;

		// Token: 0x040009C1 RID: 2497
		private float _eyeHeight;

		// Token: 0x040009C2 RID: 2498
		private bool _immersiveFpModeSetting;

		// Token: 0x040009C3 RID: 2499
		private long _ownerEntityId;

		// Token: 0x040009C4 RID: 2500
		private bool _restIfpSetting;

		// Token: 0x040009C5 RID: 2501
		private long _lastAnimationAdvanceMs;

		// Token: 0x040009C6 RID: 2502
		private const long _mountedAnimationFallbackIntervalMs = 50L;
	}
}
