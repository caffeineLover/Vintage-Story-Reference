using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000176 RID: 374
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationSystemClient
	{
		// Token: 0x0600129B RID: 4763 RVA: 0x0005A270 File Offset: 0x00058470
		public AnimationSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CO-animation-system").RegisterMessageType<AnimationRequestPacket>().RegisterMessageType<AnimationStopRequestPacket>().SetMessageHandler<AnimationRequestPacket>(new NetworkServerMessageHandler<AnimationRequestPacket>(this.HandlePacket)).SetMessageHandler<AnimationStopRequestPacket>(new NetworkServerMessageHandler<AnimationStopRequestPacket>(this.HandlePacket));
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0005A2CC File Offset: 0x000584CC
		public void SendPlayPacket(AnimationRequestByCode request, bool mainHand, long entityId, int itemId)
		{
			this._clientChannel.SendPacket<AnimationRequestPacket>(new AnimationRequestPacket(request, mainHand, entityId, itemId));
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0005A2E3 File Offset: 0x000584E3
		public void SendStopPacket(string category, long entityId)
		{
			this._clientChannel.SendPacket<AnimationStopRequestPacket>(new AnimationStopRequestPacket(category, entityId));
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0005A2F8 File Offset: 0x000584F8
		private void HandlePacket(AnimationRequestPacket packet)
		{
			EntityPlayer player = this._api.World.GetEntityById(packet.EntityId) as EntityPlayer;
			if (player == null)
			{
				return;
			}
			if (AnimationSystemClient.GetCurrentItemId(packet.MainHand, player) != packet.ItemId)
			{
				return;
			}
			ValueTuple<AnimationRequestByCode, bool> valueTuple = packet.ToRequest();
			AnimationRequestByCode request = valueTuple.Item1;
			bool mainHand = valueTuple.Item2;
			ThirdPersonAnimationsBehavior behavior = player.GetBehavior<ThirdPersonAnimationsBehavior>();
			if (behavior == null)
			{
				return;
			}
			behavior.Play(request, mainHand);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0005A35F File Offset: 0x0005855F
		private void HandlePacket(AnimationStopRequestPacket packet)
		{
			Entity entityById = this._api.World.GetEntityById(packet.EntityId);
			if (entityById == null)
			{
				return;
			}
			ThirdPersonAnimationsBehavior behavior = entityById.GetBehavior<ThirdPersonAnimationsBehavior>();
			if (behavior == null)
			{
				return;
			}
			behavior.Stop(packet.Category);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0005A394 File Offset: 0x00058594
		private static int GetCurrentItemId(bool mainHand, EntityPlayer player)
		{
			int? num2;
			if (!mainHand)
			{
				int? num;
				if (player == null)
				{
					num = default(int?);
				}
				else
				{
					ItemSlot leftHandItemSlot = player.LeftHandItemSlot;
					if (leftHandItemSlot == null)
					{
						num = default(int?);
					}
					else
					{
						ItemStack itemstack = leftHandItemSlot.Itemstack;
						if (itemstack == null)
						{
							num = default(int?);
						}
						else
						{
							Item item = itemstack.Item;
							num = ((item != null) ? new int?(item.Id) : default(int?));
						}
					}
				}
				num2 = num;
				return num2.GetValueOrDefault();
			}
			int? num3;
			if (player == null)
			{
				num3 = default(int?);
			}
			else
			{
				ItemSlot rightHandItemSlot = player.RightHandItemSlot;
				if (rightHandItemSlot == null)
				{
					num3 = default(int?);
				}
				else
				{
					ItemStack itemstack2 = rightHandItemSlot.Itemstack;
					if (itemstack2 == null)
					{
						num3 = default(int?);
					}
					else
					{
						Item item2 = itemstack2.Item;
						num3 = ((item2 != null) ? new int?(item2.Id) : default(int?));
					}
				}
			}
			num2 = num3;
			return num2.GetValueOrDefault();
		}

		// Token: 0x040008AA RID: 2218
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x040008AB RID: 2219
		private readonly ICoreClientAPI _api;
	}
}
