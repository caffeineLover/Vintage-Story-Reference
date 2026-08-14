using System;
using System.Windows;
using System.Windows.Input;

namespace ModernWpf.Input
{
	// Token: 0x0200025B RID: 603
	internal class GettingFocusEventArgs : IGettingFocusEventArgs2
	{
		// Token: 0x06001135 RID: 4405 RVA: 0x00032C70 File Offset: 0x00030E70
		internal GettingFocusEventArgs(KeyboardFocusChangedEventArgs args)
		{
			this._args = args;
			InputDevice mostRecentInputDevice = InputManager.Current.MostRecentInputDevice;
			FocusInputDeviceKind focusInputDeviceKind;
			if (!(mostRecentInputDevice is MouseDevice))
			{
				if (!(mostRecentInputDevice is TouchDevice))
				{
					if (!(mostRecentInputDevice is StylusDevice))
					{
						if (!(mostRecentInputDevice is TabletDevice))
						{
							if (!(mostRecentInputDevice is KeyboardDevice))
							{
								focusInputDeviceKind = FocusInputDeviceKind.Mouse;
							}
							else
							{
								focusInputDeviceKind = FocusInputDeviceKind.Keyboard;
							}
						}
						else
						{
							focusInputDeviceKind = FocusInputDeviceKind.Pen;
						}
					}
					else
					{
						focusInputDeviceKind = FocusInputDeviceKind.Pen;
					}
				}
				else
				{
					focusInputDeviceKind = FocusInputDeviceKind.Touch;
				}
			}
			else
			{
				focusInputDeviceKind = FocusInputDeviceKind.Mouse;
			}
			this.InputDevice = focusInputDeviceKind;
			this.OldFocusedElement = (args.OldFocus as DependencyObject);
			this.NewFocusedElement = (args.NewFocus as DependencyObject);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00032D00 File Offset: 0x00030F00
		public bool TrySetNewFocusedElement(DependencyObject element)
		{
			IInputElement inputElement = element as IInputElement;
			if (inputElement != null && Keyboard.Focus(inputElement) == inputElement)
			{
				this.Cancel = true;
				return true;
			}
			return false;
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00032D2A File Offset: 0x00030F2A
		// (set) Token: 0x06001138 RID: 4408 RVA: 0x00032D32 File Offset: 0x00030F32
		public DependencyObject NewFocusedElement { get; set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x00032D3B File Offset: 0x00030F3B
		// (set) Token: 0x0600113A RID: 4410 RVA: 0x00032D43 File Offset: 0x00030F43
		public bool Handled { get; set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00032D4C File Offset: 0x00030F4C
		// (set) Token: 0x0600113C RID: 4412 RVA: 0x00032D54 File Offset: 0x00030F54
		public bool Cancel { get; set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x00032D5D File Offset: 0x00030F5D
		public FocusInputDeviceKind InputDevice { get; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x00032D65 File Offset: 0x00030F65
		public DependencyObject OldFocusedElement { get; }

		// Token: 0x040004BC RID: 1212
		private KeyboardFocusChangedEventArgs _args;
	}
}
