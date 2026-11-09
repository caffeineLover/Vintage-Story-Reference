using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Allclasses
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AllclassesElectricalProgressiveCompatSystem : ModSystem
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000023C0 File Offset: 0x000005C0
		public override void Start(ICoreAPI api)
		{
			Type eDrillType = AccessTools.TypeByName("ElectricalProgressive.Content.Item.Tool.EDrill");
			MethodInfo destroyBlocks = (eDrillType == null) ? null : AccessTools.Method(eDrillType, "destroyBlocks", new Type[]
			{
				typeof(IWorldAccessor),
				typeof(BlockPos),
				typeof(BlockPos),
				typeof(IPlayer),
				typeof(BlockSelection),
				typeof(ItemSlot)
			}, null);
			if (destroyBlocks == null)
			{
				return;
			}
			this.harmony = new Harmony("dejfidoff.allclasses.electricalprogressivecompat");
			this.harmony.Patch(destroyBlocks, new HarmonyMethod(typeof(AllclassesElectricalProgressiveCompatSystem), "DestroyBlocksPrefix", null), null, null, null);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002484 File Offset: 0x00000684
		public override void Dispose()
		{
			Harmony harmony = this.harmony;
			if (harmony != null)
			{
				harmony.UnpatchAll("dejfidoff.allclasses.electricalprogressivecompat");
			}
			this.harmony = null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024A4 File Offset: 0x000006A4
		private static bool DestroyBlocksPrefix(CollectibleObject __instance, IWorldAccessor world, BlockPos min, BlockPos max, IPlayer player, BlockSelection block, ItemSlot slot)
		{
			if (world == null || player == null || ((block != null) ? block.Position : null) == null || ((slot != null) ? slot.Itemstack : null) == null)
			{
				return false;
			}
			int durability = slot.Itemstack.Attributes.GetInt("durability", 0);
			IBlockAccessor blockAccessor = world.BlockAccessor;
			Block selectedBlock = AllclassesElectricalProgressiveCompatSystem.GetSelectedBlock(blockAccessor, block);
			float selectedMiningSpeed = AllclassesElectricalProgressiveCompatSystem.GetMiningSpeed(__instance, slot, block, selectedBlock, player);
			BlockPos tempPos = new BlockPos(min.dimension);
			for (int x = min.X; x <= max.X; x++)
			{
				for (int y = min.Y; y <= max.Y; y++)
				{
					for (int z = min.Z; z <= max.Z; z++)
					{
						tempPos.Set(x, y, z);
						if (!AllclassesElectricalProgressiveCompatSystem.SamePosition(tempPos, block.Position))
						{
							Block targetBlock = blockAccessor.GetBlock(tempPos);
							if (player.WorldData.CurrentGameMode == 2)
							{
								blockAccessor.SetBlock(0, tempPos);
							}
							else if (durability > 1 && AllclassesElectricalProgressiveCompatSystem.CanDrillBreakBlock(__instance, slot, world, block, tempPos, targetBlock, player, selectedMiningSpeed))
							{
								blockAccessor.BreakBlock(tempPos, player, 1f);
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025E7 File Offset: 0x000007E7
		private static Block GetSelectedBlock(IBlockAccessor blockAccessor, BlockSelection blockSel)
		{
			if (blockSel.Block != null && !AllclassesElectricalProgressiveCompatSystem.IsAir(blockSel.Block))
			{
				return blockSel.Block;
			}
			return blockAccessor.GetBlock(blockSel.Position);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002614 File Offset: 0x00000814
		private static bool CanDrillBreakBlock(CollectibleObject drill, ItemSlot slot, IWorldAccessor world, BlockSelection selectedBlockSel, BlockPos targetPos, Block targetBlock, IPlayer player, float selectedMiningSpeed)
		{
			EnumBlockMaterial material = targetBlock.GetBlockMaterial(world.BlockAccessor, targetPos, null);
			Dictionary<EnumBlockMaterial, float> miningSpeeds = drill.GetMiningSpeeds(slot);
			if (miningSpeeds == null || !miningSpeeds.ContainsKey(material))
			{
				return false;
			}
			if (drill.GetToolTier(slot) < targetBlock.GetRequiredMiningTier(world, targetPos))
			{
				return false;
			}
			float targetMiningSpeed = AllclassesElectricalProgressiveCompatSystem.GetMiningSpeed(drill, slot, selectedBlockSel, targetBlock, player);
			return ((selectedMiningSpeed > 0f) ? selectedMiningSpeed : targetMiningSpeed) * 1.5f >= targetMiningSpeed;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002684 File Offset: 0x00000884
		private static float GetMiningSpeed(CollectibleObject collectible, ItemSlot slot, BlockSelection blockSel, Block block, IPlayer player)
		{
			return collectible.GetMiningSpeed(slot.Itemstack, blockSel, block, player);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002696 File Offset: 0x00000896
		private static bool SamePosition(BlockPos left, BlockPos right)
		{
			return left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.dimension == right.dimension;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026D2 File Offset: 0x000008D2
		private static bool IsAir(Block block)
		{
			AssetLocation code = block.Code;
			return ((code != null) ? code.Path : null) == "air";
		}

		// Token: 0x04000005 RID: 5
		private const string HarmonyId = "dejfidoff.allclasses.electricalprogressivecompat";

		// Token: 0x04000006 RID: 6
		[Nullable(2)]
		private Harmony harmony;
	}
}
