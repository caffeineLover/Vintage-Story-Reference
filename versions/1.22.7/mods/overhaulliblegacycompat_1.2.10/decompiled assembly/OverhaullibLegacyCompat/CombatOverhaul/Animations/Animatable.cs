using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019D RID: 413
	[NullableContext(1)]
	[Nullable(0)]
	public class Animatable : CollectibleBehavior
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x00062044 File Offset: 0x00060244
		[Nullable(2)]
		public Shape CurrentShape
		{
			[NullableContext(2)]
			get
			{
				AnimatableShape currentAnimatableShape = this.CurrentAnimatableShape;
				if (currentAnimatableShape == null)
				{
					return null;
				}
				return currentAnimatableShape.Shape;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x00062057 File Offset: 0x00060257
		[Nullable(2)]
		public AnimatableShape CurrentAnimatableShape
		{
			[NullableContext(2)]
			get
			{
				AnimatableShape result;
				if ((result = ((this.CurrentFirstPerson || this.SwitchModels) ? this.ShapeFirstPerson : this.Shape)) == null)
				{
					result = (this.Shape ?? this.ShapeFirstPerson);
				}
				return result;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x0006208B File Offset: 0x0006028B
		[Nullable(2)]
		public Shape FirstPersonShape
		{
			[NullableContext(2)]
			get
			{
				AnimatableShape shapeFirstPerson = this.ShapeFirstPerson;
				if (shapeFirstPerson == null)
				{
					return null;
				}
				return shapeFirstPerson.Shape;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0006209E File Offset: 0x0006029E
		[Nullable(2)]
		public Shape ThirdPersonShape
		{
			[NullableContext(2)]
			get
			{
				AnimatableShape shape = this.Shape;
				if (shape == null)
				{
					return null;
				}
				return shape.Shape;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x000620B1 File Offset: 0x000602B1
		// (set) Token: 0x060013F0 RID: 5104 RVA: 0x000620B9 File Offset: 0x000602B9
		public bool DetachedAnchor { get; set; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x000620C2 File Offset: 0x000602C2
		// (set) Token: 0x060013F2 RID: 5106 RVA: 0x000620CA File Offset: 0x000602CA
		public bool SwitchArms { get; set; }

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x000620D3 File Offset: 0x000602D3
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x000620DB File Offset: 0x000602DB
		public bool SwitchModels { get; protected set; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x000620E4 File Offset: 0x000602E4
		// (set) Token: 0x060013F6 RID: 5110 RVA: 0x000620EC File Offset: 0x000602EC
		[Nullable(2)]
		public string AttachmentPointOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x060013F7 RID: 5111 RVA: 0x000620F8 File Offset: 0x000602F8
		public Animatable(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00062150 File Offset: 0x00060350
		public override void Initialize(JsonObject properties)
		{
			this.AnimatedShapePath = properties["animated-shape"].AsString(null);
			this.AnimatedShapeFirstPersonPath = properties["animated-shape-fp"].AsString(null);
			JsonObject[] ShapeAlternatesPathsUnsorted = properties["animated-shape-alternates"].AsArray();
			JsonObject[] ShapeAlternatesFirstPersonPathsUnsorted = properties["animated-shape-fp-alternates"].AsArray();
			if (ShapeAlternatesPathsUnsorted != null)
			{
				foreach (JsonObject shapePath in ShapeAlternatesPathsUnsorted)
				{
					this.ShapeAlternatesPaths.Add(shapePath.AsString(null));
				}
			}
			if (ShapeAlternatesFirstPersonPathsUnsorted != null)
			{
				foreach (JsonObject shapePath2 in ShapeAlternatesFirstPersonPathsUnsorted)
				{
					this.ShapeAlternatesFirstPersonPaths.Add(shapePath2.AsString(null));
				}
			}
			base.Initialize(properties);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0006220C File Offset: 0x0006040C
		public override void OnLoaded(ICoreAPI api)
		{
			this.ModSystem = api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true);
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi != null)
			{
				if (!(this.collObj is Item))
				{
					throw new InvalidOperationException("CollectibleBehavior Animatable can only be used on Items, not Blocks!");
				}
				this.ClientApi = clientApi;
				this.InitAnimatable();
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0006225C File Offset: 0x0006045C
		public virtual void BeforeRender(ICoreClientAPI clientApi, ItemStack itemStack, Entity player, EnumItemRenderTarget target, float dt)
		{
			this.CurrentFirstPerson = Animatable.IsFirstPerson(player);
			if (this.CurrentAnimatableShape != null)
			{
				AnimatableShape currentShape = this.GetCurrentShape(itemStack);
				this.CalculateAnimation((currentShape != null) ? currentShape.GetAnimator(player.EntityId) : null, this.CurrentAnimatableShape.Shape, clientApi, player, target, dt);
			}
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x000622B0 File Offset: 0x000604B0
		public bool RenderHeldItem(float[] modelMat, ICoreClientAPI api, ItemSlot itemSlot, Entity entity, Vec4f lightrgbs, float dt, bool isShadowPass, bool right, ItemRenderInfo renderInfo, EnumItemRenderTarget renderTarget)
		{
			if (this.CurrentAnimatableShape != null && itemSlot.Itemstack != null)
			{
				CombatOverhaulAnimationsSystem modSystem = this.ModSystem;
				if (((modSystem != null) ? modSystem.AnimatedItemShaderProgram : null) != null)
				{
					ItemRenderInfo itemStackRenderInfo = this.PrepareShape(api, this.ItemModelMat, modelMat, itemSlot, entity, right, dt);
					if (itemStackRenderInfo == null)
					{
						return false;
					}
					if (!isShadowPass)
					{
						IShaderProgram shader = this.CurrentFirstPerson ? this.ModSystem.AnimatedItemShaderProgramFirstPerson : this.ModSystem.AnimatedItemShaderProgram;
						if (shader == null)
						{
							return false;
						}
						CollectibleObject collectible = itemSlot.Itemstack.Collectible;
						IMeshReplacer meshReplacer = (collectible != null) ? collectible.GetCollectibleInterface<IMeshReplacer>() : null;
						if (meshReplacer != null)
						{
							this.RenderShape(shader, api.World, this.CurrentAnimatableShape, itemStackRenderInfo, api.Render, itemSlot.Itemstack, lightrgbs, this.ItemModelMat, itemSlot, entity, dt, meshReplacer.GetReplacement(api, itemSlot, entity));
						}
						else
						{
							this.RenderShape(shader, api.World, this.CurrentAnimatableShape, itemStackRenderInfo, api.Render, itemSlot.Itemstack, lightrgbs, this.ItemModelMat, itemSlot, entity, dt, null);
						}
					}
					return !AnimatableShape.HadRenderErrorLastCall;
				}
			}
			return false;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000623B8 File Offset: 0x000605B8
		protected virtual void InitAnimatable()
		{
			Item item = this.collObj as Item;
			if (item == null || this.ClientApi == null || (item.Shape == null && this.AnimatedShapePath == null && this.AnimatedShapeFirstPersonPath == null))
			{
				return;
			}
			ICoreClientAPI clientApi = this.ClientApi;
			string shapePath3;
			if ((shapePath3 = this.AnimatedShapePath) == null && (shapePath3 = this.AnimatedShapeFirstPersonPath) == null)
			{
				CompositeShape shape = item.Shape;
				shapePath3 = (((shape != null) ? shape.Base.ToString() : null) ?? "");
			}
			this.Shape = AnimatableShape.Create(clientApi, shapePath3, item);
			ICoreClientAPI clientApi2 = this.ClientApi;
			string shapePath4;
			if ((shapePath4 = this.AnimatedShapeFirstPersonPath) == null && (shapePath4 = this.AnimatedShapePath) == null)
			{
				CompositeShape shape2 = item.Shape;
				shapePath4 = (((shape2 != null) ? shape2.Base.ToString() : null) ?? "");
			}
			this.ShapeFirstPerson = AnimatableShape.Create(clientApi2, shapePath4, item);
			foreach (string shapePath in this.ShapeAlternatesPaths)
			{
				this.ShapeAlternates.Add(AnimatableShape.Create(this.ClientApi, shapePath, item));
			}
			foreach (string shapePath2 in this.ShapeAlternatesFirstPersonPaths)
			{
				this.ShapeAlternatesFirstPerson.Add(AnimatableShape.Create(this.ClientApi, shapePath2, item));
			}
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0006252C File Offset: 0x0006072C
		[return: Nullable(2)]
		protected virtual AnimatableShape GetCurrentShape(ItemStack itemStack)
		{
			int renderVariant = itemStack.Attributes.GetAsInt("renderVariant", 0);
			if (renderVariant <= 0)
			{
				AnimatableShape result;
				if ((result = ((this.CurrentFirstPerson || this.SwitchModels) ? this.ShapeFirstPerson : this.Shape)) == null)
				{
					result = (this.Shape ?? this.ShapeFirstPerson);
				}
				return result;
			}
			renderVariant -= 2;
			if (renderVariant < 0)
			{
				AnimatableShape result2;
				if ((result2 = ((this.CurrentFirstPerson || this.SwitchModels) ? this.ShapeFirstPerson : this.Shape)) == null)
				{
					result2 = (this.Shape ?? this.ShapeFirstPerson);
				}
				return result2;
			}
			if (this.CurrentFirstPerson && this.ShapeFirstPerson != null)
			{
				if (renderVariant < this.ShapeAlternatesFirstPerson.Count)
				{
					if (this.ShapeAlternatesFirstPerson.Count < renderVariant)
					{
						return this.ShapeFirstPerson;
					}
					return this.ShapeAlternatesFirstPerson[renderVariant];
				}
				else
				{
					if (renderVariant >= this.ShapeAlternates.Count)
					{
						return this.ShapeFirstPerson;
					}
					if (this.ShapeAlternates.Count < renderVariant)
					{
						return this.ShapeFirstPerson;
					}
					return this.ShapeAlternates[renderVariant];
				}
			}
			else
			{
				if (renderVariant >= this.ShapeAlternates.Count)
				{
					return this.Shape;
				}
				return this.ShapeAlternates[renderVariant];
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00062658 File Offset: 0x00060858
		protected virtual void RenderShape(IShaderProgram shaderProgram, IWorldAccessor world, AnimatableShape shape, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMat, ItemSlot itemSlot, Entity entity, float dt, [Nullable(2)] MultiTextureMeshRef meshOverride = null)
		{
			AnimatableShape currentShape = this.GetCurrentShape(itemStack);
			if (currentShape == null)
			{
				return;
			}
			currentShape.Render(shaderProgram, itemStackRenderInfo, render, itemStack, lightrgbs, itemModelMat, entity, dt, meshOverride);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00062688 File Offset: 0x00060888
		protected virtual void CalculateAnimation([Nullable(2)] AnimatorBase animator, Shape shape, ICoreClientAPI clientApi, Entity entity, EnumItemRenderTarget target, float dt)
		{
			if (animator != null && !clientApi.IsGamePaused && target == 1)
			{
				ClientAnimator clientAnimator = animator as ClientAnimator;
				if (clientAnimator != null)
				{
					EntityPlayer agent = entity as EntityPlayer;
					if (agent != null)
					{
						AnimationPatches.AnimatorPlayerMap animators = AnimationPatches.Animators;
						if (animators != null)
						{
							animators.Add(clientAnimator, agent);
						}
					}
				}
				animator.OnFrame(this.ActiveAnimationsByCode, dt);
			}
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000626DC File Offset: 0x000608DC
		protected static bool IsFirstPerson(Entity entity)
		{
			ICoreClientAPI coreClientAPI = entity.Api as ICoreClientAPI;
			long? num = (coreClientAPI != null) ? new long?(coreClientAPI.World.Player.Entity.EntityId) : default(long?);
			long entityId = entity.EntityId;
			if (!(num.GetValueOrDefault() == entityId & num != null))
			{
				return false;
			}
			ICoreClientAPI coreClientAPI2 = entity.Api as ICoreClientAPI;
			if (coreClientAPI2 != null)
			{
				IClientWorldAccessor world = coreClientAPI2.World;
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

		// Token: 0x06001401 RID: 5121 RVA: 0x00062770 File Offset: 0x00060970
		[return: Nullable(2)]
		protected ItemRenderInfo PrepareShape(ICoreClientAPI api, Matrixf itemModelMat, float[] modelMat, ItemSlot itemSlot, Entity entity, bool right, float dt)
		{
			if (((itemSlot != null) ? itemSlot.Itemstack : null) == null)
			{
				return null;
			}
			string attachmentPoint = this.GetAttachmentPointName(right, entity);
			IAnimationManager animManager = entity.AnimManager;
			AttachmentPointAndPose attachmentPointAndPose2;
			if (animManager == null)
			{
				attachmentPointAndPose2 = null;
			}
			else
			{
				IAnimator animator = animManager.Animator;
				attachmentPointAndPose2 = ((animator != null) ? animator.GetAttachmentPointPose(attachmentPoint) : null);
			}
			AttachmentPointAndPose attachmentPointAndPose = attachmentPointAndPose2;
			if (attachmentPointAndPose == null)
			{
				return null;
			}
			AttachmentPoint attachPoint = attachmentPointAndPose.AttachPoint;
			ItemRenderInfo itemStackRenderInfo = api.Render.GetItemStackRenderInfo(itemSlot, right ? 2 : 3, dt);
			if (((itemStackRenderInfo != null) ? itemStackRenderInfo.Transform : null) == null)
			{
				return null;
			}
			itemModelMat.Set(modelMat).Mul(attachmentPointAndPose.AnimModelMatrix).Translate(itemStackRenderInfo.Transform.Origin.X, itemStackRenderInfo.Transform.Origin.Y, itemStackRenderInfo.Transform.Origin.Z).Scale(itemStackRenderInfo.Transform.ScaleXYZ.X, itemStackRenderInfo.Transform.ScaleXYZ.Y, itemStackRenderInfo.Transform.ScaleXYZ.Z).Translate(attachPoint.PosX / 16.0 + (double)itemStackRenderInfo.Transform.Translation.X, attachPoint.PosY / 16.0 + (double)itemStackRenderInfo.Transform.Translation.Y, attachPoint.PosZ / 16.0 + (double)itemStackRenderInfo.Transform.Translation.Z).RotateX((float)(attachPoint.RotationX + (double)itemStackRenderInfo.Transform.Rotation.X) * 0.017453292f).RotateY((float)(attachPoint.RotationY + (double)itemStackRenderInfo.Transform.Rotation.Y) * 0.017453292f).RotateZ((float)(attachPoint.RotationZ + (double)itemStackRenderInfo.Transform.Rotation.Z) * 0.017453292f).Translate(0f - itemStackRenderInfo.Transform.Origin.X, 0f - itemStackRenderInfo.Transform.Origin.Y, 0f - itemStackRenderInfo.Transform.Origin.Z);
			return itemStackRenderInfo;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0006298C File Offset: 0x00060B8C
		protected string GetAttachmentPointName(bool right, Entity entity)
		{
			PlayerItemFrame? activeFrame = Animatable.GetActivePlayerItemFrame(entity);
			if (activeFrame != null)
			{
				if (activeFrame.Value.DetachedAnchor)
				{
					return "DetachedAnchor";
				}
				if (activeFrame.Value.SwitchArms)
				{
					if (!right)
					{
						return "RightHand";
					}
					return "LeftHand";
				}
			}
			if (this.AttachmentPointOverride != null)
			{
				string attachmentPointOverride = this.AttachmentPointOverride;
				this.AttachmentPointOverride = null;
				this.DetachedAnchor = false;
				this.SwitchArms = false;
				return attachmentPointOverride;
			}
			if (this.DetachedAnchor)
			{
				this.DetachedAnchor = false;
				return "DetachedAnchor";
			}
			if (!this.SwitchArms)
			{
				if (!right)
				{
					return "LeftHand";
				}
				return "RightHand";
			}
			else
			{
				this.SwitchArms = false;
				if (!right)
				{
					return "RightHand";
				}
				return "LeftHand";
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00062A40 File Offset: 0x00060C40
		protected static PlayerItemFrame? GetActivePlayerItemFrame(Entity entity)
		{
			EntityPlayer player = entity as EntityPlayer;
			if (player == null)
			{
				return default(PlayerItemFrame?);
			}
			if (Animatable.IsOwner(entity) && Animatable.IsFirstPerson(entity))
			{
				FirstPersonAnimationsBehavior firstPersonBehavior = player.GetBehavior<FirstPersonAnimationsBehavior>();
				if (firstPersonBehavior != null && firstPersonBehavior.HasActiveAnimationFrame)
				{
					return new PlayerItemFrame?(firstPersonBehavior.CurrentFrame);
				}
			}
			ThirdPersonAnimationsBehavior thirdPersonBehavior = player.GetBehavior<ThirdPersonAnimationsBehavior>();
			if (thirdPersonBehavior != null && thirdPersonBehavior.HasActiveAnimationFrame)
			{
				return new PlayerItemFrame?(thirdPersonBehavior.CurrentFrame);
			}
			return default(PlayerItemFrame?);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00062AB8 File Offset: 0x00060CB8
		protected static void ShadowPass(ICoreClientAPI api, ItemRenderInfo itemStackRenderInfo, Matrixf itemModelMat, AnimatableShape shape)
		{
			IRenderAPI render = api.Render;
			string textureSampleName = "tex2d";
			render.CurrentActiveShader.BindTexture2D("tex2d", itemStackRenderInfo.TextureId, 0);
			float[] array = Mat4f.Mul(itemModelMat.Values, api.Render.CurrentModelviewMatrix, itemModelMat.Values);
			Mat4f.Mul(array, api.Render.CurrentProjectionMatrix, array);
			api.Render.CurrentActiveShader.UniformMatrix("mvpMatrix", array);
			api.Render.CurrentActiveShader.Uniform("origin", new Vec3f());
			if (!itemStackRenderInfo.CullFaces)
			{
				render.GlDisableCullFace();
			}
			render.RenderMultiTextureMesh(shape.MeshRef, textureSampleName, 0);
			if (!itemStackRenderInfo.CullFaces)
			{
				render.GlEnableCullFace();
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00062B74 File Offset: 0x00060D74
		protected static bool IsOwner(Entity entity)
		{
			ICoreClientAPI coreClientAPI = entity.Api as ICoreClientAPI;
			long? num = (coreClientAPI != null) ? new long?(coreClientAPI.World.Player.Entity.EntityId) : default(long?);
			long entityId = entity.EntityId;
			return num.GetValueOrDefault() == entityId & num != null;
		}

		// Token: 0x040009D4 RID: 2516
		[Nullable(2)]
		protected CombatOverhaulAnimationsSystem ModSystem;

		// Token: 0x040009D5 RID: 2517
		protected Dictionary<string, AnimationMetaData> ActiveAnimationsByCode = new Dictionary<string, AnimationMetaData>();

		// Token: 0x040009D6 RID: 2518
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;

		// Token: 0x040009D7 RID: 2519
		[Nullable(2)]
		protected string AnimatedShapePath;

		// Token: 0x040009D8 RID: 2520
		[Nullable(2)]
		protected string AnimatedShapeFirstPersonPath;

		// Token: 0x040009D9 RID: 2521
		protected readonly List<string> ShapeAlternatesPaths = new List<string>();

		// Token: 0x040009DA RID: 2522
		protected readonly List<string> ShapeAlternatesFirstPersonPaths = new List<string>();

		// Token: 0x040009DB RID: 2523
		[Nullable(2)]
		protected AnimatableShape Shape;

		// Token: 0x040009DC RID: 2524
		[Nullable(2)]
		protected AnimatableShape ShapeFirstPerson;

		// Token: 0x040009DD RID: 2525
		[Nullable(new byte[]
		{
			1,
			2
		})]
		protected readonly List<AnimatableShape> ShapeAlternates = new List<AnimatableShape>();

		// Token: 0x040009DE RID: 2526
		[Nullable(new byte[]
		{
			1,
			2
		})]
		protected readonly List<AnimatableShape> ShapeAlternatesFirstPerson = new List<AnimatableShape>();

		// Token: 0x040009DF RID: 2527
		protected Matrixf ItemModelMat = new Matrixf();

		// Token: 0x040009E0 RID: 2528
		protected float TimeAccumulation;

		// Token: 0x040009E1 RID: 2529
		protected bool CurrentFirstPerson;

		// Token: 0x040009E2 RID: 2530
		protected const int RenderVariantOffset = 2;
	}
}
