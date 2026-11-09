using System;
using System.Windows;
using System.Windows.Data;
using ModernWpf.Controls;

namespace ModernWpf
{
	// Token: 0x0200022C RID: 556
	internal sealed class CoreApplicationViewTitleBar
	{
		// Token: 0x06001050 RID: 4176 RVA: 0x0002EC35 File Offset: 0x0002CE35
		private CoreApplicationViewTitleBar(Window owner)
		{
			this._owner = owner;
			this._listener = new CoreApplicationViewTitleBar.Listener(this);
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0002EC50 File Offset: 0x0002CE50
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x0002EC5D File Offset: 0x0002CE5D
		public bool ExtendViewIntoTitleBar
		{
			get
			{
				return TitleBar.GetExtendViewIntoTitleBar(this._owner);
			}
			set
			{
				TitleBar.SetExtendViewIntoTitleBar(this._owner, value);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0002EC6B File Offset: 0x0002CE6B
		public double Height
		{
			get
			{
				return TitleBar.GetHeight(this._owner);
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0002EC78 File Offset: 0x0002CE78
		public bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0002EC7B File Offset: 0x0002CE7B
		public double SystemOverlayLeftInset
		{
			get
			{
				return TitleBar.GetSystemOverlayLeftInset(this._owner);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0002EC88 File Offset: 0x0002CE88
		public double SystemOverlayRightInset
		{
			get
			{
				return TitleBar.GetSystemOverlayRightInset(this._owner);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001057 RID: 4183 RVA: 0x0002EC98 File Offset: 0x0002CE98
		// (remove) Token: 0x06001058 RID: 4184 RVA: 0x0002ECD0 File Offset: 0x0002CED0
		public event TypedEventHandler<CoreApplicationViewTitleBar, object> IsVisibleChanged;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06001059 RID: 4185 RVA: 0x0002ED08 File Offset: 0x0002CF08
		// (remove) Token: 0x0600105A RID: 4186 RVA: 0x0002ED40 File Offset: 0x0002CF40
		public event TypedEventHandler<CoreApplicationViewTitleBar, object> LayoutMetricsChanged;

		// Token: 0x0600105B RID: 4187 RVA: 0x0002ED75 File Offset: 0x0002CF75
		private void RaiseIsVisibleChanged()
		{
			TypedEventHandler<CoreApplicationViewTitleBar, object> isVisibleChanged = this.IsVisibleChanged;
			if (isVisibleChanged == null)
			{
				return;
			}
			isVisibleChanged.Invoke(this, null);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0002ED89 File Offset: 0x0002CF89
		private void RaiseLayoutMetricsChanged()
		{
			TypedEventHandler<CoreApplicationViewTitleBar, object> layoutMetricsChanged = this.LayoutMetricsChanged;
			if (layoutMetricsChanged == null)
			{
				return;
			}
			layoutMetricsChanged.Invoke(this, null);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0002EDA0 File Offset: 0x0002CFA0
		internal static CoreApplicationViewTitleBar GetTitleBar(Window window)
		{
			CoreApplicationViewTitleBar coreApplicationViewTitleBar = (CoreApplicationViewTitleBar)window.GetValue(CoreApplicationViewTitleBar.TitleBarProperty);
			if (coreApplicationViewTitleBar == null)
			{
				coreApplicationViewTitleBar = new CoreApplicationViewTitleBar(window);
				CoreApplicationViewTitleBar.SetTitleBar(window, coreApplicationViewTitleBar);
			}
			return coreApplicationViewTitleBar;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0002EDD0 File Offset: 0x0002CFD0
		internal static CoreApplicationViewTitleBar GetTitleBar(DependencyObject dependencyObject)
		{
			Window window = Window.GetWindow(dependencyObject);
			if (window != null)
			{
				return CoreApplicationViewTitleBar.GetTitleBar(window);
			}
			return null;
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0002EDEF File Offset: 0x0002CFEF
		private static void SetTitleBar(Window window, CoreApplicationViewTitleBar value)
		{
			window.SetValue(CoreApplicationViewTitleBar.TitleBarProperty, value);
		}

		// Token: 0x04000445 RID: 1093
		private static readonly DependencyProperty TitleBarProperty = DependencyProperty.RegisterAttached("TitleBar", typeof(CoreApplicationViewTitleBar), typeof(CoreApplicationViewTitleBar));

		// Token: 0x04000446 RID: 1094
		private readonly Window _owner;

		// Token: 0x04000447 RID: 1095
		private readonly CoreApplicationViewTitleBar.Listener _listener;

		// Token: 0x0200040E RID: 1038
		private class Listener : DependencyObject
		{
			// Token: 0x06001BC3 RID: 7107 RVA: 0x0004A68C File Offset: 0x0004888C
			public Listener(CoreApplicationViewTitleBar owner)
			{
				this._owner = owner;
				Window owner2 = this._owner._owner;
				BindingOperations.SetBinding(this, CoreApplicationViewTitleBar.Listener.ExtendViewIntoTitleBarProperty, new Binding
				{
					Path = new PropertyPath(TitleBar.ExtendViewIntoTitleBarProperty),
					Source = owner2
				});
				BindingOperations.SetBinding(this, CoreApplicationViewTitleBar.Listener.HeightProperty, new Binding
				{
					Path = new PropertyPath(TitleBar.HeightProperty),
					Source = owner2
				});
				BindingOperations.SetBinding(this, CoreApplicationViewTitleBar.Listener.SystemOverlayLeftInsetProperty, new Binding
				{
					Path = new PropertyPath(TitleBar.SystemOverlayLeftInsetProperty),
					Source = owner2
				});
				BindingOperations.SetBinding(this, CoreApplicationViewTitleBar.Listener.SystemOverlayRightInsetProperty, new Binding
				{
					Path = new PropertyPath(TitleBar.SystemOverlayRightInsetProperty),
					Source = owner2
				});
			}

			// Token: 0x17000507 RID: 1287
			// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x0004A752 File Offset: 0x00048952
			// (set) Token: 0x06001BC5 RID: 7109 RVA: 0x0004A764 File Offset: 0x00048964
			public bool ExtendViewIntoTitleBar
			{
				get
				{
					return (bool)base.GetValue(CoreApplicationViewTitleBar.Listener.ExtendViewIntoTitleBarProperty);
				}
				set
				{
					base.SetValue(CoreApplicationViewTitleBar.Listener.ExtendViewIntoTitleBarProperty, value);
				}
			}

			// Token: 0x06001BC6 RID: 7110 RVA: 0x0004A777 File Offset: 0x00048977
			private static void OnExtendViewIntoTitleBarPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				((CoreApplicationViewTitleBar.Listener)sender).OnExtendViewIntoTitleBarPropertyChanged(args);
			}

			// Token: 0x06001BC7 RID: 7111 RVA: 0x0004A785 File Offset: 0x00048985
			private void OnExtendViewIntoTitleBarPropertyChanged(DependencyPropertyChangedEventArgs args)
			{
				this._owner.RaiseLayoutMetricsChanged();
				this._owner.RaiseIsVisibleChanged();
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x0004A79D File Offset: 0x0004899D
			// (set) Token: 0x06001BC9 RID: 7113 RVA: 0x0004A7AF File Offset: 0x000489AF
			public double Height
			{
				get
				{
					return (double)base.GetValue(CoreApplicationViewTitleBar.Listener.HeightProperty);
				}
				set
				{
					base.SetValue(CoreApplicationViewTitleBar.Listener.HeightProperty, value);
				}
			}

			// Token: 0x06001BCA RID: 7114 RVA: 0x0004A7C2 File Offset: 0x000489C2
			private static void OnHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				((CoreApplicationViewTitleBar.Listener)sender).OnHeightPropertyChanged(args);
			}

			// Token: 0x06001BCB RID: 7115 RVA: 0x0004A7D0 File Offset: 0x000489D0
			private void OnHeightPropertyChanged(DependencyPropertyChangedEventArgs args)
			{
				this._owner.RaiseLayoutMetricsChanged();
			}

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0004A7DD File Offset: 0x000489DD
			// (set) Token: 0x06001BCD RID: 7117 RVA: 0x0004A7EF File Offset: 0x000489EF
			public double SystemOverlayLeftInset
			{
				get
				{
					return (double)base.GetValue(CoreApplicationViewTitleBar.Listener.SystemOverlayLeftInsetProperty);
				}
				set
				{
					base.SetValue(CoreApplicationViewTitleBar.Listener.SystemOverlayLeftInsetProperty, value);
				}
			}

			// Token: 0x06001BCE RID: 7118 RVA: 0x0004A802 File Offset: 0x00048A02
			private static void OnSystemOverlayLeftInsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				((CoreApplicationViewTitleBar.Listener)sender).OnSystemOverlayLeftInsetPropertyChanged(args);
			}

			// Token: 0x06001BCF RID: 7119 RVA: 0x0004A810 File Offset: 0x00048A10
			private void OnSystemOverlayLeftInsetPropertyChanged(DependencyPropertyChangedEventArgs args)
			{
				this._owner.RaiseLayoutMetricsChanged();
			}

			// Token: 0x1700050A RID: 1290
			// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x0004A81D File Offset: 0x00048A1D
			// (set) Token: 0x06001BD1 RID: 7121 RVA: 0x0004A82F File Offset: 0x00048A2F
			public double SystemOverlayRightInset
			{
				get
				{
					return (double)base.GetValue(CoreApplicationViewTitleBar.Listener.SystemOverlayRightInsetProperty);
				}
				set
				{
					base.SetValue(CoreApplicationViewTitleBar.Listener.SystemOverlayRightInsetProperty, value);
				}
			}

			// Token: 0x06001BD2 RID: 7122 RVA: 0x0004A842 File Offset: 0x00048A42
			private static void OnSystemOverlayRightInsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
			{
				((CoreApplicationViewTitleBar.Listener)sender).OnSystemOverlayRightInsetPropertyChanged(args);
			}

			// Token: 0x06001BD3 RID: 7123 RVA: 0x0004A850 File Offset: 0x00048A50
			private void OnSystemOverlayRightInsetPropertyChanged(DependencyPropertyChangedEventArgs args)
			{
				this._owner.RaiseLayoutMetricsChanged();
			}

			// Token: 0x04000A00 RID: 2560
			public static readonly DependencyProperty ExtendViewIntoTitleBarProperty = DependencyProperty.Register("ExtendViewIntoTitleBar", typeof(bool), typeof(CoreApplicationViewTitleBar.Listener), new PropertyMetadata(new PropertyChangedCallback(CoreApplicationViewTitleBar.Listener.OnExtendViewIntoTitleBarPropertyChanged)));

			// Token: 0x04000A01 RID: 2561
			public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(CoreApplicationViewTitleBar.Listener), new PropertyMetadata(new PropertyChangedCallback(CoreApplicationViewTitleBar.Listener.OnHeightPropertyChanged)));

			// Token: 0x04000A02 RID: 2562
			public static readonly DependencyProperty SystemOverlayLeftInsetProperty = DependencyProperty.Register("SystemOverlayLeftInset", typeof(double), typeof(CoreApplicationViewTitleBar.Listener), new PropertyMetadata(new PropertyChangedCallback(CoreApplicationViewTitleBar.Listener.OnSystemOverlayLeftInsetPropertyChanged)));

			// Token: 0x04000A03 RID: 2563
			public static readonly DependencyProperty SystemOverlayRightInsetProperty = DependencyProperty.Register("SystemOverlayRightInset", typeof(double), typeof(CoreApplicationViewTitleBar.Listener), new PropertyMetadata(new PropertyChangedCallback(CoreApplicationViewTitleBar.Listener.OnSystemOverlayRightInsetPropertyChanged)));

			// Token: 0x04000A04 RID: 2564
			private readonly CoreApplicationViewTitleBar _owner;
		}
	}
}
