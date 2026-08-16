using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CF RID: 207
	[NullableContext(1)]
	[Nullable(0)]
	public class RangedWeaponSystemClient
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000AD9 RID: 2777 RVA: 0x00038C3C File Offset: 0x00036E3C
		// (remove) Token: 0x06000ADA RID: 2778 RVA: 0x00038C74 File Offset: 0x00036E74
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event RangedWeaponSystemClient.RangedWeaponStatusChangedDelegate RangedWeaponStatusChanged;

		// Token: 0x06000ADB RID: 2779 RVA: 0x00038CAC File Offset: 0x00036EAC
		public RangedWeaponSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:rangeWeapon").RegisterMessageType<ReloadPacket>().RegisterMessageType<ReloadConfirmPacket>().RegisterMessageType<ShotPacket>().RegisterMessageType<ShotConfirmPacket>().RegisterMessageType<RangedWeaponStatusPacket>().SetMessageHandler<ReloadConfirmPacket>(new NetworkServerMessageHandler<ReloadConfirmPacket>(this.HandleReloadPacket)).SetMessageHandler<ShotConfirmPacket>(new NetworkServerMessageHandler<ShotConfirmPacket>(this.HandleShotPacket));
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00038D24 File Offset: 0x00036F24
		public void SendStatusChange(EntityPlayer attacker, RangedWeaponStatus status, bool mainHand)
		{
			ItemSlot weaponSlot = mainHand ? attacker.ActiveHandItemSlot : attacker.LeftHandItemSlot;
			RangedWeaponSystemClient.RangedWeaponStatusChangedDelegate rangedWeaponStatusChanged = this.RangedWeaponStatusChanged;
			if (rangedWeaponStatusChanged != null)
			{
				rangedWeaponStatusChanged(attacker, weaponSlot, status);
			}
			this._clientChannel.SendPacket<RangedWeaponStatusPacket>(new RangedWeaponStatusPacket
			{
				Status = status,
				MainHand = mainHand
			});
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00038D78 File Offset: 0x00036F78
		public void Reload(ItemSlot weapon, ItemSlot ammo, int amount, bool rightHand, Action<bool> reloadCallback, [Nullable(2)] byte[] data = null)
		{
			if (this._nextId > 1073741823)
			{
				this._nextId = 0;
			}
			int nextId = this._nextId;
			this._nextId = nextId + 1;
			int id = nextId;
			this._callbacks[id] = reloadCallback;
			InventoryBase inventory = ammo.Inventory;
			int slotId = inventory.GetSlotId(ammo);
			ReloadPacket reloadPacket = new ReloadPacket();
			reloadPacket.InventoryId = inventory.InventoryID;
			reloadPacket.SlotId = new int?(slotId);
			reloadPacket.Amount = amount;
			reloadPacket.RightHand = rightHand;
			ItemStack itemstack = weapon.Itemstack;
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
			reloadPacket.ItemId = num2.GetValueOrDefault();
			reloadPacket.ReloadId = id;
			reloadPacket.Data = (data ?? Array.Empty<byte>());
			ReloadPacket packet = reloadPacket;
			this._clientChannel.SendPacket<ReloadPacket>(packet);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00038E60 File Offset: 0x00037060
		public void Unload(ItemSlot weapon, int amount, bool rightHand, Action<bool> reloadCallback, [Nullable(2)] byte[] data = null)
		{
			if (this._nextId > 1073741823)
			{
				this._nextId = 0;
			}
			int nextId = this._nextId;
			this._nextId = nextId + 1;
			int id = nextId;
			this._callbacks[id] = reloadCallback;
			ReloadPacket reloadPacket = new ReloadPacket();
			reloadPacket.InventoryId = "";
			reloadPacket.SlotId = default(int?);
			reloadPacket.Amount = amount;
			reloadPacket.RightHand = rightHand;
			ItemStack itemstack = weapon.Itemstack;
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
			reloadPacket.ItemId = num2.GetValueOrDefault();
			reloadPacket.ReloadId = id;
			reloadPacket.Data = (data ?? Array.Empty<byte>());
			ReloadPacket packet = reloadPacket;
			this._clientChannel.SendPacket<ReloadPacket>(packet);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00038F34 File Offset: 0x00037134
		public void Load(ItemSlot weapon, bool rightHand, Action<bool> reloadCallback, int amount = 0, [Nullable(2)] byte[] data = null)
		{
			if (this._nextId > 1073741823)
			{
				this._nextId = 0;
			}
			int nextId = this._nextId;
			this._nextId = nextId + 1;
			int id = nextId;
			this._callbacks[id] = reloadCallback;
			ReloadPacket reloadPacket = new ReloadPacket();
			reloadPacket.InventoryId = "";
			reloadPacket.SlotId = default(int?);
			reloadPacket.Amount = amount;
			reloadPacket.RightHand = rightHand;
			ItemStack itemstack = weapon.Itemstack;
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
			reloadPacket.ItemId = num2.GetValueOrDefault();
			reloadPacket.ReloadId = id;
			reloadPacket.Data = (data ?? Array.Empty<byte>());
			ReloadPacket packet = reloadPacket;
			this._clientChannel.SendPacket<ReloadPacket>(packet);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00039008 File Offset: 0x00037208
		public void Shoot(ItemSlot weapon, int amount, Vector3d position, Vector3d velocity, bool rightHand, Action<bool> shootCallback, [Nullable(2)] byte[] data = null)
		{
			List<Guid> projectilesIds = new List<Guid>();
			for (int count = 0; count < amount; count++)
			{
				projectilesIds.Add(Guid.NewGuid());
			}
			if (this._nextId > 1073741823)
			{
				this._nextId = 0;
			}
			int nextId = this._nextId;
			this._nextId = nextId + 1;
			int id = nextId;
			this._callbacks[id] = shootCallback;
			ShotPacket shotPacket = new ShotPacket();
			shotPacket.ProjectileId = projectilesIds.ToArray();
			shotPacket.ShotId = id;
			shotPacket.Position = new double[]
			{
				position.X,
				position.Y,
				position.Z
			};
			shotPacket.Velocity = new double[]
			{
				velocity.X,
				velocity.Y,
				velocity.Z
			};
			ShotPacket shotPacket2 = shotPacket;
			ItemStack itemstack = weapon.Itemstack;
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
			shotPacket2.ItemId = num2.GetValueOrDefault();
			shotPacket.Amount = amount;
			shotPacket.RightHand = rightHand;
			shotPacket.Data = (data ?? Array.Empty<byte>());
			ShotPacket packet = shotPacket;
			this._clientChannel.SendPacket<ShotPacket>(packet);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00039150 File Offset: 0x00037350
		private void HandleReloadPacket(ReloadConfirmPacket packet)
		{
			Action<bool> callback;
			if (this._callbacks.TryGetValue(packet.ReloadId, ref callback))
			{
				callback.Invoke(packet.Success);
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00039180 File Offset: 0x00037380
		private void HandleShotPacket(ShotConfirmPacket packet)
		{
			Action<bool> callback;
			if (this._callbacks.TryGetValue(packet.ShotId, ref callback))
			{
				callback.Invoke(packet.Success);
			}
		}

		// Token: 0x04000516 RID: 1302
		public const string NetworkChannelId = "CombatOverhaul:rangeWeapon";

		// Token: 0x04000518 RID: 1304
		private readonly ICoreClientAPI _api;

		// Token: 0x04000519 RID: 1305
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x0400051A RID: 1306
		private readonly Dictionary<int, Action<bool>> _callbacks = new Dictionary<int, Action<bool>>();

		// Token: 0x0400051B RID: 1307
		private int _nextId;

		// Token: 0x0200024E RID: 590
		// (Invoke) Token: 0x0600167D RID: 5757
		[NullableContext(0)]
		public delegate void RangedWeaponStatusChangedDelegate(Entity attacker, ItemSlot weaponSlot, RangedWeaponStatus status);
	}
}
