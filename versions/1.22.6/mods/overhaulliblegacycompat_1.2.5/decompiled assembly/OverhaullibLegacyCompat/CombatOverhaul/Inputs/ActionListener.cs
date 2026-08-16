using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000063 RID: 99
	[NullableContext(1)]
	[Nullable(0)]
	public class ActionListener : IDisposable
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001C574 File Offset: 0x0001A774
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0001C57C File Offset: 0x0001A77C
		public TimeSpan HoldDuration { get; set; } = TimeSpan.FromSeconds(0.5);

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0001C585 File Offset: 0x0001A785
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x0001C58D File Offset: 0x0001A78D
		public bool SuppressLMB { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0001C596 File Offset: 0x0001A796
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0001C59E File Offset: 0x0001A79E
		public bool SuppressRMB { get; set; }

		// Token: 0x060004DF RID: 1247 RVA: 0x0001C5A8 File Offset: 0x0001A7A8
		public ActionListener(ICoreClientAPI api)
		{
			HashSet<EnumEntityAction> hashSet = new HashSet<EnumEntityAction>();
			hashSet.Add(15);
			hashSet.Add(16);
			this._inconsistentInWorldMouseActions = hashSet;
			this._timers = new Dictionary<EnumEntityAction, long>();
			this._actionStates = new Dictionary<EnumEntityAction, ActionState>();
			Dictionary<EnumEntityAction, EnumEntityAction> dictionary = new Dictionary<EnumEntityAction, EnumEntityAction>();
			dictionary.Add(14, 5);
			dictionary.Add(13, 6);
			this._modifiersRemapping = dictionary;
			HashSet<EnumEntityAction> hashSet2 = new HashSet<EnumEntityAction>();
			hashSet2.Add(14);
			hashSet2.Add(13);
			this._modifiers = hashSet2;
			this._activeActionsSnapshot = Array.Empty<EnumEntityAction>();
			this._activeActionsSnapshotDirty = true;
			base..ctor();
			this._system = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			this._system.OnDispose += new Action(this.Dispose);
			this._clientApi = api;
			api.Input.InWorldAction += new OnEntityAction(this.OnEntityAction);
			this._tickListener = api.World.RegisterGameTickListener(delegate(float dt)
			{
				this.TickListener();
			}, 33, 0);
			api.Event.MouseDown += new MouseEventDelegate(this.HandleMouseDownEvents);
			api.Event.MouseUp += new MouseEventDelegate(this.HandleMouseUpEvents);
			foreach (EnumEntityAction action in ActionListener._allActions)
			{
				this._actionStates[action] = ActionState.Inactive;
				this._timers[action] = 0L;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001C724 File Offset: 0x0001A924
		public bool IsActive(EnumEntityAction action, bool asModifier)
		{
			if (asModifier && this._modifiers.Contains(action) && !this._clientApi.Settings.Bool.Get("separateCtrlKeyForMouse", false))
			{
				return this.IsActive(action) || this.IsActive(this._modifiersRemapping[action]);
			}
			return this.IsActive(action);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001C784 File Offset: 0x0001A984
		public bool IsActive(EnumEntityAction action)
		{
			return this._actionStates[action] != ActionState.Inactive && this._actionStates[action] != ActionState.Released;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001C7A8 File Offset: 0x0001A9A8
		public void Subscribe(ActionEventId action, Func<ActionEventData, bool> callback)
		{
			if (!this._subscriptions.ContainsKey(action))
			{
				this._subscriptions[action] = new List<Func<ActionEventData, bool>>();
			}
			this._subscriptions[action].Add(callback);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001C7DC File Offset: 0x0001A9DC
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this._system.OnDispose -= new Action(this.Dispose);
			this._clientApi.Input.InWorldAction -= new OnEntityAction(this.OnEntityAction);
			this._clientApi.World.UnregisterGameTickListener(this._tickListener);
			this._clientApi.Event.MouseDown -= new MouseEventDelegate(this.HandleMouseDownEvents);
			this._clientApi.Event.MouseUp -= new MouseEventDelegate(this.HandleMouseUpEvents);
			foreach (long timer in this._timers.Values)
			{
				if (timer != 0L)
				{
					this._clientApi.World.UnregisterCallback(timer);
				}
			}
			foreach (EnumEntityAction action in ActionListener._allActions)
			{
				this._timers[action] = 0L;
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001C8FC File Offset: 0x0001AAFC
		public static bool AltPressed(ICoreClientAPI api)
		{
			return (api != null && api.Input.KeyboardKeyState[5]) || (api != null && api.Input.KeyboardKeyState[6]);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001C924 File Offset: 0x0001AB24
		public IEnumerable<EnumEntityAction> GetModifiers()
		{
			return Enumerable.Where<EnumEntityAction>(this._modifiers, new Func<EnumEntityAction, bool>(this.IsActive));
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001C93D File Offset: 0x0001AB3D
		private void HandleMouseDownEvents(MouseEvent mouseEvent)
		{
			this.HandleMouseEvents(mouseEvent, true);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001C947 File Offset: 0x0001AB47
		private void HandleMouseUpEvents(MouseEvent mouseEvent)
		{
			this.HandleMouseEvents(mouseEvent, false);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001C954 File Offset: 0x0001AB54
		private void HandleMouseEvents(MouseEvent mouseEvent, bool on)
		{
			if (!this._clientApi.Input.MouseGrabbed)
			{
				on = false;
				this._suppressLMB = false;
				this._suppressRMB = false;
			}
			EnumMouseButton button = mouseEvent.Button;
			if (button != null)
			{
				if (button != 2)
				{
					return;
				}
				this.UpdateVanillaShieldRmbBridge(on);
				this.OnEntityAction(10, on, mouseEvent);
				if (on && mouseEvent.Handled)
				{
					this._suppressRMB = true;
				}
				if (!on)
				{
					this._suppressRMB = false;
				}
				if (on && (this._suppressRMB || this.SuppressRMB))
				{
					mouseEvent.Handled = true;
				}
			}
			else
			{
				this.OnEntityAction(9, on, mouseEvent);
				if (on && mouseEvent.Handled)
				{
					this._suppressLMB = true;
				}
				if (!on)
				{
					this._suppressLMB = false;
				}
				if (on && (this._suppressLMB || this.SuppressLMB))
				{
					mouseEvent.Handled = true;
					return;
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001CA1C File Offset: 0x0001AC1C
		private void UpdateVanillaShieldRmbBridge(bool on)
		{
			if (!ShieldAutoPatcher.IsCombatOverhaulEnabled(this._clientApi))
			{
				return;
			}
			IClientWorldAccessor world = this._clientApi.World;
			EntityPlayer entityPlayer;
			if (world == null)
			{
				entityPlayer = null;
			}
			else
			{
				IClientPlayer player2 = world.Player;
				entityPlayer = ((player2 != null) ? player2.Entity : null);
			}
			EntityPlayer player = entityPlayer;
			if (player == null)
			{
				return;
			}
			ItemSlot leftHandItemSlot = player.LeftHandItemSlot;
			Item item;
			if (leftHandItemSlot == null)
			{
				item = null;
			}
			else
			{
				ItemStack itemstack = leftHandItemSlot.Itemstack;
				item = ((itemstack != null) ? itemstack.Item : null);
			}
			if (!CollectibleClassifier.IsVanillaItemShield(item))
			{
				return;
			}
			player.Controls.RightMouseDown = on;
			player.ServerControls.RightMouseDown = on;
			player.Controls.CtrlKey = on;
			player.ServerControls.CtrlKey = on;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001CAB8 File Offset: 0x0001ACB8
		private void TickListener()
		{
			if (!this._clientApi.Input.MouseGrabbed)
			{
				return;
			}
			bool rightDown = this.IsActive(10) || this.IsActive(16);
			this.UpdateVanillaShieldRmbBridge(rightDown);
			switch (this._actionStates[9])
			{
			case ActionState.Inactive:
			case ActionState.Active:
			case ActionState.Hold:
				this.CallSubscriptions(9);
				break;
			}
			switch (this._actionStates[10])
			{
			case ActionState.Inactive:
			case ActionState.Active:
			case ActionState.Hold:
				this.CallSubscriptions(10);
				break;
			case ActionState.Pressed:
			case ActionState.Released:
				break;
			default:
				return;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001CB5C File Offset: 0x0001AD5C
		private void OnEntityAction(EnumEntityAction action, bool on, MouseEvent mouseEvent)
		{
			this.SetActionState(action, this.SwitchState(action, on));
			switch (this._actionStates[action])
			{
			case ActionState.Inactive:
			case ActionState.Released:
				this.StopHoldTimer(action);
				break;
			case ActionState.Pressed:
				this.StartHoldTimer(action);
				break;
			}
			if (this.CallSubscriptions(action) && on)
			{
				mouseEvent.Handled = true;
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001CBC4 File Offset: 0x0001ADC4
		private void OnEntityAction(EnumEntityAction action, bool on, ref EnumHandling handled)
		{
			if (action == 15 || action == 16)
			{
				this.OnEntityActionInWorldMouse(action, ref handled);
				return;
			}
			if (action == 9 || action == 10)
			{
				return;
			}
			if (this._inconsistentInWorldMouseActions.Contains(action))
			{
				this.OnEntityActionInconsistent(action, ref handled);
				return;
			}
			this.SetActionState(action, this.SwitchState(action, on));
			switch (this._actionStates[action])
			{
			case ActionState.Inactive:
			case ActionState.Released:
				this.StopHoldTimer(action);
				break;
			case ActionState.Pressed:
				this.StartHoldTimer(action);
				break;
			}
			if (this.CallSubscriptions(action))
			{
				handled = 1;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001CC5C File Offset: 0x0001AE5C
		private void OnEntityActionInWorldMouse(EnumEntityAction action, ref EnumHandling handled)
		{
			EnumEntityAction enumEntityAction;
			if (action != 15)
			{
				if (action != 16)
				{
					enumEntityAction = action;
				}
				else
				{
					enumEntityAction = 10;
				}
			}
			else
			{
				enumEntityAction = 9;
			}
			EnumEntityAction mappedAction = enumEntityAction;
			this.SetActionState(mappedAction, this.SwitchState(mappedAction, true));
			if (this._actionStates[mappedAction] == ActionState.Pressed)
			{
				this.StartHoldTimer(mappedAction);
			}
			if (this.CallSubscriptions(mappedAction))
			{
				handled = 1;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001CCB4 File Offset: 0x0001AEB4
		private void OnEntityActionInconsistent(EnumEntityAction action, ref EnumHandling handled)
		{
			this.SetActionState(action, this.SwitchStateInconsistent(action));
			if (this.CallSubscriptions(action))
			{
				handled = 1;
			}
			this.SetActionState(action, this.SwitchStateInconsistent(action));
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001CCE0 File Offset: 0x0001AEE0
		private void OnHoldTimer(EnumEntityAction action)
		{
			this._timers[action] = 0L;
			ActionState state;
			if (this._actionStates[action] == ActionState.Pressed)
			{
				state = ActionState.Hold;
			}
			else
			{
				state = this._actionStates[action];
			}
			this.SetActionState(action, state);
			this.CallSubscriptions(action);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001CD30 File Offset: 0x0001AF30
		private void StartHoldTimer(EnumEntityAction action)
		{
			this.StopHoldTimer(action);
			this._timers[action] = this._clientApi.World.RegisterCallback(delegate(float _)
			{
				this.OnHoldTimer(action);
			}, (int)this.HoldDuration.TotalMilliseconds);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001CD9C File Offset: 0x0001AF9C
		private void StopHoldTimer(EnumEntityAction action)
		{
			long timer = this._timers[action];
			if (timer == 0L)
			{
				return;
			}
			this._clientApi.World.UnregisterCallback(timer);
			this._timers[action] = 0L;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001CDDC File Offset: 0x0001AFDC
		private bool CallSubscriptions(EnumEntityAction action)
		{
			ActionState state = this._actionStates[action];
			bool handled = this.CallSubscriptionsForState(action, state);
			if ((this._actionStates[action] == ActionState.Hold || this._actionStates[action] == ActionState.Pressed) && this.CallSubscriptionsForState(action, ActionState.Active))
			{
				handled = true;
			}
			if (this._actionStates[action] == ActionState.Released && this.CallSubscriptionsForState(action, ActionState.Inactive))
			{
				handled = true;
			}
			return handled;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001CE48 File Offset: 0x0001B048
		private bool CallSubscriptionsForState(EnumEntityAction action, ActionState state)
		{
			ActionEventId id = new ActionEventId(action, state);
			List<Func<ActionEventData, bool>> value;
			if (!this._subscriptions.TryGetValue(id, ref value))
			{
				return false;
			}
			bool handled = false;
			ActionEventData eventData = new ActionEventData(id, this.GetActiveActions(), ActionListener.AltPressed(this._clientApi));
			using (List<Func<ActionEventData, bool>>.Enumerator enumerator = value.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Invoke(eventData))
					{
						handled = true;
						break;
					}
				}
			}
			return handled;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001CED4 File Offset: 0x0001B0D4
		private EnumEntityAction[] GetActiveActions()
		{
			if (!this._activeActionsSnapshotDirty)
			{
				return this._activeActionsSnapshot;
			}
			int count = 0;
			foreach (EnumEntityAction action in ActionListener._allActions)
			{
				if (this.IsActive(action))
				{
					count++;
				}
			}
			EnumEntityAction[] activeActions = new EnumEntityAction[count];
			int index = 0;
			foreach (EnumEntityAction action2 in ActionListener._allActions)
			{
				if (this.IsActive(action2))
				{
					activeActions[index++] = action2;
				}
			}
			this._activeActionsSnapshot = activeActions;
			this._activeActionsSnapshotDirty = false;
			return this._activeActionsSnapshot;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001CF6C File Offset: 0x0001B16C
		private void SetActionState(EnumEntityAction action, ActionState state)
		{
			ActionState current;
			if (this._actionStates.TryGetValue(action, ref current) && current == state)
			{
				return;
			}
			this._actionStates[action] = state;
			this._activeActionsSnapshotDirty = true;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001CFA4 File Offset: 0x0001B1A4
		private ActionState SwitchState(EnumEntityAction action, bool on)
		{
			ActionState actionState = this._actionStates[action];
			if (on)
			{
				switch (actionState)
				{
				case ActionState.Inactive:
					return ActionState.Pressed;
				case ActionState.Pressed:
					return ActionState.Active;
				case ActionState.Active:
					return ActionState.Active;
				case ActionState.Hold:
					return ActionState.Active;
				case ActionState.Released:
					return ActionState.Pressed;
				}
			}
			else
			{
				switch (actionState)
				{
				case ActionState.Inactive:
					return ActionState.Inactive;
				case ActionState.Pressed:
					return ActionState.Released;
				case ActionState.Active:
					return ActionState.Released;
				case ActionState.Hold:
					return ActionState.Released;
				case ActionState.Released:
					return ActionState.Inactive;
				}
			}
			return ActionState.Inactive;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001D024 File Offset: 0x0001B224
		private ActionState SwitchStateInconsistent(EnumEntityAction action)
		{
			if (this._actionStates[action] != ActionState.Inactive)
			{
				return ActionState.Inactive;
			}
			return ActionState.Pressed;
		}

		// Token: 0x040001D6 RID: 470
		private readonly Dictionary<ActionEventId, List<Func<ActionEventData, bool>>> _subscriptions = new Dictionary<ActionEventId, List<Func<ActionEventData, bool>>>();

		// Token: 0x040001D7 RID: 471
		private static readonly EnumEntityAction[] _allActions = Enum.GetValues<EnumEntityAction>();

		// Token: 0x040001D8 RID: 472
		private readonly HashSet<EnumEntityAction> _inconsistentInWorldMouseActions;

		// Token: 0x040001D9 RID: 473
		private readonly Dictionary<EnumEntityAction, long> _timers;

		// Token: 0x040001DA RID: 474
		private readonly Dictionary<EnumEntityAction, ActionState> _actionStates;

		// Token: 0x040001DB RID: 475
		private readonly Dictionary<EnumEntityAction, EnumEntityAction> _modifiersRemapping;

		// Token: 0x040001DC RID: 476
		private readonly HashSet<EnumEntityAction> _modifiers;

		// Token: 0x040001DD RID: 477
		private readonly CombatOverhaulSystem _system;

		// Token: 0x040001DE RID: 478
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x040001DF RID: 479
		private readonly long _tickListener;

		// Token: 0x040001E0 RID: 480
		private bool _suppressLMB;

		// Token: 0x040001E1 RID: 481
		private bool _suppressRMB;

		// Token: 0x040001E2 RID: 482
		private bool _disposed;

		// Token: 0x040001E3 RID: 483
		private EnumEntityAction[] _activeActionsSnapshot;

		// Token: 0x040001E4 RID: 484
		private bool _activeActionsSnapshotDirty;
	}
}
