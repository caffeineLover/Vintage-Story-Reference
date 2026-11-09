using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000026 RID: 38
	public class BlockSkullOfTheDead : Block
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x00016C9B File Offset: 0x00014E9B
		public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
		{
			return base.GetHeldTpUseAnimation(activeHotbarSlot, byEntity);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00016CA8 File Offset: 0x00014EA8
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			if (!this.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
			{
				return false;
			}
			BlockSelection blockSrc = blockSel.Clone();
			blockSrc.Position.Y--;
			if (world.BlockAccessor.GetBlock(blockSrc.Position, 0).Fertility <= 0)
			{
				failureCode = Lang.Get("softer-ground-needed", Array.Empty<object>());
				return false;
			}
			AssetLocation location = new AssetLocation(this.Code.Domain, this.Code.Path);
			EntityProperties type = byPlayer.WorldData.EntityPlayer.World.GetEntityType(location);
			if (type == null)
			{
				this.api.World.Logger.Error("BlockSkullOfTheDead: No such entity - {0}", new object[]
				{
					location
				});
				return false;
			}
			Entity entity = byPlayer.WorldData.EntityPlayer.World.ClassRegistry.CreateEntity(type);
			if (entity != null)
			{
				entity.Pos.X = (double)((float)(blockSel.Position.X + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.X)) + 0.5f);
				entity.Pos.Y = (double)(blockSel.Position.Y + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.Y));
				entity.Pos.Z = (double)((float)(blockSel.Position.Z + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.Z)) + 0.5f);
				entity.Pos.Yaw = 0f;
				entity.Pos.SetFrom(entity.Pos);
				entity.PositionBeforeFalling.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
				entity.Attributes.SetString("origin", "playerplaced");
				byPlayer.WorldData.EntityPlayer.World.SpawnEntity(entity);
				if (this.api.Side == 1)
				{
					byPlayer.WorldData.EntityPlayer.World.PlaySoundAt(new AssetLocation("game:sounds/player/buildhigh"), entity.Pos.X, entity.Pos.Y, entity.Pos.Z, null, true, 32f, 1f);
				}
			}
			return true;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00016F14 File Offset: 0x00015114
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (!playerSlot.Empty)
			{
				ItemStack playerStack = playerSlot.Itemstack;
				if (playerStack.Block != null)
				{
					if (playerStack.Block.FirstCodePart(0) == "fruit")
					{
						string newPath = this.Code.Path;
						if (newPath.Contains("-empty"))
						{
							newPath = newPath.Replace("-empty", "-baited");
							Block blockToPlace = this.api.World.GetBlock(this.CodeWithPath(newPath));
							world.BlockAccessor.SetBlock(blockToPlace.BlockId, blockSel.Position);
							world.PlaySoundAt(new AssetLocation("sounds/player/buildhigh"), (double)((float)blockSel.Position.X + 0.5f), (double)((float)blockSel.Position.Y + 0.5f), (double)((float)blockSel.Position.Z + 0.5f), byPlayer, true, 32f, 1f);
							playerSlot.TakeOut(1);
							playerSlot.MarkDirty();
							return true;
						}
					}
				}
				else if (playerStack.Item != null && playerStack.Item.FirstCodePart(0) == "fruit")
				{
					string newPath2 = this.Code.Path;
					if (newPath2.Contains("-empty"))
					{
						newPath2 = newPath2.Replace("-empty", "-baited");
						Block blockToPlace2 = this.api.World.GetBlock(this.CodeWithPath(newPath2));
						world.BlockAccessor.SetBlock(blockToPlace2.BlockId, blockSel.Position);
						world.PlaySoundAt(new AssetLocation("sounds/player/buildhigh"), (double)((float)blockSel.Position.X + 0.5f), (double)((float)blockSel.Position.Y + 0.5f), (double)((float)blockSel.Position.Z + 0.5f), byPlayer, true, 32f, 1f);
						playerSlot.TakeOut(1);
						playerSlot.MarkDirty();
						return true;
					}
				}
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00017124 File Offset: 0x00015324
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "heldhelp-place",
					MouseButton = 2
				}
			}, base.GetHeldInteractionHelp(inSlot));
		}
	}
}
