using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

// Token: 0x0200000A RID: 10
public class ChurchBellPartBlock : Block
{
	// Token: 0x06000044 RID: 68 RVA: 0x000039AB File Offset: 0x00001BAB
	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return new WorldInteraction[]
		{
			new WorldInteraction
			{
				ActionLangCode = "rpvoicechat:Bell.Interaction.Ring",
				MouseButton = 2
			}
		};
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000039D0 File Offset: 0x00001BD0
	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityChurchBellPart churchBellPart = blockAccessor.GetBlockEntity(pos) as BlockEntityChurchBellPart;
		if (((churchBellPart != null) ? churchBellPart.Inventory : null) != null && churchBellPart.Inventory[2].Empty)
		{
			return new Cuboidf[]
			{
				this.CollisionBoxes[0]
			};
		}
		return base.GetCollisionBoxes(blockAccessor, pos);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003A28 File Offset: 0x00001C28
	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityChurchBellPart churchBellPart = blockAccessor.GetBlockEntity(pos) as BlockEntityChurchBellPart;
		if (((churchBellPart != null) ? churchBellPart.Inventory : null) != null && churchBellPart.Inventory[2].Empty)
		{
			return new Cuboidf[]
			{
				this.SelectionBoxes[0]
			};
		}
		return base.GetSelectionBoxes(blockAccessor, pos);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003A7D File Offset: 0x00001C7D
	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		BlockEntityChurchBellPart blockEntityChurchBellPart = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityChurchBellPart;
		if (blockEntityChurchBellPart != null)
		{
			blockEntityChurchBellPart.OnInteract(byPlayer);
		}
		return true;
	}
}
