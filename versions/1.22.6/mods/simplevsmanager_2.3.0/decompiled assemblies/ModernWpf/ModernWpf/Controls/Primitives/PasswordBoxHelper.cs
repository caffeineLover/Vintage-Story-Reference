using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000297 RID: 663
	public class PasswordBoxHelper : DependencyObject
	{
		// Token: 0x060013A0 RID: 5024 RVA: 0x00039CD4 File Offset: 0x00037ED4
		static PasswordBoxHelper()
		{
			PasswordBoxHelper.TextBoxCutBinding = new CommandBinding(ApplicationCommands.Cut);
			PasswordBoxHelper.TextBoxCutBinding.CanExecute += new CanExecuteRoutedEventHandler(PasswordBoxHelper.OnDisabledCommandCanExecute);
			PasswordBoxHelper.TextBoxCopyBinding = new CommandBinding(ApplicationCommands.Copy);
			PasswordBoxHelper.TextBoxCopyBinding.CanExecute += new CanExecuteRoutedEventHandler(PasswordBoxHelper.OnDisabledCommandCanExecute);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00039E0A File Offset: 0x0003800A
		public PasswordBoxHelper(PasswordBox passwordBox)
		{
			this._passwordBox = passwordBox;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00039E19 File Offset: 0x00038019
		public static PasswordRevealMode GetPasswordRevealMode(PasswordBox passwordBox)
		{
			return (PasswordRevealMode)passwordBox.GetValue(PasswordBoxHelper.PasswordRevealModeProperty);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00039E2B File Offset: 0x0003802B
		public static void SetPasswordRevealMode(PasswordBox passwordBox, PasswordRevealMode value)
		{
			passwordBox.SetValue(PasswordBoxHelper.PasswordRevealModeProperty, value);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00039E3E File Offset: 0x0003803E
		private static void OnPasswordRevealModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBoxHelper helperInstance = PasswordBoxHelper.GetHelperInstance((PasswordBox)d);
			if (helperInstance == null)
			{
				return;
			}
			helperInstance.UpdateVisualState(true);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00039E56 File Offset: 0x00038056
		public static bool GetIsEnabled(PasswordBox passwordBox)
		{
			return (bool)passwordBox.GetValue(PasswordBoxHelper.IsEnabledProperty);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00039E68 File Offset: 0x00038068
		public static void SetIsEnabled(PasswordBox passwordBox, bool value)
		{
			passwordBox.SetValue(PasswordBoxHelper.IsEnabledProperty, value);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00039E7C File Offset: 0x0003807C
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if ((bool)e.NewValue)
			{
				PasswordBoxHelper.SetHelperInstance(passwordBox, new PasswordBoxHelper(passwordBox));
				return;
			}
			passwordBox.ClearValue(PasswordBoxHelper.HelperInstanceProperty);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00039EB6 File Offset: 0x000380B6
		public static Visibility GetPlaceholderTextVisibility(Control control)
		{
			return (Visibility)control.GetValue(PasswordBoxHelper.PlaceholderTextVisibilityProperty);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00039EC8 File Offset: 0x000380C8
		private static void SetPlaceholderTextVisibility(Control control, Visibility value)
		{
			control.SetValue(PasswordBoxHelper.PlaceholderTextVisibilityPropertyKey, value);
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00039EDB File Offset: 0x000380DB
		private static PasswordBoxHelper GetHelperInstance(PasswordBox passwordBox)
		{
			return (PasswordBoxHelper)passwordBox.GetValue(PasswordBoxHelper.HelperInstanceProperty);
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00039EED File Offset: 0x000380ED
		private static void SetHelperInstance(PasswordBox passwordBox, PasswordBoxHelper value)
		{
			passwordBox.SetValue(PasswordBoxHelper.HelperInstanceProperty, value);
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00039EFC File Offset: 0x000380FC
		private static void OnHelperInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBoxHelper passwordBoxHelper = e.OldValue as PasswordBoxHelper;
			if (passwordBoxHelper != null)
			{
				passwordBoxHelper.Detach();
			}
			PasswordBoxHelper passwordBoxHelper2 = e.NewValue as PasswordBoxHelper;
			if (passwordBoxHelper2 != null)
			{
				passwordBoxHelper2.Attach();
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x00039F35 File Offset: 0x00038135
		// (set) Token: 0x060013AE RID: 5038 RVA: 0x00039F3D File Offset: 0x0003813D
		private TextBox TextBox { get; set; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x00039F46 File Offset: 0x00038146
		private PasswordRevealMode PasswordRevealMode
		{
			get
			{
				return PasswordBoxHelper.GetPasswordRevealMode(this._passwordBox);
			}
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00039F53 File Offset: 0x00038153
		private static void OnDisabledCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = false;
			e.Handled = true;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00039F64 File Offset: 0x00038164
		private void Attach()
		{
			this._passwordBox.PasswordChanged += new RoutedEventHandler(this.OnPasswordChanged);
			this._passwordBox.GotFocus += new RoutedEventHandler(this.OnGotFocus);
			this._passwordBox.LostFocus += new RoutedEventHandler(this.OnLostFocus);
			if (this._passwordBox.IsLoaded)
			{
				this.OnApplyTemplate();
				return;
			}
			this._passwordBox.Loaded += new RoutedEventHandler(this.OnLoaded);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00039FE4 File Offset: 0x000381E4
		private void Detach()
		{
			this._passwordBox.PasswordChanged -= new RoutedEventHandler(this.OnPasswordChanged);
			this._passwordBox.GotFocus -= new RoutedEventHandler(this.OnGotFocus);
			this._passwordBox.LostFocus -= new RoutedEventHandler(this.OnLostFocus);
			this._passwordBox.Loaded -= new RoutedEventHandler(this.OnLoaded);
			if (this.TextBox != null)
			{
				this.TextBox.CommandBindings.Remove(PasswordBoxHelper.TextBoxCutBinding);
				this.TextBox.CommandBindings.Remove(PasswordBoxHelper.TextBoxCopyBinding);
				this.TextBox.TextChanged -= new TextChangedEventHandler(this.OnTextBoxTextChanged);
				this.TextBox = null;
			}
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0003A09D File Offset: 0x0003829D
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this._passwordBox.Loaded -= new RoutedEventHandler(this.OnLoaded);
			this.OnApplyTemplate();
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0003A0BC File Offset: 0x000382BC
		private void OnApplyTemplate()
		{
			this._passwordBox.ApplyTemplate();
			this.TextBox = this._passwordBox.GetTemplateChild("TextBox");
			if (this.TextBox != null)
			{
				this.TextBox.IsUndoEnabled = false;
				SpellCheck.SetIsEnabled(this.TextBox, false);
				this.TextBox.CommandBindings.Add(PasswordBoxHelper.TextBoxCutBinding);
				this.TextBox.CommandBindings.Add(PasswordBoxHelper.TextBoxCopyBinding);
				this.TextBox.TextChanged += new TextChangedEventHandler(this.OnTextBoxTextChanged);
				this.UpdateTextBox();
			}
			this.UpdateVisualState(false);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0003A15C File Offset: 0x0003835C
		private void OnGotFocus(object sender, RoutedEventArgs e)
		{
			if (this.PasswordRevealMode == PasswordRevealMode.Visible && this.TextBox != null && e.OriginalSource == this._passwordBox)
			{
				this.TextBox.Focus();
				e.Handled = true;
			}
			if (!string.IsNullOrEmpty(this._passwordBox.Password))
			{
				this._hideRevealButton = true;
			}
			this.UpdateVisualState(true);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0003A1BB File Offset: 0x000383BB
		private void OnLostFocus(object sender, RoutedEventArgs e)
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0003A1C4 File Offset: 0x000383C4
		private void OnPasswordChanged(object sender, RoutedEventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(this._passwordBox.Password);
			if (!flag)
			{
				this._hideRevealButton = false;
			}
			PasswordBoxHelper.SetPlaceholderTextVisibility(this._passwordBox, flag ? 2 : 0);
			this.UpdateTextBox();
			this.UpdateVisualState(true);
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0003A20E File Offset: 0x0003840E
		private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			if (this.PasswordRevealMode == PasswordRevealMode.Visible)
			{
				this._passwordBox.Password = ((TextBox)sender).Text;
			}
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0003A22F File Offset: 0x0003842F
		private void UpdateTextBox()
		{
			if (this.TextBox != null)
			{
				this.TextBox.Text = this._passwordBox.Password;
			}
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0003A250 File Offset: 0x00038450
		private void UpdateVisualState(bool useTransitions)
		{
			bool flag = false;
			if (this._passwordBox.IsFocused)
			{
				PasswordRevealMode passwordRevealMode = this.PasswordRevealMode;
				if (passwordRevealMode != PasswordRevealMode.Peek)
				{
					if (passwordRevealMode - PasswordRevealMode.Hidden <= 1)
					{
						flag = false;
					}
				}
				else
				{
					flag = (!this._hideRevealButton && !string.IsNullOrEmpty(this._passwordBox.Password));
				}
			}
			VisualStateManager.GoToState(this._passwordBox, flag ? "ButtonVisible" : "ButtonCollapsed", useTransitions);
		}

		// Token: 0x04000692 RID: 1682
		private const string ButtonStatesGroup = "ButtonStates";

		// Token: 0x04000693 RID: 1683
		private const string ButtonVisibleState = "ButtonVisible";

		// Token: 0x04000694 RID: 1684
		private const string ButtonCollapsedState = "ButtonCollapsed";

		// Token: 0x04000695 RID: 1685
		private static readonly CommandBinding TextBoxCutBinding;

		// Token: 0x04000696 RID: 1686
		private static readonly CommandBinding TextBoxCopyBinding;

		// Token: 0x04000697 RID: 1687
		private readonly PasswordBox _passwordBox;

		// Token: 0x04000698 RID: 1688
		private bool _hideRevealButton;

		// Token: 0x04000699 RID: 1689
		public static readonly DependencyProperty PasswordRevealModeProperty = DependencyProperty.RegisterAttached("PasswordRevealMode", typeof(PasswordRevealMode), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(PasswordRevealMode.Peek, new PropertyChangedCallback(PasswordBoxHelper.OnPasswordRevealModeChanged)));

		// Token: 0x0400069A RID: 1690
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBoxHelper.OnIsEnabledChanged)));

		// Token: 0x0400069B RID: 1691
		private static readonly DependencyPropertyKey PlaceholderTextVisibilityPropertyKey = DependencyProperty.RegisterAttachedReadOnly("PlaceholderTextVisibility", typeof(Visibility), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(0));

		// Token: 0x0400069C RID: 1692
		public static readonly DependencyProperty PlaceholderTextVisibilityProperty = PasswordBoxHelper.PlaceholderTextVisibilityPropertyKey.DependencyProperty;

		// Token: 0x0400069D RID: 1693
		private static readonly DependencyProperty HelperInstanceProperty = DependencyProperty.RegisterAttached("HelperInstance", typeof(PasswordBoxHelper), typeof(PasswordBoxHelper), new PropertyMetadata(new PropertyChangedCallback(PasswordBoxHelper.OnHelperInstanceChanged)));
	}
}
