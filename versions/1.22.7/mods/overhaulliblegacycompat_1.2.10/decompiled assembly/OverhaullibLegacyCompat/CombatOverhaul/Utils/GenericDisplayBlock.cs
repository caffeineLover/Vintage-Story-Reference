using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000026 RID: 38
	public class GenericDisplayBlock : BlockDisplayCase
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000B728 File Offset: 0x00009928
		[NullableContext(1)]
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			GenericDisplayBlockEntity genericDisplayBlockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position) as GenericDisplayBlockEntity;
			if (genericDisplayBlockEntity == null)
			{
				return base.OnBlockInteractStart(world, byPlayer, blockSel);
			}
			return genericDisplayBlockEntity.OnInteract(byPlayer, blockSel);
		}
	}
}
