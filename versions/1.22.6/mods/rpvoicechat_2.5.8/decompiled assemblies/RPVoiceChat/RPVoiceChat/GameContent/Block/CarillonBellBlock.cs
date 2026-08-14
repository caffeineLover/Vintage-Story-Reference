using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000A8 RID: 168
	public class CarillonBellBlock : Block
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x0001B7D6 File Offset: 0x000199D6
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

		// Token: 0x06000519 RID: 1305 RVA: 0x0001B802 File Offset: 0x00019A02
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			base.OnBlockInteractStart(world, byPlayer, blockSel);
			return true;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001B80F File Offset: 0x00019A0F
		public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			return base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel) || true;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001B821 File Offset: 0x00019A21
		public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
		{
			base.OnBlockInteractCancel(secondsUsed, world, byPlayer, blockSel, cancelReason);
			return true;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001B832 File Offset: 0x00019A32
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
