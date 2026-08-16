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
	// Token: 0x0200018F RID: 399
	[NullableContext(1)]
	[Nullable(0)]
	public class Animatable : CollectibleBehavior
	{
		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x000605C4 File Offset: 0x0005E7C4
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

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x000605D7 File Offset: 0x0005E7D7
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

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x0006060B File Offset: 0x0005E80B
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

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x0006061E File Offset: 0x0005E81E
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

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00060631 File Offset: 0x0005E831
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x00060639 File Offset: 0x0005E839
		public bool DetachedAnchor { get; set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00060642 File Offset: 0x0005E842
		// (set) Token: 0x0600137A RID: 4986 RVA: 0x0006064A File Offset: 0x0005E84A
		public bool SwitchArms { get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x00060653 File Offset: 0x0005E853
		// (set) Token: 0x0600137C RID: 4988 RVA: 0x0006065B File Offset: 0x0005E85B
		public bool SwitchModels { get; protected set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x00060664 File Offset: 0x0005E864
		// (set) Token: 0x0600137E RID: 4990 RVA: 0x0006066C File Offset: 0x0005E86C
		[Nullable(2)]
		public string AttachmentPointOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x0600137F RID: 4991 RVA: 0x00060678 File Offset: 0x0005E878
		public Animatable(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000606D0 File Offset: 0x0005E8D0
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

		// Token: 0x06001381 RID: 4993 RVA: 0x0006078C File Offset: 0x0005E98C
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

		// Token: 0x06001382 RID: 4994 RVA: 0x000607DC File Offset: 0x0005E9DC
		public virtual void BeforeRender(ICoreClientAPI clientApi, ItemStack itemStack, Entity player, EnumItemRenderTarget target, float dt)
		{
			this.CurrentFirstPerson = Animatable.IsFirstPerson(player);
			if (this.CurrentAnimatableShape != null)
			{
				AnimatableShape currentShape = this.GetCurrentShape(itemStack);
				this.CalculateAnimation((currentShape != null) ? currentShape.GetAnimator(player.EntityId) : null, this.CurrentAnimatableShape.Shape, clientApi, player, target, dt);
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00060830 File Offset: 0x0005EA30
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

		// Token: 0x06001384 RID: 4996 RVA: 0x00060938 File Offset: 0x0005EB38
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

		// Token: 0x06001385 RID: 4997 RVA: 0x00060AAC File Offset: 0x0005ECAC
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

		// Token: 0x06001386 RID: 4998 RVA: 0x00060BD8 File Offset: 0x0005EDD8
		protected virtual void RenderShape(IShaderProgram shaderProgram, IWorldAccessor world, AnimatableShape shape, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMat, ItemSlot itemSlot, Entity entity, float dt, [Nullable(2)] MultiTextureMeshRef meshOverride = null)
		{
			AnimatableShape currentShape = this.GetCurrentShape(itemStack);
			if (currentShape == null)
			{
				return;
			}
			currentShape.Render(shaderProgram, itemStackRenderInfo, render, itemStack, lightrgbs, itemModelMat, entity, dt, meshOverride);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00060C08 File Offset: 0x0005EE08
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

		// Token: 0x06001388 RID: 5000 RVA: 0x00060C5C File Offset: 0x0005EE5C
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

		// Token: 0x06001389 RID: 5001 RVA: 0x00060CF0 File Offset: 0x0005EEF0
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

		// Token: 0x0600138A RID: 5002 RVA: 0x00060F0C File Offset: 0x0005F10C
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

		// Token: 0x0600138B RID: 5003 RVA: 0x00060FC0 File Offset: 0x0005F1C0
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

		// Token: 0x0600138C RID: 5004 RVA: 0x00061038 File Offset: 0x0005F238
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

		// Token: 0x0600138D RID: 5005 RVA: 0x000610F4 File Offset: 0x0005F2F4
		protected static bool IsOwner(Entity entity)
		{
			ICoreClientAPI coreClientAPI = entity.Api as ICoreClientAPI;
			long? num = (coreClientAPI != null) ? new long?(coreClientAPI.World.Player.Entity.EntityId) : default(long?);
			long entityId = entity.EntityId;
			return num.GetValueOrDefault() == entityId & num != null;
		}

		// Token: 0x040009AC RID: 2476
		[Nullable(2)]
		protected CombatOverhaulAnimationsSystem ModSystem;

		// Token: 0x040009AD RID: 2477
		protected Dictionary<string, AnimationMetaData> ActiveAnimationsByCode = new Dictionary<string, AnimationMetaData>();

		// Token: 0x040009AE RID: 2478
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;

		// Token: 0x040009AF RID: 2479
		[Nullable(2)]
		protected string AnimatedShapePath;

		// Token: 0x040009B0 RID: 2480
		[Nullable(2)]
		protected string AnimatedShapeFirstPersonPath;

		// Token: 0x040009B1 RID: 2481
		protected readonly List<string> ShapeAlternatesPaths = new List<string>();

		// Token: 0x040009B2 RID: 2482
		protected readonly List<string> ShapeAlternatesFirstPersonPaths = new List<string>();

		// Token: 0x040009B3 RID: 2483
		[Nullable(2)]
		protected AnimatableShape Shape;

		// Token: 0x040009B4 RID: 2484
		[Nullable(2)]
		protected AnimatableShape ShapeFirstPerson;

		// Token: 0x040009B5 RID: 2485
		[Nullable(new byte[]
		{
			1,
			2
		})]
		protected readonly List<AnimatableShape> ShapeAlternates = new List<AnimatableShape>();

		// Token: 0x040009B6 RID: 2486
		[Nullable(new byte[]
		{
			1,
			2
		})]
		protected readonly List<AnimatableShape> ShapeAlternatesFirstPerson = new List<AnimatableShape>();

		// Token: 0x040009B7 RID: 2487
		protected Matrixf ItemModelMat = new Matrixf();

		// Token: 0x040009B8 RID: 2488
		protected float TimeAccumulation;

		// Token: 0x040009B9 RID: 2489
		protected bool CurrentFirstPerson;

		// Token: 0x040009BA RID: 2490
		protected const int RenderVariantOffset = 2;
	}
}
