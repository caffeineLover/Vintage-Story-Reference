using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

// Token: 0x02000010 RID: 16
public abstract class WireNodeBlock : Block
{
	// Token: 0x0600005A RID: 90 RVA: 0x00003D84 File Offset: 0x00001F84
	public override float OnGettingBroken(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter)
	{
		BEWireNode wireNode = player.Entity.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BEWireNode;
		if (wireNode != null && wireNode.GetConnections().Count > 0)
		{
			ICoreClientAPI capi = player.Entity.World.Api as ICoreClientAPI;
			if (capi != null)
			{
				long currentTime = capi.World.ElapsedMilliseconds;
				if (blockSel.Position != WireNodeBlock.lastWarningPos)
				{
					WireNodeBlock.lastWarningPos = blockSel.Position;
					WireNodeBlock.lastWarningTime = 0L;
				}
				if (currentTime - WireNodeBlock.lastWarningTime >= 2000L)
				{
					capi.TriggerIngameError(this, "wire-connected", UIUtils.I18n("Wire.CannotBreakConnected"));
					WireNodeBlock.lastWarningTime = currentTime;
				}
			}
			return remainingResistance;
		}
		return base.OnGettingBroken(player, blockSel, itemslot, remainingResistance, dt, counter);
	}

	// Token: 0x0400001A RID: 26
	private static BlockPos lastWarningPos;

	// Token: 0x0400001B RID: 27
	private static long lastWarningTime;
}
