using System;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x020000CB RID: 203
	public class ItemTelegraphWire : Item
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x000214B0 File Offset: 0x0001F6B0
		private int MaxConnectionDistance
		{
			get
			{
				return ServerConfigManager.TelegraphMaxConnectionDistance;
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000214B8 File Offset: 0x0001F6B8
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			if (blockSel == null || ((byEntity != null) ? byEntity.World : null) == null)
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
				return;
			}
			IWireConnectable node = byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) as IWireConnectable;
			if (node == null)
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
				return;
			}
			handling = 4;
			if (this.firstNodePos == null)
			{
				BEWireNode firstWireNode = node as BEWireNode;
				if (firstWireNode != null && !firstWireNode.CanAcceptMoreConnections())
				{
					ICoreClientAPI coreClientAPI = byEntity.World.Api as ICoreClientAPI;
					if (coreClientAPI == null)
					{
						return;
					}
					coreClientAPI.TriggerIngameError(this, "wire-max-connections", UIUtils.I18n("Wire.ConnectionDenied.MaxConnections", new object[]
					{
						firstWireNode.GetMaxConnections()
					}));
					return;
				}
				else
				{
					this.firstNodePos = blockSel.Position.Copy();
					ICoreClientAPI coreClientAPI2 = byEntity.World.Api as ICoreClientAPI;
					if (coreClientAPI2 == null)
					{
						return;
					}
					coreClientAPI2.TriggerChatMessage(UIUtils.I18n("Wire.StartConnection"));
					return;
				}
			}
			else
			{
				IWireConnectable firstNode = byEntity.World.BlockAccessor.GetBlockEntity(this.firstNodePos) as IWireConnectable;
				if (firstNode == null)
				{
					this.firstNodePos = null;
					return;
				}
				if (firstNode != null && firstNode != node)
				{
					if ((double)this.firstNodePos.DistanceTo(node.Position) > (double)this.MaxConnectionDistance)
					{
						ICoreClientAPI coreClientAPI3 = byEntity.Api as ICoreClientAPI;
						if (coreClientAPI3 != null)
						{
							coreClientAPI3.TriggerChatMessage(UIUtils.I18n("Wire.ConnectionTooFar", new object[]
							{
								this.MaxConnectionDistance
							}));
						}
					}
					else
					{
						WireConnection connection = new WireConnection(firstNode, node);
						string failureLangKey = null;
						object[] failureArgs = Array.Empty<object>();
						BEWireNode firstWireNode2 = firstNode as BEWireNode;
						bool connected;
						if (firstWireNode2 != null)
						{
							connected = firstWireNode2.Connect(connection, out failureLangKey, out failureArgs);
						}
						else
						{
							BEWireNode secondWireNode = node as BEWireNode;
							if (secondWireNode == null)
							{
								this.firstNodePos = null;
								return;
							}
							connected = secondWireNode.Connect(connection, out failureLangKey, out failureArgs);
						}
						if (!connected)
						{
							if (!string.IsNullOrWhiteSpace(failureLangKey))
							{
								ICoreClientAPI capi = byEntity.Api as ICoreClientAPI;
								string message = UIUtils.I18n(failureLangKey, failureArgs);
								if (failureLangKey.StartsWith("Wire.ConnectionDenied.", 4))
								{
									if (capi != null)
									{
										capi.TriggerIngameError(this, "wire-connection-denied", message);
									}
								}
								else if (capi != null)
								{
									capi.TriggerChatMessage(message);
								}
							}
							else
							{
								ICoreClientAPI coreClientAPI4 = byEntity.Api as ICoreClientAPI;
								if (coreClientAPI4 != null)
								{
									coreClientAPI4.TriggerChatMessage(UIUtils.I18n("Wire.ConnectionFailed"));
								}
							}
							this.firstNodePos = null;
							return;
						}
						ICoreClientAPI coreClientAPI5 = byEntity.Api as ICoreClientAPI;
						if (coreClientAPI5 != null)
						{
							coreClientAPI5.TriggerChatMessage(UIUtils.I18n("Wire.ConnectionSuccess"));
						}
						slot.TakeOut(1);
						slot.MarkDirty();
					}
				}
				this.firstNodePos = null;
				return;
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00021748 File Offset: 0x0001F948
		public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
		{
			base.OnHeldIdle(slot, byEntity);
			if (this.firstNodePos != null)
			{
				ICoreClientAPI capi = ((byEntity != null) ? byEntity.Api : null) as ICoreClientAPI;
				if (capi != null && byEntity == capi.World.Player.Entity)
				{
					capi.TriggerIngameError(this, "wire-pending", UIUtils.I18n("Wire.Pending", new object[]
					{
						this.firstNodePos
					}));
				}
			}
		}

		// Token: 0x0400031D RID: 797
		private BlockPos firstNodePos;
	}
}
