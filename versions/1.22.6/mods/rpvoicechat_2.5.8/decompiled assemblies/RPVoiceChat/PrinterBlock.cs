using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// Token: 0x0200000C RID: 12
public class PrinterBlock : Block
{
	// Token: 0x0600004A RID: 74 RVA: 0x000039E8 File Offset: 0x00001BE8
	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return new WorldInteraction[]
		{
			new WorldInteraction
			{
				ActionLangCode = "rpvoicechat:Printer.Interaction.CheckContents",
				MouseButton = 2
			}
		};
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003A0C File Offset: 0x00001C0C
	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (blockSel == null)
		{
			return false;
		}
		if (world.Side == 2)
		{
			BlockEntityPrinter blockEntityPrinter = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityPrinter;
			if (blockEntityPrinter != null)
			{
				blockEntityPrinter.NotifyClientInventoryOpenIntent();
			}
		}
		BlockEntityPrinter printer = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityPrinter;
		return printer != null && printer.OnPlayerRightClick(byPlayer, blockSel);
	}
}
