using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000014 RID: 20
	public sealed class AutoSuggestBox : ItemsControl
	{
		// Token: 0x06000092 RID: 146 RVA: 0x000030EC File Offset: 0x000012EC
		static AutoSuggestBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoSuggestBox), new FrameworkPropertyMetadata(typeof(AutoSuggestBox)));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003304 File Offset: 0x00001504
		public AutoSuggestBox()
		{
			this.m_delayTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(150.0)
			};
			this.m_delayTimer.Tick += new EventHandler(this.OnDelayTimerTick);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003358 File Offset: 0x00001558
		public override void OnApplyTemplate()
		{
			if (this.m_textBox != null)
			{
				this.m_textBox.TextChanged -= new TextChangedEventHandler(this.OnTextBoxTextChanged);
				this.m_textBox.PreviewKeyDown -= new KeyEventHandler(this.OnTextBoxPreviewKeyDown);
			}
			if (this.m_queryButton != null)
			{
				this.m_queryButton.Click -= new RoutedEventHandler(this.OnQueryButtonClick);
				this.m_queryButton.ClearValue(ContentControl.ContentProperty);
				this.m_queryButton = null;
			}
			if (this.m_suggestionsPopup != null)
			{
				this.m_suggestionsPopup.Opened -= new EventHandler(this.OnSuggestionsPopupOpened);
				this.m_suggestionsPopup.Closed -= new EventHandler(this.OnSuggestionsPopupClosed);
				this.m_suggestionsPopup.ClearValue(Popup.PlacementTargetProperty);
			}
			if (this.m_popupRepositionHelper != null)
			{
				this.m_popupRepositionHelper.Dispose();
				this.m_popupRepositionHelper = null;
			}
			if (this.m_suggestionsList != null)
			{
				this.m_suggestionsList.Loaded -= new RoutedEventHandler(this.OnSuggestionsListLoaded);
				this.m_suggestionsList.SelectionChanged -= new SelectionChangedEventHandler(this.OnSuggestionsListSelectionChanged);
				this.m_suggestionsList.ItemClick -= this.OnSuggestionsListItemClick;
			}
			base.OnApplyTemplate();
			this.m_textBox = (base.GetTemplateChild("TextBox") as TextBox);
			this.m_suggestionsPopup = (base.GetTemplateChild("SuggestionsPopup") as Popup);
			this.m_suggestionsList = (base.GetTemplateChild("SuggestionsList") as AutoSuggestBoxListView);
			if (this.m_textBox != null)
			{
				this.m_textBox.ApplyTemplate();
				this.m_queryButton = ControlExtensions.GetTemplateChild<Button>(this.m_textBox, "QueryButton");
				this.m_textBox.TextChanged += new TextChangedEventHandler(this.OnTextBoxTextChanged);
				this.m_textBox.PreviewKeyDown += new KeyEventHandler(this.OnTextBoxPreviewKeyDown);
				this.UpdateTextBox();
			}
			if (this.m_queryButton != null)
			{
				this.m_queryButton.Click += new RoutedEventHandler(this.OnQueryButtonClick);
				this.OnQueryIconChanged(null, this.QueryIcon);
			}
			if (this.m_suggestionsPopup != null)
			{
				this.m_suggestionsPopup.Opened += new EventHandler(this.OnSuggestionsPopupOpened);
				this.m_suggestionsPopup.Closed += new EventHandler(this.OnSuggestionsPopupClosed);
				this.m_popupRepositionHelper = new PopupRepositionHelper(this.m_suggestionsPopup, this);
				if (this.m_textBox != null)
				{
					FrameworkElement templateChild = ControlExtensions.GetTemplateChild<FrameworkElement>(this.m_textBox, "BorderElement");
					if (templateChild != null)
					{
						this.m_suggestionsPopup.PlacementTarget = templateChild;
					}
				}
			}
			if (this.m_suggestionsList != null)
			{
				this.m_suggestionsList.Loaded += new RoutedEventHandler(this.OnSuggestionsListLoaded);
				this.m_suggestionsList.SelectionChanged += new SelectionChangedEventHandler(this.OnSuggestionsListSelectionChanged);
				this.m_suggestionsList.ItemClick += this.OnSuggestionsListItemClick;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000360D File Offset: 0x0000180D
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			this.OpenOrCloseSuggestionListIfFocused();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000361C File Offset: 0x0000181C
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			this.ClearSelection();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000362C File Offset: 0x0000182C
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			TextBox textBox = this.m_textBox;
			if (textBox == null)
			{
				return;
			}
			textBox.Focus();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003646 File Offset: 0x00001846
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (!(bool)e.NewValue)
			{
				this.CloseSuggestionList();
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003664 File Offset: 0x00001864
		private void OnTextChanged(DependencyPropertyChangedEventArgs args)
		{
			this.m_delayTimer.Stop();
			this.m_delayTimer.Tag = null;
			this.OpenOrCloseSuggestionListIfFocused();
			AutoSuggestionBoxTextChangeReason? textChangeReason = this.m_textChangeReason;
			AutoSuggestionBoxTextChangeReason autoSuggestionBoxTextChangeReason = AutoSuggestionBoxTextChangeReason.SuggestionChosen;
			if (!(textChangeReason.GetValueOrDefault() == autoSuggestionBoxTextChangeReason & textChangeReason != null))
			{
				this.UpdateSearchText((string)args.NewValue);
			}
			this.UpdateTextBox();
			this.m_delayTimer.Tag = (this.m_textChangeReason ?? AutoSuggestionBoxTextChangeReason.ProgrammaticChange);
			this.m_delayTimer.Start();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000036F8 File Offset: 0x000018F8
		private void OnIsSuggestionListOpenChanged(DependencyPropertyChangedEventArgs args)
		{
			if (!(bool)args.NewValue)
			{
				this.UpdateSearchText(this.Text);
				this.ClearSelection();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000371A File Offset: 0x0000191A
		private void OnQueryIconChanged(DependencyPropertyChangedEventArgs args)
		{
			this.OnQueryIconChanged(args.OldValue as IconElement, args.NewValue as IconElement);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000373C File Offset: 0x0000193C
		private void OnQueryIconChanged(IconElement oldQueryIcon, IconElement newQueryIcon)
		{
			if (oldQueryIcon != null)
			{
				oldQueryIcon.ClearValue(IconElement.ForegroundProperty);
				if (newQueryIcon is SymbolIcon)
				{
					oldQueryIcon.ClearValue(SymbolIcon.FontSizeProperty);
				}
			}
			if (newQueryIcon != null && this.m_queryButton != null && newQueryIcon is SymbolIcon)
			{
				newQueryIcon.SetBinding(SymbolIcon.FontSizeProperty, new Binding
				{
					Path = new PropertyPath(TextElement.FontSizeProperty),
					RelativeSource = new RelativeSource
					{
						AncestorType = typeof(ContentPresenter)
					}
				});
			}
			this.UpdateQueryButton();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000037BF File Offset: 0x000019BF
		private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			if (this.m_ignoreTextBoxTextChange)
			{
				return;
			}
			this.UpdateTextValue(this.m_textBox.Text, AutoSuggestionBoxTextChangeReason.UserInput);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000037DC File Offset: 0x000019DC
		private void OnTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (this.IsSuggestionListOpen)
			{
				Key key = e.Key;
				if (key <= 13)
				{
					if (key != 6)
					{
						if (key != 13)
						{
							return;
						}
						if (this.IsSuggestionListOpen)
						{
							this.UpdateTextValue(this.m_searchText, AutoSuggestionBoxTextChangeReason.ProgrammaticChange);
							this.TryMoveCaretToEnd();
							this.CloseSuggestionList();
							e.Handled = true;
							return;
						}
					}
					else if (this.TryCommitChosenSuggestion() || this.TryCommitTextBoxText())
					{
						e.Handled = true;
						return;
					}
				}
				else
				{
					if (key == 24)
					{
						this.SelectedIndexDecrement();
						e.Handled = true;
						return;
					}
					if (key != 26)
					{
						return;
					}
					if ((1 & Keyboard.Modifiers) == null)
					{
						if (!this.TryMoveCaretToEnd())
						{
							this.SelectedIndexIncrement();
						}
						e.Handled = true;
						return;
					}
				}
			}
			else if (e.Key == 6 && this.TryCommitTextBoxText())
			{
				e.Handled = true;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000389C File Offset: 0x00001A9C
		private void OnQueryButtonClick(object sender, RoutedEventArgs e)
		{
			this.TryCommitTextBoxText();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000038A5 File Offset: 0x00001AA5
		private void OnSuggestionsPopupOpened(object sender, EventArgs e)
		{
			this.UpdateCornerRadius(true);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000038AE File Offset: 0x00001AAE
		private void OnSuggestionsPopupClosed(object sender, EventArgs e)
		{
			this.UpdateCornerRadius(false);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000038B7 File Offset: 0x00001AB7
		private void OnSuggestionsListLoaded(object sender, RoutedEventArgs e)
		{
			this.ClearSelection();
			this.m_suggestionsList.ScrollToTop();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000038CC File Offset: 0x00001ACC
		private void OnSuggestionsListSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.m_ignoreSelectionChange)
			{
				return;
			}
			if (this.IsSuggestionListOpen)
			{
				object selectedItem = this.m_suggestionsList.SelectedItem;
				if (selectedItem != null)
				{
					this.m_suggestionsList.ScrollIntoView(selectedItem);
					TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> suggestionChosen = this.SuggestionChosen;
					if (suggestionChosen != null)
					{
						suggestionChosen.Invoke(this, new AutoSuggestBoxSuggestionChosenEventArgs
						{
							SelectedItem = selectedItem
						});
					}
					if (this.UpdateTextOnSelect)
					{
						object selectedValue = this.m_suggestionsList.SelectedValue;
						if (selectedValue != null)
						{
							this.UpdateTextValue(selectedValue.ToString(), AutoSuggestionBoxTextChangeReason.SuggestionChosen);
						}
					}
				}
				else
				{
					this.m_suggestionsList.ScrollToTop();
					this.UpdateTextValue(this.m_searchText, AutoSuggestionBoxTextChangeReason.ProgrammaticChange);
				}
				if (this.m_textBox != null)
				{
					this.m_textBox.CaretIndex = this.m_textBox.Text.Length;
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003987 File Offset: 0x00001B87
		private void OnSuggestionsListItemClick(object sender, ItemClickEventArgs e)
		{
			this.m_suggestionsList.SelectedItem = e.ClickedItem;
			this.TryCommitChosenSuggestion();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000039A4 File Offset: 0x00001BA4
		private void OnDelayTimerTick(object sender, EventArgs e)
		{
			this.m_delayTimer.Stop();
			object tag = this.m_delayTimer.Tag;
			if (tag is AutoSuggestionBoxTextChangeReason)
			{
				AutoSuggestionBoxTextChangeReason reason = (AutoSuggestionBoxTextChangeReason)tag;
				this.m_delayTimer.Tag = null;
				TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> textChanged = this.TextChanged;
				if (textChanged == null)
				{
					return;
				}
				textChanged.Invoke(this, new AutoSuggestBoxTextChangedEventArgs(this, this.Text, reason));
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003A01 File Offset: 0x00001C01
		private void UpdateTextValue(string value, AutoSuggestionBoxTextChangeReason reason = AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
		{
			if (this.Text != value)
			{
				this.m_textChangeReason = new AutoSuggestionBoxTextChangeReason?(reason);
				base.SetCurrentValue(AutoSuggestBox.TextProperty, value);
				this.m_textChangeReason = default(AutoSuggestionBoxTextChangeReason?);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003A35 File Offset: 0x00001C35
		private void UpdateSearchText(string value)
		{
			if (this.m_searchText != value)
			{
				this.m_searchText = value;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003A4C File Offset: 0x00001C4C
		private void UpdateTextBox()
		{
			if (this.m_textBox != null)
			{
				string text = this.Text;
				if (this.m_textBox.Text != text)
				{
					this.m_ignoreTextBoxTextChange = true;
					this.m_textBox.Text = text;
					this.m_ignoreTextBoxTextChange = false;
				}
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003A98 File Offset: 0x00001C98
		private void UpdateQueryButton()
		{
			if (this.m_queryButton != null)
			{
				IconElement queryIcon = this.QueryIcon;
				this.m_queryButton.Content = queryIcon;
				this.m_queryButton.Visibility = ((queryIcon != null) ? 0 : 2);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003AD2 File Offset: 0x00001CD2
		private void OpenSuggestionList()
		{
			if (!this.IsSuggestionListOpen)
			{
				base.SetCurrentValue(AutoSuggestBox.IsSuggestionListOpenProperty, true);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003AED File Offset: 0x00001CED
		private void CloseSuggestionList()
		{
			if (this.IsSuggestionListOpen)
			{
				base.SetCurrentValue(AutoSuggestBox.IsSuggestionListOpenProperty, false);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003B08 File Offset: 0x00001D08
		private void OpenOrCloseSuggestionListIfFocused()
		{
			if (base.IsKeyboardFocusWithin)
			{
				if (base.HasItems)
				{
					this.OpenSuggestionList();
					return;
				}
				this.CloseSuggestionList();
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003B28 File Offset: 0x00001D28
		private void SelectedIndexIncrement()
		{
			if (this.m_suggestionsList != null)
			{
				int selectedIndex = this.m_suggestionsList.SelectedIndex;
				this.m_suggestionsList.SelectedIndex = ((selectedIndex + 1 >= this.m_suggestionsList.Items.Count) ? -1 : (selectedIndex + 1));
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003B70 File Offset: 0x00001D70
		private void SelectedIndexDecrement()
		{
			if (this.m_suggestionsList != null)
			{
				int selectedIndex = this.m_suggestionsList.SelectedIndex;
				if (selectedIndex >= 0)
				{
					AutoSuggestBoxListView suggestionsList = this.m_suggestionsList;
					int selectedIndex2 = suggestionsList.SelectedIndex;
					suggestionsList.SelectedIndex = selectedIndex2 - 1;
					return;
				}
				if (selectedIndex == -1)
				{
					this.m_suggestionsList.SelectedIndex = this.m_suggestionsList.Items.Count - 1;
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003BCC File Offset: 0x00001DCC
		private void ClearSelection()
		{
			if (this.m_suggestionsList != null)
			{
				this.m_ignoreSelectionChange = true;
				this.m_suggestionsList.ClearValue(Selector.SelectedItemProperty);
				this.m_suggestionsList.ClearValue(Selector.SelectedIndexProperty);
				this.m_ignoreSelectionChange = false;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003C04 File Offset: 0x00001E04
		private bool TryCommitChosenSuggestion()
		{
			if (this.IsSuggestionListOpen && this.m_textBox != null && this.m_suggestionsList != null)
			{
				object selectedItem = this.m_suggestionsList.SelectedItem;
				if (selectedItem != null)
				{
					this.CloseSuggestionList();
					this.SubmitQuery(this.m_textBox.Text, selectedItem);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003C53 File Offset: 0x00001E53
		private bool TryCommitTextBoxText()
		{
			if (this.m_textBox != null)
			{
				bool isSuggestionListOpen = this.IsSuggestionListOpen;
				this.SubmitQuery(this.m_textBox.Text, null);
				if (isSuggestionListOpen)
				{
					this.CloseSuggestionList();
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003C80 File Offset: 0x00001E80
		private void SubmitQuery(string queryText, object chosenSuggestion)
		{
			TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> querySubmitted = this.QuerySubmitted;
			if (querySubmitted == null)
			{
				return;
			}
			querySubmitted.Invoke(this, new AutoSuggestBoxQuerySubmittedEventArgs
			{
				QueryText = queryText,
				ChosenSuggestion = chosenSuggestion
			});
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003CA8 File Offset: 0x00001EA8
		private bool TryMoveCaretToEnd()
		{
			if (this.m_textBox != null)
			{
				int length = this.m_textBox.Text.Length;
				if (this.m_textBox.CaretIndex < length)
				{
					this.m_textBox.CaretIndex = length;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003CEC File Offset: 0x00001EEC
		private void UpdateCornerRadius(bool isPopupOpen)
		{
			CornerRadius cornerRadius = this.CornerRadius;
			CornerRadius cornerRadius2 = (CornerRadius)this.ResourceLookup("OverlayCornerRadius");
			if (isPopupOpen)
			{
				bool flag = this.IsPopupOpenDown();
				CornerRadiusFilterConverter cornerRadiusFilterConverter = new CornerRadiusFilterConverter();
				CornerRadiusFilterKind cornerRadiusFilterKind = flag ? 3 : 1;
				cornerRadius2 = cornerRadiusFilterConverter.Convert(cornerRadius2, cornerRadiusFilterKind);
				CornerRadiusFilterKind cornerRadiusFilterKind2 = flag ? 1 : 3;
				cornerRadius = cornerRadiusFilterConverter.Convert(cornerRadius, cornerRadiusFilterKind2);
			}
			Border border = base.GetTemplateChild("SuggestionsContainer") as Border;
			if (border != null)
			{
				border.CornerRadius = cornerRadius2;
			}
			TextBox textBox = base.GetTemplateChild("TextBox") as TextBox;
			if (textBox != null)
			{
				ControlHelper.SetCornerRadius(textBox, cornerRadius);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003D80 File Offset: 0x00001F80
		private bool IsPopupOpenDown()
		{
			double num = 0.0;
			Border border = base.GetTemplateChild("SuggestionsContainer") as Border;
			if (border != null)
			{
				TextBox textBox = base.GetTemplateChild("TextBox") as TextBox;
				if (textBox != null)
				{
					num = border.TranslatePoint(new Point(0.0, 0.0), textBox).Y;
				}
			}
			return num >= 0.0;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003DF5 File Offset: 0x00001FF5
		private object ResourceLookup(object key)
		{
			return base.TryFindResource(key);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003DFE File Offset: 0x00001FFE
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003E10 File Offset: 0x00002010
		public bool UpdateTextOnSelect
		{
			get
			{
				return (bool)base.GetValue(AutoSuggestBox.UpdateTextOnSelectProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.UpdateTextOnSelectProperty, value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003E23 File Offset: 0x00002023
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003E35 File Offset: 0x00002035
		public string TextMemberPath
		{
			get
			{
				return (string)base.GetValue(AutoSuggestBox.TextMemberPathProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.TextMemberPathProperty, value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003E43 File Offset: 0x00002043
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00003E55 File Offset: 0x00002055
		public Style TextBoxStyle
		{
			get
			{
				return (Style)base.GetValue(AutoSuggestBox.TextBoxStyleProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.TextBoxStyleProperty, value);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003E63 File Offset: 0x00002063
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003E75 File Offset: 0x00002075
		public string Text
		{
			get
			{
				return (string)base.GetValue(AutoSuggestBox.TextProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.TextProperty, value);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003E83 File Offset: 0x00002083
		private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((AutoSuggestBox)sender).OnTextChanged(args);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003E91 File Offset: 0x00002091
		private static object CoerceText(DependencyObject d, object baseValue)
		{
			return baseValue ?? string.Empty;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003E9D File Offset: 0x0000209D
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003EAF File Offset: 0x000020AF
		public string PlaceholderText
		{
			get
			{
				return (string)base.GetValue(AutoSuggestBox.PlaceholderTextProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.PlaceholderTextProperty, value);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003EBD File Offset: 0x000020BD
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003ECF File Offset: 0x000020CF
		public double MaxSuggestionListHeight
		{
			get
			{
				return (double)base.GetValue(AutoSuggestBox.MaxSuggestionListHeightProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.MaxSuggestionListHeightProperty, value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003EE2 File Offset: 0x000020E2
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003EF4 File Offset: 0x000020F4
		public bool IsSuggestionListOpen
		{
			get
			{
				return (bool)base.GetValue(AutoSuggestBox.IsSuggestionListOpenProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.IsSuggestionListOpenProperty, value);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003F07 File Offset: 0x00002107
		private static void OnIsSuggestionListOpenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((AutoSuggestBox)sender).OnIsSuggestionListOpenChanged(args);
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003F15 File Offset: 0x00002115
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00003F22 File Offset: 0x00002122
		public object Header
		{
			get
			{
				return base.GetValue(AutoSuggestBox.HeaderProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.HeaderProperty, value);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003F30 File Offset: 0x00002130
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003F42 File Offset: 0x00002142
		public IconElement QueryIcon
		{
			get
			{
				return (IconElement)base.GetValue(AutoSuggestBox.QueryIconProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.QueryIconProperty, value);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003F50 File Offset: 0x00002150
		private static void OnQueryIconPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((AutoSuggestBox)sender).OnQueryIconChanged(args);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003F5E File Offset: 0x0000215E
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00003F6B File Offset: 0x0000216B
		public object Description
		{
			get
			{
				return base.GetValue(AutoSuggestBox.DescriptionProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.DescriptionProperty, value);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003F79 File Offset: 0x00002179
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00003F8B File Offset: 0x0000218B
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(AutoSuggestBox.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003F9E File Offset: 0x0000219E
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003FB0 File Offset: 0x000021B0
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(AutoSuggestBox.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(AutoSuggestBox.CornerRadiusProperty, value);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000D3 RID: 211 RVA: 0x00003FC4 File Offset: 0x000021C4
		// (remove) Token: 0x060000D4 RID: 212 RVA: 0x00003FFC File Offset: 0x000021FC
		public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000D5 RID: 213 RVA: 0x00004034 File Offset: 0x00002234
		// (remove) Token: 0x060000D6 RID: 214 RVA: 0x0000406C File Offset: 0x0000226C
		public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> TextChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000D7 RID: 215 RVA: 0x000040A4 File Offset: 0x000022A4
		// (remove) Token: 0x060000D8 RID: 216 RVA: 0x000040DC File Offset: 0x000022DC
		public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted;

		// Token: 0x04000008 RID: 8
		private const string c_popupName = "SuggestionsPopup";

		// Token: 0x04000009 RID: 9
		private const string c_popupBorderName = "SuggestionsContainer";

		// Token: 0x0400000A RID: 10
		private const string c_textBoxName = "TextBox";

		// Token: 0x0400000B RID: 11
		private const string c_textBoxBorderName = "BorderElement";

		// Token: 0x0400000C RID: 12
		private const string c_controlCornerRadiusKey = "ControlCornerRadius";

		// Token: 0x0400000D RID: 13
		private const string c_overlayCornerRadiusKey = "OverlayCornerRadius";

		// Token: 0x0400000E RID: 14
		private TextBox m_textBox;

		// Token: 0x0400000F RID: 15
		private Button m_queryButton;

		// Token: 0x04000010 RID: 16
		private Popup m_suggestionsPopup;

		// Token: 0x04000011 RID: 17
		private AutoSuggestBoxListView m_suggestionsList;

		// Token: 0x04000012 RID: 18
		private PopupRepositionHelper m_popupRepositionHelper;

		// Token: 0x04000013 RID: 19
		private string m_searchText = string.Empty;

		// Token: 0x04000014 RID: 20
		private readonly DispatcherTimer m_delayTimer;

		// Token: 0x04000015 RID: 21
		private AutoSuggestionBoxTextChangeReason? m_textChangeReason;

		// Token: 0x04000016 RID: 22
		private bool m_ignoreTextBoxTextChange;

		// Token: 0x04000017 RID: 23
		private bool m_ignoreSelectionChange;

		// Token: 0x04000018 RID: 24
		public static readonly DependencyProperty UpdateTextOnSelectProperty = DependencyProperty.Register("UpdateTextOnSelect", typeof(bool), typeof(AutoSuggestBox), new PropertyMetadata(true));

		// Token: 0x04000019 RID: 25
		public static readonly DependencyProperty TextMemberPathProperty = DependencyProperty.Register("TextMemberPath", typeof(string), typeof(AutoSuggestBox), new PropertyMetadata(string.Empty));

		// Token: 0x0400001A RID: 26
		public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register("TextBoxStyle", typeof(Style), typeof(AutoSuggestBox), null);

		// Token: 0x0400001B RID: 27
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoSuggestBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(AutoSuggestBox.OnTextPropertyChanged), new CoerceValueCallback(AutoSuggestBox.CoerceText)));

		// Token: 0x0400001C RID: 28
		public static readonly DependencyProperty PlaceholderTextProperty = ControlHelper.PlaceholderTextProperty.AddOwner(typeof(AutoSuggestBox));

		// Token: 0x0400001D RID: 29
		public static readonly DependencyProperty MaxSuggestionListHeightProperty = DependencyProperty.Register("MaxSuggestionListHeight", typeof(double), typeof(AutoSuggestBox), new PropertyMetadata(double.PositiveInfinity));

		// Token: 0x0400001E RID: 30
		public static readonly DependencyProperty IsSuggestionListOpenProperty = DependencyProperty.Register("IsSuggestionListOpen", typeof(bool), typeof(AutoSuggestBox), new PropertyMetadata(false, new PropertyChangedCallback(AutoSuggestBox.OnIsSuggestionListOpenPropertyChanged)));

		// Token: 0x0400001F RID: 31
		public static readonly DependencyProperty HeaderProperty = ControlHelper.HeaderProperty.AddOwner(typeof(AutoSuggestBox));

		// Token: 0x04000020 RID: 32
		public static readonly DependencyProperty QueryIconProperty = DependencyProperty.Register("QueryIcon", typeof(IconElement), typeof(AutoSuggestBox), new PropertyMetadata(null, new PropertyChangedCallback(AutoSuggestBox.OnQueryIconPropertyChanged)));

		// Token: 0x04000021 RID: 33
		public static readonly DependencyProperty DescriptionProperty = ControlHelper.DescriptionProperty.AddOwner(typeof(AutoSuggestBox));

		// Token: 0x04000022 RID: 34
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(AutoSuggestBox));

		// Token: 0x04000023 RID: 35
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(AutoSuggestBox));
	}
}
