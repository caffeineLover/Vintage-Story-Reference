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
	// Token: 0x02000188 RID: 392
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FirstPersonAnimationsBehavior : EntityBehavior, IDisposable
	{
		// Token: 0x0600131C RID: 4892 RVA: 0x0005E7AC File Offset: 0x0005C9AC
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

		// Token: 0x0600131D RID: 4893 RVA: 0x0005E988 File Offset: 0x0005CB88
		public override string PropertyName()
		{
			return "CombatOverhaul:FirstPersonAnimations";
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0005E98F File Offset: 0x0005CB8F
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.Dispose();
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0005E99E File Offset: 0x0005CB9E
		public override void AfterInitialized(bool onFirstSpawn)
		{
			this.TryActivateMainPlayer();
			if (!this._mainPlayer)
			{
				return;
			}
			this.ResolveThirdPersonAnimations();
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0005E9B8 File Offset: 0x0005CBB8
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

		// Token: 0x06001321 RID: 4897 RVA: 0x0005EBE0 File Offset: 0x0005CDE0
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

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x0005EC98 File Offset: 0x0005CE98
		// (set) Token: 0x06001323 RID: 4899 RVA: 0x0005ECA0 File Offset: 0x0005CEA0
		public PlayerItemFrame? FrameOverride { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x0005ECAC File Offset: 0x0005CEAC
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

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x0005ECD8 File Offset: 0x0005CED8
		public bool HasActiveAnimationFrame
		{
			get
			{
				return this.FrameOverride != null || this._composer.AnyActiveAnimations();
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x0005ED02 File Offset: 0x0005CF02
		// (set) Token: 0x06001327 RID: 4903 RVA: 0x0005ED09 File Offset: 0x0005CF09
		public static float CurrentFov { get; set; } = (float)ClientSettings.FieldOfView;

		// Token: 0x06001328 RID: 4904 RVA: 0x0005ED14 File Offset: 0x0005CF14
		public void Play(AnimationRequest request, bool mainHand = true)
		{
			this.TryActivateMainPlayer();
			if (request.Category == this.GetIdleAnimationCategory(mainHand))
			{
				(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Pause();
			}
			this._playRequests.Add(new ValueTuple<AnimationRequest, bool, bool, int>(request, mainHand, false, this.CurrentItemId(mainHand)));
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0005ED6C File Offset: 0x0005CF6C
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

		// Token: 0x0600132A RID: 4906 RVA: 0x0005EDC8 File Offset: 0x0005CFC8
		public void Play(bool mainHand, string animation, string category = "main", float animationSpeed = 1f, float weight = 1f, [Nullable(2)] Func<bool> callback = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> callbackHandler = null, bool easeOut = true)
		{
			AnimationRequestByCode request = new AnimationRequestByCode(animation, animationSpeed, weight, category, TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1), easeOut, callback, callbackHandler);
			this.Play(request, mainHand);
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0005EE0C File Offset: 0x0005D00C
		public void PlayReadyAnimation(bool mainHand = true)
		{
			this.TryActivateMainPlayer();
			(mainHand ? this._mainHandIdleAnimationsController : this._offHandIdleAnimationsController).Start();
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0005EE2C File Offset: 0x0005D02C
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

		// Token: 0x0600132D RID: 4909 RVA: 0x0005EEA2 File Offset: 0x0005D0A2
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

		// Token: 0x0600132E RID: 4910 RVA: 0x0005EEE2 File Offset: 0x0005D0E2
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

		// Token: 0x0600132F RID: 4911 RVA: 0x0005EF14 File Offset: 0x0005D114
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

		// Token: 0x06001330 RID: 4912 RVA: 0x0005EF80 File Offset: 0x0005D180
		public void SetSpeedModifier(AnimationSpeedModifierDelegate modifier)
		{
			this._composer.SetSpeedModifier(modifier);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0005EF8E File Offset: 0x0005D18E
		public void StopSpeedModifier()
		{
			this._composer.StopSpeedModifier();
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0005EF9B File Offset: 0x0005D19B
		public bool IsSpeedModifierActive()
		{
			return this._composer.IsSpeedModifierActive();
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0005EFA8 File Offset: 0x0005D1A8
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

		// Token: 0x06001334 RID: 4916 RVA: 0x0005F028 File Offset: 0x0005D228
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

		// Token: 0x06001335 RID: 4917 RVA: 0x0005F08E File Offset: 0x0005D28E
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

		// Token: 0x06001336 RID: 4918 RVA: 0x0005F0B8 File Offset: 0x0005D2B8
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

		// Token: 0x06001337 RID: 4919 RVA: 0x0005F140 File Offset: 0x0005D340
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

		// Token: 0x06001338 RID: 4920 RVA: 0x0005F23C File Offset: 0x0005D43C
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

		// Token: 0x06001339 RID: 4921 RVA: 0x0005F2FC File Offset: 0x0005D4FC
		private void ResolveThirdPersonAnimations()
		{
			if (this._thirdPersonAnimations == null)
			{
				this._thirdPersonAnimations = this.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
			}
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0005F317 File Offset: 0x0005D517
		internal void OnBeforeFrame(Entity targetEntity, float dt)
		{
			if (!FirstPersonAnimationsBehavior.IsOwner(targetEntity))
			{
				return;
			}
			this.AdvanceAnimationFrame(targetEntity, dt, true);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0005F32C File Offset: 0x0005D52C
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

		// Token: 0x0600133C RID: 4924 RVA: 0x0005F380 File Offset: 0x0005D580
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

		// Token: 0x0600133D RID: 4925 RVA: 0x0005F62C File Offset: 0x0005D82C
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

		// Token: 0x0600133E RID: 4926 RVA: 0x0005F670 File Offset: 0x0005D870
		private void ApplyFrame(PlayerItemFrame frame, ElementPose pose, AnimatorBase animator, bool clearPose = false)
		{
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
				frame.Apply(pose, element, this._eyePosition, this._eyeHeight, 0f, false, true);
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
				frame.Apply(extendedPoseValue, element, this._eyePosition, this._eyeHeight, 0f, false, true);
			}
			else
			{
				frame.Apply(pose, element, this._eyePosition, this._eyeHeight, 0f, false, true);
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

		// Token: 0x0600133F RID: 4927 RVA: 0x0005F7CC File Offset: 0x0005D9CC
		private static bool IsOwner(Entity entity)
		{
			ICoreClientAPI coreClientAPI = entity.Api as ICoreClientAPI;
			long? num = (coreClientAPI != null) ? new long?(coreClientAPI.World.Player.Entity.EntityId) : default(long?);
			long entityId = entity.EntityId;
			return num.GetValueOrDefault() == entityId & num != null;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0005F828 File Offset: 0x0005DA28
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

		// Token: 0x06001341 RID: 4929 RVA: 0x0005F875 File Offset: 0x0005DA75
		private bool IsImmersiveFirstPerson(Entity entity)
		{
			return this._immersiveFpModeSetting && this.IsFirstPerson(entity);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0005F888 File Offset: 0x0005DA88
		public static void SetFirstPersonHandsPitch(IClientPlayer player, float value)
		{
			EntityPlayerShapeRenderer renderer = player.Entity.Properties.Client.Renderer as EntityPlayerShapeRenderer;
			if (renderer == null)
			{
				return;
			}
			renderer.HeldItemPitchFollowOverride = new float?(0.8f * value);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0005F8C8 File Offset: 0x0005DAC8
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

		// Token: 0x06001344 RID: 4932 RVA: 0x0005F946 File Offset: 0x0005DB46
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

		// Token: 0x06001345 RID: 4933 RVA: 0x0005F97C File Offset: 0x0005DB7C
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

		// Token: 0x06001346 RID: 4934 RVA: 0x0005F9E4 File Offset: 0x0005DBE4
		private string GetIdleAnimationCategory(bool mainHand)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0005F9F4 File Offset: 0x0005DBF4
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

		// Token: 0x06001348 RID: 4936 RVA: 0x0005FAC8 File Offset: 0x0005DCC8
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

		// Token: 0x06001349 RID: 4937 RVA: 0x0005FB64 File Offset: 0x0005DD64
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

		// Token: 0x0600134A RID: 4938 RVA: 0x0005FBBC File Offset: 0x0005DDBC
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

		// Token: 0x04000979 RID: 2425
		private readonly Composer _composer;

		// Token: 0x0400097A RID: 2426
		private readonly EntityPlayer _player;

		// Token: 0x0400097B RID: 2427
		private readonly AnimationsManager _animationsManager;

		// Token: 0x0400097C RID: 2428
		private readonly VanillaAnimationsSystemClient _vanillaAnimationsManager;

		// Token: 0x0400097D RID: 2429
		private readonly CombatOverhaulSystem _system;

		// Token: 0x0400097E RID: 2430
		private PlayerItemFrame _lastFrame = PlayerItemFrame.Zero;

		// Token: 0x0400097F RID: 2431
		private readonly List<string> _offhandCategories = new List<string>();

		// Token: 0x04000980 RID: 2432
		private readonly List<string> _mainHandCategories = new List<string>();

		// Token: 0x04000981 RID: 2433
		private readonly HashSet<string> _offhandVanillaAnimations = new HashSet<string>();

		// Token: 0x04000982 RID: 2434
		private readonly HashSet<string> _mainHandVanillaAnimations = new HashSet<string>();

		// Token: 0x04000983 RID: 2435
		private bool _mainPlayer;

		// Token: 0x04000984 RID: 2436
		private bool _registeredDisposeHook;

		// Token: 0x04000985 RID: 2437
		private bool _disposed;

		// Token: 0x04000986 RID: 2438
		private bool _reportedMainPlayerActivation;

		// Token: 0x04000987 RID: 2439
		private readonly Settings _settings;

		// Token: 0x04000988 RID: 2440
		private readonly IdleAnimationsController _mainHandIdleAnimationsController;

		// Token: 0x04000989 RID: 2441
		private readonly IdleAnimationsController _offHandIdleAnimationsController;

		// Token: 0x0400098A RID: 2442
		[Nullable(2)]
		private ThirdPersonAnimationsBehavior _thirdPersonAnimations;

		// Token: 0x0400098B RID: 2443
		private bool _frameApplied;

		// Token: 0x0400098C RID: 2444
		private int _offHandItemId;

		// Token: 0x0400098D RID: 2445
		private int _mainHandItemId;

		// Token: 0x0400098E RID: 2446
		private bool _resetFov;

		// Token: 0x0400098F RID: 2447
		private readonly ICoreClientAPI _api;

		// Token: 0x04000990 RID: 2448
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

		// Token: 0x04000991 RID: 2449
		private float _previousHeadBobbingAmplitudeFactor = 1f;

		// Token: 0x04000992 RID: 2450
		private int _settingsFOV = ClientSettings.FieldOfView;

		// Token: 0x04000993 RID: 2451
		private int _settingsHandsFOV = ClientSettings.FpHandsFoV;

		// Token: 0x04000994 RID: 2452
		private const float _settingsUpdatePeriodSec = 3f;

		// Token: 0x04000995 RID: 2453
		private float _settingsUpdateTimeSec;

		// Token: 0x04000996 RID: 2454
		[Nullable(2)]
		private Animatable _animatable;

		// Token: 0x04000997 RID: 2455
		private int _animatableItemId = -1;

		// Token: 0x04000998 RID: 2456
		private Vector3 _eyePosition;

		// Token: 0x04000999 RID: 2457
		private float _eyeHeight;

		// Token: 0x0400099A RID: 2458
		private bool _immersiveFpModeSetting;

		// Token: 0x0400099B RID: 2459
		private long _ownerEntityId;

		// Token: 0x0400099C RID: 2460
		private bool _restIfpSetting;

		// Token: 0x0400099D RID: 2461
		private long _lastAnimationAdvanceMs;

		// Token: 0x0400099E RID: 2462
		private const long _mountedAnimationFallbackIntervalMs = 50L;
	}
}
