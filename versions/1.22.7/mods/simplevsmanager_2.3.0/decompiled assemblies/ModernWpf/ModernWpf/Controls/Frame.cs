using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xaml;
using ModernWpf.Media.Animation;

namespace ModernWpf.Controls
{
	// Token: 0x0200027B RID: 635
	[TemplatePart(Name = "FirstContentPresenter", Type = typeof(ContentPresenter))]
	[TemplatePart(Name = "SecondContentPresenter", Type = typeof(ContentPresenter))]
	public class Frame : Frame
	{
		// Token: 0x06001234 RID: 4660 RVA: 0x00035050 File Offset: 0x00033250
		static Frame()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(typeof(Frame)));
			Frame.NavigationUIVisibilityProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(2));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(false));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(false));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(null));
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00035204 File Offset: 0x00033404
		public Frame()
		{
			base.InheritanceBehavior = 0;
			base.JournalOwnership = 1;
			base.SetCurrentValue(Frame.ContentTransitionsProperty, new TransitionCollection());
			Frame.SetFrame(base.NavigationService, this);
			base.Navigating += new NavigatingCancelEventHandler(this.OnNavigating);
			base.Navigated += new NavigatedEventHandler(this.OnNavigated);
			base.NavigationStopped += new NavigationStoppedEventHandler(this.OnNavigationStopped);
			base.NavigationFailed += new NavigationFailedEventHandler(this.OnNavigationFailed);
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x00035289 File Offset: 0x00033489
		// (set) Token: 0x06001237 RID: 4663 RVA: 0x0003529B File Offset: 0x0003349B
		public Type SourcePageType
		{
			get
			{
				return (Type)base.GetValue(Frame.SourcePageTypeProperty);
			}
			set
			{
				base.SetValue(Frame.SourcePageTypeProperty, value);
			}
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000352A9 File Offset: 0x000334A9
		private static void OnSourcePageTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((Frame)sender).OnSourcePageTypePropertyChanged(args);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000352B7 File Offset: 0x000334B7
		private void OnSourcePageTypePropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (!this._ignoreSourcePageTypeChanged)
			{
				this.Navigate((Type)args.NewValue);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x000352D4 File Offset: 0x000334D4
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x000352E6 File Offset: 0x000334E6
		public Type CurrentSourcePageType
		{
			get
			{
				return (Type)base.GetValue(Frame.CurrentSourcePageTypeProperty);
			}
			private set
			{
				base.SetValue(Frame.CurrentSourcePageTypePropertyKey, value);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x000352F4 File Offset: 0x000334F4
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x00035306 File Offset: 0x00033506
		public int BackStackDepth
		{
			get
			{
				return (int)base.GetValue(Frame.BackStackDepthProperty);
			}
			private set
			{
				base.SetValue(Frame.BackStackDepthPropertyKey, value);
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00035319 File Offset: 0x00033519
		private void UpdateBackStackDepth()
		{
			IEnumerable backStack = base.BackStack;
			this.BackStackDepth = ((backStack != null) ? Enumerable.Count<object>(Enumerable.Cast<object>(backStack)) : 0);
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00035338 File Offset: 0x00033538
		// (set) Token: 0x06001240 RID: 4672 RVA: 0x0003534A File Offset: 0x0003354A
		public TransitionCollection ContentTransitions
		{
			get
			{
				return (TransitionCollection)base.GetValue(Frame.ContentTransitionsProperty);
			}
			set
			{
				base.SetValue(Frame.ContentTransitionsProperty, value);
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00035358 File Offset: 0x00033558
		private static void OnContentTransitionsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((Frame)sender).OnContentTransitionsPropertyChanged(args);
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00035366 File Offset: 0x00033566
		private void OnContentTransitionsPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			TransitionCollection transitionCollection = (TransitionCollection)args.NewValue;
			NavigationTransitionInfo navigationTransitionInfo;
			if (transitionCollection == null)
			{
				navigationTransitionInfo = null;
			}
			else
			{
				NavigationThemeTransition navigationThemeTransition = Enumerable.LastOrDefault<NavigationThemeTransition>(Enumerable.OfType<NavigationThemeTransition>(transitionCollection));
				navigationTransitionInfo = ((navigationThemeTransition != null) ? navigationThemeTransition.DefaultNavigationTransitionInfo : null);
			}
			this.DefaultNavigationTransitionInfo = (navigationTransitionInfo ?? new EntranceNavigationTransitionInfo());
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000353A0 File Offset: 0x000335A0
		internal static Frame GetFrame(NavigationService navigationService)
		{
			Frame result;
			AttachablePropertyServices.TryGetProperty<Frame>(navigationService, Frame.FrameProperty, ref result);
			return result;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000353BC File Offset: 0x000335BC
		private static void SetFrame(NavigationService navigationService, Frame value)
		{
			AttachablePropertyServices.SetProperty(navigationService, Frame.FrameProperty, value);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000353CA File Offset: 0x000335CA
		private static NavigationTransitionInfo GetNavigationTransitionInfo(JournalEntry entry)
		{
			return (NavigationTransitionInfo)entry.GetValue(Frame.NavigationTransitionInfoProperty);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000353DC File Offset: 0x000335DC
		private static void SetNavigationTransitionInfo(JournalEntry entry, NavigationTransitionInfo value)
		{
			entry.SetValue(Frame.NavigationTransitionInfoProperty, value);
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x000353EA File Offset: 0x000335EA
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x000353F2 File Offset: 0x000335F2
		private NavigationTransitionInfo DefaultNavigationTransitionInfo { get; set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x000353FB File Offset: 0x000335FB
		private JournalEntry BackEntry
		{
			get
			{
				IEnumerable backStack = base.BackStack;
				if (backStack == null)
				{
					return null;
				}
				return Enumerable.FirstOrDefault<JournalEntry>(Enumerable.OfType<JournalEntry>(backStack));
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00035413 File Offset: 0x00033613
		public bool Navigate(Type sourcePageType)
		{
			return base.Navigate(Activator.CreateInstance(sourcePageType));
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00035421 File Offset: 0x00033621
		public bool Navigate(Type sourcePageType, object parameter)
		{
			return base.Navigate(Activator.CreateInstance(sourcePageType), parameter);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00035430 File Offset: 0x00033630
		public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
		{
			this._transitionInfoOverride = infoOverride;
			return base.Navigate(Activator.CreateInstance(sourcePageType), parameter);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00035446 File Offset: 0x00033646
		public bool Navigate(Uri source, object extraData, NavigationTransitionInfo infoOverride)
		{
			this._transitionInfoOverride = infoOverride;
			return base.Navigate(source, extraData);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00035457 File Offset: 0x00033657
		public void GoBack(NavigationTransitionInfo transitionInfoOverride)
		{
			this._transitionInfoOverride = transitionInfoOverride;
			base.GoBack();
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00035468 File Offset: 0x00033668
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._oldContentPresenter = (base.GetTemplateChild("FirstContentPresenter") as ContentPresenter);
			this._newContentPresenter = (base.GetTemplateChild("SecondContentPresenter") as ContentPresenter);
			if (base.Content != null)
			{
				this.OnContentChanged(null, base.Content);
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000354BC File Offset: 0x000336BC
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			this.StopTransition();
			Page page = oldContent as Page;
			if (page != null)
			{
				this._oldPage = page;
			}
			if (this._oldContentPresenter == null || this._newContentPresenter == null)
			{
				return;
			}
			bool flag = false;
			if (Helper.IsAnimationsEnabled)
			{
				FrameworkElement frameworkElement = oldContent as FrameworkElement;
				if (frameworkElement != null)
				{
					FrameworkElement frameworkElement2 = newContent as FrameworkElement;
					if (frameworkElement2 != null)
					{
						NavigationTransitionInfo navigationTransitionInfo = this._transitionInfoOverride ?? this.DefaultNavigationTransitionInfo;
						this._exitAnimation = navigationTransitionInfo.GetExitAnimation(frameworkElement, this._movingBackwards);
						this._enterAnimation = navigationTransitionInfo.GetEnterAnimation(frameworkElement2, this._movingBackwards);
						if (this._exitAnimation != null || this._enterAnimation != null)
						{
							ContentPresenter oldContentPresenter = this._oldContentPresenter;
							ContentPresenter newContentPresenter = this._newContentPresenter;
							this._newContentPresenter = oldContentPresenter;
							this._oldContentPresenter = newContentPresenter;
							this._newContentPresenter.Opacity = 0.0;
							this._newContentPresenter.Visibility = 0;
							this._newContentPresenter.IsHitTestVisible = false;
							this._newContentPresenter.Content = frameworkElement2;
							this._oldContentPresenter.Opacity = 1.0;
							this._oldContentPresenter.Visibility = 0;
							this._oldContentPresenter.IsHitTestVisible = false;
							this._oldContentPresenter.Content = frameworkElement;
							this.BeginTransition();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				this._oldContentPresenter.Visibility = 2;
				this._oldContentPresenter.Content = null;
				this._newContentPresenter.Visibility = 0;
				this._newContentPresenter.Content = base.Content;
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0003563C File Offset: 0x0003383C
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == Frame.BackStackProperty)
			{
				this.OnBackStackPropertyChanged(e);
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0003565C File Offset: 0x0003385C
		private void OnBackStackPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			INotifyCollectionChanged notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.<OnBackStackPropertyChanged>g__OnCollectionChanged|45_0);
			}
			INotifyCollectionChanged notifyCollectionChanged2 = e.NewValue as INotifyCollectionChanged;
			if (notifyCollectionChanged2 != null)
			{
				notifyCollectionChanged2.CollectionChanged += new NotifyCollectionChangedEventHandler(this.<OnBackStackPropertyChanged>g__OnCollectionChanged|45_0);
			}
			this.UpdateBackStackDepth();
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000356B4 File Offset: 0x000338B4
		private void OnNavigating(object sender, NavigatingCancelEventArgs e)
		{
			Page page = base.Content as Page;
			if (page != null)
			{
				page.InternalOnNavigatingFrom(e);
				if (e.Cancel)
				{
					return;
				}
			}
			this._movingBackwards = (e.NavigationMode == 1);
			if (this._transitionInfoOverride == null && this._movingBackwards)
			{
				JournalEntry backEntry = this.BackEntry;
				if (backEntry != null)
				{
					this._transitionInfoOverride = Frame.GetNavigationTransitionInfo(backEntry);
				}
			}
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00035718 File Offset: 0x00033918
		private void OnNavigated(object sender, NavigationEventArgs e)
		{
			if (this._transitionInfoOverride != null)
			{
				if (!this._movingBackwards)
				{
					JournalEntry backEntry = this.BackEntry;
					if (backEntry != null)
					{
						Frame.SetNavigationTransitionInfo(backEntry, this._transitionInfoOverride);
					}
				}
				this._transitionInfoOverride = null;
			}
			try
			{
				this._ignoreSourcePageTypeChanged = true;
				object content = e.Content;
				Type type = (content != null) ? content.GetType() : null;
				this.SourcePageType = type;
				this.CurrentSourcePageType = type;
			}
			finally
			{
				this._ignoreSourcePageTypeChanged = false;
			}
			Page oldPage = this._oldPage;
			if (oldPage != null)
			{
				this._oldPage = null;
				oldPage.InternalOnNavigatedFrom(e);
			}
			Page page = e.Content as Page;
			if (page != null)
			{
				page.InternalOnNavigatedTo(e);
			}
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x000357C4 File Offset: 0x000339C4
		private void OnNavigationStopped(object sender, NavigationEventArgs e)
		{
			this._transitionInfoOverride = null;
			this._oldPage = null;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000357D4 File Offset: 0x000339D4
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			this._transitionInfoOverride = null;
			this._oldPage = null;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000357E4 File Offset: 0x000339E4
		private void BeginTransition()
		{
			if (this._exitAnimation != null)
			{
				this._exitAnimation.Completed += new EventHandler(this.OnExitAnimationCompleted);
			}
			if (this._enterAnimation != null)
			{
				this._enterAnimation.Completed += new EventHandler(this.OnEnterAnimationCompleted);
			}
			this._asyncBeginTransition = base.Dispatcher.BeginInvoke(delegate()
			{
				this._asyncBeginTransition = null;
				if (this._exitAnimation != null)
				{
					this._exitAnimation.Begin();
					return;
				}
				if (this._enterAnimation != null)
				{
					this.BeginEnterAnimation();
				}
			}, 2, Array.Empty<object>());
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00035854 File Offset: 0x00033A54
		private void BeginEnterAnimation()
		{
			if (this._oldContentPresenter != null)
			{
				this._oldContentPresenter.Visibility = 2;
				this._oldContentPresenter.Content = null;
			}
			if (this._newContentPresenter != null)
			{
				this._newContentPresenter.Opacity = 1.0;
			}
			this._enterAnimation.Begin();
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x000358A8 File Offset: 0x00033AA8
		private void OnExitAnimationCompleted(object sender, EventArgs e)
		{
			if (this._exitAnimation != null)
			{
				this._exitAnimation.Stop();
				this._exitAnimation = null;
			}
			if (this._enterAnimation != null)
			{
				this.BeginEnterAnimation();
				return;
			}
			this.StopTransition();
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000358D9 File Offset: 0x00033AD9
		private void OnEnterAnimationCompleted(object sender, EventArgs e)
		{
			if (this._enterAnimation != null)
			{
				this._enterAnimation.Stop();
				this._enterAnimation = null;
			}
			this.StopTransition();
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000358FC File Offset: 0x00033AFC
		private void StopTransition()
		{
			if (this._asyncBeginTransition != null)
			{
				this._asyncBeginTransition.Abort();
				this._asyncBeginTransition = null;
			}
			if (this._exitAnimation != null)
			{
				this._exitAnimation.Stop();
				this._exitAnimation = null;
			}
			if (this._enterAnimation != null)
			{
				this._enterAnimation.Stop();
				this._enterAnimation = null;
			}
			if (this._oldContentPresenter != null)
			{
				this._oldContentPresenter.Content = null;
				this._oldContentPresenter.ClearValue(UIElement.OpacityProperty);
				this._oldContentPresenter.ClearValue(UIElement.IsHitTestVisibleProperty);
			}
			if (this._newContentPresenter != null)
			{
				this._newContentPresenter.ClearValue(UIElement.OpacityProperty);
				this._newContentPresenter.ClearValue(UIElement.IsHitTestVisibleProperty);
			}
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000359B4 File Offset: 0x00033BB4
		[CompilerGenerated]
		private void <OnBackStackPropertyChanged>g__OnCollectionChanged|45_0(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.UpdateBackStackDepth();
		}

		// Token: 0x040005D7 RID: 1495
		public static readonly DependencyProperty SourcePageTypeProperty = DependencyProperty.Register("SourcePageType", typeof(Type), typeof(Frame), new PropertyMetadata(new PropertyChangedCallback(Frame.OnSourcePageTypePropertyChanged)));

		// Token: 0x040005D8 RID: 1496
		private static readonly DependencyPropertyKey CurrentSourcePageTypePropertyKey = DependencyProperty.RegisterReadOnly("CurrentSourcePageType", typeof(Type), typeof(Frame), null);

		// Token: 0x040005D9 RID: 1497
		public static readonly DependencyProperty CurrentSourcePageTypeProperty = Frame.CurrentSourcePageTypePropertyKey.DependencyProperty;

		// Token: 0x040005DA RID: 1498
		private static readonly DependencyPropertyKey BackStackDepthPropertyKey = DependencyProperty.RegisterReadOnly("BackStackDepth", typeof(int), typeof(Frame), null);

		// Token: 0x040005DB RID: 1499
		public static readonly DependencyProperty BackStackDepthProperty = Frame.BackStackDepthPropertyKey.DependencyProperty;

		// Token: 0x040005DC RID: 1500
		public static readonly DependencyProperty ContentTransitionsProperty = DependencyProperty.Register("ContentTransitions", typeof(TransitionCollection), typeof(Frame), new PropertyMetadata(new PropertyChangedCallback(Frame.OnContentTransitionsPropertyChanged)));

		// Token: 0x040005DD RID: 1501
		private static readonly AttachableMemberIdentifier FrameProperty = new AttachableMemberIdentifier(typeof(Frame), "Frame");

		// Token: 0x040005DE RID: 1502
		private static readonly DependencyProperty NavigationTransitionInfoProperty = DependencyProperty.RegisterAttached("NavigationTransitionInfo", typeof(NavigationTransitionInfo), typeof(Frame));

		// Token: 0x040005E0 RID: 1504
		private const string FirstContentPresenterName = "FirstContentPresenter";

		// Token: 0x040005E1 RID: 1505
		private const string SecondContentPresenterName = "SecondContentPresenter";

		// Token: 0x040005E2 RID: 1506
		private ContentPresenter _oldContentPresenter;

		// Token: 0x040005E3 RID: 1507
		private ContentPresenter _newContentPresenter;

		// Token: 0x040005E4 RID: 1508
		private bool _movingBackwards;

		// Token: 0x040005E5 RID: 1509
		private bool _ignoreSourcePageTypeChanged;

		// Token: 0x040005E6 RID: 1510
		private Page _oldPage;

		// Token: 0x040005E7 RID: 1511
		private NavigationTransitionInfo _transitionInfoOverride;

		// Token: 0x040005E8 RID: 1512
		private NavigationAnimation _exitAnimation;

		// Token: 0x040005E9 RID: 1513
		private NavigationAnimation _enterAnimation;

		// Token: 0x040005EA RID: 1514
		private DispatcherOperation _asyncBeginTransition;
	}
}
