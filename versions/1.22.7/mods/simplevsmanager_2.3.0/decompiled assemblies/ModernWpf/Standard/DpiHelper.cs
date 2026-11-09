using System;
using System.Windows;
using System.Windows.Media;

namespace Standard
{
	// Token: 0x020001F9 RID: 505
	internal static class DpiHelper
	{
		// Token: 0x06000F00 RID: 3840 RVA: 0x0002B49A File Offset: 0x0002969A
		public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
		{
			DpiHelper._transformToDevice = Matrix.Identity;
			DpiHelper._transformToDevice.Scale(dpiScaleX, dpiScaleY);
			return DpiHelper._transformToDevice.Transform(logicalPoint);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0002B4BD File Offset: 0x000296BD
		public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
		{
			DpiHelper._transformToDip = Matrix.Identity;
			DpiHelper._transformToDip.Scale(1.0 / dpiScaleX, 1.0 / dpiScaleY);
			return DpiHelper._transformToDip.Transform(devicePoint);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0002B4F4 File Offset: 0x000296F4
		public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top), dpiScaleX, dpiScaleY);
			Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom), dpiScaleX, dpiScaleY);
			return new Rect(point, point2);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0002B53C File Offset: 0x0002973C
		public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top), dpiScaleX, dpiScaleY);
			Point point2 = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom), dpiScaleX, dpiScaleY);
			return new Rect(point, point2);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0002B584 File Offset: 0x00029784
		public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height), dpiScaleX, dpiScaleY);
			Size result = default(Size);
			result.Width = point.X;
			result.Height = point.Y;
			return result;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0002B5D4 File Offset: 0x000297D4
		public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);
			return new Size(point.X, point.Y);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0002B610 File Offset: 0x00029810
		public static Thickness LogicalThicknessToDevice(Thickness logicalThickness, double dpiScaleX, double dpiScaleY)
		{
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top), dpiScaleX, dpiScaleY);
			Point point2 = DpiHelper.LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom), dpiScaleX, dpiScaleY);
			return new Thickness(point.X, point.Y, point2.X, point2.Y);
		}

		// Token: 0x040002F1 RID: 753
		[ThreadStatic]
		private static Matrix _transformToDevice;

		// Token: 0x040002F2 RID: 754
		[ThreadStatic]
		private static Matrix _transformToDip;
	}
}
