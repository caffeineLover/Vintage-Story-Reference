using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MS.Win32;
using Standard;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000292 RID: 658
	internal class MaximizedWindowFixer
	{
		// Token: 0x06001372 RID: 4978 RVA: 0x00039347 File Offset: 0x00037547
		public static MaximizedWindowFixer GetMaximizedWindowFixer(Window window)
		{
			return (MaximizedWindowFixer)window.GetValue(MaximizedWindowFixer.MaximizedWindowFixerProperty);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00039359 File Offset: 0x00037559
		public static void SetMaximizedWindowFixer(Window window, MaximizedWindowFixer value)
		{
			window.SetValue(MaximizedWindowFixer.MaximizedWindowFixerProperty, value);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00039368 File Offset: 0x00037568
		private static void OnMaximizedWindowFixerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MaximizedWindowFixer maximizedWindowFixer = e.OldValue as MaximizedWindowFixer;
			if (maximizedWindowFixer != null)
			{
				maximizedWindowFixer.UnsetWindow();
			}
			MaximizedWindowFixer maximizedWindowFixer2 = e.NewValue as MaximizedWindowFixer;
			if (maximizedWindowFixer2 != null)
			{
				maximizedWindowFixer2.SetWindow((Window)d);
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x000393A8 File Offset: 0x000375A8
		private Thickness MaximizedWindowBorder
		{
			get
			{
				Thickness thickness = this._maximizedWindowBorder.GetValueOrDefault();
				if (this._maximizedWindowBorder == null)
				{
					thickness = this.GetMaximizedWindowBorder();
					this._maximizedWindowBorder = new Thickness?(thickness);
					return thickness;
				}
				return thickness;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x000393E4 File Offset: 0x000375E4
		// (set) Token: 0x06001377 RID: 4983 RVA: 0x000393EC File Offset: 0x000375EC
		private bool IsWindowPosAdjusted
		{
			get
			{
				return this._isWindowPosAdjusted;
			}
			set
			{
				if (this._isWindowPosAdjusted != value)
				{
					this._isWindowPosAdjusted = value;
					this.InvalidateMaximizedWindowBorder();
				}
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00039404 File Offset: 0x00037604
		private void SetWindow(Window window)
		{
			this.UnsubscribeWindowEvents();
			this._window = window;
			this._hwnd = new WindowInteropHelper(window).Handle;
			this._window.StateChanged += new EventHandler(this.WindowStateChanged);
			this._window.DpiChanged += new DpiChangedEventHandler(this.WindowDpiChanged);
			this._window.Closed += new EventHandler(this.WindowClosed);
			if (this._hwnd != IntPtr.Zero)
			{
				this.WindowSourceInitialized(null, null);
				return;
			}
			this._window.SourceInitialized += new EventHandler(this.WindowSourceInitialized);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000394A6 File Offset: 0x000376A6
		private void UnsetWindow()
		{
			this.UnsubscribeWindowEvents();
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x000394B0 File Offset: 0x000376B0
		private void UnsubscribeWindowEvents()
		{
			if (this._window != null)
			{
				this._window.SourceInitialized -= new EventHandler(this.WindowSourceInitialized);
				this._window.StateChanged -= new EventHandler(this.WindowStateChanged);
				this._window.DpiChanged -= new DpiChangedEventHandler(this.WindowDpiChanged);
				this._window.Closed -= new EventHandler(this.WindowClosed);
				this._window.ClearValue(Control.PaddingProperty);
				this._window = null;
			}
			if (this._hwndSource != null)
			{
				this._hwndSource.RemoveHook(new HwndSourceHook(this.WindowFilterMessage));
				this._hwndSource = null;
			}
			this._hwnd = IntPtr.Zero;
			this._maximizedWindowBorder = default(Thickness?);
			this._isWindowPosAdjusted = false;
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0003957C File Offset: 0x0003777C
		private void WindowSourceInitialized(object sender, EventArgs e)
		{
			this._hwnd = new WindowInteropHelper(this._window).Handle;
			this._hwndSource = HwndSource.FromHwnd(this._hwnd);
			this._hwndSource.AddHook(new HwndSourceHook(this.WindowFilterMessage));
			this.UpdateWindowPadding();
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000395CD File Offset: 0x000377CD
		private void WindowStateChanged(object sender, EventArgs e)
		{
			this.UpdateWindowPadding();
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000395D5 File Offset: 0x000377D5
		private void WindowDpiChanged(object sender, DpiChangedEventArgs e)
		{
			this.InvalidateMaximizedWindowBorder();
			this.UpdateWindowPadding();
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000395E3 File Offset: 0x000377E3
		private void WindowClosed(object sender, EventArgs e)
		{
			this.UnsetWindow();
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000395EC File Offset: 0x000377EC
		private IntPtr WindowFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			if (msg != 26)
			{
				if (msg != 70)
				{
					if (msg == 71)
					{
						if (this._maximizedWindowBorder == null)
						{
							this.UpdateWindowPadding();
						}
					}
				}
				else
				{
					this.OnWindowPosChanging(lParam);
				}
			}
			else
			{
				this.InvalidateMaximizedWindowBorder();
				this.UpdateWindowPadding();
			}
			return zero;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00039640 File Offset: 0x00037840
		private void OnWindowPosChanging(IntPtr lParam)
		{
			WINDOWPOS windowpos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
			if ((windowpos.flags & 1) == 0)
			{
				bool isWindowPosAdjusted = false;
				MaximizedWindowFixer.ABEdge edge;
				if (Standard.NativeMethods.GetWindowPlacement(windowpos.hwnd).showCmd == SW.SHOWMAXIMIZED && MaximizedWindowFixer.GetTaskbarAutoHide(out edge))
				{
					MS.Win32.NativeMethods.RECT rect = new MS.Win32.NativeMethods.RECT(windowpos.x, windowpos.y, windowpos.x + windowpos.cx, windowpos.y + windowpos.cy);
					IntPtr intPtr = SafeNativeMethods.MonitorFromRect(ref rect, 2);
					if (intPtr != IntPtr.Zero)
					{
						MONITORINFO monitorInfo = Standard.NativeMethods.GetMonitorInfo(intPtr);
						if ((monitorInfo.dwFlags & 1) != 0)
						{
							if (windowpos.x < 0 && windowpos.y < 0 && windowpos.cx > monitorInfo.rcMonitor.Width && windowpos.cy > monitorInfo.rcMonitor.Height)
							{
								MaximizedWindowFixer.AdjustWindowPosForTaskbarAutoHide(ref windowpos, edge);
								Marshal.StructureToPtr<WINDOWPOS>(windowpos, lParam, true);
								isWindowPosAdjusted = true;
							}
							else if (windowpos.x == 0 && windowpos.y == 0)
							{
								isWindowPosAdjusted = true;
							}
						}
					}
				}
				this.IsWindowPosAdjusted = isWindowPosAdjusted;
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00039758 File Offset: 0x00037958
		private Thickness GetMaximizedWindowBorder()
		{
			if (this.IsWindowPosAdjusted)
			{
				return default(Thickness);
			}
			DpiScale2 dpi = this._window.GetDpi();
			int systemMetrics = Standard.NativeMethods.GetSystemMetrics(SM.CXFRAME);
			int systemMetrics2 = Standard.NativeMethods.GetSystemMetrics(SM.CYFRAME);
			int systemMetrics3 = Standard.NativeMethods.GetSystemMetrics(SM.CXPADDEDBORDER);
			Size size = DpiHelper.DeviceSizeToLogical(new Size((double)(systemMetrics + systemMetrics3), (double)(systemMetrics2 + systemMetrics3)), dpi.DpiScaleX, dpi.DpiScaleY);
			return new Thickness(size.Width, size.Height, size.Width, size.Height);
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000397DF File Offset: 0x000379DF
		private void InvalidateMaximizedWindowBorder()
		{
			this._maximizedWindowBorder = default(Thickness?);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x000397F0 File Offset: 0x000379F0
		private void UpdateWindowPadding()
		{
			if (this._hwndSource == null || this._hwndSource.IsDisposed || this._hwndSource.CompositionTarget == null)
			{
				return;
			}
			if (this._window.WindowState == 2)
			{
				this._window.Padding = this.MaximizedWindowBorder;
				return;
			}
			this._window.ClearValue(Control.PaddingProperty);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00039850 File Offset: 0x00037A50
		private static bool GetTaskbarAutoHide(out MaximizedWindowFixer.ABEdge edge)
		{
			IntPtr intPtr = MaximizedWindowFixer.FindWindow("Shell_TrayWnd", null);
			if (intPtr != IntPtr.Zero)
			{
				MaximizedWindowFixer.APPBARDATA appbardata = default(MaximizedWindowFixer.APPBARDATA);
				appbardata.cbSize = Marshal.SizeOf<MaximizedWindowFixer.APPBARDATA>(appbardata);
				appbardata.hWnd = intPtr;
				MaximizedWindowFixer.SHAppBarMessage(MaximizedWindowFixer.ABMsg.ABM_GETTASKBARPOS, ref appbardata);
				bool flag = Convert.ToBoolean(MaximizedWindowFixer.SHAppBarMessage(MaximizedWindowFixer.ABMsg.ABM_GETSTATE, ref appbardata));
				edge = (flag ? MaximizedWindowFixer.<GetTaskbarAutoHide>g__GetEdge|21_0(appbardata.rc) : MaximizedWindowFixer.ABEdge.ABE_LEFT);
				return flag;
			}
			edge = MaximizedWindowFixer.ABEdge.ABE_LEFT;
			return false;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000398C4 File Offset: 0x00037AC4
		private static void AdjustWindowPosForTaskbarAutoHide(ref WINDOWPOS pos, MaximizedWindowFixer.ABEdge edge)
		{
			pos.cx += pos.x * 2;
			pos.cy += pos.y * 2;
			pos.x = 0;
			pos.y = 0;
			switch (edge)
			{
			case MaximizedWindowFixer.ABEdge.ABE_LEFT:
				pos.x = 2;
				pos.cx -= 2;
				return;
			case MaximizedWindowFixer.ABEdge.ABE_TOP:
				pos.y = 2;
				pos.cy -= 2;
				return;
			case MaximizedWindowFixer.ABEdge.ABE_RIGHT:
				pos.cx -= 2;
				return;
			case MaximizedWindowFixer.ABEdge.ABE_BOTTOM:
				pos.cy -= 2;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001386 RID: 4998
		[DllImport("shell32", CallingConvention = 3)]
		private static extern uint SHAppBarMessage(MaximizedWindowFixer.ABMsg dwMessage, ref MaximizedWindowFixer.APPBARDATA pData);

		// Token: 0x06001387 RID: 4999
		[DllImport("user32", CharSet = 4)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x0600138A RID: 5002 RVA: 0x00039998 File Offset: 0x00037B98
		[CompilerGenerated]
		internal static MaximizedWindowFixer.ABEdge <GetTaskbarAutoHide>g__GetEdge|21_0(RECT rc)
		{
			if (rc.Top == rc.Left && rc.Bottom > rc.Right)
			{
				return MaximizedWindowFixer.ABEdge.ABE_LEFT;
			}
			if (rc.Top == rc.Left && rc.Bottom < rc.Right)
			{
				return MaximizedWindowFixer.ABEdge.ABE_TOP;
			}
			if (rc.Top > rc.Left)
			{
				return MaximizedWindowFixer.ABEdge.ABE_BOTTOM;
			}
			return MaximizedWindowFixer.ABEdge.ABE_RIGHT;
		}

		// Token: 0x04000683 RID: 1667
		public static readonly DependencyProperty MaximizedWindowFixerProperty = DependencyProperty.RegisterAttached("MaximizedWindowFixer", typeof(MaximizedWindowFixer), typeof(MaximizedWindowFixer), new PropertyMetadata(new PropertyChangedCallback(MaximizedWindowFixer.OnMaximizedWindowFixerChanged)));

		// Token: 0x04000684 RID: 1668
		private const int MONITOR_DEFAULTTONEAREST = 2;

		// Token: 0x04000685 RID: 1669
		private const int MONITORINFOF_PRIMARY = 1;

		// Token: 0x04000686 RID: 1670
		private Window _window;

		// Token: 0x04000687 RID: 1671
		private IntPtr _hwnd;

		// Token: 0x04000688 RID: 1672
		private HwndSource _hwndSource;

		// Token: 0x04000689 RID: 1673
		private Thickness? _maximizedWindowBorder;

		// Token: 0x0400068A RID: 1674
		private bool _isWindowPosAdjusted;

		// Token: 0x02000418 RID: 1048
		private enum WindowMessage
		{
			// Token: 0x04000A19 RID: 2585
			WM_SETTINGCHANGE = 26,
			// Token: 0x04000A1A RID: 2586
			WM_WINDOWPOSCHANGING = 70,
			// Token: 0x04000A1B RID: 2587
			WM_WINDOWPOSCHANGED
		}

		// Token: 0x02000419 RID: 1049
		private enum ABEdge
		{
			// Token: 0x04000A1D RID: 2589
			ABE_LEFT,
			// Token: 0x04000A1E RID: 2590
			ABE_TOP,
			// Token: 0x04000A1F RID: 2591
			ABE_RIGHT,
			// Token: 0x04000A20 RID: 2592
			ABE_BOTTOM
		}

		// Token: 0x0200041A RID: 1050
		private enum ABMsg
		{
			// Token: 0x04000A22 RID: 2594
			ABM_GETSTATE = 4,
			// Token: 0x04000A23 RID: 2595
			ABM_GETTASKBARPOS
		}

		// Token: 0x0200041B RID: 1051
		private struct APPBARDATA
		{
			// Token: 0x04000A24 RID: 2596
			public int cbSize;

			// Token: 0x04000A25 RID: 2597
			public IntPtr hWnd;

			// Token: 0x04000A26 RID: 2598
			public int uCallbackMessage;

			// Token: 0x04000A27 RID: 2599
			public int uEdge;

			// Token: 0x04000A28 RID: 2600
			public RECT rc;

			// Token: 0x04000A29 RID: 2601
			public bool lParam;
		}
	}
}
