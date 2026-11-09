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
	// Token: 0x0200016A RID: 362
	[NullableContext(1)]
	[Nullable(0)]
	public static class ColliderTools
	{
		// Token: 0x06001226 RID: 4646 RVA: 0x00057204 File Offset: 0x00055404
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

		// Token: 0x06001227 RID: 4647 RVA: 0x000572C4 File Offset: 0x000554C4
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

		// Token: 0x06001228 RID: 4648 RVA: 0x00057380 File Offset: 0x00055580
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

		// Token: 0x06001229 RID: 4649 RVA: 0x000575C0 File Offset: 0x000557C0
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

		// Token: 0x04000882 RID: 2178
		private static readonly Vec4f _inputBuffer = new Vec4f(0f, 0f, 0f, 1f);

		// Token: 0x04000883 RID: 2179
		private static readonly Vec4f _outputBuffer = new Vec4f(0f, 0f, 0f, 1f);

		// Token: 0x04000884 RID: 2180
		private static readonly Vec4d _inputBufferD = new Vec4d(0.0, 0.0, 0.0, 1.0);

		// Token: 0x04000885 RID: 2181
		private static readonly Vec4d _outputBufferD = new Vec4d(0.0, 0.0, 0.0, 1.0);

		// Token: 0x04000886 RID: 2182
		private static readonly Matrixf _matrixBuffer = new Matrixf();
	}
}
