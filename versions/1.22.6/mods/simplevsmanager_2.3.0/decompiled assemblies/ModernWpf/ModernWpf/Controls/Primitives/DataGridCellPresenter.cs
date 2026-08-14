using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200028B RID: 651
	public class DataGridCellPresenter : ContentPresenter
	{
		// Token: 0x0600130A RID: 4874 RVA: 0x00037F2C File Offset: 0x0003612C
		public DataGridCellPresenter()
		{
			this._currencyVisualHelper = new DataGridCellPresenter.BorderHelper(this);
			this._focusVisualPrimaryHelper = new DataGridCellPresenter.BorderHelper(this);
			this._focusVisualSecondaryHelper = new DataGridCellPresenter.BorderHelper(this);
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x00037F58 File Offset: 0x00036158
		// (set) Token: 0x0600130C RID: 4876 RVA: 0x00037F6A File Offset: 0x0003616A
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(DataGridCellPresenter.BackgroundProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.BackgroundProperty, value);
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x00037F78 File Offset: 0x00036178
		// (set) Token: 0x0600130E RID: 4878 RVA: 0x00037F8A File Offset: 0x0003618A
		public Brush CurrencyVisualBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridCellPresenter.CurrencyVisualBrushProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.CurrencyVisualBrushProperty, value);
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x00037F98 File Offset: 0x00036198
		// (set) Token: 0x06001310 RID: 4880 RVA: 0x00037FAA File Offset: 0x000361AA
		public double CurrencyVisualThickness
		{
			get
			{
				return (double)base.GetValue(DataGridCellPresenter.CurrencyVisualThicknessProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.CurrencyVisualThicknessProperty, value);
			}
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00037FBD File Offset: 0x000361BD
		private static void OnCurrencyVisualThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCellPresenter)d)._currencyVisualHelper.ClearPenCache();
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x00037FCF File Offset: 0x000361CF
		// (set) Token: 0x06001313 RID: 4883 RVA: 0x00037FE1 File Offset: 0x000361E1
		public Brush FocusVisualPrimaryBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridCellPresenter.FocusVisualPrimaryBrushProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.FocusVisualPrimaryBrushProperty, value);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x00037FEF File Offset: 0x000361EF
		// (set) Token: 0x06001315 RID: 4885 RVA: 0x00038001 File Offset: 0x00036201
		public double FocusVisualPrimaryThickness
		{
			get
			{
				return (double)base.GetValue(DataGridCellPresenter.FocusVisualPrimaryThicknessProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.FocusVisualPrimaryThicknessProperty, value);
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00038014 File Offset: 0x00036214
		private static void OnFocusVisualPrimaryThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCellPresenter)d)._focusVisualPrimaryHelper.ClearPenCache();
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x00038026 File Offset: 0x00036226
		// (set) Token: 0x06001318 RID: 4888 RVA: 0x00038038 File Offset: 0x00036238
		public Brush FocusVisualSecondaryBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridCellPresenter.FocusVisualSecondaryBrushProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.FocusVisualSecondaryBrushProperty, value);
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x00038046 File Offset: 0x00036246
		// (set) Token: 0x0600131A RID: 4890 RVA: 0x00038058 File Offset: 0x00036258
		public double FocusVisualSecondaryThickness
		{
			get
			{
				return (double)base.GetValue(DataGridCellPresenter.FocusVisualSecondaryThicknessProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.FocusVisualSecondaryThicknessProperty, value);
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0003806B File Offset: 0x0003626B
		private static void OnFocusVisualSecondaryThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCellPresenter)d)._focusVisualSecondaryHelper.ClearPenCache();
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x0003807D File Offset: 0x0003627D
		// (set) Token: 0x0600131D RID: 4893 RVA: 0x0003808F File Offset: 0x0003628F
		public bool IsCurrencyVisualVisible
		{
			get
			{
				return (bool)base.GetValue(DataGridCellPresenter.IsCurrencyVisualVisibleProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.IsCurrencyVisualVisibleProperty, value);
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x000380A2 File Offset: 0x000362A2
		// (set) Token: 0x0600131F RID: 4895 RVA: 0x000380B4 File Offset: 0x000362B4
		public bool IsFocusVisualVisible
		{
			get
			{
				return (bool)base.GetValue(DataGridCellPresenter.IsFocusVisualVisibleProperty);
			}
			set
			{
				base.SetValue(DataGridCellPresenter.IsFocusVisualVisibleProperty, value);
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x000380C8 File Offset: 0x000362C8
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(base.RenderSize));
			}
			base.OnRender(dc);
			if (this.IsCurrencyVisualVisible)
			{
				this._currencyVisualHelper.DrawBorder(dc, this.CurrencyVisualBrush, this.CurrencyVisualThickness, 0.0);
			}
			if (this.IsFocusVisualVisible)
			{
				double focusVisualPrimaryThickness = this.FocusVisualPrimaryThickness;
				this._focusVisualPrimaryHelper.DrawBorder(dc, this.FocusVisualPrimaryBrush, focusVisualPrimaryThickness, 0.0);
				this._focusVisualSecondaryHelper.DrawBorder(dc, this.FocusVisualSecondaryBrush, this.FocusVisualSecondaryThickness, focusVisualPrimaryThickness);
			}
		}

		// Token: 0x04000653 RID: 1619
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(null, 2064));

		// Token: 0x04000654 RID: 1620
		public static readonly DependencyProperty CurrencyVisualBrushProperty = DependencyProperty.Register("CurrencyVisualBrush", typeof(Brush), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(null, 16));

		// Token: 0x04000655 RID: 1621
		public static readonly DependencyProperty CurrencyVisualThicknessProperty = DependencyProperty.Register("CurrencyVisualThickness", typeof(double), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(0.0, 16, new PropertyChangedCallback(DataGridCellPresenter.OnCurrencyVisualThicknessChanged)));

		// Token: 0x04000656 RID: 1622
		public static readonly DependencyProperty FocusVisualPrimaryBrushProperty = DependencyProperty.Register("FocusVisualPrimaryBrush", typeof(Brush), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(null, 16));

		// Token: 0x04000657 RID: 1623
		public static readonly DependencyProperty FocusVisualPrimaryThicknessProperty = DependencyProperty.Register("FocusVisualPrimaryThickness", typeof(double), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(0.0, 16, new PropertyChangedCallback(DataGridCellPresenter.OnFocusVisualPrimaryThicknessChanged)));

		// Token: 0x04000658 RID: 1624
		public static readonly DependencyProperty FocusVisualSecondaryBrushProperty = DependencyProperty.Register("FocusVisualSecondaryBrush", typeof(Brush), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(null, 16));

		// Token: 0x04000659 RID: 1625
		public static readonly DependencyProperty FocusVisualSecondaryThicknessProperty = DependencyProperty.Register("FocusVisualSecondaryThickness", typeof(double), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(0.0, 16, new PropertyChangedCallback(DataGridCellPresenter.OnFocusVisualSecondaryThicknessChanged)));

		// Token: 0x0400065A RID: 1626
		public static readonly DependencyProperty IsCurrencyVisualVisibleProperty = DependencyProperty.Register("IsCurrencyVisualVisible", typeof(bool), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(false, 16));

		// Token: 0x0400065B RID: 1627
		public static readonly DependencyProperty IsFocusVisualVisibleProperty = DependencyProperty.Register("IsFocusVisualVisible", typeof(bool), typeof(DataGridCellPresenter), new FrameworkPropertyMetadata(false, 16));

		// Token: 0x0400065C RID: 1628
		private readonly DataGridCellPresenter.BorderHelper _currencyVisualHelper;

		// Token: 0x0400065D RID: 1629
		private readonly DataGridCellPresenter.BorderHelper _focusVisualPrimaryHelper;

		// Token: 0x0400065E RID: 1630
		private readonly DataGridCellPresenter.BorderHelper _focusVisualSecondaryHelper;

		// Token: 0x02000414 RID: 1044
		private class BorderHelper
		{
			// Token: 0x06001BEF RID: 7151 RVA: 0x0004AB3C File Offset: 0x00048D3C
			public BorderHelper(UIElement owner)
			{
				this._owner = owner;
			}

			// Token: 0x17000516 RID: 1302
			// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x0004AB4B File Offset: 0x00048D4B
			private Size RenderSize
			{
				get
				{
					return this._owner.RenderSize;
				}
			}

			// Token: 0x17000517 RID: 1303
			// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x0004AB58 File Offset: 0x00048D58
			// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x0004AB60 File Offset: 0x00048D60
			private Pen PenCache { get; set; }

			// Token: 0x06001BF3 RID: 7155 RVA: 0x0004AB69 File Offset: 0x00048D69
			public void ClearPenCache()
			{
				this.PenCache = null;
			}

			// Token: 0x06001BF4 RID: 7156 RVA: 0x0004AB74 File Offset: 0x00048D74
			public void DrawBorder(DrawingContext dc, Brush brush, double thickness, double margin = 0.0)
			{
				if (thickness > 0.0 && brush != null)
				{
					Pen pen = this.PenCache;
					if (pen == null)
					{
						pen = new Pen(brush, thickness);
						if (brush.IsFrozen)
						{
							pen.Freeze();
						}
						this.PenCache = pen;
					}
					double num = thickness * 0.5;
					Rect rect;
					rect..ctor(new Point(margin + num, margin + num), new Point(this.RenderSize.Width - margin - num, this.RenderSize.Height - margin - num));
					dc.DrawRectangle(null, pen, rect);
				}
			}

			// Token: 0x04000A13 RID: 2579
			private readonly UIElement _owner;
		}
	}
}
