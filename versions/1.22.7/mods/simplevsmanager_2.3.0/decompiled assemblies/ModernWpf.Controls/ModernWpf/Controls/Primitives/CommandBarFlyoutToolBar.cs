using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E6 RID: 230
	public class CommandBarFlyoutToolBar : CommandBarToolBar
	{
		// Token: 0x06000B9A RID: 2970 RVA: 0x0002719C File Offset: 0x0002539C
		static CommandBarFlyoutToolBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBarFlyoutToolBar), new FrameworkPropertyMetadata(typeof(CommandBarFlyoutToolBar)));
			ToolBar.IsOverflowOpenProperty.OverrideMetadata(typeof(CommandBarFlyoutToolBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(CommandBarFlyoutToolBar.OnOverflowOpenChanged)));
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00027224 File Offset: 0x00025424
		public CommandBarFlyoutToolBar()
		{
			base.SetValue(CommandBarFlyoutToolBar.FlyoutTemplateSettingsPropertyKey, new CommandBarFlyoutCommandBarTemplateSettings());
			base.Loaded += delegate(object <p0>, RoutedEventArgs <p1>)
			{
				this.UpdateUI(true);
				ObservableCollection<ICommandBarElement> commands = (this.PrimaryCommands.Count > 0) ? this.PrimaryCommands : ((this.SecondaryCommands.Count > 0) ? this.SecondaryCommands : null);
				if (commands != null)
				{
					bool usingPrimaryCommands = commands == this.PrimaryCommands;
					bool ensureTabStopUniqueness = usingPrimaryCommands || true;
					FrameworkElement frameworkElement = commands[0] as FrameworkElement;
					if (frameworkElement != null)
					{
						if (SharedHelpers.IsFrameworkElementLoaded(frameworkElement))
						{
							this.FocusCommand(commands, usingPrimaryCommands ? this.m_moreButton : null, true, ensureTabStopUniqueness);
							return;
						}
						this.m_firstItemLoadedRevoker = new RoutedEventHandlerRevoker(frameworkElement, FrameworkElement.LoadedEvent, delegate
						{
							this.FocusCommand(commands, usingPrimaryCommands ? this.m_moreButton : null, true, ensureTabStopUniqueness);
							RoutedEventHandlerRevoker firstItemLoadedRevoker = this.m_firstItemLoadedRevoker;
							if (firstItemLoadedRevoker == null)
							{
								return;
							}
							firstItemLoadedRevoker.Revoke();
						});
					}
				}
			};
			base.Unloaded += delegate(object <p0>, RoutedEventArgs <p1>)
			{
				this.StopOpenAnimation();
				this.SetOpacity(1.0);
			};
			base.SizeChanged += delegate(object <p0>, SizeChangedEventArgs <p1>)
			{
				this.UpdateUI(true);
			};
			base.OverflowOpened += delegate([Nullable(2)] object <p0>, EventArgs <p1>)
			{
				this.m_secondaryItemsRootSized = true;
				this.UpdateFlowsFromAndFlowsTo();
				this.UpdateUI(true);
			};
			base.OverflowClosed += delegate([Nullable(2)] object <p0>, EventArgs <p1>)
			{
				this.m_secondaryItemsRootSized = false;
				if (this.PrimaryCommands.Count > 0)
				{
					this.EnsureFocusedPrimaryCommand();
				}
			};
			base.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(this.OnMouseDown), true);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000272B9 File Offset: 0x000254B9
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.PrimaryCommands.CollectionChanged += delegate([Nullable(2)] object <p0>, NotifyCollectionChangedEventArgs <p1>)
			{
				this.UpdateFlowsFromAndFlowsTo();
				this.UpdateUI(true);
			};
			this.SecondaryCommands.CollectionChanged += delegate([Nullable(2)] object <p0>, NotifyCollectionChangedEventArgs <p1>)
			{
				this.m_secondaryItemsRootSized = false;
				this.UpdateFlowsFromAndFlowsTo();
				this.UpdateUI(true);
			};
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x000272F0 File Offset: 0x000254F0
		public CommandBarFlyoutCommandBarTemplateSettings FlyoutTemplateSettings
		{
			get
			{
				return (CommandBarFlyoutCommandBarTemplateSettings)base.GetValue(CommandBarFlyoutToolBar.FlyoutTemplateSettingsProperty);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00027302 File Offset: 0x00025502
		private ObservableCollection<ICommandBarElement> PrimaryCommands
		{
			get
			{
				CommandBar commandBar = base.TemplatedParent as CommandBar;
				if (commandBar == null)
				{
					return null;
				}
				return commandBar.PrimaryCommands;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0002731A File Offset: 0x0002551A
		private ObservableCollection<ICommandBarElement> SecondaryCommands
		{
			get
			{
				CommandBar commandBar = base.TemplatedParent as CommandBar;
				if (commandBar == null)
				{
					return null;
				}
				return commandBar.SecondaryCommands;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00027332 File Offset: 0x00025532
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x0002733A File Offset: 0x0002553A
		private bool IsOpen
		{
			get
			{
				return base.IsOverflowOpen;
			}
			set
			{
				base.IsOverflowOpen = value;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00027343 File Offset: 0x00025543
		private WeakReference<CommandBarFlyout> OwningFlyout
		{
			get
			{
				CommandBarFlyoutCommandBar commandBarFlyoutCommandBar = base.TemplatedParent as CommandBarFlyoutCommandBar;
				if (commandBarFlyoutCommandBar == null)
				{
					return null;
				}
				return commandBarFlyoutCommandBar.OwningFlyout;
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002735C File Offset: 0x0002555C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.DetachEventHandlers();
			this.m_layoutRoot = (base.GetTemplateChild("LayoutRoot") as FrameworkElement);
			this.m_primaryItemsRoot = (base.GetTemplateChild("PrimaryItemsRoot") as FrameworkElement);
			this.m_secondaryItemsRoot = (base.GetTemplateChild("OverflowContentRoot") as FrameworkElement);
			this.m_moreButton = (base.GetTemplateChild("MoreButton") as ButtonBase);
			if (this.m_layoutRoot != null)
			{
				this.m_openingStoryboard = (this.m_layoutRoot.Resources["OpeningStoryboard"] as Storyboard);
				this.m_closingStoryboard = (this.m_layoutRoot.Resources["ClosingStoryboard"] as Storyboard);
			}
			if (this.m_moreButton != null && this.m_moreButton.IsTabStop)
			{
				this.m_moreButton.IsTabStop = false;
			}
			PopupEx popupEx = base.OverflowPopup as PopupEx;
			if (popupEx != null)
			{
				popupEx.SuppressFadeAnimation = true;
			}
			this.AttachEventHandlers();
			this.UpdateFlowsFromAndFlowsTo();
			this.UpdateUI(false);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00027460 File Offset: 0x00025660
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			CommandBarFlyout commandBarFlyout;
			if (!(bool)e.NewValue && this.TryGetOwningFlyout(out commandBarFlyout) && commandBarFlyout.IsOpen)
			{
				this.MoveFocus(new TraversalRequest(0));
			}
			base.OnIsKeyboardFocusWithinChanged(e);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000274A1 File Offset: 0x000256A1
		private static void OnOverflowOpenChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			((CommandBarFlyoutToolBar)element).OnOverflowOpenChanged(e);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x000274AF File Offset: 0x000256AF
		private void OnOverflowOpenChanged(DependencyPropertyChangedEventArgs e)
		{
			this.UpdateFlowsFromAndFlowsTo();
			this.UpdateUI(true);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000274C0 File Offset: 0x000256C0
		private void AttachEventHandlers()
		{
			if (this.m_secondaryItemsRoot != null)
			{
				this.m_secondaryItemsRoot.SizeChanged += new SizeChangedEventHandler(this.SecondaryItemsRootSizeChanged);
				this.m_secondaryItemsRoot.PreviewKeyDown += new KeyEventHandler(this.SecondaryItemsRootPreviewKeyDown);
			}
			if (this.m_openingStoryboard != null)
			{
				this.m_openingStoryboard.Completed += new EventHandler(this.OpeningStoryboardCompleted);
				this.m_openingStoryboard.CurrentStateInvalidated += new EventHandler(this.OpeningStoryboardCurrentStateInvalidated);
			}
			if (this.m_closingStoryboard != null)
			{
				this.m_closingStoryboard.Completed += new EventHandler(this.ClosingStoryboardCompleted);
				this.m_closingStoryboard.CurrentStateInvalidated += new EventHandler(this.ClosingStoryboardCurrentStateInvalidated);
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00027570 File Offset: 0x00025770
		private void DetachEventHandlers()
		{
			if (this.m_secondaryItemsRoot != null)
			{
				this.m_secondaryItemsRoot.PreviewKeyDown -= new KeyEventHandler(this.SecondaryItemsRootPreviewKeyDown);
				this.m_secondaryItemsRoot.SizeChanged -= new SizeChangedEventHandler(this.SecondaryItemsRootSizeChanged);
			}
			RoutedEventHandlerRevoker firstItemLoadedRevoker = this.m_firstItemLoadedRevoker;
			if (firstItemLoadedRevoker != null)
			{
				firstItemLoadedRevoker.Revoke();
			}
			if (this.m_openingStoryboard != null)
			{
				this.m_openingStoryboard.Completed -= new EventHandler(this.OpeningStoryboardCompleted);
				this.m_openingStoryboard.CurrentStateInvalidated -= new EventHandler(this.OpeningStoryboardCurrentStateInvalidated);
				this.m_openingStoryboardState = default(ClockState?);
			}
			if (this.m_closingStoryboard != null)
			{
				this.m_closingStoryboard.Completed -= new EventHandler(this.ClosingStoryboardCompleted);
				this.m_closingStoryboard.CurrentStateInvalidated -= new EventHandler(this.ClosingStoryboardCurrentStateInvalidated);
				this.m_closingStoryboardState = default(ClockState?);
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00027648 File Offset: 0x00025848
		internal bool HasOpenAnimation()
		{
			return this.m_openingStoryboard != null && SharedHelpers.IsAnimationsEnabled;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0002765C File Offset: 0x0002585C
		internal void PlayOpenAnimation()
		{
			this.StopOpenAnimation();
			if (this.m_openingStoryboard != null)
			{
				ClockState? openingStoryboardState = this.m_openingStoryboardState;
				ClockState clockState = 0;
				if (!(openingStoryboardState.GetValueOrDefault() == clockState & openingStoryboardState != null))
				{
					CommandBar commandBar = base.TemplatedParent as CommandBar;
					if (commandBar != null && commandBar.IsOpen)
					{
						this.m_openAnimationPending = true;
						this.SetOpacity(0.0);
						return;
					}
					this.m_openAnimationPending = false;
					this.SetOpacity(0.0);
					DispatcherHelper.DoEvents(8);
					this.SetOpacity(1.0);
					this.m_openingStoryboard.Begin(this.m_layoutRoot, true);
				}
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00027702 File Offset: 0x00025902
		internal bool HasCloseAnimation()
		{
			return this.m_closingStoryboard != null && SharedHelpers.IsAnimationsEnabled;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00027714 File Offset: 0x00025914
		internal void PlayCloseAnimation(Action onCompleteFunc)
		{
			CommandBarFlyoutToolBar.<>c__DisplayClass25_0 CS$<>8__locals1 = new CommandBarFlyoutToolBar.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onCompleteFunc = onCompleteFunc;
			this.StopOpenAnimation();
			if (this.m_closingStoryboard != null)
			{
				ClockState? closingStoryboardState = this.m_closingStoryboardState;
				ClockState clockState = 0;
				if (!(closingStoryboardState.GetValueOrDefault() == clockState & closingStoryboardState != null))
				{
					this.m_closingStoryboard.Completed += new EventHandler(CS$<>8__locals1.<PlayCloseAnimation>g__closingStoryboardCompletedCallback|0);
					this.UpdateTemplateSettings();
					this.m_closingStoryboard.Begin(this.m_layoutRoot, true);
					return;
				}
			}
			else
			{
				CS$<>8__locals1.onCompleteFunc.Invoke();
			}
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0002779C File Offset: 0x0002599C
		private void UpdateFlowsFromAndFlowsTo()
		{
			ButtonBase moreButton = this.m_moreButton;
			this.EnsureTabStopUniqueness(this.PrimaryCommands, moreButton);
			this.EnsureTabStopUniqueness(this.SecondaryCommands, null);
			this.EnsureAutomationSetCountAndPosition();
			if (this.m_currentPrimaryItemsEndElement != null)
			{
				this.m_currentPrimaryItemsEndElement = null;
			}
			if (this.m_currentSecondaryItemsStartElement != null)
			{
				this.m_currentSecondaryItemsStartElement = null;
			}
			if (this.IsOpen)
			{
				ObservableCollection<ICommandBarElement> primaryCommands = this.PrimaryCommands;
				for (int i = primaryCommands.Count - 1; i >= 0; i--)
				{
					ICommandBarElement commandBarElement = primaryCommands[i];
					if (this.<UpdateFlowsFromAndFlowsTo>g__isElementFocusable|26_0(commandBarElement, false))
					{
						this.m_currentPrimaryItemsEndElement = (commandBarElement as FrameworkElement);
						break;
					}
				}
				if (moreButton != null && this.m_currentPrimaryItemsEndElement != null)
				{
					this.m_currentPrimaryItemsEndElement = moreButton;
				}
				foreach (ICommandBarElement commandBarElement2 in this.SecondaryCommands)
				{
					if (this.<UpdateFlowsFromAndFlowsTo>g__isElementFocusable|26_0(commandBarElement2, false))
					{
						this.m_currentSecondaryItemsStartElement = (commandBarElement2 as FrameworkElement);
						break;
					}
				}
			}
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x000278A0 File Offset: 0x00025AA0
		private void UpdateUI(bool useTransitions = true)
		{
			this.UpdateTemplateSettings();
			this.UpdateVisualState(useTransitions);
			this.UpdateShadow();
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000278B8 File Offset: 0x00025AB8
		private void UpdateVisualState(bool useTransitions)
		{
			CommandBarFlyoutToolBar.<>c__DisplayClass28_0 CS$<>8__locals1 = new CommandBarFlyoutToolBar.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.useTransitions = useTransitions;
			if (!this.IsOpen)
			{
				VisualStateManager.GoToState(this, "Default", CS$<>8__locals1.useTransitions);
				VisualStateManager.GoToState(this, "Collapsed", CS$<>8__locals1.useTransitions);
				return;
			}
			if (!this.m_secondaryItemsRootSized)
			{
				return;
			}
			CS$<>8__locals1.shouldExpandUp = false;
			if (this.m_secondaryItemsRoot != null && base.IsVisible && this.m_secondaryItemsRoot.IsVisible)
			{
				base.UpdateLayout();
				CS$<>8__locals1.shouldExpandUp = (this.m_secondaryItemsRoot.TranslatePoint(default(Point), this).Y < 0.0);
			}
			if (this.m_openAnimationPending)
			{
				this.m_openAnimationPending = false;
				this.CancelAsyncOpenAnimation();
				this.m_asyncOpenAnimation = base.Dispatcher.BeginInvoke(delegate()
				{
					CS$<>8__locals1.<>4__this.m_asyncOpenAnimation = null;
					CS$<>8__locals1.<>4__this.SetOpacity(1.0);
					CS$<>8__locals1.<>4__this.m_openingStoryboard.Begin(CS$<>8__locals1.<>4__this.m_layoutRoot, true);
					base.<UpdateVisualState>g__updateExpansionStates|0();
				}, 7, Array.Empty<object>());
			}
			else if (this.m_asyncOpenAnimation == null)
			{
				CS$<>8__locals1.<UpdateVisualState>g__updateExpansionStates|0();
			}
			if (this.PrimaryCommands.Count != 0)
			{
				if (CS$<>8__locals1.shouldExpandUp)
				{
					VisualStateManager.GoToState(this, "ExpandedUpWithPrimaryCommands", CS$<>8__locals1.useTransitions);
					return;
				}
				VisualStateManager.GoToState(this, "ExpandedDownWithPrimaryCommands", CS$<>8__locals1.useTransitions);
				return;
			}
			else
			{
				if (CS$<>8__locals1.shouldExpandUp)
				{
					VisualStateManager.GoToState(this, "ExpandedUpWithoutPrimaryCommands", CS$<>8__locals1.useTransitions);
					return;
				}
				VisualStateManager.GoToState(this, "ExpandedDownWithoutPrimaryCommands", CS$<>8__locals1.useTransitions);
				return;
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00027A20 File Offset: 0x00025C20
		private void UpdateTemplateSettings()
		{
			if (this.m_primaryItemsRoot != null && this.m_secondaryItemsRoot != null)
			{
				CommandBarFlyoutCommandBarTemplateSettings flyoutTemplateSettings = this.FlyoutTemplateSettings;
				if (flyoutTemplateSettings == null)
				{
					return;
				}
				double maxWidth = base.MaxWidth;
				Size size;
				size..ctor(double.PositiveInfinity, double.PositiveInfinity);
				this.m_primaryItemsRoot.Measure(size);
				Size desiredSize = this.m_primaryItemsRoot.DesiredSize;
				double num = Math.Min(maxWidth, desiredSize.Width);
				if (this.m_secondaryItemsRoot != null)
				{
					this.m_secondaryItemsRoot.Measure(size);
					Size desiredSize2 = this.m_secondaryItemsRoot.DesiredSize;
					flyoutTemplateSettings.ExpandedWidth = Math.Min(maxWidth, Math.Max(num, desiredSize2.Width));
					flyoutTemplateSettings.ExpandUpOverflowVerticalPosition = -desiredSize2.Height;
					flyoutTemplateSettings.ExpandUpAnimationStartPosition = desiredSize2.Height / 2.0;
					flyoutTemplateSettings.ExpandUpAnimationEndPosition = 0.0;
					flyoutTemplateSettings.ExpandUpAnimationHoldPosition = desiredSize2.Height;
					flyoutTemplateSettings.ExpandDownAnimationStartPosition = -desiredSize2.Height / 2.0;
					flyoutTemplateSettings.ExpandDownAnimationEndPosition = 0.0;
					flyoutTemplateSettings.ExpandDownAnimationHoldPosition = -desiredSize2.Height;
					flyoutTemplateSettings.OverflowContentClipRect = new Rect(0.0, 0.0, flyoutTemplateSettings.ExpandedWidth, desiredSize2.Height + 2.0);
				}
				else
				{
					flyoutTemplateSettings.ExpandedWidth = num;
					flyoutTemplateSettings.ExpandUpOverflowVerticalPosition = 0.0;
					flyoutTemplateSettings.ExpandUpAnimationStartPosition = 0.0;
					flyoutTemplateSettings.ExpandUpAnimationEndPosition = 0.0;
					flyoutTemplateSettings.ExpandUpAnimationHoldPosition = 0.0;
					flyoutTemplateSettings.ExpandDownAnimationStartPosition = 0.0;
					flyoutTemplateSettings.ExpandDownAnimationEndPosition = 0.0;
					flyoutTemplateSettings.ExpandDownAnimationHoldPosition = 0.0;
					flyoutTemplateSettings.OverflowContentClipRect = new Rect(0.0, 0.0, 0.0, 0.0);
				}
				double expandedWidth = flyoutTemplateSettings.ExpandedWidth;
				if (num == 0.0)
				{
					num = expandedWidth;
				}
				flyoutTemplateSettings.WidthExpansionDelta = num - expandedWidth;
				flyoutTemplateSettings.WidthExpansionAnimationStartPosition = -flyoutTemplateSettings.WidthExpansionDelta / 2.0;
				flyoutTemplateSettings.WidthExpansionAnimationEndPosition = -flyoutTemplateSettings.WidthExpansionDelta;
				flyoutTemplateSettings.ContentClipRect = new Rect(0.0, 0.0, expandedWidth, desiredSize.Height);
				if (this.IsOpen)
				{
					flyoutTemplateSettings.CurrentWidth = expandedWidth;
				}
				else
				{
					flyoutTemplateSettings.CurrentWidth = num;
				}
				bool flag = false;
				if (this.m_closingStoryboard != null)
				{
					ClockState? closingStoryboardState = this.m_closingStoryboardState;
					ClockState clockState = 0;
					flag = (closingStoryboardState.GetValueOrDefault() == clockState & closingStoryboardState != null);
				}
				if (!flag)
				{
					if (this.IsOpen)
					{
						flyoutTemplateSettings.OpenAnimationStartPosition = -expandedWidth / 2.0;
						flyoutTemplateSettings.OpenAnimationEndPosition = 0.0;
					}
					else
					{
						flyoutTemplateSettings.OpenAnimationStartPosition = flyoutTemplateSettings.WidthExpansionDelta - num / 2.0;
						flyoutTemplateSettings.OpenAnimationEndPosition = flyoutTemplateSettings.WidthExpansionDelta;
					}
					flyoutTemplateSettings.CloseAnimationEndPosition = -expandedWidth;
				}
				flyoutTemplateSettings.WidthExpansionMoreButtonAnimationStartPosition = flyoutTemplateSettings.WidthExpansionDelta / 2.0;
				flyoutTemplateSettings.WidthExpansionMoreButtonAnimationEndPosition = flyoutTemplateSettings.WidthExpansionDelta;
				if (this.PrimaryCommands.Count > 0)
				{
					flyoutTemplateSettings.ExpandDownOverflowVerticalPosition = base.Height;
					return;
				}
				flyoutTemplateSettings.ExpandDownOverflowVerticalPosition = 0.0;
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00027D80 File Offset: 0x00025F80
		private void EnsureAutomationSetCountAndPosition()
		{
			ButtonBase moreButton = this.m_moreButton;
			int num = 0;
			foreach (ICommandBarElement commandBarElement in this.PrimaryCommands)
			{
				UIElement uielement = commandBarElement as UIElement;
				if (uielement != null && uielement.Visibility == null)
				{
					num++;
				}
			}
			if (moreButton != null)
			{
				num++;
			}
			foreach (ICommandBarElement commandBarElement2 in this.PrimaryCommands)
			{
				UIElement uielement2 = commandBarElement2 as UIElement;
				if (uielement2 != null && uielement2.Visibility == null)
				{
					AutomationProperties.SetSizeOfSet(uielement2, num);
				}
			}
			if (moreButton != null)
			{
				AutomationProperties.SetSizeOfSet(moreButton, num);
				AutomationProperties.SetPositionInSet(moreButton, num);
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00027E4C File Offset: 0x0002604C
		private void EnsureFocusedPrimaryCommand()
		{
			ButtonBase moreButton = this.m_moreButton;
			Control control = this.GetFirstTabStopControl(this.PrimaryCommands);
			if (control == null && moreButton != null && moreButton.IsTabStop)
			{
				control = moreButton;
			}
			if (control != null)
			{
				if (!control.IsFocused)
				{
					this.FocusControl(control, null, false);
					return;
				}
			}
			else
			{
				this.FocusCommand(this.PrimaryCommands, moreButton, true, true);
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00027EA4 File Offset: 0x000260A4
		protected override void OnKeyDown(KeyEventArgs args)
		{
			if (args.Handled)
			{
				return;
			}
			Key key = args.Key;
			if (key != 3)
			{
				CommandBarFlyout commandBarFlyout;
				if (key != 13)
				{
					if (key - 23 <= 3)
					{
						bool flag = this.m_primaryItemsRoot != null && this.m_primaryItemsRoot.FlowDirection == 1;
						bool flag2 = (args.Key == 23 && !flag) || (args.Key == 25 && flag);
						bool flag3 = (args.Key == 25 && !flag) || (args.Key == 23 && flag);
						bool flag4 = args.Key == 26;
						bool flag5 = args.Key == 24;
						ButtonBase moreButton = this.m_moreButton;
						if (flag4 && moreButton != null && moreButton.IsFocused && this.SecondaryCommands.Count > 0)
						{
							if (!this.IsOpen)
							{
								this.IsOpen = true;
							}
							if (this.FocusCommand(this.SecondaryCommands, null, true, true))
							{
								args.Handled = true;
							}
						}
						if (!args.Handled && this.PrimaryCommands.Count > 0)
						{
							Control control = null;
							int num = 0;
							int num2 = this.PrimaryCommands.Count;
							int num3 = 1;
							if (flag2 || flag5)
							{
								num3 = -1;
								num = num2 - 1;
								num2 = -1;
								if (moreButton != null && moreButton.IsFocused)
								{
									control = moreButton;
								}
							}
							for (int num4 = num; num4 != num2; num4 += num3)
							{
								Control control2 = this.PrimaryCommands[num4] as Control;
								if (control2 != null)
								{
									if (control2.IsFocused)
									{
										control = control2;
									}
									else if (control != null && this.IsControlFocusable(control2, false) && this.FocusControl(control2, control, true))
									{
										args.Handled = true;
										break;
									}
								}
							}
							if (!args.Handled)
							{
								if ((flag3 || flag4) && control != null && moreButton != null && this.IsControlFocusable(moreButton, false))
								{
									if (this.FocusControl(moreButton, control, true))
									{
										args.Handled = true;
									}
								}
								else if (flag5 && this.SecondaryCommands.Count > 0)
								{
									if (!this.IsOpen)
									{
										this.IsOpen = true;
									}
									if (this.FocusCommand(this.SecondaryCommands, null, false, true))
									{
										args.Handled = true;
									}
								}
							}
						}
						if (!args.Handled)
						{
							args.Handled = true;
						}
					}
				}
				else if (this.TryGetOwningFlyout(out commandBarFlyout))
				{
					commandBarFlyout.Hide();
					args.Handled = true;
				}
			}
			else if (this.SecondaryCommands.Count > 0 && !this.IsOpen)
			{
				this.IsOpen = true;
				this.FocusCommand(this.SecondaryCommands, null, true, true);
			}
			base.OnKeyDown(args);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00028123 File Offset: 0x00026323
		private bool IsControlFocusable(Control control, bool checkTabStop)
		{
			return control != null && control.Visibility == null && control.IsEnabled && (!checkTabStop || control.IsTabStop);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00028148 File Offset: 0x00026348
		private Control GetFirstTabStopControl(IList<ICommandBarElement> commands)
		{
			foreach (ICommandBarElement commandBarElement in commands)
			{
				Control control = commandBarElement as Control;
				if (control != null && control.IsTabStop)
				{
					return control;
				}
			}
			return null;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000281A0 File Offset: 0x000263A0
		private bool FocusControl(Control newFocus, Control oldFocus, bool updateTabStop)
		{
			if (updateTabStop)
			{
				newFocus.IsTabStop = true;
			}
			if (newFocus.Focus())
			{
				if (oldFocus != null && updateTabStop)
				{
					oldFocus.IsTabStop = false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000281C8 File Offset: 0x000263C8
		private bool FocusCommand(IList<ICommandBarElement> commands, Control moreButton, bool firstCommand, bool ensureTabStopUniqueness)
		{
			Control control = null;
			int num = 0;
			int num2 = commands.Count;
			int num3 = 1;
			if (!firstCommand)
			{
				num3 = -1;
				num = num2 - 1;
				num2 = -1;
			}
			for (int num4 = num; num4 != num2; num4 += num3)
			{
				Control control2 = commands[num4] as Control;
				if (control2 != null && this.IsControlFocusable(control2, !ensureTabStopUniqueness))
				{
					if (control == null)
					{
						if (this.FocusControl(control2, null, ensureTabStopUniqueness))
						{
							if (ensureTabStopUniqueness && moreButton != null && moreButton.IsTabStop)
							{
								moreButton.IsTabStop = false;
							}
							control = control2;
							if (!ensureTabStopUniqueness)
							{
								break;
							}
						}
					}
					else if (control != null && control2.IsTabStop)
					{
						control2.IsTabStop = false;
					}
				}
			}
			return control != null;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00028268 File Offset: 0x00026468
		private void EnsureTabStopUniqueness(IList<ICommandBarElement> commands, Control moreButton)
		{
			bool flag = moreButton != null && moreButton.IsTabStop;
			if (flag || this.GetFirstTabStopControl(commands) != null)
			{
				using (IEnumerator<ICommandBarElement> enumerator = commands.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ICommandBarElement commandBarElement = enumerator.Current;
						Control control = commandBarElement as Control;
						if (control != null && this.IsControlFocusable(control, false) && control.IsTabStop)
						{
							if (!flag)
							{
								flag = true;
							}
							else
							{
								control.IsTabStop = false;
							}
						}
					}
					return;
				}
			}
			foreach (ICommandBarElement commandBarElement2 in commands)
			{
				Control control2 = commandBarElement2 as Control;
				if (control2 != null && this.IsControlFocusable(control2, false))
				{
					control2.IsTabStop = true;
					break;
				}
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00028338 File Offset: 0x00026538
		private void UpdateShadow()
		{
			if (this.PrimaryCommands.Count > 0)
			{
				this.AddShadow();
				return;
			}
			if (this.PrimaryCommands.Count == 0)
			{
				this.ClearShadow();
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00028362 File Offset: 0x00026562
		private void AddShadow()
		{
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00028364 File Offset: 0x00026564
		internal void ClearShadow()
		{
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00028366 File Offset: 0x00026566
		private void SecondaryItemsRootSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateUI(true);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00028370 File Offset: 0x00026570
		private void SecondaryItemsRootPreviewKeyDown(object sender, KeyEventArgs args)
		{
			if (args.Handled)
			{
				return;
			}
			Key key = args.Key;
			CommandBarFlyout commandBarFlyout;
			if (key != 13)
			{
				if (key != 24 && key != 26)
				{
					return;
				}
				if (this.SecondaryCommands.Count > 1)
				{
					Control control = null;
					int num = 0;
					int num2 = this.SecondaryCommands.Count;
					int num3 = 1;
					int num4 = 0;
					if (args.Key == 24)
					{
						num3 = -1;
						num = num2 - 1;
						num2 = -1;
					}
					for (;;)
					{
						for (int num5 = num; num5 != num2; num5 += num3)
						{
							Control control2 = this.SecondaryCommands[num5] as Control;
							if (control2 != null)
							{
								if (control2.IsFocused)
								{
									control = control2;
								}
								else if (control != null && this.IsControlFocusable(control2, false) && control != control2 && this.FocusControl(control2, control, true))
								{
									goto Block_13;
								}
							}
						}
						if (num4 == 0 && this.PrimaryCommands.Count > 0)
						{
							ButtonBase moreButton = this.m_moreButton;
							if (num3 == 1 && this.FocusCommand(this.PrimaryCommands, moreButton, true, true))
							{
								goto Block_18;
							}
							if (num3 == -1 && control != null && moreButton != null && this.IsControlFocusable(moreButton, false) && this.FocusControl(moreButton, control, true))
							{
								goto Block_23;
							}
						}
						num4++;
						if (num4 >= 2 || control == null)
						{
							goto IL_145;
						}
					}
					Block_13:
					args.Handled = true;
					return;
					Block_18:
					args.Handled = true;
					return;
					Block_23:
					args.Handled = true;
					return;
				}
				IL_145:
				args.Handled = true;
			}
			else if (this.TryGetOwningFlyout(out commandBarFlyout))
			{
				commandBarFlyout.Hide();
				return;
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000284C9 File Offset: 0x000266C9
		private void OpeningStoryboardCompleted(object sender, EventArgs e)
		{
			this.m_openingStoryboard.Stop(this.m_layoutRoot);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000284DC File Offset: 0x000266DC
		private void ClosingStoryboardCompleted(object sender, EventArgs e)
		{
			this.m_closingStoryboard.Stop(this.m_layoutRoot);
			this.SetOpacity(0.0);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00028500 File Offset: 0x00026700
		private void OpeningStoryboardCurrentStateInvalidated(object sender, EventArgs e)
		{
			Clock clock = (Clock)sender;
			this.m_openingStoryboardState = new ClockState?(clock.CurrentState);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00028528 File Offset: 0x00026728
		private void ClosingStoryboardCurrentStateInvalidated(object sender, EventArgs e)
		{
			Clock clock = (Clock)sender;
			this.m_closingStoryboardState = new ClockState?(clock.CurrentState);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002854D File Offset: 0x0002674D
		private void CancelAsyncOpenAnimation()
		{
			if (this.m_asyncOpenAnimation != null)
			{
				this.m_asyncOpenAnimation.Abort();
				this.m_asyncOpenAnimation = null;
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002856C File Offset: 0x0002676C
		private void StopOpenAnimation()
		{
			this.CancelAsyncOpenAnimation();
			if (this.m_openAnimationPending)
			{
				this.m_openAnimationPending = false;
				this.SetOpacity(1.0);
			}
			if (this.m_openingStoryboard != null)
			{
				ClockState? openingStoryboardState = this.m_openingStoryboardState;
				ClockState clockState = 0;
				if (openingStoryboardState.GetValueOrDefault() == clockState & openingStoryboardState != null)
				{
					this.m_openingStoryboard.Stop(this.m_layoutRoot);
				}
			}
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000285D4 File Offset: 0x000267D4
		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			CommandBarFlyout commandBarFlyout;
			if (!base.IsOverflowOpen && e.Handled && e.OriginalSource == this && this.TryGetOwningFlyout(out commandBarFlyout))
			{
				commandBarFlyout.Hide();
			}
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002860C File Offset: 0x0002680C
		private bool TryGetOwningFlyout(out CommandBarFlyout flyout)
		{
			WeakReference<CommandBarFlyout> owningFlyout = this.OwningFlyout;
			if (owningFlyout != null)
			{
				return owningFlyout.TryGetTarget(ref flyout);
			}
			flyout = null;
			return false;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0002862F File Offset: 0x0002682F
		private void SetOpacity(double value)
		{
			if (this.m_layoutRoot != null)
			{
				this.m_layoutRoot.Opacity = value;
			}
			if (this.m_secondaryItemsRoot != null)
			{
				this.m_secondaryItemsRoot.Opacity = value;
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000287BC File Offset: 0x000269BC
		[CompilerGenerated]
		private bool <UpdateFlowsFromAndFlowsTo>g__isElementFocusable|26_0(ICommandBarElement element, bool checkTabStop)
		{
			Control control = element as Control;
			return this.IsControlFocusable(control, checkTabStop);
		}

		// Token: 0x040004D0 RID: 1232
		private static readonly DependencyPropertyKey FlyoutTemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly("FlyoutTemplateSettings", typeof(CommandBarFlyoutCommandBarTemplateSettings), typeof(CommandBarFlyoutToolBar), null);

		// Token: 0x040004D1 RID: 1233
		public static readonly DependencyProperty FlyoutTemplateSettingsProperty = CommandBarFlyoutToolBar.FlyoutTemplateSettingsPropertyKey.DependencyProperty;

		// Token: 0x040004D2 RID: 1234
		private FrameworkElement m_layoutRoot;

		// Token: 0x040004D3 RID: 1235
		private FrameworkElement m_primaryItemsRoot;

		// Token: 0x040004D4 RID: 1236
		private FrameworkElement m_secondaryItemsRoot;

		// Token: 0x040004D5 RID: 1237
		private ButtonBase m_moreButton;

		// Token: 0x040004D6 RID: 1238
		private RoutedEventHandlerRevoker m_firstItemLoadedRevoker;

		// Token: 0x040004D7 RID: 1239
		private FrameworkElement m_currentPrimaryItemsEndElement;

		// Token: 0x040004D8 RID: 1240
		private FrameworkElement m_currentSecondaryItemsStartElement;

		// Token: 0x040004D9 RID: 1241
		private Storyboard m_openingStoryboard;

		// Token: 0x040004DA RID: 1242
		private Storyboard m_closingStoryboard;

		// Token: 0x040004DB RID: 1243
		private ClockState? m_openingStoryboardState;

		// Token: 0x040004DC RID: 1244
		private ClockState? m_closingStoryboardState;

		// Token: 0x040004DD RID: 1245
		private bool m_secondaryItemsRootSized;

		// Token: 0x040004DE RID: 1246
		private bool m_openAnimationPending;

		// Token: 0x040004DF RID: 1247
		private DispatcherOperation m_asyncOpenAnimation;
	}
}
