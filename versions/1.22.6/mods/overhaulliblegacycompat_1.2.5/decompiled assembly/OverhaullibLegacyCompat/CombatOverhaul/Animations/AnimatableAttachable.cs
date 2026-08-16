using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000190 RID: 400
	[NullableContext(1)]
	[Nullable(0)]
	public class AnimatableAttachable : Animatable
	{
		// Token: 0x0600138E RID: 5006 RVA: 0x0006114E File Offset: 0x0005F34E
		public AnimatableAttachable(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00061178 File Offset: 0x0005F378
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

		// Token: 0x06001390 RID: 5008 RVA: 0x0006120E File Offset: 0x0005F40E
		public bool ToggleAttachment(long entityId, string attachmentCode, bool toggle)
		{
			if (!this.ActiveAttachments.ContainsKey(entityId) || !this.ActiveAttachments[entityId].ContainsKey(attachmentCode))
			{
				return false;
			}
			this.ActiveAttachments[entityId][attachmentCode] = toggle;
			return true;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00061248 File Offset: 0x0005F448
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

		// Token: 0x06001392 RID: 5010 RVA: 0x000612B8 File Offset: 0x0005F4B8
		public bool? CheckAttachment(long entityId, string attachmentCode)
		{
			if (!this.ActiveAttachments.ContainsKey(entityId) || !this.ActiveAttachments[entityId].ContainsKey(attachmentCode))
			{
				return default(bool?);
			}
			return new bool?(this.ActiveAttachments[entityId][attachmentCode]);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00061308 File Offset: 0x0005F508
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

		// Token: 0x06001394 RID: 5012 RVA: 0x000613A0 File Offset: 0x0005F5A0
		public void SetSwitchModels(long entityId, bool switchModels)
		{
			this.SwitchModelsPerEntity[entityId] = switchModels;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000613B0 File Offset: 0x0005F5B0
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

		// Token: 0x06001396 RID: 5014 RVA: 0x0006146C File Offset: 0x0005F66C
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

		// Token: 0x06001397 RID: 5015 RVA: 0x00061504 File Offset: 0x0005F704
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

		// Token: 0x040009BB RID: 2491
		protected readonly Dictionary<long, Dictionary<string, Attachment>> Attachments = new Dictionary<long, Dictionary<string, Attachment>>();

		// Token: 0x040009BC RID: 2492
		protected readonly Dictionary<long, Dictionary<string, bool>> ActiveAttachments = new Dictionary<long, Dictionary<string, bool>>();

		// Token: 0x040009BD RID: 2493
		protected readonly Dictionary<long, bool> SwitchModelsPerEntity = new Dictionary<long, bool>();
	}
}
