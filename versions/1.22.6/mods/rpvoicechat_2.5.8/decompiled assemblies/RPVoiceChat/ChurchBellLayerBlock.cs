using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

// Token: 0x02000009 RID: 9
public class ChurchBellLayerBlock : Block
{
	// Token: 0x0600003E RID: 62 RVA: 0x00003788 File Offset: 0x00001988
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

	// Token: 0x0600003F RID: 63 RVA: 0x000037AC File Offset: 0x000019AC
	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityChurchBellLayer churchBellPart = blockAccessor.GetBlockEntity(pos) as BlockEntityChurchBellLayer;
		if (((churchBellPart != null) ? churchBellPart.Inventory : null) != null && churchBellPart.Inventory[2].Empty)
		{
			return new Cuboidf[]
			{
				this.CollisionBoxes[0]
			};
		}
		return base.GetCollisionBoxes(blockAccessor, pos);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003804 File Offset: 0x00001A04
	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityChurchBellLayer churchBellPart = blockAccessor.GetBlockEntity(pos) as BlockEntityChurchBellLayer;
		if (((churchBellPart != null) ? churchBellPart.Inventory : null) != null && churchBellPart.Inventory[2].Empty)
		{
			return new Cuboidf[]
			{
				this.SelectionBoxes[0]
			};
		}
		return base.GetSelectionBoxes(blockAccessor, pos);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003859 File Offset: 0x00001A59
	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		BlockEntityChurchBellLayer blockEntityChurchBellLayer = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityChurchBellLayer;
		if (blockEntityChurchBellLayer != null)
		{
			blockEntityChurchBellLayer.OnInteract(byPlayer);
		}
		return true;
	}
}
