using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200028A RID: 650
	internal static class CustomPopupPlacementHelper
	{
		// Token: 0x06001304 RID: 4868 RVA: 0x00037B00 File Offset: 0x00035D00
		public static CustomPlacementMode GetPlacement(DependencyObject element)
		{
			return (CustomPlacementMode)element.GetValue(CustomPopupPlacementHelper.PlacementProperty);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00037B12 File Offset: 0x00035D12
		public static void SetPlacement(DependencyObject element, CustomPlacementMode value)
		{
			element.SetValue(CustomPopupPlacementHelper.PlacementProperty, value);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00037B28 File Offset: 0x00035D28
		internal static CustomPopupPlacement[] PositionPopup(CustomPlacementMode placement, Size popupSize, Size targetSize, Point offset, FrameworkElement child = null)
		{
			Matrix transformToDevice = default(Matrix);
			if (child != null)
			{
				Helper.TryGetTransformToDevice(child, out transformToDevice);
			}
			CustomPopupPlacement customPopupPlacement = CustomPopupPlacementHelper.CalculatePopupPlacement(placement, popupSize, targetSize, offset, child, transformToDevice);
			CustomPopupPlacement? customPopupPlacement2 = default(CustomPopupPlacement?);
			CustomPlacementMode? alternativePlacementMode = CustomPopupPlacementHelper.GetAlternativePlacementMode(placement);
			if (alternativePlacementMode != null)
			{
				customPopupPlacement2 = new CustomPopupPlacement?(CustomPopupPlacementHelper.CalculatePopupPlacement(alternativePlacementMode.Value, popupSize, targetSize, offset, child, transformToDevice));
			}
			if (customPopupPlacement2 != null)
			{
				return new CustomPopupPlacement[]
				{
					customPopupPlacement,
					customPopupPlacement2.Value
				};
			}
			return new CustomPopupPlacement[]
			{
				customPopupPlacement
			};
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00037BC0 File Offset: 0x00035DC0
		private static CustomPopupPlacement CalculatePopupPlacement(CustomPlacementMode placement, Size popupSize, Size targetSize, Point offset, FrameworkElement child = null, Matrix transformToDevice = default(Matrix))
		{
			Point point;
			PopupPrimaryAxis popupPrimaryAxis;
			switch (placement)
			{
			case CustomPlacementMode.Top:
				point..ctor((targetSize.Width - popupSize.Width) / 2.0, -popupSize.Height);
				popupPrimaryAxis = 1;
				break;
			case CustomPlacementMode.Bottom:
				point..ctor((targetSize.Width - popupSize.Width) / 2.0, targetSize.Height);
				popupPrimaryAxis = 1;
				break;
			case CustomPlacementMode.Left:
				point..ctor(-popupSize.Width, (targetSize.Height - popupSize.Height) / 2.0);
				popupPrimaryAxis = 2;
				break;
			case CustomPlacementMode.Right:
				point..ctor(targetSize.Width, (targetSize.Height - popupSize.Height) / 2.0);
				popupPrimaryAxis = 2;
				break;
			case CustomPlacementMode.Full:
				point..ctor((targetSize.Width - popupSize.Width) / 2.0, (targetSize.Height - popupSize.Height) / 2.0);
				popupPrimaryAxis = 0;
				break;
			case CustomPlacementMode.TopEdgeAlignedLeft:
				point..ctor(0.0, -popupSize.Height);
				popupPrimaryAxis = 1;
				break;
			case CustomPlacementMode.TopEdgeAlignedRight:
				point..ctor(targetSize.Width - popupSize.Width, -popupSize.Height);
				popupPrimaryAxis = 1;
				break;
			case CustomPlacementMode.BottomEdgeAlignedLeft:
				point..ctor(0.0, targetSize.Height);
				popupPrimaryAxis = 1;
				break;
			case CustomPlacementMode.BottomEdgeAlignedRight:
				point..ctor(targetSize.Width - popupSize.Width, targetSize.Height);
				popupPrimaryAxis = 1;
				break;
			case CustomPlacementMode.LeftEdgeAlignedTop:
				point..ctor(-popupSize.Width, 0.0);
				popupPrimaryAxis = 2;
				break;
			case CustomPlacementMode.LeftEdgeAlignedBottom:
				point..ctor(-popupSize.Width, targetSize.Height - popupSize.Height);
				popupPrimaryAxis = 2;
				break;
			case CustomPlacementMode.RightEdgeAlignedTop:
				point..ctor(targetSize.Width, 0.0);
				popupPrimaryAxis = 2;
				break;
			case CustomPlacementMode.RightEdgeAlignedBottom:
				point..ctor(targetSize.Width, targetSize.Height - popupSize.Height);
				popupPrimaryAxis = 2;
				break;
			default:
				throw new ArgumentOutOfRangeException("placement");
			}
			if (child != null)
			{
				Vector vector = VisualTreeHelper.GetOffset(child);
				if (transformToDevice != default(Matrix))
				{
					vector = transformToDevice.Transform(vector);
				}
				point -= vector;
			}
			return new CustomPopupPlacement(point, popupPrimaryAxis);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00037E48 File Offset: 0x00036048
		private static CustomPlacementMode? GetAlternativePlacementMode(CustomPlacementMode placement)
		{
			switch (placement)
			{
			case CustomPlacementMode.Top:
				return new CustomPlacementMode?(CustomPlacementMode.Bottom);
			case CustomPlacementMode.Bottom:
				return new CustomPlacementMode?(CustomPlacementMode.Top);
			case CustomPlacementMode.Left:
				return new CustomPlacementMode?(CustomPlacementMode.Right);
			case CustomPlacementMode.Right:
				return new CustomPlacementMode?(CustomPlacementMode.Left);
			case CustomPlacementMode.Full:
				return default(CustomPlacementMode?);
			case CustomPlacementMode.TopEdgeAlignedLeft:
				return new CustomPlacementMode?(CustomPlacementMode.BottomEdgeAlignedLeft);
			case CustomPlacementMode.TopEdgeAlignedRight:
				return new CustomPlacementMode?(CustomPlacementMode.BottomEdgeAlignedRight);
			case CustomPlacementMode.BottomEdgeAlignedLeft:
				return new CustomPlacementMode?(CustomPlacementMode.TopEdgeAlignedLeft);
			case CustomPlacementMode.BottomEdgeAlignedRight:
				return new CustomPlacementMode?(CustomPlacementMode.TopEdgeAlignedRight);
			case CustomPlacementMode.LeftEdgeAlignedTop:
				return new CustomPlacementMode?(CustomPlacementMode.RightEdgeAlignedTop);
			case CustomPlacementMode.LeftEdgeAlignedBottom:
				return new CustomPlacementMode?(CustomPlacementMode.RightEdgeAlignedBottom);
			case CustomPlacementMode.RightEdgeAlignedTop:
				return new CustomPlacementMode?(CustomPlacementMode.RightEdgeAlignedTop);
			case CustomPlacementMode.RightEdgeAlignedBottom:
				return new CustomPlacementMode?(CustomPlacementMode.LeftEdgeAlignedBottom);
			default:
				return default(CustomPlacementMode?);
			}
		}

		// Token: 0x04000652 RID: 1618
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(CustomPlacementMode), typeof(CustomPopupPlacementHelper), new PropertyMetadata(CustomPlacementMode.Top));
	}
}
