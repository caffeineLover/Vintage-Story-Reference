using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A5 RID: 677
	public static class SliderAutoToolTipHelper
	{
		// Token: 0x06001443 RID: 5187 RVA: 0x0003C267 File Offset: 0x0003A467
		public static bool GetIsEnabled(ToolTip toolTip)
		{
			return (bool)toolTip.GetValue(SliderAutoToolTipHelper.IsEnabledProperty);
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0003C279 File Offset: 0x0003A479
		public static void SetIsEnabled(ToolTip toolTip, bool value)
		{
			toolTip.SetValue(SliderAutoToolTipHelper.IsEnabledProperty, value);
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0003C28C File Offset: 0x0003A48C
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToolTip toolTip = (ToolTip)d;
			if ((bool)e.NewValue)
			{
				Thumb thumb = toolTip.PlacementTarget as Thumb;
				if (thumb != null)
				{
					DependencyObject templatedParent = thumb.TemplatedParent;
					Slider slider = templatedParent as Slider;
					if (slider != null)
					{
						SliderAutoToolTipHelper.SetOriginalCustomPopupPlacementCallback(toolTip, toolTip.CustomPopupPlacementCallback);
						toolTip.CustomPopupPlacementCallback = ((Size popupSize, Size targetSize, Point offset) => SliderAutoToolTipHelper.PositionAutoToolTip(slider, toolTip, popupSize, targetSize));
					}
				}
				toolTip.IsVisibleChanged += new DependencyPropertyChangedEventHandler(SliderAutoToolTipHelper.OnToolTipIsVisibleChanged);
				return;
			}
			if (toolTip.ReadLocalValue(SliderAutoToolTipHelper.OriginalCustomPopupPlacementCallbackProperty) != DependencyProperty.UnsetValue)
			{
				toolTip.CustomPopupPlacementCallback = SliderAutoToolTipHelper.GetOriginalCustomPopupPlacementCallback(toolTip);
				toolTip.ClearValue(SliderAutoToolTipHelper.OriginalCustomPopupPlacementCallbackProperty);
			}
			toolTip.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(SliderAutoToolTipHelper.OnToolTipIsVisibleChanged);
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0003C381 File Offset: 0x0003A581
		private static CustomPopupPlacementCallback GetOriginalCustomPopupPlacementCallback(ToolTip toolTip)
		{
			return (CustomPopupPlacementCallback)toolTip.GetValue(SliderAutoToolTipHelper.OriginalCustomPopupPlacementCallbackProperty);
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0003C393 File Offset: 0x0003A593
		private static void SetOriginalCustomPopupPlacementCallback(ToolTip toolTip, CustomPopupPlacementCallback value)
		{
			toolTip.SetValue(SliderAutoToolTipHelper.OriginalCustomPopupPlacementCallbackProperty, value);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0003C3A4 File Offset: 0x0003A5A4
		private static void OnToolTipIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ToolTip toolTip = (ToolTip)sender;
			Thumb thumb = toolTip.PlacementTarget as Thumb;
			if (thumb != null)
			{
				if ((bool)e.NewValue)
				{
					thumb.SizeChanged += new SizeChangedEventHandler(SliderAutoToolTipHelper.OnThumbSizeChanged);
					SliderAutoToolTipHelper.UpdatePlacementRectangle(toolTip, thumb.RenderSize);
					return;
				}
				thumb.SizeChanged -= new SizeChangedEventHandler(SliderAutoToolTipHelper.OnThumbSizeChanged);
				toolTip.ClearValue(ToolTip.PlacementRectangleProperty);
			}
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0003C414 File Offset: 0x0003A614
		private static void OnThumbSizeChanged(object sender, SizeChangedEventArgs e)
		{
			ToolTip toolTip = ((Thumb)sender).ToolTip as ToolTip;
			if (toolTip != null)
			{
				SliderAutoToolTipHelper.UpdatePlacementRectangle(toolTip, e.NewSize);
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0003C444 File Offset: 0x0003A644
		private static void UpdatePlacementRectangle(ToolTip toolTip, Size targetSize)
		{
			toolTip.PlacementRectangle = new Rect(new Point(-20.0, -20.0), new Point(targetSize.Width + 20.0, targetSize.Height + 20.0));
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0003C49C File Offset: 0x0003A69C
		private static CustomPopupPlacement[] PositionAutoToolTip(Slider slider, ToolTip autoToolTip, Size popupSize, Size targetSize)
		{
			AutoToolTipPlacement autoToolTipPlacement = slider.AutoToolTipPlacement;
			Point point;
			PopupPrimaryAxis popupPrimaryAxis;
			if (autoToolTipPlacement != 1)
			{
				if (autoToolTipPlacement != 2)
				{
					return new CustomPopupPlacement[0];
				}
				if (slider.Orientation == null)
				{
					point..ctor((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height);
					popupPrimaryAxis = 1;
				}
				else
				{
					point..ctor(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5);
					popupPrimaryAxis = 2;
				}
			}
			else if (slider.Orientation == null)
			{
				point..ctor((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height);
				popupPrimaryAxis = 1;
			}
			else
			{
				point..ctor(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5);
				popupPrimaryAxis = 2;
			}
			Matrix matrix;
			if (Helper.TryGetTransformToDevice(autoToolTip, out matrix))
			{
				Vector offset = VisualTreeHelper.GetOffset(autoToolTip);
				point -= matrix.Transform(offset);
			}
			return new CustomPopupPlacement[]
			{
				new CustomPopupPlacement(point, popupPrimaryAxis)
			};
		}

		// Token: 0x040006CA RID: 1738
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SliderAutoToolTipHelper), new PropertyMetadata(new PropertyChangedCallback(SliderAutoToolTipHelper.OnIsEnabledChanged)));

		// Token: 0x040006CB RID: 1739
		private static readonly DependencyProperty OriginalCustomPopupPlacementCallbackProperty = DependencyProperty.RegisterAttached("OriginalCustomPopupPlacementCallback", typeof(CustomPopupPlacementCallback), typeof(SliderAutoToolTipHelper));
	}
}
