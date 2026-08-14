using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000019 RID: 25
	[ContentProperty("PrimaryCommands")]
	public class CommandBarFlyout : FlyoutBase
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000041CC File Offset: 0x000023CC
		public CommandBarFlyout()
		{
			this.PrimaryCommands = new ObservableCollection<ICommandBarElement>();
			this.SecondaryCommands = new ObservableCollection<ICommandBarElement>();
			this.PrimaryCommands.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs args)
			{
				if (this.m_commandBar != null)
				{
					SharedHelpers.ForwardCollectionChange<ICommandBarElement>((ObservableCollection<ICommandBarElement>)sender, this.m_commandBar.PrimaryCommands, args);
				}
			};
			this.SecondaryCommands.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs args)
			{
				CommandBarFlyoutCommandBar commandBar = this.m_commandBar;
				if (commandBar != null)
				{
					SharedHelpers.ForwardCollectionChange<ICommandBarElement>((ObservableCollection<ICommandBarElement>)sender, commandBar.SecondaryCommands, args);
					RoutedEventHandler handler = delegate(object <p0>, RoutedEventArgs <p1>)
					{
						base.Hide();
					};
					switch (args.Action)
					{
					case 0:
					{
						ICommandBarElement commandBarElement = (ICommandBarElement)args.NewItems[0];
						AppBarButton appBarButton = commandBarElement as AppBarButton;
						AppBarToggleButton appBarToggleButton = commandBarElement as AppBarToggleButton;
						if (appBarButton != null && appBarButton.Flyout == null)
						{
							this.m_secondaryButtonClickRevokerByElementMap[commandBarElement] = new RoutedEventHandlerRevoker(appBarButton, ButtonBase.ClickEvent, handler);
							return;
						}
						if (appBarToggleButton != null)
						{
							this.m_secondaryToggleButtonCheckedRevokerByElementMap[commandBarElement] = new RoutedEventHandlerRevoker(appBarToggleButton, ToggleButton.CheckedEvent, handler);
							this.m_secondaryToggleButtonUncheckedRevokerByElementMap[commandBarElement] = new RoutedEventHandlerRevoker(appBarToggleButton, ToggleButton.UncheckedEvent, handler);
							return;
						}
						break;
					}
					case 1:
					{
						ICommandBarElement element = (ICommandBarElement)args.OldItems[0];
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryButtonClickRevokerByElementMap, element);
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonCheckedRevokerByElementMap, element);
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonUncheckedRevokerByElementMap, element);
						return;
					}
					case 2:
					{
						ICommandBarElement commandBarElement2 = (ICommandBarElement)args.NewItems[0];
						ICommandBarElement element2 = (ICommandBarElement)args.OldItems[0];
						AppBarButton appBarButton2 = commandBarElement2 as AppBarButton;
						AppBarToggleButton appBarToggleButton2 = commandBarElement2 as AppBarToggleButton;
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryButtonClickRevokerByElementMap, element2);
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonCheckedRevokerByElementMap, element2);
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonUncheckedRevokerByElementMap, element2);
						if (appBarButton2 != null && appBarButton2.Flyout == null)
						{
							this.m_secondaryButtonClickRevokerByElementMap[commandBarElement2] = new RoutedEventHandlerRevoker(appBarButton2, ButtonBase.ClickEvent, handler);
							CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonCheckedRevokerByElementMap, commandBarElement2);
							CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonUncheckedRevokerByElementMap, commandBarElement2);
							return;
						}
						if (appBarToggleButton2 != null)
						{
							CommandBarFlyout.RevokeAndRemove(this.m_secondaryButtonClickRevokerByElementMap, commandBarElement2);
							this.m_secondaryToggleButtonCheckedRevokerByElementMap[commandBarElement2] = new RoutedEventHandlerRevoker(appBarToggleButton2, ToggleButton.CheckedEvent, handler);
							this.m_secondaryToggleButtonUncheckedRevokerByElementMap[commandBarElement2] = new RoutedEventHandlerRevoker(appBarToggleButton2, ToggleButton.UncheckedEvent, handler);
							return;
						}
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryButtonClickRevokerByElementMap, commandBarElement2);
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonCheckedRevokerByElementMap, commandBarElement2);
						CommandBarFlyout.RevokeAndRemove(this.m_secondaryToggleButtonUncheckedRevokerByElementMap, commandBarElement2);
						return;
					}
					case 3:
						break;
					case 4:
						this.SetSecondaryCommandsToCloseWhenExecuted();
						break;
					default:
						return;
					}
				}
			};
			base.Opening += delegate([Nullable(2)] object <p0>, object <p1>)
			{
				base.InternalPopup.SuppressFadeAnimation = true;
				if (base.ShowMode == FlyoutShowMode.Standard)
				{
					this.m_commandBar.IsOpen = true;
				}
			};
			base.Opened += delegate([Nullable(2)] object <p0>, object <p1>)
			{
				if (this.m_commandBar != null && this.m_commandBar.HasOpenAnimation())
				{
					this.m_commandBar.PlayOpenAnimation();
				}
			};
			base.Closing += delegate(FlyoutBase <p0>, FlyoutBaseClosingEventArgs <p1>)
			{
				CommandBarFlyoutCommandBar commandBar = this.m_commandBar;
				if (commandBar != null)
				{
					if (!this.m_isClosingAfterCloseAnimation && commandBar.HasCloseAnimation())
					{
						commandBar.PlayCloseAnimation(delegate
						{
							this.m_isClosingAfterCloseAnimation = true;
							base.Hide();
							this.m_isClosingAfterCloseAnimation = false;
						});
					}
					commandBar.IsOpen = false;
					commandBar.ClearShadow();
				}
			};
			base.Closed += delegate([Nullable(2)] object <p0>, object <p1>)
			{
				if (this.m_commandBar != null && this.m_commandBar.IsOpen)
				{
					this.m_commandBar.IsOpen = false;
				}
			};
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000428C File Offset: 0x0000248C
		public ObservableCollection<ICommandBarElement> PrimaryCommands { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004294 File Offset: 0x00002494
		public ObservableCollection<ICommandBarElement> SecondaryCommands { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000429C File Offset: 0x0000249C
		internal override PopupAnimation DesiredPopupAnimation
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000042A0 File Offset: 0x000024A0
		protected override Control CreatePresenter()
		{
			CommandBarFlyoutCommandBar commandBarFlyoutCommandBar = new CommandBarFlyoutCommandBar();
			commandBarFlyoutCommandBar.Opened += delegate([Nullable(2)] object <p0>, object <p1>)
			{
				base.SetCurrentValue(FlyoutBase.ShowModeProperty, FlyoutShowMode.Standard);
			};
			SharedHelpers.CopyList<ICommandBarElement>(this.PrimaryCommands, commandBarFlyoutCommandBar.PrimaryCommands);
			SharedHelpers.CopyList<ICommandBarElement>(this.SecondaryCommands, commandBarFlyoutCommandBar.SecondaryCommands);
			this.SetSecondaryCommandsToCloseWhenExecuted();
			FlyoutPresenter flyoutPresenter = new FlyoutPresenter();
			flyoutPresenter.Background = null;
			flyoutPresenter.Foreground = null;
			flyoutPresenter.BorderBrush = null;
			flyoutPresenter.MinWidth = 0.0;
			flyoutPresenter.MaxWidth = double.PositiveInfinity;
			flyoutPresenter.MinHeight = 0.0;
			flyoutPresenter.MaxHeight = double.PositiveInfinity;
			flyoutPresenter.BorderThickness = new Thickness(0.0);
			flyoutPresenter.Padding = new Thickness(0.0);
			flyoutPresenter.Content = commandBarFlyoutCommandBar;
			flyoutPresenter.CornerRadius = new CornerRadius(0.0);
			flyoutPresenter.IsDefaultShadowEnabled = false;
			commandBarFlyoutCommandBar.SetOwningFlyout(this);
			this.m_commandBar = commandBarFlyoutCommandBar;
			return flyoutPresenter;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000439C File Offset: 0x0000259C
		private void SetSecondaryCommandsToCloseWhenExecuted()
		{
			CommandBarFlyout.RevokeAndClear(this.m_secondaryButtonClickRevokerByElementMap);
			CommandBarFlyout.RevokeAndClear(this.m_secondaryToggleButtonCheckedRevokerByElementMap);
			CommandBarFlyout.RevokeAndClear(this.m_secondaryToggleButtonUncheckedRevokerByElementMap);
			RoutedEventHandler handler = delegate(object <p0>, RoutedEventArgs <p1>)
			{
				base.Hide();
			};
			for (int i = 0; i < this.SecondaryCommands.Count; i++)
			{
				ICommandBarElement commandBarElement = this.SecondaryCommands[i];
				AppBarButton appBarButton = commandBarElement as AppBarButton;
				AppBarToggleButton appBarToggleButton = commandBarElement as AppBarToggleButton;
				if (appBarButton != null && appBarButton.Flyout == null)
				{
					this.m_secondaryButtonClickRevokerByElementMap[commandBarElement] = new RoutedEventHandlerRevoker(appBarButton, ButtonBase.ClickEvent, handler);
				}
				else if (appBarToggleButton != null)
				{
					this.m_secondaryToggleButtonCheckedRevokerByElementMap[commandBarElement] = new RoutedEventHandlerRevoker(appBarToggleButton, ToggleButton.CheckedEvent, handler);
					this.m_secondaryToggleButtonUncheckedRevokerByElementMap[commandBarElement] = new RoutedEventHandlerRevoker(appBarToggleButton, ToggleButton.UncheckedEvent, handler);
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004468 File Offset: 0x00002668
		private static void RevokeAndRemove(IDictionary<ICommandBarElement, RoutedEventHandlerRevoker> map, ICommandBarElement element)
		{
			RoutedEventHandlerRevoker routedEventHandlerRevoker;
			if (map.TryGetValue(element, ref routedEventHandlerRevoker))
			{
				routedEventHandlerRevoker.Revoke();
				map.Remove(element);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004490 File Offset: 0x00002690
		private static void RevokeAndClear(IDictionary<ICommandBarElement, RoutedEventHandlerRevoker> map)
		{
			foreach (RoutedEventHandlerRevoker routedEventHandlerRevoker in map.Values)
			{
				routedEventHandlerRevoker.Revoke();
			}
			map.Clear();
		}

		// Token: 0x04000033 RID: 51
		private CommandBarFlyoutCommandBar m_commandBar;

		// Token: 0x04000034 RID: 52
		private Dictionary<ICommandBarElement, RoutedEventHandlerRevoker> m_secondaryButtonClickRevokerByElementMap = new Dictionary<ICommandBarElement, RoutedEventHandlerRevoker>();

		// Token: 0x04000035 RID: 53
		private Dictionary<ICommandBarElement, RoutedEventHandlerRevoker> m_secondaryToggleButtonCheckedRevokerByElementMap = new Dictionary<ICommandBarElement, RoutedEventHandlerRevoker>();

		// Token: 0x04000036 RID: 54
		private Dictionary<ICommandBarElement, RoutedEventHandlerRevoker> m_secondaryToggleButtonUncheckedRevokerByElementMap = new Dictionary<ICommandBarElement, RoutedEventHandlerRevoker>();

		// Token: 0x04000037 RID: 55
		private bool m_isClosingAfterCloseAnimation;
	}
}
