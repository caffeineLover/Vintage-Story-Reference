using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x0200006F RID: 111
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DirectionController
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001E811 File Offset: 0x0001CA11
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0001E819 File Offset: 0x0001CA19
		public DirectionsConfiguration DirectionsConfiguration { get; set; } = DirectionsConfiguration.Eight;

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001E822 File Offset: 0x0001CA22
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0001E82A File Offset: 0x0001CA2A
		public int Depth { get; set; } = 5;

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001E833 File Offset: 0x0001CA33
		public float Sensitivity
		{
			get
			{
				return this._settings.DirectionsSensitivity;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001E840 File Offset: 0x0001CA40
		public bool Invert
		{
			get
			{
				return this._settings.DirectionsInvert;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001E84D File Offset: 0x0001CA4D
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0001E855 File Offset: 0x0001CA55
		public AttackDirection CurrentDirection { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001E85E File Offset: 0x0001CA5E
		public AttackDirection CurrentDirectionWithInversion
		{
			get
			{
				if (!this.Invert)
				{
					return this.CurrentDirection;
				}
				return DirectionController.InvertDirection(this.CurrentDirection, this.DirectionsConfiguration);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001E880 File Offset: 0x0001CA80
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x0001E888 File Offset: 0x0001CA88
		public int CurrentDirectionNormalized { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001E891 File Offset: 0x0001CA91
		public bool AlternativeDirectionControls
		{
			get
			{
				return this._settings.DirectionsMovementControls;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001E89E File Offset: 0x0001CA9E
		public bool DirectionsHotkeysControls
		{
			get
			{
				return this._settings.DirectionsHotkeysControls;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001E8AB File Offset: 0x0001CAAB
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x0001E8B3 File Offset: 0x0001CAB3
		public bool DirectionLocked { get; private set; }

		// Token: 0x06000541 RID: 1345 RVA: 0x0001E8BC File Offset: 0x0001CABC
		public DirectionController(ICoreClientAPI api, DirectionCursorRenderer renderer, Settings settings)
		{
			this._api = api;
			this._directionCursorRenderer = renderer;
			this._settings = settings;
			for (int count = 0; count < this.Depth * 2; count++)
			{
				this._directionQueue.Enqueue(new MouseMovementData(0f, 0f, 0f, 0f));
			}
			api.Input.RegisterHotKey("combatoverhaul:directions-cursor-forward", "(CO) Directions cursor Up", 105, 4, false, false, false);
			api.Input.RegisterHotKey("combatoverhaul:directions-cursor-backward", "(CO) Directions cursor Down", 101, 4, false, false, false);
			api.Input.RegisterHotKey("combatoverhaul:directions-cursor-left", "(CO) Directions cursor Left", 83, 4, false, false, false);
			api.Input.RegisterHotKey("combatoverhaul:directions-cursor-right", "(CO) Directions cursor Right", 86, 4, false, false, false);
			api.Input.RegisterHotKey("combatoverhaul:directions-cursor-lock", "(CO) Directions cursor lock", 100, 4, false, false, true);
			api.Input.SetHotKeyHandler("combatoverhaul:directions-cursor-lock", (KeyCombination _) => this.ToggleDirectionsLock());
			this._forwardHotkey = api.Input.HotKeys["combatoverhaul:directions-cursor-forward"];
			this._backwardHotkey = api.Input.HotKeys["combatoverhaul:directions-cursor-backward"];
			this._leftHotkey = api.Input.HotKeys["combatoverhaul:directions-cursor-left"];
			this._rightHotkey = api.Input.HotKeys["combatoverhaul:directions-cursor-right"];
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001EA44 File Offset: 0x0001CC44
		public bool ToggleDirectionsLock()
		{
			if (this._api.World.ElapsedMilliseconds - this._lastDirectionToggle < 1000L)
			{
				return false;
			}
			if (this.DirectionLocked)
			{
				this._api.TriggerIngameError(this, "", "Direction cursor unlocked");
				this.DirectionLocked = false;
				this._lastDirectionToggle = this._api.World.ElapsedMilliseconds;
				return true;
			}
			this._api.TriggerIngameError(this, "", "Direction cursor locked");
			this.DirectionLocked = true;
			this._lastDirectionToggle = this._api.World.ElapsedMilliseconds;
			return true;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001EAE4 File Offset: 0x0001CCE4
		public void OnGameTick(bool forceNewDirection = false)
		{
			if (this.DirectionLocked && !forceNewDirection)
			{
				return;
			}
			if (this.DirectionsConfiguration == (DirectionsConfiguration)0)
			{
				this.DirectionsConfiguration = DirectionsConfiguration.None;
			}
			if (this.DirectionsConfiguration == DirectionsConfiguration.None)
			{
				this._directionCursorRenderer.Show = false;
				return;
			}
			this._directionCursorRenderer.Show = true;
			float pitch = this._api.Input.MousePitch;
			float yaw = this._api.Input.MouseYaw;
			this._directionQueue.Enqueue(new MouseMovementData(pitch, yaw, pitch - Enumerable.Last<MouseMovementData>(this._directionQueue).Pitch, yaw - Enumerable.Last<MouseMovementData>(this._directionQueue).Yaw));
			MouseMovementData previous = this._directionQueue.Dequeue();
			if (this.AlternativeDirectionControls || this.DirectionsHotkeysControls)
			{
				bool changeDirection;
				AttackDirection direction = this.CalculateDirectionWithAlternativeControls(out changeDirection);
				if (changeDirection)
				{
					this.CurrentDirectionNormalized = (int)direction;
					this.CurrentDirection = direction;
					this._directionCursorRenderer.CurrentDirection = (int)this.CurrentDirection;
				}
			}
			else
			{
				int direction2 = this.CalculateDirection(previous.Yaw - yaw, previous.Pitch - pitch, (int)this.DirectionsConfiguration);
				float delta = Enumerable.Last<MouseMovementData>(this._directionQueue).DeltaPitch * Enumerable.Last<MouseMovementData>(this._directionQueue).DeltaPitch + Enumerable.Last<MouseMovementData>(this._directionQueue).DeltaYaw * Enumerable.Last<MouseMovementData>(this._directionQueue).DeltaYaw;
				if (forceNewDirection || delta > 0.001f / this.Sensitivity)
				{
					this.CurrentDirectionNormalized = direction2;
					this.CurrentDirection = (AttackDirection)DirectionController.Configurations[this.DirectionsConfiguration][this.CurrentDirectionNormalized];
					this._directionCursorRenderer.CurrentDirection = (int)this.CurrentDirection;
				}
			}
			List<int> allowedDirections;
			if (DirectionController.Configurations.TryGetValue(this.DirectionsConfiguration, ref allowedDirections) && !allowedDirections.Contains((int)this.CurrentDirection))
			{
				this.CurrentDirection = (AttackDirection)allowedDirections[0];
				this.CurrentDirectionNormalized = allowedDirections[0];
				this._directionCursorRenderer.CurrentDirection = (int)this.CurrentDirection;
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
		public void FlipDirectionToOpposite()
		{
			AttackDirection newDirection = DirectionController.GetOppositeDirection(this.CurrentDirection, this.DirectionsConfiguration);
			this.CurrentDirection = newDirection;
			this.CurrentDirectionNormalized = (int)newDirection;
			this._directionCursorRenderer.CurrentDirection = (int)this.CurrentDirection;
			List<int> allowedDirections;
			if (DirectionController.Configurations.TryGetValue(this.DirectionsConfiguration, ref allowedDirections) && !allowedDirections.Contains((int)this.CurrentDirection))
			{
				this.CurrentDirection = (AttackDirection)allowedDirections[0];
				this.CurrentDirectionNormalized = allowedDirections[0];
				this._directionCursorRenderer.CurrentDirection = (int)this.CurrentDirection;
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001ED7C File Offset: 0x0001CF7C
		private int CalculateDirection(float yaw, float pitch, int directionsCount)
		{
			if (this.Invert)
			{
				yaw *= -1f;
				pitch *= -1f;
			}
			float angleSegment = 360f / (float)directionsCount;
			float directionOffset = angleSegment / 2f;
			return (int)((MathF.Atan2(yaw, pitch) * 57.295776f + directionOffset + 360f) / angleSegment) % directionsCount;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001EDD0 File Offset: 0x0001CFD0
		private static AttackDirection GetOppositeDirection(AttackDirection direction, DirectionsConfiguration configuration)
		{
			switch (configuration)
			{
			case DirectionsConfiguration.None:
			case DirectionsConfiguration.TopBottom:
			case DirectionsConfiguration.Square:
			case DirectionsConfiguration.Eight:
			{
				AttackDirection result;
				switch (direction)
				{
				case AttackDirection.Top:
					result = AttackDirection.Bottom;
					break;
				case AttackDirection.TopRight:
					result = AttackDirection.BottomLeft;
					break;
				case AttackDirection.Right:
					result = AttackDirection.Left;
					break;
				case AttackDirection.BottomRight:
					result = AttackDirection.TopLeft;
					break;
				case AttackDirection.Bottom:
					result = AttackDirection.Top;
					break;
				case AttackDirection.BottomLeft:
					result = AttackDirection.TopRight;
					break;
				case AttackDirection.Left:
					result = AttackDirection.Right;
					break;
				case AttackDirection.TopLeft:
					result = AttackDirection.BottomRight;
					break;
				default:
					result = AttackDirection.Top;
					break;
				}
				return result;
			}
			case DirectionsConfiguration.Triangle:
			{
				AttackDirection result;
				switch (direction)
				{
				case AttackDirection.Top:
					result = AttackDirection.Top;
					break;
				case AttackDirection.TopRight:
					result = AttackDirection.TopLeft;
					break;
				case AttackDirection.Right:
					result = AttackDirection.Left;
					break;
				case AttackDirection.BottomRight:
					result = AttackDirection.BottomLeft;
					break;
				case AttackDirection.Bottom:
					result = AttackDirection.Bottom;
					break;
				case AttackDirection.BottomLeft:
					result = AttackDirection.BottomRight;
					break;
				case AttackDirection.Left:
					result = AttackDirection.Right;
					break;
				case AttackDirection.TopLeft:
					result = AttackDirection.TopRight;
					break;
				default:
					result = AttackDirection.Top;
					break;
				}
				return result;
			}
			case DirectionsConfiguration.Star:
			{
				AttackDirection result;
				switch (direction)
				{
				case AttackDirection.Top:
					result = AttackDirection.Top;
					break;
				case AttackDirection.TopRight:
					result = AttackDirection.TopRight;
					break;
				case AttackDirection.Right:
					result = AttackDirection.Left;
					break;
				case AttackDirection.BottomRight:
					result = AttackDirection.BottomLeft;
					break;
				case AttackDirection.Bottom:
					result = AttackDirection.Bottom;
					break;
				case AttackDirection.BottomLeft:
					result = AttackDirection.BottomRight;
					break;
				case AttackDirection.Left:
					result = AttackDirection.Right;
					break;
				case AttackDirection.TopLeft:
					result = AttackDirection.TopRight;
					break;
				default:
					result = AttackDirection.Top;
					break;
				}
				return result;
			}
			}
			return AttackDirection.Top;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001EEF0 File Offset: 0x0001D0F0
		private static AttackDirection InvertDirection(AttackDirection direction, DirectionsConfiguration configuration)
		{
			switch (configuration)
			{
			case DirectionsConfiguration.None:
			case DirectionsConfiguration.TopBottom:
			case DirectionsConfiguration.Square:
			case DirectionsConfiguration.Eight:
			{
				AttackDirection result;
				switch (direction)
				{
				case AttackDirection.Top:
					result = AttackDirection.Bottom;
					break;
				case AttackDirection.TopRight:
					result = AttackDirection.BottomLeft;
					break;
				case AttackDirection.Right:
					result = AttackDirection.Left;
					break;
				case AttackDirection.BottomRight:
					result = AttackDirection.TopLeft;
					break;
				case AttackDirection.Bottom:
					result = AttackDirection.Top;
					break;
				case AttackDirection.BottomLeft:
					result = AttackDirection.TopRight;
					break;
				case AttackDirection.Left:
					result = AttackDirection.Right;
					break;
				case AttackDirection.TopLeft:
					result = AttackDirection.BottomRight;
					break;
				default:
					result = AttackDirection.Top;
					break;
				}
				return result;
			}
			case DirectionsConfiguration.Triangle:
			{
				AttackDirection result;
				switch (direction)
				{
				case AttackDirection.Top:
					result = AttackDirection.Bottom;
					break;
				case AttackDirection.TopRight:
					result = AttackDirection.BottomLeft;
					break;
				case AttackDirection.Right:
					result = AttackDirection.Left;
					break;
				case AttackDirection.BottomRight:
					result = AttackDirection.TopLeft;
					break;
				case AttackDirection.Bottom:
					result = AttackDirection.Top;
					break;
				case AttackDirection.BottomLeft:
					result = AttackDirection.TopRight;
					break;
				case AttackDirection.Left:
					result = AttackDirection.Right;
					break;
				case AttackDirection.TopLeft:
					result = AttackDirection.BottomRight;
					break;
				default:
					result = AttackDirection.Top;
					break;
				}
				return result;
			}
			case DirectionsConfiguration.Star:
			{
				AttackDirection result;
				switch (direction)
				{
				case AttackDirection.Top:
					result = AttackDirection.Bottom;
					break;
				case AttackDirection.TopRight:
					result = AttackDirection.BottomLeft;
					break;
				case AttackDirection.Right:
					result = AttackDirection.Left;
					break;
				case AttackDirection.BottomRight:
					result = AttackDirection.TopLeft;
					break;
				case AttackDirection.Bottom:
					result = AttackDirection.Top;
					break;
				case AttackDirection.BottomLeft:
					result = AttackDirection.TopRight;
					break;
				case AttackDirection.Left:
					result = AttackDirection.Right;
					break;
				case AttackDirection.TopLeft:
					result = AttackDirection.BottomRight;
					break;
				default:
					result = AttackDirection.Top;
					break;
				}
				return result;
			}
			}
			return AttackDirection.Top;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001F010 File Offset: 0x0001D210
		private AttackDirection CalculateDirectionWithAlternativeControls(out bool changeDirection)
		{
			bool forward;
			bool backward;
			bool left;
			bool right;
			if (!this.DirectionsHotkeysControls)
			{
				ValueTuple<bool, bool, bool, bool> valueTuple = this.GetControlsState();
				forward = valueTuple.Item1;
				backward = valueTuple.Item2;
				left = valueTuple.Item3;
				right = valueTuple.Item4;
			}
			else
			{
				ValueTuple<bool, bool, bool, bool> valueTuple = this.GetHotkeysState();
				forward = valueTuple.Item1;
				backward = valueTuple.Item2;
				left = valueTuple.Item3;
				right = valueTuple.Item4;
			}
			if (this.Invert)
			{
				bool flag = forward;
				forward = backward;
				backward = flag;
				bool flag2 = left;
				left = right;
				right = flag2;
			}
			changeDirection = true;
			switch (this.DirectionsConfiguration)
			{
			case DirectionsConfiguration.None:
				return AttackDirection.Top;
			case DirectionsConfiguration.TopBottom:
				if (forward)
				{
					return AttackDirection.Top;
				}
				if (backward)
				{
					return AttackDirection.Bottom;
				}
				changeDirection = false;
				return AttackDirection.Top;
			case DirectionsConfiguration.Triangle:
				if (forward)
				{
					return AttackDirection.Top;
				}
				if (right)
				{
					return AttackDirection.BottomRight;
				}
				if (left)
				{
					return AttackDirection.BottomLeft;
				}
				changeDirection = false;
				return AttackDirection.Top;
			case DirectionsConfiguration.Square:
				if (forward)
				{
					return AttackDirection.Top;
				}
				if (backward)
				{
					return AttackDirection.Bottom;
				}
				if (left)
				{
					return AttackDirection.Left;
				}
				if (right)
				{
					return AttackDirection.Right;
				}
				changeDirection = false;
				return AttackDirection.Top;
			case DirectionsConfiguration.Star:
				if (forward)
				{
					return AttackDirection.Top;
				}
				if (left && !backward)
				{
					return AttackDirection.TopLeft;
				}
				if (right && !backward)
				{
					return AttackDirection.TopRight;
				}
				if (left)
				{
					return AttackDirection.BottomLeft;
				}
				if (right)
				{
					return AttackDirection.BottomRight;
				}
				changeDirection = false;
				return AttackDirection.Top;
			case DirectionsConfiguration.Eight:
				if (forward && left)
				{
					return AttackDirection.TopLeft;
				}
				if (forward && right)
				{
					return AttackDirection.TopRight;
				}
				if (backward && left)
				{
					return AttackDirection.BottomLeft;
				}
				if (backward && right)
				{
					return AttackDirection.BottomRight;
				}
				if (forward)
				{
					return AttackDirection.Top;
				}
				if (backward)
				{
					return AttackDirection.Bottom;
				}
				if (left)
				{
					return AttackDirection.Left;
				}
				if (right)
				{
					return AttackDirection.Right;
				}
				changeDirection = false;
				return AttackDirection.Top;
			}
			changeDirection = false;
			return AttackDirection.Top;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001F160 File Offset: 0x0001D360
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"forward",
			"backward",
			"left",
			"right"
		})]
		private ValueTuple<bool, bool, bool, bool> GetControlsState()
		{
			EntityControls controls = this._api.World.Player.Entity.Controls;
			bool forward = controls.Forward;
			bool backward = controls.Backward;
			bool left = controls.Left;
			bool right = controls.Right;
			return new ValueTuple<bool, bool, bool, bool>(forward, backward, left, right);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"forward",
			"backward",
			"left",
			"right"
		})]
		private ValueTuple<bool, bool, bool, bool> GetHotkeysState()
		{
			bool flag = this._api.Input.IsHotKeyPressed(this._forwardHotkey);
			bool backward = this._api.Input.IsHotKeyPressed(this._backwardHotkey);
			bool left = this._api.Input.IsHotKeyPressed(this._leftHotkey);
			bool right = this._api.Input.IsHotKeyPressed(this._rightHotkey);
			return new ValueTuple<bool, bool, bool, bool>(flag, backward, left, right);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001F21C File Offset: 0x0001D41C
		// Note: this type is marked as 'beforefieldinit'.
		static DirectionController()
		{
			Dictionary<DirectionsConfiguration, List<int>> dictionary = new Dictionary<DirectionsConfiguration, List<int>>();
			DirectionsConfiguration directionsConfiguration = DirectionsConfiguration.TopBottom;
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(4);
			dictionary.Add(directionsConfiguration, list);
			DirectionsConfiguration directionsConfiguration2 = DirectionsConfiguration.Triangle;
			List<int> list2 = new List<int>();
			list2.Add(0);
			list2.Add(3);
			list2.Add(5);
			dictionary.Add(directionsConfiguration2, list2);
			DirectionsConfiguration directionsConfiguration3 = DirectionsConfiguration.Square;
			List<int> list3 = new List<int>();
			list3.Add(0);
			list3.Add(2);
			list3.Add(4);
			list3.Add(6);
			dictionary.Add(directionsConfiguration3, list3);
			DirectionsConfiguration directionsConfiguration4 = DirectionsConfiguration.Star;
			List<int> list4 = new List<int>();
			list4.Add(0);
			list4.Add(1);
			list4.Add(3);
			list4.Add(5);
			list4.Add(7);
			dictionary.Add(directionsConfiguration4, list4);
			DirectionsConfiguration directionsConfiguration5 = DirectionsConfiguration.Eight;
			List<int> list5 = new List<int>();
			list5.Add(0);
			list5.Add(1);
			list5.Add(2);
			list5.Add(3);
			list5.Add(4);
			list5.Add(5);
			list5.Add(6);
			list5.Add(7);
			dictionary.Add(directionsConfiguration5, list5);
			DirectionController.Configurations = dictionary;
		}

		// Token: 0x04000227 RID: 551
		public static readonly Dictionary<DirectionsConfiguration, List<int>> Configurations;

		// Token: 0x04000228 RID: 552
		private const float _sensitivityFactor = 0.001f;

		// Token: 0x04000229 RID: 553
		private readonly ICoreClientAPI _api;

		// Token: 0x0400022A RID: 554
		private readonly Queue<MouseMovementData> _directionQueue = new Queue<MouseMovementData>();

		// Token: 0x0400022B RID: 555
		private readonly DirectionCursorRenderer _directionCursorRenderer;

		// Token: 0x0400022C RID: 556
		private readonly Settings _settings;

		// Token: 0x0400022D RID: 557
		private readonly HotKey _forwardHotkey;

		// Token: 0x0400022E RID: 558
		private readonly HotKey _backwardHotkey;

		// Token: 0x0400022F RID: 559
		private readonly HotKey _leftHotkey;

		// Token: 0x04000230 RID: 560
		private readonly HotKey _rightHotkey;

		// Token: 0x04000231 RID: 561
		private long _lastDirectionToggle;
	}
}
