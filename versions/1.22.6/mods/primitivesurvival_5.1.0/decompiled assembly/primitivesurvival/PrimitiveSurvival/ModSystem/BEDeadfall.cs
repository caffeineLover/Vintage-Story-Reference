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
	// Token: 0x02000008 RID: 8
	public class BEDeadfall : BlockEntityDisplayCase, IAnimalFoodSource, IPointOfInterest, ITexPositionSource
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00003343 File Offset: 0x00001543
		public ItemSlot BaitSlot
		{
			get
			{
				return this.inventory[0];
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003351 File Offset: 0x00001551
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003364 File Offset: 0x00001564
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

		// Token: 0x06000032 RID: 50 RVA: 0x00003378 File Offset: 0x00001578
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

		// Token: 0x06000033 RID: 51 RVA: 0x0000344A File Offset: 0x0000164A
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003477 File Offset: 0x00001677
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			if (this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000034A4 File Offset: 0x000016A4
		public bool IsSuitableFor(Entity entity, CreatureDiet diet)
		{
			return true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000034A7 File Offset: 0x000016A7
		public float ConsumeOnePortion(Entity entity)
		{
			return 1f;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000034AE File Offset: 0x000016AE
		public string Type
		{
			get
			{
				return "food";
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000034B5 File Offset: 0x000016B5
		public Vec3d Position
		{
			get
			{
				return this.Pos.ToVec3d().Add(0.5, 0.5, 0.5);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000034E2 File Offset: 0x000016E2
		public override string InventoryClassName
		{
			get
			{
				return "deadfall";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000034E9 File Offset: 0x000016E9
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000034F4 File Offset: 0x000016F4
		public BEDeadfall()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000035CB File Offset: 0x000017CB
		public bool TryClearContents()
		{
			if (!this.BaitSlot.Empty)
			{
				this.BaitSlot.TakeOut(1);
				return true;
			}
			return false;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000035EC File Offset: 0x000017EC
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

		// Token: 0x0600003E RID: 62 RVA: 0x00003684 File Offset: 0x00001884
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

		// Token: 0x0600003F RID: 63 RVA: 0x000037A8 File Offset: 0x000019A8
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

		// Token: 0x06000040 RID: 64 RVA: 0x00003828 File Offset: 0x00001A28
		public void StealBait(BlockPos pos)
		{
			BlockSnare block = this.Api.World.BlockAccessor.GetBlock(pos, 0) as BlockSnare;
			if (this.BaitSlot.TakeOut(1) != null && this.Api.Side == 1)
			{
				this.Api.ModLoader.GetModSystem<POIRegistry>(true).RemovePOI(this);
			}
			string blockPath = block.Code.Path;
			block = (this.Api.World.BlockAccessor.GetBlock(block.CodeWithPath(blockPath)) as BlockSnare);
			this.Api.World.BlockAccessor.SetBlock(block.BlockId, pos);
			this.MarkDirty(true, null);
			this.updateMesh(0);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000038E0 File Offset: 0x00001AE0
		public void TripTrap(BlockPos pos)
		{
			BlockDeadfall block = this.Api.World.BlockAccessor.GetBlock(pos, 0) as BlockDeadfall;
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
			block = (this.Api.World.BlockAccessor.GetBlock(block.CodeWithPath(blockPath)) as BlockDeadfall);
			this.Api.World.BlockAccessor.SetBlock(block.BlockId, pos);
			this.MarkDirty(true, null);
			this.updateMesh(0);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000039E4 File Offset: 0x00001BE4
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			if (this.Api.World.BlockAccessor.GetBlock(this.Pos, 0).Code.Path.Contains("tripped"))
			{
				sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-tripped", Array.Empty<object>()));
				return;
			}
			if (this.BaitStack != null)
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

		// Token: 0x06000043 RID: 67 RVA: 0x00003B08 File Offset: 0x00001D08
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapeBase = "primitivesurvival:shapes/";
			BlockDeadfall block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockDeadfall;
			ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
			ITexPositionSource tmpTextureSource = texture;
			string shapePath;
			if (block.FirstCodePart(1) == "set")
			{
				shapePath = "block/deadfall/deadfall-set";
			}
			else
			{
				shapePath = "block/deadfall/deadfall-tripped";
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

		// Token: 0x04000015 RID: 21
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

		// Token: 0x04000016 RID: 22
		protected static readonly Random Rnd = new Random();

		// Token: 0x04000017 RID: 23
		private readonly int maxSlots = 1;

		// Token: 0x04000018 RID: 24
		protected InventoryGeneric inventory;
	}
}
