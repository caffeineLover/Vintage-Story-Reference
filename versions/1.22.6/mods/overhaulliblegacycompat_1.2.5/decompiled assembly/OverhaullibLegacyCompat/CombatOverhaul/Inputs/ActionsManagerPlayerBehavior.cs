using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CombatOverhaul.Integration.Transpilers;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace CombatOverhaul.Inputs
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ActionsManagerPlayerBehavior : EntityBehavior
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001D0E0 File Offset: 0x0001B2E0
		private ItemSlot MainHandItemSlot
		{
			get
			{
				ItemSlot active = this._player.ActiveHandItemSlot;
				if (((active != null) ? active.Itemstack : null) != null)
				{
					return active;
				}
				ItemSlot right = this._player.RightHandItemSlot;
				if (((right != null) ? right.Itemstack : null) != null)
				{
					return right;
				}
				ItemSlot result;
				if ((result = active) == null)
				{
					result = (right ?? this._player.LeftHandItemSlot);
				}
				return result;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0001D13A File Offset: 0x0001B33A
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x0001D142 File Offset: 0x0001B342
		public bool SuppressLMB { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001D14B File Offset: 0x0001B34B
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x0001D153 File Offset: 0x0001B353
		public bool SuppressRMB { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001D15C File Offset: 0x0001B35C
		public float ManipulationSpeed
		{
			get
			{
				return Math.Clamp(this.entity.Stats.GetBlended("manipulationSpeed"), this.MinManipulationSpeed, this.MaxManipulationSpeed);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001D184 File Offset: 0x0001B384
		public ActionListener ActionListener { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001D18C File Offset: 0x0001B38C
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0001D194 File Offset: 0x0001B394
		public float MaxManipulationSpeed { get; set; } = 2f;

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001D19D File Offset: 0x0001B39D
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0001D1A5 File Offset: 0x0001B3A5
		public float MinManipulationSpeed { get; set; } = 0.5f;

		// Token: 0x06000518 RID: 1304 RVA: 0x0001D1B0 File Offset: 0x0001B3B0
		public ActionsManagerPlayerBehavior(Entity entity) : base(entity)
		{
			EntityPlayer player = entity as EntityPlayer;
			if (player == null)
			{
				throw new ArgumentException("This behavior should only be applied to player");
			}
			ICoreClientAPI clientApi = entity.Api as ICoreClientAPI;
			if (clientApi == null)
			{
				throw new ArgumentException("This behavior is client side only");
			}
			try
			{
				this._configuredPlayerUid = clientApi.Settings.String["playeruid"];
			}
			catch
			{
				this._configuredPlayerUid = null;
			}
			this._player = player;
			this._api = clientApi;
			CombatOverhaulSystem system = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			ActionListener actionListener = system.ActionListener;
			if (actionListener == null)
			{
				throw new Exception();
			}
			this.ActionListener = actionListener;
			DirectionController directionController = system.DirectionController;
			if (directionController == null)
			{
				throw new Exception();
			}
			this._directionController = directionController;
			DirectionCursorRenderer directionCursorRenderer = system.DirectionCursorRenderer;
			if (directionCursorRenderer == null)
			{
				throw new Exception();
			}
			this._directionRenderer = directionCursorRenderer;
			StatsSystemClient clientStatsSystem = system.ClientStatsSystem;
			if (clientStatsSystem == null)
			{
				throw new Exception();
			}
			this._statsSystem = clientStatsSystem;
			this.TryBecomeMainPlayer();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001D334 File Offset: 0x0001B534
		public override string PropertyName()
		{
			return "CombatOverhaul:melee-weapon-player-behavior";
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001D33C File Offset: 0x0001B53C
		public override void OnGameTick(float deltaTime)
		{
			this.TryBecomeMainPlayer();
			if (!this._mainPlayer)
			{
				return;
			}
			if (!this.entity.Alive)
			{
				foreach (string stat in this._currentMainHandPlayerStats)
				{
					this._player.Stats.Remove("CombatOverhaul:melee-weapon-player-behavior", stat);
				}
				foreach (string stat2 in this._currentOffHandPlayerStats)
				{
					this._player.Stats.Remove("CombatOverhaul:melee-weapon-player-behavior", stat2);
				}
			}
			bool configurationChanged = this.SetRenderDirectionCursorForMainHand();
			this._directionController.OnGameTick(configurationChanged);
			this.CheckIfItemsInHandsChanged();
			ItemSlot mainHandSlot = this.MainHandItemSlot;
			ItemStack itemstack = mainHandSlot.Itemstack;
			IOnGameTick mainhandTickListener = ((itemstack != null) ? itemstack.Item : null) as IOnGameTick;
			if (mainhandTickListener != null)
			{
				mainhandTickListener.OnGameTick(mainHandSlot, this._player, ref this._mainHandState, true);
			}
			IOnGameTick[] behaviorTickListeners = ActionsManagerPlayerBehavior.GetBehaviorTickListeners(itemstack, ref this._mainHandTickListenersItemId, ref this._mainHandBehaviorTickListeners);
			for (int i = 0; i < behaviorTickListeners.Length; i++)
			{
				behaviorTickListeners[i].OnGameTick(mainHandSlot, this._player, ref this._mainHandState, true);
			}
			ItemSlot offHandSlot = this._player.LeftHandItemSlot;
			ItemStack itemstack2 = offHandSlot.Itemstack;
			IOnGameTick offhandTickListener = ((itemstack2 != null) ? itemstack2.Item : null) as IOnGameTick;
			if (offhandTickListener != null)
			{
				offhandTickListener.OnGameTick(offHandSlot, this._player, ref this._offHandState, false);
			}
			behaviorTickListeners = ActionsManagerPlayerBehavior.GetBehaviorTickListeners(itemstack2, ref this._offHandTickListenersItemId, ref this._offHandBehaviorTickListeners);
			for (int i = 0; i < behaviorTickListeners.Length; i++)
			{
				behaviorTickListeners[i].OnGameTick(offHandSlot, this._player, ref this._offHandState, false);
			}
			this.ActionListener.SuppressLMB = this.SuppressLMB;
			this.ActionListener.SuppressRMB = this.SuppressRMB;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001D544 File Offset: 0x0001B744
		private static IOnGameTick[] GetBehaviorTickListeners([Nullable(2)] ItemStack stack, ref int cachedItemId, ref IOnGameTick[] cachedListeners)
		{
			int? num;
			if (stack == null)
			{
				num = default(int?);
			}
			else
			{
				Item item = stack.Item;
				num = ((item != null) ? new int?(item.Id) : default(int?));
			}
			int? num2 = num;
			int itemId = num2.GetValueOrDefault(-1);
			if (itemId == cachedItemId)
			{
				return cachedListeners;
			}
			cachedItemId = itemId;
			CollectibleBehavior[] array;
			if (stack == null)
			{
				array = null;
			}
			else
			{
				Item item2 = stack.Item;
				array = ((item2 != null) ? item2.CollectibleBehaviors : null);
			}
			CollectibleBehavior[] behaviors = array;
			if (behaviors == null || behaviors.Length == 0)
			{
				cachedListeners = Array.Empty<IOnGameTick>();
				return cachedListeners;
			}
			List<IOnGameTick> listeners = null;
			CollectibleBehavior[] array2 = behaviors;
			for (int i = 0; i < array2.Length; i++)
			{
				IOnGameTick listener = array2[i] as IOnGameTick;
				if (listener != null)
				{
					if (listeners == null)
					{
						listeners = new List<IOnGameTick>();
					}
					listeners.Add(listener);
				}
			}
			cachedListeners = (((listeners != null) ? listeners.ToArray() : null) ?? Array.Empty<IOnGameTick>());
			return cachedListeners;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001D60F File Offset: 0x0001B80F
		public int GetState(bool mainHand = true)
		{
			if (!mainHand)
			{
				return this._offHandState;
			}
			return this._mainHandState;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001D621 File Offset: 0x0001B821
		public void SetState(int state, bool mainHand = true)
		{
			if (mainHand)
			{
				this._mainHandState = state;
				return;
			}
			this._offHandState = state;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001D635 File Offset: 0x0001B835
		public void SetStat(string stat, string category, float value = 0f)
		{
			if (!this._mainPlayer)
			{
				return;
			}
			this._statsSystem.SetStat(stat, category, value);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001D64E File Offset: 0x0001B84E
		public void FlipDirectionToOpposite()
		{
			this._directionController.FlipDirectionToOpposite();
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001D65B File Offset: 0x0001B85B
		public void SetItemChanged(bool mainHand)
		{
			if (mainHand)
			{
				this._currentMainHandItemId = -1;
				return;
			}
			this._currentOffHandItemId = -1;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001D670 File Offset: 0x0001B870
		private void RegisterWeapons()
		{
			if (this._weaponsRegistered)
			{
				return;
			}
			IEnumerable<CollectibleBehavior> enumerable = Enumerable.SelectMany<Item, CollectibleBehavior>(this._api.World.Items, (Item item) => item.CollectibleBehaviors ?? Array.Empty<CollectibleBehavior>());
			EnumerableExtensions.Foreach<IClientWeaponLogic>(Enumerable.SelectMany<IHasMultipleWeaponLogicModes, IClientWeaponLogic>(Enumerable.OfType<IHasMultipleWeaponLogicModes>(this._api.World.Items), (IHasMultipleWeaponLogicModes element) => element.ClientLogicModes), new Action<IClientWeaponLogic>(this.RegisterWeapon));
			EnumerableExtensions.Foreach<IClientWeaponLogic>(Enumerable.SelectMany<IHasMultipleWeaponLogicModes, IClientWeaponLogic>(Enumerable.OfType<IHasMultipleWeaponLogicModes>(enumerable), (IHasMultipleWeaponLogicModes element) => element.ClientLogicModes), new Action<IClientWeaponLogic>(this.RegisterWeapon));
			EnumerableExtensions.Foreach<IClientWeaponLogic>(Enumerable.Where<IClientWeaponLogic>(Enumerable.OfType<IClientWeaponLogic>(enumerable), (IClientWeaponLogic item) => !(item is IHasMultipleWeaponLogicModes)), new Action<IClientWeaponLogic>(this.RegisterWeapon));
			EnumerableExtensions.Foreach<IClientWeaponLogic>(Enumerable.Select<IHasWeaponLogic, IClientWeaponLogic>(Enumerable.Where<IHasWeaponLogic>(Enumerable.OfType<IHasWeaponLogic>(this._api.World.Items), (IHasWeaponLogic item) => !(item is IHasMultipleWeaponLogicModes)), (IHasWeaponLogic element) => element.ClientLogic), new Action<IClientWeaponLogic>(this.RegisterWeapon));
			this._weaponsRegistered = true;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001D7F0 File Offset: 0x0001B9F0
		private void RegisterHotkeys()
		{
			string text;
			foreach (KeyValuePair<string, HotkeyEventHandlerAttribute> keyValuePair in this._hotkeys)
			{
				HotkeyEventHandlerAttribute hotkeyEventHandlerAttribute;
				keyValuePair.Deconstruct(ref text, ref hotkeyEventHandlerAttribute);
				string code = text;
				HotkeyEventHandlerAttribute data = hotkeyEventHandlerAttribute;
				this._api.Input.RegisterHotKey(code, Lang.Get(data.HotkeyName, Array.Empty<object>()), data.Key, data.HotkeyType, data.Alt, data.Ctrl, data.Shift);
			}
			foreach (KeyValuePair<string, Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>>> keyValuePair2 in this._hotkeysCallbacks)
			{
				Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> delegatesPerItem2;
				keyValuePair2.Deconstruct(ref text, ref delegatesPerItem2);
				string code2 = text;
				Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> delegatesPerItem = delegatesPerItem2;
				this._api.Input.SetHotKeyHandler(code2, (KeyCombination keyCombination) => this.HandleHotkeyEvent(delegatesPerItem, keyCombination));
			}
			this._hotkeys.Clear();
			this._hotkeysCallbacks.Clear();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001D928 File Offset: 0x0001BB28
		[NullableContext(2)]
		private void RegisterWeapon(IClientWeaponLogic weapon)
		{
			if (weapon == null)
			{
				return;
			}
			Dictionary<ActionEventId, List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate>> dictionary = ActionsManagerPlayerBehavior.CollectHandlers(weapon);
			int itemId = weapon.ItemId;
			foreach (KeyValuePair<ActionEventId, List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate>> keyValuePair in dictionary)
			{
				ActionEventId eventId2;
				List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate> list2;
				keyValuePair.Deconstruct(ref eventId2, ref list2);
				ActionEventId eventId = eventId2;
				list2.ForEach(delegate(ActionsManagerPlayerBehavior.ActionEventCallbackDelegate callback)
				{
					this.ActionListener.Subscribe(eventId, (ActionEventData eventData) => this.HandleActionEvent(eventData, itemId, callback));
				});
			}
			foreach (KeyValuePair<string, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> keyValuePair2 in this.CollectHotkeysHandlers(weapon))
			{
				string text;
				List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> list3;
				keyValuePair2.Deconstruct(ref text, ref list3);
				string code = text;
				List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> list = list3;
				Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> itemCallbacks;
				if (this._hotkeysCallbacks.TryGetValue(code, ref itemCallbacks))
				{
					List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> existingList;
					if (itemCallbacks.TryGetValue(itemId, ref existingList))
					{
						existingList.AddRange(list);
					}
					else
					{
						itemCallbacks[itemId] = list;
					}
				}
				else
				{
					Dictionary<string, Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>>> hotkeysCallbacks = this._hotkeysCallbacks;
					string text2 = code;
					Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> dictionary2 = new Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>>();
					dictionary2.Add(itemId, list);
					hotkeysCallbacks[text2] = dictionary2;
				}
			}
			weapon.OnRegistered(this, this._api);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001DA84 File Offset: 0x0001BC84
		private static Dictionary<ActionEventId, List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate>> CollectHandlers(object owner)
		{
			IEnumerable<MethodInfo> enumerable = Enumerable.Where<MethodInfo>(owner.GetType().GetMethods(52), (MethodInfo method) => Enumerable.Any<object>(method.GetCustomAttributes(typeof(ActionEventHandlerAttribute), true)));
			Dictionary<ActionEventId, List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate>> handlers = new Dictionary<ActionEventId, List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate>>();
			foreach (MethodInfo method2 in enumerable)
			{
				ActionEventHandlerAttribute attribute = method2.GetCustomAttributes(typeof(ActionEventHandlerAttribute), true)[0] as ActionEventHandlerAttribute;
				if (attribute != null)
				{
					ActionsManagerPlayerBehavior.ActionEventCallbackDelegate handler = Delegate.CreateDelegate(typeof(ActionsManagerPlayerBehavior.ActionEventCallbackDelegate), owner, method2) as ActionsManagerPlayerBehavior.ActionEventCallbackDelegate;
					if (handler == null)
					{
						throw new InvalidOperationException("Handler should have same signature as 'ActionEventCallbackDelegate' delegate.");
					}
					List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate> callbackDelegates;
					if (handlers.TryGetValue(attribute.Event, ref callbackDelegates))
					{
						callbackDelegates.Add(handler);
					}
					else
					{
						List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate> list = new List<ActionsManagerPlayerBehavior.ActionEventCallbackDelegate>();
						list.Add(handler);
						callbackDelegates = list;
						handlers.Add(attribute.Event, callbackDelegates);
					}
				}
			}
			return handlers;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001DB80 File Offset: 0x0001BD80
		private unsafe Dictionary<string, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> CollectHotkeysHandlers(object owner)
		{
			IEnumerable<MethodInfo> enumerable = Enumerable.Where<MethodInfo>(owner.GetType().GetMethods(52), (MethodInfo method) => Enumerable.Any<object>(method.GetCustomAttributes(typeof(HotkeyEventHandlerAttribute), true)));
			Dictionary<string, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> handlers = new Dictionary<string, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>>();
			foreach (MethodInfo method2 in enumerable)
			{
				HotkeyEventHandlerAttribute attribute = method2.GetCustomAttributes(typeof(HotkeyEventHandlerAttribute), true)[0] as HotkeyEventHandlerAttribute;
				if (attribute != null)
				{
					ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate handler = Delegate.CreateDelegate(typeof(ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate), owner, method2) as ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate;
					if (handler == null)
					{
						throw new InvalidOperationException("Hotkey handler should have same signature as 'HotkeyEventCallbackDelegate' delegate.");
					}
					List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> callbackDelegates;
					if (handlers.TryGetValue(attribute.HotkeyCode, ref callbackDelegates))
					{
						callbackDelegates.Add(handler);
					}
					else
					{
						int num = 1;
						List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> list = new List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>(num);
						CollectionsMarshal.SetCount<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>(list, num);
						*CollectionsMarshal.AsSpan<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>(list)[0] = handler;
						callbackDelegates = list;
						handlers.Add(attribute.HotkeyCode, callbackDelegates);
					}
					if (!this._hotkeys.ContainsKey(attribute.HotkeyCode))
					{
						this._hotkeys[attribute.HotkeyCode] = attribute;
					}
				}
			}
			return handlers;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001DCBC File Offset: 0x0001BEBC
		private bool HandleActionEvent(ActionEventData eventData, int itemId, ActionsManagerPlayerBehavior.ActionEventCallbackDelegate callback)
		{
			ItemStack itemstack = this.MainHandItemSlot.Itemstack;
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
			int valueOrDefault = num2.GetValueOrDefault(-1);
			ItemStack itemstack2 = this._player.LeftHandItemSlot.Itemstack;
			int? num3;
			if (itemstack2 == null)
			{
				num3 = default(int?);
			}
			else
			{
				Item item2 = itemstack2.Item;
				num3 = ((item2 != null) ? new int?(item2.Id) : default(int?));
			}
			num2 = num3;
			int offHandId = num2.GetValueOrDefault(-1);
			bool mainHandHandled = false;
			bool offHandHandled = false;
			if (valueOrDefault == itemId)
			{
				try
				{
					mainHandHandled = callback(this.MainHandItemSlot, this._player, ref this._mainHandState, eventData, true, this._directionController.CurrentDirection);
				}
				catch (Exception exception)
				{
					ICoreAPI api = this._api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(44, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Error while handling action '");
					defaultInterpolatedStringHandler.AppendFormatted<ActionEventId>(eventData.Action);
					defaultInterpolatedStringHandler.AppendLiteral("' for item '");
					ItemStack itemstack3 = this.MainHandItemSlot.Itemstack;
					AssetLocation assetLocation;
					if (itemstack3 == null)
					{
						assetLocation = null;
					}
					else
					{
						CollectibleObject collectible = itemstack3.Collectible;
						assetLocation = ((collectible != null) ? collectible.Code : null);
					}
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(assetLocation);
					defaultInterpolatedStringHandler.AppendLiteral("':\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (offHandId == itemId)
			{
				try
				{
					offHandHandled = callback(this._player.LeftHandItemSlot, this._player, ref this._offHandState, eventData, false, this._directionController.CurrentDirection);
				}
				catch (Exception exception2)
				{
					ICoreAPI api2 = this._api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(44, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("Error while handling action '");
					defaultInterpolatedStringHandler2.AppendFormatted<ActionEventId>(eventData.Action);
					defaultInterpolatedStringHandler2.AppendLiteral("' for item '");
					ItemStack itemstack4 = this._player.LeftHandItemSlot.Itemstack;
					AssetLocation assetLocation2;
					if (itemstack4 == null)
					{
						assetLocation2 = null;
					}
					else
					{
						CollectibleObject collectible2 = itemstack4.Collectible;
						assetLocation2 = ((collectible2 != null) ? collectible2.Code : null);
					}
					defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(assetLocation2);
					defaultInterpolatedStringHandler2.AppendLiteral("':\n");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception2);
					LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			return mainHandHandled || offHandHandled;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001DEEC File Offset: 0x0001C0EC
		private bool HandleHotkeyEvent(Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>> delegates, KeyCombination keyCombination)
		{
			ItemStack itemstack = this.MainHandItemSlot.Itemstack;
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
			int mainHandId = num2.GetValueOrDefault(-1);
			ItemStack itemstack2 = this._player.LeftHandItemSlot.Itemstack;
			int? num3;
			if (itemstack2 == null)
			{
				num3 = default(int?);
			}
			else
			{
				Item item2 = itemstack2.Item;
				num3 = ((item2 != null) ? new int?(item2.Id) : default(int?));
			}
			num2 = num3;
			int offHandId = num2.GetValueOrDefault(-1);
			bool mainHandHandled = false;
			bool offHandHandled = false;
			List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> delegatesForItemInMainHand;
			if (delegates.TryGetValue(mainHandId, ref delegatesForItemInMainHand))
			{
				foreach (ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate hotkeyDelegate in delegatesForItemInMainHand)
				{
					try
					{
						if (hotkeyDelegate(this.MainHandItemSlot, this._player, ref this._mainHandState, keyCombination, true, this._directionController.CurrentDirection))
						{
							mainHandHandled = true;
							break;
						}
					}
					catch (Exception exception)
					{
						ICoreAPI api = this._api;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Error while handling hotkey '");
						defaultInterpolatedStringHandler.AppendFormatted<KeyCombination>(keyCombination);
						defaultInterpolatedStringHandler.AppendLiteral("' for item '");
						ItemStack itemstack3 = this.MainHandItemSlot.Itemstack;
						AssetLocation assetLocation;
						if (itemstack3 == null)
						{
							assetLocation = null;
						}
						else
						{
							CollectibleObject collectible = itemstack3.Collectible;
							assetLocation = ((collectible != null) ? collectible.Code : null);
						}
						defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(assetLocation);
						defaultInterpolatedStringHandler.AppendLiteral("':\n");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
						LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate> delegatesForItemInOffhand;
			if (delegates.TryGetValue(offHandId, ref delegatesForItemInOffhand))
			{
				foreach (ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate hotkeyDelegate2 in delegatesForItemInOffhand)
				{
					try
					{
						if (hotkeyDelegate2(this._player.LeftHandItemSlot, this._player, ref this._mainHandState, keyCombination, false, this._directionController.CurrentDirection))
						{
							offHandHandled = true;
							break;
						}
					}
					catch (Exception exception2)
					{
						ICoreAPI api2 = this._api;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(44, 3);
						defaultInterpolatedStringHandler2.AppendLiteral("Error while handling hotkey '");
						defaultInterpolatedStringHandler2.AppendFormatted<KeyCombination>(keyCombination);
						defaultInterpolatedStringHandler2.AppendLiteral("' for item '");
						ItemStack itemstack4 = this._player.LeftHandItemSlot.Itemstack;
						AssetLocation assetLocation2;
						if (itemstack4 == null)
						{
							assetLocation2 = null;
						}
						else
						{
							CollectibleObject collectible2 = itemstack4.Collectible;
							assetLocation2 = ((collectible2 != null) ? collectible2.Code : null);
						}
						defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(assetLocation2);
						defaultInterpolatedStringHandler2.AppendLiteral("':\n");
						defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception2);
						LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
			}
			return mainHandHandled || offHandHandled;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		private bool CheckIfItemsInHandsChanged()
		{
			ItemStack itemstack = this.MainHandItemSlot.Itemstack;
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
			int mainHandId = num2.GetValueOrDefault(-1);
			int mainHandSlotId = this.MainHandItemSlot.Inventory.GetSlotId(this.MainHandItemSlot);
			ItemStack itemstack2 = this._player.LeftHandItemSlot.Itemstack;
			int? num3;
			if (itemstack2 == null)
			{
				num3 = default(int?);
			}
			else
			{
				Item item2 = itemstack2.Item;
				num3 = ((item2 != null) ? new int?(item2.Id) : default(int?));
			}
			num2 = num3;
			int offHandId = num2.GetValueOrDefault(-1);
			long mainHandStackId = GeneralUtils.GetItemMark(this.MainHandItemSlot);
			long offHandStackId = GeneralUtils.GetItemMark(this._player.LeftHandItemSlot);
			bool flag = mainHandId != this._currentMainHandItemId || offHandId != this._currentOffHandItemId || mainHandSlotId != this._currentMainHandSlotId || this._currentMainHandStackItemId != mainHandStackId || this._currentOffHandStackItemId != offHandStackId;
			if (flag)
			{
				this.SuppressLMB = false;
				this.SuppressRMB = false;
			}
			if (flag && (this._currentMainHandItemId != mainHandId || mainHandSlotId != this._currentMainHandSlotId || mainHandStackId != this._currentMainHandStackItemId))
			{
				this.ProcessMainHandItemChanged();
				this._currentMainHandItemId = mainHandId;
				this._currentMainHandSlotId = mainHandSlotId;
				this._currentMainHandStackItemId = mainHandStackId;
			}
			if (flag && (this._currentOffHandItemId != offHandId || offHandStackId != this._currentOffHandStackItemId))
			{
				this.ProcessOffHandItemChanged();
				this._currentOffHandItemId = offHandId;
				this._currentOffHandStackItemId = offHandStackId;
			}
			return !flag;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001E338 File Offset: 0x0001C538
		private void ProcessMainHandItemChanged()
		{
			IClientWeaponLogic currentMainHandWeapon = this._currentMainHandWeapon;
			if (currentMainHandWeapon != null)
			{
				currentMainHandWeapon.OnDeselected(this._player, true, ref this._mainHandState);
			}
			this._currentMainHandWeapon = null;
			foreach (string stat in this._currentMainHandPlayerStats)
			{
				this._player.Stats.Remove("CombatOverhaul:melee-weapon-player-behavior", stat);
			}
			ItemStack stack = this.MainHandItemSlot.Itemstack;
			ISetsRenderingOffset setsRenderingOffset;
			if (stack == null)
			{
				setsRenderingOffset = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				setsRenderingOffset = ((collectible != null) ? collectible.GetCollectibleInterface<ISetsRenderingOffset>() : null);
			}
			ISetsRenderingOffset offset = setsRenderingOffset;
			if (offset != null)
			{
				this._mainHandRenderingOffset = offset.RenderingOffset;
			}
			else
			{
				this._mainHandRenderingOffset = true;
			}
			if (this._mainHandRenderingOffset && this._offHandRenderingOffset)
			{
				PlayerRenderingPatches.ResetOffset();
			}
			else
			{
				PlayerRenderingPatches.SetOffset(0f);
			}
			if (stack != null)
			{
				IHasWeaponLogic weapon = stack.Item as IHasWeaponLogic;
				if (weapon != null)
				{
					this._mainHandState = 0;
					IClientWeaponLogic clientLogic = weapon.ClientLogic;
					if (clientLogic != null)
					{
						clientLogic.OnSelected(this.MainHandItemSlot, this._player, true, ref this._mainHandState);
					}
					this._currentMainHandWeapon = weapon.ClientLogic;
					JsonObject attributes = stack.Item.Attributes;
					if (attributes != null && attributes["fpHandsOffset"].Exists)
					{
						PlayerRenderingPatches.SetOffset(stack.Item.Attributes["fpHandsOffset"].AsFloat(0f));
					}
					return;
				}
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001E4B4 File Offset: 0x0001C6B4
		private void ProcessOffHandItemChanged()
		{
			IClientWeaponLogic currentOffHandWeapon = this._currentOffHandWeapon;
			if (currentOffHandWeapon != null)
			{
				currentOffHandWeapon.OnDeselected(this._player, false, ref this._offHandState);
			}
			this._currentOffHandWeapon = null;
			foreach (string stat in this._currentOffHandPlayerStats)
			{
				this._player.Stats.Remove("CombatOverhaul:melee-weapon-player-behavior", stat);
			}
			ItemStack stack = this._player.LeftHandItemSlot.Itemstack;
			ISetsRenderingOffset setsRenderingOffset;
			if (stack == null)
			{
				setsRenderingOffset = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				setsRenderingOffset = ((collectible != null) ? collectible.GetCollectibleInterface<ISetsRenderingOffset>() : null);
			}
			ISetsRenderingOffset offset = setsRenderingOffset;
			if (offset != null)
			{
				this._offHandRenderingOffset = offset.RenderingOffset;
			}
			else
			{
				this._offHandRenderingOffset = true;
			}
			if (this._mainHandRenderingOffset && this._offHandRenderingOffset)
			{
				PlayerRenderingPatches.ResetOffset();
			}
			else
			{
				PlayerRenderingPatches.SetOffset(0f);
			}
			if (stack != null)
			{
				IHasWeaponLogic weapon = stack.Item as IHasWeaponLogic;
				if (weapon != null)
				{
					this._offHandState = 0;
					IClientWeaponLogic clientLogic = weapon.ClientLogic;
					if (clientLogic != null)
					{
						clientLogic.OnSelected(this._player.LeftHandItemSlot, this._player, false, ref this._offHandState);
					}
					this._currentOffHandWeapon = weapon.ClientLogic;
					JsonObject attributes = stack.Item.Attributes;
					if (attributes != null && attributes["fpHandsOffset"].Exists)
					{
						PlayerRenderingPatches.SetOffset(stack.Item.Attributes["fpHandsOffset"].AsFloat(0f));
					}
					return;
				}
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001E63C File Offset: 0x0001C83C
		private bool SetRenderDirectionCursorForMainHand()
		{
			ItemStack stack = this.MainHandItemSlot.Itemstack;
			DirectionsConfiguration configuration = this._directionController.DirectionsConfiguration;
			if (stack != null)
			{
				IHasWeaponLogic weapon = stack.Item as IHasWeaponLogic;
				if (weapon != null)
				{
					if (weapon.ClientLogic != null)
					{
						DirectionsConfiguration directionsType = weapon.ClientLogic.DirectionsType;
						this._directionController.DirectionsConfiguration = directionsType;
						this._directionRenderer.Show = (directionsType != DirectionsConfiguration.None);
					}
					return configuration != this._directionController.DirectionsConfiguration;
				}
			}
			this._directionController.DirectionsConfiguration = DirectionsConfiguration.None;
			this._directionRenderer.Show = false;
			return configuration != this._directionController.DirectionsConfiguration;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001E6E0 File Offset: 0x0001C8E0
		private void TryBecomeMainPlayer()
		{
			if (this._mainPlayer)
			{
				return;
			}
			bool flag = this._player.PlayerUID != null && this._configuredPlayerUid != null && this._player.PlayerUID == this._configuredPlayerUid;
			IClientPlayer player = this._api.World.Player;
			long? num;
			if (player == null)
			{
				num = default(long?);
			}
			else
			{
				EntityPlayer entity = player.Entity;
				num = ((entity != null) ? new long?(entity.EntityId) : default(long?));
			}
			long? num2 = num;
			long entityId = this.entity.EntityId;
			bool isWorldMainPlayer = num2.GetValueOrDefault() == entityId & num2 != null;
			if (!flag && !isWorldMainPlayer)
			{
				return;
			}
			this._mainPlayer = true;
			this.RegisterWeapons();
			this.RegisterHotkeys();
		}

		// Token: 0x040001F2 RID: 498
		private bool _mainPlayer;

		// Token: 0x040001F3 RID: 499
		private readonly ICoreClientAPI _api;

		// Token: 0x040001F4 RID: 500
		private readonly EntityPlayer _player;

		// Token: 0x040001F5 RID: 501
		[Nullable(2)]
		private readonly string _configuredPlayerUid;

		// Token: 0x040001F6 RID: 502
		private bool _weaponsRegistered;

		// Token: 0x040001F7 RID: 503
		private readonly HashSet<string> _currentMainHandPlayerStats = new HashSet<string>();

		// Token: 0x040001F8 RID: 504
		private readonly HashSet<string> _currentOffHandPlayerStats = new HashSet<string>();

		// Token: 0x040001F9 RID: 505
		private const string _statCategory = "CombatOverhaul:melee-weapon-player-behavior";

		// Token: 0x040001FA RID: 506
		private readonly DirectionController _directionController;

		// Token: 0x040001FB RID: 507
		private readonly DirectionCursorRenderer _directionRenderer;

		// Token: 0x040001FC RID: 508
		private readonly StatsSystemClient _statsSystem;

		// Token: 0x040001FD RID: 509
		private readonly Dictionary<string, HotkeyEventHandlerAttribute> _hotkeys = new Dictionary<string, HotkeyEventHandlerAttribute>();

		// Token: 0x040001FE RID: 510
		private readonly Dictionary<string, Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>>> _hotkeysCallbacks = new Dictionary<string, Dictionary<int, List<ActionsManagerPlayerBehavior.HotkeyEventCallbackDelegate>>>();

		// Token: 0x040001FF RID: 511
		[Nullable(2)]
		private IClientWeaponLogic _currentMainHandWeapon;

		// Token: 0x04000200 RID: 512
		[Nullable(2)]
		private IClientWeaponLogic _currentOffHandWeapon;

		// Token: 0x04000201 RID: 513
		private int _currentMainHandItemId = -1;

		// Token: 0x04000202 RID: 514
		private int _currentOffHandItemId = -1;

		// Token: 0x04000203 RID: 515
		private long _currentMainHandStackItemId;

		// Token: 0x04000204 RID: 516
		private long _currentOffHandStackItemId;

		// Token: 0x04000205 RID: 517
		private int _currentMainHandSlotId = -1;

		// Token: 0x04000206 RID: 518
		private int _mainHandState;

		// Token: 0x04000207 RID: 519
		private int _offHandState;

		// Token: 0x04000208 RID: 520
		private int _mainHandTickListenersItemId = -1;

		// Token: 0x04000209 RID: 521
		private int _offHandTickListenersItemId = -1;

		// Token: 0x0400020A RID: 522
		private IOnGameTick[] _mainHandBehaviorTickListeners = Array.Empty<IOnGameTick>();

		// Token: 0x0400020B RID: 523
		private IOnGameTick[] _offHandBehaviorTickListeners = Array.Empty<IOnGameTick>();

		// Token: 0x0400020C RID: 524
		private bool _mainHandRenderingOffset = true;

		// Token: 0x0400020D RID: 525
		private bool _offHandRenderingOffset = true;

		// Token: 0x020001F5 RID: 501
		// (Invoke) Token: 0x06001578 RID: 5496
		[NullableContext(0)]
		public delegate bool ActionEventCallbackDelegate(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction);

		// Token: 0x020001F6 RID: 502
		// (Invoke) Token: 0x0600157C RID: 5500
		[NullableContext(0)]
		public delegate bool HotkeyEventCallbackDelegate(ItemSlot slot, EntityPlayer player, ref int state, KeyCombination keyCombination, bool mainHand, AttackDirection direction);
	}
}
