using System;
using Vintagestory.API.Common;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000024 RID: 36
	public class BlockNSEW : Block
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x000160D8 File Offset: 0x000142D8
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			string facing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0].ToString();
			bool flag = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
			if (flag)
			{
				Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
				string newPath = block.Code.Path;
				newPath = newPath.Replace("north", facing);
				block = this.api.World.GetBlock(block.CodeWithPath(newPath));
				this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
			}
			return flag;
		}
	}
}
