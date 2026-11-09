using System;
using System.Linq;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200000D RID: 13
	public class BEFurrowedLand : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00008915 File Offset: 0x00006B15
		public override string InventoryClassName
		{
			get
			{
				return "furrowedland";
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000891C File Offset: 0x00006B1C
		public BEFurrowedLand()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
			this.tmpCloseNeibPos = new BlockPos[8];
			for (int i = 0; i < this.tmpCloseNeibPos.Length; i++)
			{
				this.tmpCloseNeibPos[i] = new BlockPos(0);
			}
			this.tmpFarNeibPos = new BlockPos[12];
			for (int j = 0; j < this.tmpFarNeibPos.Length; j++)
			{
				this.tmpFarNeibPos[j] = new BlockPos(0);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00008A42 File Offset: 0x00006C42
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00008A4A File Offset: 0x00006C4A
		public ItemSlot OtherSlot
		{
			get
			{
				return this.inventory[0];
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00008A58 File Offset: 0x00006C58
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00008A6B File Offset: 0x00006C6B
		public ItemStack OtherStack
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

		// Token: 0x0600009A RID: 154 RVA: 0x00008A80 File Offset: 0x00006C80
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.capi = (api as ICoreClientAPI);
			this.takeSound = new AssetLocation("game", "sounds/block/stickplace");
			this.roomRegistry = this.Api.ModLoader.GetModSystem<RoomRegistry>(true);
			if (base.Block.Attributes != null)
			{
				if (base.Block.Attributes["sideShape"].Exists)
				{
					this.sideShape = base.Block.Attributes["sideShape"].AsString(null);
				}
				if (base.Block.Attributes["renderSides"].Exists)
				{
					this.renderSides = base.Block.Attributes["renderSides"].AsString(null);
				}
			}
			this.RegisterGameTickListener(new Action<float>(this.OnPipeTick), this.FurrowedLandUpdateFrequency * 1000, 0);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00008B73 File Offset: 0x00006D73
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			this.currentConnections = "";
			base.OnBlockPlaced(byItemStack);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00008B88 File Offset: 0x00006D88
		private void OnPipeTick(float dt)
		{
			if (this.Api.Side == 2)
			{
				return;
			}
			Block block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 2);
			if (block.Code.Path.Contains("water") && this.OtherSlot.Empty && !block.Code.Path.Contains("saltwater"))
			{
				int baseX = this.Pos.X;
				int baseY = this.Pos.Y;
				int baseZ = this.Pos.Z;
				int dim = this.Pos.dimension;
				if (BEFurrowedLand.Rnd.NextDouble() < this.FurrowedLandBlockageChancePercent / 100.0)
				{
					bool createblockage = true;
					BlockPos upPos = this.tmpUpPos;
					upPos.dimension = dim;
					upPos.Set(baseX, baseY + 1, baseZ);
					if (this.Api.World.BlockAccessor.GetBlock(upPos, 0).BlockId != 0)
					{
						createblockage = false;
					}
					RoomRegistry roomRegistry;
					if ((roomRegistry = this.roomRegistry) == null)
					{
						roomRegistry = (this.roomRegistry = this.Api.ModLoader.GetModSystem<RoomRegistry>(true));
					}
					Room room = roomRegistry.GetRoomForPosition(upPos);
					if (room != null && room.ExitCount == 0)
					{
						createblockage = false;
					}
					if (createblockage)
					{
						Item blockageItem = this.Api.World.GetItem(new AssetLocation("game:" + this.blockageTypes[BEFurrowedLand.Rnd.Next(Enumerable.Count<string>(this.blockageTypes))]));
						if (blockageItem != null)
						{
							this.OtherStack = new ItemStack(blockageItem, BEFurrowedLand.Rnd.Next(3) + 1);
						}
						this.Api.World.BlockAccessor.SetBlock(0, this.Pos, 2);
						this.MarkDirty(true, null);
						return;
					}
				}
				else
				{
					BlockPos[] neibPos = this.tmpCloseNeibPos;
					for (int i = 0; i < neibPos.Length; i++)
					{
						neibPos[i].dimension = dim;
					}
					neibPos[0].Set(baseX + 1, baseY, baseZ);
					neibPos[1].Set(baseX + 1, baseY, baseZ - 1);
					neibPos[2].Set(baseX, baseY, baseZ - 1);
					neibPos[3].Set(baseX - 1, baseY, baseZ - 1);
					neibPos[4].Set(baseX, baseY, baseZ + 1);
					neibPos[5].Set(baseX + 1, baseY, baseZ + 1);
					neibPos[6].Set(baseX - 1, baseY, baseZ);
					neibPos[7].Set(baseX - 1, baseY, baseZ + 1);
					foreach (BlockPos neib in neibPos)
					{
						BlockEntityFarmland be = this.Api.World.BlockAccessor.GetBlockEntity(neib) as BlockEntityFarmland;
						if (be != null)
						{
							if ((double)be.MoistureLevel < this.FurrowedLandMinMoistureClose)
							{
								be.WaterFarmland(0.1f, false);
							}
							TreeAttribute tree = new TreeAttribute();
							be.ToTreeAttributes(tree);
						}
					}
					neibPos = this.tmpFarNeibPos;
					for (int j = 0; j < neibPos.Length; j++)
					{
						neibPos[j].dimension = dim;
					}
					neibPos[0].Set(baseX + 2, baseY, baseZ);
					neibPos[1].Set(baseX + 2, baseY, baseZ - 1);
					neibPos[2].Set(baseX + 2, baseY, baseZ + 1);
					neibPos[3].Set(baseX, baseY, baseZ - 2);
					neibPos[4].Set(baseX + 1, baseY, baseZ - 2);
					neibPos[5].Set(baseX - 1, baseY, baseZ - 2);
					neibPos[6].Set(baseX, baseY, baseZ + 2);
					neibPos[7].Set(baseX + 1, baseY, baseZ + 2);
					neibPos[8].Set(baseX - 1, baseY, baseZ + 2);
					neibPos[9].Set(baseX - 2, baseY, baseZ);
					neibPos[10].Set(baseX - 2, baseY, baseZ - 1);
					neibPos[11].Set(baseX - 2, baseY, baseZ + 1);
					foreach (BlockPos neib2 in neibPos)
					{
						BlockEntityFarmland be2 = this.Api.World.BlockAccessor.GetBlockEntity(neib2) as BlockEntityFarmland;
						if (be2 != null)
						{
							if ((double)be2.MoistureLevel < this.FurrowedLandMinMoistureFar)
							{
								be2.WaterFarmland(0.05f, false);
							}
							TreeAttribute tree2 = new TreeAttribute();
							be2.ToTreeAttributes(tree2);
						}
					}
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00008FCE File Offset: 0x000071CE
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			if (!this.OtherSlot.Empty)
			{
				sb.AppendLine(Lang.Get("primitivesurvival:item-blockage", Array.Empty<object>()));
			}
			sb.AppendLine();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00008FFA File Offset: 0x000071FA
		internal string GetConnections()
		{
			return this.currentConnections;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00009004 File Offset: 0x00007204
		internal bool AddConnection(BlockPos pos, BlockFacing facing)
		{
			char face = char.ToLower(facing.ToString().get_Chars(0));
			if (!this.currentConnections.Contains(face))
			{
				ReadOnlySpan<char> readOnlySpan = this.currentConnections;
				char c = face;
				this.currentConnections = readOnlySpan + new ReadOnlySpan<char>(ref c);
				return true;
			}
			return false;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00009054 File Offset: 0x00007254
		internal bool RemoveConnection(BlockPos pos, BlockFacing facing)
		{
			string face = char.ToLower(facing.ToString().get_Chars(0)).ToString();
			if (this.currentConnections.Contains(face))
			{
				this.currentConnections = this.currentConnections.Replace(face, string.Empty);
				return true;
			}
			return false;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000090A4 File Offset: 0x000072A4
		internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			ItemStack itemstack = playerSlot.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) != null)
			{
				JsonObject attributes = playerSlot.Itemstack.Collectible.Attributes;
				if (attributes != null && attributes["liquidContainerProps"].Exists)
				{
					ItemStack otherStack = this.OtherStack;
					if (((otherStack != null) ? otherStack.Collectible : null) != null && byPlayer.Entity.Controls.Sprint)
					{
						return true;
					}
				}
			}
			ItemStack otherStack2 = this.OtherStack;
			if (((otherStack2 != null) ? otherStack2.Collectible : null) != null && this.TryTake(byPlayer, this.OtherSlot))
			{
				this.Api.World.PlaySoundAt(this.takeSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
				(this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockFurrowedLand).FillPlacedBlock(this.Api.World, this.Pos);
				return true;
			}
			return false;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000091C8 File Offset: 0x000073C8
		private bool TryTake(IPlayer byPlayer, ItemSlot sourceSlot)
		{
			if (sourceSlot.Empty)
			{
				return false;
			}
			if (this.Api.Side == 1)
			{
				int totalItems = sourceSlot.Itemstack.StackSize;
				for (int i = 0; i < totalItems; i++)
				{
					ItemStack singleItem = sourceSlot.Itemstack.Clone();
					singleItem.StackSize = 1;
					double randX = (BEFurrowedLand.Rnd.NextDouble() - 0.5) * 0.2;
					double randZ = (BEFurrowedLand.Rnd.NextDouble() - 0.5) * 0.2;
					Vec3d spawnPos = this.Pos.ToVec3d().Add(0.5 + randX, 1.1, 0.5 + randZ);
					EntityItem entity = this.Api.World.SpawnItemEntity(singleItem, spawnPos, null) as EntityItem;
					if (entity != null)
					{
						entity.Pos.Motion.X = (double)((float)(BEFurrowedLand.Rnd.NextDouble() - 0.5) * 0.03f);
						entity.Pos.Motion.Y = (double)((float)BEFurrowedLand.Rnd.NextDouble() * 0.06f);
						entity.Pos.Motion.Z = (double)((float)(BEFurrowedLand.Rnd.NextDouble() - 0.5) * 0.03f);
					}
				}
				Block block = sourceSlot.Itemstack.Block;
				SoundAttributes? soundAttributes;
				if (block == null)
				{
					soundAttributes = default(SoundAttributes?);
				}
				else
				{
					BlockSounds sounds = block.Sounds;
					soundAttributes = ((sounds != null) ? new SoundAttributes?(sounds.Place) : default(SoundAttributes?));
				}
				SoundAttributes? sound = soundAttributes;
				this.Api.World.PlaySoundAt(sound ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer, 1f);
				sourceSlot.Itemstack = null;
				this.MarkDirty(true, null);
			}
			return true;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000093B4 File Offset: 0x000075B4
		public virtual MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, char side)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, null, 0, 0, 0, default(int?), null);
			if (mesh != null)
			{
				mesh.Translate(0f, 0f, 0f);
				if (side != 'e')
				{
					if (side != 'n')
					{
						switch (side)
						{
						case 's':
							mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), -1.5707964f, 0f, 0f);
							break;
						case 'u':
							mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 0f, 3.1415927f);
							break;
						case 'w':
							mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 0f, -1.5707964f);
							mesh.Translate(new Vec3f(0.0015f, 0.0015f, 0.0015f));
							break;
						}
					}
					else
					{
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.5707964f, 0f, 0f);
					}
				}
				else
				{
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 0f, 1.5707964f);
					mesh.Translate(new Vec3f(0.0015f, 0.0015f, 0.0015f));
				}
			}
			return mesh;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00009568 File Offset: 0x00007768
		public virtual string BuildShapePath(string partialPath)
		{
			if (partialPath == "")
			{
				return "";
			}
			string fullPath = "";
			if (!partialPath.Contains(":"))
			{
				fullPath = base.Block.Code.Domain + ":";
			}
			if (!partialPath.Contains("shapes/"))
			{
				fullPath += "shapes/";
			}
			return fullPath + partialPath;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000095D8 File Offset: 0x000077D8
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			MeshData mesh;
			this.capi.Tesselator.TesselateBlock(base.Block, ref mesh);
			if (mesh != null)
			{
				mesher.AddMeshData(mesh, 1);
			}
			if (!this.OtherSlot.Empty)
			{
				ITexPositionSource othertexture = tesselator.GetTextureSource(base.Block, 0, false);
				string othershapePath = this.BuildShapePath("block/pipe/soil/blockage-" + this.OtherStack.Collectible.Code.Path);
				mesh = this.GenMesh(this.capi, othershapePath, othertexture, 'd');
				if (mesh != null)
				{
					mesh.Translate(new Vec3f(0f, 0.85f - (float)this.OtherSlot.StackSize * 0.05f, 0f));
					mesh.Scale(new Vec3f(0.5f, 0.5f, 0.5f), 0.85f, 0.85f, 0.85f);
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, (float)(this.OtherSlot.StackSize * 45) * 0.017453292f, 0f);
					mesher.AddMeshData(mesh, 1);
				}
			}
			ITexPositionSource texture = tesselator.GetTextureSource(base.Block, 0, false);
			string shapePath = this.BuildShapePath(this.sideShape);
			string text = this.renderSides;
			for (int i = 0; i < text.Length; i++)
			{
				char side = text.get_Chars(i);
				mesh = null;
				if (!this.currentConnections.Contains(side) && shapePath != "")
				{
					mesh = this.GenMesh(this.capi, shapePath, texture, side);
				}
				if (mesh != null)
				{
					mesher.AddMeshData(mesh, 1);
				}
			}
			return true;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00009780 File Offset: 0x00007980
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.currentConnections = tree.GetString("currentConnections", null);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000979C File Offset: 0x0000799C
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("currentConnections", this.currentConnections);
		}

		// Token: 0x04000053 RID: 83
		private readonly int FurrowedLandUpdateFrequency = ModConfig.Loaded.FurrowedLandUpdateFrequency;

		// Token: 0x04000054 RID: 84
		private readonly double FurrowedLandBlockageChancePercent = ModConfig.Loaded.FurrowedLandBlockageChancePercent;

		// Token: 0x04000055 RID: 85
		private readonly double FurrowedLandMinMoistureClose = ModConfig.Loaded.FurrowedLandMinMoistureClose;

		// Token: 0x04000056 RID: 86
		private readonly double FurrowedLandMinMoistureFar = ModConfig.Loaded.FurrowedLandMinMoistureFar;

		// Token: 0x04000057 RID: 87
		public ICoreClientAPI capi;

		// Token: 0x04000058 RID: 88
		public ICoreServerAPI sapi;

		// Token: 0x04000059 RID: 89
		private static readonly Random Rnd = new Random();

		// Token: 0x0400005A RID: 90
		private readonly string[] blockageTypes = new string[]
		{
			"rot",
			"stick",
			"drygrass"
		};

		// Token: 0x0400005B RID: 91
		private AssetLocation takeSound;

		// Token: 0x0400005C RID: 92
		private readonly BlockPos tmpUpPos = new BlockPos(0);

		// Token: 0x0400005D RID: 93
		private readonly BlockPos[] tmpCloseNeibPos;

		// Token: 0x0400005E RID: 94
		private readonly BlockPos[] tmpFarNeibPos;

		// Token: 0x0400005F RID: 95
		private RoomRegistry roomRegistry;

		// Token: 0x04000060 RID: 96
		private readonly int maxSlots = 1;

		// Token: 0x04000061 RID: 97
		protected InventoryGeneric inventory;

		// Token: 0x04000062 RID: 98
		private string renderSides = "nsew";

		// Token: 0x04000063 RID: 99
		private string sideShape = "";

		// Token: 0x04000064 RID: 100
		private string currentConnections = "";
	}
}
