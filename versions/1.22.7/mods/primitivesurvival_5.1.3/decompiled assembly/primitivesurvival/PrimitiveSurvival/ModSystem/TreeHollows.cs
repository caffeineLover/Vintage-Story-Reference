using System;
using System.Collections.Generic;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000054 RID: 84
	public class TreeHollows : ModSystem
	{
		// Token: 0x06000315 RID: 789 RVA: 0x000245BC File Offset: 0x000227BC
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			this.worldBlockAccessor = api.World.BlockAccessor;
			this.chunkSize = 32;
			this.treeTypes = new HashSet<string>();
			this.LoadTreeTypes(this.treeTypes);
			if (ModConfig.Loaded.TreeHollowsEnableDeveloperTools)
			{
				this.sapi.ChatCommands.GetOrCreate("hollow").WithDescription("Place a tree hollow with random items").RequiresPrivilege(Privilege.controlserver).HandleWith(new OnCommandDelegate(this.PlaceTreeHollowInFrontOfPlayer));
			}
			this.sapi.Event.GetWorldgenBlockAccessor(new WorldGenThreadDelegate(this.OnWorldGenBlockAccessor));
			this.sapi.Event.ChunkColumnGeneration(new ChunkColumnGenerationDelegate(this.OnChunkColumnGeneration), 5, "standard");
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00024688 File Offset: 0x00022888
		private TextCommandResult PlaceTreeHollowInFrontOfPlayer(TextCommandCallingArgs args)
		{
			IPlayer player = args.Caller.Player;
			this.PlaceTreeHollow(this.sapi.World.BlockAccessor, player.Entity.Pos.HorizontalAheadCopy(2.0).AsBlockPos);
			return TextCommandResult.Success("", null);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000246E1 File Offset: 0x000228E1
		public override bool ShouldLoad(EnumAppSide side)
		{
			return side == 1;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000246E8 File Offset: 0x000228E8
		private void LoadTreeTypes(ISet<string> treeTypes)
		{
			foreach (string variant in this.woods)
			{
				treeTypes.Add("log-grown-" + variant + "-ud");
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00024725 File Offset: 0x00022925
		private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
		{
			this.chunkGenBlockAccessor = chunkProvider.GetBlockAccessor(true);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00024734 File Offset: 0x00022934
		private void OnChunkColumnGeneration(IChunkColumnGenerateRequest request)
		{
			IServerChunk[] chunks = request.Chunks;
			int chunkX = request.ChunkX;
			int chunkZ = request.ChunkZ;
			if (!this.ShouldPlaceHollow())
			{
				return;
			}
			int hollowsPlacedCount = 0;
			for (int i = 0; i < chunks.Length - 2; i += 2)
			{
				BlockPos blockPos = new BlockPos(0, 0, 0, 0);
				for (int x = 0; x < this.chunkSize / 2; x += 2)
				{
					for (int z = 0; z < this.chunkSize / 2; z += 2)
					{
						int terrainHeight = this.worldBlockAccessor.GetTerrainMapheightAt(blockPos);
						for (int y = terrainHeight; y < terrainHeight + 6; y++)
						{
							if (hollowsPlacedCount >= ModConfig.Loaded.TreeHollowsMaxPerChunk)
							{
								return;
							}
							blockPos.X = chunkX * this.chunkSize + x;
							blockPos.Y = y;
							blockPos.Z = chunkZ * this.chunkSize + z;
							BlockPos hollowLocation = this.TryGetHollowLocation(blockPos);
							if (hollowLocation != null && this.PlaceTreeHollow(this.chunkGenBlockAccessor, hollowLocation))
							{
								hollowsPlacedCount++;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00024848 File Offset: 0x00022A48
		private BlockPos TryGetHollowLocation(BlockPos pos)
		{
			Block block = this.chunkGenBlockAccessor.GetBlock(pos, 0);
			if (this.IsTreeLog(block))
			{
				int posY = pos.Y;
				while (posY >= 0)
				{
					for (;;)
					{
						int y = pos.Y;
						pos.Y = y - 1;
						if (y <= 0)
						{
							break;
						}
						Block underBlock = this.chunkGenBlockAccessor.GetBlock(pos, 0);
						if (!this.IsTreeLog(underBlock))
						{
							goto Block_2;
						}
					}
					posY--;
					continue;
					Block_2:
					return pos.UpCopy(1);
				}
			}
			return null;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000248B1 File Offset: 0x00022AB1
		private bool IsTreeLog(Block block)
		{
			return this.treeTypes.Contains(block.Code.Path);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000248CC File Offset: 0x00022ACC
		private bool PlaceTreeHollow(IBlockAccessor blockAccessor, BlockPos pos)
		{
			int upCount = this.sapi.World.Rand.Next(4);
			if (blockAccessor.GetBlock(pos.UpCopy(upCount), 0).FirstCodePart(0) == "log")
			{
				pos = pos.Up(upCount);
			}
			Block treeBlock = blockAccessor.GetBlock(pos, 0);
			string woodType = "pine";
			if (treeBlock.FirstCodePart(0) == "log")
			{
				woodType = treeBlock.FirstCodePart(2);
			}
			string hollowType = "up";
			if (this.sapi.World.Rand.Next(2) == 1)
			{
				hollowType = "up2";
			}
			if (blockAccessor.GetBlock(pos.DownCopy(1), 0).Fertility > 0)
			{
				if (this.sapi.World.Rand.Next(2) == 1)
				{
					hollowType = "base";
				}
				else
				{
					hollowType = "base2";
				}
			}
			string withPath = string.Concat(new string[]
			{
				"primitivesurvival:treehollowgrown-",
				hollowType,
				"-",
				woodType,
				"-",
				this.dirs[this.sapi.World.Rand.Next(4)]
			});
			int withBlockID = this.sapi.WorldManager.GetBlockId(new AssetLocation(withPath));
			Block withBlock = blockAccessor.GetBlock(withBlockID);
			blockAccessor.SetBlock(0, pos);
			if (withBlock.TryPlaceBlockForWorldGen(blockAccessor, pos, BlockFacing.UP, null, null))
			{
				BlockTreeHollowGrown block = blockAccessor.GetBlock(pos, 0) as BlockTreeHollowGrown;
				if (block.EntityClass != null && block.EntityClass == withBlock.EntityClass)
				{
					blockAccessor.SpawnBlockEntity(block.EntityClass, pos, null);
					if (blockAccessor.GetBlockEntity(pos) is BETreeHollowGrown)
					{
						BETreeHollowGrown hollow = blockAccessor.GetBlockEntity(pos) as BETreeHollowGrown;
						if (block != null)
						{
							IEnumerable<ItemStack> makeStacks = this.MakeItemStacks(block, this.sapi);
							if (makeStacks != null)
							{
								this.AddItemStacks(hollow, makeStacks);
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00024AAC File Offset: 0x00022CAC
		private bool ShouldPlaceHollow()
		{
			int randomNumber = this.sapi.World.Rand.Next(0, 100);
			return randomNumber > 0 && (float)randomNumber <= ModConfig.Loaded.TreeHollowsSpawnProbability * 100f;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00024AF0 File Offset: 0x00022CF0
		public IEnumerable<ItemStack> MakeItemStacks(Block block, ICoreServerAPI sapi)
		{
			if (sapi == null)
			{
				return null;
			}
			ShuffleBag<string> shuffleBag = this.MakeShuffleBag(block, sapi);
			Dictionary<string, ItemStack> itemStacks = new Dictionary<string, ItemStack>();
			int minDrops = 1;
			if (ModConfig.Loaded.TreeHollowsMaxItems < 1)
			{
				minDrops = 0;
			}
			int maxDrops = 8;
			if (ModConfig.Loaded.TreeHollowsMaxItems < 8)
			{
				maxDrops = ModConfig.Loaded.TreeHollowsMaxItems;
			}
			int grabCount = sapi.World.Rand.Next(minDrops, maxDrops);
			for (int i = 0; i < grabCount; i++)
			{
				string nextItem = shuffleBag.Next();
				if (nextItem.Contains("item-"))
				{
					nextItem = nextItem.Replace("item-", "");
					AssetLocation aloc = new AssetLocation(nextItem);
					if (aloc != null)
					{
						Item item = sapi.World.GetItem(aloc);
						if (item != null)
						{
							if (itemStacks.ContainsKey(nextItem))
							{
								ItemStack itemStack = itemStacks[nextItem];
								int stackSize = itemStack.StackSize;
								itemStack.StackSize = stackSize + 1;
							}
							else
							{
								itemStacks.Add(nextItem, new ItemStack(item, 1));
							}
						}
					}
				}
				else
				{
					nextItem = nextItem.Replace("block-", "");
					AssetLocation aloc2 = new AssetLocation(nextItem);
					if (aloc2 != null)
					{
						Block item2 = sapi.World.GetBlock(aloc2);
						if (item2 != null)
						{
							if (itemStacks.ContainsKey(nextItem))
							{
								ItemStack itemStack2 = itemStacks[nextItem];
								int stackSize = itemStack2.StackSize;
								itemStack2.StackSize = stackSize + 1;
							}
							else
							{
								itemStacks.Add(nextItem, new ItemStack(item2, 1));
							}
						}
					}
				}
			}
			return itemStacks.Values;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00024C6C File Offset: 0x00022E6C
		public void AddItemStacks(IBlockEntityContainer hollow, IEnumerable<ItemStack> itemStacks)
		{
			int slotNumber = 0;
			if (itemStacks != null)
			{
				foreach (ItemStack itemStack in itemStacks)
				{
					slotNumber = Math.Min(slotNumber, hollow.Inventory.Count - 1);
					hollow.Inventory[slotNumber].Itemstack = itemStack;
					slotNumber++;
				}
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00024CDC File Offset: 0x00022EDC
		private ShuffleBag<string> MakeShuffleBag(Block block, ICoreServerAPI sapi)
		{
			ShuffleBag<string> shuffleBag = new ShuffleBag<string>(100, sapi.World.Rand);
			if (block == null)
			{
				return shuffleBag;
			}
			JsonObject attributes = block.Attributes;
			JsonObject psAttributes = (attributes != null) ? attributes["primitivesurvival"] : null;
			if (psAttributes == null)
			{
				return shuffleBag;
			}
			string hollowType = psAttributes["treeHollowType"].AsString("all");
			JsonObject contentsByHollowType = psAttributes["treeHollowContentsByHollowType"];
			if (hollowType != "all")
			{
				this.AddItemsToShuffleBag(shuffleBag, contentsByHollowType[hollowType]);
			}
			this.AddItemsToShuffleBag(shuffleBag, contentsByHollowType["all"]);
			return shuffleBag;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00024D70 File Offset: 0x00022F70
		private void AddItemsToShuffleBag(ShuffleBag<string> shuffleBag, JsonObject attributesObjectArray)
		{
			if (shuffleBag == null)
			{
				return;
			}
			if (attributesObjectArray == null)
			{
				return;
			}
			JsonObject[] attArray = attributesObjectArray.AsArray();
			if (attArray == null)
			{
				return;
			}
			foreach (JsonObject collectible in attArray)
			{
				if (collectible["code"].Exists)
				{
					shuffleBag.Add(collectible["code"].AsString(null), collectible["amount"].AsInt(1));
				}
			}
		}

		// Token: 0x04000185 RID: 389
		private const int MinItems = 1;

		// Token: 0x04000186 RID: 390
		private const int MaxItems = 8;

		// Token: 0x04000187 RID: 391
		private ICoreServerAPI sapi;

		// Token: 0x04000188 RID: 392
		private int chunkSize;

		// Token: 0x04000189 RID: 393
		private ISet<string> treeTypes;

		// Token: 0x0400018A RID: 394
		private IBlockAccessor chunkGenBlockAccessor;

		// Token: 0x0400018B RID: 395
		private IBlockAccessor worldBlockAccessor;

		// Token: 0x0400018C RID: 396
		private readonly string[] dirs = new string[]
		{
			"north",
			"south",
			"east",
			"west"
		};

		// Token: 0x0400018D RID: 397
		private readonly string[] woods = new string[]
		{
			"acacia",
			"birch",
			"kapok",
			"larch",
			"maple",
			"oak",
			"pine",
			"walnut"
		};
	}
}
