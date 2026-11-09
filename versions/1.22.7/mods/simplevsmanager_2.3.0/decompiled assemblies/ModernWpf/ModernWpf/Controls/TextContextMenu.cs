using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x0200027E RID: 638
	public class TextContextMenu : ContextMenu
	{
		// Token: 0x0600128C RID: 4748 RVA: 0x00036368 File Offset: 0x00034568
		static TextContextMenu()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextContextMenu), new FrameworkPropertyMetadata(typeof(TextContextMenu)));
			TextContextMenu._selectAllBinding = new CommandBinding(ApplicationCommands.SelectAll);
			TextContextMenu._selectAllBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(TextContextMenu.OnSelectAllPreviewCanExecute);
			TextContextMenu._undoBinding = new CommandBinding(ApplicationCommands.Undo);
			TextContextMenu._undoBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(TextContextMenu.OnUndoRedoPreviewCanExecute);
			TextContextMenu._redoBinding = new CommandBinding(ApplicationCommands.Redo);
			TextContextMenu._redoBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(TextContextMenu.OnUndoRedoPreviewCanExecute);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00036458 File Offset: 0x00034658
		public TextContextMenu()
		{
			this._proofingMenuItem = new MenuItem();
			base.Items.Add(this._proofingMenuItem);
			base.Items.Add(new MenuItem
			{
				Command = ApplicationCommands.Cut,
				Icon = new SymbolIcon(Symbol.Cut)
			});
			base.Items.Add(new MenuItem
			{
				Command = ApplicationCommands.Copy,
				Icon = new SymbolIcon(Symbol.Copy)
			});
			base.Items.Add(new MenuItem
			{
				Command = ApplicationCommands.Paste,
				Icon = new SymbolIcon(Symbol.Paste)
			});
			base.Items.Add(new MenuItem
			{
				Command = ApplicationCommands.Undo,
				Icon = new SymbolIcon(Symbol.Undo)
			});
			base.Items.Add(new MenuItem
			{
				Command = ApplicationCommands.Redo,
				Icon = new SymbolIcon(Symbol.Redo)
			});
			base.Items.Add(new MenuItem
			{
				Command = ApplicationCommands.SelectAll
			});
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00036580 File Offset: 0x00034780
		public static bool GetUsingTextContextMenu(Control textControl)
		{
			return (bool)textControl.GetValue(TextContextMenu.UsingTextContextMenuProperty);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00036592 File Offset: 0x00034792
		public static void SetUsingTextContextMenu(Control textControl, bool value)
		{
			textControl.SetValue(TextContextMenu.UsingTextContextMenuProperty, value);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x000365A8 File Offset: 0x000347A8
		private static void OnUsingTextContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = (Control)d;
			if ((bool)e.NewValue)
			{
				control.CommandBindings.Add(TextContextMenu._selectAllBinding);
				control.CommandBindings.Add(TextContextMenu._undoBinding);
				control.CommandBindings.Add(TextContextMenu._redoBinding);
				control.ContextMenuOpening += new ContextMenuEventHandler(TextContextMenu.OnContextMenuOpening);
				return;
			}
			control.CommandBindings.Remove(TextContextMenu._selectAllBinding);
			control.CommandBindings.Remove(TextContextMenu._undoBinding);
			control.CommandBindings.Remove(TextContextMenu._redoBinding);
			control.ContextMenuOpening -= new ContextMenuEventHandler(TextContextMenu.OnContextMenuOpening);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00036652 File Offset: 0x00034852
		protected override void OnOpened(RoutedEventArgs e)
		{
			base.OnOpened(e);
			if (this._proofingMenuItem.IsVisible)
			{
				this._proofingMenuItem.IsSubmenuOpen = true;
			}
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00036674 File Offset: 0x00034874
		protected override void OnClosed(RoutedEventArgs e)
		{
			base.OnClosed(e);
			if (!base.IsOpen)
			{
				this._proofingMenuItem.Items.Clear();
				foreach (object obj in base.Items)
				{
					((MenuItem)obj).ClearValue(MenuItem.CommandTargetProperty);
				}
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000366F0 File Offset: 0x000348F0
		private static void OnSelectAllPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			if (textBox != null && string.IsNullOrEmpty(textBox.Text))
			{
				e.CanExecute = false;
				e.Handled = true;
				return;
			}
			PasswordBox passwordBox = sender as PasswordBox;
			if (passwordBox != null && string.IsNullOrEmpty(passwordBox.Password))
			{
				e.CanExecute = false;
				e.Handled = true;
			}
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00036748 File Offset: 0x00034948
		private static void OnUndoRedoPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			TextBoxBase textBoxBase = sender as TextBoxBase;
			if (textBoxBase != null && textBoxBase.IsReadOnly)
			{
				e.CanExecute = false;
				e.Handled = true;
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00036778 File Offset: 0x00034978
		private static void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			Control control = (Control)sender;
			TextContextMenu textContextMenu = control.ContextMenu as TextContextMenu;
			if (textContextMenu != null)
			{
				PasswordBox passwordBox = control as PasswordBox;
				Control target;
				if (passwordBox != null && PasswordBoxHelper.GetPasswordRevealMode(passwordBox) == PasswordRevealMode.Visible && e.Source is TextBox)
				{
					target = (Control)e.Source;
				}
				else
				{
					target = control;
				}
				textContextMenu.UpdateItems(target);
				if (!Enumerable.Any<MenuItem>(Enumerable.OfType<MenuItem>(textContextMenu.Items), (MenuItem mi) => mi.Visibility == 0))
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0003680C File Offset: 0x00034A0C
		private void UpdateProofingMenuItem(Control target)
		{
			this._proofingMenuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("ProofingMenuItemLabel");
			this._proofingMenuItem.Items.Clear();
			SpellingError spellingError = null;
			TextBox textBox = target as TextBox;
			if (textBox != null)
			{
				spellingError = textBox.GetSpellingError(textBox.CaretIndex);
			}
			else
			{
				RichTextBox richTextBox = target as RichTextBox;
				if (richTextBox != null)
				{
					spellingError = richTextBox.GetSpellingError(richTextBox.CaretPosition);
				}
			}
			if (spellingError != null)
			{
				foreach (string text in spellingError.Suggestions)
				{
					MenuItem menuItem = new MenuItem
					{
						Header = text,
						Command = EditingCommands.CorrectSpellingError,
						CommandParameter = text,
						CommandTarget = target
					};
					this._proofingMenuItem.Items.Add(menuItem);
				}
				if (this._proofingMenuItem.HasItems)
				{
					this._proofingMenuItem.Items.Add(new Separator());
				}
				this._proofingMenuItem.Items.Add(new MenuItem
				{
					Header = Strings.IgnoreMenuItemLabel,
					Command = EditingCommands.IgnoreSpellingError,
					CommandTarget = target
				});
				this._proofingMenuItem.Visibility = 0;
				return;
			}
			this._proofingMenuItem.Visibility = 2;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00036960 File Offset: 0x00034B60
		private void UpdateItems(Control target)
		{
			this.UpdateProofingMenuItem(target);
			foreach (object obj in base.Items)
			{
				MenuItem menuItem = (MenuItem)obj;
				RoutedUICommand routedUICommand = menuItem.Command as RoutedUICommand;
				if (routedUICommand != null)
				{
					if (routedUICommand == ApplicationCommands.Cut)
					{
						menuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("TextCommandLabelCut");
					}
					else if (routedUICommand == ApplicationCommands.Copy)
					{
						menuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("TextCommandLabelCopy");
					}
					else if (routedUICommand == ApplicationCommands.Paste)
					{
						menuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("TextCommandLabelPaste");
					}
					else if (routedUICommand == ApplicationCommands.Undo)
					{
						menuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("TextCommandLabelUndo");
					}
					else if (routedUICommand == ApplicationCommands.Redo)
					{
						menuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("TextCommandLabelRedo");
					}
					else if (routedUICommand == ApplicationCommands.SelectAll)
					{
						menuItem.Header = TextContextMenu.ResourceAccessor.GetLocalizedStringResource("TextCommandLabelSelectAll");
					}
					menuItem.CommandTarget = target;
					menuItem.Visibility = (routedUICommand.CanExecute(null, target) ? 0 : 2);
				}
			}
		}

		// Token: 0x04000604 RID: 1540
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(TextContextMenu));

		// Token: 0x04000605 RID: 1541
		private static readonly CommandBinding _selectAllBinding;

		// Token: 0x04000606 RID: 1542
		private static readonly CommandBinding _undoBinding;

		// Token: 0x04000607 RID: 1543
		private static readonly CommandBinding _redoBinding;

		// Token: 0x04000608 RID: 1544
		private readonly MenuItem _proofingMenuItem;

		// Token: 0x04000609 RID: 1545
		public static readonly DependencyProperty UsingTextContextMenuProperty = DependencyProperty.RegisterAttached("UsingTextContextMenu", typeof(bool), typeof(TextContextMenu), new PropertyMetadata(false, new PropertyChangedCallback(TextContextMenu.OnUsingTextContextMenuChanged)));
	}
}
