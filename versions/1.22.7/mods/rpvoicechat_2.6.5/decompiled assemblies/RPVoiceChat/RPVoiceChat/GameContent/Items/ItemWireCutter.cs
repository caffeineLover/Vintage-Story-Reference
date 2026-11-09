using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x020000CC RID: 204
	public class ItemWireCutter : Item
	{
		// Token: 0x0600062F RID: 1583 RVA: 0x000217C0 File Offset: 0x0001F9C0
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			if (blockSel == null || ((byEntity != null) ? byEntity.World : null) == null)
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
				return;
			}
			BEWireNode wireNode = byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BEWireNode;
			if (wireNode == null)
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
				return;
			}
			handling = 4;
			if (byEntity.World.Side != 1)
			{
				return;
			}
			int connectionCount = wireNode.GetConnections().Count;
			foreach (WireConnection connection in new List<WireConnection>(wireNode.GetConnections()))
			{
				BEWireNode other = connection.GetOtherNode(wireNode);
				if (other != null)
				{
					other.RemoveConnection(connection);
					other.MarkForUpdate();
				}
				wireNode.RemoveConnection(connection);
			}
			wireNode.MarkForUpdate();
			if (byEntity.World.Side == 1 && connectionCount > 0)
			{
				this.DamageItem(byEntity.World, byEntity, slot, 1, true);
				ItemWireCutter.DropRecoveredCableLoot(byEntity.World, blockSel.Position, connectionCount);
			}
			ICoreClientAPI capi = ((byEntity != null) ? byEntity.Api : null) as ICoreClientAPI;
			if (capi != null)
			{
				capi.TriggerChatMessage(UIUtils.I18n("Wire.ConnectionsRemoved", new object[]
				{
					connectionCount
				}));
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00021918 File Offset: 0x0001FB18
		private static void DropRecoveredCableLoot(IWorldAccessor world, BlockPos atPos, int removedConnections)
		{
			Item wireItem = world.GetItem(new AssetLocation(RPVoiceChatMod.modID, "telegraphwire"));
			Item copperBits = world.GetItem(new AssetLocation("game", "metalbit-copper"));
			if (wireItem == null && copperBits == null)
			{
				return;
			}
			for (int i = 0; i < removedConnections; i++)
			{
				ItemStack dropStack = null;
				if (world.Rand.NextDouble() < 0.75)
				{
					if (wireItem != null)
					{
						dropStack = new ItemStack(wireItem, 1);
					}
				}
				else if (copperBits != null)
				{
					int copperAmount = 3 + world.Rand.Next(5);
					dropStack = new ItemStack(copperBits, copperAmount);
				}
				if (dropStack != null)
				{
					world.SpawnItemEntity(dropStack, atPos.ToVec3d().Add(0.5, 0.5, 0.5), null);
				}
			}
		}

		// Token: 0x0400031E RID: 798
		private const double ReturnWireChance = 0.75;
	}
}
