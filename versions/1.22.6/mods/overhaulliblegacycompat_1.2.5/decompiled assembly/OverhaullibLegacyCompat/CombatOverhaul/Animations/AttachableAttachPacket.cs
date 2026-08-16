using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000192 RID: 402
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class AttachableAttachPacket
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00061B3F File Offset: 0x0005FD3F
		// (set) Token: 0x060013A0 RID: 5024 RVA: 0x00061B47 File Offset: 0x0005FD47
		public long EntityId { get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00061B50 File Offset: 0x0005FD50
		// (set) Token: 0x060013A2 RID: 5026 RVA: 0x00061B58 File Offset: 0x0005FD58
		public string AttachmentCode { get; set; } = "";

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x00061B61 File Offset: 0x0005FD61
		// (set) Token: 0x060013A4 RID: 5028 RVA: 0x00061B69 File Offset: 0x0005FD69
		public string ItemCode { get; set; } = "";

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00061B72 File Offset: 0x0005FD72
		// (set) Token: 0x060013A6 RID: 5030 RVA: 0x00061B7A File Offset: 0x0005FD7A
		public float[] Transform { get; set; } = Array.Empty<float>();

		// Token: 0x060013A7 RID: 5031 RVA: 0x00061B83 File Offset: 0x0005FD83
		public AttachableAttachPacket()
		{
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00061BAC File Offset: 0x0005FDAC
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

		// Token: 0x060013A9 RID: 5033 RVA: 0x00061CE5 File Offset: 0x0005FEE5
		public ItemStack GetItemStack(IWorldAccessor world)
		{
			JsonItemStack jsonItemStack = new JsonItemStack();
			jsonItemStack.Code = new AssetLocation(this.ItemCode);
			jsonItemStack.Type = 1;
			jsonItemStack.Resolve(world, "attach packet");
			return jsonItemStack.ResolvedItemstack;
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00061D18 File Offset: 0x0005FF18
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
