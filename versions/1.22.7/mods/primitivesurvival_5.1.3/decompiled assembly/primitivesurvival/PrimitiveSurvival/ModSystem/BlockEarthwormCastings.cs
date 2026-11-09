using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000019 RID: 25
	public class BlockEarthwormCastings : Block
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00012620 File Offset: 0x00010820
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			List<ItemStack> list = new List<ItemStack>();
			Item castingsItem = this.api.World.GetItem(new AssetLocation("primitivesurvival:earthwormcastings"));
			if (castingsItem != null)
			{
				for (int i = 0; i < 16; i++)
				{
					list.Add(new ItemStack(castingsItem, 1));
				}
			}
			foreach (ItemStack itemStack in list)
			{
				double d = (double)(this.api.World.Rand.Next(0, 10) / 10);
				world.SpawnItemEntity(itemStack, pos.ToVec3d().Add(d + 0.5, 1.3 + d, d + 0.5), null);
			}
			world.BlockAccessor.SetBlock(0, pos, 0);
			world.BlockAccessor.MarkBlockDirty(pos, null);
		}
	}
}
