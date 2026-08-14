using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Navigation;
using ModernWpf.Automation.Peers;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000045 RID: 69
	public class HyperlinkButton : ButtonBase
	{
		// Token: 0x06000298 RID: 664 RVA: 0x00008FCC File Offset: 0x000071CC
		static HyperlinkButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HyperlinkButton), new FrameworkPropertyMetadata(typeof(HyperlinkButton)));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(HyperlinkButton), new FrameworkPropertyMetadata(1));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(HyperlinkButton), new FrameworkPropertyMetadata(1));
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000090C0 File Offset: 0x000072C0
		public HyperlinkButton()
		{
			this.m_hyperlink = new Hyperlink
			{
				NavigateUri = this.NavigateUri,
				TargetName = this.TargetName
			};
			this.m_hyperlink.RequestNavigate += new RequestNavigateEventHandler(this.OnRequestNavigate);
			base.AddLogicalChild(this.m_hyperlink);
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00009119 File Offset: 0x00007319
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000912B File Offset: 0x0000732B
		public Uri NavigateUri
		{
			get
			{
				return (Uri)base.GetValue(HyperlinkButton.NavigateUriProperty);
			}
			set
			{
				base.SetValue(HyperlinkButton.NavigateUriProperty, value);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00009139 File Offset: 0x00007339
		private static void OnNavigateUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HyperlinkButton)d).m_hyperlink.NavigateUri = (Uri)e.NewValue;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00009157 File Offset: 0x00007357
		// (set) Token: 0x0600029E RID: 670 RVA: 0x00009169 File Offset: 0x00007369
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(HyperlinkButton.TargetNameProperty);
			}
			set
			{
				base.SetValue(HyperlinkButton.TargetNameProperty, value);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00009177 File Offset: 0x00007377
		private static void OnTargetNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HyperlinkButton)d).m_hyperlink.TargetName = (string)e.NewValue;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00009195 File Offset: 0x00007395
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x000091A7 File Offset: 0x000073A7
		public bool UseSystemFocusVisuals
		{
			get
			{
				return (bool)base.GetValue(HyperlinkButton.UseSystemFocusVisualsProperty);
			}
			set
			{
				base.SetValue(HyperlinkButton.UseSystemFocusVisualsProperty, value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x000091BA File Offset: 0x000073BA
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x000091CC File Offset: 0x000073CC
		public Thickness FocusVisualMargin
		{
			get
			{
				return (Thickness)base.GetValue(HyperlinkButton.FocusVisualMarginProperty);
			}
			set
			{
				base.SetValue(HyperlinkButton.FocusVisualMarginProperty, value);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000091DF File Offset: 0x000073DF
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HyperlinkButtonAutomationPeer(this);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000091E8 File Offset: 0x000073E8
		protected override void OnClick()
		{
			if (AutomationPeer.ListenerExists(5))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(5);
				}
			}
			this.m_hyperlink.DoClick();
			base.OnClick();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000921F File Offset: 0x0000741F
		internal void AutomationButtonBaseClick()
		{
			this.OnClick();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00009228 File Offset: 0x00007428
		private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Uri uri = e.Uri;
			if (uri.IsAbsoluteUri && uri.Scheme.IndexOf("http", 5) >= 0)
			{
				Process.Start(new ProcessStartInfo(uri.ToString())
				{
					UseShellExecute = true
				});
				e.Handled = true;
			}
		}

		// Token: 0x04000101 RID: 257
		public static readonly DependencyProperty NavigateUriProperty = Hyperlink.NavigateUriProperty.AddOwner(typeof(HyperlinkButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(HyperlinkButton.OnNavigateUriChanged)));

		// Token: 0x04000102 RID: 258
		public static readonly DependencyProperty TargetNameProperty = Hyperlink.TargetNameProperty.AddOwner(typeof(HyperlinkButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(HyperlinkButton.OnTargetNameChanged)));

		// Token: 0x04000103 RID: 259
		public static readonly DependencyProperty UseSystemFocusVisualsProperty = FocusVisualHelper.UseSystemFocusVisualsProperty.AddOwner(typeof(HyperlinkButton));

		// Token: 0x04000104 RID: 260
		public static readonly DependencyProperty FocusVisualMarginProperty = FocusVisualHelper.FocusVisualMarginProperty.AddOwner(typeof(HyperlinkButton));

		// Token: 0x04000105 RID: 261
		private readonly Hyperlink m_hyperlink;
	}
}
