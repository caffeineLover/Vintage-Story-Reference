using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002A RID: 42
	public class BlockStakeInWater : Block
	{
		// Token: 0x060001FC RID: 508 RVA: 0x00017E84 File Offset: 0x00016084
		private string GetOrientations(IWorldAccessor world, BlockPos pos)
		{
			string orientations = this.GetFenceCode(world, pos, BlockFacing.NORTH) + this.GetFenceCode(world, pos, BlockFacing.EAST) + this.GetFenceCode(world, pos, BlockFacing.SOUTH) + this.GetFenceCode(world, pos, BlockFacing.WEST);
			if (orientations.Length == 0)
			{
				orientations = "empty";
			}
			return orientations;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00017EDC File Offset: 0x000160DC
		private string GetFenceCode(IWorldAccessor world, BlockPos pos, BlockFacing facing)
		{
			if (this.ShouldConnectAt(world, pos, facing))
			{
				return facing.Code.get_Chars(0).ToString() ?? "";
			}
			return "";
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00017F18 File Offset: 0x00016118
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			Block block = world.BlockAccessor.GetBlock(pos, 0);
			if (block.Class == "blockstakeinwater" && block.Code.Path.Contains("open"))
			{
				foreach (BlockPos neighbor in new BlockPos[]
				{
					pos.EastCopy(1),
					pos.WestCopy(1),
					pos.NorthCopy(1),
					pos.SouthCopy(1)
				})
				{
					if (world.BlockAccessor.GetBlock(neighbor, 0).Code.Path.Contains("weirtrap"))
					{
						world.BlockAccessor.BreakBlock(neighbor, null, 1f);
					}
				}
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
			world.BlockAccessor.GetBlock(pos, 0).OnNeighbourBlockChange(world, pos, pos);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00017FF8 File Offset: 0x000161F8
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			BlockPos pos = blockSel.Position;
			Block block = world.BlockAccessor.GetBlock(pos, 0);
			if (block.Class == "blockstakeinwater" || block.Code.Path.Contains("fishbasket-"))
			{
				return false;
			}
			BlockSelection blockSelBelow = blockSel.Clone();
			blockSelBelow.Position.Y--;
			if (world.BlockAccessor.GetBlock(blockSelBelow.Position, 0).Fertility <= 0)
			{
				failureCode = Lang.Get("softer-ground-needed", Array.Empty<object>());
				return false;
			}
			string orientations = this.GetOrientations(world, pos);
			if (world.BlockAccessor.GetBlock(base.CodeWithVariant("type", orientations)) == null)
			{
			}
			return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000180C8 File Offset: 0x000162C8
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			Block neibBlock = world.BlockAccessor.GetBlock(neibpos, 0);
			Block block = world.BlockAccessor.GetBlock(pos, 0);
			if (block.Class == "blockstakeinwater" && block.Code.Path.Contains("open"))
			{
				if (neibBlock.Class == "blockstakeinwater")
				{
					return;
				}
				foreach (BlockPos neighbor in new BlockPos[]
				{
					pos.EastCopy(1),
					pos.WestCopy(1),
					pos.NorthCopy(1),
					pos.SouthCopy(1)
				})
				{
					if (world.BlockAccessor.GetBlock(neighbor, 0).Code.Path.Contains("weirtrap"))
					{
						world.BlockAccessor.BreakBlock(neighbor, null, 1f);
					}
				}
			}
			string orientations = this.GetOrientations(world, pos);
			AssetLocation newBlockCode = base.CodeWithVariant("type", orientations);
			if (this.Code.Equals(newBlockCode))
			{
				base.OnNeighbourBlockChange(world, pos, neibpos);
				return;
			}
			block = world.BlockAccessor.GetBlock(newBlockCode);
			if (block == null)
			{
				return;
			}
			world.BlockAccessor.SetBlock(block.BlockId, pos);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00018211 File Offset: 0x00016411
		public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
		{
			return new BlockDropItemStack[]
			{
				new BlockDropItemStack(handbookStack, 1f)
			};
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00018228 File Offset: 0x00016428
		public bool ShouldConnectAt(IWorldAccessor world, BlockPos ownPos, BlockFacing side)
		{
			Block block = world.BlockAccessor.GetBlock(ownPos.AddCopy(side), 0);
			return block.FirstCodePart(0) == base.FirstCodePart(0) || block.SideSolid[side.Opposite.Index];
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00018278 File Offset: 0x00016478
		static BlockStakeInWater()
		{
			BlockStakeInWater.AngleGroups["n"] = new KeyValuePair<string[], int>(BlockStakeInWater.OneDir, 0);
			BlockStakeInWater.AngleGroups["e"] = new KeyValuePair<string[], int>(BlockStakeInWater.OneDir, 1);
			BlockStakeInWater.AngleGroups["s"] = new KeyValuePair<string[], int>(BlockStakeInWater.OneDir, 2);
			BlockStakeInWater.AngleGroups["w"] = new KeyValuePair<string[], int>(BlockStakeInWater.OneDir, 3);
			BlockStakeInWater.AngleGroups["ns"] = new KeyValuePair<string[], int>(BlockStakeInWater.TwoDir, 0);
			BlockStakeInWater.AngleGroups["ew"] = new KeyValuePair<string[], int>(BlockStakeInWater.TwoDir, 1);
			BlockStakeInWater.AngleGroups["ne"] = new KeyValuePair<string[], int>(BlockStakeInWater.AngledDir, 0);
			BlockStakeInWater.AngleGroups["nw"] = new KeyValuePair<string[], int>(BlockStakeInWater.AngledDir, 1);
			BlockStakeInWater.AngleGroups["es"] = new KeyValuePair<string[], int>(BlockStakeInWater.AngledDir, 2);
			BlockStakeInWater.AngleGroups["sw"] = new KeyValuePair<string[], int>(BlockStakeInWater.AngledDir, 3);
			BlockStakeInWater.AngleGroups["nes"] = new KeyValuePair<string[], int>(BlockStakeInWater.ThreeDir, 0);
			BlockStakeInWater.AngleGroups["new"] = new KeyValuePair<string[], int>(BlockStakeInWater.ThreeDir, 1);
			BlockStakeInWater.AngleGroups["nsw"] = new KeyValuePair<string[], int>(BlockStakeInWater.ThreeDir, 2);
			BlockStakeInWater.AngleGroups["esw"] = new KeyValuePair<string[], int>(BlockStakeInWater.ThreeDir, 3);
			BlockStakeInWater.AngleGroups["egw"] = new KeyValuePair<string[], int>(BlockStakeInWater.GateLeft, 0);
			BlockStakeInWater.AngleGroups["ngs"] = new KeyValuePair<string[], int>(BlockStakeInWater.GateLeft, 1);
			BlockStakeInWater.AngleGroups["gew"] = new KeyValuePair<string[], int>(BlockStakeInWater.GateRight, 0);
			BlockStakeInWater.AngleGroups["gns"] = new KeyValuePair<string[], int>(BlockStakeInWater.GateRight, 1);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00018538 File Offset: 0x00016738
		public override AssetLocation GetRotatedBlockCode(int angle)
		{
			string type = this.Variant["type"];
			if (type == "empty" || type == "nesw")
			{
				return this.Code;
			}
			int angleIndex = angle / 90;
			KeyValuePair<string[], int> val = BlockStakeInWater.AngleGroups[type];
			string newFacing = val.Key[(angleIndex + val.Value) % val.Key.Length];
			return base.CodeWithVariant("type", newFacing);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000185B0 File Offset: 0x000167B0
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BlockPos pos = blockSel.Position;
			Block block = world.BlockAccessor.GetBlock(blockSel.Position, 0);
			string path = block.Code.Path;
			BlockFacing facing = byPlayer.CurrentBlockSelection.Face;
			if (facing.IsHorizontal && (path.EndsWith("-ew") || path.EndsWith("-ns")))
			{
				BlockPos waterPos;
				BlockPos[] weirSidesPos;
				BlockPos[] weirBasesPos;
				if (facing.ToString() == "north")
				{
					path = path.Replace("-ew", "-we");
					waterPos = pos.NorthCopy(1);
					weirSidesPos = new BlockPos[]
					{
						waterPos.EastCopy(1),
						waterPos.WestCopy(1)
					};
					weirBasesPos = new BlockPos[]
					{
						waterPos.NorthCopy(1),
						waterPos.NorthCopy(1).EastCopy(1),
						waterPos.NorthCopy(1).WestCopy(1)
					};
				}
				else if (facing.ToString() == "west")
				{
					path = path.Replace("-ns", "-sn");
					waterPos = pos.WestCopy(1);
					weirSidesPos = new BlockPos[]
					{
						waterPos.NorthCopy(1),
						waterPos.SouthCopy(1)
					};
					weirBasesPos = new BlockPos[]
					{
						waterPos.WestCopy(1),
						waterPos.WestCopy(1).NorthCopy(1),
						waterPos.WestCopy(1).SouthCopy(1)
					};
				}
				else if (facing.ToString() == "south")
				{
					waterPos = pos.SouthCopy(1);
					weirSidesPos = new BlockPos[]
					{
						waterPos.EastCopy(1),
						waterPos.WestCopy(1)
					};
					weirBasesPos = new BlockPos[]
					{
						waterPos.SouthCopy(1),
						waterPos.SouthCopy(1).EastCopy(1),
						waterPos.SouthCopy(1).WestCopy(1)
					};
				}
				else
				{
					waterPos = pos.EastCopy(1);
					weirSidesPos = new BlockPos[]
					{
						waterPos.NorthCopy(1),
						waterPos.SouthCopy(1)
					};
					weirBasesPos = new BlockPos[]
					{
						waterPos.EastCopy(1),
						waterPos.EastCopy(1).NorthCopy(1),
						waterPos.EastCopy(1).SouthCopy(1)
					};
				}
				Block waterBlock = world.BlockAccessor.GetBlock(waterPos, 0);
				bool areaOK = true;
				foreach (BlockPos neighbor in weirSidesPos)
				{
					Block testBlock = world.BlockAccessor.GetBlock(neighbor, 0);
					if (testBlock.Class != "blockstakeinwater")
					{
						areaOK = false;
					}
				}
				foreach (BlockPos neighbor2 in weirBasesPos)
				{
					Block testBlock = world.BlockAccessor.GetBlock(neighbor2, 0);
					if (testBlock.BlockId == 0 || (testBlock.Code.Path.Contains("water") && testBlock.Class != "blockstakeinwater"))
					{
						areaOK = false;
					}
				}
				if (waterBlock.Code.Path.Contains("water") && areaOK)
				{
					path += "open";
					Block openBlock = world.GetBlock(block.CodeWithPath(path));
					if (openBlock == null)
					{
						return base.OnBlockInteractStart(world, byPlayer, blockSel);
					}
					Block trapBlock = null;
					Block testBlock = world.BlockAccessor.GetBlock(waterPos, 0);
					if (!testBlock.Code.Path.Contains("weirtrap"))
					{
						trapBlock = world.BlockAccessor.GetBlock(new AssetLocation("primitivesurvival:weirtrap-" + facing.ToString()));
						if (trapBlock == null)
						{
							return base.OnBlockInteractStart(world, byPlayer, blockSel);
						}
					}
					world.BlockAccessor.SetBlock(openBlock.BlockId, blockSel.Position);
					if (trapBlock != null)
					{
						world.BlockAccessor.SetBlock(trapBlock.BlockId, waterPos);
					}
					return true;
				}
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00018972 File Offset: 0x00016B72
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
		}

		// Token: 0x040000DA RID: 218
		private static readonly string[] OneDir = new string[]
		{
			"n",
			"e",
			"s",
			"w"
		};

		// Token: 0x040000DB RID: 219
		private static readonly string[] TwoDir = new string[]
		{
			"ns",
			"ew"
		};

		// Token: 0x040000DC RID: 220
		private static readonly string[] AngledDir = new string[]
		{
			"ne",
			"es",
			"sw",
			"nw"
		};

		// Token: 0x040000DD RID: 221
		private static readonly string[] ThreeDir = new string[]
		{
			"nes",
			"new",
			"nsw",
			"esw"
		};

		// Token: 0x040000DE RID: 222
		private static readonly string[] GateLeft = new string[]
		{
			"egw",
			"ngs"
		};

		// Token: 0x040000DF RID: 223
		private static readonly string[] GateRight = new string[]
		{
			"gew",
			"gns"
		};

		// Token: 0x040000E0 RID: 224
		private static readonly Dictionary<string, KeyValuePair<string[], int>> AngleGroups = new Dictionary<string, KeyValuePair<string[], int>>();
	}
}
