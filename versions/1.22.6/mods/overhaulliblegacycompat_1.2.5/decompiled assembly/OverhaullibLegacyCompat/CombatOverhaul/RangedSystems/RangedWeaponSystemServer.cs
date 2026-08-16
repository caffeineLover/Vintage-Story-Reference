using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D0 RID: 208
	[NullableContext(1)]
	[Nullable(0)]
	public class RangedWeaponSystemServer
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000AE3 RID: 2787 RVA: 0x000391B0 File Offset: 0x000373B0
		// (remove) Token: 0x06000AE4 RID: 2788 RVA: 0x000391E8 File Offset: 0x000373E8
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event RangedWeaponSystemServer.RangedWeaponStatusChangedDelegate RangedWeaponStatusChanged;

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00039220 File Offset: 0x00037420
		public RangedWeaponSystemServer(ICoreServerAPI api)
		{
			this._serverChannel = api.Network.RegisterChannel("CombatOverhaul:rangeWeapon").RegisterMessageType<ReloadPacket>().RegisterMessageType<ReloadConfirmPacket>().RegisterMessageType<ShotPacket>().RegisterMessageType<ShotConfirmPacket>().RegisterMessageType<RangedWeaponStatusPacket>().SetMessageHandler<ReloadPacket>(new NetworkClientMessageHandler<ReloadPacket>(this.HandleReloadPacket)).SetMessageHandler<ShotPacket>(new NetworkClientMessageHandler<ShotPacket>(this.HandleShotPacket)).SetMessageHandler<RangedWeaponStatusPacket>(new NetworkClientMessageHandler<RangedWeaponStatusPacket>(this.HandleWeaponStatusPacket));
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00039298 File Offset: 0x00037498
		private void HandleWeaponStatusPacket(IServerPlayer player, RangedWeaponStatusPacket packet)
		{
			ItemSlot weaponSlot = packet.MainHand ? player.Entity.ActiveHandItemSlot : player.Entity.LeftHandItemSlot;
			if (SlowDiagnostics.Enabled)
			{
				SlowDiagnostics.InvokeRangedWeaponStatusSubscribers(this.RangedWeaponStatusChanged, player, weaponSlot, packet.Status, packet.MainHand);
				return;
			}
			RangedWeaponSystemServer.RangedWeaponStatusChangedDelegate rangedWeaponStatusChanged = this.RangedWeaponStatusChanged;
			if (rangedWeaponStatusChanged == null)
			{
				return;
			}
			rangedWeaponStatusChanged(player.Entity, weaponSlot, packet.Status);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00039304 File Offset: 0x00037504
		private void HandleReloadPacket(IServerPlayer player, ReloadPacket packet)
		{
			ItemSlot weaponSlot = RangedWeaponSystemServer.GetWeaponSlot(player, packet.RightHand, packet.ItemId);
			ItemStack itemstack = weaponSlot.Itemstack;
			int? num;
			if (itemstack == null)
			{
				num = default(int?);
			}
			else
			{
				Item item = itemstack.Item;
				num = ((item != null) ? new int?(item.Id) : default(int?));
			}
			int? num2 = num;
			int itemId = packet.ItemId;
			if (!(num2.GetValueOrDefault() == itemId & num2 != null))
			{
				this.OnFailedReload(player, packet);
				return;
			}
			IHasRangedWeaponLogic rangedWeapon = weaponSlot.Itemstack.Item as IHasRangedWeaponLogic;
			if (rangedWeapon == null)
			{
				this.OnFailedReload(player, packet);
				return;
			}
			ItemSlot ammoSlot = null;
			if (packet.SlotId != null)
			{
				ammoSlot = RangedWeaponSystemServer.GetAmmoSlot(player, packet.InventoryId, packet.SlotId.Value);
			}
			IServerRangedWeaponLogic serverWeaponLogic = rangedWeapon.ServerWeaponLogic;
			if (serverWeaponLogic != null && serverWeaponLogic.Reload(player, weaponSlot, ammoSlot, packet))
			{
				this.OnSuccessfulReload(player, packet);
				return;
			}
			this.OnFailedReload(player, packet);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000393F4 File Offset: 0x000375F4
		private void HandleShotPacket(IServerPlayer player, ShotPacket packet)
		{
			ItemSlot weaponSlot = RangedWeaponSystemServer.GetWeaponSlot(player, packet.RightHand, packet.ItemId);
			ItemStack itemstack = weaponSlot.Itemstack;
			int? num;
			if (itemstack == null)
			{
				num = default(int?);
			}
			else
			{
				Item item = itemstack.Item;
				num = ((item != null) ? new int?(item.Id) : default(int?));
			}
			int? num2 = num;
			int itemId = packet.ItemId;
			if (!(num2.GetValueOrDefault() == itemId & num2 != null))
			{
				this.OnFailedShot(player, packet);
				return;
			}
			IHasRangedWeaponLogic rangedWeapon = weaponSlot.Itemstack.Item as IHasRangedWeaponLogic;
			if (rangedWeapon == null)
			{
				this.OnFailedShot(player, packet);
				return;
			}
			IServerRangedWeaponLogic serverWeaponLogic = rangedWeapon.ServerWeaponLogic;
			if (serverWeaponLogic != null && serverWeaponLogic.Shoot(player, weaponSlot, packet, player.Entity))
			{
				WeaponBuffSystem weaponBuffSystem = WeaponBuffSystem.Current;
				if (weaponBuffSystem != null)
				{
					weaponBuffSystem.Consume(weaponSlot, WeaponBuffConsumptionTrigger.RangedShot);
				}
				this.OnSuccessfulShot(player, packet);
				return;
			}
			this.OnFailedShot(player, packet);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000394CC File Offset: 0x000376CC
		private void OnFailedReload(IServerPlayer player, ReloadPacket packet)
		{
			this._serverChannel.SendPacket<ReloadConfirmPacket>(new ReloadConfirmPacket
			{
				ReloadId = packet.ReloadId,
				Success = false
			}, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000394FB File Offset: 0x000376FB
		private void OnSuccessfulReload(IServerPlayer player, ReloadPacket packet)
		{
			this._serverChannel.SendPacket<ReloadConfirmPacket>(new ReloadConfirmPacket
			{
				ReloadId = packet.ReloadId,
				Success = true
			}, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0003952A File Offset: 0x0003772A
		private void OnFailedShot(IServerPlayer player, ShotPacket packet)
		{
			this._serverChannel.SendPacket<ShotConfirmPacket>(new ShotConfirmPacket
			{
				ShotId = packet.ShotId,
				Success = false
			}, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00039559 File Offset: 0x00037759
		private void OnSuccessfulShot(IServerPlayer player, ShotPacket packet)
		{
			this._serverChannel.SendPacket<ShotConfirmPacket>(new ShotConfirmPacket
			{
				ShotId = packet.ShotId,
				Success = true
			}, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00039588 File Offset: 0x00037788
		private static ItemSlot GetWeaponSlot(IServerPlayer player, bool rightHand, int itemId)
		{
			if (!rightHand)
			{
				return player.Entity.LeftHandItemSlot;
			}
			ItemSlot active = player.Entity.ActiveHandItemSlot;
			ItemStack itemstack = active.Itemstack;
			bool flag;
			if (itemstack == null)
			{
				flag = false;
			}
			else
			{
				Item item = itemstack.Item;
				int? num = (item != null) ? new int?(item.Id) : default(int?);
				flag = (num.GetValueOrDefault() == itemId & num != null);
			}
			if (flag)
			{
				return active;
			}
			ItemSlot right = player.Entity.RightHandItemSlot;
			ItemStack itemstack2 = right.Itemstack;
			bool flag2;
			if (itemstack2 == null)
			{
				flag2 = false;
			}
			else
			{
				Item item2 = itemstack2.Item;
				int? num = (item2 != null) ? new int?(item2.Id) : default(int?);
				flag2 = (num.GetValueOrDefault() == itemId & num != null);
			}
			if (flag2)
			{
				return right;
			}
			if (active.Itemstack == null)
			{
				return right;
			}
			return active;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00039651 File Offset: 0x00037851
		[return: Nullable(2)]
		private static ItemSlot GetAmmoSlot(IServerPlayer player, string inventoryId, int slotId)
		{
			IInventory inventory = player.InventoryManager.GetInventory(inventoryId);
			if (inventory == null)
			{
				return null;
			}
			return inventory[slotId];
		}

		// Token: 0x0400051C RID: 1308
		public const string NetworkChannelId = "CombatOverhaul:rangeWeapon";

		// Token: 0x0400051E RID: 1310
		private readonly IServerNetworkChannel _serverChannel;

		// Token: 0x0200024F RID: 591
		// (Invoke) Token: 0x06001681 RID: 5761
		[NullableContext(0)]
		public delegate void RangedWeaponStatusChangedDelegate(Entity attacker, ItemSlot weaponSlot, RangedWeaponStatus status);
	}
}
