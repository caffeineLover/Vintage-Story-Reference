using System;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E2 RID: 226
	public class BlockEntityChurchBellPart : BEWeldable
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00024BAF File Offset: 0x00022DAF
		public override string InventoryClassName
		{
			get
			{
				return "churchbellpart";
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00024BB6 File Offset: 0x00022DB6
		public BlockEntityChurchBellPart() : base(4)
		{
			this.FluxNeeded = 4;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00024BC8 File Offset: 0x00022DC8
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

		// Token: 0x060006C7 RID: 1735 RVA: 0x00024C74 File Offset: 0x00022E74
		protected override MeshData RenderPart(int numPart)
		{
			ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
			Shape shape = Shape.TryGet(this.Api, new AssetLocation(RPVoiceChatMod.modID, "shapes/" + base.Block.Shape.Base.Path + ".json"));
			MeshData meshdata = new MeshData(true);
			coreClientAPI.Tesselator.TesselateShape(base.Block, shape, ref meshdata, null, default(int?), null);
			return meshdata.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 1.5707964f * (float)numPart, 0f);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00024D18 File Offset: 0x00022F18
		protected override MeshData RenderFlux(int numFlux)
		{
			MeshData meshdata;
			(this.Api as ICoreClientAPI).Tesselator.TesselateShape(base.Block, this.FluxShape, ref meshdata, null, default(int?), null);
			return meshdata.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 1.5707964f * (float)numFlux, 0f);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00024D80 File Offset: 0x00022F80
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

		// Token: 0x060006CA RID: 1738 RVA: 0x00024DCC File Offset: 0x00022FCC
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

		// Token: 0x0400033A RID: 826
		private Shape FluxShape;
	}
}
