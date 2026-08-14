using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200029C RID: 668
	internal class PopupPositioner : DependencyObject, IDisposable
	{
		// Token: 0x060013F1 RID: 5105 RVA: 0x0003AB48 File Offset: 0x00038D48
		public PopupPositioner(Popup popup)
		{
			if (!PopupPositioner.IsSupported)
			{
				throw new NotSupportedException();
			}
			this._popup = popup;
			this._secHelper = new PopupPositioner.PopupSecurityHelper();
			PopupPositioner.SetPositioner(popup, this);
			popup.Opened += new EventHandler(this.OnPopupOpened);
			popup.Closed += new EventHandler(this.OnPopupClosed);
			if (popup.IsOpen)
			{
				this.OnPopupOpened(null, null);
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0003ABB8 File Offset: 0x00038DB8
		public void Dispose()
		{
			if (this._isDisposed)
			{
				return;
			}
			this._isDisposed = true;
			if (this._popup != null)
			{
				this._popup.Opened -= new EventHandler(this.OnPopupOpened);
				this._popup.Closed -= new EventHandler(this.OnPopupClosed);
				this._popup.ClearValue(PopupPositioner.PositionerProperty);
			}
			this.OnPopupClosed(null, null);
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0003AC23 File Offset: 0x00038E23
		public static bool IsSupported { get; } = PopupPositioner.Delegates.GetPlacementInternal != null && PopupPositioner.Delegates.GetDropOpposite != null && PopupPositioner.Delegates.GetPlacementTargetInterestPoints != null && PopupPositioner.Delegates.GetChildInterestPoints != null && PopupPositioner.Delegates.GetScreenBounds != null;

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0003AC2A File Offset: 0x00038E2A
		public bool IsOpen
		{
			get
			{
				return this._popup.IsOpen;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0003AC37 File Offset: 0x00038E37
		public PlacementMode Placement
		{
			get
			{
				return this._popup.Placement;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x0003AC44 File Offset: 0x00038E44
		internal PlacementMode PlacementInternal
		{
			get
			{
				return PopupPositioner.Delegates.GetPlacementInternal.Invoke(this._popup);
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x0003AC56 File Offset: 0x00038E56
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return this._popup.CustomPopupPlacementCallback;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0003AC63 File Offset: 0x00038E63
		public double HorizontalOffset
		{
			get
			{
				return this._popup.HorizontalOffset;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0003AC70 File Offset: 0x00038E70
		public double VerticalOffset
		{
			get
			{
				return this._popup.VerticalOffset;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x0003AC7D File Offset: 0x00038E7D
		internal bool DropOpposite
		{
			get
			{
				return PopupPositioner.Delegates.GetDropOpposite.Invoke(this._popup);
			}
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0003AC8F File Offset: 0x00038E8F
		private void OnWindowResize(object sender, AutoResizedEventArgs e)
		{
			if (this._positionInfo == null)
			{
				return;
			}
			if (e.Size != this._positionInfo.ChildSize)
			{
				this._positionInfo.ChildSize = e.Size;
				this.Reposition();
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0003ACCC File Offset: 0x00038ECC
		internal void Reposition()
		{
			if (this.IsOpen && this._secHelper.IsWindowAlive())
			{
				if (base.CheckAccess())
				{
					this.UpdatePosition();
					return;
				}
				base.Dispatcher.BeginInvoke(9, delegate(object param)
				{
					this.Reposition();
					return null;
				}, null);
			}
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0003AD18 File Offset: 0x00038F18
		private void UpdatePosition()
		{
			if (this._popupRoot == null)
			{
				return;
			}
			PlacementMode placementInternal = this.PlacementInternal;
			Point[] placementTargetInterestPoints = this.GetPlacementTargetInterestPoints(placementInternal);
			Point[] childInterestPoints = this.GetChildInterestPoints(placementInternal);
			Rect bounds = this.GetBounds(placementTargetInterestPoints);
			Rect bounds2 = this.GetBounds(childInterestPoints);
			double num = bounds2.Width * bounds2.Height;
			Rect windowRect = this._secHelper.GetWindowRect();
			if (this._positionInfo == null)
			{
				this._positionInfo = new PopupPositioner.PositionInfo();
			}
			this._positionInfo.X = (int)windowRect.X;
			this._positionInfo.Y = (int)windowRect.Y;
			this._positionInfo.ChildSize = windowRect.Size;
			Vector vector;
			vector..ctor((double)this._positionInfo.X, (double)this._positionInfo.Y);
			double num2 = -1.0;
			CustomPopupPlacement[] array = null;
			int num3;
			if (placementInternal == 11)
			{
				CustomPopupPlacementCallback customPopupPlacementCallback = this.CustomPopupPlacementCallback;
				if (customPopupPlacementCallback != null)
				{
					array = customPopupPlacementCallback.Invoke(bounds2.Size, bounds.Size, new Point(this.HorizontalOffset, this.VerticalOffset));
				}
				num3 = ((array == null) ? 0 : array.Length);
				if (!this.IsOpen)
				{
					return;
				}
			}
			else
			{
				num3 = PopupPositioner.GetNumberOfCombinations(placementInternal);
			}
			Rect screenBounds;
			for (int i = 0; i < num3; i++)
			{
				Vector vector2;
				if (placementInternal == 11)
				{
					vector2 = (Vector)placementTargetInterestPoints[0] + (Vector)array[i].Point;
					PopupPrimaryAxis primaryAxis = array[i].PrimaryAxis;
				}
				else
				{
					PopupPrimaryAxis primaryAxis;
					PopupPositioner.PointCombination pointCombination = this.GetPointCombination(placementInternal, i, out primaryAxis);
					InterestPoint targetInterestPoint = pointCombination.TargetInterestPoint;
					InterestPoint childInterestPoint = pointCombination.ChildInterestPoint;
					vector2 = placementTargetInterestPoints[(int)targetInterestPoint] - childInterestPoints[(int)childInterestPoint];
				}
				Rect rect = Rect.Offset(bounds2, vector2);
				screenBounds = this.GetScreenBounds(bounds, placementTargetInterestPoints[0]);
				Rect rect2 = Rect.Intersect(screenBounds, rect);
				double num4 = (rect2 != Rect.Empty) ? (rect2.Width * rect2.Height) : 0.0;
				if (num4 - num2 > 0.01)
				{
					vector = vector2;
					num2 = num4;
					if (Math.Abs(num4 - num) < 0.01)
					{
						break;
					}
				}
			}
			Matrix transformToDevice = this._secHelper.GetTransformToDevice();
			bounds2..ctor((Size)transformToDevice.Transform((Point)this.<UpdatePosition>g__GetChildSize|24_0()));
			bounds2.Offset(vector);
			Vector vector3 = (Vector)transformToDevice.Transform(this.<UpdatePosition>g__GetChildTranslation|24_1());
			bounds2.Offset(vector3);
			screenBounds = this.GetScreenBounds(bounds, placementTargetInterestPoints[0]);
			Rect rect3 = Rect.Intersect(screenBounds, bounds2);
			if (Math.Abs(rect3.Width - bounds2.Width) > 0.01 || Math.Abs(rect3.Height - bounds2.Height) > 0.01)
			{
				Point point = placementTargetInterestPoints[0];
				Vector vector4 = placementTargetInterestPoints[1] - point;
				vector4.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector4.Y) || Math.Abs(vector4.Y) < 0.01)
				{
					if (bounds2.Right > screenBounds.Right)
					{
						vector.X = screenBounds.Right - bounds2.Width;
						vector.X -= vector3.X;
					}
					else if (bounds2.Left < screenBounds.Left)
					{
						vector.X = screenBounds.Left;
						vector.X -= vector3.X;
					}
				}
				else if (this.IsTransparent && Math.Abs(vector4.X) < 0.01)
				{
					if (bounds2.Bottom > screenBounds.Bottom)
					{
						vector.Y = screenBounds.Bottom - bounds2.Height;
						vector.Y -= vector3.Y;
					}
					else if (bounds2.Top < screenBounds.Top)
					{
						vector.Y = screenBounds.Top;
						vector.Y -= vector3.Y;
					}
				}
				Point point2 = placementTargetInterestPoints[2];
				Vector vector5 = point - point2;
				vector5.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector5.X) || Math.Abs(vector5.X) < 0.01)
				{
					if (bounds2.Bottom > screenBounds.Bottom)
					{
						vector.Y = screenBounds.Bottom - bounds2.Height;
						vector.Y -= vector3.Y;
					}
					else if (bounds2.Top < screenBounds.Top)
					{
						vector.Y = screenBounds.Top;
						vector.Y -= vector3.Y;
					}
				}
				else if (this.IsTransparent && Math.Abs(vector5.Y) < 0.01)
				{
					if (bounds2.Right > screenBounds.Right)
					{
						vector.X = screenBounds.Right - bounds2.Width;
						vector.X -= vector3.X;
					}
					else if (bounds2.Left < screenBounds.Left)
					{
						vector.X = screenBounds.Left;
						vector.X -= vector3.X;
					}
				}
			}
			int num5 = DoubleUtil.DoubleToInt(vector.X);
			int num6 = DoubleUtil.DoubleToInt(vector.Y);
			if (num5 != this._positionInfo.X || num6 != this._positionInfo.Y)
			{
				this._positionInfo.X = num5;
				this._positionInfo.Y = num6;
				this._secHelper.SetPopupPos(true, num5, num6, false, 0, 0);
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0003B308 File Offset: 0x00039508
		private Point[] GetPlacementTargetInterestPoints(PlacementMode placement)
		{
			return PopupPositioner.Delegates.GetPlacementTargetInterestPoints.Invoke(this._popup, placement);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0003B31C File Offset: 0x0003951C
		private PopupPositioner.PointCombination GetPointCombination(PlacementMode placement, int i, out PopupPrimaryAxis axis)
		{
			bool flag = SystemParameters.MenuDropAlignment;
			switch (placement)
			{
			case 1:
			case 5:
			case 6:
			case 8:
				axis = 1;
				if (flag)
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopLeft);
					}
					if (i == 2)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.BottomRight);
					}
					if (i == 3)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
					}
					if (i == 2)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
					}
					if (i == 3)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				break;
			case 2:
			case 7:
				axis = 1;
				if (flag)
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomRight, InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopRight, InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomLeft, InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				break;
			case 3:
				axis = 0;
				return new PopupPositioner.PointCombination(InterestPoint.Center, InterestPoint.Center);
			case 4:
			case 9:
				axis = 2;
				flag |= this.DropOpposite;
				if ((flag && placement == 4) || (!flag && placement == 9))
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomLeft, InterestPoint.BottomRight);
					}
					if (i == 2)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopRight, InterestPoint.TopLeft);
					}
					if (i == 3)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomRight, InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopRight, InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomRight, InterestPoint.BottomLeft);
					}
					if (i == 2)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
					}
					if (i == 3)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomLeft, InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				break;
			case 10:
				axis = 1;
				if (flag)
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopRight, InterestPoint.BottomRight);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomRight, InterestPoint.TopRight);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.BottomLeft);
					}
					if (i == 1)
					{
						return new PopupPositioner.PointCombination(InterestPoint.BottomLeft, InterestPoint.TopLeft);
					}
					goto IL_1AE;
				}
				break;
			}
			axis = 0;
			return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopLeft);
			IL_1AE:
			return new PopupPositioner.PointCombination(InterestPoint.TopLeft, InterestPoint.TopRight);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0003B4DE File Offset: 0x000396DE
		private Point[] GetChildInterestPoints(PlacementMode placement)
		{
			return PopupPositioner.Delegates.GetChildInterestPoints.Invoke(this._popup, placement);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0003B4F4 File Offset: 0x000396F4
		private Rect GetBounds(Point[] interestPoints)
		{
			double num2;
			double num = num2 = interestPoints[0].X;
			double num4;
			double num3 = num4 = interestPoints[0].Y;
			for (int i = 1; i < interestPoints.Length; i++)
			{
				double x = interestPoints[i].X;
				double y = interestPoints[i].Y;
				if (x < num2)
				{
					num2 = x;
				}
				if (x > num)
				{
					num = x;
				}
				if (y < num4)
				{
					num4 = y;
				}
				if (y > num3)
				{
					num3 = y;
				}
			}
			return new Rect(num2, num4, num - num2, num3 - num4);
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0003B57C File Offset: 0x0003977C
		private static int GetNumberOfCombinations(PlacementMode placement)
		{
			switch (placement)
			{
			case 2:
			case 7:
			case 10:
				return 2;
			case 4:
			case 5:
			case 6:
			case 8:
			case 9:
				return 4;
			case 11:
				return 0;
			}
			return 1;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0003B5C8 File Offset: 0x000397C8
		private Rect GetScreenBounds(Rect boundingBox, Point p)
		{
			return PopupPositioner.Delegates.GetScreenBounds.Invoke(this._popup, boundingBox, p);
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x0003B5DC File Offset: 0x000397DC
		private bool IsTransparent
		{
			get
			{
				return this._popup.AllowsTransparency;
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0003B5E9 File Offset: 0x000397E9
		internal static PopupPositioner GetPositioner(Popup popup)
		{
			return (PopupPositioner)popup.GetValue(PopupPositioner.PositionerProperty);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0003B5FB File Offset: 0x000397FB
		private static void SetPositioner(Popup popup, PopupPositioner value)
		{
			popup.SetValue(PopupPositioner.PositionerProperty, value);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0003B60C File Offset: 0x0003980C
		private static void OnPositionerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PopupPositioner popupPositioner = e.OldValue as PopupPositioner;
			if (popupPositioner != null)
			{
				popupPositioner.Dispose();
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0003B630 File Offset: 0x00039830
		private void OnPopupOpened(object sender, EventArgs e)
		{
			if (!this._secHelper.AttachedToWindow)
			{
				UIElement child = this._popup.Child;
				if (child != null)
				{
					HwndSource hwndSource = PresentationSource.FromVisual(child) as HwndSource;
					if (hwndSource != null)
					{
						this._secHelper.AttachToWindow(hwndSource, new AutoResizedEventHandler(this.OnWindowResize));
						this._popupRoot = (hwndSource.RootVisual as FrameworkElement);
						DependencyPropertyDescriptor.FromProperty(Popup.ChildProperty, typeof(Popup)).AddValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
						DependencyPropertyDescriptor.FromProperty(Popup.PlacementProperty, typeof(Popup)).AddValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
						DependencyPropertyDescriptor.FromProperty(Popup.HorizontalOffsetProperty, typeof(Popup)).AddValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
						DependencyPropertyDescriptor.FromProperty(Popup.VerticalOffsetProperty, typeof(Popup)).AddValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
						DependencyPropertyDescriptor.FromProperty(Popup.PlacementRectangleProperty, typeof(Popup)).AddValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
						this.Reposition();
					}
				}
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0003B778 File Offset: 0x00039978
		private void OnPopupClosed(object sender, EventArgs e)
		{
			if (this._secHelper.AttachedToWindow)
			{
				DependencyPropertyDescriptor.FromProperty(Popup.ChildProperty, typeof(Popup)).RemoveValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
				DependencyPropertyDescriptor.FromProperty(Popup.PlacementProperty, typeof(Popup)).RemoveValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
				DependencyPropertyDescriptor.FromProperty(Popup.HorizontalOffsetProperty, typeof(Popup)).RemoveValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
				DependencyPropertyDescriptor.FromProperty(Popup.VerticalOffsetProperty, typeof(Popup)).RemoveValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
				DependencyPropertyDescriptor.FromProperty(Popup.PlacementRectangleProperty, typeof(Popup)).RemoveValueChanged(this._popup, new EventHandler(this.OnPopupPropertyChanged));
				this._secHelper.DetachFromWindow(new AutoResizedEventHandler(this.OnWindowResize));
				this._popupRoot = null;
				this._positionInfo = null;
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0003B891 File Offset: 0x00039A91
		private void OnPopupPropertyChanged(object sender, EventArgs e)
		{
			this.Reposition();
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0003B8A4 File Offset: 0x00039AA4
		[CompilerGenerated]
		private Size <UpdatePosition>g__GetChildSize|24_0()
		{
			UIElement child = this._popup.Child;
			if (child != null)
			{
				return child.RenderSize;
			}
			return this._popupRoot.RenderSize;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0003B8D4 File Offset: 0x00039AD4
		[CompilerGenerated]
		private Point <UpdatePosition>g__GetChildTranslation|24_1()
		{
			UIElement child = this._popup.Child;
			if (child != null)
			{
				return child.TranslatePoint(default(Point), this._popupRoot);
			}
			return default(Point);
		}

		// Token: 0x040006AF RID: 1711
		internal const double Tolerance = 0.01;

		// Token: 0x040006B0 RID: 1712
		private PopupPositioner.PositionInfo _positionInfo;

		// Token: 0x040006B1 RID: 1713
		private FrameworkElement _popupRoot;

		// Token: 0x040006B2 RID: 1714
		private PopupPositioner.PopupSecurityHelper _secHelper;

		// Token: 0x040006B3 RID: 1715
		private static readonly DependencyProperty PositionerProperty = DependencyProperty.RegisterAttached("Positioner", typeof(PopupPositioner), typeof(PopupPositioner), new PropertyMetadata(new PropertyChangedCallback(PopupPositioner.OnPositionerChanged)));

		// Token: 0x040006B4 RID: 1716
		private readonly Popup _popup;

		// Token: 0x040006B5 RID: 1717
		private bool _isDisposed;

		// Token: 0x0200041C RID: 1052
		private struct PointCombination
		{
			// Token: 0x06001C06 RID: 7174 RVA: 0x0004B11B File Offset: 0x0004931B
			public PointCombination(InterestPoint targetInterestPoint, InterestPoint childInterestPoint)
			{
				this.TargetInterestPoint = targetInterestPoint;
				this.ChildInterestPoint = childInterestPoint;
			}

			// Token: 0x04000A2A RID: 2602
			public InterestPoint TargetInterestPoint;

			// Token: 0x04000A2B RID: 2603
			public InterestPoint ChildInterestPoint;
		}

		// Token: 0x0200041D RID: 1053
		private class PositionInfo
		{
			// Token: 0x04000A2C RID: 2604
			public int X;

			// Token: 0x04000A2D RID: 2605
			public int Y;

			// Token: 0x04000A2E RID: 2606
			public Size ChildSize;
		}

		// Token: 0x0200041E RID: 1054
		private class PopupSecurityHelper
		{
			// Token: 0x06001C08 RID: 7176 RVA: 0x0004B133 File Offset: 0x00049333
			internal PopupSecurityHelper()
			{
			}

			// Token: 0x1700051A RID: 1306
			// (get) Token: 0x06001C09 RID: 7177 RVA: 0x0004B13B File Offset: 0x0004933B
			internal bool AttachedToWindow
			{
				get
				{
					return this._window != null;
				}
			}

			// Token: 0x06001C0A RID: 7178 RVA: 0x0004B146 File Offset: 0x00049346
			internal void AttachToWindow(HwndSource window, AutoResizedEventHandler handler)
			{
				if (this._window == null)
				{
					this._window = window;
					window.AutoResized += handler;
				}
			}

			// Token: 0x06001C0B RID: 7179 RVA: 0x0004B15E File Offset: 0x0004935E
			internal void DetachFromWindow(AutoResizedEventHandler onAutoResizedEventHandler)
			{
				if (this._window != null)
				{
					HwndSource window = this._window;
					this._window = null;
					window.AutoResized -= onAutoResizedEventHandler;
				}
			}

			// Token: 0x06001C0C RID: 7180 RVA: 0x0004B17C File Offset: 0x0004937C
			internal bool IsWindowAlive()
			{
				if (this._window != null)
				{
					HwndSource window = this._window;
					return window != null && !window.IsDisposed;
				}
				return false;
			}

			// Token: 0x06001C0D RID: 7181 RVA: 0x0004B1A8 File Offset: 0x000493A8
			internal void SetPopupPos(bool position, int x, int y, bool size, int width, int height)
			{
				int num = 20;
				if (!position)
				{
					num |= 2;
				}
				if (!size)
				{
					num |= 1;
				}
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), new HandleRef(null, IntPtr.Zero), x, y, width, height, num);
			}

			// Token: 0x06001C0E RID: 7182 RVA: 0x0004B1EC File Offset: 0x000493EC
			internal Rect GetWindowRect()
			{
				NativeMethods.RECT rc = new NativeMethods.RECT(0, 0, 0, 0);
				if (this.IsWindowAlive())
				{
					SafeNativeMethods.GetWindowRect(this._window.CreateHandleRef(), ref rc);
				}
				return PointUtil.ToRect(rc);
			}

			// Token: 0x06001C0F RID: 7183 RVA: 0x0004B224 File Offset: 0x00049424
			internal Matrix GetTransformToDevice()
			{
				CompositionTarget compositionTarget = this._window.CompositionTarget;
				if (compositionTarget != null)
				{
					try
					{
						return compositionTarget.TransformToDevice;
					}
					catch (ObjectDisposedException)
					{
					}
				}
				return Matrix.Identity;
			}

			// Token: 0x06001C10 RID: 7184 RVA: 0x0004B264 File Offset: 0x00049464
			internal Matrix GetTransformFromDevice()
			{
				CompositionTarget compositionTarget = this._window.CompositionTarget;
				if (compositionTarget != null)
				{
					try
					{
						return compositionTarget.TransformFromDevice;
					}
					catch (ObjectDisposedException)
					{
					}
				}
				return Matrix.Identity;
			}

			// Token: 0x06001C11 RID: 7185 RVA: 0x0004B2A4 File Offset: 0x000494A4
			private static IntPtr GetHandle(HwndSource hwnd)
			{
				if (hwnd == null)
				{
					return IntPtr.Zero;
				}
				return hwnd.Handle;
			}

			// Token: 0x1700051B RID: 1307
			// (get) Token: 0x06001C12 RID: 7186 RVA: 0x0004B2B5 File Offset: 0x000494B5
			private IntPtr Handle
			{
				get
				{
					return PopupPositioner.PopupSecurityHelper.GetHandle(this._window);
				}
			}

			// Token: 0x04000A2F RID: 2607
			private HwndSource _window;
		}

		// Token: 0x0200041F RID: 1055
		private static class Delegates
		{
			// Token: 0x06001C13 RID: 7187 RVA: 0x0004B2C4 File Offset: 0x000494C4
			static Delegates()
			{
				try
				{
					PopupPositioner.Delegates.GetPlacementInternal = DelegateHelper.CreatePropertyGetter<Popup, PlacementMode>("PlacementInternal", 36, true);
					PopupPositioner.Delegates.GetDropOpposite = DelegateHelper.CreatePropertyGetter<Popup, bool>("DropOpposite", 36, true);
					PopupPositioner.Delegates.GetPlacementTargetInterestPoints = DelegateHelper.CreateDelegate<Func<Popup, PlacementMode, Point[]>>(typeof(Popup), "GetPlacementTargetInterestPoints", 36);
					PopupPositioner.Delegates.GetChildInterestPoints = DelegateHelper.CreateDelegate<Func<Popup, PlacementMode, Point[]>>(typeof(Popup), "GetChildInterestPoints", 36);
					PopupPositioner.Delegates.GetScreenBounds = DelegateHelper.CreateDelegate<Func<Popup, Rect, Point, Rect>>(typeof(Popup), "GetScreenBounds", 36);
				}
				catch
				{
				}
			}

			// Token: 0x1700051C RID: 1308
			// (get) Token: 0x06001C14 RID: 7188 RVA: 0x0004B35C File Offset: 0x0004955C
			public static Func<Popup, PlacementMode> GetPlacementInternal { get; }

			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x06001C15 RID: 7189 RVA: 0x0004B363 File Offset: 0x00049563
			public static Func<Popup, bool> GetDropOpposite { get; }

			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0004B36A File Offset: 0x0004956A
			public static Func<Popup, PlacementMode, Point[]> GetPlacementTargetInterestPoints { get; }

			// Token: 0x1700051F RID: 1311
			// (get) Token: 0x06001C17 RID: 7191 RVA: 0x0004B371 File Offset: 0x00049571
			public static Func<Popup, PlacementMode, Point[]> GetChildInterestPoints { get; }

			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0004B378 File Offset: 0x00049578
			public static Func<Popup, Rect, Point, Rect> GetScreenBounds { get; }
		}
	}
}
