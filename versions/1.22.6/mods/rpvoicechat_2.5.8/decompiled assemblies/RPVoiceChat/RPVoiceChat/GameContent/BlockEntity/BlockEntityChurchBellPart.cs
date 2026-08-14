using System;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B0 RID: 176
	public class BlockEntityChurchBellPart : BEWeldable
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001D583 File Offset: 0x0001B783
		public override string InventoryClassName
		{
			get
			{
				return "churchbellpart";
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001D58A File Offset: 0x0001B78A
		public BlockEntityChurchBellPart() : base(4)
		{
			this.FluxNeeded = 4;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001D59C File Offset: 0x0001B79C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.ResultingBlockCode = base.Block.Code.Path.Replace("part", "layer");
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi != null)
			{
				this.Renderer = new WeldableRenderer(capi, this);
				AssetLocation assetLoc = new AssetLocation(RPVoiceChatMod.modID, "shapes/" + base.Block.Shape.Base.Path + "-flux.json");
				this.FluxShape = Shape.TryGet(api, assetLoc);
				this.Inv.SlotModified += delegate(int i)
				{
					this.UpdateMeshRefs();
				};
			}
			this.UpdateMeshRefs();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001D648 File Offset: 0x0001B848
		protected override MeshData RenderPart(int numPart)
		{
			ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
			Shape shape = Shape.TryGet(this.Api, new AssetLocation(RPVoiceChatMod.modID, "shapes/" + base.Block.Shape.Base.Path + ".json"));
			MeshData meshdata = new MeshData(true);
			coreClientAPI.Tesselator.TesselateShape(base.Block, shape, ref meshdata, null, default(int?), null);
			return meshdata.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 1.5707964f * (float)numPart, 0f);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001D6EC File Offset: 0x0001B8EC
		protected override MeshData RenderFlux(int numFlux)
		{
			MeshData meshdata;
			(this.Api as ICoreClientAPI).Tesselator.TesselateShape(base.Block, this.FluxShape, ref meshdata, null, default(int?), null);
			return meshdata.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 1.5707964f * (float)numFlux, 0f);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001D754 File Offset: 0x0001B954
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			if (byItemStack != null)
			{
				this.Inv[1].Itemstack = byItemStack.Clone();
				this.Inv[1].Itemstack.StackSize = 1;
				this.MarkDirty(true, null);
				this.UpdateMeshRefs();
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001D7A0 File Offset: 0x0001B9A0
		public bool OnInteract(IPlayer byPlayer)
		{
			ItemSlot hotbarslot = byPlayer.InventoryManager.ActiveHotbarSlot;
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (hotbarslot.Empty)
			{
				return true;
			}
			if (!(hotbarslot.Itemstack.Collectible.Code.Path == "powder-borax"))
			{
				if (hotbarslot.Itemstack.Collectible.Code.Path == base.Block.Code.Path)
				{
					for (int i = 0; i < 4; i++)
					{
						if (this.Inv[i + 1].Empty)
						{
							this.Inv[i + 1].Itemstack = hotbarslot.TakeOut(1);
							this.MarkDirty(true, null);
							this.UpdateMeshRefs();
							return true;
						}
					}
				}
				return true;
			}
			if (base.FluxSlot.Empty || base.FluxSlot.Itemstack.StackSize < 4)
			{
				ItemStack itemStack = hotbarslot.TakeOut(1);
				if (base.FluxSlot.Empty)
				{
					base.FluxSlot.Itemstack = itemStack;
				}
				else
				{
					ItemStack itemstack = base.FluxSlot.Itemstack;
					int stackSize = itemstack.StackSize;
					itemstack.StackSize = stackSize + 1;
				}
				this.MarkDirty(true, null);
				this.UpdateMeshRefs();
				return true;
			}
			if (capi != null)
			{
				capi.TriggerIngameError(capi.World.Player, "toomuchflux", UIUtils.I18n(this.i18nPrefix + ".TooMuchFlux"));
			}
			return false;
		}

		// Token: 0x0400028D RID: 653
		private Shape FluxShape;
	}
}
