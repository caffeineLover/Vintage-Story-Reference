using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D4 RID: 212
	public class CarillonBellBlock : Block
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x0002269D File Offset: 0x0002089D
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = RPVoiceChatMod.modID + ":Bell.Interaction.Ring",
					MouseButton = 2
				}
			};
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x000226C9 File Offset: 0x000208C9
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			base.OnBlockInteractStart(world, byPlayer, blockSel);
			return true;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000226D6 File Offset: 0x000208D6
		public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			return base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel) || true;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000226E8 File Offset: 0x000208E8
		public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
		{
			base.OnBlockInteractCancel(secondsUsed, world, byPlayer, blockSel, cancelReason);
			return true;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000226F9 File Offset: 0x000208F9
		public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			if (EnumAppSideExtensions.IsServer(world.Side))
			{
				BlockEntityCarillonBell blockEntityCarillonBell = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityCarillonBell;
				if (blockEntityCarillonBell != null)
				{
					blockEntityCarillonBell.OnRung();
				}
			}
			base.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel);
		}
	}
}
