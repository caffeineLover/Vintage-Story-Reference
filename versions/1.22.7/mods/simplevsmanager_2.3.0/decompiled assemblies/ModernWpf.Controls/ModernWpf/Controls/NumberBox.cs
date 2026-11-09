using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000078 RID: 120
	[TemplatePart(Name = "DownSpinButton", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "UpSpinButton", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "InputBox", Type = typeof(TextBox))]
	[TemplatePart(Name = "UpDownPopup", Type = typeof(Popup))]
	[TemplatePart(Name = "PopupDownSpinButton", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PopupUpSpinButton", Type = typeof(RepeatButton))]
	public class NumberBox : Control
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x000145A4 File Offset: 0x000127A4
		static NumberBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00014964 File Offset: 0x00012B64
		public NumberBox()
		{
			base.SetCurrentValue(NumberBox.NumberFormatterProperty, this.GetRegionalSettingsAwareDecimalFormatter());
			base.MouseWheel += new MouseWheelEventHandler(this.OnNumberBoxScroll);
			base.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnNumberBoxGotFocus);
			base.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnNumberBoxLostFocus);
			this.SetDefaultInputScope();
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000149D0 File Offset: 0x00012BD0
		private void SetDefaultInputScope()
		{
			InputScopeName inputScopeName = new InputScopeName(29);
			InputScope inputScope = new InputScope();
			inputScope.Names.Add(inputScopeName);
			base.SetValue(FrameworkElement.InputScopeProperty, inputScope);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00014A04 File Offset: 0x00012C04
		private INumberBoxNumberFormatter GetRegionalSettingsAwareDecimalFormatter()
		{
			return new DefaultNumberBoxNumberFormatter();
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00014A0B File Offset: 0x00012C0B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new NumberBoxAutomationPeer(this);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014A14 File Offset: 0x00012C14
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			string localizedStringResource = NumberBox.ResourceAccessor.GetLocalizedStringResource("NumberBoxDownSpinButtonName");
			string localizedStringResource2 = NumberBox.ResourceAccessor.GetLocalizedStringResource("NumberBoxUpSpinButtonName");
			RepeatButton repeatButton = base.GetTemplateChild("DownSpinButton") as RepeatButton;
			if (repeatButton != null)
			{
				repeatButton.Click += new RoutedEventHandler(this.OnSpinDownClick);
				if (string.IsNullOrEmpty(AutomationProperties.GetName(repeatButton)))
				{
					AutomationProperties.SetName(repeatButton, localizedStringResource);
				}
			}
			RepeatButton repeatButton2 = base.GetTemplateChild("UpSpinButton") as RepeatButton;
			if (repeatButton2 != null)
			{
				repeatButton2.Click += new RoutedEventHandler(this.OnSpinUpClick);
				if (string.IsNullOrEmpty(AutomationProperties.GetName(repeatButton2)))
				{
					AutomationProperties.SetName(repeatButton2, localizedStringResource2);
				}
			}
			this.UpdateHeaderPresenterState();
			this.m_textBox = this.<OnApplyTemplate>g__GetTextBox|17_0();
			this.m_popup = (base.GetTemplateChild("UpDownPopup") as Popup);
			if (this.m_popup != null)
			{
				if (Helper.HasDefaultValue(this.m_popup, Popup.PlacementTargetProperty))
				{
					this.m_popup.PlacementTarget = this.m_textBox;
				}
				this.m_popupRepositionHelper = new PopupRepositionHelper(this.m_popup, this);
			}
			RepeatButton repeatButton3 = base.GetTemplateChild("PopupDownSpinButton") as RepeatButton;
			if (repeatButton3 != null)
			{
				repeatButton3.Click += new RoutedEventHandler(this.OnSpinDownClick);
			}
			RepeatButton repeatButton4 = base.GetTemplateChild("PopupUpSpinButton") as RepeatButton;
			if (repeatButton4 != null)
			{
				repeatButton4.Click += new RoutedEventHandler(this.OnSpinUpClick);
			}
			base.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
			this.UpdateSpinButtonPlacement();
			this.UpdateSpinButtonEnabled();
			this.UpdateVisualStateForIsEnabledChange();
			if (base.ReadLocalValue(NumberBox.ValueProperty) == DependencyProperty.UnsetValue && base.ReadLocalValue(NumberBox.TextProperty) != DependencyProperty.UnsetValue)
			{
				this.UpdateValueToText();
				return;
			}
			this.UpdateTextToValue();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00014BC8 File Offset: 0x00012DC8
		private void OnValuePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (!this.m_valueUpdating)
			{
				double num = (double)args.OldValue;
				try
				{
					this.m_valueUpdating = true;
					this.CoerceValue();
					double value = this.Value;
					if (value != num && (!double.IsNaN(value) || !double.IsNaN(num)))
					{
						NumberBoxValueChangedEventArgs numberBoxValueChangedEventArgs = new NumberBoxValueChangedEventArgs(num, value);
						TypedEventHandler<NumberBox, NumberBoxValueChangedEventArgs> valueChanged = this.ValueChanged;
						if (valueChanged != null)
						{
							valueChanged.Invoke(this, numberBoxValueChangedEventArgs);
						}
						NumberBoxAutomationPeer numberBoxAutomationPeer = UIElementAutomationPeer.FromElement(this) as NumberBoxAutomationPeer;
						if (numberBoxAutomationPeer != null)
						{
							numberBoxAutomationPeer.RaiseValueChangedEvent(num, value);
						}
					}
					this.UpdateTextToValue();
					this.UpdateSpinButtonEnabled();
				}
				finally
				{
					this.m_valueUpdating = false;
				}
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00014C6C File Offset: 0x00012E6C
		private void OnMinimumPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.CoerceMaximum();
			this.CoerceValue();
			this.UpdateSpinButtonEnabled();
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00014C80 File Offset: 0x00012E80
		private void OnMaximumPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.CoerceMinimum();
			this.CoerceValue();
			this.UpdateSpinButtonEnabled();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00014C94 File Offset: 0x00012E94
		private void OnSmallChangePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateSpinButtonEnabled();
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00014C9C File Offset: 0x00012E9C
		private void OnIsWrapEnabledPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateSpinButtonEnabled();
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00014CA4 File Offset: 0x00012EA4
		private void OnNumberFormatterPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateTextToValue();
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00014CAC File Offset: 0x00012EAC
		private void ValidateNumberFormatter(INumberBoxNumberFormatter value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00014CBC File Offset: 0x00012EBC
		private void OnSpinButtonPlacementModePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateSpinButtonPlacement();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00014CC4 File Offset: 0x00012EC4
		private void OnTextPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (!this.m_textUpdating)
			{
				this.UpdateValueToText();
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00014CD4 File Offset: 0x00012ED4
		private void UpdateValueToText()
		{
			if (this.m_textBox != null)
			{
				this.m_textBox.Text = this.Text;
				this.ValidateInput();
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00014CF5 File Offset: 0x00012EF5
		private void OnHeaderPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateHeaderPresenterState();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00014CFD File Offset: 0x00012EFD
		private void OnHeaderTemplatePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.UpdateHeaderPresenterState();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00014D05 File Offset: 0x00012F05
		private void OnValidationModePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			this.ValidateInput();
			this.UpdateSpinButtonEnabled();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00014D13 File Offset: 0x00012F13
		private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			this.UpdateVisualStateForIsEnabledChange();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00014D1B File Offset: 0x00012F1B
		private void UpdateVisualStateForIsEnabledChange()
		{
			VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", false);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00014D39 File Offset: 0x00012F39
		private void OnNumberBoxGotFocus(object sender, RoutedEventArgs e)
		{
			if (this.m_textBox != null)
			{
				this.m_textBox.SelectAll();
			}
			if (this.SpinButtonPlacementMode == NumberBoxSpinButtonPlacementMode.Compact && this.m_popup != null)
			{
				this.m_popup.IsOpen = true;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014D6B File Offset: 0x00012F6B
		private void OnNumberBoxLostFocus(object sender, RoutedEventArgs e)
		{
			this.ValidateInput();
			if (this.m_popup != null)
			{
				this.m_popup.IsOpen = false;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00014D88 File Offset: 0x00012F88
		private void CoerceMinimum()
		{
			double maximum = this.Maximum;
			if (this.Minimum > maximum)
			{
				base.SetCurrentValue(NumberBox.MinimumProperty, maximum);
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00014DB8 File Offset: 0x00012FB8
		private void CoerceMaximum()
		{
			double minimum = this.Minimum;
			if (this.Maximum < minimum)
			{
				base.SetCurrentValue(NumberBox.MaximumProperty, minimum);
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00014DE8 File Offset: 0x00012FE8
		private void CoerceValue()
		{
			double value = this.Value;
			if (!double.IsNaN(value) && !this.IsInBounds(value) && this.ValidationMode == NumberBoxValidationMode.InvalidInputOverwritten)
			{
				double maximum = this.Maximum;
				if (value > maximum)
				{
					base.SetCurrentValue(NumberBox.ValueProperty, maximum);
					return;
				}
				base.SetCurrentValue(NumberBox.ValueProperty, this.Minimum);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00014E48 File Offset: 0x00013048
		private void ValidateInput()
		{
			if (this.m_textBox != null)
			{
				string text = this.m_textBox.Text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					base.SetCurrentValue(NumberBox.ValueProperty, double.NaN);
					return;
				}
				INumberBoxNumberFormatter numberFormatter = this.NumberFormatter;
				double? num = this.AcceptsExpression ? NumberBoxParser.Compute(text, numberFormatter) : numberFormatter.ParseDouble(text);
				if (num == null)
				{
					if (this.ValidationMode == NumberBoxValidationMode.InvalidInputOverwritten)
					{
						this.UpdateTextToValue();
						return;
					}
				}
				else
				{
					if (num.Value == this.Value)
					{
						this.UpdateTextToValue();
						return;
					}
					base.SetCurrentValue(NumberBox.ValueProperty, num.Value);
				}
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00014EF8 File Offset: 0x000130F8
		private void OnSpinDownClick(object sender, RoutedEventArgs args)
		{
			this.StepValue(-this.SmallChange);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00014F07 File Offset: 0x00013107
		private void OnSpinUpClick(object sender, RoutedEventArgs args)
		{
			this.StepValue(this.SmallChange);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00014F18 File Offset: 0x00013118
		private void OnNumberBoxKeyDown(object sender, KeyEventArgs args)
		{
			Key key = args.Key;
			if (key <= 20)
			{
				if (key == 19)
				{
					this.StepValue(this.LargeChange);
					args.Handled = true;
					return;
				}
				if (key != 20)
				{
					return;
				}
				this.StepValue(-this.LargeChange);
				args.Handled = true;
				return;
			}
			else
			{
				if (key == 24)
				{
					this.StepValue(this.SmallChange);
					args.Handled = true;
					return;
				}
				if (key != 26)
				{
					return;
				}
				this.StepValue(-this.SmallChange);
				args.Handled = true;
				return;
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00014F98 File Offset: 0x00013198
		private void OnNumberBoxKeyUp(object sender, KeyEventArgs args)
		{
			Key key = args.Key;
			if (key == 6)
			{
				this.ValidateInput();
				args.Handled = true;
				return;
			}
			if (key != 13)
			{
				return;
			}
			this.UpdateTextToValue();
			args.Handled = true;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00014FD4 File Offset: 0x000131D4
		private void OnNumberBoxScroll(object sender, MouseWheelEventArgs args)
		{
			if (this.m_textBox != null && this.m_textBox.IsFocused)
			{
				int delta = args.Delta;
				if (delta > 0)
				{
					this.StepValue(this.SmallChange);
				}
				else if (delta < 0)
				{
					this.StepValue(-this.SmallChange);
				}
				args.Handled = true;
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00015028 File Offset: 0x00013228
		private void StepValue(double change)
		{
			this.ValidateInput();
			double num = this.Value;
			if (!double.IsNaN(num))
			{
				num += change;
				if (this.IsWrapEnabled)
				{
					double maximum = this.Maximum;
					double minimum = this.Minimum;
					if (num > maximum)
					{
						num = minimum;
					}
					else if (num < minimum)
					{
						num = maximum;
					}
				}
				base.SetCurrentValue(NumberBox.ValueProperty, num);
				this.MoveCaretToTextEnd();
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001508C File Offset: 0x0001328C
		private void UpdateTextToValue()
		{
			if (this.m_textBox != null)
			{
				string text = string.Empty;
				double value = this.Value;
				if (!double.IsNaN(value))
				{
					double value2 = this.m_displayRounder.RoundDouble(value);
					text = this.NumberFormatter.FormatDouble(value2);
				}
				this.m_textBox.Text = text;
				try
				{
					this.m_textUpdating = true;
					base.SetCurrentValue(NumberBox.TextProperty, text);
				}
				finally
				{
					this.m_textUpdating = false;
				}
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001510C File Offset: 0x0001330C
		private void UpdateSpinButtonPlacement()
		{
			NumberBoxSpinButtonPlacementMode spinButtonPlacementMode = this.SpinButtonPlacementMode;
			if (spinButtonPlacementMode == NumberBoxSpinButtonPlacementMode.Inline)
			{
				VisualStateManager.GoToState(this, "SpinButtonsVisible", false);
				return;
			}
			if (spinButtonPlacementMode == NumberBoxSpinButtonPlacementMode.Compact)
			{
				VisualStateManager.GoToState(this, "SpinButtonsPopup", false);
				return;
			}
			VisualStateManager.GoToState(this, "SpinButtonsCollapsed", false);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00015154 File Offset: 0x00013354
		private void UpdateSpinButtonEnabled()
		{
			double value = this.Value;
			bool flag = false;
			bool flag2 = false;
			if (!double.IsNaN(value))
			{
				if (this.IsWrapEnabled || this.ValidationMode != NumberBoxValidationMode.InvalidInputOverwritten)
				{
					flag = true;
					flag2 = true;
				}
				else
				{
					if (value < this.Maximum)
					{
						flag = true;
					}
					if (value > this.Minimum)
					{
						flag2 = true;
					}
				}
			}
			VisualStateManager.GoToState(this, flag ? "UpSpinButtonEnabled" : "UpSpinButtonDisabled", false);
			VisualStateManager.GoToState(this, flag2 ? "DownSpinButtonEnabled" : "DownSpinButtonDisabled", false);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000151CE File Offset: 0x000133CE
		private bool IsInBounds(double value)
		{
			return value >= this.Minimum && value <= this.Maximum;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000151E8 File Offset: 0x000133E8
		private void UpdateHeaderPresenterState()
		{
			bool flag = false;
			object header = this.Header;
			if (header != null)
			{
				string text = header as string;
				if (text != null)
				{
					if (!string.IsNullOrEmpty(text))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
			}
			if (this.HeaderTemplate != null)
			{
				flag = true;
			}
			if (flag && this.m_headerPresenter == null)
			{
				ContentPresenter contentPresenter = base.GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
				if (contentPresenter != null)
				{
					this.m_headerPresenter = contentPresenter;
				}
			}
			if (this.m_headerPresenter != null)
			{
				this.m_headerPresenter.Visibility = (flag ? 0 : 2);
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00015264 File Offset: 0x00013464
		private void MoveCaretToTextEnd()
		{
			TextBox textBox = this.m_textBox;
			if (textBox != null)
			{
				textBox.Select(textBox.Text.Length, 0);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0001528D File Offset: 0x0001348D
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0001529F File Offset: 0x0001349F
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(NumberBox.MinimumProperty);
			}
			set
			{
				base.SetValue(NumberBox.MinimumProperty, value);
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000152B2 File Offset: 0x000134B2
		private static void OnMinimumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnMinimumPropertyChanged(args);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000152C0 File Offset: 0x000134C0
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x000152D2 File Offset: 0x000134D2
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(NumberBox.MaximumProperty);
			}
			set
			{
				base.SetValue(NumberBox.MaximumProperty, value);
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000152E5 File Offset: 0x000134E5
		private static void OnMaximumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnMaximumPropertyChanged(args);
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x000152F3 File Offset: 0x000134F3
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x00015305 File Offset: 0x00013505
		public double Value
		{
			get
			{
				return (double)base.GetValue(NumberBox.ValueProperty);
			}
			set
			{
				base.SetValue(NumberBox.ValueProperty, value);
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00015318 File Offset: 0x00013518
		private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnValuePropertyChanged(args);
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00015326 File Offset: 0x00013526
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x00015338 File Offset: 0x00013538
		public double SmallChange
		{
			get
			{
				return (double)base.GetValue(NumberBox.SmallChangeProperty);
			}
			set
			{
				base.SetValue(NumberBox.SmallChangeProperty, value);
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001534B File Offset: 0x0001354B
		private static void OnSmallChangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnSmallChangePropertyChanged(args);
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00015359 File Offset: 0x00013559
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x0001536B File Offset: 0x0001356B
		public double LargeChange
		{
			get
			{
				return (double)base.GetValue(NumberBox.LargeChangeProperty);
			}
			set
			{
				base.SetValue(NumberBox.LargeChangeProperty, value);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001537E File Offset: 0x0001357E
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x00015390 File Offset: 0x00013590
		public string Text
		{
			get
			{
				return (string)base.GetValue(NumberBox.TextProperty);
			}
			set
			{
				base.SetValue(NumberBox.TextProperty, value);
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001539E File Offset: 0x0001359E
		private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnTextPropertyChanged(args);
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x000153AC File Offset: 0x000135AC
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x000153B9 File Offset: 0x000135B9
		public object Header
		{
			get
			{
				return base.GetValue(NumberBox.HeaderProperty);
			}
			set
			{
				base.SetValue(NumberBox.HeaderProperty, value);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000153C7 File Offset: 0x000135C7
		private static void OnHeaderPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnHeaderPropertyChanged(args);
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x000153D5 File Offset: 0x000135D5
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x000153E7 File Offset: 0x000135E7
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(NumberBox.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(NumberBox.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000153F5 File Offset: 0x000135F5
		private static void OnHeaderTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnHeaderTemplatePropertyChanged(args);
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00015403 File Offset: 0x00013603
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x00015415 File Offset: 0x00013615
		public string PlaceholderText
		{
			get
			{
				return (string)base.GetValue(NumberBox.PlaceholderTextProperty);
			}
			set
			{
				base.SetValue(NumberBox.PlaceholderTextProperty, value);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00015423 File Offset: 0x00013623
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00015435 File Offset: 0x00013635
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(NumberBox.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(NumberBox.SelectionBrushProperty, value);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00015443 File Offset: 0x00013643
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x00015455 File Offset: 0x00013655
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(NumberBox.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(NumberBox.TextAlignmentProperty, value);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00015468 File Offset: 0x00013668
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x00015475 File Offset: 0x00013675
		public object Description
		{
			get
			{
				return base.GetValue(NumberBox.DescriptionProperty);
			}
			set
			{
				base.SetValue(NumberBox.DescriptionProperty, value);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00015483 File Offset: 0x00013683
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x00015495 File Offset: 0x00013695
		public NumberBoxValidationMode ValidationMode
		{
			get
			{
				return (NumberBoxValidationMode)base.GetValue(NumberBox.ValidationModeProperty);
			}
			set
			{
				base.SetValue(NumberBox.ValidationModeProperty, value);
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000154A8 File Offset: 0x000136A8
		private static void OnValidationModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnValidationModePropertyChanged(args);
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x000154B6 File Offset: 0x000136B6
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x000154C8 File Offset: 0x000136C8
		public NumberBoxSpinButtonPlacementMode SpinButtonPlacementMode
		{
			get
			{
				return (NumberBoxSpinButtonPlacementMode)base.GetValue(NumberBox.SpinButtonPlacementModeProperty);
			}
			set
			{
				base.SetValue(NumberBox.SpinButtonPlacementModeProperty, value);
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000154DB File Offset: 0x000136DB
		private static void OnSpinButtonPlacementModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnSpinButtonPlacementModePropertyChanged(args);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x000154E9 File Offset: 0x000136E9
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x000154FB File Offset: 0x000136FB
		public bool IsWrapEnabled
		{
			get
			{
				return (bool)base.GetValue(NumberBox.IsWrapEnabledProperty);
			}
			set
			{
				base.SetValue(NumberBox.IsWrapEnabledProperty, value);
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001550E File Offset: 0x0001370E
		private static void OnIsWrapEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((NumberBox)sender).OnIsWrapEnabledPropertyChanged(args);
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0001551C File Offset: 0x0001371C
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0001552E File Offset: 0x0001372E
		public bool AcceptsExpression
		{
			get
			{
				return (bool)base.GetValue(NumberBox.AcceptsExpressionProperty);
			}
			set
			{
				base.SetValue(NumberBox.AcceptsExpressionProperty, value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00015541 File Offset: 0x00013741
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x00015554 File Offset: 0x00013754
		public INumberBoxNumberFormatter NumberFormatter
		{
			get
			{
				return (INumberBoxNumberFormatter)base.GetValue(NumberBox.NumberFormatterProperty);
			}
			set
			{
				this.ValidateNumberFormatter(value);
				base.SetValue(NumberBox.NumberFormatterProperty, value);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00015578 File Offset: 0x00013778
		private static void OnNumberFormatterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			NumberBox numberBox = (NumberBox)sender;
			INumberBoxNumberFormatter numberBoxNumberFormatter2;
			INumberBoxNumberFormatter numberBoxNumberFormatter = numberBoxNumberFormatter2 = (INumberBoxNumberFormatter)args.NewValue;
			numberBox.ValidateNumberFormatter(numberBoxNumberFormatter2);
			if (numberBoxNumberFormatter != numberBoxNumberFormatter2)
			{
				sender.SetCurrentValue(args.Property, numberBoxNumberFormatter2);
				return;
			}
			numberBox.OnNumberFormatterPropertyChanged(args);
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x000155BA File Offset: 0x000137BA
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x000155CC File Offset: 0x000137CC
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(NumberBox.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(NumberBox.CornerRadiusProperty, value);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060005FA RID: 1530 RVA: 0x000155E0 File Offset: 0x000137E0
		// (remove) Token: 0x060005FB RID: 1531 RVA: 0x00015618 File Offset: 0x00013818
		public event TypedEventHandler<NumberBox, NumberBoxValueChangedEventArgs> ValueChanged;

		// Token: 0x060005FC RID: 1532 RVA: 0x00015650 File Offset: 0x00013850
		[CompilerGenerated]
		private TextBox <OnApplyTemplate>g__GetTextBox|17_0()
		{
			TextBox textBox = base.GetTemplateChild("InputBox") as TextBox;
			if (textBox != null)
			{
				if (SharedHelpers.IsRS3OrHigher())
				{
					textBox.PreviewKeyDown += new KeyEventHandler(this.OnNumberBoxKeyDown);
				}
				else
				{
					textBox.KeyDown += new KeyEventHandler(this.OnNumberBoxKeyDown);
				}
				textBox.KeyUp += new KeyEventHandler(this.OnNumberBoxKeyUp);
			}
			return textBox;
		}

		// Token: 0x0400029C RID: 668
		private const string c_numberBoxHeaderName = "HeaderContentPresenter";

		// Token: 0x0400029D RID: 669
		private const string c_numberBoxDownButtonName = "DownSpinButton";

		// Token: 0x0400029E RID: 670
		private const string c_numberBoxUpButtonName = "UpSpinButton";

		// Token: 0x0400029F RID: 671
		private const string c_numberBoxTextBoxName = "InputBox";

		// Token: 0x040002A0 RID: 672
		private const string c_numberBoxPopupButtonName = "PopupButton";

		// Token: 0x040002A1 RID: 673
		private const string c_numberBoxPopupName = "UpDownPopup";

		// Token: 0x040002A2 RID: 674
		private const string c_numberBoxPopupDownButtonName = "PopupDownSpinButton";

		// Token: 0x040002A3 RID: 675
		private const string c_numberBoxPopupUpButtonName = "PopupUpSpinButton";

		// Token: 0x040002A4 RID: 676
		private const string c_numberBoxPopupContentRootName = "PopupContentRoot";

		// Token: 0x040002A5 RID: 677
		private const double c_popupShadowDepth = 16.0;

		// Token: 0x040002A6 RID: 678
		private const string c_numberBoxPopupShadowDepthName = "NumberBoxPopupShadowDepth";

		// Token: 0x040002A7 RID: 679
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(NumberBox));

		// Token: 0x040002A8 RID: 680
		private bool m_valueUpdating;

		// Token: 0x040002A9 RID: 681
		private bool m_textUpdating;

		// Token: 0x040002AA RID: 682
		private DefaultNumberRounder m_displayRounder = new DefaultNumberRounder();

		// Token: 0x040002AB RID: 683
		private TextBox m_textBox;

		// Token: 0x040002AC RID: 684
		private ContentPresenter m_headerPresenter;

		// Token: 0x040002AD RID: 685
		private Popup m_popup;

		// Token: 0x040002AE RID: 686
		private PopupRepositionHelper m_popupRepositionHelper;

		// Token: 0x040002AF RID: 687
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumberBox), new PropertyMetadata(double.MinValue, new PropertyChangedCallback(NumberBox.OnMinimumPropertyChanged)));

		// Token: 0x040002B0 RID: 688
		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumberBox), new PropertyMetadata(double.MaxValue, new PropertyChangedCallback(NumberBox.OnMaximumPropertyChanged)));

		// Token: 0x040002B1 RID: 689
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumberBox), new FrameworkPropertyMetadata(double.NaN, 1280, new PropertyChangedCallback(NumberBox.OnValuePropertyChanged)));

		// Token: 0x040002B2 RID: 690
		public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(double), typeof(NumberBox), new PropertyMetadata(1.0, new PropertyChangedCallback(NumberBox.OnSmallChangePropertyChanged)));

		// Token: 0x040002B3 RID: 691
		public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register("LargeChange", typeof(double), typeof(NumberBox), new PropertyMetadata(10.0));

		// Token: 0x040002B4 RID: 692
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NumberBox), new FrameworkPropertyMetadata(string.Empty, 1280, new PropertyChangedCallback(NumberBox.OnTextPropertyChanged)));

		// Token: 0x040002B5 RID: 693
		public static readonly DependencyProperty HeaderProperty = ControlHelper.HeaderProperty.AddOwner(typeof(NumberBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(NumberBox.OnHeaderPropertyChanged)));

		// Token: 0x040002B6 RID: 694
		public static readonly DependencyProperty HeaderTemplateProperty = ControlHelper.HeaderTemplateProperty.AddOwner(typeof(NumberBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(NumberBox.OnHeaderTemplatePropertyChanged)));

		// Token: 0x040002B7 RID: 695
		public static readonly DependencyProperty PlaceholderTextProperty = ControlHelper.PlaceholderTextProperty.AddOwner(typeof(NumberBox));

		// Token: 0x040002B8 RID: 696
		public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(NumberBox));

		// Token: 0x040002B9 RID: 697
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(NumberBox), new PropertyMetadata(0));

		// Token: 0x040002BA RID: 698
		public static readonly DependencyProperty DescriptionProperty = ControlHelper.DescriptionProperty.AddOwner(typeof(NumberBox));

		// Token: 0x040002BB RID: 699
		public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.Register("ValidationMode", typeof(NumberBoxValidationMode), typeof(NumberBox), new PropertyMetadata(new PropertyChangedCallback(NumberBox.OnValidationModePropertyChanged)));

		// Token: 0x040002BC RID: 700
		public static readonly DependencyProperty SpinButtonPlacementModeProperty = DependencyProperty.Register("SpinButtonPlacementMode", typeof(NumberBoxSpinButtonPlacementMode), typeof(NumberBox), new PropertyMetadata(NumberBoxSpinButtonPlacementMode.Hidden, new PropertyChangedCallback(NumberBox.OnSpinButtonPlacementModePropertyChanged)));

		// Token: 0x040002BD RID: 701
		public static readonly DependencyProperty IsWrapEnabledProperty = DependencyProperty.Register("IsWrapEnabled", typeof(bool), typeof(NumberBox), new PropertyMetadata(false, new PropertyChangedCallback(NumberBox.OnIsWrapEnabledPropertyChanged)));

		// Token: 0x040002BE RID: 702
		public static readonly DependencyProperty AcceptsExpressionProperty = DependencyProperty.Register("AcceptsExpression", typeof(bool), typeof(NumberBox), new PropertyMetadata(false));

		// Token: 0x040002BF RID: 703
		public static readonly DependencyProperty NumberFormatterProperty = DependencyProperty.Register("NumberFormatter", typeof(INumberBoxNumberFormatter), typeof(NumberBox), new PropertyMetadata(new PropertyChangedCallback(NumberBox.OnNumberFormatterPropertyChanged)));

		// Token: 0x040002C0 RID: 704
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(NumberBox));
	}
}
