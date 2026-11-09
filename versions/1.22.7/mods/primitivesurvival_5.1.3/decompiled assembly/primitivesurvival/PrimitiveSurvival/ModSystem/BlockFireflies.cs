using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200001A RID: 26
	public class BlockFireflies : Block
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00012720 File Offset: 0x00010920
		public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
		{
			return base.GetHeldTpUseAnimation(activeHotbarSlot, byEntity);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001272C File Offset: 0x0001092C
		private string Facing(IPlayer byPlayer, BlockSelection blockSel)
		{
			BlockPos targetPos = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
			double num = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
			double dz = byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
			if (Convert.ToInt32((Math.Atan2(num, dz) + 3.141592653589793) / 0.7853981633974483) % 8 % 2 == 0)
			{
				return "straight";
			}
			return "angled";
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000127DC File Offset: 0x000109DC
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			if (!this.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
			{
				return false;
			}
			if (this.Code.Path.Contains("-empty") || this.Code.Path.Contains("-baited"))
			{
				if (world.BlockAccessor.GetBlock(blockSel.Position.DownCopy(1), 0).BlockId == 0)
				{
					failureCode = "requiresolidground";
					return false;
				}
				string newPath = this.Code.Path;
				newPath = newPath.Replace("straight", this.Facing(byPlayer, blockSel));
				Block blockToPlace = this.api.World.GetBlock(this.CodeWithPath(newPath));
				world.BlockAccessor.SetBlock(blockToPlace.BlockId, blockSel.Position);
				return true;
			}
			else
			{
				AssetLocation location = new AssetLocation(this.Code.Domain, base.CodeWithoutParts(1));
				EntityProperties type = byPlayer.WorldData.EntityPlayer.World.GetEntityType(location);
				if (type == null)
				{
					this.api.World.Logger.Error("BlockFireflies: No such entity - {0}", new object[]
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
					if (this.Facing(byPlayer, blockSel) == "angled")
					{
						entity.Pos.Yaw = 0.78f;
					}
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
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00012ACC File Offset: 0x00010CCC
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

		// Token: 0x0600017E RID: 382 RVA: 0x00012CDC File Offset: 0x00010EDC
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
