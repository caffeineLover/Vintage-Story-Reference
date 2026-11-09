using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x0200026C RID: 620
	public class SimpleStackPanel : Panel
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x00033BA0 File Offset: 0x00031DA0
		// (set) Token: 0x060011B0 RID: 4528 RVA: 0x00033BB2 File Offset: 0x00031DB2
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(SimpleStackPanel.OrientationProperty);
			}
			set
			{
				base.SetValue(SimpleStackPanel.OrientationProperty, value);
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00033BC5 File Offset: 0x00031DC5
		// (set) Token: 0x060011B2 RID: 4530 RVA: 0x00033BD7 File Offset: 0x00031DD7
		public double Spacing
		{
			get
			{
				return (double)base.GetValue(SimpleStackPanel.SpacingProperty);
			}
			set
			{
				base.SetValue(SimpleStackPanel.SpacingProperty, value);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00033BEA File Offset: 0x00031DEA
		protected override bool HasLogicalOrientation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00033BED File Offset: 0x00031DED
		protected override Orientation LogicalOrientation
		{
			get
			{
				return this.Orientation;
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00033BF8 File Offset: 0x00031DF8
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			UIElementCollection internalChildren = base.InternalChildren;
			Size size = constraint;
			bool flag = this.Orientation == 0;
			double spacing = this.Spacing;
			bool flag2 = false;
			if (flag)
			{
				size.Width = double.PositiveInfinity;
			}
			else
			{
				size.Height = double.PositiveInfinity;
			}
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					bool flag3 = uielement.Visibility != 2;
					if (flag3 && !flag2)
					{
						flag2 = true;
					}
					uielement.Measure(size);
					Size desiredSize = uielement.DesiredSize;
					if (flag)
					{
						result.Width += (flag3 ? spacing : 0.0) + desiredSize.Width;
						result.Height = Math.Max(result.Height, desiredSize.Height);
					}
					else
					{
						result.Width = Math.Max(result.Width, desiredSize.Width);
						result.Height += (flag3 ? spacing : 0.0) + desiredSize.Height;
					}
				}
				i++;
			}
			if (flag)
			{
				result.Width -= (flag2 ? spacing : 0.0);
			}
			else
			{
				result.Height -= (flag2 ? spacing : 0.0);
			}
			return result;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00033D78 File Offset: 0x00031F78
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			bool flag = this.Orientation == 0;
			Rect rect;
			rect..ctor(arrangeSize);
			double num = 0.0;
			double spacing = this.Spacing;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					if (flag)
					{
						rect.X += num;
						num = uielement.DesiredSize.Width;
						rect.Width = num;
						rect.Height = Math.Max(arrangeSize.Height, uielement.DesiredSize.Height);
					}
					else
					{
						rect.Y += num;
						num = uielement.DesiredSize.Height;
						rect.Height = num;
						rect.Width = Math.Max(arrangeSize.Width, uielement.DesiredSize.Width);
					}
					if (uielement.Visibility != 2)
					{
						num += spacing;
					}
					uielement.Arrange(rect);
				}
				i++;
			}
			return arrangeSize;
		}

		// Token: 0x040004E5 RID: 1253
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SimpleStackPanel), new FrameworkPropertyMetadata(1, 1));

		// Token: 0x040004E6 RID: 1254
		public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(SimpleStackPanel), new FrameworkPropertyMetadata(0.0, 1));
	}
}
