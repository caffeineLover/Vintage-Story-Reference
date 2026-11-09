using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace VintageStoryModManager.Views
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	public class OverlappingTagPanel : Panel
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x0001ECC8 File Offset: 0x0001CEC8
		public OverlappingTagPanel()
		{
			base.ClipToBounds = true;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001ECE2 File Offset: 0x0001CEE2
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0001ECF4 File Offset: 0x0001CEF4
		public double MaxSpacing
		{
			get
			{
				return (double)base.GetValue(OverlappingTagPanel.MaxSpacingProperty);
			}
			set
			{
				base.SetValue(OverlappingTagPanel.MaxSpacingProperty, value);
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001ED08 File Offset: 0x0001CF08
		protected override Size MeasureOverride(Size availableSize)
		{
			int count = base.InternalChildren.Count;
			if (count == 0)
			{
				return new Size(0.0, 0.0);
			}
			double[] array = new double[count];
			double[] array2 = new double[count];
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = base.InternalChildren[i];
				if (uielement != null)
				{
					uielement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
					array[i] = uielement.DesiredSize.Width;
					array2[i] = uielement.DesiredSize.Height;
				}
			}
			return this.ComputeDesiredSize(availableSize, array, array2);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001EDB8 File Offset: 0x0001CFB8
		protected override Size ArrangeOverride(Size finalSize)
		{
			int count = base.InternalChildren.Count;
			if (count == 0)
			{
				return finalSize;
			}
			double[] array = new double[count];
			double[] array2 = new double[count];
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = base.InternalChildren[i];
				if (uielement != null)
				{
					array[i] = uielement.DesiredSize.Width;
					array2[i] = uielement.DesiredSize.Height;
				}
			}
			Size result = this.ArrangeChildren(finalSize, array, array2);
			for (int j = 0; j < count; j++)
			{
				UIElement uielement2 = base.InternalChildren[j];
				if (uielement2 != null)
				{
					Rect rect = (j < this._arrangeRects.Count) ? this._arrangeRects[j] : new Rect(new Size(array[j], array2[j]));
					uielement2.Arrange(rect);
					Panel.SetZIndex(uielement2, count - j);
				}
			}
			return result;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
		private Size ComputeDesiredSize(Size availableSize, IReadOnlyList<double> widths, IReadOnlyList<double> heights)
		{
			double widthConstraint = double.IsInfinity(availableSize.Width) ? double.PositiveInfinity : Math.Max(0.0, availableSize.Width);
			double heightConstraint = double.IsInfinity(availableSize.Height) ? double.PositiveInfinity : Math.Max(0.0, availableSize.Height);
			double num;
			double num2;
			this.DetermineSpacing(widthConstraint, heightConstraint, widths, heights, null, out num, out num2);
			return new Size(num, num2);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001EF28 File Offset: 0x0001D128
		private Size ArrangeChildren(Size finalSize, IReadOnlyList<double> widths, IReadOnlyList<double> heights)
		{
			double widthConstraint = double.IsInfinity(finalSize.Width) ? double.PositiveInfinity : Math.Max(0.0, finalSize.Width);
			double heightConstraint = double.IsInfinity(finalSize.Height) ? double.PositiveInfinity : Math.Max(0.0, finalSize.Height);
			double num;
			double num2;
			this.DetermineSpacing(widthConstraint, heightConstraint, widths, heights, this._arrangeRects, out num, out num2);
			return new Size(num, num2);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		private double DetermineSpacing(double widthConstraint, double heightConstraint, IReadOnlyList<double> widths, IReadOnlyList<double> heights, [Nullable(2)] List<Rect> rects, out double measuredWidth, out double measuredHeight)
		{
			int count = widths.Count;
			if (count == 0)
			{
				measuredWidth = 0.0;
				measuredHeight = 0.0;
				if (rects != null)
				{
					rects.Clear();
				}
				return 0.0;
			}
			double num = this.MaxSpacing;
			if (count > 1)
			{
				double num2 = this.MaxSpacing / Math.Sqrt((double)count);
				num = Math.Max(0.0, Math.Min(this.MaxSpacing, num2));
			}
			double num3 = 0.0;
			if (!double.IsInfinity(widthConstraint) && count > 1)
			{
				double num4 = 0.0;
				for (int i = 0; i < count; i++)
				{
					num4 += widths[i];
				}
				double num5 = (widthConstraint - num4) / (double)(count - 1);
				num3 = Math.Max(0.0, Math.Min(num, num5));
			}
			num3 = Math.Min(num3, num);
			double num6 = num;
			double num8;
			double num7 = this.ComputeLayout(widthConstraint, num, widths, heights, null, out num8);
			double num9 = num7;
			double num10 = num8;
			if (!double.IsInfinity(heightConstraint) && num7 > heightConstraint && count > 1)
			{
				double num12;
				double num11 = this.ComputeLayout(widthConstraint, num3, widths, heights, null, out num12);
				if (num11 > heightConstraint)
				{
					num6 = num3;
					num9 = num11;
					num10 = num12;
				}
				else
				{
					double num13 = num3;
					double num14 = num;
					for (int j = 0; j < 48; j++)
					{
						double num15 = (num13 + num14) * 0.5;
						double num17;
						double num16 = this.ComputeLayout(widthConstraint, num15, widths, heights, null, out num17);
						if (num16 <= heightConstraint)
						{
							num6 = num15;
							num9 = num16;
							num10 = num17;
							num14 = num15;
						}
						else
						{
							num13 = num15;
						}
					}
				}
			}
			if (rects != null)
			{
				rects.Clear();
				double num18;
				this.ComputeLayout(widthConstraint, num6, widths, heights, rects, out num18);
			}
			measuredWidth = num10;
			measuredHeight = num9;
			return num6;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001F158 File Offset: 0x0001D358
		private double ComputeLayout(double widthConstraint, double spacing, IReadOnlyList<double> widths, IReadOnlyList<double> heights, [Nullable(2)] List<Rect> rects, out double measuredWidth)
		{
			if (rects != null)
			{
				rects.Clear();
			}
			int count = widths.Count;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			for (int i = 0; i < count; i++)
			{
				double num5 = widths[i];
				double num6 = heights[i];
				if (!double.IsInfinity(widthConstraint) && num > 0.0 && num + num5 > widthConstraint + 0.1)
				{
					num2 += num3;
					num3 = 0.0;
					num = 0.0;
				}
				double num7 = num;
				if (num7 < 0.0)
				{
					num7 = 0.0;
				}
				Rect rect;
				rect..ctor(num7, num2, num5, num6);
				if (rects != null)
				{
					rects.Add(rect);
				}
				num3 = Math.Max(num3, num6);
				num4 = Math.Max(num4, rect.Right);
				num = num7 + num5 + spacing;
				if (num < 0.0)
				{
					num = 0.0;
				}
			}
			double result = num2 + num3;
			measuredWidth = (double.IsInfinity(widthConstraint) ? num4 : Math.Min(num4, widthConstraint));
			return result;
		}

		// Token: 0x040001FD RID: 509
		public static readonly DependencyProperty MaxSpacingProperty = DependencyProperty.Register("MaxSpacing", typeof(double), typeof(OverlappingTagPanel), new FrameworkPropertyMetadata(6.0, 3));

		// Token: 0x040001FE RID: 510
		private readonly List<Rect> _arrangeRects = new List<Rect>();
	}
}
