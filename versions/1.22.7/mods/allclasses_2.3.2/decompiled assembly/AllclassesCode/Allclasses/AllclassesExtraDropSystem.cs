using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Allclasses
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AllclassesExtraDropSystem : ModSystem
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000026F8 File Offset: 0x000008F8
		public override bool ShouldLoad(EnumAppSide side)
		{
			return side == 1;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026FE File Offset: 0x000008FE
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.api = api;
			api.Event.BreakBlock += new BlockBreakDelegate(this.OnBreakBlock);
			api.Event.DidBreakBlock += new BlockBrokenDelegate(this.OnDidBreakBlock);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002738 File Offset: 0x00000938
		private void OnBreakBlock(IServerPlayer byPlayer, BlockSelection blockSel, ref float dropQuantityMultiplier, ref EnumHandling handling)
		{
			if (this.api == null || ((byPlayer != null) ? byPlayer.Entity : null) == null || ((blockSel != null) ? blockSel.Position : null) == null)
			{
				return;
			}
			if (!AllclassesExtraDropSystem.IsFruitTreeCuttingDropBlock(this.api.World.BlockAccessor.GetBlock(blockSel.Position)))
			{
				return;
			}
			BlockEntityFruitTreeBranch branch = this.api.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityFruitTreeBranch;
			if (branch == null || string.IsNullOrEmpty(branch.TreeType))
			{
				return;
			}
			this.pendingFruitTreeBreaks[AllclassesExtraDropSystem.GetBreakKey(byPlayer.PlayerUID, blockSel.Position)] = new AllclassesExtraDropSystem.FruitTreeBreakInfo(branch.TreeType, branch.FoliageState != 5);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027F8 File Offset: 0x000009F8
		private void OnDidBreakBlock(IServerPlayer byPlayer, int oldblockId, BlockSelection blockSel)
		{
			if (this.api == null || ((byPlayer != null) ? byPlayer.Entity : null) == null || ((blockSel != null) ? blockSel.Position : null) == null || oldblockId <= 0 || oldblockId >= this.api.World.Blocks.Count)
			{
				return;
			}
			Block block = this.api.World.Blocks[oldblockId];
			if (((block != null) ? block.Code : null) == null)
			{
				return;
			}
			string path = block.Code.Path;
			if (path.StartsWith("charcoalpile-", 4))
			{
				this.TrySpawnExtraItem(byPlayer, blockSel.Position, "charcoalPileDropRate", new AssetLocation("game", "charcoal"), 1);
				return;
			}
			if (path.StartsWith("log-grown-", 4))
			{
				this.TrySpawnExtraLog(byPlayer, blockSel.Position, block);
				return;
			}
			if (path.StartsWith("loosestick-", 4))
			{
				this.TrySpawnExtraItem(byPlayer, blockSel.Position, "stickDropRate", new AssetLocation("game", "stick"), 1);
				return;
			}
			if (path.StartsWith("crop-", 4))
			{
				BlockPos position = blockSel.Position;
				Block block2 = block;
				string statCode = "produceDropRate";
				Func<BlockDropItemStack, bool> shouldUseDrop;
				if ((shouldUseDrop = AllclassesExtraDropSystem.<>O.<0>__IsProduceDrop) == null)
				{
					shouldUseDrop = (AllclassesExtraDropSystem.<>O.<0>__IsProduceDrop = new Func<BlockDropItemStack, bool>(AllclassesExtraDropSystem.IsProduceDrop));
				}
				this.TrySpawnExtraConfiguredDrops(byPlayer, position, block2, statCode, shouldUseDrop);
				return;
			}
			if (path.StartsWith("leaves-", 4) || path.StartsWith("leavesbranchy-", 4) || path.StartsWith("leavesnarrow-", 4))
			{
				BlockPos position2 = blockSel.Position;
				Block block3 = block;
				string statCode2 = "treeseedDropRate";
				Func<BlockDropItemStack, bool> shouldUseDrop2;
				if ((shouldUseDrop2 = AllclassesExtraDropSystem.<>O.<1>__IsTreeSeedDrop) == null)
				{
					shouldUseDrop2 = (AllclassesExtraDropSystem.<>O.<1>__IsTreeSeedDrop = new Func<BlockDropItemStack, bool>(AllclassesExtraDropSystem.IsTreeSeedDrop));
				}
				this.TrySpawnExtraConfiguredDrops(byPlayer, position2, block3, statCode2, shouldUseDrop2);
				BlockPos position3 = blockSel.Position;
				Block block4 = block;
				string statCode3 = "stickDropRate";
				Func<BlockDropItemStack, bool> shouldUseDrop3;
				if ((shouldUseDrop3 = AllclassesExtraDropSystem.<>O.<2>__IsStickDrop) == null)
				{
					shouldUseDrop3 = (AllclassesExtraDropSystem.<>O.<2>__IsStickDrop = new Func<BlockDropItemStack, bool>(AllclassesExtraDropSystem.IsStickDrop));
				}
				this.TrySpawnExtraConfiguredDrops(byPlayer, position3, block4, statCode3, shouldUseDrop3);
				return;
			}
			string fruitTreePart;
			if (path.StartsWith("fruittree-", 4) && block.Variant != null && block.Variant.TryGetValue("type", ref fruitTreePart))
			{
				bool flag = fruitTreePart == "branch" || fruitTreePart == "cutting";
				if (flag)
				{
					this.TryGiveExtraFruitTreeDrop(byPlayer, blockSel.Position);
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002A24 File Offset: 0x00000C24
		private void TryGiveExtraFruitTreeDrop(IServerPlayer byPlayer, BlockPos pos)
		{
			AllclassesExtraDropSystem.FruitTreeBreakInfo breakInfo;
			if (this.api == null || !this.pendingFruitTreeBreaks.Remove(AllclassesExtraDropSystem.GetBreakKey(byPlayer.PlayerUID, pos), ref breakInfo))
			{
				return;
			}
			float extraRate = byPlayer.Entity.Stats.GetBlended("fruittreeDropRate") - 1f;
			if (extraRate <= 0f)
			{
				return;
			}
			ItemStack stack;
			if (breakInfo.IsAlive)
			{
				Block cuttingBlock = this.api.World.GetBlock(new AssetLocation("game", "fruittree-cutting"));
				if (cuttingBlock == null)
				{
					return;
				}
				stack = new ItemStack(cuttingBlock, 1);
				stack.Attributes.SetString("type", breakInfo.TreeType);
			}
			else
			{
				Item firewood = this.api.World.GetItem(new AssetLocation("game", "firewood"));
				if (firewood == null)
				{
					return;
				}
				stack = new ItemStack(firewood, 2);
			}
			int extraQuantity = this.RollExtraQuantity((float)stack.StackSize * extraRate);
			if (extraQuantity <= 0)
			{
				return;
			}
			stack.StackSize = extraQuantity;
			this.GiveOrSpawnItemStack(byPlayer, pos, stack);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B20 File Offset: 0x00000D20
		private void TrySpawnExtraLog(IServerPlayer byPlayer, BlockPos pos, Block brokenLog)
		{
			string wood;
			if (this.api == null || brokenLog.Variant == null || !brokenLog.Variant.TryGetValue("wood", ref wood))
			{
				return;
			}
			Block dropBlock = this.api.World.GetBlock(new AssetLocation(brokenLog.Code.Domain, "log-placed-" + wood + "-ud"));
			if (dropBlock == null)
			{
				return;
			}
			this.TrySpawnExtraStack(byPlayer, pos, "woodDropRate", new ItemStack(dropBlock, 1), 1);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B9C File Offset: 0x00000D9C
		private void TrySpawnExtraItem(IServerPlayer byPlayer, BlockPos pos, string statCode, AssetLocation itemCode, int baseQuantity)
		{
			if (this.api == null)
			{
				return;
			}
			Item item = this.api.World.GetItem(itemCode);
			if (item == null)
			{
				return;
			}
			this.TrySpawnExtraStack(byPlayer, pos, statCode, new ItemStack(item, 1), baseQuantity);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002BDC File Offset: 0x00000DDC
		private void TrySpawnExtraStack(IServerPlayer byPlayer, BlockPos pos, string statCode, ItemStack stack, int baseQuantity)
		{
			if (this.api == null)
			{
				return;
			}
			float extraRate = byPlayer.Entity.Stats.GetBlended(statCode) - 1f;
			if (extraRate <= 0f)
			{
				return;
			}
			int extraQuantity = this.ConsumeAccumulatedExtraQuantity(byPlayer.PlayerUID, statCode, stack.Collectible.Code, (float)baseQuantity * extraRate);
			if (extraQuantity <= 0)
			{
				return;
			}
			stack.StackSize = extraQuantity;
			this.api.World.SpawnItemEntity(stack, new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C8C File Offset: 0x00000E8C
		private void TrySpawnExtraConfiguredDrops(IServerPlayer byPlayer, BlockPos pos, Block block, string statCode, Func<BlockDropItemStack, bool> shouldUseDrop)
		{
			if (this.api == null || block.Drops == null)
			{
				return;
			}
			float extraRate = byPlayer.Entity.Stats.GetBlended(statCode) - 1f;
			if (extraRate <= 0f)
			{
				return;
			}
			foreach (BlockDropItemStack configuredDrop in block.Drops)
			{
				if (shouldUseDrop.Invoke(configuredDrop))
				{
					BlockDropItemStack blockDropItemStack = configuredDrop.Clone();
					blockDropItemStack.DropModbyStat = null;
					ItemStack stack = blockDropItemStack.ToRandomItemstackForPlayer(byPlayer, this.api.World, extraRate);
					if (stack != null)
					{
						this.api.World.SpawnItemEntity(stack, new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), null);
					}
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D68 File Offset: 0x00000F68
		private int ConsumeAccumulatedExtraQuantity(string playerUid, string statCode, AssetLocation collectibleCode, float amount)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted(playerUid);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(statCode);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(collectibleCode);
			string key = defaultInterpolatedStringHandler.ToStringAndClear();
			float progress;
			this.dropProgressByPlayerAndStat.TryGetValue(key, ref progress);
			progress += amount;
			int wholeItems = (int)progress;
			this.dropProgressByPlayerAndStat[key] = progress - (float)wholeItems;
			return wholeItems;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002DE0 File Offset: 0x00000FE0
		private int RollExtraQuantity(float amount)
		{
			int wholeItems = (int)amount;
			float fraction = amount - (float)wholeItems;
			if (this.random.NextDouble() >= (double)fraction)
			{
				return wholeItems;
			}
			return wholeItems + 1;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E0C File Offset: 0x0000100C
		private void GiveOrSpawnItemStack(IServerPlayer byPlayer, BlockPos pos, ItemStack stack)
		{
			if (this.api == null)
			{
				return;
			}
			if (byPlayer.InventoryManager.TryGiveItemstack(stack, true))
			{
				return;
			}
			if (stack.StackSize > 0)
			{
				this.api.World.SpawnItemEntity(stack, new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), null);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E88 File Offset: 0x00001088
		private static bool IsFruitTreeCuttingDropBlock(Block block)
		{
			AssetLocation code = block.Code;
			string partType;
			bool flag = code != null && code.Path.StartsWith("fruittree-", 4) && block.Variant != null && block.Variant.TryGetValue("type", ref partType);
			if (flag)
			{
				bool flag2 = partType == "branch" || partType == "cutting";
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002EF8 File Offset: 0x000010F8
		private static string GetBreakKey(string playerUid, BlockPos pos)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted(playerUid);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(pos);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002F34 File Offset: 0x00001134
		private static bool IsProduceDrop(BlockDropItemStack drop)
		{
			ItemStack resolvedItemstack = drop.ResolvedItemstack;
			string text;
			if (resolvedItemstack == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = resolvedItemstack.Collectible.Code;
				text = ((code != null) ? code.Path : null);
			}
			string text2;
			if ((text2 = text) == null)
			{
				AssetLocation code2 = drop.Code;
				text2 = ((code2 != null) ? code2.Path : null);
			}
			string path = text2;
			return path != null && !path.StartsWith("seeds-", 4);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002F90 File Offset: 0x00001190
		private static bool IsTreeSeedDrop(BlockDropItemStack drop)
		{
			ItemStack resolvedItemstack = drop.ResolvedItemstack;
			string text;
			if (resolvedItemstack == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = resolvedItemstack.Collectible.Code;
				text = ((code != null) ? code.Path : null);
			}
			string text2;
			if ((text2 = text) == null)
			{
				AssetLocation code2 = drop.Code;
				text2 = ((code2 != null) ? code2.Path : null);
			}
			string path = text2;
			return path != null && path.StartsWith("treeseed-", 4);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002FE8 File Offset: 0x000011E8
		private static bool IsStickDrop(BlockDropItemStack drop)
		{
			ItemStack resolvedItemstack = drop.ResolvedItemstack;
			string text;
			if (resolvedItemstack == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = resolvedItemstack.Collectible.Code;
				text = ((code != null) ? code.Path : null);
			}
			string text2;
			if ((text2 = text) == null)
			{
				AssetLocation code2 = drop.Code;
				text2 = ((code2 != null) ? code2.Path : null);
			}
			return text2 == "stick";
		}

		// Token: 0x04000007 RID: 7
		private readonly Dictionary<string, float> dropProgressByPlayerAndStat = new Dictionary<string, float>();

		// Token: 0x04000008 RID: 8
		private readonly Dictionary<string, AllclassesExtraDropSystem.FruitTreeBreakInfo> pendingFruitTreeBreaks = new Dictionary<string, AllclassesExtraDropSystem.FruitTreeBreakInfo>();

		// Token: 0x04000009 RID: 9
		private readonly Random random = new Random();

		// Token: 0x0400000A RID: 10
		[Nullable(2)]
		private ICoreServerAPI api;

		// Token: 0x0200000A RID: 10
		[Nullable(0)]
		private readonly struct FruitTreeBreakInfo : IEquatable<AllclassesExtraDropSystem.FruitTreeBreakInfo>
		{
			// Token: 0x06000037 RID: 55 RVA: 0x000035A4 File Offset: 0x000017A4
			public FruitTreeBreakInfo(string TreeType, bool IsAlive)
			{
				this.TreeType = TreeType;
				this.IsAlive = IsAlive;
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000038 RID: 56 RVA: 0x000035B4 File Offset: 0x000017B4
			// (set) Token: 0x06000039 RID: 57 RVA: 0x000035BC File Offset: 0x000017BC
			public string TreeType { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600003A RID: 58 RVA: 0x000035C5 File Offset: 0x000017C5
			// (set) Token: 0x0600003B RID: 59 RVA: 0x000035CD File Offset: 0x000017CD
			public bool IsAlive { get; set; }

			// Token: 0x0600003C RID: 60 RVA: 0x000035D8 File Offset: 0x000017D8
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("FruitTreeBreakInfo");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600003D RID: 61 RVA: 0x00003624 File Offset: 0x00001824
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("TreeType = ");
				builder.Append(this.TreeType);
				builder.Append(", IsAlive = ");
				builder.Append(this.IsAlive.ToString());
				return true;
			}

			// Token: 0x0600003E RID: 62 RVA: 0x00003672 File Offset: 0x00001872
			[CompilerGenerated]
			public static bool operator !=(AllclassesExtraDropSystem.FruitTreeBreakInfo left, AllclassesExtraDropSystem.FruitTreeBreakInfo right)
			{
				return !(left == right);
			}

			// Token: 0x0600003F RID: 63 RVA: 0x0000367E File Offset: 0x0000187E
			[CompilerGenerated]
			public static bool operator ==(AllclassesExtraDropSystem.FruitTreeBreakInfo left, AllclassesExtraDropSystem.FruitTreeBreakInfo right)
			{
				return left.Equals(right);
			}

			// Token: 0x06000040 RID: 64 RVA: 0x00003688 File Offset: 0x00001888
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<string>.Default.GetHashCode(this.<TreeType>k__BackingField) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsAlive>k__BackingField);
			}

			// Token: 0x06000041 RID: 65 RVA: 0x000036B1 File Offset: 0x000018B1
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is AllclassesExtraDropSystem.FruitTreeBreakInfo && this.Equals((AllclassesExtraDropSystem.FruitTreeBreakInfo)obj);
			}

			// Token: 0x06000042 RID: 66 RVA: 0x000036C9 File Offset: 0x000018C9
			[CompilerGenerated]
			public bool Equals(AllclassesExtraDropSystem.FruitTreeBreakInfo other)
			{
				return EqualityComparer<string>.Default.Equals(this.<TreeType>k__BackingField, other.<TreeType>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsAlive>k__BackingField, other.<IsAlive>k__BackingField);
			}

			// Token: 0x06000043 RID: 67 RVA: 0x000036FB File Offset: 0x000018FB
			[CompilerGenerated]
			public void Deconstruct(out string TreeType, out bool IsAlive)
			{
				TreeType = this.TreeType;
				IsAlive = this.IsAlive;
			}
		}

		// Token: 0x0200000B RID: 11
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000018 RID: 24
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Func<BlockDropItemStack, bool> <0>__IsProduceDrop;

			// Token: 0x04000019 RID: 25
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Func<BlockDropItemStack, bool> <1>__IsTreeSeedDrop;

			// Token: 0x0400001A RID: 26
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Func<BlockDropItemStack, bool> <2>__IsStickDrop;
		}
	}
}
