using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000046 RID: 70
	public class LayoutPanel : Panel
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000927F File Offset: 0x0000747F
		// (set) Token: 0x060002AA RID: 682 RVA: 0x00009287 File Offset: 0x00007487
		public Layout Layout
		{
			get
			{
				return this.m_layout;
			}
			set
			{
				base.SetValue(LayoutPanel.LayoutProperty, value);
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00009295 File Offset: 0x00007495
		private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LayoutPanel)d).OnLayoutChanged((Layout)e.OldValue, (Layout)e.NewValue);
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002AC RID: 684 RVA: 0x000092BA File Offset: 0x000074BA
		// (set) Token: 0x060002AD RID: 685 RVA: 0x000092CC File Offset: 0x000074CC
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(LayoutPanel.PaddingProperty);
			}
			set
			{
				base.SetValue(LayoutPanel.PaddingProperty, value);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002AE RID: 686 RVA: 0x000092DF File Offset: 0x000074DF
		// (set) Token: 0x060002AF RID: 687 RVA: 0x000092E7 File Offset: 0x000074E7
		internal object LayoutState { get; set; }

		// Token: 0x060002B0 RID: 688 RVA: 0x000092F0 File Offset: 0x000074F0
		protected override Size MeasureOverride(Size availableSize)
		{
			Thickness padding = this.Padding;
			double num = padding.Left + padding.Right;
			double num2 = padding.Top + padding.Bottom;
			Size size = availableSize;
			size.Width -= num;
			size.Height -= num2;
			size.Width = Math.Max(0.0, size.Width);
			size.Height = Math.Max(0.0, size.Height);
			Layout layout = this.Layout;
			Size result;
			if (layout != null)
			{
				Size size2 = layout.Measure(this.m_layoutContext, size);
				size2.Width += num;
				size2.Height += num2;
				result = size2;
			}
			else
			{
				Size size3 = default(Size);
				foreach (object obj in base.Children)
				{
					UIElement uielement = (UIElement)obj;
					uielement.Measure(size);
					Size desiredSize = uielement.DesiredSize;
					size3.Width = Math.Max(size3.Width, desiredSize.Width);
					size3.Height = Math.Max(size3.Height, desiredSize.Height);
				}
				result = size3;
				result.Width += num;
				result.Height += num2;
			}
			return result;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00009478 File Offset: 0x00007678
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = finalSize;
			Thickness padding = this.Padding;
			double num = padding.Left + padding.Right;
			double num2 = padding.Top + padding.Bottom;
			double left = padding.Left;
			double top = padding.Top;
			Size finalSize2 = finalSize;
			finalSize2.Width -= num;
			finalSize2.Height -= num2;
			finalSize2.Width = Math.Max(0.0, finalSize2.Width);
			finalSize2.Height = Math.Max(0.0, finalSize2.Height);
			Layout layout = this.Layout;
			if (layout != null)
			{
				Size size = layout.Arrange(this.m_layoutContext, finalSize2);
				size.Width += num;
				size.Height += num2;
				if (left != 0.0 || top != 0.0)
				{
					foreach (object obj in base.Children)
					{
						FrameworkElement frameworkElement = ((UIElement)obj) as FrameworkElement;
						if (frameworkElement != null)
						{
							Rect layoutSlot = LayoutInformation.GetLayoutSlot(frameworkElement);
							layoutSlot.X += left;
							layoutSlot.Y += top;
							frameworkElement.Arrange(layoutSlot);
						}
					}
				}
				result = size;
			}
			else
			{
				Rect rect;
				rect..ctor(left, top, finalSize2.Width, finalSize2.Height);
				foreach (object obj2 in base.Children)
				{
					((UIElement)obj2).Arrange(rect);
				}
			}
			return result;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00009668 File Offset: 0x00007868
		private void OnLayoutChanged(Layout oldValue, Layout newValue)
		{
			if (this.m_layoutContext == null)
			{
				this.m_layoutContext = new LayoutPanelLayoutContext(this);
			}
			if (oldValue != null)
			{
				oldValue.UninitializeForContext(this.m_layoutContext);
				oldValue.MeasureInvalidated -= new TypedEventHandler<Layout, object>(this.InvalidateMeasureForLayout);
				oldValue.ArrangeInvalidated -= new TypedEventHandler<Layout, object>(this.InvalidateArrangeForLayout);
			}
			this.m_layout = newValue;
			if (newValue != null)
			{
				newValue.InitializeForContext(this.m_layoutContext);
				newValue.MeasureInvalidated += new TypedEventHandler<Layout, object>(this.InvalidateMeasureForLayout);
				newValue.ArrangeInvalidated += new TypedEventHandler<Layout, object>(this.InvalidateArrangeForLayout);
			}
			base.InvalidateMeasure();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000096FC File Offset: 0x000078FC
		private void InvalidateMeasureForLayout(Layout sender, object args)
		{
			base.InvalidateMeasure();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00009704 File Offset: 0x00007904
		private void InvalidateArrangeForLayout(Layout sender, object args)
		{
			base.InvalidateArrange();
		}

		// Token: 0x04000106 RID: 262
		public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout", typeof(Layout), typeof(LayoutPanel), new FrameworkPropertyMetadata(new PropertyChangedCallback(LayoutPanel.OnLayoutChanged)));

		// Token: 0x04000107 RID: 263
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(LayoutPanel), new FrameworkPropertyMetadata(new Thickness(0.0, 0.0, 0.0, 0.0), 1));

		// Token: 0x04000109 RID: 265
		private LayoutContext m_layoutContext;

		// Token: 0x0400010A RID: 266
		private Layout m_layout;
	}
}
