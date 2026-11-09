using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Colliders;
using CombatOverhaul.Integration.Transpilers;
using CombatOverhaul.Utils;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	internal static class AnimationPatches
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000386 RID: 902 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		// (remove) Token: 0x06000387 RID: 903 RVA: 0x0001002C File Offset: 0x0000E22C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static event Action<Entity, float> OnBeforeFrame;

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0001005F File Offset: 0x0000E25F
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00010066 File Offset: 0x0000E266
		public static Settings ClientSettings { get; set; } = new Settings();

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0001006E File Offset: 0x0000E26E
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00010075 File Offset: 0x0000E275
		public static Settings ServerSettings { get; set; } = new Settings();

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0001007D File Offset: 0x0000E27D
		public static Dictionary<long, ThirdPersonAnimationsBehavior> AnimationBehaviors { get; } = new Dictionary<long, ThirdPersonAnimationsBehavior>();

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00010084 File Offset: 0x0000E284
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0001008B File Offset: 0x0000E28B
		[Nullable(2)]
		public static FirstPersonAnimationsBehavior FirstPersonAnimationBehavior { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00010093 File Offset: 0x0000E293
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0001009A File Offset: 0x0000E29A
		public static long OwnerEntityId { get; set; } = 0L;

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000391 RID: 913 RVA: 0x000100A2 File Offset: 0x0000E2A2
		// (set) Token: 0x06000392 RID: 914 RVA: 0x000100A9 File Offset: 0x0000E2A9
		public static HashSet<long> ActiveEntities { get; set; } = new HashSet<long>();

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000393 RID: 915 RVA: 0x000100B1 File Offset: 0x0000E2B1
		// (set) Token: 0x06000394 RID: 916 RVA: 0x000100B8 File Offset: 0x0000E2B8
		[Nullable(2)]
		public static AnimationPatches.AnimatorPlayerMap Animators { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x06000395 RID: 917 RVA: 0x000100C0 File Offset: 0x0000E2C0
		public static void Patch(string harmonyId, ICoreAPI api)
		{
			AnimationPatches.Animators = new AnimationPatches.AnimatorPlayerMap();
			Harmony harmony = new Harmony(harmonyId);
			harmony.Patch(typeof(EntityShapeRenderer).GetMethod("RenderHeldItem", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(AnimationPatches), "RenderHeldItem", null, null)), null, null, null);
			harmony.Patch(typeof(EntityShapeRenderer).GetMethod("DoRender3DOpaque", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(AnimationPatches), "DoRender3DOpaque", null, null)), null, null, null);
			harmony.Patch(typeof(EntityPlayerShapeRenderer).GetMethod("DoRender3DOpaque", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(AnimationPatches), "DoRender3DOpaquePlayer", null, null)), null, null, null);
			harmony.Patch(typeof(EntityShapeRenderer).GetMethod("BeforeRender", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(AnimationPatches), "BeforeRender", null, null)), null, null, null);
			harmony.Patch(typeof(EntityPlayer).GetMethod("OnSelfBeforeRender", AccessTools.all), null, new HarmonyMethod(AccessTools.Method(typeof(AnimationPatches), "OnSelfBeforeRender", null, null)), null, null);
			harmony.Patch(typeof(AnimationManager).GetMethod("OnClientFrame", AccessTools.all), null, new HarmonyMethod(AccessTools.Method(typeof(AnimationPatches), "AnimationManagerOnClientFrame", null, null)), null, null);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00010250 File Offset: 0x0000E450
		public static void Unpatch(string harmonyId, ICoreAPI api)
		{
			Harmony harmony = new Harmony(harmonyId);
			harmony.Unpatch(typeof(EntityShapeRenderer).GetMethod("RenderHeldItem", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(EntityShapeRenderer).GetMethod("DoRender3DOpaque", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(EntityPlayerShapeRenderer).GetMethod("DoRender3DOpaque", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(EntityShapeRenderer).GetMethod("BeforeRender", AccessTools.all), 1, harmonyId);
			harmony.Unpatch(typeof(EntityPlayer).GetMethod("OnSelfBeforeRender", AccessTools.all), 2, harmonyId);
			harmony.Unpatch(typeof(AnimationManager).GetMethod("OnClientFrame", AccessTools.all), 2, harmonyId);
			AnimationPatches.AnimationBehaviors.Clear();
			AnimationPatches.ActiveEntities.Clear();
			AnimationPatches._heldItemAnimatableCache.Clear();
			AnimationPatches.AnimatorPlayerMap animators = AnimationPatches.Animators;
			if (animators != null)
			{
				animators.Clear();
			}
			AnimationPatches.Animators = null;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001035C File Offset: 0x0000E55C
		[return: Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		private static Func<EntityShapeRenderer, Vec4f> BuildLightRgbsGetter()
		{
			if (AnimationPatches._lightrgbsField == null)
			{
				return null;
			}
			Func<EntityShapeRenderer, Vec4f> result;
			try
			{
				ParameterExpression instance;
				result = Expression.Lambda<Func<EntityShapeRenderer, Vec4f>>(Expression.Field(instance, AnimationPatches._lightrgbsField), new ParameterExpression[]
				{
					instance
				}).Compile();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000103C8 File Offset: 0x0000E5C8
		[return: Nullable(2)]
		private static CollidersEntityBehavior GetCachedColliderBehavior(EntityShapeRenderer renderer)
		{
			if (renderer.entity == null)
			{
				return null;
			}
			return AnimationPatches._colliderBehaviorCache.GetValue(renderer, delegate(EntityShapeRenderer r)
			{
				Entity entity = r.entity;
				return new StrongBox<CollidersEntityBehavior>((entity != null) ? entity.GetBehavior<CollidersEntityBehavior>() : null);
			}).Value;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00010404 File Offset: 0x0000E604
		[MethodImpl(256)]
		public static void OnFrameInvoke([Nullable(2)] ClientAnimator animator, ElementPose pose)
		{
			if (AnimationPatches.ClientSettings.DisableAllAnimations || animator == null || AnimationPatches.Animators == null)
			{
				return;
			}
			ExtendedElementPose extendedPose = pose as ExtendedElementPose;
			if (extendedPose != null && extendedPose.Player != null)
			{
				ThirdPersonAnimationsBehavior behavior;
				if (extendedPose.ElementNameEnum != EnumAnimatedElement.Unknown && !AnimationPatches.ClientSettings.DisableThirdPersonAnimations && AnimationPatches.AnimationBehaviors.TryGetValue(extendedPose.Player.EntityId, ref behavior))
				{
					behavior.OnFrame(extendedPose.Player, pose, animator);
				}
				if (extendedPose.ElementNameEnum != EnumAnimatedElement.Unknown && extendedPose.Player.EntityId == AnimationPatches.OwnerEntityId)
				{
					FirstPersonAnimationsBehavior firstPersonAnimationBehavior = AnimationPatches.FirstPersonAnimationBehavior;
					if (firstPersonAnimationBehavior != null)
					{
						firstPersonAnimationBehavior.OnFrame(extendedPose.Player, pose, animator);
					}
				}
				if (extendedPose.ElementNameEnum != EnumAnimatedElement.Unknown)
				{
					return;
				}
			}
			EntityPlayer entity;
			if (AnimationPatches.Animators.Get(animator, out entity))
			{
				ThirdPersonAnimationsBehavior behavior2;
				if (!AnimationPatches.ClientSettings.DisableThirdPersonAnimations && AnimationPatches.AnimationBehaviors.TryGetValue(entity.EntityId, ref behavior2))
				{
					behavior2.OnFrame(entity, pose, animator);
				}
				if (entity.EntityId == AnimationPatches.OwnerEntityId)
				{
					FirstPersonAnimationsBehavior firstPersonAnimationBehavior2 = AnimationPatches.FirstPersonAnimationBehavior;
					if (firstPersonAnimationBehavior2 != null)
					{
						firstPersonAnimationBehavior2.OnFrame(entity, pose, animator);
					}
				}
				ExtendedElementPose extendedPose2 = pose as ExtendedElementPose;
				if (extendedPose2 != null && extendedPose2.Player == null)
				{
					extendedPose2.Player = entity;
				}
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00010528 File Offset: 0x0000E728
		private static void BeforeRender(EntityShapeRenderer __instance, float dt)
		{
			if (AnimationPatches.ClientSettings.DisableAllAnimations)
			{
				return;
			}
			EntityPlayer player = __instance.entity as EntityPlayer;
			if (player != null && AnimationPatches.IsLocalPlayer(player))
			{
				return;
			}
			AnimationPatches.DispatchBeforeFrame(__instance.entity, dt);
			Action<Entity, float> onBeforeFrame = AnimationPatches.OnBeforeFrame;
			if (onBeforeFrame == null)
			{
				return;
			}
			onBeforeFrame.Invoke(__instance.entity, dt);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001057C File Offset: 0x0000E77C
		private static void OnSelfBeforeRender(EntityPlayer __instance, float dt)
		{
			if (AnimationPatches.ClientSettings.DisableAllAnimations)
			{
				return;
			}
			AnimationPatches.DispatchBeforeFrame(__instance, dt);
			Action<Entity, float> onBeforeFrame = AnimationPatches.OnBeforeFrame;
			if (onBeforeFrame == null)
			{
				return;
			}
			onBeforeFrame.Invoke(__instance, dt);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000105A4 File Offset: 0x0000E7A4
		private static void DispatchBeforeFrame(Entity entity, float dt)
		{
			EntityPlayer player = entity as EntityPlayer;
			if (player == null)
			{
				return;
			}
			ThirdPersonAnimationsBehavior thirdPersonBehavior;
			if (!AnimationPatches.ClientSettings.DisableThirdPersonAnimations && AnimationPatches.AnimationBehaviors.TryGetValue(player.EntityId, ref thirdPersonBehavior))
			{
				thirdPersonBehavior.OnBeforeFrame(entity, dt);
			}
			if (player.EntityId == AnimationPatches.OwnerEntityId)
			{
				FirstPersonAnimationsBehavior firstPersonAnimationBehavior = AnimationPatches.FirstPersonAnimationBehavior;
				if (firstPersonAnimationBehavior == null)
				{
					return;
				}
				firstPersonAnimationBehavior.OnBeforeFrame(entity, dt);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00010602 File Offset: 0x0000E802
		private static void AnimationManagerOnClientFrame(AnimationManager __instance, float dt)
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00010604 File Offset: 0x0000E804
		private static void DoRender3DOpaque(EntityShapeRenderer __instance, float dt, bool isShadowPass)
		{
			try
			{
				CollidersEntityBehavior cachedColliderBehavior = AnimationPatches.GetCachedColliderBehavior(__instance);
				if (cachedColliderBehavior != null)
				{
					Entity entity = __instance.entity;
					cachedColliderBehavior.Render(((entity != null) ? entity.Api : null) as ICoreClientAPI, __instance.entity, __instance, -1);
				}
			}
			catch (Exception exception)
			{
				Entity entity2 = __instance.entity;
				AnimationPatches.LogColliderRenderError((entity2 != null) ? entity2.Api : null, exception);
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00010670 File Offset: 0x0000E870
		private static void DoRender3DOpaquePlayer(EntityPlayerShapeRenderer __instance, float dt, bool isShadowPass)
		{
			try
			{
				CollidersEntityBehavior cachedColliderBehavior = AnimationPatches.GetCachedColliderBehavior(__instance);
				if (cachedColliderBehavior != null)
				{
					Entity entity = __instance.entity;
					cachedColliderBehavior.Render(((entity != null) ? entity.Api : null) as ICoreClientAPI, __instance.entity, __instance, -1);
				}
			}
			catch (Exception exception)
			{
				Entity entity2 = __instance.entity;
				AnimationPatches.LogColliderRenderError((entity2 != null) ? entity2.Api : null, exception);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000106DC File Offset: 0x0000E8DC
		private static bool RenderHeldItem(EntityShapeRenderer __instance, float dt, bool isShadowPass, bool right)
		{
			EntityPlayer player = __instance.entity as EntityPlayer;
			ItemSlot slot = right ? ((player != null) ? player.RightHandItemSlot : null) : ((player != null) ? player.LeftHandItemSlot : null);
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
			if (player != null && AnimationPatches.IsTongsHeldItemRender(player, right))
			{
				return true;
			}
			int itemId = slot.Itemstack.Item.Id;
			Animatable behavior;
			if (!AnimationPatches._heldItemAnimatableCache.TryGetValue(itemId, ref behavior))
			{
				behavior = (slot.Itemstack.Item.GetCollectibleBehavior(typeof(Animatable), true) as Animatable);
				AnimationPatches._heldItemAnimatableCache[itemId] = behavior;
			}
			if (behavior == null)
			{
				return true;
			}
			EnumItemRenderTarget renderTarget = right ? 2 : 3;
			ItemRenderInfo renderInfo = __instance.capi.Render.GetItemStackRenderInfo(slot, renderTarget, dt);
			behavior.BeforeRender(__instance.capi, slot.Itemstack, __instance.entity, 1, dt);
			if (slot.Itemstack.Item.Textures.Count > 0)
			{
				using (Dictionary<string, CompositeTexture>.Enumerator enumerator = slot.Itemstack.Item.Textures.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<string, CompositeTexture> keyValuePair = enumerator.Current;
						string text;
						CompositeTexture compositeTexture;
						keyValuePair.Deconstruct(ref text, ref compositeTexture);
						string textureName = text;
						TextureAtlasPosition atlasPos = __instance.capi.ItemTextureAtlas.GetPosition(slot.Itemstack.Item, textureName, false);
						renderInfo.TextureId = atlasPos.atlasTextureId;
					}
				}
			}
			Vec4f vec4f;
			if (AnimationPatches._getLightRgbs == null)
			{
				FieldInfo lightrgbsField = AnimationPatches._lightrgbsField;
				vec4f = (Vec4f)((lightrgbsField != null) ? lightrgbsField.GetValue(__instance) : null);
			}
			else
			{
				vec4f = AnimationPatches._getLightRgbs.Invoke(__instance);
			}
			Vec4f lightrgbs = vec4f;
			bool result;
			try
			{
				behavior.AttachmentPointOverride = AnimationPatches.GetHeldItemAttachmentPointOverride(player, right);
				result = !behavior.RenderHeldItem(__instance.ModelMat, __instance.capi, slot, __instance.entity, lightrgbs, dt, isShadowPass, right, renderInfo, renderTarget);
			}
			catch (Exception exception)
			{
				if (!AnimationPatches._reportedHeldItemRenderError)
				{
					AnimationPatches._reportedHeldItemRenderError = true;
					Entity entity = __instance.entity;
					ICoreAPI api = (entity != null) ? entity.Api : null;
					Type typeFromHandle = typeof(AnimationPatches);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(49, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Error while rendering animated held item for '");
					CollectibleObject collectible = slot.Itemstack.Collectible;
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>((collectible != null) ? collectible.Code : null);
					defaultInterpolatedStringHandler.AppendLiteral("':\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00010964 File Offset: 0x0000EB64
		private static void LogColliderRenderError([Nullable(2)] ICoreAPI api, Exception exception)
		{
			if (AnimationPatches._reportedColliderRenderError)
			{
				return;
			}
			AnimationPatches._reportedColliderRenderError = true;
			Type typeFromHandle = typeof(AnimationPatches);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(46, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error while rendering collider debug overlay:\n");
			defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
			LoggerUtil.Warn(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000109B4 File Offset: 0x0000EBB4
		private static bool IsTongsHeldItemRender(EntityPlayer player, bool right)
		{
			ItemSlot rightHandItemSlot = player.RightHandItemSlot;
			ItemStack stack = (rightHandItemSlot != null) ? rightHandItemSlot.Itemstack : null;
			ItemSlot leftHandItemSlot = player.LeftHandItemSlot;
			ItemStack leftStack = (leftHandItemSlot != null) ? leftHandItemSlot.Itemstack : null;
			bool flag = AnimationPatches.IsTongsStack(stack);
			bool leftIsTongs = AnimationPatches.IsTongsStack(leftStack);
			return flag || leftIsTongs;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000109F9 File Offset: 0x0000EBF9
		[NullableContext(2)]
		private static bool IsTongsStack(ItemStack stack)
		{
			return CollectibleClassifier.IsTongs(stack);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00010A04 File Offset: 0x0000EC04
		[NullableContext(2)]
		private static string GetHeldItemAttachmentPointOverride(EntityPlayer player, bool right)
		{
			if (player == null)
			{
				return null;
			}
			PlayerItemFrame? frame = default(PlayerItemFrame?);
			if (AnimationPatches.IsLocalFirstPerson(player))
			{
				FirstPersonAnimationsBehavior firstPersonAnimationBehavior = AnimationPatches.FirstPersonAnimationBehavior;
				if (firstPersonAnimationBehavior != null && firstPersonAnimationBehavior.HasActiveAnimationFrame)
				{
					frame = new PlayerItemFrame?(AnimationPatches.FirstPersonAnimationBehavior.CurrentFrame);
					goto IL_70;
				}
			}
			ThirdPersonAnimationsBehavior thirdPersonBehavior;
			if (!AnimationPatches.ClientSettings.DisableThirdPersonAnimations && AnimationPatches.AnimationBehaviors.TryGetValue(player.EntityId, ref thirdPersonBehavior) && thirdPersonBehavior.HasActiveAnimationFrame)
			{
				frame = new PlayerItemFrame?(thirdPersonBehavior.CurrentFrame);
			}
			IL_70:
			if (frame == null)
			{
				return null;
			}
			if (frame.Value.DetachedAnchor)
			{
				return "DetachedAnchor";
			}
			if (!frame.Value.SwitchArms)
			{
				return null;
			}
			if (!right)
			{
				return "RightHand";
			}
			return "LeftHand";
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		private static bool IsLocalPlayer(EntityPlayer player)
		{
			ICoreClientAPI clientApi = player.Api as ICoreClientAPI;
			if (clientApi != null)
			{
				IClientWorldAccessor world = clientApi.World;
				long? num;
				if (world == null)
				{
					num = default(long?);
				}
				else
				{
					IClientPlayer player2 = world.Player;
					if (player2 == null)
					{
						num = default(long?);
					}
					else
					{
						EntityPlayer entity = player2.Entity;
						num = ((entity != null) ? new long?(entity.EntityId) : default(long?));
					}
				}
				long? num2 = num;
				long entityId = player.EntityId;
				return num2.GetValueOrDefault() == entityId & num2 != null;
			}
			return false;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00010B40 File Offset: 0x0000ED40
		private static bool IsLocalFirstPerson(EntityPlayer player)
		{
			ICoreClientAPI clientApi = player.Api as ICoreClientAPI;
			if (clientApi != null)
			{
				IClientWorldAccessor world = clientApi.World;
				long? num;
				if (world == null)
				{
					num = default(long?);
				}
				else
				{
					IClientPlayer player2 = world.Player;
					if (player2 == null)
					{
						num = default(long?);
					}
					else
					{
						EntityPlayer entity = player2.Entity;
						num = ((entity != null) ? new long?(entity.EntityId) : default(long?));
					}
				}
				long? num2 = num;
				long entityId = player.EntityId;
				if (num2.GetValueOrDefault() == entityId & num2 != null)
				{
					return clientApi.World.Player.CameraMode == 0;
				}
			}
			return false;
		}

		// Token: 0x0400013E RID: 318
		[Nullable(2)]
		private static readonly FieldInfo _lightrgbsField = typeof(EntityShapeRenderer).GetField("lightrgbs", 36);

		// Token: 0x0400013F RID: 319
		[Nullable(new byte[]
		{
			2,
			1,
			2
		})]
		private static readonly Func<EntityShapeRenderer, Vec4f> _getLightRgbs = AnimationPatches.BuildLightRgbsGetter();

		// Token: 0x04000140 RID: 320
		[Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})]
		private static readonly ConditionalWeakTable<EntityShapeRenderer, StrongBox<CollidersEntityBehavior>> _colliderBehaviorCache = new ConditionalWeakTable<EntityShapeRenderer, StrongBox<CollidersEntityBehavior>>();

		// Token: 0x04000141 RID: 321
		[Nullable(new byte[]
		{
			1,
			2
		})]
		private static readonly Dictionary<int, Animatable> _heldItemAnimatableCache = new Dictionary<int, Animatable>();

		// Token: 0x04000142 RID: 322
		private static bool _reportedColliderRenderError;

		// Token: 0x04000143 RID: 323
		private static bool _reportedHeldItemRenderError;

		// Token: 0x04000144 RID: 324
		[Nullable(2)]
		private static readonly FieldInfo _animationManagerEntity = typeof(AnimationManager).GetField("entity", 36);

		// Token: 0x020001D1 RID: 465
		[NullableContext(0)]
		private enum HeldItemAttachmentMode
		{
			// Token: 0x04000AE1 RID: 2785
			Normal,
			// Token: 0x04000AE2 RID: 2786
			SwitchArms,
			// Token: 0x04000AE3 RID: 2787
			DetachedAnchor
		}

		// Token: 0x020001D2 RID: 466
		[Nullable(0)]
		public sealed class AnimatorPlayerMap
		{
			// Token: 0x0600153D RID: 5437 RVA: 0x00068844 File Offset: 0x00066A44
			public void Add(ClientAnimator animator, EntityPlayer player)
			{
				EntityPlayer existing;
				if (this._mapping.TryGetValue(animator, ref existing) && existing.EntityId == player.EntityId)
				{
					return;
				}
				this._mapping.AddOrUpdate(animator, player);
			}

			// Token: 0x0600153E RID: 5438 RVA: 0x0006887D File Offset: 0x00066A7D
			public bool Get(ClientAnimator animator, [Nullable(2)] out EntityPlayer player)
			{
				return this._mapping.TryGetValue(animator, ref player);
			}

			// Token: 0x0600153F RID: 5439 RVA: 0x0006888C File Offset: 0x00066A8C
			public void Clear()
			{
				this._mapping.Clear();
			}

			// Token: 0x04000AE4 RID: 2788
			private readonly ConditionalWeakTable<ClientAnimator, EntityPlayer> _mapping = new ConditionalWeakTable<ClientAnimator, EntityPlayer>();
		}
	}
}
