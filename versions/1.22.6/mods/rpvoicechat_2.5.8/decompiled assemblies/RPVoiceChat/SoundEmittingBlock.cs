using System;
using RPVoiceChat.GameContent.BlockEntityBehavior;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// Token: 0x0200000D RID: 13
public class SoundEmittingBlock : Block
{
	// Token: 0x0600004D RID: 77 RVA: 0x00003A74 File Offset: 0x00001C74
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

	// Token: 0x0600004E RID: 78 RVA: 0x00003A96 File Offset: 0x00001C96
	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		return true;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003A99 File Offset: 0x00001C99
	public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		return true;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003A9C File Offset: 0x00001C9C
	public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
	{
		return true;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003AA0 File Offset: 0x00001CA0
	public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (EnumAppSideExtensions.IsServer(world.Side))
		{
			BlockEntity be = world.BlockAccessor.GetBlockEntity(blockSel.Position);
			if (be != null)
			{
				foreach (BlockEntityBehavior blockEntityBehavior in be.Behaviors)
				{
					IBlockEntityRungBehavior rung = blockEntityBehavior as IBlockEntityRungBehavior;
					if (rung != null)
					{
						rung.OnRung();
					}
				}
			}
		}
		base.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel);
	}
}
