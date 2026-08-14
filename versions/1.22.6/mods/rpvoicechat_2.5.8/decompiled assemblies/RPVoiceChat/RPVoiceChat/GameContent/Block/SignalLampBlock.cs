using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000AA RID: 170
	public class SignalLampBlock : Block
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x0001BB08 File Offset: 0x00019D08
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

		// Token: 0x06000523 RID: 1315 RVA: 0x0001BB58 File Offset: 0x00019D58
		public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			return true;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001BB5C File Offset: 0x00019D5C
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

		// Token: 0x06000525 RID: 1317 RVA: 0x0001BBB0 File Offset: 0x00019DB0
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
