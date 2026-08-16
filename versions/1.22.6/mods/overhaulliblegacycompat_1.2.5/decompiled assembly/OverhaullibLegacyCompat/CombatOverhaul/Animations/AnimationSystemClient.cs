using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200016A RID: 362
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationSystemClient
	{
		// Token: 0x06001227 RID: 4647 RVA: 0x00058950 File Offset: 0x00056B50
		public AnimationSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CO-animation-system").RegisterMessageType<AnimationRequestPacket>().RegisterMessageType<AnimationStopRequestPacket>().SetMessageHandler<AnimationRequestPacket>(new NetworkServerMessageHandler<AnimationRequestPacket>(this.HandlePacket)).SetMessageHandler<AnimationStopRequestPacket>(new NetworkServerMessageHandler<AnimationStopRequestPacket>(this.HandlePacket));
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000589AC File Offset: 0x00056BAC
		public void SendPlayPacket(AnimationRequestByCode request, bool mainHand, long entityId, int itemId)
		{
			this._clientChannel.SendPacket<AnimationRequestPacket>(new AnimationRequestPacket(request, mainHand, entityId, itemId));
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x000589C3 File Offset: 0x00056BC3
		public void SendStopPacket(string category, long entityId)
		{
			this._clientChannel.SendPacket<AnimationStopRequestPacket>(new AnimationStopRequestPacket(category, entityId));
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x000589D8 File Offset: 0x00056BD8
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

		// Token: 0x0600122B RID: 4651 RVA: 0x00058A3F File Offset: 0x00056C3F
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

		// Token: 0x0600122C RID: 4652 RVA: 0x00058A74 File Offset: 0x00056C74
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

		// Token: 0x04000884 RID: 2180
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x04000885 RID: 2181
		private readonly ICoreClientAPI _api;
	}
}
