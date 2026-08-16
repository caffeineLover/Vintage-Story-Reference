using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000191 RID: 401
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Attachment : IDisposable
	{
		// Token: 0x06001398 RID: 5016 RVA: 0x00061610 File Offset: 0x0005F810
		public Attachment(ICoreClientAPI api, string attachmentPointCode, ItemStack attachment, ModelTransform transform, bool newAnimatableShape = false)
		{
			this._api = api;
			this._itemStack = attachment.Clone();
			this._attachedTransform = transform;
			this._attachmentPointCode = attachmentPointCode;
			if (attachment.Attributes.HasAttribute("attachableShape"))
			{
				this._shape = AnimatableShape.Create(api, attachment.Attributes.GetAsString("attachableShape", null), attachment.Item);
				this._disposeShape = true;
				return;
			}
			Item item = attachment.Item;
			bool flag;
			if (item == null)
			{
				flag = false;
			}
			else
			{
				JsonObject attributes = item.Attributes;
				flag = ((attributes != null) ? new bool?(attributes.KeyExists("attachableShape")) : default(bool?)).GetValueOrDefault();
			}
			if (!flag)
			{
				if (!newAnimatableShape && attachment.Item.HasBehavior(typeof(Animatable), true))
				{
					Animatable behavior = attachment.Item.GetCollectibleBehavior(typeof(Animatable), true) as Animatable;
					if (behavior != null)
					{
						this._behavior = behavior;
						this._disposeShape = false;
						return;
					}
				}
				this._shape = AnimatableShape.Create(api, attachment.Item.Shape.Base.ToString(), attachment.Item);
				this._disposeShape = true;
				return;
			}
			string shapePath = attachment.Item.Attributes["attachableShape"].AsString(null);
			if (!this._api.Assets.Exists(new AssetLocation(shapePath + ".json")))
			{
				LoggerUtil.Warn(this._api, this, "Shape was not found: " + shapePath);
				return;
			}
			this._shape = AnimatableShape.Create(api, attachment.Item.Attributes["attachableShape"].AsString(null), attachment.Item);
			this._disposeShape = true;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x000617D0 File Offset: 0x0005F9D0
		public void Render(AnimatableShape parentShape, IShaderProgram shaderProgram, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, Vec4f lightrgbs, Matrixf itemModelMat, Entity entity, float dt)
		{
			ItemRenderInfo attachedRenderInfo = this.GetAttachmentRenderInfo(itemStackRenderInfo.dt);
			AnimatorBase animator = parentShape.GetAnimator(entity.EntityId);
			AttachmentPointAndPose attachmentPointAndPose = (animator != null) ? animator.GetAttachmentPointPose(this._attachmentPointCode) : null;
			if (attachmentPointAndPose == null)
			{
				LoggerUtil.Warn(this._api, this, "Attachment point '" + this._attachmentPointCode + "' not found");
				return;
			}
			AttachmentPoint attachmentPoint = attachmentPointAndPose.AttachPoint;
			this.CalculateMeshMatrix(itemModelMat, itemStackRenderInfo, attachedRenderInfo, attachmentPointAndPose, attachmentPoint);
			AnimatableShape shape = this.GetShape();
			if (shape == null)
			{
				return;
			}
			shape.Render(shaderProgram, attachedRenderInfo, render, this._itemStack, lightrgbs, this._attachedMeshMatrix, entity, dt, null);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00061869 File Offset: 0x0005FA69
		public void BeforeRender(EnumItemRenderTarget target, Entity entity, float dt)
		{
			Animatable behavior = this._behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.BeforeRender(this._api, this._itemStack, entity, target, dt);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0006188A File Offset: 0x0005FA8A
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			if (this._disposeShape)
			{
				AnimatableShape shape = this._shape;
				if (shape == null)
				{
					return;
				}
				shape.Dispose();
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000618B4 File Offset: 0x0005FAB4
		[NullableContext(2)]
		private AnimatableShape GetShape()
		{
			AnimatableShape result;
			if ((result = this._shape) == null)
			{
				Animatable behavior = this._behavior;
				if (behavior == null)
				{
					return null;
				}
				result = behavior.CurrentAnimatableShape;
			}
			return result;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000618D4 File Offset: 0x0005FAD4
		private void CalculateMeshMatrix(Matrixf modelMat, ItemRenderInfo renderInfo, ItemRenderInfo attachedRenderInfo, AttachmentPointAndPose apap, AttachmentPoint ap)
		{
			this._attachedMeshMatrix.Set(modelMat.Values).Translate(-renderInfo.Transform.Origin.X, -renderInfo.Transform.Origin.Y, -renderInfo.Transform.Origin.Z).Mul(apap.AnimModelMatrix).Translate((ap.PosX + (double)attachedRenderInfo.Transform.Translation.X) / 16.0, (ap.PosY + (double)attachedRenderInfo.Transform.Translation.Y) / 16.0, (ap.PosZ + (double)attachedRenderInfo.Transform.Translation.Z) / 16.0).Translate(renderInfo.Transform.Origin.X, renderInfo.Transform.Origin.Y, renderInfo.Transform.Origin.Z).RotateX((float)(ap.RotationX + (double)attachedRenderInfo.Transform.Rotation.X) * 0.017453292f).RotateY((float)(ap.RotationY + (double)attachedRenderInfo.Transform.Rotation.Y) * 0.017453292f).RotateZ((float)(ap.RotationZ + (double)attachedRenderInfo.Transform.Rotation.Z) * 0.017453292f).Scale(attachedRenderInfo.Transform.ScaleXYZ.X, attachedRenderInfo.Transform.ScaleXYZ.Y, attachedRenderInfo.Transform.ScaleXYZ.Z).Translate(-attachedRenderInfo.Transform.Origin.X / 16f, -attachedRenderInfo.Transform.Origin.Y / 16f, -attachedRenderInfo.Transform.Origin.Z / 16f).Translate(-renderInfo.Transform.Origin.X, -renderInfo.Transform.Origin.Y, -renderInfo.Transform.Origin.Z);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00061B00 File Offset: 0x0005FD00
		private ItemRenderInfo GetAttachmentRenderInfo(float dt)
		{
			if (this._dummySlot == null)
			{
				this._dummySlot = new DummySlot(this._itemStack);
			}
			ItemRenderInfo itemStackRenderInfo = this._api.Render.GetItemStackRenderInfo(this._dummySlot, 4, dt);
			itemStackRenderInfo.Transform = this._attachedTransform;
			return itemStackRenderInfo;
		}

		// Token: 0x040009BE RID: 2494
		private readonly ICoreClientAPI _api;

		// Token: 0x040009BF RID: 2495
		private readonly ModelTransform _attachedTransform;

		// Token: 0x040009C0 RID: 2496
		private readonly ItemStack _itemStack;

		// Token: 0x040009C1 RID: 2497
		private readonly string _attachmentPointCode;

		// Token: 0x040009C2 RID: 2498
		[Nullable(2)]
		private readonly AnimatableShape _shape;

		// Token: 0x040009C3 RID: 2499
		[Nullable(2)]
		private readonly Animatable _behavior;

		// Token: 0x040009C4 RID: 2500
		private readonly bool _disposeShape;

		// Token: 0x040009C5 RID: 2501
		private readonly Matrixf _attachedMeshMatrix = new Matrixf();

		// Token: 0x040009C6 RID: 2502
		[Nullable(2)]
		private DummySlot _dummySlot;

		// Token: 0x040009C7 RID: 2503
		private bool _disposed;
	}
}
