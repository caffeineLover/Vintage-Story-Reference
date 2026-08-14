using System;
using System.Collections.Generic;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000049 RID: 73
	public class ItemMonkeyBridge : Item
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x00020948 File Offset: 0x0001EB48
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.inGameErrorCode = "";
			this.goodcolors.Add(ColorUtil.ColorFromRgba(94, 215, 94, 64));
			this.badcolors.Add(ColorUtil.ColorFromRgba(215, 94, 94, 64));
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000209A0 File Offset: 0x0001EBA0
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

		// Token: 0x060002CA RID: 714 RVA: 0x00020A1C File Offset: 0x0001EC1C
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

		// Token: 0x060002CB RID: 715 RVA: 0x00020AC8 File Offset: 0x0001ECC8
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
				else if (blockChk.Code.GetName().Contains("woodenfence-"))
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

		// Token: 0x060002CC RID: 716 RVA: 0x00020B88 File Offset: 0x0001ED88
		public virtual bool ValidEndpoint(IBlockAccessor blockAccessor, BlockPos testpos)
		{
			Block blockChk = blockAccessor.GetBlock(testpos, 0);
			bool validEnd;
			if (blockChk.Code.GetName().Contains("limbtrotlinelure"))
			{
				validEnd = false;
				this.badblocks.Add(testpos);
				this.inGameErrorCode = "endpoint-invalid";
			}
			else
			{
				blockChk.GetSelectionBoxes(blockAccessor, testpos);
				validEnd = (ItemMonkeyBridge.BlockHeight(blockAccessor, testpos) == "large");
				if (validEnd)
				{
					this.goodblocks.Add(testpos);
				}
				else
				{
					this.badblocks.Add(testpos);
					this.inGameErrorCode = "endpoint-invalid";
				}
				if (validEnd)
				{
					foreach (BlockPos neighbor in ItemMonkeyBridge.AreaAround(new BlockPos(testpos.X, testpos.Y, testpos.Z, 0)))
					{
						blockChk = blockAccessor.GetBlock(neighbor, 0);
						if (blockChk.BlockId > 0)
						{
							if (ItemMonkeyBridge.BlockHeight(blockAccessor, neighbor) != "small" && !blockChk.Code.Path.Contains("monkeybridge"))
							{
								validEnd = false;
								this.inGameErrorCode = "endpoint-neighbor-invalid";
								this.badblocks.Add(neighbor);
							}
							else
							{
								this.goodblocks.Add(neighbor);
							}
						}
						else
						{
							this.goodblocks.Add(neighbor);
						}
					}
				}
				else
				{
					this.inGameErrorCode = "endpoint-invalid";
				}
				if (validEnd)
				{
					testpos = testpos.UpCopy(1);
					validEnd = (ItemMonkeyBridge.BlockHeight(blockAccessor, testpos) == "large");
					if (validEnd)
					{
						this.goodblocks.Add(testpos);
					}
					else
					{
						this.badblocks.Add(testpos);
						this.inGameErrorCode = "endpoint-invalid";
					}
					if (validEnd)
					{
						foreach (BlockPos neighbor2 in ItemMonkeyBridge.AreaAround(new BlockPos(testpos.X, testpos.Y, testpos.Z, 0)))
						{
							blockChk = blockAccessor.GetBlock(neighbor2, 0);
							if (blockChk.BlockId > 0)
							{
								if (ItemMonkeyBridge.BlockHeight(blockAccessor, neighbor2) != "small" && !blockChk.Code.Path.Contains("monkeybridge"))
								{
									validEnd = false;
									this.inGameErrorCode = "endpoint-neighbor-invalid";
									this.badblocks.Add(neighbor2);
								}
								else
								{
									this.goodblocks.Add(neighbor2);
								}
							}
							else
							{
								this.goodblocks.Add(neighbor2);
							}
						}
					}
					else
					{
						this.inGameErrorCode = "endpoint-invalid";
					}
				}
			}
			return validEnd;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00020DDC File Offset: 0x0001EFDC
		public virtual int GetLineLength(IBlockAccessor blockAccessor, BlockSelection blockDest, BlockFacing facing)
		{
			int maxLength = ModConfig.Loaded.MonkeyBridgeMaxLength;
			int count = 0;
			bool foundEnd = false;
			BlockPos testpos = blockDest.Position.Copy();
			string testPath = "primitivesurvival:monkeybridge-middle-north";
			Block testBlock = blockAccessor.GetBlock(new AssetLocation(testPath));
			for (;;)
			{
				count++;
				testpos = testpos.Offset(facing);
				Block blockChk = blockAccessor.GetBlock(testpos, 0);
				if (!blockChk.IsReplacableBy(testBlock))
				{
					if (ItemMonkeyBridge.BlockHeight(blockAccessor, testpos) != "small" || blockChk.Code.GetName().Contains("monkeybridge-"))
					{
						foundEnd = true;
						if (count == 1)
						{
							break;
						}
						if (count == 2)
						{
							goto Block_4;
						}
						if (!this.ValidEndpoint(blockAccessor, testpos))
						{
							goto Block_5;
						}
						this.goodblocks.Add(testpos.Copy());
					}
					else
					{
						this.inGameErrorCode = "mb-far-" + this.inGameErrorCode;
						this.badblocks.Add(testpos.Copy());
					}
				}
				else
				{
					this.goodblocks.Add(testpos.Copy());
				}
				if (foundEnd || count >= maxLength)
				{
					goto IL_148;
				}
			}
			this.goodblocks.Add(testpos.Copy());
			return 0;
			Block_4:
			this.inGameErrorCode = "mb-far-endpoint-invalid";
			this.badblocks.Add(testpos.Copy());
			return 1;
			Block_5:
			this.inGameErrorCode = "mb-far-" + this.inGameErrorCode;
			this.badblocks.Add(testpos.Copy());
			return 1;
			IL_148:
			if (!foundEnd)
			{
				this.inGameErrorCode = "mb-bridge-too-far";
				this.badblocks.Add(testpos.Copy());
				return 1;
			}
			return count;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00020F53 File Offset: 0x0001F153
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 3;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00020F5C File Offset: 0x0001F15C
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			try
			{
				this.highlighterEnabled = false;
				IPlayer byPlayer = null;
				this.inGameErrorCode = "";
				this.goodblocks.Clear();
				this.badblocks.Clear();
				EntityPlayer player = byEntity as EntityPlayer;
				if (player != null)
				{
					byPlayer = byEntity.World.PlayerByUid(player.PlayerUID);
				}
				if (blockSel != null && byEntity.World != null && byPlayer != null)
				{
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
					if (facing != null)
					{
						IBlockAccessor blockAccessor = byEntity.World.BlockAccessor;
						BlockPos currPos = blockSel.Position.Copy();
						bool validStart = this.ValidEndpoint(blockAccessor, currPos);
						IServerPlayer splr = byPlayer as IServerPlayer;
						if (!validStart)
						{
							this.inGameErrorCode = "mb-close-" + this.inGameErrorCode;
							this.badblocks.Add(currPos);
							if (splr != null)
							{
								splr.SendIngameError("mb-error", Lang.Get("primitivesurvival:ingameerror-" + this.inGameErrorCode, Array.Empty<object>()), Array.Empty<object>());
							}
						}
						else
						{
							this.goodblocks.Add(currPos);
						}
						if (facing.IsHorizontal && validStart)
						{
							int linelength = this.GetLineLength(blockAccessor, blockSel, facing);
							ItemStack itemstack = slot.Itemstack;
							if (slot.StackSize < linelength - 1 && linelength > 0)
							{
								linelength = 999;
							}
							if (linelength > 0)
							{
								if (linelength > ModConfig.Loaded.MonkeyBridgeMaxLength)
								{
									linelength = 1;
								}
								if (linelength <= 1)
								{
									if (this.inGameErrorCode == "")
									{
										this.inGameErrorCode = "mb-bridge-too-far";
									}
									if (splr != null)
									{
										splr.SendIngameError("mb-error", Lang.Get("primitivesurvival:ingameerror-" + this.inGameErrorCode, Array.Empty<object>()), Array.Empty<object>());
									}
								}
								BlockPos abovePos = currPos.AddCopy(facing).UpCopy(1);
								Block blockChk = blockAccessor.GetBlock(abovePos, 0);
								if (linelength > 1 && blockChk.Code.GetName().Contains("monkeybridge-"))
								{
									this.inGameErrorCode = "mb-bridge-cant-place";
									if (splr != null)
									{
										splr.SendIngameError("mb-error", Lang.Get("primitivesurvival:ingameerror-" + this.inGameErrorCode, Array.Empty<object>()), Array.Empty<object>());
									}
								}
								else if (linelength > 1)
								{
									ItemMonkeyBridge.BlockWidth(blockAccessor, currPos);
									string newPath;
									Block blocknew;
									BlockPos nullPos;
									for (int count = 0; count < linelength; count++)
									{
										newPath = "primitivesurvival:monkeybridge-";
										if (count == 0)
										{
											currPos = currPos.AddCopy(facing);
											newPath = newPath + "end-" + facing.ToString();
											blocknew = byEntity.World.GetBlock(new AssetLocation(newPath));
											blockAccessor.SetBlock(blocknew.BlockId, currPos);
											nullPos = new BlockPos(currPos.X, currPos.Y + 1, currPos.Z, 0);
											blocknew = byEntity.World.GetBlock(new AssetLocation(newPath.Replace("-end", "-null")));
											blockAccessor.SetBlock(blocknew.BlockId, nullPos);
										}
										else if (count < linelength - 1)
										{
											currPos = currPos.AddCopy(facing);
											newPath = newPath + "middle-" + facing.ToString();
											blocknew = byEntity.World.GetBlock(new AssetLocation(newPath));
											blockAccessor.SetBlock(blocknew.BlockId, currPos);
										}
									}
									BlockPos endPos = currPos.AddCopy(facing);
									ItemMonkeyBridge.BlockWidth(blockAccessor, endPos);
									newPath = "primitivesurvival:monkeybridge-end-" + byPlayer.CurrentBlockSelection.Face.ToString();
									blocknew = byEntity.World.GetBlock(new AssetLocation(newPath));
									blockAccessor.SetBlock(blocknew.BlockId, currPos);
									nullPos = new BlockPos(currPos.X, currPos.Y + 1, currPos.Z, 0);
									blocknew = byEntity.World.GetBlock(new AssetLocation(newPath.Replace("-end", "-null")));
									blockAccessor.SetBlock(blocknew.BlockId, nullPos);
									linelength--;
									slot.TakeOut(linelength);
									slot.MarkDirty();
								}
							}
							else
							{
								this.inGameErrorCode = "mb-bridge-cant-place";
								if (splr != null)
								{
									splr.SendIngameError("mb-error", Lang.Get("primitivesurvival:ingameerror-" + this.inGameErrorCode, Array.Empty<object>()), Array.Empty<object>());
								}
							}
						}
						else if (this.highlighterEnabled)
						{
							byEntity.World.HighlightBlocks(byPlayer, this.highlightSlotId1, this.goodblocks, this.goodcolors, 0, 0, this.scale);
							byEntity.World.HighlightBlocks(byPlayer, this.highlightSlotId2, this.badblocks, this.badcolors, 0, 0, this.scale);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000122 RID: 290
		private bool highlighterEnabled;

		// Token: 0x04000123 RID: 291
		private int highlightSlotId1;

		// Token: 0x04000124 RID: 292
		private int highlightSlotId2 = 1;

		// Token: 0x04000125 RID: 293
		private List<BlockPos> goodblocks = new List<BlockPos>();

		// Token: 0x04000126 RID: 294
		private List<BlockPos> badblocks = new List<BlockPos>();

		// Token: 0x04000127 RID: 295
		private List<int> goodcolors = new List<int>();

		// Token: 0x04000128 RID: 296
		private List<int> badcolors = new List<int>();

		// Token: 0x04000129 RID: 297
		private float scale = 1f;

		// Token: 0x0400012A RID: 298
		private string inGameErrorCode;
	}
}
