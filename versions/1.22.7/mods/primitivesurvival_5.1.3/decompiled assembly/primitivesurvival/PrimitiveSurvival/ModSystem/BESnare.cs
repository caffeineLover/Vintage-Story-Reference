using System;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000012 RID: 18
	public class BESnare : BlockEntityDisplayCase, IAnimalFoodSource, IPointOfInterest, ITexPositionSource
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000E347 File Offset: 0x0000C547
		public ItemSlot BaitSlot
		{
			get
			{
				return this.inventory[0];
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000E355 File Offset: 0x0000C555
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000E368 File Offset: 0x0000C568
		public ItemStack BaitStack
		{
			get
			{
				return this.inventory[0].Itemstack;
			}
			set
			{
				this.inventory[0].Itemstack = value;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000E37C File Offset: 0x0000C57C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (this.inventory != null && !this.BaitSlot.Empty)
			{
				if (this.BaitStack.Item != null)
				{
					if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) >= 0 && this.Api.Side == 1)
					{
						this.Api.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
						return;
					}
				}
				else if (this.BaitStack.Block != null && Array.IndexOf<string>(this.baitTypes, this.BaitStack.Block.FirstCodePart(0)) >= 0 && this.Api.Side == 1)
				{
					this.Api.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
				}
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000E44E File Offset: 0x0000C64E
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000E47B File Offset: 0x0000C67B
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		public bool IsSuitableFor(Entity entity, CreatureDiet diet)
		{
			return true;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000E4AB File Offset: 0x0000C6AB
		public float ConsumeOnePortion(Entity entity)
		{
			this.TryClearContents();
			return 1f;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000E4B9 File Offset: 0x0000C6B9
		public string Type
		{
			get
			{
				return "food";
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		public Vec3d Position
		{
			get
			{
				return this.Pos.ToVec3d().Add(0.5, 0.5, 0.5);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000E4ED File Offset: 0x0000C6ED
		public override string InventoryClassName
		{
			get
			{
				return "snare";
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		public BESnare()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000E5D3 File Offset: 0x0000C7D3
		public bool TryClearContents()
		{
			if (!this.BaitSlot.Empty)
			{
				this.BaitSlot.TakeOut(1);
				return true;
			}
			return false;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000E5F4 File Offset: 0x0000C7F4
		internal bool OnInteract(IPlayer byPlayer)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (playerSlot.Empty)
			{
				if (this.TryTake())
				{
					if (this.Api.Side == 1)
					{
						this.Api.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
					}
					return true;
				}
				return false;
			}
			else
			{
				if (playerSlot.Itemstack.Collectible.Attributes == null)
				{
					return false;
				}
				if (this.TryPut(playerSlot))
				{
					if (this.Api.Side == 1)
					{
						this.Api.ModLoader.GetModSystem<POIRegistry>(true).AddPOI(this);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000E68C File Offset: 0x0000C88C
		private bool TryPut(ItemSlot playerSlot)
		{
			int index = -1;
			ItemStack playerStack = playerSlot.Itemstack;
			if (this.inventory != null)
			{
				index = Enumerable.Count<ItemStack>(Enumerable.ToArray<ItemStack>(Enumerable.Select<ItemSlot, ItemStack>(Enumerable.Where<ItemSlot>(this.inventory, (ItemSlot slot) => !slot.Empty), (ItemSlot slot) => slot.Itemstack)));
				if (index >= this.maxSlots)
				{
					return false;
				}
			}
			if (playerStack.Item != null)
			{
				if (Array.IndexOf<string>(this.baitTypes, playerStack.Item.FirstCodePart(0)) < 0 || !this.BaitSlot.Empty)
				{
					return false;
				}
				index = 0;
			}
			else if (playerStack.Block != null)
			{
				if (Array.IndexOf<string>(this.baitTypes, playerStack.Block.FirstCodePart(0)) < 0 || !this.BaitSlot.Empty)
				{
					return false;
				}
				index = 0;
			}
			if (index == -1)
			{
				return false;
			}
			int moved = playerSlot.TryPutInto(this.Api.World, this.inventory[index], 1);
			if (moved > 0)
			{
				this.MarkDirty(true, null);
				return moved > 0;
			}
			return false;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000E7B0 File Offset: 0x0000C9B0
		private bool TryTake()
		{
			if (!this.BaitSlot.Empty)
			{
				ItemStack stack = this.BaitSlot.TakeOut(1);
				if (stack.StackSize > 0)
				{
					this.Api.World.SpawnItemEntity(stack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
				}
				this.updateMesh(0);
				this.MarkDirty(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000E830 File Offset: 0x0000CA30
		public void StealBait(BlockPos pos)
		{
			BlockSnare block = this.Api.World.BlockAccessor.GetBlock(pos, 0) as BlockSnare;
			this.BaitSlot.TakeOut(1);
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
			string blockPath = block.Code.Path;
			block = (this.Api.World.BlockAccessor.GetBlock(block.CodeWithPath(blockPath)) as BlockSnare);
			this.Api.World.BlockAccessor.SetBlock(block.BlockId, pos);
			this.MarkDirty(true, null);
			this.updateMesh(0);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
		public void TripTrap(BlockPos pos)
		{
			BlockSnare block = this.Api.World.BlockAccessor.GetBlock(pos, 0) as BlockSnare;
			ItemStack stack = this.BaitSlot.TakeOut(1);
			if (stack != null)
			{
				this.Api.World.SpawnItemEntity(stack, pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
				if (this.Api.Side == 1)
				{
					this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
				}
			}
			string blockPath = block.Code.Path;
			blockPath = blockPath.Replace("set", "tripped");
			block = (this.Api.World.BlockAccessor.GetBlock(block.CodeWithPath(blockPath)) as BlockSnare);
			this.Api.World.BlockAccessor.SetBlock(block.BlockId, pos);
			this.MarkDirty(true, null);
			this.updateMesh(0);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000E9EC File Offset: 0x0000CBEC
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			if (this.Api.World.BlockAccessor.GetBlock(this.Pos, 0).Code.Path.Contains("tripped"))
			{
				sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-deadfall-tripped", Array.Empty<object>()));
				return;
			}
			if (!this.BaitSlot.Empty)
			{
				if (this.BaitStack.Item != null)
				{
					if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) < 0)
					{
						sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-rotten", Array.Empty<object>()));
						return;
					}
					sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-ok", Array.Empty<object>()));
					return;
				}
				else if (this.BaitStack.Block != null)
				{
					if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Block.FirstCodePart(0)) < 0)
					{
						sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-rotten", Array.Empty<object>()));
						return;
					}
					sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-ok", Array.Empty<object>()));
					return;
				}
			}
			else
			{
				bool empty = this.BaitSlot.Empty;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000EB14 File Offset: 0x0000CD14
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapeBase = "primitivesurvival:shapes/";
			BlockSnare block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockSnare;
			ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
			ITexPositionSource tmpTextureSource = texture;
			string shapePath;
			if (block.FirstCodePart(1) == "set")
			{
				shapePath = "block/snare/snare-set";
			}
			else
			{
				shapePath = "block/snare/snare-tripped";
			}
			MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, -1, false);
			mesher.AddMeshData(mesh, 1);
			if (this.inventory != null && !this.BaitSlot.Empty)
			{
				bool tripped = true;
				if (block.FirstCodePart(1) == "set")
				{
					tripped = false;
				}
				if (this.BaitStack.Item != null)
				{
					if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) < 0)
					{
						Block tempblock = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
						tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock, 0, false);
					}
					else
					{
						tmpTextureSource = texture;
					}
				}
				else if (this.BaitStack.Block != null)
				{
					if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Block.FirstCodePart(0)) < 0)
					{
						Block tempblock2 = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
						tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock2, 0, false);
					}
					else
					{
						tmpTextureSource = texture;
					}
				}
				shapePath = "block/trapbait";
				mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, tmpTextureSource, 0, tripped);
				mesher.AddMeshData(mesh, 1);
			}
			return true;
		}

		// Token: 0x04000099 RID: 153
		private readonly string[] baitTypes = new string[]
		{
			"fruit",
			"grain",
			"legume",
			"meat",
			"vegetable",
			"jerky",
			"mushroom",
			"bread",
			"poultry",
			"pickledvegetable",
			"redmeat",
			"bushmeat",
			"cheese",
			"fisheggs",
			"fisheggscooked",
			"fish",
			"fishingbait"
		};

		// Token: 0x0400009A RID: 154
		protected static readonly Random Rnd = new Random();

		// Token: 0x0400009B RID: 155
		private readonly int maxSlots = 1;

		// Token: 0x0400009C RID: 156
		protected InventoryGeneric inventory;
	}
}
