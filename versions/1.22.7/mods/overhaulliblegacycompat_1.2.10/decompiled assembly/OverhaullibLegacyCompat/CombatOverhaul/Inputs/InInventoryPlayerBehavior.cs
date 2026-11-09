using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InInventoryPlayerBehavior : EntityBehavior
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x0001F544 File Offset: 0x0001D744
		public InInventoryPlayerBehavior(Entity entity) : base(entity)
		{
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer == null)
			{
				throw new Exception("This behavior should be attached only to player");
			}
			this._player = entityPlayer;
			bool process;
			if (this._player.Api.Side != 1)
			{
				string playerUID = this._player.PlayerUID;
				ICoreClientAPI coreClientAPI = this._player.Api as ICoreClientAPI;
				process = (playerUID == ((coreClientAPI != null) ? coreClientAPI.Settings.String["playeruid"] : null));
			}
			else
			{
				process = true;
			}
			this._process = process;
			this._timeSinceUpdate = 0f - entity.Api.World.Rand.NextSingle() * 1f;
			this._timeSinceReport = entity.Api.World.Rand.NextSingle() * 600f;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001F612 File Offset: 0x0001D812
		public override string PropertyName()
		{
			return "CombatOverhaul:InInventory";
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001F61C File Offset: 0x0001D81C
		public override void OnGameTick(float deltaTime)
		{
			if (!this._process)
			{
				return;
			}
			this._timeSinceUpdate += deltaTime;
			this._timeSinceReport += deltaTime;
			if (this._timeSinceUpdate < 1f)
			{
				return;
			}
			this._timeSinceUpdate = 0f;
			this.Update();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001F670 File Offset: 0x0001D870
		private void Update()
		{
			EntityPlayer player = this._player;
			bool flag;
			if (player == null)
			{
				flag = (null != null);
			}
			else
			{
				IPlayer player2 = player.Player;
				if (player2 == null)
				{
					flag = (null != null);
				}
				else
				{
					IPlayerInventoryManager inventoryManager = player2.InventoryManager;
					flag = (((inventoryManager != null) ? inventoryManager.Inventories : null) != null);
				}
			}
			if (!flag)
			{
				return;
			}
			foreach (KeyValuePair<string, IInventory> keyValuePair in this._player.Player.InventoryManager.Inventories)
			{
				string text;
				IInventory inventory2;
				keyValuePair.Deconstruct(ref text, ref inventory2);
				IInventory inventory = inventory2;
				if (inventory != null && !(inventory is InventoryPlayerCreative))
				{
					foreach (ItemSlot slot in inventory)
					{
						try
						{
							this.ProcessSlot(slot);
						}
						catch (Exception exception)
						{
							if (this._timeSinceReport > 600f)
							{
								this._timeSinceReport = 0f;
								ICoreAPI api = this._player.Api;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Error for inventory: '");
								defaultInterpolatedStringHandler.AppendFormatted(inventory.ClassName);
								defaultInterpolatedStringHandler.AppendLiteral("', item: '");
								AssetLocation assetLocation;
								if (slot == null)
								{
									assetLocation = null;
								}
								else
								{
									ItemStack itemstack = slot.Itemstack;
									if (itemstack == null)
									{
										assetLocation = null;
									}
									else
									{
										CollectibleObject collectible = itemstack.Collectible;
										assetLocation = ((collectible != null) ? collectible.Code : null);
									}
								}
								defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(assetLocation);
								defaultInterpolatedStringHandler.AppendLiteral("':\n");
								defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
								LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001F840 File Offset: 0x0001DA40
		[NullableContext(2)]
		private void ProcessSlot(ItemSlot slot)
		{
			if (slot == null || slot.Empty)
			{
				return;
			}
			ItemStack itemstack = slot.Itemstack;
			CollectibleObject collectible = (itemstack != null) ? itemstack.Collectible : null;
			if (collectible == null)
			{
				return;
			}
			IOnInInventory inInventory = collectible as IOnInInventory;
			if (inInventory != null)
			{
				inInventory.OnInInventory(this._player, slot);
				return;
			}
			CollectibleBehavior[] collectibleBehaviors = collectible.CollectibleBehaviors;
			for (int i = 0; i < collectibleBehaviors.Length; i++)
			{
				IOnInInventory inInventory2 = collectibleBehaviors[i] as IOnInInventory;
				if (inInventory2 != null)
				{
					inInventory2.OnInInventory(this._player, slot);
				}
			}
		}

		// Token: 0x0400023A RID: 570
		private readonly EntityPlayer _player;

		// Token: 0x0400023B RID: 571
		private const float _updatePeriodSec = 1f;

		// Token: 0x0400023C RID: 572
		private const float _reportPeriodSec = 600f;

		// Token: 0x0400023D RID: 573
		private readonly bool _process;

		// Token: 0x0400023E RID: 574
		private float _timeSinceUpdate;

		// Token: 0x0400023F RID: 575
		private float _timeSinceReport;
	}
}
