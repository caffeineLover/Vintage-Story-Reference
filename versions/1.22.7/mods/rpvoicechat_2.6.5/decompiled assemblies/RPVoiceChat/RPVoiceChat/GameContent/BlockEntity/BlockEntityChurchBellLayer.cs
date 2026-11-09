using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E1 RID: 225
	public class BlockEntityChurchBellLayer : BEWeldable
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00024548 File Offset: 0x00022748
		public ItemSlot[] BellLayerSlots
		{
			get
			{
				return new ItemSlot[]
				{
					this.Inv[1],
					this.Inv[2],
					this.Inv[3],
					this.Inv[4]
				};
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00024597 File Offset: 0x00022797
		public override string InventoryClassName
		{
			get
			{
				return "churchbelllayer";
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0002459E File Offset: 0x0002279E
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x000245A6 File Offset: 0x000227A6
		public string[] BellLayerName
		{
			get
			{
				return this.bellLayerNames;
			}
			set
			{
				this.bellLayerNames = value;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000245B0 File Offset: 0x000227B0
		public BlockEntityChurchBellLayer() : base(4)
		{
			this.FluxNeeded = 3;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00024610 File Offset: 0x00022810
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.ResultingBlockCode = "churchbell";
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi != null)
			{
				this.Renderer = new WeldableRenderer(capi, this);
				this.Inv.SlotModified += delegate(int i)
				{
					this.UpdateMeshRefs();
				};
			}
			this.UpdateMeshRefs();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00024664 File Offset: 0x00022864
		protected override void UpdateMeshRefs()
		{
			if (this.Api.Side == 1)
			{
				return;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			for (int i = 0; i < 4; i++)
			{
				if (!this.BellLayerSlots[i].Empty)
				{
					MeshData meshdata = this.RenderPart(i);
					this.PartMeshRefs[i] = capi.Render.UploadMesh(meshdata);
					meshdata.Clear();
				}
			}
			for (int j = 0; j < this.FluxNeeded; j++)
			{
				if ((!this.BellLayerSlots[j].Empty || !this.BellLayerSlots[j + 1].Empty) && !base.FluxSlot.Empty && base.FluxSlot.StackSize >= j + 1)
				{
					MeshData meshdata2 = this.RenderFlux(j);
					this.FluxMeshRefs[j] = capi.Render.UploadMesh(meshdata2);
					meshdata2.Clear();
				}
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00024740 File Offset: 0x00022940
		protected override MeshData RenderPart(int numPart)
		{
			ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
			Shape shape = Shape.TryGet(this.Api, new AssetLocation(RPVoiceChatMod.modID, "shapes/" + this.BellLayerSlots[numPart].Itemstack.Block.Shape.Base.Path + ".json"));
			MeshData meshdata = new MeshData(true);
			coreClientAPI.Tesselator.TesselateShape(base.Block, shape, ref meshdata, null, default(int?), null);
			float vertOffset = 0f;
			for (int i = 0; i < numPart; i++)
			{
				if (!this.BellLayerSlots[i].Empty)
				{
					vertOffset += this.bellLayerHeights[i + 1];
				}
			}
			return meshdata.Translate(0f, vertOffset, 0f);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00024808 File Offset: 0x00022A08
		protected override MeshData RenderFlux(int numFlux)
		{
			ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
			Shape fluxShape = Shape.TryGet(this.Api, new AssetLocation(RPVoiceChatMod.modID, "shapes/block/churchbell/" + this.bellLayerNames[numFlux] + "-flux.json"));
			if (fluxShape == null)
			{
				throw new Exception("Layer flux shape is null for: " + this.bellLayerNames[numFlux]);
			}
			MeshData meshdata;
			coreClientAPI.Tesselator.TesselateShape(base.Block, fluxShape, ref meshdata, null, default(int?), null);
			float vertOffset = 0f;
			for (int i = 0; i < numFlux; i++)
			{
				if (!this.BellLayerSlots[i].Empty)
				{
					vertOffset += this.bellLayerHeights[i + 1];
				}
			}
			return meshdata.Translate(0f, vertOffset, 0f);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000248CC File Offset: 0x00022ACC
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			if (this.Api.Side == 2)
			{
				return;
			}
			if (byItemStack != null)
			{
				for (int i = 0; i < 4; i++)
				{
					string[] split = this.BellLayerName[i].Split("-", 0);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(8, 3);
					defaultInterpolatedStringHandler.AppendFormatted(split[0]);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted(split[1]);
					defaultInterpolatedStringHandler.AppendLiteral("-brass-");
					defaultInterpolatedStringHandler.AppendFormatted(split[2]);
					string layerName = defaultInterpolatedStringHandler.ToStringAndClear();
					if (byItemStack.Collectible.Code.Path == layerName)
					{
						this.BellLayerSlots[i].Itemstack = byItemStack.Clone();
						this.BellLayerSlots[i].Itemstack.StackSize = 1;
						this.MarkDirty(true, null);
						break;
					}
				}
			}
			this.UpdateMeshRefs();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000249AC File Offset: 0x00022BAC
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
				string[] layerNames = new string[]
				{
					"churchbell-layer-brass-bottom",
					"churchbell-layer-brass-middle",
					"churchbell-layer-brass-top",
					"churchbell-layer-brass-topmost"
				};
				if (ArrayExtensions.Contains<string>(layerNames, hotbarslot.Itemstack.Collectible.Code.Path))
				{
					for (int i = 0; i < 4; i++)
					{
						if (this.BellLayerSlots[i].Empty && !(hotbarslot.Itemstack.Collectible.Code.Path != layerNames[i]))
						{
							this.BellLayerSlots[i].Itemstack = hotbarslot.TakeOut(1);
							this.MarkDirty(true, null);
							this.UpdateMeshRefs();
							return true;
						}
					}
				}
				return true;
			}
			int filledLayerSlots = 0;
			ItemSlot[] bellLayerSlots = this.BellLayerSlots;
			for (int j = 0; j < bellLayerSlots.Length; j++)
			{
				if (!bellLayerSlots[j].Empty)
				{
					filledLayerSlots++;
				}
			}
			if (base.FluxSlot.Empty && 1 < filledLayerSlots)
			{
				base.FluxSlot.Itemstack = hotbarslot.TakeOut(1);
				this.MarkDirty(true, null);
				this.UpdateMeshRefs();
				return true;
			}
			if (!base.FluxSlot.Empty && base.FluxSlot.Itemstack.StackSize < filledLayerSlots - 1)
			{
				ItemStack itemstack = base.FluxSlot.Itemstack;
				int j = itemstack.StackSize;
				itemstack.StackSize = j + 1;
				this.MarkDirty(true, null);
				hotbarslot.TakeOut(1);
				this.UpdateMeshRefs();
				return true;
			}
			if (capi != null)
			{
				capi.TriggerIngameError(capi.World.Player, "toomuchflux", UIUtils.I18n(this.i18nPrefix + ".TooMuchFlux"));
			}
			return false;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00024B8F File Offset: 0x00022D8F
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			if (this.Api != null)
			{
				this.UpdateMeshRefs();
			}
		}

		// Token: 0x04000338 RID: 824
		private string[] bellLayerNames = new string[]
		{
			"churchbell-layer-bottom",
			"churchbell-layer-middle",
			"churchbell-layer-top",
			"churchbell-layer-topmost"
		};

		// Token: 0x04000339 RID: 825
		private float[] bellLayerHeights = new float[]
		{
			0f,
			0.75f,
			1.25f,
			1.75f
		};
	}
}
