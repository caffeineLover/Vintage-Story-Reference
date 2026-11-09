using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace CombatOverhaul
{
	// Token: 0x02000011 RID: 17
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class FueledItemSystem : ModSystem, IRenderer, IDisposable
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003C4D File Offset: 0x00001E4D
		public double RenderOrder
		{
			get
			{
				return 0.01;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003C58 File Offset: 0x00001E58
		public int RenderRange
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003C5C File Offset: 0x00001E5C
		[NullableContext(1)]
		public override void Start(ICoreAPI api)
		{
			CombatOverhaulSystem system = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			this._settings = system.Settings;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003C82 File Offset: 0x00001E82
		[NullableContext(1)]
		public override void StartClientSide(ICoreClientAPI api)
		{
			this._clientApi = api;
			api.Event.RegisterRenderer(this, 0, "nightvisionCO");
			api.Event.LevelFinalize += new Action(this.OnLevelFinalize);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003CB4 File Offset: 0x00001EB4
		[NullableContext(1)]
		public override void StartServerSide(ICoreServerAPI api)
		{
			base.StartServerSide(api);
			this._serverApi = api;
			this._serverTickListener = api.Event.RegisterGameTickListener(new Action<float>(this.OnServerTick), 1000, 200);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003CEC File Offset: 0x00001EEC
		public override void Dispose()
		{
			ICoreClientAPI clientApi = this._clientApi;
			if (clientApi != null)
			{
				ClientThreadCleanup.Run(clientApi, delegate
				{
					clientApi.Event.UnregisterRenderer(this, 0);
					clientApi.Event.LevelFinalize -= new Action(this.OnLevelFinalize);
					clientApi.Render.ShaderUniforms.NightVisionStrength = 0f;
				}, "fueled-item-system-client-dispose");
			}
			if (this._serverApi != null && this._serverTickListener != 0L)
			{
				this._serverApi.Event.UnregisterGameTickListener(this._serverTickListener);
				this._serverTickListener = 0L;
			}
			this.UnsubscribeClientInventory();
			this._nightVisionSlot = null;
			this._clientApi = null;
			this._serverApi = null;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003D80 File Offset: 0x00001F80
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (this._clientApi == null)
			{
				return;
			}
			IInventory inventory = this.GetClientCharacterInventory();
			if (inventory == null)
			{
				this.SetNightVisionStrength(0f);
				return;
			}
			if (inventory != this._subscribedClientInventory)
			{
				this.SetClientInventory(inventory);
			}
			long now = this._clientApi.World.ElapsedMilliseconds;
			if (this._nightVisionSlotDirty || now >= this._nextNightVisionRefreshMs)
			{
				this.RefreshNightVisionState();
				this._nextNightVisionRefreshMs = now + 250L;
			}
			this.SetNightVisionStrength(this._cachedNightVisionStrength);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003E00 File Offset: 0x00002000
		private void OnServerTick(float dt)
		{
			if (this._serverApi == null)
			{
				return;
			}
			double totalHours = this._serverApi.World.Calendar.TotalHours;
			double hoursPassed = totalHours - this._lastCheckTotalHours;
			if (hoursPassed < (double)this._settings.FueledItemUpdateInGameHours)
			{
				return;
			}
			foreach (IPlayer player in this._serverApi.World.AllOnlinePlayers)
			{
				if (player.Entity != null)
				{
					IInventory inventory = player.InventoryManager.GetOwnInventory("character");
					if (inventory != null)
					{
						int nightVisionSlotIndex = NightVisionDeviceUtil.GetBestNightVisionSlotIndex(inventory);
						if (nightVisionSlotIndex != 12)
						{
							ItemSlot slot = (nightVisionSlotIndex >= 0) ? inventory[nightVisionSlotIndex] : null;
							object obj;
							if (slot == null)
							{
								obj = null;
							}
							else
							{
								ItemStack itemstack = slot.Itemstack;
								obj = ((itemstack != null) ? itemstack.Collectible : null);
							}
							ItemNightvisiondevice device = obj as ItemNightvisiondevice;
							if (device != null)
							{
								device.AddFuelHours(slot.Itemstack, -hoursPassed);
								slot.MarkDirty();
							}
						}
					}
				}
			}
			foreach (IPlayer player2 in this._serverApi.World.AllOnlinePlayers)
			{
				if (player2.Entity != null)
				{
					IInventory inventory2 = player2.InventoryManager.GetOwnInventory("character");
					if (inventory2 != null)
					{
						foreach (ItemSlot slot2 in inventory2)
						{
							IFueledItem fueledItem;
							if (slot2 == null)
							{
								fueledItem = null;
							}
							else
							{
								ItemStack itemstack2 = slot2.Itemstack;
								if (itemstack2 == null)
								{
									fueledItem = null;
								}
								else
								{
									CollectibleObject collectible = itemstack2.Collectible;
									fueledItem = ((collectible != null) ? collectible.GetCollectibleInterface<IFueledItem>() : null);
								}
							}
							IFueledItem item = fueledItem;
							if (slot2 != null && item != null && (!this.IsSleeping(player2.Entity) || item.ConsumeFuelWhenSleeping(player2, slot2)))
							{
								item.AddFuelHours(player2, slot2, -hoursPassed);
								slot2.MarkDirty();
							}
						}
					}
				}
			}
			this._lastCheckTotalHours = totalHours;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003FE0 File Offset: 0x000021E0
		[NullableContext(1)]
		private bool IsSleeping(EntityPlayer player)
		{
			bool result;
			try
			{
				EntityBehaviorTiredness behavior = player.GetBehavior<EntityBehaviorTiredness>();
				result = (behavior != null && behavior.IsSleeping);
			}
			catch (Exception exception)
			{
				if (!this._reportedSleepingStateError)
				{
					this._reportedSleepingStateError = true;
					ICoreAPI serverApi = this._serverApi;
					ICoreAPI api = serverApi ?? player.Api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(41, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Error while checking sleep state for '");
					IPlayer player2 = player.Player;
					defaultInterpolatedStringHandler.AppendFormatted((player2 != null) ? player2.PlayerName : null);
					defaultInterpolatedStringHandler.AppendLiteral("':\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000408C File Offset: 0x0000228C
		private void OnLevelFinalize()
		{
			try
			{
				this.SetClientInventory(this.GetClientCharacterInventory());
			}
			catch (Exception exception)
			{
				if (!this._reportedInventoryLookupError)
				{
					this._reportedInventoryLookupError = true;
					ICoreAPI clientApi = this._clientApi;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(75, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error while resolving player inventory behavior for fueled item rendering:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Warn(clientApi, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000040FC File Offset: 0x000022FC
		private IInventory GetClientCharacterInventory()
		{
			ICoreClientAPI clientApi = this._clientApi;
			if (clientApi == null)
			{
				return null;
			}
			IClientWorldAccessor world = clientApi.World;
			if (world == null)
			{
				return null;
			}
			IClientPlayer player = world.Player;
			if (player == null)
			{
				return null;
			}
			IPlayerInventoryManager inventoryManager = player.InventoryManager;
			if (inventoryManager == null)
			{
				return null;
			}
			return inventoryManager.GetOwnInventory("character");
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004135 File Offset: 0x00002335
		private void SetClientInventory(IInventory inventory)
		{
			this.UnsubscribeClientInventory();
			this._subscribedClientInventory = inventory;
			if (this._subscribedClientInventory != null)
			{
				this._subscribedClientInventory.SlotModified += new Action<int>(this.OnClientInventorySlotModified);
			}
			this._nightVisionSlotDirty = true;
			this._nextNightVisionRefreshMs = 0L;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004172 File Offset: 0x00002372
		private void UnsubscribeClientInventory()
		{
			if (this._subscribedClientInventory != null)
			{
				this._subscribedClientInventory.SlotModified -= new Action<int>(this.OnClientInventorySlotModified);
				this._subscribedClientInventory = null;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000419A File Offset: 0x0000239A
		private void OnClientInventorySlotModified(int slotId)
		{
			this._nightVisionSlotDirty = true;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000041A4 File Offset: 0x000023A4
		private void RefreshNightVisionState()
		{
			this._nightVisionSlotDirty = false;
			this._nightVisionSlot = this.FindNightVisionSlot();
			ItemSlot nightVisionSlot = this._nightVisionSlot;
			object obj;
			if (nightVisionSlot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = nightVisionSlot.Itemstack;
				obj = ((itemstack != null) ? itemstack.Collectible : null);
			}
			ItemNightvisiondevice itemNightvisiondevice = obj as ItemNightvisiondevice;
			double fuelLeft = (itemNightvisiondevice != null) ? itemNightvisiondevice.GetFuelHours(this._nightVisionSlot.Itemstack) : 0.0;
			this._cachedNightVisionStrength = ((fuelLeft > 0.0) ? ((float)GameMath.Clamp(fuelLeft * 20.0, 0.0, 0.8)) : 0f);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004244 File Offset: 0x00002444
		private ItemSlot FindNightVisionSlot()
		{
			IInventory inventory = this._subscribedClientInventory ?? this.GetClientCharacterInventory();
			if (inventory == null)
			{
				return null;
			}
			return NightVisionDeviceUtil.GetBestNightVisionSlot(inventory);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004270 File Offset: 0x00002470
		private void SetNightVisionStrength(float strength)
		{
			if (this._clientApi == null || Math.Abs(this._clientApi.Render.ShaderUniforms.NightVisionStrength - strength) < 0.0001f)
			{
				return;
			}
			this._clientApi.Render.ShaderUniforms.NightVisionStrength = strength;
		}

		// Token: 0x04000034 RID: 52
		private double _lastCheckTotalHours;

		// Token: 0x04000035 RID: 53
		private ICoreClientAPI _clientApi;

		// Token: 0x04000036 RID: 54
		private ICoreServerAPI _serverApi;

		// Token: 0x04000037 RID: 55
		private IInventory _subscribedClientInventory;

		// Token: 0x04000038 RID: 56
		private ItemSlot _nightVisionSlot;

		// Token: 0x04000039 RID: 57
		private long _serverTickListener;

		// Token: 0x0400003A RID: 58
		[Nullable(1)]
		private Settings _settings = new Settings();

		// Token: 0x0400003B RID: 59
		private bool _reportedSleepingStateError;

		// Token: 0x0400003C RID: 60
		private bool _reportedInventoryLookupError;

		// Token: 0x0400003D RID: 61
		private bool _nightVisionSlotDirty = true;

		// Token: 0x0400003E RID: 62
		private long _nextNightVisionRefreshMs;

		// Token: 0x0400003F RID: 63
		private float _cachedNightVisionStrength;

		// Token: 0x04000040 RID: 64
		private const int NightVisionRefreshIntervalMs = 250;
	}
}
