using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A0 RID: 416
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AttachableAttachPacket
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x000635BF File Offset: 0x000617BF
		// (set) Token: 0x06001418 RID: 5144 RVA: 0x000635C7 File Offset: 0x000617C7
		public long EntityId { get; set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x000635D0 File Offset: 0x000617D0
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x000635D8 File Offset: 0x000617D8
		public string AttachmentCode { get; set; } = "";

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x000635E1 File Offset: 0x000617E1
		// (set) Token: 0x0600141C RID: 5148 RVA: 0x000635E9 File Offset: 0x000617E9
		public string ItemCode { get; set; } = "";

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x000635F2 File Offset: 0x000617F2
		// (set) Token: 0x0600141E RID: 5150 RVA: 0x000635FA File Offset: 0x000617FA
		public float[] Transform { get; set; } = Array.Empty<float>();

		// Token: 0x0600141F RID: 5151 RVA: 0x00063603 File Offset: 0x00061803
		public AttachableAttachPacket()
		{
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0006362C File Offset: 0x0006182C
		public AttachableAttachPacket(long entityId, string attachmentCode, ItemStack stack, ModelTransform transform)
		{
			this.EntityId = entityId;
			this.AttachmentCode = attachmentCode;
			string text;
			if (stack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.ToString() : null);
				}
			}
			this.ItemCode = (text ?? "");
			this.Transform = new float[]
			{
				transform.Translation.X,
				transform.Translation.Y,
				transform.Translation.Z,
				transform.Rotation.X,
				transform.Rotation.Y,
				transform.Rotation.Z,
				transform.Origin.X,
				transform.Origin.Y,
				transform.Origin.Z,
				transform.ScaleXYZ.X,
				transform.ScaleXYZ.Y,
				transform.ScaleXYZ.Z
			};
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00063765 File Offset: 0x00061965
		public ItemStack GetItemStack(IWorldAccessor world)
		{
			JsonItemStack jsonItemStack = new JsonItemStack();
			jsonItemStack.Code = new AssetLocation(this.ItemCode);
			jsonItemStack.Type = 1;
			jsonItemStack.Resolve(world, "attach packet");
			return jsonItemStack.ResolvedItemstack;
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00063798 File Offset: 0x00061998
		public ModelTransform GetModelTransform()
		{
			return new ModelTransform
			{
				Translation = new FastVec3f(this.Transform[0], this.Transform[1], this.Transform[2]),
				Rotation = new FastVec3f(this.Transform[3], this.Transform[4], this.Transform[5]),
				Origin = new FastVec3f(this.Transform[6], this.Transform[7], this.Transform[8]),
				ScaleXYZ = new FastVec3f(this.Transform[9], this.Transform[10], this.Transform[11])
			};
		}
	}
}
