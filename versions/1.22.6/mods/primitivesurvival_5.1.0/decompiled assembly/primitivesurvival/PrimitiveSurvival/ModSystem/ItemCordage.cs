using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000042 RID: 66
	public class ItemCordage : Item
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x0001EB88 File Offset: 0x0001CD88
		private static BlockPos[] AreaAround(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos.NorthCopy(1),
				pos.SouthCopy(1),
				pos.EastCopy(1),
				pos.WestCopy(1),
				pos.NorthCopy(1).EastCopy(1),
				pos.SouthCopy(1).WestCopy(1),
				pos.SouthCopy(1).EastCopy(1),
				pos.NorthCopy(1).WestCopy(1)
			};
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0001EC04 File Offset: 0x0001CE04
		public static string BlockHeight(IBlockAccessor blockAccessor, BlockPos pos)
		{
			string heightStr = "small";
			Block blockChk = blockAccessor.GetBlock(pos, 0);
			if (blockChk.BlockId > 0)
			{
				Cuboidf[] sbs = blockChk.GetSelectionBoxes(blockAccessor, pos);
				if (sbs == null)
				{
					return heightStr;
				}
				foreach (Cuboidf sb in sbs)
				{
					if ((double)Math.Abs(sb.Y2 - sb.Y1) >= 0.5)
					{
						heightStr = "large";
					}
					else if ((double)Math.Abs(sb.Y2 - sb.Y1) >= 0.3 && heightStr != "large")
					{
						heightStr = "medium";
					}
				}
			}
			return heightStr;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0001ECB0 File Offset: 0x0001CEB0
		public static string BlockWidth(IBlockAccessor blockAccessor, BlockPos pos)
		{
			string widthStr = "small";
			Block blockChk = blockAccessor.GetBlock(pos, 0);
			if (blockChk.BlockId > 0)
			{
				if (blockChk.Code.Path.Contains("stake-") || blockChk.Code.Path.Contains("stakeinwater-"))
				{
					widthStr = "small";
				}
				else if (blockChk.Code.GetName().Contains("fence-"))
				{
					widthStr = "medium";
				}
				else
				{
					foreach (Cuboidf sb in blockChk.GetSelectionBoxes(blockAccessor, pos))
					{
						if ((double)Math.Abs(sb.X2 - sb.X1) > 0.5)
						{
							widthStr = "large";
						}
					}
				}
			}
			return widthStr;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0001ED70 File Offset: 0x0001CF70
		public static bool ValidEndpoint(IBlockAccessor blockAccessor, BlockPos testpos)
		{
			Block blockChk = blockAccessor.GetBlock(testpos, 0);
			bool validEnd;
			if (blockChk.Code.GetName().Contains("fence-") || blockChk.Code.Path.Contains("stake-") || blockChk.Code.Path.Contains("stakeinwater-"))
			{
				validEnd = true;
			}
			else if (blockChk.Code.GetName().Contains("limbtrotlinelure"))
			{
				validEnd = false;
			}
			else
			{
				validEnd = (ItemCordage.BlockHeight(blockAccessor, testpos) == "large");
				foreach (BlockPos neighbor in ItemCordage.AreaAround(new BlockPos(testpos.X, testpos.Y, testpos.Z, 0)))
				{
					blockChk = blockAccessor.GetBlock(neighbor, 0);
					if (blockChk.BlockId > 0 && ItemCordage.BlockHeight(blockAccessor, neighbor) != "small" && !blockChk.Code.Path.Contains("limbtrotlinelure") && !blockChk.Code.GetName().Contains("fence-") && !blockChk.Code.Path.Contains("stake-") && !blockChk.Code.Path.Contains("stakeinwater-"))
					{
						validEnd = false;
					}
					if (blockAccessor.GetBlock(neighbor, 2).BlockId > 0)
					{
						validEnd = false;
					}
				}
			}
			return validEnd;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0001EED4 File Offset: 0x0001D0D4
		public virtual int GetLineLength(IBlockAccessor blockAccessor, BlockSelection blockDest, BlockFacing facing)
		{
			int maxLength = 41;
			int count = 0;
			bool foundEnd = false;
			BlockPos testpos = blockDest.Position.Copy();
			string testPath = "primitivesurvival:limbtrotlinelure-middle-north";
			Block testBlock = blockAccessor.GetBlock(new AssetLocation(testPath));
			for (;;)
			{
				count++;
				testpos = testpos.Offset(facing);
				Block blockChk = blockAccessor.GetBlock(testpos, 0);
				if (!blockChk.IsReplacableBy(testBlock) && (ItemCordage.BlockHeight(blockAccessor, testpos) != "small" || blockChk.Code.GetName().Contains("limbtrotlinelure-")))
				{
					foundEnd = true;
					if (count == 1)
					{
						break;
					}
					if (count == 2)
					{
						return 1;
					}
					if (!ItemCordage.ValidEndpoint(blockAccessor, testpos))
					{
						return 1;
					}
				}
				if (foundEnd || count >= maxLength)
				{
					goto IL_95;
				}
			}
			return 0;
			IL_95:
			if (!foundEnd)
			{
				return 1;
			}
			return count;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0001EF7C File Offset: 0x0001D17C
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 3;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0001EF84 File Offset: 0x0001D184
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			IPlayer byPlayer = null;
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				byPlayer = byEntity.World.PlayerByUid(player.PlayerUID);
			}
			if (blockSel == null || byEntity == null || byPlayer == null)
			{
				return;
			}
			if (byEntity.World == null)
			{
				return;
			}
			BlockFacing blockFacing;
			if (byPlayer == null)
			{
				blockFacing = null;
			}
			else
			{
				BlockSelection currentBlockSelection = byPlayer.CurrentBlockSelection;
				if (currentBlockSelection == null)
				{
					blockFacing = null;
				}
				else
				{
					BlockFacing face = currentBlockSelection.Face;
					blockFacing = ((face != null) ? face.Opposite : null);
				}
			}
			BlockFacing facing = blockFacing;
			if (facing == null)
			{
				return;
			}
			IBlockAccessor blockAccessor = byEntity.World.BlockAccessor;
			BlockPos currPos = blockSel.Position.Copy();
			if (blockAccessor.GetBlock(currPos, 2).Code.Path.Contains("water"))
			{
				return;
			}
			bool validStart = ItemCordage.ValidEndpoint(blockAccessor, currPos);
			if (facing.IsHorizontal && validStart)
			{
				int linelength = this.GetLineLength(blockAccessor, blockSel, facing);
				ItemStack itemstack = slot.Itemstack;
				string tempStr = "";
				if (slot.StackSize < linelength - 1 && linelength > 0)
				{
					linelength = 999;
				}
				if (linelength > 0)
				{
					if (linelength > 41)
					{
						linelength = 1;
					}
					string blockSize = ItemCordage.BlockWidth(blockAccessor, currPos);
					for (int count = 0; count < linelength; count++)
					{
						string newPath = "primitivesurvival:limbtrotlinelure-";
						if (count == 0)
						{
							currPos = currPos.AddCopy(facing);
							if (linelength > 1)
							{
								tempStr = "withmiddle-";
							}
							newPath = string.Concat(new string[]
							{
								newPath,
								"end-",
								blockSize,
								"-",
								tempStr,
								facing.ToString()
							});
							Block blocknew = byEntity.World.GetBlock(new AssetLocation(newPath));
							blockAccessor.SetBlock(blocknew.BlockId, currPos);
						}
						else if (count < linelength - 1)
						{
							currPos = currPos.AddCopy(facing);
							newPath = newPath + "middle-" + facing.ToString();
							Block blocknew2 = byEntity.World.GetBlock(new AssetLocation(newPath));
							blockAccessor.SetBlock(blocknew2.BlockId, currPos);
						}
					}
					if (linelength > 1)
					{
						BlockPos endPos = currPos.AddCopy(facing);
						blockSize = ItemCordage.BlockWidth(blockAccessor, endPos);
						string newPath = "primitivesurvival:limbtrotlinelure-end-" + blockSize + "-withmiddle-" + byPlayer.CurrentBlockSelection.Face.ToString();
						Block blocknew3 = byEntity.World.GetBlock(new AssetLocation(newPath));
						blockAccessor.SetBlock(blocknew3.BlockId, currPos);
						linelength--;
					}
					slot.TakeOut(linelength);
					slot.MarkDirty();
				}
			}
		}
	}
}
