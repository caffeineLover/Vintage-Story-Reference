using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using VintageStoryModManager.Models;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000017 RID: 23
	[NullableContext(1)]
	[Nullable(0)]
	public class ModBrowserView : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0001C934 File Offset: 0x0001AB34
		public bool IsModBrowserInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001C93C File Offset: 0x0001AB3C
		public ModBrowserView()
		{
			this.InitializeComponent();
			base.DataContextChanged += new DependencyPropertyChangedEventHandler(this.OnDataContextChanged);
			base.Unloaded += new RoutedEventHandler(this.OnControlUnloaded);
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0001C96E File Offset: 0x0001AB6E
		[Nullable(2)]
		private ModBrowserViewModel ViewModel
		{
			[NullableContext(2)]
			get
			{
				return base.DataContext as ModBrowserViewModel;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001C97B File Offset: 0x0001AB7B
		private void OnViewLoaded(object sender, RoutedEventArgs e)
		{
			this.SubscribeToViewModel(this.ViewModel);
			this.UpdateScreenshotImageAsync(true);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001C994 File Offset: 0x0001AB94
		public Task InitializeAsync()
		{
			ModBrowserView.<InitializeAsync>d__11 <InitializeAsync>d__;
			<InitializeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InitializeAsync>d__.<>4__this = this;
			<InitializeAsync>d__.<>1__state = -1;
			<InitializeAsync>d__.<>t__builder.Start<ModBrowserView.<InitializeAsync>d__11>(ref <InitializeAsync>d__);
			return <InitializeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001C9D7 File Offset: 0x0001ABD7
		private void OnControlUnloaded(object sender, RoutedEventArgs e)
		{
			this.CancelScreenshotLoad();
			this.CurrentScreenshotImage.Source = null;
			this.SubscribeToViewModel(null);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001C9F2 File Offset: 0x0001ABF2
		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (base.IsLoaded)
			{
				this.SubscribeToViewModel(e.NewValue as ModBrowserViewModel);
				this.UpdateScreenshotImageAsync(true);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001CA18 File Offset: 0x0001AC18
		[NullableContext(2)]
		private void SubscribeToViewModel(ModBrowserViewModel viewModel)
		{
			if (this._subscribedViewModel == viewModel)
			{
				return;
			}
			if (this._subscribedViewModel != null)
			{
				this._subscribedViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.OnViewModelPropertyChanged);
			}
			this._subscribedViewModel = viewModel;
			if (this._subscribedViewModel != null)
			{
				this._subscribedViewModel.PropertyChanged += new PropertyChangedEventHandler(this.OnViewModelPropertyChanged);
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001CA74 File Offset: 0x0001AC74
		private void OnViewModelPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentScreenshotUrl")
			{
				this.UpdateScreenshotImageAsync(true);
				return;
			}
			if (e.PropertyName == "IsModDetailsOpen")
			{
				ModBrowserViewModel viewModel = this.ViewModel;
				if (viewModel == null || !viewModel.IsModDetailsOpen)
				{
					this.CancelScreenshotLoad();
					this.CurrentScreenshotImage.Source = null;
				}
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
		private Task UpdateScreenshotImageAsync(bool clearExistingImage)
		{
			ModBrowserView.<UpdateScreenshotImageAsync>d__16 <UpdateScreenshotImageAsync>d__;
			<UpdateScreenshotImageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateScreenshotImageAsync>d__.<>4__this = this;
			<UpdateScreenshotImageAsync>d__.clearExistingImage = clearExistingImage;
			<UpdateScreenshotImageAsync>d__.<>1__state = -1;
			<UpdateScreenshotImageAsync>d__.<>t__builder.Start<ModBrowserView.<UpdateScreenshotImageAsync>d__16>(ref <UpdateScreenshotImageAsync>d__);
			return <UpdateScreenshotImageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001CB24 File Offset: 0x0001AD24
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private static Task<byte[]> LoadScreenshotBytesAsync(string screenshotUrl, CancellationToken cancellationToken)
		{
			ModBrowserView.<LoadScreenshotBytesAsync>d__17 <LoadScreenshotBytesAsync>d__;
			<LoadScreenshotBytesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<LoadScreenshotBytesAsync>d__.screenshotUrl = screenshotUrl;
			<LoadScreenshotBytesAsync>d__.cancellationToken = cancellationToken;
			<LoadScreenshotBytesAsync>d__.<>1__state = -1;
			<LoadScreenshotBytesAsync>d__.<>t__builder.Start<ModBrowserView.<LoadScreenshotBytesAsync>d__17>(ref <LoadScreenshotBytesAsync>d__);
			return <LoadScreenshotBytesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001CB70 File Offset: 0x0001AD70
		private static BitmapImage CreateBitmapImage(byte[] imageBytes)
		{
			BitmapImage result;
			using (MemoryStream memoryStream = new MemoryStream(imageBytes, false))
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = 1;
				bitmapImage.CreateOptions = 4;
				bitmapImage.StreamSource = memoryStream;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				result = bitmapImage;
			}
			return result;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
		private void CancelScreenshotLoad()
		{
			this._screenshotLoadVersion++;
			CancellationTokenSource screenshotLoadCancellation = this._screenshotLoadCancellation;
			if (screenshotLoadCancellation != null)
			{
				screenshotLoadCancellation.Cancel();
			}
			CancellationTokenSource screenshotLoadCancellation2 = this._screenshotLoadCancellation;
			if (screenshotLoadCancellation2 != null)
			{
				screenshotLoadCancellation2.Dispose();
			}
			this._screenshotLoadCancellation = null;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001CC09 File Offset: 0x0001AE09
		private static HttpClient CreateScreenshotHttpClient()
		{
			HttpClient httpClient = new HttpClient();
			httpClient.Timeout = TimeSpan.FromSeconds(30.0);
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SimpleVSManager/1.0");
			return httpClient;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001CC39 File Offset: 0x0001AE39
		private void ScrollToTop_Click(object sender, RoutedEventArgs e)
		{
			this.ModsScrollViewer.ScrollToTop();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001CC46 File Offset: 0x0001AE46
		private void ModsScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight - e.ViewportHeight * 1.5)
			{
				ModBrowserViewModel viewModel = this.ViewModel;
				if (viewModel == null)
				{
					return;
				}
				viewModel.LoadMoreCommand.Execute(null);
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001CC84 File Offset: 0x0001AE84
		private void ModsScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer scrollViewer = sender as ScrollViewer;
			if (scrollViewer != null)
			{
				double num = scrollViewer.VerticalOffset - (double)e.Delta * 65.0 / 120.0;
				scrollViewer.ScrollToVerticalOffset(num);
				e.Handled = true;
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001CCCC File Offset: 0x0001AECC
		private void FavoriteButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				object tag = frameworkElement.Tag;
				if (tag is int)
				{
					int num = (int)tag;
					ModBrowserViewModel viewModel = this.ViewModel;
					if (viewModel == null)
					{
						return;
					}
					viewModel.ToggleFavoriteCommand.Execute(num);
				}
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001CD18 File Offset: 0x0001AF18
		private void InstallButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			if (this.ViewModel == null)
			{
				return;
			}
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				object tag = frameworkElement.Tag;
				if (tag is int)
				{
					int num = (int)tag;
					if (this.ViewModel.InstallModCommand == null)
					{
						return;
					}
					this.ViewModel.InstallModCommand.Execute(num);
				}
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001CD74 File Offset: 0x0001AF74
		private void OpenInBrowserButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				object tag = frameworkElement.Tag;
				if (tag is int)
				{
					int num = (int)tag;
					ModBrowserViewModel viewModel = this.ViewModel;
					if (viewModel == null)
					{
						return;
					}
					viewModel.OpenModInBrowserCommand.Execute(num);
				}
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
		private void ModCard_Click(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				DownloadableModOnList downloadableModOnList = frameworkElement.DataContext as DownloadableModOnList;
				if (downloadableModOnList != null)
				{
					ModBrowserViewModel viewModel = this.ViewModel;
					if (viewModel == null)
					{
						return;
					}
					viewModel.OpenModDetailsCommand.Execute(downloadableModOnList);
				}
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001CDFC File Offset: 0x0001AFFC
		private void ModDetailOverlay_Click(object sender, MouseButtonEventArgs e)
		{
			ModBrowserViewModel viewModel = this.ViewModel;
			if (viewModel == null)
			{
				return;
			}
			viewModel.CloseModDetailsCommand.Execute(null);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001CE14 File Offset: 0x0001B014
		private void ModDetailCard_Click(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001CE20 File Offset: 0x0001B020
		private void ModDetailOverlay_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UIElement overlay = sender as UIElement;
			if (overlay == null || !overlay.IsVisible)
			{
				return;
			}
			overlay.Dispatcher.BeginInvoke(5, delegate()
			{
				if (overlay.IsVisible)
				{
					Keyboard.Focus(overlay);
				}
			});
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001CE73 File Offset: 0x0001B073
		private void ModBrowserView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == 13)
			{
				ModBrowserViewModel viewModel = this.ViewModel;
				if (viewModel != null && viewModel.IsModDetailsOpen)
				{
					this.ViewModel.CloseModDetailsCommand.Execute(null);
					e.Handled = true;
				}
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001CEAC File Offset: 0x0001B0AC
		private void VersionsDropdown_SelectionChanged([Nullable(2)] object sender, EventArgs e)
		{
			ModBrowserView.<VersionsDropdown_SelectionChanged>d__32 <VersionsDropdown_SelectionChanged>d__;
			<VersionsDropdown_SelectionChanged>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<VersionsDropdown_SelectionChanged>d__.<>4__this = this;
			<VersionsDropdown_SelectionChanged>d__.<>1__state = -1;
			<VersionsDropdown_SelectionChanged>d__.<>t__builder.Start<ModBrowserView.<VersionsDropdown_SelectionChanged>d__32>(ref <VersionsDropdown_SelectionChanged>d__);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
		private void TagsDropdown_SelectionChanged([Nullable(2)] object sender, EventArgs e)
		{
			ModBrowserView.<TagsDropdown_SelectionChanged>d__33 <TagsDropdown_SelectionChanged>d__;
			<TagsDropdown_SelectionChanged>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<TagsDropdown_SelectionChanged>d__.<>4__this = this;
			<TagsDropdown_SelectionChanged>d__.<>1__state = -1;
			<TagsDropdown_SelectionChanged>d__.<>t__builder.Start<ModBrowserView.<TagsDropdown_SelectionChanged>d__33>(ref <TagsDropdown_SelectionChanged>d__);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001CF1C File Offset: 0x0001B11C
		private Task TriggerSearchOnSelectionChanged()
		{
			ModBrowserView.<TriggerSearchOnSelectionChanged>d__34 <TriggerSearchOnSelectionChanged>d__;
			<TriggerSearchOnSelectionChanged>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<TriggerSearchOnSelectionChanged>d__.<>4__this = this;
			<TriggerSearchOnSelectionChanged>d__.<>1__state = -1;
			<TriggerSearchOnSelectionChanged>d__.<>t__builder.Start<ModBrowserView.<TriggerSearchOnSelectionChanged>d__34>(ref <TriggerSearchOnSelectionChanged>d__);
			return <TriggerSearchOnSelectionChanged>d__.<>t__builder.Task;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001CF60 File Offset: 0x0001B160
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/modbrowserview.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001CF90 File Offset: 0x0001B190
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001CF9C File Offset: 0x0001B19C
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((ModBrowserView)target).Loaded += new RoutedEventHandler(this.OnViewLoaded);
				((ModBrowserView)target).PreviewKeyDown += new KeyEventHandler(this.ModBrowserView_PreviewKeyDown);
				return;
			case 2:
				((Button)target).Click += new RoutedEventHandler(this.ScrollToTop_Click);
				return;
			case 3:
				this.ModsScrollViewer = (ScrollViewer)target;
				this.ModsScrollViewer.PreviewMouseWheel += new MouseWheelEventHandler(this.ModsScrollViewer_PreviewMouseWheel);
				this.ModsScrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ModsScrollViewer_ScrollChanged);
				return;
			case 7:
				this.ModDetailOverlay = (Border)target;
				this.ModDetailOverlay.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ModDetailOverlay_IsVisibleChanged);
				this.ModDetailOverlay.MouseLeftButtonDown += new MouseButtonEventHandler(this.ModDetailOverlay_Click);
				return;
			case 8:
				((Border)target).MouseLeftButtonDown += new MouseButtonEventHandler(this.ModDetailCard_Click);
				return;
			case 9:
				this.CurrentScreenshotImage = (Image)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001D0C4 File Offset: 0x0001B2C4
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 4:
				((Border)target).MouseLeftButtonUp += new MouseButtonEventHandler(this.ModCard_Click);
				return;
			case 5:
				((Button)target).Click += new RoutedEventHandler(this.FavoriteButton_Click);
				return;
			case 6:
				((Button)target).Click += new RoutedEventHandler(this.InstallButton_Click);
				return;
			default:
				return;
			}
		}

		// Token: 0x040001D5 RID: 469
		private static readonly HttpClient ScreenshotHttpClient = ModBrowserView.CreateScreenshotHttpClient();

		// Token: 0x040001D6 RID: 470
		private bool _isInitialized;

		// Token: 0x040001D7 RID: 471
		private int _screenshotLoadVersion;

		// Token: 0x040001D8 RID: 472
		[Nullable(2)]
		private CancellationTokenSource _screenshotLoadCancellation;

		// Token: 0x040001D9 RID: 473
		[Nullable(2)]
		private ModBrowserViewModel _subscribedViewModel;

		// Token: 0x040001DA RID: 474
		[Nullable(0)]
		internal ScrollViewer ModsScrollViewer;

		// Token: 0x040001DB RID: 475
		[Nullable(0)]
		internal Border ModDetailOverlay;

		// Token: 0x040001DC RID: 476
		[Nullable(0)]
		internal Image CurrentScreenshotImage;

		// Token: 0x040001DD RID: 477
		private bool _contentLoaded;
	}
}
