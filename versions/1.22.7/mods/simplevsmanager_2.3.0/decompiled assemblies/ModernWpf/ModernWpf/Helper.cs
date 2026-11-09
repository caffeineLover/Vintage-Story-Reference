using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x02000218 RID: 536
	internal static class Helper
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0002CB9E File Offset: 0x0002AD9E
		public static bool IsAnimationsEnabled
		{
			get
			{
				return SystemParameters.ClientAreaAnimation && RenderCapability.Tier > 0;
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0002CBB4 File Offset: 0x0002ADB4
		public static bool TryGetTransformToDevice(Visual visual, out Matrix value)
		{
			PresentationSource presentationSource = PresentationSource.FromVisual(visual);
			if (presentationSource != null)
			{
				value = presentationSource.CompositionTarget.TransformToDevice;
				return true;
			}
			value = default(Matrix);
			return false;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0002CBE8 File Offset: 0x0002ADE8
		public static Vector GetOffset(UIElement element1, InterestPoint interestPoint1, UIElement element2, InterestPoint interestPoint2, Rect element2Bounds)
		{
			Point point = element1.TranslatePoint(Helper.GetPoint(element1, interestPoint1), element2);
			if (element2Bounds.IsEmpty)
			{
				return point - Helper.GetPoint(element2, interestPoint2);
			}
			return point - Helper.GetPoint(element2Bounds, interestPoint2);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0002CC29 File Offset: 0x0002AE29
		private static Point GetPoint(UIElement element, InterestPoint interestPoint)
		{
			return Helper.GetPoint(new Rect(element.RenderSize), interestPoint);
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0002CC3C File Offset: 0x0002AE3C
		private static Point GetPoint(Rect rect, InterestPoint interestPoint)
		{
			switch (interestPoint)
			{
			case InterestPoint.TopLeft:
				return rect.TopLeft;
			case InterestPoint.TopRight:
				return rect.TopRight;
			case InterestPoint.BottomLeft:
				return rect.BottomLeft;
			case InterestPoint.BottomRight:
				return rect.BottomRight;
			case InterestPoint.Center:
				return new Point(rect.Left + rect.Width / 2.0, rect.Top + rect.Height / 2.0);
			default:
				throw new ArgumentOutOfRangeException("interestPoint");
			}
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002CCC8 File Offset: 0x0002AEC8
		public static bool HasDefaultValue(this DependencyObject d, DependencyProperty dp)
		{
			return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == 1;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002CCE7 File Offset: 0x0002AEE7
		public static bool HasNonDefaultValue(this DependencyObject d, DependencyProperty dp)
		{
			return !d.HasDefaultValue(dp);
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002CCF3 File Offset: 0x0002AEF3
		public static bool HasLocalValue(this DependencyObject d, DependencyProperty dp)
		{
			return d.ReadLocalValue(dp) != DependencyProperty.UnsetValue;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002CD06 File Offset: 0x0002AF06
		public static DpiScale2 GetDpi(this Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			return new DpiScale2(VisualTreeHelper.GetDpi(window));
		}
	}
}
