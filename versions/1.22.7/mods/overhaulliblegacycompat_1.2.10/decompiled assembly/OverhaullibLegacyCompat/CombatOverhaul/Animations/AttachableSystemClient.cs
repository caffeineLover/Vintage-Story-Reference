using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A3 RID: 419
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AttachableSystemClient
	{
		// Token: 0x0600142D RID: 5165 RVA: 0x000638A4 File Offset: 0x00061AA4
		public AttachableSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CO-attachablesystem").RegisterMessageType<AttachableAttachPacket>().RegisterMessageType<AttachableClearPacket>().RegisterMessageType<AttachableSwitchModelsPacket>().SetMessageHandler<AttachableAttachPacket>(new NetworkServerMessageHandler<AttachableAttachPacket>(this.HandlePacket)).SetMessageHandler<AttachableClearPacket>(new NetworkServerMessageHandler<AttachableClearPacket>(this.HandlePacket)).SetMessageHandler<AttachableSwitchModelsPacket>(new NetworkServerMessageHandler<AttachableSwitchModelsPacket>(this.HandlePacket));
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00063916 File Offset: 0x00061B16
		public void SendAttachPacket(long entityId, string attachmentCode, ItemStack stack, ModelTransform transform)
		{
			this._clientChannel.SendPacket<AttachableAttachPacket>(new AttachableAttachPacket(entityId, attachmentCode, stack, transform));
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0006392D File Offset: 0x00061B2D
		public void SendClearPacket(long entityId)
		{
			this._clientChannel.SendPacket<AttachableClearPacket>(new AttachableClearPacket(entityId));
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00063940 File Offset: 0x00061B40
		public void SendSwitchModelPacket(long entityId, bool switchModel)
		{
			this._clientChannel.SendPacket<AttachableSwitchModelsPacket>(new AttachableSwitchModelsPacket(entityId, switchModel));
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x00063954 File Offset: 0x00061B54
		private void HandlePacket(AttachableAttachPacket packet)
		{
			EntityPlayer player = this._api.World.GetEntityById(packet.EntityId) as EntityPlayer;
			if (player == null)
			{
				return;
			}
			ItemSlot activeHandItemSlot = player.ActiveHandItemSlot;
			object obj;
			if (activeHandItemSlot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = activeHandItemSlot.Itemstack;
				if (itemstack == null)
				{
					obj = null;
				}
				else
				{
					Item item = itemstack.Item;
					obj = ((item != null) ? item.GetCollectibleBehavior<AnimatableAttachable>(true) : null);
				}
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.SetAttachment(packet.EntityId, packet.AttachmentCode, packet.GetItemStack(player.Api.World), packet.GetModelTransform(), true, false);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000639DC File Offset: 0x00061BDC
		private void HandlePacket(AttachableClearPacket packet)
		{
			EntityPlayer player = this._api.World.GetEntityById(packet.EntityId) as EntityPlayer;
			if (player == null)
			{
				return;
			}
			ItemSlot activeHandItemSlot = player.ActiveHandItemSlot;
			object obj;
			if (activeHandItemSlot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = activeHandItemSlot.Itemstack;
				if (itemstack == null)
				{
					obj = null;
				}
				else
				{
					Item item = itemstack.Item;
					obj = ((item != null) ? item.GetCollectibleBehavior<AnimatableAttachable>(true) : null);
				}
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.ClearAttachments(packet.EntityId);
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00063A48 File Offset: 0x00061C48
		private void HandlePacket(AttachableSwitchModelsPacket packet)
		{
			EntityPlayer player = this._api.World.GetEntityById(packet.EntityId) as EntityPlayer;
			if (player == null)
			{
				return;
			}
			ItemSlot activeHandItemSlot = player.ActiveHandItemSlot;
			object obj;
			if (activeHandItemSlot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = activeHandItemSlot.Itemstack;
				if (itemstack == null)
				{
					obj = null;
				}
				else
				{
					Item item = itemstack.Item;
					obj = ((item != null) ? item.GetCollectibleBehavior<AnimatableAttachable>(true) : null);
				}
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.SetSwitchModels(packet.EntityId, packet.SwitchModels);
		}

		// Token: 0x040009F7 RID: 2551
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x040009F8 RID: 2552
		private readonly ICoreClientAPI _api;
	}
}
