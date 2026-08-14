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
	// Token: 0x0200009F RID: 159
	public class ItemTelegraphWire : Item
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001A68C File Offset: 0x0001888C
		private int MaxConnectionDistance
		{
			get
			{
				return ServerConfigManager.TelegraphMaxConnectionDistance;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001A694 File Offset: 0x00018894
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

		// Token: 0x060004EF RID: 1263 RVA: 0x0001A924 File Offset: 0x00018B24
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

		// Token: 0x04000270 RID: 624
		private BlockPos firstNodePos;
	}
}
