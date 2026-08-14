using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200004F RID: 79
	public class ItemWoodSpikeBundle : Item
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x00022965 File Offset: 0x00020B65
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 3;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0002296C File Offset: 0x00020B6C
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (blockSel == null || byEntity == null)
			{
				return;
			}
			IWorldAccessor world = byEntity.World;
			if (world == null)
			{
				return;
			}
			Block block = world.BlockAccessor.GetBlock(blockSel.Position, 0);
			string face = blockSel.Face.ToString();
			if (face == "down")
			{
				return;
			}
			if (face == "up")
			{
				if (block.Fertility <= 0 && !block.Code.Path.Contains("tallgrass-"))
				{
					if (this.api.Side == 2)
					{
						(this.api as ICoreClientAPI).TriggerIngameError(this, "cantplace", Lang.Get("game:placefailure-softer-ground-needed", Array.Empty<object>()));
					}
					return;
				}
				BlockSelection blockSelAbove = blockSel.Clone();
				blockSelAbove.Position.Y++;
				Block blockAbove = world.BlockAccessor.GetBlock(blockSelAbove.Position, 0);
				if (blockAbove.BlockId == 0 || blockAbove.Code.Path.Contains("tallgrass-") || block.Code.Path.Contains("tallgrass-"))
				{
					Block blockNew = world.GetBlock(new AssetLocation("primitivesurvival:woodspikes"));
					if (blockNew == null)
					{
						return;
					}
					IBlockAccessor blockAccessor = world.BlockAccessor;
					if (block.Code.Path.Contains("tallgrass-"))
					{
						blockAccessor.SetBlock(blockNew.BlockId, blockSel.Position);
					}
					else
					{
						blockAccessor.SetBlock(blockNew.BlockId, blockSelAbove.Position);
					}
					slot.TakeOut(1);
					slot.MarkDirty();
					return;
				}
			}
			else
			{
				BlockSelection blockSelBeside = blockSel.Clone();
				if (face == "east")
				{
					blockSelBeside.Position.X++;
				}
				else if (face == "west")
				{
					blockSelBeside.Position.X--;
				}
				else if (face == "north")
				{
					blockSelBeside.Position.Z--;
				}
				else
				{
					blockSelBeside.Position.Z++;
				}
				Block blockBeside = world.BlockAccessor.GetBlock(blockSelBeside.Position, 0);
				if (blockBeside.BlockId == 0 || block.FirstCodePart(0) == "woodsupportspikes")
				{
					bool placeOk = false;
					if (blockBeside.BlockId == 0 && block.Fertility > 0)
					{
						placeOk = true;
					}
					else
					{
						string selFace = block.LastCodePart(0);
						if ((face == "east" || face == "west") && (selFace == "north" || selFace == "south"))
						{
							placeOk = true;
						}
						else if ((face == "north" || face == "south") && (selFace == "east" || selFace == "west"))
						{
							placeOk = true;
						}
					}
					if (placeOk)
					{
						Block blockNew2 = world.GetBlock(new AssetLocation("primitivesurvival:woodsupportspikes-" + face));
						if (blockNew2 == null)
						{
							return;
						}
						world.BlockAccessor.SetBlock(blockNew2.BlockId, blockSelBeside.Position);
						slot.TakeOut(1);
						slot.MarkDirty();
						return;
					}
					else if (block.Fertility <= 0 && this.api.Side == 2)
					{
						(this.api as ICoreClientAPI).TriggerIngameError(this, "cantplace", Lang.Get("game:placefailure-softer-ground-needed", Array.Empty<object>()));
					}
				}
			}
		}
	}
}
