using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000195 RID: 405
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AttachableSystemClient
	{
		// Token: 0x060013B5 RID: 5045 RVA: 0x00061E24 File Offset: 0x00060024
		public AttachableSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CO-attachablesystem").RegisterMessageType<AttachableAttachPacket>().RegisterMessageType<AttachableClearPacket>().RegisterMessageType<AttachableSwitchModelsPacket>().SetMessageHandler<AttachableAttachPacket>(new NetworkServerMessageHandler<AttachableAttachPacket>(this.HandlePacket)).SetMessageHandler<AttachableClearPacket>(new NetworkServerMessageHandler<AttachableClearPacket>(this.HandlePacket)).SetMessageHandler<AttachableSwitchModelsPacket>(new NetworkServerMessageHandler<AttachableSwitchModelsPacket>(this.HandlePacket));
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00061E96 File Offset: 0x00060096
		public void SendAttachPacket(long entityId, string attachmentCode, ItemStack stack, ModelTransform transform)
		{
			this._clientChannel.SendPacket<AttachableAttachPacket>(new AttachableAttachPacket(entityId, attachmentCode, stack, transform));
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00061EAD File Offset: 0x000600AD
		public void SendClearPacket(long entityId)
		{
			this._clientChannel.SendPacket<AttachableClearPacket>(new AttachableClearPacket(entityId));
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00061EC0 File Offset: 0x000600C0
		public void SendSwitchModelPacket(long entityId, bool switchModel)
		{
			this._clientChannel.SendPacket<AttachableSwitchModelsPacket>(new AttachableSwitchModelsPacket(entityId, switchModel));
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00061ED4 File Offset: 0x000600D4
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

		// Token: 0x060013BA RID: 5050 RVA: 0x00061F5C File Offset: 0x0006015C
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

		// Token: 0x060013BB RID: 5051 RVA: 0x00061FC8 File Offset: 0x000601C8
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

		// Token: 0x040009CF RID: 2511
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x040009D0 RID: 2512
		private readonly ICoreClientAPI _api;
	}
}
