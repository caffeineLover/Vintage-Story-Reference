using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019E RID: 414
	[NullableContext(1)]
	[Nullable(0)]
	public class AnimatableAttachable : Animatable
	{
		// Token: 0x06001406 RID: 5126 RVA: 0x00062BCE File Offset: 0x00060DCE
		public AnimatableAttachable(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00062BF8 File Offset: 0x00060DF8
		public bool SetAttachment(long entityId, string attachmentCode, ItemStack attachmentItem, ModelTransform transform, bool activate = true, bool newAnimatableShape = false)
		{
			if (this.ClientApi == null)
			{
				return false;
			}
			if (!this.Attachments.ContainsKey(entityId))
			{
				this.Attachments.Add(entityId, new Dictionary<string, Attachment>());
			}
			if (!this.ActiveAttachments.ContainsKey(entityId))
			{
				this.ActiveAttachments.Add(entityId, new Dictionary<string, bool>());
			}
			this.RemoveAttachment(entityId, attachmentCode);
			this.Attachments[entityId][attachmentCode] = new Attachment(this.ClientApi, attachmentCode, attachmentItem, transform, newAnimatableShape);
			this.ActiveAttachments[entityId][attachmentCode] = activate;
			return true;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00062C8E File Offset: 0x00060E8E
		public bool ToggleAttachment(long entityId, string attachmentCode, bool toggle)
		{
			if (!this.ActiveAttachments.ContainsKey(entityId) || !this.ActiveAttachments[entityId].ContainsKey(attachmentCode))
			{
				return false;
			}
			this.ActiveAttachments[entityId][attachmentCode] = toggle;
			return true;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00062CC8 File Offset: 0x00060EC8
		public bool RemoveAttachment(long entityId, string attachmentCode)
		{
			if (!this.ActiveAttachments.ContainsKey(entityId) || !this.ActiveAttachments[entityId].ContainsKey(attachmentCode))
			{
				return false;
			}
			this.Attachments[entityId][attachmentCode].Dispose();
			this.Attachments[entityId].Remove(attachmentCode);
			this.ActiveAttachments[entityId].Remove(attachmentCode);
			return true;
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00062D38 File Offset: 0x00060F38
		public bool? CheckAttachment(long entityId, string attachmentCode)
		{
			if (!this.ActiveAttachments.ContainsKey(entityId) || !this.ActiveAttachments[entityId].ContainsKey(attachmentCode))
			{
				return default(bool?);
			}
			return new bool?(this.ActiveAttachments[entityId][attachmentCode]);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00062D88 File Offset: 0x00060F88
		public bool ClearAttachments(long entityId)
		{
			if (!this.ActiveAttachments.ContainsKey(entityId))
			{
				return false;
			}
			foreach (KeyValuePair<string, Attachment> keyValuePair in this.Attachments[entityId])
			{
				string text;
				Attachment attachment;
				keyValuePair.Deconstruct(ref text, ref attachment);
				attachment.Dispose();
			}
			this.Attachments[entityId].Clear();
			this.ActiveAttachments[entityId].Clear();
			return true;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00062E20 File Offset: 0x00061020
		public void SetSwitchModels(long entityId, bool switchModels)
		{
			this.SwitchModelsPerEntity[entityId] = switchModels;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00062E30 File Offset: 0x00061030
		public override void BeforeRender(ICoreClientAPI clientApi, ItemStack itemStack, Entity player, EnumItemRenderTarget target, float dt)
		{
			bool switchModels;
			this.SwitchModelsPerEntity.TryGetValue(player.EntityId, ref switchModels);
			base.SwitchModels = switchModels;
			base.BeforeRender(clientApi, itemStack, player, target, dt);
			foreach (Dictionary<string, Attachment> dictionary in this.Attachments.Values)
			{
				foreach (Attachment attachment in dictionary.Values)
				{
					attachment.BeforeRender(target, player, dt);
				}
			}
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00062EEC File Offset: 0x000610EC
		public override void OnUnloaded(ICoreAPI api)
		{
			base.OnUnloaded(api);
			foreach (Dictionary<string, Attachment> dictionary in this.Attachments.Values)
			{
				foreach (Attachment attachment in dictionary.Values)
				{
					attachment.Dispose();
				}
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00062F84 File Offset: 0x00061184
		protected override void RenderShape(IShaderProgram shaderProgram, IWorldAccessor world, AnimatableShape shape, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMat, ItemSlot itemSlot, Entity entity, float dt, [Nullable(2)] MultiTextureMeshRef meshOverride = null)
		{
			bool switchModels;
			this.SwitchModelsPerEntity.TryGetValue(entity.EntityId, ref switchModels);
			base.SwitchModels = switchModels;
			base.RenderShape(shaderProgram, world, shape, itemStackRenderInfo, render, itemStack, lightrgbs, this.ItemModelMat, itemSlot, entity, dt, meshOverride);
			AnimatableShape shape2 = this.Shape;
			if (((shape2 != null) ? shape2.GetAnimator(entity.EntityId) : null) == null)
			{
				return;
			}
			Dictionary<string, bool> activeAttachments;
			if (!this.ActiveAttachments.TryGetValue(entity.EntityId, ref activeAttachments))
			{
				return;
			}
			Dictionary<string, Attachment> attachments;
			if (!this.Attachments.TryGetValue(entity.EntityId, ref attachments))
			{
				return;
			}
			AnimatableShape currentShape = this.GetCurrentShape(itemStack);
			if (currentShape == null)
			{
				return;
			}
			foreach (KeyValuePair<string, bool> keyValuePair in activeAttachments)
			{
				string text;
				bool flag;
				keyValuePair.Deconstruct(ref text, ref flag);
				string code = text;
				if (flag)
				{
					attachments[code].Render(currentShape, shaderProgram, itemStackRenderInfo, render, lightrgbs, itemModelMat, entity, dt);
				}
			}
		}

		// Token: 0x040009E3 RID: 2531
		protected readonly Dictionary<long, Dictionary<string, Attachment>> Attachments = new Dictionary<long, Dictionary<string, Attachment>>();

		// Token: 0x040009E4 RID: 2532
		protected readonly Dictionary<long, Dictionary<string, bool>> ActiveAttachments = new Dictionary<long, Dictionary<string, bool>>();

		// Token: 0x040009E5 RID: 2533
		protected readonly Dictionary<long, bool> SwitchModelsPerEntity = new Dictionary<long, bool>();
	}
}
