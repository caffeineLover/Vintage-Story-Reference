using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000016 RID: 22
	public class BEWoodSupportSpikes : BlockEntityDisplayCase, IAnimalFoodSource, IPointOfInterest, ITexPositionSource
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00011AA0 File Offset: 0x0000FCA0
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (this.inventory != null && !this.inventory[this.maxSlots - 1].Empty && this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00011AFB File Offset: 0x0000FCFB
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00011B28 File Offset: 0x0000FD28
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00011B55 File Offset: 0x0000FD55
		public bool IsSuitableFor(Entity entity, CreatureDiet diet)
		{
			return true;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00011B58 File Offset: 0x0000FD58
		public float ConsumeOnePortion(Entity entity)
		{
			return 1f;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00011B5F File Offset: 0x0000FD5F
		public string Type
		{
			get
			{
				return "food";
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00011B66 File Offset: 0x0000FD66
		public Vec3d Position
		{
			get
			{
				return this.Pos.ToVec3d().Add(0.5, 0.5, 0.5);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00011B93 File Offset: 0x0000FD93
		public override string InventoryClassName
		{
			get
			{
				return "woodsupportspikes";
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00011B9A File Offset: 0x0000FD9A
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00011BA2 File Offset: 0x0000FDA2
		public BEWoodSupportSpikes()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		internal bool OnInteract(IPlayer byPlayer)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (playerSlot.Empty)
			{
				if (this.TryTake(byPlayer))
				{
					if (this.inventory[this.maxSlots - 1].Empty && this.Api.Side == 1)
					{
						this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
					}
					return true;
				}
				return false;
			}
			else
			{
				if (this.TryPut(playerSlot))
				{
					if (!this.inventory[this.maxSlots - 1].Empty && this.Api.Side == 1)
					{
						this.Api.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00011C8C File Offset: 0x0000FE8C
		private bool TryPut(ItemSlot playerSlot)
		{
			ItemStack playerStack = playerSlot.Itemstack;
			int availSlot = this.maxSlots;
			for (int slot = this.maxSlots - 1; slot >= 0; slot--)
			{
				if (this.inventory[slot].Empty)
				{
					availSlot = slot;
				}
			}
			if (availSlot == this.maxSlots)
			{
				return false;
			}
			bool canPlace = false;
			if (availSlot < this.maxSlots - 1)
			{
				if (playerStack.Item != null)
				{
					if (playerStack.Item.Code.Path == "drygrass")
					{
						canPlace = true;
					}
				}
				else if (playerStack.Block.BlockMaterial == 13)
				{
					canPlace = true;
				}
			}
			else if (playerStack.Block != null && playerStack.Block.Fertility > 0)
			{
				canPlace = true;
			}
			if (canPlace)
			{
				playerSlot.TryPutInto(this.Api.World, this.inventory[availSlot], 1);
				this.MarkDirty(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00011D68 File Offset: 0x0000FF68
		private bool TryTake(IPlayer byPlayer)
		{
			int usedSlot = -1;
			for (int slot = 0; slot < this.maxSlots; slot++)
			{
				if (!this.inventory[slot].Empty)
				{
					usedSlot = slot;
				}
			}
			if (usedSlot > -1)
			{
				ItemStack stack = this.inventory[usedSlot].TakeOut(1);
				if (stack.StackSize > 0)
				{
					byPlayer.InventoryManager.TryGiveItemstack(stack, false);
				}
				this.MarkDirty(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00011DD8 File Offset: 0x0000FFD8
		public string GetBlockName(IWorldAccessor world, BlockPos pos)
		{
			if (this.inventory != null && !this.inventory[3].Empty)
			{
				ItemStack stack = this.inventory[3].Itemstack;
				if (stack.Block != null)
				{
					return Lang.Get("game:block-" + stack.Block.Code.Path, Array.Empty<object>());
				}
			}
			return Lang.Get("primitivesurvival:blockdesc-woodsuppportspikes-concealment-needed", Array.Empty<object>());
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00011E50 File Offset: 0x00010050
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			if (this.inventory != null)
			{
				string msg = "";
				for (int i = 0; i < 3; i++)
				{
					if (this.inventory[i].Empty)
					{
						msg = Lang.Get("primitivesurvival:blockdesc-woodsuppportspikes-concealment-size-" + i.ToString(), Array.Empty<object>());
						break;
					}
				}
				if (!string.IsNullOrEmpty(msg))
				{
					sb.Append(Lang.Get("primitivesurvival:blockdesc-woodsuppportspikes-concealment-add", Array.Empty<object>()) + " ").Append(msg).Append(" " + Lang.Get("primitivesurvival:blockdesc-woodsuppportspikes-concealment-plants", Array.Empty<object>()));
				}
				else if (this.inventory[3].Empty)
				{
					sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-woodsuppportspikes-concealment-dirt", Array.Empty<object>()));
				}
				if (this.inventory[3].Empty)
				{
					sb.AppendLine().AppendLine();
				}
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00011F44 File Offset: 0x00010144
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapeBase = "primitivesurvival:shapes/";
			BlockWoodSupportSpikes block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockWoodSupportSpikes;
			if (block == null)
			{
				return true;
			}
			ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
			string shapePath = "block/woodsupportspikes";
			MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, -1);
			mesher.AddMeshData(mesh, 1);
			if (this.inventory != null)
			{
				int usedSlots = 0;
				for (int i = 0; i < this.maxSlots; i++)
				{
					if (!this.inventory[i].Empty)
					{
						usedSlots++;
					}
				}
				if (usedSlots > 0 && usedSlots < 4)
				{
					Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath("foilage-" + usedSlots.ToString()));
					shapePath = "block/foilage";
					ITexPositionSource tmpTextureSource = tesselator.GetTextureSource(tmpBlock, 0, false);
					mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, tmpTextureSource, usedSlots);
					mesher.AddMeshData(mesh, 1);
				}
				else if (usedSlots == 4)
				{
					shapePath = "block/foilage";
					ITexPositionSource tmpTextureSource = tesselator.GetTextureSource(this.inventory[3].Itemstack.Block, 0, false);
					mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, tmpTextureSource, usedSlots);
					mesher.AddMeshData(mesh, 1);
				}
			}
			return true;
		}

		// Token: 0x040000BE RID: 190
		protected static readonly Random Rnd = new Random();

		// Token: 0x040000BF RID: 191
		private readonly int maxSlots = 4;

		// Token: 0x040000C0 RID: 192
		protected InventoryGeneric inventory;
	}
}
