using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200015E RID: 350
	[NullableContext(1)]
	[Nullable(0)]
	public static class ColliderTools
	{
		// Token: 0x060011B2 RID: 4530 RVA: 0x000558E4 File Offset: 0x00053AE4
		public static Vector3 TransformVector(Vector3 value, Matrixf modelMatrix, EntityPos playerPos)
		{
			ColliderTools._inputBuffer.X = value.X;
			ColliderTools._inputBuffer.Y = value.Y;
			ColliderTools._inputBuffer.Z = value.Z;
			Mat4f.MulWithVec4(modelMatrix.Values, ColliderTools._inputBuffer, ColliderTools._outputBuffer);
			ColliderTools._outputBuffer.X += (float)playerPos.X;
			ColliderTools._outputBuffer.Y += (float)playerPos.Y;
			ColliderTools._outputBuffer.Z += (float)playerPos.Z;
			return new Vector3(ColliderTools._outputBuffer.X, ColliderTools._outputBuffer.Y, ColliderTools._outputBuffer.Z);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000559A4 File Offset: 0x00053BA4
		public static Vector3d TransformVector(Vector3d value, Matrixf modelMatrix, EntityPos playerPos)
		{
			ColliderTools._inputBufferD.X = value.X;
			ColliderTools._inputBufferD.Y = value.Y;
			ColliderTools._inputBufferD.Z = value.Z;
			Mat4f.MulWithVec4(modelMatrix.Values, ColliderTools._inputBufferD, ColliderTools._outputBufferD);
			ColliderTools._outputBufferD.X += playerPos.X;
			ColliderTools._outputBufferD.Y += playerPos.Y;
			ColliderTools._outputBufferD.Z += playerPos.Z;
			return new Vector3d(ColliderTools._outputBufferD.X, ColliderTools._outputBufferD.Y, ColliderTools._outputBufferD.Z);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00055A60 File Offset: 0x00053C60
		[return: Nullable(2)]
		public static Matrixf GetHeldItemModelMatrix(EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			EntityShapeRenderer entityShapeRenderer = entity.Properties.Client.Renderer as EntityShapeRenderer;
			if (entityShapeRenderer == null)
			{
				return null;
			}
			if (((itemSlot != null) ? itemSlot.Itemstack : null) == null)
			{
				return null;
			}
			IAnimationManager animManager = entity.AnimManager;
			AttachmentPointAndPose attachmentPointAndPose2;
			if (animManager == null)
			{
				attachmentPointAndPose2 = null;
			}
			else
			{
				IAnimator animator = animManager.Animator;
				attachmentPointAndPose2 = ((animator != null) ? animator.GetAttachmentPointPose(right ? "RightHand" : "LeftHand") : null);
			}
			AttachmentPointAndPose attachmentPointAndPose = attachmentPointAndPose2;
			if (attachmentPointAndPose == null)
			{
				return null;
			}
			AttachmentPoint attachPoint = attachmentPointAndPose.AttachPoint;
			ItemRenderInfo itemStackRenderInfo = api.Render.GetItemStackRenderInfo(itemSlot, right ? 2 : 3, 0f);
			if (((itemStackRenderInfo != null) ? itemStackRenderInfo.Transform : null) == null)
			{
				return null;
			}
			return ColliderTools._matrixBuffer.Set(entityShapeRenderer.ModelMat).Mul(attachmentPointAndPose.AnimModelMatrix).Translate(itemStackRenderInfo.Transform.Origin.X, itemStackRenderInfo.Transform.Origin.Y, itemStackRenderInfo.Transform.Origin.Z).Scale(itemStackRenderInfo.Transform.ScaleXYZ.X, itemStackRenderInfo.Transform.ScaleXYZ.Y, itemStackRenderInfo.Transform.ScaleXYZ.Z).Translate(attachPoint.PosX / 16.0 + (double)itemStackRenderInfo.Transform.Translation.X, attachPoint.PosY / 16.0 + (double)itemStackRenderInfo.Transform.Translation.Y, attachPoint.PosZ / 16.0 + (double)itemStackRenderInfo.Transform.Translation.Z).RotateX((float)(attachPoint.RotationX + (double)itemStackRenderInfo.Transform.Rotation.X) * 0.017453292f).RotateY((float)(attachPoint.RotationY + (double)itemStackRenderInfo.Transform.Rotation.Y) * 0.017453292f).RotateZ((float)(attachPoint.RotationZ + (double)itemStackRenderInfo.Transform.Rotation.Z) * 0.017453292f).Translate(0f - itemStackRenderInfo.Transform.Origin.X, 0f - itemStackRenderInfo.Transform.Origin.Y, 0f - itemStackRenderInfo.Transform.Origin.Z);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00055CA0 File Offset: 0x00053EA0
		public static bool Transform(IEnumerable<IHasCollider> colliders, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			EntityPos playerPos = entity.Pos;
			Matrixf modelMatrix = ColliderTools.GetHeldItemModelMatrix(entity, itemSlot, api, right);
			if (modelMatrix == null)
			{
				return false;
			}
			foreach (IHasCollider hasCollider in colliders)
			{
				hasCollider.InWorldCollider = hasCollider.RelativeCollider.Transform(modelMatrix, playerPos);
			}
			return true;
		}

		// Token: 0x0400085C RID: 2140
		private static readonly Vec4f _inputBuffer = new Vec4f(0f, 0f, 0f, 1f);

		// Token: 0x0400085D RID: 2141
		private static readonly Vec4f _outputBuffer = new Vec4f(0f, 0f, 0f, 1f);

		// Token: 0x0400085E RID: 2142
		private static readonly Vec4d _inputBufferD = new Vec4d(0.0, 0.0, 0.0, 1.0);

		// Token: 0x0400085F RID: 2143
		private static readonly Vec4d _outputBufferD = new Vec4d(0.0, 0.0, 0.0, 1.0);

		// Token: 0x04000860 RID: 2144
		private static readonly Matrixf _matrixBuffer = new Matrixf();
	}
}
