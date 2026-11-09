using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000DC RID: 220
	public class SignalLampBlock : Block
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x00023134 File Offset: 0x00021334
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BlockEntitySignalLamp blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntitySignalLamp;
			if (blockEntity != null)
			{
				if (world.Side == 1)
				{
					BEBehaviorLightable lightBehavior = blockEntity.GetBehavior<BEBehaviorLightable>();
					if (lightBehavior != null)
					{
						lightBehavior.SetLightActive(true);
					}
					blockEntity.StartSlatesAnimation();
				}
				else
				{
					blockEntity.StartSlatesAnimation();
				}
			}
			return true;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00023184 File Offset: 0x00021384
		public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			return true;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00023188 File Offset: 0x00021388
		public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
		{
			BlockEntitySignalLamp blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntitySignalLamp;
			if (blockEntity != null)
			{
				if (world.Side == 1)
				{
					BEBehaviorLightable lightBehavior = blockEntity.GetBehavior<BEBehaviorLightable>();
					if (lightBehavior != null)
					{
						lightBehavior.SetLightActive(false);
					}
					blockEntity.StopSlatesAnimation();
				}
				else
				{
					blockEntity.StopSlatesAnimation();
				}
			}
			return true;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000231DC File Offset: 0x000213DC
		public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BlockEntitySignalLamp blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntitySignalLamp;
			if (blockEntity != null)
			{
				if (world.Side == 1)
				{
					BEBehaviorLightable lightBehavior = blockEntity.GetBehavior<BEBehaviorLightable>();
					if (lightBehavior != null)
					{
						lightBehavior.SetLightActive(false);
					}
					blockEntity.StopSlatesAnimation();
					return;
				}
				blockEntity.StopSlatesAnimation();
			}
		}
	}
}
