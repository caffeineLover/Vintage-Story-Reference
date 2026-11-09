using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000DA RID: 218
	[NullableContext(1)]
	[Nullable(0)]
	public class RangedWeaponSystemClient
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000B35 RID: 2869 RVA: 0x00039C2C File Offset: 0x00037E2C
		// (remove) Token: 0x06000B36 RID: 2870 RVA: 0x00039C64 File Offset: 0x00037E64
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event RangedWeaponSystemClient.RangedWeaponStatusChangedDelegate RangedWeaponStatusChanged;

		// Token: 0x06000B37 RID: 2871 RVA: 0x00039C9C File Offset: 0x00037E9C
		public RangedWeaponSystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:rangeWeapon").RegisterMessageType<ReloadPacket>().RegisterMessageType<ReloadConfirmPacket>().RegisterMessageType<ShotPacket>().RegisterMessageType<ShotConfirmPacket>().RegisterMessageType<RangedWeaponStatusPacket>().SetMessageHandler<ReloadConfirmPacket>(new NetworkServerMessageHandler<ReloadConfirmPacket>(this.HandleReloadPacket)).SetMessageHandler<ShotConfirmPacket>(new NetworkServerMessageHandler<ShotConfirmPacket>(this.HandleShotPacket));
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00039D14 File Offset: 0x00037F14
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

		// Token: 0x06000B39 RID: 2873 RVA: 0x00039D68 File Offset: 0x00037F68
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

		// Token: 0x06000B3A RID: 2874 RVA: 0x00039E50 File Offset: 0x00038050
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

		// Token: 0x06000B3B RID: 2875 RVA: 0x00039F24 File Offset: 0x00038124
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

		// Token: 0x06000B3C RID: 2876 RVA: 0x00039FF8 File Offset: 0x000381F8
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

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003A140 File Offset: 0x00038340
		private void HandleReloadPacket(ReloadConfirmPacket packet)
		{
			Action<bool> callback;
			if (this._callbacks.TryGetValue(packet.ReloadId, ref callback))
			{
				callback.Invoke(packet.Success);
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0003A170 File Offset: 0x00038370
		private void HandleShotPacket(ShotConfirmPacket packet)
		{
			Action<bool> callback;
			if (this._callbacks.TryGetValue(packet.ShotId, ref callback))
			{
				callback.Invoke(packet.Success);
			}
		}

		// Token: 0x04000538 RID: 1336
		public const string NetworkChannelId = "CombatOverhaul:rangeWeapon";

		// Token: 0x0400053A RID: 1338
		private readonly ICoreClientAPI _api;

		// Token: 0x0400053B RID: 1339
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x0400053C RID: 1340
		private readonly Dictionary<int, Action<bool>> _callbacks = new Dictionary<int, Action<bool>>();

		// Token: 0x0400053D RID: 1341
		private int _nextId;

		// Token: 0x0200025D RID: 605
		// (Invoke) Token: 0x060016F8 RID: 5880
		[NullableContext(0)]
		public delegate void RangedWeaponStatusChangedDelegate(Entity attacker, ItemSlot weaponSlot, RangedWeaponStatus status);
	}
}
