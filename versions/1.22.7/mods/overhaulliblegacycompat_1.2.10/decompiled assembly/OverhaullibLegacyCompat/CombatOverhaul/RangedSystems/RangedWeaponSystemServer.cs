using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	public class RangedWeaponSystemServer
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000B3F RID: 2879 RVA: 0x0003A1A0 File Offset: 0x000383A0
		// (remove) Token: 0x06000B40 RID: 2880 RVA: 0x0003A1D8 File Offset: 0x000383D8
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event RangedWeaponSystemServer.RangedWeaponStatusChangedDelegate RangedWeaponStatusChanged;

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003A210 File Offset: 0x00038410
		public RangedWeaponSystemServer(ICoreServerAPI api)
		{
			this._serverChannel = api.Network.RegisterChannel("CombatOverhaul:rangeWeapon").RegisterMessageType<ReloadPacket>().RegisterMessageType<ReloadConfirmPacket>().RegisterMessageType<ShotPacket>().RegisterMessageType<ShotConfirmPacket>().RegisterMessageType<RangedWeaponStatusPacket>().SetMessageHandler<ReloadPacket>(new NetworkClientMessageHandler<ReloadPacket>(this.HandleReloadPacket)).SetMessageHandler<ShotPacket>(new NetworkClientMessageHandler<ShotPacket>(this.HandleShotPacket)).SetMessageHandler<RangedWeaponStatusPacket>(new NetworkClientMessageHandler<RangedWeaponStatusPacket>(this.HandleWeaponStatusPacket));
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003A288 File Offset: 0x00038488
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

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003A2F4 File Offset: 0x000384F4
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

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003A3E4 File Offset: 0x000385E4
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

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003A4BC File Offset: 0x000386BC
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

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003A4EB File Offset: 0x000386EB
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

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003A51A File Offset: 0x0003871A
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

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003A549 File Offset: 0x00038749
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

		// Token: 0x06000B49 RID: 2889 RVA: 0x0003A578 File Offset: 0x00038778
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

		// Token: 0x06000B4A RID: 2890 RVA: 0x0003A641 File Offset: 0x00038841
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

		// Token: 0x0400053E RID: 1342
		public const string NetworkChannelId = "CombatOverhaul:rangeWeapon";

		// Token: 0x04000540 RID: 1344
		private readonly IServerNetworkChannel _serverChannel;

		// Token: 0x0200025E RID: 606
		// (Invoke) Token: 0x060016FC RID: 5884
		[NullableContext(0)]
		public delegate void RangedWeaponStatusChangedDelegate(Entity attacker, ItemSlot weaponSlot, RangedWeaponStatus status);
	}
}
