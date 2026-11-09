using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ModernWpf.Controls
{
	// Token: 0x0200027C RID: 636
	public class Page : PageFunctionBase
	{
		// Token: 0x0600125E RID: 4702 RVA: 0x000359E8 File Offset: 0x00033BE8
		static Page()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(typeof(Page)));
			Page.BackgroundProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(Brushes.Transparent));
			Page.FontSizeProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(14.0));
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00035AA7 File Offset: 0x00033CA7
		// (set) Token: 0x06001261 RID: 4705 RVA: 0x00035AB9 File Offset: 0x00033CB9
		public Frame Frame
		{
			get
			{
				return (Frame)base.GetValue(Page.FrameProperty);
			}
			private set
			{
				base.SetValue(Page.FramePropertyKey, value);
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00035AC8 File Offset: 0x00033CC8
		private void UpdateFrame(NavigationService navigationService)
		{
			if (navigationService != null)
			{
				Frame frame = Frame.GetFrame(navigationService);
				if (frame != null)
				{
					this.Frame = frame;
					return;
				}
			}
			base.ClearValue(Page.FramePropertyKey);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00035AF5 File Offset: 0x00033CF5
		protected virtual void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00035AF7 File Offset: 0x00033CF7
		protected virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00035AF9 File Offset: 0x00033CF9
		protected virtual void OnNavigatedFrom(NavigationEventArgs e)
		{
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00035AFC File Offset: 0x00033CFC
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property.PropertyType == Page.NavigationServiceType && e.Property.OwnerType == Page.NavigationServiceType)
			{
				this.UpdateFrame((NavigationService)e.NewValue);
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00035B52 File Offset: 0x00033D52
		internal void InternalOnNavigatedTo(NavigationEventArgs e)
		{
			this.OnNavigatedTo(e);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00035B5B File Offset: 0x00033D5B
		internal void InternalOnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			this.OnNavigatingFrom(e);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00035B64 File Offset: 0x00033D64
		internal void InternalOnNavigatedFrom(NavigationEventArgs e)
		{
			this.OnNavigatedFrom(e);
		}

		// Token: 0x040005EB RID: 1515
		private static readonly DependencyPropertyKey FramePropertyKey = DependencyProperty.RegisterReadOnly("Frame", typeof(Frame), typeof(Page), null);

		// Token: 0x040005EC RID: 1516
		public static readonly DependencyProperty FrameProperty = Page.FramePropertyKey.DependencyProperty;

		// Token: 0x040005ED RID: 1517
		private static readonly Type NavigationServiceType = typeof(NavigationService);
	}
}
