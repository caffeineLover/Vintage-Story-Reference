using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200029B RID: 667
	public class PopupEx : Popup
	{
		// Token: 0x060013DF RID: 5087 RVA: 0x0003A872 File Offset: 0x00038A72
		static PopupEx()
		{
			Popup.IsOpenProperty.OverrideMetadata(typeof(PopupEx), new FrameworkPropertyMetadata(new PropertyChangedCallback(PopupEx.OnIsOpenPropertyChanged)));
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0003A899 File Offset: 0x00038A99
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x0003A8A1 File Offset: 0x00038AA1
		internal bool SuppressFadeAnimation { get; set; }

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060013E2 RID: 5090 RVA: 0x0003A8AC File Offset: 0x00038AAC
		// (remove) Token: 0x060013E3 RID: 5091 RVA: 0x0003A8E4 File Offset: 0x00038AE4
		internal event EventHandler Closing;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060013E4 RID: 5092 RVA: 0x0003A91C File Offset: 0x00038B1C
		// (remove) Token: 0x060013E5 RID: 5093 RVA: 0x0003A954 File Offset: 0x00038B54
		internal event EventHandler IsOpenChanged;

		// Token: 0x060013E6 RID: 5094 RVA: 0x0003A989 File Offset: 0x00038B89
		protected override void OnOpened(EventArgs e)
		{
			base.OnOpened(e);
			if (base.PopupAnimation == 1 && this.SuppressFadeAnimation)
			{
				this.StopAnimation();
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0003A9A9 File Offset: 0x00038BA9
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);
			if (!base.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0003A9C1 File Offset: 0x00038BC1
		protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseRightButtonDown(e);
			if (!base.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0003A9D9 File Offset: 0x00038BD9
		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonUp(e);
			if (!base.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0003A9F1 File Offset: 0x00038BF1
		protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseRightButtonUp(e);
			if (!base.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0003AA09 File Offset: 0x00038C09
		private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PopupEx)d).OnIsOpenChanged();
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0003AA18 File Offset: 0x00038C18
		private void OnIsOpenChanged()
		{
			EventHandler isOpenChanged = this.IsOpenChanged;
			if (isOpenChanged != null)
			{
				isOpenChanged.Invoke(this, EventArgs.Empty);
			}
			if (!base.IsOpen)
			{
				if (base.PopupAnimation == 1 && this.SuppressFadeAnimation)
				{
					this.StopAnimation();
				}
				EventHandler closing = this.Closing;
				if (closing == null)
				{
					return;
				}
				closing.Invoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0003AA74 File Offset: 0x00038C74
		private void StopAnimation()
		{
			FrameworkElement frameworkElement = base.Child as FrameworkElement;
			if (frameworkElement != null)
			{
				FrameworkElement frameworkElement2 = PopupEx.FindPopupRoot(frameworkElement) as FrameworkElement;
				if (frameworkElement2 != null)
				{
					frameworkElement2.BeginAnimation(UIElement.OpacityProperty, null);
				}
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0003AAAC File Offset: 0x00038CAC
		private static DependencyObject FindPopupRoot(DependencyObject child)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			if (parent != null)
			{
				return PopupEx.FindPopupRoot(parent);
			}
			return child;
		}
	}
}
