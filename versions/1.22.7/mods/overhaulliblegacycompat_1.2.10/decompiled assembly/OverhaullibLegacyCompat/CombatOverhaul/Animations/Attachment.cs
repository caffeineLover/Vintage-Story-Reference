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
	// Token: 0x0200019F RID: 415
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Attachment : IDisposable
	{
		// Token: 0x06001410 RID: 5136 RVA: 0x00063090 File Offset: 0x00061290
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

		// Token: 0x06001411 RID: 5137 RVA: 0x00063250 File Offset: 0x00061450
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

		// Token: 0x06001412 RID: 5138 RVA: 0x000632E9 File Offset: 0x000614E9
		public void BeforeRender(EnumItemRenderTarget target, Entity entity, float dt)
		{
			Animatable behavior = this._behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.BeforeRender(this._api, this._itemStack, entity, target, dt);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0006330A File Offset: 0x0006150A
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

		// Token: 0x06001414 RID: 5140 RVA: 0x00063334 File Offset: 0x00061534
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

		// Token: 0x06001415 RID: 5141 RVA: 0x00063354 File Offset: 0x00061554
		private void CalculateMeshMatrix(Matrixf modelMat, ItemRenderInfo renderInfo, ItemRenderInfo attachedRenderInfo, AttachmentPointAndPose apap, AttachmentPoint ap)
		{
			this._attachedMeshMatrix.Set(modelMat.Values).Translate(-renderInfo.Transform.Origin.X, -renderInfo.Transform.Origin.Y, -renderInfo.Transform.Origin.Z).Mul(apap.AnimModelMatrix).Translate((ap.PosX + (double)attachedRenderInfo.Transform.Translation.X) / 16.0, (ap.PosY + (double)attachedRenderInfo.Transform.Translation.Y) / 16.0, (ap.PosZ + (double)attachedRenderInfo.Transform.Translation.Z) / 16.0).Translate(renderInfo.Transform.Origin.X, renderInfo.Transform.Origin.Y, renderInfo.Transform.Origin.Z).RotateX((float)(ap.RotationX + (double)attachedRenderInfo.Transform.Rotation.X) * 0.017453292f).RotateY((float)(ap.RotationY + (double)attachedRenderInfo.Transform.Rotation.Y) * 0.017453292f).RotateZ((float)(ap.RotationZ + (double)attachedRenderInfo.Transform.Rotation.Z) * 0.017453292f).Scale(attachedRenderInfo.Transform.ScaleXYZ.X, attachedRenderInfo.Transform.ScaleXYZ.Y, attachedRenderInfo.Transform.ScaleXYZ.Z).Translate(-attachedRenderInfo.Transform.Origin.X / 16f, -attachedRenderInfo.Transform.Origin.Y / 16f, -attachedRenderInfo.Transform.Origin.Z / 16f).Translate(-renderInfo.Transform.Origin.X, -renderInfo.Transform.Origin.Y, -renderInfo.Transform.Origin.Z);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00063580 File Offset: 0x00061780
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

		// Token: 0x040009E6 RID: 2534
		private readonly ICoreClientAPI _api;

		// Token: 0x040009E7 RID: 2535
		private readonly ModelTransform _attachedTransform;

		// Token: 0x040009E8 RID: 2536
		private readonly ItemStack _itemStack;

		// Token: 0x040009E9 RID: 2537
		private readonly string _attachmentPointCode;

		// Token: 0x040009EA RID: 2538
		[Nullable(2)]
		private readonly AnimatableShape _shape;

		// Token: 0x040009EB RID: 2539
		[Nullable(2)]
		private readonly Animatable _behavior;

		// Token: 0x040009EC RID: 2540
		private readonly bool _disposeShape;

		// Token: 0x040009ED RID: 2541
		private readonly Matrixf _attachedMeshMatrix = new Matrixf();

		// Token: 0x040009EE RID: 2542
		[Nullable(2)]
		private DummySlot _dummySlot;

		// Token: 0x040009EF RID: 2543
		private bool _disposed;
	}
}
